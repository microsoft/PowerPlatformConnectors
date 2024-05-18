    public class Script : ScriptBase
    {        
        public class ScriptOperation
        {
            public const string RoundUp = "Math_RoundUp";
            public const string Sum = "MathSum";
        }

        public override async Task<HttpResponseMessage> ExecuteAsync()
        {
            var content = await Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            Context.Logger.LogInformation(JsonConvert.SerializeObject(new
            {
                Context.OperationId,
                Input = content
            }));
            try
            {
                var output = await ExecuteAsync(Context.OperationId, content);
                return output;
            }
            catch (Exception ex)
            {
                var error = new
                {
                    Context.OperationId,
                    Input = content,
                    ex.Message                    
                };
                return BuildOutput(error, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<HttpResponseMessage> ExecuteAsync(string operationId, string content)
        {
            return operationId switch
            {
                ScriptOperation.RoundUp => BuildOutput(await HandleRoundUpOperation(content)),
                ScriptOperation.Sum => BuildOutput(await HandleSumOperation(content)),
                _ => BuildOutput($"Unknown operation ID '{this.Context.OperationId}'"),
            };
        }
        #region Math
        /// <summary>
        /// get round up value for input number
        /// </summary>
        /// <returns></returns>
        private async Task<HttpResponseMessage> HandleRoundUpOperation(string content)
        {
            // Parse as JSON object
            var contentAsJson = JObject.Parse(content);
            // Get the value of text to check
            var input = (decimal)contentAsJson["Input"];
            var result = Math.Ceiling(input);
            return BuildOutput(result);
        }
        /// <summary>
        /// sum array values
        /// </summary>
        /// <returns></returns>
        public async Task<double> HandleSumOperation(string content)
        {
            var inputType = new
            {
                Data = "",
                Path = ""
            };
            var input = JsonConvert.DeserializeAnonymousType(content, inputType);
            var dataValue = JToken.Parse(input.Data);
            var datas = dataValue.SelectTokens($"$.{input.Path}").ToList();
            var result = datas.Sum(d => (double)d);
            return result;
        }
        #endregion 
        private HttpResponseMessage BuildOutput(object result, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var output = JsonConvert.SerializeObject(result);
            HttpResponseMessage response = new HttpResponseMessage(statusCode);
            response.Content = CreateJsonContent(output);
            return response;
        }
    }
