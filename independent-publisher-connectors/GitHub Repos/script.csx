using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (this.Context.OperationId == "RepositoryZipGet") 
        {
            return await this.GetZipOperation().ConfigureAwait(false);
        }
        else if (this.Context.OperationId == "UserRepositoriesGet") 
        {
            return await this.GetRepoOperation().ConfigureAwait(false);
        }

        // Handle an invalid operation ID
        HttpResponseMessage response = new HttpResponseMessage(
            HttpStatusCode.BadRequest
        );
        response.Content = CreateJsonContent(
            $"Unknown operation ID '{this.Context.OperationId}'"
        );
        return response;
    }

    private async Task<HttpResponseMessage> GetZipOperation()
    {
        using (HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false))
        {
            if (response.StatusCode == HttpStatusCode.Redirect && response.Headers.Location != null)
            {
                var redirectUrl = response.Headers.Location.ToString();
                var redirectRequest = new HttpRequestMessage(HttpMethod.Get, redirectUrl);
                redirectRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/zip"));

                using (var redirectResponse = await this.Context.SendAsync(redirectRequest, this.CancellationToken).ConfigureAwait(false))
                {
                    if (redirectResponse.IsSuccessStatusCode && redirectResponse.Content.Headers.ContentType.MediaType == "application/zip")
                    {
                        // Fetch the ZIP file content as a byte array
                        byte[] zipBytes = await redirectResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                        // Base64 encode the ZIP file content
                        string base64Zip = Convert.ToBase64String(zipBytes);

                        var jsonContent = JsonConvert.SerializeObject(new Dictionary<string, object>
                        {
                            {"$content-type", "application/zip"},
                            {"$content", base64Zip}
                        });

                        // Return the JSON object as a response
                        return new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                        };
                    }
                    else
                    {
                        // Handle unexpected content types or errors
                        return CreateErrorResponse("The response did not contain a file as expected.");
                    }
                }
            }
            else
            {
                // Handle cases where the initial response is not a redirect or no Location header is present
                return CreateErrorResponse("Failed to retrieve the ZIP file.");
            }
        }
    }

    private async Task<HttpResponseMessage> GetRepoOperation()
    {
        using var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        using var reader = new JsonTextReader(new StreamReader(await response.Content.ReadAsStreamAsync().ConfigureAwait(false)));
        var responseArray = await JArray.LoadAsync(reader).ConfigureAwait(false);

        var ownersArray = responseArray
            .Select(item => item["owner"]?["login"]?.Value<string>())
            .Distinct()
            .Where(login => !string.IsNullOrEmpty(login))
            .Select(login => new { owner = login })
            .ToArray();

        var responseObject = new
        {
            owners = ownersArray,
            repositories = responseArray
        };

        string responseString = JsonConvert.SerializeObject(responseObject);
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseString, Encoding.UTF8, "application/json")
        };
    }


    private HttpResponseMessage CreateErrorResponse(string message)
    {
        return new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(message, Encoding.UTF8, "text/plain")
        };
    }
}
