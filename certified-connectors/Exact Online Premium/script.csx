using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (this.Context.Request.RequestUri == null) { return ErrorMessage("Missing Request URI", HttpStatusCode.BadRequest); }

        var baseUriPath = this.Context.Request.RequestUri.GetBaseUriPath();
        if (string.IsNullOrWhiteSpace(baseUriPath)) { return ErrorMessage("Cannot find Request URI", HttpStatusCode.InternalServerError); }

        var baseUri = new Uri(baseUriPath);
        string operationId = this.Context.OperationId.ToLowerInvariant();

        return operationId switch
        {
            "getcompanies" or "getdivisions" => await GetDivisionsAsync(this.Context, baseUri, this.CancellationToken),
            "getschema" => GetSchema(this.Context),
            "getsupportedentitytypes" => GetEntityTypes(),
            "getuserdata" => await GetUserDataAsync(this.Context, baseUri, this.CancellationToken),
            "getvalues" => await GetValuesAsync(this.Context),
            "getwebhooksubscriptions" => await GetExistingSubscriptionsAsync(this.Context, baseUri, this.CancellationToken),
            "subscribesalesorderchanged" or "subscribeprojectchanged" or "subscribeopportunitychanged" or "subscribeaccountchanged"
                or "subscribeentitychanged" => await CreateSubscriptionAsync(this.Context, baseUri, operationId, this.CancellationToken),
            "unsubscribe" => await DeleteSubscriptionAsync(this.Context, baseUri, this.CancellationToken),
            _ => ErrorMessage("Operation not supported", HttpStatusCode.BadRequest)
        };
    }

    private static HttpResponseMessage GetEntityTypes() => CreateJsonResponse(new JArray
        (
            new JObject { ["Name"] = "Account", ["Topic"] = "PowerAutomateAccounts" },
            new JObject { ["Name"] = "Opportunity", ["Topic"] = "PowerAutomateOpportunities" },
            new JObject { ["Name"] = "Project", ["Topic"] = "PowerAutomateProjects" },
            new JObject { ["Name"] = "Sales Order", ["Topic"] = "PowerAutomateSalesOrders" }
        ));

    private static async Task<HttpResponseMessage> GetUserDataAsync(IScriptContext context, Uri baseUri, CancellationToken cancellationToken)
        => await context.SendAndReturnFirstResultAsync(HttpMethod.Get, baseUri.SetToUserDataUriWithQuery(), cancellationToken);

    private static async Task<string> GetUserDivisionAsync(IScriptContext context, Uri baseUri, CancellationToken cancellationToken)
        => await context.SendAndReturnPropertyAsync(HttpMethod.Get, baseUri.SetToUserDataUriWithQuery(), "CurrentDivision", cancellationToken);

    private static async Task<string> GetUserIDAsync(IScriptContext context, Uri baseUri, CancellationToken cancellationToken)
        => await context.SendAndReturnPropertyAsync(HttpMethod.Get, baseUri.SetToUserDataUriWithQuery(), "UserID", cancellationToken);

    private static async Task<HttpResponseMessage> GetDivisionsAsync(IScriptContext context, Uri baseUri, CancellationToken cancellationToken)
    {
        string divisionId = await GetUserDivisionAsync(context, baseUri, cancellationToken);
        if (string.IsNullOrWhiteSpace(divisionId))
        {
            return ErrorMessage("Missing data in user profile", HttpStatusCode.InternalServerError);
        }

        return await context.SendAndReturnResultsAsync(HttpMethod.Get, baseUri.SetToDivisionsUriWithQuery(divisionId), cancellationToken);
    }

    private static async Task<HttpResponseMessage> GetValuesAsync(IScriptContext context, string key = "")
    {
        string jsonText = await context.Request.Content.ReadAsStringOrDefaultAsync();
        if (string.IsNullOrWhiteSpace(jsonText))
            return ErrorMessage("Empty payload");

        var token = JToken.Parse(jsonText);
        if (token == null)
            return ErrorMessage("Unable to read payload");
        if (token.Type == JTokenType.String)
            token = JToken.Parse(token.Value<string>());
        if (token.Type != JTokenType.Object)
            return ErrorMessage("Payload is not a valid json object");

        var obj = token as JObject;
        if (obj == null)
            return ErrorMessage("Unable to read json from payload");

        if (string.IsNullOrWhiteSpace(key))
        {
            key = obj.GetString("ValueType");
        }
        if (string.IsNullOrWhiteSpace(key))
        {
            var queryData = context.Request.GetQueryData();
            if (queryData.ContainsKey("valuetype"))
            {
                key = queryData["valuetype"];
            }
        }
        if (string.IsNullOrWhiteSpace(key))
            return ErrorMessage("Unable to parse value type");

        var valueType = key.ToLowerInvariant();
        string emptyJson = valueType switch
        {
            "oldvalue" or "newvalue" => "{}",
            "table" => "[]",
            _ => ""
        };

        if (string.IsNullOrWhiteSpace(emptyJson))
            return ErrorMessage($"Invalid value type: {key}");

        var changes = obj.Property("Changes");
        if (changes == null || changes.Value.Type != JTokenType.Array || changes.Value is not JArray)
        {
            changes = obj.GetString("Payload").AsJObject()?.Property("Changes");
        }
        if (changes == null || changes.Value.Type != JTokenType.Array || changes.Value is not JArray arr)
            return CreateJsonResponse(emptyJson)
                .WithHeader("x-requested-valuetype", valueType);

        arr.Add(new JObject { ["name"] = "BusinessComponent", ["oldValue"] = obj.GetString("BusinessComponent"), ["newValue"] = obj.GetString("BusinessComponent") });
        arr.Add(new JObject { ["name"] = "EntityEvent", ["oldValue"] = obj.GetString("EntityEvent"), ["newValue"] = obj.GetString("EntityEvent") });

        if (valueType == "table")
            return CreateJsonResponse(arr)
                .WithHeader("x-requested-valuetype", valueType);

        return CreateJsonResponse(new JObject(arr.OfType<JObject>()
            .Where(o => o.ContainsKey("name") && o.ContainsKey(key))
            .Select(o => new JProperty(o.GetString("name"), o.Property(key)?.Value))
            .Where(p => !string.IsNullOrWhiteSpace(p.Name))
            )).WithHeader("x-requested-valuetype", valueType);
    }

    private static async Task<HttpResponseMessage> GetExistingSubscriptionsAsync(IScriptContext context, Uri baseUri, CancellationToken cancellationToken)
    {
        string divisionId = await GetUserDivisionAsync(context, baseUri, cancellationToken);
        if (string.IsNullOrWhiteSpace(divisionId))
        {
            return ErrorMessage("Missing data in user profile", HttpStatusCode.InternalServerError);
        }

        return await context.SendAndReturnResultsAsync(HttpMethod.Get, baseUri.SetToSubscriptionUri(divisionId), cancellationToken);
    }

    private static async Task<JObject> GetExistingSubscriptionAsync(IScriptContext context, Uri baseUri, string userId, string divisionId, string topicName, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await context.SendAsync(HttpMethod.Get, baseUri.SetToSubscriptionUri(divisionId), cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var body = await response.Content.GetJObjectAsync();
            if (body != null)
            {
                var subscription = body.GetResults()?.FirstOrDefault(r => r.Compare("UserID", userId) && r.Compare("Division", divisionId) && r.Compare("Topic", topicName));
                if (subscription != null)
                {
                    return (JObject)subscription;
                }
            }
        }

        return new JObject();
    }

    private static async Task<HttpResponseMessage> CreateSubscriptionAsync(IScriptContext context, Uri baseUri, string operationId, CancellationToken cancellationToken)
    {
        JObject? incomingBody = await context.Request.Content.GetJObjectAsync();

        string divisionId = GetCompanyOrDivisionFromBody(incomingBody);
        if (string.IsNullOrWhiteSpace(divisionId)) { return ErrorMessage("Required - Company (Division)", HttpStatusCode.BadRequest); }

        string topicName = GetTopicNameFromOperationOrBody(operationId, incomingBody);
        if (string.IsNullOrWhiteSpace(topicName)) { return ErrorMessage("Required - Operation or Topic name", HttpStatusCode.BadRequest); }

        string userId = await GetUserIDAsync(context, baseUri, cancellationToken);
        if (string.IsNullOrWhiteSpace(userId)) { return ErrorMessage("Missing User ID", HttpStatusCode.InternalServerError); }

        var existing = await GetExistingSubscriptionAsync(context, baseUri, userId, divisionId, topicName, cancellationToken);
        if (existing.GetString("Topic").Equals(topicName, StringComparison.InvariantCultureIgnoreCase))
        {
            return CreateFakeCreatedResponse(existing, baseUri);
        }

        return await CreateNewSubscriptionAsync(context, baseUri, divisionId, topicName, incomingBody, cancellationToken);
    }

    private static HttpResponseMessage CreateFakeCreatedResponse(JObject? existing, Uri baseUri)
    {
        var location = new Uri(existing?["__metadata"]?["uri"]?.GetString());
        var newLocation = baseUri.SetToSubscriptionUriWithGuid(location.GetDivisionFromUri(), location.GetGuidFromUri());

        var result = CreateJsonResponse(existing, HttpStatusCode.Created);
        result.Headers.TryAddWithoutValidation("Location", newLocation.ToString());
        result.Content?.Headers?.TryAddWithoutValidation("x-is-existing-subscription", "true");
        return result;
    }

    private static async Task<HttpResponseMessage> CreateNewSubscriptionAsync(IScriptContext context, Uri baseUri, string divisionId, string topicName, JObject? incomingBody, CancellationToken cancellationToken)
    {
        string bodyWithTopic = incomingBody.WithProperty("Topic", topicName)?.ToString() ?? string.Empty;

        var request = context.Request.Copy(HttpMethod.Post, baseUri.SetToSubscriptionUri(divisionId), bodyWithTopic);
        var response = await context.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode) { return response; }

        if (response.Headers.Location == null) { return ErrorMessage("Missing header: Location", HttpStatusCode.InternalServerError); }

        response.StatusCode = HttpStatusCode.Created;

        Uri location = response.Headers.Location;
        response.Headers.Location = baseUri.SetToSubscriptionUriWithGuid(location.GetDivisionFromUri(), location.GetGuidFromUri());

        return response;
    }

    private static async Task<HttpResponseMessage> DeleteSubscriptionAsync(IScriptContext context, Uri baseUri, CancellationToken cancellationToken)
    {
        Uri? uri = context.Request.RequestUri;
        if (uri == null)
        {
            return ErrorMessage("Invalid Request URI", HttpStatusCode.BadRequest);
        }

        context.Request.RequestUri = baseUri.SetToSubscriptionUriWithGuid(uri.GetDivisionFromUri(), uri.GetGuidFromUri());
        context.Request.Headers.TryAddWithoutValidation("Accept", "application/json");

        var response = await context.SendAsync(context.Request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.NoContent)
        {
            response.StatusCode = HttpStatusCode.OK;
        }
        return response;
    }

    private static HttpResponseMessage GetSchema(IScriptContext context)
    {
        var queryData = context.Request.GetQueryData();
        string? entityType;
        string? valueType = null;
        if (!queryData.TryGetValue("entitytype", out entityType)
            || !queryData.TryGetValue("valuetype", out valueType)
            || string.IsNullOrWhiteSpace(entityType)
            || string.IsNullOrWhiteSpace(valueType)
            )
            return ErrorMessage("Combination entity type / value type not found", HttpStatusCode.BadRequest)
                .WithHeader("x-requested-entitytype", entityType ?? "")
                .WithHeader("x-requested-valuetype", valueType ?? "");

        return CreateJsonResponse(valueType.ToLowerInvariant() switch
        {
            "table" => SchemaHelper.GetTableSchemaObject(),
            "oldvalue" or "newvalue" => SchemaHelper.GetValueSchemaObject(entityType),
            _ => "{}"
        }).WithHeader("x-requested-entitytype", entityType)
        .WithHeader("x-requested-valuetype", valueType);
    }

    private static string GetTopicNameFromOperationOrBody(string operationId, JObject? incomingBody)
    {
        string topicName = operationId.ToLowerInvariant() switch
        {
            "subscribesalesorderchanged" => "PowerAutomateSalesOrders",
            "subscribeprojectchanged" => "PowerAutomateProjects",
            "subscribeopportunitychanged" => "PowerAutomateOpportunities",
            "subscribeaccountchanged" => "PowerAutomateAccounts",
            _ => string.Empty,
        };

        string entityType = incomingBody.GetString("EntityType");
        incomingBody?.Remove("EntityType");
        return string.IsNullOrWhiteSpace(topicName) ? entityType : topicName;
    }

    private static string GetCompanyOrDivisionFromBody(JObject? incomingBody)
    {
        string companyId = incomingBody.GetString("Company");
        string divisionId = incomingBody.GetString("Division");
        incomingBody?.Remove("Company");
        incomingBody?.Remove("Division");
        return string.IsNullOrWhiteSpace(companyId) ? divisionId : companyId;
    }

    private static HttpResponseMessage CreateJsonResponse(string jsonText, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var response = new HttpResponseMessage(statusCode);
        if (!string.IsNullOrEmpty(jsonText))
        {
            response.Content = CreateJsonContent(jsonText);
        }
        return response;
    }

    private static HttpResponseMessage CreateJsonResponse(JToken? token, HttpStatusCode statusCode = HttpStatusCode.OK)
        => CreateJsonResponse(token.GetString(), statusCode);

    private static HttpResponseMessage ErrorMessage(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => CreateJsonResponse($"{{\"error\":\"{errorMessage}\"}}", statusCode);

    private static HttpResponseMessage ErrorMessage(string errorMessage, string incomingMessage) => CreateJsonResponse($"{{\"error\":\"{errorMessage}\",\"message\":\"{incomingMessage}\"}}", HttpStatusCode.OK);

    private static HttpResponseMessage EmptyJsonObject() => CreateJsonResponse("{}", HttpStatusCode.OK);
}

