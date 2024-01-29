using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Net;
using System.Net.Http;


public class Script : ScriptBase
{
    private const string API_ENDPOINT = "https://api.lucidtech.ai/v1";
    private const string TOKEN_ENDPOINT = "https://auth.lucidtech.ai/oauth2/token";

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        string clientCredentials = this.Context.Request.Headers.GetValues("ClientCredentials").First();
       
        var accessToken = await this.GetAccessToken(clientCredentials);
        var path = this.Context.Request.RequestUri.AbsolutePath.ToString();
        
        switch (path) {
            case "/v1/workflows":
                return await CreateRun(accessToken);
                break;
            case "/v1/documents": 
                return await CreateDocument(accessToken);
                break;
            case "/v1/models":
                return await GetModels(accessToken);
                break;
        }

        return null;
    }
    
    private async Task<HttpResponseMessage> CreateRun(string accessToken)
    {
        var request = this.Context.Request;
        string workflowId = request.Headers.GetValues("WorkflowId").First();
        request.RequestUri = new Uri($"{Script.API_ENDPOINT}/workflows/{workflowId}/executions");

        return await this.Context.SendAsync(request, this.CancellationToken);       
    }
    
    private async Task<HttpResponseMessage> GetModels(string accessToken)
    {
        var myModelsRes = this.Context.SendAsync(
            CreateAuthorizedRequest(
                method: HttpMethod.Get,
                path: $"/models",
                accessToken: accessToken
            ),
            this.CancellationToken
        );

        var publicModelsRes = this.Context.SendAsync(
            CreateAuthorizedRequest(
                method: HttpMethod.Get,
                path: $"/models?owner=las:organization:cradl",
                accessToken: accessToken
            ),
            this.CancellationToken
        );

        var response = await myModelsRes;
        var content = await ToJson(response);
        
        var myModels = (JArray) content["models"];
        foreach (var pretrainedModel in (await ToJson(await publicModelsRes))["models"]) {
            pretrainedModel["modelId"] = "las:organization:cradl/" + pretrainedModel["modelId"];
            myModels.Add(pretrainedModel);
        }

        response.Content = CreateJsonContent(content.ToString());
        return response;
    }

    private async Task<string> GetAccessToken(string authString)
    {
        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Script.TOKEN_ENDPOINT}?grant_type=client_credentials"));
        tokenRequest.Headers.Add("Authorization", $"Basic {authString}");
        tokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>() {});
        
        var tokenResponse = await this.Context.SendAsync(tokenRequest, this.CancellationToken);
        return (string) (await ToJson(tokenResponse))["access_token"];
    }
    
    private async Task<HttpResponseMessage> CreateDocument(string accessToken)
    {
        string fileName = this.Context.Request.Headers.GetValues("Name").First();
        var fileContent = await this.Context.Request.Content.ReadAsByteArrayAsync();

        var request = CreateAuthorizedRequest(
            method: HttpMethod.Post,
            path: "/documents",
            accessToken: accessToken
        );

        request.Content = CreateJsonContent(new JObject { ["name"] = fileName }.ToString());
        var response = await this.Context.SendAsync(request, this.CancellationToken);
        
        var fileUrl = (string) (await ToJson(response))["fileUrl"];
        
        var putRequest = new HttpRequestMessage(HttpMethod.Put, new Uri(fileUrl));
        putRequest.Headers.Add("Authorization", $"Bearer {accessToken}");
        putRequest.Content = new ByteArrayContent(fileContent);
        var putResponse = await this.Context.SendAsync(putRequest, this.CancellationToken);

        return response;
    }
    
    private static HttpRequestMessage CreateAuthorizedRequest(HttpMethod method, string path, string accessToken)
    {
        var request = new HttpRequestMessage(method, new Uri($"{Script.API_ENDPOINT}{path}"));
        request.Headers.Add("Authorization", $"Bearer {accessToken}");
        return request;
    }
    
    private static async Task<JObject> ToJson(HttpResponseMessage response)
    {
        return JObject.Parse(await response.Content.ReadAsStringAsync());
    }
}