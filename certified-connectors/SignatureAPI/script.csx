using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
        {
            return await InnerExecuteAsync();
        }
        catch(ClientException clientException)
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = new StringContent(clientException.Message);
            return response;
        }
        catch(Exception exception)
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            response.Content = new StringContent(exception.Message);
            return response;
        }
    }


    private async Task<HttpResponseMessage> InnerExecuteAsync()
    {
        if ("AddDocument".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase) ||
        "AddTemplate".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);

            JObject contentAsJson;

            try
            {
                contentAsJson = JObject.Parse(contentAsString);
            }
            catch (Exception exception)
            {
                throw new ClientException($"Could not parse JSON. Message: {exception.Message}");
            }

            var dataAsToken = contentAsJson.GetValue("data");

            if(dataAsToken != null)
            {
                var dataAsString = (string)dataAsToken;

                JObject dataAsJson;

                try
                {
                    dataAsJson = JObject.Parse(dataAsString);
                }
                catch (Exception exception)
                {
                    throw new ClientException($"Could not parse Template Data JSON. Message: {exception.Message}");
                }

                DecodeBase64Properties(dataAsJson);

                contentAsJson["data"] = dataAsJson;
            }

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

            Uri initialLocationUrl = createFileResponse.Headers.Location;

            // We force the host and scheme so that the API can recognize as an internal file url
            UriBuilder uriBuilder = new UriBuilder(initialLocationUrl)
            {
                Scheme = "https",
                Host = "api.signatureapi.com",
                Port = -1 // Ensures default port is used (443 for HTTPS)
            };

            Uri locationUrl = uriBuilder.Uri;

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

    private static void DecodeBase64Properties(JToken token)
    {
        if (token is JValue value && value.Type == JTokenType.String)
        {
            string str = value.ToString();
            if (str.StartsWith("base64:"))
            {
                string base64String = str.Substring(7);
                
                byte[] data;
                try
                {
                    data = Convert.FromBase64String(base64String);
                }
                catch (Exception exception)
                {
                    throw new ClientException($"Could not convert base 64 string in {token.Path}.");
                }

                string decodedString = Encoding.UTF8.GetString(data);
                value.Replace(JValue.FromObject(decodedString));
            }
        }
        else if (token is JObject obj)
        {
            foreach (var property in obj.Properties())
            {
                DecodeBase64Properties(property.Value);
            }
        }
        else if (token is JArray array)
        {
            foreach (var item in array)
            {
                DecodeBase64Properties(item);
            }
        }
    }
}

public class ClientException : Exception
{
    public ClientException(string message) : base(message)
    {

    }
    
}