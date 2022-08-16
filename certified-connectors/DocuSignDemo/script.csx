public class Script : ScriptBase
{
  public override async Task<HttpResponseMessage> ExecuteAsync()
  {
    try
    {
      if ("WebhookResponse".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
      {
        await this.RedirectWebhookNotification().ConfigureAwait(false);
        return new HttpResponseMessage(HttpStatusCode.OK);
      }

      await this.UpdateRequest().ConfigureAwait(false);

      if (this.Context.OperationId.StartsWith("StaticResponse", StringComparison.OrdinalIgnoreCase))
      {
        var staticResponse = new HttpResponseMessage();
        staticResponse.Content = GetStaticResponse(this.Context);
        return staticResponse;
      }

      var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
      if (response.IsSuccessStatusCode)
      {
        await this.UpdateResponse(response).ConfigureAwait(false);
      }

      return response;
    }
    catch (ConnectorException ex)
    {
      var response = new HttpResponseMessage(ex.StatusCode);
      response.Content = CreateJsonContent(ex.Message);
      return response;
    }
  }

  private StringContent GetStaticResponse(IScriptContext context)
  {
    var response = new JObject();
    string operationId = context.OperationId;

    if (operationId.Equals("StaticResponseForDocumentTypes", StringComparison.OrdinalIgnoreCase))
    {
      var docTypesArray = new JArray();
      string[] docTypes = { "pdf", "docx", "doc", "xlsx", "xls", "jpg" };
      foreach (var docType in docTypes)
      {
        var docTypeObject = new JObject()
        {
          ["name"] = docType
        };
        docTypesArray.Add(docTypeObject);
      }

      response["documentTypes"] = docTypesArray;
    }

    if (operationId.Equals("StaticResponseForTabTypes", StringComparison.OrdinalIgnoreCase))
    {
      var tabTypesArray = new JArray();
      string [,] tabTypes = { 
        { "signHereTabs", "Signature" }, 
        { "dateSignedTabs", "Date Signed" }, 
        { "textTabs", "Text" }, 
        { "fullNameTabs", "Name" },
        { "initialHereTabs", "Initial" },
        { "checkboxTabs", "Checkbox" },
        { "titleTabs", "Title" },
        { "signerAttachmentTabs", "Attachment" },
        { "emailTabs", "Email" },
        { "approveTabs", "Approval" }
      };
      for (var i = 0; i < tabTypes.GetLength(0); i++)
      {
        var tabTypeObject = new JObject()
        {
          ["type"] = tabTypes[i,0],
          ["name"] = tabTypes[i,1]
        };
        tabTypesArray.Add(tabTypeObject);
      }

      response["tabTypes"] = tabTypesArray;
    }

    if (operationId.StartsWith("StaticResponseForFont", StringComparison.OrdinalIgnoreCase))
    {
      var fontNamesArray = new JArray();
      string[] fontNames = getFontNames(operationId);

      foreach (var fontName in fontNames)
      {
        var fontNameObject = new JObject()
        {
          ["name"] = fontName
        };
        fontNamesArray.Add(fontNameObject);
      }

      response["fontNames"] = fontNamesArray;
    }

    if (operationId.Equals("StaticResponseForAnchorTabSchema", StringComparison.OrdinalIgnoreCase))
    {
      var query = HttpUtility.ParseQueryString(context.Request.RequestUri.Query);
      var tabType = query.Get("tabType");

      response["name"] = "dynamicSchema";
      response["title"] = "dynamicSchema";
      response["schema"] = new JObject
      {
        ["type"] = "object",
        ["properties"] = new JObject
        {
          ["tabs"] = new JObject
          {
            ["type"] = "array",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "anchor string *"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "X offset (pixels)"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "Y offset (pixels)"
                }
              }
            }
          }
        }
      };

      if (tabType.Equals("textTabs", StringComparison.OrdinalIgnoreCase))
      {
        response["schema"]["properties"]["tabs"]["items"]["properties"]["value"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "value"
        };
      }

      if (tabType.Equals("textTabs", StringComparison.OrdinalIgnoreCase) ||
          tabType.Equals("dateSignedTabs", StringComparison.OrdinalIgnoreCase) ||
          tabType.Equals("fullNameTabs", StringComparison.OrdinalIgnoreCase) ||
          tabType.Equals("titleTabs", StringComparison.OrdinalIgnoreCase) ||
          tabType.Equals("emailTabs", StringComparison.OrdinalIgnoreCase))
      {
        response["schema"]["properties"]["tabs"]["items"]["properties"]["font"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-dynamic-values"] = new JObject
            {
              ["operationId"] = "StaticResponseForFontFaces",
              ["value-collection"] = "fontNames",
              ["value-path"] = "name",
              ["value-title"] = "name"
            },
          ["x-ms-summary"] = "font"
        };
        response["schema"]["properties"]["tabs"]["items"]["properties"]["fontColor"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-dynamic-values"] = new JObject
          {
            ["operationId"] = "StaticResponseForFontColors",
            ["value-collection"] = "fontNames",
            ["value-path"] = "name",
            ["value-title"] = "name"
          },
          ["x-ms-summary"] = "font color"
        };
        response["schema"]["properties"]["tabs"]["items"]["properties"]["fontSize"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-dynamic-values"] = new JObject
          {
            ["operationId"] = "StaticResponseForFontSizes",
            ["value-collection"] = "fontNames",
            ["value-path"] = "name",
            ["value-title"] = "name"
          },
          ["x-ms-summary"] = "font size"
        }; 
        response["schema"]["properties"]["tabs"]["items"]["properties"]["bold"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "bold",
          ["description"] = "true/false"
        };
        response["schema"]["properties"]["tabs"]["items"]["properties"]["italic"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "italic",
          ["description"] = "true/false"
        };
      }
    }

    if (operationId.Equals("StaticResponseForEmbeddedSigningSchema", StringComparison.OrdinalIgnoreCase))
    {
      var query = HttpUtility.ParseQueryString(context.Request.RequestUri.Query);
      var returnUrl = query.Get("returnUrl");

      response["name"] = "dynamicSchema";
      response["title"] = "dynamicSchema";
      response["schema"] = new JObject
      {
        ["type"] = "object",
        ["properties"] = new JObject()
      };

      if (returnUrl.Equals("Add A Different URL", StringComparison.OrdinalIgnoreCase))
      {
        response["schema"]["properties"]["returnURL"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "* Add Return URL"
        };
      }
      else {
        response["schema"] = null;
      }
    }

    if (operationId.Equals("StaticResponseForEmbeddedSenderSchema", StringComparison.OrdinalIgnoreCase))
    {
      var query = HttpUtility.ParseQueryString(context.Request.RequestUri.Query);
      var returnUrl = query.Get("returnUrl");

      response["name"] = "dynamicSchema";
      response["title"] = "dynamicSchema";
      response["schema"] = new JObject
      {
        ["type"] = "object",
        ["properties"] = new JObject()
      };

      if (returnUrl.Equals("Add A Different URL", StringComparison.OrdinalIgnoreCase))
      {
        response["schema"]["properties"]["returnURL"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "* Add Return URL"
        };
      }
      else {
        response["schema"] = null;
      }
    }

    if (operationId.Equals("StaticResponseForVerificationTypeSchema", StringComparison.OrdinalIgnoreCase))
    {
      var query = HttpUtility.ParseQueryString(context.Request.RequestUri.Query);
      var verificationType = query.Get("verificationType");

      response["name"] = "dynamicSchema";
      response["title"] = "dynamicSchema";
      response["schema"] = new JObject
      {
        ["type"] = "object",
        ["properties"] = new JObject()
      };

      if (verificationType.Equals("Phone Authentication", StringComparison.OrdinalIgnoreCase))
      {
        response["schema"]["properties"]["countryCode"] = new JObject 
        {
          ["type"] = "string",
          ["x-ms-summary"] = "* Country Code (+)"
        };
        response["schema"]["properties"]["phoneNumber"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "* Recipient's Phone Number"
        };
        response["schema"]["properties"]["workflowID"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-dynamic-values"] = new JObject
            {
              ["operationId"] = "GetWorkflowIDs",
              ["parameters"] = new JObject
              {
                ["accountId"] = new JObject
                {
                  ["parameter"] = "accountId"
                }
              },
              ["value-collection"] = "workFlowIds",
              ["value-path"] = "type",
              ["value-title"] = "name",
            },
          ["x-ms-summary"] = "* Workflow IDs"
        };
      }
      else if (verificationType.Equals("Access Code", StringComparison.OrdinalIgnoreCase))
      {
        response["schema"]["properties"]["accessCode"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "* Access Code"
        };
      }
      else if (verificationType.Equals("ID Verification", StringComparison.OrdinalIgnoreCase))
      {
        response["schema"]["properties"]["workflowID"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-dynamic-values"] = new JObject
            {
              ["operationId"] = "GetWorkflowIDs",
              ["parameters"] = new JObject
              {
                ["accountId"] = new JObject
                {
                  ["parameter"] = "accountId"
                }
              },
              ["value-collection"] = "workFlowIds",
              ["value-path"] = "type",
              ["value-title"] = "name",
            },
          ["x-ms-summary"] = "* Workflow IDs"
        };
      }
      else {
        response["schema"] = null;
      }
    }
    return CreateJsonContent(response.ToString());
  }

  private string [] getFontNames(string operationId)
  {
    string[] fontNames = null;

    if (operationId.Equals("StaticResponseForFontFaces", StringComparison.OrdinalIgnoreCase))
    {
      fontNames = new string[] { "Default","Arial","ArialNarrow","Calibri","CourierNew","Garamond","Georgia",
        "Helvetica","LucidaConsole","MSGothic","MSMincho","OCR-A","Tahoma","TimesNewRoman","Trebuchet","Verdana"};
    }
    else if (operationId.Equals("StaticResponseForFontColors", StringComparison.OrdinalIgnoreCase))
    {
      fontNames = new string[] { "Black","BrightBlue","BrightRed","DarkGreen","DarkRed","Gold","Green",
        "NavyBlue","Purple","White" };
    }
    else if (operationId.Equals("StaticResponseForFontSizes", StringComparison.OrdinalIgnoreCase))
    {
      fontNames = new string[] { "Size7","Size8","Size9","Size10","Size11","Size12","Size14","Size16","Size18",
        "Size20","Size22","Size24","Size26","Size28","Size36","Size48","Size72" };
    }

    return fontNames;
  }

  private static JObject ParseContentAsJObject(string content, bool isRequest)
  {
    JObject body;
    try
    {
      body = JObject.Parse(content);
    }
    catch (JsonReaderException ex)
    {
      if (isRequest)
      {
        throw new ConnectorException(HttpStatusCode.BadRequest, "Unable to parse the request body", ex);
      }
      else
      {
        throw new ConnectorException(HttpStatusCode.BadGateway, "Unable to parse the response body", ex);
      }
    }

    if (body == null)
    {
      if (isRequest)
      {
        throw new ConnectorException(HttpStatusCode.BadRequest, "The request body is empty");
      }
      else
      {
        throw new ConnectorException(HttpStatusCode.BadGateway, "The response body is empty");
      }
    }

    return body;
  }

  private static string TransformWebhookNotificationBody(string content)
  {
    JObject body = ParseContentAsJObject(content, true);

    // customfield code
    if (body["data"] is JObject && body["data"]["envelopeSummary"] is JObject)
    {
      var envelopeSummary = body["data"]["envelopeSummary"];
      var customFields = envelopeSummary["customFields"];
      var newCustomFields = new JObject();

      if (customFields is JObject)
      {
        var customFieldsArray = customFields["textCustomFields"];
        customFieldsArray = customFieldsArray is JObject ? new JArray(customFieldsArray) : customFieldsArray;

        foreach (var field in customFieldsArray as JArray ?? new JArray())
        {
          var fieldName = field.Type == JTokenType.Object ? (string)field["name"] : null;
          if (!string.IsNullOrWhiteSpace(fieldName) && newCustomFields[fieldName] == null)
          {
            newCustomFields.Add(fieldName, field["value"]);
          }
        }
      }

      body["data"]["envelopeSummary"]["customFields"] = newCustomFields;

      // tab code
      var recipientStatuses = envelopeSummary["recipients"];
      if (recipientStatuses is JObject)
      {
        foreach (var recipient in recipientStatuses["signers"] ?? new JArray())
        {
          var tabs = recipient["tabs"];
          if (tabs is JObject)
          {
            var newTabs = new JObject();

            string[] tabTypes = { "textTabs", "fullNameTabs", "dateSignedTabs", "companyTabs", "titleTabs", "numberTabs",
              "ssnTabs", "dateTabs", "zipTabs", "emailTabs", "noteTabs", "listTabs", "firstNameTabs", "lastNameTabs", "emailAddressTabs",
              "formulaTabs" };
            foreach (var tabType in tabTypes)
            {
              var tabStatusArray = tabs[tabType];

              foreach (var tab in tabStatusArray as JArray ?? new JArray())
              {
                if (tab is JObject)
                {
                  var tabLabel = (string)tab["tabLabel"];
                  var tabValue = (string)tab["value"];

                  if (!string.IsNullOrWhiteSpace(tabLabel) && !string.IsNullOrWhiteSpace(tabValue))
                  {
                    if (newTabs[tabLabel] == null)
                    {
                      newTabs.Add(tabLabel, tabValue);
                    }
                  }
                }
              }
            }
            
            recipient["tabs"] = newTabs;
          }
        }
      }
    }

    return body.ToString();
  }

  private async Task<HttpResponseMessage> RedirectWebhookNotification()
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var logicAppsUri = query.Get("logicAppsUri");

    if (string.IsNullOrEmpty(logicAppsUri))
    {
      return new HttpResponseMessage(HttpStatusCode.BadRequest)
      {
        Content = new StringContent("Required 'logicAppsUri' parameter is empty"),
      };
    }

    try
    {
      logicAppsUri = Encoding.UTF8.GetString(Convert.FromBase64String(logicAppsUri));
    }
    catch (FormatException)
    {
      return new HttpResponseMessage(HttpStatusCode.BadRequest)
      {
        Content = new StringContent("'logicAppsUri' value is not a correct base64-encoded string"),
      };
    }

    var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
    var notificationContent = TransformWebhookNotificationBody(content);

    using var logicAppsRequest = new HttpRequestMessage(HttpMethod.Post, logicAppsUri);
    logicAppsRequest.Content = CreateJsonContent(notificationContent);

    return await this.Context.SendAsync(logicAppsRequest, this.CancellationToken).ConfigureAwait(false);
  }

  private JObject CreateHookEnvelopeBodyTransformation(JObject original)
  {
    var body = new JObject();
    
    var uriLogicApps = original["urlToPublishTo"]?.ToString();
    var uriLogicAppsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(uriLogicApps ?? string.Empty));
    var notificationProxyUri = this.Context.CreateNotificationUri($"/webhook_response?logicAppsUri={uriLogicAppsBase64}");

    body["allUsers"] = "true";
    body["allowEnvelopePublish"] = "true";
    body["includeDocumentFields"] = "true";
    body["requiresAcknowledgement"] = "true";
    body["urlToPublishTo"] = notificationProxyUri.AbsoluteUri;
    body["name"] = original["name"]?.ToString();

    var envelopeEvent = original["envelopeEvents"]?.ToString();
    var envelopeEventsArray = new JArray();
    envelopeEventsArray.Add(envelopeEvent);
    body["envelopeEvents"] = envelopeEventsArray;
    body["configurationType"] = "custom";
    body["deliveryMode"] = "sim";

    string eventData = @"[
      'tabs',
      'custom_fields',
      'recipients'
    ]";

    JArray includeData = JArray.Parse(eventData);
    body["eventData"] = new JObject
    {
        ["version"] = "restv2.1",
        ["format"] = "json",
        ["includeData"] = includeData
    };
    return body;
  }

  private JObject CreateEnvelopeFromTemplateBodyTransformation(JObject body)
  {
    var templateRoles = new JArray();
    var signer = new JObject();
    var count = 0;

    foreach (var property in body)
    {
      var value = (string)property.Value;
      var key = (string)property.Key;

      if (key.Contains(" Name"))
      {
        signer["roleName"] = key.Substring(0, key.Length - 5);
        signer["name"] = value;
      }

      if (key.Contains(" Email"))
      {
        signer["email"] = value;
      }

      if (count % 2 != 0)
      {
        templateRoles.Add(signer);
        signer = new JObject();
      }

      count++;
    }

    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var newBody = new JObject()
    {
      ["templateRoles"] = templateRoles,
      ["templateId"] = query.Get("templateId")
    };

    if (!string.IsNullOrEmpty(query.Get("status")))
    {
      newBody["status"] = query.Get("status");
    }

    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
    uriBuilder.Path = uriBuilder.Path.Replace("envelopes/createFromTemplate", "/envelopes");
    this.Context.Request.RequestUri = uriBuilder.Uri;

    return newBody;
  }

  private JObject CreateBlankEnvelopeBodyTransformation(JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);

    body["emailSubject"] = query.Get("emailSubject");
    var emailBody = query.Get("emailBody");

    if (!string.IsNullOrEmpty(emailBody))
    {
      body["emailBlurb"] = emailBody;
    }

    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
    uriBuilder.Path = uriBuilder.Path.Replace("/envelopes/createBlankEnvelope", "/envelopes");
    this.Context.Request.RequestUri = uriBuilder.Uri;

    return body;
  }

  private JObject AddRecipientToEnvelopeBodyTransformation(JObject body)
  {
    var signers = new JArray
    {
      new JObject(),
    };
    AddCoreRecipientParams(signers, body);
    AddParamsForSelectedVerificationType(signers, body);

    body["signers"] = signers;
    return body;
  }

  private JObject GenerateEmbeddedSigningURLBodyTransformation (JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    body["userName"] = query.Get("signerName");
    body["email"] = query.Get("signerEmail");
    body["authenticationMethod"] = query.Get("authenticationMethod");
    body["clientUserId"] = query.Get("clientUserId");
    
    var returnUrl = query.Get("returnUrl");
    if (returnUrl.Equals("Default URL"))
    {
      body["returnUrl"] = "https://postsign.docusign.com/postsigning/en/finish-signing";
    }
    else if (returnUrl.Equals("Add A Different URL"))
    {
      body["returnUrl"] = body["returnURL"];
    }

    return body;
  }

  private JObject GenerateEmbeddedSenderURLBodyTransformation (JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var returnUrl = query.Get("returnUrl");
    var url = this.Context.Request.RequestUri.Authority;

    if (returnUrl.Equals("DocuSign's URL"))
    {
      if (url.Equals("demo.docusign.net"))
      {
        body["returnUrl"] = " https://appdemo.docusign.com/";
      }
      else
      {
        body["returnUrl"] = " https://app.docusign.com/";
      }
    }
    else
    {
      body["returnUrl"] = body["returnURL"];
    }
    return body;
  }

  private void AddCoreRecipientParams(JArray signers, JObject body) 
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    signers[0]["recipientId"] = Guid.NewGuid();
    if (!string.IsNullOrEmpty(query.Get("routingOrder")))
    {
      signers[0]["routingOrder"] = query.Get("routingOrder");
    }

    if (!string.IsNullOrEmpty(query.Get("clientUserId")))
    {
      signers[0]["clientUserId"] = query.Get("clientUserId");
    }

    signers[0]["name"] = Uri.UnescapeDataString(query.Get("recipientName")).Replace("+", " ");
    signers[0]["email"] = Uri.UnescapeDataString(query.Get("recipientEmail")).Replace("+", " ");
  }

  private void AddParamsForSelectedVerificationType (JArray signers, JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var verificationType = query.Get("verificationType");

    if (verificationType.Equals("Phone Authentication"))
    {
      var identityVerification = new JObject();
      var inputOptions = new JArray();
      var inputObject = new JObject();
      var phoneNumberList = new JArray();
      var phoneNumberObject = new JObject();

      phoneNumberObject["Number"] = body["phoneNumber"];
      phoneNumberObject["CountryCode"] = body["countryCode"];
      phoneNumberList.Add(phoneNumberObject);

      inputObject["phoneNumberList"] = phoneNumberList;
      inputObject["name"] = "phone_number_list";
      inputObject["valueType"] = "PhoneNumberList";
      inputOptions.Add(inputObject);

      identityVerification["workflowId"] = body["workflowID"];
      identityVerification["inputOptions"] = inputOptions;
      signers[0]["identityVerification"] = identityVerification;
    }
    else if (verificationType.Equals("Access Code"))
    {
      signers[0]["accessCode"] = body["accessCode"];
    }
    else if (verificationType.Equals("Knowledge Based"))
    {
      signers[0]["idCheckConfigurationName"] = "ID Check $";
    }
    else if (verificationType.Equals("ID Verification"))
    {
      var identityVerification = new JObject();
      identityVerification["workflowId"] = body["workflowID"];
      signers[0]["identityVerification"] = identityVerification;
    }
  }

  private int GenerateDocumentId()
  {
    DateTimeOffset now = DateTimeOffset.UtcNow;
    DateTime midnight = DateTime.Now.Date;
    TimeSpan ts = now.Subtract(midnight);
    return (int)ts.TotalMilliseconds;
  }

  private JObject AddDocumentsToEnvelopeBodyTransformation(JObject body)
  {
    var documents = body["documents"] as JArray;

    for (var i = 0; i < documents.Count; i++)
    {
      documents[i]["documentId"] = $"{GenerateDocumentId() + i}";
    }

    body["documents"] = documents;
    return body;
  }

  private JObject AddRecipientTabsBodyTransformation(JObject body)
  {
    var res_tabs = new JArray();
    var tabs = body["tabs"] as JArray;
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var tabType = query.Get("tabType");
    
    for (var i = 0; i < tabs.Count; i++)
    {
      JObject tab = tabs[i] as JObject;
      if (tabType.Equals("textTabs"))
      {
        tab["locked"] = "false";
      }
      res_tabs.Add(tab);
    }

    body[tabType] = res_tabs;

    return body;
  }

  private async Task UpdateApiEndpoint()
  {
    string content = string.Empty;
    using var userInfoRequest = new HttpRequestMessage(HttpMethod.Get, "https://account-d.docusign.com/oauth/userinfo");

    // Access token is in the authorization header already
    userInfoRequest.Headers.Authorization = this.Context.Request.Headers.Authorization;

    try
    {
      using var userInfoResponse = await this.Context.SendAsync(userInfoRequest, this.CancellationToken).ConfigureAwait(false);
      content = await userInfoResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

      if (userInfoResponse.IsSuccessStatusCode)
      {
        var jsonContent = JObject.Parse(content);
        var baseUri = jsonContent["accounts"]?[0]?["base_uri"]?.ToString();
        if (!string.IsNullOrEmpty(baseUri))
        {
          this.Context.Request.RequestUri = new Uri(new Uri(baseUri), this.Context.Request.RequestUri.PathAndQuery);
        }
        else
        {
          throw new ConnectorException(HttpStatusCode.BadGateway, "Unable to get User's API endpoint from the response: " + content);
        }
      }
      else
      {
        throw new ConnectorException(userInfoResponse.StatusCode, content);
      }
    }
    catch (HttpRequestException ex)
    {
      throw new ConnectorException(HttpStatusCode.BadGateway, "Unable to get User Info: " + ex.Message, ex);
    }
    catch (JsonReaderException ex)
    {
      throw new ConnectorException(HttpStatusCode.BadGateway, "Unable to parse User Info response: " + content, ex);
    }
    catch (UriFormatException ex)
    {
      throw new ConnectorException(HttpStatusCode.BadGateway, "Unable to construct User's API endpoint from the response: " + content, ex);
    }
  }

  private async Task TransformRequestJsonBody(Func<JObject, JObject> transformationFunction)
  {
    var content = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);

    var body = new JObject();
    if (!String.IsNullOrWhiteSpace(content))
    {
      body = transformationFunction(ParseContentAsJObject(content, true));
    }
    else
    {
      body = transformationFunction(body);
    }

    this.Context.Request.Content = CreateJsonContent(body.ToString());
  }

  private async Task UpdateRequest()
  {
    await this.UpdateApiEndpoint().ConfigureAwait(false);

    if ("SendDraftEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      this.Context.Request.Content = new StringContent("{ \"status\": \"sent\" }", Encoding.UTF8, "application/json");
    }

    if ("CreateHookEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.CreateHookEnvelopeBodyTransformation).ConfigureAwait(false);
    }

    if ("CreateBlankEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.CreateBlankEnvelopeBodyTransformation).ConfigureAwait(false);
    }

    if ("CreateEnvelopeFromTemplate".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.CreateEnvelopeFromTemplateBodyTransformation).ConfigureAwait(false);
    }

    if ("AddRecipientToEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.AddRecipientToEnvelopeBodyTransformation).ConfigureAwait(false);
    }

    if ("GenerateEmbeddedSigningURL".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.GenerateEmbeddedSigningURLBodyTransformation).ConfigureAwait(false);
    }

    if ("GenerateEmbeddedSenderURL".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.GenerateEmbeddedSenderURLBodyTransformation).ConfigureAwait(false);
    }

    if ("AddDocumentsToEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.AddDocumentsToEnvelopeBodyTransformation).ConfigureAwait(false);
    }

    if ("AddRecipientTabs".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.AddRecipientTabsBodyTransformation).ConfigureAwait(false);
    }

    if ("RemoveRecipientFromEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var newBody = new JObject();
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      newBody["signers"] = new JArray
            {
                new JObject
                {
                    ["recipientId"] = Uri.UnescapeDataString(query.Get("RemoveRecipientFromEnvelopeRecipientId")).Replace("+", " "),
                },
            };

      this.Context.Request.Content = CreateJsonContent(newBody.ToString());
    }

    if ("OnEnvelopeStatusChanges".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      uriBuilder.Path = uriBuilder.Path.Replace("/trigger/accounts/", "/accounts/");
      var filterValue = query.Get("triggerState");
      if (string.IsNullOrEmpty(filterValue))
      {
        // initial trigger state to get existing items
        filterValue = DateTimeOffset.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
      }
      else
      {
        // remove triggerState
        query.Remove("triggerState");
      }

      query["from_date"] = filterValue;
      uriBuilder.Query = query.ToString();
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    if ("GetDynamicSigners".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      uriBuilder.Path = uriBuilder.Path.Replace("/signers/accounts/", "/accounts/");
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    if ("GetLoginAccounts".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      query["include_account_id_guid"] = "true";
      uriBuilder.Query = query.ToString();
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    if ("GetFolderEnvelopeList".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      query["include_items"] = "true";
      uriBuilder.Query = query.ToString();
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    // update Accept Header
    this.Context.Request.Headers.Accept.Clear();
    var acceptHeaderValue = "application/json";
    if ("GetDocuments".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      acceptHeaderValue = "application/pdf";
    }

    this.Context.Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeaderValue));
  }

  private async Task UpdateResponse(HttpResponseMessage response)
  {
    if ("CreateHookEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)
        && response.Headers?.Location != null)
    {
      var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
      var body = ParseContentAsJObject(content, false);
      response.Headers.Location = new Uri(string.Format(
          "{0}/{1}",
          this.Context.OriginalRequestUri.ToString(),
          body.GetValue("connectId").ToString()));
    }

    if ("GenerateEmbeddedSenderURL".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      var openIn = query.Get("openIn");

      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var url = body["url"].ToString();

      if (openIn.Equals("Prepare"))
      {
        url = url.Replace("&send=" + 1, "&send=" + 0);
      }
      body["url"] = url;
      response.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
    }

    if ("GetWorkflowIds".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var workflowsArray = new JArray();

      foreach (var id in (body["identityVerification"] as JArray)) {
        var workflowObj = new JObject()
        {
          ["type"] = id["workflowId"],
          ["name"] = id["defaultName"]
        };
        workflowsArray.Add(workflowObj);
      }
      body["workflowIds"] = workflowsArray;
      response.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
    }

    if ("GetDynamicSigners".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var itemProperties = new JObject();
      var basePropertyDefinition = new JObject
      {
        ["type"] = "string",
        ["x-ms-keyOrder"] = 0,
        ["x-ms-keyType"] = "none",
        ["x-ms-sort"] = "none",
      };

      foreach (var signer in (body["signers"] as JArray) ?? new JArray())
      {
        var roleName = signer["roleName"];
        itemProperties[roleName + " Name"] = basePropertyDefinition.DeepClone();
        itemProperties[roleName + " Email"] = basePropertyDefinition.DeepClone();
      }

      var newBody = new JObject
      {
        ["name"] = "dynamicSchema",
        ["title"] = "dynamicSchema",
        ["x-ms-permission"] = "read-write",
        ["schema"] = new JObject
        {
          ["type"] = "array",
          ["items"] = new JObject
          {
            ["type"] = "object",
            ["properties"] = itemProperties,
          },
        },
      };

      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("AddRecipientToEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var newBody = new JObject();

      foreach (var signer in (body["signers"] as JArray) ?? new JArray())
      {
        newBody = signer as JObject;
        break;
      }

      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("OnEnvelopeStatusChanges".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var originalQuery = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      var triggerState = originalQuery.Get("triggerState");
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var items = body.SelectToken("envelopes") as JArray;

      if (string.IsNullOrEmpty(triggerState) || items == null || items.Count == 0)
      {
        response.Content = null;
        response.StatusCode = HttpStatusCode.Accepted;
      }

      if (string.IsNullOrEmpty(triggerState))
      {
        // initial trigger call
        triggerState = DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
      }

      if (items?.Count > 0)
      {
        triggerState = items.Max(x => DateTimeOffset.Parse(x["statusChangedDateTime"].ToString())).AddMilliseconds(10).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
      }

      var locationUriBuilder = new UriBuilder(this.Context.OriginalRequestUri);
      originalQuery["triggerState"] = triggerState;
      locationUriBuilder.Query = originalQuery.ToString();
      response.Headers.Location = locationUriBuilder.Uri;
      response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(120));
    }

    if ("GetFolderEnvelopeList".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var newBody = new JObject();

      foreach (var folder in (body["folders"] as JArray) ?? new JArray())
      {
        newBody = folder as JObject;
        break;
      }

      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if (response.Content?.Headers?.ContentType != null)
    {
      if ("GetDocuments".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
      {
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
      }
      else
      {
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
      }
    }
  }

  public class ConnectorException : Exception
  {
    public ConnectorException(
        HttpStatusCode statusCode,
        string message,
        Exception innerException = null)
        : base(
                message,
                innerException)
    {
      this.StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }

    public override string ToString()
    {
      var error = new StringBuilder($"ConnectorException: Status code={this.StatusCode}, Message='{this.Message}'");
      var inner = this.InnerException;
      var level = 0;
      while (inner != null && level < 10)
      {
        level += 1;
        error.AppendLine($"Inner exception {level}: {inner.Message}");
        inner = inner.InnerException;
      }
         
      error.AppendLine($"Stack trace: {this.StackTrace}");
      return error.ToString();
    }
  }
}