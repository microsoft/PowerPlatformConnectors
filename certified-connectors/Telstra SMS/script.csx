using System;
using System.Text;

public class Script : ScriptBase
{
    private string _accessToken = string.Empty;
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Request AccessToken        
        var authResp = await GetAuthToken();
        var authRespObject = JObject.Parse(await authResp.Content.ReadAsStringAsync().ConfigureAwait(false));
        _accessToken = authRespObject["access_token"].ToString();

        if (authResp.IsSuccessStatusCode)
        {
            var response = await SubmitRequest();
            return response;
            
        }
        else
        {
            return authResp;
        }
    }

     private async Task<HttpResponseMessage> SubmitRequest()
    {
        this.Context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        this.Context.Logger.LogInformation(response.StatusCode.ToString());
        this.Context.Logger.LogError(response.StatusCode.ToString());

        return response;       
    }

    private async Task<HttpResponseMessage> GetAuthToken()
    {
        var clientCreds = GetUserCreds();

        HttpRequestMessage msgAPIRequest = new HttpRequestMessage(HttpMethod.Post, "https://products.api.telstra.com/v2/oauth/token");
        
        var formData = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", clientCreds.ClientId },
            { "client_secret", clientCreds.ClientSecret },
            { "scope", "free-trial-numbers:read free-trial-numbers:write virtual-numbers:read virtual-numbers:write messages:read messaging:write reports:read reports:write" }
        };
        msgAPIRequest.Content = new FormUrlEncodedContent(formData);
        msgAPIRequest.Headers.Add("Accept", "*/*");

        HttpResponseMessage response = await this.Context.SendAsync(msgAPIRequest, this.CancellationToken).ConfigureAwait(false);
        this.Context.Logger.LogInformation(response.StatusCode.ToString());
        this.Context.Logger.LogError(response.StatusCode.ToString());
        
        return response;
    }

    private MsgUserCredentials GetUserCreds()
    {
        string authHeader = this.Context.Request.Headers.Authorization.ToString();
        string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
        string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
        this.Context.Logger.LogError(usernamePassword);
        
        int seperatorIndex = usernamePassword.IndexOf(':');

        string username = usernamePassword.Substring(0, seperatorIndex);
        string password = usernamePassword.Substring(seperatorIndex + 1);

        var creds = new MsgUserCredentials(username, password);

        return creds;
    }
}

public class MsgUserCredentials
{
    public string ClientId {get; set;}
    public string ClientSecret {get; set;}

    public MsgUserCredentials(string clientId, string clientSecret)    
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
    }    
}
