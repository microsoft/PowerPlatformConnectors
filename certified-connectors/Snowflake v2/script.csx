using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Web;

public class Script : ScriptBase
{
    #region Constants

    private readonly string HEADER_INSTANCE = "Instance";

    private readonly string OP_EXECUTE_SQL = "ExecuteSqlStatement";
    private readonly string OP_GET_RESULTS = "GetResults";
    private readonly string OP_CONVERT = "Convert";

    private readonly string Attr_Metadata = "resultSetMetaData";
    private readonly string Attr_PartitionInfo = "partitionInfo";
    private readonly string Attr_RowType = "rowType";
    private readonly string Attr_Data = "data";
    private readonly string Attr_Schema = "schema";

    private readonly string Attr_Column_Name = "name";
    private readonly string Attr_Column_Type = "type";
    private readonly string Attr_Column_Scale = "scale";

    private const string Snowflake_Type_Fixed = "fixed";
    private const string Snowflake_Type_Float = "float";
    private const string Snowflake_Type_Boolean = "boolean";
    private const string Snowflake_Type_Time = "time";
    private const string Snowflake_Type_Object = "object";
    private const string Snowflake_Type_Array = "array";

    private const string QueryString_Partition = "partition";

    private const string QueryString_Nullable = "nullable";
    private const string QueryString_Async = "async";
    #endregion

    public HttpResponseMessage TestConvert(string content, string operationId)
    {
        return ConvertToObjects_FullReponseWithData(content, operationId, content).GetAsResponse();
    }

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
        {
            var originalContent = await Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (Context.OperationId == OP_CONVERT)
            {
                var content = await Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);

                return ConvertToObjects_FullReponseWithData(content, Context.OperationId, originalContent).GetAsResponse();
            }

            var snowflakeInstanceURL = Context.Request.Headers.GetValues(HEADER_INSTANCE).First();
            if (Uri.IsWellFormedUriString(snowflakeInstanceURL, UriKind.Absolute))
            {
                Uri uri = new Uri(snowflakeInstanceURL);
                snowflakeInstanceURL = uri.Host;
            }
            if (!IsUrlValid(snowflakeInstanceURL))
            {
                return createErrorResponse(HttpStatusCode.BadRequest, "Invalid Instance URL!", "https://docs.snowflake.com/en/developer-guide/sql-api/about-endpoints");
            }

            var uriBuilder = new UriBuilder(Context.Request.RequestUri);
            uriBuilder.Host = snowflakeInstanceURL;
            Context.Request.RequestUri = uriBuilder.Uri;

            // We had to change GetResults to a POST so that we could pass the DataSchema in the request body.
            if(Context.OperationId == OP_GET_RESULTS)
            {
                Context.Request.Method = HttpMethod.Get;
            }

