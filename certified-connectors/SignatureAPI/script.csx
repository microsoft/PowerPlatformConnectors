using System;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;


public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        string operationId = this.Context.OperationId;

        // operationId can be a base64 in certain regions

        // "CreateCeremonyCustom" is a valid base64 encoded string, so we don't convert if they are known.

        string[] knownOperationIds = { "AddDocument", "AddTemplate", "AddDocumentDocx", "CreateCeremonyCustom", "GetDeliverable", "GetCapture" };

        if(!knownOperationIds.Contains(operationId, StringComparer.OrdinalIgnoreCase))
        {
            try {
                byte[] data = Convert.FromBase64String(this.Context.OperationId);
                operationId = System.Text.Encoding.UTF8.GetString(data);
            }
            catch (FormatException) {}
        }

        try
        {
            return await InnerExecuteAsync(operationId);
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


    private async Task<HttpResponseMessage> InnerExecuteAsync(string operationId)
    {
        if ("AddDocument".Equals(operationId, StringComparison.OrdinalIgnoreCase) ||
        "AddTemplate".Equals(operationId, StringComparison.OrdinalIgnoreCase) ||
        "AddDocumentDocx".Equals(operationId, StringComparison.OrdinalIgnoreCase))
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

            // Converts the array of Key: Value pairs into an JSON object { key: value }

            if(contentAsJson.TryGetValue("data", out JToken dataToken))
            {
                if(dataToken is JArray dataArray)
                {
                    var dataObject = ConvertJsonArrayToJObject(dataArray);
                    
                    contentAsJson["data"] = dataObject;
                }
                else
                {
                    throw new ClientException("The 'data' property exists but is not a JSON array.");
                }
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

            // Normalize alias (remove in the URL "+" and everything after that)

            this.Context.Request.RequestUri = RemoveAlias(this.Context.Request.RequestUri);

    
            HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            return response;
        }

        if("CreateCeremonyCustom".Equals(operationId, StringComparison.OrdinalIgnoreCase))
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

            if(contentAsJson.TryGetValue("authentication", out JToken authToken) && authToken is JObject authObject)
            {
                if(authObject.TryGetValue("data", out JToken dataToken))
                {
                    // Only convert if dataToken is an array.
                    if(dataToken is JArray dataArray)
                    {
                        var dataObject = ConvertJsonArrayToJObject(dataArray);
                        authObject["data"] = dataObject;
                    }
                    // If it's already an object (or anything other than a JArray), ignore it.
                }
            }

            string outboundContentAsString = contentAsJson.ToString();

            Context.Request.Content = CreateJsonContent(outboundContentAsString);

            // Normalize alias (remove in the URL "+" and everything after that)

            this.Context.Request.RequestUri = RemoveAlias(this.Context.Request.RequestUri);
    
            HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            return response;
        }


        if("GetDeliverable".Equals(operationId, StringComparison.OrdinalIgnoreCase))
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
                }
            }

            return response;
        }

        if("GetCapture".Equals(operationId, StringComparison.OrdinalIgnoreCase))
        {
            Uri originalUrl = this.Context.Request.RequestUri;

            string captureKey = HttpUtility.ParseQueryString(originalUrl.Query).Get("captureKey");
            
            // Normalize alias (remove in the URL "+" and everything after that)

            this.Context.Request.RequestUri = RemoveAlias(this.Context.Request.RequestUri);

            HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                var resultJson = JObject.Parse(responseString);
                var captures = resultJson["captures"];

                if(captures != null)
                {
                    string captureValue = captures.Value<string>(captureKey) ?? string.Empty;
                    
                    JObject result = new JObject{{ "value", captureValue }};

                    response.Content = CreateJsonContent(result.ToString());
                }
            }

            return response;
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

    private static Uri RemoveAlias(Uri uri)
    {
        // Original URI
        // Convert the URI to a string.
        string uriString = uri.AbsoluteUri;

        // Find the index of the '+' character.
        int plusIndex = uriString.IndexOf('+');

        // If a '+' is found, take a substring from the start to just before the '+'.
        if (plusIndex >= 0)
        {
            uriString = uriString.Substring(0, plusIndex);
        }

        // Create a new Uri instance with the modified string.
        return new Uri(uriString);
    }

    public static JObject ConvertJsonArrayToJObject(JArray jsonArray)
    {
        var jObject = new JObject();

        foreach (var token in jsonArray)
        {
            // Convert the token to a string.
            string item = token.ToString();

            // Find the index of the first colon
            int colonIndex = item.IndexOf(':');
            if (colonIndex >= 0)
            {
                // Split the entry into key and value parts.
                string fullKey = item.Substring(0, colonIndex).Trim();
                string valuePart = item.Substring(colonIndex + 1).Trim();

                // Attempt to parse the valuePart as a JSON array literal, if applicable.
                JToken valueToken;
                if (valuePart.StartsWith("[") && valuePart.EndsWith("]"))
                {
                    try
                    {
                        valueToken = JArray.Parse(valuePart);
                    }
                    catch (Exception)
                    {
                        valueToken = valuePart;
                    }
                }
                // Check for boolean values.
                else if (valuePart.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    valueToken = new JValue(true);
                }
                else if (valuePart.Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    valueToken = new JValue(false);
                }
                else
                {
                    valueToken = valuePart;
                }

                // Split the key on dots for deep nesting.
                string[] keys = fullKey.Split('.');
                JObject current = jObject;
                for (int i = 0; i < keys.Length - 1; i++)
                {
                    string key = keys[i];
                    if (current[key] == null)
                    {
                        current[key] = new JObject();
                    }
                    current = (JObject)current[key];
                }

                // Process the final key, checking for the array notation "[]".
                string finalKey = keys.Last();
                bool appendToArray = false;
                if (finalKey.EndsWith("[]"))
                {
                    appendToArray = true;
                    // Remove the "[]" from the key.
                    finalKey = finalKey.Substring(0, finalKey.Length - 2);
                }

                if (appendToArray)
                {
                    // If the property doesn't exist, create a new JArray.
                    JArray array;
                    if (current[finalKey] == null)
                    {
                        array = new JArray();
                        current[finalKey] = array;
                    }
                    else if (current[finalKey] is JArray)
                    {
                        array = (JArray)current[finalKey];
                    }
                    else
                    {
                        // If the property exists but isn't an array, convert it to one.
                        array = new JArray { current[finalKey] };
                        current[finalKey] = array;
                    }
                    // Append the value.
                    array.Add(valueToken);
                }
                else
                {
                    // Direct assignment for non-array keys.
                    current[finalKey] = valueToken;
                }
            }
            else
            {
                // Optionally handle tokens that do not contain a colon.
            }
        }

        return jObject;
    }
}

public class ClientException : Exception
{
    public ClientException(string message) : base(message)
    {

    }
    
}