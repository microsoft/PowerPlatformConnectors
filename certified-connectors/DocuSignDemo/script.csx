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
      else
      {
        await this.UpdateErrorResponse(response).ConfigureAwait(false);
      }

      return response;
    }
    catch (ConnectorException ex)
    {
      var response = new HttpResponseMessage(ex.StatusCode);

      if(ex.Message.Contains("ValidationFailure:"))
      {
        response.Content = CreateJsonContent(ex.JsonMessage());
      }
      else
      {
        response.Content = CreateJsonContent(ex.Message);
      }
      
      return response;
    }
  }

  private async Task UpdateErrorResponse(HttpResponseMessage response)
  {
    if ("GetMaestroWorkflowDefinitions".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (response.StatusCode == HttpStatusCode.Unauthorized && content.Equals("Jwt payload is an invalid JSON"))
        {
          response.Content = new StringContent("You will need to reconnect to your Docusign account", Encoding.UTF8, "text/plain");
        }
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
        { "approveTabs", "Approve" },
        { "checkboxTabs", "Checkbox" },
        { "companyTabs", "Company" },
        { "dateSignedTabs", "Date Signed" },
        { "dateTabs", "Date" },
        { "declineTabs", "Decline" },
        { "emailTabs", "Email" },
        { "firstNameTabs", "First Name" },
        { "formulaTabs", "Formula" },
        { "fullNameTabs", "Full Name" },
        { "initialHereTabs", "Initial" },
        { "lastNameTabs", "Last Name" },
        { "listTabs", "Dropdown" },
        { "noteTabs", "Note" },
        { "numberTabs", "Number" },
        { "numericalTabs", "Numerical" },
        { "radioGroupTabs", "Radio Group" },
        { "signHereTabs", "Signature" }, 
        { "signerAttachmentTabs", "Signer Attachment" },
        { "ssnTabs", "SSN" },
        { "tabGroups", "Checkbox Group"},
        { "textTabs", "Text" },
        { "titleTabs", "Title" },
        { "zipTabs", "Zip" }
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

    if (operationId.Equals("StaticResponseForRecipientTypes", StringComparison.OrdinalIgnoreCase))
    {
      var recipientTypesArray = new JArray();
      
      string [,] recipientTypes = { 
        { "agents", "Specify Recipients" }, 
        { "carbonCopies", "Receives a Copy" }, 
        { "certifiedDeliveries", "Needs to View" }, 
        { "editors", "Allow to Edit" },
        { "inPersonSigners", "In Person Signer" },
        { "intermediaries", "Update Recipients" },
        { "signers", "Needs to Sign" },
        { "witnesses", "Signs with Witness" }
      };

      for (var i = 0; i < recipientTypes.GetLength(0); i++)
      {
        var recipientTypeObject = new JObject()
        {
          ["type"] = recipientTypes[i,0],
          ["name"] = recipientTypes[i,1]
        };
        recipientTypesArray.Add(recipientTypeObject);
      }

      response["recipientTypes"] = recipientTypesArray;
    }

    if (operationId.Equals("StaticResponseForSignatureTypes", StringComparison.OrdinalIgnoreCase))
    {
      var signatureTypesArray = new JArray();

      string [,] signatureTypes = { 
        { "UniversalSignaturePen_ImageOnly" , "DS Electronic (SES)" }, 
        { "UniversalSignaturePen_OpenTrust_Hash_TSP", "DS EU Advanced (AES)" }, 
        { "docusign_eu_qualified_idnow_tsp", "DS EU Qualified (QES)" }
      };

      for (var i = 0; i < signatureTypes.GetLength(0); i++)
      {
        var signatureTypeObject = new JObject()
        {
          ["type"] = signatureTypes[i,0],
          ["name"] = signatureTypes[i,1]
        };
        signatureTypesArray.Add(signatureTypeObject);
      }

      response["signatureTypes"] = signatureTypesArray;
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
      if (tabType.Equals("radioGroupTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                  ["groupName"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Group Name"
                  },
                  ["radios"] = new JObject
                  {
                    ["type"] = "array",
                    ["items"] = new JObject
                      {
                        ["type"] = "object",
                        ["x-ms-summary"] = "Radios",
                        ["properties"] = new JObject
                        {
                          ["anchorString"] = new JObject
                          {
                            ["type"] = "string",
                            ["x-ms-summary"] = "Anchor String"
                          },
                          ["anchorHorizontalAlignment"] = new JObject
                          {
                            ["x-ms-summary"] = "Anchor Horizontal Alignment",
                            ["type"] = "string",
                            ["description"] = "left/right"
                          },
                          ["value"] = new JObject
                          {
                            ["type"] = "string",
                            ["x-ms-summary"] = "Value"
                          },
                          ["selected"] = new JObject
                          {
                            ["type"] = "string",
                            ["x-ms-summary"] = "Selected",
                            ["description"] = "true/false"
                          },
                          ["locked"] = new JObject
                          {
                            ["type"] = "string",
                            ["x-ms-summary"] = "Read Only",
                            ["description"] = "true/false"
                          },
                          ["required"] = new JObject
                          {
                            ["type"] = "string",
                            ["x-ms-summary"] = "Required",
                            ["description"] = "true/false"
                          },
                          ["anchorXOffset"] = new JObject
                          {
                            ["x-ms-summary"] = "Anchor X Offset",
                            ["type"] = "string"
                          }
                          ["anchorYOffset"] = new JObject
                          {
                            ["x-ms-summary"] = "Anchor Y Offset",
                            ["type"] = "string"
                          }
                        }
                      }
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("companyTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["locked"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Read Only",
                    ["description"] = "true/false"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("dateTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["value"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Value"
                  },
                  ["locked"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Read Only",
                    ["description"] = "true/false"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("approveTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["tabLabel"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "label"
                  },
                  ["buttonText"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Button Text"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("declineTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["buttonText"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Button Text"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("signerAttachmentTabs", StringComparison.OrdinalIgnoreCase) ||
      tabType.Equals("signHereTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["optional"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Optional",
                    ["description"] = "true/false"
                  },
                  ["tabLabel"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "label"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("firstNameTabs", StringComparison.OrdinalIgnoreCase) || 
      tabType.Equals("lastNameTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("formulaTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["formula"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Formula"
                  },
                  ["hidden"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Hidden",
                    ["description"] = "true/false"
                  },
                  ["roundDecimalPlaces"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Decimal places"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("listTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["listItems"] = new JObject
                  {
                    ["type"] = "array",
                    ["items"] = new JObject
                      {
                        ["type"] = "object",
                        ["x-ms-summary"] = "List Item",
                        ["properties"] = new JObject
                        {
                          ["selected"] = new JObject
                          {
                            ["type"] = "string",
                            ["x-ms-summary"] = "Selected",
                            ["description"] = "true/false"
                          },
                          ["text"] = new JObject
                          {
                            ["x-ms-summary"] = "Text",
                            ["type"] = "string"
                          },
                          ["value"] = new JObject
                          {
                            ["x-ms-summary"] = "Value",
                            ["type"] = "string"
                          }
                        }
                      }
                  },
                  ["listSelectedValue"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Default Option"
                  },
                  ["locked"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Read Only",
                    ["description"] = "true/false"
                  },
                  ["required"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Required",
                    ["description"] = "true/false"
                  },
                  ["tooltip"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Tooltip"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("noteTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["value"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Note Text"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("numberTabs", StringComparison.OrdinalIgnoreCase) ||
      (tabType.Equals("ssnTabs", StringComparison.OrdinalIgnoreCase)) ||
      (tabType.Equals("zipTabs", StringComparison.OrdinalIgnoreCase)))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["value"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Value"
                  },
                  ["locked"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Read Only",
                    ["description"] = "true/false"
                  },
                  ["required"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Required",
                    ["description"] = "true/false"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("numericalTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["numericalValue"] = new JObject
                  {
                    ["x-ms-summary"] = "Value",
                    ["type"] = "string"
                  },
                  ["locked"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Read Only",
                    ["description"] = "true/false"
                  },
                  ["required"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Required",
                    ["description"] = "true/false"
                  },
                  ["validationType"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Validation Type",
                    ["description"] = "Select",
                    ["enum"] = new JArray("Currency", "Number")
                  },
                  ["minNumericalValue"] = new JObject
                  {
                    ["x-ms-summary"] = "Minimum Amount",
                    ["type"] = "string"
                  },
                  ["maxNumericalValue"] = new JObject
                  {
                    ["x-ms-summary"] = "Maximum Amount",
                    ["type"] = "string"
                  },
                  ["localePolicyTab"] = new JObject
                  {
                    ["type"] = "array",
                    ["x-ms-summary"] = "Locale Policy",
                    ["items"] = new JObject
                      {
                        ["type"] = "object",
                        ["x-ms-summary"] = "Locale Policy",
                        ["properties"] = new JObject
                        {
                          ["cultureName"] = new JObject
                          {
                            ["type"] = "string",
                            ["x-ms-summary"] = "Culture Name",
                            ["description"] = "The two letter ISO 639-1 language code.",
                          },
                          ["currencyCode"] = new JObject
                          {
                            ["type"] = "string",
                            ["x-ms-summary"] = "Currency Code",
                            ["description"] = "The ISO 4217 currency code.",
                          },
                          ["currencyPositiveFormat"] = new JObject
                          {
                            ["type"] = "string",
                            ["x-ms-summary"] = "Currency Positive Format"
                          },
                          ["currencyNegativeFormat"] = new JObject
                          {
                            ["type"] = "string",
                            ["x-ms-summary"] = "Currency Negative Format"
                          },
                          ["useLongCurrencyFormat"] = new JObject
                          {
                            ["type"] = "string",
                            ["x-ms-summary"] = "Use Long Currency Format",
                            ["description"] = "true/false",
                          }
                        }
                      }
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("checkboxTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                  ["tabLabel"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Tab Label"
                  },
                  ["anchorString"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["anchorHorizontalAlignment"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Horizontal Alignment"
                  },
                  ["locked"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Read Only",
                    ["description"] = "true/false"
                  },
                  ["selected"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Selected",
                    ["description"] = "true/false"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["tabGroupLabels"] = new JObject
                  {
                    ["type"] = "array",
                    ["x-ms-summary"] = "Tab Group Labels",
                    ["items"] = new JObject
                      {
                        ["type"] = "string",
                        ["x-ms-summary"] = "",
                      }
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("dateSignedTabs", StringComparison.OrdinalIgnoreCase) ||
      tabType.Equals("emailTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["tabLabel"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Label"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("initialHereTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["optional"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Optional",
                    ["description"] = "true/false"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("fullNameTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["tabLabel"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Label"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  },
                  ["font"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-dynamic-values"] = new JObject
                      {
                        ["operationId"] = "StaticResponseForFontFaces",
                        ["value-collection"] = "fontNames",
                        ["value-path"] = "name",
                        ["value-title"] = "name"
                      },
                    ["x-ms-summary"] = "Font"
                  },
                  ["fontColor"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-dynamic-values"] = new JObject
                    {
                      ["operationId"] = "StaticResponseForFontColors",
                      ["value-collection"] = "fontNames",
                      ["value-path"] = "name",
                      ["value-title"] = "name"
                    },
                    ["x-ms-summary"] = "Font Color"
                  },
                  ["fontSize"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-dynamic-values"] = new JObject
                    {
                      ["operationId"] = "StaticResponseForFontSizes",
                      ["value-collection"] = "fontNames",
                      ["value-path"] = "name",
                      ["value-title"] = "name"
                    },
                    ["x-ms-summary"] = "Font Size"
                  },
                  ["bold"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Bold",
                    ["description"] = "true/false"
                  },
                  ["italic"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Italic",
                    ["description"] = "true/false"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("textTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                    ["x-ms-summary"] = "Anchor String"
                  },
                  ["value"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Value"
                  },
                  ["required"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Required",
                    ["description"] = "true/false"
                  },
                  ["locked"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Read Only",
                    ["description"] = "true/false"
                  },
                  ["validationPattern"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Validation Pattern",
                    ["description"] = "enter custom regex pattern"
                  },
                  ["validationMessage"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Validation Message"
                  },
                  ["tabLabel"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Label"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset"
                  },
                  ["font"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-dynamic-values"] = new JObject
                      {
                        ["operationId"] = "StaticResponseForFontFaces",
                        ["value-collection"] = "fontNames",
                        ["value-path"] = "name",
                        ["value-title"] = "name"
                      },
                    ["x-ms-summary"] = "Font"
                  },
                  ["fontColor"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-dynamic-values"] = new JObject
                    {
                      ["operationId"] = "StaticResponseForFontColors",
                      ["value-collection"] = "fontNames",
                      ["value-path"] = "name",
                      ["value-title"] = "name"
                    },
                    ["x-ms-summary"] = "Font Color"
                  },
                  ["fontSize"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-dynamic-values"] = new JObject
                    {
                      ["operationId"] = "StaticResponseForFontSizes",
                      ["value-collection"] = "fontNames",
                      ["value-path"] = "name",
                      ["value-title"] = "name"
                    },
                    ["x-ms-summary"] = "Font Size"
                  },
                  ["bold"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Bold",
                    ["description"] = "true/false"
                  },
                  ["italic"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Italic",
                    ["description"] = "true/false"
                  }
                }
              }
            }
          }
        };
      }
      if (tabType.Equals("titleTabs", StringComparison.OrdinalIgnoreCase))
      {
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
                  ["x-ms-summary"] = "Anchor String"
                },
                ["required"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "Required",
                  ["description"] = "true/false"
                },
                ["locked"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "Read Only",
                  ["description"] = "true/false"
                },
                ["tabLabel"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "Label"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "Anchor Y Offset"
                }
              }
              }
            }
          }
        };
      }
      if (tabType.Equals("tabGroups", StringComparison.OrdinalIgnoreCase))
      {
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
                  ["groupLabel"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Group Label"
                  },
                  ["documentId"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Document ID"
                  },
                  ["validationMessage"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Validation Message"
                  },
                  ["groupRule"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Group Rule",
                    ["description"] = "Select",
                    ["enum"] = new JArray ("SelectAtLeast", "SelectAtMost", "SelectExactly", "SelectARange")
                  },
                  ["minimumRequired"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Minimum Required"
                  },
                  ["maximumAllowed"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Maximum Allowed"
                  },
                  ["anchorXOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor X Offset"
                  },
                  ["anchorYOffset"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "Anchor Y Offset",
                  }
                }
              }
            }
          }
        };
      }
    }

    if (operationId.Equals("StaticResponseForBuildNumberSchema", StringComparison.OrdinalIgnoreCase))
    {
      response["name"] = "dynamicSchema";
      response["title"] = "dynamicSchema";
      response["schema"] = new JObject
      {
        ["type"] = "object",
        ["properties"] = new JObject()
      };

      response["schema"]["properties"]["Build Number"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "DS1008"
      };
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

    if (operationId.Equals("StaticResponseForEmbeddedSigningSchemaV2", StringComparison.OrdinalIgnoreCase))
    {
      var query = HttpUtility.ParseQueryString(context.Request.RequestUri.Query);
      var returnUrl = query.Get("returnUrl");
      var isInPersonSigner = query.Get("isInPersonSigner");

      response["name"] = "dynamicSchema";
      response["title"] = "dynamicSchema";
      response["schema"] = null;

      if (returnUrl.Equals("Add A Different URL", StringComparison.OrdinalIgnoreCase) && isInPersonSigner.Equals("Yes", StringComparison.OrdinalIgnoreCase))
      {
        response["schema"] = new JObject
        {
          ["type"] = "object",
          ["properties"] = new JObject(),
          ["required"] = new JArray("userName", "email", "recipientId", "returnURL")
        };
        response["schema"]["properties"]["userName"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Host name",
          ["description"] = "Host name needs to be sender name"
        };
        response["schema"]["properties"]["email"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Host email",
          ["description"] = "Host email needs to be sender email"
        };
        response["schema"]["properties"]["recipientId"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Recipient ID"
        };
        response["schema"]["properties"]["returnURL"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Add return URL"
        };
      }
      else if (returnUrl.Equals("Add A Different URL", StringComparison.OrdinalIgnoreCase) && isInPersonSigner.Equals("No", StringComparison.OrdinalIgnoreCase))
      {
        response["schema"] = new JObject
        {
          ["type"] = "object",
          ["properties"] = new JObject(),
          ["required"] = new JArray("userName", "email", "clientUserId", "returnURL")
        };
        response["schema"]["properties"]["userName"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Signer name"
        };
        response["schema"]["properties"]["email"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Signer email"
        };
        response["schema"]["properties"]["clientUserId"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Client user ID"
        };
        response["schema"]["properties"]["returnURL"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Add return URL"
        };
      }
      else
      {
        if (isInPersonSigner.Equals("No", StringComparison.OrdinalIgnoreCase)) {
        response["schema"] = new JObject
        {
          ["type"] = "object",
          ["properties"] = new JObject(),
          ["required"] = new JArray("userName", "email", "clientUserId")
        };
        response["schema"]["properties"]["userName"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Signer name"
        };
        response["schema"]["properties"]["email"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Signer email"
        };
        response["schema"]["properties"]["clientUserId"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Client user ID"
        };
      }
      if (isInPersonSigner.Equals("Yes", StringComparison.OrdinalIgnoreCase)) {
        response["schema"] = new JObject
        {
          ["type"] = "object",
          ["properties"] = new JObject(),
          ["required"] = new JArray("userName", "email", "recipientId")
        };
        response["schema"]["properties"]["userName"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Host name",
          ["description"] = "Host name needs to be sender name"
        };
        response["schema"]["properties"]["email"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Host email",
          ["description"] = "Host email needs to be sender email"
        };
        response["schema"]["properties"]["recipientId"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Recipient ID"
        };
      }
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

      if (returnUrl.Equals("Add a different URL", StringComparison.OrdinalIgnoreCase))
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

      if (verificationType.Equals("Phone Authentication"))
      {
        response["schema"]["properties"]["countryCode"] = new JObject 
        {
          ["type"] = "integer",
          ["x-ms-summary"] = "* Country Code, without the leading + sign."
        };
        response["schema"]["properties"]["phoneNumber"] = new JObject
        {
          ["type"] = "integer",
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
          ["description"] = "Select a verification workflow from the dropdown.",
          ["x-ms-summary"] = "* Verification workflow (IDV workflows with signature types are not supported in this action)"
        };        
      }
      else if (verificationType.Equals("Access Code"))
      {
        response["schema"]["properties"]["accessCode"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "* Access Code"
        };
      }
      else if (verificationType.Equals("ID Verification"))
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
          ["description"] = "Select a verification workflow from the dropdown.",
          ["x-ms-summary"] = "* Verification workflow (IDV workflows with signature types are not supported in this action)"
        };
      }
      else {
        response["schema"] = null;
      }
    }

    if (operationId.Equals("StaticResponseForRecipientTypeSchema", StringComparison.OrdinalIgnoreCase))
    {
      var query = HttpUtility.ParseQueryString(context.Request.RequestUri.Query);
      var recipientType = query.Get("recipientType");
      var signatureType = query.Get("signatureType") ?? "";

      response["name"] = "dynamicSchema";
      response["title"] = "dynamicSchema";
      response["schema"] = new JObject
      {
        ["type"] = "object",
        ["properties"] = new JObject()
      };

      if (signatureType.Equals("UniversalSignaturePen_OpenTrust_Hash_TSP", StringComparison.OrdinalIgnoreCase))
      {
        response["schema"]["properties"]["aesMethod"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "* AES Method",
          ["description"] = "AES Method",
          ["enum"] = new JArray("Access Code", "SMS <+ CountryCode PhoneNumber>")
        };
        response["schema"]["properties"]["aesMethodValue"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "* AES Method Value",
          ["description"] = "AES Method Value"
        };
      }

      if (recipientType.Equals("inPersonSigners", StringComparison.OrdinalIgnoreCase))
      {
        response["schema"]["properties"]["hostName"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "* Host name"
        };
        response["schema"]["properties"]["hostEmail"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "* Host email"
        };
        response["schema"]["properties"]["signerName"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "* Signer"
        };
      }
      else if (recipientType.Equals("signers", StringComparison.OrdinalIgnoreCase))
      {
        response["schema"]["properties"]["name"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "* Signer or signing group name"
        };
        response["schema"]["properties"]["email"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Signer email",
          ["description"] = "Signer email or SMS phone number is required"
        };
      }
      else if (recipientType.Equals("witnesses", StringComparison.OrdinalIgnoreCase))
      {
        response["schema"]["properties"]["witnessFor"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "* Witness for (Specify Recipient ID)"
        };        
        response["schema"]["properties"]["witnessName"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Witness name"
        };
        response["schema"]["properties"]["witnessEmail"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Witness email"
        };
      }
      else
      {
        response["schema"]["properties"]["name"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "* Recipient or signing group name"
        };
        response["schema"]["properties"]["email"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = "Recipient email (leave empty if there’s a signing group)"
        };
      }
    }

    // Composite Templates Request Body (please toggle up when not working on this part of code)
    if (operationId.Equals("StaticResponseForCompositeTemplates", StringComparison.OrdinalIgnoreCase))
    {
      var tabsJsonObj = new JObject
      {
        ["type"] = "object",
        ["properties"] = new JObject
        {
          ["- approveTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Approve",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["tabLabel"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- label"
                },
                ["buttonText"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Button Text"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- checkboxTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Checkbox",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["tabLabel"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Tab Label"
                },
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["anchorHorizontalAlignment"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Horizontal Alignment"
                },
                ["locked"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Read Only",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["selected"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Selected",
                  ["description"] = "true/false"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["- tabGroupLabels"] = new JObject
                {
                  ["type"] = "array",
                  ["x-ms-summary"] = "- Tab Group Labels",
                  ["items"] = new JObject
                    {
                      ["type"] = "string",
                      ["x-ms-summary"] = "",
                    }
                }
              }
            }
          },
          ["- tabGroups"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Checkbox Group",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["groupLabel"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Group Label"
                },
                ["documentId"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Document ID"
                },
                ["validationMessage"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Validation Message"
                },
                ["groupRule"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Group Rule",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("SelectAtLeast", "SelectAtMost", "SelectExactly", "SelectARange")
                },
                ["minimumRequired"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Minimum Required"
                },
                ["maximumAllowed"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Maximum Allowed"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset",
                }
              }
            }
          },
          ["- companyTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Company",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["locked"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Read Only",
                  ["description"] = "Select",
                  ["enum"] = new JArray("true", "false")
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- dateTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Date",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["value"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Value"
                },
                ["locked"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Read Only",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- dateSignedTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Date Signed",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["tabLabel"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Label"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- declineTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Decline",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["buttonText"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Button Text"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- listTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Dropdown",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["- listItems"] = new JObject
                {
                  ["type"] = "array",
                  ["items"] = new JObject
                    {
                      ["type"] = "object",
                      ["x-ms-summary"] = "- List Item",
                      ["properties"] = new JObject
                      {
                        ["selected"] = new JObject
                        {
                          ["type"] = "string",
                          ["x-ms-summary"] = "- Selected",
                          ["description"] = "true/false"
                        },
                        ["text"] = new JObject
                        {
                          ["x-ms-summary"] = "- Text",
                          ["type"] = "string"
                        },
                        ["value"] = new JObject
                        {
                          ["x-ms-summary"] = "- Value",
                          ["type"] = "string"
                        }
                      }
                    }
                },
                ["listSelectedValue"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Default Option"
                },
                ["locked"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Read Only",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["required"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Required",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["tooltip"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Tooltip"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- emailTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Email",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["tabLabel"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Label"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- firstNameTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "First Name",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- formulaTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Formula",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["formula"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Formula"
                },
                ["hidden"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Hidden",
                  ["description"] = "true/false"
                },
                ["roundDecimalPlaces"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Decimal places"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- fullNameTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Full Name",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["tabLabel"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Label"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                },
                ["font"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Font"
                },
                ["fontColor"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Font Color"
                },
                ["fontSize"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Font Size"
                },
                ["bold"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Bold",
                  ["description"] = "true/false"
                },
                ["italic"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Italic",
                  ["description"] = "true/false"
                }
              }
            }
          },
          ["- initialHereTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Initial",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["optional"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Optional",
                  ["description"] = "Select",
                  ["enum"] = new JArray("true", "false")
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- lastNameTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Last Name",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- noteTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Note",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["value"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Note Text"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- numberTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Number",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["value"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Value"
                },
                ["locked"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Read Only",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["required"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Required",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- numericalTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Numerical",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["numericalValue"] = new JObject
                {
                  ["x-ms-summary"] = "- Value",
                  ["type"] = "string"
                },
                ["locked"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Read Only",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["required"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Required",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["validationType"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Validation Type",
                  ["description"] = "Select",
                  ["enum"] = new JArray("Currency", "Number")
                },
                ["minNumericalValue"] = new JObject
                {
                  ["x-ms-summary"] = "- Minimum Amount",
                  ["type"] = "string"
                },
                ["maxNumericalValue"] = new JObject
                {
                  ["x-ms-summary"] = "- Maximum Amount",
                  ["type"] = "string"
                },
                ["- localePolicyTab"] = new JObject
                {
                  ["type"] = "array",
                  ["x-ms-summary"] = "- Locale Policy",
                  ["items"] = new JObject
                    {
                      ["type"] = "object",
                      ["x-ms-summary"] = "- Locale Policy",
                      ["properties"] = new JObject
                      {
                        ["cultureName"] = new JObject
                        {
                          ["type"] = "string",
                          ["x-ms-summary"] = "Culture Name",
                          ["description"] = "- The two letter ISO 639-1 language code.",
                        },
                        ["currencyCode"] = new JObject
                        {
                          ["type"] = "string",
                          ["x-ms-summary"] = "Currency Code",
                          ["description"] = "- The ISO 4217 currency code.",
                        },
                        ["currencyPositiveFormat"] = new JObject
                        {
                          ["type"] = "string",
                          ["x-ms-summary"] = "- Currency Positive Format"
                        },
                        ["currencyNegativeFormat"] = new JObject
                        {
                          ["type"] = "string",
                          ["x-ms-summary"] = "- Currency Negative Format"
                        },
                        ["useLongCurrencyFormat"] = new JObject
                        {
                          ["type"] = "string",
                          ["x-ms-summary"] = "- Use Long Currency Format",
                          ["description"] = "true/false",
                        }
                      }
                    }
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- radioGroupTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Radio Group",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["anchorHorizontalAlignment"] = new JObject
                {
                  ["x-ms-summary"] = "- Anchor Horizontal Alignment",
                  ["type"] = "string",
                  ["description"] = "left/right"
                },
                ["value"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Value"
                },
                ["selected"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Selected",
                  ["description"] = "true/false"
                },
                ["locked"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Read Only",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["required"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Required",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["anchorXOffset"] = new JObject
                {
                  ["x-ms-summary"] = "- Anchor X Offset",
                  ["type"] = "string"
                }
                ["anchorYOffset"] = new JObject
                {
                  ["x-ms-summary"] = "- Anchor Y Offset",
                  ["type"] = "string"
                }
              }
            }
          },
          ["- signHereTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Signature",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["optional"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Optional",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["tabLabel"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- label"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- signerAttachmentTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Signer Attachment",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["optional"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Optional",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["tabLabel"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- label"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- ssnTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- SSN",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["value"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Value"
                },
                ["locked"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Read Only",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["required"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Required",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- textTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Text",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["value"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Value"
                },
                ["required"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Required",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["locked"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Read Only",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["validationPattern"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Validation Pattern",
                  ["description"] = "enter custom regex pattern"
                },
                ["validationMessage"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Validation Message"
                },
                ["tabLabel"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Label"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                },
                ["font"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Font"
                },
                ["fontColor"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Font Color"
                },
                ["fontSize"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Font Size"
                },
                ["bold"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Bold",
                  ["description"] = "true/false"
                },
                ["italic"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Italic",
                  ["description"] = "true/false"
                }
              }
            }
          },
          ["- titleTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Title",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["required"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Required",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["locked"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Read Only",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["tabLabel"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Label"
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          },
          ["- zipTabs"] = new JObject
          {
            ["type"] = "array",
            ["x-ms-summary"] = "- Zip",
            ["items"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["anchorString"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor String"
                },
                ["value"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Value"
                },
                ["locked"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Read Only",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["required"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Required",
                  ["description"] = "Select",
                  ["enum"] = new JArray ("true", "false")
                },
                ["anchorXOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor X Offset"
                },
                ["anchorYOffset"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Anchor Y Offset"
                }
              }
            }
          }
        }
      };

      var query = HttpUtility.ParseQueryString(context.Request.RequestUri.Query);
      var templateId = query.Get("templateId") ?? "";

      response["name"] = "dynamicSchema";
      response["title"] = "dynamicSchema";
      response["schema"] = new JObject
      {
        ["type"] = "object",
        ["properties"] = new JObject()
      };
      response["schema"]["properties"]["compositeTemplates"] = new JObject
      {
        ["type"] = "array",
        ["x-ms-summary"] = "Composite Templates",
        ["items"] = new JObject
        {
          ["type"] = "object",
          ["properties"] = new JObject
          {
            ["serverTemplates"] = new JObject
            {
              ["type"] = "array",
              ["x-ms-summary"] = "Server templates",
              ["description"] = "Server templates",
              ["x-ms-visibility"] = "important",
              ["items"] = new JObject
              {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                  ["sequence"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "- sequence"
                  },
                  ["templateId"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "- Template ID"
                  }
                }
              }
            },
            ["document"] = new JObject
            {
              ["type"] = "object",
              ["properties"] = new JObject
              {
                ["documentId"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Document ID"
                },
                ["name"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Name"
                },
                ["fileExtension"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Document type",
                  ["description"] = "pdf, docx etc."
                },
                ["documentBase64"] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = "- Document base64"
                }
              }
            },
            ["inlineTemplates"] = new JObject
            {
              ["type"] = "array",
              ["x-ms-summary"] = "Inline Templates",
              ["description"] = "Inline Templates",
              ["items"] = new JObject
              {
                ["type"] = "object",
                ["properties"] = new JObject
                {
                  ["sequence"] = new JObject
                  {
                    ["type"] = "string",
                    ["x-ms-summary"] = "- sequence"
                  },
                  ["- Envelope Custom Fields"] = new JObject
                  {
                    ["type"] = "object",
                    ["x-ms-summary"] = "- Envelope Custom Fields",
                    ["properties"] = new JObject
                    {
                      ["- Text Custom Fields"] = new JObject
                      {
                        ["type"] = "array",
                        ["items"] = new JObject
                        {
                          ["type"] = "object",
                          ["x-ms-summary"] = "- Text Custom Fields",
                          ["properties"] = new JObject
                          {
                            ["name"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Name"
                            },
                            ["value"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Value"
                            },
                            ["show"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Show",
                              ["description"] = "Select",
                              ["enum"] = new JArray ("true", "false")
                            }
                          }
                        }
                      }
                    }
                  },
                  ["- recipients"] = new JObject
                  {
                    ["type"] = "object",
                    ["x-ms-summary"] = "recipients",
                    ["properties"] = new JObject
                    {
                      ["- Receives a Copy"] = new JObject
                      {
                        ["type"] = "array",
                        ["x-ms-summary"] = "- Receives a Copy",
                        ["items"] = new JObject
                        {
                          ["type"] = "object",
                          ["properties"] = new JObject
                          {
                            ["recipientId"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Recipient ID"
                            },
                            ["email"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Email"
                            },
                            ["- phoneNumber"] = new JObject
                            {
                              ["type"] = "object",
                              ["x-ms-summary"] = "- Phone Number",
                              ["properties"] = new JObject
                              {
                                ["countryCode"] = new JObject
                                {
                                  ["type"] = "string",
                                  ["x-ms-summary"] = "- Country Code"
                                },
                                ["number"] = new JObject
                                {
                                  ["type"] = "string",
                                  ["x-ms-summary"] = "- Number"
                                }
                              }
                            },
                            ["name"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Name"
                            },
                            ["roleName"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Role Name"
                            },
                            ["routingOrder"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Routing Order"
                            },
                            ["accessCode"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Access Code"
                            }
                          }
                        }
                      },
                      ["- Needs to View"] = new JObject
                      {
                        ["type"] = "array",
                        ["x-ms-summary"] = "- Needs to View",
                        ["items"] = new JObject
                        {
                          ["type"] = "object",
                          ["properties"] = new JObject
                          {
                            ["recipientId"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Recipient ID"
                            },
                            ["email"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Email"
                            },
                            ["- phoneNumber"] = new JObject
                            {
                              ["type"] = "object",
                              ["x-ms-summary"] = "- Phone Number",
                              ["properties"] = new JObject
                              {
                                ["countryCode"] = new JObject
                                {
                                  ["type"] = "string",
                                  ["x-ms-summary"] = "- Country Code"
                                },
                                ["number"] = new JObject
                                {
                                  ["type"] = "string",
                                  ["x-ms-summary"] = "- Number"
                                }
                              }
                            },
                            ["name"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Name"
                            },
                            ["roleName"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Role Name"
                            },
                            ["routingOrder"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Routing Order"
                            },
                            ["accessCode"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Access Code"
                            }
                          }
                        }
                      },
                      ["- Allow to Edit"] = new JObject
                      {
                        ["type"] = "array",
                        ["x-ms-summary"] = "- Allow to Edit",
                        ["items"] = new JObject
                        {
                          ["type"] = "object",
                          ["properties"] = new JObject
                          {
                            ["recipientId"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Recipient ID"
                            },
                            ["email"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Email"
                            },
                            ["name"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Name"
                            },
                            ["roleName"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Role Name"
                            },
                            ["routingOrder"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Routing Order"
                            },
                            ["accessCode"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Access Code"
                            }
                          }
                        }
                      },
                      ["- In Person Signer"] = new JObject
                      {
                        ["type"] = "array",
                        ["x-ms-summary"] = "- In Person Signer",
                        ["items"] = new JObject
                        {
                          ["type"] = "object",
                          ["properties"] = new JObject
                          {
                            ["recipientId"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Recipient ID"
                            },
                            ["hostEmail"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Host Email"
                            },
                            ["hostName"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Host Name"
                            },
                            ["signerEmail"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Signer Email"
                            },
                            ["signerName"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Signer Name"
                            },
                            ["roleName"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Role Name"
                            },
                            ["routingOrder"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Routing Order"
                            },
                            ["accessCode"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Access Code"
                            },
                            ["- tabs"] = tabsJsonObj.DeepClone()
                          }
                        }
                      },
                      ["- Update Recipients"] = new JObject
                      {
                        ["type"] = "array",
                        ["x-ms-summary"] = "- Update Recipients",
                        ["items"] = new JObject
                        {
                          ["type"] = "object",
                          ["properties"] = new JObject
                          {
                            ["recipientId"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Recipient ID"
                            },
                            ["email"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Email"
                            },
                            ["name"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Name"
                            },
                            ["roleName"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Role Name"
                            },
                            ["routingOrder"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Routing Order"
                            },
                            ["accessCode"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Access Code"
                            }
                          }
                        }
                      },
                      ["- Signs with Witness"] = new JObject
                      {
                        ["type"] = "array",
                        ["x-ms-summary"] = "- Signs with Witness",
                        ["items"] = new JObject
                        {
                          ["type"] = "object",
                          ["properties"] = new JObject
                          {
                            ["recipientId"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Recipient ID"
                            },
                            ["email"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Email"
                            },
                            ["name"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Name"
                            },
                            ["witnessFor"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Witness For"
                            },
                            ["roleName"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Role Name"
                            },
                            ["routingOrder"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Routing Order"
                            },
                            ["accessCode"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Access Code"
                            },
                            ["- tabs"] = tabsJsonObj.DeepClone()
                          }
                        }
                      },
                      ["- Needs to Sign"] = new JObject
                      {
                        ["type"] = "array",
                        ["x-ms-summary"] = "- Needs to Sign",
                        ["items"] = new JObject
                        {
                          ["type"] = "object",
                          ["properties"] = new JObject
                          {
                            ["recipientId"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Recipient ID"
                            },
                            ["email"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Email"
                            },
                            ["name"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Name"
                            },
                            ["roleName"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Role Name"
                            },
                            ["routingOrder"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Routing Order"
                            },
                            ["accessCode"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Access Code"
                            },
                            ["clientUserId"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Client User ID"
                            },
                            ["- phoneNumber"] = new JObject
                            {
                              ["type"] = "object",
                              ["x-ms-summary"] = "- Phone Number",
                              ["properties"] = new JObject
                              {
                                ["countryCode"] = new JObject
                                {
                                  ["type"] = "string",
                                  ["x-ms-summary"] = "- Country Code"
                                },
                                ["number"] = new JObject
                                {
                                  ["type"] = "string",
                                  ["x-ms-summary"] = "- Number"
                                }
                              }
                            },
                            ["- recipientSignatureProviders"] = new JObject
                            {
                              ["type"] = "array",
                              ["x-ms-summary"] = "- Recipient Signature Providers",
                              ["items"] = new JObject
                              {
                                ["type"] = "object",
                                ["properties"] = new JObject
                                {
                                  ["signatureProviderName"] = new JObject
                                  {
                                    ["type"] = "string",
                                    ["x-ms-summary"] = "- Signature Provider Name"
                                  },
                                  ["- signatureProviderOptions"] = new JObject
                                  {
                                    ["type"] = "object",
                                    ["properties"] = new JObject
                                    {
                                      ["SMS"] = new JObject
                                      {
                                        ["type"] = "string",
                                        ["x-ms-summary"] = "- SMS"
                                      }
                                    }
                                  }
                                }
                              }
                            },
                            ["- identityVerification"] = new JObject
                            {
                              ["type"] = "object",
                              ["properties"] = new JObject
                              {
                                ["workflowId"] = new JObject
                                {
                                  ["type"] = "string",
                                  ["x-ms-summary"] = "- Workflow ID"
                                },
                                ["- inputOptions"] = new JObject
                                {
                                  ["type"] = "array",
                                  ["x-ms-summary"] = "- Input Options",
                                  ["items"] = new JObject
                                  {
                                    ["type"] = "object",
                                    ["properties"] = new JObject
                                    {
                                      ["name"] = new JObject
                                      {
                                        ["type"] = "string",
                                        ["x-ms-summary"] = "- Name"
                                      },
                                      ["valueType"] = new JObject
                                      {
                                        ["type"] = "string",
                                        ["x-ms-summary"] = "- Value Type"
                                      },
                                      ["- phoneNumberList"] = new JObject
                                      {
                                        ["type"] = "array",
                                        ["x-ms-summary"] = "- Phone Number List",
                                        ["items"] = new JObject
                                        {
                                          ["type"] = "object",
                                          ["properties"] = new JObject
                                          {
                                            ["countryCode"] = new JObject
                                            {
                                              ["type"] = "string",
                                              ["x-ms-summary"] = "- Country Code"
                                            },
                                            ["number"] = new JObject
                                            {
                                              ["type"] = "string",
                                              ["x-ms-summary"] = "- Number"
                                            },
                                            ["extension"] = new JObject
                                            {
                                              ["type"] = "string",
                                              ["x-ms-summary"] = "- Extension"
                                            }
                                          }
                                        }
                                      }
                                    }
                                  }
                                }
                              }
                            },
                            ["signingGroupId"] = new JObject
                            {
                              ["type"] = "string",
                              ["x-ms-summary"] = "- Signing Group ID"
                            },
                            ["- tabs"] = tabsJsonObj.DeepClone()
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      };
    }
    // end of Composite Templates Request Body
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
  
  private static JArray ParseContentAsJArray(string content, bool isRequest)
  {
    JArray body;
    try
    {
      body = JArray.Parse(content);
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

  private static string TransformWebhookNotificationBodyDeprecated(string content)
  {
    JObject body = ParseContentAsJObject(content, true);

    // customfield code
    if (body["DocuSignEnvelopeInformation"] is JObject && body["DocuSignEnvelopeInformation"]["EnvelopeStatus"] is JObject)
    {
        var envelopeStatus = body["DocuSignEnvelopeInformation"]["EnvelopeStatus"];
        var customFields = envelopeStatus["CustomFields"];
        var newCustomFields = new JObject();

        if (customFields is JObject)
        {
            var customFieldsArray = customFields["CustomField"];
            customFieldsArray = customFieldsArray is JObject ? new JArray(customFieldsArray) : customFieldsArray;
            customFields["CustomField"] = customFieldsArray;

            foreach (var field in customFieldsArray as JArray ?? new JArray())
            {
                var fieldName = field.Type == JTokenType.Object ? (string)field["Name"] : null;
                if (!string.IsNullOrWhiteSpace(fieldName) && newCustomFields[fieldName] == null)
                {
                    newCustomFields.Add(fieldName, field["Value"]);
                }
            }
        }

        body["customFields"] = newCustomFields;

        // tab code
        var recipientStatuses = envelopeStatus["RecipientStatuses"];
        if (recipientStatuses is JObject)
        {
            var statusArray = recipientStatuses["RecipientStatus"];
            statusArray = statusArray is JObject ? new JArray(statusArray) : statusArray;
            recipientStatuses["RecipientStatus"] = statusArray;

            // RecipientStatus is an array at this point so now check TabStatus
            foreach (var recipient in recipientStatuses["RecipientStatus"] ?? new JArray())
            {
                var tabStatuses = recipient["TabStatuses"];
                if (tabStatuses is JObject)
                {
                    var tabStatusArray = tabStatuses["TabStatus"];
                    tabStatusArray = tabStatusArray is JObject ? new JArray(tabStatusArray) : tabStatusArray;
                    tabStatuses["TabStatus"] = tabStatusArray;

                    // TabStatus is an array at this point
                    var newTabStatuses = new JObject();
                    foreach (var tab in tabStatusArray as JArray ?? new JArray())
                    {
                        if (tab is JObject)
                        {
                            var tabLabel = (string)tab["TabLabel"];
                            var tabValue = (string)tab["TabValue"];
                            var customTabType = (string)tab["CustomTabType"];

                            // skip Radio and List tabs that are not selected
                            if (!string.IsNullOrWhiteSpace(tabLabel) && !string.IsNullOrWhiteSpace(tabValue) && customTabType != "Radio" && customTabType != "List")
                            {
                                if (newTabStatuses[tabLabel] == null)
                                {
                                    newTabStatuses.Add(tabLabel, tabValue);
                                }
                            }
                        }
                    }

                    recipient["tabs"] = newTabStatuses;
                }
            }
        }
    }

    return body.ToString();
  }

  private static void ParseCustomFields(JToken customFields, JObject parsedCustomFields)
  {
    var customFieldsArray = customFields is JObject ? new JArray(customFields) : customFields;

    foreach (var field in customFieldsArray as JArray ?? new JArray())
    {
      var fieldName = field.Type == JTokenType.Object ? (string)field["name"] : null;
      if (!string.IsNullOrWhiteSpace(fieldName) && parsedCustomFields[fieldName] == null)
      {
        parsedCustomFields.Add(fieldName, field["value"]);
      }
    }
  }
  
  private static string TransformWebhookNotificationBody(string content)
  {
    JObject body = ParseContentAsJObject(content, true);

    // customfield code
    if (body["data"] is JObject && body["data"]["envelopeSummary"] is JObject)
    {
      var envelopeSummary = body["data"]["envelopeSummary"];
      var customFields = envelopeSummary["customFields"];
      var parsedCustomFields = new JObject();

      if (customFields is JObject)
      {
        var textCustomFields = customFields["textCustomFields"];
        ParseCustomFields(textCustomFields, parsedCustomFields);

        var listCustomFields = customFields["listCustomFields"];
        ParseCustomFields(listCustomFields, parsedCustomFields);
      }

      body["data"]["envelopeSummary"]["customFields"] = parsedCustomFields;

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
              "formulaTabs", "checkboxTabs", "radioGroupTabs" };
            foreach (var tabType in tabTypes)
            {
              var tabStatusArray = tabs[tabType];
              foreach (var tab in tabStatusArray as JArray ?? new JArray())
              {
                if (tab is JObject)
                {
                  if (tabType.Equals("checkboxTabs"))
                  {
                    if (newTabs[(string)tab["tabLabel"]] == null)
                    {
                      newTabs.Add((string)tab["tabLabel"], (string)tab["selected"]);
                    }
                  }

                  var tabValue = (string)tab["value"];
                  if (tabType.Equals("radioGroupTabs") && !string.IsNullOrWhiteSpace(tabValue))
                  {
                    var tabGroupName = (string)tab["groupName"];
                    if (newTabs[tabGroupName] == null)
                    {
                      newTabs.Add(tabGroupName, (string)tab["value"]);
                    }
                  }

                  var tabLabel = (string)tab["tabLabel"];
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
    var notificationContent = "";
    if (content.Contains("DocuSignEnvelopeInformation"))
    {
      var doc = new XmlDocument();
      doc.LoadXml(content);
      var jsonContent = JsonConvert.SerializeXmlNode(doc);
      notificationContent = TransformWebhookNotificationBodyDeprecated(jsonContent);
    }
    else
    {
      notificationContent = TransformWebhookNotificationBody(content);
    }

    using var logicAppsRequest = new HttpRequestMessage(HttpMethod.Post, logicAppsUri);
    logicAppsRequest.Content = CreateJsonContent(notificationContent);

    return await this.Context.SendAsync(logicAppsRequest, this.CancellationToken).ConfigureAwait(false);
  }

  private JObject CreateHookEnvelopeV2BodyTransformation(JObject original)
  {
    var body = new JObject();
    
    var uriLogicApps = original["urlToPublishTo"]?.ToString();
    var uriLogicAppsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(uriLogicApps ?? string.Empty));
    var notificationProxyUri = this.Context.CreateNotificationUri($"/webhook_response?logicAppsUri={uriLogicAppsBase64}");

    // TODO: This map is added for backward compatibility. This will be removed once old events are deprecated
    var envelopeEventMap = new Dictionary<string, string>() {
      {"Sent", "envelope-sent"},
      {"Delivered", "envelope-delivered"},
      {"Completed", "envelope-completed"},
      {"Declined", "envelope-declined"},
      {"Voided", "envelope-voided"}
    };

    body["allUsers"] = "true";
    body["allowEnvelopePublish"] = "true";
    body["includeDocumentFields"] = "true";
    body["requiresAcknowledgement"] = "true";
    body["urlToPublishTo"] = notificationProxyUri.AbsoluteUri;
    body["name"] = original["name"]?.ToString();

    var envelopeEvent = original["envelopeEvents"]?.ToString();
    var webhookEvent = envelopeEventMap.ContainsKey(envelopeEvent) ? envelopeEventMap[envelopeEvent] : envelopeEvent;

    var webhookEventsArray = new JArray();
    webhookEventsArray.Add(webhookEvent);
    body["events"] = webhookEventsArray;
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
    
    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
    uriBuilder.Path = uriBuilder.Path.Replace("connectV2", "connect");
    this.Context.Request.RequestUri = uriBuilder.Uri;
    return body;
  }
  
  private JObject CreateHookEnvelopeV3BodyTransformation(JObject original)
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
    body["events"] = envelopeEventsArray;
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
    
    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
    uriBuilder.Path = uriBuilder.Path.Replace("connectV3", "connect");
    this.Context.Request.RequestUri = uriBuilder.Uri;
    return body;
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
    body["includeEnvelopeVoidReason"] = "true";
    body["includeTimeZoneInformation"] = "true";
    body["requiresAcknowledgement"] = "true";
    body["urlToPublishTo"] = notificationProxyUri.AbsoluteUri;
    body["name"] = original["name"]?.ToString();
    body["envelopeEvents"] = original["envelopeEvents"]?.ToString();
    body["includeSenderAccountasCustomField"] = "true";
    
    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
    uriBuilder.Path = uriBuilder.Path.Replace("v2.1", "v2");
    this.Context.Request.RequestUri = uriBuilder.Uri;

    return body;
  }

  private JObject AddRemindersBodyTransformation(JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var expiryAfter = string.IsNullOrEmpty(query.Get("expireAfter")) ?
          "120" : query.Get("expireAfter");

    var newBody = new JObject()
    {
      ["useAccountDefaults"] = "false",
      ["reminders"] = new JObject()
      {
        ["reminderDelay"] = query.Get("reminderDelay"),
        ["reminderEnabled"] = query.Get("reminderEnabled"),
        ["reminderFrequency"] = query.Get("reminderFrequency")
      },
      ["expirations"] = new JObject()
      {
        ["expireAfter"] = expiryAfter
      }
    };

    return newBody;
  }

  private JObject CreateEnvelopeFromTemplateV1BodyTransformation(JObject body)
  {
    var templateRoles = new JArray();
    var signer = new JObject();
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);

    var newBody = new JObject()
    {
      ["templateId"] = query.Get("templateId"),
      ["emailSubject"] = query.Get("emailSubject"),
      ["emailBlurb"] = query.Get("emailBody")
    };

    foreach (var property in body)
    {
      var value = (string)property.Value;
      var key = (string)property.Key;

      if (key.Contains(" Name"))
      {
        signer["roleName"] = key.Substring(0, key.Length - 5);
        signer["name"] = value;
      }

      //add every (name, email) pairs
      if (key.Contains(" Email"))
      {
        signer["email"] = value;
        templateRoles.Add(signer);
        signer = new JObject();
      }
    }

    newBody["templateRoles"] = templateRoles;

    if (!string.IsNullOrEmpty(query.Get("status")))
    {
      newBody["status"] = query.Get("status");
    }

    return newBody;
  }

  private JObject CreateEnvelopeFromTemplateV3BodyTransformation(JObject body)
  {
    var templateRoles = new JArray();
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);

    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
    uriBuilder.Path = uriBuilder.Path.Replace("/envelopes/createWithRecipientFields", "/envelopes");
    this.Context.Request.RequestUri = uriBuilder.Uri;

    var newBody = new JObject()
    {
      ["templateId"] = query.Get("templateId"),
      ["emailSubject"] = query.Get("emailSubject"),
      ["emailBlurb"] = body["emailBlurb"]
    };

    Dictionary<string, JObject> recipientMapping = new Dictionary<string, JObject>();
    foreach (var property in body)
    {
      var value = (string)property.Value;
      var key = (string)property.Key;
      if (key.Equals("emailBlurb"))
      {
        continue;
      }
      string[] keyArray = key.Split(new string[]{":::"}, StringSplitOptions.None);
      var roleName = keyArray[0];
      // custom fields parsing to match request body object from Docusign API
      if (string.Equals(keyArray[0], "List Custom Fields", StringComparison.OrdinalIgnoreCase))
      {
        if (!newBody.ContainsKey("customFields"))
        {
          newBody["customFields"] = new JObject();
        }
        JObject recipientCustomFieldObj = (JObject) newBody["customFields"];
        if (!recipientCustomFieldObj.ContainsKey("listCustomFields"))
        {
          recipientCustomFieldObj["listCustomFields"] = new JArray();
        }
        JArray listCustomFieldsArray = (JArray) recipientCustomFieldObj["listCustomFields"];
        listCustomFieldsArray.Add(new JObject
        {
          ["name"] = keyArray[1],
          ["value"] = value,
          ["show"] = "true"
        });
        continue;
      }
      if (string.Equals(keyArray[0], "Text Custom Fields", StringComparison.OrdinalIgnoreCase))
      {
        if (!newBody.ContainsKey("customFields"))
        {
          newBody["customFields"] = new JObject();
        }
        JObject recipientCustomFieldObj = (JObject) newBody["customFields"];
        if (!recipientCustomFieldObj.ContainsKey("textCustomFields"))
        {
          recipientCustomFieldObj["textCustomFields"] = new JArray();
        }
        JArray textCustomFieldsArray = (JArray) recipientCustomFieldObj["textCustomFields"];
        textCustomFieldsArray.Add(new JObject
        {
          ["name"] = keyArray[1],
          ["value"] = value,
          ["show"] = "true"
        });
        continue;
      }

      // template roles parsing to match request body object from Docusign API
      ParseRecipientFields(recipientMapping, keyArray, value, roleName);
    }

    foreach (JObject value in recipientMapping.Values)
    {
      templateRoles.Add(value);
    }

    newBody["templateRoles"] = templateRoles;
    return newBody;
  }

  private JObject CreateEnvelopeFromTemplateV2BodyTransformation(JObject body)
  {
    var templateRoles = new JArray();
    var signer = new JObject();
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var textCustomFields = new JArray();
    var listCustomFields = new JArray();

    ParseCustomFields(body, textCustomFields, listCustomFields);

    var newBody = new JObject()
    {
      ["templateId"] = query.Get("templateId"),
      ["customFields"] = new JObject()
        {
          ["textCustomFields"] = textCustomFields,
          ["listCustomFields"] = listCustomFields
        }
    };

    if (!string.IsNullOrEmpty(query.Get("status")))
    {
      newBody["status"] = query.Get("status");
    }

    return newBody;
  }

  private JObject CreateEnvelopeFromTemplateNoRecipientsBodyTransformation(JObject body)
  {
    var templateRoles = new JArray();
    var signer = new JObject();
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);

    var newBody = new JObject()
    {
      ["templateId"] = query.Get("templateId")
    };

    if (!string.IsNullOrEmpty(query.Get("status")))
    {
      newBody["status"] = query.Get("status");
    }

    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
    uriBuilder.Path = uriBuilder.Path.Replace("/envelopes/createFromTemplateNoRecipients", "/envelopes");
    this.Context.Request.RequestUri = uriBuilder.Uri;

    return newBody;
  }

  private JObject UpdateEnvelopeCustomFieldBodyTransformation(JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var textCustomFields = new JArray();
    var listCustomFields = new JArray();
    var fieldType = query.Get("fieldType");
    var customField = new JObject() {
      ["fieldId"] = query.Get("fieldId"),
	    ["name"] = query.Get("name"),
	    ["value"] = query.Get("value")
    };

    if (fieldType.Equals("Text"))
    {
      textCustomFields.Add(customField);
    }

    if (fieldType.Equals("List"))
    {
      listCustomFields.Add(customField);
    }

    var newBody = new JObject()
    {
      ["textCustomFields"] = textCustomFields,
      ["listCustomFields"] = listCustomFields
    };

    return newBody;
  }
  
  private async Task UpdateEnvelopePrefillTabsBodyTransformation()
  {
    var body = ParseContentAsJArray(await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false), true);
    var tabs = new JObject();

    foreach (var tab in body)
    {
      var tabType = tab["tabType"].ToString();
      var tabsForType = tabs[tabType] as JArray ?? new JArray();
      
      tabsForType.Add(new JObject
        {
          ["tabId"] = tab["tabId"],
          ["value"] = tab["value"]
        });

      tabs[tabType] = tabsForType;
    }

    var newBody = new JObject()
    {
      ["prefillTabs"] = tabs
    };

    this.Context.Request.Content = CreateJsonContent(newBody.ToString());
  }

  private void ParseCustomFields(JObject body, JArray textCustomFields,  JArray listCustomFields)
  {
    foreach (var property in body)
    {
      var customField = new JObject();
      var key = (string)property.Key;
      var value = (string)property.Value;

      if (key.StartsWith("* "))
      {
        customField["required"] = "true";
        key = key.Replace("* ", "");
      }
      else
      {
        key = key.Replace(" [optional]", "");
      }

      if (key.EndsWith(" [hidden]"))
      {
        key = key.Replace(" [hidden]", "");
      }
      else
      {
        customField["show"] = "true";
      }

      if (key.EndsWith("[Custom Field List]"))
      {
        key = key.Replace(" [Custom Field List]", "");
        customField["name"] = key;
        customField["value"] = value;
        listCustomFields.Add(customField);
      }
      else
      {
        key = key.Replace(" [Custom Field Text]", "");
        customField["name"] = key;
        customField["value"] = value;
        textCustomFields.Add(customField);
      }
    }
  }
  
  private JObject CreateBlankEnvelopeBodyTransformation(JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var textCustomFields = new JArray();
    var listCustomFields = new JArray();

    ParseCustomFields(body, textCustomFields, listCustomFields);

    body["customFields"] = new JObject()
    {
      ["textCustomFields"] = textCustomFields,
      ["listCustomFields"] = listCustomFields
    };

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

  private JObject CompositeTemplatesBodyTransformation(JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);

    body["emailSubject"] = query.Get("emailSubject");
    var emailBody = query.Get("emailBody");

    if (!string.IsNullOrEmpty(emailBody))
    {
      body["emailBlurb"] = emailBody;
    }

    if (!string.IsNullOrEmpty(query.Get("status")))
    {
      body["status"] = query.Get("status");
    }

    RenameKeysWithoutDashes(body);

    var keyMappings = new Dictionary<string, string> { 
      { "Envelope Custom Fields", "customFields" },
      { "Text Custom Fields", "textCustomFields" },
      { "Receives a Copy", "carbonCopies" },
      { "Needs to View", "certifiedDeliveries" },
      { "Allow to Edit", "editors" },
      { "In Person Signer", "inPersonSigners" },
      { "Update Recipients", "intermediaries" },
      { "Signs with Witness", "witnesses" },
      { "Needs to Sign", "signers" }
    };

    RenameSpecificKeys(body, keyMappings);

    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
    uriBuilder.Path = uriBuilder.Path.Replace("/envelopes/compositeTemplates", "/envelopes");
    this.Context.Request.RequestUri = uriBuilder.Uri;

    return body;
  }

// Removes all dashes (-) appended to field names
private void RenameKeysWithoutDashes(JObject jObject)
{
    var propertiesToRename = new List<JProperty>();

    foreach (var property in jObject.Properties())
    {
        if (property.Name.StartsWith("-"))
        {
            propertiesToRename.Add(property);
        }

        if (property.Value is JObject nestedObject)
        {
            RenameKeysWithoutDashes(nestedObject);
        }
        else if (property.Value is JArray array)
        {
            foreach (var item in array)
            {
                if (item is JObject arrayObject)
                {
                    RenameKeysWithoutDashes(arrayObject);
                }
            }
        }
    }

    foreach (var property in propertiesToRename)
    {
        var newKey = property.Name.TrimStart('-', ' ');
        jObject[newKey] = property.Value;
        jObject.Remove(property.Name);
    }
}

private void RenameSpecificKeys(JObject jObject, Dictionary<string, string> keyMappings)
{
    var propertiesToRename = new List<JProperty>();

    // Collect properties that need renaming based on the provided keyMappings
    foreach (var property in jObject.Properties())
    {
        if (keyMappings.ContainsKey(property.Name))
        {
            propertiesToRename.Add(property);
        }

        // If the property is a nested object, recursively call the function
        if (property.Value is JObject nestedObject)
        {
            RenameSpecificKeys(nestedObject, keyMappings);
        }
        else if (property.Value is JArray array)
        {
            foreach (var item in array)
            {
                if (item is JObject arrayObject)
                {
                    RenameSpecificKeys(arrayObject, keyMappings);
                }
            }
        }
    }

    // Rename collected properties
    foreach (var property in propertiesToRename)
    {
        var newKey = keyMappings[property.Name];
        jObject[newKey] = property.Value;
        jObject.Remove(property.Name);
    }
}

  private JObject EnvelopeVoidBodyTransformation(JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);

    body["status"] = "Voided";
    body["voidedReason"] = query.Get("voidedReason");

    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
    uriBuilder.Path = uriBuilder.Path.Replace("/voidEnvelope", "");
    this.Context.Request.RequestUri = uriBuilder.Uri;

    return body;
  }

  private JObject EnvelopeResendBodyTransformation(JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
    uriBuilder.Path = uriBuilder.Path.Replace("/resendEnvelope", "");
    uriBuilder.Path = uriBuilder.Path.Replace("copilotAccount", this.Context.Request.Headers.GetValues("AccountId").FirstOrDefault());
    
    query["resend_envelope"] = "true";
    uriBuilder.Query = query.ToString();
    this.Context.Request.RequestUri = uriBuilder.Uri;
    return body;
  }

  private JObject SearchListEnvelopesTransformation(JObject body)
  { 
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      uriBuilder.Path = uriBuilder.Path.Replace("/SearchListEnvelopes", "");

      var orderByMapping = new Dictionary<string, string> { 
      { "Action required", "action_required" },
      { "Created", "created" },
      { "Completed", "completed" },
      { "Envelope name", "envelope_name" },
      { "Expire", "expire" },
      { "Last modified", "last_modified" },
      { "Sent", "sent" },
      { "Signer list", "signer_list" },
      { "Status", "status" },
      { "Subject", "subject" },
      { "User name", "user_name" },
      { "Status changed", "status_changed" }
    };

      var folderIDMapping = new Dictionary<string, string> {
      { "Awaiting my signature", "awaiting_my_signature" },
      { "Completed", "completed" },
      { "Draft", "draft" },
      { "Drafts", "drafts" },
      { "Expiring soon", "expiring_soon" },
      { "Inbox", "inbox" },
      { "Out for signature", "out_for_signature" },
      { "Recycle bin", "recyclebin" },
      { "Sent items", "sent_items" },
      { "Waiting for others", "waiting_for_others" }
    };

      var envelopeStatusMapping = new Dictionary<string, string> {
      { "Any", "any" },
      { "Created", "created" },
      { "Sent", "sent" },
      { "Delivered", "delivered" },
      { "Signed", "signed" },
      { "Completed", "completed" },
      { "Declined", "declined" },
      { "Voided", "voided" },
      { "Deleted", "deleted" }
    };
      query["include"] = "custom_fields, recipients, documents, folders";
      query["order"] = "desc";

      query["status"] = string.IsNullOrEmpty(query.Get("envelopeStatus")) ? 
        null : envelopeStatusMapping[query.Get("envelopeStatus")];
      query["folder_ids"] = string.IsNullOrEmpty(query.Get("folder_ids")) ? 
        null : folderIDMapping[query.Get("folder_ids").ToString()];
       query["order_by"] = string.IsNullOrEmpty(query.Get("order_by")) ? 
        "status_changed" : orderByMapping[query.Get("order_by")];
      query["from_date"] = string.IsNullOrEmpty(query.Get("from_date")) ? 
        "2000-01-02T12:45Z" : query.Get("from_date");
      query["to_date"] = string.IsNullOrEmpty(query.Get("to_date")) ? 
        DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : query.Get("to_date");

      uriBuilder.Query = query.ToString();
      this.Context.Request.RequestUri = uriBuilder.Uri;

    return body;
  }

  private JObject AddRecipientToEnvelopeBodyTransformation(JObject body)
  {
      var signers = body["signers"] as JArray;
      if (signers == null || signers.Count == 0)
      {
          signers = new JArray
          {
              new JObject(),
          };
      }

      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      signers[0]["name"] = Uri.UnescapeDataString(query.Get("AddRecipientToEnvelopeName")).Replace("+", " ");
      signers[0]["email"] = Uri.UnescapeDataString(query.Get("AddRecipientToEnvelopeEmail")).Replace("+", " ");
      if (string.IsNullOrWhiteSpace((string)signers[0]["recipientId"]))
      {
          signers[0]["recipientId"] = Guid.NewGuid();
      }

      body["signers"] = signers;
      return body;
  }

  private JObject AddRecipientToEnvelopeV2BodyTransformation(JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var recipientType = query.Get("recipientType");
    
    var signers = new JArray
    {
      new JObject(),
    };

    AddCoreRecipientParams(signers, body);
    bool missingInput = AddParamsForSelectedRecipientType(signers, body);

    if (!string.IsNullOrEmpty(query.Get("embeddedRecipientStartURL")))
    {
      signers[0]["embeddedRecipientStartURL"] = query.Get("embeddedRecipientStartURL").ToString();
    }

    if (!string.IsNullOrEmpty(query.Get("signatureType")))
    {
      AddParamsForSelectedSignatureType(signers, body);
    }

    body[recipientType] = signers;

    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
    uriBuilder.Path = uriBuilder.Path.Replace("/recipients/addRecipientV2", "/recipients");
    uriBuilder.Path = uriBuilder.Path.Replace("/recipients/updateRecipient", "/recipients");
    this.Context.Request.RequestUri = uriBuilder.Uri;

    if (missingInput) {
      throw new ConnectorException(HttpStatusCode.BadRequest, "ValidationFailure: at least one of email or phone number is required");
    }

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

  private JObject GenerateEmbeddedSigningURLV2BodyTransformation (JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    
    body["authenticationMethod"] = query.Get("authenticationMethod");
    
    var returnUrl = query.Get("returnUrl");
    if (returnUrl.Equals("Default URL"))
    {
      body["returnUrl"] = "https://postsign.docusign.com/postsigning/en/finish-signing";
    }
    else if (returnUrl.Equals("Add A Different URL"))
    {
      body["returnUrl"] = body["returnURL"];
    }

    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
    uriBuilder.Path = uriBuilder.Path.Replace("/recipientV2", "/recipient");
    this.Context.Request.RequestUri = uriBuilder.Uri;

    return body;
  }

  private JObject GenerateEmbeddedSenderURLBodyTransformation (JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var returnUrl = query.Get("returnUrl");
    var url = this.Context.Request.RequestUri.Authority;

    if (returnUrl.Equals("DocuSign homepage"))
    {
      if (url.Equals("demo.docusign.net"))
      {
        body["returnUrl"] = "https://appdemo.docusign.com/";
      }
      else
      {
        body["returnUrl"] = "https://app.docusign.com/";
      }
    }
    else
    {
      body["returnUrl"] = body["returnURL"];
    }
    return body;
  }

  private JObject AddVerificationToRecipientBodyTransformation (JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var verificationType = query.Get("verificationType");
    var recipientTypeMap = new Dictionary<string, string>() {
      {"agent", "agents"},
      {"editor", "editors"},
      {"inpersonsigner", "inPersonSigners"},
      {"certifieddelivery", "certifiedDeliveries"},
      {"signer", "signers"},
      {"carboncopy", "carbonCopies"},
      {"intermediary", "intermediaries"},
      {"witness", "witnesses"}
    };

    var recipientType = recipientTypeMap[query.Get("recipientType")];
    var recipientId = query.Get("recipientId");

    var recipient = new JObject();
    var recipientArray = new JArray();

    if (verificationType.Equals("Phone Authentication"))
    {
      var phoneNumbers = new JArray();

      if (body["phoneNumber"] == null || body["countryCode"] == null)
      {
        throw new ConnectorException(HttpStatusCode.BadRequest, "ValidationFailure: Phone number or country code is missing");
      }

      var identityVerification = new JObject();
      var inputOptions = new JArray();
      var inputObject = new JObject();
      var phoneNumberObject = new JObject();

      if (body["workflowID"] == null)
      {
        throw new ConnectorException(HttpStatusCode.BadRequest, "ValidationFailure: Workflow ID is missing");
      }

      phoneNumberObject["Number"] = body["phoneNumber"];
      phoneNumberObject["CountryCode"] = body["countryCode"];
      phoneNumbers.Add(phoneNumberObject);

      inputObject["phoneNumberList"] = phoneNumbers;
      inputObject["name"] = "phone_number_list";
      inputObject["valueType"] = "PhoneNumberList";
      inputOptions.Add(inputObject);

      identityVerification["workflowId"] = body["workflowID"];
      identityVerification["inputOptions"] = inputOptions;
      recipient["identityVerification"] = identityVerification;
    }
    else if (verificationType.Equals("Access Code"))
    {
      if (body["accessCode"] == null)
      {
        throw new ConnectorException(HttpStatusCode.BadRequest, "ValidationFailure: Access Code is missing");
      }

      recipient["accessCode"] = body["accessCode"];
    }
    else if (verificationType.Equals("Knowledge Based"))
    {
      recipient["idCheckConfigurationName"] = "ID Check $";
    }
    else if (verificationType.Equals("ID Verification"))
    {
      var identityVerification = new JObject();
      if (body["workflowID"] == null)
      {
        throw new ConnectorException(HttpStatusCode.BadRequest, "ValidationFailure: Workflow ID is missing");
      }

      identityVerification["workflowId"] = body["workflowID"];
      recipient["identityVerification"] = identityVerification;
    }
    
    recipient["recipientId"] = recipientId;
    recipientArray.Add(recipient);
    body[recipientType] = recipientArray;

    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
    uriBuilder.Path = uriBuilder.Path.Replace("/recipients/addRecipientV2", "/recipients");
    this.Context.Request.RequestUri = uriBuilder.Uri;

    return body;
  }

  private string GetDescriptionNLPForRelatedActivities(JToken envelope)
  {
    JToken recipientsToken = envelope["recipients"];
    JToken signersToken = recipientsToken?["signers"];
    JArray signersArray = signersToken as JArray ?? new JArray();
    JToken envelopeDocumentsToken = envelope["envelopeDocuments"];
    JArray documentArray = envelopeDocumentsToken as JArray ?? new JArray();
    JToken statusToken = envelope["status"];
    JToken senderToken = envelope["sender"];
    JToken statusChangedDateTimeToken = envelope["statusChangedDateTime"];
    JToken emailSubjectToken = envelope["emailSubject"];
    JToken envelopeIdToken = envelope["envelopeId"];
    JToken sentDateTimeToken = envelope["sentDateTime"];


    int recipientCount = recipientsToken?["recipientCount"]?.ToObject<int>() ?? 0;
    string recipientCountInNaturalLanguage = recipientCount > 1 ? $" and {recipientCount - 1} others have " : " ";
    int documentCount = documentArray.Count;
    string documentCountInNaturalLanguage = documentCount == 3 ? " and 1 other document" : documentCount > 3 ? $" and {documentCount - 2} other documents" : "";
    string envelopeStatus = statusToken?.ToString() ?? "Unknown status";
    string senderName = senderToken?["userName"]?.ToString() ?? "No sender name";
    string recipientName = signersArray.FirstOrDefault()?["name"]?.ToString() ?? "No recipient name";
    string envelopeDocumentName = documentArray.FirstOrDefault()?["name"]?.ToString() ?? "No document name";
    string statusDateChangeTime = statusChangedDateTimeToken?.ToString() ?? "Date is empty";

    var descriptionBuilder = new StringBuilder();
    if (envelopeStatus.Equals("sent", StringComparison.OrdinalIgnoreCase))
    {
        descriptionBuilder.Append(senderName)
                          .Append(" ")
                          .Append(envelopeStatus)
                          .Append(" ")
                          .Append(envelopeDocumentName)
                          .Append(" ")
                          .Append(documentCountInNaturalLanguage)
                          .Append(" on ")
                          .Append(statusDateChangeTime);
    }
    else if (signersArray.Count > 0)
    {
        descriptionBuilder.Append(recipientName)
                          .Append(recipientCountInNaturalLanguage)
                          .Append(envelopeStatus)
                          .Append(" ")
                          .Append(envelopeDocumentName)
                          .Append(documentCountInNaturalLanguage)
                          .Append(" on ")
                          .Append(statusDateChangeTime);
    }
    else
    {
        descriptionBuilder.Append("No signer recipients found for this envelope. Only 'Signer' recipient types are supported in the current response.");
    }

    return descriptionBuilder.ToString();
  }

  private string GetEnvelopeUrl(JToken envelope)
  {
    var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
    var path = uriBuilder.Uri.ToString().Contains("demo") || uriBuilder.Uri.ToString().Contains("stage") ? 
    "/send/documents/details/" : "/documents/details/";
    var envelopeUrl = GetDocusignApiBaseUri() + path + envelope["envelopeId"];

    return envelopeUrl;
  }

  private string GetDocusignApiBaseUri()
  {
    var host = this.Context.Request.RequestUri.Host.ToLower();
    var docusignApiBaseUri = host.Contains("demo") ?
        "https://apps-d.docusign.com"
      : host.Contains("stage") ?
        "https://apps-s.docusign.com"
      : host.Contains(".mil") ?
        "https://app.docusign.mil"
      : "https://app.docusign.com";

    return docusignApiBaseUri;
  }

  private string GetAccountServerBaseUri()
  {
    var host = this.Context.Request.RequestUri.Host.ToLower();
    var accountServerBaseUri = host.Contains("demo") ?
        "https://account-d.docusign.com"
      : host.Contains("stage") ?
        "https://account-s.docusign.com"
      : host.Contains(".mil") ?
        "https://account.docusign.mil"
      : "https://account.docusign.com";

    return accountServerBaseUri;
  }

  private string GetPartnerIntegrationsBaseUri()
  {
    var host = this.Context.Request.RequestUri.Host.ToLower();
    var pIBaseUri = host.Contains("demo") ?
        "https://demo.services.docusign.net/partner-integrations/v1.0"
      : host.Contains("stage") ?
        "https://services.stage.docusign.net/partner-integrations/v1.0"
      : "https://services.docusign.net/partner-integrations/v1.0";

    return pIBaseUri;
  }

  private JObject TriggerMaestroWorkflowTransformation(JObject body)
  {
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      var newBody = new JObject();
      newBody["instanceName"] = query.Get("instanceName");
      var inputVariables = new JArray();
      foreach(var property in body)
      {
          var key = (string)property.Key;
          var value = property.Value;
          inputVariables.Add(new JObject
          {
              ["propertyName"] = key,
              ["value"] = value
          });
      }
      newBody["inputVariables"] = inputVariables;
      return newBody;
  }

  private void AddCoreRecipientParams(JArray signers, JObject body) 
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);

    if (!string.IsNullOrEmpty(query.Get("recipientId")))
    {
      signers[0]["recipientId"] = query.Get("recipientId");
    }
    else
    {
      signers[0]["recipientId"] = GenerateId();
    }

    if (!string.IsNullOrEmpty(query.Get("routingOrder")))
    {
      signers[0]["routingOrder"] = query.Get("routingOrder");
    }

    if (!string.IsNullOrEmpty(query.Get("clientUserId")))
    {
      signers[0]["clientUserId"] = query.Get("clientUserId");
    }

    var emailNotification = new JObject();
    var emailNotificationSet = false;

    if (!string.IsNullOrEmpty(query.Get("emailNotificationLanguage")))
    {
      var language = query.Get("emailNotificationLanguage").Split("()".ToCharArray())[1];
      emailNotification["supportedLanguage"] = language;
      emailNotificationSet = true;
    }

    if (!string.IsNullOrEmpty(query.Get("emailNotificationSubject")))
    {
      emailNotification["emailSubject"] = query.Get("emailNotificationSubject");
      emailNotificationSet = true;
    }

    if (!string.IsNullOrEmpty(query.Get("emailNotificationBody")))
    {
      emailNotification["emailBody"] = query.Get("emailNotificationBody");
      emailNotificationSet = true;
    }

    if(emailNotificationSet)
    {
      signers[0]["emailNotification"] = emailNotification;
    }

    if (!string.IsNullOrEmpty(query.Get("note")))
    {
      signers[0]["note"] = query.Get("note");
    }

    if (!string.IsNullOrEmpty(query.Get("signingGroupId")))
    {
      signers[0]["signingGroupId"] = query.Get("signingGroupId");
    }

    if (!string.IsNullOrEmpty(query.Get("roleName")))
    {
      signers[0]["roleName"] = query.Get("roleName");
    }

    if (!string.IsNullOrEmpty(query.Get("countryCode")) && !string.IsNullOrEmpty(query.Get("phoneNumber")))
    {
      var phoneNumber = new JObject();
      phoneNumber["countryCode"] = query.Get("countryCode");
      phoneNumber["number"] = query.Get("phoneNumber");
      if (body["email"] != null)
      {
        var additionalNotification = new JObject();
        additionalNotification["secondaryDeliveryMethod"] = "SMS";
        additionalNotification["phoneNumber"] = phoneNumber;

        var additionalNotifications = new JArray();
        additionalNotifications.Add(additionalNotification);
        signers[0]["additionalNotifications"] = additionalNotifications;
      }
      else
      {
        signers[0]["phoneNumber"] = phoneNumber;
      }
    }

    if (!string.IsNullOrEmpty(query.Get("workflowId")))
    {
      var identityVerification = new JObject
      {
        ["workflowId"] = query.Get("workflowId")
      };
      signers[0]["identityVerification"] = identityVerification;
    }
  }

  private JArray GetFilteredEnvelopeDetailsForSalesCopilot(JArray filteredEnvelopes)
  {
    TimeZoneInfo userTimeZone = TimeZoneInfo.Local;
    var filteredEnvelopesDetails = new JArray();
        
    foreach (var envelope in filteredEnvelopes)
    {
      DateTime statusUpdateTime = envelope["statusChangedDateTime"]?.ToObject<DateTime>() ?? DateTime.MinValue;
      DateTime statusUpdateTimeInLocalTimeZone = TimeZoneInfo.ConvertTimeFromUtc(statusUpdateTime, userTimeZone);
      System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;

      JArray recipientNames = new JArray(
      (envelope["recipients"]?["signers"] as JArray)?.Select(recipient => recipient["name"]));
      JArray documentNames = new JArray(
      (envelope["envelopeDocuments"] as JArray)?.Select(envelopeDocument => envelopeDocument["name"]));

      JObject additionalPropertiesForSalesEnvelope = new JObject()
        {
          ["documents"] = string.Join(",", documentNames),
          ["recipients"] = string.Join(", ", recipientNames),
          ["statusDate"] = statusUpdateTimeInLocalTimeZone.ToString("h:mm tt, M/d/yy"),
          ["status"] = textInfo.ToTitleCase(envelope["status"]?.ToString() ?? "Unknown status"),
          ["sender"] = envelope["sender"]?["userName"]?.ToString() ?? "Sender name empty"
        };

      filteredEnvelopesDetails.Add(new JObject()
      {
        ["title"] = envelope["emailSubject"]?.ToString() ?? "Title empty",
        ["subTitle"] = "Agreement",
        ["url"] = GetEnvelopeUrl(envelope),
        ["additionalPropertiesForSalesEnvelope"] = additionalPropertiesForSalesEnvelope
      });
    }

    return filteredEnvelopesDetails;
  }

  private JArray GetFilteredEnvelopeDetails(JArray filteredEnvelopes)
  {
    TimeZoneInfo userTimeZone = TimeZoneInfo.Local;
    var filteredEnvelopesDetails = new JArray();

    foreach (var envelope in filteredEnvelopes)
    {
      DateTime statusUpdateTime = envelope["statusChangedDateTime"]?.ToObject<DateTime>() ?? DateTime.MinValue;
      DateTime statusUpdateTimeInLocalTimeZone = TimeZoneInfo.ConvertTimeFromUtc(statusUpdateTime, userTimeZone);
      System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;

      JArray recipientNames = new JArray(
        (envelope["recipients"]?["signers"] as JArray)?.Select(recipient => recipient["name"]));

      JArray documentNames = new JArray(
        (envelope["envelopeDocuments"] as JArray)?.Select(envelopeDocument => envelopeDocument["name"]));

      filteredEnvelopesDetails.Add(new JObject()
      {
        ["title"] = envelope["emailSubject"]?.ToString() ?? "Email subject empty",
        ["description"] = GetDescriptionNLPForRelatedActivities(envelope),
        ["envelopeId"] = envelope["envelopeId"]?.ToString() ?? "Envelope ID not found",
        ["statusDate"] = statusUpdateTimeInLocalTimeZone.ToString("h:mm tt, M/d/yy"),
        ["url"] = GetEnvelopeUrl(envelope),
        ["recipients"] = string.Join(", ", recipientNames),
        ["documents"] = string.Join(",", documentNames),
        ["sender"] = envelope["sender"]?["userName"]?.ToString() ?? "Sender username empty",
        ["status"] = envelope["status"] != null ? textInfo.ToTitleCase(envelope["status"].ToString()) : "Unknown status",
        ["dateSent"] = envelope["sentDateTime"]?.ToString() ?? "No sent date"
      });
    }

    return filteredEnvelopesDetails;
  }
  
  private JArray createRowValueList(Dictionary<int, List<JToken>> tableMap)
  {
    var rowValueList = new JArray();
    foreach (var row in tableMap)
    {
      var docGenFormFieldList = new JArray();
      foreach (var column in row.Value)
      {
        docGenFormFieldList.Add(new JObject
        {
          ["name"] = column["name"],
          ["value"] = column["value"]
        });
      }

      rowValueList.Add(new JObject
      {
        ["docGenFormFieldList"] = docGenFormFieldList
      });
    }

    return rowValueList;
  }

  private JArray GetFormFields(JArray docGenFormfields, JArray formFields)
  {
    foreach (var doc in docGenFormfields)
    {
      foreach(var field in (doc["docGenFormFieldList"] as JArray) ?? new JArray())
      {
        formFields.Add(new JObject()
        {
          ["name"] =  field["name"],
          ["type"] =  field["type"],
          ["value"] = field["value"],
          ["label"] =  field["label"],
          ["documentId"] =  doc["documentId"]
        });

        if (field["type"].ToString().Equals("TableRow"))
        {
          JArray rowValues = (field["rowValues"] as JArray) ?? new JArray();
          formFields = GetFormFields(rowValues, formFields);
        }
      }
    }

    return formFields;
  }

  private bool AddParamsForSelectedRecipientType(JArray signers, JObject body) 
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var recipientType = query.Get("recipientType");

    if (recipientType.Equals("inPersonSigners"))
    {
      signers[0]["hostName"] = body["hostName"];
      signers[0]["hostEmail"] = body["hostEmail"];
      signers[0]["signerName"] = body["signerName"];
    }
    else if (recipientType.Equals("witnesses"))
    {
      signers[0]["name"] = body["witnessName"];
      signers[0]["email"] = body["witnessEmail"];
      signers[0]["witnessFor"] = body["witnessFor"];
    }
    else
    {
      signers[0]["name"] = body["name"];
      if (body["email"] == null && string.IsNullOrEmpty(query.Get("signingGroupId")) && string.IsNullOrEmpty(query.Get("phoneNumber"))) 
      {
        return true;
      }
      else 
      {
        if (body["email"] != null)
        {
          signers[0]["email"] = body["email"];
        }
      }
    }
    return false;
  }

  private void AddParamsForSelectedSignatureType(JArray signers, JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var signatureType = query.Get("signatureType");

    var recipientSignatureProviders = new JArray
    {
        new JObject
        {
            ["signatureProviderName"] = signatureType
        }
    };

    if (signatureType.Equals("UniversalSignaturePen_OpenTrust_Hash_TSP"))
    {
        var aesMethod = body["aesMethod"].ToString().Contains("SMS") ? "sms" : "oneTimePassword";
        recipientSignatureProviders[0]["signatureProviderOptions"] = new JObject
        {
            [aesMethod] = body["aesMethodValue"]
        };
    }

    signers[0]["recipientSignatureProviders"] = recipientSignatureProviders;
  }

  private string GetHostFromUrl(string url)
  {
    if (!string.IsNullOrEmpty(url))
    {
      Uri uriResult;
      bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
      if (result)
      {
        return uriResult.Host;
      }
    }
    return null;
  }

  private JArray GetFilteredEnvelopes(JArray envelopes, string[] filters)
  {
    JArray filteredRecords = new JArray();
    foreach (var filter in filters.Where(filter => filter != null)) 
      {
        foreach (var envelope in envelopes)
        {
          if (envelope.ToString().ToLower().Contains(filter.ToLower()))
          {
            filteredRecords.Add(envelope);
          }
        }

        if (filteredRecords.Count > 0)
        {
          envelopes.Clear();
          envelopes = new JArray(filteredRecords);
          filteredRecords.Clear();
        }
        else
        {
          envelopes.Clear();
          break;
        }
      }

    return envelopes;
  }

  private int GenerateId()
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
      documents[i]["documentId"] = $"{GenerateId() + i}";
    }

    body["documents"] = documents;
    return body;
  }  
  
  private async Task UpdateRecipientTabsValuesBodyTransformation()
  {
    var body = ParseContentAsJArray(await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false), true);
    var tabs = new JObject();

    var tabsMap = new Dictionary<string, string>() { 
      { "Text", "textTabs" }, 
      { "Note", "noteTabs" },
    };

    foreach (var tab in body)
    {
      var tabType = tab["tabType"].ToString();
      tabType = tabsMap.ContainsKey(tabType) ? tabsMap[tabType] : tabType;
      var tabsForType = tabs[tabType] as JArray ?? new JArray();

      tabsForType.Add(new JObject
        {
          ["tabId"] = tab["tabId"],
          ["value"] = tab["value"]
        });

      tabs[tabType] = tabsForType;
    }

    var newBody = tabs;
    this.Context.Request.Content = CreateJsonContent(newBody.ToString());
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
      if (tabType.Equals("tabGroups"))
      {
        tab["pageNumber"] = "1";
      }
      res_tabs.Add(tab);
    }

    body[tabType] = res_tabs;
    return body;
  }
  private JObject ApplyTemplateBodyTransformation(JObject body)
  {
    var documentTemplatesArray = new JArray();
    var docTemplates = body["documentTemplates"] as JArray;
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var templateId = query.Get("templateId");
    
    if (docTemplates == null)
    {
      JObject tempObj = new JObject();
      tempObj["templateId"] = templateId;
      documentTemplatesArray.Add(tempObj);
    }
    else 
    {
      for (var i = 0; i < docTemplates.Count; i++)
      {
        JObject tempObj = docTemplates[i] as JObject;
        tempObj["templateId"] = templateId;
        documentTemplatesArray.Add(tempObj);
      }
    }

    body["documentTemplates"] = documentTemplatesArray;
    return body;
  }

  private JObject BulkSendBodyTransformation(JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var name = query.Get("name");
    JObject newBody = ParseCSV(body);
    newBody["name"] = name;
    return newBody;
  }

  private JObject ParseCSV(JObject inputBody)
  {
    var input = inputBody.GetValue("csv").ToString();
    var body = new JObject();
    var result = new JObject();
    try
    {
        var lines = input.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        var headerLine = lines[0];
        var headerItems = headerLine.Split(',');
        var parsedHeaders = new string[headerItems.Length][];
        string[] recipientFields = { "accessCode", "clientUserId", "deliveryMethod", "email", "embeddedRecipientStartURL", "hostEmail", "hostName", "idCheckConfigurationName", "name", "note", "recipientId", "roleName", "signerName", "signingGroupId" };
        // This map contains each copy of recipients. The key here would be the role name and the value is the recipient request object that gets added as request body
        Dictionary<string, JObject> recipientDataMap = new Dictionary<string, JObject>();
        body["recipients"] = new JArray();
        result["bulkCopies"] = new JArray();
        var recipientObject = new JObject();
        for (int i = 0; i < headerItems.Length; i++)
        {
            parsedHeaders[i] = headerItems[i].Split(new string[] { "::" }, StringSplitOptions.None);
        }
        // Iterate over the other lines (index at 1 to skip header line)
        for (var index = 1; index < lines.Length; index++)
        {
            var fieldValues = lines[index].Split(',');
            var roleName = "";
            var fieldName = "";
            for (var index2 = 0; index2 < fieldValues.Length; index2++)
            {
              var columnName = parsedHeaders[index2];
              var value = fieldValues[index2];
              if (string.IsNullOrEmpty(value))
              {
                  continue;
              }
              // recipient info
              if (columnName.Length > 1)
              {
                roleName = columnName[0];
                fieldName = columnName[1];
                fieldName = fieldName.Replace(" ", "");
                fieldName = char.ToLower(fieldName[0]) + fieldName.Substring(1);
                JObject recipientObj;
                if (recipientDataMap.ContainsKey(roleName))
                {
                  recipientObj = recipientDataMap[roleName];
                }
                else
                {
                  recipientDataMap[roleName] = new JObject();
                  recipientObj = recipientDataMap[roleName];
                  recipientObj["roleName"] = roleName;
                }
                if (recipientFields.Contains(fieldName))
                {
                    recipientObj[fieldName] = value;
                    continue;
                }
                if (fieldName.Equals("emailSubject", StringComparison.OrdinalIgnoreCase) ||
                fieldName.Equals("emailBody", StringComparison.OrdinalIgnoreCase) ||
                fieldName.Equals("language", StringComparison.OrdinalIgnoreCase))
                {
                  if (!recipientObj.ContainsKey("emailNotification"))
                  {
                    recipientObj["emailNotification"] = new JObject();
                  }
                  recipientObj["emailNotification"][fieldName] = value;
                }
                else
                {
                  if (!recipientObj.ContainsKey("tabs"))
                  {
                      recipientObj["tabs"] = new JArray();
                  }
                  ((JArray)recipientObj["tabs"]).Add(new JObject()
                  {
                      ["tabLabel"] = fieldName,
                      ["initialValue"] = value
                  });
                }
              }
              else
              {
                  // custom fields info
                  if (!body.ContainsKey("customFields"))
                  {
                    body["customFields"] = new JArray();
                  }
                  ((JArray) body["customFields"]).Add(new JObject()
                  {
                    ["name"] = columnName[0],
                    ["value"] = value
                  });
              }
            }
            foreach (KeyValuePair<string, JObject> pair in recipientDataMap)
            {
              var recipientObj = pair.Value;
              ((JArray)body["recipients"]).Add(recipientObj.DeepClone());
            }
            recipientDataMap = new Dictionary<string, JObject>();
            ((JArray)result["bulkCopies"]).Add(body.DeepClone());
            body["recipients"] = new JArray();
            body["customFields"] = new JArray();
            recipientDataMap = new Dictionary<string, JObject>(); 
        }
    }
    catch (JsonReaderException ex)
    {
        throw new ConnectorException(HttpStatusCode.BadRequest, "Please refer to Docusign documentations and follow CSV file guidelines. Unable to parse the request body", ex);
    }
    return result;
  }

  private JObject BulkSendRequestBodyTransformation(JObject body)
  {
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var envelopeOrTemplateId = query.Get("envelopeOrTemplateId");

    body["envelopeOrTemplateId"] = envelopeOrTemplateId;
    return body;
  }

  private async Task UpdateDocgenFormFieldsBodyTransformation()
  {
    var body = ParseContentAsJArray(await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false), true);
    var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
    var fieldList = new JArray();
    var rowValueList = new JArray();
    var documentId = query.Get("documentGuid");
    string tableName = string.Empty;
    Dictionary<int, List<JToken>> tableMap = new Dictionary<int, List<JToken>>();

    try
    {
      foreach (var field in body)
      {
        if ((field["fieldType"] != null) && (field["fieldType"].ToString() == "Table row"))
        {
          var rowNumber = field["rowNumber"].Value<int>();
          tableName = field["tableName"].ToString();

          if (!tableMap.ContainsKey(rowNumber))
          {
            tableMap[rowNumber] = new List<JToken>();
          }
          tableMap[rowNumber].Add(field);
        }
        else
        {
          fieldList.Add(new JObject
          {
            ["name"] = field["name"],
            ["value"] = field["value"]
          });
        }
      }
    }
     catch (HttpRequestException ex)
    {
      throw new ConnectorException(HttpStatusCode.BadGateway, "Docgen field name not found" + ex.Message, ex);
    }

    if (!string.IsNullOrEmpty(tableName))
    {
      rowValueList = createRowValueList(tableMap);
      fieldList.Add(new JObject
      {
        ["label"] = tableName,
        ["type"] = "TableRow",
        ["required"] = "True",
        ["name"] = tableName,
        ["rowValues"] = rowValueList
      });
    }
    
    var docGenFormFields = new JArray
    {
      new JObject
      {
        ["documentId"] = documentId,
        ["docGenFormFieldList"] = fieldList
      },
    };

    var newBody = new JObject();
    newBody["docGenFormFields"] = docGenFormFields;

    this.Context.Request.Content = CreateJsonContent(newBody.ToString());
  }

  private Dictionary<string, JObject> GenerateRecipientsMappings(JObject body)
  {
    Dictionary<string, JObject> recipientData = new Dictionary<string, JObject>();
    string[] recipientTypes = new string[] {"agents", "carbonCopies", "certifiedDeliveries", "editors", "inPersonSigners", "signers", "intermediaries"};
    if (body["recipients"] != null)
    {
      foreach (var recipientType in recipientTypes)
      {
        if (body["recipients"][recipientType] != null)
        {
          var recipients = body["recipients"][recipientType] as JArray;
          foreach (JObject recipient in recipients)
          {
            if (recipient.ContainsKey("roleName"))
            {
              recipientData[recipient["roleName"].ToString()] = recipient as JObject;
            }
          }
        }
      }
    }
    return recipientData;
  }

  private void GenerateRecipientInformationFields(Dictionary<string, JObject> recipientData, JObject itemProperties)
  {
    string[] editableTabs = new string[]{"emailTabs", "formulaTabs", "noteTabs", "ssnTabs", "textTabs", "zipTabs", "checkboxTabs", "numberTabs"};

    foreach (KeyValuePair<string, JObject> pair in recipientData)
    {
      var roleName = pair.Key;
      JObject recipientObj = pair.Value as JObject;
      // Name fields
      if (string.Equals(recipientObj["recipientType"].ToString(), "signer"))
      {
        itemProperties[roleName + ":::Name"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = roleName + " Recipient Or Signing Group Name"
        };
        itemProperties[roleName + ":::Signing Group"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = roleName + " Signing Group",
          ["x-ms-dynamic-values"] = new JObject
          {
            ["operationId"] = "GetSigningGroups",
            ["value-collection"] = "groups",
            ["value-path"] = "signingGroupId",
            ["value-title"] = "groupName",
            ["parameters"] = new JObject
            {
              ["accountId"] = new JObject
              {
                ["parameter"] = "accountId"
              }
            }
          }
        };
      }
      else
      {
        if (string.Equals(recipientObj["recipientType"].ToString(), "inpersonsigner", StringComparison.OrdinalIgnoreCase))
        {
          itemProperties[roleName + ":::In Person Signer"] = new JObject
          {
            ["type"] = "string",
            ["x-ms-summary"] = roleName + " Signer Name"
          };
          itemProperties[roleName + ":::Name"] = new JObject
          {
            ["type"] = "string",
            ["x-ms-summary"] = roleName + " Host Name"
          };
          itemProperties[roleName + ":::Email"] = new JObject
          {
            ["type"] = "string",
            ["x-ms-summary"] = roleName + " Host Email"
          };
        }
        else
        {
          itemProperties[roleName + ":::Name"] = new JObject
          {
            ["type"] = "string",
            ["x-ms-summary"] = roleName + " Recipient Name"
          };
        }
      }

      // SMS/Email fields
      if ((recipientObj["additionalNotifications"] != null && 
      ((JArray) recipientObj["additionalNotifications"]).Count > 0 &&
      recipientObj["additionalNotifications"][0]["secondaryDeliveryMethod"] != null && 
      string.Equals(recipientObj["additionalNotifications"][0]["secondaryDeliveryMethod"].ToString(), "SMS", StringComparison.OrdinalIgnoreCase)))
      {
        itemProperties[roleName + ":::Secondary Country Code"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = roleName + " SMS Country Code"
        };
        itemProperties[roleName + ":::Secondary Phone Number"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = roleName + " SMS Phone Number"
        };
      }
      if (string.Equals(recipientObj["deliveryMethod"].ToString(), "SMS", StringComparison.OrdinalIgnoreCase))
      {
        itemProperties[roleName + ":::Country Code"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = roleName + " SMS Country Code"
        };
        itemProperties[roleName + ":::Phone Number"] = new JObject
        {
          ["type"] = "string",
          ["x-ms-summary"] = roleName + " SMS Phone Number"
        };
      }
      else
      {
        if (string.Equals(recipientObj["recipientType"].ToString(), "signer"))
        {
          itemProperties[roleName + ":::Email"] = new JObject
          {
            ["type"] = "string",
            ["x-ms-summary"] = roleName + " Recipient Email (Leave empty if there’s a signing group)"
          };
        }
        else
        {
          if (!string.Equals(recipientObj["recipientType"].ToString(), "inpersonsigner", StringComparison.OrdinalIgnoreCase))
          {
            itemProperties[roleName + ":::Email"] = new JObject
            {
              ["type"] = "string",
              ["x-ms-summary"] = roleName + " Recipient Email"
            };
          }
        }
      }

      // Tabs fields
      JObject singleRecipientData = recipientObj as JObject;
      JObject tabsData = singleRecipientData["tabs"] as JObject;
      if (tabsData != null)
      {
        foreach (var tabType in editableTabs)
        {
          if (tabsData.ContainsKey(tabType))
          {
            foreach (var tab in tabsData[tabType] as JArray)
            {
              if (string.Equals(tabType, "checkboxTabs", StringComparison.OrdinalIgnoreCase))
              {
                itemProperties[roleName + ":::" + tabType + ":::" + tab["tabLabel"].ToString() + ":::" + tab["name"].ToString()] = new JObject
                {
                  ["type"] = "string",
                  ["enum"] = new JArray("true", "false"),
                  ["x-ms-summary"] = roleName + " - Tab Type: " + tabType + " - Tab label: " + tab["tabLabel"].ToString() + " - Name: " + tab["name"].ToString() +" Selected"
                };
              }
              else
              {
                itemProperties[roleName + ":::" + tabType + ":::" + tab["tabLabel"].ToString()] = new JObject
                {
                  ["type"] = "string",
                  ["x-ms-summary"] = roleName + " - Tab Type: " + tabType + " - Tab label: " + tab["tabLabel"].ToString()
                };
              }
            }
          }
        }
      }
    }
  }

  private void GenerateCustomFields(JObject body, JObject itemProperties)
  {
    if (body["customFields"] != null)
    {
      if (body["customFields"]["listCustomFields"] != null)
      {
        foreach (var customField in body["customFields"]["listCustomFields"] as JArray)
        {
          var required = "";
          if (string.Equals(customField["required"].ToString(), "true"))
          {
            required = " *";
          }
          itemProperties["List Custom Fields:::" + customField["name"].ToString()] = new JObject
          {
            ["type"] = "string",
            ["x-ms-summary"] = "List Custom Fields: " + required + customField["name"].ToString(),
            ["enum"] = customField["listItems"]
          };
        }
      }
      if (body["customFields"]["textCustomFields"] != null)
      {
        foreach (var customField in body["customFields"]["textCustomFields"] as JArray)
        {
          var required = "";
          if (string.Equals(customField["required"].ToString(), "true"))
          {
            required = " *";
          }
          itemProperties["Text Custom Fields:::" + customField["name"].ToString()] = new JObject
          {
            ["type"] = "string",
            ["x-ms-summary"] = "Text Custom Fields: " + required + customField["name"].ToString()
          };
        }
      }
    }
  }

  private void ParseRecipientFields(Dictionary<string, JObject> recipientMapping, string[] keyArray, string value, string roleName)
  {
    if (!recipientMapping.ContainsKey(roleName))
    {
      JObject newRecipientObj = new JObject();
      newRecipientObj["roleName"] = roleName;
      recipientMapping[roleName] = newRecipientObj;
    }
    JObject recipientObj = recipientMapping[roleName];

    if (keyArray.Length > 2)
    {
      if (!recipientObj.ContainsKey("tabs"))
      {
        recipientObj["tabs"] = new JObject();
      }
      var tabType = keyArray[1];
      var tabLabel = keyArray[2];
      if (string.Equals(tabType, "checkboxTabs", StringComparison.OrdinalIgnoreCase))
      {
        var tabObj = new JObject
        {
          ["tabLabel"] = tabLabel,
          ["name"] = keyArray[3],
          ["selected"] = value
        };
        JObject recipientTabs = (JObject) recipientObj["tabs"];
        if (!recipientTabs.ContainsKey(tabType))
        {
          recipientTabs[tabType] = new JArray();
        }
        JArray recipientObjArray = (JArray) recipientTabs[tabType];
        recipientObjArray.Add(tabObj);
      }
      else
      {
        var tabObj = new JObject
        {
          ["tabLabel"] = tabLabel,
          ["value"] = value
        };
        JObject recipientTabs = (JObject) recipientObj["tabs"];
        if (!recipientTabs.ContainsKey(tabType))
        {
          recipientTabs[tabType] = new JArray();
        }
        JArray recipientObjArray = (JArray) recipientTabs[tabType];
        recipientObjArray.Add(tabObj);
      }
    }
    if (string.Equals(keyArray[1], "Name", StringComparison.OrdinalIgnoreCase))
    {
      recipientObj["name"] = value;
    }
    if (string.Equals(keyArray[1], "Email", StringComparison.OrdinalIgnoreCase))
    {
      recipientObj["email"] = value;
    }
    if (string.Equals(keyArray[1], "Signing Group", StringComparison.OrdinalIgnoreCase))
    {
      recipientObj["signingGroupId"] = value;
    }
    if (string.Equals(keyArray[1], "In Person Signer", StringComparison.OrdinalIgnoreCase))
    {
      recipientObj["inPersonSignerName"] = value;
    }
    if (string.Equals(keyArray[1], "Secondary Country Code", StringComparison.OrdinalIgnoreCase))
    {
      if (!recipientObj.ContainsKey("additionalNotifications"))
      {
        recipientObj["additionalNotifications"] = new JArray();
      }
      JArray additionalNotificationsArray = recipientObj["additionalNotifications"] as JArray;
      if (additionalNotificationsArray.Count == 0)
      {
        additionalNotificationsArray.Add(new JObject 
        {
          ["phoneNumber"] = new JObject 
          {
            ["countryCode"] = value
          },
          ["secondaryDeliveryMethod"] = "SMS"
        });
      }
      else
      {
        additionalNotificationsArray[0]["phoneNumber"]["countryCode"] = value;
      }
    }
    if (string.Equals(keyArray[1], "Secondary Phone Number", StringComparison.OrdinalIgnoreCase))
    {
      if (!recipientObj.ContainsKey("additionalNotifications"))
      {
        recipientObj["additionalNotifications"] = new JArray();
      }
      JArray additionalNotificationsArray = recipientObj["additionalNotifications"] as JArray;
      if (additionalNotificationsArray.Count == 0)
      {
        additionalNotificationsArray.Add(new JObject 
        {
          ["phoneNumber"] = new JObject 
          {
            ["number"] = value
          },
          ["secondaryDeliveryMethod"] = "SMS"
        });
      }
      else
      {
        additionalNotificationsArray[0]["phoneNumber"]["number"] = value;
      }
    }
    if (string.Equals(keyArray[1], "Country Code", StringComparison.OrdinalIgnoreCase))
    {
      if (!recipientObj.ContainsKey("phoneNumber"))
      {
        recipientObj["phoneNumber"] = new JObject();
      }
      JObject recipientPhoneObj = (JObject) recipientObj["phoneNumber"];
      recipientPhoneObj["countryCode"] = value;
    }
    if (string.Equals(keyArray[1], "Phone Number", StringComparison.OrdinalIgnoreCase))
    {
      if (!recipientObj.ContainsKey("phoneNumber"))
      {
        recipientObj["phoneNumber"] = new JObject();
      }
      JObject recipientPhoneObj = (JObject) recipientObj["phoneNumber"];
      recipientPhoneObj["number"] = value;
    }
  }

  private async Task UpdateApiEndpoint()
  {
    string content = string.Empty;
    using var userInfoRequest = new HttpRequestMessage(HttpMethod.Get, GetAccountServerBaseUri() + "/oauth/userinfo");

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
        var accountId = (string)jsonContent["accounts"].Where(a => (bool)a["is_default"]).FirstOrDefault()["account_id"];
        if (!string.IsNullOrEmpty(baseUri))
        {
          this.Context.Request.RequestUri = new Uri(new Uri(baseUri), this.Context.Request.RequestUri.PathAndQuery);
        }
        else
        {
          throw new ConnectorException(HttpStatusCode.BadGateway, "Unable to get User's API endpoint from the response: " + content);
        }

        if (!string.IsNullOrEmpty(accountId))
        { 
          this.Context.Request.Headers.Add("AccountId", accountId);
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

    if("DeleteHookV2".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      uriBuilder.Path = uriBuilder.Path.Replace("connectV2", "connect");
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    if("DeleteHookV3".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      uriBuilder.Path = uriBuilder.Path.Replace("connectV3", "connect");
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    if ("SendDraftEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      this.Context.Request.Content = new StringContent("{ \"status\": \"sent\" }", Encoding.UTF8, "application/json");
    }
    
    if ("CreateHookEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.CreateHookEnvelopeBodyTransformation).ConfigureAwait(false);
    }
    
    if ("CreateHookEnvelopeV2".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.CreateHookEnvelopeV2BodyTransformation).ConfigureAwait(false);
    }
	
    if ("CreateHookEnvelopeV3".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.CreateHookEnvelopeV3BodyTransformation).ConfigureAwait(false);
    }

    if ("CreateBlankEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.CreateBlankEnvelopeBodyTransformation).ConfigureAwait(false);
    }

    if ("CompositeTemplates".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.CompositeTemplatesBodyTransformation).ConfigureAwait(false);
    }

    if("VoidEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.EnvelopeVoidBodyTransformation).ConfigureAwait(false);
    }

    if("ResendEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.EnvelopeResendBodyTransformation).ConfigureAwait(false);
    }
    
    if(("SearchListEnvelopes".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)))
    {
      await this.TransformRequestJsonBody(this.SearchListEnvelopesTransformation).ConfigureAwait(false);
    }

    if ("SendEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.CreateEnvelopeFromTemplateV1BodyTransformation).ConfigureAwait(false);
    }

    if ("SendEnvelopeWithRecipientFields".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.CreateEnvelopeFromTemplateV3BodyTransformation).ConfigureAwait(false);
    }

    if ("AddReminders".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.AddRemindersBodyTransformation).ConfigureAwait(false);
    }
    
    if ("CreateEnvelopeFromTemplate".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.CreateEnvelopeFromTemplateV2BodyTransformation).ConfigureAwait(false);
    }

    if ("CreateEnvelopeFromTemplateNoRecipients".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.CreateEnvelopeFromTemplateNoRecipientsBodyTransformation).ConfigureAwait(false);
    }

    if ("AddRecipientToEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.AddRecipientToEnvelopeBodyTransformation).ConfigureAwait(false);
    }
    
    if ("AddRecipientToEnvelopeV2".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase) ||
        "UpdateEnvelopeRecipient".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.AddRecipientToEnvelopeV2BodyTransformation).ConfigureAwait(false);
    }

    if ("AddVerificationToRecipient".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.AddVerificationToRecipientBodyTransformation).ConfigureAwait(false);
    }

    if ("UpdateEnvelopeCustomField".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.UpdateEnvelopeCustomFieldBodyTransformation).ConfigureAwait(false);
    }

    if ("GenerateEmbeddedSigningURL".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.GenerateEmbeddedSigningURLBodyTransformation).ConfigureAwait(false);
    }

    if ("GenerateEmbeddedSigningURLV2".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.GenerateEmbeddedSigningURLV2BodyTransformation).ConfigureAwait(false);
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

    if ("ApplyTemplatesToDocuments".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.ApplyTemplateBodyTransformation).ConfigureAwait(false);
    }

    if ("CreateBulkSendList".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.BulkSendBodyTransformation).ConfigureAwait(false);
    }

    if ("BulkSend".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.TransformRequestJsonBody(this.BulkSendRequestBodyTransformation).ConfigureAwait(false);
    }

    if ("UpdateRecipientTabsValues".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.UpdateRecipientTabsValuesBodyTransformation().ConfigureAwait(false);
    }

    if ("UpdateEnvelopePrefillTabs".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.UpdateEnvelopePrefillTabsBodyTransformation().ConfigureAwait(false);
    }

    if ("UpdateDocgenFormFields".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      await this.UpdateDocgenFormFieldsBodyTransformation().ConfigureAwait(false);
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

    if("scp-get-email-summary".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      uriBuilder.Path = uriBuilder.Path.Replace("/getEmailSummary", "");
      uriBuilder.Path = uriBuilder.Path.Replace("salesCopilotAccount", this.Context.Request.Headers.GetValues("AccountId").FirstOrDefault());
      this.Context.Request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());
      this.Context.Request.Headers.Add("x-ms-user-agent", "sales-copilot");

      if (!string.IsNullOrEmpty(query.Get("crmType")))
      {
        if (query.Get("crmType").ToString().Equals("Dynamics365"))
        {
          query["custom_field"] = "entityLogicalName=" + query.Get("recordType");
        }
      }

      query["from_date"] = string.IsNullOrEmpty(query.Get("startDateTime")) ? 
        "2000-01-02T12:45Z" :
        query.Get("startDateTime");

      if (!string.IsNullOrEmpty(query.Get("endDateTime")))
      {
        query["to_date"] = query.Get("endDateTime");
      }

      query["include"] = "custom_fields,recipients,documents";
      query["order"] = "desc";
      uriBuilder.Query = query.ToString();
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    if("scp-get-related-activities".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      uriBuilder.Path = uriBuilder.Path.Replace("/getRelatedActivities", "");
      uriBuilder.Path = uriBuilder.Path.Replace("salesCopilotAccount", this.Context.Request.Headers.GetValues("AccountId").FirstOrDefault());
      this.Context.Request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());
      this.Context.Request.Headers.Add("x-ms-user-agent", "sales-copilot");

      if (query.Get("crmType").ToString().Equals("Dynamics365"))
      {
        query["custom_field"] = "entityLogicalName=" + query.Get("recordType");
      }

      query["from_date"] = string.IsNullOrEmpty(query.Get("startDateTime")) ? 
        "2000-01-02T12:45Z" :
        query.Get("startDateTime");

      if (!string.IsNullOrEmpty(query.Get("endDateTime")))
      {
        query["to_date"] = query.Get("endDateTime");
      }

      query["include"] = "custom_fields,recipients,documents";
      query["order"] = "desc";
      uriBuilder.Query = query.ToString();
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    if(("scp-get-related-records".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)) || 
    ("scp-get-key-sales".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      uriBuilder.Path = uriBuilder.Path.Replace("/getRelatedRecords", "");
      uriBuilder.Path = uriBuilder.Path.Replace("/getKeySales", "");
      uriBuilder.Path = uriBuilder.Path.Replace("salesCopilotAccount", this.Context.Request.Headers.GetValues("AccountId").FirstOrDefault());
      this.Context.Request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());
      this.Context.Request.Headers.Add("x-ms-user-agent", "sales-copilot");

      if (query.Get("crmType").ToString().Equals("Dynamics365"))
      {
        query["custom_field"] = "entityLogicalName=" + query.Get("recordType");
      }

      query["from_date"] = string.IsNullOrEmpty(query.Get("startDateTime")) ? 
        "2000-01-02T12:45Z" :
        query.Get("startDateTime");

      query["include"] = "custom_fields, recipients, documents";
      query["order"] = "desc";
      uriBuilder.Query = query.ToString();
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    if(("ListEnvelopes".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)) ||
    ("SalesCopilotListEnvelopes".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      uriBuilder.Path = uriBuilder.Path.Replace("/listEnvelopes", "");
      uriBuilder.Path = uriBuilder.Path.Replace("ForSalesCopilot", "");
      uriBuilder.Path = uriBuilder.Path.Replace("copilotAccount", this.Context.Request.Headers.GetValues("AccountId").FirstOrDefault());
      this.Context.Request.Headers.Add("generative-ai-request-id", Guid.NewGuid().ToString());
      this.Context.Request.Headers.Add("generative-ai-user-agent", "sales-copilot");

      query["include"] = "custom_fields, recipients, documents, folders";
      query["order"] = "desc";
      
      query["status"] = string.IsNullOrEmpty(query.Get("envelopeStatus")) ? 
        null : query.Get("envelopeStatus");
      query["folder_ids"] = string.IsNullOrEmpty(query.Get("folder_ids")) ? 
        null : query.Get("folder_ids").ToString();
       query["order_by"] = string.IsNullOrEmpty(query.Get("order_by")) ? 
        "status_changed" : query.Get("order_by");
      query["from_date"] = string.IsNullOrEmpty(query.Get("from_date")) ? 
        "2000-01-02T12:45Z" : query.Get("from_date");
      query["to_date"] = string.IsNullOrEmpty(query.Get("to_date")) ? 
        DateTimeOffset.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : query.Get("to_date");

      uriBuilder.Query = query.ToString();
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    if ("GetRecipientFields".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      uriBuilder.Path = uriBuilder.Path.Replace("/recipientFields", "/recipients");
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      query["include_extended"] = "true";
      uriBuilder.Query = query.ToString();
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    if ("GetEnvelopeRecipientTabs".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      uriBuilder.Path = uriBuilder.Path.Replace("/recipientTabs", "/tabs");
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    if ("ListEnvelopeDocuments".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      uriBuilder.Path = uriBuilder.Path.Replace("/envelopeDocuments", "/documents");
      this.Context.Request.RequestUri = uriBuilder.Uri;
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

    if ("GetDynamicRecipients".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      uriBuilder.Path = uriBuilder.Path.Replace("/signers/accounts/", "/accounts/");
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      query["include"] = "tabs";
      uriBuilder.Query = query.ToString();
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    if ("GetAccountCustomFields".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      uriBuilder.Path = uriBuilder.Path.Replace("/account_custom_fields", "/custom_fields");
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    if ("GetEnvelopeDocumentInfo".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      uriBuilder.Path = uriBuilder.Path.Replace("/get_document_info", "/documents");
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

    if( "TriggerMaestroFlow".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase) 
        || "GetMaestroWorkflowDefinition".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)
        || "GetMaestroWorkflowDefinitions".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
        
        var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
        var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        if("GetMaestroWorkflowDefinitions".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
          query["triggerType"] = "1" ;
        }
        var maestroAPIUrl = GetPartnerIntegrationsBaseUri() + uriBuilder.Path.Replace("/restapi/v2.1", "");
        var newUriBilder = new UriBuilder(maestroAPIUrl);
        newUriBilder.Query = query.ToString();
        this.Context.Request.RequestUri = newUriBilder.Uri;
    }

    if( "TriggerMaestroFlow".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
        await this.TransformRequestJsonBody(this.TriggerMaestroWorkflowTransformation).ConfigureAwait(false);
    }

    if ("GetAllWorkflowIds".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      uriBuilder.Path = uriBuilder.Path.Replace("/all_identity_verification", "/identity_verification");
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    // update Accept Header
    this.Context.Request.Headers.Accept.Clear();
    var acceptHeaderValue = "application/json";
    if ("GetDocuments".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      acceptHeaderValue = "application/pdf";
    }

    if ("GetDocumentsV2".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      var newPath = uriBuilder.Path;
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      string[] documentDownloadOptions = { "Combined", "Archive", "Certificate", "Portfolio" };

      acceptHeaderValue = "application/pdf";
      string documentId = null;

      foreach(var downloadOption in documentDownloadOptions)
      {
        if (newPath.Contains(downloadOption))
        {
          documentId = downloadOption;
          break;
        }
      }

      if (HttpUtility.UrlDecode(uriBuilder.Path).Trim().Contains("Combined with COC"))
      {
        query["certificate"] = "true";
        uriBuilder.Query = query.ToString();
      }
      
      uriBuilder.Path = documentId == null ? newPath.Replace("/documentsDownload", "") : newPath.Substring(0, newPath.IndexOf(documentId) + documentId.Length);
      this.Context.Request.RequestUri = uriBuilder.Uri;
    }

    this.Context.Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeaderValue));
  }

  private async Task UpdateResponse(HttpResponseMessage response)
  {
    if (this.Context.OperationId.Contains("CreateHookEnvelope")
        && response.Headers?.Location != null)
    {
      var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
      var body = ParseContentAsJObject(content, false);

      response.Headers.Location = new Uri(string.Format(
          "{0}/{1}",
          this.Context.OriginalRequestUri.ToString(),
          body.GetValue("connectId").ToString()));
    }

    if ("AddReminders".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var reminderEnabled = body["reminders"]["reminderEnabled"];

      body["reminderEnabled"] = reminderEnabled;
      response.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
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

    if ("GetMaestroWorkflowDefinition".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
      var body = JObject.Parse(content);
      var payload = body["payloadSchema"];
      var itemProperties = new JObject();
      foreach (var item in payload as JArray)
      {
        var propertyName = (string)item["propertyName"];
        if (!propertyName.Equals("dacId") && !propertyName.Equals("id"))
        {     
          itemProperties.Add(propertyName, new JObject());
          var type = (string)item["type"];
          itemProperties[propertyName]["type"] = type == "Float" ? "number" : type.ToLower();
        }
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
            }
          }
      };
      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("GetWorkflowIds".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var workflowsArray = new JArray();

      foreach (var id in (body["identityVerification"] as JArray)) {
        if (!string.Equals(id["defaultName"].ToString(), "DocuSign ID Verification for EU Qualified") &&
            !string.Equals(id["defaultName"].ToString(), "DocuSign ID Verification for EU Advanced"))
        {
          var workflowObj = new JObject()
          {
            ["type"] = id["workflowId"],
            ["name"] = id["defaultName"]
          };
          workflowsArray.Add(workflowObj);
        }
      }
      body["workflowIds"] = workflowsArray;
      response.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
    }

    if ("GetAllWorkflowIds".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
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

    if ("GetEnvelopeDocumentFields".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      var newBody = new JObject();
      JArray envelopeDocumentFields = new JArray();

      foreach(JProperty documentFields in body.Properties())
      {
        foreach(var field in documentFields.Value)
        {
          envelopeDocumentFields.Add(new JObject()
          {
            ["name"] = field["name"],
            ["value"] = field["value"]
          });
        }
      }

      newBody["envelopeDocumentFields"] = envelopeDocumentFields;
      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }
  
    if ("GetEnvelopeDocumentTabs".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var newBody = new JObject();
      JArray tabs = new JArray();

      //non-prefill tabs
      var nonPrefillTabs = body as JObject ?? new JObject();

      foreach (JProperty tabTypes in nonPrefillTabs.Properties())
      {
        if (tabTypes.Name.Equals("prefillTabs"))
        {
          continue;
        }

        foreach (var tab in tabTypes.Value)
        {
          tabs.Add(new JObject()
          {
            ["tabLabel"] = tab["tabLabel"],
            ["value"] = tab["value"],
            ["documentId"] = tab["documentId"],
            ["recipientId"] = tab["recipientId"],
            ["tabId"] = tab["tabId"],
            ["tabType"] = tabTypes.Name,
            ["prefill"] = false,
            ["selected"] = tab["selected"] ?? null,
            ["name"] = tab["name"] ?? null
          });
        }
      }

      //prefill tabs
      var prefillTabs = body["prefillTabs"] as JObject ?? new JObject();

      foreach (JProperty tabTypes in prefillTabs.Properties())
      {
        foreach (var tab in tabTypes.Value)
        {
          tabs.Add(new JObject()
          {
            ["tabLabel"] = tab["tabLabel"],
            ["value"] = tab["value"],
            ["documentId"] = tab["documentId"],
            ["tabId"] = tab["tabId"],
            ["tabType"] = tabTypes.Name,
            ["prefill"] = true
          });
        }
      }

      newBody["tabs"] = tabs;

      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("GetTemplateDocumentTabs".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var newBody = new JObject();
      JArray tabs = new JArray();

      foreach(JProperty tabTypes in body.Properties())
      {
        foreach(var tab in tabTypes.Value)
        {
          tabs.Add(new JObject()
          {
            ["name"] = tab["name"],
            ["tabLabel"] = tab["tabLabel"],
            ["value"] = tab["value"],
            ["documentId"] = tab["documentId"],
            ["tabId"] = tab["tabId"],
            ["tabType"] = tab["tabType"],
            ["recipientId"] = tab["recipientId"]
          });
        }
      }

      newBody["tabs"] = tabs;

      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("GetTabInfo".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      var tabLabel = query.Get("tabLabel");
      var newBody = new JObject();

      foreach(JProperty tabTypes in body.Properties())
      {
        foreach(var tab in tabTypes.Value)
        {
          if(tab["tabType"].ToString().Equals("radiogroup"))
          {
            newBody["value"] = tab["value"] ?? tab["value"];
            newBody["documentId"] = tab["documentId"] ?? tab["documentId"];
            newBody["tabType"] = tab["tabType"];
            newBody["recipientId"] = tab["recipientId"];
          }

          if(tab["tabLabel"] != null && (tab["tabLabel"].ToString()).Equals(tabLabel.ToString()))
          {
            newBody["name"] = tab["name"] ?? tab["name"];
            newBody["tabLabel"] = tab["tabLabel"];
            newBody["value"] = tab["value"] ?? tab["value"];
            newBody["documentId"] = tab["documentId"] ?? tab["documentId"];
            newBody["tabId"] = tab["tabId"];
            newBody["tabType"] = tab["tabType"];
            newBody["recipientId"] = tab["recipientId"];
            break;
          }
        }
      }

      if (newBody["tabType"] == null)
      {
        throw new ConnectorException(HttpStatusCode.BadRequest, "ValidationFailure: Could not find the Tab Type for the specified recipient");
      }

      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("GetEnvelopeRecipientTabs".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      JObject newBody = new JObject();
      JArray recipientTabs = new JArray();

      foreach(JProperty tabTypes in body.Properties())
      {
        foreach(var tab in tabTypes.Value)
        {
          recipientTabs.Add(new JObject()
          {
            ["name"] =  tab["name"],
            ["tabType"] =  tab["tabType"],
            ["value"] =  tab["value"],
            ["tabLabel"] =  tab["tabLabel"],
            ["documentId"] =  tab["documentId"],
            ["tabId"] = tab["tabId"],
            ["recipientId"] = tab["recipientId"]
          });
        }
      }

      newBody["recipientTabs"] = recipientTabs;
      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("GetDocgenFormFields".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      JObject newBody = new JObject();
      JArray formFields = new JArray();
      JArray docGenFormfields = (body["docGenFormFields"] as JArray) ?? new JArray();

      formFields = GetFormFields(docGenFormfields, formFields);

      newBody["docgenFields"] = formFields;
      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("scp-get-email-summary".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      JObject newBody = new JObject();
      TimeZoneInfo userTimeZone = TimeZoneInfo.Local;

      JArray envelopes = (body["envelopes"] as JArray) ?? new JArray();
      JArray emailSummary = new JArray();
      int top = string.IsNullOrEmpty(query.Get("top")) ? 3: int.Parse(query.Get("top"));
      int skip = string.IsNullOrEmpty(query.Get("skip")) ? 0: int.Parse(query.Get("skip"));

      var crmOrgUrl = GetHostFromUrl(query.Get("crmOrgUrl"));
      var recordId = query.Get("recordId") ?? null;
      var crmType = string.IsNullOrEmpty(query.Get("crmType")) ? null
        : query.Get("crmType").ToString().Equals("Dynamics365") ? "CRMToken"
        : "SFToken";

      var recordType = query.Get("recordType");
      string[] recipientEmail = query.Get("emailContacts").Replace(" ","").Split(',');

      string[] filters = { crmType, crmOrgUrl, recordId, recordType };
      filters = recipientEmail.Concat(filters).ToArray();

      envelopes = GetFilteredEnvelopes(envelopes, filters);

      foreach (var envelope in envelopes)
      {
        DateTime statusUpdateTime = envelope["statusChangedDateTime"].ToObject<DateTime>();
        DateTime statusUpdateTimeInLocalTimeZone = TimeZoneInfo.ConvertTimeFromUtc(statusUpdateTime, userTimeZone);
        JArray recipientNames = new JArray();
        System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;
        foreach (var recipient in (envelope["recipients"]["signers"] as JArray) ?? new JArray())
        {
          recipientNames.Add(recipient["name"]);
        }

        emailSummary.Add(new JObject()
        {
          ["Title"] = "Docusign: " + envelope["emailSubject"],
          ["Description"] = GetDescriptionNLPForRelatedActivities(envelope)
        });
      }

      newBody["value"] = (emailSummary.Count < top) ? emailSummary : new JArray(emailSummary.Skip(skip).Take(top).ToArray());
      newBody["hasMoreResults"] = (skip + top < emailSummary.Count) ? true : false;

      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("scp-get-related-activities".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      JObject newBody = new JObject();
      TimeZoneInfo userTimeZone = TimeZoneInfo.Local;

      JArray envelopes = (body["envelopes"] as JArray) ?? new JArray();
      JArray activities = new JArray();
      int top = string.IsNullOrEmpty(query.Get("top")) ? 3: int.Parse(query.Get("top"));
      int skip = string.IsNullOrEmpty(query.Get("skip")) ? 0: int.Parse(query.Get("skip"));

      var crmOrgUrl = GetHostFromUrl(query.Get("crmOrgUrl"));
      var recordId = query.Get("recordId") ?? null;
      var crmType = query.Get("crmType").ToString().Equals("Dynamics365") ? "CRMToken" : "SFToken";
      var recordType = query.Get("recordType");

      string[] filters = { crmType, crmOrgUrl, recordId, recordType };
      envelopes = GetFilteredEnvelopes(envelopes, filters);

      foreach (var envelope in envelopes)
      {
        DateTime statusUpdateTime = envelope["statusChangedDateTime"].ToObject<DateTime>();
        DateTime statusUpdateTimeInLocalTimeZone = TimeZoneInfo.ConvertTimeFromUtc(statusUpdateTime, userTimeZone);
        JArray recipientNames = new JArray();
        System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;
        foreach (var recipient in (envelope["recipients"]["signers"] as JArray) ?? new JArray())
        {
          recipientNames.Add(recipient["name"]);
        }

        JObject additionalPropertiesForActivity = new JObject()
        {
          ["Recipients"] = string.Join(", ", recipientNames),
          ["Sender Name"] = envelope["sender"]["userName"],
          ["Status"] = textInfo.ToTitleCase(envelope["status"].ToString()),
          ["Date"] = statusUpdateTimeInLocalTimeZone.ToString("h:mm tt, M/d/yy")
        };
        activities.Add(new JObject()
        {
          ["title"] = envelope["emailSubject"],
          ["description"] = GetDescriptionNLPForRelatedActivities(envelope),
          ["dateTime"] = statusUpdateTimeInLocalTimeZone.ToString("h:mm tt, M/d/yy"),
          ["url"] = GetEnvelopeUrl(envelope),
          ["additionalProperties"] = additionalPropertiesForActivity,
        });
      }

      newBody["value"] = (activities.Count < top) ? activities : new JArray(activities.Skip(skip).Take(top).ToArray());
      newBody["hasMoreResults"] = (skip + top < activities.Count) ? true : false;

      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("scp-get-key-sales".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      JObject newBody = new JObject();
      TimeZoneInfo userTimeZone = TimeZoneInfo.Local;

      JArray envelopes = (body["envelopes"] as JArray) ?? new JArray();
      JArray documentRecords = new JArray();
      int top = string.IsNullOrEmpty(query.Get("top")) ? 3: int.Parse(query.Get("top"));
      int skip = string.IsNullOrEmpty(query.Get("skip")) ? 0: int.Parse(query.Get("skip"));

      var crmOrgUrl = GetHostFromUrl(query.Get("crmOrgUrl"));
      var recordId = query.Get("recordId") ?? null;
      var crmType = query.Get("crmType").ToString().Equals("Dynamics365") ? "CRMToken" : "SFToken";
      var recordType = query.Get("recordType");
      string[] filters = { crmType, recordId, crmOrgUrl, recordType};

      envelopes = GetFilteredEnvelopes(envelopes, filters);

      foreach (var envelope in envelopes)
      {
        DateTime statusUpdateTime = envelope["statusChangedDateTime"].ToObject<DateTime>();
        DateTime statusUpdateTimeInLocalTimeZone = TimeZoneInfo.ConvertTimeFromUtc(statusUpdateTime, userTimeZone);
        System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;
        JArray recipientNames = new JArray();
        foreach (var recipient in (envelope["recipients"]["signers"] as JArray) ?? new JArray())
        {
          recipientNames.Add(recipient["name"]);
        }
        JArray documentNames = new JArray(
        (envelope["envelopeDocuments"] as JArray)?.Select(envelopeDocument => envelopeDocument["name"]));

        JObject additionalProperties = new JObject()
        {
          ["Recipients"] = string.Join(", ", recipientNames),
          ["Sender Name"] = envelope["sender"]["userName"],
          ["Status"] = textInfo.ToTitleCase(envelope["status"].ToString()),
          ["Date"] = statusUpdateTimeInLocalTimeZone.ToString("h:mm tt, M/d/yy"),
          ["Documents"] = string.Join(",", documentNames)
        };

        documentRecords.Add(new JObject()
        {
          ["Title"] = envelope["emailSubject"],
          ["description"] = GetDescriptionNLPForRelatedActivities(envelope),
          ["url"] = GetEnvelopeUrl(envelope),
          ["additionalProperties"] = additionalProperties
        });
      }

      newBody["value"] = (documentRecords.Count < top) ? documentRecords : new JArray(documentRecords.Skip(skip).Take(top).ToArray());
      newBody["hasMoreResults"] = (skip + top < documentRecords.Count) ? true : false;
      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("scp-get-related-records".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      JObject newBody = new JObject();
      TimeZoneInfo userTimeZone = TimeZoneInfo.Local;

      JArray envelopes = (body["envelopes"] as JArray) ?? new JArray();
      JArray documentRecords = new JArray();
      int top = string.IsNullOrEmpty(query.Get("top")) ? 3: int.Parse(query.Get("top"));
      int skip = string.IsNullOrEmpty(query.Get("skip")) ? 0: int.Parse(query.Get("skip"));

      var crmOrgUrl = GetHostFromUrl(query.Get("crmOrgUrl"));
      var recordId = query.Get("recordId") ?? null;
      var crmType = query.Get("crmType").ToString().Equals("Dynamics365") ? "CRMToken" : "SFToken";
      var recordType = query.Get("recordType");
      string[] filters = { crmType, recordId, crmOrgUrl, recordType};

      envelopes = GetFilteredEnvelopes(envelopes, filters);

      foreach (var envelope in envelopes)
      {
        DateTime statusUpdateTime = envelope["statusChangedDateTime"].ToObject<DateTime>();
        DateTime statusUpdateTimeInLocalTimeZone = TimeZoneInfo.ConvertTimeFromUtc(statusUpdateTime, userTimeZone);
        System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;
        JArray recipientNames = new JArray();
        foreach (var recipient in (envelope["recipients"]["signers"] as JArray) ?? new JArray())
        {
          recipientNames.Add(recipient["name"]);
        }

        JObject additionalPropertiesForDocumentRecords = new JObject()
        {
          ["Recipients"] = string.Join(", ", recipientNames),
          ["Sender Name"] = envelope["sender"]["userName"],
          ["Status"] = textInfo.ToTitleCase(envelope["status"].ToString()),
          ["Date"] = statusUpdateTimeInLocalTimeZone.ToString("h:mm tt, M/d/yy")
        };

        documentRecords.Add(new JObject()
        {
          ["recordId"] = envelope["envelopeId"],
          ["recordTypeDisplayName"] = "Agreement",
          ["recordTypePluralDisplayName"] = "Agreements",
          ["recordType"] = "Agreement",
          ["recordTitle"] = envelope["emailSubject"],
          ["url"] = GetEnvelopeUrl(envelope),
          ["additionalProperties"] = additionalPropertiesForDocumentRecords
        });
      }

      newBody["value"] = (documentRecords.Count < top) ? documentRecords : new JArray(documentRecords.Skip(skip).Take(top).ToArray());
      newBody["hasMoreResults"] = (skip + top < documentRecords.Count) ? true : false;

      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if (("ListEnvelopes".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)) || 
    ("SalesCopilotListEnvelopes".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)) ||
    ("SearchListEnvelopes".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      JObject newBody = new JObject();

      int top = string.IsNullOrEmpty(query.Get("top")) ? 10: int.Parse(query.Get("top"));
      int skip = string.IsNullOrEmpty(query.Get("skip")) ? 0: int.Parse(query.Get("skip"));

      JArray envelopes = (body["envelopes"] as JArray) ?? new JArray();
      JArray filteredEnvelopes = new JArray();
      var filteredEnvelopesDetails = new JArray();
      var recipientName = query.Get("recipientName") ?? null;
      var recipientEmailId = query.Get("recipientEmailId") ?? null;
      var envelopeTitle = query.Get("envelopeTitle") ?? null;
      var customFieldName = query.Get("customFieldName") ?? null;
      var customFieldValue = query.Get("customFieldValue") ?? null;

      var envelopeFilterMap = new Dictionary<string, string>() {
        {"recipientName", recipientName},
        {"recipientEmailId", recipientEmailId},
        {"envelopeTitle", envelopeTitle},
        {"customFieldName", customFieldName},
        {"customFieldValue", customFieldValue}
      };

      foreach (var filter in envelopeFilterMap.Keys) 
      {
        if (envelopeFilterMap[filter] != null)
        {
          switch (filter)
          {
            case "recipientName":
            case "recipientEmailId":
              filteredEnvelopes = new JArray(envelopes.Where(envelope =>
                envelope["recipients"]?.ToString().ToLower().Contains(envelopeFilterMap[filter].ToString().ToLower()) ?? false));
              break;
            case "envelopeTitle":
              filteredEnvelopes = new JArray(envelopes.Where(envelope =>
                envelope["emailSubject"]?.ToString().ToLower().Contains(envelopeFilterMap[filter].ToString().ToLower()) ?? false));
              break;
            case "customFieldName":
            case "customFieldValue":
              filteredEnvelopes = new JArray(envelopes.Where(envelope =>
              {
                var customFields = envelope["customFields"] as JToken;
                return customFields?.ToString().ToLower().Contains(envelopeFilterMap[filter].ToString().ToLower()) ?? false;
              }));
              break;
            default:
              break;
          }

          if (filteredEnvelopes.Count > 0)
          {
            envelopes.Clear();
            envelopes = new JArray(filteredEnvelopes);
            filteredEnvelopes.Clear();
          }
          else
          {
            envelopes.Clear();
            break;
          }
        }
      }

      filteredEnvelopesDetails = this.Context.OperationId.Contains("SalesCopilot") ? 
        GetFilteredEnvelopeDetailsForSalesCopilot(envelopes) :
        GetFilteredEnvelopeDetails(envelopes);

      newBody["value"] = (filteredEnvelopesDetails.Count < top) ? 
        filteredEnvelopesDetails : 
        new JArray(filteredEnvelopesDetails.Skip(skip).Take(top).ToArray());

      newBody["hasMoreResults"] = (skip + top < filteredEnvelopesDetails.Count) ? true : false;

      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("GetRecipientFields".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);

      var matchingSigner = new JObject();
      var newBody = new JObject();
      var recipientEmailId = query.Get("recipientEmail");
      var recipientId = query.Get("recipientId");
      var phoneNumber = query.Get("areaCode") + " " + query.Get("phoneNumber");
      var signerPhoneNumber = "";

      string [] signerTypes = {
        "signers", "agents", "editors", "carbonCopies", "certifiedDeliveries", "intermediaries",
        "inPersonSigners", "seals", "witnesses", "notaries"
      };

      for(var i = 0; i < signerTypes.Length; i++)
      {
        foreach(var signer in body[signerTypes[i]])
        {
          if (recipientEmailId?.ToString() == signer.SelectToken("email")?.ToString())
          {
            matchingSigner = signer as JObject;
            break;
          }

          if (recipientId?.ToString() == signer.SelectToken("recipientId")?.ToString())
          {
            matchingSigner = signer as JObject;
            break;
          }

          if (query.Get("phoneNumber") != null)
          {
            phoneNumber = Regex.Replace(phoneNumber, @"[^a-zA-Z0-9]", "");

            signerPhoneNumber = 
              signer.ToString().Contains("phoneAuthentication") ? 
                signer["phoneAuthentication"]["senderProvidedNumbers"][0].ToString() 
              : signer.ToString().Contains("identityVerification") ? 
                signer["identityVerification"]["inputOptions"][0]["phoneNumberList"][0]["countryCode"].ToString() + " " +
                signer["identityVerification"]["inputOptions"][0]["phoneNumberList"][0]["number"].ToString()
              : signer.ToString().Contains("additionalNotifications") ? 
                signer["additionalNotifications"][0]["phoneNumber"]["countryCode"].ToString() + " " + 
                signer["additionalNotifications"][0]["phoneNumber"]["number"].ToString()
              : signer.ToString().Contains("smsAuthentication") ? 
                signer["smsAuthentication"]["senderProvidedNumbers"][0].ToString() 
              : signer.ToString().Contains("phoneNumber") ? 
                signer["phoneNumber"]["countryCode"].ToString() + " " + signer["phoneNumber"]["number"].ToString() : "0";

            signerPhoneNumber = Regex.Replace(signerPhoneNumber, @"[^a-zA-Z0-9]", "");

            if (phoneNumber.ToString().Equals(signerPhoneNumber))
            {
              matchingSigner = signer as JObject;
              break;
            }
          }
        }
      }

      if ((recipientEmailId == null) && (query.Get("phoneNumber") == null) && (recipientId == null))
      {
        throw new ConnectorException(HttpStatusCode.BadRequest, "ValidationFailure: Please fill either Recipient Email or Phone Number or recipient Id to retrieve Recipient information");
      } 

      if (string.IsNullOrEmpty((string)matchingSigner["recipientIdGuid"]))
      {
        throw new ConnectorException(HttpStatusCode.BadRequest, "ValidationFailure: No recipient found for the given information");
      }
      else 
      {
        newBody["recipientId"] = matchingSigner["recipientId"];
        newBody["routingOrder"] = matchingSigner["routingOrder"];
        newBody["roleName"] = matchingSigner["roleName"];
        newBody["name"] = matchingSigner["name"];
        newBody["email"] = matchingSigner["email"];
        newBody["recipientType"] = matchingSigner["recipientType"];
        newBody["verificationType"] = matchingSigner["verificationType"];
        newBody["recipientIdGuid"] = matchingSigner["recipientIdGuid"];
      }

      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("ListEnvelopeDocuments".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      JArray envelopeDocuments = new JArray();
      JObject newBody = new JObject();

      if (body["envelopeDocuments"] == null)
      {
        throw new ConnectorException(HttpStatusCode.BadRequest, "ValidationFailure: Envelope do not contain documents");
      }

      foreach(var document in body["envelopeDocuments"] as JArray ?? new JArray())
      {
        envelopeDocuments.Add(new JObject()
        {
          ["documentId"] = document["documentId"],
          ["name"] = document["name"]
        });
      }

      newBody["envelopeDocuments"] = envelopeDocuments;
      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
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

      var signers = (body["signers"] as JArray) ?? new JArray();

      foreach (var signer in signers)
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

    if ("GetDynamicRecipients".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var itemProperties = new JObject();

      // Add email body
      itemProperties["emailBlurb"] = new JObject
      {
        ["type"] = "string",
        ["x-ms-summary"] = "Email body",
        ["description"] = "Email body"
      };

      // Generate a recipient role name to reicipient data object mapping
      Dictionary<string, JObject> recipientData = GenerateRecipientsMappings(body);

      // generate flattened-recipient related fields and add to itemProperties
      // for instance, for role name "tester", with tab type "textTab" and tab label "text label",
      // generate field name: "Tester:::textTabs:::Text label", that can be easily parsed out before sending API request to DS
      GenerateRecipientInformationFields(recipientData, itemProperties);

      // generate flattened-custom fields and add to itemProperties
      GenerateCustomFields(body, itemProperties);

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

    if ("ListTemplateDocuments".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      JArray documents = new JArray();
      JObject newBody = new JObject();

      if (body["templateDocuments"] == null)
      {
        throw new ConnectorException(HttpStatusCode.BadRequest, "ValidationFailure: Specified template do not contain documents");
      }

      foreach(var document in body["templateDocuments"] as JArray ?? new JArray())
      {
        documents.Add(new JObject()
        {
          ["documentId"] = document["documentId"],
          ["name"] = document["name"]
        });
      }

      newBody["templateDocuments"] = documents;
      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("GetCustomFields".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var itemProperties = new JObject();
      var basePropertyDefinition = new JObject
      {
        ["type"] = "string"
      };

      var count = 0;
      foreach (var customField in (body["textCustomFields"] as JArray) ?? new JArray())
      {
        var name = customField["name"].ToString() + " [Custom Field Text]";

        if (customField["required"].ToString() == "true")
        {
          name = "* " + name;
        }

        if (customField["show"].ToString() == "false") 
        {
          name = name + " [hidden]";
        }

        itemProperties[name] = basePropertyDefinition.DeepClone();
        count++;
      }
      
      foreach (var customField in (body["listCustomFields"] as JArray) ?? new JArray())
      {
        var name = customField["name"].ToString() + " [Custom Field List]";

        if (customField["required"].ToString() == "true") 
        {
          name = "* " + name;
        }

        if (customField["show"].ToString() == "false") 
        {
          name = name + " [hidden]";
        }

        var definition = basePropertyDefinition.DeepClone();
        definition["enum"] = customField["listItems"];
        itemProperties[name] = definition;
        count++;
      }

      if (count == 0)
      {
        itemProperties["Custom Field [optional]"] = new JObject
          {
            ["type"] = "string",
            ["x-ms-visibility"]= "advanced"
          };
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

    if ("GetAccountCustomFields".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var customFields = new JArray();

      foreach (var customField in (body["textCustomFields"] as JArray) ?? new JArray())
      {
       customFields.Add(customField);
      }
      
      foreach (var customField in (body["listCustomFields"] as JArray) ?? new JArray())
      {
       customFields.Add(customField);
      }

      var newBody = new JObject
      {
        ["customFields"] = customFields
      };
      
      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("GetEnvelopeCustomField".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      var customFieldName = query.Get("fieldName");
      var responseCustomField = new JObject();

      foreach (var customField in (body["textCustomFields"] as JArray) ?? new JArray())
      {
        if (customField["name"].ToString().Equals(customFieldName))
        {
          customField["fieldType"] = "Text";
          responseCustomField = customField as JObject;
          break;
        }
      }
      
      foreach (var customField in (body["listCustomFields"] as JArray) ?? new JArray())
      {
        if (customField["name"].ToString().Equals(customFieldName))
        {
          customField["fieldType"] = "List";
          responseCustomField = customField as JObject;
          break;
        }
      }
      
      response.Content = new StringContent(responseCustomField.ToString(), Encoding.UTF8, "application/json");
    }

    if ("UpdateEnvelopeCustomField".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var responseCustomField = new JObject();

      foreach (var customField in (body["textCustomFields"] as JArray) ?? new JArray())
      {
        responseCustomField = customField as JObject;
        responseCustomField["fieldType"] = "Text";
        break;
      }
      
      foreach (var customField in (body["listCustomFields"] as JArray) ?? new JArray())
      {
        responseCustomField = customField as JObject;
        responseCustomField["fieldType"] = "List";
        break;
      }
      
      response.Content = new StringContent(responseCustomField.ToString(), Encoding.UTF8, "application/json");
    }

    if ("AddRecipientToEnvelopeV2".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      var recipientType = query.Get("recipientType");
      var newBody = new JObject();
      var signers = new JArray();

      if(recipientType.Equals("inPersonSigners"))
      {
        signers = body["inPersonSigners"] as JArray;
        signers[0]["name"] = signers[0]["hostName"];
        signers[0]["email"] = signers[0]["hostEmail"];
      }
      else
      {
        signers = body[recipientType] as JArray;
      }

      foreach (var signer in signers)
      {
        newBody = signer as JObject;
        break;
      }

      if (newBody["errorDetails"] != null)
      {
        throw new ConnectorException(HttpStatusCode.BadRequest, "ValidationFailure: " + newBody["errorDetails"]["message"]);
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

    if ("GetEnvelopeDocumentInfo".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      var documentName = query.Get("documentName");
      var newBody = new JObject();

      foreach (var documentInfo in (body["envelopeDocuments"] as JArray) ?? new JArray())
      {
        if (documentName.Equals(documentInfo["name"].ToString()))
        {
          newBody["documentId"] = documentInfo["documentId"];
          newBody["documentIdGuid"] = documentInfo["documentIdGuid"];
          newBody["name"] = documentInfo["name"];
          break;
        }
      }

      if (newBody["documentId"] == null)
      {
        throw new ConnectorException(HttpStatusCode.BadRequest, "ValidationFailure: No document found matching the provided name");
      }

      response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
    }

    if ("CreateEnvelopeFromTemplateNoRecipients".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase) ||
        "SendEnvelope".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
    {
      var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
      var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
      var templateId = query.Get("templateId");
      body["templateId"] = templateId;

      response.Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
    }

    if (response.Content?.Headers?.ContentType != null)
    {
      if (("GetDocuments".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)) ||
      ("GetDocumentsV2".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase)))
      {
        var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);

        response.Content.Headers.ContentType = uriBuilder.Path.Contains("Archive") ? new MediaTypeHeaderValue("application/zip") :
          new MediaTypeHeaderValue("application/pdf");
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

    public string JsonMessage()
    {
      var error = new StringBuilder($"{{\"ConnectorException\": \"Status code={this.StatusCode}, Message='{this.Message}'\"}}");
      return error.ToString();
    }

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