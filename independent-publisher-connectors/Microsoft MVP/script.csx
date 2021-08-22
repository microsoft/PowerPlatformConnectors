public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Check if the operation ID matches what is specified in the OpenAPI definition of the connector
        if (this.Context.OperationId == "GetContributionAreas")
        {
            return await this.TransformOperationResult().ConfigureAwait(false);
        }
    }

    private async Task<HttpResponseMessage> TransformOperationResult()
    {
        // Use the context to forward/send an HTTP request
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        // Do the transformation if the response was successful, otherwise return error responses as-is
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            // Response string is JSON array
            var result = JArray.Parse(responseString);

            JArray resultArray = new JArray();

            JArray myAwardAreas = (JArray)result[0]["Contributions"][0]["ContributionArea"];
            foreach(JObject item in myAwardAreas)
            {
                resultArray.Add(item);
            }
            
            JArray otherAwardAreas = (JArray)result[1]["Contributions"];
            foreach (JObject item in otherAwardAreas)
            {
                JArray areas = (JArray)item["ContributionArea"];
                foreach(JObject subItem in areas)
                {
                    resultArray.Add(subItem);
                }
            }
            response.Content = CreateJsonContent(resultArray.ToString());
        }

        return response;
    }
}
