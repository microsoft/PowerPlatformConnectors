public class script: ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        return await this.ConvertAndTransformOperation().ConfigureAwait(false);
    }

    private async Task<HttpResponseMessage> ConvertAndTransformOperation()
    {
        // Manipulate the response data before returning it
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
        
        // Remove the set string from the response string
        string htmlString = @"
<a href=""https://twitter.com/igor_chubin?ref_src=twsrc%5Etfw"" class=""twitter-follow-button"" data-show-count=""false"">Follow @igor_chubin</a><script async src=""https://platform.twitter.com/widgets.js"" charset=""utf-8""></script>

<a aria-label=""Star chubin/wttr.in on GitHub"" data-count-aria-label=""# stargazers on GitHub"" data-count-api=""/repos/chubin/wttr.in#stargazers_count"" data-count-href=""/chubin/wttr.in/stargazers"" data-show-count=""true"" data-icon=""octicon-star"" href=""https://github.com/chubin/wttr.in"" class=""github-button"">wttr.in</a>

<!-- Place this tag where you want the button to render. -->
<a aria-label=""Star chubin/pyphoon on GitHub"" data-count-aria-label=""# stargazers on GitHub"" data-count-api=""/repos/chubin/pyphoon#stargazers_count"" data-count-href=""/chubin/pyphoon/stargazers"" data-show-count=""true"" data-icon=""octicon-star"" href=""https://github.com/chubin/pyphoon"" class=""github-button"">pyphoon</a>

<!-- Place this tag where you want the button to render. -->
<a aria-label=""Star schachmat/wego on GitHub"" data-count-aria-label=""# stargazers on GitHub"" data-count-api=""/repos/schachmat/wego#stargazers_count"" data-count-href=""/schachmat/wego/stargazers"" data-show-count=""true"" data-icon=""octicon-star"" href=""https://github.com/schachmat/wego"" class=""github-button"">wego</a>

<!-- Place this tag right after the last button or just before your close body tag. -->
<script async defer id=""github-bjs"" src=""https://buttons.github.io/buttons.js""></script>
";
        responseString = responseString.Replace(htmlString, "");

        // Update the response content with the modified string
        response.Content = new StringContent(responseString, Encoding.UTF8, "text/html"); 

        return response;
    }
}