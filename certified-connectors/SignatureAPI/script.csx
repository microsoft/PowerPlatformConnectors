using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if ("AddDocument".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);

            var contentAsJson = JObject.Parse(contentAsString);

            var fileContentBase64 = (string)contentAsJson["file_content"];

            var fileContent = Convert.FromBase64String(fileContentBase64);

            Uri baseUri = BaseUri(Context.Request.RequestUri);
            string apiKey = Context.Request.Headers.GetValues("X-Api-Key").FirstOrDefault();

            HttpRequestMessage createFileRequest = new HttpRequestMessage();

            createFileRequest.Method = HttpMethod.Post;
            createFileRequest.RequestUri = new Uri(baseUri, "/v1/files");
            createFileRequest.Headers.Add("X-Api-Key", apiKey);

            var createFileResponse = await Context.SendAsync(createFileRequest, this.CancellationToken);

            if(!createFileResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Could not create temporary file. Status code: {createFileResponse.StatusCode}");
            }

            Uri locationUrl = createFileResponse.Headers.Location;

            string createFileResponseString = await createFileResponse.Content.ReadAsStringAsync();

            JObject createFileResponseJson = JObject.Parse(createFileResponseString);

            string putUrl = (string)createFileResponseJson["put_url"];

            HttpRequestMessage uploadFileRequest = new HttpRequestMessage();

            uploadFileRequest.Method = HttpMethod.Put;
            uploadFileRequest.RequestUri = new Uri(putUrl);
            uploadFileRequest.Content = new ByteArrayContent(fileContent);
            
            var uploadFileResponse = await Context.SendAsync(uploadFileRequest, this.CancellationToken);

            if(!uploadFileResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Could not upload temporary file. Status code: {createFileResponse.StatusCode}");
            }

            contentAsJson["file_content"] = null;
            contentAsJson["url"] = locationUrl.ToString();
            string outboundContentAsString = contentAsJson.ToString();

            Context.Request.Content = CreateJsonContent(outboundContentAsString);
    
            HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            return response;
        }

        if("GetDeliverable".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                var resultJson = JObject.Parse(responseString);
                var deliverableUrl = (string)resultJson["url"];

                if(deliverableUrl != null)
                {
                    HttpRequestMessage downloadRequest = new HttpRequestMessage();
                    downloadRequest.Method = HttpMethod.Get;
                    downloadRequest.RequestUri = new Uri(deliverableUrl);
                    
                    var downloadResponse = await Context.SendAsync(downloadRequest, this.CancellationToken);
                    var downloadBytes = await downloadResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(continueOnCapturedContext: false);
                    string downloadBase64 = Convert.ToBase64String(downloadBytes);

                    resultJson["file_content"] = downloadBase64;

                    response.Content = CreateJsonContent(resultJson.ToString());

                    return response;
                }
            }

        }


        throw new NotImplementedException();
    }

    private Uri BaseUri(Uri uri)
    {
        string baseUri = $"{uri.Scheme}://{uri.Host}";
        if(!uri.IsDefaultPort)
        {
            baseUri += $":{uri.Port}";
        }
        return new Uri(baseUri);
    }
}