public static partial class ScriptExtensions
{
    private const string UserDataApiPath = "current/Me";
    private const string UserDataFields = "CurrentDivision,UserID";
    private const string DivisionsApiPath = "system/Divisions";
    private const string DivisionFields = "Code,Description";
    private const string SubscriptionApiPath = "webhooks/WebhookSubscriptions";
    private const string BaseUriRx = @"(https?://.*/api/v[0-9]+/)";
    private const string DivisionUriRx = @"https?://.*/api/v[0-9]+/([0-9]+)/";
    private const string GuidUriRx = @"\(guid'([0-9A-Fa-f]{8}-(?:[0-9A-Fa-f]{4}-){3}[0-9A-Fa-f]{12})'\)";

    public static JObject? AsJObject(this string body) => (string.IsNullOrWhiteSpace(body) ? null : JToken.Parse(body)) as JObject;
    public static string GetString(this JToken? token) => token?.ToString() ?? string.Empty;
    public static string GetString(this JToken? token, string key) => token?.Value<string>(key) ?? string.Empty;
    public static JArray GetResults(this JObject? body) => (JArray?)body?["d"]?["results"] ?? new JArray();
    public static JObject? GetFirstResult(this JObject? body) => body?["d"]?["results"]?[0] as JObject;
    public static JObject? WithProperty(this JObject? body, string propName, string value)
    {
        if (body == null) return null;

        body[propName] = value;
        return body;
    }
    public static JObject? WithoutProperty(this JObject? body, string propName)
    {
        if (body == null) return null;

        body.Remove(propName);
        return body;
    }
    public static JArray? WithoutProperty(this JArray? body, string propName)
    {
        if (body == null) return null;

        foreach (JObject obj in body)
        {
            obj.Remove(propName);
        }

        return body;
    }
    public static bool Compare(this JToken? t, string propName, string compare)
        => (t != null)
        && ((string.IsNullOrWhiteSpace(t.Value<string>(propName)) && string.IsNullOrWhiteSpace(compare))
            || compare.Equals(t.Value<string>(propName), StringComparison.InvariantCultureIgnoreCase)
        );
    public static string GetBaseUriPath(this Uri? uri) => Regex.Match(uri?.AbsoluteUri, BaseUriRx)?.Value ?? string.Empty;
    public static string GetDivisionFromUri(this Uri? uri) => Regex.Match(uri?.AbsoluteUri, DivisionUriRx)?.Groups?[1]?.Value ?? string.Empty;
    public static string GetGuidFromUri(this Uri? uri) => Regex.Match(uri?.AbsoluteUri, GuidUriRx)?.Groups?[1]?.Value ?? string.Empty;
    public static Uri Rewrite(this Uri uri, string newPath)
    {
        if (newPath.StartsWith("http://") || newPath.StartsWith("https://"))
            return new Uri(newPath);

        return new Uri(uri, newPath.StartsWith("/") ? newPath.Substring(1) : newPath);
    }
    public static Uri SetToUserDataUriWithQuery(this Uri baseUri) => baseUri.Rewrite($"{UserDataApiPath}?$select={UserDataFields}");
    public static Uri SetToDivisionsUriWithQuery(this Uri baseUri, string divisionId) => baseUri.Rewrite($"{divisionId}/{DivisionsApiPath}?$select={DivisionFields}");
    public static Uri SetToSubscriptionUri(this Uri baseUri, string divisionId) => baseUri.Rewrite($"{divisionId}/{SubscriptionApiPath}");
    public static Uri SetToSubscriptionUriWithGuid(this Uri baseUri, string divisionId, string subscriptionId) => baseUri.Rewrite($"{divisionId}/{SubscriptionApiPath}(guid'{subscriptionId}')");
    public static async Task<string> ReadAsStringOrDefaultAsync(this HttpContent? content) => content == null ? string.Empty : await content.ReadAsStringAsync();
    public static async Task<JObject?> GetJObjectAsync(this HttpContent? content) => (await content.ReadAsStringOrDefaultAsync()).AsJObject();
    public static async Task<JObject> GetJObjectOrDefaultAsync(this HttpContent? content) => (await content.ReadAsStringOrDefaultAsync()).AsJObject() ?? new JObject();

