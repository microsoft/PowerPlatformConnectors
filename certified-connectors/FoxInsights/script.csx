public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var request = this.Context.Request;

        // The connection stores Client ID / Client secret as Basic credentials; the API itself only accepts Bearer tokens.
        var credentials = ReadBasicCredentials(request.Headers.Authorization);
        if (credentials == null)
        {
            return Error(HttpStatusCode.Unauthorized, "The connection has no Client ID / Client secret. Recreate the connection.");
        }

        Guid parsed;
        if (!Guid.TryParse(credentials.Item1, out parsed) || !Guid.TryParse(credentials.Item2, out parsed))
        {
            return Error(HttpStatusCode.Unauthorized, "Client ID and Client secret must be UUIDs as shown in FoxInsights under Settings > Partner API.");
        }

        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(request.RequestUri, "/oauth2/token"))
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", credentials.Item1 },
                { "client_secret", credentials.Item2 },
            }),
        };

        var tokenResponse = await this.Context.SendAsync(tokenRequest, this.CancellationToken).ConfigureAwait(false);
        if (!tokenResponse.IsSuccessStatusCode)
        {
            return TokenError(tokenResponse.StatusCode);
        }

        var token = JObject.Parse(await tokenResponse.Content.ReadAsStringAsync().ConfigureAwait(false))["access_token"]?.ToString();
        if (string.IsNullOrEmpty(token))
        {
            return Error(HttpStatusCode.BadGateway, "FoxInsights returned no access token.");
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var operationId = this.Context.OperationId;
        var transformsResponse = operationId == "ListPartners" || DeviceOperations.Contains(operationId);
        if (operationId == "ListPartners")
        {
            // Connector-only operation: same backend call as the partner tree, flattened below.
            request.RequestUri = new Uri(request.RequestUri, "/external/v5/partner");
        }
        else if (operationId == "SearchUsersByCustomField")
        {
            // The API takes the custom field name as the query key, which a connector parameter cannot express.
            var searchUri = CustomFieldSearchUri(request.RequestUri);
            if (searchUri == null)
            {
                return Error(HttpStatusCode.BadRequest, "Custom field name and value are required.");
            }

            request.RequestUri = searchUri;
        }

        if (transformsResponse)
        {
            // The body is parsed here, so it must not come back compressed.
            request.Headers.AcceptEncoding.Clear();
        }

        var response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
        if (!transformsResponse || !response.IsSuccessStatusCode)
        {
            return response;
        }

        var result = JToken.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        if (operationId == "ListPartners")
        {
            result = FlattenPartners((JObject)result);
        }
        else
        {
            var devices = result as JArray;
            if (devices != null)
            {
                foreach (var device in devices)
                {
                    AddMeasuredAt(device);
                }
            }
            else
            {
                AddMeasuredAt(result);
            }
        }

        response.Content = CreateJsonContent(result.ToString(Newtonsoft.Json.Formatting.None));
        return response;
    }

    private static readonly HashSet<string> DeviceOperations = new HashSet<string>
    {
        "GetDevice",
        "GetPartnerDevices",
        "GetUserDevices",
        "SearchDevicesByInternalId",
        "SearchDevicesByCompany",
        "UpdateDeviceMetadata",
        "UpdateDeviceAddress",
        "UpdateDeviceLabels",
        "UpdateDeviceNotification",
        "UpdateDeviceLock",
        "MoveDeviceToPartner",
        "UpdateDeviceProduct",
        "SetDeviceTankCustom",
        "SetDeviceTankPredefined",
        "SetDeviceSilo",
        "DeleteDeviceConfig",
        "AssignDeviceToUser",
        "UnassignDeviceFromUser",
    };

    private static Uri CustomFieldSearchUri(Uri requestUri)
    {
        string name = null;
        string value = null;
        foreach (var pair in requestUri.Query.TrimStart('?').Split('&'))
        {
            var parts = pair.Split(new[] { '=' }, 2);
            if (parts.Length != 2)
            {
                continue;
            }

            var key = Uri.UnescapeDataString(parts[0].Replace('+', ' '));
            if (key == "fieldName")
            {
                name = Uri.UnescapeDataString(parts[1].Replace('+', ' '));
            }
            else if (key == "fieldValue")
            {
                value = Uri.UnescapeDataString(parts[1].Replace('+', ' '));
            }
        }

        if (string.IsNullOrEmpty(name) || value == null)
        {
            return null;
        }

        return new Uri(requestUri, "/external/v5/user/search?" + Uri.EscapeDataString(name) + "=" + Uri.EscapeDataString(value));
    }

    private static JArray FlattenPartners(JObject root)
    {
        var partners = new JArray();
        AddPartner(partners, root, null, 0);
        return partners;
    }

    private static void AddPartner(JArray partners, JObject partner, string parentId, int depth)
    {
        partners.Add(new JObject
        {
            ["id"] = partner["id"],
            ["name"] = partner["name"],
            ["parentId"] = parentId == null ? JValue.CreateNull() : new JValue(parentId),
            ["depth"] = depth,
            ["products"] = partner["products"] ?? new JArray(),
        });

        var children = partner["children"] as JArray;
        if (children == null)
        {
            return;
        }

        foreach (var child in children.OfType<JObject>())
        {
            AddPartner(partners, child, partner["id"]?.ToString(), depth + 1);
        }
    }

    // Unix seconds are awkward to map in flows, so expose the same instant as ISO 8601 UTC.
    private static void AddMeasuredAt(JToken device)
    {
        var metering = device["lastMetering"] as JObject;
        var timestamp = metering?["timestamp"];
        if (timestamp == null || timestamp.Type != JTokenType.Integer)
        {
            return;
        }

        metering["measuredAt"] = DateTimeOffset.FromUnixTimeSeconds(timestamp.Value<long>()).UtcDateTime.ToString("o");
    }

    private static Tuple<string, string> ReadBasicCredentials(AuthenticationHeaderValue header)
    {
        if (header == null || !"Basic".Equals(header.Scheme, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(header.Parameter))
        {
            return null;
        }

        string decoded;
        try
        {
            decoded = Encoding.UTF8.GetString(Convert.FromBase64String(header.Parameter));
        }
        catch (FormatException)
        {
            return null;
        }

        var separator = decoded.IndexOf(':');
        if (separator <= 0)
        {
            return null;
        }

        return Tuple.Create(decoded.Substring(0, separator).Trim(), decoded.Substring(separator + 1).Trim());
    }

    private HttpResponseMessage TokenError(HttpStatusCode status)
    {
        switch ((int)status)
        {
            case 400:
            case 401:
            case 403:
                return Error(HttpStatusCode.Unauthorized, "FoxInsights rejected the Client ID / Client secret.");
            case 402:
                return Error(HttpStatusCode.Forbidden, "Partner API is not enabled for this FoxInsights partner.");
            case 429:
                return Error((HttpStatusCode)429, "FoxInsights rate limit reached while requesting a token. Retry later.");
            default:
                return Error(HttpStatusCode.BadGateway, "FoxInsights token request failed with HTTP " + (int)status + ".");
        }
    }

    private HttpResponseMessage Error(HttpStatusCode status, string message)
    {
        return new HttpResponseMessage(status)
        {
            Content = CreateJsonContent(new JObject { ["message"] = message }.ToString()),
        };
    }
}
