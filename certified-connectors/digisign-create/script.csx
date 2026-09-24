// Developer: Lukáš Toman (https://github.com/toman-lukas85)
// Author: Lukáš Toman (https://github.com/toman-lukas85)
// Description: Custom code for Digisign custom connectors (Create, Get) to handle Auth-Token exchange and Base64 unified file uploads.

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        CancellationToken cancellationToken = this.CancellationToken;

        // 1. Extract credentials and environment from custom headers or Basic Auth
        string accessKey = null;
        string secretKey = null;
        string targetEnvironment = null;

        // Custom headers (triple API keys mapping)
        if (this.Context.Request.Headers.TryGetValues("x-access-key", out var accessKeyValues))
        {
            foreach (var val in accessKeyValues) { accessKey = val; break; }
        }
        if (this.Context.Request.Headers.TryGetValues("x-secret-key", out var secretKeyValues))
        {
            foreach (var val in secretKeyValues) { secretKey = val; break; }
        }
        if (this.Context.Request.Headers.TryGetValues("x-environment", out var envValues))
        {
            foreach (var val in envValues) { targetEnvironment = val; break; }
        }

        // Clean up temporary custom headers
        this.Context.Request.Headers.Remove("x-access-key");
        this.Context.Request.Headers.Remove("x-secret-key");
        this.Context.Request.Headers.Remove("x-environment");

        // Fallback to Basic Auth header decoding if custom headers were not present
        if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
        {
            var authHeader = this.Context.Request.Headers.Authorization;
            if (authHeader != null && authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    byte[] credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                    string[] credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                    accessKey = credentials[0];
                    secretKey = credentials[1];
                }
                catch (Exception ex)
                {
                    this.Context.Logger.LogWarning($"[Digisign Custom Code] Failed to decode Basic auth header: {ex.Message}");
                }
            }
        }

        // 2. Rewrite Request URI (default to production if no custom environment specified)
        if (string.IsNullOrEmpty(targetEnvironment) || targetEnvironment.StartsWith("@connectionParameters", StringComparison.OrdinalIgnoreCase))
        {
            targetEnvironment = "https://api.digisign.org";
        }

        targetEnvironment = targetEnvironment.Trim().TrimEnd('/');
        try
        {
            var envUri = new Uri(targetEnvironment);
            var builder = new UriBuilder(this.Context.Request.RequestUri)
            {
                Scheme = envUri.Scheme,
                Host = envUri.Host,
                Port = envUri.Port
            };
            this.Context.Request.RequestUri = builder.Uri;
        }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = new StringContent($"Digisign Custom Code: Invalid Environment URL '{targetEnvironment}': {ex.Message}", Encoding.UTF8, "text/plain");
                return errorResponse;
            }

        string requestUriString = this.Context.Request.RequestUri.ToString();
        string baseUrl = this.Context.Request.RequestUri.GetLeftPart(UriPartial.Authority);

        this.Context.Logger.LogInformation($"[Digisign Custom Code] Target Request URI: {requestUriString}");

        // If this is a direct call to exchange keys, just forward the request
        if (requestUriString.EndsWith("/api/auth-token", StringComparison.OrdinalIgnoreCase))
        {
            return await this.Context.SendAsync(this.Context.Request, cancellationToken);
        }

        // 3. Exchange AccessKey and SecretKey for Bearer Token
        string bearerToken = null;
        if (!string.IsNullOrEmpty(accessKey) && !string.IsNullOrEmpty(secretKey))
        {
            try
            {
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(baseUrl + "/api/auth-token"));
                var tokenBody = new JObject
                {
                    ["accessKey"] = accessKey,
                    ["secretKey"] = secretKey
                };
                
                tokenRequest.Content = new StringContent(tokenBody.ToString(), Encoding.UTF8, "application/json");
                
                HttpResponseMessage tokenResponse = await this.Context.SendAsync(tokenRequest, cancellationToken);
                
                if (tokenResponse.IsSuccessStatusCode)
                {
                    string responseContent = await tokenResponse.Content.ReadAsStringAsync();
                    var tokenJson = JObject.Parse(responseContent);
                    bearerToken = tokenJson["token"]?.ToString();
                }
                else
                {
                    var errorResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    errorResponse.Content = new StringContent("Digisign: Token exchange failed. Verify accessKey and secretKey.", Encoding.UTF8, "text/plain");
                    return errorResponse;
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                errorResponse.Content = new StringContent($"Digisign Auth Error: {ex.Message}", Encoding.UTF8, "text/plain");
                return errorResponse;
            }
        }

        // Set the Authorization Bearer header for the request
        if (!string.IsNullOrEmpty(bearerToken))
        {
            this.Context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }

        // 4. Intercept virtual endpoint for unified file upload
        if (this.Context.Request.Method == HttpMethod.Post && 
            requestUriString.Contains("/documents/unified-upload"))
        {
            return await HandleUnifiedUploadAsync(baseUrl, bearerToken, cancellationToken);
        }

        // 5. Intercept virtual endpoints for role-specific recipients to strip the role suffix from the URL path
        if (this.Context.Request.Method == HttpMethod.Post &&
            (requestUriString.Contains("/recipients/signer") ||
             requestUriString.Contains("/recipients/approver") ||
             requestUriString.Contains("/recipients/cc")))
        {
            var currentUriStr = this.Context.Request.RequestUri.ToString();
            int lastSlashIndex = currentUriStr.LastIndexOf('/');
            if (lastSlashIndex > 0)
            {
                string cleanUriStr = currentUriStr.Substring(0, lastSlashIndex);
                this.Context.Request.RequestUri = new Uri(cleanUriStr);
            }

            // Normalise identificationNumber to 8 digits if it is numeric and shorter
            try
            {
                string reqContent = await this.Context.Request.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(reqContent))
                {
                    var payload = JObject.Parse(reqContent);
                    var icoToken = payload["identificationNumber"];
                    if (icoToken != null && icoToken.Type == JTokenType.String)
                    {
                        string ico = icoToken.ToString().Trim();
                        if (!string.IsNullOrEmpty(ico) && ico.Length < 8)
                        {
                            bool isNumeric = true;
                            foreach (char c in ico)
                            {
                                if (!char.IsDigit(c)) { isNumeric = false; break; }
                            }
                            if (isNumeric)
                            {
                                payload["identificationNumber"] = ico.PadLeft(8, '0');
                                this.Context.Request.Content = new StringContent(payload.ToString(Newtonsoft.Json.Formatting.None), Encoding.UTF8, "application/json");
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignore parsing failures and proceed with original content
            }
        }

        // 6. Intercept virtual endpoint for Universal API Call
        if (requestUriString.Contains("/api/call-api"))
        {
            return await HandleUniversalCallAsync(baseUrl, bearerToken, cancellationToken);
        }

        // Forward standard requests with the Bearer token
        return await this.Context.SendAsync(this.Context.Request, cancellationToken);
    }

    private async Task<HttpResponseMessage> HandleUnifiedUploadAsync(string baseUrl, string token, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Read and parse the JSON payload from Power Automate
            string jsonString = await this.Context.Request.Content.ReadAsStringAsync();
            var payload = JObject.Parse(jsonString);

            // Extract the envelope ID from the URI path
            string requestPath = this.Context.Request.RequestUri.AbsolutePath; // e.g. "/api/envelopes/123/documents/unified-upload"
            string[] segments = requestPath.Split('/');
            string envelopeId = "";
            for (int i = 0; i < segments.Length; i++)
            {
                if (segments[i].Equals("envelopes", StringComparison.OrdinalIgnoreCase) && i + 1 < segments.Length)
                {
                    envelopeId = segments[i + 1];
                    break;
                }
            }

            if (string.IsNullOrEmpty(envelopeId))
            {
                var badRequest = new HttpResponseMessage(HttpStatusCode.BadRequest);
                badRequest.Content = new StringContent("Envelope ID not found in URL path.", Encoding.UTF8, "text/plain");
                return badRequest;
            }

            string fileName = payload["fileName"]?.ToString();
            string fileContentBase64 = null;
            if (payload["fileContent"] != null)
            {
                if (payload["fileContent"].Type == JTokenType.Object)
                {
                    fileContentBase64 = payload["fileContent"]["$content"]?.ToString();
                }
                else
                {
                    fileContentBase64 = payload["fileContent"].ToString();
                }
            }
            int position = payload["position"]?.Value<int>() ?? 0;
            string metadata = payload["metadata"]?.ToString();
            string labelPositioning = payload["labelPositioning"]?.ToString() ?? "none";
            int labelPositionX = payload["labelPositionX"]?.Value<int>() ?? 0;
            int labelPositionY = payload["labelPositionY"]?.Value<int>() ?? 0;

            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileContentBase64))
            {
                var badRequest = new HttpResponseMessage(HttpStatusCode.BadRequest);
                badRequest.Content = new StringContent("Missing required parameters: 'fileName' or 'fileContent'.", Encoding.UTF8, "text/plain");
                return badRequest;
            }

            // Sanitize filename to ASCII (strip Czech diacritics and spaces) to prevent header encoding issues
            if (!string.IsNullOrEmpty(fileName))
            {
                var normalizedString = fileName.Normalize(NormalizationForm.FormD);
                var stringBuilder = new StringBuilder();
                foreach (var c in normalizedString)
                {
                    var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                    if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                    {
                        if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '.' || c == '-' || c == '_')
                        {
                            stringBuilder.Append(c);
                        }
                        else if (c == ' ')
                        {
                            stringBuilder.Append('_');
                        }
                    }
                }
                fileName = stringBuilder.ToString();
            }

            // Clean the base64 string defensively (strip data URI prefix and non-base64 characters)
            int commaIndex = fileContentBase64.IndexOf(',');
            if (commaIndex >= 0)
            {
                fileContentBase64 = fileContentBase64.Substring(commaIndex + 1);
            }

            var sbBase64 = new StringBuilder();
            foreach (char c in fileContentBase64)
            {
                if ((c >= 'A' && c <= 'Z') || 
                    (c >= 'a' && c <= 'z') || 
                    (c >= '0' && c <= '9') || 
                    c == '+' || c == '/' || c == '=')
                {
                    sbBase64.Append(c);
                }
            }
            fileContentBase64 = sbBase64.ToString();

            // Decode Base64 data into a byte array with detailed debug output on error
            byte[] fileBytes;
            try
            {
                fileBytes = Convert.FromBase64String(fileContentBase64);
            }
            catch (Exception ex)
            {
                var badRequest = new HttpResponseMessage(HttpStatusCode.BadRequest);
                string sampleStart = fileContentBase64.Length > 100 ? fileContentBase64.Substring(0, 100) : fileContentBase64;
                string sampleEnd = fileContentBase64.Length > 100 ? fileContentBase64.Substring(fileContentBase64.Length - 100) : "";
                badRequest.Content = new StringContent($"Base64 Decoding Failed. Length: {fileContentBase64.Length}. Error: {ex.Message}. Sample Start: '{sampleStart}', Sample End: '{sampleEnd}'", Encoding.UTF8, "text/plain");
                return badRequest;
            }

            // --- STEP A: Upload the file to /api/files ---
            var uploadRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(baseUrl + "/api/files"));
            uploadRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Build MultipartFormDataContent
            var multipartContent = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(fileBytes);
            
            // Detect content type (simplified for PDF, or others)
            string contentType = "application/pdf";
            if (fileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase)) contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            else if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)) contentType = "image/png";
            else if (fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)) contentType = "image/jpeg";
            
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
            
            // Digisign standardly expects the file in the "file" parameter
            multipartContent.Add(fileContent, "file", fileName);
            uploadRequest.Content = multipartContent;

            HttpResponseMessage uploadResponse = await this.Context.SendAsync(uploadRequest, cancellationToken);
            string uploadResponseString = await uploadResponse.Content.ReadAsStringAsync();
            
            // Log file upload result
            this.Context.Logger.LogInformation($"[Digisign Custom Code] Upload File HTTP Status: {uploadResponse.StatusCode}");
            this.Context.Logger.LogInformation($"[Digisign Custom Code] Upload File Response Body: {uploadResponseString}");

            if (!uploadResponse.IsSuccessStatusCode)
            {
                JToken parsedUploadResponse;
                try
                {
                    parsedUploadResponse = JToken.Parse(uploadResponseString);
                }
                catch
                {
                    parsedUploadResponse = uploadResponseString;
                }

                var errorObj = new JObject
                {
                    ["error"] = "UnifiedUpload_UploadStepFailed",
                    ["uploadStatusCode"] = (int)uploadResponse.StatusCode,
                    ["uploadStatus"] = uploadResponse.StatusCode.ToString(),
                    ["fileBytesLength"] = fileBytes.Length,
                    ["detectedContentType"] = contentType,
                    ["uploadResponseBody"] = parsedUploadResponse,
                    ["envelopeId"] = envelopeId,
                    ["requestPath"] = requestPath
                };
                
                var customResponse = new HttpResponseMessage(uploadResponse.StatusCode);
                customResponse.Content = new StringContent(errorObj.ToString(Newtonsoft.Json.Formatting.None), Encoding.UTF8, "application/json");
                return customResponse;
            }

            var uploadJson = JObject.Parse(uploadResponseString);
            string fileId = uploadJson["id"]?.ToString(); // Expected file ID field in the response

            if (string.IsNullOrEmpty(fileId))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                errorResponse.Content = new StringContent("Digisign returned a successful upload but no 'id' was found in the response.", Encoding.UTF8, "text/plain");
                return errorResponse;
            }

            // Get file IRI format safely (checking @id, links structure, and falling back)
            string fileIri = null;
            if (uploadJson["@id"] != null)
            {
                fileIri = uploadJson["@id"].ToString();
            }
            else if (uploadJson["_links"]?["self"] != null)
            {
                var selfToken = uploadJson["_links"]["self"];
                var selfObj = selfToken as JObject;
                if (selfObj != null && selfObj["href"] != null)
                {
                    fileIri = selfObj["href"].ToString();
                }
                else
                {
                    string selfStr = selfToken.ToString().Trim();
                    if (!selfStr.StartsWith("{"))
                    {
                        fileIri = selfStr;
                    }
                }
            }

            if (string.IsNullOrEmpty(fileIri))
            {
                fileIri = $"/api/files/{fileId}";
            }

            // --- STEP B: Link the file to the envelope via /api/envelopes/{envelopeId}/documents ---
            var linkRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(baseUrl + $"/api/envelopes/{envelopeId}/documents"));
            linkRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var linkBody = new JObject
            {
                ["file"] = fileIri,
                ["name"] = fileName,
                ["labelPositioning"] = labelPositioning,
                ["labelPositionX"] = labelPositionX,
                ["labelPositionY"] = labelPositionY
            };

            if (payload["position"] != null && payload["position"].Type != JTokenType.Null)
            {
                linkBody["position"] = payload["position"].Value<int>();
            }
            
            if (!string.IsNullOrEmpty(metadata))
            {
                linkBody["metadata"] = metadata;
            }

            linkRequest.Content = new StringContent(linkBody.ToString(), Encoding.UTF8, "application/json");
            
            HttpResponseMessage linkResponse = await this.Context.SendAsync(linkRequest, cancellationToken);
            string linkResponseString = await linkResponse.Content.ReadAsStringAsync();

            if (!linkResponse.IsSuccessStatusCode)
            {
                JToken parsedLinkResponse;
                try
                {
                    parsedLinkResponse = JToken.Parse(linkResponseString);
                }
                catch
                {
                    parsedLinkResponse = linkResponseString;
                }

                var errorObj = new JObject
                {
                    ["error"] = "UnifiedUpload_LinkStepFailed",
                    ["linkStatusCode"] = (int)linkResponse.StatusCode,
                    ["linkStatus"] = linkResponse.StatusCode.ToString(),
                    ["uploadStatusCode"] = (int)uploadResponse.StatusCode,
                    ["uploadResponseBody"] = uploadJson,
                    ["linkRequestBody"] = linkBody,
                    ["linkResponseBody"] = parsedLinkResponse,
                    ["envelopeId"] = envelopeId,
                    ["requestPath"] = requestPath
                };
                
                var customResponse = new HttpResponseMessage(linkResponse.StatusCode);
                customResponse.Content = new StringContent(errorObj.ToString(Newtonsoft.Json.Formatting.None), Encoding.UTF8, "application/json");
                return customResponse;
            }
            
            // Add custom debug headers to inspect in the Power Platform Test tab
            linkResponse.Headers.Add("X-Debug-Upload-Status", "Success_201");
            linkResponse.Headers.Add("X-Debug-Link-Status", "Success_201");
            
            return linkResponse; // Return the result of the second step (linking)
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = new StringContent($"Error in custom file upload process: {ex.Message}", Encoding.UTF8, "text/plain");
            return errorResponse;
        }
    }

    private async Task<HttpResponseMessage> HandleUniversalCallAsync(string baseUrl, string token, CancellationToken cancellationToken)
    {
        try
        {
            string jsonString = await this.Context.Request.Content.ReadAsStringAsync();
            var payload = JObject.Parse(jsonString);

            string methodStr = payload["method"]?.ToString()?.ToUpper() ?? "GET";
            
            // Restrict methods to only GET and POST (GET is preserved only for baseline functionality/retrieval checks)
            if (methodStr != "GET" && methodStr != "POST")
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
                errorResponse.Content = new StringContent($"Method '{methodStr}' is not allowed in this connector. Only GET and POST are supported.", Encoding.UTF8, "text/plain");
                return errorResponse;
            }

            string relativePath = payload["path"]?.ToString() ?? "";
            string queryString = payload["queryString"]?.ToString() ?? "";
            JToken bodyToken = payload["body"];

            // Ensure relative path starts with a slash and does not duplicate /api if already present
            if (!relativePath.StartsWith("/")) relativePath = "/" + relativePath;

            // Construct target Uri
            string targetUrl = baseUrl.TrimEnd('/') + relativePath;
            if (!string.IsNullOrEmpty(queryString))
            {
                targetUrl += (queryString.StartsWith("?") ? "" : "?") + queryString;
            }

            var request = new HttpRequestMessage(new HttpMethod(methodStr), new Uri(targetUrl));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (bodyToken != null && bodyToken.Type != JTokenType.Null && methodStr != "GET" && methodStr != "DELETE")
            {
                request.Content = new StringContent(bodyToken.ToString(Newtonsoft.Json.Formatting.None), Encoding.UTF8, "application/json");
            }

            return await this.Context.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = new StringContent($"Error in Universal API Call: {ex.Message}", Encoding.UTF8, "text/plain");
            return errorResponse;
        }
    }
}
