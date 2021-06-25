public class Script : ScriptBase
{
	public override async Task<HttpResponseMessage> ExecuteAsync()
	{
		//this.Context.Request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("X-Shopify-Access-Token", this.Context.Request.Headers.Authorization.Parameter);
		this.Context.Request.Headers.Add("X-Shopify-Access-Token", this.Context.Request.Headers.Authorization.Parameter);
		return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}
}