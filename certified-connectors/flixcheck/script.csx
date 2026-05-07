    public class Script : ScriptBase
    {
        public override async Task<HttpResponseMessage> ExecuteAsync()
        {
            HttpResponseMessage response;
            try
            {
                var context = GetContext();
                if (context.OperationId.EndsWith("Event"))
                {
                    this.log($"execute {context.OperationId} Operation");

                    var nullableResponse = await this.UpdateEventRequestBody().ConfigureAwait(continueOnCapturedContext: false);
                    if (nullableResponse is not null)
                    {
                        response = nullableResponse;
                    }
                    else
                    {
                        throw new ConnectorException(HttpStatusCode.InternalServerError, "No response recieved");
                    }
                }
                else if (context.OperationId.Equals("GetDynamicSchemaFromTemplate"))
                {
                    response = new HttpResponseMessage(HttpStatusCode.OK);
                    this.log($"execute {context.OperationId} Operation");

                    await this.UpdateDynamicSchemaFromTemplateBodyResponse(response).ConfigureAwait(continueOnCapturedContext: false);
                }
                else if (context.OperationId.Equals("StaticResponseForSendBy"))
                {
                    response = new HttpResponseMessage(HttpStatusCode.OK);
                    this.log($"execute {context.OperationId} Operation");

                    this.UpdateStaticResponseForSendByResponse(response);
                }
                else if (context.OperationId.Equals("StaticResponseForCheckFolders"))
                {
                    response = new HttpResponseMessage(HttpStatusCode.OK);
                    this.log($"execute {context.OperationId} Operation");

                    this.UpdateStaticResponseForCheckFoldersResponse(response);
                }
                else if (context.OperationId.Equals("FilterQuestionBoolElements"))
                {
                    response = new HttpResponseMessage(HttpStatusCode.OK);
                    this.log($"execute {context.OperationId} Operation");

                    await this.FilterQuestionBoolElementsResponse(response).ConfigureAwait(continueOnCapturedContext: false);
                }
                else
                {
                    response = await ExecuteRequest().ConfigureAwait(continueOnCapturedContext: false);
                }

                return response;
            }
            catch (ConnectorException ex)
            {
                response = new HttpResponseMessage(ex.StatusCode);

                if (ex.Message.Contains("ValidationFailure:"))
                {
                    response.Content = CreateJsonContent(ex.JsonMessage());
                }
                else
                {
                    response.Content = CreateJsonContent(ex.Message);
                }

                return response;
            }
        }

        private async Task<HttpResponseMessage> ExecuteRequest()
        {
            HttpRequestMessage request = GetRequest();
            if (this.Context == null)
            {
                throw new ConnectorException(HttpStatusCode.BadRequest, "Context not set");
            }
            return await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        private async Task FilterQuestionBoolElementsResponse(HttpResponseMessage response)
        {
            HttpRequestMessage request = GetRequest();
            var content = await ReadHttpContent(request.Content);
            if (content == null)
            {
                response.Content = CreateJsonContent(@"[]");
                return;
            }
            var allElements = ParseContentAsJArray(content, true);
            if (allElements == null)
            {
                response.Content = CreateJsonContent(@"[]");
                return;
            }
            else
            {
                var boolElements = allElements.OfType<JObject>().Where(element =>
                    element is not null &&
                    "question" == (string)element["type"] &&
                    "bool" == (string)element["subtype"]
                ).ToList();

                response.Content = CreateJsonContent(JsonConvert.SerializeObject(boolElements));
            }
        }

        private async Task<HttpResponseMessage?> UpdateEventRequestBody()
        {
            HttpRequestMessage request = GetRequest();
            var context = GetContext();
            if (request.RequestUri == null)
            {
                return null;
            }
            var content = await ReadHttpContent(request.Content);
            if (content == null)
            {
                return null;
            }
            var jsonBody = ParseContentAsJObject(content, true);
            if (jsonBody == null)
            {
                return null;
            }
            var uriBuilder = new UriBuilder(request.RequestUri);
            var absolutePath = request.RequestUri.AbsolutePath;
            var lastIndexOfSlash = absolutePath.LastIndexOf("/");
            uriBuilder.Path = "/user/v7/connections/powerautomate/subscriptions";

            var eventName = absolutePath.Substring(lastIndexOfSlash + 1);
            jsonBody["event"] = eventName;

            request.RequestUri = uriBuilder.Uri;
            request.Content = CreateJsonContent(JsonConvert.SerializeObject(jsonBody));
            return await ExecuteRequest().ConfigureAwait(continueOnCapturedContext: false);
        }

        private async Task UpdateDynamicSchemaFromTemplateBodyResponse(HttpResponseMessage response)
        {

            var templateId = "";
            try
            {
                var request = GetRequest();
                var requestLog = await FormatRequest(request).ConfigureAwait(continueOnCapturedContext: false);

                var requestUri = request.RequestUri;
                if (requestUri == null)
                {
                    throw new ConnectorException(HttpStatusCode.BadRequest, "Request URI not set");
                }


                var absolutePath = requestUri.AbsolutePath;
                var lastIndexOfSlash = absolutePath.LastIndexOf("/");

                var pathSegments = absolutePath.Split('/');
                var lastSegments = pathSegments.Skip(Math.Max(0, pathSegments.Length - 1)).ToArray();

                templateId = lastSegments[0];

                request.Method = HttpMethod.Get;

                request.RequestUri = new Uri(requestUri, $"/user/v7/templates/{templateId}");

                var templateResponse = await ExecuteRequest().ConfigureAwait(continueOnCapturedContext: false);

                var templateJson = ParseContentAsJObject(ReadHttpContent(templateResponse.Content).Result ?? "", false);

                var newSchema = new JObject
                {
                    ["schema"] = new JObject
                    {
                        ["valuePath"] = GenerateSchema(templateJson["resource"] as JObject)
                    }
                };

                response.Content = CreateJsonContent(newSchema.ToString());
            }
            catch (System.Exception e)
            {
                throw new ConnectorException(HttpStatusCode.InternalServerError, $"Error while updating dynamic schema from templates {templateId} body response: " + e.Message, e);
            }
        }

        private void UpdateStaticResponseForSendByResponse(HttpResponseMessage response)
        {
            var body = new JObject
            { ["value"] = convertKeyLabelListToJArray(sendByOptions) };

            response.Content = CreateJsonContent(body.ToString());
        }

        private void UpdateStaticResponseForCheckFoldersResponse(HttpResponseMessage response)
        {
            var body = new JObject
            { ["value"] = convertKeyLabelListToJArray(checkDefaultFolder) };

            response.Content = CreateJsonContent(body.ToString());
        }

        private void log(string value)
        {
            if (Context != null)
            {
                Context.Logger.Log(LogLevel.Warning, 1, value, null, (state, exception) => state);
            }
        }

        private static JArray convertKeyLabelListToJArray(List<KeyLabel> list)
        {
            return new JArray(
                list.Select(ev =>
                    new JObject
                    {
                        ["key"] = ev.key,
                        ["label"] = ev.label
                    }
                )
            );
        }

        public static async Task<string?> ReadHttpContent(System.Net.Http.HttpContent? content)
        {
            if (content == null)
            {
                return null;
            }
            return await content.ReadAsStringAsync();
        }

        private IScriptContext GetContext()
        {
            if (this.Context == null)
            {
                throw new ConnectorException(HttpStatusCode.BadRequest, "Context not set");
            }
            return this.Context;
        }

        private HttpRequestMessage GetRequest()
        {
            var context = GetContext();
            return context.Request;
        }

        private static JObject ParseContentAsJObject(string content, bool isRequest)
        {
            JObject body;
            try
            {
                body = JObject.Parse(content);
            }
            catch (JsonReaderException ex)
            {
                if (isRequest)
                {
                    throw new ConnectorException(HttpStatusCode.BadRequest, "Unable to parse the request body: " + content, ex);
                }
                else
                {
                    throw new ConnectorException(HttpStatusCode.BadGateway, "Unable to parse the response body: " + content, ex);
                }
            }

            if (body == null)
            {
                if (isRequest)
                {
                    throw new ConnectorException(HttpStatusCode.BadRequest, "The request body is empty");
                }
                else
                {
                    throw new ConnectorException(HttpStatusCode.BadGateway, "The response body is empty");
                }
            }

            return body;
        }

        private static JArray ParseContentAsJArray(string content, bool isRequest)
        {
            JArray body;
            try
            {
                body = JArray.Parse(content);
            }
            catch (JsonReaderException ex)
            {
                if (isRequest)
                {
                    throw new ConnectorException(HttpStatusCode.BadRequest, "Unable to parse the request body: " + content, ex);
                }
                else
                {
                    throw new ConnectorException(HttpStatusCode.BadGateway, "Unable to parse the response body: " + content, ex);
                }
            }

            if (body == null)
            {
                if (isRequest)
                {
                    throw new ConnectorException(HttpStatusCode.BadRequest, "The request body is empty");
                }
                else
                {
                    throw new ConnectorException(HttpStatusCode.BadGateway, "The response body is empty");
                }
            }

            return body;
        }

        static JObject GenerateSchema(JObject json)
        {
            var schema = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject()
            };

            var properties = (JObject)schema["properties"];

            foreach (var prop in json.Properties())
            {
                string type = GetJsonType(prop.Value.Type);
                properties[prop.Name] = new JObject
                {
                    ["type"] = type,
                    ["summary"] = prop.Name,
                    ["description"] = prop.Value.ToString()
                };
                if (type == "array")
                {
                    if (prop.Value is JArray array && array.Count > 0)
                    {
                        properties[prop.Name]["items"] = new JObject
                        {
                            ["type"] = "object",
                            ["properties"] = GenerateSchema((JObject)array[0])["properties"]
                        };
                    }
                }
                else if (type == "object")
                {
                    properties[prop.Name]["properties"] = GenerateSchema((JObject)prop.Value)["properties"];
                }
            }

            return schema;
        }

        static string GetJsonType(JTokenType type)
        {
            return type switch
            {
                JTokenType.String => "string",
                JTokenType.Integer => "integer",
                JTokenType.Float => "number",
                JTokenType.Boolean => "boolean",
                JTokenType.Array => "array",
                JTokenType.Object => "object",
                _ => "string"
            };
        }

        private readonly List<KeyLabel> sendByOptions = new List<KeyLabel>
            {
                new KeyLabel("mobile", "Send Check to Mobile Number"),
                new KeyLabel("email", "Send Check to E-Mail"),
            };

        private readonly List<KeyLabel> checkDefaultFolder = new List<KeyLabel>
            {
                new KeyLabel("inbox", "Check inbox"),
                new KeyLabel("outbox", "Check outbox"),
                new KeyLabel("sent", "Check sent"),
                new KeyLabel("archive", "Check archive"),
                new KeyLabel("trash", "Trash")
            };

        private class KeyLabel
        {
            public string key;
            public string label;
            public KeyLabel(string key, string label)
            {
                this.key = key;
                this.label = label;
            }
        }

        private class ConnectorException : Exception
        {
            public ConnectorException(
                HttpStatusCode statusCode,
                string message,
                Exception? innerException = null)
                : base(
                        message,
                        innerException)
            {
                this.StatusCode = statusCode;
            }

            public HttpStatusCode StatusCode { get; }

            public string JsonMessage()
            {
                var error = new StringBuilder($"{{\"ConnectorException\": \"Status code={this.StatusCode}, Message='{this.Message}'\"}}");
                return error.ToString();
            }

            public override string ToString()
            {
                var error = new StringBuilder($"ConnectorException: Status code={this.StatusCode}, Message='{this.Message}'");
                var inner = this.InnerException;
                var level = 0;
                while (inner != null && level < 10)
                {
                    level += 1;
                    error.AppendLine($"Inner exception {level}: {inner.Message}");
                    inner = inner.InnerException;
                }

                error.AppendLine($"Stack trace: {this.StackTrace}");
                return error.ToString();
            }
        }

        public static async Task<string> FormatRequest(HttpRequestMessage request)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{request.Method} {request.RequestUri}");

            sb.AppendLine("Properties:");

            foreach (var prop in request.Properties)
            {
                sb.AppendLine($"{prop.Key}: {string.Join(", ", prop.Value)}");
            }

            sb.AppendLine("Headers:");

            foreach (var header in request.Headers)
            {
                sb.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }

            if (request.Content != null)
            {
                foreach (var header in request.Content.Headers)
                {
                    sb.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                var content = await request.Content.ReadAsStringAsync();
                sb.AppendLine("Body:");
                sb.AppendLine(content);
            }

            return sb.ToString();
        }
    }