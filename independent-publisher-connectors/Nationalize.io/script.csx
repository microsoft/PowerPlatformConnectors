using System.Net;
using System.Web;

public class Script : ScriptBase
{

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        return await this.HandleForwardOperation().ConfigureAwait(false);
    }

    private async Task<HttpResponseMessage> HandleForwardOperation()
    {
        // get current request Uri
        var strRequestUri = this.Context.Request.RequestUri.AbsoluteUri;
        /* 
         * url?name=x&country_id=y&apikey=z
         * after split: 
         * 0: url
         * 1: name
         * 2: apikey
         */
        string[] parts = strRequestUri.ToString().Split(new char[] {'?','&'});
        
        int noOfParts = parts.Count();

        // init request Uri params
        string reqName = "";
        string reqApiKey= "";

        // build a request string for names
        string[] reqNameParts = parts[1].Split('=');
        string[] reqNames = (HttpUtility.UrlDecode(reqNameParts[1])).Split(',');
        int loopNo = 0;
        foreach (string reqNamePart in reqNames) 
        {
            reqName = reqName + "name[]=" + reqNamePart.Trim() + "&";
            loopNo++;
            // Max. 10 names
            if (loopNo == 10) break;
        }
        // remove last &
        reqName = reqName.Remove(reqName.Length-1);

        reqApiKey = "&"+parts[2];
        // if api key is none - remove it from the request URI, 
        // to make a free request, otherwise use apikey
        string[] reqApiKeyParts = reqApiKey.Split('=');
        if (reqApiKeyParts[1].Contains("none")) {
            reqApiKey = "";
        }
        
        // change request Uri in the request object 
        this.Context.Request.RequestUri = new Uri($"https://api.nationalize.io/?{reqName+reqApiKey}");

        // Use the context to an HTTP request
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        return response;
    }
}