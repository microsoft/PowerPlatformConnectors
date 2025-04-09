using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Manipulate the request data as applicable before setting it back
        var requestContentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var requestContentAsJson = JObject.Parse(requestContentAsString);
        if (requestContentAsJson.ContainsKey("options"))
        {
            ((JObject)requestContentAsJson["options"])["ApplicationName"] = "Power Automate";
        }
        else
        {
            requestContentAsJson["options"] = new JObject
            {
                ["ApplicationName"] = "Power Automate"
            };
        }

        var queryParams = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        foreach (var queryParam in queryParams.AllKeys)
        {
            if (!requestContentAsJson.ContainsKey(queryParam))
                requestContentAsJson[queryParam] = queryParams[queryParam];
        }

        this.Context.Request.Content = CreateJsonContent(requestContentAsJson.ToString());

        // Make the actual API request
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            
        // Manipulate the response data as applicable before returning it
        var responseContentAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var responseContentAsJson = JObject.Parse(responseContentAsString);
        response.Content = CreateJsonContent(responseContentAsJson.ToString());

        return response;
    }
}