    public static Dictionary<string, string> GetQueryData(this HttpRequestMessage request)
    {
        var query = request.RequestUri?.Query;
        var result = new Dictionary<string, string>();
        if (!string.IsNullOrWhiteSpace(query))
        {
            query = query.StartsWith("?") ? query.Substring(1) : query;
            result = query.Split('&').Select(q => q.Split('=').Take(2)).ToDictionary(q => q.First().ToLowerInvariant(), q => q.Last());
        }
        return result;
    }

    public static HttpRequestMessage Copy(this HttpRequestMessage request, HttpMethod method, Uri uri, string messageText = "")
    {
        HttpRequestMessage newRequest = new(method, uri) { Version = request.Version };

        if (!string.IsNullOrWhiteSpace(messageText))
        {
            newRequest.Content = ScriptBase.CreateJsonContent(messageText);
        }

        var auth = request.Headers.Authorization;
        if (auth != null)
        {
            newRequest.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, auth.Parameter);
        }

        newRequest.Headers.Accept.Clear();
        newRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return newRequest;
    }

    public static HttpResponseMessage WithHeader(this HttpResponseMessage message, string name, string value)
    {
        message.Headers.Add(name, value);
        return message;
    }

    public static async Task<HttpResponseMessage> SendAsync(this IScriptContext context, HttpMethod method, Uri uri, CancellationToken cancellationToken)
        => await context.SendAsync(context.Request.Copy(method, uri), cancellationToken);
    public static async Task<HttpResponseMessage> SendAndReturnResultsAsync(this IScriptContext context, HttpMethod method, Uri uri, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await context.SendAsync(method, uri, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            string responseText = (await response.Content.GetJObjectOrDefaultAsync()).GetResults().WithoutProperty("__metadata").GetString();
            response.Content = ScriptBase.CreateJsonContent(responseText);
        }

        return response;
    }
    public static async Task<HttpResponseMessage> SendAndReturnFirstResultAsync(this IScriptContext context, HttpMethod method, Uri uri, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await context.SendAsync(method, uri, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            string responseText = (await response.Content.GetJObjectOrDefaultAsync()).GetFirstResult().WithoutProperty("__metadata").GetString();
            response.Content = ScriptBase.CreateJsonContent(responseText);
        }

        return response;
    }
    public static async Task<string> SendAndReturnPropertyAsync(this IScriptContext context, HttpMethod method, Uri uri, string propertyName, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await context.SendAsync(method, uri, cancellationToken);

        return response.IsSuccessStatusCode ? (await response.Content.GetJObjectOrDefaultAsync()).GetFirstResult().GetString(propertyName) : string.Empty;
    }
}

