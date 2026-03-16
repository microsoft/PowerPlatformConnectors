using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    // Cache compiled regex for better performance
    private static readonly Regex WhitespaceRegex = new Regex(@"\s+", RegexOptions.Compiled);
    private static readonly Regex SoundEffectsRegex = new Regex(@"\[.*?\]", RegexOptions.Compiled);
    private static readonly Regex BackgroundNoiseRegex = new Regex(@"\(.*?\)", RegexOptions.Compiled);
    
    // Constants for better maintainability
    private const string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
    private const string DefaultLanguage = "English (auto-generated)";
    private const int MaxSearchDepth = 5;
    
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        this.Context.Logger.LogInformation("YouTube Transcript: Starting processing");
        try
        {
            var modifiedRequest = await ProcessRequest(this.Context.Request);
            var response = await this.Context.SendAsync(modifiedRequest, this.CancellationToken).ConfigureAwait(false);
            
            this.Context.Logger.LogInformation($"YouTube API response: {response.StatusCode}");
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                this.Context.Logger.LogWarning($"API error: {response.StatusCode} - {errorContent}");
                return CreateErrorResponse(response.StatusCode, "YouTube API request failed");
            }

            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var transformedResponse = TransformYouTubeResponse(responseString);
            response.Content = CreateJsonContent(transformedResponse);
            
            this.Context.Logger.LogInformation("Response transformation completed successfully");
            return response;
        }
        catch (Exception ex)
        {
            this.Context.Logger.LogError($"Processing error: {ex.Message}");
            return CreateErrorResponse(HttpStatusCode.InternalServerError, $"Processing error: {ex.Message}");
        }
    }

    private async Task<HttpRequestMessage> ProcessRequest(HttpRequestMessage originalRequest)
    {
        if (originalRequest.Content == null)
            return originalRequest;

        try
        {
            var contentString = await originalRequest.Content.ReadAsStringAsync().ConfigureAwait(false);
            var requestJson = JObject.Parse(contentString);

            var videoId = requestJson["externalVideoId"]?.ToString();
            var existingParams = requestJson["params"]?.ToString();
            
            // Generate params if needed
            if (ShouldGenerateParams(existingParams) && !string.IsNullOrWhiteSpace(videoId))
            {
                requestJson["params"] = GenerateYouTubeParams(videoId);
            }

            // Ensure context is set
            EnsureContext(requestJson);

            var newRequest = CreateRequestWithHeaders(originalRequest, requestJson.ToString(), videoId);
            return newRequest;
        }
        catch (Exception ex)
        {
            this.Context.Logger.LogError($"Request processing error: {ex.Message}");
            return originalRequest;
        }
    }

    private bool ShouldGenerateParams(string existingParams)
    {
        return string.IsNullOrWhiteSpace(existingParams) || 
               existingParams == "AUTO_GENERATE" || 
               !IsValidBase64(existingParams);
    }

    private void EnsureContext(JObject requestJson)
    {
        if (requestJson["context"] == null)
        {
            requestJson["context"] = new JObject
            {
                ["client"] = new JObject
                {
                    ["clientName"] = "WEB",
                    ["clientVersion"] = "2.20250923.08.00"
                }
            };
        }
    }

    private HttpRequestMessage CreateRequestWithHeaders(HttpRequestMessage originalRequest, string content, string videoId)
    {
        var newRequest = new HttpRequestMessage(originalRequest.Method, originalRequest.RequestUri);
        
        // Add YouTube-specific headers for bot detection avoidance
        var headers = new Dictionary<string, string>
        {
            ["User-Agent"] = DefaultUserAgent,
            ["Accept"] = "application/json",
            ["Accept-Language"] = "en-US,en;q=0.9",
            ["Origin"] = "https://www.youtube.com",
            ["Referer"] = $"https://www.youtube.com/watch?v={videoId}",
            ["Sec-Ch-Ua"] = "\"Not_A Brand\";v=\"99\", \"Google Chrome\";v=\"109\", \"Chromium\";v=\"109\"",
            ["Sec-Ch-Ua-Mobile"] = "?0",
            ["Sec-Ch-Ua-Platform"] = "\"Windows\"",
            ["Sec-Fetch-Dest"] = "empty",
            ["Sec-Fetch-Mode"] = "cors",
            ["Sec-Fetch-Site"] = "same-origin"
        };

        foreach (var header in headers)
        {
            newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        // Copy non-conflicting original headers
        foreach (var header in originalRequest.Headers)
        {
            if (!headers.ContainsKey(header.Key))
            {
                newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        newRequest.Content = new StringContent(content, Encoding.UTF8, "application/json");
        return newRequest;
    }

    private string GenerateYouTubeParams(string videoId)
    {
        try
        {
            // Optimized protobuf generation using pre-calculated byte arrays
            var inner = new List<byte>(20); // Pre-allocate capacity
            
            // Field 1: "asr"
            inner.AddRange(new byte[] { 0x0A, 0x03 });
            inner.AddRange(Encoding.UTF8.GetBytes("asr"));
            
            // Field 2: "en" 
            inner.AddRange(new byte[] { 0x12, 0x02 });
            inner.AddRange(Encoding.UTF8.GetBytes("en"));
            
            // Field 3: empty bytes
            inner.AddRange(new byte[] { 0x1A, 0x00 });

            var innerB64 = ToBase64Url(inner.ToArray());
            var innerB64UrlEncoded = Uri.EscapeDataString(innerB64);

            var outer = new List<byte>(50); // Pre-allocate capacity
            
            // Field 1: videoId
            outer.Add(0x0A);
            outer.Add((byte)videoId.Length);
            outer.AddRange(Encoding.UTF8.GetBytes(videoId));
            
            // Field 2: encoded inner params
            outer.Add(0x12);
            var innerBytes = Encoding.UTF8.GetBytes(innerB64UrlEncoded);
            outer.Add((byte)innerBytes.Length);
            outer.AddRange(innerBytes);

            return ToBase64Url(outer.ToArray());
        }
        catch (Exception ex)
        {
            this.Context.Logger.LogWarning($"Params generation failed, using fallback: {ex.Message}");
            return "CgtEQzJwM2tGamNLMBISQ2dOaGMzSVNBbVZ1R2dBJTNE"; // Fallback
        }
    }

    private bool IsValidBase64(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        try
        {
            var base64 = value.Replace('-', '+').Replace('_', '/');
            while (base64.Length % 4 != 0)
                base64 += "=";
            
            Convert.FromBase64String(base64);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private string ToBase64Url(byte[] bytes) =>
        Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');

    private string TransformYouTubeResponse(string youtubeResponse)
    {
        try
        {
            var youtubeData = JObject.Parse(youtubeResponse);
            var segments = ExtractTranscriptSegments(youtubeData);
            
            var result = new JObject
            {
                ["success"] = true,
                ["segments"] = JArray.FromObject(segments),
                ["totalSegments"] = segments.Count,
                ["totalDurationMs"] = segments.Count > 0 ? segments.Last().EndMs : 0,
                ["totalDurationFormatted"] = segments.Count > 0 ? FormatDuration(segments.Last().EndMs) : "0:00",
                ["fullTranscript"] = string.Join(" ", segments.Select(s => s.Text)),
                ["language"] = ExtractLanguageInfo(youtubeData),
                ["processedAt"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            };

            return result.ToString();
        }
        catch (Exception ex)
        {
            this.Context.Logger.LogError($"Response transformation error: {ex.Message}");
            return CreateFallbackResponse(youtubeResponse, ex.Message).ToString();
        }
    }

    private List<TranscriptSegment> ExtractTranscriptSegments(JObject youtubeData)
    {
        var segments = new List<TranscriptSegment>();

        // Try primary navigation path
        if (TryExtractFromActions(youtubeData, segments))
        {
            this.Context.Logger.LogInformation($"Primary extraction successful: {segments.Count} segments found");
            return segments.OrderBy(s => s.StartMs).ToList();
        }

        // Fallback: deep search with early termination
        this.Context.Logger.LogInformation("Primary extraction failed, attempting deep search");
        SearchForTranscriptContentOptimized(youtubeData, segments, 0, MaxSearchDepth);
        this.Context.Logger.LogInformation($"Deep search completed: {segments.Count} segments found");
        
        return segments.OrderBy(s => s.StartMs).ToList();
    }

    private bool TryExtractFromActions(JObject youtubeData, List<TranscriptSegment> segments)
    {
        try
        {
            // Log the top-level structure for debugging
            this.Context.Logger.LogInformation($"YouTube response top-level keys: {string.Join(", ", youtubeData.Properties().Select(p => p.Name))}");
            
            var actions = youtubeData["actions"];
            this.Context.Logger.LogInformation($"Actions found: {actions != null}, HasValues: {actions?.HasValues}");
            
            if (actions?.HasValues == true)
            {
                this.Context.Logger.LogInformation($"Found {actions.Count()} actions");
                var firstAction = actions[0];
                this.Context.Logger.LogInformation($"First action keys: {string.Join(", ", ((JObject)firstAction).Properties().Select(p => p.Name))}");
                
                var updateAction = firstAction["updateEngagementPanelAction"];
                this.Context.Logger.LogInformation($"UpdateEngagementPanelAction found: {updateAction != null}");
                
                if (updateAction != null)
                {
                    var content = updateAction["content"];
                    this.Context.Logger.LogInformation($"Content found: {content != null}");
                    
                    if (content != null)
                    {
                        this.Context.Logger.LogInformation($"Content keys: {string.Join(", ", ((JObject)content).Properties().Select(p => p.Name))}");
                        var transcriptRenderer = content["transcriptRenderer"];
                        this.Context.Logger.LogInformation($"TranscriptRenderer found: {transcriptRenderer != null}");
                        
                        if (transcriptRenderer != null)
                        {
                            this.Context.Logger.LogInformation($"TranscriptRenderer keys: {string.Join(", ", ((JObject)transcriptRenderer).Properties().Select(p => p.Name))}");
                        }
                    }
                }
            }

            var cueGroups = youtubeData["actions"]?[0]?["updateEngagementPanelAction"]?["content"]?
                ["transcriptRenderer"]?["body"]?["transcriptBodyRenderer"]?["cueGroups"];

            if (cueGroups?.HasValues == true)
            {
                this.Context.Logger.LogInformation($"Found {cueGroups.Count()} cue groups in primary path");
                foreach (var cueGroup in cueGroups)
                {
                    var segment = CreateTranscriptSegment(cueGroup["transcriptSegmentRenderer"]);
                    if (segment != null)
                        segments.Add(segment);
                }
                return segments.Count > 0;
            }

            // Try alternative structure
            var altTranscriptRenderer = youtubeData["actions"]?[0]?["updateEngagementPanelAction"]?["content"]?["transcriptRenderer"];
            if (altTranscriptRenderer != null)
            {
                this.Context.Logger.LogInformation("Trying alternative structure extraction");
                return TryExtractFromAlternativeStructure(altTranscriptRenderer, segments);
            }
        }
        catch (Exception ex)
        {
            this.Context.Logger.LogWarning($"Primary extraction failed: {ex.Message}");
        }

        return false;
    }

    private bool TryExtractFromAlternativeStructure(JToken transcriptRenderer, List<TranscriptSegment> segments)
    {
        this.Context.Logger.LogInformation($"Alternative structure - TranscriptRenderer keys: {string.Join(", ", ((JObject)transcriptRenderer).Properties().Select(p => p.Name))}");
        
        var content = transcriptRenderer["content"];
        if (content != null)
        {
            this.Context.Logger.LogInformation($"Content keys: {string.Join(", ", ((JObject)content).Properties().Select(p => p.Name))}");
            
            var searchPanel = content["transcriptSearchPanelRenderer"];
            if (searchPanel != null)
            {
                this.Context.Logger.LogInformation("Found transcriptSearchPanelRenderer, attempting extraction");
                this.Context.Logger.LogInformation($"SearchPanel keys: {string.Join(", ", ((JObject)searchPanel).Properties().Select(p => p.Name))}");
                
                // Look for body in search panel
                var body = searchPanel["body"];
                if (body != null)
                {
                    this.Context.Logger.LogInformation($"SearchPanel body keys: {string.Join(", ", ((JObject)body).Properties().Select(p => p.Name))}");
                    
                    var bodyRenderer = body["transcriptSegmentListRenderer"];
                    if (bodyRenderer != null)
                    {
                        this.Context.Logger.LogInformation($"TranscriptSegmentListRenderer keys: {string.Join(", ", ((JObject)bodyRenderer).Properties().Select(p => p.Name))}");
                        
                        var initialSegments = bodyRenderer["initialSegments"];
                        if (initialSegments?.HasValues == true)
                        {
                            this.Context.Logger.LogInformation($"Found {initialSegments.Count()} initial segments");
                            
                            foreach (var segmentItem in initialSegments)
                            {
                                var renderer = segmentItem["transcriptSegmentRenderer"];
                                if (renderer != null)
                                {
                                    var segment = CreateTranscriptSegment(renderer);
                                    if (segment != null)
                                        segments.Add(segment);
                                }
                            }
                            
                            if (segments.Count > 0)
                            {
                                this.Context.Logger.LogInformation($"Extracted {segments.Count} segments from initialSegments");
                                return true;
                            }
                        }
                    }
                }
                
                // If no segments found in structured approach, try deep search
                SearchForTranscriptContentOptimized(searchPanel, segments, 0, 3);
                if (segments.Count > 0)
                {
                    this.Context.Logger.LogInformation($"Extracted {segments.Count} segments from search panel deep search");
                    return true;
                }
            }

            // Try direct content search as fallback
            SearchForTranscriptContentOptimized(content, segments, 0, 3);
            if (segments.Count > 0)
            {
                this.Context.Logger.LogInformation($"Extracted {segments.Count} segments from direct content");
                return true;
            }
        }

        return false;
    }

    private void SearchForTranscriptContentOptimized(JToken token, List<TranscriptSegment> segments, int depth, int maxDepth)
    {
        if (depth > maxDepth || token == null || segments.Count > 1000) // Early termination
            return;

        if (token.Type == JTokenType.Object)
        {
            var obj = (JObject)token;
            
            // Quick check for transcript patterns
            if (obj.TryGetValue("startMs", out var startMsToken) && 
                (obj.TryGetValue("snippet", out _) || obj.TryGetValue("text", out _)))
            {
                var segment = CreateTranscriptSegment(obj);
                if (segment != null)
                    segments.Add(segment);
            }

            // Continue searching efficiently
            foreach (var property in obj.Properties())
            {
                if (segments.Count > 1000) break; // Prevent excessive processing
                SearchForTranscriptContentOptimized(property.Value, segments, depth + 1, maxDepth);
            }
        }
        else if (token.Type == JTokenType.Array)
        {
            foreach (var item in (JArray)token)
            {
                if (segments.Count > 1000) break;
                SearchForTranscriptContentOptimized(item, segments, depth + 1, maxDepth);
            }
        }
    }

    private TranscriptSegment CreateTranscriptSegment(JToken renderer)
    {
        if (renderer == null)
            return null;

        try
        {
            var startMs = ParseMilliseconds(renderer["startMs"]?.ToString());
            var endMs = ParseMilliseconds(renderer["endMs"]?.ToString());
            var text = ExtractTextFromRuns(renderer["snippet"]?["runs"]) ?? 
                      renderer["text"]?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(text))
                return null;

            var cleanText = CleanTranscriptTextOptimized(text);
            var durationMs = endMs - startMs;

            return new TranscriptSegment
            {
                Text = cleanText,
                StartMs = startMs,
                EndMs = endMs,
                DurationMs = durationMs,
                StartTime = renderer["startTimeText"]?["simpleText"]?.ToString() ?? "",
                StartTimeFormatted = FormatDuration(startMs),
                EndTimeFormatted = FormatDuration(endMs),
                DurationFormatted = FormatDuration(durationMs),
                WordCount = CountWordsOptimized(cleanText),
                CharacterCount = cleanText.Length
            };
        }
        catch
        {
            return null;
        }
    }

    private string ExtractTextFromRuns(JToken runs)
    {
        if (runs?.HasValues != true)
            return "";

        var sb = new StringBuilder();
        foreach (var run in runs)
        {
            var text = run["text"]?.ToString();
            if (!string.IsNullOrEmpty(text))
                sb.Append(text);
        }
        
        return sb.ToString();
    }

    private string CleanTranscriptTextOptimized(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";

        text = text.Trim()
                  .Replace("♪", "");
        
        // Use compiled regexes for better performance
        text = WhitespaceRegex.Replace(text, " ");
        text = SoundEffectsRegex.Replace(text, "");
        text = BackgroundNoiseRegex.Replace(text, "");
        
        return text.Trim();
    }

    private int CountWordsOptimized(string text) =>
        string.IsNullOrWhiteSpace(text) ? 0 : 
        text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;

    private int ParseMilliseconds(string msString) =>
        int.TryParse(msString, out int ms) ? ms : 0;

    private string FormatDuration(int milliseconds)
    {
        var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
        return timeSpan.TotalHours >= 1 ? 
            $"{(int)timeSpan.TotalHours}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}" :
            $"{timeSpan.Minutes}:{timeSpan.Seconds:D2}";
    }

    private string ExtractLanguageInfo(JObject youtubeData)
    {
        try
        {
            var languageMenu = youtubeData["actions"]?[0]?["updateEngagementPanelAction"]?["content"]?
                ["transcriptRenderer"]?["footer"]?["transcriptFooterRenderer"]?["languageMenu"];
            
            if (languageMenu != null)
            {
                var selectedItem = languageMenu["sortFilterSubMenuRenderer"]?["subMenuItems"]?
                    .FirstOrDefault(item => item["selected"]?.Value<bool>() == true);
                
                return selectedItem?["title"]?.ToString() ?? DefaultLanguage;
            }
        }
        catch
        {
            // Graceful fallback
        }

        return DefaultLanguage;
    }

    private JObject CreateFallbackResponse(string originalResponse, string errorMessage) =>
        new JObject
        {
            ["success"] = false,
            ["error"] = $"Response transformation failed: {errorMessage}",
            ["segments"] = new JArray(),
            ["totalSegments"] = 0,
            ["processedAt"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            ["rawResponseSample"] = originalResponse.Length > 2000 ? 
                originalResponse.Substring(0, 2000) + "..." : originalResponse
        };

    private HttpResponseMessage CreateErrorResponse(HttpStatusCode statusCode, string message)
    {
        var errorResponse = new HttpResponseMessage(statusCode);
        var errorObject = new JObject
        {
            ["success"] = false,
            ["error"] = message,
            ["segments"] = new JArray(),
            ["totalSegments"] = 0,
            ["processedAt"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
        
        errorResponse.Content = CreateJsonContent(errorObject.ToString());
        return errorResponse;
    }

    private class TranscriptSegment
    {
        public string Text { get; set; }
        public int StartMs { get; set; }
        public int EndMs { get; set; }
        public int DurationMs { get; set; }
        public string StartTime { get; set; }
        public string StartTimeFormatted { get; set; }
        public string EndTimeFormatted { get; set; }
        public string DurationFormatted { get; set; }
        public int WordCount { get; set; }
        public int CharacterCount { get; set; }
    }
}