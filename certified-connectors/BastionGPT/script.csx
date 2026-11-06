// BastionGPT connector custom code.
//
// Applies to two operations only (see scriptOperations in apiProperties.json):
//   AskQuestion   - accepts a single question (+ optional instructions / document ID), builds the
//                   messages array the chat completion endpoint expects, forwards to
//                   POST /v1/ChatCompletion, and flattens the response to { answer, finish_reason, ... }.
//   GetTranscript - forwards GET /get/TranscribeFile and normalizes the text/plain result into
//                   { status: processing|completed, segments: [...], text: "..." } with status 200,
//                   so makers can loop on the status field instead of on HTTP status codes.
// Every other operation is forwarded unchanged. API error responses (4xx/5xx) are passed through
// unchanged so the service's own message and code reach the caller.
public class Script : ScriptBase
{
    private const string AskQuestionOperationId = "AskQuestion";
    private const string GetTranscriptOperationId = "GetTranscript";
    private const string ChatCompletionPath = "/v1/ChatCompletion";
    private const int DefaultMaxTokens = 1000;

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (this.IsOperation(AskQuestionOperationId))
        {
            return await this.HandleAskQuestion().ConfigureAwait(false);
        }

        if (this.IsOperation(GetTranscriptOperationId))
        {
            return await this.HandleGetTranscript().ConfigureAwait(false);
        }

