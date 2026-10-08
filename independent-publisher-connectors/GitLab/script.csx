public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (this.Context.OperationId == "createCommit")
        {
            var query = System.Web.HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
            var branch = query["branch"];
            var commitMessage = query["commit_message"];
            var bodyStr = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            JObject body = new JObject();
            if (!string.IsNullOrEmpty(bodyStr))
            {
                var token = JToken.Parse(bodyStr);
                if (token is JArray arr)
                {
                    body["actions"] = arr;
                }
                else if (token is JObject obj)
                {
                    var inner = obj["body"];
                    if (inner != null && inner["$"] != null)
                    {
                        var innerStr = inner["$"].ToString();
                        var innerToken = JToken.Parse(innerStr);
                        if (innerToken is JArray innerArr)
                            body["actions"] = innerArr;
                        else
                            body = (JObject)innerToken;
                    }
                    else
                    {
                        body = obj;
                    }
                }
            }
            if (!string.IsNullOrEmpty(branch)) body["branch"] = branch;
            if (!string.IsNullOrEmpty(commitMessage)) body["commit_message"] = commitMessage;
            var ub = new UriBuilder(this.Context.Request.RequestUri);
            var nq = System.Web.HttpUtility.ParseQueryString(ub.Query);
            nq.Remove("branch");
            nq.Remove("commit_message");
            ub.Query = nq.ToString();
            this.Context.Request.RequestUri = ub.Uri;
            this.Context.Request.Content = CreateJsonContent(body.ToString());
        }
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
    }
}
