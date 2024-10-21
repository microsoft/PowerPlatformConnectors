public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        this.Context.Logger.LogInformation("ExecuteAsync started");        

        // Check for the domain header and add X-Socrata-Host header
        if (this.Context.Request.Headers.TryGetValues("domain", out var domainHeaderValues))
        {
            var domain = domainHeaderValues.FirstOrDefault();
            if (!string.IsNullOrEmpty(domain))
            {
                this.Context.Request.Headers.Add("X-Socrata-Host", domain);
            }
        }
        
        if (this.Context.OperationId.ToLower() == "getdatasetschema")
        {
            return await this.HandleGetDatasetSchema().ConfigureAwait(false);
        }
        else if (this.Context.OperationId.ToLower() == "searchdataset")
        {
            return await HandleSearchDatasetOperation().ConfigureAwait(false);
        }
        else if (this.Context.OperationId.ToLower() == "searchdatasetsoql")
        {
            return await HandleSearchDatasetSoQLOperation().ConfigureAwait(false);
        }
        else if (this.Context.OperationId.ToLower() == "getsearchdata")
        {
            return await HandleGetSearchDataOperation().ConfigureAwait(false);
        }

        // If the operation ID does not match any of the predefined handlers, forward the request normally
        HttpResponseMessage response;
        try
        {
            response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.Context.Logger.LogError($"Error forwarding request: {ex.Message}");
            response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = CreateJsonContent($"Error forwarding request: {ex.Message}")
            };
        }

        return response;
    }    

    private async Task<HttpResponseMessage> HandleGetDatasetSchema()
    {
        this.Context.Logger.LogInformation("HandleGetDatasetSchema started");

        // Get the domain header value
        if (!this.Context.Request.Headers.TryGetValues("domain", out var domainHeaderValues))
        {
            this.Context.Logger.LogError("Domain header is missing.");
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent("Domain header is missing.")
            };
        }

        var domain = domainHeaderValues.FirstOrDefault();

        if (string.IsNullOrEmpty(domain))
        {
            this.Context.Logger.LogError("Domain header is empty.");
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent("Domain header is empty.")
            };
        }

        // Update the host in the URI
        var uriBuilder = new UriBuilder(this.Context.Request.RequestUri)
        {
            Host = domain
        };
        this.Context.Request.RequestUri = uriBuilder.Uri;

        HttpResponseMessage response;
        try
        {
            response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.Context.Logger.LogError($"Error sending request: {ex.Message}");
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = CreateJsonContent($"Error sending request: {ex.Message}")
            };
        }

        if (response.IsSuccessStatusCode)
        {
            this.Context.Logger.LogInformation("Request successful");

            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            this.Context.Logger.LogInformation($"Response received: {responseString}");

            JObject schema;
            try
            {
                schema = JObject.Parse(responseString);
            }
            catch (Exception ex)
            {
                this.Context.Logger.LogError($"Error parsing response: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = CreateJsonContent($"Error parsing response: {ex.Message}")
                };
            }

            if (schema["columns"] == null)
            {
                this.Context.Logger.LogError("Columns section is missing in the schema.");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = CreateJsonContent("Columns section is missing in the schema.")
                };
            }

            var openApiSchema = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject(),
                ["required"] = new JArray()
            };

            bool hasGeoType = false;

            foreach (var column in schema["columns"])
            {
                string columnName = column["name"]?.ToString();
                string fieldName = column["fieldName"]?.ToString();
                string columnType = ConvertToOpenApiType(column["dataTypeName"]?.ToString());
                string columnDescription = column["description"]?.ToString();

                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(columnType))
                {
                    this.Context.Logger.LogWarning("Column information is incomplete.");
                    continue;
                }

                var property = new JObject
                {
                    ["type"] = columnType,
                    ["title"] = columnName,
                    ["x-ms-dynamic-values"] = new JObject
                    {
                        ["operationId"] = "GetSearchData",
                        ["value-path"] = "value",
                        ["value-title"] = "value",
                        ["parameters"] = new JObject
                        {
                            ["domain"] = new JObject
                            {
                                ["parameter"] = "domain"
                            },
                            ["resource_id"] = new JObject
                            {
                                ["parameter"] = "resource_id"
                            },
                            ["$query"] = $"SELECT `{fieldName}` AS `__auto_alias_abcdef` |> SELECT DISTINCT `__auto_alias_abcdef` WHERE UPPER(`__auto_alias_abcdef`::text) like \"%%\" |> SELECT `__auto_alias_abcdef`::text as value"
                        }
                    }
                };

                if (!string.IsNullOrEmpty(columnDescription))
                {
                    property["description"] = columnDescription;
                }

                openApiSchema["properties"][fieldName] = property;
            }            

            this.Context.Logger.LogInformation($"Transformed schema: {openApiSchema}");

            var wrappedSchema = new JObject
            {
                ["data"] = openApiSchema,                
            };

            response.Content = CreateJsonContent(wrappedSchema.ToString());
        }
        else
        {
            this.Context.Logger.LogError($"Request failed with status code: {response.StatusCode}");
        }

        return response;
    }

    private string ConvertToOpenApiType(string socrataType)
    {
        this.Context.Logger.LogInformation($"Converting Socrata type '{socrataType}' to OpenAPI type");

        return socrataType switch
        {
            "text" => "string",
            "number" => "integer",
            "checkbox" => "boolean",
            _ => "string"
        };
    }

    private async Task<HttpResponseMessage> HandleSearchDatasetOperation()
    {
        this.Context.Logger.LogInformation("HandleSearchDatasetOperation started");

        // Get the domain header value
        if (!this.Context.Request.Headers.TryGetValues("domain", out var domainHeaderValues))
        {
            this.Context.Logger.LogError("Domain header is missing.");
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent("Domain header is missing.")
            };
        }

        var domain = domainHeaderValues.FirstOrDefault();

        // Get the details of the incoming request
        var requestContent = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);

        // Extract URL parameters
        var requestParams = System.Web.HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);

        // Parse the body to get additional parameters
        if (!string.IsNullOrEmpty(requestContent))
        {
            var bodyParams = JObject.Parse(requestContent);
            foreach (var prop in bodyParams.Properties())
            {
                requestParams[prop.Name] = prop.Value.ToString();
            }
        }

        this.Context.Logger.LogInformation($"Request details before modification: Method={this.Context.Request.Method.Method}, URI={this.Context.Request.RequestUri}, Params={requestParams}");

        // Declare the uriBuilder
        var uriBuilder = new UriBuilder(this.Context.Request.RequestUri)
        {
            Query = requestParams.ToString(),
            Host = domain
        };

        // Update the URI with the new query parameters
        uriBuilder.Query = requestParams.ToString();
        this.Context.Request.RequestUri = uriBuilder.Uri;

        // Modify the request to be a GET
        this.Context.Request.Method = HttpMethod.Get;
        this.Context.Request.Content = null; // Clear the body content

        this.Context.Logger.LogInformation($"Modified request details: Method={this.Context.Request.Method}, URI={this.Context.Request.RequestUri}, Params={requestParams}");

        // Forward the modified request
        HttpResponseMessage response;
        try
        {
            response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.Context.Logger.LogError($"Error forwarding request: {ex.Message}");
            response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = CreateJsonContent($"Error forwarding request: {ex.Message}")
            };
        }

        this.Context.Logger.LogInformation("HandleSearchDatasetOperation completed");

        return response;
    }

    private async Task<HttpResponseMessage> HandleSearchDatasetSoQLOperation()
    {
        this.Context.Logger.LogInformation("HandleSearchDatasetSoQLOperation started");

        // Get the domain header value
        if (!this.Context.Request.Headers.TryGetValues("domain", out var domainHeaderValues))
        {
            this.Context.Logger.LogError("Domain header is missing.");
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent("Domain header is missing.")
            };
        }

        var domain = domainHeaderValues.FirstOrDefault();

        // Extract URL parameters
        var requestParams = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);

        this.Context.Logger.LogInformation($"Request details before modification: Method={this.Context.Request.Method.Method}, URI={this.Context.Request.RequestUri}, Params={requestParams}");

        // Modify the request to be a GET and update the URI with new query parameters
        this.Context.Request.Method = HttpMethod.Get;
        var uriBuilder = new UriBuilder(this.Context.Request.RequestUri)
        {
            Query = requestParams.ToString(),
            Host = domain
        };

        // Remove /SoQL from the path
        var newPath = uriBuilder.Path.Replace("/SoQL", "");
        uriBuilder.Path = newPath;
        this.Context.Logger.LogInformation($"Updated path to: {newPath}");

        this.Context.Request.RequestUri = uriBuilder.Uri;
        this.Context.Request.Content = null; // Clear the body content

        this.Context.Logger.LogInformation($"Modified request details: Method={this.Context.Request.Method}, URI={this.Context.Request.RequestUri}, Params={requestParams}");

        // Forward the modified request
        HttpResponseMessage response;
        try
        {
            response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.Context.Logger.LogError($"Error forwarding request: {ex.Message}");
            response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = CreateJsonContent($"Error forwarding request: {ex.Message}")
            };
        }

        this.Context.Logger.LogInformation("HandleSearchDatasetSoQLOperation completed");

        return response;
    }
    
    private async Task<HttpResponseMessage> HandleGetSearchDataOperation()
    {
        this.Context.Logger.LogInformation("HandleGetSearchDataOperation started");

        // Get the domain header value
        if (!this.Context.Request.Headers.TryGetValues("domain", out var domainHeaderValues))
        {
            this.Context.Logger.LogError("Domain header is missing.");
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent("Domain header is missing.")
            };
        }

        var domain = domainHeaderValues.FirstOrDefault();        

        // Remove /GetSearchData from the path
        var originalUri = this.Context.Request.RequestUri.ToString();
        var newUri = originalUri.Replace("/GetSearchData", "");

        // Update the host in the URI
        var uriBuilder = new UriBuilder(newUri)
        {
            Host = domain
        };

        // Update the request URI
        this.Context.Request.RequestUri = uriBuilder.Uri;

        this.Context.Logger.LogInformation($"Modified request URI: {this.Context.Request.RequestUri}");

        // Forward the modified request
        HttpResponseMessage response;
        try
        {
            response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.Context.Logger.LogError($"Error forwarding request: {ex.Message}");
            response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = CreateJsonContent($"Error forwarding request: {ex.Message}")
            };
        }

        this.Context.Logger.LogInformation("HandleGetSearchDataOperation completed");

        return response;
    }
    
    private static HttpContent CreateJsonContent(string content)
    {
        return new StringContent(content, System.Text.Encoding.UTF8, "application/json");
    }
}