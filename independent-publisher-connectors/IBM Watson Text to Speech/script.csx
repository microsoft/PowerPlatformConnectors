public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var response = await Context.SendAsync(Context.Request, CancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var bytes = await response.Content.ReadAsByteArrayAsync();
            
            var json = new JObject
            {
                ["base64"] = Convert.ToBase64String(bytes),
            };
            
            response.Content = CreateJsonContent(json.ToString());
        }

        return response;
    }
}
