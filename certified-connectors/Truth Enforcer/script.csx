    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class Script : ScriptBase
    {
        private static string ToQueryString(System.Collections.Specialized.NameValueCollection nvc)
        {
            return string.Join("&", Array.ConvertAll(nvc.AllKeys, key => $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(nvc[key])}"));
        }

        private static string ComputeSha256Hash(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);

            byte[] hashBytes;
            using (SHA256 sha256 = SHA256.Create())
            {
                hashBytes = sha256.ComputeHash(bytes);
            }

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            string hashString = sb.ToString();

            return hashString;
        }

        public override async Task<HttpResponseMessage> ExecuteAsync()
        {

            if (this.Context.OperationId == "CreateSeal")
            {
                var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
                JObject body = JObject.Parse(contentAsString);
                var hash = ComputeSha256Hash((string)body["file"]);

                string newUrl = $"https://digitalportoftrust.connecting-software.com/api/v2/CreateSeal/1/{hash}";

                var modifiedRequest = new HttpRequestMessage(HttpMethod.Post, newUrl);

                foreach (var header in this.Context.Request.Headers)
                {
                    modifiedRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
                
                modifiedRequest.Content = CreateJsonContent(body.ToString());

                HttpResponseMessage response = await this.Context.SendAsync(modifiedRequest, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            
                return response;
            }

            if (this.Context.OperationId == "ListSeals")
            {
                var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
                JObject body = JObject.Parse(contentAsString);
                var hash = ComputeSha256Hash((string)body["file"]);

                string newUrl = $"https://digitalportoftrust.connecting-software.com/api/v2/GetSeals/{hash}";
            
                var modifiedRequest = new HttpRequestMessage(HttpMethod.Get, newUrl);

                foreach (var header in this.Context.Request.Headers)
                {
                    modifiedRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
                
                HttpResponseMessage response = await this.Context.SendAsync(modifiedRequest, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            
                return response;
            }
            if (this.Context.OperationId == "VerifySeal") 
            {
                var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
                JObject body = JObject.Parse(contentAsString);
                var hash = ComputeSha256Hash((string)body["file"]);
                var sealId = (string)body["sealId"];

                string newUrl = $"https://digitalportoftrust.connecting-software.com/api/v2/VerifySeal/{hash}/{sealId}";
            
                var queryParams = new System.Collections.Specialized.NameValueCollection();
                queryParams["ignorePrivateBlockchains"] = "false";

                UriBuilder uriBuilder = new UriBuilder(newUrl);
                uriBuilder.Query = ToQueryString(queryParams);


                var modifiedRequest = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

                foreach (var header in this.Context.Request.Headers)
                {
                    modifiedRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
                
                HttpResponseMessage response = await this.Context.SendAsync(modifiedRequest, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            
                return response;
            }
            else
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                return response;
            }
        }
    }
