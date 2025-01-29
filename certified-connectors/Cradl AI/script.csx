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
        var path = this.Context.Request.RequestUri.AbsolutePath.ToString();
        
        switch (path) {
            case "/v1/workflows":
                return await CreateRun();
                break;
            case "/v1/documents": 
                return await CreateDocument();
                break;
            case "/v1/models":
                return await GetModels();
                break;
        }

        return null;
    }
    
    private async Task<HttpResponseMessage> CreateRun()
    {
        var request = this.Context.Request;
        string workflowId = request.Headers.GetValues("WorkflowId").First();
        request.RequestUri = new Uri($"{Script.API_ENDPOINT}/workflows/{workflowId}/executions");

        return await this.Context.SendAsync(request, this.CancellationToken);       
    }
    
    private async Task<HttpResponseMessage> GetModels()
    {
        var myModelsRes = this.Context.SendAsync(
            CreateAuthorizedRequest(
                method: HttpMethod.Get,
                path: $"/models"
            ),
            this.CancellationToken
        );

        var publicModelsRes = this.Context.SendAsync(
            CreateAuthorizedRequest(
                method: HttpMethod.Get,
                path: $"/models?owner=las:organization:cradl"
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

    private async Task<HttpResponseMessage> CreateDocument()
    {
        string fileName = this.Context.Request.Headers.GetValues("Name").First();
        var fileContent = await this.Context.Request.Content.ReadAsByteArrayAsync();

        var request = CreateAuthorizedRequest(
            method: HttpMethod.Post,
            path: "/documents"
        );

        request.Content = CreateJsonContent(new JObject { ["name"] = fileName }.ToString());
        var response = await this.Context.SendAsync(request, this.CancellationToken);
        
        var fileUrl = (string) (await ToJson(response))["fileUrl"];
        
        var putRequest = new HttpRequestMessage(HttpMethod.Put, new Uri(fileUrl));
        putRequest.Headers.Add("Authorization", this.Context.Request.Headers.GetValues("Authorization").First());
        putRequest.Content = new ByteArrayContent(fileContent);
        var putResponse = await this.Context.SendAsync(putRequest, this.CancellationToken);
        
        if (!putResponse.IsSuccessStatusCode) {
            return putResponse;
        }
        
        return response;
    }
    
    private HttpRequestMessage CreateAuthorizedRequest(HttpMethod method, string path)
    {
        var request = new HttpRequestMessage(method, new Uri($"{Script.API_ENDPOINT}{path}"));
        request.Headers.Add("Authorization", this.Context.Request.Headers.GetValues("Authorization").First());
        return request;
    }
    
    private static async Task<JObject> ToJson(HttpResponseMessage response)
    {
        return JObject.Parse(await response.Content.ReadAsStringAsync());
    }
}