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

        if (string.Equals(operationId, "ListItems", StringComparison.OrdinalIgnoreCase))
            return await HandleListItemsAsync();

        if (string.Equals(operationId, "DescribeItem", StringComparison.OrdinalIgnoreCase))
            return await HandleDescribeItemAsync();

        if (string.Equals(operationId, "GetSecret", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(operationId, "GetPassword", StringComparison.OrdinalIgnoreCase))
            return await HandleGetSecretAsync(operationId);

        if (string.Equals(operationId, "ListAuthMethods", StringComparison.OrdinalIgnoreCase))
            return await HandleListAuthMethodsAsync();

        if (string.Equals(operationId, "ListRoles", StringComparison.OrdinalIgnoreCase))
            return await HandleListRolesAsync();

        if (string.Equals(operationId, "ListGateways", StringComparison.OrdinalIgnoreCase))
            return await HandleListGatewaysAsync();

        if (string.Equals(operationId, "ListTargets", StringComparison.OrdinalIgnoreCase))
            return await HandleListTargetsAsync();

        if (string.Equals(operationId, "UscList", StringComparison.OrdinalIgnoreCase))
            return await HandleUscListAsync();

        if (string.Equals(operationId, "TargetGet", StringComparison.OrdinalIgnoreCase))
            return await HandleTargetGetAsync();

        if (string.Equals(operationId, "GetTags", StringComparison.OrdinalIgnoreCase))
            return await HandleGetTagsAsync();

        if (string.Equals(operationId, "GetRole", StringComparison.OrdinalIgnoreCase))
            return await HandleGetRoleAsync();

        if (string.Equals(operationId, "GetAuthMethod", StringComparison.OrdinalIgnoreCase))
            return await HandleGetAuthMethodAsync();

        if (string.Equals(operationId, "GetAnalyticsData", StringComparison.OrdinalIgnoreCase))
            return await HandleGetAnalyticsDataAsync();

        if (string.Equals(operationId, "ListGroups", StringComparison.OrdinalIgnoreCase))
            return await HandleListGroupsAsync();

        if (string.Equals(operationId, "CreateSecret", StringComparison.OrdinalIgnoreCase))
            return await HandleCreateSecretAsync();

        if (string.Equals(operationId, "UpdateItem", StringComparison.OrdinalIgnoreCase))
            return await HandleUpdateItemAsync();

        if (string.Equals(operationId, "DeleteItem", StringComparison.OrdinalIgnoreCase))
            return await HandleDeleteItemAsync();

        if (string.Equals(operationId, "MoveObjects", StringComparison.OrdinalIgnoreCase))
            return await HandleMoveObjectsAsync();

        var bad = new HttpResponseMessage(HttpStatusCode.BadRequest);
        bad.Content = CreateJsonContent("{\"error\":\"Unknown operation: " + operationId + "\"}");
        return bad;
    }

    private async Task<JObject> ReadBodyObjectAsync()
    {
        var bodyText = await this.Context.Request.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(bodyText) ? new JObject() : JObject.Parse(bodyText);
    }

    private async Task<HttpResponseMessage> PostAkeylessAsync(string path, JObject body)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, ApiHost + path);
        req.Content = CreateJsonContent(body.ToString());
        var resp = await this.Context.SendAsync(req, this.CancellationToken);
        var text = await resp.Content.ReadAsStringAsync();
        var finalResp = new HttpResponseMessage(resp.StatusCode);
        finalResp.Content = CreateJsonContent(text);
        return finalResp;
    }

    private static string GetString(JObject body, params string[] keys)
    {
        foreach (var k in keys)
        {
            var v = body[k];
            if (v != null && v.Type != JTokenType.Null)
            {
                var s = v.Type == JTokenType.String ? (string)v : v.ToString();
                if (!string.IsNullOrEmpty(s))
                    return s;
            }
        }
        return null;
    }

    private static bool GetBool(JObject body, bool defaultValue, params string[] keys)
    {
        foreach (var k in keys)
        {
            var v = body[k];
            if (v == null || v.Type == JTokenType.Null)
                continue;
            if (v.Type == JTokenType.Boolean)
                return v.Value<bool>();
            if (v.Type == JTokenType.String)
                return string.Equals((string)v, "true", StringComparison.OrdinalIgnoreCase);
            if (v.Type == JTokenType.Integer)
                return v.Value<long>() != 0;
        }
        return defaultValue;
    }

    private static JArray GetStringArray(JObject body, params string[] keys)
    {
        foreach (var k in keys)
        {
            var v = body[k];
            if (v == null || v.Type == JTokenType.Null)
                continue;
            if (v.Type == JTokenType.Array)
                return (JArray)v;
            if (v.Type == JTokenType.String)
            {
                var s = (string)v;
                if (!string.IsNullOrWhiteSpace(s))
                    return new JArray(s);
            }
        }
        return null;
    }

    private static JArray CoerceTypeArray(JToken token)
    {
        if (token == null || token.Type == JTokenType.Null)
            return null;
        if (token.Type == JTokenType.Array)
            return (JArray)token;
        if (token.Type == JTokenType.String)
        {
            var s = (string)token;
            return string.IsNullOrWhiteSpace(s) ? null : new JArray(s);
        }
        return null;
    }

    private async Task<HttpResponseMessage> HandleListItemsAsync()
    {
        var body = await ReadBodyObjectAsync();

        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;
        var tToken = auth.Token;

        var path = GetString(body, "path", "Path") ?? "";
        var filter = GetString(body, "filter", "Filter");
        var paginationToken = GetString(body, "pagination_token", "pagination-token", "PaginationToken");
        var currentFolder = body["current_folder"] ?? body["current-folder"];
        var currentFolderBool = currentFolder != null && currentFolder.Type == JTokenType.Boolean
            ? currentFolder.Value<bool>()
            : string.Equals((string)currentFolder, "true", StringComparison.OrdinalIgnoreCase);

        var listBody = new JObject
        {
            ["token"] = tToken,
            ["path"] = path,
            ["accessibility"] = "regular",
            ["json"] = true,
            ["current-folder"] = currentFolderBool
        };

        if (!string.IsNullOrEmpty(filter))
            listBody["filter"] = filter;
        if (!string.IsNullOrEmpty(paginationToken))
            listBody["pagination-token"] = paginationToken;

        return await PostAkeylessAsync("/list-items", listBody);
    }

    private async Task<HttpResponseMessage> HandleDescribeItemAsync()
    {
        var body = await ReadBodyObjectAsync();

        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;
        var tToken = auth.Token;

        var name = GetString(body, "name", "Name", "item_name");
        if (string.IsNullOrEmpty(name))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"name (item path) is required\"}");
            return err;
        }

        var showVersions = body["show_versions"] ?? body["show-versions"];
        var showVersionsBool = showVersions != null && showVersions.Type == JTokenType.Boolean
            ? showVersions.Value<bool>()
            : string.Equals((string)showVersions, "true", StringComparison.OrdinalIgnoreCase);

        var describeBody = new JObject
        {
            ["token"] = tToken,
            ["name"] = name,
            ["accessibility"] = "regular",
            ["json"] = true,
            ["show-versions"] = showVersionsBool
        };

        return await PostAkeylessAsync("/describe-item", describeBody);
    }

    private async Task<HttpResponseMessage> HandleGetSecretAsync(string operationId)
    {
        var body = await ReadBodyObjectAsync();

        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;
        var tToken = auth.Token;

        var secretName = GetString(body, "secret_name", "Secret Name", "name");
        if (string.IsNullOrEmpty(secretName))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"secret_name is required\"}");
            return err;
        }

        var useJsonOutput = string.Equals(operationId, "GetPassword", StringComparison.OrdinalIgnoreCase);

        return await PostAkeylessAsync("/get-secret-value", new JObject
        {
            ["names"] = new JArray { secretName },
            ["token"] = tToken,
            ["accessibility"] = "regular",
            ["ignore-cache"] = "false",
            ["json"] = useJsonOutput
        });
    }

    private async Task<HttpResponseMessage> HandleListAuthMethodsAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;

        var filter = GetString(body, "filter", "Filter") ?? "";
        var paginationToken = GetString(body, "pagination_token", "pagination-token");
        var typeArr = CoerceTypeArray(body["type"] ?? body["Type"]);

        var apiBody = new JObject
        {
            ["token"] = auth.Token,
            ["filter"] = filter,
            ["json"] = true
        };
        if (!string.IsNullOrEmpty(paginationToken))
            apiBody["pagination-token"] = paginationToken;
        if (typeArr != null && typeArr.Count > 0)
            apiBody["type"] = typeArr;

        return await PostAkeylessAsync("/list-auth-methods", apiBody);
    }

    private async Task<HttpResponseMessage> HandleListRolesAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;

        var filter = GetString(body, "filter", "Filter") ?? "";
        var paginationToken = GetString(body, "pagination_token", "pagination-token");

        var apiBody = new JObject { ["token"] = auth.Token, ["filter"] = filter, ["json"] = true };
        if (!string.IsNullOrEmpty(paginationToken))
            apiBody["pagination-token"] = paginationToken;

        return await PostAkeylessAsync("/list-roles", apiBody);
    }

    private async Task<HttpResponseMessage> HandleListGatewaysAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;

        return await PostAkeylessAsync("/list-gateways", new JObject
        {
            ["token"] = auth.Token,
            ["json"] = true
        });
    }

    private async Task<HttpResponseMessage> HandleListTargetsAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;

        var filter = GetString(body, "filter", "Filter") ?? "";
        var paginationToken = GetString(body, "pagination_token", "pagination-token");
        var typeArr = CoerceTypeArray(body["type"] ?? body["Type"]);

        var apiBody = new JObject { ["token"] = auth.Token, ["filter"] = filter, ["json"] = true };
        if (!string.IsNullOrEmpty(paginationToken))
            apiBody["pagination-token"] = paginationToken;
        if (typeArr != null && typeArr.Count > 0)
            apiBody["type"] = typeArr;

        return await PostAkeylessAsync("/list-targets", apiBody);
    }

    private async Task<HttpResponseMessage> HandleUscListAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;

        var uscName = GetString(body, "usc_name", "usc-name", "UscName");
        if (string.IsNullOrEmpty(uscName))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"usc_name (Universal Secrets Connector name) is required\"}");
            return err;
        }

        return await PostAkeylessAsync("/usc-list", new JObject
        {
            ["token"] = auth.Token,
            ["usc-name"] = uscName,
            ["json"] = true
        });
    }

    private async Task<HttpResponseMessage> HandleTargetGetAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;

        var name = GetString(body, "name", "Name");
        if (string.IsNullOrEmpty(name))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"name (target name) is required\"}");
            return err;
        }

        var showVersions = GetBool(body, false, "show_versions", "show-versions", "showVersions");

        return await PostAkeylessAsync("/target-get", new JObject
        {
            ["token"] = auth.Token,
            ["name"] = name,
            ["json"] = true,
            ["show-versions"] = showVersions
        });
    }

    private async Task<HttpResponseMessage> HandleGetTagsAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;

        var name = GetString(body, "name", "Name");
        if (string.IsNullOrEmpty(name))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"name (item path) is required\"}");
            return err;
        }

        return await PostAkeylessAsync("/get-tags", new JObject
        {
            ["token"] = auth.Token,
            ["name"] = name,
            ["json"] = true
        });
    }

    private async Task<HttpResponseMessage> HandleGetRoleAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;

        var name = GetString(body, "name", "Name");
        if (string.IsNullOrEmpty(name))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"name (role name) is required\"}");
            return err;
        }

        return await PostAkeylessAsync("/get-role", new JObject
        {
            ["token"] = auth.Token,
            ["name"] = name,
            ["json"] = true
        });
    }

    private async Task<HttpResponseMessage> HandleGetAuthMethodAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;

        var name = GetString(body, "name", "Name");
        if (string.IsNullOrEmpty(name))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"name (auth method name) is required\"}");
            return err;
        }

        return await PostAkeylessAsync("/get-auth-method", new JObject
        {
            ["token"] = auth.Token,
            ["name"] = name,
            ["json"] = true
        });
    }

    private async Task<HttpResponseMessage> HandleGetAnalyticsDataAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;

        return await PostAkeylessAsync("/get-analytics-data", new JObject
        {
            ["token"] = auth.Token,
            ["json"] = true
        });
    }

    private async Task<HttpResponseMessage> HandleListGroupsAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;

        var filter = GetString(body, "filter", "Filter") ?? "";
        var paginationToken = GetString(body, "pagination_token", "pagination-token");

        var apiBody = new JObject { ["token"] = auth.Token, ["filter"] = filter, ["json"] = true };
        if (!string.IsNullOrEmpty(paginationToken))
            apiBody["pagination-token"] = paginationToken;

        return await PostAkeylessAsync("/list-groups", apiBody);
    }

    private async Task<HttpResponseMessage> HandleCreateSecretAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;

        var name = GetString(body, "name", "Name");
        var value = GetString(body, "value", "Value");
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"name and value are required\"}");
            return err;
        }

        var description = GetString(body, "description", "Description");
        var protectionKey = GetString(body, "protection_key", "protectionKey");
        var secretType = GetString(body, "secret_type", "secretType", "type") ?? "generic";
        var format = GetString(body, "format", "Format") ?? "text";
        var maxVersions = GetString(body, "max_versions", "maxVersions");
        var multiline = GetBool(body, false, "multiline", "Multiline");
        var deleteProtection = GetBool(body, false, "delete_protection", "deleteProtection");

        var tags = GetStringArray(body, "tags", "Tags");

        var apiBody = new JObject
        {
            ["token"] = auth.Token,
            ["name"] = name,
            ["value"] = value,
            ["accessibility"] = "regular",
            ["json"] = true,
            ["type"] = secretType,
            ["format"] = format
        };

        if (!string.IsNullOrEmpty(description))
            apiBody["description"] = description;
        if (tags != null && tags.Count > 0)
            apiBody["tags"] = tags;
        if (!string.IsNullOrEmpty(protectionKey))
            apiBody["protection_key"] = protectionKey;
        if (multiline)
            apiBody["multiline_value"] = true;
        if (!string.IsNullOrEmpty(maxVersions))
            apiBody["max-versions"] = maxVersions;
        if (deleteProtection)
            apiBody["delete_protection"] = "true";

        return await PostAkeylessAsync("/create-secret", apiBody);
    }

    private async Task<HttpResponseMessage> HandleUpdateItemAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;
        var token = auth.Token;

        var name = GetString(body, "name", "Name");
        if (string.IsNullOrEmpty(name))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"name (item path) is required\"}");
            return err;
        }

        var newValue = GetString(body, "new_value", "newValue", "value");
        var keepPrevVersion = GetBool(body, false, "keep_prev_version", "keepPrevVersion");
        var description = GetString(body, "description", "Description");
        var newName = GetString(body, "new_name", "newName");
        var addTags = GetStringArray(body, "add_tags", "addTags");
        var removeTags = GetStringArray(body, "remove_tags", "removeTags");
        var maxVersions = GetString(body, "max_versions", "maxVersions");
        var deleteProtection = GetBool(body, false, "delete_protection", "deleteProtection");

        var hasMeta = !string.IsNullOrEmpty(description) || !string.IsNullOrEmpty(newName) ||
                      (addTags != null && addTags.Count > 0) || (removeTags != null && removeTags.Count > 0) ||
                      !string.IsNullOrEmpty(maxVersions) || deleteProtection;

        if (!string.IsNullOrEmpty(newValue) && !hasMeta)
        {
            var vBody = new JObject { ["token"] = token, ["name"] = name, ["value"] = newValue, ["json"] = true };
            if (keepPrevVersion)
                vBody["keep-prev-version"] = "true";
            return await PostAkeylessAsync("/update-secret-val", vBody);
        }

        if (string.IsNullOrEmpty(newValue) && string.IsNullOrEmpty(description) && string.IsNullOrEmpty(newName) &&
            (addTags == null || addTags.Count == 0) && (removeTags == null || removeTags.Count == 0) &&
            string.IsNullOrEmpty(maxVersions) && !deleteProtection)
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"At least one update field is required (new_value, description, new_name, add_tags, remove_tags, max_versions, or delete_protection).\"}");
            return err;
        }

        var updateBody = new JObject { ["token"] = token, ["name"] = name, ["json"] = true };
        if (!string.IsNullOrEmpty(description))
            updateBody["description"] = description;
        if (!string.IsNullOrEmpty(newName))
            updateBody["new_name"] = newName;
        if (addTags != null && addTags.Count > 0)
            updateBody["add_tag"] = addTags;
        if (removeTags != null && removeTags.Count > 0)
            updateBody["rm_tag"] = removeTags;
        if (!string.IsNullOrEmpty(maxVersions))
            updateBody["max-versions"] = maxVersions;
        if (deleteProtection)
            updateBody["delete_protection"] = "true";

        var metaResp = await PostAkeylessAsync("/update-item", updateBody);
        if (!metaResp.IsSuccessStatusCode)
            return metaResp;

        if (string.IsNullOrEmpty(newValue))
            return metaResp;

        var valueName = string.IsNullOrEmpty(newName) ? name : newName;
        var secretBody = new JObject { ["token"] = token, ["name"] = valueName, ["value"] = newValue, ["json"] = true };
        if (keepPrevVersion)
            secretBody["keep-prev-version"] = "true";

        var secretResp = await PostAkeylessAsync("/update-secret-val", secretBody);
        if (!secretResp.IsSuccessStatusCode)
            return secretResp;

        var metaText = await metaResp.Content.ReadAsStringAsync();
        var secretText = await secretResp.Content.ReadAsStringAsync();
        JToken metaTok = null;
        JToken secretTok = null;
        try { metaTok = JToken.Parse(metaText); } catch { metaTok = new JValue(metaText); }
        try { secretTok = JToken.Parse(secretText); } catch { secretTok = new JValue(secretText); }

        var merged = new JObject
        {
            ["update_item"] = metaTok,
            ["update_secret_val"] = secretTok
        };
        var ok = new HttpResponseMessage(HttpStatusCode.OK);
        ok.Content = CreateJsonContent(merged.ToString());
        return ok;
    }

    private async Task<HttpResponseMessage> HandleDeleteItemAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;

        var name = GetString(body, "name", "Name");
        if (string.IsNullOrEmpty(name))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"name (item path) is required\"}");
            return err;
        }

        var deleteImmediately = GetBool(body, false, "delete_immediately", "deleteImmediately");
        var deleteInDays = body["delete_in_days"] ?? body["delete-in-days"] ?? body["deleteInDays"];
        int? days = null;
        if (deleteInDays != null && deleteInDays.Type != JTokenType.Null)
        {
            if (deleteInDays.Type == JTokenType.Integer || deleteInDays.Type == JTokenType.Float)
                days = deleteInDays.Value<int>();
            else if (deleteInDays.Type == JTokenType.String && int.TryParse((string)deleteInDays, out var d))
                days = d;
        }

        var versionTok = body["version"] ?? body["Version"];
        int? version = null;
        if (versionTok != null && versionTok.Type != JTokenType.Null)
        {
            if (versionTok.Type == JTokenType.Integer || versionTok.Type == JTokenType.Float)
                version = versionTok.Value<int>();
            else if (versionTok.Type == JTokenType.String && int.TryParse((string)versionTok, out var vi))
                version = vi;
        }

        var apiBody = new JObject { ["token"] = auth.Token, ["name"] = name, ["json"] = true };
        if (deleteImmediately)
            apiBody["delete-immediately"] = true;
        if (days.HasValue && days.Value > 0)
            apiBody["delete-in-days"] = days.Value;
        if (version.HasValue)
            apiBody["version"] = version.Value;

        return await PostAkeylessAsync("/delete-item", apiBody);
    }

    private async Task<HttpResponseMessage> HandleMoveObjectsAsync()
    {
        var body = await ReadBodyObjectAsync();
        var auth = await AuthenticateAsync(body);
        if (auth.Error != null)
            return auth.Error;

        var source = GetString(body, "source_path", "sourcePath", "source");
        var target = GetString(body, "target_path", "targetPath", "target");
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"source_path and target_path are required\"}");
            return err;
        }

        if (string.Equals(source, target, StringComparison.Ordinal))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"source_path and target_path cannot be the same\"}");
            return err;
        }

        var objectsType = GetString(body, "objects_type", "objectsType", "objects-type") ?? "item";

        return await PostAkeylessAsync("/move-objects", new JObject
        {
            ["token"] = auth.Token,
            ["source"] = source,
            ["target"] = target,
            ["objects-type"] = objectsType,
            ["json"] = true
        });
    }

    private async Task<AuthOutcome> AuthenticateAsync(JObject body)
    {
        if (!TryGetCredentials(out var accessId, out var accessKey, body))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent(
                "{\"error\":\"Set Access Id and Access Key on the connection, or pass access-id and access-key in the action body.\"}");
            return new AuthOutcome { Error = err };
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
            return new AuthOutcome { Error = err };
        }

        var tToken = (string)JObject.Parse(authText)["token"];
        if (string.IsNullOrEmpty(tToken))
        {
            var err = new HttpResponseMessage(HttpStatusCode.BadRequest);
            err.Content = CreateJsonContent("{\"error\":\"Auth response did not include a token\"}");
            return new AuthOutcome { Error = err };
        }

        return new AuthOutcome { Token = tToken };
    }

    private sealed class AuthOutcome
    {
        public HttpResponseMessage Error { get; set; }
        public string Token { get; set; }
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
