using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var _logger = this.Context.Logger;
        
        var authorizationHeader = "Bot " + this.Context.Request.Headers.Authorization.ToString();
        this.Context.Request.Headers.Authorization = AuthenticationHeaderValue.Parse(authorizationHeader);

        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken);
        
        if (response.IsSuccessStatusCode && response.Content != null)
        {
            string contentType = response.Content.Headers.ContentType?.MediaType;
            
            if (this.Context.OperationId == "SmallWebGet" && contentType != null && contentType.Contains("xml"))
            {
                string xmlContent = await response.Content.ReadAsStringAsync();
                
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);
                
                JObject feedJson = new JObject();
                XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
                nsManager.AddNamespace("atom", "http://www.w3.org/2005/Atom");
                
                // Extract feed details
                feedJson["title"] = doc.SelectSingleNode("//atom:feed/atom:title", nsManager)?.InnerText;
                feedJson["id"] = doc.SelectSingleNode("//atom:feed/atom:id", nsManager)?.InnerText;
                feedJson["updated"] = doc.SelectSingleNode("//atom:feed/atom:updated", nsManager)?.InnerText;
                
                // Process entries
                JArray entries = new JArray();
                XmlNodeList entryNodes = doc.SelectNodes("//atom:feed/atom:entry", nsManager);
                
                foreach (XmlNode entry in entryNodes)
                {
                    JObject entryJson = new JObject();
                    entryJson["title"] = entry.SelectSingleNode("atom:title", nsManager)?.InnerText;
                    entryJson["id"] = entry.SelectSingleNode("atom:id", nsManager)?.InnerText;
                    entryJson["updated"] = entry.SelectSingleNode("atom:updated", nsManager)?.InnerText;
                    entryJson["published"] = entry.SelectSingleNode("atom:published", nsManager)?.InnerText;
                    
                    XmlNode linkNode = entry.SelectSingleNode("atom:link", nsManager);
                    if (linkNode != null)
                    {
                        entryJson["url"] = linkNode.Attributes["href"]?.Value;
                    }
                    
                    XmlNode authorNode = entry.SelectSingleNode("atom:author/atom:name", nsManager);
                    if (authorNode != null)
                    {
                        entryJson["author"] = authorNode.InnerText;
                    }
                    
                    entries.Add(entryJson);
                }
                
                feedJson["entries"] = entries;
                
                response.Content = new StringContent(
                    feedJson.ToString(),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
            }
            else if (contentType != null && contentType.Contains("application/json"))
            {
                string content = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(content);
                
                if (json["thumbnail"] != null && json["thumbnail"]["url"] != null)
                {
                    string urlValue = json["thumbnail"]["url"].ToString();
                    json["thumbnail"]["url"] = "https://kagi.com" + urlValue;
                    
                    response.Content = new StringContent(
                        json.ToString(), 
                        System.Text.Encoding.UTF8, 
                        "application/json"
                    );
                }
            }
        }
        
        return response;
    }
}