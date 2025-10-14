public class Script : ScriptBase
{
    /// <summary>
    /// Definition for the list of input batch translation request
    /// </summary>
    public class BatchRequestInput
    {
        /// <summary>
        /// The input batch translation request
        /// </summary>
        public List<BatchRequest> inputs { get; set; }
    }

    /// <summary>
    /// Definition for the input batch translation request
    /// </summary>
    public class BatchRequest
    {
        /// <summary>
        /// Whether the storage source is a folder or a file
        /// </summary>
        /// <example>File</example>
        public string storageType { get; set; }

        /// <summary>
        /// Location of the source containing the documents
        /// </summary>
        public SourceInput source { get; set; }

        /// <summary>
        /// Location of the destination for the output
        /// </summary>
        public List<TargetInput> targets { get; set; } = new List<TargetInput>();
    }

    /// <summary>
    /// Source of the input documents
    /// </summary>
    public class SourceInput
    {
        /// <summary>
        /// Language code
        /// If none is specified, we will perform auto detect on the document
        /// </summary>
        /// <example>en</example>
        public string language { get; set; }

        /// <summary>
        /// Location of the folder / container or single file with your documents
        /// </summary>
        /// <example>https://myblob.blob.core.windows.net/Container/</example>
        public string sourceUrl { get; set; }

        /// <summary>
        /// Filter for the source
        /// </summary>
        public DocumentFilter filter { get; set; }

    }

    /// <summary>
    /// Filter for the source
    /// </summary>
    public class DocumentFilter
    {
        /// <summary>
        /// A case-sensitive prefix string to filter documents in the source path for translation. 
        /// For example, when using a Azure storage blob Uri, use the prefix to restrict sub folders for translation.
        /// </summary>
        /// <example>FolderA</example>
        public string prefix { get; set; } = string.Empty;

        /// <summary>
        /// A case-sensitive suffix string to filter documents in the source path for translation. 
        /// This is most often use for file extensions
        /// </summary>
        /// <example>.txt</example>
        public string suffix { get; set; } = string.Empty;
    }

    /// <summary>
    /// Destination for the finished translated documents
    /// </summary>
    public class TargetInput
    {
        /// <summary>
        /// Category / custom system for translation request
        /// </summary>
        /// <example>general</example>
        public string category { get; set; } = "general";

        /// <summary>
        /// Location of the folder / container with your documents
        /// </summary>
        /// <example>https://myblob.blob.core.windows.net/TargetUrl/</example>
        public string targetUrl { get; set; }

        /// <summary>
        /// Target Language
        /// </summary>
        /// <example>fr</example>
        public string language { get; set; }

        /// <summary>
        /// List of Glossary
        /// </summary>
        public List<Glossary> glossaries { get; set; }
    }

    /// <summary>
    /// Glossary / translation memory for the request
    /// </summary>
    public class Glossary
    {
        /// <summary>
        /// Location of the glossary. 
        /// We will use the file extension to extract the formatting if the format parameter is not supplied.
        /// If the translation language pair is not present in the glossary, it will not be applied
        /// </summary>
        /// <example>https://myblob.blob.core.windows.net/Container/myglossary.tsv</example>
        public string glossaryUrl { get; set; }

        /// <summary>
        /// Format
        /// </summary>
        /// <example>XLIFF</example>
        public string format { get; set; }

        /// <summary>
        /// Optional Version.  If not specified, default is used.
        /// </summary>
        /// <example>2.0</example>
        public string version { get; set; }
    }

    /// <summary>
    /// Translation Result
    /// </summary>
    public class TranslationResult
    {
        /// <summary>
        /// The detected language of the source text
        /// </summary>
        public DetectedLanguage DetectedLanguage { get; set; }
        
        /// <summary>
        /// True Text value
        /// </summary>
        public TextResult TrueText { get; set; }
        
        /// <summary>
        /// The actual source text
        /// </summary>
        public TextResult SourceText { get; set; }
        
        /// <summary>
        /// The Translations returned
        /// </summary>
        public Translations[] Translations { get; set; }
    }

    /// <summary>
    /// The Transliteraion results
    /// </summary>
    public class TextResult
    {
        /// <summary>
        /// The translated or transliterated text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The translated or transliterated script
        /// </summary>
        public string Script { get; set; }
    }
    /// <summary>
    /// The language detected for translation
    /// </summary>
    public class DetectedLanguage
    {
        /// <summary>
        /// Language code
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Confidence score between 0 and 1
        /// </summary>
        public float Score { get; set; }
    }

    /// <summary>
    /// The Translations returned
    /// </summary>
    public class Translations
    {
        /// <summary>
        /// The translated text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The Transliterated text
        /// </summary>
        public TextResult Transliteration { get; set; }
        
        /// <summary>
        /// The translate to language
        /// </summary>
        public string To { get; set; }
        //public Alignment Alignment { get; set; }

        /// <summary>
        /// Sentence length
        /// </summary>
        public SentenceLength SentenceLength { get; set; }
    }

    /// <summary>
    /// The sentence length returned from translations
    /// </summary>
    public class SentenceLength
    {
        /// <summary>
        /// Source sentence length
        /// </summary>
        public int[] SourceSentenceLengths { get; set; }

        /// <summary>
        /// The translation sentence lenghts
        /// </summary>
        public int[] TranslationSentenceLengths { get; set; }

    }

    /// <summary>
    /// Definition of all supported text translation languages
    /// </summary>
    public class Languages
    {
        /// <summary>
        /// Supported text translation languages
        /// </summary>
        public Dictionary<string, TextLanguage> translation { get; set; }
    }

    /// <summary>
    /// Definition of a text translation language
    /// </summary>
    public class TextLanguage
    {
        /// <summary>
        /// The display name of the language in the locale requested
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The display name of the language in the locale native for this language
        /// </summary>
        public string nativeName { get; set; }

        /// <summary>
        /// The directionality, which is rtl for right-to-left languages or ltr for left-to-right languages
        /// </summary>
        public string dir { get; set; }
    }

    /// <summary>
    /// The definition of the incoming request parameters from the Power automate UI
    /// </summary>
    public class IncomingRequest
    {
        /// <summary>
        /// Whether the storage source is a folder or a file
        /// </summary>
        /// <example>File</example>
        public string storageType { get; set; }

        /// <summary>
        /// Language code
        /// If none is specified, we will perform auto detect on the document
        /// </summary>
        /// <example>en</example>
        public string sourceLanguage { get; set; }

        /// <summary>
        /// The SAS URL of the Azure blobl storage source file
        /// </summary>
        public string sourceURL { get; set; }

        /// <summary>
        /// A case-sensitive prefix string to filter documents in the source path for translation. 
        /// For example, when using a Azure storage blob Uri, use the prefix to restrict sub folders for translation.
        /// </summary>
        /// <example>FolderA</example>
        public string documentFilterPrefix { get; set; } = string.Empty;

        /// <summary>
        /// A case-sensitive suffix string to filter documents in the source path for translation. 
        /// This is most often use for file extensions
        /// </summary>
        /// <example>.txt</example>
        public string documentFilterSuffix { get; set; } = string.Empty;

        /// <summary>
        /// Category / custom system for translation request
        /// </summary>
        /// <example>general</example>
        public string targetCategory { get; set; } = "general";

        /// <summary>
        /// Target container Azure blob URL
        /// </summary>
        /// <example>fr</example>
        public string targetContainerURL { get; set; }

        /// <summary>
        /// Target Language
        /// </summary>
        /// <example>fr</example>
        public string targetLanguage { get; set; }

        /// <summary>
        /// Location of the glossary. 
        /// We will use the file extension to extract the formatting if the format parameter is not supplied.
        /// If the translation language pair is not present in the glossary, it will not be applied
        /// </summary>
        /// <example>https://myblob.blob.core.windows.net/Container/myglossary.tsv</example>
        public string glossaryUrl { get; set; }

        /// <summary>
        /// Format
        /// </summary>
        /// <example>XLIFF_1.2</example>
        public string glossaryFormatVersion { get; set; }
    }

    /// <summary>
    /// The List result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListResult<T>
    {
        /// <summary>
        /// list of objects
        /// </summary>
        public List<T> Value { get; set; } = new List<T>();
    }

    /// <summary>
    /// The definition of the FileFormat
    /// </summary>
    public class FileFormat
    {
        /// <summary>
        /// Name of the format
        /// </summary>
        /// <example>PlainText</example>
        public string Format { get; set; }

        /// <summary>
        /// Supported file extension for this format
        /// </summary>
        /// <example>.txt</example>
        public List<string> FileExtensions { get; set; }

        /// <summary>
        /// Supported Content-Types for this format
        /// </summary>
        /// <example>text/plain</example>
        public List<string> ContentTypes { get; set; }

        /// <summary>
        /// Default version if none is specified
        /// </summary>
        public string DefaultVersion { get; set; }

        /// <summary>
        /// Supported Version
        /// </summary>
        /// <example>2.0</example>
        public List<string> Versions { get; set; }
    }

    /// <summary>
    /// List of supported file formats
    /// </summary>
    public class SupportedFileFormats : ListResult<FileFormat>
    {
    }

    /// <summary>
    /// Transliteration Languages
    /// </summary>
    public class TransliterationLanguages
    {
        /// <summary>
        /// List of Supported transliteration languages
        /// </summary>
        public Dictionary<string, TransliterationLanguage> transliteration { get; set; }
    }

    /// <summary>
    /// Transliteration Language object
    /// </summary>
    public class TransliterationLanguage
    {
        /// <summary>
        /// The language name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The language native name
        /// </summary>
        public string nativeName { get; set; }

        /// <summary>
        /// The list of Transliterate scripts
        /// </summary>
        public List<TransliterateScript> scripts { get; set; }
    }

    /// <summary>
    /// The transliterated script object
    /// </summary>
    public class TransliterateScript
    {
        /// <summary>
        /// Transliterated  script code
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// Transliterated  script name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Transliterated  script nativeName
        /// </summary>
        public string nativeName { get; set; }

        /// <summary>
        /// Direction
        /// </summary>
        public Direction dir { get; set; }

        /// <summary>
        /// List of transliterate toScripts
        /// </summary>
        public List<TransliterateScript> toScripts { get; set; }
    }

    /// <summary>
    /// direction
    /// </summary>
    public enum Direction
    {
        ltr,
        rtl
    }

    private const string LANGUAGES_URI = "https://api.cognitive.microsofttranslator.com/languages?api-version=3.0";
    private const string TRANSLATION_SCOPE = "&scope=translation";
    private const string TRANSLITERATION_SCOPE = "&scope=transliteration";
    private const string OPERATION_LOCATION_HEADER = "Operation-Location";
    BatchRequestInput input = new BatchRequestInput();

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        switch (this.Context.OperationId)
        {
            case "Translate":
                return await this.AsyncInvokeTranslate().ConfigureAwait(false);
                break;
            case "StartDocumentTranslation":
                return await this.AsyncInvokeStartDocumentTranslation().ConfigureAwait(false);
                break;
            case "GetTranslateSupportedLanguages":
                return await this.AsyncInvokeGetTranslateSupportedLanguages().ConfigureAwait(false);
                break;
            case "GetTranslateSupportedSourceLanguages":
                return await this.AsyncInvokeGetTranslateSupportedSourceLanguages().ConfigureAwait(false);
                break;
            case "GetTransliterationLanguages":
                return await this.AsyncInvokeGetTransliterationLanguages().ConfigureAwait(false);
                break;
            case "GetTransliterationFromScripts":
                return await this.AsyncInvokeGetTransliterationFromScripts().ConfigureAwait(false);
                break;
            case "GetTransliterationToScripts":
                return await this.AsyncInvokeGetTransliterationToScripts().ConfigureAwait(false);
                break;
            case "GetTransliterationToScriptsForLanguage":
                return await this.AsyncInvokeGetTransliterationToScriptsForLanguage().ConfigureAwait(false);
                break;
            case "GetGlossaryFormatVersion":
                return await this.AsyncInvokeGetGlossaryFormatVersion().ConfigureAwait(false);
                break;
        }
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        return response;
    }

    //Invokes a translate call and returns a translated text
    private async Task<HttpResponseMessage> AsyncInvokeTranslate()
    {
        //if auto-detect is sent in the from parameter, then send an empty string to the server
        var originalQuery = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        string fromLanguage = originalQuery.Get("from");
        if(string.Equals(fromLanguage, "Auto-detect", StringComparison.OrdinalIgnoreCase) || string.Equals(fromLanguage, "auto", StringComparison.OrdinalIgnoreCase))
        {
            this.Context.Request.RequestUri = new Uri(this.Context.Request.RequestUri.AbsoluteUri.Replace($"&from={fromLanguage}", ""));
        }

        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

        var responseContentAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        List<TranslationResult> translationResults = JsonConvert.DeserializeObject<List<TranslationResult>>(responseContentAsString);
        var newResponse = new JArray();
        foreach (TranslationResult translationResult in translationResults)
        {
            if (translationResult.Translations[0].Transliteration != null)
            {
                newResponse.Add(JObject.FromObject(new
                {
                    TranslatedText = translationResult.Translations[0].Text,
                    TransliteratedText = translationResult.Translations[0].Transliteration.Text
                }));
            }
            else
            {
                newResponse.Add(JObject.FromObject(new
                {
                    TranslatedText = translationResult.Translations[0].Text
                }));
            }
        }

        response.Content = CreateJsonContent(newResponse.ToString());
        return response;
    }

    //Invokes Document translation batch request
    private async Task<HttpResponseMessage> AsyncInvokeStartDocumentTranslation()
    {
        await this.ExtractRequestInput().ConfigureAwait(false);
        this.Context.Request.Content = CreateJsonContent(JsonConvert.SerializeObject(input));


        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Accepted);
        response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.Accepted)
        {
            var operationLocation = response.Headers.GetValues(OPERATION_LOCATION_HEADER).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(operationLocation))
            {
                var operationLocationUri = new Uri(operationLocation);
                var operationId = operationLocationUri.Segments.Last();
                response.Content = CreateJsonContent("{\"operationID\": \"" + operationId + "\"}");

                return response;
            }
        }

        // if we can not find operation location in the response, return response "as is"
        return response;
    }

    //Gets a list of all translate supported languages
    private async Task<HttpResponseMessage> AsyncInvokeGetTranslateSupportedLanguages()
    {
        this.Context.Request.RequestUri = new Uri(LANGUAGES_URI + TRANSLATION_SCOPE);
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

        var responseContentAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var newBody = GetSupportedLanguages(responseContentAsString);

        response.Content = CreateJsonContent(newBody.ToString());
        return response;
    }

    //Source Language drop down will have auto-detect option along with other Translate supported languages
    private async Task<HttpResponseMessage> AsyncInvokeGetTranslateSupportedSourceLanguages()
    {
        this.Context.Request.RequestUri = new Uri(LANGUAGES_URI + TRANSLATION_SCOPE);
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

        var responseContentAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var newBody = new JArray();
        //add auto-detect for source language
        newBody.Add(JObject.FromObject(new
        {
            Code = "auto",
            Name = "Auto-detect"
        }));
        foreach (var element in GetSupportedLanguages(responseContentAsString))
        {
            newBody.Add(element);
        }

        response.Content = CreateJsonContent(newBody.ToString());
        return response;
    }

    //Gets all Transliterate supported languages
    private async Task<HttpResponseMessage> AsyncInvokeGetTransliterationLanguages()
    {
        this.Context.Request.RequestUri = new Uri(LANGUAGES_URI + TRANSLITERATION_SCOPE);
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

        var responseContentAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        TransliterationLanguages transliterationLanguages = JsonConvert.DeserializeObject<TransliterationLanguages>(responseContentAsString);
        var newBody = new JArray();
        var languagesForTransliteration = transliterationLanguages.transliteration.Keys.ToList();
        foreach (var language in languagesForTransliteration)
        {
            newBody.Add(JObject.FromObject(new
            {
                Code = language,
                Name = transliterationLanguages.transliteration[language].name
            }));
        }
        response.Content = CreateJsonContent(newBody.ToString());
        return response;
    }

    //Gets Transliterate supported FromScripts for a given language
    private async Task<HttpResponseMessage> AsyncInvokeGetTransliterationFromScripts()
    {
        var originalQuery = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        string language = originalQuery.Get("language");

        this.Context.Request.RequestUri = new Uri(LANGUAGES_URI + TRANSLITERATION_SCOPE);
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

        var responseContentAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        TransliterationLanguages transliterationLanguages = JsonConvert.DeserializeObject<TransliterationLanguages>(responseContentAsString);
        List<TransliterateScript> transliterateScripts = transliterationLanguages.transliteration[language].scripts;
        var newBody = new JArray();
        foreach (var script in transliterateScripts)
        {
            newBody.Add(JObject.FromObject(new
            {
                Code = script.code,
                Name = script.name
            }));
        }
        response.Content = CreateJsonContent(newBody.ToString());
        return response;
    }

    //Gets Transliterate supported ToScripts for a given language and FromScript
    private async Task<HttpResponseMessage> AsyncInvokeGetTransliterationToScripts()
    {
        var originalQuery = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        string language = originalQuery.Get("language");
        string fromScript = originalQuery.Get("fromScript");

        this.Context.Request.RequestUri = new Uri(LANGUAGES_URI + TRANSLITERATION_SCOPE);
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

        var responseContentAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var transliterationLanguages = JsonConvert.DeserializeObject<TransliterationLanguages>(responseContentAsString);
        List<TransliterateScript> transliterateScripts = transliterationLanguages.transliteration[language].scripts;

        var newBody = new JArray();
        foreach (var script in transliterateScripts)
        {
            if (string.Compare(script.code, fromScript, StringComparison.OrdinalIgnoreCase) == 0)
            { 
                foreach (var toScript in script.toScripts)
                {
                    newBody.Add(JObject.FromObject(new
                    {
                        Code = toScript.code,
                        Name = toScript.name
                    }));
                }
            }
        }
        response.Content = CreateJsonContent(newBody.ToString());
        return response;
    }

    //Gets all Transliterate supported ToScripts for a given language 
    private async Task<HttpResponseMessage> AsyncInvokeGetTransliterationToScriptsForLanguage()
    {
        var originalQuery = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        string language = originalQuery.Get("language");

        this.Context.Request.RequestUri = new Uri(LANGUAGES_URI + TRANSLITERATION_SCOPE);
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

        var responseContentAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var transliterationLanguages = JsonConvert.DeserializeObject<TransliterationLanguages>(responseContentAsString);
        List<TransliterateScript> transliterateScripts = transliterationLanguages.transliteration[language].scripts;

        var newBody = new JArray();
        foreach (var script in transliterateScripts)
        {
            foreach (var toScript in script.toScripts)
            {
                JObject jo = newBody.Children<JObject>().FirstOrDefault(o => o["Code"].ToString() == toScript.code && o["Name"].ToString() == toScript.name);

                if (jo == null)
                {
                    newBody.Add(JObject.FromObject(new
                    {
                        Code = toScript.code,
                        Name = toScript.name
                    }));
                }
            }
        }
        response.Content = CreateJsonContent(newBody.ToString());
        return response;
    }

    //Gets a list of all supported Glossary format and versions
    private async Task<HttpResponseMessage> AsyncInvokeGetGlossaryFormatVersion()
    {
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        string host = this.Context.Request.RequestUri.Scheme + "://" + this.Context.Request.RequestUri.IdnHost;
        var request = new HttpRequestMessage(HttpMethod.Get, host + "/translator/text/batch/v1.1/glossaries/formats");

        foreach (var header in this.Context.Request.Headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
        var responseContentAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var fileFormats = JsonConvert.DeserializeObject<SupportedFileFormats>(responseContentAsString);

        var newBody = new JArray();
        foreach (var fileFormat in fileFormats.Value)
        {
            if (fileFormat.Versions != null)
            {
                foreach (string version in fileFormat.Versions)
                {
                    newBody.Add(JObject.FromObject(new
                    {
                        FormatVersionCode = fileFormat.Format + "_" + version,//sent to backend
                        FormatVersionName = fileFormat.Format + "_" + version //Displayed in UI
                    }));
                }
            }
            else
            {
                newBody.Add(JObject.FromObject(new
                {
                    FormatVersionCode = fileFormat.Format,//sent to backend
                    FormatVersionName = fileFormat.Format //Displayed in UI
                }));
            }
        }
        response.Content = CreateJsonContent(newBody.ToString());
        return response;
    }

    //Extract the code and name for each language from the response string
    private JArray GetSupportedLanguages(string responseContentAsString)
    {
        var newBody = new JArray();
        Languages languages = JsonConvert.DeserializeObject<Languages>(responseContentAsString);
        var languagesForTranslation = languages.translation.Keys.ToList();
        foreach (var language in languagesForTranslation)
        {
            newBody.Add(JObject.FromObject(new
            {
                Code = language,
                Name = languages.translation[language].name
            }));
        }
        return newBody;
    }

    //Extract the user entered input details to IncomingRequest object
    private async Task ExtractRequestInput()
    {
        var requestContentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var requestContentAsJson = JObject.Parse(requestContentAsString);

        IncomingRequest incomingRequest = new IncomingRequest();
        if (requestContentAsJson.ContainsKey(nameof(IncomingRequest.storageType)))
        {
            incomingRequest.storageType = requestContentAsJson[nameof(IncomingRequest.storageType)].ToString();
        }
        if (requestContentAsJson.ContainsKey(nameof(IncomingRequest.sourceLanguage)))
        {
            if ((requestContentAsJson[nameof(IncomingRequest.sourceLanguage)].ToString() == "auto") ||
                (requestContentAsJson[nameof(IncomingRequest.sourceLanguage)].ToString() == "Auto-detect"))
            {
                incomingRequest.sourceLanguage = null;
            }
            else
            {
                incomingRequest.sourceLanguage = requestContentAsJson[nameof(IncomingRequest.sourceLanguage)].ToString();
            }
        }
        if (requestContentAsJson.ContainsKey(nameof(IncomingRequest.sourceURL)))
        {
            incomingRequest.sourceURL = requestContentAsJson[nameof(IncomingRequest.sourceURL)].ToString();
        }
        if (requestContentAsJson.ContainsKey(nameof(IncomingRequest.documentFilterPrefix)))
        {
            incomingRequest.documentFilterPrefix = requestContentAsJson[nameof(IncomingRequest.documentFilterPrefix)].ToString();
        }
        if (requestContentAsJson.ContainsKey(nameof(IncomingRequest.documentFilterSuffix)))
        {
            incomingRequest.documentFilterSuffix = requestContentAsJson[nameof(IncomingRequest.documentFilterSuffix)].ToString();
        }
        if (requestContentAsJson.ContainsKey(nameof(IncomingRequest.targetCategory)))
        {
            incomingRequest.targetCategory = requestContentAsJson[nameof(IncomingRequest.targetCategory)].ToString();
        }
        if (requestContentAsJson.ContainsKey(nameof(IncomingRequest.targetContainerURL)))
        {
            incomingRequest.targetContainerURL = requestContentAsJson[nameof(IncomingRequest.targetContainerURL)].ToString();
        }
        if (requestContentAsJson.ContainsKey(nameof(IncomingRequest.targetLanguage)))
        {
            incomingRequest.targetLanguage = requestContentAsJson[nameof(IncomingRequest.targetLanguage)].ToString();
        }
        if (requestContentAsJson.ContainsKey(nameof(IncomingRequest.glossaryUrl)))
        {
            incomingRequest.glossaryUrl = requestContentAsJson[nameof(IncomingRequest.glossaryUrl)].ToString();
        }
        if (requestContentAsJson.ContainsKey(nameof(IncomingRequest.glossaryFormatVersion)))
        {
            incomingRequest.glossaryFormatVersion = requestContentAsJson[nameof(IncomingRequest.glossaryFormatVersion)].ToString();
        }
        CreateRequestInput(incomingRequest);
    }

    //Construct the Batch request for Document Translation
    private void CreateRequestInput(IncomingRequest incomingRequest)
    {
        TargetInput targetinput = new TargetInput();
        targetinput.category = incomingRequest.targetCategory;
        targetinput.targetUrl = getTargetURL(incomingRequest.storageType, incomingRequest.sourceURL, incomingRequest.targetContainerURL, incomingRequest.targetLanguage);
        targetinput.language = incomingRequest.targetLanguage;
        if ((incomingRequest.glossaryUrl != null) && (incomingRequest.glossaryFormatVersion != null))
        {
            string[] result = incomingRequest.glossaryFormatVersion.Split('_');
            targetinput.glossaries = new List<Glossary>()
            {
                new Glossary()
                {
                    glossaryUrl = incomingRequest.glossaryUrl,
                    format = (result.ElementAtOrDefault(0) != null) ? result[0] : null, //XLIFF from XLIFF_1.2, CSV from CSV
                    version = (result.ElementAtOrDefault(1) != null) ? result[1] : null //1.2 from XLIFF_1.2, null from CSV
                }
            };
        }

        input.inputs = new List<BatchRequest>()
        {
            new BatchRequest()
            {
                storageType = incomingRequest.storageType,
                source = new SourceInput()
                {
                    sourceUrl = incomingRequest.sourceURL,
                    language = incomingRequest.sourceLanguage,
                    filter = new DocumentFilter()
                    {
                        prefix = incomingRequest.documentFilterPrefix,
                        suffix = incomingRequest.documentFilterSuffix
                    }
                },
                targets = new List<TargetInput>()
                {
                    targetinput
                }
            }
        };
    }

    //Contruct the target URL based on the storage type options
    private static string getTargetURL(string storageType, string sourceURL, string targetContainerURL, string targetLang)
    {
        Uri sourceUri = new Uri(sourceURL);
        UriBuilder targetContainerUri = new UriBuilder(targetContainerURL);
        targetContainerUri.Port = -1;
        if (storageType == "File")
        {
            //get the source file name from sourceURL
            string sourceFileName = Path.GetFileName(sourceUri.LocalPath);

            //create target url for StorageType = File
            targetContainerUri.Path = targetContainerUri.Path.TrimEnd('/') + "/" + targetLang + "/" + sourceFileName;
        }
        else
        {
            //create target url for StorageType = Folder
            targetContainerUri.Path = targetContainerUri.Path.TrimEnd('/') + "/" + targetLang;
        }
        return targetContainerUri.ToString();
    }
}