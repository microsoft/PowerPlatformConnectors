using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Request Handler
        switch (this.Context.OperationId)
        {
            case "TriggerProgramCreateV1":
                UpdateRequestForCreateWebhooksApi("ProgramCreateV1"); // Pass the exact event version Id as defined in webhooks v2 service
                break;
            case "TriggerProgramUpdateV1":
                UpdateRequestForCreateWebhooksApi("ProgramUpdateV1");
                break;
            case "TriggerProgramDeleteV1":
                UpdateRequestForCreateWebhooksApi("ProgramDeleteV1");
                break;
            case "TriggerProgramTaskCreateV1":
                UpdateRequestForCreateWebhooksApi("ProgramTaskCreateV1");
                break;
            case "TriggerProgramTaskUpdateV1":
                UpdateRequestForCreateWebhooksApi("ProgramTaskUpdateV1");
                break;
            case "TriggerProgramTaskDeleteV1":
                UpdateRequestForCreateWebhooksApi("ProgramTaskDeleteV1");
                break;
            case "TriggerProgramRequestCreateV1":
                UpdateRequestForCreateWebhooksApi("ProgramRequestCreateV1");
                break;
            case "TriggerProgramRequestUpdateV1":
                UpdateRequestForCreateWebhooksApi("ProgramRequestUpdateV1");
                break;
            case "TriggerProgramRequestDeleteV1":
                UpdateRequestForCreateWebhooksApi("ProgramRequestDeleteV1");
                break;
            default:
                break;
        }

        // Send the request
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }

    private async Task UpdateRequestForCreateWebhooksApi(string eventType)
    {
        // Update the request URI to point to `/webhooks/v1/subscriptions`
        var baseUri = new Uri(this.Context.Request.RequestUri.GetLeftPart(UriPartial.Authority));
        this.Context.Request.RequestUri = new Uri(baseUri, "/webhooks/v1/subscriptions");

        string originalRequestBody = await this.Context.Request.Content.ReadAsStringAsync();
        JObject requestBodyJson = string.IsNullOrEmpty(originalRequestBody)
            ? new JObject()
            : JObject.Parse(originalRequestBody);

        // Add or update the `eventType` property
        requestBodyJson["eventTypes"] = new JArray(eventType);

        // Set the modified body back to the request
        this.Context.Request.Content = new StringContent(
            requestBodyJson.ToString(),
            System.Text.Encoding.UTF8,
            this.Context.Request.Content.Headers.ContentType?.MediaType ?? "application/json"
        );
    }
}
