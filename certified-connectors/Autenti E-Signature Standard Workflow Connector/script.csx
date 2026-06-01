using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
 
public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        Context.Logger.LogInformation($"Received request with OperationId: {Context.OperationId}");
        Context.Logger.LogInformation("Starting request transformation...");
 
        // Aktualizacja żądania
        await UpdateRequest();
 
        HttpResponseMessage response;
        switch (Context.OperationId)
        {
            case "Actions_availability":
                response = await TransformActionsAvailabilityResponse();
                break;
            case "Download_file":
                response = await TransformDownloadFileResponse();
                break;
            case "Document_change":
                response = await TransformDocumentChangeResponse();
                break;
            default:
                // Wysłanie przekształconego żądania dalej
                Context.Logger.LogInformation("Sending the request forward...");
                response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
                Context.Logger.LogInformation($"Received response with status: {response.StatusCode}");
                break;
        }
 
        return response;
    }
 
        private async Task UpdateRequest()
    {
        switch (Context.OperationId)
        {
            case "Document_process_participants":
                await TransformDocumentProcessParticipantsRequest();
                break;
            case "Actions_availability":
                await TransformActionsAvailabilityRequest();
                break;
            default:
                Context.Logger.LogWarning($"Unknown OperationId: {Context.OperationId}");
                break;
        }
    }
 
private async Task TransformActionsAvailabilityRequest()
{
    var requestData = await Context.Request.Content.ReadAsStringAsync();
    var inputObject = JObject.Parse(requestData);
    var eventType = (string)inputObject["event_type"];

    // Dodane warunki dotyczące zmiany wartości eventType
    if (!string.IsNullOrEmpty(eventType))
    {
        if (eventType == "Start document signing process")
        {
            eventType = "DOCUMENT_SENT";
        }
        else if (eventType == "Withdraw (stop) document process")
        {
            eventType = "DOCUMENT_WITHDRAWAL";
        }

        const string classifiers = "CHALLENGE_CLASSIFIER-UNIQUE_TYPE:ACTION_SELECTION";
        string xAssertion = $@"{{""classifiers"":[""{classifiers}""],""attributes"":{{""selectedIds"":[""EVENT_CLASSIFIER-UNIQUE_TYPE:{eventType}""]}}}}";
        string base64XAssertion = Convert.ToBase64String(Encoding.UTF8.GetBytes(xAssertion));

        Context.Request.Headers.Add("X-ASSERTION", base64XAssertion);
    }
}
 
