public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        this.Context.Logger.LogInformation("ExecuteAsync started");

        if (this.Context.OperationId.ToLower() == "getassetproperties")
        {
            return await this.HandleGetAssetSchema().ConfigureAwait(false);
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

        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent($"Unknown operation ID '{this.Context.OperationId}'");
        this.Context.Logger.LogError($"Unknown operation ID '{this.Context.OperationId}'");
        return response;
    }

private async Task<HttpResponseMessage> HandleGetAssetSchema()
{
    this.Context.Logger.LogInformation("HandleGetAssetSchema started");

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

    // Add the X-Socrata-Host header
    this.Context.Request.Headers.Add("X-Socrata-Host", domain);

    // Update the host in the URI
    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri)
    {
        Host = domain
    };
    this.Context.Request.RequestUri = uriBuilder.Uri;

    HttpResponseMessage response;
    try
    {
        response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
    catch (Exception ex)
    {
        this.Context.Logger.LogError($"Error sending request: {ex.Message}");
        response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = CreateJsonContent($"Error sending request: {ex.Message}")
        };
        return response;
    }

    if (response.IsSuccessStatusCode)
    {
        this.Context.Logger.LogInformation("Request successful");

        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
        this.Context.Logger.LogInformation($"Response received: {responseString}");

        var schema = JObject.Parse(responseString);
        var openApiSchema = new JObject
        {
            ["type"] = "object",
            ["properties"] = new JObject(),
            ["required"] = new JArray()
        };

        bool hasPointType = false;

        foreach (var column in schema["columns"])
        {
            string columnName = column["name"].ToString();
            string fieldName = column["fieldName"].ToString();
            string columnType = ConvertToOpenApiType(column["dataTypeName"].ToString());
            string columnDescription = column["description"].ToString();

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

            if (column["dataTypeName"].ToString() == "point")
            {
                hasPointType = true;
            }

            openApiSchema["properties"][fieldName] = property;
        }

        if (hasPointType && !openApiSchema["properties"].Children<JProperty>().Any(prop => prop.Name == "Format"))
        {
            var formatProperty = new JObject
            {
                ["type"] = "string",
                ["enum"] = new JArray("JSON", "GeoJSON")
            };

            ((JObject)openApiSchema["properties"]).AddFirst(new JProperty("Format", formatProperty));
        }

        this.Context.Logger.LogInformation($"Transformed schema: {openApiSchema}");

        var wrappedSchema = new JObject
        {
            ["data"] = openApiSchema,
                ["schema"] = openApiSchema
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
    if (string.IsNullOrEmpty(domain))
    {
        this.Context.Logger.LogError("Domain header is empty.");
        return new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = CreateJsonContent("Domain header is empty.")
        };
    }

    // Add the X-Socrata-Host header
    this.Context.Request.Headers.Add("X-Socrata-Host", domain);

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

    // Modify the request to be a GET and update the URI with new query parameters
    this.Context.Request.Method = HttpMethod.Get;
    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri)
    {
        Query = requestParams.ToString(),
        Host = domain
    };

    // Check if GeoJSON format is requested and update the path accordingly
    if (requestParams["Format"] == "GeoJSON")
    {
        var newPath = uriBuilder.Path.Replace(".json", ".geojson");
        uriBuilder.Path = newPath;
        this.Context.Logger.LogInformation($"GeoJSON format selected. Updated path to: {newPath}");
    }
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
        if (string.IsNullOrEmpty(domain))
        {
            this.Context.Logger.LogError("Domain header is empty.");
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent("Domain header is empty.")
            };
        }

        // Add the X-Socrata-Host header
        this.Context.Request.Headers.Add("X-Socrata-Host", domain);

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
    if (string.IsNullOrEmpty(domain))
    {
        this.Context.Logger.LogError("Domain header is empty.");
        return new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = CreateJsonContent("Domain header is empty.")
        };
    }

    // Add the X-Socrata-Host header
    this.Context.Request.Headers.Add("X-Socrata-Host", domain);

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