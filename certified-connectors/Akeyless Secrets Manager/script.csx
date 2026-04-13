using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    private const string ApiHost = "https://api.akeyless.io";

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var operationId = this.Context.OperationId;

        if (string.Equals(operationId, "GetSecret", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(operationId, "GetPassword", StringComparison.OrdinalIgnoreCase))
            return await HandleGetSecretAsync(operationId);

        var bad = new HttpResponseMessage(HttpStatusCode.BadRequest);
        bad.Content = CreateJsonContent("{\"error\":\"Unknown operation: " + operationId + "\"}");
        return bad;
    }

    private async Task<HttpResponseMessage> HandleGetSecretAsync(string operationId)
    {
        var bodyText = await this.Context.Request.Content.ReadAsStringAsync();
        var body = string.IsNullOrWhiteSpace(bodyText) ? new JObject() : JObject.Parse(bodyText);

        if (!TryGetCredentials(out var accessId, out var accessKey, body))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent(
                "{\"error\":\"Set Access Id and Access Key on the connection, or pass access-id and access-key in the action body.\"}");
            return err;
        }

        var secretName = (string)body["secret_name"] ?? (string)body["Secret Name"] ?? (string)body["name"];
        if (string.IsNullOrEmpty(secretName))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"secret_name is required\"}");
            return err;
        }

        var useJsonOutput = string.Equals(operationId, "GetPassword", StringComparison.OrdinalIgnoreCase);

        var authReq = new HttpRequestMessage(HttpMethod.Post, ApiHost + "/auth");
        authReq.Content = CreateJsonContent(new JObject
        {
            ["access-id"] = accessId,
            ["access-key"] = accessKey,
            ["access-type"] = "access_key"
        }.ToString());

        var authResp = await this.Context.SendAsync(authReq, this.CancellationToken);
        var authText = await authResp.Content.ReadAsStringAsync();

        if (!authResp.IsSuccessStatusCode)
        {
            var err = new HttpResponseMessage(authResp.StatusCode);
            err.Content = CreateJsonContent(
                new JObject { ["error"] = "Authentication failed", ["details"] = authText }.ToString());
            return err;
        }

        var tToken = (string)JObject.Parse(authText)["token"];

        var secretReq = new HttpRequestMessage(HttpMethod.Post, ApiHost + "/get-secret-value");
        secretReq.Content = CreateJsonContent(new JObject
        {
            ["names"] = new JArray { secretName },
            ["token"] = tToken,
            ["accessibility"] = "regular",
            ["ignore-cache"] = "false",
            ["json"] = useJsonOutput
        }.ToString());

        var secretResp = await this.Context.SendAsync(secretReq, this.CancellationToken);
        var secretText = await secretResp.Content.ReadAsStringAsync();

        var finalResp = new HttpResponseMessage(secretResp.StatusCode);
        finalResp.Content = CreateJsonContent(secretText);
        return finalResp;
    }

    private bool TryGetCredentials(out string accessId, out string accessKey, JObject body)
    {
        accessId = null;
        accessKey = null;

        if (this.Context.Request.Headers.TryGetValues("Authorization", out var authHeaders))
        {
            var authHeader = authHeaders.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(authHeader) &&
                authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var encoded = authHeader.Substring(6).Trim();
                    var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
                    var sep = decoded.IndexOf(':');
                    if (sep > 0)
                    {
                        accessId = decoded.Substring(0, sep);
                        accessKey = decoded.Substring(sep + 1);
                        if (!string.IsNullOrEmpty(accessId) && !string.IsNullOrEmpty(accessKey))
                            return true;
                    }
                }
                catch
                {
                    // fall through to body
                }
            }
        }

        accessId = (string)body["access-id"] ?? (string)body["Access Id"] ?? (string)body["accessId"];
        accessKey = (string)body["access-key"] ?? (string)body["Access Key"] ?? (string)body["accessKey"];
        return !string.IsNullOrEmpty(accessId) && !string.IsNullOrEmpty(accessKey);
    }
}
