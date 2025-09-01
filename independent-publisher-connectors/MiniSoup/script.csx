using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Xml;

/// <summary>
/// Lightweight HTML parsing library "MiniSoup" for Power Automate custom connectors
/// Inspired by Beautiful Soup, providing simple and efficient HTML parsing capabilities
/// </summary>
public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
        {
            // Parse the request
            string contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentAsJson = JObject.Parse(contentAsString);
            
            // Get operation type
            string operation = (string)contentAsJson["operation"];
            
            // Execute appropriate operation
            switch (operation?.ToLower())
            {
                case "fetch_html":
                    return await FetchHtmlOperation(contentAsJson);
                    
                case "select":
                    return await SelectOperation(contentAsJson);
                    
                case "extract":
                    return await ExtractOperation(contentAsJson);
                    
                case "find_all":
                    return await FindAllOperation(contentAsJson);
                    
                case "parse_table":
                    return await ParseTableOperation(contentAsJson);
                
                default:
                    return CreateErrorResponse("Unknown operation. Supported operations: fetch_html, select, extract, find_all, parse_table");
            }
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error: {ex.Message}");
        }
    }
    
    #region Operations
    
    // Operation to fetch HTML from a URL
    private async Task<HttpResponseMessage> FetchHtmlOperation(JObject request)
    {
        string url = (string)request["url"];
        
        if (string.IsNullOrEmpty(url))
            return CreateErrorResponse("URL is required");
            
        // Normalize URL
        if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            url = "https://" + url;
            
        try
        {
            // Fetch HTML
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            httpRequest.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            httpRequest.Headers.Add("Accept", "text/html,application/xhtml+xml");
            
            HttpResponseMessage webResponse = await this.Context.SendAsync(httpRequest, this.CancellationToken).ConfigureAwait(false);
            
            if (!webResponse.IsSuccessStatusCode)
                return CreateErrorResponse($"Failed to fetch URL: {webResponse.StatusCode}");
                
            string htmlContent = await webResponse.Content.ReadAsStringAsync();
            
            // Preprocess HTML (remove comments, extra whitespace, etc.)
            htmlContent = PreprocessHtml(htmlContent);
            
            var resultObject = new JObject
            {
                ["html"] = htmlContent,
                ["success"] = true
            };
            
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(resultObject.ToString());
            return response;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error fetching HTML: {ex.Message}");
        }
    }
    
    // Operation to select elements using a selector
    private async Task<HttpResponseMessage> SelectOperation(JObject request)
    {
        string html = (string)request["html"];
        string selector = (string)request["selector"];
        string selectorType = (string)request["selector_type"] ?? "css";
        
        if (string.IsNullOrEmpty(html))
            return CreateErrorResponse("HTML content is required");
            
        if (string.IsNullOrEmpty(selector))
            return CreateErrorResponse("Selector is required");
            
        try
        {
            // Select elements from HTML
            List<HtmlElement> elements = new List<HtmlElement>();
            
            if (selectorType.Equals("css", StringComparison.OrdinalIgnoreCase))
                elements = SelectElementsByCssSelector(html, selector);
            else if (selectorType.Equals("xpath", StringComparison.OrdinalIgnoreCase))
                elements = SelectElementsByXPath(html, selector);
            else
                return CreateErrorResponse($"Unsupported selector type: {selectorType}. Supported: css, xpath");
                
            var resultElements = new JArray();
            foreach (var element in elements)
            {
                resultElements.Add(element.ToJson());
            }
            
            var resultObject = new JObject
            {
                ["elements"] = resultElements,
                ["count"] = elements.Count,
                ["success"] = true
            };
            
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(resultObject.ToString());
            return response;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error selecting elements: {ex.Message}");
        }
    }
    
    // Operation to extract specific information from elements
    private async Task<HttpResponseMessage> ExtractOperation(JObject request)
    {
        string html = (string)request["html"];
        string selector = (string)request["selector"];
        string attribute = (string)request["attribute"];
        string selectorType = (string)request["selector_type"] ?? "css";
        
        if (string.IsNullOrEmpty(html))
            return CreateErrorResponse("HTML content is required");
            
        if (string.IsNullOrEmpty(selector))
            return CreateErrorResponse("Selector is required");
            
        try
        {
            // Select elements from HTML
            List<HtmlElement> elements = new List<HtmlElement>();
            
            if (selectorType.Equals("css", StringComparison.OrdinalIgnoreCase))
                elements = SelectElementsByCssSelector(html, selector);
            else if (selectorType.Equals("xpath", StringComparison.OrdinalIgnoreCase))
                elements = SelectElementsByXPath(html, selector);
            else
                return CreateErrorResponse($"Unsupported selector type: {selectorType}. Supported: css, xpath");
                
            // Get results
            var extractedValues = new JArray();
            
            foreach (var element in elements)
            {
                if (string.IsNullOrEmpty(attribute) || attribute.Equals("text", StringComparison.OrdinalIgnoreCase))
                {
                    extractedValues.Add(element.InnerText);
                }
                else if (attribute.Equals("html", StringComparison.OrdinalIgnoreCase))
                {
                    extractedValues.Add(element.OuterHtml);
                }
                else
                {
                    extractedValues.Add(element.GetAttribute(attribute) ?? string.Empty);
                }
            }
            
            var resultObject = new JObject
            {
                ["values"] = extractedValues,
                ["count"] = extractedValues.Count,
                ["success"] = true
            };
            
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(resultObject.ToString());
            return response;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error extracting values: {ex.Message}");
        }
    }
    
    // Operation to find all matching elements
    private async Task<HttpResponseMessage> FindAllOperation(JObject request)
    {
        string html = (string)request["html"];
        string tagName = (string)request["tag_name"];
        JObject attributes = request["attributes"] as JObject;
        
        if (string.IsNullOrEmpty(html))
            return CreateErrorResponse("HTML content is required");
            
        if (string.IsNullOrEmpty(tagName))
            return CreateErrorResponse("Tag name is required");
            
        try
        {
            // Build attribute filters
            Dictionary<string, string> attributeFilters = null;
            
            if (attributes != null && attributes.Count > 0)
            {
                attributeFilters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var prop in attributes.Properties())
                {
                    attributeFilters[prop.Name] = prop.Value.ToString();
                }
            }
            
            // Find elements in HTML
            List<HtmlElement> elements = FindAllElements(html, tagName, attributeFilters);
            
            var resultElements = new JArray();
            foreach (var element in elements)
            {
                resultElements.Add(element.ToJson());
            }
            
            var resultObject = new JObject
            {
                ["elements"] = resultElements,
                ["count"] = elements.Count,
                ["success"] = true
            };
            
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(resultObject.ToString());
            return response;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error finding elements: {ex.Message}");
        }
    }
    
    // Operation to parse HTML tables
    private async Task<HttpResponseMessage> ParseTableOperation(JObject request)
    {
        string html = (string)request["html"];
        string tableSelector = (string)request["table_selector"] ?? "table";
        bool headerRowsExist = request["header_rows_exist"]?.Value<bool>() ?? true;
        
        if (string.IsNullOrEmpty(html))
            return CreateErrorResponse("HTML content is required");
            
        try
        {
            // Select table elements
            List<HtmlElement> tableElements = SelectElementsByCssSelector(html, tableSelector);
            
            if (tableElements.Count == 0)
                return CreateErrorResponse("No tables found with the specified selector");
                
            // Parse the first table
            var tableData = ParseHtmlTable(tableElements[0], headerRowsExist);
            
            var resultObject = new JObject
            {
                ["data"] = JToken.FromObject(tableData),
                ["success"] = true
            };
            
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(resultObject.ToString());
            return response;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse($"Error parsing table: {ex.Message}");
        }
    }
    
    #endregion

    #region Core HTML Parsing Logic
    
    // Preprocessing: Normalize HTML
    private string PreprocessHtml(string html)
    {
        if (string.IsNullOrEmpty(html))
            return string.Empty;
            
        // Remove HTML comments
        html = Regex.Replace(html, @"<!--.*?-->", string.Empty, RegexOptions.Singleline);
        
        // Optional: Remove script tags
        // html = Regex.Replace(html, @"<script\b[^<]*(?:(?!</script>)<[^<]*)*</script>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        
        // Replace consecutive whitespace with a single space
        html = Regex.Replace(html, @"\s+", " ");
        
        return html;
    }
    
    // Select elements using CSS selector
    private List<HtmlElement> SelectElementsByCssSelector(string html, string cssSelector)
    {
        if (string.IsNullOrEmpty(html) || string.IsNullOrEmpty(cssSelector))
            return new List<HtmlElement>();
            
        // Process different types of selectors
        if (cssSelector.StartsWith("#"))
        {
            // ID selector
            string id = cssSelector.Substring(1);
            return FindAllElements(html, "*", new Dictionary<string, string> { { "id", id } });
        }
        else if (cssSelector.StartsWith("."))
        {
            // Class selector
            string className = cssSelector.Substring(1);
            return FindElementsByClass(html, className);
        }
        else if (cssSelector.Contains("[") && cssSelector.Contains("]"))
        {
            // Attribute selector
            var parts = cssSelector.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            string tagName = parts[0].Trim();
            if (string.IsNullOrEmpty(tagName))
                tagName = "*";
                
            if (parts.Length > 1)
            {
                string attrSelector = parts[1].Trim();
                if (attrSelector.Contains("="))
                {
                    // Attribute with value
                    var attrParts = attrSelector.Split('=');
                    string attrName = attrParts[0].Trim();
                    string attrValue = attrParts[1].Trim('\'', '"');
                    
                    return FindAllElements(html, tagName, new Dictionary<string, string> { { attrName, attrValue } });
                }
                else
                {
                    // Attribute existence only
                    return FindElementsWithAttribute(html, tagName, attrSelector);
                }
            }
        }
        else
        {
            // Simple tag selector
            return FindAllElements(html, cssSelector, null);
        }
        
        return new List<HtmlElement>();
    }
    
    // Select elements using XPath (simplified implementation)
    private List<HtmlElement> SelectElementsByXPath(string html, string xpath)
    {
        // Note: A complete XPath implementation is complex, so we support only common cases
        if (string.IsNullOrEmpty(html) || string.IsNullOrEmpty(xpath))
            return new List<HtmlElement>();
            
        // Support basic XPath patterns
        if (xpath.StartsWith("//"))
        {
            // Select tags at any position
            string tagPattern = xpath.Substring(2);
            if (tagPattern.Contains("["))
            {
                // With predicate
                var parts = tagPattern.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                string tagName = parts[0];
                
                if (parts.Length > 1 && parts[1].StartsWith("@"))
                {
                    // Selection by attribute (//@attr='value')
                    var attrParts = parts[1].Substring(1).Split('=');
                    if (attrParts.Length == 2)
                    {
                        string attrName = attrParts[0].Trim();
                        string attrValue = attrParts[1].Trim('\'', '"');
                        
                        return FindAllElements(html, tagName, new Dictionary<string, string> { { attrName, attrValue } });
                    }
                }
            }
            else
            {
                // Simple tag name only
                return FindAllElements(html, tagPattern, null);
            }
        }
        
        return new List<HtmlElement>();
    }
    
    // Find elements by tag name and attribute conditions - Completely rewritten
    private List<HtmlElement> FindAllElements(string html, string tagName, Dictionary<string, string> attributes)
    {
        if (string.IsNullOrEmpty(html))
            return new List<HtmlElement>();
            
        List<HtmlElement> elements = new List<HtmlElement>();
            
        try
        {
            // We'll use a direct string approach rather than relying solely on regex
            string lowerHtml = html.ToLower();
            string lowerTagName = tagName.ToLower();
            bool anyTag = lowerTagName == "*";
            
            int currentPos = 0;
            while (currentPos < html.Length)
            {
                // Find the next opening tag
                int openTagStart = -1;
                string currentTagName = null;
                
                if (anyTag)
                {
                    openTagStart = lowerHtml.IndexOf('<', currentPos);
                    if (openTagStart >= 0)
                    {
                        // Check if this is an HTML tag (not a comment or other special tag)
                        int tagNameStart = openTagStart + 1;
                        if (tagNameStart < lowerHtml.Length && IsValidTagNameStart(lowerHtml[tagNameStart]))
                        {
                            // Extract the tag name
                            int tagNameEnd = FindTagNameEnd(lowerHtml, tagNameStart);
                            if (tagNameEnd > tagNameStart)
                            {
                                currentTagName = lowerHtml.Substring(tagNameStart, tagNameEnd - tagNameStart);
                            }
                        }
                    }
                }
                else
                {
                    // Search for the specific tag
                    string openTag = "<" + lowerTagName;
                    openTagStart = lowerHtml.IndexOf(openTag, currentPos);
                    if (openTagStart >= 0)
                    {
                        currentTagName = lowerTagName;
                    }
                }
                
                if (openTagStart < 0 || currentTagName == null)
                    break; // No more tags found
                
                // Find the end of the opening tag
                int openTagEnd = lowerHtml.IndexOf('>', openTagStart);
                if (openTagEnd < 0)
                    break; // Invalid HTML
                
                bool isSelfClosing = lowerHtml[openTagEnd - 1] == '/' || IsSelfClosingTag(currentTagName);
                
                string fullTag = null;
                string innerContent = null;
                string tagAttributes = null;
                
                if (isSelfClosing)
                {
                    fullTag = html.Substring(openTagStart, openTagEnd - openTagStart + 1);
                    innerContent = string.Empty;
                    tagAttributes = html.Substring(openTagStart + currentTagName.Length + 1, 
                                                 openTagEnd - (openTagStart + currentTagName.Length + 1) - (lowerHtml[openTagEnd - 1] == '/' ? 1 : 0));
                    currentPos = openTagEnd + 1;
                }
                else
                {
                    // Find the closing tag
                    string closeTag = "</" + currentTagName + ">";
                    int closeTagStart = lowerHtml.IndexOf(closeTag, openTagEnd);
                    
                    if (closeTagStart < 0)
                    {
                        // No proper closing tag found, treat as self-closing or move on
                        fullTag = html.Substring(openTagStart, openTagEnd - openTagStart + 1);
                        innerContent = string.Empty;
                        tagAttributes = html.Substring(openTagStart + currentTagName.Length + 1, 
                                                     openTagEnd - (openTagStart + currentTagName.Length + 1));
                        currentPos = openTagEnd + 1;
                    }
                    else
                    {
                        // Extract full tag and inner content
                        fullTag = html.Substring(openTagStart, closeTagStart + closeTag.Length - openTagStart);
                        innerContent = html.Substring(openTagEnd + 1, closeTagStart - (openTagEnd + 1));
                        tagAttributes = html.Substring(openTagStart + currentTagName.Length + 1, 
                                                     openTagEnd - (openTagStart + currentTagName.Length + 1));
                        currentPos = closeTagStart + closeTag.Length;
                    }
                }
                
                // Parse attributes
                Dictionary<string, string> parsedAttributes = ParseAttributes(tagAttributes.Trim());
                
                // Apply attribute filters
                if (attributes != null && attributes.Count > 0)
                {
                    bool matchesAllAttributes = true;
                    foreach (var attr in attributes)
                    {
                        if (!parsedAttributes.TryGetValue(attr.Key.ToLower(), out string value) || value != attr.Value)
                        {
                            matchesAllAttributes = false;
                            break;
                        }
                    }
                    
                    if (!matchesAllAttributes)
                    {
                        continue; // Skip this element as it doesn't match the attribute filter
                    }
                }
                
                // Create the HTML element
                var element = new HtmlElement
                {
                    TagName = anyTag ? currentTagName : tagName,
                    OuterHtml = fullTag,
                    InnerHtml = innerContent,
                    InnerText = ExtractTextFromHtml(innerContent),
                    IsSelfClosing = isSelfClosing
                };
                
                // Add attributes to the element
                foreach (var attr in parsedAttributes)
                {
                    element.Attributes[attr.Key] = attr.Value;
                }
                
                elements.Add(element);
            }
        }
        catch (Exception ex)
        {
            // Log or handle parsing exceptions
            System.Diagnostics.Trace.WriteLine($"HTML parsing error: {ex.Message}");
        }
        
        return elements;
    }
    
    // Check if a character is a valid start for a tag name
    private bool IsValidTagNameStart(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }
    
    // Find the end of a tag name
    private int FindTagNameEnd(string html, int startPos)
    {
        for (int i = startPos; i < html.Length; i++)
        {
            char c = html[i];
            if (c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '>' || c == '/')
                return i;
        }
        return html.Length;
    }
    
    // Check if a tag is self-closing
    private bool IsSelfClosingTag(string tagName)
    {
        string[] selfClosingTags = { "area", "base", "br", "col", "embed", "hr", "img", "input", 
                                    "link", "meta", "param", "source", "track", "wbr" };
        return Array.IndexOf(selfClosingTags, tagName.ToLower()) >= 0;
    }
    
    // Parse HTML attributes - improved implementation
    private Dictionary<string, string> ParseAttributes(string attributesString)
    {
        var attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        
        if (string.IsNullOrEmpty(attributesString))
            return attributes;
        
        try
        {
            // Use regex to extract attributes in a more robust way
            string pattern = @"([a-zA-Z0-9_\-:]+)(?:\s*=\s*(?:""([^""]*)""|'([^']*)'|([^\s>]*)))?";
            var matches = Regex.Matches(attributesString, pattern);
            
            foreach (Match match in matches)
            {
                if (match.Groups[1].Success)
                {
                    string name = match.Groups[1].Value.ToLower();
                    string value = match.Groups[2].Success ? match.Groups[2].Value :
                                  match.Groups[3].Success ? match.Groups[3].Value :
                                  match.Groups[4].Success ? match.Groups[4].Value : string.Empty;
                    
                    attributes[name] = value;
                }
            }
        }
        catch (Exception ex)
        {
            // Log the error but don't crash
            System.Diagnostics.Trace.WriteLine($"Error parsing attributes: {ex.Message}");
            
            // Fallback to manual parsing
            try
            {
                int pos = 0;
                while (pos < attributesString.Length)
                {
                    // Skip whitespace
                    while (pos < attributesString.Length && char.IsWhiteSpace(attributesString[pos]))
                        pos++;
                        
                    if (pos >= attributesString.Length)
                        break;
                        
                    // Find attribute name
                    int nameStart = pos;
                    while (pos < attributesString.Length && 
                          !char.IsWhiteSpace(attributesString[pos]) && 
                          attributesString[pos] != '=' && 
                          attributesString[pos] != '>' && 
                          attributesString[pos] != '/')
                        pos++;
                    
                    if (nameStart == pos) // No name found
                    {
                        pos++;
                        continue;
                    }
                    
                    string name = attributesString.Substring(nameStart, pos - nameStart).ToLower();
                    
                    // Skip whitespace after name
                    while (pos < attributesString.Length && char.IsWhiteSpace(attributesString[pos]))
                        pos++;
                        
                    if (pos >= attributesString.Length || attributesString[pos] != '=')
                    {
                        // Boolean attribute (no value)
                        attributes[name] = string.Empty;
                        continue;
                    }
                    
                    pos++; // Skip the equals sign
                    
                    // Skip whitespace after equals sign
                    while (pos < attributesString.Length && char.IsWhiteSpace(attributesString[pos]))
                        pos++;
                        
                    if (pos >= attributesString.Length)
                        break;
                        
                    // Parse attribute value
                    string value;
                    if (attributesString[pos] == '"' || attributesString[pos] == '\'')
                    {
                        // Quoted value
                        char quote = attributesString[pos];
                        int valueStart = pos + 1;
                        pos = attributesString.IndexOf(quote, valueStart);
                        
                        if (pos < 0) // No closing quote
                        {
                            pos = attributesString.Length;
                            value = attributesString.Substring(valueStart);
                        }
                        else
                        {
                            value = attributesString.Substring(valueStart, pos - valueStart);
                            pos++; // Skip closing quote
                        }
                    }
                    else
                    {
                        // Unquoted value
                        int valueStart = pos;
                        while (pos < attributesString.Length && 
                              !char.IsWhiteSpace(attributesString[pos]) && 
                              attributesString[pos] != '>' && 
                              attributesString[pos] != '/')
                            pos++;
                        
                        value = attributesString.Substring(valueStart, pos - valueStart);
                    }
                    
                    attributes[name] = value;
                }
            }
            catch
            {
                // Last resort fallback - if everything fails, at least return something
                System.Diagnostics.Trace.WriteLine("Fallback parsing also failed");
            }
        }
        
        return attributes;
    }
    
    // Find elements by class name - improved implementation
    private List<HtmlElement> FindElementsByClass(string html, string className)
    {
        if (string.IsNullOrEmpty(html) || string.IsNullOrEmpty(className))
            return new List<HtmlElement>();
            
        // Directly search for elements with class attribute
        List<HtmlElement> result = new List<HtmlElement>();
        
        // This pattern looks for elements with class attribute that contains the specified class
        string pattern = $@"<([a-zA-Z][a-zA-Z0-9\-_:]*)([^>]*?class\s*=\s*[""'](?:[^""']*\s)?{Regex.Escape(className)}(?:\s[^""']*)?[""'][^>]*?)>(.*?)</\1>|" +
                         $@"<([a-zA-Z][a-zA-Z0-9\-_:]*)([^>]*?class\s*=\s*[""'](?:[^""']*\s)?{Regex.Escape(className)}(?:\s[^""']*)?[""'][^>]*?)/>"; 
        
        var matches = Regex.Matches(html, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        
        foreach (Match match in matches)
        {
            // Determine if this is a self-closing tag or a normal tag
            bool isSelfClosing;
            string tagName;
            string fullTag = match.Value;
            string tagAttributes;
            string innerContent;
            
            if (match.Groups[4].Success) // Self-closing tag
            {
                isSelfClosing = true;
                tagName = match.Groups[4].Value;
                tagAttributes = match.Groups[5].Value;
                innerContent = string.Empty;
            }
            else // Normal tag
            {
                isSelfClosing = false;
                tagName = match.Groups[1].Value;
                tagAttributes = match.Groups[2].Value;
                innerContent = match.Groups[3].Value;
            }
            
            // Create the HTML element
            var element = new HtmlElement
            {
                TagName = tagName,
                OuterHtml = fullTag,
                InnerHtml = innerContent,
                InnerText = ExtractTextFromHtml(innerContent),
                IsSelfClosing = isSelfClosing
            };
            
            // Parse attributes
            var parsedAttributes = ParseAttributes(tagAttributes);
            foreach (var attr in parsedAttributes)
            {
                element.Attributes[attr.Key] = attr.Value;
            }
            
            // Double-check the class attribute to ensure it actually contains our target class
            if (element.Attributes.TryGetValue("class", out string classValue))
            {
                string[] classes = classValue.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (Array.Exists(classes, c => c.Equals(className, StringComparison.OrdinalIgnoreCase)))
                {
                    result.Add(element);
                }
            }
        }
        
        return result;
    }
    
    // Find elements with a specific attribute
    private List<HtmlElement> FindElementsWithAttribute(string html, string tagName, string attributeName)
    {
        if (string.IsNullOrEmpty(html) || string.IsNullOrEmpty(attributeName))
            return new List<HtmlElement>();
            
        // Create attribute dictionary for filtering
        var attrFilter = new Dictionary<string, string>
        {
            { attributeName, "*" } // Any value
        };
        
        // Use the main element finder with attribute filter
        var elements = FindAllElements(html, tagName, null);
        
        // Filter elements that have the specified attribute
        return elements.Where(e => e.Attributes.ContainsKey(attributeName)).ToList();
    }
    
    // Extract plain text from HTML
    private string ExtractTextFromHtml(string html)
    {
        if (string.IsNullOrEmpty(html))
            return string.Empty;
            
        // Remove tags
        string text = Regex.Replace(html, @"<[^>]*>", string.Empty);
        
        // Decode HTML entities
        text = HttpUtility.HtmlDecode(text);
        
        // Remove extra whitespace
        text = Regex.Replace(text, @"\s+", " ").Trim();
        
        return text;
    }
    
    // Parse HTML table
    private TableData ParseHtmlTable(HtmlElement tableElement, bool hasHeaders)
    {
        var result = new TableData();
        
        try
        {
            // Find all rows
            var rows = FindAllElements(tableElement.InnerHtml, "tr", null);
            
            if (rows.Count == 0)
                return result;
                
            // Check for header rows (with th elements)
            bool hasHeaderRow = false;
            if (hasHeaders)
            {
                var firstRow = rows[0];
                var headerCells = FindAllElements(firstRow.InnerHtml, "th", null);
                
                if (headerCells.Count > 0)
                {
                    hasHeaderRow = true;
                    foreach (var cell in headerCells)
                    {
                        result.Headers.Add(cell.InnerText.Trim());
                    }
                }
                else
                {
                    // Use first row as header
                    var cells = FindAllElements(firstRow.InnerHtml, "td", null);
                    foreach (var cell in cells)
                    {
                        result.Headers.Add(cell.InnerText.Trim());
                    }
                }
            }
            
            // Process data rows
            for (int i = (hasHeaders ? 1 : 0); i < rows.Count; i++)
            {
                var row = rows[i];
                
                // Skip rows with th elements if not processing headers
                if (!hasHeaders && row.InnerHtml.Contains("<th"))
                    continue;
                    
                var cells = FindAllElements(row.InnerHtml, "td", null);
                if (cells.Count == 0)
                    continue;
                    
                var rowData = new List<string>();
                foreach (var cell in cells)
                {
                    rowData.Add(cell.InnerText.Trim());
                }
                
                result.Rows.Add(rowData);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.WriteLine($"Table parsing error: {ex.Message}");
        }
        
        return result;
    }
    
    #endregion
    
    #region Helper Classes
    
    // Class representing an HTML element
    public class HtmlElement
    {
        public string TagName { get; set; }
        public string OuterHtml { get; set; }
        public string InnerHtml { get; set; }
        public string InnerText { get; set; }
        public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public bool IsSelfClosing { get; set; }
        
        // Get attribute value
        public string GetAttribute(string name)
        {
            if (Attributes.TryGetValue(name, out string value))
                return value;
                
            return null;
        }
        
        // Convert to JSON format
        public JObject ToJson()
        {
            var attributesObj = new JObject();
            foreach (var attr in Attributes)
            {
                attributesObj[attr.Key] = attr.Value;
            }
            
            return new JObject
            {
                ["tag"] = TagName,
                ["outerHtml"] = OuterHtml,
                ["innerHtml"] = InnerHtml,
                ["innerText"] = InnerText,
                ["attributes"] = attributesObj,
                ["isSelfClosing"] = IsSelfClosing
            };
        }
    }
    
    // Class representing table data
    public class TableData
    {
        public List<string> Headers { get; } = new List<string>();
        public List<List<string>> Rows { get; } = new List<List<string>>();
    }
    
    #endregion
    
    #region Utility Methods
    
    private HttpResponseMessage CreateErrorResponse(string message)
    {
        var errorObject = new JObject
        {
            ["success"] = false,
            ["error"] = message
        };
        
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent(errorObject.ToString());
        return response;
    }
    
    #endregion
}