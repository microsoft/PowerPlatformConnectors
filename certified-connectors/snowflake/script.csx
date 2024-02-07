using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System;

public class Script : ScriptBase
{
    #region Constants

    private readonly string HEADER_INSTANCE = "Instance";
    private readonly string OP_EXECUTE_SQL = "ExecuteSqlStatement";
    private readonly string OP_GET_RESULTS = "GetResults";

    private readonly string Attr_Metadata = "resultSetMetaData";
    private readonly string Attr_RowType = "rowType";
    private readonly string Attr_Data = "data";

    private readonly string Attr_Column_Name = "name";
    private readonly string Attr_Column_Type = "type";
    private readonly string Attr_Column_Scale = "scale";

    private const string Snowflake_Type_Fixed = "fixed";
    private const string Snowflake_Type_Float = "float";
    private const string Snowflake_Type_Boolean = "boolean";
    private const string Snowflake_Type_Time = "time";

    #endregion

    public async Task<HttpResponseMessage> TestMethod(string content)
    {
        return await ConvertToObjects(content);
    }

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var domain = Context.Request.Headers.GetValues(HEADER_INSTANCE).First();
        if (Uri.IsWellFormedUriString(domain, UriKind.Absolute))
        {
            Uri uri = new Uri(domain);
            domain = uri.Host;
        }

        var uriBuilder = new UriBuilder(Context.Request.RequestUri);
        uriBuilder.Host = domain;
        Context.Request.RequestUri = uriBuilder.Uri;

        HttpResponseMessage response = await Context.SendAsync(Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        if (response.IsSuccessStatusCode && IsTransformable())
        {
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            response = await ConvertToObjects(responseContent);
        }

        return response;
    }

    private bool IsTransformable()
    {
        return (Context.OperationId == OP_EXECUTE_SQL || Context.OperationId == OP_GET_RESULTS);
    }

    private async Task<HttpResponseMessage> ConvertToObjects(string content)
    {
        try
        {
            var contentAsJson = JObject.Parse(content);

            // check for parameters
            if (contentAsJson[Attr_Data] == null || contentAsJson[Attr_Metadata] == null || contentAsJson[Attr_Metadata][Attr_RowType] == null)
            {
                throw new Exception($"'{Attr_Metadata}' or '{Attr_Data}' parameter are empty!");
            }

            // get metadata
            var cols = JArray.Parse(contentAsJson[Attr_Metadata][Attr_RowType].ToString());
            var rows = JArray.Parse(contentAsJson[Attr_Data].ToString());

            JArray newRows = new JArray();

            foreach (var row in rows)
            {
                JObject newRow = new JObject();
                int i = 0;

                foreach (var col in cols)
                {
                    var name = col[Attr_Column_Name].ToString();
                    string type = col[Attr_Column_Type].ToString();
                    if (newRow.ContainsKey(name)) name = name + "_" + Convert.ToString(i);

                    switch (type)
                    {
                        case Snowflake_Type_Fixed:
                            long scale = 0;
                            long.TryParse(col[Attr_Column_Scale].ToString(), out scale);
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

                        case Snowflake_Type_Float:
                            float myFloat = float.Parse(row[i].ToString());
                            newRow.Add(new JProperty(name.ToString(), myFloat));
                            break;

                        case Snowflake_Type_Boolean:
                            bool myBool = bool.Parse(row[i].ToString());
                            newRow.Add(new JProperty(name.ToString(), myBool));
                            break;

                        default:
                            if (type.ToString().IndexOf(Snowflake_Type_Time) >= 0)
                            {
                                var utcTime = ConvertToUTC(row[i].ToString());
                                newRow.Add(new JProperty(name.ToString(), utcTime));
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
                    Rows = Convert.ToInt64(contentAsJson[Attr_Metadata]["numRows"]),
                    Code = contentAsJson["code"].ToString(),
                    StatementStatusUrl = contentAsJson["statementStatusUrl"].ToString(),
                    RequestId = contentAsJson["requestId"].ToString(),
                    SqlState = contentAsJson["sqlState"].ToString(),
                    StatementHandle = contentAsJson["statementHandle"].ToString(),
                    CreatedOn = ConvertToUTC(contentAsJson["createdOn"].ToString(), TimeInterval.Milliseconds)
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
            return createErrorResponse($"'{Attr_Metadata}' or '{Attr_Data}' are in an invalid format: " + ex, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            return createErrorResponse(ex.GetType() + ":" + ex, HttpStatusCode.InternalServerError);
        }
    }

    private HttpResponseMessage createErrorResponse(string msg, HttpStatusCode code)
    {
        JObject output = new JObject
        {
            ["Message"] = msg
        };
        var response = new HttpResponseMessage(code);
        response.Content = CreateJsonContent(output.ToString());
        return response;
    }

    private string ConvertToUTC(string epochTime, TimeInterval timeInterval = TimeInterval.Seconds)
    {
        DateTimeOffset utcTime;
        long longEpochTime = Convert.ToInt64(Convert.ToDouble(epochTime));

        if (timeInterval == TimeInterval.Milliseconds)
        {
            utcTime = DateTimeOffset.FromUnixTimeMilliseconds(longEpochTime);
        }
        else
        {
            utcTime = DateTimeOffset.FromUnixTimeSeconds(longEpochTime);
        }

        return utcTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
    }

    public enum TimeInterval
    {
        Seconds = 1,
        Milliseconds = 2,
    }

    public class ConnectorResponseMetadata
    {
        public long Rows { get; set; }

        public string? Code { get; set; }

        public string? StatementStatusUrl { get; set; }

        public string? RequestId { get; set; }

        public string? SqlState { get; set; }

        public string? StatementHandle { get; set; }

        public string? CreatedOn { get; set; }
    }

    public class ConnectorResponse
    {
        public object? Data { get; set; }

        public ConnectorResponseMetadata? Metadata { get; set; }
    }
}