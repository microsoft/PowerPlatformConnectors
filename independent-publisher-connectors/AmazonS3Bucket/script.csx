public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var logger = this.Context.Logger;

        string authHeader = string.Empty;
        // Retrieve the Authorization header from the request
        if (this.Context.Request.Headers.TryGetValues("Authorization", out IEnumerable<string> authHeaders))
        {
            authHeader = authHeaders.FirstOrDefault();
        }
        
        // Check if the Authorization header is present and starts with "Basic "
        if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Basic "))
        {
            // Extract the base64 encoded credentials and decode them
            string encodedCredentials = authHeader.Substring(6);
            byte[] decodedBytes = Convert.FromBase64String(encodedCredentials);
            string decodedCredentials = Encoding.UTF8.GetString(decodedBytes);
            
            // Split credentials into username and password
            string[] credentials = decodedCredentials.Split(':');
            string accessKeyId = credentials[0];
            string accessKeySecret = credentials[1];
            
            var requestUri = this.Context.Request.RequestUri;
            int awsIndex = requestUri.AbsolutePath.IndexOf("/aws/", StringComparison.OrdinalIgnoreCase);
            string pathAfterAws = requestUri.AbsolutePath.Substring(awsIndex + 5); // +5 to skip "/aws/"

            var path = pathAfterAws.Split('/');
            string service    = path.Skip(0).FirstOrDefault(); // Service;
            string region     = path.Skip(1).FirstOrDefault(); // Region;
            string objectName = path.Skip(2).FirstOrDefault(); // Object Name;
            string objectKey  = path.Skip(3).FirstOrDefault(); // Object Key;
            
            // Object key can include / in the name, so we need to decode it
            if (! string.IsNullOrEmpty(objectKey))
            {
                objectKey = System.Web.HttpUtility.UrlDecode(objectKey);
            }

            logger.LogInformation($"Service:     {(service ?? "---")}");
            logger.LogInformation($"Region:      {(region ?? "---")}");
            logger.LogInformation($"Object Name: {(objectName ?? "---")}");
            logger.LogInformation($"Object Key:  {(objectKey  ?? "---")}");
            
            var regionUrlPart = string.Empty;
            if (!string.IsNullOrEmpty(region))
            {
                if (!region.Equals("us-east-1", StringComparison.OrdinalIgnoreCase))
                    regionUrlPart = string.Format("-{0}", region);
            }

            var endpointUri = string.Format("https://{2}.{0}{1}.amazonaws.com",
                                                service,
                                                regionUrlPart,
                                                objectName);
            if (! string.IsNullOrEmpty(objectKey)) {
                endpointUri = string.Format("{0}/{1}", endpointUri, objectKey);
            }  
            var uri = new Uri($"{endpointUri}{requestUri.Query}");
            
            var request = new System.Net.Http.HttpRequestMessage(this.Context.Request.Method, uri)
            {
                Content = this.Context.Request.Content
            };
            SignRequest(request, service, region, accessKeyId, accessKeySecret);
            logger.LogInformation($"Call: {request.Method} {request.RequestUri!}");
            
            // Use the context to forward/send an HTTP request
            var response = await this.Context.SendAsync(request,
                this.CancellationToken
            ).ConfigureAwait(continueOnCapturedContext: false);

            // Transform the XML Result to JSON
            if (string.Equals(this.Context.OperationId, "list-objects-s3", StringComparison.OrdinalIgnoreCase))
            {
                return await this.HandleTransformXML2Json(response).ConfigureAwait(false);
            }

            return response;
        }
        else 
            throw new Exception("Unexpected Authentication");
    }

    private async Task<HttpResponseMessage> HandleTransformXML2Json(HttpResponseMessage response)
    {
        // Do the transformation if the response was successful, otherwise return error responses as-is
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            
            var doc = new XmlDocument();
            doc.LoadXml(responseString);
            
            var jsonContent = JsonConvert.SerializeXmlNode(doc);

            // ListBucketResult.Contents is returned as a single object and not as array, when only one object is returned
            var content = JObject.Parse(jsonContent);
            var listBucketResult = content["ListBucketResult"];
            var contents         = listBucketResult["Contents"];
            if (contents is JObject)
            {
                listBucketResult["Contents"] = new JArray(contents);
                jsonContent = JsonConvert.SerializeObject(content);
            }            
            response.Content = CreateJsonContent(jsonContent.ToString());
        }

        return response;
    }

    /// <summary>
    /// Sign the request with the given credentials (awsAccessKey and awsSecretKey).
    /// </summary>
    /// <param name="request">The request to sign</param>
    /// <param name="service">The service (should be S3)</param>
    /// <param name="region">The region of the service.</param>
    /// <param name="awsAccessKey">The AWS account access key id</param>
    /// <param name="awsSecretKey">The AWS account access key secret</param>
    /// <returns></returns>
    public static async Task SignRequest(HttpRequestMessage request,
                                string service,
                                string region,
                                string awsAccessKey,
                                string awsSecretKey)            
    {
        var signer = new AWS4SignerForAuthorizationHeader
            {
                EndpointUri = request.RequestUri!,
                HttpMethod = request.Method.Method.ToUpper(),
                Service = service,
                Region = region,
            };
        
        string queryParameters = request.RequestUri.Query?.TrimStart('?') ?? "";
        string bodyHash = string.Empty;
        if (request.Content is not null) {
            var bytes = await request.Content.ReadAsByteArrayAsync();
            var contentHash = AWS4SignerBase.CanonicalRequestHashAlgorithm.ComputeHash(bytes);
            bodyHash = AWS4SignerBase.ToHexString(contentHash, true);                
        } else {
            bodyHash = AWS4SignerBase.EMPTY_BODY_SHA256;
        }
        
        var headers = new Dictionary<string, string>();
        if (request.Method == HttpMethod.Put || request.Method == HttpMethod.Post) {                
            headers.Add(AWS4SignerBase.X_Amz_Content_SHA256, bodyHash);
            
            foreach(var header in request.Content?.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>()) {
                headers.Add(header.Key, header.Value.First());
            }
        } else {
            headers.Add(AWS4SignerBase.X_Amz_Content_SHA256, bodyHash);                
        }

        string signature = signer.ComputeSignature(headers, queryParameters, bodyHash, awsAccessKey, awsSecretKey);
        
        foreach (var header in headers.Keys)
        {
            // not all headers can be set via the dictionary
            if (header.Equals("host", StringComparison.OrdinalIgnoreCase))
                continue;
            else if (header.Equals("content-length", StringComparison.OrdinalIgnoreCase))
                continue;
            else if (header.Equals("content-type", StringComparison.OrdinalIgnoreCase))
                continue;
            else 
                request.Headers.TryAddWithoutValidation(header, headers[header]);
        }
        
        // Add the authorization header
        request.Headers.TryAddWithoutValidation("Authorization", signature);
    }

    # region AWS 4 Signer

    /// <summary>
    /// Various Http helper routines
    /// </summary>
    internal static class HttpHelpers
    {
        /// <summary>
        /// Helper routine to url encode canonicalized header names and values for safe
        /// inclusion in the presigned url.
        /// </summary>
        /// <param name="data">The string to encode</param>
        /// <param name="isPath">Whether the string is a URL path or not</param>
        /// <returns>The encoded string</returns>
        public static string UrlEncode(string data, bool isPath = false)
        {
            // The Set of accepted and valid Url characters per RFC3986. Characters outside of this set will be encoded.
            const string validUrlCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

            var encoded = new System.Text.StringBuilder(data.Length * 2);
            string unreservedChars = String.Concat(validUrlCharacters, (isPath ? "/:" : ""));

            foreach (char symbol in System.Text.Encoding.UTF8.GetBytes(data))
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                    encoded.Append(symbol);
                else
                    encoded.Append("%").Append(String.Format("{0:X2}", (int)symbol));
            }

            return encoded.ToString();
        }
    }
    
    /// <summary>
    /// Common methods and properties for all AWS4 signer variants
    /// </summary>
    internal abstract class AWS4SignerBase
    {
        // SHA256 hash of an empty request body
        public const string EMPTY_BODY_SHA256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

        public const string SCHEME = "AWS4";
        public const string ALGORITHM = "HMAC-SHA256";
        public const string TERMINATOR = "aws4_request";

        // format strings for the date/time and date stamps required during signing
        public const string ISO8601BasicFormat = "yyyyMMddTHHmmssZ";
        public const string DateStringFormat = "yyyyMMdd";

        // some common x-amz-* parameters
        public const string X_Amz_Algorithm = "X-Amz-Algorithm";
        public const string X_Amz_Credential = "X-Amz-Credential";
        public const string X_Amz_SignedHeaders = "X-Amz-SignedHeaders";
        public const string X_Amz_Date = "X-Amz-Date";
        public const string X_Amz_Signature = "X-Amz-Signature";
        public const string X_Amz_Expires = "X-Amz-Expires";
        public const string X_Amz_Content_SHA256 = "X-Amz-Content-SHA256";
        public const string X_Amz_Decoded_Content_Length = "X-Amz-Decoded-Content-Length";
        public const string X_Amz_Meta_UUID = "X-Amz-Meta-UUID";

        // the name of the keyed hash algorithm used in signing
        public const string HMACSHA256 = "HMACSHA256";

        // request canonicalization requires multiple whitespace compression
        protected static readonly System.Text.RegularExpressions.Regex CompressWhitespaceRegex = new System.Text.RegularExpressions.Regex("\\s+");

        // algorithm used to hash the canonical request that is supplied to
        // the signature computation
        public static System.Security.Cryptography.HashAlgorithm CanonicalRequestHashAlgorithm = System.Security.Cryptography.HashAlgorithm.Create("SHA-256");

        /// <summary>
        /// The service endpoint, including the path to any resource.
        /// </summary>
        public Uri EndpointUri { get; set; }

        /// <summary>
        /// The HTTP verb for the request, e.g. GET.
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// The signing name of the service, e.g. 's3'.
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// The system name of the AWS region associated with the endpoint, e.g. us-east-1.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Returns the canonical collection of header names that will be included in
        /// the signature. For AWS4, all header names must be included in the process 
        /// in sorted canonicalized order.
        /// </summary>
        /// <param name="headers">
        /// The set of header names and values that will be sent with the request
        /// </param>
        /// <returns>
        /// The set of header names canonicalized to a flattened, ;-delimited string
        /// </returns>
        protected string CanonicalizeHeaderNames(IDictionary<string, string> headers)
        {
            var headersToSign = new List<string>(headers.Keys);
            headersToSign.Sort(StringComparer.OrdinalIgnoreCase);

            var sb = new System.Text.StringBuilder();
            foreach (var header in headersToSign)
            {
                if (sb.Length > 0)
                    sb.Append(";");
                sb.Append(header.ToLower());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Computes the canonical headers with values for the request. 
        /// For AWS4, all headers must be included in the signing process.
        /// </summary>
        /// <param name="headers">The set of headers to be encoded</param>
        /// <returns>Canonicalized string of headers with values</returns>
        protected virtual string CanonicalizeHeaders(IDictionary<string, string> headers)
        {
            if (headers == null || headers.Count == 0)
                return string.Empty;

            // step1: sort the headers into lower-case format; we create a new
            // map to ensure we can do a subsequent key lookup using a lower-case
            // key regardless of how 'headers' was created.
            var sortedHeaderMap = new SortedDictionary<string, string>();
            foreach (var header in headers.Keys)
            {
                sortedHeaderMap.Add(header.ToLower(), headers[header]);
            }

            // step2: form the canonical header:value entries in sorted order. 
            // Multiple white spaces in the values should be compressed to a single 
            // space.
            var sb = new System.Text.StringBuilder();
            foreach (var header in sortedHeaderMap.Keys)
            {
                var headerValue = CompressWhitespaceRegex.Replace(sortedHeaderMap[header], " ");
                sb.AppendFormat("{0}:{1}\n", header, headerValue.Trim());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the canonical request string to go into the signer process; this 
        /// consists of several canonical sub-parts.
        /// </summary>
        /// <param name="endpointUri"></param>
        /// <param name="httpMethod"></param>
        /// <param name="queryParameters"></param>
        /// <param name="canonicalizedHeaderNames">
        /// The set of header names to be included in the signature, formatted as a flattened, ;-delimited string
        /// </param>
        /// <param name="canonicalizedHeaders">
        /// </param>
        /// <param name="bodyHash">
        /// Precomputed SHA256 hash of the request body content. For chunked encoding this
        /// should be the fixed string ''.
        /// </param>
        /// <returns>String representing the canonicalized request for signing</returns>
        protected string CanonicalizeRequest(Uri endpointUri,
                                             string httpMethod,
                                             string queryParameters,
                                             string canonicalizedHeaderNames,
                                             string canonicalizedHeaders,
                                             string bodyHash)
        {
            var canonicalRequest = new System.Text.StringBuilder();

            canonicalRequest.AppendFormat("{0}\n", httpMethod);
            canonicalRequest.AppendFormat("{0}\n", CanonicalResourcePath(endpointUri));
            canonicalRequest.AppendFormat("{0}\n", queryParameters);

            canonicalRequest.AppendFormat("{0}\n", canonicalizedHeaders);
            canonicalRequest.AppendFormat("{0}\n", canonicalizedHeaderNames);

            canonicalRequest.Append(bodyHash);

            return canonicalRequest.ToString();
        }

        /// <summary>
        /// Returns the canonicalized resource path for the service endpoint
        /// </summary>
        /// <param name="endpointUri">Endpoint to the service/resource</param>
        /// <returns>Canonicalized resource path for the endpoint</returns>
        protected string CanonicalResourcePath(Uri endpointUri)
        {
            if (string.IsNullOrEmpty(endpointUri.AbsolutePath))
                return "/";

            return endpointUri.AbsolutePath;
        }

        /// <summary>
        /// Compute and return the multi-stage signing key for the request.
        /// </summary>
        /// <param name="algorithm">Hashing algorithm to use</param>
        /// <param name="awsSecretAccessKey">The clear-text AWS secret key</param>
        /// <param name="region">The region in which the service request will be processed</param>
        /// <param name="date">Date of the request, in yyyyMMdd format</param>
        /// <param name="service">The name of the service being called by the request</param>
        /// <returns>Computed signing key</returns>
        protected byte[] DeriveSigningKey(string algorithm, string awsSecretAccessKey, string region, string date, string service)
        {
            const string ksecretPrefix = SCHEME;
            char[] ksecret = null;

            ksecret = (ksecretPrefix + awsSecretAccessKey).ToCharArray();

            byte[] hashDate = ComputeKeyedHash(algorithm,  System.Text.Encoding.UTF8.GetBytes(ksecret),  System.Text.Encoding.UTF8.GetBytes(date));
            byte[] hashRegion = ComputeKeyedHash(algorithm, hashDate,  System.Text.Encoding.UTF8.GetBytes(region));
            byte[] hashService = ComputeKeyedHash(algorithm, hashRegion,  System.Text.Encoding.UTF8.GetBytes(service));
            return ComputeKeyedHash(algorithm, hashService,  System.Text.Encoding.UTF8.GetBytes(TERMINATOR));
        }

        /// <summary>
        /// Compute and return the hash of a data blob using the specified algorithm
        /// and key
        /// </summary>
        /// <param name="algorithm">Algorithm to use for hashing</param>
        /// <param name="key">Hash key</param>
        /// <param name="data">Data blob</param>
        /// <returns>Hash of the data</returns>
        protected byte[] ComputeKeyedHash(string algorithm, byte[] key, byte[] data)
        {
            var kha = System.Security.Cryptography.KeyedHashAlgorithm.Create(algorithm);
            kha.Key = key;
            return kha.ComputeHash(data);
        }

        /// <summary>
        /// Helper to format a byte array into string
        /// </summary>
        /// <param name="data">The data blob to process</param>
        /// <param name="lowercase">If true, returns hex digits in lower case form</param>
        /// <returns>String version of the data</returns>
        public static string ToHexString(byte[] data, bool lowercase)
        {
            var sb = new System.Text.StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString(lowercase ? "x2" : "X2"));
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// Sample AWS4 signer demonstrating how to sign requests to Amazon S3
    /// using an 'Authorization' header.
    /// </summary>
    internal class AWS4SignerForAuthorizationHeader : AWS4SignerBase
    {
        /// <summary>
        /// Computes an AWS4 signature for a request, ready for inclusion as an 
        /// 'Authorization' header.
        /// </summary>
        /// <param name="headers">
        /// The request headers; 'Host' and 'X-Amz-Date' will be added to this set.
        /// </param>
        /// <param name="queryParameters">
        /// Any query parameters that will be added to the endpoint. The parameters 
        /// should be specified in canonical format.
        /// </param>
        /// <param name="bodyHash">
        /// Precomputed SHA256 hash of the request body content; this value should also
        /// be set as the header 'X-Amz-Content-SHA256' for non-streaming uploads.
        /// </param>
        /// <param name="awsAccessKey">
        /// The user's AWS Access Key.
        /// </param>
        /// <param name="awsSecretKey">
        /// The user's AWS Secret Key.
        /// </param>
        /// <returns>
        /// The computed authorization string for the request. This value needs to be set as the 
        /// header 'Authorization' on the subsequent HTTP request.
        /// </returns>
        public string ComputeSignature(IDictionary<string, string> headers,
                                       string queryParameters,
                                       string bodyHash,
                                       string awsAccessKey,
                                       string awsSecretKey)
        {
            // first get the date and time for the subsequent request, and convert to ISO 8601 format
            // for use in signature generation
            var requestDateTime = DateTime.UtcNow;
            var dateTimeStamp = requestDateTime.ToString(ISO8601BasicFormat, System.Globalization.CultureInfo.InvariantCulture);

            // update the headers with required 'x-amz-date' and 'host' values
            headers.Add(X_Amz_Date, dateTimeStamp);

            var hostHeader = EndpointUri.Host;
            if (!EndpointUri.IsDefaultPort)
                hostHeader += ":" + EndpointUri.Port;
            headers.Add("Host", hostHeader);
                        
            // canonicalize the headers; we need the set of header names as well as the
            // names and values to go into the signature process
            var canonicalizedHeaderNames = CanonicalizeHeaderNames(headers);
            var canonicalizedHeaders = CanonicalizeHeaders(headers);

            // if any query string parameters have been supplied, canonicalize them
            // (note this sample assumes any required url encoding has been done already)
            var canonicalizedQueryParameters = string.Empty;
            if (!string.IsNullOrEmpty(queryParameters))
            {
                var paramDictionary = queryParameters.Split('&').Select(p => p.Split('='))
                                                     .ToDictionary(nameval => nameval[0],
                                                                   nameval => nameval.Length > 1
                                                                        ? nameval[1] : "");

                var sb = new System.Text.StringBuilder();
                var paramKeys = new List<string>(paramDictionary.Keys);
                paramKeys.Sort(StringComparer.Ordinal);
                foreach (var p in paramKeys)
                {
                    if (sb.Length > 0)
                        sb.Append("&");
                    sb.AppendFormat("{0}={1}", p, paramDictionary[p]);
                }

                canonicalizedQueryParameters = sb.ToString();
            }
            
            // canonicalize the various components of the request
            var canonicalRequest = CanonicalizeRequest(EndpointUri,
                                                       HttpMethod,
                                                       canonicalizedQueryParameters,
                                                       canonicalizedHeaderNames,
                                                       canonicalizedHeaders,
                                                       bodyHash);
            Console.WriteLine("\nCanonicalRequest:\n{0}", canonicalRequest);

            // generate a hash of the canonical request, to go into signature computation
            var canonicalRequestHashBytes
                = CanonicalRequestHashAlgorithm.ComputeHash( System.Text.Encoding.UTF8.GetBytes(canonicalRequest));

            // construct the string to be signed
            var stringToSign = new System.Text.StringBuilder();

            var dateStamp = requestDateTime.ToString(DateStringFormat, System.Globalization.CultureInfo.InvariantCulture);
            var scope = string.Format("{0}/{1}/{2}/{3}", 
                                      dateStamp, 
                                      Region, 
                                      Service, 
                                      TERMINATOR);

            stringToSign.AppendFormat("{0}-{1}\n{2}\n{3}\n", SCHEME, ALGORITHM, dateTimeStamp, scope);
            stringToSign.Append(ToHexString(canonicalRequestHashBytes, true));

            Console.WriteLine("\nStringToSign:\n{0}", stringToSign);

            // compute the signing key
            var kha = System.Security.Cryptography.KeyedHashAlgorithm.Create(HMACSHA256);
            kha.Key = DeriveSigningKey(HMACSHA256, awsSecretKey, Region, dateStamp, Service);

            // compute the AWS4 signature and return it
            var signature = kha.ComputeHash( System.Text.Encoding.UTF8.GetBytes(stringToSign.ToString()));
            var signatureString = ToHexString(signature, true);
            Console.WriteLine("\nSignature:\n{0}", signatureString);

            var authString = new System.Text.StringBuilder();
            authString.AppendFormat("{0}-{1} ", SCHEME, ALGORITHM);
            authString.AppendFormat("Credential={0}/{1}, ", awsAccessKey, scope);
            authString.AppendFormat("SignedHeaders={0}, ", canonicalizedHeaderNames);
            authString.AppendFormat("Signature={0}", signatureString);

            var authorization = authString.ToString();
            Console.WriteLine("\nAuthorization:\n{0}", authorization);

            return authorization;
        }
    }


    /// <summary>
    /// Sample AWS4 signer demonstrating how to sign 'chunked' uploads
    /// to Amazon S3 using an Authorization header.
    /// </summary>
    internal class AWS4SignerForChunkedUpload : AWS4SignerBase
    {
        // SHA256 substitute marker used in place of x-amz-content-sha256 when employing 
        // chunked uploads
        public const string STREAMING_BODY_SHA256 = "STREAMING-AWS4-HMAC-SHA256-PAYLOAD";

        static readonly string CLRF = "\r\n";
        static readonly string CHUNK_STRING_TO_SIGN_PREFIX = "AWS4-HMAC-SHA256-PAYLOAD";
        static readonly string CHUNK_SIGNATURE_HEADER = ";chunk-signature=";
        static readonly int SIGNATURE_LENGTH = 64;
        static byte[] FINAL_CHUNK = new byte[0];

        /// <summary>
        /// Tracks the previously computed signature value; for chunk 0 this will
        /// contain the signature included in the Authorization header. For subsequent
        /// chunks it contains the computed signature of the prior chunk.
        /// </summary>
        public string LastComputedSignature { get; private set; }

        /// <summary>
        /// Date and time of the original signing computation, in ISO 8601 basic format,
        /// reused for each chunk
        /// </summary>
        public string DateTimeStamp { get; private set; }

        /// <summary>
        /// The scope value of the original signing computation, reused for each chunk
        /// </summary>
        public string Scope { get; private set; }

        /// <summary>
        /// The derived signing key used in the original signature computation and
        /// re-used for each chunk
        /// </summary>
        byte[] SigningKey { get; set; }

        /// <summary>
        /// Computes an AWS4 signature for a request, ready for inclusion as an 
        /// 'Authorization' header.
        /// </summary>
        /// <param name="headers">
        /// The request headers; 'Host' and 'X-Amz-Date' will be added to this set.
        /// </param>
        /// <param name="queryParameters">
        /// Any query parameters that will be added to the endpoint. The parameters 
        /// should be specified in canonical format.
        /// </param>
        /// <param name="bodyHash">
        /// Precomputed SHA256 hash of the request body content; this value should also
        /// be set as the header 'X-Amz-Content-SHA256' for non-streaming uploads.
        /// </param>
        /// <param name="awsAccessKey">
        /// The user's AWS Access Key.
        /// </param>
        /// <param name="awsSecretKey">
        /// The user's AWS Secret Key.
        /// </param>
        /// <returns>
        /// The computed authorization string for the request. This value needs to be set as the 
        /// header 'Authorization' on the subsequent HTTP request.
        /// </returns>
        public string ComputeSignature(IDictionary<string, string> headers,
                                       string queryParameters,
                                       string bodyHash,
                                       string awsAccessKey,
                                       string awsSecretKey)
        {
            // first get the date and time for the subsequent request, and convert to ISO 8601 format
            // for use in signature generation
            var requestDateTime = DateTime.UtcNow;
            DateTimeStamp = requestDateTime.ToString(ISO8601BasicFormat, System.Globalization.CultureInfo.InvariantCulture);

            // update the headers with required 'x-amz-date' and 'host' values
            headers.Add(X_Amz_Date, DateTimeStamp);

            var hostHeader = EndpointUri.Host;
            if (!EndpointUri.IsDefaultPort)
                hostHeader += ":" + EndpointUri.Port;
            headers.Add("Host", hostHeader);

            // canonicalize the headers; we need the set of header names as well as the
            // names and values to go into the signature process
            var canonicalizedHeaderNames = CanonicalizeHeaderNames(headers);
            var canonicalizedHeaders = CanonicalizeHeaders(headers);

            // if any query string parameters have been supplied, canonicalize them
            // (note this sample assumes any required url encoding has been done already)
            var canonicalizedQueryParameters = string.Empty;
            if (!string.IsNullOrEmpty(queryParameters))
            {
                var paramDictionary = queryParameters.Split('&').Select(p => p.Split('='))
                                                     .ToDictionary(nameval => nameval[0],
                                                                   nameval => nameval.Length > 1
                                                                        ? nameval[1] : "");

                var sb = new System.Text.StringBuilder();
                var paramKeys = new List<string>(paramDictionary.Keys);
                paramKeys.Sort(StringComparer.Ordinal);
                foreach (var p in paramKeys)
                {
                    if (sb.Length > 0)
                        sb.Append("&");
                    sb.AppendFormat("{0}={1}", p, paramDictionary[p]);
                }

                canonicalizedQueryParameters = sb.ToString();
            }

            // canonicalize the various components of the request
            var canonicalRequest = CanonicalizeRequest(EndpointUri,
                                                       HttpMethod,
                                                       canonicalizedQueryParameters,
                                                       canonicalizedHeaderNames,
                                                       canonicalizedHeaders,
                                                       bodyHash);
            Console.WriteLine("\nCanonicalRequest:\n{0}", canonicalRequest);

            // generate a hash of the canonical request, to go into signature computation
            var canonicalRequestHashBytes 
                = CanonicalRequestHashAlgorithm.ComputeHash( System.Text.Encoding.UTF8.GetBytes(canonicalRequest));

            // construct the string to be signed
            var stringToSign = new System.Text.StringBuilder();

            var dateStamp = requestDateTime.ToString(DateStringFormat, System.Globalization.CultureInfo.InvariantCulture);
            Scope = string.Format("{0}/{1}/{2}/{3}",
                                  dateStamp,
                                  Region,
                                  Service,
                                  TERMINATOR);

            stringToSign.AppendFormat("{0}-{1}\n{2}\n{3}\n", SCHEME, ALGORITHM, DateTimeStamp, Scope);
            stringToSign.Append(ToHexString(canonicalRequestHashBytes, true));

            Console.WriteLine("\nStringToSign:\n{0}", stringToSign);

            // compute the signing key
            SigningKey = DeriveSigningKey(HMACSHA256, awsSecretKey, Region, dateStamp, Service);

            var kha = System.Security.Cryptography.KeyedHashAlgorithm.Create(HMACSHA256);
            kha.Key = SigningKey;

            // compute the AWS4 signature and return it
            var signature = kha.ComputeHash( System.Text.Encoding.UTF8.GetBytes(stringToSign.ToString()));
            var signatureString = ToHexString(signature, true);
            Console.WriteLine("\nSignature:\n{0}", signatureString);

            // cache the computed signature ready for chunk 0 upload
            LastComputedSignature = signatureString;

            var authString = new System.Text.StringBuilder();
            authString.AppendFormat("{0}-{1} ", SCHEME, ALGORITHM);
            authString.AppendFormat("Credential={0}/{1}, ", awsAccessKey, Scope);
            authString.AppendFormat("SignedHeaders={0}, ", canonicalizedHeaderNames);
            authString.AppendFormat("Signature={0}", signatureString);

            var authorization = authString.ToString();
            Console.WriteLine("\nAuthorization:\n{0}", authorization);

            return authorization;
        }

        /// <summary>
        /// Calculates the expanded payload size of our data when it is chunked
        /// </summary>
        /// <param name="originalLength">
        /// The true size of the data payload to be uploaded
        /// </param>
        /// <param name="chunkSize">
        /// The size of each chunk we intend to send; each chunk will be
        /// prefixed with signed header data, expanding the overall size
        /// by a determinable amount
        /// </param>
        /// <returns>
        /// The overall payload size to use as content-length on a chunked upload
        /// </returns>
        public long CalculateChunkedContentLength(long originalLength, long chunkSize)
        {
            if (originalLength <= 0)
                throw new ArgumentOutOfRangeException("originalLength");
            if (chunkSize <= 0)
                throw new ArgumentOutOfRangeException("chunkSize");

            var maxSizeChunks = originalLength / chunkSize;
            var remainingBytes = originalLength % chunkSize;

            var chunkedContentLength = maxSizeChunks*CalculateChunkHeaderLength(chunkSize)
                                       + (remainingBytes > 0 ? CalculateChunkHeaderLength(remainingBytes) : 0)
                                       + CalculateChunkHeaderLength(0);

            Console.WriteLine("\nComputed chunked content length for original length {0} bytes, chunk size {1}KB is {2} bytes", originalLength, chunkSize/1024, chunkedContentLength);
            return chunkedContentLength;
        }

        /// <summary>
        /// Returns the size of a chunk header, which only varies depending
        /// on the selected chunk size
        /// </summary>
        /// <param name="chunkSize">
        /// The intended size of each chunk; this is placed into the chunk 
        /// header
        /// </param>
        /// <returns>
        /// The overall size of the header that will prefix the user data in 
        /// each chunk
        /// </returns>
        static long CalculateChunkHeaderLength(long chunkSize)
        {
            return chunkSize.ToString("X").Length
                    + CHUNK_SIGNATURE_HEADER.Length
                    + SIGNATURE_LENGTH
                    + CLRF.Length
                    + chunkSize
                    + CLRF.Length;
        }

        /// <summary>
        /// Returns a chunk for upload consisting of the signed 'header' or chunk
        /// prefix plus the user data. The signature of the chunk incorporates the
        /// signature of the previous chunk (or, if the first chunk, the signature
        /// of the headers portion of the request).
        /// </summary>
        /// <param name="userDataLen">
        /// The length of the user data contained in userData
        /// </param>
        /// <param name="userData">
        /// Contains the user data to be sent in the upload chunk
        /// </param>
        /// <returns>
        /// A new buffer of data for upload containing the chunk header plus user data
        /// </returns>
        public byte[] ConstructSignedChunk(long userDataLen, byte[] userData)
        {
            // to keep our computation routine signatures simple, if the userData
            // buffer contains less data than it could, shrink it. Note the special case
            // to handle the requirement that we send an empty chunk to complete
            // our chunked upload.
            byte[] dataToChunk;
            if (userDataLen == 0)
                dataToChunk = FINAL_CHUNK;
            else
            {
                if (userDataLen < userData.Length)
                {
                    // shrink the chunkdata to fit
                    dataToChunk = new byte[userDataLen];
                    Array.Copy(userData, 0, dataToChunk, 0, userDataLen);
                }
                else
                    dataToChunk = userData;
            }

            var chunkHeader = new System.Text.StringBuilder();

            // start with size of user data
            chunkHeader.Append(dataToChunk.Length.ToString("X"));

            // nonsig-extension; we have none in these samples
            const string nonsigExtension = "";

            // if this is the first chunk, we package it with the signing result
            // of the request headers, otherwise we use the cached signature
            // of the previous chunk

            // sig-extension
            var chunkStringToSign =
                    CHUNK_STRING_TO_SIGN_PREFIX + "\n" +
                    DateTimeStamp + "\n" +
                    Scope + "\n" +
                    LastComputedSignature + "\n" +
                    ToHexString(CanonicalRequestHashAlgorithm.ComputeHash( System.Text.Encoding.UTF8.GetBytes(nonsigExtension)), true) + "\n" +
                    ToHexString(CanonicalRequestHashAlgorithm.ComputeHash(dataToChunk), true);

            Console.WriteLine("\nChunkStringToSign:\n{0}", chunkStringToSign);

            // compute the V4 signature for the chunk
            var chunkSignature
                = ToHexString(ComputeKeyedHash("HMACSHA256",
                                               SigningKey,
                                                System.Text.Encoding.UTF8.GetBytes(chunkStringToSign)),
                                     true);

            Console.WriteLine("\nChunkSignature:\n{0}", chunkSignature);

            // cache the signature to include with the next chunk's signature computation
            this.LastComputedSignature = chunkSignature;

            // construct the actual chunk, comprised of the non-signed extensions, the
            // 'headers' we just signed and their signature, plus a newline then copy
            // that plus the user's data to a payload to be written to the request stream
            chunkHeader.Append(nonsigExtension + CHUNK_SIGNATURE_HEADER + chunkSignature);
            chunkHeader.Append(CLRF);

            Console.WriteLine("\nChunkHeader:\n{0}", chunkHeader);

            try
            {
                var header =  System.Text.Encoding.UTF8.GetBytes(chunkHeader.ToString());
                var trailer =  System.Text.Encoding.UTF8.GetBytes(CLRF);
                var signedChunk = new byte[header.Length + dataToChunk.Length + trailer.Length];

                Array.Copy(header, 0, signedChunk, 0, header.Length);
                Array.Copy(dataToChunk, 0, signedChunk, header.Length, dataToChunk.Length);
                Array.Copy(trailer, 0, signedChunk, header.Length + dataToChunk.Length, trailer.Length);

                // this is the total data for the chunk that will be sent to the request stream
                return signedChunk;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to sign the chunked data. " + e.Message, e);
            }
        }
    }

    /// <summary>
    /// Sample AWS4 signer demonstrating how to sign POST requests to Amazon S3
    /// using a policy.
    /// </summary>
    internal class AWS4SignerForPOST : AWS4SignerBase
    {
        public string FormatCredentialStringForPolicy(DateTime dateTimeStamp)
        {
            return "AKIAIOSFODNN7EXAMPLE/20130806/cn-north-1/s3/aws4_request";                
        }

        public string FormatAlgorithmForPolicy
        {
            get { return "AWS4-HMAC-SHA256"; }
        }

        public string FormatDateTimeForPolicy(DateTime dateTimeStamp)
        {
            return dateTimeStamp.ToString(ISO8601BasicFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Computes an AWS4 signature for a request, ready for inclusion as an 
        /// 'Authorization' header.
        /// </summary>
        /// <param name="headers">
        /// The request headers; 'Host' and 'X-Amz-Date' will be added to this set.
        /// </param>
        /// <param name="queryParameters">
        /// Any query parameters that will be added to the endpoint. The parameters 
        /// should be specified in canonical format.
        /// </param>
        /// <param name="bodyHash">
        /// Precomputed SHA256 hash of the request body content; this value should also
        /// be set as the header 'X-Amz-Content-SHA256' for non-streaming uploads.
        /// </param>
        /// <param name="awsAccessKey">
        /// The user's AWS Access Key.
        /// </param>
        /// <param name="awsSecretKey">
        /// The user's AWS Secret Key.
        /// </param>
        /// <returns>
        /// The computed authorization string for the request. This value needs to be set as the 
        /// header 'Authorization' on the subsequent HTTP request.
        /// </returns>
        public string ComputeSignature(IDictionary<string, string> headers,
                                       string queryParameters,
                                       string bodyHash,
                                       string awsAccessKey,
                                       string awsSecretKey)
        {
            // first get the date and time for the subsequent request, and convert to ISO 8601 format
            // for use in signature generation
            var requestDateTime = DateTime.UtcNow;
            var dateTimeStamp = requestDateTime.ToString(ISO8601BasicFormat, System.Globalization.CultureInfo.InvariantCulture);

            // update the headers with required 'x-amz-date' and 'host' values
            headers.Add(X_Amz_Date, dateTimeStamp);

            var hostHeader = EndpointUri.Host;
            if (!EndpointUri.IsDefaultPort)
                hostHeader += ":" + EndpointUri.Port;
            headers.Add("Host", hostHeader);

            // canonicalize the headers; we need the set of header names as well as the
            // names and values to go into the signature process
            var canonicalizedHeaderNames = CanonicalizeHeaderNames(headers);
            var canonicalizedHeaders = CanonicalizeHeaders(headers);

            // if any query string parameters have been supplied, canonicalize them
            // (note this sample assumes any required url encoding has been done already)
            var canonicalizedQueryParameters = string.Empty;
            if (!string.IsNullOrEmpty(queryParameters))
            {
                var paramDictionary = queryParameters.Split('&').Select(p => p.Split('='))
                                                     .ToDictionary(nameval => nameval[0],
                                                                   nameval => nameval.Length > 1
                                                                        ? nameval[1] : "");

                var sb = new System.Text.StringBuilder();
                var paramKeys = new List<string>(paramDictionary.Keys);
                paramKeys.Sort(StringComparer.Ordinal);
                foreach (var p in paramKeys)
                {
                    if (sb.Length > 0)
                        sb.Append("&");
                    sb.AppendFormat("{0}={1}", p, paramDictionary[p]);
                }

                canonicalizedQueryParameters = sb.ToString();
            }

            // canonicalize the various components of the request
            var canonicalRequest = CanonicalizeRequest(EndpointUri,
                                                       HttpMethod,
                                                       canonicalizedQueryParameters,
                                                       canonicalizedHeaderNames,
                                                       canonicalizedHeaders,
                                                       bodyHash);
            Console.WriteLine("\nCanonicalRequest:\n{0}", canonicalRequest);

            // generate a hash of the canonical request, to go into signature computation
            var canonicalRequestHashBytes
                = CanonicalRequestHashAlgorithm.ComputeHash( System.Text.Encoding.UTF8.GetBytes(canonicalRequest));

            // construct the string to be signed
            var stringToSign = new System.Text.StringBuilder();

            var dateStamp = requestDateTime.ToString(DateStringFormat, System.Globalization.CultureInfo.InvariantCulture);
            var scope = string.Format("{0}/{1}/{2}/{3}",
                                      dateStamp,
                                      Region,
                                      Service,
                                      TERMINATOR);

            stringToSign.AppendFormat("{0}-{1}\n{2}\n{3}\n", SCHEME, ALGORITHM, dateTimeStamp, scope);
            stringToSign.Append(ToHexString(canonicalRequestHashBytes, true));

            Console.WriteLine("\nStringToSign:\n{0}", stringToSign);

            // compute the signing key
            var kha = System.Security.Cryptography.KeyedHashAlgorithm.Create(HMACSHA256);
            kha.Key = DeriveSigningKey(HMACSHA256, awsSecretKey, Region, dateStamp, Service);

            // compute the AWS4 signature and return it
            var signature = kha.ComputeHash( System.Text.Encoding.UTF8.GetBytes(stringToSign.ToString()));
            var signatureString = ToHexString(signature, true);
            Console.WriteLine("\nSignature:\n{0}", signatureString);

            var authString = new System.Text.StringBuilder();
            authString.AppendFormat("{0}-{1} ", SCHEME, ALGORITHM);
            authString.AppendFormat("Credential={0}/{1}, ", awsAccessKey, scope);
            authString.AppendFormat("SignedHeaders={0}, ", canonicalizedHeaderNames);
            authString.AppendFormat("Signature={0}", signatureString);

            var authorization = authString.ToString();
            Console.WriteLine("\nAuthorization:\n{0}", authorization);

            return authorization;
        }
    }

    /// <summary>
    /// Sample AWS4 signer demonstrating how to sign requests to Amazon S3
    /// using query string parameters.
    /// </summary>
    internal class AWS4SignerForQueryParameterAuth : AWS4SignerBase
    {
        /// <summary>
        /// Computes an AWS4 authorization for a request, suitable for embedding 
        /// in query parameters.
        /// </summary>
        /// <param name="headers">
        /// The request headers; 'Host' and 'X-Amz-Date' will be added to this set.
        /// </param>
        /// <param name="queryParameters">
        /// Any query parameters that will be added to the endpoint. The parameters 
        /// should be specified in canonical format.
        /// </param>
        /// <param name="bodyHash">
        /// Precomputed SHA256 hash of the request body content; this value should also
        /// be set as the header 'X-Amz-Content-SHA256' for non-streaming uploads.
        /// </param>
        /// <param name="awsAccessKey">
        /// The user's AWS Access Key.
        /// </param>
        /// <param name="awsSecretKey">
        /// The user's AWS Secret Key.
        /// </param>
        /// <returns>
        /// The string expressing the Signature V4 components to add to query parameters.
        /// </returns>
        public string ComputeSignature(IDictionary<string, string> headers,
                                       string queryParameters,
                                       string bodyHash,
                                       string awsAccessKey,
                                       string awsSecretKey)
        {
            // first get the date and time for the subsequent request, and convert to ISO 8601 format
            // for use in signature generation
            var requestDateTime = DateTime.UtcNow;
            var dateTimeStamp = requestDateTime.ToString(ISO8601BasicFormat, System.Globalization.CultureInfo.InvariantCulture);

            // extract the host portion of the endpoint to include in the signature calculation,
            // unless already set
            if (!headers.ContainsKey("Host"))
            {
                var hostHeader = EndpointUri.Host;
                if (!EndpointUri.IsDefaultPort)
                    hostHeader += ":" + EndpointUri.Port;
                headers.Add("Host", hostHeader);
            }

            var dateStamp = requestDateTime.ToString(DateStringFormat, System.Globalization.CultureInfo.InvariantCulture);
            var scope = string.Format("{0}/{1}/{2}/{3}",
                                      dateStamp,
                                      Region,
                                      Service,
                                      TERMINATOR);

            // canonicalized headers need to be expressed in the query
            // parameters processed in the signature
            var canonicalizedHeaderNames = CanonicalizeHeaderNames(headers);
            var canonicalizedHeaders = CanonicalizeHeaders(headers);

            // reform the query parameters to (a) add the parameters required for
            // Signature V4 and (b) canonicalize the set before they go into the
            // signature calculation. Note that this assumes parameter names and 
            // values added outside this routine are already url encoded
            var paramDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(queryParameters))
            {
                paramDictionary = queryParameters.Split('&').Select(p => p.Split('='))
                                                     .ToDictionary(nameval => nameval[0],
                                                                   nameval => nameval.Length > 1
                                                                        ? nameval[1] : "");
            }

            // add the fixed authorization params required by Signature V4
            paramDictionary.Add(X_Amz_Algorithm, HttpHelpers.UrlEncode(string.Format("{0}-{1}", SCHEME, ALGORITHM)));
            paramDictionary.Add(X_Amz_Credential, HttpHelpers.UrlEncode(string.Format("{0}/{1}", awsAccessKey, scope)));
            paramDictionary.Add(X_Amz_SignedHeaders, HttpHelpers.UrlEncode(canonicalizedHeaderNames));

            // x-amz-date is now added as a query parameter, not a header, but still needs to be in ISO8601 basic form
            paramDictionary.Add(X_Amz_Date, HttpHelpers.UrlEncode(dateTimeStamp));

            // build the expanded canonical query parameter string that will go into the
            // signature computation
            var sb = new System.Text.StringBuilder();
            var paramKeys = new List<string>(paramDictionary.Keys);
            paramKeys.Sort(StringComparer.Ordinal);
            foreach (var p in paramKeys)
            {
                if (sb.Length > 0)
                    sb.Append("&");
                sb.AppendFormat("{0}={1}", p, paramDictionary[p]);
            }
            var canonicalizedQueryParameters = sb.ToString();

            // express all the header and query parameter data as a canonical request string
            var canonicalRequest = CanonicalizeRequest(EndpointUri,
                                                       HttpMethod,
                                                       canonicalizedQueryParameters,
                                                       canonicalizedHeaderNames,
                                                       canonicalizedHeaders,
                                                       bodyHash);
            Console.WriteLine("\nCanonicalRequest:\n{0}", canonicalRequest);

            byte[] canonicalRequestHashBytes 
                = CanonicalRequestHashAlgorithm.ComputeHash( System.Text.Encoding.UTF8.GetBytes(canonicalRequest));

            // construct the string to be signed
            var stringToSign = new System.Text.StringBuilder();

            stringToSign.AppendFormat("{0}-{1}\n{2}\n{3}\n", SCHEME, ALGORITHM, dateTimeStamp, scope);
            stringToSign.Append(ToHexString(canonicalRequestHashBytes, true));

            Console.WriteLine("\nStringToSign:\n{0}", stringToSign);

            // compute the multi-stage signing key
            System.Security.Cryptography.KeyedHashAlgorithm kha = System.Security.Cryptography.KeyedHashAlgorithm.Create(HMACSHA256);
            kha.Key = DeriveSigningKey(HMACSHA256, awsSecretKey, Region, dateStamp, Service);

            // compute the final signature for the request, place into the result and return to the 
            // user to be embedded in the request as needed
            var signature = kha.ComputeHash( System.Text.Encoding.UTF8.GetBytes(stringToSign.ToString()));
            var signatureString = ToHexString(signature, true);
            Console.WriteLine("\nSignature:\n{0}", signatureString);

            // form up the authorization parameters for the caller to place in the query string
            var authString = new System.Text.StringBuilder();
            var authParams = new string[]
                {
                    X_Amz_Algorithm, 
                    X_Amz_Credential, 
                    X_Amz_Date, 
                    X_Amz_SignedHeaders 
                };

            foreach (var p in authParams)
            {
                if (authString.Length > 0)
                    authString.Append("&");
                authString.AppendFormat("{0}={1}", p, paramDictionary[p]);
            }

            authString.AppendFormat("&{0}={1}", X_Amz_Signature, signatureString);

            var authorization = authString.ToString();
            Console.WriteLine("\nAuthorization:\n{0}", authorization);

            return authorization;
        }
    }

    #endregion
}