public static class SchemaHelper
{
    public static JObject GetValueSchemaObject(string entityType)
    {
        if (string.IsNullOrWhiteSpace(entityType))
            return new JObject();

        string[] fields = entityType.ToLowerInvariant() switch
        {
            "account" or "accounts" or "powerautomateaccounts" => AccountFields,
            "opportunity" or "opportunities" or "powerautomateopportunities" => OpportunityFields,
            "project" or "projects" or "powerautomateprojects" => ProjectFields,
            "salesorder" or "salesorders" or "powerautomatesalesorders" => SalesOrderFields,
            _ => Array.Empty<string>()
        };
        if (fields == null || fields.Length == 0)
            return new JObject();

        return new JObject
        {
            ["type"] = "object",
            ["properties"] = ConvertToJSONSchema(fields)
        };
    }

    public static JObject GetTableSchemaObject()
        => new JObject
        {
            ["type"] = "array",
            ["items"] = new JObject
            {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["name"] = new JObject { ["type"] = "string" },
                    ["oldValue"] = new JObject { ["type"] = "string" },
                    ["newValue"] = new JObject { ["type"] = "string" }
                }
            }
        };

    private static JObject ConvertToJSONSchema(string[] fields)
        => new JObject(fields.Select(f =>
            new JProperty(f.Substring(1), f[0] switch
            {
                'i' => new JObject { ["type"] = "integer" },
                'g' => new JObject { ["type"] = "string", ["format"] = "uuid" },
                'd' => new JObject { ["type"] = "string", ["format"] = "date-time" },
                'b' => new JObject { ["type"] = "boolean" },
                'n' => new JObject { ["type"] = "number", ["format"] = "decimal" },
                _ => new JObject { ["type"] = "string" }
            })
        ));

    private static readonly string[] AccountFields = new[] { "sBusinessComponent","sEntityEvent","dViesLastCalledDate","bViesResult",
        "gAccountant","gAccountantAccountManager","sAccountantAccountManagerFullName","gAccountManager","sAccountManagerFullName","iAccountManagerHID","gActivitySector",
        "sActivitySectorDescription","gActivitySectorParent","gActivitySubSector","sActivitySubSectorDescription","gActivitySubSectorParent","sAddressLine1","sAddressLine2",
        "sAddressLine3","bAutomaticProcessProposedEntry","sBankAccountCountry","sBankAccountCountryDescription","sBankAccountIBAN","bBlocked","gBRIN","sBRINName","gBusinessType",
        "sBusinessTypeDescription","bCanDropShip","sChamberOfCommerce","sChamberOfCommerceEstablishment","sCity","sClassification","gClassification1",
        "sClassification1Description","sClassification1Name","gClassification2","sClassification2Description","sClassification2Name","gClassification3",
        "sClassification3Description","sClassification3Name","gClassification4","sClassification4Description","sClassification4Name","gClassification5",
        "sClassification5Description","sClassification5Name","gClassification6","sClassification6Description","sClassification6Name","gClassification7",
        "sClassification7Description","sClassification7Name","gClassification8","sClassification8Description","sClassification8Name","sClassificationDescription","sCode",
        "sCodeAtSupplier","gCompanySize","sCompanySizeDescription","iConsolidationScenario","sCostcenter","sCostcenterDescription","bCostPaid","sCountry","sCountryDescription",
        "dCreated","gCreator","sCreatorFullName","nCreditLinePurchase","nCreditLineSales","gCreditManagementScenario","gCreditManagementStatus","sCreditManagementStatusCode",
        "sCreditManagementStatusDescription","sCurrency","dCustomerSince","gDefaultApprover","sDefaultApproverFullName","nDiscountPurchase","nDiscountSales","iDivision",
        "sDivisionDescription","gDocument","iDocumentHID","sDocumentSubject","sDunsNumber","sEmail","dEndDate","dEstablishedDate","gExtension","sFax","gGLAccountPurchase",
        "sGLAccountPurchaseCode","sGLAccountPurchaseDescription","gGLAccountSales","sGLAccountSalesCode","sGLAccountSalesDescription","gGLAP","sGLAPCode","sGLAPDescription",
        "gGLAR","sGLARCode","sGLARDescription","sGlnNumber","gID","sIntraStatArea","sIntraStatDeliveryTerm","sIntraStatSystem","sIntraStatTransactionA","sIntraStatTransactionB",
        "sIntraStatTransportMethod","gInvoiceAccount","sInvoiceAccountCode","sInvoiceAccountName","iInvoiceAttachmentType","iInvoicingMethod","bIsAccountant","bIsAgency",
        "bIsAnonymized","bIsCheckOverdueInvoices","bIsCompetitor","bIsCustomer","bIsMailing","bIsPilot","bIsReseller","bIsSales","bIsSupplier","nLaborMarkupPercentage",
        "sLanguage","sLanguageDescription","gLeadPurpose","sLeadPurposeDescription","gLeadSource","sLeadSourceDescription","sLogoFileName","gMainContact","dModified","gModifier",
        "sModifierFullName","sName","sOBNumber","sOINNumber","gParent","sParentCode","sPaymentConditionPurchase","sPaymentConditionPurchaseDescription","sPaymentConditionSales",
        "sPaymentConditionSalesDescription","sPeppolIdentifier","sPhone","sPhoneExtension","sPostcode","gPriceList","nProfitPercentage","sPurchaseCurrency",
        "sPurchaseCurrencyDescription","iPurchaseLeadDays","nPurchaseMarkupPercentage","nPurchaseMarkupPercentageProject","iPurchaseOrderAttachmentType","sPurchaseVATCode",
        "sPurchaseVATCodeDescription","iQuotationMarkupType","bRecepientOfCommissions","sRemarks","iReminderFlowCategory","gReseller","sResellerCode","sResellerName",
        "sSalesCurrency","sSalesCurrencyDescription","sSalesVATCode","sSalesVATCodeDescription","gSBICode","sSbiCodeDescription","gSbiCodeSector","gSbiCodeSubSector",
        "sSearchCode","iSecurityLevel","iSendPurchaseOrderMethod","sSepaDDCreditorIdentifier","bSeparateInvPerProject","bSeparateInvPerSubscription","iShippingLeadDays",
        "gShippingMethod","sShippingMethodCode","sShippingMethodDescription","iSource","dStartDate","sState","sStateDisplayValue","sStateName","sStatus","dStatusSince",
        "sTaxReferenceNumber","sTradeName","sType","bUseTimeSpecification","sVATLiability","sVATNumber","sVatSystem","sWebsite","sWithholdingTaxDescription","sWithholdingTaxKey"
    };

    private static readonly string[] OpportunityFields = new[] { "sBusinessComponent", "sEntityEvent",
        "gAccount","gAccountant","sAccountantName","sAccountCode","sAccountName","dActionDate","nAmountDC","nAmountFC","gCampaign","sCampaignDescription",
        "gCampaignMarketingList","dCloseDate","gCompetitor","sCompetitorCode","sCompetitorName","dCreated","gCreator","sCreatorFullName","sCurrency","iDivision",
        "gExchangeRateType","gExtension","gID","iInvolvementType","gLeadDeveloper","gLeadSource","sLeadSourceName","gMainContact","dModified","gModifier","sModifierFullName",
        "sName","sNextAction","sNotes","iNumber","iOpportunityDepartment","gOpportunityStage","sOpportunityStageDescription","iOpportunityStatus","iOpportunityType","gOwner",
        "bOwnerRead","nProbability","gProject","sProjectName","nRateFC","gReasonCode","sReasonCodeDescription","gReseller","sResellerName","gSalesType","sSalesTypeName"
    };

    private static readonly string[] SalesOrderFields = new[] { "sBusinessComponent", "sEntityEvent",
        "iActionCheckBlockedAccount","nAmountDC","nAmountDiscountFC","nAmountFC","nAmountVATDiscountFC","nAmountVATFC","dApprovalDateTime","gApprover","gCopyEntryID","sCurrency",
        "gDeliveryAccount","sDeliveryAccountCode","gDeliveryAccountContact","sDeliveryAccountName","gDeliveryAddress","dDeliveryDate","nDepositChargeAmountDC",
        "nDepositChargeAmountFC","nDepositChargeAmountVATFC","sDescription","nDiscount","nDiscountAmountDC","nDiscountAmountFC","iDiscountType","iDivision","sDivisionName",
        "gDocument","dEntryDate","gEntryID","nEnvironmentalTaxChargeAmountDC","nEnvironmentalTaxChargeAmountFC","nEnvironmentalTaxChargeAmountVATFC","gExchangeRateType",
        "gGLAccount","sGLAccountCode","sGLAccountDescription","iGLAccountType","gID","iIncoterm","sIncotermAddress","sIncotermCode","gInvoiceAccount","sInvoiceAccountCode",
        "gInvoiceAccountContact","sInvoiceAccountName","iInvoiceStatus","sInvoiceStatusDescription","sNotes","gOffsetGLAccount","sOffsetGLAccountCode","iOffsetGLAccountType",
        "sOffsetGLDesc","gOrderAccount","sOrderAccountCode","gOrderAccountContact","sOrderAccountName","sPaymentCondition","sPaymentConditionDescription","sPaymentReference",
        "gProject","gQuotationEntryID","nRateFC","gSalesChannel","sSalesChannelCode","iSalesOrderNumber","gSalesRepresentative","sSalesRepresentativeFullName",
        "sSelectionCodeCode","gSelectionCodeID","gShippingMethod","sShippingMethodCode","sShippingMethodDescription","iShippingStatus","sShippingStatusDescription","iSource",
        "iStatus","iStatusApproval","sStatusDescription","dsyscreated","gsyscreator","ssyscreatorFullName","dsysmodified","gsysmodifier","ssysmodifierFullName","gTaxSchedule",
        "iType","gUsageTransaction","sVATCode","gWarehouse","sWarehouseCode","sYourRef"
    };

    private static readonly string[] ProjectFields = new[] { "sBusinessComponent", "sEntityEvent",
        "gAccount","gAccountContact","sAccountName","sActionCode","bAllowAdditionalInvoicing","bAllowMemberEntryOnly","bBlockEntry","bBlockInvoicing","bBlockPlanning",
        "bBlockPurchasing","bBlockRebilling","nBudgetCostsAmountDC","iBudgetedHoursOverrunAction","nBudgetSalesAmountDC","iBudgetType","sBudgetTypeDescription","gClassification",
        "sClassificationDescription","sCode","dCreated","gCreator","sCreatorFullName","sCustomerPOnumber","sDescription","iDivision","sDivisionName","dEndDate","iFinancialYear",
        "gFixedPriceItem","sFixedPriceItemDescription","gID","bIncludeSpecificationInInvoicePdf","sInternalNotes","gInvoiceAddress","bInvoiceAsQuoted","gManager",
        "sManagerFullname","dModified","gModifier","sModifierFullName","sNotes","sPaymentCondition","gPrepaidItem","iPrepaidType","sProposalDocSubject","gProposalDocument",
        "nPurchaseMarkupPercentage","nSalesAmountDC","nSalesTimeQuantity","gSourceQuotation","dStartDate","nTimeQuantityToAlert","iTimeSpecificationType","iType",
        "sTypeDescription","bUseBillingMilestones"
    };
}
