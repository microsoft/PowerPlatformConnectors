using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(responseString))
            {
                var transformedJson = ConvertNullsToEmptyStrings(responseString);
                response.Content = CreateJsonContent(transformedJson);
            }
        }

        return response;
    }
    private string ConvertNullsToEmptyStrings(string jsonString)
    {
        if (string.IsNullOrEmpty(jsonString))
            return jsonString;
        try
        {
            var token = JToken.Parse(jsonString);
            var typeCache = new Dictionary<string, JTokenType>();

            BuildTypeCache(token, typeCache);
            ProcessToken(token, typeCache);
            return token.ToString(Newtonsoft.Json.Formatting.None);
        }
        catch (JsonReaderException)
        {
            return jsonString;
        }
    }
    private void ProcessToken(JToken token, Dictionary<string, JTokenType> typeCache)
    {
        switch (token.Type)
        {
            case JTokenType.Object:
                foreach (var property in token.Children<JProperty>().ToList())
                {
                    if (property.Value.Type == JTokenType.Null)
                    {
                        if (property.Name.EndsWith("_id"))
                        {
                            property.Remove();
                        }
                        else if (typeCache.TryGetValue(property.Name, out var cachedType))
                        {
                            if (cachedType == JTokenType.Boolean)
                            {
                                property.Remove();
                            }
                            else
                            {
                                property.Value = GetDefaultForType(cachedType);
                            }
                        }
                        else
                        {
                            var inferredType = InferTypeFromPropertyName(property.Name);
                            if (inferredType == JTokenType.Boolean)
                            {
                                property.Remove();
                            }
                            else
                            {
                                property.Value = GetDefaultForType(inferredType);
                            }
                        }
                    }
                    else
                    {
                        ProcessToken(property.Value, typeCache);
                    }
                }
                break;

            case JTokenType.Array:
                foreach (var item in token.Children().ToList())
                {
                    if (item.Type == JTokenType.Null)
                    {
                        item.Replace("");
                    }
                    else
                    {
                        ProcessToken(item, typeCache);
                    }
                }
                break;
        }
    }
    private void BuildTypeCache(JToken token, Dictionary<string, JTokenType> typeCache)
    {
        if (token.Type == JTokenType.Array)
        {
            foreach (var item in token.Children())
            {
                if (item.Type == JTokenType.Object)
                {
                    foreach (var prop in item.Children<JProperty>())
                    {
                        if (prop.Value.Type != JTokenType.Null && !typeCache.ContainsKey(prop.Name))
                        {
                            typeCache[prop.Name] = prop.Value.Type;
                        }
                    }
                }
            }
        }
        else if (token.Type == JTokenType.Object)
        {
            foreach (var prop in token.Children<JProperty>())
            {
                if (prop.Value.Type != JTokenType.Null && !typeCache.ContainsKey(prop.Name))
                {
                    typeCache[prop.Name] = prop.Value.Type;
                }
            }
        }
    }
    private JToken GetDefaultForType(JTokenType tokenType)
    {
        switch (tokenType)
        {
            case JTokenType.Integer:
            case JTokenType.Float:
                return 0;
            case JTokenType.String:
                return "";
            case JTokenType.Array:
                return new JArray();
            case JTokenType.Object:
                return new JObject();
            default:
                return "";
        }
    }

    // Add specific property name checks to infer types    
    private JTokenType InferTypeFromPropertyName(string propertyName)
    {
        var lowerName = propertyName.ToLower();

        // Date/time fields should be strings (highest priority)
        if (lowerName.EndsWith("_at") ||
            lowerName.EndsWith("_updated") ||
            lowerName.EndsWith("_date") ||
            lowerName.EndsWith("_time") ||
            lowerName.Contains("created") ||
            lowerName.Contains("updated") ||
            lowerName.Contains("deleted") ||
            lowerName.Equals("at"))
        {
            return JTokenType.String;
        }

        if (lowerName.Contains("hours") ||
            lowerName.Contains("seconds") ||
            lowerName.Contains("count") ||
            lowerName.EndsWith("_count") ||
            lowerName.Equals("total_count"))
        {
            return JTokenType.Integer;
        }

        if (lowerName.Contains("rate") ||
            lowerName.Contains("fee") ||
            lowerName.Contains("cost") ||
            lowerName.Contains("amount") ||
            lowerName.Contains("price"))
        {
            return JTokenType.Float;
        }
        if (lowerName.Contains("parameters") &&
            (lowerName.Contains("recurring") || lowerName.EndsWith("_parameters")))
        {
            return JTokenType.Array;
        }

        if (lowerName.Contains("metadata") ||
            lowerName.Contains("_config") ||
            lowerName.Contains("settings") ||
            lowerName.Contains("_object") ||
            lowerName.EndsWith("_data"))
        {
            return JTokenType.Object;
        }

        return JTokenType.String;
    }
}
