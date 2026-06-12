public class Script : ScriptBase
{

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        switch (this.Context.OperationId)
        {
            case "CreateInvoice": 
                return await this.HandleCreateInvoiceOperation().ConfigureAwait(false);

            case "EditInvoice": 
                return await this.HandleEditInvoiceOperation().ConfigureAwait(false);

            case "CreateVendor": 
                return await this.HandleCreateVendorOperation().ConfigureAwait(false);

            case "EditVendorAddress":
                return await this.HandleEditVendorAddressOperation().ConfigureAwait(false);

            case "CreateInvoiceAttachment": 
            case "CreateVendorAttachment":
                return await this.HandleCreateAttachmentOperation().ConfigureAwait(false);

            case "CreatePurchaseOrderLineItem":
                return await this.HandleCreatePurchaseOrderLineItemOperation().ConfigureAwait(false);

            case "CreateSingleDistributionInvoice":
                return await this.HandleCreateSingleDistributionInvoiceOperation().ConfigureAwait(false);

            case "GetSingleDistributionInvoiceSchema":
                return await this.HandleGetSingleDistributionInvoiceSchemaOperation().ConfigureAwait(false);

            case "ListInvoiceVendorAddresses":
                return await this.HandleListInvoiceVendorAddressesOperation().ConfigureAwait(false);
        }

        return CreateErrorResponse(HttpStatusCode.BadRequest, $"Unhandled operation ID '{this.Context.OperationId}'");
    }

    private static HttpResponseMessage CreateErrorResponse(HttpStatusCode statusCode, string message)
    {
        var response = new HttpResponseMessage(statusCode);
        response.Content = CreateJsonContent(new JObject() {
            ["error"] = new JObject() {
                ["message"] = message
            }
        }.ToString());

        return response;
    }    

    private async Task<JArray> GetVendorDefaultDistributions(int vendorId) {

        // get the default distributions for the specified vendor, and fail if the request is not successful

        // build the URL to call ListVendorDefaultDistributions
        var uriBuilder = new UriBuilder("https://api.sky.blackbaud.com");
        uriBuilder.Path = $"accountspayable/v1/vendors/{vendorId}/defaultdistributions";

        using var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

        // Copy the subscription key and Authorization headers from the request
        request.Headers.Authorization = this.Context.Request.Headers.Authorization;
        request.Headers.Add("Bb-Api-Subscription-Key", this.Context.Request.Headers.GetValues("Bb-Api-Subscription-Key").FirstOrDefault<string>());

        var response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode) 
        { 
            return null; 
        }

        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
        var defaultDistributionsResponseJson = JObject.Parse(responseString);
        var defaultDistributionsJson = defaultDistributionsResponseJson["value"] as JArray;

        return defaultDistributionsJson;
    }

    private static Dictionary<int, decimal> SplitByPercentages(decimal amount, Dictionary<int, decimal> percentages)
    {
        // this routine is from the FENXT backend - it splits the given amount by the percentages in the given dictionary
        // and returns the split amounts in similarly-keyed dictionary.  This is used to generate distribution amounts
        // accounting for rounding in a consistent fashion with FENXT.

        if (percentages.Count() == 0)
        {
            return new Dictionary<int, decimal>();
        }

        var roughAmounts = percentages.ToDictionary(p => p.Key, p => Math.Round(p.Value * amount, 2));
        var roughAmountsSum = roughAmounts.Values.Sum();
        var diffToBeSpread = Math.Abs(amount - roughAmountsSum);
        var diffModifier = amount > roughAmountsSum ? 1 : -1;
        var fractionalDiff = diffToBeSpread % (percentages.Count() * 0.01m);

        if (diffToBeSpread > fractionalDiff)
        {
            roughAmounts = roughAmounts.ToDictionary(p => p.Key, p => p.Value + (diffModifier * Math.Floor(diffToBeSpread / (percentages.Count() * 0.01m))));
        }

        for (int i = 1; i <= fractionalDiff / 0.01m; i++)
        {
            var curDist = roughAmounts.ElementAt(roughAmounts.Count() - i);
            roughAmounts[curDist.Key] = curDist.Value + (diffModifier * 0.01m);
        }

        return roughAmounts;
    }

    private async Task<int> GetInvoiceVendorId(int invoiceId) {

        var uriBuilder = new UriBuilder("https://api.sky.blackbaud.com");
        uriBuilder.Path = $"/accountspayable/v1/invoices/{invoiceId}";

        using var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

        // Copy the subscription key and Authorization headers from the request
        request.Headers.Authorization = this.Context.Request.Headers.Authorization;
        request.Headers.Add("Bb-Api-Subscription-Key", this.Context.Request.Headers.GetValues("Bb-Api-Subscription-Key").FirstOrDefault<string>());

        var response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode) {
            return 0;
        }

        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
        var invoiceJson = JObject.Parse(responseString);
        var vendorId = (int)invoiceJson["vendor_id"];

        return vendorId;
    }

    private async Task<int> GetPurchaseOrderVendorId(int purchaseOrderId) {

        var uriBuilder = new UriBuilder("https://api.sky.blackbaud.com");
        uriBuilder.Path = $"/accountspayable/v1/purchaseorders/{purchaseOrderId}";

        using var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

        // Copy the subscription key and Authorization headers from the request
        request.Headers.Authorization = this.Context.Request.Headers.Authorization;
        request.Headers.Add("Bb-Api-Subscription-Key", this.Context.Request.Headers.GetValues("Bb-Api-Subscription-Key").FirstOrDefault<string>());

        var response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode) {
            return 0;
        }

        var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
        var invoiceJson = JObject.Parse(responseString);
        var vendorId = (int)invoiceJson["vendor_id"];

        return vendorId;
    }

    private async Task<HttpResponseMessage> HandleCreatePurchaseOrderLineItemOperation() 
    {
        // This handles transforming the line item shape from the request into the shape needed by the CreateLineItems endpoint.
        // It generates the line item distributions from the line item extended cost amount and the vendor default distributions.

        // first, get the line item object from the request
        var lineItemContentString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var lineItemJson = JObject.Parse(lineItemContentString);

        // get the purchase order ID from the request
        var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
        var paths = uriBuilder.Path.Split('/').Skip(2).ToArray();
        var purchaseOrderId = int.Parse(paths[Array.IndexOf(paths, "purchaseorders") + 1]);

        // get the vendor ID from the purchase order
        var vendorId = await this.GetPurchaseOrderVendorId(purchaseOrderId).ConfigureAwait(false);      
        if (vendorId == 0)
        {
            return CreateErrorResponse(HttpStatusCode.BadRequest, $"The vendor for purchase order {purchaseOrderId} could not be determined.");
        }

        // get the default distributions for the vendor
        var defaultDistributionsJson = await this.GetVendorDefaultDistributions(vendorId).ConfigureAwait(false);

        // The SKY API actually allows an empty distribution list for purchase order line items.  So for now, given the challenges with 
        // computing the distributions (and prompting for the credit account to use), just leave the distributions off.  In the future, 
        // we can implement logic for computing the line item distributions based on the vendor default distributions.
        // // if there are no vendor default distributions, we must fail because line item distributions are required
        // if (defaultDistributionsJson?.Count == 0)
        // {
        //     return CreateErrorResponse(HttpStatusCode.BadRequest, "No default distributions were found for the specified vendor.");
        // }

        var distributions = new JArray();
        
        // update the line item request with the distributions
        lineItemJson["distributions"] = distributions;

        // stuff the single line item into an array as expected by the backend
        var newBody = new JArray(lineItemJson);

        // send the request to SKY API
        this.Context.Request.Content = CreateJsonContent(newBody.ToString());

        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
    
    private async Task<HttpResponseMessage> HandleCreateInvoiceOperation()
    {
        // This handles transforming the invoice shape from the request into the shape needed by the CreateInvoice endpoint.  
        // It generates the invoice distributions from the invoice amount and credit account and the vendor default distributions.

        // first, get the invoice object from the request
        var invoiceContentString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var invoiceJson = JObject.Parse(invoiceContentString);

        var vendorId = (int)invoiceJson["vendor_id"];
        var defaultDistributionsJson = await this.GetVendorDefaultDistributions(vendorId).ConfigureAwait(false);

        // if there are no vendor default distributions, we must fail because invoice distributions are required
        if (defaultDistributionsJson?.Count == 0) 
        {
            return CreateErrorResponse(HttpStatusCode.BadRequest, "No default distributions were found for the specified vendor.");
        }

        // This logic is based on the (internal) Invoice Import API within FENXT.
        // SKY API only returns default debits, so use them to calculate amounts so that percentages add up to 100% and we can be sure that paired
        // debits and credits end up with the same amount.
        var defaultPercentages = defaultDistributionsJson.ToDictionary(d => (int)d["distribution_id"], d => (decimal)d["percent"] / 100 );
        var amounts = SplitByPercentages((decimal)invoiceJson["amount"], defaultPercentages);

        // SKY API default distributions only include debits, so we have to ask the user to supply the credit account to use
        // when generating distributions
        var creditAccount = (string)invoiceJson["credit_account_number"];
        
        // generate distributions
        var distributions = new JArray(defaultDistributionsJson.SelectMany(d => new JObject[] {
            // debit
            new JObject() {
                ["type_code"] = "Debit",
                ["account_number"] = d["account_number"],
                ["descripton"] = d["descripton"],
                ["amount"] = amounts[(int)d["distribution_id"]],
                ["distribution_splits"] = new JArray(((JArray)d["default_distribution_splits"]).Select(split => new JObject(){
                    ["ui_project_id"] = split["ui_project_id"],
                    ["account_class"] = split["account_class"],
                    ["percent"] = split["percent"],
                    ["transaction_code_values"] = new JArray(((JArray)split["transaction_code_values"]).Select(tc => new JObject(){
                        ["id"] = tc["id"],
                        ["name"] = tc["name"],
                        ["value"] = tc["value"]
                    }))
                })),
                ["custom_fields"] = d["custom_fields"]
            },
            
            // credit
            new JObject() {
                ["type_code"] = "Credit",
                ["account_number"] = creditAccount,
                ["descripton"] = d["descripton"],
                ["amount"] = amounts[(int)d["distribution_id"]],
                ["distribution_splits"] = new JArray(((JArray)d["default_distribution_splits"]).Select(split => new JObject(){
                    ["ui_project_id"] = split["ui_project_id"],
                    ["account_class"] = split["account_class"],
                    ["percent"] = split["percent"],
                    ["transaction_code_values"] = new JArray(((JArray)split["transaction_code_values"]).Select(tc => new JObject(){
                        ["id"] = tc["id"],
                        ["name"] = tc["name"],
                        ["value"] = tc["value"]
                    }))
                })),
                ["custom_fields"] = d["custom_fields"]
            },
        }));    

        // compute distribution splits amounts
        foreach (var d in distributions)
        {
            var splits = (JArray)d["distribution_splits"];

            // if the distribution isn't split, then use the distribution amount
            if (splits?.Count == 1)
            {
                var first = splits.First();
                first["amount"] = (decimal)d["amount"];
                ((JObject)first).Remove("percent");
            } 
            else if (splits?.Count > 1)
            {
                // otherwise, split the distribution amount accordingly
                var splitPercentages = splits?.ToDictionary(s => s.GetHashCode(), s => (decimal)s["percent"] / 100 );
                var splitAmounts = SplitByPercentages((decimal)d["amount"], splitPercentages);
                foreach (var split in splits)
                {
                    split["amount"] = splitAmounts[split.GetHashCode()];
                    ((JObject)split).Remove("percent");
                }
            }
        }

        // update the invoice request with the distributions
        invoiceJson["distributions"] = distributions;
        var newBody = invoiceJson;       

        // send the request to SKY API
        this.Context.Request.Content = CreateJsonContent(newBody.ToString());

        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private async Task<HttpResponseMessage> HandleEditInvoiceOperation()
    {
        // The initial connector definition used the wrong field name "payment_details" - this script replaces that with the expected field "invoice_payment_detail".

        // first, get the invoice object from the request
        var invoiceContentString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var invoiceJson = JObject.Parse(invoiceContentString);

        // replace "payment_details" with "invoice_payment_detail" if present
        if (invoiceJson.ContainsKey("payment_details"))
        {
            invoiceJson.Add("invoice_payment_detail", invoiceJson["payment_details"]);
            invoiceJson.Remove("payment_details");
        }

        var newBody = invoiceJson;

        // send the request to SKY API
        this.Context.Request.Content = CreateJsonContent(newBody.ToString());

        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private async Task<HttpResponseMessage> HandleCreateVendorOperation()
    {
        // Conver the single address object on the request to an array as expected by the backend

        // first, get the vendor object from the request
        var vendorContentString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var vendorJson = JObject.Parse(vendorContentString);

        // if the address object is present, convert it to an array
        if (vendorJson.ContainsKey("address"))
        {
            vendorJson["address"] = new JArray(vendorJson["address"]);
        }

        var newBody = vendorJson;

        // send the request to SKY API
        this.Context.Request.Content = CreateJsonContent(newBody.ToString());

        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    private async Task<HttpResponseMessage> HandleEditVendorAddressOperation()
    {
        // The initial connector definition used the wrong field name "issue_1099s" - this script replaces that with the expected field "is_1099".

        // first, get the vendor address object from the request
        var vendorAddressContentString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var vendorAddressJson = JObject.Parse(vendorAddressContentString);

        // replace "issue_1099s" with "is_1099" if present
        if (vendorAddressJson.ContainsKey("issue_1099s"))
        {
            vendorAddressJson.Add("is_1099", vendorAddressJson["issue_1099s"]);
            vendorAddressJson.Remove("issue_1099s");
        }

        var newBody = vendorAddressJson;

        // send the request to SKY API
        this.Context.Request.Content = CreateJsonContent(newBody.ToString());

        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
        var type = requestJson["type"].ToString();

        // for physical attachments, transform to a multipart request
        if (type.Equals("Physical"))
        {

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
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
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

    private async Task<HttpResponseMessage> HandleCreateSingleDistributionInvoiceOperation()
    {
        // transform the NewSingleDistributionInvoice object into the shape needed by the backend

        // first, get the object from the request
        var requestContent = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var requestJson = JObject.Parse(requestContent);
        var meta = requestJson["meta"];

        // when testing the custom connector, the meta object must be provided manually in the request body.
        // because this action uses dynamic schema, the test harness GUI will have the "raw body" toggle disabled, 
        // so you'll have to provide the complete expected JSON body, which includes the hidden+required meta property.
        if (!requestJson.ContainsKey("meta"))
        {
            return CreateErrorResponse(HttpStatusCode.BadRequest, "No meta property was provided with the request.");
        }

        // transform the distribution object to a single-distribution debit for the invoice amount
        var distributionJson = requestJson["distribution"] as JObject;
        var debitAccountNumber = distributionJson["debit_account_number"];
        var creditAccountNumber = distributionJson["credit_account_number"];
        var transactionCodeValues = new JArray();

        // transform the single split object (if present)
        var distributionSplitJson = distributionJson["distribution_split"] as JObject;
        if (distributionSplitJson is null) {
            distributionSplitJson = new JObject();
        
        } else {
            // collect any transaction codes and place into the transaction_code_values array
            foreach (var tc in distributionSplitJson.Properties().Where(p => p.Name.StartsWith("transaction_code_")).Select(p => p.Name)) 
            {
                transactionCodeValues.Add(new JObject() {
                    ["name"] = meta[tc],
                    ["value"] = distributionSplitJson[tc]
                });
            }

            // remove distribution split fields not used by the backend (should be ignored, but let's not risk it)
            distributionSplitJson.Remove("transaction_code_1");
            distributionSplitJson.Remove("transaction_code_2");
            distributionSplitJson.Remove("transaction_code_3");
            distributionSplitJson.Remove("transaction_code_4");
            distributionSplitJson.Remove("transaction_code_5");
        }

        // for single-distribution invoices, the split amount should match the invoice amount
        distributionSplitJson["amount"] = requestJson["amount"];
        distributionJson["type_code"] = "Debit";
        distributionJson["account_number"] = debitAccountNumber;
        distributionJson["amount"] = requestJson["amount"];
        distributionSplitJson.Add("transaction_code_values", transactionCodeValues);

        // put the single distribution split into an array
        distributionJson.Add("distribution_splits", new JArray(distributionSplitJson));

        // remove distribution fields not used by the backend (should be ignored, but let's not risk it)
        distributionJson.Remove("distribution_split");
        distributionJson.Remove("debit_account_number");
        distributionJson.Remove("credit_account_number");

        // put the distribution into an array
        requestJson.Add("distributions", new JArray(distributionJson));

        // create a copy of the debit distribution for the corresponding credit distribution
        distributionJson["type_code"] = "Credit";
        distributionJson["account_number"] = creditAccountNumber;
        ((JArray)requestJson["distributions"]).Add(distributionJson);

        // remove root properties not used by the backend (should be ignored, but let's not risk it)
        requestJson.Remove("distribution");
        requestJson.Remove("meta");

        // compose the new body
        var newBody = requestJson;

        this.Context.Logger.LogInformation(newBody.ToString());

        // send the request to the backend and return the response
        this.Context.Request.Content = CreateJsonContent(newBody.ToString());
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
    
    private async Task<HttpResponseMessage> HandleGetSingleDistributionInvoiceSchemaOperation()
    {
        // build the URL to call ListTransactionCodes
        var uriBuilder = new UriBuilder("https://api.sky.blackbaud.com");
        uriBuilder.Path = "/accountspayable/v1/transactioncodes";
        this.Context.Request.RequestUri = uriBuilder.Uri;

        // call the ListTransactionCodes endpoint
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        // if the call is successful, build the schema from the transaction codes that are defined
        if (response.IsSuccessStatusCode)
        {
            // get the response string, convert it to a JArray
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            var transactionCodes = JArray.Parse(responseString);

            var distributionSplitProperties = new JObject() {
                ["ui_project_id"] = new JObject() {
                    ["type"] = "string",
                    ["description"] = "The user-visible identifier of the project for the distribution.",
                    ["x-ms-summary"] = "Project"
                },
                ["account_class"] = new JObject() {
                    ["type"] = "string",
                    ["description"] = "The class for the distribution.",
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

            // include a hidden field for the transaction code metadata so we don't have to perform a lookup later
            var metaProperties = new JObject();
  
            // if the array contains any items, append them as new fields to the schema
            if (transactionCodes?.Count() > 0)
            {
                var i = 0;
                foreach (var tc in transactionCodes)
                {
                    var name = $"transaction_code_{(++i).ToString()}";
                    
                    // add a property to represent the transaction code dropdown
                    distributionSplitProperties.Add(name, new JObject() {
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

                    // store the transaction code name as metadata, which will be used later when building the array of transaction codes
                    metaProperties.Add(name, new JObject() {
                        ["type"] = "string",
                        ["description"] = "The transaction code name.",
                        ["x-ms-summary"] = $"m.{name}",
                        ["default"] = tc["name"]
                    });
                }
            }

            // compose the schema for adding an invoice
            var result = new JObject() {
                ["schema"] = new JObject() {
                    ["type"] = "object",
                    ["description"] = "The schema for adding an invoice.",
                    ["required"] = new JArray(
                        "vendor_id",
                        "invoice_number",
                        "invoice_date",
                        "due_date",
                        "post_status",
                        "post_date",
                        "amount",
                        "description",
                        "approval_status",
                        "distribution",
                        "payment_details",
                        "meta"
                    ),
                    ["properties"] = new JObject() {
                        ["vendor_id"] = new JObject() {
                            ["type"] = "integer",
                            ["format"] = "int32",
                            ["description"] = "The system record ID of the vendor.",
                            ["x-ms-summary"] = "Vendor ID"
                        },        
                        ["invoice_number"] = new JObject() {
                            ["type"] = "string",
                            ["maxLength"] = 20,
                            ["description"] = "The invoice number.",
                            ["x-ms-summary"] = "Invoice number"
                        },
                        ["invoice_date"] = new JObject() {
                            ["type"] = "string",
                            ["format"] = "date",
                            ["description"] = "The invoice date (ex: '2005-09-18').",
                            ["x-ms-summary"] = "Invoice date"
                        },
                        ["due_date"] = new JObject() {
                            ["type"] = "string",
                            ["format"] = "date",
                            ["description"] = "The invoice due date (ex: '2005-09-18').",
                            ["x-ms-summary"] = "Due date"
                        },
                        ["post_status"] = new JObject() {
                            ["type"] = "string",
                            ["description"] = "The post status for the invoice.",
                            ["x-ms-summary"] = "Post status",
                            ["enum"] = new JArray(
                                "Posted",
                                "NotYetPosted",
                                "DoNotPost"
                            ),
                            ["x-ms-enum-values"] = new JArray(
                                new JObject() {
                                    ["value"] = "Posted",
                                    ["displayName"] = "Posted"
                                },
                                new JObject() {
                                    ["value"] = "NotYetPosted",
                                    ["displayName"] = "Not yet posted"
                                },
                                new JObject() {
                                    ["value"] = "DoNotPost",
                                    ["displayName"] = "Do not post"
                                }
                            )
                        },
                        ["post_date"] = new JObject() {
                            ["type"] = "string",
                            ["format"] = "date",
                            ["description"] = "The invoice post date (ex: '2005-09-18').",
                            ["x-ms-summary"] = "Post date"
                        },
                        ["amount"] = new JObject() {
                            ["type"] = "number",
                            ["format"] = "decimal",
                            ["description"] = "The invoice amount.",
                            ["x-ms-summary"] = "Amount"
                        },
                        ["description"] = new JObject() {
                            ["type"] = "string",
                            ["maxLength"] = 60,
                            ["description"] = "The invoice description.",
                            ["x-ms-summary"] = "Description"
                        },        
                        ["approval_status"] = new JObject() {
                            ["type"] = "string",
                            ["description"] = "The approval status for the invoice.",
                            ["x-ms-summary"] = "Approval status",
                            ["enum"] = new JArray(
                                "Pending",
                                "Approved",
                                "Deleted",
                                "Paid",
                                "PartiallyPaid"
                            ),
                            ["x-ms-enum-values"] = new JArray(
                                new JObject() {
                                    ["value"] = "Pending",
                                    ["displayName"] = "Pending"
                                },
                                new JObject() {
                                    ["value"] = "Approved",
                                    ["displayName"] = "Approved"
                                },
                                new JObject() {
                                    ["value"] = "Deleted",
                                    ["displayName"] = "Deleted"
                                },
                                new JObject() {
                                    ["value"] = "Paid",
                                    ["displayName"] = "Paid"
                                },
                                new JObject() {
                                    ["value"] = "PartiallyPaid",
                                    ["displayName"] = "Partially paid"
                                }
                            )                      
                        },
                        ["distribution"] = new JObject() {
                            ["type"] = "object",
                            ["description"] = "The invoice distribution.",
                            ["required"] = new JArray(
                                "debit_account_number",
                                "credit_account_number"
                            ),
                            ["properties"] = new JObject() {
                                ["debit_account_number"] = new JObject() {
                                    ["type"] = "string",
                                    ["description"] = "The debit account number.",
                                    ["x-ms-summary"] = "Debit account"
                                },                
                                ["credit_account_number"] = new JObject() {
                                    ["type"] = "string",
                                    ["description"] = "The credit account number.",
                                    ["x-ms-summary"] = "Credit account"
                                },                
                                ["description"] = new JObject() {
                                    ["type"] = "string",
                                    ["description"] = "The description for the distribution.",
                                    ["x-ms-summary"] = "Distribution description"
                                },
                                ["distribution_split"] = new JObject() {
                                    ["type"] = "object",
                                    ["description"] = "The distribution split.",
                                    ["properties"] = distributionSplitProperties
                                }
                            }
                        },
                        ["distribute_discounts"] = new JObject() {
                            ["type"] = "boolean",
                            ["description"] = "Distribute discounts to invoice expense accounts?",
                            ["x-ms-summary"] = "Distribute discounts?"
                        },
                        ["payment_details"] = new JObject() {
                            ["type"] = "object",
                            ["description"] = "The invoice payment details.",
                            ["required"] = new JArray(
                                "payment_method"
                            ),
                            ["properties"] = new JObject() {
                                ["payment_method"] = new JObject() {
                                    ["type"] = "string",
                                    ["description"] = "The invoice payment method.",
                                    ["x-ms-summary"] = "Payment method",
                                    ["enum"] = new JArray(
                                        "Check",
                                        "EFT",
                                        "BankDraft",
                                        "CreditCard"
                                    ),
                                    ["x-ms-enum-values"] = new JArray(
                                        new JObject() {
                                            ["value"] = "Check",
                                            ["displayName"] = "Check"
                                        },
                                        new JObject() {
                                            ["value"] = "EFT",
                                            ["displayName"] = "EFT"
                                        },
                                        new JObject() {
                                            ["value"] = "BankDraft",
                                            ["displayName"] = "Bank draft"
                                        },
                                        new JObject() {
                                            ["value"] = "CreditCard",
                                            ["displayName"] = "Credit card"
                                        }
                                    )
                                },
                                ["remit_to"] = new JObject() {
                                    ["type"] = "object",
                                    ["description"] = "The remit to address for the invoice.",
                                    ["required"] = new JArray(
                                        "address_id"
                                    ),
                                    ["properties"] = new JObject() {
                                        ["address_id"] = new JObject() {
                                            ["type"] = "integer",
                                            ["format"] = "int32",
                                            ["description"] = "The remit to address for the invoice.",
                                            ["x-ms-summary"] = "Remit to",
                                            ["x-ms-dynamic-values"] = new JObject() {
                                                ["operationId"] = "ListVendorAddresses",
                                                ["value-collection"] = "results",
                                                ["value-path"] = "address_id",
                                                ["value-title"] = "description",
                                                ["parameters"] = new JObject() {
                                                    ["vendor_id"] = new JObject() {
                                                        ["parameter"] = "vendor_id"
                                                    }
                                                }
                                            },
                                            ["x-ms-dynamic-list"] = new JObject() {
                                                ["operationId"] = "ListVendorAddresses",
                                                ["itemsPath"] = "results",
                                                ["itemValuePath"] = "address_id",
                                                ["itemTitlePath"] = "description",
                                                ["parameters"] = new JObject() {
                                                    ["vendor_id"] = new JObject() {
                                                        ["parameterReference"]  = "body/vendor_id"
                                                    }
                                                }
                                            }                                        
                                        }                                        
                                    }
                                },                                
                                ["paid_from"] = new JObject() {
                                    ["type"] = "string",
                                    ["description"] = "The bank from which the invoice is paid.",
                                    ["x-ms-summary"] = "Paid from"
                                },                                
                                ["credit_card_account_id"] = new JObject() {
                                    ["type"] = "integer",
                                    ["format"] = "int32",
                                    ["description"] = "The system record ID of the credit card account from which the invoice is paid.",
                                    ["x-ms-summary"] = "Card account ID"
                                },                                
                                ["credit_card_id"] = new JObject() {
                                    ["type"] = "integer",
                                    ["format"] = "int32",
                                    ["description"] = "The system record ID of the credit card from which the invoice is paid.",
                                    ["x-ms-summary"] = "Card ID"
                                },                                
                                ["hold_payment"] = new JObject() {
                                    ["type"] = "boolean",
                                    ["description"] = "Hold payment on this invoice?",
                                    ["x-ms-summary"] = "Hold payment?"
                                },
                                ["create_separate_payment"] = new JObject() {
                                    ["type"] = "boolean",
                                    ["description"] = "Always create a separate payment for this invoice?",
                                    ["x-ms-summary"] = "Separate payment?"
                                }
                            }
                        },
                        ["form_1099_box_numbers"] = new JObject() {
                            ["type"] = "array",
                            ["description"] = "The 1099 box numbers for the invoice.",
                            ["x-ms-summary"] = "1099",
                            ["x-ms-visibility"] = "advanced",
                            ["items"] = new JObject() {
                                ["type"] = "object",
                                ["description"] = "Box number",                                
                                ["required"] = new JArray(
                                    "number",
                                    "amount"
                                ),
                                ["properties"] = new JObject() {
                                    ["number"] = new JObject() {
                                        ["type"] = "string",
                                        ["description"] = "The box number.",
                                        ["x-ms-summary"] = "box number",
                                        ["x-ms-dynamic-values"] = new JObject() {
                                            ["operationId"] = "ListBoxNumbers",
                                            ["value-collection"] = "value"
                                        },
                                        ["x-ms-dynamic-list"] = new JObject() {
                                            ["operationId"] = "ListBoxNumbers",
                                            ["itemsPath"] = "value"
                                        }
                                    },
                                    ["state"] = new JObject() {
                                        ["type"] = "string",
                                        ["description"] = "The state for the box number.",
                                        ["x-ms-summary"] = "state"
                                    },
                                    ["amount"] = new JObject() {
                                        ["type"] = "number",
                                        ["format"] = "decimal",
                                        ["description"] = "The box number amount.",
                                        ["x-ms-summary"] = "amount"
                                    }
                                }
                            }
                        },
                        ["meta"] = new JObject() {
                            ["type"] = "object",
                            ["description"] = "The transaction code metadata.",
                            ["x-ms-visibility"] = "internal",
                            ["properties"] = metaProperties
                        }
                    }
                }
            };

            // set the new response content
            response.Content = CreateJsonContent(result.ToString());
        }

        return response;
    }

    private async Task<HttpResponseMessage> HandleListInvoiceVendorAddressesOperation()
    {
        // get the invoice ID from the request
        var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
        var paths = uriBuilder.Path.Split('/').Skip(2).ToArray();
        var invoiceId = int.Parse(paths[Array.IndexOf(paths, "invoices") + 1]);

        // call the GetInvoice endpoint to get the vendor ID
        var vendorId = await this.GetInvoiceVendorId(invoiceId).ConfigureAwait(false);      
        if (vendorId == 0)
        {
            return CreateErrorResponse(HttpStatusCode.BadRequest, $"The vendor for invoice {invoiceId} could not be determined.");
        }

        // now build the URL to call ListVendorAddresses
        uriBuilder.Path = $"/accountspayable/v1/vendors/{vendorId}/addresses";
        this.Context.Request.RequestUri = uriBuilder.Uri;

        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
}
 