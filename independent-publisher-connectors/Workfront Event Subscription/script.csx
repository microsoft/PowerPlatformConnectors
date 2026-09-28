public class Script : ScriptBase
{
    private readonly string HTTP_HEADER_NAME_API_KEY = "X-WORKFRONT-API-KEY";
    private readonly string HTTP_HEADER_NAME_AUTHORIZATION = "Authorization";
    private readonly string HTTP_HEADER_NAME_SESSION_ID = "sessionID";
    private readonly string HTTP_HEADER_NAME_JWT_CLIENT_ID = "X-WORKFRONT-JWT-CLIENT-ID";
    private readonly string HTTP_HEADER_NAME_JWT_CLIENT_SECRET = "X-WORKFRONT-JWT-CLIENT-SECRET";
    private readonly string HTTP_HEADER_NAME_JWT_CUSTOMER_ID = "X-WORKFRONT-JWT-CUSTOMER-ID";
    private readonly string HTTP_HEADER_NAME_JWT_SUBJECT_USER = "X-WORKFRONT-JWT-SUBJECT-USER";
    private readonly string HTTP_HEADER_NAME_JWT_RSA_PRIVATE_KEY_INFO = "X-WORKFRONT-JWT-RSA-PRIVATE-KEY-INFO";

    private string BuildSessionApiAbsoluteUrl(string workfrontDomain, string apiKey)
        => $"https://{workfrontDomain}/attask/api/v19.0/session?apiKey={apiKey}";

    private string BuildJwtExchangeAbsoluteUrl (string workfrontDomain)
        => $"https://{workfrontDomain}/integrations/oauth2/api/v1/jwt/exchange";
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var request = this.Context.Request;

        // API key authentication
        if (CheckIsHttpHeaderExistsAndHasValue(HTTP_HEADER_NAME_API_KEY))
        {
            var sessionID = await GenerateSessionID(
                    request.Headers.GetValues(HTTP_HEADER_NAME_API_KEY).FirstOrDefault(),
                    request.RequestUri.Host)
                .ConfigureAwait(false);

            SetSessionIdHttpHeader(sessionID);
            RemoveHttpHeaderByName(HTTP_HEADER_NAME_API_KEY);
        }

        // JWT token authentication
        if (new[]
            {
                HTTP_HEADER_NAME_JWT_CLIENT_ID,
                HTTP_HEADER_NAME_JWT_CLIENT_SECRET,
                HTTP_HEADER_NAME_JWT_CUSTOMER_ID,
                HTTP_HEADER_NAME_JWT_SUBJECT_USER,
                HTTP_HEADER_NAME_JWT_RSA_PRIVATE_KEY_INFO,
            }
            .All(CheckIsHttpHeaderExistsAndHasValue))
        {
            var accessToken = await ExchangeJwtForAccessToken(
                    workfrontHost: request.RequestUri.Host,
                    customerId: request.Headers.GetValues(HTTP_HEADER_NAME_JWT_CUSTOMER_ID).First(),
                    clientId: request.Headers.GetValues(HTTP_HEADER_NAME_JWT_CLIENT_ID).First(),
                    clientSecret: request.Headers.GetValues(HTTP_HEADER_NAME_JWT_CLIENT_SECRET).First(),
                    rsaParamsB64: request.Headers.GetValues(HTTP_HEADER_NAME_JWT_RSA_PRIVATE_KEY_INFO).First(),
                    subjectUserId: request.Headers.GetValues(HTTP_HEADER_NAME_JWT_SUBJECT_USER).First())
                .ConfigureAwait(false);

            SetSessionIdHttpHeader(accessToken);
        }

        RemoveHttpHeaderByName(HTTP_HEADER_NAME_JWT_CLIENT_ID);
        RemoveHttpHeaderByName(HTTP_HEADER_NAME_JWT_CLIENT_SECRET);
        RemoveHttpHeaderByName(HTTP_HEADER_NAME_JWT_CUSTOMER_ID);
        RemoveHttpHeaderByName(HTTP_HEADER_NAME_JWT_SUBJECT_USER);
        RemoveHttpHeaderByName(HTTP_HEADER_NAME_JWT_RSA_PRIVATE_KEY_INFO);

        var response = await this.Context
            .SendAsync(request, this.CancellationToken)
            .ConfigureAwait(false);

        return response;
    }

    private void SetSessionIdHttpHeader(string headerValue)
    {
        var request = this.Context.Request;
        if (request.Headers.Contains(HTTP_HEADER_NAME_SESSION_ID))
        {
            request.Headers.Remove(HTTP_HEADER_NAME_SESSION_ID);
        }
        request.Headers.Add(HTTP_HEADER_NAME_SESSION_ID, headerValue);
    }

    private async Task<string> GenerateSessionID(string apiKey, string workfrontDomain)
    {
        try
        {
            var sessionRequest = new HttpRequestMessage(
                HttpMethod.Get,
                BuildSessionApiAbsoluteUrl(workfrontDomain, apiKey));

            var sessionResponse = await this.Context
                .SendAsync(sessionRequest, this.CancellationToken)
                .ConfigureAwait(false);

            if (sessionResponse.IsSuccessStatusCode)
            {
                var responseBody = await sessionResponse.Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);

                var parsedResponseBody = JObject.Parse(responseBody);

                return (string?)parsedResponseBody.SelectToken("data.sessionID");
            }
        }
        catch (Exception exc)
        {
            this.Context.Logger.LogError(exc, "Error during SessionID generation");
        }

        return null;
    }

    private bool CheckIsHttpHeaderExistsAndHasValue(string httpHeaderName)
    {
        var requestHeaders = this.Context.Request.Headers;
        return requestHeaders.Contains(httpHeaderName)
            && !string.IsNullOrEmpty(requestHeaders.GetValues(httpHeaderName)?.FirstOrDefault()?.Trim());
    }

    private void RemoveHttpHeaderByName(string httpHeaderName)
    {
        var request = this.Context.Request;
        if (request.Headers.Contains(httpHeaderName))
        {
            request.Headers.Remove(httpHeaderName);
        }
    }

    private async Task<string> ExchangeJwtForAccessToken(
        string workfrontHost,
        string customerId,
        string clientId,
        string clientSecret,
        string rsaParamsB64,
        string subjectUserId)
    {
        var jwtToken = CreateJwtAssertion(customerId, subjectUserId, rsaParamsB64);

        var req = new HttpRequestMessage(
            HttpMethod.Post,
            BuildJwtExchangeAbsoluteUrl(workfrontHost));

        req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["jwt_token"] = jwtToken
        });

        var res = await this.Context.SendAsync(req, this.CancellationToken).ConfigureAwait(false);
        var body = await res.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (!res.IsSuccessStatusCode)
        {
            AddErrorToLogAndThrowException($"JWT exchange failed ({(int)res.StatusCode}): {body}");
        }

        var json = JObject.Parse(body);
        var accessToken = (string?)json["access_token"];
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            AddErrorToLogAndThrowException("JWT exchange response missing access_token.");
        }

        return accessToken;
    }

    private string CreateJwtAssertion(string clientId, string subjectUserId, string rsaParamsB64)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var exp = now + 60; // 1 min

        var headerJson = JsonConvert.SerializeObject(new
        {
            alg = "RS256",
            typ = "JWT"
        });

        var payloadJson = JsonConvert.SerializeObject(new
        {
            iss = clientId,
            sub = subjectUserId,
            iat = now,
            exp = exp
        });

        var headerB64 = Base64UrlEncode(Encoding.UTF8.GetBytes(headerJson));
        var payloadB64 = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));
        var signingInput = headerB64 + "." + payloadB64;

        byte[] signatureBytes;
        var rsaParams = ParseRsaParamsFromB64Json(rsaParamsB64);
        using (var rsa = new RSACryptoServiceProvider())
        using (var sha = new SHA256CryptoServiceProvider())
        {
            rsa.PersistKeyInCsp = false;
            rsa.ImportParameters(rsaParams);
            var hash = sha.ComputeHash(Encoding.ASCII.GetBytes(signingInput));
            signatureBytes = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA256"));
        }

        return signingInput + "." + Base64UrlEncode(signatureBytes);
    }

    private RSAParameters ParseRsaParamsFromB64Json(string rsaParamsB64)
    {
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(rsaParamsB64.Trim()));
        var parsedJson = JObject.Parse(json);

        return new RSAParameters
        {
            Modulus = Convert.FromBase64String((string)parsedJson["modulus"]),
            Exponent = Convert.FromBase64String((string)parsedJson["exponent"]),
            D = Convert.FromBase64String((string)parsedJson["d"]),
            P = Convert.FromBase64String((string)parsedJson["p"]),
            Q = Convert.FromBase64String((string)parsedJson["q"]),
            DP = Convert.FromBase64String((string)parsedJson["dp"]),
            DQ = Convert.FromBase64String((string)parsedJson["dq"]),
            InverseQ = Convert.FromBase64String((string)parsedJson["inverseQ"])
        };
    }

    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private void AddErrorToLogAndThrowException(string errorMessage)
    {
        this.Context.Logger.LogError(errorMessage);
        throw new Exception(errorMessage);
    }
}