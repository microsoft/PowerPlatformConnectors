public class Script : ScriptBase
{

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Check if the operation ID matches what is specified in the OpenAPI definition of the connector
        if (this.Context.OperationId == "ExecuteSqlStatement" ||
            this.Context.OperationId == "GetResults")
        {
            
            return await this.ParseandProcessString().ConfigureAwait(false);
        }

        // Handle an invalid operation ID
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent($"Unknown operation ID '{this.Context.OperationId}'");
        return response;
    }

    private async Task<HttpResponseMessage> ParseandProcessString()
    {
        bool rowsAsObjects = true;

        // Use the context to forward/send an HTTP request
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        // Do the transformation if the response was successful, otherwise return error responses as-is
        if (response.IsSuccessStatusCode)
        {
            //var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);

            // Parse as JSON object
            var contentAsJson = JObject.Parse(contentAsString);
           try
            {
                if (response.StatusCode == HttpStatusCode.Accepted) {
                     return response; 
                }

                var rows = contentAsJson["data"];
                
                try
                {


                    var meta = contentAsJson["resultSetMetaData"];
                    try
                    {
                        var cols = meta["rowType"];

                        if (rowsAsObjects)
                        {
                            JArray newRows = new JArray();
                            JProperty newData = new JProperty("data", newRows);
                            foreach (var row in rows)
                            {
                                JObject newRow = new JObject();
                                int i = 1;
                                
                                foreach (var col in cols)
                                {
                                    var name = col["name"];
                                    string type = col["type"].ToString();

                                    switch (type)
                                    {
                                        case "fixed":
                                            long myLong = long.Parse( row[i].ToString());
                                            newRow.Add(new JProperty(name.ToString(), myLong));
                                            break;
                                        case "float":
                                            float myFloat = float.Parse(row[i].ToString());
                                            newRow.Add(new JProperty(name.ToString(), myFloat));
                                            break;
                                        case "boolean":
                                            bool myBool = bool.Parse(row[i].ToString());
                                            newRow.Add(new JProperty(name.ToString(), myBool));
                                            break;
                                        default:
                                            if (type.ToString().IndexOf("time") >= 0)
                                            {

                                                double unixTimeStamp = Convert.ToDouble(row[i].ToString());

                                                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                                                dateTime = dateTime.AddSeconds(unixTimeStamp);

                                                newRow.Add(new JProperty(name.ToString(), dateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")));
                                            }
                                            else
                                            {
                                                var val = row[i];
                                                newRow.Add(new JProperty(name.ToString(), val));
                                            }
                                            break;
                                    }
                                    i++;

                                }

                                newRows.Add(newRow);

                            }

                            rows.Replace(newRows);
                            response.Content = CreateJsonContent(contentAsJson.ToString());
                        }
                    }
                    catch (Exception){
                        response.ReasonPhrase = "Problem with schema (rowType )";
                        response.Headers.Add("ErrorMessage", "Problem with schema (rowType )");
                        response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                        return response; 
                    }
                }
                catch (Exception)
                {
                    response.ReasonPhrase = "Problem with schema (resultSetMetaData )";
                    response.Headers.Add("ErrorMessage", "Problem with schema (resultSetMetaData )");
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    return response;
                }

            }
            catch (Exception)
            {
                response.ReasonPhrase = "Problem with schema (data)";
                response.Headers.Add("ErrorMessage", "Problem with schema (data)");
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return response;
            }

        }
  
        return response;    

    }
}
