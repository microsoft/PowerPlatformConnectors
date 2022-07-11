public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jObject = JObject.Parse(contentAsString);
        if (!jObject.ContainsKey("event_time"))
        {
            jObject.Add("event_time", DateTime.UtcNow);
        }
        if (this.Context.Request.Content.Headers.Contains("datacenter"))
        {
            string datacenter = this.Context.Request.Content.Headers.GetValues("datacenter").First();
            if (datacenter != "US") {
                string replace = "eu";
                string url = this.Context.Request.RequestUri.ToString();
                if (datacenter == "APAC")
                {
                    replace = "ap";
                }
                url = url.Replace("api.", string.Format("api{0}.", replace));
                this.Context.Logger.LogDebug("Url set to datacenter:{0} Url:{1}", datacenter, url);
                this.Context.Request.RequestUri = new Uri(url);
            }
        }
        this.Context.Request.Content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");
        this.Context.Request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await this.Context.SendAsync(
        this.Context.Request,
        this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        return response;
    }
}
