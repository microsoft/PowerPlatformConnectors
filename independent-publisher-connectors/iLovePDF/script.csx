using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var _logger = this.Context.Logger;

        var authURL = "https://api.ilovepdf.com/v1/auth";
        var startURL = "https://api.ilovepdf.com/v1/start/{tool}";
        var uploadURL = "https://{server}/v1/upload";
        var processURL = "https://{server}/v1/process";
        var downloadURL = "https://{server}/v1/download/{task}";

        var authorizationHeader = this.Context.Request.Headers.GetValues("public_key").FirstOrDefault();
        var toolHeader = this.Context.Request.Headers.GetValues("tool").FirstOrDefault();
        var cloudFileHeader = this.Context.Request.Headers.Contains("cloud_file") ? 
            this.Context.Request.Headers.GetValues("cloud_file").FirstOrDefault() : null;
        var filenameHeader = this.Context.Request.Headers.GetValues("filename").FirstOrDefault();
        
        // Determine operation type
        var requestPath = this.Context.Request.RequestUri.AbsolutePath;
        var isLocalFileUpload = requestPath.Contains("processLocalFile");
        
        _logger.LogInformation($"Request path: {requestPath}");
        _logger.LogInformation($"Is local file upload: {isLocalFileUpload}");

        var authtUrl = new Uri(authURL);
        HttpRequestMessage authRequest = new HttpRequestMessage(HttpMethod.Post, authtUrl);
        var authRequestBody = new Dictionary<string, string>
        {
            { "public_key", authorizationHeader }
        };
        var authRequestBodyEncoded = new FormUrlEncodedContent(authRequestBody);
        authRequest.Content = authRequestBodyEncoded;
        HttpResponseMessage authResponse = await this.Context.SendAsync(authRequest, this.CancellationToken);
        var responseString = await authResponse.Content.ReadAsStringAsync();
        var jsonResponse = JObject.Parse(responseString);
        jsonResponse.TryGetValue("token", out JToken signedToken);
        var signedBearerToken = AuthenticationHeaderValue.Parse("Bearer " + signedToken.ToString());

        var starttUrl = new Uri(startURL.Replace("{tool}", toolHeader));
        HttpRequestMessage startRequest = new HttpRequestMessage(HttpMethod.Get, starttUrl);
        startRequest.Headers.Authorization = signedBearerToken;

        HttpResponseMessage startResponse = await this.Context.SendAsync(startRequest, this.CancellationToken);
        startResponse.EnsureSuccessStatusCode();

        var startResponseString = await startResponse.Content.ReadAsStringAsync();
        var startJsonResponse = JObject.Parse(startResponseString);
        startJsonResponse.TryGetValue("server", out JToken serverToken);
        var server = serverToken.ToString();
        startJsonResponse.TryGetValue("task", out JToken taskToken);
        var task = taskToken.ToString();
        startJsonResponse.TryGetValue("remaining_files", out JToken remainingFilesToken);
        var remainingFiles = remainingFilesToken.ToObject<int>();

        var upldUrl = new Uri(uploadURL.Replace("{server}", server));
        _logger.LogInformation($"Upload URL: {upldUrl}");
        HttpRequestMessage uploadRequest = new HttpRequestMessage(HttpMethod.Post, upldUrl);
        _logger.LogInformation($"Upload request: {uploadRequest}");
        uploadRequest.Headers.Authorization = signedBearerToken;
        _logger.LogInformation($"Authorization header: {signedBearerToken}");
        
        var uploadRequestBody = new MultipartFormDataContent();
        uploadRequestBody.Add(new StringContent(task), "task");
        
        string serverFilename;
        
        if (isLocalFileUpload)
        {
            // Handle local file upload
            _logger.LogInformation("Processing local file upload");
            
            // Get the uploaded file from the request
            var fileContent = await this.Context.Request.Content.ReadAsByteArrayAsync();
            var byteArrayContent = new ByteArrayContent(fileContent);
            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            
            uploadRequestBody.Add(byteArrayContent, "file", filenameHeader);
            _logger.LogInformation($"Added local file: {filenameHeader}");
        }
        else
        {
            // Handle cloud file upload (existing logic)
            _logger.LogInformation("Processing cloud file upload");
            var cloudFile = cloudFileHeader.ToString();
            uploadRequestBody.Add(new StringContent(cloudFile), "cloud_file");
            _logger.LogInformation($"Added cloud file: {cloudFile}");
        }
        
        _logger.LogInformation($"Upload request body: {uploadRequestBody}");
        uploadRequest.Content = uploadRequestBody;
        _logger.LogInformation($"Upload request content: {uploadRequest.Content}");
        var uploadResponse = await this.Context.SendAsync(uploadRequest, this.CancellationToken);
        _logger.LogInformation($"Upload response: {uploadResponse}");
        var uploadResponseString = await uploadResponse.Content.ReadAsStringAsync();
        var uploadJsonResponse = JObject.Parse(uploadResponseString);
        uploadJsonResponse.TryGetValue("server_filename", out JToken serverFilenameID);
        serverFilename = serverFilenameID.ToString();

        var processUrl = new Uri(processURL.Replace("{server}", server));
        _logger.LogInformation($"Process URL: {processUrl}");
        HttpRequestMessage processRequest = new HttpRequestMessage(HttpMethod.Post, processUrl);
        _logger.LogInformation($"Process request: {processRequest}");
        processRequest.Headers.Authorization = signedBearerToken;
        _logger.LogInformation($"Authorization header: {signedBearerToken}");
        var processRequestBodyObject = new JObject
        {
            { "task", task },
            { "tool", toolHeader },
            { "files", new JArray(new JObject
                {
                    { "server_filename", serverFilename },
                    { "filename", filenameHeader }
                })
            }
        };
        var processRequestBodyString = processRequestBodyObject.ToString();
        _logger.LogInformation($"Process request JSON body: {processRequestBodyString}");
        var processRequestBody = new MultipartFormDataContent();
        processRequest.Content = new StringContent(processRequestBodyString, Encoding.UTF8, "application/json");
        _logger.LogInformation($"Process request body: {processRequestBody}");
        _logger.LogInformation($"Process request content: {processRequest.Content}");
        var processResponse = await this.Context.SendAsync(processRequest, this.CancellationToken);
        _logger.LogInformation($"Process response: {processResponse}");

        var downloadUrl = new Uri(downloadURL.Replace("{server}", server).Replace("{task}", task));
        _logger.LogInformation($"Download URL: {downloadUrl}");
        HttpRequestMessage downloadRequest = new HttpRequestMessage(HttpMethod.Get, downloadUrl);
        _logger.LogInformation($"Download request: {downloadRequest}");
        downloadRequest.Headers.Authorization = signedBearerToken;
        _logger.LogInformation($"Authorization header: {signedBearerToken}");
        var downloadResponse = await this.Context.SendAsync(downloadRequest, this.CancellationToken);
        _logger.LogInformation($"Download response: {downloadResponse}");
        
        return downloadResponse;
    }
}