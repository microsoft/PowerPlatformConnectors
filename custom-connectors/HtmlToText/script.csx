public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Check if the operation ID matches what is specified in the OpenAPI definition of the connector
        if (this.Context.OperationId == "HtmlToText")
        {
            return await this.HandleHtmlToTextOperation().ConfigureAwait(false);
        }

        // Handle an invalid operation ID
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent($"Unknown operation ID '{this.Context.OperationId}'");
        return response;
    }

    private async Task<HttpResponseMessage> HandleHtmlToTextOperation()
    {
        HttpResponseMessage response;

        // The body of the incoming request is of following format:
        // {
        //   "html": "<some html>"
        // }
        var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);

        // Parse as JSON object
        var contentAsJson = JObject.Parse(contentAsString);

        // Get the value of text to check
        var html = (string)contentAsJson["html"];

        string outText;

        // Remove tables from html
        outText = Regex.Replace(html, @"<(table)[^>]*>(?><(table)[^>]*(?<tableTag>)|</table(?<-tableTag>)|.?)*(?(tableTag)(?!))</table>", "", RegexOptions.IgnoreCase);

        // Replace blocks with new lines
        string block = "div|header|p|hr|li|ol|ul";
        string patNestedBlock = $"(\\s*?</?({block})[^>]*?>)+\\s*";
        outText = Regex.Replace(outText, patNestedBlock, "\n", RegexOptions.IgnoreCase);

        // Replace br tag to newline.
        outText = Regex.Replace(outText, @"<(br)[^>]*>", "\n", RegexOptions.IgnoreCase);

        // Remove styles and scripts.
        outText = Regex.Replace(outText, @"<(script|style)[^>]*?>.*?</\1>", "", RegexOptions.Singleline);

        // Remove all tags.
        outText = Regex.Replace(outText, @"<[^>]*(>|$)", "", RegexOptions.Multiline);

        // Remove the whitespace preceding a new line.
        outText = Regex.Replace(outText, @"^(&nbsp;)\n", "\n", RegexOptions.Multiline);

        // Decode html specific characters
        outText = HttpUtility.HtmlDecode(outText); 

        // Removes leading and trailing whitespaces
        outText = outText.Trim();

        response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = new StringContent(outText);
        return response;
    }
}