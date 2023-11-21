    public class Script : ScriptBase
    {
        private const string DEFAULT_USER = "None";
        private const int DEFAULT_MAX_TOKENS = 2048;
        private const double DEFAULT_TEMPERATURE = 0.9d;
        private const double DEFAULT_TOP_P = 1d;
        private const double DEFAULT_FREQUENCY_PENALTY = 0d;
        private const double DEFAULT_PRESENCE_PENALTY = 0.6d;
        private const string DEFAULT_STOP = "None";
        public override async Task<HttpResponseMessage> ExecuteAsync()
        {
            Context.Logger.LogInformation($"started {DateTime.UtcNow}");

            switch (Context.OperationId)
            {
                case "CreateCompletion":
                    return await ProcessCompletion().ConfigureAwait(false);
                    break;
                case "ChatCompletion":
                    return await ProcessChatCompletion().ConfigureAwait(false);
                    break;
                default:
                    break;
            }
            // Handle an invalid operation ID
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = CreateJsonContent($"Unknown operation ID '{this.Context.OperationId}'");
            return response;
        }

        private async Task<HttpResponseMessage> ProcessChatCompletion()
        {

            Context.Logger.LogInformation("reading body");
            var body = JsonConvert.DeserializeObject<IncomingChatRequestBody>(await Context.Request.Content.ReadAsStringAsync());

            if (body == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = CreateJsonContent(JsonConvert.SerializeObject(new { message = "error parsing body" })) };
            }

            var newBody = new ChatRequestBody
            {
                Messages = body.BuildMessages(),
                MaxTokens = body?.MaxTokens ?? DEFAULT_MAX_TOKENS,
                Temperature = body?.Temperature ?? DEFAULT_TEMPERATURE,
                FrequencyPenalty = body?.FrequencyPenalty ?? DEFAULT_FREQUENCY_PENALTY,
                PresencePenalty = body?.PresencePenalty ?? DEFAULT_PRESENCE_PENALTY,
                Stop = body?.Stop ?? DEFAULT_STOP,
                TopP = body?.TopP ?? DEFAULT_TOP_P,
                Stream = body?.Stream ?? false,
                N = body?.N ?? 1,
                User = body?.User ?? DEFAULT_USER
            };

            Context.Request.Content = CreateJsonContent(JsonConvert.SerializeObject(newBody));

            HttpResponseMessage response = await Context.SendAsync(Context.Request, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);


            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                // var responseJson = JObject.Parse(responseBody);
                var rawResult = JsonConvert.DeserializeObject<RawChatResult>(responseBody);
                body.Messages.Add(new ChatMessage { Role = Role.assistant, Content = rawResult.Choices?[0].Message.Content ?? "NO MATCH" });

                var newResponseBody = new ChatResponseBody
                {
                    Messages = body.Messages,
                    Answer = rawResult.Choices?[0].Message.Content ?? "NO MATCH",
                    SystemInstruction = body.SystemInstruction,
                    RawResult = rawResult
                };

                HttpResponseMessage newResponse = new HttpResponseMessage(HttpStatusCode.OK);
                newResponse.Content = CreateJsonContent(JsonConvert.SerializeObject(newResponseBody));
                return newResponse;
            }

            return response;
        }

        private async Task<HttpResponseMessage> ProcessCompletion()
        {
            const string DEFAULT_API_VERSION = "2021-08-01";

            Context.Logger.LogInformation("reading body");
            var body = JsonConvert.DeserializeObject<IncomingRequestBody>(await Context.Request.Content.ReadAsStringAsync());

            if (body == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = CreateJsonContent(JsonConvert.SerializeObject(new { message = "error parsing body" })) };
            }

            var newBody = new RequestBody
            {
                Prompt = body.BuildPrompt(),
                MaxTokens = body?.MaxTokens ?? DEFAULT_MAX_TOKENS,
                Temperature = body?.Temperature ?? DEFAULT_TEMPERATURE,
                TopP = body?.TopP ?? DEFAULT_TOP_P,
                FrequencyPenalty = body?.FrequencyPenalty ?? DEFAULT_FREQUENCY_PENALTY,
                PresencePenalty = body?.PresencePenalty ?? DEFAULT_PRESENCE_PENALTY,
                Stop = body?.Stop ?? DEFAULT_STOP
            };

            Context.Request.Content = CreateJsonContent(JsonConvert.SerializeObject(newBody));

            HttpResponseMessage response = await Context.SendAsync(Context.Request, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseJson = JObject.Parse(responseBody);
                var completionObject = JsonConvert.DeserializeObject<AnswerObject>(responseJson?["choices"]?[0]?.ToString());
                body.History.Add(new QAPair() { Question = body.Prompt, Answer = StripTrailingTokens(completionObject?.Text ?? "NO MATCH") });

                var newResponseBody = new ResponseBody
                {
                    History = body.History,
                    Answer = StripTrailingTokens(completionObject?.Text ?? "NO MATCH"),
                    InitialScope = body.InitialScope
                };

                HttpResponseMessage newResponse = new HttpResponseMessage(HttpStatusCode.OK);
                newResponse.Content = CreateJsonContent(JsonConvert.SerializeObject(newResponseBody));
                return newResponse;
            }

            return response;
        }

        const string END_TOKEN = "<|im_end|>";

        private string StripTrailingTokens(string rawanswer)
        {
            return rawanswer.Replace(END_TOKEN, "").Trim();
        }
    }

    internal class ChatResponseBody
    {
        [JsonProperty("messages")]
        public List<ChatMessage> Messages { get; set; }
        [JsonProperty("answer")]
        public string Answer { get; set; }
        [JsonProperty("system_instruction")]
        public string SystemInstruction { get; set; }
        [JsonProperty("raw_result")]
        public RawChatResult RawResult { get; set; }
    }

    public class RawChatResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("object")]
        public string Object { get; set; }
        [JsonProperty("created")]
        public int Created { get; set; }
        [JsonProperty("model")]
        public string Model { get; set; }
        [JsonProperty("choices")]
        public List<ChatChoice> Choices { get; set; }
        [JsonProperty("usage")]
        public Usage Usage { get; set; }
    }

    public class Usage
    {
        [JsonProperty("prompt_tokens")]
        public int PromptTokens { get; set; }
        [JsonProperty("completion_tokens")]
        public int CompletionTokens { get; set; }
        [JsonProperty("total_tokens")]
        public int TotalTokens { get; set; }
    }

    public class ChatChoice
    {
        [JsonProperty("message")]
        public ChatMessage Message { get; set; }
    }

    public class ChatRequestBody
    {
        [JsonProperty("messages")]
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; }
        [JsonProperty("temperature")]
        public double Temperature { get; set; }
        [JsonProperty("frequency_penalty")]
        public double FrequencyPenalty { get; set; }
        [JsonProperty("presence_penalty")]
        public double PresencePenalty { get; set; }
        [JsonProperty("stop")]
        public string Stop { get; set; }
        [JsonProperty("top_p")]
        public double TopP { get; set; }
        [JsonProperty("n")]
        public int N { get; set; }
        [JsonProperty("stream")]
        public bool Stream { get; set; }
        [JsonProperty("user")]
        public string User { get; set; }
    }

    public class IncomingChatRequestBody : ChatRequestBody
    {
        const string DEFAULT_SYSTEM_INSTRUCTION = "You are a helpful assistant. Answer in a friendly, informal tone.";
        const string DEFAULT_MORE_INFO_QUESTION = "Tell me more about that";
        [JsonProperty("user_message")]
        public string UserMessage { get; set; } = DEFAULT_MORE_INFO_QUESTION;
        [JsonProperty("system_instruction")]
        public string SystemInstruction { get; set; } = DEFAULT_SYSTEM_INSTRUCTION;

        public List<ChatMessage> BuildMessages()
        {
            if (Messages.Count == 0)
            {
                Messages.Add(new ChatMessage
                {
                    Role = Role.system,
                    Content = SystemInstruction ?? DEFAULT_SYSTEM_INSTRUCTION
                });
            }

            Messages.Add(new ChatMessage
            {
                Role = Role.user,
                Content = UserMessage ?? DEFAULT_MORE_INFO_QUESTION
            });
            return Messages;
        }
    }

    public class ChatMessage
    {
        [JsonProperty("role")]
        public Role Role { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public class RequestBody
    {
        [JsonProperty(PropertyName = "prompt")]
        public string Prompt { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "max_tokens")]
        public int MaxTokens { get; set; } = 2048;
        [JsonProperty(PropertyName = "temperature")]
        public double Temperature { get; set; } = 0.9d;
        [JsonProperty(PropertyName = "top_p")]
        public double TopP { get; set; } = 1d;
        [JsonProperty(PropertyName = "frequency_penalty")]
        public double FrequencyPenalty { get; set; } = 0d;
        [JsonProperty(PropertyName = "presence_penalty")]
        public double PresencePenalty { get; set; } = 0.6d;
        [JsonProperty(PropertyName = "stop")]
        public string Stop { get; set; } = "None";
    }

    public class IncomingRequestBody : RequestBody
    {

        const string START_SYSTEM_TOKEN = "<|im_start|>system";
        const string START_USER_TOKEN = "<|im_start|>user";
        const string START_ASSISTANT_TOKEN = "<|im_start|>assistant";
        const string END_TOKEN = "<|im_end|>";
        const string DEFAULT_QUESTION = "Tell me more about that";
        const string DEFAULT_SCOPE = @"You are a helpful assistant";

        [JsonProperty(PropertyName = "history")]
        public List<QAPair> History { get; set; } = new List<QAPair>();
        [JsonProperty(PropertyName = "initial_scope")]
        public string InitialScope { get; set; } = DEFAULT_SCOPE;
        [JsonProperty(PropertyName = "max_history_size")]
        public int MaxHistorySize { get; set; } = int.MaxValue;

        public string BuildPrompt()
        {
            var prompt = new StringBuilder();

            prompt.AppendLine($"{START_SYSTEM_TOKEN}\n{InitialScope}\n{END_TOKEN}");

            // take the last {MaxHistorySize} items from the history if there are more than {MaxHistorySize} items
            foreach (var qa in History.Skip(MaxHistorySize > History.Count ? 0 : History.Count - MaxHistorySize))
            {
                prompt.AppendLine($"{START_USER_TOKEN}\n{qa.Question}\n{END_TOKEN}");
                prompt.AppendLine($"{qa.Answer}");
            }

            prompt.AppendLine($"{START_USER_TOKEN}\n{Prompt}\n{END_TOKEN}");
            prompt.AppendLine($"{START_ASSISTANT_TOKEN}");

            return prompt.ToString();
        }
    }

    public class ResponseBody
    {
        public List<QAPair> History { get; set; } = new List<QAPair>();
        public string Answer { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "initial_scope")]
        public string InitialScope { get; set; }

    }

    public class AnswerObject
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "index")]
        public int Index { get; set; }
        [JsonProperty(PropertyName = "finish_reason")]
        public string FinishReason { get; set; }
        [JsonProperty(PropertyName = "logprobs")]
        public string LogProbs { get; set; }
    }

    public class QAPair
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }


    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum Role
    {
        system,
        user,
        assistant
    }