private async Task<HttpResponseMessage> TransformActionsAvailabilityResponse()
{
    // Pobranie odpowiedzi
    HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
    Context.Logger.LogInformation($"Received response with status: {response.StatusCode}");

    if (response.StatusCode == HttpStatusCode.Forbidden && response.Headers.Contains("x-challenge"))
    {
        string challengeHeader = response.Headers.GetValues("x-challenge").FirstOrDefault();
        string challengeJson = Encoding.UTF8.GetString(Base64UrlDecode(challengeHeader));
        Context.Logger.LogInformation($"Decoded challengeJson: {challengeJson}");

        var challengeObject = JObject.Parse(challengeJson);

        var newContent = new StringContent(challengeObject.ToString(), Encoding.UTF8, "application/json");
        var newResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = newContent
        };

        Context.Logger.LogInformation("Successfully transformed the Actions_availability response.");
        return newResponse;
    }
    else
    {
        // Oryginalna odpowiedź API
        var originalResponseContent = await response.Content.ReadAsStringAsync();

        // Tworzenie nowego obiektu JSON z właściwością 'body'
        var newJsonObject = new JObject
        {
            { "body", JArray.Parse(originalResponseContent) }
        };

        // Tworzenie nowej odpowiedzi z poprawionym obiektem JSON
        var newContent = new StringContent(newJsonObject.ToString(), Encoding.UTF8, "application/json");
        var newResponse = new HttpResponseMessage(response.StatusCode)
        {
            Content = newContent
        };

        Context.Logger.LogInformation("Transformed the Actions_availability response by wrapping it in an object with 'body' property.");
        return newResponse;
    }

    // Zwrócenie oryginalnej odpowiedzi, jeśli transformacja nie była możliwa
    return response;
}


    
private async Task<HttpResponseMessage> TransformDownloadFileResponse()
{
    HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
 
    if (response.IsSuccessStatusCode)
    {
        var byteArray = await response.Content.ReadAsByteArrayAsync();
        var base64String = Convert.ToBase64String(byteArray);
        // Tworzenie odpowiedzi w formie JSON z polem "file"
        var jsonResponse = new JObject
        {
            ["file"] = base64String
        };
 
        var newContent = new StringContent(jsonResponse.ToString(), Encoding.UTF8, "application/json");
 
        var newResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = newContent
        };
        return newResponse;
    }
 
    return response;  // Zwróć oryginalną odpowiedź, jeśli nie powiodło się
}
 
    private async Task TransformDocumentProcessParticipantsRequest()
    {
        try
        {
            var requestData = await Context.Request.Content.ReadAsStringAsync();
            var inputObject = JObject.Parse(requestData);
            var outputData = new JObject
            {
                ["party"] = new JObject
                {
                    ["firstName"] = inputObject["firstName"],
                    ["lastName"] = inputObject["lastName"],
                    ["contacts"] = new JArray
                    {
                        new JObject
                        {
                            ["type"] = inputObject["contacts_type"],
                            ["attributes"] = new JObject
                            {
                                ["email"] = inputObject["contacts_attributes_email"]
                            }
                        }
                    },
                    ["relationships"] = new JArray
                    {
                        new JObject
                        {
                            ["type"] = inputObject["relationships_type"],
                            ["party"] = new JObject
                            {
                                ["name"] = inputObject["relationships_party_name"],
                                ["extIds"] = new JArray
                                {
                                    new JObject
                                    {
                                        ["identificationSpace"] = inputObject["relationships_party_extIds_identificationSpace"],
                                        ["identifier"] = inputObject["relationships_party_extIds_identifier"]
                                    }
                                }
                            },
                            ["attributes"] = new JObject
                            {
                                ["relationshipDescription"] = inputObject["relationships_attributes_relationshipDescription"]
                            }
                        }
                    }
                },
                ["role"] = inputObject["role"],
                ["constraints"] = new JArray
                {
                    new JObject
                    {
                        ["constrainedActions"] = new JArray { inputObject["constraints_constrainedActions"] },
                        ["classifiers"] = new JArray { inputObject["constraints_classifiers"] },
                        ["attributes"] = new JObject
                        {
                            ["requiredClassifiers"] = new JArray { inputObject["constraints_attributes_requiredClassifiers"] }
                        }
                    }
                }
            };
        
            var transformedContent = JsonConvert.SerializeObject(outputData, Newtonsoft.Json.Formatting.Indented);
            Context.Request.Content = new StringContent(transformedContent, System.Text.Encoding.UTF8, "application/json");
        }
        catch (Exception ex)
        {
            Context.Logger.LogError(ex, "Wystąpił błąd podczas przetwarzania żądania dla Document_process_participants");
        }
    }
 
 
private async Task<HttpResponseMessage> TransformDocumentChangeResponse()
{
    // Pobierz oryginalną odpowiedź
    HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
    
    // Odczytaj zawartość odpowiedzi
    string responseContent = await response.Content.ReadAsStringAsync();
    JArray jsonArray = JArray.Parse(responseContent);
 
    // Zakładając, że odpowiedź zawsze zawiera co najmniej jeden element
    if(jsonArray.Count > 0)
    {
        JObject firstObject = (JObject)jsonArray[0];
        string eventType = firstObject["eventType"].ToString();
        string id = firstObject["object"]["id"].ToString();
        string type = firstObject["object"]["type"].ToString();
 
        // Utwórz nową zawartość odpowiedzi
        JObject newResponseContent = new JObject
        {
            ["body"] = $"eventType: {eventType}, id: {id}, type: {type}"
        };
 
        // Ustaw nową zawartość dla odpowiedzi
        response.Content = new StringContent(newResponseContent.ToString(), Encoding.UTF8, "application/json");
    }
 
    return response;
}
 
 
private bool TryParseResponseBody(string responseContent, out string responseBody)
{
    try
    {
        var responseObject = JObject.Parse(responseContent);
        if (responseObject.ContainsKey("body"))
        {
            responseBody = responseObject["body"].ToString();
            return true;
        }
    }
    catch (Exception ex)
    {
        Context.Logger.LogError($"Error parsing response content: {ex.Message}");
    }
 
    responseBody = null;
    return false;
}
 
    private byte[] Base64UrlDecode(string base64Url)
    {
        string padded = base64Url.Length % 4 == 0
            ? base64Url : base64Url + "====".Substring(base64Url.Length % 4);
        string base64 = padded.Replace("_", "/")
                              .Replace("-", "+");
        return Convert.FromBase64String(base64);
    }
}