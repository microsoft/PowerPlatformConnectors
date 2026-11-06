using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    private const int MaximumFileBytes = 10 * 1024 * 1024;
    private const string ApiOrigin = "https://api.folderit.com";
    private class InputError : Exception { public InputError(string message) : base(message) { } }
    private class ApiError : Exception
    {
        public HttpResponseMessage Response;
        public ApiError(HttpResponseMessage response) { Response = response; }
    }

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
        {
            string operation = Context.OperationId;
            if (!Regex.IsMatch(operation ?? "", "^[A-Za-z]+$"))
            {
                try { operation = Encoding.UTF8.GetString(Convert.FromBase64String(operation)); }
                catch (FormatException) { throw new InputError("Invalid operation identifier."); }
            }
            switch (operation)
            {
                case "UploadFile": return await Upload(false);
                case "AddFileVersion": return await Upload(true);
                case "DownloadFile": return await Download();
                case "UpdateMetadata": return await UpdateMetadata();
                case "GetMetadata": return await ProjectMetadata(false);
                case "ListMetadataFields": return await ProjectMetadata(true);
                case "ListAccounts": return await ListAccounts();
                default:
                    ValidateApiUri(Context.Request.RequestUri);
                    CleanQuery(Context.Request);
                    return await Context.SendAsync(Context.Request, CancellationToken);
            }
        }
        catch (InputError e) { return Error(400, e.Message); }
        catch (JsonException) { return Error(400, "The supplied JSON is invalid."); }
        catch (ApiError e) { return e.Response; }
        catch (OperationCanceledException) { return Error(409, "The request timed out. Check the exact record before retrying a write.", "unknown"); }
        catch (Exception) { return Error(409, "The operation could not be verified. Check the exact record before retrying a write.", "unknown"); }
    }

    private HttpResponseMessage Error(int status, string message, string state = null)
    {
        var payload = new JObject { ["name"] = "Folderit connector error", ["message"] = message, ["status"] = status };
        if (state != null) payload["mutationState"] = state;
        return Json(status, payload);
    }
    private HttpResponseMessage Json(int status, JToken payload)
    {
        return new HttpResponseMessage((HttpStatusCode)status) { Content = CreateJsonContent(payload.ToString(Newtonsoft.Json.Formatting.None)) };
    }
    private JToken Parse(string text)
    {
        return JsonConvert.DeserializeObject<JToken>(text,new JsonSerializerSettings {DateParseHandling=DateParseHandling.None});
    }
    private void ValidateApiUri(Uri uri)
    {
        if (uri == null || uri.Scheme != "https" || uri.Host != "api.folderit.com" || !uri.IsDefaultPort || uri.UserInfo.Length != 0)
            throw new InputError("Folderit requests must use the production API host.");
    }
    private void CleanQuery(HttpRequestMessage request)
    {
        // The Microsoft test editor serializes optional blank inputs as query=value.
        // Folderit's cursor decoder must not receive an empty cursor string.
        var uri = new UriBuilder(request.RequestUri);
        var parts = uri.Query.TrimStart('?').Split('&').Where(p => p.Length > 0).ToArray();
        var kept = parts.Where(p => {
            int equals=p.IndexOf('=');
            return equals < 0 || Uri.UnescapeDataString(p.Substring(equals+1)).Length > 0;
        }).ToArray();
        if (kept.Length != parts.Length) { uri.Query=String.Join("&",kept); request.RequestUri=uri.Uri; }
    }
    private Uri ValidateStorageUri(string text)
    {
        Uri uri;
        if (!Uri.TryCreate(text, UriKind.Absolute, out uri) || uri.Scheme != "https" || !uri.IsDefaultPort || uri.UserInfo.Length != 0 || uri.IsLoopback)
            throw new InputError("Folderit returned an invalid storage URL.");
        // Only service-returned HTTPS storage URLs are used. There is no caller-provided URL input.
        string host = uri.Host.ToLowerInvariant();
        if (!(host.EndsWith(".amazonaws.com") || host.EndsWith(".linodeobjects.com") || host.EndsWith(".folderit.com")))
            throw new InputError("The returned storage host has not been verified for this connector.");
        return uri;
    }
    private string Uid(string value)
    {
        if (!Regex.IsMatch(value ?? "", "^[A-Za-z0-9_-]{10}$")) throw new InputError("An exact ten character Folderit UID is required.");
        return value;
    }
    private string AccountPath()
    {
        ValidateApiUri(Context.Request.RequestUri);
        var match = Regex.Match(Context.Request.RequestUri.AbsolutePath, "^/v2/accounts/([A-Za-z0-9_-]{10})(?:/|$)");
        if (!match.Success) throw new InputError("An exact account UID is required.");
        return "/v2/accounts/" + match.Groups[1].Value;
    }
    private string RouteUid(string name)
    {
        var match = Regex.Match(Context.Request.RequestUri.AbsolutePath, "/" + name + "/([A-Za-z0-9_-]{10})(?:/|$)");
        if (!match.Success) throw new InputError("An exact resource UID is required.");
        return match.Groups[1].Value;
    }
    private async Task<JObject> Body()
    {
        if (Context.Request.Content == null) throw new InputError("Action inputs are required.");
        string text = await Context.Request.Content.ReadAsStringAsync();
        if (text.Length > 14 * 1024 * 1024) throw new InputError("Action input exceeds the supported size.");
        return (JObject)Parse(text);
    }
    private async Task<HttpResponseMessage> Api(HttpMethod method, string path, JToken payload = null)
    {
        var uri = new Uri(ApiOrigin + path);
        ValidateApiUri(uri);
        var request = new HttpRequestMessage(method, uri);
        request.Headers.Authorization = Context.Request.Headers.Authorization;
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        IEnumerable<string> key;
        if (Context.Request.Headers.TryGetValues("X-Folderit-Request-Key", out key)) request.Headers.TryAddWithoutValidation("X-Folderit-Request-Key", key);
        if (payload != null) request.Content = CreateJsonContent(payload.ToString(Newtonsoft.Json.Formatting.None));
        return await Context.SendAsync(request, CancellationToken);
    }
    private async Task<JToken> Read(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode) throw new ApiError(response);
        return Parse(await response.Content.ReadAsStringAsync());
    }
    private JArray MetadataValues(JArray values)
    {
        return new JArray(values.Select(v => new JObject {
            ["metaUid"] = (string)v["meta"]?["uid"], ["entityMetaUid"] = (string)v["uid"],
            ["name"] = (string)v["meta"]?["name"], ["type"] = (string)v["meta"]?["type"],
            ["valueJson"] = (v["value"] ?? JValue.CreateNull()).ToString(Newtonsoft.Json.Formatting.None)
        }));
    }
    private async Task<HttpResponseMessage> ListAccounts()
    {
        ValidateApiUri(Context.Request.RequestUri);
        var uri=new UriBuilder(Context.Request.RequestUri); uri.Query="";
        Context.Request.RequestUri=uri.Uri;
        var response=await Context.SendAsync(Context.Request,CancellationToken);
        if (!response.IsSuccessStatusCode) return response;
        var accounts=(JArray)await Read(response);
        if (accounts.Count>1000) return Error(413,"Account discovery exceeds this candidate's one thousand account limit.");
        return response;
    }
    private async Task<HttpResponseMessage> ProjectMetadata(bool definitions)
    {
        ValidateApiUri(Context.Request.RequestUri);
        CleanQuery(Context.Request);
        if (definitions) { var uri=new UriBuilder(Context.Request.RequestUri); uri.Query=""; Context.Request.RequestUri=uri.Uri; }
        var response = await Context.SendAsync(Context.Request, CancellationToken);
        if (!response.IsSuccessStatusCode) return response;
        var values = (JArray)await Read(response);
        if (definitions && values.Count>1000) return Error(413,"Metadata discovery exceeds this candidate's one thousand field limit.");
        JToken projected = definitions ? new JArray(values.Select(v => new JObject {
            ["uid"] = (string)v["uid"], ["name"] = (string)v["name"], ["type"] = (string)v["type"],
            ["multiple"] = (bool?)v["multiple"] ?? false,
            ["optionsJson"] = (v["options"] ?? new JArray()).ToString(Newtonsoft.Json.Formatting.None)
        })) : MetadataValues(values);
        var result = Json(200, projected);
        foreach (var header in response.Headers.Where(h => h.Key.StartsWith("X-Pagination-", StringComparison.OrdinalIgnoreCase) || h.Key == "Link"))
            result.Headers.TryAddWithoutValidation(header.Key, header.Value);
        return result;
    }
    private void ValidateValue(JObject definition, JToken value)
    {
        if ((bool?)definition["multiple"] == true) throw new InputError("Multiple value metadata is outside this first release.");
        string type = (string)definition["type"];
        if (type == "datetime") throw new InputError("Datetime writes are disabled in this candidate because the live API rejected its documented format. Existing datetime values remain readable.");
        if (!new[] {"string","text","integer","float","date","time","boolean","url","email","select"}.Contains(type))
            throw new InputError("This metadata type is outside the first release.");
        if (value.Type == JTokenType.Null) return; // Clearing must be explicit JSON null.
        if (type == "integer")
        {
            if (value.Type != JTokenType.Integer || (decimal)value > 9007199254740991m || (decimal)value < -9007199254740991m) throw new InputError("Integer metadata requires a safe integer.");
            return;
        }
        if (type == "boolean") { if (value.Type != JTokenType.Boolean) throw new InputError("Boolean metadata requires true or false."); return; }
        if (value.Type != JTokenType.String) throw new InputError("This metadata field requires a string value.");
        string text = (string)value;
        if (text.Length == 0) throw new InputError("Use explicit null to clear a value.");
        if (type == "string" && text.Length > 512) throw new InputError("Short text metadata is limited to 512 characters.");
        if (type == "float")
        {
            if (!Regex.IsMatch(text,"^-?(?:0|[1-9][0-9]*)(?:\\.[0-9]+)?$")) throw new InputError("Decimal metadata requires canonical decimal text.");
            int scale = (int?)definition["decimalScale"] ?? 18;
            if (text.Contains(".") && text.Split('.')[1].Length > scale) throw new InputError("Decimal precision exceeds the field definition.");
        }
        if (type == "date")
        {
            if (!Regex.IsMatch(text,"^[0-9]{4}-[0-9]{2}-[0-9]{2}$")) throw new InputError("Date metadata requires YYYY-MM-DD.");
            try { new DateTime(Int32.Parse(text.Substring(0,4)),Int32.Parse(text.Substring(5,2)),Int32.Parse(text.Substring(8,2))); }
            catch(ArgumentOutOfRangeException) { throw new InputError("Date metadata requires a valid calendar date."); }
        }
        if (type == "time")
        {
            if (!Regex.IsMatch(text,"^(?:[01][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$")) throw new InputError("Time metadata requires HH:mm:ss.");
        }
        if (type == "url")
        {
            Uri uri;
            if (!Uri.TryCreate(text,UriKind.Absolute,out uri) || !(uri.Scheme == "https" || uri.Scheme == "http")) throw new InputError("URL metadata requires an HTTP or HTTPS URL.");
        }
        if (type == "email" && !Regex.IsMatch(text,"^[^\\s@]+@[^\\s@]+\\.[^\\s@]+$")) throw new InputError("Email metadata requires an email address.");
        if (type == "select" && !((JArray)definition["options"] ?? new JArray()).Any(o => (string)o["uid"] == text)) throw new InputError("Select metadata requires an exact option UID from the field definition.");
    }
    private bool SameValue(JToken expected, JToken actual, string type)
    {
        if (JToken.DeepEquals(expected,actual)) return true;
        return false;
    }
    private async Task<HttpResponseMessage> UpdateMetadata()
    {
        string account = AccountPath(), record = RouteUid("entities");
        var inputs = await Body();
        string raw = (string)inputs["fieldsJson"];
        if (raw == null || raw.Length > 65536) throw new InputError("Provide a bounded fieldsJson array.");
        var fields = (JArray)Parse(raw);
        if (fields.Count == 0 || fields.Count > 50) throw new InputError("Provide between one and fifty metadata fields.");
        var seen = new HashSet<string>(); var types = new Dictionary<string,string>();
        await Read(await Api(HttpMethod.Get,account + "/entities/" + record));
        foreach (JObject field in fields)
        {
            if (field.Properties().Any(p => p.Name != "metaUid" && p.Name != "value") || field["value"] == null) throw new InputError("Each field requires exactly metaUid and value.");
            string metaUid = Uid((string)field["metaUid"]);
            if (!seen.Add(metaUid)) throw new InputError("Duplicate metadata UIDs are not allowed.");
            var definition = (JObject)await Read(await Api(HttpMethod.Get,account + "/meta/" + metaUid));
            if ((string)definition["uid"] != metaUid) throw new InputError("Metadata definition identity did not match.");
            ValidateValue(definition,field["value"]); types[metaUid] = (string)definition["type"];
        }
        var write = await Api(HttpMethod.Put,account + "/entities/" + record + "/meta",new JObject {["fields"]=fields});
        if (!write.IsSuccessStatusCode)
            return Error(409,"Folderit returned HTTP " + (int)write.StatusCode + ". The batch may have partially applied. Read the record before retrying.","partial_or_unknown");
        JArray readback;
        try { readback = (JArray)await Read(await Api(HttpMethod.Get,account + "/entities/" + record + "/meta?expand=meta")); }
        catch (Exception) { return Error(409,"Metadata was written but readback could not be completed. Check the exact record.","written_unverified"); }
        foreach (JObject field in fields)
        {
            string metaUid=(string)field["metaUid"];
            var matches=readback.Where(v=>(string)v["meta"]?["uid"]==metaUid).ToArray();
            if (field["value"].Type == JTokenType.Null && matches.Length == 0) continue;
            if (matches.Length != 1 || !SameValue(field["value"],matches[0]["value"],types[metaUid]))
                return Error(409,"Metadata write did not match independent readback. Check the exact record.","written_unverified");
        }
        return Json(200,new JObject {["verified"]=true,["values"]=MetadataValues(readback)});
    }
    private async Task<HttpResponseMessage> Upload(bool version)
    {
        string account=AccountPath(); var input=await Body();
        string target=Uid(version ? RouteUid("files") : (string)input["folderUid"]);
        string fileName=(string)input["fileName"], contentType=(string)input["contentType"] ?? "application/octet-stream";
        if (String.IsNullOrWhiteSpace(fileName) || fileName.Length > 255 || fileName.IndexOfAny(new[] {'\\','/',':','*','?','<','>','|'})>=0) throw new InputError("Provide a valid Folderit filename.");
        string encoded=(string)input["fileContent"];
        if (encoded == null || encoded.Length > 13981016) return Error(413,"This first release supports files up to 10 MiB.");
        byte[] bytes;
        try { bytes=Convert.FromBase64String(encoded); } catch(FormatException) { throw new InputError("File content must contain base64 encoded bytes."); }
        if (bytes.Length == 0 || bytes.Length > MaximumFileBytes) return Error(413,"Provide a nonempty file up to 10 MiB.");
        MediaTypeHeaderValue mime;
        if (!MediaTypeHeaderValue.TryParse(contentType,out mime)) throw new InputError("Provide a valid MIME type.");
        var existing=(JObject)await Read(await Api(HttpMethod.Get,account+(version?"/files/":"/folders/")+target));
        if ((string)existing["uid"] != target) throw new InputError("Target identity did not match.");
        if (version && (bool?)existing["isLocked"]==true) return Error(409,"The file is locked. The connector does not override locks.");
        var create=new JObject {["action"]="create",[version?"fileUid":"folderUid"]=target,["fileName"]=fileName,["fileSize"]=bytes.Length,["contentType"]=contentType,["parts"]=new JObject {["1"]=bytes.Length}};
        var session=(JObject)await Read(await Api(HttpMethod.Post,account+"/files/upload",create));
        string uploadId=(string)session["uploadId"], key=(string)session["key"];
        if (String.IsNullOrEmpty(uploadId)||String.IsNullOrEmpty(key)) return Error(409,"Upload session could not be verified.","unknown");
        bool completionStarted=false;
        try
        {
            string url=(string)session["urls"]?["1"];
            if (String.IsNullOrEmpty(url))
            {
                var part=(JObject)await Read(await Api(HttpMethod.Post,account+"/files/upload",new JObject {["action"]="part",["uploadId"]=uploadId,["key"]=key,["partNumber"]=1,["contentLength"]=bytes.Length}));
                url=(string)part["url"];
            }
            var put=new HttpRequestMessage(HttpMethod.Put,ValidateStorageUri(url));
            put.Content=new ByteArrayContent(bytes); put.Content.Headers.ContentType=mime;
            // Never copy the Folderit bearer credential or correlation header to storage.
            var storage=await Context.SendAsync(put,CancellationToken);
            if(!storage.IsSuccessStatusCode) throw new InputError("Storage did not accept the file bytes.");
            completionStarted=true;
            var complete=new JObject {["action"]="complete",["uploadId"]=uploadId,["key"]=key,[version?"fileUid":"folderUid"]=target,["fileSize"]=bytes.Length,["fileName"]=fileName};
            var result=(JObject)await Read(await Api(HttpMethod.Post,account+"/files/upload",complete));
            string created=Uid((string)result["uid"]);
            if(version && created!=target) return Error(409,"Version completion returned an unexpected file identity.","written_unverified");
            var verified=(JObject)await Read(await Api(HttpMethod.Get,account+"/files/"+created));
            if ((string)verified["uid"]!=created || (long?)verified["size"]!=bytes.Length || (version && (string)verified["versionUid"]==(string)existing["versionUid"]))
                return Error(409,"Upload completed but file readback did not match. Check the exact record.","written_unverified");
            string hash;
            using(var sha=SHA256.Create()) hash=BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-","").ToLowerInvariant();
            return Json(200,new JObject {["file"]=verified,["byteLength"]=bytes.Length,["sha256"]=hash,["versionAdded"]=version});
        }
        catch(Exception)
        {
            if (!completionStarted)
            {
                try { await Api(HttpMethod.Post,account+"/files/upload",new JObject {["action"]="abort",["uploadId"]=uploadId,["key"]=key}); } catch(Exception) { }
                return Error(409,"Upload stopped before completion and cleanup was attempted. Check the source before retrying.","not_completed");
            }
            return Error(409,"Upload completion is uncertain. Check the destination before retrying; do not create a duplicate.","unknown");
        }
    }
    private async Task<HttpResponseMessage> Download()
    {
        string account=AccountPath(), file=RouteUid("files");
        var response=await Api(HttpMethod.Get,account+"/files/"+file+"/download");
        if(response.StatusCode==HttpStatusCode.Accepted) return Error(202,"Folderit is preparing this download. Retry later.");
        if((int)response.StatusCode==302 || (int)response.StatusCode==303)
        {
            var url=response.Headers.Location;
            var request=new HttpRequestMessage(HttpMethod.Get,ValidateStorageUri(url == null?null:url.ToString()));
            response=await Context.SendAsync(request,CancellationToken);
        }
        if(!response.IsSuccessStatusCode) return response;
        if(response.Content.Headers.ContentLength>MaximumFileBytes) return Error(413,"This first release supports downloads up to 10 MiB.");
        byte[] bytes=await response.Content.ReadAsByteArrayAsync();
        if(bytes.Length>MaximumFileBytes) return Error(413,"This first release supports downloads up to 10 MiB.");
        string hash;
        using(var sha=SHA256.Create()) hash=BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-","").ToLowerInvariant();
        return Json(200,new JObject { ["fileUid"]=file,["fileContent"]=Convert.ToBase64String(bytes),["contentType"]=response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream",["byteLength"]=bytes.Length,["sha256"]=hash });
    }
}
