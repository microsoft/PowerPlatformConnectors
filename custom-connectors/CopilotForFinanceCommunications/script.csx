using System.Net;

public class Script : ScriptBase
{
    private string pdfContent = @"%PDF-1.4
1 0 obj
<< /Type /Catalog /Pages 2 0 R >>
endobj
2 0 obj
<< /Type /Pages /Kids [3 0 R] /Count 1 >>
endobj
3 0 obj
<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Contents 4 0 R >>
endobj
4 0 obj
<< /Length 44 >>
stream
BT
/F1 24 Tf
100 700 Td
(Hello World!) Tj
ET
endstream
endobj
5 0 obj
<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>
endobj
xref
0 6
0000000000 65535 f
0000000010 00000 n
0000000056 00000 n
0000000103 00000 n
0000000175 00000 n
0000000380 00000 n
trailer
<< /Root 1 0 R /Size 6 >>
startxref
484
%%EOF";
    
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (this.Context.OperationId == "Contacts_list")
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["data"] = new JArray
                {
                    new JObject
                    {
                        ["id"] = "[usmf]000006",
                        ["organizationCode"] = "usmf",
                        ["customerId"] = "[usmf]US-008",
                        ["customerName"] = "Sparrow Retail",
                        ["firstName"] = "Adele",
                        ["lastName"] = "test",
                        ["email"] = "AdeleV@M365x91783967.OnMicrosoft.com",
                        ["primaryPhoneNumber"] = "1234567894zz",
                        ["professionalTitle"] = "Collector test 23"
                    },
                    new JObject
                    {
                        ["id"] = "[ussi]000007",
                        ["organizationCode"] = "ussi",
                        ["customerId"] = "[ussi]US009",
                        ["customerName"] = "Contoso",
                        ["firstName"] = "Tom",
                        ["lastName"] = "Contoso",
                        ["email"] = "test@teast.OnMicrosoft.com",
                        ["primaryPhoneNumber"] = "1234567894x",
                        ["professionalTitle"] = "Collector"
                    }
                }
            }.ToString());

            return response;
        }

        if (this.Context.OperationId == "Customers_list")
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["data"] = new JArray
                {
                    new JObject
                    {
                        ["id"] = "[usmf]US-008",
                        ["organizationCode"] = "usmf",
                        ["name"] = "Sparrow Retail",
                        ["accountNumber"] = "US-008",
                        ["industryName"] = "Retail customers",
                        ["address"] = new JObject
                        {
                            ["country"] = "USA",
                            ["street"] = "199 Purple Road",
                            ["city"] = "Alamo"
                        }
                    },
                    new JObject
                    {
                        ["id"] = "[ussi]US009",
                        ["organizationCode"] = "ussi",
                        ["name"] = "Sherlock Holmes",
                        ["accountNumber"] = "US009",
                        ["industryName"] = "Private detective",
                        ["address"] = new JObject
                        {
                            ["country"] = "UK",
                            ["street"] = "221B Baker Street",
                            ["city"] = "London"
                        }
                    }
                }
            }.ToString());

            return response;
        }

        if (this.Context.OperationId == "Contacts_create")
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = CreateJsonContent(new JObject
            {
                ["id"] = "[usmf]US-010",
                ["email"] = "New@OnMicrosoft.com",
                ["firstName"] = "New",
                ["lastName"] = "New",
                ["professionalTitle"] = "professionalTitle",
                ["customerId"] = "[usmf]000006",
                ["primaryPhoneNumber"] = "000006"
            }.ToString());

            return response;
        }

        if (this.Context.OperationId == "Contacts_update")
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = CreateJsonContent(new JObject
            {
                ["email"] = "New@OnMicrosoft.com",
                ["firstName"] = "New",
                ["lastName"] = "New",
                ["professionalTitle"] = "professionalTitle",
                ["customerId"] = "[usmf]000006",
                ["primaryPhoneNumber"] = "000006"
            }.ToString());

            return response;
        }

        if (this.Context.OperationId == "Customers_invoices")
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = CreateJsonContent(new JObject
            {
                ["data"] = new JArray
                {
                    new JObject
                    {
                        ["id"] = "[usmf]inv001",
                        ["voucher"] = "180000087",
                        ["transactionDate"] = "2017-06-05T12:00:00Z",
                        ["transactionAmount"] = 247.06,
                        ["invoiceNumber"] = "000088",
                        ["dueDate"] = "2017-07-20T12:00:00Z",
                        ["currency"] = "USD",
                        ["transactionType"] = "Project",
                        ["hasDocument"] = true,
                        ["statusId"] = "Disputed",
                        ["statusName"] = "Disputed",
                        ["promiseToPayDate"] = "2024-05-30T12:00:00Z",
                        ["statusReason"] = "test",
                        ["statusComment"] = "test"
                    },
                    new JObject
                    {
                        ["id"] = "[usmf]inv002",
                        ["voucher"] = "180000999",
                        ["transactionDate"] = "2019-06-06T12:00:00Z",
                        ["transactionAmount"] = 1007.06,
                        ["invoiceNumber"] = "000077",
                        ["dueDate"] = "2019-07-25T12:00:00Z",
                        ["currency"] = "USD",
                        ["transactionType"] = "Project",
                        ["hasDocument"] = true,
                        ["statusId"] = "Test",
                        ["statusName"] = "Test",
                        ["promiseToPayDate"] = "2024-05-30T12:00:00Z",
                        ["statusReason"] = "test2",
                        ["statusComment"] = "test2"
                    }
                }
            }.ToString());

            return response;
        }

        if (this.Context.OperationId == "Invoices_updateStatus")
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["statusId"] = "Disputed",
                ["statusName"] = "Disputed",
                ["promiseToPayDate"] = "2024-05-30T12:00:00Z",
                ["statusReason"] = "test2",
                ["statusComment"] = "test2"
            }.ToString());

            return response;
        }

        if (this.Context.OperationId == "Customers_agedBalances")
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["data"] = new JArray
                {
                    new JObject
                    {
                        ["id"] = "[usmf]5637151670",
                        ["name"] = "30-60-90",
                        ["isDefault"] = false,
                        ["agingDate"] = "2023-07-20T12:00:00Z",
                        ["currency"] = "USD",
                        ["totalAmountDue"] = 600,
                        ["totalOpenInvoices"] = 6,
                        ["agingPeriods"] = new JArray
                        {
                            new JObject
                            {
                                ["label"] = "30",
                                ["amount"] = 100
                            },
                            new JObject
                            {
                                ["label"] = "60",
                                ["amount"] = 200
                            },
                            new JObject
                            {
                                ["label"] = "90",
                                ["amount"] = 300
                            }
                        }
                    },
                    new JObject
                    {
                        ["id"] = "[usmf]123456",
                        ["name"] = "two weeks",
                        ["isDefault"] = true,
                        ["agingDate"] = "2022-06-20T12:00:00Z",
                        ["currency"] = "USD",
                        ["totalAmountDue"] = 600,
                        ["totalOpenInvoices"] = 3,
                        ["agingPeriods"] = new JArray
                        {
                            new JObject
                            {
                                ["label"] = "2 weeks",
                                ["amount"] = 100
                            },
                            new JObject
                            {
                                ["label"] = "one week",
                                ["amount"] = 200
                            },
                            new JObject
                            {
                                ["label"] = "now",
                                ["amount"] = 300
                            }
                        }
                    }
                }
            }.ToString());

            return response;
        }

        if (this.Context.OperationId == "Customers_activities")
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["data"] = new JArray
                {
                    new JObject
                    {
                        ["id"] = "[usmf]act001",
                        ["title"] = "Changed status",
                        ["type"] = "Type 1",
                        ["text"] = "status has been changed",
                        ["timeStamp"] = "2024-05-30T12:00:00Z"
                    },
                    new JObject
                    {
                        ["id"] = "[usmf]act002",
                        ["title"] = "Foolow up email",
                        ["type"] = "Email",
                        ["text"] = "Folow up email has been sent",
                        ["timeStamp"] = "2024-05-30T12:00:00Z"
                    }
                }
            }.ToString());

            return response;
        }

        if (this.Context.OperationId == "Invoices_listOfStatuses")
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(new JObject
            {
                ["data"] = new JArray
                {
                    new JObject
                    {
                        ["id"] = "Disputed",
                        ["name"] = "Disputed"
                    },
                    new JObject
                    {
                        ["id"] = "Test",
                        ["name"] = "Test"
                    },
                    new JObject
                    {
                        ["id"] = "Resolved",
                        ["name"] = "Resolved"
                    },
                    new JObject
                    {
                        ["id"] = "Closed",
                        ["name"] = "Closed"
                    }
                }
            }.ToString());

            return response;
        }

        if (this.Context.OperationId == "Version_supportedVersion")
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent($"2024.06.20");
            return response;
        }

        if (this.Context.OperationId == "Customers_accountStatement")
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);

            // Convert the PDF content to a byte array
            byte[] pdfBytes = Encoding.UTF8.GetBytes(pdfContent);

            // Set the response content
            response.Content = new ByteArrayContent(pdfBytes);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "document.pdf"
            };

            return response;
        }

        if (this.Context.OperationId == "Customers_invoiceDocument")
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);

            // Convert the PDF content to a byte array
            byte[] pdfBytes = Encoding.UTF8.GetBytes(pdfContent);

            // Set the response content
            response.Content = new ByteArrayContent(pdfBytes);

            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (contentAsString == "{{}}" || contentAsString == "{}")
            {
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "document.zip"
                };
            }
            else
            {
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "document.pdf"
                };
            }

            return response;
        }

        // Handle an invalid operation ID
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent($"Unknown operation ID '{this.Context.OperationId}'");
        return response;
    }
}
