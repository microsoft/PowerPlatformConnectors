public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Update request data.
        await this.UpdateRequest().ConfigureAwait(false);

        // Send the request and get the response.
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken)
            .ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            // Update response data.
            await this.UpdateResponse(response).ConfigureAwait(false);
        }

        return response;
    }

    private async Task UpdateRequest()
    {
        if ("SendDocumentFromTemplate".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            await this.TransformRequestJsonBody(this.SendDocumentFromTemplateBodyTransformation).ConfigureAwait(false);
            this.Context.Request.Headers.Add("RequestedFrom", "PowerAutomate");
        }

        // update Accept Header
        this.Context.Request.Headers.Accept.Clear();
        const string acceptHeaderValue = "application/json";
        this.Context.Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeaderValue));
    }

    private async Task UpdateResponse(HttpResponseMessage response)
    {
        if ("SendDocumentFromTemplate".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            var documentId = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
            response.Content = new StringContent(documentId.ToString(), Encoding.UTF8, "application/json");
        }

        if ("GetTemplateDynamicSigners".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
            var itemProperties = new JObject();

            var basePropertyDefinition = new JObject
            {
                ["type"] = "string",
                ["x-ms-keyOrder"] = 0,
                ["x-ms-keyType"] = "none",
                ["x-ms-sort"] = "none",
                ["x-ms-visibility"] = "important",
            };

            var currentUri = this.Context.Request.RequestUri;
            var uriBuilder = new UriBuilder(currentUri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            var autoReminder = query.Get("enableAutoReminder");

            if (autoReminder != null && autoReminder.ToString() == "true")
            {
                var reminderFrequency = basePropertyDefinition.DeepClone();
                reminderFrequency["type"] = "integer";
                reminderFrequency["description"] = "Specify the number of days between reminder emails";
                reminderFrequency["x-ms-visibility"] = "advanced";
                reminderFrequency["default"] = 3;
                itemProperties["Reminder frequency"] = reminderFrequency;

                var reminderCount = basePropertyDefinition.DeepClone();
                reminderCount["type"] = "integer";
                reminderCount["description"] = "Set the maximum number of reminders to be sent";
                reminderCount["x-ms-visibility"] = "advanced";
                reminderCount["default"] = 5;
                itemProperties["Reminder count"] = reminderCount;
            }

            var signers = (body["roles"] as JArray) ?? new JArray();

            foreach (var signer in signers)
            {
                var roleName = signer["name"];
                var signerNameDefinition = basePropertyDefinition.DeepClone();
                signerNameDefinition["description"] = "Provide signer name";
                var signerEmailDefinition = basePropertyDefinition.DeepClone();
                signerEmailDefinition["description"] = "Provide signer email";
                itemProperties[roleName + " Name"] = signerNameDefinition;
                itemProperties[roleName + " Email"] = signerEmailDefinition;

                var signerFormFields = (signer["formFields"] as JArray) ?? new JArray();
                var radioButtonFields = new JArray();

                foreach (var formField in signerFormFields)
                {
                    var formFieldType = formField["type"].ToString();
                    var fieldId = formField["id"];
                    var defaultValue = formField["value"].ToString();
                    var fieldDefinition = basePropertyDefinition.DeepClone();

                    if (formFieldType.Equals("textbox", StringComparison.OrdinalIgnoreCase))
                    {
                        fieldDefinition["description"] = "Provide a default value for the textbox field";

                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            fieldDefinition["default"] = defaultValue;
                        }

                        itemProperties[fieldId + " (Textbox)"] = fieldDefinition;
                    }

                    if (formFieldType.Equals("editabledate", StringComparison.OrdinalIgnoreCase))
                    {
                        var format = formField["editableDateFieldSettings"]["dateFormat"];
                        fieldDefinition["description"] = $"Provide a default value for the editable date field in {format} format";

                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            fieldDefinition["default"] = defaultValue;
                        }

                        itemProperties[fieldId + " (Editable date)"] = fieldDefinition;
                    }

                    if (formFieldType.Equals("checkbox", StringComparison.OrdinalIgnoreCase))
                    {
                        fieldDefinition["description"] = "Preselect a checkbox to be checked if necessary";
                        fieldDefinition["enum"] = new JArray { "on", "off" };
                        fieldDefinition["default"] = defaultValue;
                        itemProperties[fieldId + " (Checkbox)"] = fieldDefinition;
                    }

                    if (formFieldType.Equals("dropdown", StringComparison.OrdinalIgnoreCase))
                    {
                        var dropdownList = formField["dropdownOptions"] as JArray;
                        fieldDefinition["description"] = "Preselect a dropdown value if necessary";
                        fieldDefinition["enum"] = dropdownList;

                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            fieldDefinition["default"] = defaultValue;
                        }

                        itemProperties[fieldId + " (Dropdown)"] = fieldDefinition;
                    }

                    if (formFieldType.Equals("radiobutton", StringComparison.OrdinalIgnoreCase))
                    {
                        radioButtonFields.Add(formField);
                    }
                }

                if (radioButtonFields != null)
                {
                    var radioGroup = radioButtonFields.GroupBy(x => (string)x["groupName"])
                        .ToDictionary(y => y.Key, y => y.ToList());

                    foreach (var group in radioGroup)
                    {
                        var radioGroupDefinition = basePropertyDefinition.DeepClone();
                        radioGroupDefinition["description"] = "Preselect a radio button to be selected if necessary";
                        var radioButtonList = new JArray();
                        var defaultRadioButton = string.Empty;

                        foreach (var radio in group.Value)
                        {
                            var radioId = radio["label"].ToString();
                            var radioValue = radio["value"].ToString();
                            radioButtonList.Add(radioId);

                            if (radioValue == "on")
                            {
                                defaultRadioButton = radioId;
                            }
                        }

                        radioGroupDefinition["enum"] = radioButtonList;

                        if (!string.IsNullOrEmpty(defaultRadioButton))
                        {
                            radioGroupDefinition["default"] = defaultRadioButton;
                        }

                        itemProperties[group.Key + " (Radio button)"] = radioGroupDefinition;
                    }
                }
            }

            var commonFields = (body["commonFields"] as JArray) ?? new JArray();

            foreach (var field in commonFields)
            {
                if (field["type"].ToString().Equals("label", StringComparison.OrdinalIgnoreCase))
                {
                    var fieldId = field["id"];
                    var defaultValue = field["value"].ToString();
                    var fieldDefinition = basePropertyDefinition.DeepClone();
                    fieldDefinition["description"] = "Provide a value for the label field (commonly displayed for all signers)";

                    if (!string.IsNullOrEmpty(defaultValue))
                    {
                        fieldDefinition["default"] = defaultValue;
                    }

                    itemProperties[fieldId + " (Label)"] = fieldDefinition;
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
                    },
                },
            };

            response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
        }

        if ("SenderIdentityList".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
        {
            var body = ParseContentAsJObject(await response.Content.ReadAsStringAsync().ConfigureAwait(false), false);
            var senderIdentityList = new JArray();
            var senderIdentities = (body["result"] as JArray) ?? new JArray();

            foreach (var senderIdentity in senderIdentities)
            {
                if (senderIdentity["status"] != null && senderIdentity["status"].ToString().Equals("Verified", StringComparison.OrdinalIgnoreCase))
                {
                    senderIdentityList.Add(senderIdentity);
                }
            }

            var newBody = new JObject
            {
                ["result"] = senderIdentityList,
            };

            response.Content = new StringContent(newBody.ToString(), Encoding.UTF8, "application/json");
        }

        if (response.Content?.Headers?.ContentType != null)
        {
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
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

    private JObject SendDocumentFromTemplateBodyTransformation(JObject body)
    {
        var templateRoles = new JArray();
        var signer = new JObject();
        var existingSignerFields = new JArray();
        var autoReminderSettings = new JObject();

        var currentUri = this.Context.Request.RequestUri;
        var uriBuilder = new UriBuilder(currentUri);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        var newBody = new JObject()
        {
            ["title"] = query.Get("title"),
            ["message"] = query.Get("message"),
            ["brandId"] = query.Get("brandId"),
            ["expiryValue"] = query.Get("expiryDays"),
            ["enablePrintAndSign"] = query.Get("enablePrintAndSign"),
            ["enableReassign"] = query.Get("enableReassign"),
            ["isSandbox"] = query.Get("isSandbox"),
        };

        if (query.Get("cc") != null)
        {
            var ccEmails = new JArray();

            foreach (var email in query.Get("cc").ToString().Trim().Split(','))
            {
                var ccEmail = new JObject()
                {
                    ["emailAddress"] = email.Trim(),
                };
                ccEmails.Add(ccEmail);
            }

            newBody["cc"] = ccEmails;
        }

        if (query.Get("hideDocumentId") != null)
        {
            newBody["hideDocumentId"] = query.Get("hideDocumentId");
        }

        if (query.Get("labels") != null)
        {
            var labelsList = new JArray();

            foreach (var label in query.Get("labels").ToString().Trim().Split(','))
            {
                labelsList.Add(label.Trim());
            }

            newBody["labels"] = labelsList;
        }

        if (query.Get("onBehalfOf") != null)
        {
            newBody["onBehalfOf"] = query.Get("onBehalfOf");
        }

        if (query.Get("enableAutoReminder") != null && query.Get("enableAutoReminder").ToString() == "true")
        {
            autoReminderSettings["enableAutoReminder"] = query.Get("enableAutoReminder");
        }

        var templateId = query.Get("templateId");
        query["templateId"] = templateId;
        uriBuilder.Query = query.ToString();
        this.Context.Request.RequestUri = uriBuilder.Uri;

        var roleIndex = 1;

        foreach (var property in body)
        {
            var value = (string)property.Value;
            var key = (string)property.Key;

            if (key.Contains(" Name"))
            {
                signer["signerRole"] = key.Substring(0, key.Length - 5);
                signer["signerName"] = value;
                signer["roleIndex"] = roleIndex++;
            }

            if (key.Contains(" Email"))
            {
                signer["signerEmail"] = value;
                templateRoles.Add(signer);
                signer = new JObject();
            }
        }

        foreach (var property in body)
        {
            var value = (string)property.Value;
            var key = (string)property.Key;

            if (key.Contains(" (Label)"))
            {
                var labelField = new JObject()
                {
                    ["id"] = key.Substring(0, key.Length - 8).Trim(),
                    ["value"] = value,
                };
                existingSignerFields.Add(labelField);
            }

            if (key.Contains(" (Textbox)"))
            {
                var textboxField = new JObject()
                {
                    ["id"] = key.Substring(0, key.Length - 10).Trim(),
                    ["value"] = value,
                };
                existingSignerFields.Add(textboxField);
            }

            if (key.Contains(" (Editable date)"))
            {
                var editableDateField = new JObject()
                {
                    ["id"] = key.Substring(0, key.Length - 16).Trim(),
                    ["value"] = value,
                };
                existingSignerFields.Add(editableDateField);
            }

            if (key.Contains(" (Checkbox)"))
            {
                var checkboxField = new JObject()
                {
                    ["id"] = key.Substring(0, key.Length - 11).Trim(),
                    ["value"] = value,
                };
                existingSignerFields.Add(checkboxField);
            }

            if (key.Contains(" (Dropdown)"))
            {
                var dropdownField = new JObject()
                {
                    ["id"] = key.Substring(0, key.Length - 11).Trim(),
                    ["value"] = value,
                };
                existingSignerFields.Add(dropdownField);
            }

            if (key.Contains(" (Radio button)"))
            {
                var radioButtonField = new JObject()
                {
                    ["id"] = key.Substring(0, key.Length - 15).Trim(),
                    ["value"] = value,
                };
                existingSignerFields.Add(radioButtonField);
            }

            if (key.Contains("Reminder frequency"))
            {
                autoReminderSettings["reminderDays"] = value;
            }

            if (key.Contains("Reminder count"))
            {
                autoReminderSettings["reminderCount"] = value;
            }
        }

        if (existingSignerFields != null)
        {
            templateRoles[0]["existingFormFields"] = existingSignerFields;
        }

        if (autoReminderSettings != null)
        {
            newBody["reminderSettings"] = autoReminderSettings;
        }

        newBody["roles"] = templateRoles;

        return newBody;
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

            throw new ConnectorException(HttpStatusCode.BadGateway, "Unable to parse the response body", ex);
        }

        if (body == null)
        {
            if (isRequest)
            {
                throw new ConnectorException(HttpStatusCode.BadRequest, "The request body is empty");
            }

            throw new ConnectorException(HttpStatusCode.BadGateway, "The response body is empty");
        }

        return body;
    }

    public class ConnectorException : Exception
    {
        public ConnectorException(
            HttpStatusCode statusCode,
            string message,
            Exception innerException = null)
            : base(message, innerException)
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
