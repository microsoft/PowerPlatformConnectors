public class Script : ScriptBase
{

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        switch (this.Context.OperationId)
        {
            case "GetSimpleJournalEntrySchema":
                return await this.HandleGetSimpleJournalEntrySchemaOperation().ConfigureAwait(false);

            case "GetSplitJournalEntrySchema":
                return await this.HandleGetSplitJournalEntrySchemaOperation().ConfigureAwait(false);

            case "CreateSimpleJournalEntry":
                return await this.HandleCreateSimpleJournalEntryOperation().ConfigureAwait(false);

            case "CreateSplitJournalEntry":
                return await this.HandleCreateSplitJournalEntryOperation().ConfigureAwait(false);

            case "CreateJournalEntryBatchAttachment":
                return await this.HandleCreateAttachmentOperation().ConfigureAwait(false);

        }

        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent($"Unhandled operation ID '{this.Context.OperationId}'");
        return response;
    }
    
    private async Task<HttpResponseMessage> HandleGetSimpleJournalEntrySchemaOperation()
    {
        // build the URL to call ListTransactionCodes
        var uriBuilder = new UriBuilder("https://api.sky.blackbaud.com");
        uriBuilder.Path = "/generalledger/v1/transactioncodes";
        this.Context.Request.RequestUri = uriBuilder.Uri;

        // call the ListTransactionCodes endpoint
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        // if the call is successful, build the schema from the transaction codes that are defined
        if (response.IsSuccessStatusCode)
        {
            // get the response string, convert it to a JArray
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            var transactionCodes = JArray.Parse(responseString);

            // build the simple journal entry properties
            var properties = new JObject() {
                ["account_number"] = new JObject() {
                    ["type"] = "string",
                    ["description"] = "The journal entry account number.",
                    ["x-ms-summary"] = "Account number"
                },
                ["post_date"] = new JObject() {
                    ["type"] = "string",
                    ["format"] = "date",
                    ["description"] = "The journal entry post date (ex: '2024-09-18').",
                    ["x-ms-summary"] = "Post date"
                },
                ["encumbrance"] = new JObject() {
                    ["type"] = "string",
                    ["description"] = "The journal entry encumbrance type.",
                    ["x-ms-summary"] = "Encumbrance",
                    ["enum"] = new JArray(
                        "Regular",
                        "Encumbrance"
                    )
                },
                ["journal"] = new JObject() {
                    ["type"] = "string",
                    ["description"] = "The name of the journal.",
                    ["x-ms-summary"] = "Journal",
                    ["x-ms-dynamic-values"] = new JObject() {
                        ["operationId"] = "ListJournalCodes",
                        ["value-path"] = "journal",
                        ["value-title"] = "journal"
                    },
                    ["x-ms-dynamic-list"] = new JObject() {
                        ["operationId"] = "ListJournalCodes",
                        ["itemValuePath"] = "journal",
                        ["itemTitlePath"] = "journal"
                    }
                },
                ["reference"] = new JObject() {
                    ["type"] = "string",
                    ["description"] = "The journal entry reference.",
                    ["x-ms-summary"] = "Journal reference"
                },
                ["type_code"] = new JObject() {
                    ["type"] = "string",
                    ["description"] = "The journal entry transaction type.",
                    ["x-ms-summary"] = "Type code",
                    ["enum"] = new JArray(
                        "Debit",
                        "Credit"
                    )
                },
                ["amount"] = new JObject() {
                    ["type"] = "number",
                    ["format"] = "double",
                    ["description"] = "The journal entry amount.",
                    ["x-ms-summary"] = "Amount"
                },
                ["ui_project_id"] = new JObject() {
                    ["type"] = "string",
                    ["description"] = "The user-visible identifier of the project.",
                    ["x-ms-summary"] = "Project"
                },
                ["account_class"] = new JObject() {
                    ["type"] = "string",
                    ["description"] = "The class for this distribution.",
                    ["x-ms-summary"] = "Class",
                    ["x-ms-dynamic-values"] = new JObject() {
                        ["operationId"] = "ListClasses",
                        ["value-path"] = "value",
                        ["value-title"] = "value"
                    },
                    ["x-ms-dynamic-list"] = new JObject() {
                        ["operationId"] = "ListClasses",
                        ["itemValuePath"] = "value",
                        ["itemTitlePath"] = "value"
                    }
                }
            };
  
            // if the array contains any items, append them as new fields to the schema
            if (transactionCodes?.Count() > 0)
            {
                var metaProperties = new JObject();
                var metaRequired = new JArray();

                var i = 0;
                foreach (var tc in transactionCodes)
                {
                    var name = $"transaction_code_{(++i).ToString()}";
                    
                    // add a property to represent the transaction code dropdown
                    properties.Add(name, new JObject() {
                        ["type"] = "string",
                        ["description"] = "The transaction code value.",
                        ["x-ms-summary"] = tc["name"], 
                        ["x-ms-dynamic-values"] = new JObject() {
                            ["operationId"] = "ListTransactionCodeValues",
                            ["value-path"] = "value",
                            ["value-title"] = "value",
                            ["parameters"] = new JObject() {
                                ["transaction_code_id"] = tc["transaction_code_id"]
                            }
                        },
                        ["x-ms-dynamic-list"] = new JObject() {
                            ["operationId"] = "ListTransactionCodeValues",
                            ["itemValuePath"] = "value",
                            ["itemTitlePath"] = "value",
                            ["parameters"] = new JObject() {
                                ["transaction_code_id"] = new JObject() {
                                    ["value"] = tc["transaction_code_id"]
                                }
                            }
                        }
                    });

                    // define a hidden field to store the transaction code name, which will be used later when building the array of transaction codes
                    metaProperties.Add(name, new JObject() {
                        ["type"] = "string",
                        ["description"] = "The transaction code name.",
                        ["x-ms-summary"] = $"m.{name}",
                        ["default"] = tc["name"],
                        ["x-ms-visibility"] = "internal"
                    });

                    metaRequired.Add(name);
                }
            
                properties.Add("meta", new JObject() {
                    ["type"] = "object",
                    ["description"] = "The transaction code metadata.",
                    ["required"] = metaRequired,
                    ["properties"] = metaProperties
                });
            }

            // add additional properties
            properties.Add("notes", new JObject() {
                ["type"] = "string",
                ["maxLength"] = 1000,
                ["minLength"] = 0,
                ["description"] = "The journal entry notes. Character limit: 1000",
                ["x-ms-summary"] = "Note"
            });
            properties.Add("reverse_date", new JObject() {
                ["type"] = "string",
                ["format"] = "date",
                ["description"] = "The journal entry reverse date (ex: '2024-09-18').",
                ["x-ms-summary"] = "Reverse on"
            });
            properties.Add("relative_source_url", new JObject() {
                ["type"] = "string",
                ["description"] = "Relative URL for the journal entry that appends to the batch source base URL.",
                ["x-ms-summary"] = "Relative source URL",
                ["x-ms-visibility"] = "advanced"
            });
            properties.Add("tax_entity_id", new JObject() {
                ["type"] = "integer",
                ["format"] = "int32",
                ["description"] = "The journal entry tax entity ID.",
                ["x-ms-summary"] = "Tax entity ID",
                ["x-ms-visibility"] = "advanced"
            });

            // finish building up the resulting object
            var result = new JObject() {
                ["schema"] = new JObject() {
                    ["type"] = "object",
                    ["description"] = "The distribution schema.",
                    ["required"] = new JArray(
                        "account_number",
                        "post_date",
                        "encumbrance",
                        "journal",
                        "reference",
                        "type_code",
                        "amount",
                        "meta"
                    ),
                    ["properties"] = properties
                }
            };

            // set the new response content
            response.Content = CreateJsonContent(result.ToString());
        }

        return response;
    }

    private async Task<HttpResponseMessage> HandleGetSplitJournalEntrySchemaOperation()
    {
        // build the URL to call ListTransactionCodes
        var uriBuilder = new UriBuilder("https://api.sky.blackbaud.com");
        uriBuilder.Path = "/generalledger/v1/transactioncodes";
        this.Context.Request.RequestUri = uriBuilder.Uri;

        // call the ListTransactionCodes endpoint
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        // if the call is successful, build the schema from the transaction codes that are defined
        if (response.IsSuccessStatusCode)
        {
            // get the response string, convert it to a JArray
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            var transactionCodes = JArray.Parse(responseString);

            // build the simple journal entry properties
            // todo: factor this out
            var properties = new JObject() {
                ["account_number"] = new JObject() {
                    ["type"] = "string",
                    ["description"] = "The journal entry account number.",
                    ["x-ms-summary"] = "Account number"
                },
                ["post_date"] = new JObject() {
                    ["type"] = "string",
                    ["format"] = "date",
                    ["description"] = "The journal entry post date (ex: '2024-09-18').",
                    ["x-ms-summary"] = "Post date"
                },
                ["encumbrance"] = new JObject() {
                    ["type"] = "string",
                    ["description"] = "The journal entry encumbrance type.",
                    ["x-ms-summary"] = "Encumbrance",
                    ["enum"] = new JArray(
                        "Regular",
                        "Encumbrance"
                    )
                },
                ["journal"] = new JObject() {
                    ["type"] = "string",
                    ["description"] = "The name of the journal.",
                    ["x-ms-summary"] = "Journal",
                    ["x-ms-dynamic-values"] = new JObject() {
                        ["operationId"] = "ListJournalCodes",
                        ["value-path"] = "journal",
                        ["value-title"] = "journal"
                    },
                    ["x-ms-dynamic-list"] = new JObject() {
                        ["operationId"] = "ListJournalCodes",
                        ["itemValuePath"] = "journal",
                        ["itemTitlePath"] = "journal"
                    }
                },
                ["reference"] = new JObject() {
                    ["type"] = "string",
                    ["description"] = "The journal entry reference.",
                    ["x-ms-summary"] = "Journal reference"
                },
                ["type_code"] = new JObject() {
                    ["type"] = "string",
                    ["description"] = "The journal entry transaction type.",
                    ["x-ms-summary"] = "Type code",
                    ["enum"] = new JArray(
                        "Debit",
                        "Credit"
                    )
                },
                ["amount"] = new JObject() {
                    ["type"] = "number",
                    ["format"] = "double",
                    ["description"] = "The journal entry amount.",
                    ["x-ms-summary"] = "Amount"
                }
            };

            var distributionProperties = new JObject() {
                    ["amount"] = new JObject() {
                        ["type"] = "number",
                        ["format"] = "double",
                        ["description"] = "The distribution split amount.",
                        ["x-ms-summary"] = "amt"
                    },
                    ["ui_project_id"] = new JObject() {
                        ["type"] = "string",
                        ["description"] = "The user-visible identifier of the project.",
                        ["x-ms-summary"] = "project"
                    },
                    ["account_class"] = new JObject() {
                        ["type"] = "string",
                        ["description"] = "The distribution class.",
                        ["x-ms-summary"] = "class",
                        ["x-ms-dynamic-values"] = new JObject() {
                            ["operationId"] = "ListClasses",
                            ["value-path"] = "value",
                            ["value-title"] = "value"
                        },
                        ["x-ms-dynamic-list"] = new JObject() {
                            ["operationId"] = "ListClasses",
                            ["itemValuePath"] = "value",
                            ["itemTitlePath"] = "value"
                        }
                    }
                };

            // if the array contains any items, append them as new fields to the schema
            if (transactionCodes?.Count() > 0)
            {
                var metaProperties = new JObject();
                var metaRequired = new JArray();

                var i = 0;
                foreach (var tc in transactionCodes)
                {
                    var name = $"transaction_code_{(++i).ToString()}";

                    // add a property to represent the transaction code dropdown
                    distributionProperties.Add(name, new JObject() {
                        ["type"] = "string",
                        ["description"] = tc["name"],
                        ["x-ms-summary"] = "t-code", 
                        ["x-ms-dynamic-values"] = new JObject() {
                            ["operationId"] = "ListTransactionCodeValues",
                            ["value-path"] = "value",
                            ["value-title"] = "value",
                            ["parameters"] = new JObject() {
                                ["transaction_code_id"] = tc["transaction_code_id"]
                            }
                        },
                        ["x-ms-dynamic-list"] = new JObject() {
                            ["operationId"] = "ListTransactionCodeValues",
                            ["itemValuePath"] = "value",
                            ["itemTitlePath"] = "value",
                            ["parameters"] = new JObject() {
                                ["transaction_code_id"] = new JObject() {
                                    ["value"] = tc["transaction_code_id"]
                                }
                            }
                        }
                    });

                    // define a hidden field to store the transaction code name, which will be used later when building the array of transaction codes
                    metaProperties.Add(name, new JObject() {
                        ["type"] = "string",
                        ["description"] = "The transaction code name.",
                        ["x-ms-summary"] = $"m.{name}",
                        ["default"] = tc["name"],
                        ["x-ms-visibility"] = "internal"
                    });

                    metaRequired.Add(name);
                }
            
                properties.Add("meta", new JObject() {
                    ["type"] = "object",
                    ["description"] = "The transaction code metadata.",
                    ["required"] = metaRequired,
                    ["properties"] = metaProperties
                });
            }

            properties.Add("distributions", new JObject() {
                ["type"] = "array",
                ["description"] = "The journal entry distributions",
                ["items"] = new JObject() {
                    ["type"] = "object",
                    ["description"] = "The split distribution for the journal entry.",
                    ["x-ms-summary"] = "Distribution",
                    ["required"] = new JArray(
                        "amount"
                    ),
                    ["properties"] = distributionProperties
                }
            });

            // add additional properties
            properties.Add("notes", new JObject() {
                ["type"] = "string",
                ["maxLength"] = 1000,
                ["minLength"] = 0,
                ["description"] = "The journal entry notes. Character limit: 1000",
                ["x-ms-summary"] = "Note"
            });
            properties.Add("reverse_date", new JObject() {
                ["type"] = "string",
                ["format"] = "date",
                ["description"] = "The journal entry reverse date (ex: '2024-09-18').",
                ["x-ms-summary"] = "Reverse on"
            });
            properties.Add("relative_source_url", new JObject() {
                ["type"] = "string",
                ["description"] = "Relative URL for the journal entry that appends to the batch source base URL.",
                ["x-ms-summary"] = "Relative source URL",
                ["x-ms-visibility"] = "advanced"
            });
            properties.Add("tax_entity_id", new JObject() {
                ["type"] = "integer",
                ["format"] = "int32",
                ["description"] = "The journal entry tax entity ID.",
                ["x-ms-summary"] = "Tax entity ID",
                ["x-ms-visibility"] = "advanced"
            });

            // finish building up the resulting object
            var result = new JObject() {
                ["schema"] = new JObject() {
                    ["type"] = "object",
                    ["description"] = "The distribution schema.",
                    ["required"] = new JArray(
                        "account_number",
                        "post_date",
                        "encumbrance",
                        "journal",
                        "reference",
                        "type_code",
                        "amount",
                        "distributions",
                        "meta"
                    ),
                    ["properties"] = properties
                }
            };

            // set the new response content
            response.Content = CreateJsonContent(result.ToString());
        }

        return response;
    }

    private async Task<HttpResponseMessage> HandleCreateSimpleJournalEntryOperation()
    {
        // transform the NewSimpleJournalEntry object into the shape needed by the backend

        // first, get the object from the request
        var requestContent = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var requestJson = JObject.Parse(requestContent);
        var meta = requestJson["meta"];

        // when testing the custom connector, the meta object must be provided manually in the request body.
        // because this action uses dynamic schema, the test harness GUI will have the "raw body" toggle disabled, 
        // so you'll have to provide the complete expected JSON body, which includes the hidden+required meta property.
        if (!requestJson.ContainsKey("meta"))
        {
            var r = new HttpResponseMessage(HttpStatusCode.BadRequest);
            r.Content = CreateJsonContent("No meta property was provided with the request.");
            return r;
        }
        
        // collect any transaction codes
        var transactionCodeValues = new JArray();
        foreach (var tc in requestJson.Properties().Where(p => p.Name.StartsWith("transaction_code_")).Select(p => p.Name)) 
        {
            transactionCodeValues.Add(new JObject() {
                ["name"] = meta[tc],
                ["value"] = requestJson[tc]
            });
        }

        // create the single distribution
        var distribution = new JObject() {
            ["amount"] = requestJson["amount"]
        };

        // add optional values
        if (requestJson.ContainsKey("ui_project_id"))
        {
            distribution.Add("ui_project_id", requestJson["ui_project_id"]);
        };
        if (requestJson.ContainsKey("account_class"))
        {
            distribution.Add("account_class", requestJson["account_class"]);
        }
        if (transactionCodeValues.Any())
        {
            distribution.Add("transaction_code_values", transactionCodeValues);
        }

        // store the distribution in an array
        requestJson.Add("distributions", new JArray(distribution));

        // remove properties not used by the backend (should be ignored, but let's not risk it)
        requestJson.Remove("ui_project_id");
        requestJson.Remove("account_class");
        requestJson.Remove("transaction_code_1");
        requestJson.Remove("transaction_code_2");
        requestJson.Remove("transaction_code_3");
        requestJson.Remove("transaction_code_4");
        requestJson.Remove("transaction_code_5");
        requestJson.Remove("meta");

        // place the journal entry into an array
        var newBody = new JArray(requestJson);

        this.Context.Logger.LogInformation($"JSON = {newBody.ToString()}");

        // send the request to the backend and return the response
        this.Context.Request.Content = CreateJsonContent(newBody.ToString());
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        return response;
    }

    private async Task<HttpResponseMessage> HandleCreateSplitJournalEntryOperation()
    {
        // transform the NewSplitJournalEntry object into the shape needed by the backend
        // todo: not yet complete

        // first, get the object from the request
        var requestContent = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var requestJson = JObject.Parse(requestContent);
        var meta = requestJson["meta"];

        // when testing the custom connector, the meta object must be provided manually in the request body.
        // because this action uses dynamic schema, the test harness GUI will have the "raw body" toggle disabled, 
        // so you'll have to provide the complete expected JSON body, which includes the hidden+required meta property.
        if (!requestJson.ContainsKey("meta"))
        {
            var r = new HttpResponseMessage(HttpStatusCode.BadRequest);
            r.Content = CreateJsonContent("No meta property was provided with the request.");
            return r;
        }
        
        // collect any transaction codes
        var transactionCodeValues = new JArray();
        foreach (var tc in requestJson.Properties().Where(p => p.Name.StartsWith("transaction_code_")).Select(p => p.Name)) 
        {
            transactionCodeValues.Add(new JObject() {
                ["name"] = meta[tc],
                ["value"] = requestJson[tc]
            });
        }

        // create the single distribution
        var distribution = new JObject() {
            ["amount"] = requestJson["amount"]
        };

        // add optional values
        if (requestJson.ContainsKey("ui_project_id"))
        {
            distribution.Add("ui_project_id", requestJson["ui_project_id"]);
        };
        if (requestJson.ContainsKey("account_class"))
        {
            distribution.Add("account_class", requestJson["account_class"]);
        }
        if (transactionCodeValues.Any())
        {
            distribution.Add("transaction_code_values", transactionCodeValues);
        }

        // store the distribution in an array
        requestJson.Add("distributions", new JArray(distribution));

        // remove properties not used by the backend (should be ignored, but let's not risk it)
        requestJson.Remove("ui_project_id");
        requestJson.Remove("account_class");
        requestJson.Remove("transaction_code_1");
        requestJson.Remove("transaction_code_2");
        requestJson.Remove("transaction_code_3");
        requestJson.Remove("transaction_code_4");
        requestJson.Remove("transaction_code_5");
        requestJson.Remove("meta");

        // place the journal entry into an array
        var newBody = new JArray(requestJson);

        this.Context.Logger.LogInformation($"JSON = {newBody.ToString()}");

        // send the request to the backend and return the response
        this.Context.Request.Content = CreateJsonContent(newBody.ToString());
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        return response;
    }

    private async Task<HttpResponseMessage> HandleCreateAttachmentOperation() 
    {
        // This logic is based on the docs here:
        // https://developer.blackbaud.com/skyapi/support/changelog/fenxt/2018#october
        //
        // Note that this routine is used by other FENXT connectors, so please keep changes in sync

        // first, get the object from the request
        var requestContent = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var requestJson = JObject.Parse(requestContent);

        // determine the attachment type
        var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        var type = requestJson["type"].ToString();

        // for physical attachments, transform to a multipart request
        if (type.Equals("Physical")) {

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new StringContent(requestJson["parent_id"].ToString()), "ParentId");
            multipartContent.Add(new StringContent(requestJson["name"].ToString()), "Name");
            multipartContent.Add(new StringContent("Physical"), "Type");
            multipartContent.Add(new StringContent(requestJson["media_type"].ToString()), "MediaType");

            var fileJson = (JObject)requestJson["file"];
            var stream = new MemoryStream(Convert.FromBase64String(fileJson["$content"].ToString()));
            var streamContent = new StreamContent(stream);

            var filename = requestJson["file_name"]?.ToString() ?? "attachment";
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(fileJson["$content-type"].ToString());
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") {
                Name = "File",
                FileName = filename
            };

            multipartContent.Add(streamContent, "file");

            // update the request content
            this.Context.Request.Content = multipartContent;
        }

        // send the request to SKY API
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
}