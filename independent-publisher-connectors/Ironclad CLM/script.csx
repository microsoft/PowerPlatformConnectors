using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.IO;
using System.Web;

public class Script : ScriptBase
{
    private JObject recordSchemaInfo;

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if ("RetrieveRecord".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            await rtrRcd_RetrieveRecordSchemaInformation().ConfigureAwait(false);
        }

        await this.UpdateRequest().ConfigureAwait(false);
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
            await this.UpdateResponse(response).ConfigureAwait(false);
        }
        return response;
    }

    private async Task UpdateRequest()
    {
        switch (this.Context.OperationId)
        {
            case "CreateSignedCopyAttachment":
            case "CreateAttachment":
                await crtAtt_TransformToMultipartRequestForRecords().ConfigureAwait(false);
                break;
            case "CreateWorkflow":
                await crtWfl_TransformToMultipartRequestForWorkflows().ConfigureAwait(false);
                break;
            case "CreateWorkflowAsync":
                await crtAsyncWfl_TransformRequestJson().ConfigureAwait(false);
                break;
            case "CreateRecord":
            case "ReplaceRecord":
                await crtRcd_TransformCreateRecordRequest().ConfigureAwait(false);
                break;
            case "UpdateRecordMetadata":
                await updRcd_TransformUpdateRecordMetadataRequest().ConfigureAwait(false);
                break;
            case "CreateWorkflowDocument":
                await crtWflDoc_TransformToMultipartRequest().ConfigureAwait(false);
                break;
            case "UpdateGroup":
                await updGrp_TransformUpdateGroupRequest().ConfigureAwait(false);
                break;
            case "UpdateUser":
                await updUsr_TransformUpdateUserRequest().ConfigureAwait(false);
                break;
        }
    }

    private async Task UpdateResponse(HttpResponseMessage response)
    {
        switch (this.Context.OperationId)
        {
            case "ListUsers":
                await this.TransformResponseJsonBody(this.lstUsr_TransformUsersList, response).ConfigureAwait(false);
                break;
            case "RetrieveWorkflowSchema":
                await this.TransformResponseJsonBody(this.rtrWflSch_TransformRetrieveWorkflowSchema, response).ConfigureAwait(false);
                break;
            case "RetrieveWorkflow":
                await this.TransformResponseJsonBody(this.rtrWfl_TransformRetrieveWorkflow, response).ConfigureAwait(false);
                break;
            case "RetrieveRecord":
                await this.TransformResponseJsonBody(this.rtrRcd_TransformRetrieveRecord, response).ConfigureAwait(false);
                break;
            case "RetrieveEmailThread":
                await this.TransformResponseJsonBody(this.rtrEml_TransformRetrieveEmailThread, response).ConfigureAwait(false);
                break;
            case "CreateRecord":
            case "ReplaceRecord":
            case "UpdateRecordMetadata":
                await this.TransformResponseJsonBody(this.crtRcd_TransformCreateRecordResponse, response).ConfigureAwait(false);
                break;
            case "ListAllRecords":
                // Get query parameter for properties
                var listAllRecordsQuery = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query)["recordPorperties"] ?? string.Empty;
                await this.TransformResponseJsonBody(body => lstAllRcd_TransformListAllRecordsResponse(body, listAllRecordsQuery), response).ConfigureAwait(false);
                break;
            case "RetrieveRecordSchemas":
                // Get query parameter for properties
                var retrieveRecordSchemasQuery = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query)["recordPorperties"] ?? string.Empty;
                await this.TransformResponseJsonBody(body => rtrRcdSch_TransformRetrieveRecordSchemas(body, retrieveRecordSchemasQuery), response).ConfigureAwait(false);
                break;
            case "ListAllWorkflows":
                await this.TransformResponseJsonBody(this.lstAllWfl_TransformListAllWorkflowsResponse, response).ConfigureAwait(false);
                break;  
        }
    }

    private async Task TransformResponseJsonBody(Func<JObject, JObject> transformationFunction, HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (!String.IsNullOrWhiteSpace(content))
        {
            var body = JObject.Parse(content);
            body = transformationFunction(body);
            response.Content = CreateJsonContent(body.ToString());
        }
    }

    // ################################################################################
    // Create Attachment / Create Signed Copy Attachment operations ###################
    // ################################################################################

    private async Task crtAtt_TransformToMultipartRequestForRecords()
    {
        var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonBody = JObject.Parse(content);

        var multipartContent = new MultipartFormDataContent();

        string filename = "document.pdf";
        if (jsonBody.TryGetValue("metadata", out var metadataToken) && metadataToken is JObject metadata)
        {
            filename = metadata["filename"]?.ToString() ?? filename;
        }

        if (jsonBody.TryGetValue("attachment", out var attachmentToken))
        {
            var attachmentBytes = Convert.FromBase64String(attachmentToken.ToString());
            var fileContent = new ByteArrayContent(attachmentBytes);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            multipartContent.Add(fileContent, "attachment", filename);
        }

        if (metadataToken != null)
        {
            var metadataJson = metadataToken.ToString();
            var metadataContent = new StringContent(metadataJson, Encoding.UTF8, "application/json");
            multipartContent.Add(metadataContent, "metadata");
        }

        this.Context.Request.Content = multipartContent;
    }

    // ################################################################################
    // Create Record ##################################################################
    // Replace Record #################################################################
    // ################################################################################

    private async Task<JObject> crtRcd_FetchRecordMetadata()
    {
        var baseUrl = this.Context.Request.RequestUri.GetLeftPart(UriPartial.Authority);
        var metadataUrl = new Uri(new Uri(baseUrl), "/public/api/v1/records/metadata");

        var request = new HttpRequestMessage(HttpMethod.Get, metadataUrl);
        
        foreach (var header in this.Context.Request.Headers)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        var response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JObject.Parse(content);
        }
        else
        {
            throw new Exception($"Failed to retrieve record metadata. Status code: {response.StatusCode}");
        }
    }

    private async Task crtRcd_TransformCreateRecordRequest()
    {
        var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonBody = JObject.Parse(content);

        if (jsonBody.TryGetValue("propertiesAsArray", out var propertiesAsArray))
        {
            var metadata = await crtRcd_FetchRecordMetadata();
            var metadataProperties = metadata["properties"] as JObject;

            var properties = new JObject();

            foreach (var prop in propertiesAsArray)
            {
                var systemPropertyName = prop["propertySystemName"].ToString();
                var value = prop["value"];

                string type = "unknown";
                if (metadataProperties != null && metadataProperties.TryGetValue(systemPropertyName, out var propertyInfo))
                {
                    type = propertyInfo["type"]?.ToString() ?? "unknown";
                }

                properties[systemPropertyName] = new JObject
                {
                    ["value"] = value,
                    ["type"] = type
                };
            }

            jsonBody["properties"] = properties;
            jsonBody.Remove("propertiesAsArray");

            this.Context.Request.Content = CreateJsonContent(jsonBody.ToString());
        }
    }

        // Add counterpartyName property to the response
    private JObject crtRcd_TransformCreateRecordResponse(JObject body)
    {
        if (body.ContainsKey("properties") && body["properties"] is JObject properties)
        {
            if (properties.ContainsKey("counterpartyName"))
            {
                body["counterpartyName"] = properties["counterpartyName"]["value"];
            }
            else
            {
                body["counterpartyName"] = null;
            }
        }
        else
        {
            body["counterpartyName"] = null;
        }

        return body;
    }

    // ################################################################################
    // Update Record Metadata #########################################################
    // ################################################################################

    private async Task<JObject> updRcd_FetchRecordMetadata()
    {
        var baseUrl = this.Context.Request.RequestUri.GetLeftPart(UriPartial.Authority);
        var metadataUrl = new Uri(new Uri(baseUrl), "/public/api/v1/records/metadata");

        var metadataRequest = new HttpRequestMessage(HttpMethod.Get, metadataUrl);
        
        foreach (var header in this.Context.Request.Headers)
        {
            metadataRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        var metadataResponse = await this.Context.SendAsync(metadataRequest, this.CancellationToken).ConfigureAwait(false);

        if (!metadataResponse.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to retrieve record metadata. Status code: {metadataResponse.StatusCode}");
        }

        var metadataContent = await metadataResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        return JObject.Parse(metadataContent);
    }

    private async Task updRcd_TransformUpdateRecordMetadataRequest()
    {
        var metadata = await updRcd_FetchRecordMetadata();
        var metadataProperties = metadata["properties"] as JObject;

        var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonBody = JObject.Parse(content);

        if (jsonBody.TryGetValue("addProperties", out var addPropertiesToken) && addPropertiesToken is JArray addPropertiesArray)
        {
            var transformedProperties = new JObject();

            foreach (var prop in addPropertiesArray)
            {
                if (prop is JObject propObject && 
                    propObject.TryGetValue("propertySystemName", out var propertySystemNameToken) &&
                    propObject.TryGetValue("value", out var valueToken))
                {
                    string propertySystemName = propertySystemNameToken.ToString();
                    JToken value = valueToken;

                    string type = "unknown";
                    if (metadataProperties != null && metadataProperties.TryGetValue(propertySystemName, out var propertyInfo))
                    {
                        type = propertyInfo["type"]?.ToString() ?? "unknown";
                    }

                    transformedProperties[propertySystemName] = new JObject
                    {
                        ["value"] = value,
                        ["type"] = type
                    };
                }
            }

            jsonBody["addProperties"] = transformedProperties;
            this.Context.Request.Content = CreateJsonContent(jsonBody.ToString());
        }
    }

    // ################################################################################
    // Create Workflow Asynchronously #################################################
    // ################################################################################

    private async Task crtAsyncWfl_TransformRequestJson()
    {
        var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonBody = JObject.Parse(content);

        // Unflatten the JSON structure
        jsonBody = crtAsyncWfl_UnflattenJson(jsonBody);

        // Update the request content with transformed JSON
        this.Context.Request.Content = CreateJsonContent(jsonBody.ToString());
    }

    private JObject crtAsyncWfl_UnflattenJson(JObject flatJson)
    {
        var result = new JObject();

        foreach (var prop in flatJson.Properties())
        {
            if (prop.Value is JArray array)
            {
                var unflattened = new JArray();
                foreach (var item in array)
                {
                    if (item is JObject objItem)
                    {
                        var unflattenedItem = new JObject();
                        var groupedProperties = objItem.Properties()
                            .GroupBy(p => p.Name.Split('/')[0])
                            .ToDictionary(g => g.Key, g => g.ToList());

                        foreach (var group in groupedProperties)
                        {
                            if (group.Value.Count == 1 && !group.Value[0].Name.Contains("/"))
                            {
                                // Simple property
                                unflattenedItem[group.Key] = group.Value[0].Value;
                            }
                            else
                            {
                                // Complex property
                                var complexObj = new JObject();
                                foreach (var complexProp in group.Value)
                                {
                                    var parts = complexProp.Name.Split('/');
                                    if (parts.Length == 1)
                                    {
                                        complexObj[parts[0]] = complexProp.Value;
                                    }
                                    else
                                    {
                                        var currentObj = complexObj;
                                        for (int i = 1; i < parts.Length - 1; i++)
                                        {
                                            if (!currentObj.ContainsKey(parts[i]))
                                            {
                                                currentObj[parts[i]] = new JObject();
                                            }
                                            currentObj = (JObject)currentObj[parts[i]];
                                        }
                                        currentObj[parts.Last()] = complexProp.Value;
                                    }
                                }
                                unflattenedItem[group.Key] = complexObj;
                            }
                        }
                        unflattened.Add(unflattenedItem);
                    }
                    else
                    {
                        // Handle primitive types (e.g., strings, numbers) by adding them directly
                        unflattened.Add(item);
                    }
                }
                result[prop.Name] = unflattened;
            }
            else if (prop.Value is JObject obj)
            {
                result[prop.Name] = crtWfl_UnflattenJson(obj);
            }
            else if (prop.Name.Contains("/"))
            {
                var parts = prop.Name.Split('/');
                var currentObj = result;
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    if (!currentObj.ContainsKey(parts[i]))
                    {
                        currentObj[parts[i]] = new JObject();
                    }
                    currentObj = (JObject)currentObj[parts[i]];
                }
                currentObj[parts.Last()] = prop.Value;
            }
            else
            {
                result[prop.Name] = prop.Value;
            }
        }

        return result;
    }

    // ################################################################################
    // Create Workflow Synchronously ##################################################
    // ################################################################################

    private async Task crtWfl_TransformToMultipartRequestForWorkflows()
    {
        var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonBody = JObject.Parse(content);

        // Unflatten the entire JSON body
        jsonBody = crtWfl_UnflattenJson(jsonBody);

        var multipartContent = new MultipartFormDataContent();

        if (jsonBody.TryGetValue("attributes", out var attributesToken) && attributesToken is JObject attributes)
        {
            crtWfl_ProcessWorkflowAttributes(attributes, multipartContent, jsonBody);
        }

        var dataContent = new StringContent(jsonBody.ToString(), Encoding.UTF8, "application/json");
        multipartContent.Add(dataContent, "data");

        this.Context.Request.Content = multipartContent;
    }

    private JObject crtWfl_UnflattenJson(JObject flatJson)
    {
        var result = new JObject();

        foreach (var prop in flatJson.Properties())
        {
            if (prop.Value is JArray array)
            {
                var unflattened = new JArray();
                foreach (var item in array)
                {
                    if (item is JObject objItem)
                    {
                        var unflattenedItem = new JObject();
                        var groupedProperties = objItem.Properties()
                            .GroupBy(p => p.Name.Split('/')[0])
                            .ToDictionary(g => g.Key, g => g.ToList());

                        foreach (var group in groupedProperties)
                        {
                            if (group.Value.Count == 1 && !group.Value[0].Name.Contains("/"))
                            {
                                // Simple property
                                unflattenedItem[group.Key] = group.Value[0].Value;
                            }
                            else
                            {
                                // Complex property
                                var complexObj = new JObject();
                                foreach (var complexProp in group.Value)
                                {
                                    var parts = complexProp.Name.Split('/');
                                    if (parts.Length == 1)
                                    {
                                        complexObj[parts[0]] = complexProp.Value;
                                    }
                                    else
                                    {
                                        var currentObj = complexObj;
                                        for (int i = 1; i < parts.Length - 1; i++)
                                        {
                                            if (!currentObj.ContainsKey(parts[i]))
                                            {
                                                currentObj[parts[i]] = new JObject();
                                            }
                                            currentObj = (JObject)currentObj[parts[i]];
                                        }
                                        currentObj[parts.Last()] = complexProp.Value;
                                    }
                                }
                                unflattenedItem[group.Key] = complexObj;
                            }
                        }
                        unflattened.Add(unflattenedItem);
                    }
                    else
                    {
                        // Handle primitive types (e.g., strings, numbers) by adding them directly
                        unflattened.Add(item);
                    }
                }
                result[prop.Name] = unflattened;
            }
            else if (prop.Value is JObject obj)
            {
                result[prop.Name] = crtWfl_UnflattenJson(obj);
            }
            else if (prop.Name.Contains("/"))
            {
                var parts = prop.Name.Split('/');
                var currentObj = result;
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    if (!currentObj.ContainsKey(parts[i]))
                    {
                        currentObj[parts[i]] = new JObject();
                    }
                    currentObj = (JObject)currentObj[parts[i]];
                }
                currentObj[parts.Last()] = prop.Value;
            }
            else
            {
                result[prop.Name] = prop.Value;
            }
        }

        return result;
    }

    private void crtWfl_ProcessWorkflowAttributes(JObject attributes, MultipartFormDataContent multipartContent, JObject jsonBody)
    {
        foreach (var attribute in attributes.Properties())
        {
            if (attribute.Value is JArray arrayValue && arrayValue.Any(item => item is JObject obj && obj.ContainsKey("fileContent")))
            {
                crtWfl_ProcessFileArrayAttribute(attribute.Name, arrayValue, multipartContent, jsonBody);
            }
        }
    }

    private void crtWfl_ProcessFileArrayAttribute(string attributeName, JArray arrayValue, MultipartFormDataContent multipartContent, JObject jsonBody)
    {
        var updatedArray = new JArray();

        foreach (var item in arrayValue)
        {
            if (item is JObject fileObject &&
                fileObject.TryGetValue("fileName", out var fileNameToken) &&
                fileObject.TryGetValue("fileContent", out var fileContentToken))
            {
                string fileName = fileNameToken.ToString();
                string fileContent = fileContentToken.ToString();

                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(fileContent))
                {
                    string fileKey = $"{attributeName}_{Guid.NewGuid().ToString("N")}";

                    var fileBytes = Convert.FromBase64String(fileContent);
                    var fileContentPart = new ByteArrayContent(fileBytes);
                    fileContentPart.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    multipartContent.Add(fileContentPart, fileKey, fileName);

                    updatedArray.Add(new JObject { ["file"] = fileKey });
                }
            }
        }

        jsonBody["attributes"][attributeName] = updatedArray;
    }

    // ################################################################################
    // Create Workflow Document operations ############################################
    // ################################################################################

    private async Task crtWflDoc_TransformToMultipartRequest()
    {
        var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonBody = JObject.Parse(content);

        var multipartContent = new MultipartFormDataContent();

        string filename = "document.pdf"; // Default filename as fallback
        if (jsonBody.TryGetValue("metadata", out var metadataToken) && metadataToken is JObject metadata)
        {
            filename = metadata["filename"]?.ToString() ?? filename;
        }

        if (jsonBody.TryGetValue("attachment", out var attachmentToken))
        {
            var attachmentBytes = Convert.FromBase64String(attachmentToken.ToString());
            var fileContent = new ByteArrayContent(attachmentBytes);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            multipartContent.Add(fileContent, "attachment", filename);
        }

        if (metadataToken != null)
        {
            var metadataJson = metadataToken.ToString();
            var metadataContent = new StringContent(metadataJson, Encoding.UTF8, "application/json");
            multipartContent.Add(metadataContent, "metadata");
        }

        this.Context.Request.Content = multipartContent;
    }

    // ################################################################################
    // List Users #####################################################################
    // ################################################################################

    private JObject lstUsr_TransformUsersList(JObject body)
    {
        var resources = body["Resources"] as JArray;
        if (resources != null)
        {
            var transformedResources = new JArray(
                resources.Select(user =>
                {
                    var givenName = user["name"]?["givenName"]?.ToString() ?? "";
                    var familyName = user["name"]?["familyName"]?.ToString() ?? "";
                    var displayName = $"{givenName} {familyName}".Trim();

                    var email = user["emails"]?.FirstOrDefault()?["value"]?.ToString().ToLower() ?? "";

                    user["displayName"] = displayName;
                    user["combinedLabel"] = $"{displayName} ({email})";

                    return user;
                })
            );

            body["Resources"] = transformedResources;
        }
        return body;
    }

    // ################################################################################
    // Update Group ###################################################################
    // ################################################################################

    private async Task updGrp_TransformUpdateGroupRequest()
    {
        var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonBody = JObject.Parse(content);

        // Ensure the "schemas" property exists and has the correct value
        if (!jsonBody.ContainsKey("schemas") || !(jsonBody["schemas"] is JArray))
        {
            jsonBody["schemas"] = new JArray();
        }

        var schemas = (JArray)jsonBody["schemas"];
        if (!schemas.Any(s => s.Type == JTokenType.String && s.Value<string>() == "urn:ietf:params:scim:api:messages:2.0:PatchOp"))
        {
            schemas.Clear(); // Remove any existing values
            schemas.Add("urn:ietf:params:scim:api:messages:2.0:PatchOp");
        }

        // Update the request content with the modified body
        this.Context.Request.Content = CreateJsonContent(jsonBody.ToString());
    }

    // ################################################################################
    // Update User ####################################################################
    // ################################################################################

    private async Task updUsr_TransformUpdateUserRequest()
    {
        var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonBody = JObject.Parse(content);

        // Ensure the "schemas" property exists and has the correct value
        if (!jsonBody.ContainsKey("schemas") || !(jsonBody["schemas"] is JArray))
        {
            jsonBody["schemas"] = new JArray();
        }

        var schemas = (JArray)jsonBody["schemas"];
        if (!schemas.Any(s => s.Type == JTokenType.String && s.Value<string>() == "urn:ietf:params:scim:api:messages:2.0:PatchOp"))
        {
            schemas.Clear(); // Remove any existing values
            schemas.Add("urn:ietf:params:scim:api:messages:2.0:PatchOp");
        }

        // Update the request content with the modified body
        this.Context.Request.Content = CreateJsonContent(jsonBody.ToString());
    }
 
    // ################################################################################
    // Retrieve Workflow Schema #######################################################
    // ################################################################################

    private JObject rtrWflSch_TransformRetrieveWorkflowSchema(JObject body)
    {
        var schema = body["schema"] as JObject;
        if (schema != null)
        {
            var launchSchema = new JObject();
            var formattedSchema = new JObject();
            var schemaAsArray = new JArray();
            var documentSchemaAsArray = new JArray();

            foreach (var property in schema.Properties())
            {
                string propertyName = property.Name;
                var propertyValue = property.Value as JObject;
                if (propertyValue != null)
                {
                    string displayName = propertyValue["displayName"]?.ToString() ?? propertyName;
                    string propertyType = propertyValue["type"]?.ToString().ToLower();
                    bool isReadOnly = propertyValue["readOnly"]?.ToObject<bool>() ?? false;

                    // Fix: Use the correct property type for special cases
                    string effectivePropertyType = propertyType;
                    if (propertyType == "object")
                    {
                        var objectType = propertyValue["objectType"]?.ToString().ToLower();
                        if (objectType == "address" || objectType == "monetaryamount" || objectType == "duration")
                        {
                            effectivePropertyType = objectType;
                        }
                    }

                    var formattedLaunchProperty = rtrWflSch_FormatLaunchProperty(effectivePropertyType, displayName, propertyName, propertyValue);
                    launchSchema[propertyName] = formattedLaunchProperty;

                    var formattedProperty = rtrWflSch_FormatProperty(effectivePropertyType, displayName, propertyName, propertyValue);
                    formattedSchema[propertyName] = formattedProperty;

                    var schemaArrayItem = rtrWflSch_ParseSchemaArrayItem(propertyName, displayName, effectivePropertyType, isReadOnly);
                    schemaAsArray.Add(schemaArrayItem);

                    if (propertyType == "array" && propertyValue["elementType"] is JObject arrayElementType && arrayElementType["type"]?.ToString().ToLower() == "document")
                    {
                        documentSchemaAsArray.Add(rtrWflSch_ParseDocumentSchemaItem(propertyName, displayName, isReadOnly));
                    }
                }
            }

            body["launchSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = launchSchema,
                ["required"] = new JArray { "counterpartyName" }  // Add the required property here
            };
            body["formattedSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = formattedSchema
            };
            body["schemaAsArray"] = schemaAsArray;
            body["documentSchemaAsArray"] = documentSchemaAsArray;
        }

        return body;
    }

    private JObject rtrWflSch_FormatLaunchProperty(string propertyType, string displayName, string propertyName, JObject propertyValue)
    {
        switch (propertyType)
        {
            case "array":
                return rtrWflSch_FormatArrayLaunchProperty(displayName, propertyName, propertyValue);
            case "address":
                return rtrWflSch_FormatAddressProperty(displayName, propertyName);
            case "monetaryamount":
                return rtrWflSch_FormatMonetaryAmountProperty(displayName, propertyName);
            case "duration":
                return rtrWflSch_FormatDurationProperty(displayName, propertyName);
            default:
                return rtrWflSch_FormatBasicProperty(propertyType, displayName, propertyName);
        }
    }

    private JObject rtrWflSch_FormatArrayLaunchProperty(string displayName, string propertyName, JObject propertyValue)
    {
        var elementType = propertyValue["elementType"] as JObject;
        if (elementType != null)
        {
            string elementTypeString = elementType["type"].ToString().ToLower();
            if (elementTypeString == "document")
            {
                return rtrWflSch_FormatDocumentArrayLaunchProperty(displayName, propertyName);
            }
            else if (elementTypeString == "object")
            {
                return rtrWflSch_FormatTableProperty(displayName, propertyName, propertyValue);
            }
            else
            {
                return new JObject
                {
                    ["type"] = "array",
                    ["title"] = displayName,
                    ["description"] = $"The {displayName}.",
                    ["x-ms-visibility"] = "important",
                    ["items"] = rtrWflSch_FormatBasicProperty(elementTypeString, $"{displayName} Item", $"{propertyName} Item")
                };
            }
        }
        return new JObject();
    }

    private JObject rtrWflSch_FormatDocumentArrayLaunchProperty(string displayName, string propertyName)
    {
        return new JObject
        {
            ["type"] = "array",
            ["title"] = displayName,
            ["description"] = $"The {displayName} files.",
            ["x-ms-visibility"] = "important",
            ["items"] = new JObject
            {
                ["type"] = "object",
                ["required"] = new JArray { "fileName", "fileContent" },
                ["properties"] = new JObject
                {
                    ["fileName"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "File Name",
                        ["description"] = "The name of the file.",
                        ["x-ms-visibility"] = "important"
                    },
                    ["fileContent"] = new JObject
                    {
                        ["type"] = "string",
                        ["format"] = "byte",
                        ["title"] = "File Content",
                        ["description"] = "The content of the file (base64 encoded).",
                        ["x-ms-visibility"] = "important"
                    }
                }
            }
        };
    }

    private JObject rtrWflSch_FormatTableProperty(string displayName, string propertyName, JObject propertyValue)
    {
        var elementTypeSchema = propertyValue["elementType"]["schema"] as JObject;
        var itemsObject = new JObject();

        foreach (var column in elementTypeSchema.Properties())
        {
            var columnSchema = column.Value as JObject;
            if (columnSchema != null)
            {
                string columnDisplayName = columnSchema["displayName"].ToString();
                string columnType = columnSchema["type"].ToString().ToLower();

                // Handle special types
                if (columnType == "monetaryamount")
                {
                    itemsObject[column.Name] = rtrWflSch_FormatMonetaryAmountProperty(columnDisplayName, column.Name);
                }
                else if (columnType == "duration")
                {
                    itemsObject[column.Name] = rtrWflSch_FormatDurationProperty(columnDisplayName, column.Name);
                }
                else if (columnType == "address")
                {
                    itemsObject[column.Name] = rtrWflSch_FormatAddressProperty(columnDisplayName, column.Name);
                }
                else
                {
                    JObject formattedColumn = rtrWflSch_FormatBasicProperty(columnType, columnDisplayName, column.Name);
                    itemsObject[column.Name] = formattedColumn;
                }
            }
        }

        return new JObject
        {
            ["type"] = "array",
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important",
            ["items"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = itemsObject
            }
        };
    }

    private JObject rtrWflSch_FormatAddressProperty(string displayName, string propertyName)
    {
    return new JObject
        {
            ["type"] = "object",
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important",
            ["properties"] = new JObject
            {
                ["lines"] = new JObject {
                    ["type"] = "array", 
                    ["items"] = new JObject { 
                        ["type"] = "string",
                        ["title"] = $"{displayName} Line",
                        ["x-ms-visibility"] = "important",
                        ["description"] = $"An address line of {displayName}."
                        } 
                    },
                ["locality"] = new JObject { 
                    ["type"] = "string",  
                    ["title"] = "Locality", 
                    ["x-ms-visibility"] = "important", 
                    ["description"] = $"The locality of {displayName}." 
                    },
                ["region"] = new JObject {
                    ["type"] = "string",
                    ["title"] = "Region",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The region of {displayName}."
                    },
                ["postcode"] = new JObject {
                    ["type"] = "string",
                    ["title"] = "Postcode",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The postcode of {displayName}."
                    },
                ["country"] = new JObject {
                    ["type"] = "string",
                    ["title"] = "Country",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The country of {displayName}."
                    }
            }
        };
    }

    private JObject rtrWflSch_FormatMonetaryAmountProperty(string displayName, string propertyName)
    {
        return new JObject
        {
            ["type"] = "object",
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important",
            ["properties"] = new JObject
            {
                ["amount"] = new JObject { ["type"] = "number", ["title"] = "Amount", ["x-ms-visibility"] = "important", ["description"] = $"The amount of {displayName}." },
                ["currency"] = new JObject { ["type"] = "string", ["title"] = "Currency", ["x-ms-visibility"] = "important", ["description"] = $"The currency of {displayName}." }
            }
        };
    }

    private JObject rtrWflSch_FormatDurationProperty(string displayName, string propertyName)
    {
        return new JObject
        {
            ["type"] = "object",
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important",
            ["properties"] = new JObject
            {
                ["years"] = new JObject { 
                    ["type"] = "number",
                    ["title"] = "Years",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The years of {displayName}." 
                    },
                ["months"] = new JObject { 
                    ["type"] = "number",
                    ["title"] = "Months",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The months of {displayName}." 
                    },
                ["weeks"] = new JObject { 
                    ["type"] = "number",
                    ["title"] = "Weeks",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The weeks of {displayName}." 
                    },
                ["days"] = new JObject {
                    ["type"] = "number",
                    ["title"] = "Days",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The days of {displayName}."
                    }
            }
        };
    }

    private JObject rtrWflSch_FormatBasicProperty(string propertyType, string displayName, string propertyName)
    {
        var formattedLaunchProperty = new JObject
        {
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important",
        };

        switch (propertyType)
        {
            case "string":
                formattedLaunchProperty["type"] = "string";
                break;
            case "number":
            case "integer":
                formattedLaunchProperty["type"] = "number";
                break;
            case "boolean":
                formattedLaunchProperty["type"] = "boolean";
                break;
            case "date":
                formattedLaunchProperty["type"] = "string";
                formattedLaunchProperty["format"] = "date-time";
                break;
            default:
                formattedLaunchProperty["type"] = "string";
                break;
        }

        return formattedLaunchProperty;
    }

    private JObject rtrWflSch_ParseSchemaArrayItem(string systemName, string displayName, string type, bool isReadOnly)
    {
        return new JObject
        {
            ["systemName"] = systemName,
            ["displayName"] = displayName,
            ["type"] = type,
            ["readOnly"] = isReadOnly
        };
    }

    private JObject rtrWflSch_ParseDocumentSchemaItem(string systemName, string displayName, bool isReadOnly)
    {
        return new JObject
        {
            ["systemName"] = systemName,
            ["displayName"] = displayName,
            ["readOnly"] = isReadOnly
        };
    }

    private JObject rtrWflSch_FormatProperty(string propertyType, string displayName, string propertyName, JObject propertyValue)
    {
        switch (propertyType)
        {
            case "array":
                return rtrWflSch_FormatArrayProperty(displayName, propertyName, propertyValue);
            case "address":
                return rtrWflSch_FormatAddressProperty(displayName, propertyName);
            case "monetaryamount":
                return rtrWflSch_FormatMonetaryAmountProperty(displayName, propertyName);
            case "duration":
                return rtrWflSch_FormatDurationProperty(displayName, propertyName);
            default:
                return rtrWflSch_FormatBasicProperty(propertyType, displayName, propertyName);
        }
    }

    private JObject rtrWflSch_FormatArrayProperty(string displayName, string propertyName, JObject propertyValue)
    {
        var elementType = propertyValue["elementType"] as JObject;
        if (elementType != null)
        {
            string elementTypeString = elementType["type"].ToString().ToLower();
            if (elementTypeString == "document")
            {
                return rtrWflSch_FormatDocumentArrayProperty(displayName, propertyName);
            }
            else if (elementTypeString == "object")
            {
                return rtrWflSch_FormatTableProperty(displayName, propertyName, propertyValue);
            }
            else
            {
                return new JObject
                {
                    ["type"] = "array",
                    ["title"] = displayName,
                    ["description"] = $"The {displayName}.",
                    ["x-ms-visibility"] = "important",
                    ["items"] = rtrWflSch_FormatBasicProperty(elementTypeString, $"{displayName} Item", $"{propertyName} Item")
                };
            }
        }
        return new JObject();
    }

    private JObject rtrWflSch_FormatDocumentArrayProperty(string displayName, string propertyName)
    {
        return new JObject
        {
            ["type"] = "array",
            ["title"] = displayName,
            ["description"] = $"The {displayName} files.",
            ["x-ms-visibility"] = "important",
            ["items"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["filename"] = new JObject {
                        ["type"] = "string",
                        ["title"] = "File Name",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The name of the file." 
                    },
                    ["download"] = new JObject {
                        ["type"] = "string",
                        ["title"] = "Download Link",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The download link of the file."
                    },
                    ["key"] = new JObject {
                        ["type"] = "string",
                        ["title"] = "Key",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The key of the file."
                    }
                }
            }
        };
    }

    // ################################################################################
    // List All Workflow ##############################################################
    // ################################################################################

    private JObject lstAllWfl_TransformListAllWorkflowsResponse(JObject body)
    {
        if (body.ContainsKey("list") && body["list"] is JArray list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] is JObject workflowObject)
                {
                    string ironcladId = workflowObject["ironcladId"]?.ToString() ?? "";
                    string title = workflowObject["title"]?.ToString() ?? "";
                    workflowObject["label"] = $"{ironcladId}: {title}";
                }
            }
        }

        return body;
    }

    // ################################################################################
    // Retrieve Workflow ##############################################################
    // ################################################################################
    private JObject rtrWfl_TransformRetrieveWorkflow(JObject body)
    {
        var schema = body["schema"] as JObject;
        if (schema != null)
        {
            var formattedSchema = new JObject();
            var schemaAsArray = new JArray();
            var documentSchemaAsArray = new JArray();

            foreach (var property in schema.Properties())
            {
                rtrWfl_ProcessSchemaProperty(property, formattedSchema, schemaAsArray, documentSchemaAsArray);
            }

            body["formattedSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = formattedSchema
            };
            body["schemaAsArray"] = schemaAsArray;
            body["documentSchemaAsArray"] = documentSchemaAsArray;

            body["formattedAttributes"] = rtrWfl_FormatWorkflowAttributes(body["attributes"] as JObject, formattedSchema);
        }

        return body;
    }

    private void rtrWfl_ProcessSchemaProperty(JProperty property, JObject formattedSchema, JArray schemaAsArray, JArray documentSchemaAsArray)
    {
        var propertySchema = property.Value as JObject;
        if (propertySchema != null)
        {
            string displayName = propertySchema["displayName"]?.ToString() ?? property.Name;
            string propertyType = propertySchema["type"]?.ToString().ToLower();
            bool isReadOnly = propertySchema["readOnly"]?.ToObject<bool>() ?? false;

            if (isReadOnly)
            {
                displayName += " (read only)";
            }

            JObject formattedProperty = rtrWfl_FormatPropertyByType(propertyType, displayName, property.Name, propertySchema);
            formattedProperty["readOnly"] = isReadOnly;
            formattedSchema[property.Name] = formattedProperty;

            schemaAsArray.Add(rtrWfl_CreateSchemaArrayItem(property.Name, displayName, propertyType, isReadOnly));

            if (propertyType == "array" && propertySchema["elementType"] is JObject arrayElementType && arrayElementType["type"]?.ToString().ToLower() == "document")
            {
                documentSchemaAsArray.Add(rtrWfl_CreateDocumentSchemaItem(property.Name, displayName, isReadOnly));
            }
        }
    }

    private JObject rtrWfl_FormatPropertyByType(string propertyType, string displayName, string propertyName, JObject propertySchema)
    {
        switch (propertyType)
        {
            case "array":
                return rtrWfl_FormatArrayProperty(displayName, propertyName, propertySchema);
            case "address":
                return rtrWfl_FormatAddressProperty(displayName, propertyName);
            case "monetaryamount":
                return rtrWfl_FormatMonetaryAmountProperty(displayName, propertyName);
            case "duration":
                return rtrWfl_FormatDurationProperty(displayName, propertyName);
            default:
                return rtrWfl_FormatBasicProperty(propertyType, displayName, propertyName);
        }
    }

    private JObject rtrWfl_FormatArrayProperty(string displayName, string propertyName, JObject propertySchema)
    {
        var elementType = propertySchema["elementType"] as JObject;
        if (elementType != null)
        {
            string elementTypeString = elementType["type"].ToString().ToLower();
            if (elementTypeString == "document")
            {
                return rtrWfl_FormatDocumentArrayProperty(displayName, propertyName);
            }
            else if (elementTypeString == "object")
            {
                return rtrWfl_FormatTableProperty(displayName, propertyName, propertySchema);
            }
            else
            {
                return new JObject
                {
                    ["type"] = "array",
                    ["title"] = displayName,
                    ["description"] = $"The {displayName}.",
                    ["x-ms-visibility"] = "important",
                    ["items"] = rtrWfl_FormatBasicProperty(elementTypeString, $"{displayName} Item", $"{propertyName} Item")
                };
            }
        }
        return new JObject();
    }

    private JObject rtrWfl_FormatDocumentArrayProperty(string displayName, string propertyName)
    {
        return new JObject
        {
            ["type"] = "array",
            ["title"] = displayName,
            ["description"] = $"The {displayName} files.",
            ["x-ms-visibility"] = "important",
            ["items"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["filename"] = new JObject {
                        ["type"] = "string",
                        ["title"] = "File Name",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The name of the file." 
                    },
                    ["version"] = new JObject {
                        ["type"] = "string",
                        ["title"] = "Version",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The version of the file."
                    },
                    ["versionNumber"] = new JObject {
                        ["type"] = "number",
                        ["title"] = "Version Number",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The version number of the file."
                    },
                    ["download"] = new JObject {
                        ["type"] = "string",
                        ["title"] = "Download Link",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The download link of the file."
                    },
                    ["key"] = new JObject {
                        ["type"] = "string",
                        ["title"] = "Key",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The key of the file."
                    },
                    ["lastModified"] = new JObject {
                        ["type"] = "object",
                        ["title"] = "Last Modified",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "Information on when the file was last modified.",
                        ["properties"] = new JObject {
                            ["timestamp"] = new JObject {
                                ["type"] = "string",
                                ["format"] = "date-time",
                                ["title"] = "Timestamp",
                                ["x-ms-visibility"] = "important",
                                ["description"] = "The date when the file was last modified."
                                },
                            ["author"] = new JObject {
                                ["type"] = "object",
                                ["title"] = "Author",
                                ["x-ms-visibility"] = "important",
                                ["description"] = "The author of the last modification.",
                                ["properties"] = new JObject {
                                    ["displayName"] = new JObject {
                                        ["type"] = "string",
                                        ["title"] = "Display Name",
                                        ["x-ms-visibility"] = "important",
                                        ["description"] = "The display name of the author."
                                        },
                                    ["email"] = new JObject {
                                        ["type"] = "string",
                                        ["title"] = "Email",
                                        ["x-ms-visibility"] = "important",
                                        ["description"] = "The email of the author."
                                        },
                                    ["userId"] = new JObject {
                                        ["type"] = "string",
                                        ["title"] = "User ID",
                                        ["x-ms-visibility"] = "important",
                                        ["description"] = "The user ID of the author."
                                        }
                                    ["type"] = new JObject {
                                        ["type"] = "string",
                                        ["title"] = "Type",
                                        ["x-ms-visibility"] = "important",
                                        ["description"] = "The type of the file."
                                        }
                                    ["companyName"] = new JObject {
                                        ["type"] = "string",
                                        ["title"] = "Company Name",
                                        ["x-ms-visibility"] = "important",
                                        ["description"] = "The company name of the author."
                                    }
                                    
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    private JObject rtrWfl_FormatTableProperty(string displayName, string propertyName, JObject propertySchema)
    {
        var elementTypeSchema = propertySchema["elementType"]["schema"] as JObject;
        var itemsObject = new JObject();

        foreach (var column in elementTypeSchema.Properties())
        {
            var columnSchema = column.Value as JObject;
            if (columnSchema != null)
            {
                string columnDisplayName = columnSchema["displayName"].ToString();
                string columnType = columnSchema["type"].ToString().ToLower();

                JObject formattedColumn = rtrWfl_FormatBasicProperty(columnType, columnDisplayName, column.Name);
                itemsObject[column.Name] = formattedColumn;
            }
        }

        return new JObject
        {
            ["type"] = "array",
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important",
            ["items"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = itemsObject
            }
        };
    }

    private JObject rtrWfl_FormatAddressProperty(string displayName, string propertyName)
    {
        return new JObject
        {
            ["type"] = "object",
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important",
            ["properties"] = new JObject
            {
                ["lines"] = new JObject {
                    ["type"] = "array", 
                    ["items"] = new JObject { 
                        ["type"] = "string",
                        ["title"] = $"{displayName} Line",
                        ["x-ms-visibility"] = "important",
                        ["description"] = $"An address line of {displayName}."
                        } 
                    },
                ["locality"] = new JObject { 
                    ["type"] = "string",  
                    ["title"] = "Locality", 
                    ["x-ms-visibility"] = "important", 
                    ["description"] = $"The locality of {displayName}." 
                    },
                ["region"] = new JObject {
                    ["type"] = "string",
                    ["title"] = "Region",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The region of {displayName}."
                    },
                ["postcode"] = new JObject {
                    ["type"] = "string",
                    ["title"] = "Postcode",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The postcode of {displayName}."
                    },
                ["country"] = new JObject {
                    ["type"] = "string",
                    ["title"] = "Country",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The country of {displayName}."
                    }
            }
        };
    }

    private JObject rtrWfl_FormatMonetaryAmountProperty(string displayName, string propertyName)
    {
        return new JObject
        {
            ["type"] = "object",
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important",
            ["properties"] = new JObject
            {
                ["amount"] = new JObject { 
                    ["type"] = "number", 
                    ["title"] = "Amount", 
                    ["x-ms-visibility"] = "important", 
                    ["description"] = $"The amount of {displayName}." 
                    },
                ["currency"] = new JObject { 
                    ["type"] = "string", 
                    ["title"] = "Currency", 
                    ["x-ms-visibility"] = "important", 
                    ["description"] = $"The currency of {displayName}." }
            }
        };
    }

    private JObject rtrWfl_FormatDurationProperty(string displayName, string propertyName)
    {
    return new JObject
        {
            ["type"] = "object",
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important",
            ["properties"] = new JObject
            {
                ["years"] = new JObject { 
                    ["type"] = "number",
                    ["title"] = "Years",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The years of {displayName}." 
                    },
                ["months"] = new JObject { 
                    ["type"] = "number",
                    ["title"] = "Months",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The months of {displayName}." 
                    },
                ["weeks"] = new JObject { 
                    ["type"] = "number",
                    ["title"] = "Weeks",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The weeks of {displayName}." 
                    },
                ["days"] = new JObject {
                    ["type"] = "number",
                    ["title"] = "Days",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The days of {displayName}."
                    }
            }
        };
    }

    private JObject rtrWfl_FormatBasicProperty(string propertyType, string displayName, string propertyName)
    {
        var formattedProperty = new JObject
        {
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important"
        };

        switch (propertyType)
        {
            case "string":
                formattedProperty["type"] = "string";
                break;
            case "number":
            case "integer":
                formattedProperty["type"] = "number";
                break;
            case "boolean":
                formattedProperty["type"] = "boolean";
                break;
            case "date":
                formattedProperty["type"] = "string";
                formattedProperty["format"] = "date-time";
                break;
            default:
                formattedProperty["type"] = "string";
                break;
        }

        return formattedProperty;
    }

    private JObject rtrWfl_CreateSchemaArrayItem(string systemName, string displayName, string type, bool isReadOnly)
    {
        return new JObject
        {
            ["systemName"] = systemName,
            ["displayName"] = displayName,
            ["type"] = type,
            ["readOnly"] = isReadOnly
        };
    }

    private JObject rtrWfl_CreateDocumentSchemaItem(string systemName, string displayName, bool isReadOnly)
    {
        return new JObject
        {
            ["systemName"] = systemName,
            ["displayName"] = displayName,
            ["readOnly"] = isReadOnly
        };
    }

    private JObject rtrWfl_FormatWorkflowAttributes(JObject attributes, JObject formattedSchema)
    {
        if (attributes == null || formattedSchema == null)
        {
            return new JObject();
        }

        var formattedAttributes = new JObject();

        foreach (var property in formattedSchema.Properties())
        {
            var propertySchema = property.Value as JObject;
            if (propertySchema != null && attributes.ContainsKey(property.Name))
            {
                var attributeValue = attributes[property.Name];
                formattedAttributes[property.Name] = rtrWfl_FormatAttributeValue(attributeValue, propertySchema);
            }
        }

        return formattedAttributes;
    }

    private JToken rtrWfl_FormatAttributeValue(JToken value, JObject schema)
    {
        string propertyType = schema["type"]?.ToString().ToLower();

        switch (propertyType)
        {
            case "array":
                return rtrWfl_FormatArrayAttributeValue(value, schema);
            case "object":
                return rtrWfl_FormatObjectAttributeValue(value, schema);
            default:
                return value;
        }
    }

    private JArray rtrWfl_FormatArrayAttributeValue(JToken value, JObject schema)
    {
        var formattedArray = new JArray();
        var items = value as JArray;

        if (items != null)
        {
            var elementType = schema["items"] as JObject;
            string itemType = elementType?["type"]?.ToString().ToLower();

            foreach (var item in items)
            {
                if (itemType == "object")
                {
                    formattedArray.Add(rtrWfl_FormatObjectAttributeValue(item, elementType));
                }
                else
                {
                    formattedArray.Add(item);
                }
            }
        }

        return formattedArray;
    }

    private JObject rtrWfl_FormatObjectAttributeValue(JToken value, JObject schema)
    {
        var formattedObject = new JObject();
        var objectValue = value as JObject;

        if (objectValue != null && schema["properties"] is JObject propertiesSchema)
        {
            foreach (var property in propertiesSchema.Properties())
            {
                if (objectValue.ContainsKey(property.Name))
                {
                    formattedObject[property.Name] = rtrWfl_FormatAttributeValue(objectValue[property.Name], property.Value as JObject);
                }
            }
        }

        return formattedObject;
    }


    // ################################################################################
    // Retrieve Record Schema #########################################################
    // ################################################################################

    private async Task<HttpResponseMessage> rtrRcdSch_HandleRequest()
    {
        try 
        {
            // Get the query parameter
            var uri = this.Context.Request.RequestUri;
            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            var propertiesQuery = queryParams["recordPorperties"] ?? string.Empty;

            // Get the content
            var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var metadata = JObject.Parse(content);

            // If we have a query, validate all properties exist
            if (!string.IsNullOrWhiteSpace(propertiesQuery))
            {
                var requestedItems = propertiesQuery.Split(',').Select(p => p.Trim()).ToList();
                var properties = metadata["properties"] as JObject;
                var attachments = metadata["attachments"] as JObject;

                foreach (var item in requestedItems)
                {
                    bool exists = false;
                    
                    // Check in properties (including clauses)
                    if (properties != null && properties.ContainsKey(item))
                    {
                        var propObj = properties[item] as JObject;
                        if (propObj != null)
                        {
                            exists = propObj["type"] != null && 
                                    (propObj["resolvesTo"] == null || propObj["resolvesTo"].Type == JTokenType.Null);
                        }
                    }
                    // Check in attachments
                    else if (attachments != null && attachments.ContainsKey(item))
                    {
                        var attachmentObj = attachments[item] as JObject;
                        exists = attachmentObj != null;
                    }

                    if (!exists)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = CreateJsonContent(new JObject
                            {
                                ["error"] = new JObject
                                {
                                    ["message"] = $"Property '{item}' not found"
                                }
                            }.ToString())
                        };
                    }
                }
            }
                
            // Transform the metadata
            var transformedData = rtrRcdSch_TransformRetrieveRecordSchemas(metadata, propertiesQuery);
                
            // Create response
            return new HttpResponseMessage(HttpStatusCode.OK) 
            {
                Content = CreateJsonContent(transformedData.ToString())
            };
        }
        catch (Exception ex)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent(new JObject
                {
                    ["error"] = new JObject
                    {
                        ["message"] = ex.Message
                    }
                }.ToString())
            };
        }
    }
    private JObject rtrRcdSch_TransformRetrieveRecordSchemas(JObject body, string propertiesQuery)
    {
        var properties = body["properties"] as JObject;
        if (properties != null)
        {
            var formattedProperties = new JArray();
            var formattedClauses = new JArray();

            // Add the requested properties to the response
            body["requestedProperties"] = propertiesQuery;

            // Get requested items if filtering is needed
            var requestedItems = !string.IsNullOrWhiteSpace(propertiesQuery) 
                ? propertiesQuery.Split(',').Select(p => p.Trim()).ToList() 
                : new List<string>();

            foreach (var prop in properties.Properties())
            {
                var propObj = prop.Value as JObject;
                if (propObj != null && (propObj["resolvesTo"] == null || propObj["resolvesTo"].Type == JTokenType.Null))
                {
                    bool isVisible = propObj["visible"]?.ToObject<bool>() ?? true;
                    
                    if (isVisible)
                    {
                        string originalType = propObj["type"]?.ToString().ToLower() ?? "unknown";
                        string effectiveType = originalType == "address" ? "string" : originalType;
                        string displayName = propObj["displayName"]?.ToString() ?? prop.Name;
                        
                        var newObj = new JObject
                        {
                            ["systemName"] = prop.Name,
                            ["type"] = effectiveType,
                            ["displayName"] = displayName,
                            ["description"] = $"The {displayName}.",
                            ["ironcladType"] = propObj["type"]
                        };

                        string formattedType = effectiveType.Replace("_", " ");
                        newObj["label"] = $"{displayName} ({formattedType})";
                        newObj["typedPropertyName"] = $"{prop.Name}?{effectiveType}";

                        foreach (var subProp in propObj.Properties())
                        {
                            if (!newObj.ContainsKey(subProp.Name))
                            {
                                newObj[subProp.Name] = subProp.Value;
                            }
                        }

                        // Only add if no filter or if property is in filter
                        if (!requestedItems.Any() || requestedItems.Contains(prop.Name))
                        {
                            if (effectiveType == "clause")
                            {
                                formattedClauses.Add(newObj);
                            }
                            else if (effectiveType != "document")
                            {
                                formattedProperties.Add(newObj);
                            }
                        }
                    }
                }
            }

            // Handle record types
            var recordTypes = body["recordTypes"] as JObject;
            if (recordTypes != null)
            {
                var formattedRecordTypes = new JArray();
                foreach (var rt in recordTypes.Properties())
                {
                    var rtObj = rt.Value as JObject;
                    if (rtObj != null)
                    {
                        var displayName = rtObj["displayName"]?.ToString() ?? rt.Name;
                        var newObj = new JObject(rtObj)
                        {
                            ["systemName"] = rt.Name,
                            ["displayName"] = displayName,
                            ["description"] = $"The {displayName} record type."
                        };
                        formattedRecordTypes.Add(newObj);
                    }
                }
                body["formattedRecordTypes"] = formattedRecordTypes;
            }

            // Handle attachments
            var attachments = body["attachments"] as JObject;
            var formattedAttachments = new JArray();
            if (attachments != null)
            {
                foreach (var attachment in attachments.Properties())
                {
                    var attachmentObj = attachment.Value as JObject;
                    if (attachmentObj != null)
                    {
                        var displayName = attachmentObj["displayName"]?.ToString() ?? attachment.Name;
                        var newObj = new JObject
                        {
                            ["systemName"] = attachment.Name,
                            ["displayName"] = displayName,
                            ["description"] = $"The {displayName} attachment."
                        };
                        if (!requestedItems.Any() || requestedItems.Contains(attachment.Name))
                        {
                            formattedAttachments.Add(newObj);
                        }
                    }
                }
            }

            body["formattedProperties"] = formattedProperties;
            body["formattedClauses"] = formattedClauses;
            body["formattedAttachments"] = formattedAttachments;

            // If properties were requested, create the formatted schema
            if (!string.IsNullOrWhiteSpace(propertiesQuery))
            {
                var propertiesSchema = new JObject();
                
                foreach (var prop in formattedProperties)
                {
                    var propertyObj = prop as JObject;
                    var propertyName = propertyObj["systemName"].ToString();
                    var propertyType = propertyObj["type"].ToString().ToLower();
                    var displayName = propertyObj["displayName"].ToString();
                    var description = propertyObj["description"]?.ToString() ?? $"The {displayName}.";

                    JObject schemaProperty;

                    switch (propertyType)
                    {
                        case "monetary_amount":
                            schemaProperty = new JObject
                            {
                                ["type"] = "object",
                                ["title"] = displayName,
                                ["description"] = description,
                                ["x-ms-visibility"] = "important",
                                ["properties"] = new JObject
                                {
                                    ["amount"] = new JObject
                                    {
                                        ["type"] = "number",
                                        ["title"] = "Amount",
                                        ["description"] = $"The monetary amount value for {displayName}."
                                    },
                                    ["currency"] = new JObject
                                    {
                                        ["type"] = "string",
                                        ["title"] = "Currency",
                                        ["description"] = $"The currency code for {displayName}."
                                    }
                                }
                            };
                            break;
                        case "duration":
                            schemaProperty = new JObject
                            {
                                ["type"] = "object",
                                ["title"] = displayName,
                                ["description"] = description,
                                ["x-ms-visibility"] = "important",
                                ["properties"] = new JObject
                                {
                                    ["isoDuration"] = new JObject
                                    {
                                        ["type"] = "string",
                                        ["title"] = "ISO Duration",
                                        ["description"] = $"The ISO 8601 duration representation for {displayName}."
                                    },
                                    ["years"] = new JObject
                                    {
                                        ["type"] = "number",
                                        ["title"] = "Years",
                                        ["description"] = $"The number of years in {displayName}."
                                    },
                                    ["months"] = new JObject
                                    {
                                        ["type"] = "number",
                                        ["title"] = "Months",
                                        ["description"] = $"The number of months in {displayName}."
                                    },
                                    ["weeks"] = new JObject
                                    {
                                        ["type"] = "number",
                                        ["title"] = "Weeks",
                                        ["description"] = $"The number of weeks in {displayName}."
                                    },
                                    ["days"] = new JObject
                                    {
                                        ["type"] = "number",
                                        ["title"] = "Days",
                                        ["description"] = $"The number of days in {displayName}."
                                    }
                                }
                            };
                            break;
                        case "boolean":
                            schemaProperty = new JObject
                            {
                                ["type"] = "boolean",
                                ["title"] = displayName,
                                ["description"] = description,
                                ["x-ms-visibility"] = "important"
                            };
                            break;
                        case "number":
                        case "integer":
                            schemaProperty = new JObject
                            {
                                ["type"] = "number",
                                ["title"] = displayName,
                                ["description"] = description,
                                ["x-ms-visibility"] = "important"
                            };
                            break;
                        case "date":
                            schemaProperty = new JObject
                            {
                                ["type"] = "string",
                                ["format"] = "date-time",
                                ["title"] = displayName,
                                ["description"] = description,
                                ["x-ms-visibility"] = "important"
                            };
                            break;
                        default:
                            schemaProperty = new JObject
                            {
                                ["type"] = "string",
                                ["title"] = displayName,
                                ["description"] = description,
                                ["x-ms-visibility"] = "important"
                            };
                            break;
                    }

                    propertiesSchema[propertyName] = schemaProperty;
                }

                body["formattedSchema"] = new JObject
                {
                    ["type"] = "object",
                    ["description"] = "The record schema formatted for compatibility with the OpenAPI standard.",
                    ["x-ms-visibility"] = "important",
                    ["properties"] = new JObject
                    {
                        ["recordProperties"] = new JObject
                        {
                            ["type"] = "object",
                            ["title"] = "Properties",
                            ["description"] = "The properties of the record.",
                            ["x-ms-visibility"] = "important",
                            ["properties"] = propertiesSchema
                        },
                        ["recordClauses"] = rtrRcdSch_FormatRecordClausesSchema(formattedClauses),
                        ["recordAttachments"] = rtrRcdSch_CreateAttachmentSchema(formattedAttachments)
                    }
                };
            }
            else
            {
                body["formattedSchema"] = new JObject
                {
                    ["type"] = "object",
                    ["description"] = "The record schema formatted for compatibility with the OpenAPI standard.",
                    ["x-ms-visibility"] = "important",
                    ["properties"] = new JObject()
                };
            }
        }

        return body;
    }

    private JObject rtrRcdSch_FormatRecordClausesSchema(JArray clauses)
    {
        var clausesSchema = new JObject();
        foreach (var clause in clauses)
        {
            var clauseObj = clause as JObject;
            var clauseName = clauseObj["systemName"].ToString();
            var displayName = clauseObj["displayName"].ToString();
            
            clausesSchema[clauseName] = new JObject
            {
                ["type"] = "object",
                ["title"] = displayName,
                ["description"] = $"The {displayName} clause.",
                ["x-ms-visibility"] = "important",
                ["properties"] = new JObject
                {
                    ["displayName"] = new JObject { 
                        ["type"] = "string", 
                        ["title"] = "Display Name",
                        ["description"] = $"The display name of the {displayName} clause."
                    },
                    ["description"] = new JObject { 
                        ["type"] = "string", 
                        ["title"] = "Description",
                        ["description"] = $"The description of the {displayName} clause."
                    },
                    ["clauseText"] = new JObject { 
                        ["type"] = "string", 
                        ["title"] = "Clause Text",
                        ["description"] = $"The text content of the {displayName} clause."
                    },
                    ["source"] = new JObject { 
                        ["type"] = "string", 
                        ["title"] = "Source",
                        ["description"] = $"The source of the {displayName} clause."
                    },
                    ["clauseType"] = new JObject { 
                        ["type"] = "string", 
                        ["title"] = "Clause Type",
                        ["description"] = $"The type of the {displayName} clause."
                    },
                    ["languagePosition"] = new JObject
                    {
                        ["type"] = "object",
                        ["properties"] = new JObject
                        {
                            ["type"] = new JObject { 
                                ["type"] = "string", 
                                ["title"] = "Type",
                                ["description"] = $"The language position type of the {displayName} clause."
                            }
                        }
                    }
                }
            };
        }

        return new JObject
        {
            ["type"] = "object",
            ["title"] = "Clauses",
            ["description"] = "The clauses of the record.",
            ["x-ms-visibility"] = "important",
            ["properties"] = clausesSchema
        };
    }

    private JObject rtrRcdSch_CreateAttachmentSchema(JArray attachments)
    {
        var attachmentsSchema = new JObject();
        foreach (var attachment in attachments)
        {
            var attachmentObj = attachment as JObject;
            var attachmentName = attachmentObj["systemName"].ToString();
            var displayName = attachmentObj["displayName"].ToString();

            attachmentsSchema[attachmentName] = new JObject
            {
                ["type"] = "object",
                ["title"] = displayName,
                ["description"] = $"The {displayName} attachment.",
                ["x-ms-visibility"] = "important",
                ["properties"] = new JObject
                {
                    ["filename"] = new JObject { 
                        ["type"] = "string", 
                        ["title"] = "Filename",
                        ["description"] = $"The filename of the {displayName} attachment."
                    },
                    ["contentType"] = new JObject { 
                        ["type"] = "string", 
                        ["title"] = "Content Type",
                        ["description"] = $"The content type of the {displayName} attachment."
                    },
                    ["href"] = new JObject { 
                        ["type"] = "string", 
                        ["title"] = "Download URL",
                        ["description"] = $"The download URL for the {displayName} attachment."
                    },
                    ["key"] = new JObject { 
                        ["type"] = "string", 
                        ["title"] = "Key",
                        ["description"] = $"The unique key identifier for the {displayName} attachment."
                    }
                }
            };
        }

        return new JObject
        {
            ["type"] = "object",
            ["title"] = "Attachments",
            ["description"] = "The attachments associated with the record.",
            ["x-ms-visibility"] = "important",
            ["properties"] = attachmentsSchema
        };
    }

    // ################################################################################
    // Retrieve All Records ###########################################################
    // ################################################################################
   
   private async Task<HttpResponseMessage> lstAllRcd_HandleRequest()
    {
        try 
        {
            // Get the query parameter
            var uri = this.Context.Request.RequestUri;
            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            var propertiesQuery = queryParams["recordPorperties"] ?? string.Empty;

            // Get the content
            var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var metadata = JObject.Parse(content);

            // If we have a query, validate all properties exist
            if (!string.IsNullOrWhiteSpace(propertiesQuery))
            {
                var requestedItems = propertiesQuery.Split(',').Select(p => p.Trim()).ToList();
                var properties = metadata["properties"] as JObject;
                var attachments = metadata["attachments"] as JObject;

                foreach (var item in requestedItems)
                {
                    bool exists = false;
                    
                    // Check in properties (including clauses)
                    if (properties != null && properties.ContainsKey(item))
                    {
                        var propObj = properties[item] as JObject;
                        if (propObj != null)
                        {
                            exists = propObj["type"] != null && 
                                    (propObj["resolvesTo"] == null || propObj["resolvesTo"].Type == JTokenType.Null);
                        }
                    }
                    // Check in attachments
                    else if (attachments != null && attachments.ContainsKey(item))
                    {
                        var attachmentObj = attachments[item] as JObject;
                        exists = attachmentObj != null;
                    }

                    if (!exists)
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = CreateJsonContent(new JObject
                            {
                                ["error"] = new JObject
                                {
                                    ["message"] = $"Property '{item}' not found"
                                }
                            }.ToString())
                        };
                    }
                }
            }

            var responseBody = metadata;
            var transformedBody = lstAllRcd_TransformListAllRecordsResponse(responseBody, propertiesQuery);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = CreateJsonContent(transformedBody.ToString())
            };
        }
        catch (Exception ex)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent(new JObject
                {
                    ["error"] = new JObject
                    {
                        ["message"] = ex.Message
                    }
                }.ToString())
            };
        }
    }
   private JObject lstAllRcd_TransformListAllRecordsResponse(JObject body, string propertiesQuery)
    {
        // Add the properties query to the response
        body["requestedProperties"] = propertiesQuery;

        var requestedProperties = !string.IsNullOrWhiteSpace(propertiesQuery)
            ? propertiesQuery.Split(',').Select(p => p.Trim()).ToList()
            : new List<string>();

        if (body.ContainsKey("list") && body["list"] is JArray list)
        {
            foreach (var record in list.Children<JObject>())
            {
                // Do not transform the properties property - keep it as is
                var originalProperties = record["properties"] as JObject;

                // Handle counterpartyName at root level if it exists
                if (originalProperties != null && 
                    originalProperties.ContainsKey("counterpartyName") && 
                    originalProperties["counterpartyName"] is JObject counterpartyProp && 
                    counterpartyProp["value"] != null)
                {
                    record["counterpartyName"] = counterpartyProp["value"];
                }

                // Add label
                string ironcladId = record["ironcladId"]?.ToString() ?? "";
                string name = record["name"]?.ToString() ?? "";
                record["label"] = $"{ironcladId}: {name}";

                // Format attachments array
                var recordAttachmentsObj = record["attachments"] as JObject;
                if (recordAttachmentsObj != null)
                {
                    var attachmentsArray = new JArray();
                    foreach (var attachment in recordAttachmentsObj)
                    {
                        attachmentsArray.Add(new JObject
                        {
                            ["displayName"] = attachment.Value["displayName"] ?? attachment.Key,
                            ["name"] = attachment.Key,
                            ["key"] = attachment.Key    // Add the key property
                        });
                    }
                    record["formattedAttachments"] = attachmentsArray;
                }

                // Handle formatted properties if filtering is requested
                if (requestedProperties.Any())
                {
                    var formattedProperties = new JObject();
                    
                    // Initialize all containers
                    var recordProperties = new JObject();
                    var recordClauses = new JObject();
                    var recordAttachments = new JObject();

                    foreach (var propertyName in requestedProperties)
                    {
                        if (originalProperties != null && originalProperties.ContainsKey(propertyName))
                        {
                            var property = originalProperties[propertyName] as JObject;
                            if (property != null)
                            {
                                var propertyType = property["type"]?.ToString().ToLower();
                                
                                if (propertyType == "clause")
                                {
                                    // Handle clause properties
                                    recordClauses[propertyName] = lstAllRcd_FormatClauseProperty(propertyName, property);
                                }
                                else
                                {
                                    // Handle regular properties
                                    recordProperties[propertyName] = lstAllRcd_FormatPropertyValue(property);
                                }
                            }
                        }

                        // Handle attachments separately
                        if (recordAttachmentsObj != null && recordAttachmentsObj.ContainsKey(propertyName))
                        {
                            var attachment = recordAttachmentsObj[propertyName] as JObject;
                            recordAttachments[propertyName] = new JObject
                            {
                                ["filename"] = attachment["filename"],
                                ["contentType"] = attachment["contentType"],
                                ["href"] = attachment["href"],
                                ["displayName"] = attachment["displayName"] ?? propertyName,
                                ["key"] = propertyName  // Add the key property
                            };
                        }
                    }

                    // Always include all three properties in formattedProperties
                    formattedProperties["recordProperties"] = recordProperties;
                    formattedProperties["recordClauses"] = recordClauses;
                    formattedProperties["recordAttachments"] = recordAttachments;

                    record["formattedProperties"] = formattedProperties;
                }
            }
        }

        return body;
    }

    private JToken lstAllRcd_FormatPropertyValue(JObject property)
    {
        if (property == null || !property.ContainsKey("type") || !property.ContainsKey("value"))
            return null;

        string propertyType = property["type"].ToString().ToLower();
        var value = property["value"];

        switch (propertyType)
        {
            case "duration":
                return lstAllRcd_FormatDurationValue(value.ToString());
            case "monetary_amount":
                return value as JObject ?? new JObject();
            // All other types (including address) are treated as simple values
            default:
                return value;
        }
    }

    private JObject lstAllRcd_FormatDurationValue(string isoDuration)
    {
        var result = new JObject
        {
            ["isoDuration"] = isoDuration
        };

        var regex = new Regex(@"P(?:(\d+)Y)?(?:(\d+)M)?(?:(\d+)W)?(?:(\d+)D)?");
        var match = regex.Match(isoDuration);

        if (match.Success)
        {
            result["years"] = lstAllRcd_ParseDurationComponent(match.Groups[1].Value);
            result["months"] = lstAllRcd_ParseDurationComponent(match.Groups[2].Value);
            result["weeks"] = lstAllRcd_ParseDurationComponent(match.Groups[3].Value);
            result["days"] = lstAllRcd_ParseDurationComponent(match.Groups[4].Value);
        }

        return result;
    }

    private int lstAllRcd_ParseDurationComponent(string value)
    {
        return string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    private JObject lstAllRcd_FormatClauseProperty(string propertyName, JObject clause)
    {
        var clauseValue = clause["value"] as JObject;
        if (clauseValue == null)
            return new JObject();

        var displayName = propertyName;
        if (!displayName.EndsWith(" Clause", StringComparison.OrdinalIgnoreCase))
        {
            displayName += " Clause";
        }

        return new JObject
        {
            ["displayName"] = displayName,
            ["description"] = $"The {propertyName} clause.",
            ["clauseText"] = clauseValue["clauseText"],
            ["source"] = clauseValue["source"],
            ["clauseType"] = clauseValue["clauseType"],
            ["languagePosition"] = clauseValue["languagePosition"]
        };
    }

    // ################################################################################
    // Retrieve Record ################################################################
    // ################################################################################

    private JObject rtrRcd_TransformRetrieveRecord(JObject body)
    {
        var properties = body["properties"] as JObject;
        var attachments = body["attachments"] as JObject;

        var formattedSchema = new JObject();
        var formattedProperties = new JObject();

        if (properties != null)
        {
            formattedSchema["recordProperties"] = rtrRcd_FormatRecordPropertiesSchema(properties);
            formattedSchema["recordClauses"] = rtrRcd_FormatRecordClausesSchema(properties);
            formattedProperties["recordProperties"] = rtrRcd_FormatRecordProperties(properties);
            formattedProperties["recordClauses"] = rtrRcd_FormatRecordClauses(properties);
        }

        if (attachments != null)
        {
            formattedSchema["recordAttachments"] = rtrRcd_CreateAttachmentSchema(attachments);
            formattedProperties["recordAttachments"] = rtrRcd_FormatAttachments(attachments);
            body["attachmentsAsArray"] = rtrRcd_CreateAttachmentsArray(attachments);
        }

        body["formattedSchema"] = new JObject
        {
            ["type"] = "object",
            ["description"] = "The record schema formatted for compatibility with the OpenAPI standard.",
            ["x-ms-visibility"] = "important",
            ["properties"] = formattedSchema
        };
        body["formattedProperties"] = formattedProperties;

        return body;
    }

    private JObject rtrRcd_FormatRecordPropertiesSchema(JObject properties)
    {
        var recordPropertiesSchema = new JObject();

        foreach (var property in properties.Properties())
        {
            if (!rtrRcd_IsClauseProperty(property))
            {
                rtrRcd_ParseRecordSchemaProperty(property, recordPropertiesSchema);
            }
        }

        return new JObject
        {
            ["title"] = "Properties",
            ["type"] = "object",
            ["description"] = "The properties of the record.",
            ["x-ms-visibility"] = "important",
            ["properties"] = recordPropertiesSchema
        };
    }

    private JObject rtrRcd_FormatRecordClausesSchema(JObject properties)
    {
        var recordClausesSchema = new JObject();

        foreach (var property in properties.Properties())
        {
            if (rtrRcd_IsClauseProperty(property))
            {
                var clauseObject = rtrRcd_FormatClauseProperty(property);
                recordClausesSchema[property.Name] = rtrRcd_CreateClauseSchema(clauseObject["displayName"].ToString(), clauseObject["description"].ToString());
            }
        }

        return new JObject
        {
            ["title"] = "Clauses",
            ["type"] = "object",
            ["description"] = "The clauses of the record.",
            ["x-ms-visibility"] = "important",
            ["properties"] = recordClausesSchema
        };
    }

    private JObject rtrRcd_FormatRecordProperties(JObject properties)
    {
        var transformedProperties = new JObject();

        foreach (var property in properties.Properties())
        {
            if (!rtrRcd_IsClauseProperty(property))
            {
                var propertySchema = rtrRcd_GetRecordSchemaProperty(property.Name);
                transformedProperties[property.Name] = rtrRcd_FormatRecordPropertyValue(property.Value as JObject, propertySchema);
            }
        }

        return transformedProperties;
    }

    private JObject rtrRcd_FormatRecordClauses(JObject properties)
    {
        var transformedClauses = new JObject();

        foreach (var property in properties.Properties())
        {
            if (rtrRcd_IsClauseProperty(property))
            {
                transformedClauses[property.Name] = rtrRcd_FormatClauseProperty(property);
            }
        }

        return transformedClauses;
    }

    private bool rtrRcd_IsClauseProperty(JProperty property)
    {
        var propertyValue = property.Value as JObject;
        return propertyValue != null && propertyValue["type"]?.ToString().ToLower() == "clause";
    }

    private void rtrRcd_ParseRecordSchemaProperty(JProperty property, JObject formattedSchema)
    {
        var propertyValue = property.Value as JObject;
        if (propertyValue != null && propertyValue["type"] != null)
        {
            string propertyName = property.Name;
            string propertyType = propertyValue["type"].ToString().ToLower();

            var schemaProperty = rtrRcd_GetRecordSchemaProperty(propertyName);
            string displayName = schemaProperty?["displayName"]?.ToString() ?? propertyName;
            string description = schemaProperty?["description"]?.ToString() ?? $"The {propertyName}.";

            JObject formattedProperty = rtrRcd_ParseRecordPropertySchemaByType(propertyType, displayName, propertyName, description);
            formattedSchema[propertyName] = formattedProperty;
        }
    }

    private JObject rtrRcd_ParseRecordPropertySchemaByType(string propertyType, string displayName, string propertyName, string description)
    {
        switch (propertyType)
        {
            case "monetary_amount":
                return rtrRcd_FormatMonetaryAmountPropertySchema(displayName, propertyName);
            case "duration":
                return rtrRcd_FormatDurationPropertySchema(displayName, propertyName, description);
            // Address and all other types are treated as basic properties
            default:
                return rtrRcd_FormatBasicPropertySchema(propertyType, displayName, propertyName);
        }
    }

    private JObject rtrRcd_FormatAddressPropertySchema(string displayName, string propertyName)
    {
        return new JObject
        {
            ["type"] = "object",
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important",
            ["properties"] = new JObject
            {
                ["lines"] = new JObject
                {
                    ["type"] = "array",
                    ["items"] = new JObject { ["type"] = "string" },
                    ["title"] = "Address Lines",
                    ["description"] = "The lines of the address."
                },
                ["locality"] = new JObject { ["type"] = "string", ["title"] = "Locality" },
                ["region"] = new JObject { ["type"] = "string", ["title"] = "Region" },
                ["postcode"] = new JObject { ["type"] = "string", ["title"] = "Postcode" },
                ["country"] = new JObject { ["type"] = "string", ["title"] = "Country" }
            }
        };
    }

    private JObject rtrRcd_FormatMonetaryAmountPropertySchema(string displayName, string propertyName)
    {
        return new JObject
        {
            ["type"] = "object",
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important",
            ["properties"] = new JObject
            {
                ["amount"] = new JObject { ["type"] = "number", ["title"] = "Amount", ["description"] = $"The amount of the {displayName}." },
                ["currency"] = new JObject { ["type"] = "string", ["title"] = "Currency", ["description"] = $"The currency of the {displayName}." }
            }
        };
    }

    private JObject rtrRcd_FormatDurationPropertySchema(string displayName, string propertyName, string description)
    {
        return new JObject
        {
            ["type"] = "object",
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important",
            ["properties"] = new JObject
            {
                ["isoDuration"] = new JObject { ["type"] = "string", ["title"] = "ISO Duration", ["description"] = $"The ISO 8601 duration representation of the {displayName}." },
                ["years"] = new JObject { ["type"] = "number", ["title"] = "Years", ["description"] = $"The years of the {displayName}." },
                ["months"] = new JObject { ["type"] = "number", ["title"] = "Months", ["description"] = $"The months of the {displayName}." },
                ["weeks"] = new JObject { ["type"] = "number", ["title"] = "Weeks", ["description"] = $"The weeks of the {displayName}." },
                ["days"] = new JObject { ["type"] = "number", ["title"] = "Days", ["description"] = $"The days of the {displayName}." }
            }
        };
    }

    private JObject rtrRcd_FormatBasicPropertySchema(string propertyType, string displayName, string propertyName)
    {
        var formattedProperty = new JObject
        {
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important"
        };

        switch (propertyType)
        {
            case "string":
            case "address": // Address is treated as string
                formattedProperty["type"] = "string";
                break;
            case "number":
            case "integer":
                formattedProperty["type"] = "number";
                break;
            case "boolean":
                formattedProperty["type"] = "boolean";
                break;
            case "date":
                formattedProperty["type"] = "string";
                formattedProperty["format"] = "date-time";
                break;
            default:
                formattedProperty["type"] = "string";
                break;
        }

        return formattedProperty;
    }

    private JObject rtrRcd_GetRecordSchemaProperty(string propertyName)
    {
        if (recordSchemaInfo == null || !recordSchemaInfo.ContainsKey("properties")) 
            return null;

        var properties = recordSchemaInfo["properties"] as JObject;
        if (properties == null || !properties.ContainsKey(propertyName))
            return null;

        var propertyInfo = properties[propertyName] as JObject;
        if (propertyInfo == null)
            return null;

        return new JObject
        {
            ["displayName"] = propertyInfo["displayName"] ?? propertyName,
            ["description"] = propertyInfo["description"] ?? $"The {propertyName}."
        };
    }

    private JToken rtrRcd_FormatRecordPropertyValue(JObject propertyValue, JObject propertySchema)
    {
        if (propertyValue == null || !propertyValue.ContainsKey("type") || !propertyValue.ContainsKey("value"))
        {
            return null;
        }

        string propertyType = propertyValue["type"].ToString().ToLower();
        var value = propertyValue["value"];

        switch (propertyType)
        {
            case "monetary_amount":
                return rtrRcd_FormatMonetaryAmountPropertyValue(value as JObject);
            case "duration":
                return rtrRcd_FormatDurationPropertyValue(value.ToString());
            default:
                return value;
        }
    }

    private JObject rtrRcd_FormatMonetaryAmountPropertyValue(JObject monetaryAmount)
    {
        if (monetaryAmount == null)
        {
            return new JObject();
        }

        return new JObject
        {
            ["amount"] = monetaryAmount["amount"],
            ["currency"] = monetaryAmount["currency"]
        };
    }

    private JObject rtrRcd_FormatDurationPropertyValue(string isoDuration)
    {
        var result = new JObject
        {
            ["isoDuration"] = isoDuration
        };

        var regex = new Regex(@"P(?:(\d+)Y)?(?:(\d+)M)?(?:(\d+)W)?(?:(\d+)D)?");
        var match = regex.Match(isoDuration);

        if (match.Success)
        {
            result["years"] = rtrRcd_ParseDurationComponent(match.Groups[1].Value);
            result["months"] = rtrRcd_ParseDurationComponent(match.Groups[2].Value);
            result["weeks"] = rtrRcd_ParseDurationComponent(match.Groups[3].Value);
            result["days"] = rtrRcd_ParseDurationComponent(match.Groups[4].Value);
        }

        return result;
    }

    private int rtrRcd_ParseDurationComponent(string value)
    {
        return string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    private JObject rtrRcd_FormatClauseProperty(JProperty property)
    {
        var clauseValue = (property.Value as JObject)?["value"] as JObject;
        if (clauseValue == null)
        {
            return new JObject();
        }

        var schemaProperty = rtrRcd_GetRecordSchemaProperty(property.Name);
        var displayName = schemaProperty?["displayName"]?.ToString() ?? property.Name;
        
        if (!displayName.EndsWith(" Clause", StringComparison.OrdinalIgnoreCase))
        {
            displayName += " Clause";
        }

        return new JObject
        {
            ["displayName"] = displayName,
            ["description"] = schemaProperty?["description"]?.ToString() ?? $"The {property.Name} clause.",
            ["clauseText"] = clauseValue["clauseText"],
            ["source"] = clauseValue["source"],
            ["clauseType"] = clauseValue["clauseType"],
            ["languagePosition"] = clauseValue["languagePosition"]
        };
    }

    private JObject rtrRcd_CreateClauseSchema(string displayName, string description)
    {
        return new JObject
        {
            ["type"] = "object",
            ["title"] = displayName,
            ["description"] = description,
            ["x-ms-visibility"] = "important",
            ["properties"] = new JObject
            {
                ["displayName"] = new JObject { ["type"] = "string", ["title"] = "Display Name", ["description"] = "The display name of the clause." },
                ["description"] = new JObject { ["type"] = "string", ["title"] = "Description", ["description"] = "The description of the clause." },
                ["clauseText"] = new JObject { ["type"] = "string", ["title"] = "Clause Text", ["description"] = "The text content of the clause." },
                ["source"] = new JObject { ["type"] = "string", ["title"] = "Source", ["description"] = "The source of the clause.", ["x-ms-visibility"] = "internal" },
                ["clauseType"] = new JObject { ["type"] = "string", ["title"] = "Clause Type", ["description"] = "The type of the clause.", ["x-ms-visibility"] = "internal" },
                ["languagePosition"] = new JObject
                {
                    ["type"] = "object",
                    ["title"] = "Language Position",
                    ["description"] = "The language position of the clause.",
                    ["x-ms-visibility"] = "internal",
                    ["properties"] = new JObject
                    {
                        ["type"] = new JObject { ["type"] = "string", ["title"] = "Type", ["description"] = "The type of language position." }
                    }
                }
            }
        };
    }

    private JObject rtrRcd_CreateAttachmentSchema(JObject attachments)
    {
        var attachmentProperties = new JObject();

        if (recordSchemaInfo != null && recordSchemaInfo["attachments"] is JObject attachmentSchemas)
        {
            foreach (var attachment in attachments.Properties())
            {
                var attachmentName = attachment.Name;
                var attachmentSchema = attachmentSchemas[attachmentName] as JObject;
                var displayName = attachmentSchema?["displayName"]?.ToString() ?? attachmentName;
                var description = attachmentSchema?["description"]?.ToString() ?? $"The {displayName} attachment.";

                attachmentProperties[attachmentName] = new JObject
                {
                    ["type"] = "object",
                    ["title"] = displayName,
                    ["description"] = description,
                    ["x-ms-visibility"] = "important",
                    ["properties"] = new JObject
                    {
                        ["filename"] = new JObject
                        {
                            ["type"] = "string",
                            ["title"] = "Filename",
                            ["description"] = $"The filename of the {displayName}."
                        },
                        ["contentType"] = new JObject
                        {
                            ["type"] = "string",
                            ["title"] = "Content Type",
                            ["description"] = $"The content type of the {displayName}."
                        },
                        ["href"] = new JObject
                        {
                            ["type"] = "string",
                            ["title"] = "Download URL",
                            ["description"] = $"The download URL for the {displayName}."
                        },
                        ["displayName"] = new JObject
                        {
                            ["type"] = "string",
                            ["title"] = "Display Name",
                            ["description"] = $"The display name of the {displayName}."
                        },
                        ["key"] = new JObject
                        {
                            ["type"] = "string",
                            ["title"] = "Key",
                            ["description"] = $"The key of the {displayName}."
                        }
                    }
                };
            }
        }

        return new JObject
        {
            ["title"] = "Attachments",
            ["type"] = "object",
            ["description"] = "The attachments associated with the record.",
            ["x-ms-visibility"] = "important",
            ["properties"] = attachmentProperties
        };
    }

    private JObject rtrRcd_FormatAttachments(JObject attachments)
    {
        var formattedAttachments = new JObject();

        if (recordSchemaInfo != null && recordSchemaInfo["attachments"] is JObject attachmentSchemas)
        {
            foreach (var attachment in attachments.Properties())
            {
                var attachmentName = attachment.Name;
                var attachmentValue = attachment.Value as JObject;
                var attachmentSchema = attachmentSchemas[attachmentName] as JObject;
                var displayName = attachmentSchema?["displayName"]?.ToString() ?? attachmentName;

                formattedAttachments[attachmentName] = new JObject
                {
                    ["filename"] = attachmentValue?["filename"],
                    ["contentType"] = attachmentValue?["contentType"],
                    ["href"] = attachmentValue?["href"],
                    ["displayName"] = displayName,
                    ["key"] = attachmentName
                };
            }
        }

        return formattedAttachments;
    }

    private JArray rtrRcd_CreateAttachmentsArray(JObject attachments)
    {
        var attachmentsArray = new JArray();

        if (recordSchemaInfo != null && recordSchemaInfo["attachments"] is JObject attachmentSchemas)
        {
            foreach (var attachment in attachments.Properties())
            {
                var attachmentName = attachment.Name;
                var attachmentSchema = attachmentSchemas[attachmentName] as JObject;
                var displayName = attachmentSchema?["displayName"]?.ToString() ?? attachmentName;

                attachmentsArray.Add(new JObject
                {
                    ["name"] = attachmentName,
                    ["displayName"] = displayName,
                    ["key"] = attachmentName
                });
            }
        }

        return attachmentsArray;
    }

    private async Task rtrRcd_RetrieveRecordSchemaInformation()
    {
        var baseUrl = this.Context.Request.RequestUri.GetLeftPart(UriPartial.Authority);
        var schemaUrl = new Uri(new Uri(baseUrl), "/public/api/v1/records/metadata");

        var request = new HttpRequestMessage(HttpMethod.Get, schemaUrl);
        
        foreach (var header in this.Context.Request.Headers)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        var response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var schemaResponse = JObject.Parse(content);
            this.recordSchemaInfo = new JObject
            {
                ["properties"] = schemaResponse["properties"],
                ["attachments"] = schemaResponse["attachments"]
            };

            this.Context.Logger.LogInformation($"Retrieved schema information: {this.recordSchemaInfo?.ToString()}");
        }
        else
        {
            throw new Exception($"Failed to retrieve schema information. Status code: {response.StatusCode}");
        }
    }

    // ################################################################################
    // Retrieve Email Thread ##########################################################
    // ################################################################################
    private JObject rtrEml_TransformRetrieveEmailThread(JObject body)
    {
        var attachments = body["attachments"] as JArray;
        if (attachments != null)
        {
            var updatedAttachments = new JArray(
                attachments
                    .OfType<JObject>()
                    .Where(attachment => attachment.ContainsKey("download"))
                    .Select(attachment =>
                    {
                        string downloadUrl = attachment["download"].ToString();
                        string key = rtrEml_ExtractKeyFromDownloadUrl(downloadUrl);
                        attachment["key"] = key;
                        return attachment;
                    })
            );
            body["attachments"] = updatedAttachments;
        }
        return body;
    }

    /// Extracts a key from the given download URL.
    private string rtrEml_ExtractKeyFromDownloadUrl(string downloadUrl)
    {
        var match = Regex.Match(downloadUrl, @"/document/([^/]+)/download");
        if (match.Success && match.Groups.Count > 1)
        {
            return match.Groups[1].Value;
        }
        return string.Empty;
    }

}