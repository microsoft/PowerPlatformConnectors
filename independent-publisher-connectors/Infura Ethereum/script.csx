public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Transform the request before sending
        await TransformRequestAsync();

        // Forward the request
        var response = await ForwardRequestAsync();

        // Transform the response after receiving
        response = await TransformResponseAsync(response);

        return response;
    }

    public async Task TransformRequestAsync()
    {
        // Check if the operation ID matches what is specified in the OpenAPI definition of the connector
        if (IsOperationId("Eth_getBalance"))
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            ConvertToPositionalParamsArray(contentAsString, "Address", "Block");
        }
    }

    private async Task<HttpResponseMessage> ForwardRequestAsync()
    {
        // Use the context to forward/send an HTTP request
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return response;
    }

    private async Task<HttpResponseMessage> TransformResponseAsync(HttpResponseMessage response)
    {
        // Do the transformation if the response was successful
        // Otherwise return error responses as-is
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);

            var result = JObject.Parse(TryConvertRegexResult(responseString));

            response.Content = CreateJsonContent(result.ToString());
        }

        return response;
    }

    private string TryConvertRegexResult(string contentAsString)
    {
        // NOTE: We assume the body of the incoming request looks like this:
        // {
        //   "result": "<some hex encoded int>"
        // }

        // Parse as JSON object
        var contentAsJson = JObject.Parse(contentAsString);

        // Get the result field value
        var resultField = (string)contentAsJson["result"];

        // Check if it is hex encoded
        if (IsHexEncoded(resultField))
        {
            // Convert result from hex to int64 for better usability
            contentAsJson["result"] = ConvertHexToInt64(resultField);
        }

        return contentAsJson.ToString();
    }

    /// <summary>
    /// Converts the given json object and its specified properties to a flat array with its values only.
    /// </summary>
    /// <param name="contentAsString">Json object as string</param>
    /// <param name="properties">Property names to flatten in exact order</param>
    private void ConvertToPositionalParamsArray(string contentAsString, params string[] properties)
    {
        // NOTE: We assume the body of the incoming request looks like this:
        // {
        //   ...,
        //   "params":
        //      {
        //          "Address": "0x__________________",
        //          "Block": "latest"
        //      }
        // }
        //
        // RESULT: Given this function was called for '(Address, Block)'
        // {
        //   ...,
        //   "params": [
        //          "0x__________________",
        //          "latest"
        //   ]
        // }

        // Parse as JSON object
        var contentAsJson = JObject.Parse(contentAsString);      
        
        JObject parameters = (JObject)contentAsJson["params"];

        JArray positionalParams = new JArray();
        for (int i = 0; i < properties.Length; i++)
        {
            positionalParams.Add(parameters[properties[i]]);
        }

        contentAsJson["params"] = positionalParams;

        this.Context.Request.Content = CreateJsonContent(contentAsJson.ToString());
    }

    private bool IsHexEncoded(string candidate)
    {
        string HEX_ENCODED_UNSIGNED_INTEGER_REGEX = "^0x([1-9a-f]+[0-9a-f]*|0)$";
        var rx = new Regex(HEX_ENCODED_UNSIGNED_INTEGER_REGEX);
        return candidate != null ? rx.IsMatch(candidate) : false;
    }

    private long ConvertHexToInt64(string hexEncodedValue) => Convert.ToInt64(hexEncodedValue, 16);
    
    private bool IsOperationId(string expected) => string.Equals(expected, this.Context.OperationId, StringComparison.InvariantCultureIgnoreCase);
}