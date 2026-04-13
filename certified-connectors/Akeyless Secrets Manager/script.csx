using System;
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
            return await HandleGetSecretAsync();

        var bad = new HttpResponseMessage(HttpStatusCode.BadRequest);
        bad.Content = CreateJsonContent("{\"error\":\"Unknown operation: " + operationId + "\"}");
        return bad;
    }

    private async Task<HttpResponseMessage> HandleGetSecretAsync()
    {
        var bodyText = await this.Context.Request.Content.ReadAsStringAsync();
        var body = string.IsNullOrWhiteSpace(bodyText) ? new JObject() : JObject.Parse(bodyText);

        var accessId = (string)body["access-id"] ?? (string)body["Access Id"] ?? (string)body["accessId"];
        var accessKey = (string)body["access-key"] ?? (string)body["Access Key"] ?? (string)body["accessKey"];
        var secretName = (string)body["secret_name"] ?? (string)body["Secret Name"] ?? (string)body["name"];

        if (string.IsNullOrEmpty(accessId) || string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretName))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent(
                "{\"error\":\"access-id, access-key, and secret_name are required on each action (per exported connector).\"}");
            return err;
        }

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
            ["json"] = false
        }.ToString());

        var secretResp = await this.Context.SendAsync(secretReq, this.CancellationToken);
        var secretText = await secretResp.Content.ReadAsStringAsync();

        var finalResp = new HttpResponseMessage(secretResp.StatusCode);
        finalResp.Content = CreateJsonContent(secretText);
        return finalResp;
    }
}
