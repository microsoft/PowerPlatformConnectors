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
        string[] clientCredentials = this.Context.Request.Headers.GetValues("ClientCredentials").First().Split(':');
        string clientId = clientCredentials[0];
        string clientSecret = clientCredentials[1];
        
        var accessToken = await this.GetAccessToken(clientId: clientId, clientSecret: clientSecret);
        var path = this.Context.Request.RequestUri.AbsolutePath.ToString();
        
        switch (path) {
            case "/v1/runs":
                return await CreateRun(accessToken);
                break;
            case "/v1/predictions": 
                return await CreatePrediction(accessToken);
                break;
            case "/v1/models":
                return await GetModels(accessToken);
                break;
            case "/v1/workflows":
                return await GetWorkflows(accessToken);
                break;
        }

        return null;
    }
    
    private async Task<HttpResponseMessage> CreateRun(string accessToken)
    {
        string workflowId = this.Context.Request.Headers.GetValues("WorkflowId").First();
        string executionName = this.Context.Request.Headers.GetValues("Name").First();
        
        var fileContent = await this.Context.Request.Content.ReadAsByteArrayAsync();

        var (documentResponse, putResponse) = await this.CreateDocument(
            content: fileContent,
            name: executionName,
            accessToken: accessToken
        );

        var documentId = (string) (await ToJson(documentResponse))["documentId"];

        var request = CreateAuthorizedRequest(
            method: HttpMethod.Post,
            path: $"/workflows/{workflowId}/executions",
            accessToken: accessToken
        );
        
        request.Content = CreateJsonContent(new JObject {
            ["input"] = new JObject {
                ["documentId"] = documentId,
                ["title"] = executionName
            }
        }.ToString());
        
        return await this.Context.SendAsync(request, this.CancellationToken);       
    }

    private async Task<HttpResponseMessage> CreatePrediction(string accessToken)
    {
        string modelId = this.Context.Request.Headers.GetValues("ModelId").First();
        string fileName = this.Context.Request.Headers.GetValues("Name").First();
        
        var fileContent = await this.Context.Request.Content.ReadAsByteArrayAsync();

        var (documentResponse, putResponse) = await this.CreateDocument(
            content: fileContent,
            name: fileName,
            accessToken: accessToken
        );
        
        var documentId = (string) (await ToJson(documentResponse))["documentId"];
        
        var request = CreateAuthorizedRequest(
            method: HttpMethod.Post,
            path: $"/predictions",
            accessToken: accessToken
        );

        request.Content = CreateJsonContent(new JObject {
            ["modelId"] = modelId,
            ["documentId"] = documentId
        }.ToString());
        
        var response = await this.Context.SendAsync(request, this.CancellationToken);

        var predictions = new Dictionary<string, JToken>();
        foreach (var pred in (await ToJson(response))["predictions"]) {
            var label = (string) pred["label"];
            try {
                if ((float) predictions[label]["confidence"] < (float) pred["confidence"]) {
                    predictions[label] = pred;
                }
            } catch (KeyNotFoundException) {
                predictions[label] = pred;
            }
        }
        
        var formatted = new JObject();
        foreach (var item in predictions) {
            formatted.Add(item.Key, item.Value);
        }
        
        var content = await ToJson(response);
        content["predictions"] = formatted;
        response.Content = CreateJsonContent(content.ToString());

        return response;
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
   
    private async Task<HttpResponseMessage> GetWorkflows(string accessToken)
    {
        var request = CreateAuthorizedRequest(
            method: HttpMethod.Get,
            path: $"/workflows",
            accessToken: accessToken
        );
        
        return await this.Context.SendAsync(request, this.CancellationToken);       
    }

    private async Task<string> GetAccessToken(string clientId, string clientSecret)
    {
        var authString = Convert.ToBase64String(new UTF8Encoding().GetBytes($"{clientId}:{clientSecret}"));
        
        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Script.TOKEN_ENDPOINT}?grant_type=client_credentials"));
        tokenRequest.Headers.Add("Authorization", $"Basic {authString}");
        tokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>() {});
        
        var tokenResponse = await this.Context.SendAsync(tokenRequest, this.CancellationToken);
        return (string) (await ToJson(tokenResponse))["access_token"];
    }
    
    private async Task<(HttpResponseMessage, HttpResponseMessage)> CreateDocument(byte[] content, string name, string accessToken)
    {
        var request = CreateAuthorizedRequest(
            method: HttpMethod.Post,
            path: "/documents",
            accessToken: accessToken
        );

        request.Content = CreateJsonContent(new JObject { ["name"] = name }.ToString());
        var response = await this.Context.SendAsync(request, this.CancellationToken);
        
        var fileUrl = (string) (await ToJson(response))["fileUrl"];
        
        var putRequest = new HttpRequestMessage(HttpMethod.Put, new Uri(fileUrl));
        putRequest.Headers.Add("Authorization", $"Bearer {accessToken}");
        
        putRequest.Content = new ByteArrayContent(content);
        
        var putResponse = await this.Context.SendAsync(putRequest, this.CancellationToken);
        return (response, putResponse);
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