public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
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

            }
            
            return response;
        }
        catch (Exception ex)
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent("error: "+ ex.Message)
            };
            return response;
        }

    }
}