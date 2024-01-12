public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Get the API key from the request header
        this.Context.Request.Headers.TryGetValues("x-api-key", out var apiKey);
        // Make a new request to get the user ID
        // Then replace the placeholder with the actual user ID in the path
        var customerIdRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.moreapp.com/api/v1.0/customers");
        customerIdRequest.Headers.TryAddWithoutValidation("x-api-key", apiKey);
        var customerIdResponse = await this.Context.SendAsync(customerIdRequest, this.CancellationToken).ConfigureAwait(false);
        var content = await customerIdResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonContent = JArray.Parse(content);
        var id = (string)jsonContent[0]["customerId"];
        this.Context.Request.RequestUri = SetUserId(this.Context.Request.RequestUri, id);

        // Add 'Location' header to webhook creation response. This is needed to support webhook deletion
        // Runs only for NewSubmission and TaskFulfilled
        if (this.Context.OperationId == "NewSubmission" || this.Context.OperationId == "TaskFulfilled")
        {
            JObject jObject = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync());
            // The url we get from Microsoft contains some escaped characters, which breaks the webhook. The url should be decoded
            String url = HttpUtility.UrlDecode(jObject["url"].ToString());
            jObject["url"] = url;
            jObject.Add("type", JArray.Parse(this.Context.OperationId == "NewSubmission" ? @"['submission.created']" : @"['submission.task.fulfilled']"));
            this.Context.Request.Content = CreateJsonContent(jObject.ToString());
            HttpResponseMessage webhookResponse = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
            // Add the location header to the response
            JObject body = JObject.Parse(await webhookResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
            webhookResponse.Headers.Location = new Uri(string.Format("{0}/{1}", SetUserId(this.Context.OriginalRequestUri, id), body.GetValue("id")));
            return webhookResponse;
        }
        // Remove the "gridfs://registrationFiles/ prefix from file IDs
        // Runs only for DownloadFile
        else if (this.Context.OperationId == "DownloadFile")
        {
            this.Context.Request.RequestUri = new Uri(HttpUtility.UrlDecode(this.Context.Request.RequestUri.ToString()).Replace("gridfs://registrationFiles/", ""));
        }

        // Always make the default request. You can alter the request above this line and the response below
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

        // Get a submission report
        // Runs only for DownloadReport
        if (this.Context.OperationId == "DownloadReport")
        {
            JObject responseBody = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            // Get the report number from the original request's header
            // It is in the header because there is no other way to specify an input value than to put it somewhere in the request (there is no body for a GET request)
            int reportNumber = Int32.Parse(this.Context.Request.Headers.GetValues("reportnumber").FirstOrDefault()) - 1;
            // See DEV-3970
            Thread.Sleep(2000);
            var reportRequest = new HttpRequestMessage(HttpMethod.Get, string.Format("https://api.moreapp.com/api/v1.0/customers/{0}/registrationFile/{1}/download", id, responseBody["mailStatuses"][reportNumber]["pdfFileId"]));
            reportRequest.Headers.TryAddWithoutValidation("x-api-key", apiKey);
            var reportResponse = await this.Context.SendAsync(reportRequest, this.CancellationToken).ConfigureAwait(false);
            // Return the response of our request to get the report instead of the original response
            return reportResponse;
        }

        return response;
    }

    private Uri SetUserId(Uri originalUri, string id)
    {
        var originalUriString = originalUri.ToString();
        // Some urls contain customerId1 instead of customerId. This is because otherwise you would get multiple identical urls in the swagger config.
        // This is not allowed, but the New submission and Task fulfilled triggers are created using the same url for example
        originalUriString = originalUriString.Replace(":customerId1:", id).Replace(":customerId:", id);
        var newUri = new Uri(originalUriString);
        return newUri;
    }
}
