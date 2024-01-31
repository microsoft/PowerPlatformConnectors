using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var domain = this.Context.Request.Headers.GetValues("Instance").First();
        if (Uri.IsWellFormedUriString(domain, UriKind.Absolute))
        {
            Uri uri = new Uri(domain);
            domain = uri.Host;
        }

        var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
        uriBuilder.Host = domain;
        this.Context.Request.RequestUri = uriBuilder.Uri;

        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        if (response.IsSuccessStatusCode && IsTransformable(this.Context))
        {
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            response = await ConvertToObjects(responseContent);
        }

        return response;
    }

    private bool IsTransformable(IScriptContext Context)
    {
        return (this.Context.OperationId == "ExecuteSqlStatement" || this.Context.OperationId == "GetResults");
    }

    private async Task<HttpResponseMessage> ConvertToObjects(string content)
    {
        try
        {
            var contentAsJson = JObject.Parse(content);

            // check for parameters
            if (contentAsJson["data"] == null || contentAsJson["resultSetMetaData"] == null || contentAsJson["resultSetMetaData"]["rowType"] == null)
            {
                throw new Exception("resultSetMetaData or data parameter are empty!");
            }

            // get metadata
            var cols = JArray.Parse(contentAsJson["resultSetMetaData"]["rowType"].ToString());
            var rows = JArray.Parse(contentAsJson["data"].ToString());

            JArray newRows = new JArray();

            foreach (var row in rows)
            {
                JObject newRow = new JObject();
                int i = 0;

                foreach (var col in cols)
                {
                    var name = col["name"].ToString();
                    string type = col["type"].ToString();
                    if (newRow.ContainsKey(name)) name = name + "_" + Convert.ToString(i);

                    switch (type)
                    {
                        case "fixed":
                            long scale = 0;
                            long.TryParse(col["scale"].ToString(), out scale);
                            if (scale == 0)
                            {
                                long myLong = long.Parse(row[i].ToString());
                                newRow.Add(new JProperty(name.ToString(), myLong));
                            }
                            else
                            {
                                double myDouble = double.Parse(row[i].ToString());
                                newRow.Add(new JProperty(name.ToString(), myDouble));
                            }
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

            var result = new ConnectorResponse
            {
                Data = newRows,
                Metadata = new ConnectorResponseMetadata
                {
                    Rows = Convert.ToInt64(contentAsJson["resultSetMetaData"]["numRows"]),
                    Code = contentAsJson["code"].ToString(),
                    StatementStatusUrl = contentAsJson["statementStatusUrl"].ToString(),
                    RequestId = contentAsJson["requestId"].ToString(),
                    SqlState = contentAsJson["sqlState"].ToString(),
                    StatementHandle = contentAsJson["statementHandle"].ToString(),
                    CreatedOn = contentAsJson["createdOn"].ToString()
                }
            };

            var responseObj = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = CreateJsonContent(JsonConvert.SerializeObject(result))
            };

            return responseObj;
        }
        catch (JsonReaderException ex)
        {
            return createErrorResponse("'resultSetMetadata' or 'data' are in an invalid format: " + ex, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            return createErrorResponse(ex.GetType() + ":" + ex, HttpStatusCode.InternalServerError);
        }
    }

    private HttpResponseMessage createErrorResponse(String msg, HttpStatusCode code)
    {
        JObject output = new JObject
        {
            ["Message"] = msg
        };
        var response = new HttpResponseMessage(code);
        response.Content = CreateJsonContent(output.ToString());
        return response;
    }

    public class ConnectorResponseMetadata
    {
        public long Rows { get; set; }

        public string Code { get; set; }

        public string StatementStatusUrl { get; set; }

        public string RequestId { get; set; }

        public string SqlState { get; set; }

        public string StatementHandle { get; set; }

        public string CreatedOn { get; set; }
    }

    public class ConnectorResponse
    {
        public object Data { get; set; }

        public ConnectorResponseMetadata Metadata { get; set; }
    }
}
