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

            switch (Context.OperationId)
            {
                case "BuildCompletionPrompt":
                    return await ProcessBuildCompletionPrompt().ConfigureAwait(false);
                    break;
                case "GetCompletionHistoryAndAnswerFromResponse":
                    return await ProcessGetCompletionHistoryAndAnswerFromResponse().ConfigureAwait(false);
                    break;
                case "GetChatCompletionHistoryAndAnswerFromResponse":
                    return await ProcessGetChatCompletionMessageAndAnswerFromResponse().ConfigureAwait(false);
                default:
                    break;
            }
            // Handle an invalid operation ID
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = CreateJsonContent($"Unknown operation ID '{this.Context.OperationId}'");
            return response;
        }

        // returns the ChatCompletions message and extracted answer from the raw chat completions result
        // Extracts the assistant:content pair (message) as well as the content on it own (answer)
        // from a raw chat completion response
        private async Task<HttpResponseMessage> ProcessGetChatCompletionMessageAndAnswerFromResponse()
        {
            var body = JsonConvert.DeserializeObject<RawChatResult>(await Context.Request.Content.ReadAsStringAsync());
            if (body == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = CreateJsonContent(JsonConvert.SerializeObject(new { message = "error parsing body" })) };
            }

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = CreateJsonContent(
                    JsonConvert.SerializeObject(
                        new
                        {
                            message = body.Choices?[0]?.Message,
                            answer = body.Choices?[0]?.Message.Content ?? "NO MATCH"
                        }
                    )
                )
            };

        }

        // returns the CompletionHistory and extracted answer from the raw completions result
        // Given a prompt and a response from the CreateCompletion operation, get
        // the qa pair to add to the history and the answer on its own
        private async Task<HttpResponseMessage> ProcessGetCompletionHistoryAndAnswerFromResponse()
        {
            var body = JsonConvert.DeserializeObject<CompletionQuestionAndResponse>(await Context.Request.Content.ReadAsStringAsync());
            if (body == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = CreateJsonContent(JsonConvert.SerializeObject(new { message = "error parsing body" })) };
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = CreateJsonContent(
                    JsonConvert.SerializeObject(
                        new
                        {
                            history = new QAPair { Question = body.Question, Answer = body.CompletionResponse.Choices[0]?.Text ?? "NO MATCH" },
                            answer = body.CompletionResponse.Choices[0]?.Text ?? "NO MATCH"
                        }
                    )
                )
            };
        }

        // Given a Question/Answer history, and system Instruction, and a new question, build a prompt in ChatML
        private async Task<HttpResponseMessage> ProcessBuildCompletionPrompt()
        {
            var body = JsonConvert.DeserializeObject<CompletionHistoryQuestionAndSystemInstruction>(await Context.Request.Content.ReadAsStringAsync());
            if (body == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = CreateJsonContent(JsonConvert.SerializeObject(new { message = "error parsing body" })) };
            }

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = CreateJsonContent(JsonConvert.SerializeObject(new { prompt = body.BuildPrompt() })) };
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

    public class ChatMessage
    {
        [JsonProperty("role")]
        public Role Role { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public class CompletionHistoryQuestionAndSystemInstruction
    {

        const string START_SYSTEM_TOKEN = "<|im_start|>system";
        const string START_USER_TOKEN = "<|im_start|>user";
        const string START_ASSISTANT_TOKEN = "<|im_start|>assistant";
        const string END_TOKEN = "<|im_end|>";
        const string DEFAULT_QUESTION = "Tell me more about that";
        const string DEFAULT_SCOPE = @"You are a helpful assistant";

        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; } = DEFAULT_QUESTION;
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

            prompt.AppendLine($"{START_USER_TOKEN}\n{Question}\n{END_TOKEN}");
            prompt.AppendLine($"{START_ASSISTANT_TOKEN}");

            return prompt.ToString();
        }
    }


    public class Choice
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

    public class CompletionResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "object")]
        public string Object { get; set; }
        [JsonProperty(PropertyName = "created")]
        public int Created { get; set; }
        [JsonProperty(PropertyName = "model")]
        public string Model { get; set; }
        [JsonProperty(PropertyName = "choices")]
        public List<Choice> Choices { get; set; }
    }

    public class CompletionQuestionAndResponse
    {
        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; }
        [JsonProperty(PropertyName = "completionResponse")]
        public CompletionResponse CompletionResponse { get; set; }
    }


    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum Role
    {
        system,
        user,
        assistant
    }
