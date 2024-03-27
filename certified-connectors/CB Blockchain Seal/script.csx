    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class Script : ScriptBase
    {
        private static string ComputeSha256Hash(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);

            // Compute SHA256 hash
            byte[] hashBytes;
            using (SHA256 sha256 = SHA256.Create())
            {
                hashBytes = sha256.ComputeHash(bytes);
            }

            // Convert hash to string
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2")); // Convert byte to hexadecimal format
            }
            string hashString = sb.ToString();

            return hashString;
        }

        public override async Task<HttpResponseMessage> ExecuteAsync()
        {
            if (
                this.Context.OperationId == "CreateSeal" || 
                this.Context.OperationId == "GetSeals" ||
                this.Context.OperationId == "VerifySeal")
            {
                var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);

                JObject body = JObject.Parse(contentAsString);

                body["applicationId"] = (string)body["authorizationId"];
                body["hash"] = ComputeSha256Hash((string)body["file"]);
                body.Remove("file");
                
                this.Context.Request.Content = CreateJsonContent(body.ToString());

                HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

                return response;
            }
            else
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                return response;
            }
        }
    }
