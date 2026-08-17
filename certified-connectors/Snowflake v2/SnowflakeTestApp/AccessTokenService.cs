using System;
using System.IdentityModel;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

public class AccessTokenService
{
    private readonly string tenantId;
    private readonly string clientId;
    private readonly string clientSecret;
    private readonly string scope;

    public AccessTokenService(string tenantId, string clientId, string clientSecret, string scope)
    {
        this.tenantId = tenantId;
        this.clientId = clientId;
        this.clientSecret = clientSecret;
        this.scope = scope;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        var authority = $"https://login.microsoftonline.com/{tenantId}";

        // The scope is the resource you want to access, suffixed with /.default
        string[] scopes = new string[] { this.scope };

        var app = ConfidentialClientApplicationBuilder.Create(clientId)
            .WithClientSecret(clientSecret)
            .WithAuthority(new Uri(authority))
            .Build();

        var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
        return result.AccessToken;
    }
}
