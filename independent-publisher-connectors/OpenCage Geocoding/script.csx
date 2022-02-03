public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (this.Context.OperationId == "ReverseGeocoding")
        {
            return await this.HandleReverseGeocodingOperation().ConfigureAwait(false);
        }
        if (this.Context.OperationId == "ForwardGeocoding")
        {
            return await this.HandleForwardGeocodingOperation().ConfigureAwait(false);
        }

        // Handle an invalid operation ID
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent($"Unknown operation ID '{this.Context.OperationId}'");
        return response;
    }

    private async Task<HttpResponseMessage> HandleReverseGeocodingOperation()
    {
        var latitude = this.Context.Request.Headers.GetValues("lat").First();
        var longitude = this.Context.Request.Headers.GetValues("long").First();
        this.Context.Request.RequestUri = new Uri(this.Context.Request.RequestUri.AbsoluteUri + "&q=" + latitude + "+" + longitude);
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        return response;
    }

    private async Task<HttpResponseMessage> HandleForwardGeocodingOperation()
    {
        var placeName = this.Context.Request.Headers.GetValues("placename").First();
        this.Context.Request.RequestUri = new Uri(this.Context.Request.RequestUri.AbsoluteUri + "&q=" + placeName);
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        return response;
    }
}
