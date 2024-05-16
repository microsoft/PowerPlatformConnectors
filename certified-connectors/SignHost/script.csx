public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
        {
            if (this.Context.OperationId.StartsWith("Getdetails", StringComparison.OrdinalIgnoreCase))
            {

                // Use the context to forward/send an HTTP request
                HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                // Do the transformation if the response was successful, otherwise return error responses as-is
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the JSON to a JObject
                    var jObject = JObject.Parse(await response.Content.ReadAsStringAsync());

                    // Get the "Files" object
                    var filesObject = (JObject?)jObject["Files"];

                    // Prepare a list to hold the transformed files
                    var filesList = new List<JObject>();

                    if (filesObject is not null)
                    {
                        // Iterate over each property (file) in the "Files" object
                        foreach (var file in filesObject.Properties())
                        {
                            // Create a JObject for each file and add it to the list
                            var fileEntry = new JObject
                            {
                                ["Links"] = file.Value["Links"],
                                ["DisplayName"] = file.Name
                            };
                            filesList.Add(fileEntry);
                        }

                        // Create the new JObject to hold the list of files
                        jObject.Remove("Files");
                        var newFilesObject = new JObject
                        {
                            ["Files"] = JArray.FromObject(filesList)
                        };
                        jObject.Add("Files", JArray.FromObject(filesList));

                    }

                    response.Content = CreateJsonContent(jObject.ToString());
                    return response;

                }
            }
        }
        catch (ConnectorException ex)
        {
            var response = new HttpResponseMessage(ex.StatusCode);

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
        return response;

    }
}

public class ConnectorException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ConnectorException(HttpStatusCode statusCode, string message, Exception innerException = null) : base(message, innerException)
    {
        this.StatusCode = statusCode;
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