public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var requestContentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var requestContentAsObject = JObject.Parse(requestContentAsString);

        var templateVariables = (JArray)requestContentAsObject["templateVariables"];

        var data = new JArray();

        foreach (var item in templateVariables)
        {
            var name = (string)item["variableName"];
            var value = (string)item["variableValue"];

            var obj = new JObject();
            obj[name] = value;

            data.Add(obj);
        }

        requestContentAsObject["templateVariables"] = data;

        using (var stringWriter = new StringWriter())
        {
            JsonSerializer.CreateDefault().Serialize(stringWriter, requestContentAsObject);
            var json = stringWriter.ToString();
            this.Context.Request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        return response;
    }
}