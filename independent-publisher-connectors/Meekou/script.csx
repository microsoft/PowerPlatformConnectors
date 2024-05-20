    public class Script : ScriptBase
    {        
        public class ScriptOperation
        {
            public const string RoundUp = "RoundUp";
            public const string Sum = "Sum";
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
                var result = await ExecuteAsync(Context.OperationId, content);
                return BuildOutput(result);
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
        public async Task<object> ExecuteAsync(string operationId, string content)
        {
            return operationId switch
            {
                ScriptOperation.Sum => await Sum(BuildInput<SumInput>(content)),
                ScriptOperation.RoundUp => await RoundUp(BuildInput<RoundUpInput>(content)),
                _ => $"Unknown operation ID '{operationId}'",
            };            
        }
        #region Sum
        /// <summary>
        /// input for sum
        /// </summary>
        public class SumInput
        {
            /// <summary>
            /// data in json format
            /// </summary>
            public string Data { get; set; }
            /// <summary>
            /// path for value to sum
            /// </summary>
            public string Path { get; set; }
        }
        /// <summary>
        /// sum array values
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> Sum(SumInput input)
        {
            var dataValue = JToken.Parse(input.Data);
            var datas = dataValue.SelectTokens($"$.{input.Path}").ToList();
            var result = datas.Sum(d => (decimal)d);
            return result;
        }
        #endregion
        #region RoundUp
        public class RoundUpInput
        {
            public decimal Value { get; set; }
        }
        /// <summary>
        /// get round up value for input number
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> RoundUp(RoundUpInput input)
        {
            var result = Math.Ceiling(input.Value);
            return result;
        }        
        #endregion 
        private T BuildInput<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input);
        }
        private HttpResponseMessage BuildOutput(object result, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var output = JsonConvert.SerializeObject(result);
            HttpResponseMessage response = new HttpResponseMessage(statusCode);
            response.Content = CreateJsonContent(output);
            return response;
        }
    }
