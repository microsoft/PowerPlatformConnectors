public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var requestContentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var requestContentAsJSON = JArray.Parse(requestContentAsString);

        var data = new JArray();

        foreach (var item in requestContentAsJSON)
        {
            var metricKeyId = "$" + (string)item["metric_key_id"];
            var value = (float)item["value"];

            var obj = new JObject();
            obj[metricKeyId] = value;

            data.Add(obj);
        }

        var newRequest = new JObject();
        newRequest["data"] = data;

        using (var stringWriter = new StringWriter())
        {
            JsonSerializer.CreateDefault().Serialize(stringWriter, newRequest);
            var json = stringWriter.ToString();
            this.Context.Request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        return response;
    }
}