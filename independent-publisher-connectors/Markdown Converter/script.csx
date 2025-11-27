using System;
using System.Collections.Generic;
using System.IO;
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
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Get the real operation ID (handle base64 encoding as per Microsoft docs)
        string realOperationId = this.Context.OperationId;
        string originalOperationId = realOperationId;

        // First, check if it's already a known operation ID
        if (!IsKnownOperationId(realOperationId))
        {
            // Only attempt base64 decoding if the string could be base64 and doesn't look like a normal operation ID
            if (IsLikelyBase64(realOperationId) && !IsValidOperationId(realOperationId))
            {
                try
                {
                    byte[] data = Convert.FromBase64String(realOperationId);
                    string decoded = System.Text.Encoding.UTF8.GetString(data);

                    // Only use decoded value if it produces a valid operation ID
                    if (IsValidOperationId(decoded) && IsKnownOperationId(decoded))
                    {
                        realOperationId = decoded;
                    }
                }
                catch (Exception)
                {
                    // Any exception means we should use the original value
                    realOperationId = originalOperationId;
                }
            }
        }

        // Check operation ID and route to appropriate handler
        if (realOperationId == "MarkdownToHtml")
        {
            return await this.HandleMarkdownToHtml().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToAdaptiveCard")
        {
            return await this.HandleMarkdownToAdaptiveCard().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToChart")
        {
            return await this.HandleMarkdownToChart().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToQr")
        {
            return await this.HandleMarkdownToQR().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToJson")
        {
            return await this.HandleMarkdownToJson().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToXml")
        {
            return await this.HandleMarkdownToXml().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToPlainText")
        {
            return await this.HandleMarkdownToPlainText().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToCsv")
        {
            return await this.HandleMarkdownToCsv().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToLaTeX")
        {
            return await this.HandleMarkdownToLaTeX().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToYaml")
        {
            return await this.HandleMarkdownToEmail().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToEmail")
        {
            return await this.HandleMarkdownToEmail().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToSvg")
        {
            return await this.HandleMarkdownToSvg().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToRss")
        {
            return await this.HandleMarkdownToRss().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToWiki")
        {
            return await this.HandleMarkdownToWiki().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToPng")
        {
            return await this.HandleMarkdownToPng().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToDiagram")
        {
            return await this.HandleMarkdownToDiagram().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownStats")
        {
            return await this.HandleMarkdownStats().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToJpeg")
        {
            return await this.HandleMarkdownToJpeg().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToBadge")
        {
            return await this.HandleMarkdownToBadge().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToInfographic")
        {
            return await this.HandleMarkdownToInfographic().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToLog")
        {
            return await this.HandleMarkdownToLog().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToMetrics")
        {
            return await this.HandleMarkdownToMetrics().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToSyslog")
        {
            return await this.HandleMarkdownToSyslog().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToJsDoc")
        {
            return await this.HandleMarkdownToJsDoc().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToXmlDoc")
        {
            return await this.HandleMarkdownToXmlDoc().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToReadme")
        {
            return await this.HandleMarkdownToReadme().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToChangelog")
        {
            return await this.HandleMarkdownToChangelog().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToTableOfContents")
        {
            return await this.HandleMarkdownToTableOfContents().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownToStyledHtml")
        {
            return await this.HandleMarkdownToStyledHtml().ConfigureAwait(false);
        }
        else if (realOperationId == "MarkdownInfo")
        {
            return await this.HandleMarkdownInfo().ConfigureAwait(false);
        }
        // Handle unknown operation ID
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent(new JObject
        {
            ["error"] = $"Unknown operation ID '{realOperationId}'",
            ["receivedOperationId"] = realOperationId,
            ["originalOperationId"] = originalOperationId,
            ["operationIdLength"] = realOperationId?.Length ?? 0,
            ["originalOperationIdLength"] = originalOperationId?.Length ?? 0,
            ["isValidFormat"] = IsValidOperationId(realOperationId ?? ""),
            ["availableOperations"] = new JArray("MarkdownToHtml", "MarkdownToAdaptiveCard", "MarkdownToChart", "MarkdownToQr", "MarkdownToJson", "MarkdownToXml", "MarkdownToPlainText", "MarkdownToCsv", "MarkdownToLaTeX", "MarkdownToYaml", "MarkdownToEmail", "MarkdownToSvg", "MarkdownToRss", "MarkdownToWiki", "MarkdownToPng", "MarkdownToDiagram", "MarkdownStats", "MarkdownToJpeg", "MarkdownToBadge", "MarkdownToInfographic", "MarkdownToLog", "MarkdownToMetrics", "MarkdownToSyslog", "MarkdownToJsDoc", "MarkdownToXmlDoc", "MarkdownToReadme", "MarkdownToChangelog", "MarkdownToTableOfContents", "MarkdownToStyledHtml", "MarkdownInfo"),
            ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        }.ToString());
        return response;
    }

    private async Task<HttpResponseMessage> HandleMarkdownToHtml()
    {
        try
        {
            // Read request content
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            var markdown = (string)contentAsJson["markdown"] ?? (string)contentAsJson["text"] ?? "";

            if (string.IsNullOrEmpty(markdown))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {
                    ["error"] = "No markdown content provided. Please include 'markdown' or 'text' field in request body.",
                    ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }.ToString());
                return errorResponse;
            }

            // Convert markdown to HTML
            var html = ConvertToHtml(markdown);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["originalMarkdown"] = markdown,
                ["html"] = html,
                ["format"] = "html",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());

            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {
                ["error"] = $"Error processing request: {ex.Message}",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return errorResponse;
        }
    }

    private async Task<HttpResponseMessage> HandleMarkdownToAdaptiveCard()
    {
        try
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            var markdown = (string)contentAsJson["markdown"] ?? (string)contentAsJson["text"] ?? "";

            if (string.IsNullOrEmpty(markdown))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {
                    ["error"] = "No markdown content provided",
                    ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }.ToString());
                return errorResponse;
            }

            // Convert to Adaptive Card
            var adaptiveCard = ConvertMarkdownToAdaptiveCardJson(markdown);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["originalMarkdown"] = markdown,
                ["adaptiveCard"] = adaptiveCard,
                ["format"] = "adaptiveCard",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());

            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {
                ["error"] = $"Error processing request: {ex.Message}",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return errorResponse;
        }
    }

    private async Task<HttpResponseMessage> HandleMarkdownToChart()
    {
        try
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            var markdown = (string)contentAsJson["markdown"] ?? (string)contentAsJson["text"] ?? "";

            if (string.IsNullOrEmpty(markdown))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {
                    ["error"] = "No markdown content provided",
                    ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }.ToString());
                return errorResponse;
            }

            // Generate chart
            var chartBase64 = ConvertToChart(markdown);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["originalMarkdown"] = markdown,
                ["chartBase64"] = chartBase64,
                ["format"] = "chart",
                ["mimeType"] = "image/png",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());

            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {
                ["error"] = $"Error processing request: {ex.Message}",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return errorResponse;
        }
    }

    private async Task<HttpResponseMessage> HandleMarkdownToQR()
    {
        try
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            var markdown = (string)contentAsJson["markdown"] ?? (string)contentAsJson["text"] ?? "";

            if (string.IsNullOrEmpty(markdown))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {
                    ["error"] = "No markdown content provided",
                    ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }.ToString());
                return errorResponse;
            }

            // Generate QR code
            var qrBase64 = ConvertToQRCode(markdown);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["originalMarkdown"] = markdown,
                ["qrBase64"] = qrBase64,
                ["format"] = "qr",
                ["mimeType"] = "image/png",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());

            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {
                ["error"] = $"Error processing request: {ex.Message}",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return errorResponse;
        }
    }

    private async Task<HttpResponseMessage> HandleMarkdownToJson()
    {
        try
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            var markdown = (string)contentAsJson["markdown"] ?? (string)contentAsJson["text"] ?? "";

            if (string.IsNullOrEmpty(markdown))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {
                    ["error"] = "No markdown content provided",
                    ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }.ToString());
                return errorResponse;
            }

            // Convert to JSON structure
            var jsonStructure = ConvertToJsonStructure(markdown);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["originalMarkdown"] = markdown,
                ["jsonStructure"] = jsonStructure,
                ["format"] = "json",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());

            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {
                ["error"] = $"Error processing request: {ex.Message}",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return errorResponse;
        }
    }

    private async Task<HttpResponseMessage> HandleMarkdownToXml()
    {
        try
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            var markdown = (string)contentAsJson["markdown"] ?? (string)contentAsJson["text"] ?? "";

            if (string.IsNullOrEmpty(markdown))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {
                    ["error"] = "No markdown content provided",
                    ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }.ToString());
                return errorResponse;
            }

            // Convert to XML
            var xml = ConvertToXml(markdown);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["originalMarkdown"] = markdown,
                ["xml"] = xml,
                ["format"] = "xml",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());

            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {
                ["error"] = $"Error processing request: {ex.Message}",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return errorResponse;
        }
    }

    private async Task<HttpResponseMessage> HandleMarkdownToPlainText()
    {
        try
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            var markdown = (string)contentAsJson["markdown"] ?? (string)contentAsJson["text"] ?? "";

            if (string.IsNullOrEmpty(markdown))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {
                    ["error"] = "No markdown content provided",
                    ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }.ToString());
                return errorResponse;
            }

            // Convert to plain text
            var plainText = ConvertToPlainText(markdown);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["originalMarkdown"] = markdown,
                ["plainText"] = plainText,
                ["format"] = "plainText",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());

            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {
                ["error"] = $"Error processing request: {ex.Message}",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return errorResponse;
        }
    }

    // Additional format handlers
    private async Task<HttpResponseMessage> HandleMarkdownToCsv()
    {
        try
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            var markdown = (string)contentAsJson["markdown"] ?? (string)contentAsJson["text"] ?? "";

            if (string.IsNullOrEmpty(markdown))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {
                    ["error"] = "No markdown content provided",
                    ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }.ToString());
                return errorResponse;
            }

            var csv = ConvertToCsv(markdown);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["originalMarkdown"] = markdown,
                ["csv"] = csv,
                ["format"] = "csv",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {
                ["error"] = $"Error processing request: {ex.Message}",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return errorResponse;
        }
    }

    private async Task<HttpResponseMessage> HandleMarkdownToLaTeX()
    {
        try
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            var markdown = (string)contentAsJson["markdown"] ?? (string)contentAsJson["text"] ?? "";

            if (string.IsNullOrEmpty(markdown))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {
                    ["error"] = "No markdown content provided",
                    ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }.ToString());
                return errorResponse;
            }

            var latex = ConvertToLaTeX(markdown);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["originalMarkdown"] = markdown,
                ["latex"] = latex,
                ["format"] = "latex",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {
                ["error"] = $"Error processing request: {ex.Message}",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return errorResponse;
        }
    }

    private async Task<HttpResponseMessage> HandleMarkdownToYaml()
    {
        try
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            var markdown = (string)contentAsJson["markdown"] ?? (string)contentAsJson["text"] ?? "";

            if (string.IsNullOrEmpty(markdown))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {
                    ["error"] = "No markdown content provided",
                    ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }.ToString());
                return errorResponse;
            }

            var yaml = ConvertToYaml(markdown);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["originalMarkdown"] = markdown,
                ["yaml"] = yaml,
                ["format"] = "yaml",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {
                ["error"] = $"Error processing request: {ex.Message}",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return errorResponse;
        }
    }

    private async Task<HttpResponseMessage> HandleMarkdownToEmail()
    {
        try
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            var markdown = (string)contentAsJson["markdown"] ?? (string)contentAsJson["text"] ?? "";

            if (string.IsNullOrEmpty(markdown))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {
                    ["error"] = "No markdown content provided",
                    ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }.ToString());
                return errorResponse;
            }

            var email = ConvertToEmail(markdown);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["originalMarkdown"] = markdown,
                ["emailHtml"] = email,
                ["format"] = "email",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {
                ["error"] = $"Error processing request: {ex.Message}",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return errorResponse;
        }
    }

    private async Task<HttpResponseMessage> HandleMarkdownStats()
    {
        try
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            var markdown = (string)contentAsJson["markdown"] ?? (string)contentAsJson["text"] ?? "";

            if (string.IsNullOrEmpty(markdown))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {
                    ["error"] = "No markdown content provided",
                    ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }.ToString());
                return errorResponse;
            }

            var stats = GetMarkdownStats(markdown);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["originalMarkdown"] = markdown,
                ["stats"] = stats,
                ["format"] = "stats",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {
                ["error"] = $"Error processing request: {ex.Message}",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return errorResponse;
        }
    }

    private async Task<HttpResponseMessage> HandleMarkdownInfo()
    {
        try
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            var markdown = (string)contentAsJson["markdown"] ?? (string)contentAsJson["text"] ?? "";

            if (string.IsNullOrEmpty(markdown))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {
                    ["error"] = "No markdown content provided",
                    ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }.ToString());
                return errorResponse;
            }

            var info = GetMarkdownInfo(markdown);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["originalMarkdown"] = markdown,
                ["info"] = info,
                ["format"] = "info",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {
                ["error"] = $"Error processing request: {ex.Message}",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return errorResponse;
        }
    }

    // Basic HTML conversion
    private string ConvertToHtml(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
            return "";

        var html = new StringBuilder();
        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        bool inCodeBlock = false;
        bool inList = false;
        bool inOrderedList = false;
        bool inBlockquote = false;
        string codeBlockLanguage = "";

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            var originalLine = line;

            // Handle code blocks
            if (trimmedLine.StartsWith("```"))
            {
                if (inCodeBlock)
                {
                    html.AppendLine("</code></pre>");
                    inCodeBlock = false;
                    codeBlockLanguage = "";
                }
                else
                {
                    codeBlockLanguage = trimmedLine.Substring(3).Trim();
                    html.AppendLine($"<pre><code class=\"language-{codeBlockLanguage}\">");
                    inCodeBlock = true;
                }
                continue;
            }

            if (inCodeBlock)
            {
                html.AppendLine(System.Web.HttpUtility.HtmlEncode(originalLine));
                continue;
            }

            // Handle empty lines
            if (string.IsNullOrWhiteSpace(trimmedLine))
            {
                if (inList)
                {
                    html.AppendLine(inOrderedList ? "</ol>" : "</ul>");
                    inList = false;
                    inOrderedList = false;
                }
                if (inBlockquote)
                {
                    html.AppendLine("</blockquote>");
                    inBlockquote = false;
                }
                continue;
            }

            // Handle headings
            if (trimmedLine.StartsWith("#"))
            {
                if (inList)
                {
                    html.AppendLine(inOrderedList ? "</ol>" : "</ul>");
                    inList = false;
                    inOrderedList = false;
                }
                if (inBlockquote)
                {
                    html.AppendLine("</blockquote>");
                    inBlockquote = false;
                }

                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#')
                    level++;

                if (level <= 6)
                {
                    var text = trimmedLine.Substring(level).Trim();
                    // Generate heading ID for anchor links
                    var headingId = text.ToLower().Replace(" ", "-").Replace("[^a-z0-9-]", "");
                    html.AppendLine($"<h{level} id=\"{headingId}\">{ProcessInlineMarkdown(text)}</h{level}>");
                    continue;
                }
            }

            // Handle blockquotes
            if (trimmedLine.StartsWith(">"))
            {
                if (!inBlockquote)
                {
                    html.AppendLine("<blockquote>");
                    inBlockquote = true;
                }
                var text = trimmedLine.Substring(1).Trim();
                html.AppendLine($"<p>{ProcessInlineMarkdown(text)}</p>");
                continue;
            }
            else if (inBlockquote)
            {
                html.AppendLine("</blockquote>");
                inBlockquote = false;
            }

            // Handle ordered lists
            if (Regex.IsMatch(trimmedLine, @"^\d+\.\s"))
            {
                if (!inList)
                {
                    html.AppendLine("<ol>");
                    inList = true;
                    inOrderedList = true;
                }
                else if (!inOrderedList)
                {
                    html.AppendLine("</ul>");
                    html.AppendLine("<ol>");
                    inOrderedList = true;
                }
                var match = Regex.Match(trimmedLine, @"^\d+\.\s(.*)");
                var text = match.Groups[1].Value;
                html.AppendLine($"<li>{ProcessInlineMarkdown(text)}</li>");
                continue;
            }

            // Handle unordered lists
            if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("* ") || trimmedLine.StartsWith("+ "))
            {
                if (!inList)
                {
                    html.AppendLine("<ul>");
                    inList = true;
                    inOrderedList = false;
                }
                else if (inOrderedList)
                {
                    html.AppendLine("</ol>");
                    html.AppendLine("<ul>");
                    inOrderedList = false;
                }
                var text = trimmedLine.Substring(2).Trim();
                html.AppendLine($"<li>{ProcessInlineMarkdown(text)}</li>");
                continue;
            }            // Handle horizontal rules
            if (trimmedLine == "---" || trimmedLine == "***" || trimmedLine == "___")
            {
                if (inList)
                {
                    html.AppendLine(inOrderedList ? "</ol>" : "</ul>");
                    inList = false;
                    inOrderedList = false;
                }
                html.AppendLine("<hr>");
                continue;
            }

            // Handle tables
            if (trimmedLine.Contains("|") && !inCodeBlock)
            {
                if (inList)
                {
                    html.AppendLine(inOrderedList ? "</ol>" : "</ul>");
                    inList = false;
                    inOrderedList = false;
                }

                var tableLines = new List<string> { originalLine };

                // Look ahead to find the complete table
                for (int i = Array.IndexOf(lines, originalLine) + 1; i < lines.Length; i++)
                {
                    if (lines[i].Contains("|"))
                    {
                        tableLines.Add(lines[i]);
                    }
                    else
                    {
                        break;
                    }
                }

                // Process the table
                if (tableLines.Count >= 2)
                {
                    html.AppendLine("<table>");

                    // Header row
                    var headerCells = tableLines[0].Split('|').Select(cell => cell.Trim()).Where(cell => !string.IsNullOrEmpty(cell)).ToArray();
                    if (headerCells.Length > 0)
                    {
                        html.AppendLine("<thead><tr>");
                        foreach (var cell in headerCells)
                        {
                            html.AppendLine($"<th>{ProcessInlineMarkdown(cell)}</th>");
                        }
                        html.AppendLine("</tr></thead>");
                    }

                    // Skip separator row (if it exists)
                    int dataRowStart = tableLines.Count > 1 && tableLines[1].Contains("-") ? 2 : 1;

                    // Data rows
                    if (dataRowStart < tableLines.Count)
                    {
                        html.AppendLine("<tbody>");
                        for (int i = dataRowStart; i < tableLines.Count; i++)
                        {
                            var dataCells = tableLines[i].Split('|').Select(cell => cell.Trim()).Where(cell => !string.IsNullOrEmpty(cell)).ToArray();
                            if (dataCells.Length > 0)
                            {
                                html.AppendLine("<tr>");
                                foreach (var cell in dataCells)
                                {
                                    html.AppendLine($"<td>{ProcessInlineMarkdown(cell)}</td>");
                                }
                                html.AppendLine("</tr>");
                            }
                        }
                        html.AppendLine("</tbody>");
                    }

                    html.AppendLine("</table>");

                    // Skip the lines we've already processed
                    for (int skip = 1; skip < tableLines.Count; skip++)
                    {
                        // This is a bit tricky since we can't modify the foreach iterator
                        // We'll handle this by checking if the line was already processed
                    }
                }
                continue;
            }

            // Close lists if we're not in one anymore
            if (inList)
            {
                html.AppendLine(inOrderedList ? "</ol>" : "</ul>");
                inList = false;
                inOrderedList = false;
            }

            // Handle paragraphs
            if (!string.IsNullOrWhiteSpace(trimmedLine))
            {
                html.AppendLine($"<p>{ProcessInlineMarkdown(trimmedLine)}</p>");
            }
        }

        // Close any remaining open tags
        if (inList)
        {
            html.AppendLine(inOrderedList ? "</ol>" : "</ul>");
        }
        if (inBlockquote)
        {
            html.AppendLine("</blockquote>");
        }
        if (inCodeBlock)
        {
            html.AppendLine("</code></pre>");
        }

        return html.ToString();
    }
    private string ProcessInlineMarkdown(string text)
    {
        // Handle HTML entities first to prevent double encoding
        text = System.Web.HttpUtility.HtmlEncode(text);

        // Links [text](url) and [text](url "title")
        text = Regex.Replace(text, @"\[([^\]]+)\]\(([^)]+?)(?:\s&quot;([^&quot;]+)&quot;)?\)",
            match =>
            {
                var linkText = match.Groups[1].Value;
                var url = match.Groups[2].Value;
                var title = match.Groups[3].Success ? $" title=\"{match.Groups[3].Value}\"" : "";
                return $"<a href=\"{url}\"{title}>{linkText}</a>";
            });

        // Reference links [text][ref] - simplified version
        text = Regex.Replace(text, @"\[([^\]]+)\]\[([^\]]+)\]", "<a href=\"#ref-$2\">$1</a>");

        // Auto links <url>
        text = Regex.Replace(text, @"&lt;(https?://[^&gt;]+)&gt;", "<a href=\"$1\">$1</a>");
        text = Regex.Replace(text, @"&lt;([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})&gt;", "<a href=\"mailto:$1\">$1</a>");

        // Images ![alt](url) and ![alt](url "title")
        text = Regex.Replace(text, @"!\[([^\]]*)\]\(([^)]+?)(?:\s&quot;([^&quot;]+)&quot;)?\)",
            match =>
            {
                var alt = match.Groups[1].Value;
                var url = match.Groups[2].Value;
                var title = match.Groups[3].Success ? $" title=\"{match.Groups[3].Value}\"" : "";
                return $"<img src=\"{url}\" alt=\"{alt}\"{title}>";
            });

        // Bold **text** or __text__ (non-greedy)
        text = Regex.Replace(text, @"\*\*(.*?)\*\*", "<strong>$1</strong>");
        text = Regex.Replace(text, @"__(.*?)__", "<strong>$1</strong>");

        // Italic *text* or _text_ (with negative lookbehind/lookahead to avoid conflicts)
        text = Regex.Replace(text, @"(?<!\*)\*([^*]+?)\*(?!\*)", "<em>$1</em>");
        text = Regex.Replace(text, @"(?<!_)_([^_]+?)_(?!_)", "<em>$1</em>");

        // Strikethrough ~~text~~
        text = Regex.Replace(text, @"~~(.*?)~~", "<del>$1</del>");

        // Superscript x^2^ and subscript H~2~O
        text = Regex.Replace(text, @"\^([^^]+?)\^", "<sup>$1</sup>");
        text = Regex.Replace(text, @"~([^~]+?)~", "<sub>$1</sub>");

        // Inline code `text` (preserve encoded content)
        text = Regex.Replace(text, @"`([^`]+?)`", match =>
        {
            var code = match.Groups[1].Value;
            // Decode for code content since it should be literal
            code = System.Web.HttpUtility.HtmlDecode(code);
            code = System.Web.HttpUtility.HtmlEncode(code);
            return $"<code>{code}</code>";
        });

        // Keyboard keys [[Ctrl+C]]
        text = Regex.Replace(text, @"\[\[([^\]]+?)\]\]", "<kbd>$1</kbd>");

        // Highlight ==text==
        text = Regex.Replace(text, @"==(.*?)==", "<mark>$1</mark>");

        // Task lists - [ ] and [x]
        text = Regex.Replace(text, @"- \[ \]", "<input type=\"checkbox\" disabled> ");
        text = Regex.Replace(text, @"- \[x\]", "<input type=\"checkbox\" checked disabled> ");

        // Footnote references [^1]
        text = Regex.Replace(text, @"\[\^([^\]]+?)\]", "<sup><a href=\"#footnote-$1\">$1</a></sup>");

        // Line breaks (two spaces at end of line)
        text = Regex.Replace(text, @"  $", "<br>", RegexOptions.Multiline);

        return text;
    }

    // Basic Adaptive Card conversion
    private JObject ConvertMarkdownToAdaptiveCardJson(string markdown)
    {
        var cardElements = new JArray();
        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            if (string.IsNullOrWhiteSpace(trimmedLine))
                continue;

            // Handle headings
            if (trimmedLine.StartsWith("#"))
            {
                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#')
                    level++;

                var text = trimmedLine.Substring(level).Trim();
                var size = level == 1 ? "Large" : level == 2 ? "Medium" : "Default";
                var weight = level <= 2 ? "Bolder" : "Default";

                cardElements.Add(new JObject
                {
                    ["type"] = "TextBlock",
                    ["text"] = text,
                    ["size"] = size,
                    ["weight"] = weight
                });
            }
            else
            {
                // Regular text
                cardElements.Add(new JObject
                {
                    ["type"] = "TextBlock",
                    ["text"] = trimmedLine,
                    ["wrap"] = true
                });
            }
        }

        return new JObject
        {
            ["type"] = "AdaptiveCard",
            ["$schema"] = "http://adaptivecards.io/schemas/adaptive-card.json",
            ["version"] = "1.3",
            ["body"] = cardElements
        };
    }

    // Basic chart generation
    private string ConvertToChart(string markdown)
    {
        var lines = markdown.Split('\n');
        var headingCount = lines.Count(line => line.TrimStart().StartsWith("#"));
        var listCount = lines.Count(line => line.TrimStart().StartsWith("- ") || line.TrimStart().StartsWith("* "));

        var svg = $@"<svg xmlns='http://www.w3.org/2000/svg' width='400' height='300'>
            <rect width='400' height='300' fill='#ffffff' stroke='#dee2e6'/>
            <text x='200' y='25' font-family='Arial' font-size='14' font-weight='bold' text-anchor='middle'>Markdown Content Chart</text>
            
            <rect x='50' y='50' width='{Math.Min(headingCount * 20, 300)}' height='30' fill='#007bff'/>
            <text x='360' y='70' font-family='Arial' font-size='12'>Headings: {headingCount}</text>
            
            <rect x='50' y='90' width='{Math.Min(listCount * 20, 300)}' height='30' fill='#28a745'/>
            <text x='360' y='110' font-family='Arial' font-size='12'>Lists: {listCount}</text>
        </svg>";

        var svgBytes = Encoding.UTF8.GetBytes(svg);
        return "data:image/svg+xml;base64," + Convert.ToBase64String(svgBytes);
    }

    // Basic QR code generation
    private string ConvertToQRCode(string markdown)
    {
        var plainText = Regex.Replace(markdown, @"[#*`~_\[\]()>-]", "");
        var hash = Math.Abs(plainText.GetHashCode()) % 1000;

        var svg = $@"<svg xmlns='http://www.w3.org/2000/svg' width='200' height='200'>
            <rect width='200' height='200' fill='white'/>
            <text x='100' y='20' font-family='Arial' font-size='12' text-anchor='middle'>QR-Style Code</text>
            
            <rect x='50' y='50' width='100' height='100' fill='none' stroke='black' stroke-width='2'/>
            <rect x='60' y='60' width='20' height='20' fill='{(hash % 2 == 0 ? "black" : "white")}'/>
            <rect x='120' y='60' width='20' height='20' fill='{(hash % 3 == 0 ? "black" : "white")}'/>
            <rect x='60' y='120' width='20' height='20' fill='{(hash % 5 == 0 ? "black" : "white")}'/>
            <rect x='120' y='120' width='20' height='20' fill='{(hash % 7 == 0 ? "black" : "white")}'/>
            
            <text x='100' y='180' font-family='Arial' font-size='10' text-anchor='middle'>Hash: {hash}</text>
        </svg>";

        var svgBytes = Encoding.UTF8.GetBytes(svg);
        return "data:image/svg+xml;base64," + Convert.ToBase64String(svgBytes);
    }

    // Basic JSON conversion
    private string ConvertToJson(string markdown)
    {
        var document = ConvertToJsonStructure(markdown);
        return JsonConvert.SerializeObject(document, Newtonsoft.Json.Formatting.Indented);
    }

    // JSON structure conversion
    private JObject ConvertToJsonStructure(string markdown)
    {
        var elements = new JArray();
        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        bool inCodeBlock = false;
        var codeBlockContent = new StringBuilder();
        string codeBlockLanguage = "";

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            // Handle code blocks
            if (trimmedLine.StartsWith("```"))
            {
                if (inCodeBlock)
                {
                    elements.Add(new JObject
                    {
                        ["type"] = "codeBlock",
                        ["language"] = codeBlockLanguage,
                        ["content"] = codeBlockContent.ToString().Trim()
                    });
                    codeBlockContent.Clear();
                    inCodeBlock = false;
                }
                else
                {
                    codeBlockLanguage = trimmedLine.Substring(3).Trim();
                    inCodeBlock = true;
                }
                continue;
            }

            if (inCodeBlock)
            {
                codeBlockContent.AppendLine(line);
                continue;
            }

            if (string.IsNullOrWhiteSpace(trimmedLine))
                continue;

            // Handle headings
            if (trimmedLine.StartsWith("#"))
            {
                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#')
                    level++;

                var text = trimmedLine.Substring(level).Trim();
                elements.Add(new JObject
                {
                    ["type"] = "heading",
                    ["level"] = level,
                    ["text"] = text
                });
            }
            // Handle lists
            else if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("* "))
            {
                var text = trimmedLine.Substring(2).Trim();
                elements.Add(new JObject
                {
                    ["type"] = "listItem",
                    ["text"] = text
                });
            }
            // Handle paragraphs
            else
            {
                elements.Add(new JObject
                {
                    ["type"] = "paragraph",
                    ["text"] = trimmedLine
                });
            }
        }

        return new JObject
        {
            ["document"] = new JObject
            {
                ["type"] = "markdown",
                ["elements"] = elements
            }
        };
    }

    // Basic XML conversion
    private string ConvertToXml(string markdown)
    {
        var xml = new StringBuilder();
        xml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        xml.AppendLine("<document type=\"markdown\">");

        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        bool inCodeBlock = false;

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            // Handle code blocks
            if (trimmedLine.StartsWith("```"))
            {
                if (inCodeBlock)
                {
                    xml.AppendLine("  </codeBlock>");
                    inCodeBlock = false;
                }
                else
                {
                    var language = trimmedLine.Substring(3).Trim();
                    xml.AppendLine($"  <codeBlock language=\"{System.Web.HttpUtility.HtmlEncode(language)}\">");
                    inCodeBlock = true;
                }
                continue;
            }

            if (inCodeBlock)
            {
                xml.AppendLine($"    {System.Web.HttpUtility.HtmlEncode(line)}");
                continue;
            }

            if (string.IsNullOrWhiteSpace(trimmedLine))
                continue;

            // Handle headings
            if (trimmedLine.StartsWith("#"))
            {
                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#')
                    level++;

                var text = trimmedLine.Substring(level).Trim();
                xml.AppendLine($"  <heading level=\"{level}\">{System.Web.HttpUtility.HtmlEncode(text)}</heading>");
            }
            // Handle lists
            else if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("* "))
            {
                var text = trimmedLine.Substring(2).Trim();
                xml.AppendLine($"  <listItem>{System.Web.HttpUtility.HtmlEncode(text)}</listItem>");
            }
            // Handle paragraphs
            else
            {
                xml.AppendLine($"  <paragraph>{System.Web.HttpUtility.HtmlEncode(trimmedLine)}</paragraph>");
            }
        }

        xml.AppendLine("</document>");
        return xml.ToString();
    }

    // Basic Plain Text conversion
    private string ConvertToPlainText(string markdown)
    {
        var text = new StringBuilder();
        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        bool inCodeBlock = false;

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            // Handle code blocks
            if (trimmedLine.StartsWith("```"))
            {
                if (inCodeBlock)
                {
                    text.AppendLine();
                    inCodeBlock = false;
                }
                else
                {
                    inCodeBlock = true;
                }
                continue;
            }

            if (inCodeBlock)
            {
                text.AppendLine(line);
                continue;
            }

            if (string.IsNullOrWhiteSpace(trimmedLine))
            {
                text.AppendLine();
                continue;
            }

            // Handle headings - just remove the # symbols
            if (trimmedLine.StartsWith("#"))
            {
                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#')
                    level++;

                var headingText = trimmedLine.Substring(level).Trim();
                text.AppendLine(headingText);
                text.AppendLine(new string('=', headingText.Length)); // Add underline for headings
            }
            // Handle lists - just remove the bullet points
            else if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("* "))
            {
                var listText = trimmedLine.Substring(2).Trim();
                text.AppendLine($"• {listText}");
            }
            // Handle paragraphs - remove markdown formatting
            else
            {
                var plainText = RemoveMarkdownFormatting(trimmedLine);
                text.AppendLine(plainText);
            }
        }

        return text.ToString();
    }

    private string RemoveMarkdownFormatting(string text)
    {
        // Remove bold
        text = Regex.Replace(text, @"\*\*(.*?)\*\*", "$1");
        // Remove italic
        text = Regex.Replace(text, @"\*(.*?)\*", "$1");
        // Remove strikethrough
        text = Regex.Replace(text, @"~~(.*?)~~", "$1");
        // Remove code
        text = Regex.Replace(text, @"`(.*?)`", "$1");
        // Remove links but keep the text
        text = Regex.Replace(text, @"\[([^\]]+)\]\([^\)]+\)", "$1");

        return text;
    }

    // Generic handler for remaining operations
    private async Task<HttpResponseMessage> HandleMarkdownToSvg() => await HandleGenericConversion("svg", ConvertToSvg);
    private async Task<HttpResponseMessage> HandleMarkdownToRss() => await HandleGenericConversion("rss", ConvertToRss);
    private async Task<HttpResponseMessage> HandleMarkdownToWiki() => await HandleGenericConversion("wiki", ConvertToWiki);
    private async Task<HttpResponseMessage> HandleMarkdownToPng() => await HandleGenericConversion("png", ConvertToPng);
    private async Task<HttpResponseMessage> HandleMarkdownToDiagram() => await HandleGenericConversion("diagram", ConvertToDiagram);
    private async Task<HttpResponseMessage> HandleMarkdownToJpeg() => await HandleGenericConversion("jpeg", ConvertToJpeg);
    private async Task<HttpResponseMessage> HandleMarkdownToBadge() => await HandleGenericConversion("badge", ConvertToBadge);
    private async Task<HttpResponseMessage> HandleMarkdownToInfographic() => await HandleGenericConversion("infographic", ConvertToInfographic);
    private async Task<HttpResponseMessage> HandleMarkdownToLog() => await HandleGenericConversion("log", ConvertToLog);
    private async Task<HttpResponseMessage> HandleMarkdownToMetrics() => await HandleGenericConversion("metrics", ConvertToMetrics);
    private async Task<HttpResponseMessage> HandleMarkdownToSyslog() => await HandleGenericConversion("syslog", ConvertToSyslog);
    private async Task<HttpResponseMessage> HandleMarkdownToJsDoc() => await HandleGenericConversion("jsdoc", ConvertToJsDoc);
    private async Task<HttpResponseMessage> HandleMarkdownToXmlDoc() => await HandleGenericConversion("xmldoc", ConvertToXmlDoc);
    private async Task<HttpResponseMessage> HandleMarkdownToReadme() => await HandleGenericConversion("readme", ConvertToReadme);
    private async Task<HttpResponseMessage> HandleMarkdownToChangelog() => await HandleGenericConversion("changelog", ConvertToChangelog);
    private async Task<HttpResponseMessage> HandleMarkdownToTableOfContents() => await HandleGenericConversion("toc", ConvertToTableOfContents);
    private async Task<HttpResponseMessage> HandleMarkdownToStyledHtml() => await HandleGenericConversion("styledHtml", ConvertToStyledHtml);

    private async Task<HttpResponseMessage> HandleGenericConversion(string format, Func<string, string> converter)
    {
        try
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            var markdown = (string)contentAsJson["markdown"] ?? (string)contentAsJson["text"] ?? "";

            if (string.IsNullOrEmpty(markdown))
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {
                    ["error"] = "No markdown content provided",
                    ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }.ToString());
                return errorResponse;
            }

            var result = converter(markdown);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["originalMarkdown"] = markdown,
                [format] = result,
                ["format"] = format,
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return response;
        }
        catch (Exception ex)
        {
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {
                ["error"] = $"Error processing request: {ex.Message}",
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }.ToString());
            return errorResponse;
        }
    }    // Additional conversion methods
    private string ConvertToCsv(string markdown)
    {
        var csv = new StringBuilder();
        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        bool foundTable = false;

        // First, check if there are any tables in the markdown
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("|"))
            {
                // Found a table, extract it
                var tableLines = new List<string>();
                for (int j = i; j < lines.Length && lines[j].Contains("|"); j++)
                {
                    tableLines.Add(lines[j]);
                    i = j; // Update outer loop index
                }

                if (tableLines.Count >= 2)
                {
                    foundTable = true;

                    // Process table headers
                    var headerCells = tableLines[0].Split('|').Select(cell => cell.Trim()).Where(cell => !string.IsNullOrEmpty(cell)).ToArray();
                    if (headerCells.Length > 0)
                    {
                        csv.AppendLine(string.Join(",", headerCells.Select(cell => $"\"{EscapeCsvField(RemoveMarkdownFormatting(cell))}\"")));
                    }

                    // Skip separator row if it exists
                    int dataRowStart = tableLines.Count > 1 && tableLines[1].Contains("-") ? 2 : 1;

                    // Process data rows
                    for (int rowIndex = dataRowStart; rowIndex < tableLines.Count; rowIndex++)
                    {
                        var dataCells = tableLines[rowIndex].Split('|').Select(cell => cell.Trim()).Where(cell => !string.IsNullOrEmpty(cell)).ToArray();
                        if (dataCells.Length > 0)
                        {
                            csv.AppendLine(string.Join(",", dataCells.Select(cell => $"\"{EscapeCsvField(RemoveMarkdownFormatting(cell))}\"")));
                        }
                    }

                    csv.AppendLine(); // Add empty line between tables
                }
            }
        }

        // If no tables found, create a structured CSV of the document content
        if (!foundTable)
        {
            csv.AppendLine("Type,Content,Level,LineNumber");

            for (int i = 0; i < lines.Length; i++)
            {
                var trimmedLine = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine)) continue;

                if (trimmedLine.StartsWith("#"))
                {
                    var level = 0;
                    while (level < trimmedLine.Length && trimmedLine[level] == '#') level++;
                    var text = RemoveMarkdownFormatting(trimmedLine.Substring(level).Trim());
                    csv.AppendLine($"Heading,\"{EscapeCsvField(text)}\",{level},{i + 1}");
                }
                else if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("* ") || trimmedLine.StartsWith("+ "))
                {
                    var text = RemoveMarkdownFormatting(trimmedLine.Substring(2).Trim());
                    csv.AppendLine($"ListItem,\"{EscapeCsvField(text)}\",0,{i + 1}");
                }
                else if (Regex.IsMatch(trimmedLine, @"^\d+\.\s"))
                {
                    var match = Regex.Match(trimmedLine, @"^\d+\.\s(.*)");
                    var text = RemoveMarkdownFormatting(match.Groups[1].Value);
                    csv.AppendLine($"OrderedListItem,\"{EscapeCsvField(text)}\",0,{i + 1}");
                }
                else if (trimmedLine.StartsWith(">"))
                {
                    var text = RemoveMarkdownFormatting(trimmedLine.Substring(1).Trim());
                    csv.AppendLine($"Blockquote,\"{EscapeCsvField(text)}\",0,{i + 1}");
                }
                else if (trimmedLine.StartsWith("```"))
                {
                    var language = trimmedLine.Substring(3).Trim();
                    csv.AppendLine($"CodeBlock,\"{EscapeCsvField(language)}\",0,{i + 1}");
                }
                else if (trimmedLine == "---" || trimmedLine == "***" || trimmedLine == "___")
                {
                    csv.AppendLine($"HorizontalRule,\"\",0,{i + 1}");
                }
                else
                {
                    var text = RemoveMarkdownFormatting(trimmedLine);
                    csv.AppendLine($"Paragraph,\"{EscapeCsvField(text)}\",0,{i + 1}");
                }
            }
        }

        return csv.ToString();
    }

    private string EscapeCsvField(string field)
    {
        if (field == null) return "";
        return field.Replace("\"", "\"\"");
    }

    private string ConvertToLaTeX(string markdown)
    {
        var latex = new StringBuilder();
        latex.AppendLine("\\documentclass{article}");
        latex.AppendLine("\\begin{document}");

        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine)) continue;

            if (trimmedLine.StartsWith("#"))
            {
                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#') level++;
                var text = trimmedLine.Substring(level).Trim();
                var command = level == 1 ? "section" : level == 2 ? "subsection" : "subsubsection";
                latex.AppendLine($"\\{command}{{{text}}}");
            }
            else if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("* "))
            {
                latex.AppendLine("\\begin{itemize}");
                latex.AppendLine($"\\item {trimmedLine.Substring(2).Trim()}");
                latex.AppendLine("\\end{itemize}");
            }
            else
            {
                latex.AppendLine(trimmedLine);
                latex.AppendLine();
            }
        }

        latex.AppendLine("\\end{document}");
        return latex.ToString();
    }

    private string ConvertToYaml(string markdown)
    {
        var yaml = new StringBuilder();
        yaml.AppendLine("document:");
        yaml.AppendLine("  type: markdown");
        yaml.AppendLine("  elements:");

        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine)) continue;

            if (trimmedLine.StartsWith("#"))
            {
                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#') level++;
                var text = trimmedLine.Substring(level).Trim();
                yaml.AppendLine($"    - type: heading");
                yaml.AppendLine($"      level: {level}");
                yaml.AppendLine($"      text: \"{text}\"");
            }
            else if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("* "))
            {
                var text = trimmedLine.Substring(2).Trim();
                yaml.AppendLine($"    - type: listItem");
                yaml.AppendLine($"      text: \"{text}\"");
            }
            else
            {
                yaml.AppendLine($"    - type: paragraph");
                yaml.AppendLine($"      text: \"{trimmedLine}\"");
            }
        }
        return yaml.ToString();
    }

    private string ConvertToEmail(string markdown)
    {
        var html = ConvertToHtml(markdown);
        var emailHtml = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        h1, h2, h3 {{ color: #2c3e50; }}
        pre {{ background: #f4f4f4; padding: 10px; border-radius: 5px; }}
        code {{ background: #f4f4f4; padding: 2px 4px; border-radius: 3px; }}
    </style>
</head>
<body>
{html}
</body>
</html>";
        return emailHtml;
    }

    private JObject GetMarkdownStats(string markdown)
    {
        var lines = markdown.Split('\n');
        var headingCount = lines.Count(line => line.TrimStart().StartsWith("#"));
        var listCount = lines.Count(line => line.TrimStart().StartsWith("- ") || line.TrimStart().StartsWith("* "));
        var codeBlockCount = lines.Count(line => line.TrimStart().StartsWith("```"));
        var wordCount = markdown.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        var characterCount = markdown.Length;

        return new JObject
        {
            ["headings"] = headingCount,
            ["lists"] = listCount,
            ["codeBlocks"] = codeBlockCount / 2, // Divide by 2 since each block has start and end
            ["words"] = wordCount,
            ["characters"] = characterCount,
            ["lines"] = lines.Length
        };
    }

    private JObject GetMarkdownInfo(string markdown)
    {
        var stats = GetMarkdownStats(markdown);
        return new JObject
        {
            ["stats"] = stats,
            ["hasHeadings"] = (int)stats["headings"] > 0,
            ["hasLists"] = (int)stats["lists"] > 0,
            ["hasCodeBlocks"] = (int)stats["codeBlocks"] > 0,
            ["estimatedReadingTime"] = Math.Ceiling((int)stats["words"] / 200.0) // 200 words per minute
        };
    }

    // Proper implementations for remaining formats
    private string ConvertToSvg(string markdown)
    {
        var stats = GetMarkdownStats(markdown);
        var svg = new StringBuilder();

        // Dynamic height based on content
        var estimatedHeight = Math.Max(400, (int)stats["lines"] * 25 + 200);

        svg.AppendLine($"<svg xmlns='http://www.w3.org/2000/svg' width='800' height='{estimatedHeight}' viewBox='0 0 800 {estimatedHeight}'>");

        // Background with gradient
        svg.AppendLine("  <defs>");
        svg.AppendLine("    <linearGradient id='bgGradient' x1='0%' y1='0%' x2='100%' y2='100%'>");
        svg.AppendLine("      <stop offset='0%' style='stop-color:#f8f9fa;stop-opacity:1' />");
        svg.AppendLine("      <stop offset='100%' style='stop-color:#e9ecef;stop-opacity:1' />");
        svg.AppendLine("    </linearGradient>");
        svg.AppendLine("    <filter id='shadow' x='-20%' y='-20%' width='140%' height='140%'>");
        svg.AppendLine("      <feDropShadow dx='2' dy='2' stdDeviation='3' flood-opacity='0.3'/>");
        svg.AppendLine("    </filter>");
        svg.AppendLine("  </defs>");

        // Background
        svg.AppendLine($"  <rect width='800' height='{estimatedHeight}' fill='url(#bgGradient)' stroke='#dee2e6' stroke-width='2'/>");

        // Header section
        svg.AppendLine("  <rect x='10' y='10' width='780' height='80' fill='#ffffff' stroke='#ced4da' stroke-width='1' rx='8' filter='url(#shadow)'/>");
        svg.AppendLine("  <text x='400' y='35' text-anchor='middle' font-family='Arial, sans-serif' font-size='20' font-weight='bold' fill='#2c3e50'>Markdown Document Visualization</text>");

        // Stats section
        svg.AppendLine($"  <text x='400' y='55' text-anchor='middle' font-family='Arial, sans-serif' font-size='12' fill='#6c757d'>Lines: {stats["lines"]} | Words: {stats["words"]} | Characters: {stats["characters"]}</text>");
        svg.AppendLine($"  <text x='400' y='75' text-anchor='middle' font-family='Arial, sans-serif' font-size='12' fill='#6c757d'>Headings: {stats["headings"]} | Lists: {stats["lists"]} | Code Blocks: {stats["codeBlocks"]}</text>");

        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        int y = 120;
        int sectionNumber = 1;
        bool inCodeBlock = false;

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            // Skip empty lines
            if (string.IsNullOrWhiteSpace(trimmedLine))
            {
                y += 10;
                continue;
            }

            // Handle code blocks
            if (trimmedLine.StartsWith("```"))
            {
                if (!inCodeBlock)
                {
                    svg.AppendLine($"  <rect x='30' y='{y - 15}' width='740' height='25' fill='#f8f9fa' stroke='#e9ecef' stroke-width='1' rx='4'/>");
                    svg.AppendLine($"  <text x='40' y='{y}' font-family='Monaco, monospace' font-size='11' fill='#495057'>Code Block Start</text>");
                    inCodeBlock = true;
                }
                else
                {
                    svg.AppendLine($"  <rect x='30' y='{y - 15}' width='740' height='25' fill='#f8f9fa' stroke='#e9ecef' stroke-width='1' rx='4'/>");
                    svg.AppendLine($"  <text x='40' y='{y}' font-family='Monaco, monospace' font-size='11' fill='#495057'>Code Block End</text>");
                    inCodeBlock = false;
                }
                y += 30;
                continue;
            }

            if (inCodeBlock)
            {
                var codeText = line.Length > 90 ? line.Substring(0, 87) + "..." : line;
                svg.AppendLine($"  <rect x='40' y='{y - 15}' width='720' height='22' fill='#f1f3f4' stroke='none'/>");
                svg.AppendLine($"  <text x='50' y='{y}' font-family='Monaco, monospace' font-size='10' fill='#d73a49'>{System.Web.HttpUtility.HtmlEncode(codeText)}</text>");
                y += 25;
                continue;
            }

            var displayText = trimmedLine.Length > 80 ? trimmedLine.Substring(0, 77) + "..." : trimmedLine;
            var escapedText = System.Web.HttpUtility.HtmlEncode(displayText);

            // Headings
            if (trimmedLine.StartsWith("#"))
            {
                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#') level++;
                var fontSize = Math.Max(10, 18 - (level - 1) * 2);
                var headingText = trimmedLine.Substring(level).Trim();
                if (headingText.Length > 60) headingText = headingText.Substring(0, 57) + "...";

                // Section divider for main headings
                if (level <= 2)
                {
                    svg.AppendLine($"  <line x1='30' y1='{y - 25}' x2='770' y2='{y - 25}' stroke='#ced4da' stroke-width='1'/>");
                    svg.AppendLine($"  <circle cx='50' cy='{y - 25}' r='8' fill='#007bff'/>");
                    svg.AppendLine($"  <text x='50' y='{y - 20}' text-anchor='middle' font-family='Arial, sans-serif' font-size='10' font-weight='bold' fill='white'>{sectionNumber}</text>");
                    sectionNumber++;
                    y += 10;
                }

                svg.AppendLine($"  <text x='30' y='{y}' font-family='Arial, sans-serif' font-size='{fontSize}' font-weight='bold' fill='#2c3e50'>{System.Web.HttpUtility.HtmlEncode(headingText)}</text>");
                y += fontSize + 10;
            }
            // Lists
            else if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("* ") || trimmedLine.StartsWith("+ "))
            {
                svg.AppendLine($"  <circle cx='45' cy='{y - 6}' r='3' fill='#28a745'/>");
                var listText = trimmedLine.Substring(2).Trim();
                if (listText.Length > 70) listText = listText.Substring(0, 67) + "...";
                svg.AppendLine($"  <text x='60' y='{y}' font-family='Arial, sans-serif' font-size='12' fill='#495057'>{System.Web.HttpUtility.HtmlEncode(listText)}</text>");
                y += 20;
            }
            // Ordered lists
            else if (System.Text.RegularExpressions.Regex.IsMatch(trimmedLine, @"^\d+\.\s"))
            {
                var match = System.Text.RegularExpressions.Regex.Match(trimmedLine, @"^(\d+)\.\s(.*)");
                var number = match.Groups[1].Value;
                var listText = match.Groups[2].Value;

                svg.AppendLine($"  <rect x='38' y='{y - 12}' width='16' height='16' fill='#17a2b8' rx='2'/>");
                svg.AppendLine($"  <text x='46' y='{y - 2}' text-anchor='middle' font-family='Arial, sans-serif' font-size='10' font-weight='bold' fill='white'>{number}</text>");

                if (listText.Length > 70) listText = listText.Substring(0, 67) + "...";
                svg.AppendLine($"  <text x='60' y='{y}' font-family='Arial, sans-serif' font-size='12' fill='#495057'>{System.Web.HttpUtility.HtmlEncode(listText)}</text>");
                y += 20;
            }
            // Blockquotes
            else if (trimmedLine.StartsWith(">"))
            {
                var quoteText = trimmedLine.Substring(1).Trim();
                if (quoteText.Length > 70) quoteText = quoteText.Substring(0, 67) + "...";
                svg.AppendLine($"  <rect x='30' y='{y - 15}' width='5' height='20' fill='#ffc107'/>");
                svg.AppendLine($"  <text x='45' y='{y}' font-family='Arial, sans-serif' font-size='12' font-style='italic' fill='#6c757d'>{System.Web.HttpUtility.HtmlEncode(quoteText)}</text>");
                y += 25;
            }
            // Tables
            else if (trimmedLine.Contains("|"))
            {
                svg.AppendLine($"  <rect x='30' y='{y - 15}' width='740' height='25' fill='#e7f3ff' stroke='#007bff' stroke-width='1' rx='4'/>");
                svg.AppendLine($"  <text x='40' y='{y}' font-family='Arial, sans-serif' font-size='11' fill='#007bff'>Table Row: {escapedText}</text>");
                y += 30;
            }
            // Regular paragraphs
            else
            {
                svg.AppendLine($"  <text x='30' y='{y}' font-family='Arial, sans-serif' font-size='12' fill='#495057'>{escapedText}</text>");
                y += 18;
            }

            // Prevent overflow
            if (y > estimatedHeight - 50) break;
        }

        // Footer
        svg.AppendLine($"  <text x='400' y='{estimatedHeight - 20}' text-anchor='middle' font-family='Arial' font-size='10' fill='#adb5bd'>Generated on {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</text>");

        svg.AppendLine("</svg>");
        return svg.ToString();
    }

    private string ConvertToRss(string markdown)
    {
        var rss = new StringBuilder();
        rss.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
        rss.AppendLine("<rss version='2.0'>");
        rss.AppendLine("  <channel>");
        rss.AppendLine("    <title>Markdown Content</title>");
        rss.AppendLine("    <description>RSS feed generated from Markdown content</description>");
        rss.AppendLine($"    <pubDate>{DateTime.UtcNow:ddd, dd MMM yyyy HH:mm:ss} GMT</pubDate>");

        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        var currentItem = new StringBuilder();
        string currentTitle = "Markdown Content";

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine)) continue;

            if (trimmedLine.StartsWith("#"))
            {
                // If we have accumulated content, create an item
                if (currentItem.Length > 0)
                {
                    rss.AppendLine("    <item>");
                    rss.AppendLine($"      <title>{System.Web.HttpUtility.HtmlEncode(currentTitle)}</title>");
                    rss.AppendLine($"      <description>{System.Web.HttpUtility.HtmlEncode(currentItem.ToString().Trim())}</description>");
                    rss.AppendLine($"      <pubDate>{DateTime.UtcNow:ddd, dd MMM yyyy HH:mm:ss} GMT</pubDate>");
                    rss.AppendLine("    </item>");
                    currentItem.Clear();
                }

                // Start new item with heading as title
                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#') level++;
                currentTitle = trimmedLine.Substring(level).Trim();
            }
            else
            {
                currentItem.AppendLine(trimmedLine);
            }
        }

        // Add final item if there's content
        if (currentItem.Length > 0)
        {
            rss.AppendLine("    <item>");
            rss.AppendLine($"      <title>{System.Web.HttpUtility.HtmlEncode(currentTitle)}</title>");
            rss.AppendLine($"      <description>{System.Web.HttpUtility.HtmlEncode(currentItem.ToString().Trim())}</description>");
            rss.AppendLine($"      <pubDate>{DateTime.UtcNow:ddd, dd MMM yyyy HH:mm:ss} GMT</pubDate>");
            rss.AppendLine("    </item>");
        }

        rss.AppendLine("  </channel>");
        rss.AppendLine("</rss>");
        return rss.ToString();
    }

    private string ConvertToWiki(string markdown)
    {
        var wiki = new StringBuilder();
        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        bool inCodeBlock = false;

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            // Handle code blocks
            if (trimmedLine.StartsWith("```"))
            {
                if (inCodeBlock)
                {
                    wiki.AppendLine("</pre>");
                    inCodeBlock = false;
                }
                else
                {
                    wiki.AppendLine("<pre>");
                    inCodeBlock = true;
                }
                continue;
            }

            if (inCodeBlock)
            {
                wiki.AppendLine(line);
                continue;
            }

            if (string.IsNullOrWhiteSpace(trimmedLine))
            {
                wiki.AppendLine();
                continue;
            }

            // Handle headings - convert # to = wiki syntax
            if (trimmedLine.StartsWith("#"))
            {
                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#') level++;
                var text = trimmedLine.Substring(level).Trim();
                var wikiEquals = new string('=', level);
                wiki.AppendLine($"{wikiEquals} {text} {wikiEquals}");
            }
            // Handle lists - convert to wiki bullet syntax
            else if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("* "))
            {
                var text = trimmedLine.Substring(2).Trim();
                wiki.AppendLine($"* {text}");
            }
            // Handle bold/italic
            else
            {
                var wikiLine = trimmedLine;
                // Convert **bold** to '''bold'''
                wikiLine = Regex.Replace(wikiLine, @"\*\*(.*?)\*\*", "'''$1'''");
                // Convert *italic* to ''italic''
                wikiLine = Regex.Replace(wikiLine, @"\*(.*?)\*", "''$1''");
                // Convert `code` to <code>code</code>
                wikiLine = Regex.Replace(wikiLine, @"`(.*?)`", "<code>$1</code>");

                wiki.AppendLine(wikiLine);
            }
        }
        return wiki.ToString();
    }

    private string ConvertToPng(string markdown)
    {
        // Generate a simple PNG representation as base64
        var svg = ConvertToSvg(markdown);
        // For a real implementation, you'd convert SVG to PNG
        // For now, return a data URL with the SVG content
        var svgBytes = Encoding.UTF8.GetBytes(svg);
        return "data:image/svg+xml;base64," + Convert.ToBase64String(svgBytes);
    }

    private string ConvertToDiagram(string markdown)
    {
        var svg = new StringBuilder();
        svg.AppendLine("<svg xmlns='http://www.w3.org/2000/svg' width='500' height='400' viewBox='0 0 500 400'>");
        svg.AppendLine("  <defs>");
        svg.AppendLine("    <style>.header { font-family: Arial; font-size: 14px; font-weight: bold; } .text { font-family: Arial; font-size: 12px; }</style>");
        svg.AppendLine("  </defs>");
        svg.AppendLine("  <rect width='500' height='400' fill='#ffffff' stroke='#cccccc' stroke-width='2'/>");
        svg.AppendLine("  <text x='250' y='25' text-anchor='middle' class='header'>Markdown Flow Diagram</text>");

        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        var headings = lines.Where(l => l.Trim().StartsWith("#")).Take(5).ToList();

        int y = 60;
        int boxHeight = 40;
        int boxWidth = 200;

        for (int i = 0; i < headings.Count; i++)
        {
            var heading = headings[i].Trim();
            var level = 0;
            while (level < heading.Length && heading[level] == '#') level++;
            var text = heading.Substring(level).Trim();

            if (text.Length > 25) text = text.Substring(0, 22) + "...";

            var x = 150 + (level - 1) * 50;
            var color = level == 1 ? "#3498db" : level == 2 ? "#2ecc71" : "#e74c3c";

            // Draw box
            svg.AppendLine($"  <rect x='{x}' y='{y}' width='{boxWidth}' height='{boxHeight}' fill='{color}' fill-opacity='0.1' stroke='{color}' stroke-width='2' rx='5'/>");
            svg.AppendLine($"  <text x='{x + boxWidth / 2}' y='{y + boxHeight / 2 + 5}' text-anchor='middle' class='text' fill='{color}'>{System.Web.HttpUtility.HtmlEncode(text)}</text>");

            // Draw arrow to next box
            if (i < headings.Count - 1)
            {
                svg.AppendLine($"  <line x1='{x + boxWidth / 2}' y1='{y + boxHeight}' x2='{x + boxWidth / 2}' y2='{y + boxHeight + 20}' stroke='#666' stroke-width='2' marker-end='url(#arrowhead)'/>");
            }

            y += boxHeight + 30;
        }

        // Add arrowhead marker
        svg.AppendLine("  <defs>");
        svg.AppendLine("    <marker id='arrowhead' markerWidth='10' markerHeight='7' refX='9' refY='3.5' orient='auto'>");
        svg.AppendLine("      <polygon points='0 0, 10 3.5, 0 7' fill='#666'/>");
        svg.AppendLine("    </marker>");
        svg.AppendLine("  </defs>");

        svg.AppendLine("</svg>");
        return svg.ToString();
    }

    private string ConvertToJpeg(string markdown)
    {
        // Similar to PNG, generate SVG and return as data URL
        var svg = ConvertToSvg(markdown);
        var svgBytes = Encoding.UTF8.GetBytes(svg); return "data:image/svg+xml;base64," + Convert.ToBase64String(svgBytes);
    }

    private string ConvertToBadge(string markdown)
    {
        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var stats = GetMarkdownStats(markdown);

        var svg = new StringBuilder();
        svg.AppendLine("<svg xmlns='http://www.w3.org/2000/svg' width='200' height='100' viewBox='0 0 200 100'>");

        // Background
        svg.AppendLine("  <rect width='200' height='100' fill='#2c3e50' rx='10'/>");

        // Badge title
        svg.AppendLine("  <text x='100' y='25' text-anchor='middle' font-family='Arial' font-size='12' font-weight='bold' fill='white'>Markdown Stats</text>");

        // Stats
        svg.AppendLine($"  <text x='20' y='45' font-family='Arial' font-size='10' fill='#ecf0f1'>Words: {stats["words"]}</text>");
        svg.AppendLine($"  <text x='20' y='60' font-family='Arial' font-size='10' fill='#ecf0f1'>Lines: {stats["lines"]}</text>");
        svg.AppendLine($"  <text x='20' y='75' font-family='Arial' font-size='10' fill='#ecf0f1'>Headings: {stats["headings"]}</text>");

        // Status indicator
        var color = (int)stats["words"] > 100 ? "#27ae60" : (int)stats["words"] > 50 ? "#f39c12" : "#e74c3c";
        svg.AppendLine($"  <circle cx='170' cy='60' r='15' fill='{color}'/>");
        svg.AppendLine($"  <text x='170' y='65' text-anchor='middle' font-family='Arial' font-size='10' font-weight='bold' fill='white'>✓</text>");

        svg.AppendLine("</svg>");
        return svg.ToString();
    }

    private string ConvertToInfographic(string markdown)
    {
        var stats = GetMarkdownStats(markdown);
        var svg = new StringBuilder();
        svg.AppendLine("<svg xmlns='http://www.w3.org/2000/svg' width='400' height='600' viewBox='0 0 400 600'>");

        // Background
        svg.AppendLine("  <rect width='400' height='600' fill='#ecf0f1'/>");

        // Header
        svg.AppendLine("  <rect width='400' height='80' fill='#3498db'/>");
        svg.AppendLine("  <text x='200' y='35' text-anchor='middle' font-family='Arial' font-size='20' font-weight='bold' fill='white'>Markdown Analysis</text>");
        svg.AppendLine("  <text x='200' y='55' text-anchor='middle' font-family='Arial' font-size='12' fill='white'>Content Overview</text>");

        // Word count section
        var wordBarWidth = Math.Min(300, (int)stats["words"] / 10);
        svg.AppendLine("  <text x='50' y='130' font-family='Arial' font-size='16' font-weight='bold' fill='#2c3e50'>Word Count</text>");
        svg.AppendLine("  <rect x='50' y='140' width='300' height='20' fill='#bdc3c7' rx='10'/>");
        svg.AppendLine($"  <rect x='50' y='140' width='{wordBarWidth}' height='20' fill='#e74c3c' rx='10'/>");
        svg.AppendLine($"  <text x='360' y='155' font-family='Arial' font-size='14' fill='#2c3e50'>{stats["words"]} words</text>");

        // Heading count
        var headingBarWidth = Math.Min(300, (int)stats["headings"] * 30);
        svg.AppendLine("  <text x='50' y='200' font-family='Arial' font-size='16' font-weight='bold' fill='#2c3e50'>Headings</text>");
        svg.AppendLine("  <rect x='50' y='210' width='300' height='20' fill='#bdc3c7' rx='10'/>");
        svg.AppendLine($"  <rect x='50' y='210' width='{headingBarWidth}' height='20' fill='#3498db' rx='10'/>");
        svg.AppendLine($"  <text x='360' y='225' font-family='Arial' font-size='14' fill='#2c3e50'>{stats["headings"]} headings</text>");

        // List items
        var listBarWidth = Math.Min(300, (int)stats["lists"] * 20);
        svg.AppendLine("  <text x='50' y='270' font-family='Arial' font-size='16' font-weight='bold' fill='#2c3e50'>Lists</text>");
        svg.AppendLine("  <rect x='50' y='280' width='300' height='20' fill='#bdc3c7' rx='10'/>");
        svg.AppendLine($"  <rect x='50' y='280' width='{listBarWidth}' height='20' fill='#27ae60' rx='10'/>");
        svg.AppendLine($"  <text x='360' y='295' font-family='Arial' font-size='14' fill='#2c3e50'>{stats["lists"]} items</text>");

        // Code blocks
        var codeBarWidth = Math.Min(300, (int)stats["codeBlocks"] * 50);
        svg.AppendLine("  <text x='50' y='340' font-family='Arial' font-size='16' font-weight='bold' fill='#2c3e50'>Code Blocks</text>");
        svg.AppendLine("  <rect x='50' y='350' width='300' height='20' fill='#bdc3c7' rx='10'/>");
        svg.AppendLine($"  <rect x='50' y='350' width='{codeBarWidth}' height='20' fill='#f39c12' rx='10'/>");
        svg.AppendLine($"  <text x='360' y='365' font-family='Arial' font-size='14' fill='#2c3e50'>{stats["codeBlocks"]} blocks</text>");

        // Reading time estimate
        var readingTime = Math.Ceiling((int)stats["words"] / 200.0);
        svg.AppendLine("  <rect x='50' y='420' width='300' height='60' fill='#34495e' rx='10'/>");
        svg.AppendLine("  <text x='200' y='440' text-anchor='middle' font-family='Arial' font-size='14' fill='white'>Estimated Reading Time</text>");
        svg.AppendLine($"  <text x='200' y='465' text-anchor='middle' font-family='Arial' font-size='24' font-weight='bold' fill='#1abc9c'>{readingTime} min</text>");

        // Footer
        svg.AppendLine($"  <text x='200' y='530' text-anchor='middle' font-family='Arial' font-size='12' fill='#7f8c8d'>Generated on {DateTime.UtcNow:yyyy-MM-dd}</text>");
        svg.AppendLine("</svg>");
        return svg.ToString();
    }

    private string ConvertToLog(string markdown)
    {
        var log = new StringBuilder();
        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        var timestamp = DateTime.UtcNow;

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine)) continue;

            var logLevel = "INFO";
            var message = trimmedLine;

            // Determine log level based on content
            if (trimmedLine.Contains("error") || trimmedLine.Contains("Error"))
                logLevel = "ERROR";
            else if (trimmedLine.Contains("warn") || trimmedLine.Contains("Warn"))
                logLevel = "WARN";
            else if (trimmedLine.Contains("debug") || trimmedLine.Contains("Debug"))
                logLevel = "DEBUG";
            else if (trimmedLine.StartsWith("#"))
            {
                logLevel = "INFO";
                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#') level++;
                message = $"Section: {trimmedLine.Substring(level).Trim()}";
            }
            else if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("* "))
            {
                logLevel = "INFO";
                message = $"Item: {trimmedLine.Substring(2).Trim()}";
            }

            log.AppendLine($"[{timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{logLevel}] {message}");
            timestamp = timestamp.AddMilliseconds(100); // Increment time slightly for each line
        }

        return log.ToString();
    }

    private string ConvertToMetrics(string markdown)
    {
        var stats = GetMarkdownStats(markdown);
        var metrics = new StringBuilder();
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        metrics.AppendLine("# HELP markdown_words_total Total number of words in markdown content");
        metrics.AppendLine("# TYPE markdown_words_total counter");
        metrics.AppendLine($"markdown_words_total {stats["words"]} {timestamp}");
        metrics.AppendLine();

        metrics.AppendLine("# HELP markdown_characters_total Total number of characters in markdown content");
        metrics.AppendLine("# TYPE markdown_characters_total counter");
        metrics.AppendLine($"markdown_characters_total {stats["characters"]} {timestamp}");
        metrics.AppendLine();

        metrics.AppendLine("# HELP markdown_lines_total Total number of lines in markdown content");
        metrics.AppendLine("# TYPE markdown_lines_total counter");
        metrics.AppendLine($"markdown_lines_total {stats["lines"]} {timestamp}");
        metrics.AppendLine();

        metrics.AppendLine("# HELP markdown_headings_total Total number of headings in markdown content");
        metrics.AppendLine("# TYPE markdown_headings_total counter");
        metrics.AppendLine($"markdown_headings_total {stats["headings"]} {timestamp}");
        metrics.AppendLine();

        metrics.AppendLine("# HELP markdown_lists_total Total number of list items in markdown content");
        metrics.AppendLine("# TYPE markdown_lists_total counter");
        metrics.AppendLine($"markdown_lists_total {stats["lists"]} {timestamp}");
        metrics.AppendLine();

        metrics.AppendLine("# HELP markdown_code_blocks_total Total number of code blocks in markdown content");
        metrics.AppendLine("# TYPE markdown_code_blocks_total counter");
        metrics.AppendLine($"markdown_code_blocks_total {stats["codeBlocks"]} {timestamp}");
        metrics.AppendLine();

        metrics.AppendLine("# HELP markdown_reading_time_minutes Estimated reading time in minutes");
        metrics.AppendLine("# TYPE markdown_reading_time_minutes gauge");
        var readingTime = Math.Ceiling((int)stats["words"] / 200.0);
        metrics.AppendLine($"markdown_reading_time_minutes {readingTime} {timestamp}");

        return metrics.ToString();
    }

    private string ConvertToSyslog(string markdown)
    {
        var syslog = new StringBuilder();
        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        var timestamp = DateTime.UtcNow;

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine)) continue;

            // Syslog format: <priority>version timestamp hostname app-name procid msgid structured-data msg
            var priority = 14; // user.info
            var version = 1;
            var hostname = "markdown-converter";
            var appName = "md2syslog";
            var procId = "-";
            var msgId = "-";
            var structuredData = "-";

            // Adjust priority based on content
            if (trimmedLine.Contains("error") || trimmedLine.Contains("Error"))
                priority = 11; // user.err
            else if (trimmedLine.Contains("warn") || trimmedLine.Contains("Warn"))
                priority = 12; // user.warning
            else if (trimmedLine.StartsWith("#"))
                priority = 13; // user.notice

            var syslogTimestamp = timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var message = trimmedLine.Replace("\t", " ").Replace("  ", " ");

            syslog.AppendLine($"<{priority}>{version} {syslogTimestamp} {hostname} {appName} {procId} {msgId} {structuredData} {message}");
            timestamp = timestamp.AddSeconds(1);
        }

        return syslog.ToString();
    }
    private string ConvertToJsDoc(string markdown)
    {
        var jsDoc = new StringBuilder();
        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);

        jsDoc.AppendLine("/**");

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine))
            {
                jsDoc.AppendLine(" *");
                continue;
            }

            if (trimmedLine.StartsWith("#"))
            {
                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#') level++;
                var text = trimmedLine.Substring(level).Trim();

                if (level == 1)
                {
                    jsDoc.AppendLine($" * @description {text}");
                }
                else
                {
                    jsDoc.AppendLine($" * @section {text}");
                }
            }
            else if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("* "))
            {
                var text = trimmedLine.Substring(2).Trim();
                if (text.Contains(":"))
                {
                    var parts = text.Split(new[] { ':' }, 2);
                    if (parts.Length == 2)
                    {
                        var paramName = parts[0].Trim();
                        var paramDesc = parts[1].Trim();
                        jsDoc.AppendLine($" * @param {{{paramName}}} {paramDesc}");
                    }
                }
                else
                {
                    jsDoc.AppendLine($" * @note {text}");
                }
            }
            else if (trimmedLine.StartsWith("```"))
            {
                if (trimmedLine.Length > 3)
                {
                    var language = trimmedLine.Substring(3).Trim();
                    jsDoc.AppendLine($" * @example");
                }
            }
            else
            {
                jsDoc.AppendLine($" * {trimmedLine}");
            }
        }

        jsDoc.AppendLine(" */");
        return jsDoc.ToString();
    }

    private string ConvertToXmlDoc(string markdown)
    {
        var xmlDoc = new StringBuilder();
        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);

        xmlDoc.AppendLine("/// <summary>");

        bool inSummary = true;
        bool inExample = false;

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine))
            {
                if (inSummary)
                {
                    xmlDoc.AppendLine("/// </summary>");
                    inSummary = false;
                }
                continue;
            }

            if (trimmedLine.StartsWith("#"))
            {
                if (inSummary)
                {
                    xmlDoc.AppendLine("/// </summary>");
                    inSummary = false;
                }

                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#') level++;
                var text = trimmedLine.Substring(level).Trim();
                xmlDoc.AppendLine($"/// <remarks>{System.Web.HttpUtility.HtmlEncode(text)}</remarks>");
            }
            else if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("* "))
            {
                var text = trimmedLine.Substring(2).Trim();
                if (text.Contains(":"))
                {
                    var parts = text.Split(new[] { ':' }, 2);
                    if (parts.Length == 2)
                    {
                        var paramName = parts[0].Trim();
                        var paramDesc = parts[1].Trim();
                        xmlDoc.AppendLine($"/// <param name=\"{paramName}\">{System.Web.HttpUtility.HtmlEncode(paramDesc)}</param>");
                    }
                }
                else
                {
                    xmlDoc.AppendLine($"/// <item>{System.Web.HttpUtility.HtmlEncode(text)}</item>");
                }
            }
            else if (trimmedLine.StartsWith("```"))
            {
                if (!inExample)
                {
                    xmlDoc.AppendLine("/// <example>");
                    xmlDoc.AppendLine("/// <code>");
                    inExample = true;
                }
                else
                {
                    xmlDoc.AppendLine("/// </code>");
                    xmlDoc.AppendLine("/// </example>");
                    inExample = false;
                }
            }
            else
            {
                if (inSummary)
                {
                    xmlDoc.AppendLine($"/// {System.Web.HttpUtility.HtmlEncode(trimmedLine)}");
                }
                else if (inExample)
                {
                    xmlDoc.AppendLine($"/// {System.Web.HttpUtility.HtmlEncode(trimmedLine)}");
                }
                else
                {
                    xmlDoc.AppendLine($"/// <para>{System.Web.HttpUtility.HtmlEncode(trimmedLine)}</para>");
                }
            }
        }

        if (inSummary)
        {
            xmlDoc.AppendLine("/// </summary>");
        }

        if (inExample)
        {
            xmlDoc.AppendLine("/// </code>");
            xmlDoc.AppendLine("/// </example>");
        }
        return xmlDoc.ToString();
    }
    private string ConvertToReadme(string markdown)
    {
        var readme = new StringBuilder();
        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);
        var stats = GetMarkdownStats(markdown);

        // Extract project name from first heading or use default
        var projectName = "Project";
        var description = "";
        var features = new List<string>();
        var installation = new List<string>();
        var usage = new List<string>();
        bool hasFoundTitle = false;

        // Parse content for structure
        for (int i = 0; i < lines.Length; i++)
        {
            var trimmedLine = lines[i].Trim();

            if (!hasFoundTitle && trimmedLine.StartsWith("#"))
            {
                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#') level++;
                if (level == 1)
                {
                    projectName = trimmedLine.Substring(level).Trim();
                    hasFoundTitle = true;

                    // Look for description in next few lines
                    for (int j = i + 1; j < Math.Min(i + 5, lines.Length); j++)
                    {
                        var nextLine = lines[j].Trim();
                        if (!string.IsNullOrWhiteSpace(nextLine) && !nextLine.StartsWith("#"))
                        {
                            description = nextLine;
                            break;
                        }
                    }
                    continue;
                }
            }

            // Look for section indicators
            if (trimmedLine.StartsWith("#"))
            {
                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#') level++;
                var heading = trimmedLine.Substring(level).Trim().ToLower();

                if (heading.Contains("feature") || heading.Contains("what") || heading.Contains("benefit"))
                {
                    // Collect features from following content
                    for (int j = i + 1; j < lines.Length; j++)
                    {
                        var featureLine = lines[j].Trim();
                        if (featureLine.StartsWith("#")) break;
                        if (featureLine.StartsWith("- ") || featureLine.StartsWith("* "))
                        {
                            features.Add(featureLine.Substring(2).Trim());
                        }
                    }
                }
                else if (heading.Contains("install") || heading.Contains("setup") || heading.Contains("getting started"))
                {
                    // Collect installation steps
                    for (int j = i + 1; j < lines.Length; j++)
                    {
                        var installLine = lines[j].Trim();
                        if (installLine.StartsWith("#")) break;
                        if (!string.IsNullOrWhiteSpace(installLine))
                        {
                            installation.Add(installLine);
                        }
                    }
                }
                else if (heading.Contains("usage") || heading.Contains("example") || heading.Contains("how to"))
                {
                    // Collect usage examples
                    for (int j = i + 1; j < lines.Length; j++)
                    {
                        var usageLine = lines[j].Trim();
                        if (usageLine.StartsWith("#")) break;
                        if (!string.IsNullOrWhiteSpace(usageLine))
                        {
                            usage.Add(usageLine);
                        }
                    }
                }
            }
        }

        // Build professional README structure
        readme.AppendLine($"# {projectName}");
        readme.AppendLine();

        if (!string.IsNullOrEmpty(description))
        {
            readme.AppendLine(description);
            readme.AppendLine();
        }

        // Add badges
        readme.AppendLine("![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)");
        readme.AppendLine($"![Lines of Code](https://img.shields.io/badge/Lines%20of%20Code-{stats["lines"]}-blue)");
        readme.AppendLine($"![Word Count](https://img.shields.io/badge/Words-{stats["words"]}-green)");
        readme.AppendLine();

        // Table of Contents
        readme.AppendLine("## Table of Contents");
        readme.AppendLine();
        readme.AppendLine("- [Description](#description)");
        if (features.Count > 0) readme.AppendLine("- [Features](#features)");
        if (installation.Count > 0) readme.AppendLine("- [Installation](#installation)");
        if (usage.Count > 0) readme.AppendLine("- [Usage](#usage)");
        readme.AppendLine("- [Contributing](#contributing)");
        readme.AppendLine("- [License](#license)");
        readme.AppendLine();

        // Description
        readme.AppendLine("## Description");
        readme.AppendLine();
        readme.AppendLine($"This project contains {stats["words"]} words across {stats["lines"]} lines, ");
        readme.AppendLine($"with {stats["headings"]} headings and {stats["lists"]} list items.");
        if ((int)stats["codeBlocks"] > 0)
        {
            readme.AppendLine($"It includes {stats["codeBlocks"]} code blocks demonstrating various concepts.");
        }
        readme.AppendLine();

        // Features
        if (features.Count > 0)
        {
            readme.AppendLine("## Features");
            readme.AppendLine();
            foreach (var feature in features)
            {
                readme.AppendLine($"- {feature}");
            }
            readme.AppendLine();
        }
        else
        {
            readme.AppendLine("## Features");
            readme.AppendLine();
            readme.AppendLine("- Comprehensive documentation");
            readme.AppendLine("- Well-structured content");
            readme.AppendLine("- Easy to understand format");
            readme.AppendLine();
        }

        // Installation
        readme.AppendLine("## Installation");
        readme.AppendLine();
        if (installation.Count > 0)
        {
            foreach (var installStep in installation.Take(10)) // Limit to prevent overflow
            {
                readme.AppendLine(installStep);
            }
        }
        else
        {
            readme.AppendLine("```bash");
            readme.AppendLine("git clone <repository-url>");
            readme.AppendLine("cd " + projectName.ToLower().Replace(" ", "-"));
            readme.AppendLine("# Follow setup instructions");
            readme.AppendLine("```");
        }
        readme.AppendLine();

        // Usage
        readme.AppendLine("## Usage");
        readme.AppendLine();
        if (usage.Count > 0)
        {
            foreach (var usageItem in usage.Take(15)) // Limit to prevent overflow
            {
                readme.AppendLine(usageItem);
            }
        }
        else
        {
            readme.AppendLine("Detailed usage instructions will be provided here.");
            readme.AppendLine();
            readme.AppendLine("```bash");
            readme.AppendLine("# Example usage");
            readme.AppendLine("./run.sh");
            readme.AppendLine("```");
        }
        readme.AppendLine();

        // Contributing
        readme.AppendLine("## Contributing");
        readme.AppendLine();
        readme.AppendLine("1. Fork the repository");
        readme.AppendLine("2. Create your feature branch (`git checkout -b feature/AmazingFeature`)");
        readme.AppendLine("3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)");
        readme.AppendLine("4. Push to the branch (`git push origin feature/AmazingFeature`)");
        readme.AppendLine("5. Open a Pull Request");
        readme.AppendLine();

        // License
        readme.AppendLine("## License");
        readme.AppendLine();
        readme.AppendLine("This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.");
        readme.AppendLine();

        // Footer
        readme.AppendLine("---");
        readme.AppendLine($"*Generated on {DateTime.UtcNow:yyyy-MM-dd} from Markdown content*");
        return readme.ToString();
    }

    private string ConvertToChangelog(string markdown)
    {
        var changelog = new StringBuilder();
        var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.None);

        changelog.AppendLine("# Changelog");
        changelog.AppendLine();
        changelog.AppendLine("All notable changes to this project will be documented in this file.");
        changelog.AppendLine();
        changelog.AppendLine("The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),");
        changelog.AppendLine("and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).");
        changelog.AppendLine();

        var currentDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
        changelog.AppendLine($"## [Unreleased] - {currentDate}");
        changelog.AppendLine();

        var addedItems = new List<string>();
        var changedItems = new List<string>();
        var fixedItems = new List<string>();

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine)) continue;

            if (trimmedLine.StartsWith("#"))
            {
                var level = 0;
                while (level < trimmedLine.Length && trimmedLine[level] == '#') level++;
                var text = trimmedLine.Substring(level).Trim();
                addedItems.Add(text);
            }
            else if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("* "))
            {
                var text = trimmedLine.Substring(2).Trim();
                if (text.ToLower().Contains("fix") || text.ToLower().Contains("bug"))
                {
                    fixedItems.Add(text);
                }
                else if (text.ToLower().Contains("change") || text.ToLower().Contains("update"))
                {
                    changedItems.Add(text);
                }
                else
                {
                    addedItems.Add(text);
                }
            }
            else
            {
                addedItems.Add(trimmedLine);
            }
        }

        if (addedItems.Count > 0)
        {
            changelog.AppendLine("### Added");
            foreach (var item in addedItems)
            {
                changelog.AppendLine($"- {item}");
            }
            changelog.AppendLine();
        }

        if (changedItems.Count > 0)
        {
            changelog.AppendLine("### Changed");
            foreach (var item in changedItems)
            {
                changelog.AppendLine($"- {item}");
            }
            changelog.AppendLine();
        }

        if (fixedItems.Count > 0)
        {
            changelog.AppendLine("### Fixed");
            foreach (var item in fixedItems)
            {
                changelog.AppendLine($"- {item}");
            }
            changelog.AppendLine();
        }

        changelog.AppendLine("## [1.0.0] - 2023-01-01");
        changelog.AppendLine();
        changelog.AppendLine("### Added");
        changelog.AppendLine("- Initial release");

        return changelog.ToString();
    }
    private string ConvertToTableOfContents(string markdown)
    {
        var toc = new StringBuilder("# Table of Contents\n\n");
        var lines = markdown.Split('\n');
        foreach (var line in lines)
        {
            if (line.TrimStart().StartsWith("#"))
            {
                var level = 0;
                var trimmed = line.Trim();
                while (level < trimmed.Length && trimmed[level] == '#') level++;
                var text = trimmed.Substring(level).Trim();
                var indent = new string(' ', (level - 1) * 2);
                toc.AppendLine($"{indent}- {text}");
            }
        }
        return toc.ToString();
    }
    private string ConvertToStyledHtml(string markdown)
    {
        var html = ConvertToHtml(markdown);
        return $@"<!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; max-width: 800px; margin: 0 auto; padding: 20px; }}
                    h1 {{ color: #2c3e50; border-bottom: 2px solid #3498db; }}
                    h2 {{ color: #34495e; border-bottom: 1px solid #bdc3c7; }}
                    pre {{ background: #f8f9fa; border: 1px solid #e9ecef; border-radius: 4px; padding: 16px; }}
                    code {{ background: #f8f9fa; padding: 2px 4px; border-radius: 3px; }}
                </style>
            </head>
            <body>{html}</body>
            </html>";
    }

    // Helper methods for operation ID validation
    private bool IsKnownOperationId(string operationId)
    {
        var knownOperations = new[]
        {
            "MarkdownToHtml", "MarkdownToAdaptiveCard", "MarkdownToChart", "MarkdownToQr",
            "MarkdownToJson", "MarkdownToXml", "MarkdownToPlainText", "MarkdownToCsv",
            "MarkdownToLaTeX", "MarkdownToYaml", "MarkdownToEmail", "MarkdownToSvg",
            "MarkdownToRss", "MarkdownToWiki", "MarkdownToPng", "MarkdownToDiagram",
            "MarkdownStats", "MarkdownToJpeg", "MarkdownToBadge", "MarkdownToInfographic",
            "MarkdownToLog", "MarkdownToMetrics", "MarkdownToSyslog", "MarkdownToJsDoc",
            "MarkdownToXmlDoc", "MarkdownToReadme", "MarkdownToChangelog",
            "MarkdownToTableOfContents", "MarkdownToStyledHtml", "MarkdownInfo"
        };
        return knownOperations.Contains(operationId);
    }
    private bool IsLikelyBase64(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        // If it's a known operation ID format, it's not base64
        if (IsValidOperationId(input)) return false;

        // Must be at least 4 characters and divisible by 4 when padding is considered
        if (input.Length < 4 || input.Length % 4 != 0) return false;

        // Should only contain base64 characters
        if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^[A-Za-z0-9+/]*={0,2}$")) return false;

        // Should not be too short (most real base64 encoded operation IDs would be longer)
        if (input.Length < 8) return false;

        return true;
    }

    private bool IsValidOperationId(string operationId)
    {
        if (string.IsNullOrEmpty(operationId)) return false;

        // Check if it contains only printable ASCII characters
        if (!operationId.All(c => c >= 32 && c <= 126)) return false;

        // Check if it looks like a valid operation ID pattern
        return System.Text.RegularExpressions.Regex.IsMatch(operationId, @"^[A-Za-z][A-Za-z0-9]*$");
    }
}