        // Not a scripted operation: forward unchanged.
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
    }

    // The operation id can arrive base64-encoded in some regions (documented known issue),
    // so compare against both the raw and the decoded value.
    private bool IsOperation(string operationId)
    {
        string raw = this.Context.OperationId ?? string.Empty;
        if (string.Equals(raw, operationId, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        try
        {
            byte[] data = Convert.FromBase64String(raw);
            string decoded = Encoding.UTF8.GetString(data);
            return string.Equals(decoded, operationId, StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private async Task<HttpResponseMessage> HandleAskQuestion()
    {
        string requestBody = string.Empty;
        if (this.Context.Request.Content != null)
        {
            requestBody = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        JObject input;
        try
        {
            input = string.IsNullOrWhiteSpace(requestBody) ? new JObject() : JObject.Parse(requestBody);
        }
        catch (JsonException)
        {
            return CreateErrorResponse(HttpStatusCode.BadRequest, "The request body must be a JSON object.", "invalid_request");
        }

        string question = GetString(input, "question");
        if (string.IsNullOrWhiteSpace(question))
        {
            return CreateErrorResponse(HttpStatusCode.BadRequest, "A question is required.", "question_required");
        }

        // Build the conversation the chat completion endpoint expects.
        JArray messages = new JArray();
        string instructions = GetString(input, "instructions");
        if (!string.IsNullOrWhiteSpace(instructions))
        {
            messages.Add(new JObject(new JProperty("role", "system"), new JProperty("content", instructions)));
        }
        messages.Add(new JObject(new JProperty("role", "user"), new JProperty("content", question)));

        JObject upstream = new JObject();
        upstream["messages"] = messages;

        JToken maxTokens = input["max_tokens"];
        upstream["max_tokens"] = HasValue(maxTokens) ? maxTokens : new JValue(DefaultMaxTokens);

        JToken temperature = input["temperature"];
        if (HasValue(temperature))
        {
            upstream["temperature"] = temperature;
        }

        string documentId = GetString(input, "document_id");
        if (!string.IsNullOrWhiteSpace(documentId))
        {
            upstream["document_id"] = documentId;
        }

        // Route to the real chat completion endpoint (this operation's own path is virtual).
        UriBuilder builder = new UriBuilder(this.Context.Request.RequestUri);
        builder.Path = ChatCompletionPath;
        builder.Query = string.Empty;
        this.Context.Request.RequestUri = builder.Uri;
        this.Context.Request.Method = HttpMethod.Post;
        this.Context.Request.Content = CreateJsonContent(upstream.ToString(Newtonsoft.Json.Formatting.None));

        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            // Pass service errors (400 text_too_long, 401, 404 document_not_found, 429, 502 ...) through unchanged.
            return response;
        }

        string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        JObject completion;
        try
        {
            completion = JObject.Parse(responseBody);
        }
        catch (JsonException)
        {
            // Unexpected shape from the service: return it as-is rather than hide it.
            return response;
        }

        JObject flat = new JObject();
        JToken first = null;
        JToken choices = completion["choices"];
        if (choices != null && choices.Type == JTokenType.Array && ((JArray)choices).Count > 0)
        {
            first = choices[0];
        }

        string answer = string.Empty;
        string finishReason = null;
        if (first != null && first.Type == JTokenType.Object)
        {
            JToken message = first["message"];
            if (message != null && message.Type == JTokenType.Object)
            {
                answer = (string)message["content"] ?? string.Empty;
            }
            finishReason = (string)first["finishReason"];
        }

        flat["answer"] = answer;
        flat["finish_reason"] = finishReason;
        flat["response_id"] = (string)completion["id"];

        JToken usage = completion["usage"];
        if (usage != null && usage.Type == JTokenType.Object)
        {
            flat["prompt_tokens"] = usage["promptTokens"];
            flat["completion_tokens"] = usage["completionTokens"];
            flat["total_tokens"] = usage["totalTokens"];
        }

        HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
        result.Content = CreateJsonContent(flat.ToString(Newtonsoft.Json.Formatting.None));
        return result;
    }

    private async Task<HttpResponseMessage> HandleGetTranscript()
    {
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.Accepted)
        {
            // Still transcribing (the service also answers 202 for unknown transcript ids).
            return CreateTranscriptResponse("processing", new JArray(), string.Empty);
        }

        if (response.StatusCode != HttpStatusCode.OK)
        {
            // 400 (missing id / failed transcription), 401, 429 ... pass through unchanged.
            return response;
        }

        string body = string.Empty;
        if (response.Content != null)
        {
            body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        // The completed transcript is served as text/plain containing a JSON array of
        // { SpeakerTimestamp, SpeakerName, SpeakerContent } objects.
        JArray rawSegments = null;
        try
        {
            JToken parsed = JToken.Parse(body);
            if (parsed.Type == JTokenType.Array)
            {
                rawSegments = (JArray)parsed;
            }
        }
        catch (JsonException)
        {
            rawSegments = null;
        }

        if (rawSegments == null)
        {
            // Unexpected shape: still report completed and expose the raw text.
            return CreateTranscriptResponse("completed", new JArray(), body ?? string.Empty);
        }

        JArray segments = new JArray();
        StringBuilder text = new StringBuilder();
        foreach (JToken item in rawSegments)
        {
            if (item.Type != JTokenType.Object)
            {
                continue;
            }

            string timestamp = (string)item["SpeakerTimestamp"] ?? string.Empty;
            string speaker = (string)item["SpeakerName"] ?? string.Empty;
            string content = (string)item["SpeakerContent"] ?? string.Empty;

            segments.Add(new JObject(
                new JProperty("timestamp", timestamp),
                new JProperty("speaker", speaker),
                new JProperty("text", content)));

            if (text.Length > 0)
            {
                text.Append('\n');
            }
            text.Append(timestamp).Append(' ').Append(speaker).Append(": ").Append(content);
        }

        return CreateTranscriptResponse("completed", segments, text.ToString());
    }

    private static HttpResponseMessage CreateTranscriptResponse(string status, JArray segments, string text)
    {
        JObject payload = new JObject();
        payload["status"] = status;
        payload["segments"] = segments;
        payload["text"] = text;

        HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
        result.Content = CreateJsonContent(payload.ToString(Newtonsoft.Json.Formatting.None));
        return result;
    }

    private static HttpResponseMessage CreateErrorResponse(HttpStatusCode statusCode, string message, string code)
    {
        JObject payload = new JObject();
        payload["message"] = message;
        payload["code"] = code;

        HttpResponseMessage result = new HttpResponseMessage(statusCode);
        result.Content = CreateJsonContent(payload.ToString(Newtonsoft.Json.Formatting.None));
        return result;
    }

    private static string GetString(JObject obj, string name)
    {
        JToken token = obj[name];
        if (!HasValue(token))
        {
            return null;
        }
        return token.Type == JTokenType.String ? (string)token : token.ToString();
    }

    private static bool HasValue(JToken token)
    {
        return token != null && token.Type != JTokenType.Null && token.Type != JTokenType.Undefined;
    }
}
