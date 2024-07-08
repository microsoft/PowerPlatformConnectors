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
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        if (response.IsSuccessStatusCode)
        {
            // If the operationId is "GetWebhooksEventTypes", we need to transform the response

            switch (this.Context.OperationId)
            {
                case "GetWebhooksEventTypes":
                    await TransformEventTypes(response);
                    break;
                default:
                    break;
            }
        }

        return response;
    }

    /// <summary>
    /// This method transforms the response of the GetWebhooksEventTypes operation from string array to object.
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    private async Task TransformEventTypes(HttpResponseMessage response)
    {
        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
        var resultArray = JArray.Parse(responseString);
        var transformedEventTypes = resultArray.Select(eventType => new { name = eventType.ToString() }).ToList();
        string json = JsonConvert.SerializeObject(new { value = transformedEventTypes });
        response.Content = CreateJsonContent(json);
    }
}