            HttpResponseMessage response = await Context.SendAsync(Context.Request, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            if (response.IsSuccessStatusCode)
            {
                if(IsFullResponseWithData(response))
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var converted = ConvertToObjects_FullReponseWithData(responseContent, Context.OperationId, originalContent);

                    return converted.GetAsResponse();
                }
                else if(IsAsyncResponse(response))
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var converted = ConvertToObjects_AsyncResponse(responseContent, Context.OperationId);

                    return converted.GetAsResponse();
                }
                else
                {
                    return response;
                }
            }
            else
            {
                return response;
            }
        }
        catch(Exception ex)
        {
            Context.Logger.LogError(ex.ToString(), ex);
            throw;
        }
    }
    
    private Dictionary<string, string> GetQueryString()
    {
        var queryStringCollection = HttpUtility.ParseQueryString(Context.Request.RequestUri.Query);
        var result = new Dictionary<string, string>();
        for (var i = 0; i < queryStringCollection.Count; i++)
        {
            result.Add(queryStringCollection.AllKeys[i], queryStringCollection[i]);
        }

        return result;
    }

    private string GetQueryStringParam(string paramName)
    {
        return GetQueryString().TryGetValue(paramName, out var value) ? value : null;
    }

    private void SetQueryStringParam(string paramName, string value)
    {
        var parms = GetQueryString();
        parms[paramName] = value;

        Context.Request.RequestUri = new Uri(Context.Request.RequestUri.AbsolutePath + parms.ToString());
    }

    private bool IsUrlValid(string url)
    {
        string patternAccount;
        patternAccount = "^[a-zA-Z0-9-_.]{0,255}.snowflakecomputing.com\\/?$";
        var matchAccount = Regex.Match(url, patternAccount, RegexOptions.Singleline);
        return matchAccount.Success;
    }

    private bool UseRealNulls()
    {
        var query = HttpUtility.ParseQueryString(Context.Request.RequestUri.Query);
        var nullable = query[QueryString_Nullable] ?? "false";
        Context.Logger.LogInformation("Use Real Nulls: " + nullable);
        return (nullable == "true");
    }

    private bool IsFullResponseWithData(HttpResponseMessage response)
    {
        return (Context.OperationId == OP_EXECUTE_SQL || Context.OperationId == OP_GET_RESULTS) &&
            response.StatusCode == HttpStatusCode.OK;
    }

    private bool IsAsyncResponse(HttpResponseMessage response)
    {
        return (Context.OperationId == OP_EXECUTE_SQL || Context.OperationId == OP_GET_RESULTS) &&
            response.StatusCode == HttpStatusCode.Accepted;
    }

    private ConvertObjectResult ConvertToObjects_AsyncResponse(string content, string operationId)
    {
        try
        {
            var contentAsJson = JObject.Parse(content);

            // There isn't much to the async response besides a message to let you know the query is running in the background
            // and a statement handle so you can retrieve the actual data later using GetResults operation.
            var snowflakeMetadata = new SnowflakeResponseMetadata
            {
                Code = contentAsJson["code"]?.ToString(),
                Message = contentAsJson["message"]?.ToString(),
                StatementStatusUrl = contentAsJson["statementStatusUrl"]?.ToString(),
                StatementHandle = contentAsJson["statementHandle"]?.ToString(),
                StatementHandles = contentAsJson["statementHandles"]?.Select(x => x.ToString()).ToArray(),
            };

            return new ConvertObjectResult()
            {
                Response = new SnowflakeResponse
                {
                    Metadata = snowflakeMetadata,
                },
                Success = true,
                ErrorStatusCode = HttpStatusCode.OK,
            };
        }
        catch (JsonReaderException ex)
        {
            Context.Logger.LogError(ex.ToString(), ex);
            return new ConvertObjectResult()
            {
                Success = false,
                ErrorStatusCode = HttpStatusCode.BadRequest,
                ErrorMessage = $"'{Attr_Metadata}' or '{Attr_Data}' are in an invalid format: " + ex,
            };
        }
        catch (Exception ex)
        {
            Context.Logger.LogError(ex.ToString(), ex);
            return new ConvertObjectResult()
            {
                Success = false,
                ErrorStatusCode = HttpStatusCode.InternalServerError,
                ErrorMessage = $"{ex.GetType()}:{ex}",
            };
        }
    }
    private ConvertObjectResult ConvertToObjects_FullReponseWithData(string content, string operationId, string originalContent)
    {
        try
        {
            var contentAsJson = JObject.Parse(content);
            // When performing a GetResults on the 0 partition, the response body is empty and therefore cannot be parsed.
            var ogContentAsJson = (originalContent ?? string.Empty) == string.Empty ? new JObject() : JObject.Parse(originalContent);
            string? schema;
            Context.Logger.LogDebug($"operationId: {operationId}");
            if (operationId == OP_CONVERT)
            {
                // check for parameters
                if (contentAsJson[Attr_Data] == null || (contentAsJson["schema"] == null && contentAsJson["Schema"] == null))
                {
                    throw new Exception($"['{Attr_Schema}'] or ['{Attr_Data}'] parameter are empty!");
                }

                schema = (contentAsJson["schema"] ?? contentAsJson["Schema"]).ToString();
            }
            else
            {
                // check for parameters
                if (contentAsJson[Attr_Data] == null)
                {
                    throw new Exception($"['{Attr_Data}'] is missing.");
                }
                else
                {
                    schema =  (contentAsJson[Attr_Metadata]?[Attr_RowType] ?? ogContentAsJson["DataSchema"])?.ToString();

                    if(schema is null)
                    {
                        throw new Exception($"['{Attr_Metadata}']['{Attr_RowType}'] values are missing from response, very likely because you are fetching a non-zero partition. If that is the case then you are required to pass ['DataSchema'] in the request body.");
                    }
                }
            }

            // get metadata
            var cols = JArray.Parse(schema);
            var rows = JArray.Parse(contentAsJson[Attr_Data].ToString());

            JArray newRows = new JArray();
            JToken tokenNull = JValue.CreateNull();

            foreach (var row in rows)
            {
                JObject newRow = new JObject();
                int i = 0;

                foreach (var col in cols)
                {
                    var name = col[Attr_Column_Name].ToString();
                    string type = col[Attr_Column_Type].ToString();
                    if (newRow.ContainsKey(name)) name = name + "_" + Convert.ToString(i);
                    JToken token = row[i];
                    if (token.Type == JTokenType.Null || token == null)
                    {
                        newRow.Add(new JProperty(name.ToString(), tokenNull));
                    }
                    else if (!UseRealNulls() && Convert.ToString(token) == "null") // This mirrors the behavior of the API which returns the string "null" for null values when nullable is false.
                    {
                        newRow.Add(new JProperty(name.ToString(), token));
                    }
                    else
                    {
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
                            case Snowflake_Type_Object:
                                newRow.Add(new JProperty(name.ToString(), JObject.Parse(row[i].ToString())));
                                break;
                            case Snowflake_Type_Array:
                                newRow.Add(new JProperty(name.ToString(), JArray.Parse(row[i].ToString())));
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
                    }

                    i++;
                }

                newRows.Add(newRow);
            }

            SnowflakeResponseMetadata? snowflakeMetadata = null;
            string? partitionInfo = null;
            if (contentAsJson[Attr_Metadata] != null)
            {
                snowflakeMetadata = new SnowflakeResponseMetadata
                {
                    Rows = Convert.ToInt64(contentAsJson[Attr_Metadata]["numRows"]),
                    Format = contentAsJson[Attr_Metadata]["format"].ToString(),
                    Code = contentAsJson["code"].ToString(),
                    StatementStatusUrl = contentAsJson["statementStatusUrl"].ToString(),
                    RequestId = contentAsJson["requestId"].ToString(),
                    SqlState = contentAsJson["sqlState"].ToString(),
                    StatementHandle = contentAsJson["statementHandle"].ToString(),
                    StatementHandles = contentAsJson["statementHandles"]?.Select(x => x.ToString()).ToArray(),
                    CreatedOn = ConvertToUTC(contentAsJson["createdOn"].ToString(), TimeInterval.Milliseconds)
                };

                if (contentAsJson[Attr_Metadata][Attr_PartitionInfo] != null)
                {
                    partitionInfo = contentAsJson[Attr_Metadata][Attr_PartitionInfo].ToString();
                }
            }

            return new ConvertObjectResult()
            {
                Response = new SnowflakeResponse
                {
                    Data = newRows,
                    Schema = JsonConvert.DeserializeObject<IList<object>>(schema),
                    Partitions = partitionInfo != null ? JsonConvert.DeserializeObject<IList<SnowflakePartitionInfo>>(partitionInfo) : null,
                    Metadata = snowflakeMetadata,
                },
                Success = true,
                ErrorStatusCode = HttpStatusCode.OK,
            };
        }
        catch (JsonReaderException ex)
        {
            Context.Logger.LogError(ex.ToString(), ex);
            return new ConvertObjectResult()
            {
                Success = false,
                ErrorStatusCode = HttpStatusCode.BadRequest,
                ErrorMessage = $"'{Attr_Metadata}' or '{Attr_Data}' are in an invalid format: " + ex,
            };
        }
        catch (Exception ex)
        {
            Context.Logger.LogError(ex.ToString(), ex);
            return new ConvertObjectResult()
            {
                Success = false,
                ErrorStatusCode = HttpStatusCode.InternalServerError,
                ErrorMessage = $"{ex.GetType()}:{ex}",
            };
        }
    }

    private static HttpResponseMessage createResponse(HttpStatusCode code, object payload)
    {
        return new HttpResponseMessage(code)
        {
            Content = CreateJsonContent(JsonConvert.SerializeObject(payload, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Include }))
        };
    }

    private static HttpResponseMessage createErrorResponse(HttpStatusCode code, string msg, string? errorReference = null)
    {
        JObject output = new JObject
        {
            ["Message"] = msg,
            ["Reference"] = errorReference
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

    #region Sub classes

    public class ConvertObjectResult
    {
        public SnowflakeResponse Response { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode ErrorStatusCode { get; set; }

        public HttpResponseMessage GetAsResponse()
        {
            if (Success)
            {
                return createResponse(HttpStatusCode.OK, Response);
            }
            else
            {
                return createErrorResponse(ErrorStatusCode, ErrorMessage);
            }
        }
    }

    public class PerformanceData
    {
        public DateTimeOffset BeginFetch {get;set;}
        public DateTimeOffset EndFetch {get;set;}
        public int FetchDurationSeconds {get;set;}

        public DateTimeOffset? BeginConvert {get;set;}
        public DateTimeOffset? EndConvert {get;set;}
        public int? ConvertDurationSeconds {get;set;}
    }

    public class SnowflakeResponseMetadata
    {
        public long Rows { get; set; }

        public string? Format { get; set; }

        public string? Code { get; set; }

        public string? StatementStatusUrl { get; set; }

        public string? RequestId { get; set; }

        public string? SqlState { get; set; }

        public string? StatementHandle { get; set; }

        public string[]? StatementHandles { get; set; }

        public string? CreatedOn { get; set; }

        public string? Message { get; set; }
    }

    public class SnowflakePartitionInfo
    {
        public int RowCount { get; set; }

        public int? UncompressedSize { get; set; }

        public int? CompressedSize { get; set; }
    }

    public class SnowflakeEntitySchema
    {
        public string? Name { get; set; }

        public string? Database { get; set; }

        public string? Schema { get; set; }

        public string? Table { get; set; }

        public bool? Nullable { get; set; }

        public long? Precision { get; set; }

        public long? Scale { get; set; }

        public long? ByteLength { get; set; }

        public string? Collation { get; set; }

        public long? Length { get; set; }

        public string? Type { get; set; }
    }

    public class SnowflakeResponse
    {
        public IList<SnowflakePartitionInfo>? Partitions { get; set; }

        public IList<object>? Schema { get; set; }

        public JArray Data { get; set; }

        public SnowflakeResponseMetadata? Metadata { get; set; }
    }

    #endregion
}
