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
        var response = await this.Context
            .SendAsync(this.Context.Request, this.CancellationToken)
            .ConfigureAwait(false);

        // -------------------------------------------------------------------------
        // Transform 400 errors for CreateWorkflow if MISSING_PARAM + approver
        // -------------------------------------------------------------------------
        if (!response.IsSuccessStatusCode 
            && response.StatusCode == HttpStatusCode.BadRequest
            && "CreateWorkflow".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            response = await TransformCreateWorkflowErrorResponseAsync(response).ConfigureAwait(false);
        }
        // -------------------------------------------------------------------------

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
            case "GetEntityRelationshipType":
                await this.getEntRltTyp_TransformRequest().ConfigureAwait(false);
                break;
            case "CreateEntity":
                await crtEnt_TransformCreateEntityRequest().ConfigureAwait(false);
                break;
            case "UpdateEntity":
                await updEnt_TransformUpdateEntityRequest().ConfigureAwait(false);
                break;
        }
    }

    private async Task UpdateResponse(HttpResponseMessage response)
    {
        switch (this.Context.OperationId)
        {
            case "ListUsers":
                await this.TransformResponseJsonBody(this.lstUsr_TransformUsersList, response)
                    .ConfigureAwait(false);
                break;
            case "RetrieveWorkflowSchema":
                await this.TransformResponseJsonBody(
                        this.rtrWflSch_TransformRetrieveWorkflowSchema,
                        response
                    )
                    .ConfigureAwait(false);
                break;
            case "RetrieveWorkflow":
                await this.TransformResponseJsonBody(
                        this.rtrWfl_TransformRetrieveWorkflow,
                        response
                    )
                    .ConfigureAwait(false);
                break;
            case "RetrieveRecord":
                await this.TransformResponseJsonBody(this.rtrRcd_TransformRetrieveRecord, response)
                    .ConfigureAwait(false);
                break;
            case "RetrieveEmailThread":
                await this.TransformResponseJsonBody(
                        this.rtrEml_TransformRetrieveEmailThread,
                        response
                    )
                    .ConfigureAwait(false);
                break;
            case "CreateRecord":
            case "ReplaceRecord":
            case "UpdateRecordMetadata":
                await this.TransformResponseJsonBody(
                        this.crtRcd_TransformCreateRecordResponse,
                        response
                    )
                    .ConfigureAwait(false);
                break;
            case "ListAllRecords":
                // Get query parameter for properties
                var listAllRecordsQuery =
                    HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query)[
                        "recordPorperties"
                    ] ?? string.Empty;
                await this.TransformResponseJsonBody(
                        body =>
                            lstAllRcd_TransformListAllRecordsResponse(body, listAllRecordsQuery),
                        response
                    )
                    .ConfigureAwait(false);
                break;
            case "RetrieveRecordSchemas":
                // Get query parameter for properties
                var retrieveRecordSchemasQuery =
                    HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query)[
                        "recordPorperties"
                    ] ?? string.Empty;
                await this.TransformResponseJsonBody(
                        body =>
                            rtrRcdSch_TransformRetrieveRecordSchemas(
                                body,
                                retrieveRecordSchemasQuery
                            ),
                        response
                    )
                    .ConfigureAwait(false);
                break;
            case "ListAllWorkflows":
                await this.TransformResponseJsonBody(
                        this.lstAllWfl_TransformListAllWorkflowsResponse,
                        response
                    )
                    .ConfigureAwait(false);
                break;
            case "ListEntityRelationshipTypes":
                await this.lstEntRltTyp_TransformListEntityRelationshipTypes(response).ConfigureAwait(false);
                break;
                break;
            case "GetEntityRelationshipType":
                await this.getEntRltTyp_TransformResponse(response).ConfigureAwait(false);
                break;
            case "RetrieveEntity":
                await this.rtvEnt_TransformResponse(response).ConfigureAwait(false);
                break;
            case "ListAllEntities":
                await this.lstAllEnt_TransformResponse(response).ConfigureAwait(false);
                break;
        }
    }

    private async Task TransformResponseJsonBody(
        Func<JObject, JObject> transformationFunction,
        HttpResponseMessage response
    )
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
        if (
            jsonBody.TryGetValue("metadata", out var metadataToken)
            && metadataToken is JObject metadata
        )
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
            var metadataContent = new StringContent(
                metadataJson,
                Encoding.UTF8,
                "application/json"
            );
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

        var response = await this.Context
            .SendAsync(request, this.CancellationToken)
            .ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JObject.Parse(content);
        }
        else
        {
            throw new Exception(
                $"Failed to retrieve record metadata. Status code: {response.StatusCode}"
            );
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
                if (
                    metadataProperties != null
                    && metadataProperties.TryGetValue(systemPropertyName, out var propertyInfo)
                )
                {
                    type = propertyInfo["type"]?.ToString() ?? "unknown";
                }

                properties[systemPropertyName] = new JObject { ["value"] = value, ["type"] = type };
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
    // Retrieve Entity ################################################################
    // ################################################################################

    private async Task rtvEnt_TransformResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(content))
            return;

        var obj = JObject.Parse(content);

        // Fetch all relationship type and property info (single call!)
        var (_, propDict, relTypeById) = await FetchEntityPropertyDefinitionsAndTypes();

        // ----------------- Relationship Types -----------------
        if (obj.TryGetValue("namedTypeIds", out var namedTypeIdsToken) && namedTypeIdsToken is JArray idArray)
        {
            var relTypesArray = new JArray();

            foreach (var relTypeIdToken in idArray)
            {
                var relTypeId = relTypeIdToken?.ToString()?.Trim().ToLowerInvariant();
                if (!string.IsNullOrEmpty(relTypeId) && relTypeById.TryGetValue(relTypeId, out var relTypeObj))
                {
                    relTypesArray.Add(new JObject
                    {
                        ["id"] = relTypeObj.Value<string>("id"),
                        ["name"] = relTypeObj.Value<string>("name"),
                        ["displayName"] = relTypeObj.Value<string>("displayName"),
                        ["description"] = relTypeObj.Value<string>("description") ?? ""
                    });
                }
            }
            obj["relationshipTypes"] = relTypesArray;
            obj.Remove("namedTypeIds");
        }

        // ----------------- Properties Handling -----------------
        if (obj["properties"] is JObject props)
        {
            var formattedSchemaProperties = new JObject();
            var transformedProps = new JObject();

            foreach (var prop in props.Properties())
            {
                var propName = prop.Name;
                var propertyObj = prop.Value as JObject;
                if (propertyObj == null) continue;

                // --- Meta info from propDict ---
                JObject propDef = null;
                if (propDict.TryGetValue(propName, out var dictValue)) propDef = dictValue;

                propertyObj["systemName"] = propName;
                propertyObj["displayName"] = propDef?.Value<string>("displayName") ?? propName;
                propertyObj["description"] = propDef?.Value<string>("description") ?? $"The {propName}.";
                propertyObj["type"] = propDef?.Value<string>("fullTypeName")
                                            ?? propDef?.SelectToken("type.typeName")?.ToString()
                                            ?? propertyObj.Value<string>("type") ?? "string";
                propertyObj["hidden"] = propDef?.Value<bool?>("hidden") ?? false;
                propertyObj["required"] = propDef?.Value<bool?>("required") ?? false;

                // --- Normalize type for further handling ---
                var type = propertyObj.Value<string>("fullTypeName")
                            ?? propertyObj.Value<string>("type")
                            ?? "string";
                type = type.ToLowerInvariant();
                if (type == "monetaryamount" || type == "monetary_amount") type = "monetary_amount";

                JObject propertySchema = null;
                JToken formattedValue = propertyObj["value"];

                switch (type)
                {
                    case "monetary_amount":
                        propertySchema = new JObject
                        {
                            ["type"] = "object",
                            ["title"] = propertyObj["displayName"],
                            ["description"] = propertyObj["description"],
                            ["x-ms-visibility"] = propertyObj.Value<bool?>("hidden") == true ? "advanced" : "important",
                            ["properties"] = new JObject
                            {
                                ["amount"] = new JObject
                                {
                                    ["type"] = "number",
                                    ["title"] = "Amount",
                                    ["description"] = $"The amount of the {propertyObj["displayName"]}.",
                                    ["x-ms-visibility"] = "important"
                                },
                                ["currency"] = new JObject
                                {
                                    ["type"] = "string",
                                    ["title"] = "Currency",
                                    ["description"] = $"The currency of the {propertyObj["displayName"]}.",
                                    ["x-ms-visibility"] = "important"
                                }
                            }
                        };
                        var val = propertyObj["value"] as JObject;
                        formattedValue = new JObject
                        {
                            ["amount"] = val?["amount"],
                            ["currency"] = val?["currency"]
                        };
                        break;

                    case "duration":
                        propertySchema = new JObject
                        {
                            ["type"] = "object",
                            ["title"] = propertyObj["displayName"],
                            ["description"] = propertyObj["description"],
                            ["x-ms-visibility"] = propertyObj.Value<bool?>("hidden") == true ? "advanced" : "important",
                            ["properties"] = new JObject
                            {
                                ["isoDuration"] = new JObject
                                {
                                    ["type"] = "string",
                                    ["title"] = "ISO Duration",
                                    ["description"] = $"The ISO 8601 duration representation of the {propertyObj["displayName"]}.",
                                    ["x-ms-visibility"] = "important"
                                },
                                ["years"] = new JObject
                                {
                                    ["type"] = "number",
                                    ["title"] = "Years",
                                    ["description"] = $"The years of the {propertyObj["displayName"]}.",
                                    ["x-ms-visibility"] = "important"
                                },
                                ["months"] = new JObject
                                {
                                    ["type"] = "number",
                                    ["title"] = "Months",
                                    ["description"] = $"The months of the {propertyObj["displayName"]}.",
                                    ["x-ms-visibility"] = "important"
                                },
                                ["weeks"] = new JObject
                                {
                                    ["type"] = "number",
                                    ["title"] = "Weeks",
                                    ["description"] = $"The weeks of the {propertyObj["displayName"]}.",
                                    ["x-ms-visibility"] = "important"
                                },
                                ["days"] = new JObject
                                {
                                    ["type"] = "number",
                                    ["title"] = "Days",
                                    ["description"] = $"The days of the {propertyObj["displayName"]}.",
                                    ["x-ms-visibility"] = "important"
                                }
                            }
                        };
                        var iso = propertyObj.Value<string>("value") ?? "";
                        var regex = new System.Text.RegularExpressions.Regex(@"P(?:(\d+)Y)?(?:(\d+)M)?(?:(\d+)W)?(?:(\d+)D)?");
                        var match = regex.Match(iso);
                        var years = match.Success && !string.IsNullOrEmpty(match.Groups[1].Value) ? int.Parse(match.Groups[1].Value) : 0;
                        var months = match.Success && !string.IsNullOrEmpty(match.Groups[2].Value) ? int.Parse(match.Groups[2].Value) : 0;
                        var weeks = match.Success && !string.IsNullOrEmpty(match.Groups[3].Value) ? int.Parse(match.Groups[3].Value) : 0;
                        var days = match.Success && !string.IsNullOrEmpty(match.Groups[4].Value) ? int.Parse(match.Groups[4].Value) : 0;
                        formattedValue = new JObject
                        {
                            ["isoDuration"] = iso,
                            ["years"] = years,
                            ["months"] = months,
                            ["weeks"] = weeks,
                            ["days"] = days
                        };
                        break;

                    case "address":
                        propertySchema = new JObject
                        {
                            ["type"] = "object",
                            ["title"] = propertyObj["displayName"],
                            ["description"] = propertyObj["description"],
                            ["x-ms-visibility"] = propertyObj.Value<bool?>("hidden") == true ? "advanced" : "important",
                            ["properties"] = new JObject
                            {
                                ["lines"] = new JObject
                                {
                                    ["type"] = "array",
                                    ["items"] = new JObject { ["type"] = "string" },
                                    ["title"] = "Address Lines",
                                    ["description"] = "The lines of the address.",
                                    ["x-ms-visibility"] = "important"
                                },
                                ["locality"] = new JObject { ["type"] = "string", ["title"] = "Locality", ["description"] = "The locality.", ["x-ms-visibility"] = "important" },
                                ["region"] = new JObject { ["type"] = "string", ["title"] = "Region", ["description"] = "The region.", ["x-ms-visibility"] = "important" },
                                ["postcode"] = new JObject { ["type"] = "string", ["title"] = "Postcode", ["description"] = "The postcode.", ["x-ms-visibility"] = "important" },
                                ["country"] = new JObject { ["type"] = "string", ["title"] = "Country", ["description"] = "The country.", ["x-ms-visibility"] = "important" }
                            }
                        };
                        formattedValue = propertyObj["value"];
                        break;

                    case "boolean":
                        propertySchema = new JObject
                        {
                            ["type"] = "boolean",
                            ["title"] = propertyObj["displayName"],
                            ["description"] = propertyObj["description"],
                            ["x-ms-visibility"] = propertyObj.Value<bool?>("hidden") == true ? "advanced" : "important"
                        };
                        formattedValue = propertyObj["value"];
                        break;

                    case "number":
                        propertySchema = new JObject
                        {
                            ["type"] = "number",
                            ["title"] = propertyObj["displayName"],
                            ["description"] = propertyObj["description"],
                            ["x-ms-visibility"] = propertyObj.Value<bool?>("hidden") == true ? "advanced" : "important"
                        };
                        formattedValue = propertyObj["value"];
                        break;

                    case "date":
                        propertySchema = new JObject
                        {
                            ["type"] = "string",
                            ["format"] = "date-time",
                            ["title"] = propertyObj["displayName"],
                            ["description"] = propertyObj["description"],
                            ["x-ms-visibility"] = propertyObj.Value<bool?>("hidden") == true ? "advanced" : "important"
                        };
                        formattedValue = propertyObj["value"];
                        break;

                    case "email":
                        propertySchema = new JObject
                        {
                            ["type"] = "string",
                            ["format"] = "email",
                            ["title"] = propertyObj["displayName"],
                            ["description"] = propertyObj["description"],
                            ["x-ms-visibility"] = propertyObj.Value<bool?>("hidden") == true ? "advanced" : "important"
                        };
                        formattedValue = propertyObj["value"];
                        break;

                    case "string":
                    default:
                        propertySchema = new JObject
                        {
                            ["type"] = "string",
                            ["title"] = propertyObj["displayName"],
                            ["description"] = propertyObj["description"],
                            ["x-ms-visibility"] = propertyObj.Value<bool?>("hidden") == true ? "advanced" : "important"
                        };
                        formattedValue = propertyObj["value"];
                        break;
                }

                formattedSchemaProperties[propName] = propertySchema;
                transformedProps[propName] = formattedValue;
            }

            // --- Build propertiesAsArray ---
            var propertiesAsArray = new JArray();
            foreach (var prop in props.Properties())
            {
                var propertyObj = prop.Value as JObject;
                if (propertyObj == null) continue;

                var arrObj = new JObject
                {
                    ["key"] = propertyObj.Value<string>("systemName") ?? prop.Name,
                    ["displayName"] = propertyObj.Value<string>("displayName") ?? prop.Name,
                    ["description"] = propertyObj.Value<string>("description") ?? $"The {prop.Name}.",
                    ["hidden"] = propertyObj.Value<bool?>("hidden") ?? false,
                    ["required"] = propertyObj.Value<bool?>("required") ?? false,
                    ["type"] = propertyObj.Value<string>("type") ?? "string",
                    ["value"] = transformedProps[prop.Name]
                };
                propertiesAsArray.Add(arrObj);
            }

            obj["propertiesAsArray"] = propertiesAsArray;
            obj["formattedSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = formattedSchemaProperties
            };
            obj["formattedProperties"] = transformedProps;
        }

        response.Content = CreateJsonContent(obj.ToString());
    }

    // ################################################################################
    // Create Entity ##################################################################
    // ################################################################################

    private async Task crtEnt_TransformCreateEntityRequest()
    {
        var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonBody = JObject.Parse(content);

        if (jsonBody.TryGetValue("properties", out var propertiesToken) && propertiesToken is JObject properties)
        {
            var reformatted = new JObject();

            foreach (var prop in properties.Properties())
            {
                var propName = prop.Name;
                var propObj = prop.Value as JObject;
                if (propObj == null)
                    continue;

                var type = propObj.Value<string>("type") ?? "";
                var valueToken = propObj["value"];

                // Special handling for durations
                if (type.Equals("duration", StringComparison.OrdinalIgnoreCase) && valueToken is JObject durationObj)
                {
                    var years = durationObj.Value<int?>("years") ?? 0;
                    var months = durationObj.Value<int?>("months") ?? 0;
                    var weeks = durationObj.Value<int?>("weeks") ?? 0;
                    var days = durationObj.Value<int?>("days") ?? 0;

                    if (years > 0 || months > 0 || weeks > 0 || days > 0)
                    {
                        var sb = new StringBuilder("P");
                        if (years > 0) sb.Append($"{years}Y");
                        if (months > 0) sb.Append($"{months}M");
                        if (weeks > 0) sb.Append($"{weeks}W");
                        if (days > 0) sb.Append($"{days}D");
                        valueToken = sb.ToString();
                    }
                    else
                    {
                        valueToken = "P0D";
                    }
                }

                // Add to new properties object in Ironclad structure
                reformatted[propName] = new JObject
                {
                    ["type"] = type,
                    ["value"] = valueToken
                };
            }

            // Overwrite original properties
            jsonBody["properties"] = reformatted;
        }

        // Transform relationshipTypeKey to array if string
        if (jsonBody.TryGetValue("relationshipTypeKey", out var relTypeKeyToken) &&
            relTypeKeyToken.Type == JTokenType.String)
        {
            jsonBody["relationshipTypeKey"] = new JArray(relTypeKeyToken.ToString());
        }

        // Write the transformed content back
        this.Context.Request.Content = CreateJsonContent(jsonBody.ToString());
    }

    // ################################################################################
    // Update Entity ##################################################################
    // ################################################################################

    private async Task updEnt_TransformUpdateEntityRequest()
    {
        var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonBody = JObject.Parse(content);

        // Transform relationshipTypeKey to array if string
        if (jsonBody.TryGetValue("relationshipTypeKey", out var relTypeKeyToken) &&
            relTypeKeyToken.Type == JTokenType.String)
        {
            jsonBody["relationshipTypeKey"] = new JArray(relTypeKeyToken.ToString());
        }

        // Transform addProperties array to object keyed by property name with type/value
        if (jsonBody.TryGetValue("addProperties", out var addPropsToken) && addPropsToken is JArray addPropsArray)
        {
            var (_, propDict, _) = await FetchEntityPropertyDefinitionsAndTypes();
            var addPropsObj = new JObject();

            foreach (var item in addPropsArray.OfType<JObject>())
            {
                var key = item.Value<string>("key");
                var value = item["value"];
                if (string.IsNullOrEmpty(key))
                    continue;

                // Get and normalize type
                var type = propDict.TryGetValue(key, out var def) ?
                    (def.Value<string>("fullTypeName")
                    ?? def.SelectToken("type.typeName")?.ToString()
                    ?? def.Value<string>("type") ?? "string")
                    : "string";

                type = type.ToLowerInvariant();
                if (type == "monetaryamount" || type == "monetary_amount")
                    type = "monetary_amount";

                addPropsObj[key] = new JObject
                {
                    ["type"] = type,
                    ["value"] = value
                };
            }

            // Overwrite array with object
            jsonBody["addProperties"] = addPropsObj;
        }

        // Write the transformed content back
        this.Context.Request.Content = CreateJsonContent(jsonBody.ToString());
    }

    // ################################################################################
    // List All Entities ##############################################################
    // ################################################################################

    private async Task lstAllEnt_TransformResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(content))
            return;

        var obj = JObject.Parse(content);

        // Fetch property definitions and all relationship types (single call!)
        var (_, propDict, relTypeById) = await FetchEntityPropertyDefinitionsAndTypes();

        if (obj["list"] is JArray list)
        {
            foreach (var entity in list.OfType<JObject>())
            {
                // ----------------- Properties Handling -----------------
                if (entity["properties"] is JObject props)
                {
                    foreach (var prop in props.Properties())
                    {
                        if (prop.Value is JObject propertyObj && propDict.TryGetValue(prop.Name, out var propDef))
                        {
                            propertyObj["systemName"]   = prop.Name;
                            propertyObj["displayName"]  = propDef?.Value<string>("displayName")  ?? prop.Name;
                            propertyObj["description"]  = propDef?.Value<string>("description")  ?? "";
                            propertyObj["fullTypeName"] = propDef?.Value<string>("fullTypeName")
                                                        ?? propDef?.SelectToken("type.typeName")?.ToString()
                                                        ?? propertyObj.Value<string>("type") ?? "string";
                            propertyObj["hidden"]       = propDef?.Value<bool?>("hidden")   ?? false;
                            propertyObj["required"]     = propDef?.Value<bool?>("required") ?? false;
                        }
                    }

                    // Create enriched propertiesAsArray
                    var propsArray = new JArray();
                    foreach (var prop in props.Properties())
                    {
                        if (prop.Value is JObject propertyObj)
                        {
                            var arrObj = new JObject(propertyObj);
                            propsArray.Add(arrObj);
                        }
                    }
                    entity["propertiesAsArray"] = propsArray;
                }

                // ----------------- RelationshipTypes Handling -----------------
                var relTypesArray = new JArray();
                if (entity["namedTypeIds"] is JArray idArray)
                {
                    foreach (var relTypeIdToken in idArray)
                    {
                        var relTypeId = relTypeIdToken?.ToString()?.Trim().ToLowerInvariant();
                        if (!string.IsNullOrEmpty(relTypeId) && relTypeById.TryGetValue(relTypeId, out var relTypeObj))
                        {
                            relTypesArray.Add(new JObject
                            {
                                ["id"] = relTypeObj.Value<string>("id"),
                                ["name"] = relTypeObj.Value<string>("name"),
                                ["displayName"] = relTypeObj.Value<string>("displayName"),
                                ["description"] = relTypeObj.Value<string>("description") ?? ""
                            });
                        }
                    }
                }
                entity["relationshipTypes"] = relTypesArray;
                entity.Remove("namedTypeIds"); // Optional

                // ----------------- Label Handling -----------------
                var name = entity["name"]?.ToString() ?? string.Empty;
                var ironcladId = entity["ironcladId"]?.ToString() ?? string.Empty;
                entity["label"] = $"{name} ({ironcladId})";
            }
        }
        response.Content = CreateJsonContent(obj.ToString());
    }

    // ################################################################################
    // Relationship Helper Function ###################################################
    // ################################################################################

    private async Task<(JArray allRelTypes, Dictionary<string, JObject> propDict, Dictionary<string, JObject> relTypeById)> FetchEntityPropertyDefinitionsAndTypes()
    {
        var uri = this.Context.Request.RequestUri;
        var baseUrl = uri.GetLeftPart(UriPartial.Authority);
        var relTypeUrl = new Uri(new Uri(baseUrl), "/public/api/v1/entities/relationship-types");
        var req = new HttpRequestMessage(HttpMethod.Get, relTypeUrl);

        foreach (var header in this.Context.Request.Headers)
            req.Headers.TryAddWithoutValidation(header.Key, header.Value);

        var resp = await this.Context.SendAsync(req, this.CancellationToken).ConfigureAwait(false);
        if (!resp.IsSuccessStatusCode)
            throw new Exception($"Failed to fetch property definitions. Status: {resp.StatusCode}");

        var respContent = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
        var arr = JArray.Parse(respContent);

        var propDict = new Dictionary<string, JObject>(StringComparer.OrdinalIgnoreCase);
        var relTypeById = new Dictionary<string, JObject>(StringComparer.OrdinalIgnoreCase);

        foreach (var relType in arr.OfType<JObject>())
        {
            var id = relType.Value<string>("id")?.Trim().ToLowerInvariant();
            if (!string.IsNullOrEmpty(id))
                relTypeById[id] = relType;

            if (relType["properties"] is JObject props)
            {
                foreach (var prop in props.Properties())
                {
                    if (prop.Value is JObject propDef && !propDict.ContainsKey(prop.Name))
                    {
                        propDict[prop.Name] = propDef;
                    }
                }
            }
        }
        return (arr, propDict, relTypeById);
    }

    // ################################################################################
    // Relationship Types #############################################################
    // ################################################################################

    private async Task lstEntRltTyp_TransformListEntityRelationshipTypes(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(content))
            return;

        var arr = JArray.Parse(content);

        // Collect all unique properties by 'key'
        var uniqueProps = new Dictionary<string, JObject>(StringComparer.OrdinalIgnoreCase);

        foreach (var relTypeObj in arr.OfType<JObject>())
        {
            if (!(relTypeObj["properties"] is JObject props))
                continue;

            foreach (var prop in props.Properties())
            {
                var propObj = prop.Value as JObject;
                if (propObj == null)
                    continue;

                var key = propObj.Value<string>("key") ?? prop.Name;
                if (uniqueProps.ContainsKey(key))
                    continue;

                string description = propObj.Value<string>("description") ?? "";
                string displayName = propObj.Value<string>("displayName") ?? key;
                string fullTypeName = 
                    propObj.Value<string>("fullTypeName") 
                    ?? propObj.SelectToken("type.typeName")?.ToString() 
                    ?? propObj.Value<string>("type") ?? "string";
                fullTypeName = fullTypeName.ToLowerInvariant();

                // Normalize monetary types
                if (fullTypeName == "monetaryamount" || fullTypeName == "monetary_amount")
                    fullTypeName = "monetary_amount";

                var propertyObj = new JObject
                {
                    ["key"] = key,
                    ["description"] = description,
                    ["displayName"] = displayName,
                    ["type"] = fullTypeName
                };
                uniqueProps[key] = propertyObj;
            }
        }

        // Compose the final result object
        var result = new JObject
        {
            ["relationshipTypes"] = arr,
            ["properties"] = new JArray(uniqueProps.Values)
        };

        response.Content = CreateJsonContent(result.ToString());
    } 

    // ################################################################################
    // Get Entity Relationship Type ###################################################
    // ################################################################################

    // Request Trandformer

    private Task getEntRltTyp_TransformRequest()
    {
        // Example incoming path:
        //   /public/api/v1/entities/relationship-types/{systemName}
        var uri = this.Context.Request.RequestUri;
        var segments = uri.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        var systemName = segments.Last(); // {systemName}

        // Stash the target system name in a transient header for later use.
        this.Context.Request.Headers.Remove("X-RelType-SystemName");
        this.Context.Request.Headers.Add("X-RelType-SystemName", systemName);

        // Rebuild the URI without the final segment so we hit the *list* endpoint.
        var listPath = "/" + string.Join("/", segments.Take(segments.Length - 1));
        var newUri = new Uri(uri.GetLeftPart(UriPartial.Authority) + listPath);
        this.Context.Request.RequestUri = newUri;

        return Task.CompletedTask;
    }
    
    private async Task getEntRltTyp_TransformResponse(HttpResponseMessage response)
    {
        if (!this.Context.Request.Headers.TryGetValues("X-RelType-SystemName", out var vals))
            return;

        var systemName = vals.FirstOrDefault();
        if (String.IsNullOrEmpty(systemName))
            return;

        var bodyText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(bodyText))
            return;

        // Just parse as JArray
        var arr = JArray.Parse(bodyText);

        JObject singleItem = null;

        foreach (var relTypeObj in arr.OfType<JObject>())
        {
            if ((relTypeObj["name"]?.ToString().Equals(systemName, StringComparison.OrdinalIgnoreCase)).GetValueOrDefault())
            {
                if (relTypeObj["properties"] is JObject props)
                {
                    var formattedSchemaProps = new JObject();
                    var propertiesAsArray = new JArray();
                    var requiredFields = new List<string>();

                    foreach (var prop in props.Properties())
                    {
                        var propName = prop.Name;
                        var propObj = prop.Value as JObject;
                        if (propObj == null) continue;

                        string type = propObj.Value<string>("fullTypeName")
                                ?? propObj.SelectToken("type.typeName")?.ToString()
                                ?? propObj.Value<string>("type") ?? "string";
                        type = type.ToLowerInvariant();

                        // Normalize monetaryamount/monetary_amount to "monetary_amount"
                        if (type == "monetaryamount" || type == "monetary_amount")
                            type = "monetary_amount";

                        string displayName = propObj.Value<string>("displayName") ?? propName;
                        string description = propObj.Value<string>("description");
                        if (string.IsNullOrWhiteSpace(description))
                            description = $"The {displayName}.";

                        bool hidden = propObj.Value<bool?>("hidden") ?? false;
                        bool required = propObj.Value<bool?>("required") ?? false;
                        if (required)
                            requiredFields.Add(propName);

                        JObject valueSchema;
                        switch (type)
                        {
                            case "monetary_amount":
                                valueSchema = new JObject
                                {
                                    ["type"] = "object",
                                    ["title"] = "Value",
                                    ["description"] = description,
                                    ["x-ms-visibility"] = "important",
                                    ["properties"] = new JObject
                                    {
                                        ["amount"] = new JObject
                                        {
                                            ["type"] = "number",
                                            ["title"] = "Amount",
                                            ["description"] = $"The amount of the {displayName}.",
                                            ["x-ms-visibility"] = "important"
                                        },
                                        ["currency"] = new JObject
                                        {
                                            ["type"] = "string",
                                            ["title"] = "Currency",
                                            ["description"] = $"The currency of the {displayName}.",
                                            ["x-ms-visibility"] = "important"
                                        }
                                    },
                                    ["required"] = new JArray { "amount", "currency" }
                                };
                                break;

                            case "duration":
                                valueSchema = new JObject
                                {
                                    ["type"] = "object",
                                    ["title"] = "Value",
                                    ["description"] = description,
                                    ["x-ms-visibility"] = "important",
                                    ["properties"] = new JObject
                                    {
                                        ["years"] = new JObject
                                        {
                                            ["type"] = "number",
                                            ["title"] = "Years",
                                            ["description"] = $"The years of the {displayName}.",
                                            ["x-ms-visibility"] = "important"
                                        },
                                        ["months"] = new JObject
                                        {
                                            ["type"] = "number",
                                            ["title"] = "Months",
                                            ["description"] = $"The months of the {displayName}.",
                                            ["x-ms-visibility"] = "important"
                                        },
                                        ["weeks"] = new JObject
                                        {
                                            ["type"] = "number",
                                            ["title"] = "Weeks",
                                            ["description"] = $"The weeks of the {displayName}.",
                                            ["x-ms-visibility"] = "important"
                                        },
                                        ["days"] = new JObject
                                        {
                                            ["type"] = "number",
                                            ["title"] = "Days",
                                            ["description"] = $"The days of the {displayName}.",
                                            ["x-ms-visibility"] = "important"
                                        }
                                    }
                                };
                                break;

                            case "address":
                                valueSchema = new JObject
                                {
                                    ["type"] = "object",
                                    ["title"] = "Value",
                                    ["description"] = description,
                                    ["x-ms-visibility"] = "important",
                                    ["properties"] = new JObject
                                    {
                                        ["lines"] = new JObject
                                        {
                                            ["type"] = "array",
                                            ["title"] = $"Address Lines",
                                            ["description"] = $"The lines of the {displayName}.",
                                            ["items"] = new JObject
                                            {
                                                ["type"] = "string",
                                                ["title"] = $"{displayName} Line",
                                                ["description"] = $"An individual line of the {displayName}.",
                                                ["x-ms-visibility"] = "important"
                                            },
                                            ["x-ms-visibility"] = "important"
                                        },
                                        ["locality"] = new JObject
                                        {
                                            ["type"] = "string",
                                            ["title"] = "Locality",
                                            ["description"] = $"The locality of the {displayName}.",
                                            ["x-ms-visibility"] = "important"
                                        },
                                        ["region"] = new JObject
                                        {
                                            ["type"] = "string",
                                            ["title"] = "Region",
                                            ["description"] = $"The region of the {displayName}.",
                                            ["x-ms-visibility"] = "important"
                                        },
                                        ["postcode"] = new JObject
                                        {
                                            ["type"] = "string",
                                            ["title"] = $"Postcode",
                                            ["description"] = $"The postcode of the {displayName}.",
                                            ["x-ms-visibility"] = "important"
                                        },
                                        ["country"] = new JObject
                                        {
                                            ["type"] = "string",
                                            ["title"] = $"Country",
                                            ["description"] = $"The country of the {displayName}.",
                                            ["x-ms-visibility"] = "important"
                                        }
                                    }
                                };
                                break;

                            case "boolean":
                                valueSchema = new JObject
                                {
                                    ["type"] = "boolean",
                                    ["title"] = "Value",
                                    ["description"] = description,
                                    ["x-ms-visibility"] = "important"
                                };
                                break;

                            case "number":
                                valueSchema = new JObject
                                {
                                    ["type"] = "number",
                                    ["title"] = "Value",
                                    ["description"] = description,
                                    ["x-ms-visibility"] = "important"
                                };
                                break;

                            case "email":
                                valueSchema = new JObject
                                {
                                    ["type"] = "string",
                                    ["format"] = "email",
                                    ["title"] = "Value",
                                    ["description"] = description,
                                    ["x-ms-visibility"] = "important"
                                };
                                break;

                            case "date":
                                valueSchema = new JObject
                                {
                                    ["type"] = "string",
                                    ["format"] = "date-time",
                                    ["title"] = "Value",
                                    ["description"] = description,
                                    ["x-ms-visibility"] = "important"
                                };
                                break;

                            default:
                                valueSchema = new JObject
                                {
                                    ["type"] = "string",
                                    ["title"] = "Value",
                                    ["description"] = description,
                                    ["x-ms-visibility"] = "important"
                                };
                                break;
                        }

                        var propSchema = new JObject
                        {
                            ["type"] = "object",
                            ["title"] = displayName,
                            ["description"] = description,
                            ["x-ms-visibility"] = hidden ? "advanced" : "important",
                            ["properties"] = new JObject
                            {
                                ["value"] = valueSchema,
                                ["type"] = new JObject
                                {
                                    ["type"] = "string",
                                    ["title"] = "Type",
                                    ["description"] = "The Ironclad data type.",
                                    ["x-ms-visibility"] = "internal",
                                    ["default"] = type
                                }
                            },
                            ["required"] = new JArray { "value", "type" }
                        };

                        formattedSchemaProps[propName] = propSchema;

                        propertiesAsArray.Add(new JObject
                        {
                            ["key"]   = propName,
                            ["displayName"]  = displayName,
                            ["description"]  = description,
                            ["hidden"]       = hidden,
                            ["required"]     = required,
                            ["type"]         = type
                        });
                    }

                    var formattedSchema = new JObject
                    {
                        ["type"] = "object",
                        ["properties"] = formattedSchemaProps
                    };

                    if (requiredFields.Count > 0)
                        formattedSchema["required"] = new JArray(requiredFields);

                    relTypeObj["propertiesAsArray"] = propertiesAsArray;
                    relTypeObj["formattedSchema"] = formattedSchema;
                }
                singleItem = relTypeObj;
                break;
            }
        }

        response.Content = CreateJsonContent((singleItem ?? new JObject()).ToString());
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

        var metadataResponse = await this.Context
            .SendAsync(metadataRequest, this.CancellationToken)
            .ConfigureAwait(false);

        if (!metadataResponse.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Failed to retrieve record metadata. Status code: {metadataResponse.StatusCode}"
            );
        }

        var metadataContent = await metadataResponse.Content
            .ReadAsStringAsync()
            .ConfigureAwait(false);
        return JObject.Parse(metadataContent);
    }

    private async Task updRcd_TransformUpdateRecordMetadataRequest()
    {
        var metadata = await updRcd_FetchRecordMetadata();
        var metadataProperties = metadata["properties"] as JObject;

        var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonBody = JObject.Parse(content);

        if (
            jsonBody.TryGetValue("addProperties", out var addPropertiesToken)
            && addPropertiesToken is JArray addPropertiesArray
        )
        {
            var transformedProperties = new JObject();

            foreach (var prop in addPropertiesArray)
            {
                if (
                    prop is JObject propObject
                    && propObject.TryGetValue("propertySystemName", out var propertySystemNameToken)
                    && propObject.TryGetValue("value", out var valueToken)
                )
                {
                    string propertySystemName = propertySystemNameToken.ToString();
                    JToken value = valueToken;

                    string type = "unknown";
                    if (
                        metadataProperties != null
                        && metadataProperties.TryGetValue(propertySystemName, out var propertyInfo)
                    )
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

        // Transform launchApprovals into attributes using the new function
        jsonBody = crtAsyncWfl_ProcessLaunchApprovals(jsonBody);

        // Update the request content with transformed JSON
        this.Context.Request.Content = CreateJsonContent(jsonBody.ToString());
    }

        private JObject crtAsyncWfl_ProcessLaunchApprovals(JObject json)
    {
        // Check if we have "attributes" at the root
        if (json.TryGetValue("attributes", out var attributesToken) && attributesToken is JObject attributes)
        {
            // Check if we have a launchApprovals array
            if (
                attributes.TryGetValue("launchApprovals", out var launchApprovalsToken)
                && launchApprovalsToken is JArray launchApprovalsArray
            )
            {
                // For each object in launchApprovals,
                // set the roleName as the key and the assignee as the value in attributes
                foreach (var item in launchApprovalsArray)
                {
                    if (item is JObject approvalObj)
                    {
                        var roleName = approvalObj["roleName"]?.ToString();
                        var assignee = approvalObj["assignee"]?.ToString();

                        // Only add if roleName is present
                        if (!string.IsNullOrEmpty(roleName))
                        {
                            attributes[roleName] = assignee ?? string.Empty;
                        }
                    }
                }

                // Finally, remove the entire launchApprovals array
                attributes.Remove("launchApprovals");
            }
        }

        return json;
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
                        var groupedProperties = objItem
                            .Properties()
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

    // Transform error message on missing param
    private async Task<HttpResponseMessage> TransformCreateWorkflowErrorResponseAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(content))
        {
            try
            {
                var json = JObject.Parse(content);

                // Check if it's the "MISSING_PARAM" scenario
                if (json["code"]?.ToString() == "MISSING_PARAM" && json["param"] != null)
                {
                    // "param" is a string that looks like '["approverdfa77..."]', so parse it
                    string paramValue = json["param"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(paramValue))
                    {
                        // safely parse the stringified array
                        var paramArray = JArray.Parse(paramValue);

                        // Look for any item that starts with "approver"
                        var approverItems = paramArray
                            .Where(token => token.Type == JTokenType.String)
                            .Select(token => token.ToString())
                            .Where(str => str.StartsWith("approver", StringComparison.OrdinalIgnoreCase))
                            .ToList();

                        if (approverItems.Any())
                        {
                            // For simplicity, just transform the message for the first approver role
                            var firstApprover = approverItems.First();
                            json["message"] =
                                $"Missing assignment for approval role: \"{firstApprover}\". "
                                + "Please add it to the Launch Approvals in the launch action using the mentioned role.";

                            // Repack the updated JSON as the response content
                            response.Content = CreateJsonContent(json.ToString());
                        }
                    }
                }
            }
            catch
            {
                // If parsing fails, just return original response
            }
        }
        return response;
    }
    // Transform Request
    private async Task crtWfl_TransformToMultipartRequestForWorkflows()
    {
        var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var jsonBody = JObject.Parse(content);

        // Unflatten the entire JSON body
        jsonBody = crtWfl_UnflattenJson(jsonBody);

        jsonBody = crtWfl_ProcessLaunchApprovals(jsonBody);

        var multipartContent = new MultipartFormDataContent();

        if (
            jsonBody.TryGetValue("attributes", out var attributesToken)
            && attributesToken is JObject attributes
        )
        {
            crtWfl_ProcessWorkflowAttributes(attributes, multipartContent, jsonBody);
        }

        var dataContent = new StringContent(jsonBody.ToString(), Encoding.UTF8, "application/json");
        multipartContent.Add(dataContent, "data");

        this.Context.Request.Content = multipartContent;
    }

    private JObject crtWfl_ProcessLaunchApprovals(JObject json)
    {
        // Check if we have "attributes" at the root
        if (json.TryGetValue("attributes", out var attributesToken) && attributesToken is JObject attributes)
        {
            // Check if we have a launchApprovals array
            if (
                attributes.TryGetValue("launchApprovals", out var launchApprovalsToken)
                && launchApprovalsToken is JArray launchApprovalsArray
            )
            {
                // For each object in launchApprovals,
                // set the roleName as the key and the assignee as the value in attributes
                foreach (var item in launchApprovalsArray)
                {
                    if (item is JObject approvalObj)
                    {
                        var roleName = approvalObj["roleName"]?.ToString();
                        var assignee = approvalObj["assignee"]?.ToString();

                        // Only add if roleName is present
                        if (!string.IsNullOrEmpty(roleName))
                        {
                            attributes[roleName] = assignee ?? string.Empty;
                        }
                    }
                }

                // Finally, remove the entire launchApprovals array
                attributes.Remove("launchApprovals");
            }
        }

        return json;
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
                        var groupedProperties = objItem
                            .Properties()
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

    private void crtWfl_ProcessWorkflowAttributes(
        JObject attributes,
        MultipartFormDataContent multipartContent,
        JObject jsonBody
    )
    {
        foreach (var attribute in attributes.Properties())
        {
            if (
                attribute.Value is JArray arrayValue
                && arrayValue.Any(item => item is JObject obj && obj.ContainsKey("fileContent"))
            )
            {
                crtWfl_ProcessFileArrayAttribute(
                    attribute.Name,
                    arrayValue,
                    multipartContent,
                    jsonBody
                );
            }
        }
    }

    private void crtWfl_ProcessFileArrayAttribute(
        string attributeName,
        JArray arrayValue,
        MultipartFormDataContent multipartContent,
        JObject jsonBody
    )
    {
        var updatedArray = new JArray();

        foreach (var item in arrayValue)
        {
            if (
                item is JObject fileObject
                && fileObject.TryGetValue("fileName", out var fileNameToken)
                && fileObject.TryGetValue("fileContent", out var fileContentToken)
            )
            {
                string fileName = fileNameToken.ToString();
                string fileContent = fileContentToken.ToString();

                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(fileContent))
                {
                    string fileKey = $"{attributeName}_{Guid.NewGuid().ToString("N")}";

                    var fileBytes = Convert.FromBase64String(fileContent);
                    var fileContentPart = new ByteArrayContent(fileBytes);
                    fileContentPart.Headers.ContentType = new MediaTypeHeaderValue(
                        "application/octet-stream"
                    );
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
        if (
            jsonBody.TryGetValue("metadata", out var metadataToken)
            && metadataToken is JObject metadata
        )
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
            var metadataContent = new StringContent(
                metadataJson,
                Encoding.UTF8,
                "application/json"
            );
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
                resources.Select(
                    user =>
                    {
                        var givenName = user["name"]?["givenName"]?.ToString() ?? "";
                        var familyName = user["name"]?["familyName"]?.ToString() ?? "";
                        var displayName = $"{givenName} {familyName}".Trim();

                        var email =
                            user["emails"]?.FirstOrDefault()?["value"]?.ToString().ToLower() ?? "";

                        user["displayName"] = displayName;
                        user["combinedLabel"] = $"{displayName} ({email})";

                        return user;
                    }
                )
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
        if (
            !schemas.Any(
                s =>
                    s.Type == JTokenType.String
                    && s.Value<string>() == "urn:ietf:params:scim:api:messages:2.0:PatchOp"
            )
        )
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
        if (
            !schemas.Any(
                s =>
                    s.Type == JTokenType.String
                    && s.Value<string>() == "urn:ietf:params:scim:api:messages:2.0:PatchOp"
            )
        )
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
            var requiredProperties = new JArray();

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
                        if (
                            objectType == "address"
                            || objectType == "monetaryamount"
                            || objectType == "duration"
                        )
                        {
                            effectivePropertyType = objectType;
                        }
                    }


                // Handle options, default, and required for launchSchema, formattedSchema, and schemaAsArray
                JObject formattedLaunchProperty = rtrWflSch_FormatLaunchProperty(
                    effectivePropertyType,
                    displayName,
                    propertyName,
                    propertyValue
                );
                // If options are present, add enum to launchSchema property
                if (propertyValue["options"] is JObject optionsObj && optionsObj["values"] is JArray valuesArr)
                {
                    formattedLaunchProperty["enum"] = valuesArr.DeepClone();
                }
                // If default is present, add to launchSchema property
                if (propertyValue["default"] != null)
                {
                    formattedLaunchProperty["default"] = propertyValue["default"].DeepClone();
                }
                // If required is 'always', add propertyName to requiredProperties
                if (propertyValue["required"] != null && propertyValue["required"].ToString() == "always")
                {
                    requiredProperties.Add(propertyName);
                }
                launchSchema[propertyName] = formattedLaunchProperty;

                // For formattedSchema, add options and default as-is if present
                JObject formattedProperty = rtrWflSch_FormatProperty(
                    effectivePropertyType,
                    displayName,
                    propertyName,
                    propertyValue
                );
                if (propertyValue["options"] != null)
                {
                    formattedProperty["options"] = propertyValue["options"].DeepClone();
                }
                if (propertyValue["default"] != null)
                {
                    formattedProperty["default"] = propertyValue["default"].DeepClone();
                }
                formattedSchema[propertyName] = formattedProperty;

                // For schemaAsArray, add options and default as-is if present
                JObject schemaArrayItem = rtrWflSch_ParseSchemaArrayItem(
                    propertyName,
                    displayName,
                    effectivePropertyType,
                    isReadOnly
                );
                if (propertyValue["options"] != null)
                {
                    schemaArrayItem["options"] = propertyValue["options"].DeepClone();
                }
                if (propertyValue["default"] != null)
                {
                    schemaArrayItem["default"] = propertyValue["default"].DeepClone();
                }
                schemaAsArray.Add(schemaArrayItem);

                    if (
                        propertyType == "array"
                        && propertyValue["elementType"] is JObject arrayElementType
                        && arrayElementType["type"]?.ToString().ToLower() == "document"
                    )
                    {
                        documentSchemaAsArray.Add(
                            rtrWflSch_ParseDocumentSchemaItem(propertyName, displayName, isReadOnly)
                        );
                    }
                }
            }

            // Add the approvals array into the launchSchema properties with a single schema object for items
            launchSchema["launchApprovals"] = new JObject
            {
                ["type"] = "array",
                ["title"] = "Workflow Approval Roles",
                ["description"] = "The approval roles for this workflow.",
                ["x-ms-visibility"] = "important",
                ["items"] = new JObject
                {
                    ["type"] = "object",
                    ["title"] = "Approval Role",
                    ["description"] = "An approval role on this workflow.",
                    ["x-ms-visibility"] = "important",
                    ["properties"] = new JObject
                    {
                        ["roleName"] = new JObject
                        {
                            ["type"] = "string",
                            ["title"] = "Property Name",
                            ["description"] = "The system name of the role property.",
                            ["x-ms-visibility"] = "important"
                        },
                        ["assignee"] = new JObject
                        {
                            ["type"] = "string",
                            ["title"] = "Assignee ID",
                            ["description"] = "The ID of the user assigned to this approval role.",
                            ["x-ms-visibility"] = "important",
                            ["x-ms-dynamic-values"] = new JObject
                            {
                                ["operationId"] = "ListUsers",
                                ["value-collection"] = "Resources",
                                ["value-path"] = "id",
                                ["value-title"] = "username"
                            }
                        }
                    },
                    ["required"] = new JArray
                    {
                        "roleName",
                        "assignee"
                    }
                }
            };

            // Compose required array: always include counterpartyName, plus any with required: always
            var launchRequired = new JArray { "counterpartyName" };
            foreach (var reqProp in requiredProperties)
            {
                if (!launchRequired.Contains(reqProp))
                {
                    launchRequired.Add(reqProp);
                }
            }
            body["launchSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = launchSchema,
                ["required"] = launchRequired
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

    private JObject rtrWflSch_FormatLaunchProperty(
        string propertyType,
        string displayName,
        string propertyName,
        JObject propertyValue
    )
    {
        switch (propertyType)
        {
            case "array":
                return rtrWflSch_FormatArrayLaunchProperty(
                    displayName,
                    propertyName,
                    propertyValue
                );
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

    private JObject rtrWflSch_FormatArrayLaunchProperty(
        string displayName,
        string propertyName,
        JObject propertyValue
    )
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
                    ["items"] = rtrWflSch_FormatBasicProperty(
                        elementTypeString,
                        $"{displayName} Item",
                        $"{propertyName} Item"
                    )
                };
            }
        }
        return new JObject();
    }

    private JObject rtrWflSch_FormatDocumentArrayLaunchProperty(
        string displayName,
        string propertyName
    )
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

    private JObject rtrWflSch_FormatTableProperty(
        string displayName,
        string propertyName,
        JObject propertyValue
    )
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
                    itemsObject[column.Name] = rtrWflSch_FormatMonetaryAmountProperty(
                        columnDisplayName,
                        column.Name
                    );
                }
                else if (columnType == "duration")
                {
                    itemsObject[column.Name] = rtrWflSch_FormatDurationProperty(
                        columnDisplayName,
                        column.Name
                    );
                }
                else if (columnType == "address")
                {
                    itemsObject[column.Name] = rtrWflSch_FormatAddressProperty(
                        columnDisplayName,
                        column.Name
                    );
                }
                else
                {
                    JObject formattedColumn = rtrWflSch_FormatBasicProperty(
                        columnType,
                        columnDisplayName,
                        column.Name
                    );
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
            ["items"] = new JObject { ["type"] = "object", ["properties"] = itemsObject }
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
                ["lines"] = new JObject
                {
                    ["type"] = "array",
                    ["items"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = $"{displayName} Line",
                        ["x-ms-visibility"] = "important",
                        ["description"] = $"An address line of {displayName}."
                    }
                },
                ["locality"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "Locality",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The locality of {displayName}."
                },
                ["region"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "Region",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The region of {displayName}."
                },
                ["postcode"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "Postcode",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The postcode of {displayName}."
                },
                ["country"] = new JObject
                {
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
                ["amount"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Amount",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The amount of {displayName}."
                },
                ["currency"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "Currency",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The currency of {displayName}."
                }
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
                ["years"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Years",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The years of {displayName}."
                },
                ["months"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Months",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The months of {displayName}."
                },
                ["weeks"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Weeks",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The weeks of {displayName}."
                },
                ["days"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Days",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The days of {displayName}."
                }
            }
        };
    }

    private JObject rtrWflSch_FormatBasicProperty(
        string propertyType,
        string displayName,
        string propertyName
    )
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

    private JObject rtrWflSch_ParseSchemaArrayItem(
        string systemName,
        string displayName,
        string type,
        bool isReadOnly
    )
    {
        return new JObject
        {
            ["systemName"] = systemName,
            ["displayName"] = displayName,
            ["type"] = type,
            ["readOnly"] = isReadOnly
        };
    }

    private JObject rtrWflSch_ParseDocumentSchemaItem(
        string systemName,
        string displayName,
        bool isReadOnly
    )
    {
        return new JObject
        {
            ["systemName"] = systemName,
            ["displayName"] = displayName,
            ["readOnly"] = isReadOnly
        };
    }

    private JObject rtrWflSch_FormatProperty(
        string propertyType,
        string displayName,
        string propertyName,
        JObject propertyValue
    )
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

    private JObject rtrWflSch_FormatArrayProperty(
        string displayName,
        string propertyName,
        JObject propertyValue
    )
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
                    ["items"] = rtrWflSch_FormatBasicProperty(
                        elementTypeString,
                        $"{displayName} Item",
                        $"{propertyName} Item"
                    )
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
                    ["filename"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "File Name",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The name of the file."
                    },
                    ["download"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "Download Link",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The download link of the file."
                    },
                    ["key"] = new JObject
                    {
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
            var documentsAsArray = new JArray();

            foreach (var property in schema.Properties())
            {
                rtrWfl_ProcessSchemaProperty(property, formattedSchema, schemaAsArray);
            }

            body["formattedSchema"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = formattedSchema
            };
            body["schemaAsArray"] = schemaAsArray;

            // Transform documents from the existing workflow documents
            if (body["attributes"] is JObject attributes)
            {
                // Handle draft documents
                if (attributes["draft"] is JArray draftDocs && draftDocs.Any())
                {
                    var draftDocObject = new JObject
                    {
                        ["systemName"] = "draft",
                        ["displayName"] = "Draft Document",
                        ["readOnly"] = true,
                        ["versions"] = draftDocs
                    };
                    documentsAsArray.Add(draftDocObject);
                }

                // Handle signed documents
                if (attributes["signed"] is JObject signedDoc)
                {
                    var signedDocObject = new JObject
                    {
                        ["systemName"] = "signed",
                        ["displayName"] = "Signed Document",
                        ["readOnly"] = true,
                        ["versions"] = new JArray(signedDoc)
                    };
                    documentsAsArray.Add(signedDocObject);
                }

                // Handle sentSignaturePacket documents
                if (attributes["sentSignaturePacket"] is JArray signaturePacketDocs && signaturePacketDocs.Any())
                {
                    var signaturePacketObject = new JObject
                    {
                        ["systemName"] = "signaturePacket",
                        ["displayName"] = "Signature Packet",
                        ["readOnly"] = true,
                        ["versions"] = signaturePacketDocs
                    };
                    documentsAsArray.Add(signaturePacketObject);
                }
            }

            body["documentsAsArray"] = documentsAsArray;
            body["formattedAttributes"] = rtrWfl_FormatWorkflowAttributes(
                body["attributes"] as JObject,
                formattedSchema
            );
        }

        // Always ensure counterpartyName is present at the root.
        if (body["attributes"] is JObject workflowAttributes && workflowAttributes.ContainsKey("counterpartyName"))
        {
            body["counterpartyName"] = workflowAttributes["counterpartyName"];
        }
        else
        {
            body["counterpartyName"] = null;
        }

        return body;
    }

    private void rtrWfl_ProcessSchemaProperty(
        JProperty property,
        JObject formattedSchema,
        JArray schemaAsArray
    )
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

            JObject formattedProperty = rtrWfl_FormatPropertyByType(
                propertyType,
                displayName,
                property.Name,
                propertySchema
            );
            formattedProperty["readOnly"] = isReadOnly;
            formattedSchema[property.Name] = formattedProperty;

            schemaAsArray.Add(
                new JObject
                {
                    ["systemName"] = property.Name,
                    ["displayName"] = displayName,
                    ["type"] = propertyType,
                    ["readOnly"] = isReadOnly
                }
            );
        }
    }

    private JObject rtrWfl_FormatPropertyByType(
        string propertyType,
        string displayName,
        string propertyName,
        JObject propertySchema
    )
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

    private JObject rtrWfl_FormatArrayProperty(
        string displayName,
        string propertyName,
        JObject propertySchema
    )
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
                    ["items"] = rtrWfl_FormatBasicProperty(
                        elementTypeString,
                        $"{displayName} Item",
                        $"{propertyName} Item"
                    )
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
                    ["filename"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "File Name",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The name of the file."
                    },
                    ["version"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "Version",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The version of the file."
                    },
                    ["versionNumber"] = new JObject
                    {
                        ["type"] = "number",
                        ["title"] = "Version Number",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The version number of the file."
                    },
                    ["download"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "Download Link",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The download link of the file."
                    },
                    ["key"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "Key",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "The key of the file."
                    },
                    ["lastModified"] = new JObject
                    {
                        ["type"] = "object",
                        ["title"] = "Last Modified",
                        ["x-ms-visibility"] = "important",
                        ["description"] = "Information on when the file was last modified.",
                        ["properties"] = new JObject
                        {
                            ["timestamp"] = new JObject
                            {
                                ["type"] = "string",
                                ["format"] = "date-time",
                                ["title"] = "Timestamp",
                                ["x-ms-visibility"] = "important",
                                ["description"] = "The date when the file was last modified."
                            },
                            ["author"] = new JObject
                            {
                                ["type"] = "object",
                                ["title"] = "Author",
                                ["x-ms-visibility"] = "important",
                                ["description"] = "The author of the last modification.",
                                ["properties"] = new JObject
                                {
                                    ["displayName"] = new JObject
                                    {
                                        ["type"] = "string",
                                        ["title"] = "Display Name",
                                        ["x-ms-visibility"] = "important",
                                        ["description"] = "The display name of the author."
                                    },
                                    ["email"] = new JObject
                                    {
                                        ["type"] = "string",
                                        ["title"] = "Email",
                                        ["x-ms-visibility"] = "important",
                                        ["description"] = "The email of the author."
                                    },
                                    ["userId"] =
                                        new JObject
                                        {
                                            ["type"] = "string",
                                            ["title"] = "User ID",
                                            ["x-ms-visibility"] = "important",
                                            ["description"] = "The user ID of the author."
                                        }["type"] =
                                        new JObject
                                        {
                                            ["type"] = "string",
                                            ["title"] = "Type",
                                            ["x-ms-visibility"] = "important",
                                            ["description"] = "The type of the file."
                                        }["companyName"] =
                                            new JObject
                                            {
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

    private JObject rtrWfl_FormatTableProperty(
        string displayName,
        string propertyName,
        JObject propertySchema
    )
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

                JObject formattedColumn = rtrWfl_FormatBasicProperty(
                    columnType,
                    columnDisplayName,
                    column.Name
                );
                itemsObject[column.Name] = formattedColumn;
            }
        }

        return new JObject
        {
            ["type"] = "array",
            ["title"] = displayName,
            ["description"] = $"The {displayName}.",
            ["x-ms-visibility"] = "important",
            ["items"] = new JObject { ["type"] = "object", ["properties"] = itemsObject }
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
                ["lines"] = new JObject
                {
                    ["type"] = "array",
                    ["items"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = $"{displayName} Line",
                        ["x-ms-visibility"] = "important",
                        ["description"] = $"An address line of {displayName}."
                    }
                },
                ["locality"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "Locality",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The locality of {displayName}."
                },
                ["region"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "Region",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The region of {displayName}."
                },
                ["postcode"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "Postcode",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The postcode of {displayName}."
                },
                ["country"] = new JObject
                {
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
                ["amount"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Amount",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The amount of {displayName}."
                },
                ["currency"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "Currency",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The currency of {displayName}."
                }
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
                ["years"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Years",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The years of {displayName}."
                },
                ["months"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Months",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The months of {displayName}."
                },
                ["weeks"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Weeks",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The weeks of {displayName}."
                },
                ["days"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Days",
                    ["x-ms-visibility"] = "important",
                    ["description"] = $"The days of {displayName}."
                }
            }
        };
    }

    private JObject rtrWfl_FormatBasicProperty(
        string propertyType,
        string displayName,
        string propertyName
    )
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

    private JObject rtrWfl_CreateSchemaArrayItem(
        string systemName,
        string displayName,
        string type,
        bool isReadOnly
    )
    {
        return new JObject
        {
            ["systemName"] = systemName,
            ["displayName"] = displayName,
            ["type"] = type,
            ["readOnly"] = isReadOnly
        };
    }

    private JObject rtrWfl_CreateDocumentSchemaItem(
        string systemName,
        string displayName,
        bool isReadOnly
    )
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
                formattedAttributes[property.Name] = rtrWfl_FormatAttributeValue(
                    attributeValue,
                    propertySchema
                );
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
                    formattedObject[property.Name] = rtrWfl_FormatAttributeValue(
                        objectValue[property.Name],
                        property.Value as JObject
                    );
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
            var content = await this.Context.Request.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);
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
                            exists =
                                propObj["type"] != null
                                && (
                                    propObj["resolvesTo"] == null
                                    || propObj["resolvesTo"].Type == JTokenType.Null
                                );
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
                            Content = CreateJsonContent(
                                new JObject
                                {
                                    ["error"] = new JObject
                                    {
                                        ["message"] = $"Property '{item}' not found"
                                    }
                                }.ToString()
                            )
                        };
                    }
                }
            }

            // Transform the metadata
            var transformedData = rtrRcdSch_TransformRetrieveRecordSchemas(
                metadata,
                propertiesQuery
            );

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
                Content = CreateJsonContent(
                    new JObject { ["error"] = new JObject { ["message"] = ex.Message } }.ToString()
                )
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
                if (
                    propObj != null
                    && (
                        propObj["resolvesTo"] == null
                        || propObj["resolvesTo"].Type == JTokenType.Null
                    )
                )
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
                        var displayName =
                            attachmentObj["displayName"]?.ToString() ?? attachment.Name;
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
                    var description =
                        propertyObj["description"]?.ToString() ?? $"The {displayName}.";

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
                                        ["description"] =
                                            $"The monetary amount value for {displayName}."
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
                                        ["description"] =
                                            $"The ISO 8601 duration representation for {displayName}."
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
                    ["description"] =
                        "The record schema formatted for compatibility with the OpenAPI standard.",
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
                        ["recordAttachments"] = rtrRcdSch_CreateAttachmentSchema(
                            formattedAttachments
                        )
                    }
                };
            }
            else
            {
                body["formattedSchema"] = new JObject
                {
                    ["type"] = "object",
                    ["description"] =
                        "The record schema formatted for compatibility with the OpenAPI standard.",
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
                    ["displayName"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "Display Name",
                        ["description"] = $"The display name of the {displayName} clause."
                    },
                    ["description"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "Description",
                        ["description"] = $"The description of the {displayName} clause."
                    },
                    ["clauseText"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "Clause Text",
                        ["description"] = $"The text content of the {displayName} clause."
                    },
                    ["source"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "Source",
                        ["description"] = $"The source of the {displayName} clause."
                    },
                    ["clauseType"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "Clause Type",
                        ["description"] = $"The type of the {displayName} clause."
                    },
                    ["languagePosition"] = new JObject
                    {
                        ["type"] = "object",
                        ["properties"] = new JObject
                        {
                            ["type"] = new JObject
                            {
                                ["type"] = "string",
                                ["title"] = "Type",
                                ["description"] =
                                    $"The language position type of the {displayName} clause."
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
                    ["filename"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "Filename",
                        ["description"] = $"The filename of the {displayName} attachment."
                    },
                    ["contentType"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "Content Type",
                        ["description"] = $"The content type of the {displayName} attachment."
                    },
                    ["href"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "Download URL",
                        ["description"] = $"The download URL for the {displayName} attachment."
                    },
                    ["key"] = new JObject
                    {
                        ["type"] = "string",
                        ["title"] = "Key",
                        ["description"] =
                            $"The unique key identifier for the {displayName} attachment."
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
            var content = await this.Context.Request.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);
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
                            exists =
                                propObj["type"] != null
                                && (
                                    propObj["resolvesTo"] == null
                                    || propObj["resolvesTo"].Type == JTokenType.Null
                                );
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
                            Content = CreateJsonContent(
                                new JObject
                                {
                                    ["error"] = new JObject
                                    {
                                        ["message"] = $"Property '{item}' not found"
                                    }
                                }.ToString()
                            )
                        };
                    }
                }
            }

            var responseBody = metadata;
            var transformedBody = lstAllRcd_TransformListAllRecordsResponse(
                responseBody,
                propertiesQuery
            );

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = CreateJsonContent(transformedBody.ToString())
            };
        }
        catch (Exception ex)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = CreateJsonContent(
                    new JObject { ["error"] = new JObject { ["message"] = ex.Message } }.ToString()
                )
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
                if (
                    originalProperties != null
                    && originalProperties.ContainsKey("counterpartyName")
                    && originalProperties["counterpartyName"] is JObject counterpartyProp
                    && counterpartyProp["value"] != null
                )
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
                        attachmentsArray.Add(
                            new JObject
                            {
                                ["displayName"] = attachment.Value["displayName"] ?? attachment.Key,
                                ["name"] = attachment.Key,
                                ["key"] = attachment.Key // Add the key property
                            }
                        );
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
                        if (
                            originalProperties != null
                            && originalProperties.ContainsKey(propertyName)
                        )
                        {
                            var property = originalProperties[propertyName] as JObject;
                            if (property != null)
                            {
                                var propertyType = property["type"]?.ToString().ToLower();

                                if (propertyType == "clause")
                                {
                                    // Handle clause properties
                                    recordClauses[propertyName] = lstAllRcd_FormatClauseProperty(
                                        propertyName,
                                        property
                                    );
                                }
                                else
                                {
                                    // Handle regular properties
                                    recordProperties[propertyName] = lstAllRcd_FormatPropertyValue(
                                        property
                                    );
                                }
                            }
                        }

                        // Handle attachments separately
                        if (
                            recordAttachmentsObj != null
                            && recordAttachmentsObj.ContainsKey(propertyName)
                        )
                        {
                            var attachment = recordAttachmentsObj[propertyName] as JObject;
                            recordAttachments[propertyName] = new JObject
                            {
                                ["filename"] = attachment["filename"],
                                ["contentType"] = attachment["contentType"],
                                ["href"] = attachment["href"],
                                ["displayName"] = attachment["displayName"] ?? propertyName,
                                ["key"] = propertyName // Add the key property
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
        var result = new JObject { ["isoDuration"] = isoDuration };

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

    // Main transformation function that transforms the API response body.
    // It formats the record schema and properties, formats attachments if available,
    // and then creates two additional root-level properties:
    // 1. propertiesByTitle – an object mapping each property’s display title to its value.
    // 2. propertiesAsArray – an array of property objects with details (title, property name, description, type, and value).
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

        // Create the new properties using helper functions.
        body["propertiesByTitle"] = rtrRcd_CreatePropertiesByTitle(formattedProperties["recordProperties"] as JObject);

        // Pass the formatted schema's properties so we can get type information.
        JObject recordSchemaProperties = (formattedSchema["recordProperties"]?["properties"] as JObject) ?? new JObject();
        body["propertiesAsArray"] = rtrRcd_CreatePropertiesAsArray(formattedProperties["recordProperties"] as JObject, recordSchemaProperties);

        body["formattedSchema"] = new JObject
        {
            ["type"] = "object",
            ["description"] = "OpenAPI Formatted Properties",
            ["x-ms-visibility"] = "important",
            ["properties"] = formattedSchema
        };
        body["formattedProperties"] = formattedProperties;

        return body;
    }

    // Creates an object (JObject) where each key is the display title of a property
    // (retrieved via the record schema) and the value is the transformed property value.
    private JObject rtrRcd_CreatePropertiesByTitle(JObject recordProperties)
    {
        var propertiesByTitle = new JObject();
        if (recordProperties != null)
        {
            foreach (var prop in recordProperties.Properties())
            {
                // Retrieve displayName from schema info (or use the property name if not found).
                var schemaProperty = rtrRcd_GetRecordSchemaProperty(prop.Name);
                string title = schemaProperty?["displayName"]?.ToString() ?? prop.Name;
                propertiesByTitle[title] = prop.Value;
            }
        }
        return propertiesByTitle;
    }

    // Creates an array (JArray) where each element is an object containing details for a property:
    // title, property name, description, type (from the formatted schema), and its value.
    private JArray rtrRcd_CreatePropertiesAsArray(JObject recordProperties, JObject recordSchemaProperties)
    {
        var propertiesAsArray = new JArray();
        if (recordProperties != null)
        {
            foreach (var prop in recordProperties.Properties())
            {
                // Retrieve display name and description from schema.
                var schemaProperty = rtrRcd_GetRecordSchemaProperty(prop.Name);
                string title = schemaProperty?["displayName"]?.ToString() ?? prop.Name;
                string description = schemaProperty?["description"]?.ToString() ?? $"The {prop.Name}.";
                string propertyType = "string";
                if (recordSchemaProperties != null && recordSchemaProperties[prop.Name]?["type"] != null)
                {
                    propertyType = recordSchemaProperties[prop.Name]["type"].ToString();
                }

                JObject propertyArrayItem = new JObject
                {
                    ["title"] = title,
                    ["property"] = prop.Name,
                    ["description"] = description,
                    ["type"] = propertyType,
                    ["value"] = prop.Value
                };
                propertiesAsArray.Add(propertyArrayItem);
            }
        }
        return propertiesAsArray;
    }

    // Formats the record properties schema by iterating over each property that is not a clause.
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

    // Formats the record clauses schema by iterating over each property that is a clause.
    private JObject rtrRcd_FormatRecordClausesSchema(JObject properties)
    {
        var recordClausesSchema = new JObject();

        foreach (var property in properties.Properties())
        {
            if (rtrRcd_IsClauseProperty(property))
            {
                var clauseObject = rtrRcd_FormatClauseProperty(property);
                recordClausesSchema[property.Name] = rtrRcd_CreateClauseSchema(
                    clauseObject["displayName"].ToString(),
                    clauseObject["description"].ToString()
                );
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

    // Transforms the record properties by formatting each property value that is not a clause.
    private JObject rtrRcd_FormatRecordProperties(JObject properties)
    {
        var transformedProperties = new JObject();

        foreach (var property in properties.Properties())
        {
            if (!rtrRcd_IsClauseProperty(property))
            {
                var propertySchema = rtrRcd_GetRecordSchemaProperty(property.Name);
                transformedProperties[property.Name] = rtrRcd_FormatRecordPropertyValue(
                    property.Value as JObject,
                    propertySchema
                );
            }
        }

        return transformedProperties;
    }

    // Transforms the record clauses by formatting each clause property.
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

    // Determines if a property is a clause property based on its "type" field.
    private bool rtrRcd_IsClauseProperty(JProperty property)
    {
        var propertyValue = property.Value as JObject;
        return propertyValue != null && propertyValue["type"]?.ToString().ToLower() == "clause";
    }

    // Parses a record schema property and adds it to the formatted schema.
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

            JObject formattedProperty = rtrRcd_ParseRecordPropertySchemaByType(
                propertyType,
                displayName,
                propertyName,
                description
            );
            formattedSchema[propertyName] = formattedProperty;
        }
    }

    // Determines which formatting function to use based on the property type.
    private JObject rtrRcd_ParseRecordPropertySchemaByType(string propertyType, string displayName, string propertyName, string description)
    {
        switch (propertyType)
        {
            case "monetary_amount":
                return rtrRcd_FormatMonetaryAmountPropertySchema(displayName, propertyName);
            case "duration":
                return rtrRcd_FormatDurationPropertySchema(displayName, propertyName, description);
            // Address and all other types are treated as basic properties.
            default:
                return rtrRcd_FormatBasicPropertySchema(propertyType, displayName, propertyName);
        }
    }

    // Formats the schema for an address property.
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

    // Formats the schema for a monetary amount property.
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
                ["amount"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Amount",
                    ["description"] = $"The amount of the {displayName}."
                },
                ["currency"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "Currency",
                    ["description"] = $"The currency of the {displayName}."
                }
            }
        };
    }

    // Formats the schema for a duration property.
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
                ["isoDuration"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "ISO Duration",
                    ["description"] = $"The ISO 8601 duration representation of the {displayName}."
                },
                ["years"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Years",
                    ["description"] = $"The years of the {displayName}."
                },
                ["months"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Months",
                    ["description"] = $"The months of the {displayName}."
                },
                ["weeks"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Weeks",
                    ["description"] = $"The weeks of the {displayName}."
                },
                ["days"] = new JObject
                {
                    ["type"] = "number",
                    ["title"] = "Days",
                    ["description"] = $"The days of the {displayName}."
                }
            }
        };
    }

    // Formats the schema for basic property types.
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
            case "address": // Address is treated as string.
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

    // Retrieves the record schema property information from the global recordSchemaInfo.
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

    // Formats the value of a record property based on its type.
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

    // Formats the value for a monetary amount property.
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

    // Formats the value for a duration property.
    private JObject rtrRcd_FormatDurationPropertyValue(string isoDuration)
    {
        var result = new JObject { ["isoDuration"] = isoDuration };

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

    // Parses a duration component from a string, returning 0 if empty.
    private int rtrRcd_ParseDurationComponent(string value)
    {
        return string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
    }

    // Formats a clause property by extracting its clause details and adding display information.
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

    // Creates the schema for a clause property.
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
                ["displayName"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "Display Name",
                    ["description"] = "The display name of the clause."
                },
                ["description"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "Description",
                    ["description"] = "The description of the clause."
                },
                ["clauseText"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "Clause Text",
                    ["description"] = "The text content of the clause."
                },
                ["source"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "Source",
                    ["description"] = "The source of the clause.",
                    ["x-ms-visibility"] = "internal"
                },
                ["clauseType"] = new JObject
                {
                    ["type"] = "string",
                    ["title"] = "Clause Type",
                    ["description"] = "The type of the clause.",
                    ["x-ms-visibility"] = "internal"
                },
                ["languagePosition"] = new JObject
                {
                    ["type"] = "object",
                    ["title"] = "Language Position",
                    ["description"] = "The language position of the clause.",
                    ["x-ms-visibility"] = "internal",
                    ["properties"] = new JObject
                    {
                        ["type"] = new JObject
                        {
                            ["type"] = "string",
                            ["title"] = "Type",
                            ["description"] = "The type of language position."
                        }
                    }
                }
            }
        };
    }

    // Creates the attachment schema for the provided attachments.
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

    // Formats the attachments values.
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

    // Creates an array representation of the attachments.
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

                attachmentsArray.Add(
                    new JObject
                    {
                        ["name"] = attachmentName,
                        ["displayName"] = displayName,
                        ["key"] = attachmentName
                    }
                );
            }
        }

        return attachmentsArray;
    }

    // Asynchronously retrieves record schema information from the API and stores it in recordSchemaInfo.
    private async Task rtrRcd_RetrieveRecordSchemaInformation()
    {
        var baseUrl = this.Context.Request.RequestUri.GetLeftPart(UriPartial.Authority);
        var schemaUrl = new Uri(new Uri(baseUrl), "/public/api/v1/records/metadata");

        var request = new HttpRequestMessage(HttpMethod.Get, schemaUrl);

        foreach (var header in this.Context.Request.Headers)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        var response = await this.Context
            .SendAsync(request, this.CancellationToken)
            .ConfigureAwait(false);

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
                    .Select(
                        attachment =>
                        {
                            string downloadUrl = attachment["download"].ToString();
                            string key = rtrEml_ExtractKeyFromDownloadUrl(downloadUrl);
                            attachment["key"] = key;
                            return attachment;
                        }
                    )
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