public class Script : ScriptBase {

    private const string OL_PATH_FIELD_SUFFIX = "_ol_path_auto_gen";
    private const string OL_QUERY_FIELD_SUFFIX = "_ol_query_auto_gen";
    private const string OL_HEADER_FIELD_SUFFIX = "_ol_header_auto_gen";
    private const string OL_HUB_URL_HEADER = "ol-hub-url";
    private const string OL_HUB_API_KEY_HEADER = "ol-hub-x-api-key";
    private const string OL_API_KEY_HEADER = "x-api-key";

    public override async Task<HttpResponseMessage> ExecuteAsync() {
        try {
            // for DoGet, DoPost operations update URI with path and query params, also add headers
            await this.UpdateRequest().ConfigureAwait(false);
            // perform a request
            var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode) {
                // for GetMethodOpenApiSpec operation - update titles of parameters with suffix (Query), (Path), (Header)
                await this.UpdateResponse(response).ConfigureAwait(false);
            }
            return response;
        } catch (ConnectorException ex) {
            var response = new HttpResponseMessage(ex.StatusCode);
            response.Content = CreateJsonContent(ex.Message);
            return response;
        }
    }

    private async Task UpdateRequest() {
        if ("AS400Cobol".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)
            || "AS400Rpg".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)
            || "AS400DataQueue".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)
            || "AS400Db2Queries".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)
            || "AS400Db2Executables".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)) {
            var host = this.Context.Request.Headers.GetValues(OL_HUB_URL_HEADER).First();
            // get selected method id
            var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
            var methodId = query.Get("method");
            // create a new request to get method metadata with HTTP enrichment
            using var methodMetadataRequest = new HttpRequestMessage(HttpMethod.Get, $"{host}/backend/api/v1/methods/{methodId}?enrichmentType=HTTP");
            // set Hub's x-api-key header
            methodMetadataRequest.Headers.Add(OL_API_KEY_HEADER, this.Context.Request.Headers.GetValues(OL_HUB_API_KEY_HEADER));
            string content = string.Empty;
            try {
                using var methodMetadataResponse = await this.Context.SendAsync(methodMetadataRequest, this.CancellationToken).ConfigureAwait(false);
                content = await methodMetadataResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (methodMetadataResponse.IsSuccessStatusCode) {
                    // parse method metadata as json object
                    var jsonContent = JObject.Parse(content);
                    // get routingKey
                    var routingKey = jsonContent["enrichment"]?["routingKey"]?.ToString();
                    if (string.IsNullOrEmpty(routingKey) && jsonContent["name"] != null) {
                        routingKey = $"/{jsonContent["name"]?.ToString()}";
                    }
                    if (!string.IsNullOrEmpty(routingKey)) {
                        string formattedRoutingKey = routingKey;
                        // get initial content from request
                        var originalRequestContent = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var apiParameters = jsonContent["enrichment"]?["parameters"]?.ToString();
                        // if content from request is empty and method metadata has information about api parameters (e.g. path, query) then throw an exception
                        if (string.IsNullOrEmpty(originalRequestContent) && !string.IsNullOrEmpty(apiParameters)) {
                            throw new ConnectorException(HttpStatusCode.InternalServerError, "Selected API requires parameters, but request content is empty.");
                        }
                        try {
                            var newBody = new JObject();
                            var newQuery = HttpUtility.ParseQueryString("");
                            if (!string.IsNullOrEmpty(originalRequestContent)) {
                                var originalRequest = JObject.Parse(originalRequestContent);
                                foreach (var entry in originalRequest) {
                                    string name = entry.Key;
                                    if (name.Contains(OL_PATH_FIELD_SUFFIX)) {
                                        string originalName = name.Replace(OL_PATH_FIELD_SUFFIX, "");
                                        if (formattedRoutingKey.Contains("{" + originalName + "}")) {
                                            formattedRoutingKey = formattedRoutingKey.Replace("{" + originalName + "}", entry.Value.ToString());
                                        } else {
                                            throw new ConnectorException(HttpStatusCode.InternalServerError, "Unable to find path parameter '" + originalName + "' in User's API");
                                        }
                                    } else if (name.Contains(OL_QUERY_FIELD_SUFFIX)) {
                                        string originalName = name.Replace(OL_QUERY_FIELD_SUFFIX, "");
                                        newQuery[originalName] = entry.Value.ToString();
                                    } else if (name.Contains(OL_HEADER_FIELD_SUFFIX)) {
                                        string originalName = name.Replace(OL_HEADER_FIELD_SUFFIX, "");
                                        this.Context.Request.Headers.Add(originalName, entry.Value.ToString());
                                    } else {                                        
                                        // body                                        
                                        newBody[name] = entry.Value;                                        
                                    }
                                }
                            }
                            var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
                            uriBuilder.Path = uriBuilder.Path.Replace("/dummy-" + this.Context.OperationId, formattedRoutingKey);
                            uriBuilder.Query = newQuery.ToString();
                            this.Context.Request.RequestUri = uriBuilder.Uri;
                            var httpMethod = jsonContent["enrichment"]?["method"]?.ToString() ?? "POST";
                            this.Context.Request.Method = new HttpMethod(httpMethod);
                            this.Context.Request.Content = CreateJsonContent(newBody.ToString());
                        } catch (JsonReaderException ex) {
                            throw new ConnectorException(HttpStatusCode.BadGateway, "Unable to parse a content of original request: " + originalRequestContent, ex);
                        }
                    } else {
                        throw new ConnectorException(HttpStatusCode.InternalServerError, "Unable to get User's API endpoint (routingKey) from the response");
                    }
                } else {
                    throw new ConnectorException(methodMetadataResponse.StatusCode, "Cannot get method metadata");
                }

            } catch (HttpRequestException ex) {
                throw new ConnectorException(HttpStatusCode.BadGateway, "Unable to get Method Metadata: " + ex.Message, ex);
            } catch (JsonReaderException ex) {
                throw new ConnectorException(HttpStatusCode.BadGateway, "Unable to parse Method Metadata response: "+ content, ex);
            } catch (UriFormatException ex) {
                throw new ConnectorException(HttpStatusCode.BadGateway, "Unable to construct User's API endpoint from the response. "+ ex.Message, ex);
            }
        }
    }

    private async Task UpdateResponse(HttpResponseMessage response) {
        // remove query, path, headers params from GetMethodOpenApiSpec operation response
        if ("GetMethodOpenApiSpec".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)) {
            // Do the transformation if the response was successful, otherwise return error responses as-is
            if (response.IsSuccessStatusCode) {
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);

                var result = JObject.Parse(responseString);
                JObject inputProperties = (JObject) result["flow-input"]?["properties"];

                var updatedProperties = new JObject();
                foreach (var entry in inputProperties) {
                    string name = entry.Key;
                    string newName = name;
                    JToken jValue = (JToken) entry.Value;
                    if (jValue["in"] != null && jValue["in"].ToString().Equals("query", StringComparison.OrdinalIgnoreCase)) {
                        jValue["title"] = name + " (Query)";
                        newName = name + OL_QUERY_FIELD_SUFFIX;
                    }
                    if (jValue["in"] != null && jValue["in"].ToString().Equals("path", StringComparison.OrdinalIgnoreCase)) {
                        jValue["title"] = name + " (Path)";
                        newName = name + OL_PATH_FIELD_SUFFIX;
                    }
                    if (jValue["in"] != null && jValue["in"].ToString().Equals("header", StringComparison.OrdinalIgnoreCase)) {
                        jValue["title"] = name + " (Header)";
                        newName = name + OL_HEADER_FIELD_SUFFIX;
                    }
                    if (jValue["in"] != null && jValue["in"].ToString().Equals("body", StringComparison.OrdinalIgnoreCase)) {
                        jValue["title"] = name + " (Body)";
                        newName = name;
                    }
                    updatedProperties.Add(newName, jValue);
                }
                result["flow-input"]["properties"] = updatedProperties;
                response.Content = CreateJsonContent(result.ToString());
            }
        } else if ("GetMethodsForContract".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)) {
            // Do the transformation if the response was successful, otherwise return error responses as-is
            if (response.IsSuccessStatusCode) {
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);

                var result = JObject.Parse(responseString);
                JArray elements = (JArray) result["elements"] ?? new JArray();

                foreach (var entry in elements) {
                    string httpMethod = entry["enrichment"]?["method"]?.ToString() ?? "POST";
                    string name = entry["name"].ToString();
                    entry["combinedRoute"] = $"{name} - {httpMethod}".Trim();
                }
                response.Content = CreateJsonContent(result.ToString());
            }
        } else if ("GetAllProjects".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)) {
            // Do the transformation if the response was successful, otherwise return error responses as-is
            if (response.IsSuccessStatusCode) {
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);

                var result = JObject.Parse(responseString);
                var flatElements = new JArray();

                JArray elements = (JArray) result["elements"] ?? new JArray();
                foreach (var entry in elements) {
                    string name = entry["name"]?.ToString() ?? "Unknown";
                    JObject versionsObj = (JObject) entry["versions"] ?? new JObject();
                    foreach (var versionPair in versionsObj) {
                        JObject versionElement = (JObject) versionPair.Value;

                        var reason = versionElement["reason"];
                        JProperty transformedName = new JProperty("name", name);
                        if (versionsObj.Count > 1) {
                            transformedName = new JProperty("name", name + $" ({reason})");
                        }

                        var contractId = versionElement["contractId"];
                        JProperty transformedContractId = new JProperty("contractId", contractId);

                        JObject transformedVersion = new JObject();
                        transformedVersion.Add(transformedName);
                        transformedVersion.Add(transformedContractId);

                        flatElements.Add(transformedVersion);
                    }
                }
                result.Add("flatElements", flatElements);
                response.Content = CreateJsonContent(result.ToString());
            }
        }
    }

    public class ConnectorException : Exception {
        public ConnectorException(
            HttpStatusCode statusCode,
            string message,
            Exception innerException = null) : base(message, innerException) {
            this.StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }

        public override string ToString() {
            var error = new StringBuilder("ConnectorException: Status code=" + this.StatusCode + ", Message='" + this.Message + "'");
            var inner = this.InnerException;
            var level = 0;
            while (inner != null && level < 10) {
                level += 1;
                error.AppendLine("Inner exception " + level + ": " + inner.Message);
                inner = inner.InnerException;
            }

            error.AppendLine("Stack trace: " + this.StackTrace);
            return error.ToString();
        }
    }
}