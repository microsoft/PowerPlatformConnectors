public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (Context?.OperationId == "RawRequest")
        {
            return await HandleRawRequestAsync().ConfigureAwait(false);
        }

        return new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = CreateJsonContent($"Unknown operation ID '{Context?.OperationId}'")
        };
    }

    private async Task<HttpResponseMessage> HandleRawRequestAsync()
    {
        if (Context == null)
        {
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = CreateJsonContent("Context not defined on request")
            };
        }

        var request = Context.Request;
        if (request?.Content == null)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent("Content not defined on request")
            };
        }
        var content = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

        var obj = JObject.Parse(content);
        var body = GetValue<string>(obj, "body");
        var verb = GetValue<string>(obj, "verb");
        var url = $"{request.RequestUri}{GetValue<string>(obj, "service")}/{GetValue<string>(obj, "relativeUrl")}";

        if (!Uri.TryCreate(url, UriKind.Absolute, out _))
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent($"Invalid Url Format '{url}'")
            };
        }

        if (!string.IsNullOrEmpty(body))
        {
            request.Content = CreateJsonContent(body);
        }

        if (string.IsNullOrEmpty(verb))
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent("Verb must be provided")
            };
        }

        request.Method = new HttpMethod(verb);
        request.RequestUri = new Uri(url);

        foreach (var token in GetValue<JArray>(obj, "headers") ?? new JArray())
        {
            var item = token.Value<JObject>();

            var key = GetValue<string>(item, "key");
            if (key == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = CreateJsonContent("Headers are invalid")
                };
            }
            // Need to to add without validation because the etag causes it to fail
            request.Headers.TryAddWithoutValidation(key, GetValue<string>(item, "value"));
        }


        return await Context.SendAsync(request, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private static T? GetValue<T>(JObject? obj, string key) where T : class
    {
        return obj?.GetValue(key, StringComparison.InvariantCulture)?.Value<T>();
    }


}
