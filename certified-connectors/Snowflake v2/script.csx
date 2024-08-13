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

    private const string QueryString_Partition = "partition";

    #endregion

    public HttpResponseMessage TestConvert(string content, string operationId)
    {
        return ConvertToObjects(content, operationId);
    }

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (Context.OperationId == OP_CONVERT)
        {
            var content = await Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);

            return ConvertToObjects(content, Context.OperationId).GetAsResult();
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

        HttpResponseMessage response = await Context.SendAsync(Context.Request, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        if (response.IsSuccessStatusCode && IsTransformable())
        {
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var results = ConvertToObjects(responseContent, Context.OperationId);
            
            // if this parameter is set in PowerApps then fetch all partitions, instead of making them page manually.
            if(fixme_Params.FetchAllPartitions)
            {
                // yes, this starts at 1 because we've already fetched the first partition.
                for(var i = 1;i < results.Partitions;i++)
                {
                    fixme_setPartitionParam = i;
                    response = await Context.SendAsync(Context.Request, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                    responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var partitionResult = ConvertObjectResult(responseContent, Context.OperationId);
                    foreach(var item in partitionResult.Data)
                    {
                        results.Data.Add(item);
                    }
                }
            }
        }

        return response;
    }

    private bool IsUrlValid(string url)
    {
        string patternAccount;
        patternAccount = "^[a-zA-Z0-9-_.]{0,255}.snowflakecomputing.com\\/?$";
        var matchAccount = Regex.Match(url, patternAccount, RegexOptions.Singleline);
        return matchAccount.Success;
    }

    private bool IsTransformable()
    {
        if (Context.OperationId == OP_EXECUTE_SQL
            || Context.OperationId == OP_GET_RESULTS)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private SnowflakeResponse ConvertToObjects(string content, string operationId)
    {
        try
        {
            var contentAsJson = JObject.Parse(content);
            string schema;

            if (operationId == OP_CONVERT)
            {
                // check for parameters
                if (contentAsJson[Attr_Data] == null || (contentAsJson["schema"] == null && contentAsJson["Schema"] == null))
                {
                    throw new Exception($"'{Attr_Schema}' or '{Attr_Data}' parameter are empty!");
                }

                schema = (contentAsJson["schema"] ?? contentAsJson["Schema"]).ToString();
            }
            else
            {
                // check for parameters
                if (contentAsJson[Attr_Data] == null || contentAsJson[Attr_Metadata] == null || contentAsJson[Attr_Metadata][Attr_RowType] == null)
                {
                    throw new Exception($"'{Attr_Metadata}' or '{Attr_Data}' parameter are empty!");
                }

                schema = contentAsJson[Attr_Metadata][Attr_RowType].ToString();
            }

            // get metadata
            var cols = JArray.Parse(schema);
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
                    JToken token = row[i];
                    JToken tokenNull = JValue.CreateNull();
                    if (token == null || Convert.ToString(token) == "null")
                    {
                        newRow.Add(new JProperty(name.ToString(), row[i]));
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
                    CreatedOn = ConvertToUTC(contentAsJson["createdOn"].ToString(), TimeInterval.Milliseconds)
                };

                if (contentAsJson[Attr_Metadata][Attr_PartitionInfo] != null)
                {
                    partitionInfo = contentAsJson[Attr_Metadata][Attr_PartitionInfo].ToString();
                }
            }

            var result = new SnowflakeResponse
            {
                Data = newRows,
                Schema = JsonConvert.DeserializeObject<IList<object>>(schema),
                Partitions = partitionInfo != null ? JsonConvert.DeserializeObject<IList<SnowflakePartitionInfo>>(partitionInfo) : null,
                Metadata = snowflakeMetadata
            };

            return result;
        }
        catch (JsonReaderException ex)
        {
            return new SnowflakeResponse()
            {
                Success = false,
                ErrorStatusCode = HttpStatusCode.BadRequest,
                ErrorMessage = $"'{Attr_Metadata}' or '{Attr_Data}' are in an invalid format: " + ex,
            };
        }
        catch (Exception ex)
        {
            return new SnowflakeResponse()
            {
                Success = false,
                ErrorStatusCode = InternalServerError.BadRequest,
                ErrorMessage = ex.GetType() + ":" + ex,
            };
        }
    }

    private HttpResponseMessage createResponse(HttpStatusCode code, object payload)
    {
        return new HttpResponseMessage(code)
        {
            Content = CreateJsonContent(JsonConvert.SerializeObject(payload,Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Include}))
        };
    }

    private HttpResponseMessage createErrorResponse(HttpStatusCode code, string msg, string? errorReference = null)
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

    public class ConvertObjectResult()
    {
        public SnowflakeResponse ResponseContent { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get ;set; }
        public HttpStatusCode ErrorStatusCode { get; set; }

        public GetAsResponse()
        {
            if(Success)
            {
                return createResponse(HttpStatusCode.OK, ResponseContent);
            }
            else{
                return createErrorResponse(ErrorStatusCode,ErrorMessage);
            }
        }
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

        public string? CreatedOn { get; set; }
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