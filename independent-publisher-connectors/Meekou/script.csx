    #region Full Script
    using Newtonsoft.Json.Serialization;
    public class Script : ScriptBase
    {
        #region Common
        public class ScriptOperation
        {
            public const string GetVersion = "GetVersion";
            public const string RoundUp = "RoundUp";
            public const string Sum = "Sum";
            public const string ImageInfo = "ImageInfo";
            public const string Regex = "Regex";
        }
        [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
        public class OutputBase
        {

        }
        #endregion

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
                ScriptOperation.ImageInfo => await ImageInfo(BuildInput<ImageInfoInput>(content)),
                ScriptOperation.GetVersion => await GetVersion(),
                ScriptOperation.Regex => await Regex(BuildInput<RegexInput>(content)),
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
        #region ImageInfo
        public class ImageInfoInput
        {
            public string ImageBase64 { get; set; }
        }
        [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
        public class ImageInfoOutput: OutputBase
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public long Length { get; set; }
        }
        /// <summary>
        /// get image info from image base64
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ImageInfoOutput> ImageInfo(ImageInfoInput input)
        {
            var imageBase64 = input.ImageBase64.Split(',').LastOrDefault();
            // Decode base64 string to byte array
            byte[] imageBytes = Convert.FromBase64String(imageBase64);

            // Create an image from the byte array
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                using (Image image = Image.FromStream(ms))
                {
                    // Get image dimensions
                    int width = image.Width;
                    int height = image.Height;
                    // Get image size in bytes
                    long sizeInBytes = imageBytes.Length;
                    return new ImageInfoOutput
                    {
                        Width = width,
                        Height = height,
                        Length = sizeInBytes
                    };
                }
            }
        }
        #endregion
        #region Regex
        public class RegexInput
        {
            public string Content { get; set; }
            public string Pattern { get; set; }
        }
        public async Task<List<string>> Regex(RegexInput input)
        {
            // Create a Regex object with the provided pattern
            Regex regex = new Regex(input.Pattern);

            // Find all matches in the input string
            MatchCollection matches = regex.Matches(input.Content);

            // Create a list to store all the matched strings
            List<string> matchResults = new List<string>();

            // Iterate through all the matches and add them to the list
            foreach (Match match in matches)
            {
                matchResults.Add(match.Value);
            }

            return matchResults;
        }
        #endregion

        /// <summary>
        /// return the current version
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetVersion()
        {
            return "2024.05.28";
        }
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
    #endregion
