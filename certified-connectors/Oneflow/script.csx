    public class Script : ScriptBase
    {
        #region fields & consctructors
        private readonly Dictionary<string, Func<IScriptContext, Task<HttpResponseMessage>>> _operationMappings;
        private readonly Dictionary<string, string> _schemaMappings;
        public Script()
        {
            _operationMappings = new Dictionary<string, Func<IScriptContext, Task<HttpResponseMessage>>>()
            {
                [Operations.GetTemplates] = HandleGetTemplates,
                [Operations.DownloadFile] = HandleFileDownload,
                [Operations.GetDynamicSchema] = HandleGetDynamicSchema,
                [Operations.PartyCreate] = HandlePartyCreate,
                [Operations.GetTemplateTypeByTemplateId] = HandleGetTemplateTypeByTemplateId,
                [Operations.ExecuteAs] = HandleGetExecuteAs
            };

            _schemaMappings = new Dictionary<string, string>()
            {
                ["Company"] = Constants.CompanyPartyDynamicSchema,
                ["Individual"] = Constants.IndividualPartyDynamicSchema,
                ["Ownerside"] = Constants.IndividualPartyDynamicSchema
            };
        }
        #endregion

        public override async Task<HttpResponseMessage> ExecuteAsync()
        {
            try
            {
                return await HandleOperation(this.Context)
                    .ConfigureAwait(false);
            }
            catch (ScriptException ex)
            {
                var response = new HttpResponseMessage(ex.StatusCode);
                string jsonContent = JsonConvert.SerializeObject(ex);
                response.Content = CreateJsonContent(jsonContent);
                return response;
            }
        }

        #region operation handling methods
        private async Task<HttpResponseMessage> HandleOperation(IScriptContext ctx)
        {
            if (ContainsHeader(ctx.Request, Constants.UserEmailOverrideHeader))
            {
                ReplaceUserEmailWithOverriddenValue(ctx.Request);
            }

            if (_operationMappings.ContainsKey(ctx.OperationId))
            {
                return await _operationMappings[ctx.OperationId].Invoke(ctx)
                    .ConfigureAwait(false);
            }
            else
                return await ctx.SendAsync(ctx.Request, CancellationToken).ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> HandleGetTemplates(IScriptContext ctx)
        {
            var req = ctx.Request;

            var response = await ctx.SendAsync(req, CancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return response;
            }

            try
            {
                IEnumerable<string> headervalues = new List<string>();
                req.Headers.TryGetValues(Constants.GetTemplatesFilterHeader, out headervalues);
                int workspaceId = Convert.ToInt32(headervalues?.First());
                string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                var result = JArray.Parse(responseString);

                response.Content = CreateJsonContent(FilterTemplatesByWorkspace(workspaceId, result));
            }
            catch (Exception e)
            {
                throw new ScriptException(HttpStatusCode.InternalServerError, e.Message, e);
            }

            return response;
        }

        private async Task<HttpResponseMessage> HandleGetDynamicSchema(IScriptContext ctx)
        {
            var req = ctx.Request;
            var schemaId = GetHeaderStringValue(Constants.SchemaIdHeader, req);
            if (string.IsNullOrEmpty(schemaId))
            {
                var defaultResponse = new HttpResponseMessage(HttpStatusCode.OK);
                defaultResponse.Content = CreateJsonContent("{}");
                return defaultResponse;
            }
            if (!_schemaMappings.ContainsKey(schemaId))
            {
                throw new ScriptException(HttpStatusCode.BadRequest, "Schema id was not found.");
            }
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(_schemaMappings[schemaId]);
            return response;
        }

        private async Task<HttpResponseMessage> HandleFileDownload(IScriptContext ctx)
        {
            var req = ctx.Request;

            var response = await ctx.SendAsync(req, CancellationToken)
                .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                var location = response.Headers.Location;
                var redirectRequest = new HttpRequestMessage(HttpMethod.Get, location);
                CopyHeaders(req, redirectRequest);

                return await ctx.SendAsync(redirectRequest, CancellationToken)
                    .ConfigureAwait(false);
            }

            return response;
        }

        private async Task<HttpResponseMessage> HandlePartyCreate(IScriptContext ctx)
        {
            StringBuilder debugInfo = new StringBuilder();
            try
            {
                var req = ctx.Request;
                var contractId = req.Headers.FirstOrDefault(x => x.Key == "contract_id").Value?.First();
                debugInfo.AppendLine($"contractId - {contractId}");

                var inputPartyJson = await req.Content.ReadAsStringAsync();
                debugInfo.AppendLine($"inputJson - {inputPartyJson}");
                var inputParty = JsonConvert.DeserializeObject<Models.Party>(inputPartyJson);
                var selectedType = GetHeaderStringValue(Constants.ParticipantTypeHeader, req);
                debugInfo.AppendLine($"selectedType - {selectedType}");
                // for individual parties, just straight up create an individual participant.
                if (selectedType.Equals(Constants.PartyTypes.Individual, StringComparison.OrdinalIgnoreCase))
                {
                    debugInfo.AppendLine("identified as individual. creating individual party.");
                    return await CreateParty(req, contractId, inputParty, ctx);
                }

                // call getParties
                debugInfo.AppendLine("trying to get existing parties.");
                var getPartiesRequest = new HttpRequestMessage(HttpMethod.Get, new Uri(string.Format(Constants.Requests.PartyEndpoint, contractId)));
                CopyHeaders(req, getPartiesRequest);

                var getPartiesResponse = await ctx.SendAsync(getPartiesRequest, CancellationToken)
                    .ConfigureAwait(false);

                if (!getPartiesResponse.IsSuccessStatusCode)
                {
                    return getPartiesResponse;
                }
                debugInfo.AppendLine("converting get party response.");
                var getPartiesResponseJson = await getPartiesResponse.Content.ReadAsStringAsync();
                var existingParties = JsonConvert.DeserializeObject<Responses.GetPartiesResponse>(getPartiesResponseJson).data;
                int matchingPartyId = 0;

                // if we need to create an ownserside participant, just find my_party and create a participant for it.
                if (selectedType.Equals(Constants.PartyTypes.Ownerside, StringComparison.OrdinalIgnoreCase))
                {
                    matchingPartyId = existingParties.FirstOrDefault(x =>
                    x.my_party.Value).id ??
                    throw new ScriptException(HttpStatusCode.InternalServerError,
                                              $"Couldn't find ownerside party for contract {contractId}");

                    return await CreateParticipant(req, matchingPartyId, contractId, inputParty.participant, ctx);
                }

                // process Company participants.                
                debugInfo.AppendLine($"Input party - {Environment.NewLine + inputParty}");
                foreach (Models.Party existingParty in existingParties)
                {
                    debugInfo.AppendLine($"Trying to match with party - {Environment.NewLine + existingParty}");
                    if (existingParty.Equals(inputParty))
                    {
                        debugInfo.AppendLine("match");
                        matchingPartyId = existingParty.id.Value;
                        break;
                    }
                    debugInfo.AppendLine("not match");
                }

                debugInfo.Append($"matching party id: {Environment.NewLine + matchingPartyId}");
                if (matchingPartyId != default)
                {
                    debugInfo.AppendLine("Party found. creating participant.");
                    return await CreateParticipant(req, matchingPartyId, contractId, inputParty.participant, ctx);
                }

                debugInfo.AppendLine("Party not found. creating new party.");
                return await CreateParty(req, contractId, inputParty, ctx);
            }
            catch (Exception e)
            {
                throw new ScriptException(HttpStatusCode.BadRequest, e.Message + $"Trace information: {debugInfo}");
            }
        }

        private async Task<HttpResponseMessage> HandleGetTemplateTypeByTemplateId(IScriptContext ctx)
        {
            var req = ctx.Request;
            var templateId = GetHeaderStringValue(Constants.TemplateIdHeader, req);
            if (String.IsNullOrEmpty(templateId)) throw new ScriptException(HttpStatusCode.BadRequest, "template id is a required property.");

            string requestUrl = string.Format(Constants.Requests.TemplatesEndpoint, templateId);
            var getTemplateRequest = new HttpRequestMessage(HttpMethod.Get, new Uri(requestUrl));
            CopyHeaders(req, getTemplateRequest);

            var getTemplateResponse = await ctx.SendAsync(getTemplateRequest, CancellationToken);
            if (!getTemplateResponse.IsSuccessStatusCode) return getTemplateResponse;

            string getTemplateResponseContent = await getTemplateResponse.Content.ReadAsStringAsync();
            var template = JsonConvert.DeserializeObject<Models.Template>(getTemplateResponseContent);
            if (template != null && template.template_type == null)
                throw new ScriptException(HttpStatusCode.BadRequest,
                    "Selected template is not a part of any template group and doesn't have data fields.");

            var templateTypeId = template.template_type.id;
            req.RequestUri = new Uri(req.RequestUri.AbsoluteUri.ToString() + templateTypeId);
            return await ctx.SendAsync(req, CancellationToken);
        }

        private async Task<HttpResponseMessage> HandleGetExecuteAs(IScriptContext ctx)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(Constants.ExecuteAsResponse);
            return response;
        }

        private async Task<HttpResponseMessage> CreateParty(HttpRequestMessage initialRequest, string contractId, Models.Party party, IScriptContext ctx)
        {
            StringBuilder debugInfo = new StringBuilder();
            try
            {
                party.AlignParticipants();
                string requestUrl = string.Format(Constants.Requests.PartyEndpoint, contractId);
                debugInfo.AppendLine($"requestUri - {requestUrl}");
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, new Uri(requestUrl));
                CopyHeaders(initialRequest, req);
                var jsonBody = JsonConvert.SerializeObject(party);
                debugInfo.AppendLine($"jsonBody - {jsonBody}");
                req.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                return await ctx.SendAsync(req, CancellationToken);
            }
            catch (Exception e)
            {
                throw new ScriptException(HttpStatusCode.BadRequest, e.Message + $"Trace information: {debugInfo}");
            }
        }

        private async Task<HttpResponseMessage> CreateParticipant(HttpRequestMessage initialRequest, int existingPartyId, string contractId, Models.Participant participant, IScriptContext ctx)
        {
            StringBuilder debugInfo = new StringBuilder();
            try
            {
                string requestUrl = String.Format(Constants.Requests.ParticipantEndpoint, contractId, existingPartyId);
                debugInfo.AppendLine($"requestUri - {requestUrl}");
                if (participant == null)
                {
                    throw new ScriptException(HttpStatusCode.BadRequest, "Participant was not provided in the request.");
                }
                string participantJson = JsonConvert.SerializeObject(participant);
                debugInfo.AppendLine($"participantJson - {participantJson}");
                var req = new HttpRequestMessage(HttpMethod.Post, new Uri(requestUrl));
                CopyHeaders(initialRequest, req);
                req.Content = new StringContent(participantJson, Encoding.UTF8, "application/json");
                return await ctx.SendAsync(req, CancellationToken);
            }
            catch (Exception e)
            {
                throw new ScriptException(HttpStatusCode.BadRequest, e.Message + $"Trace information: {debugInfo}");
            }
        }

        private string FilterTemplatesByWorkspace(int workspaceId, JArray data)
        {
            var templates = data.FirstOrDefault(x => x["id"]?.Value<int>() == workspaceId);
            if (templates != null)
            {
                return templates["templates"]?.ToString();
            }
            return new JArray().ToString();
        }

        private void CopyHeaders(HttpRequestMessage copyFrom, HttpRequestMessage copyTo)
        {
            if (copyFrom == null || copyTo == null)
            {
                throw new ScriptException(HttpStatusCode.BadRequest, "CopyHeaders - one of the input parameters is null.");
            }

            foreach (var header in copyFrom.Headers)
            {
                copyTo.Headers.Add(header.Key, header.Value);
            }
        }

        private string GetHeaderStringValue(string headerName, HttpRequestMessage req)
        {
            if (req == null) throw new ScriptException(HttpStatusCode.InternalServerError,
                                                       "GetHeaderStringValue - req is null.");

            return req.Headers.First(x =>
                string.Equals(x.Key, headerName, StringComparison.OrdinalIgnoreCase))
                .Value?.First();
        }
        
        private bool ContainsHeader(HttpRequestMessage req, string headerName)
        {
            if (req == null) return false;
            return req.Headers?.Any(x => string.Equals(x.Key, headerName, StringComparison.OrdinalIgnoreCase)) == true;
        }

        private void ReplaceUserEmailWithOverriddenValue(HttpRequestMessage req)
        {
            string overrideWith = req.Headers.FirstOrDefault(x => string.Equals(x.Key, Constants.UserEmailOverrideHeader))
                .Value?.First();
            if (String.IsNullOrEmpty(overrideWith) || overrideWith.Equals(Constants.ExecuteAsOptions.CurrentUser, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            string headerValue = string.Empty;
            bool isValidEmail = IsStringAValidEmail(overrideWith);

            if (overrideWith.Equals(Constants.ExecuteAsOptions.Creator, StringComparison.OrdinalIgnoreCase))
            {
                headerValue = Constants.CreatorHeaderValue;
            }            
            else if (isValidEmail)
            {
                headerValue = overrideWith;
            }
            else
            {
                throw new ScriptException(HttpStatusCode.BadRequest, $"{overrideWith} is not recognized as a valid email address.");
            }
            req.Headers.Remove(Constants.UserEmailOverrideHeader);
            req.Headers.Remove(Constants.UserEmailHeader);
            req.Headers.Add(Constants.UserEmailHeader, headerValue);
        }

        private bool IsStringAValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            return Constants.EmailRegex.Match(email).Length > 0;
        }
        #endregion

        #region additional classes
        public static class Operations
        {
            public const string GetTemplates = "GetTemplatesForWorkspace";
            public const string AddAttachment = "UploadAttachments";
            public const string DownloadFile = "GetAContractFileById";
            public const string GetDynamicSchema = "GetDynamicSchema";
            public const string PartyCreate = "PartyCreate";
            public const string GetTemplateTypeByTemplateId = "GetTemplateByTemplateId";            
            public const string ExecuteAs = "GetExecuteAs";
        }

        public static class Constants
        {
            public static class Requests
            {
                public const string PartyEndpoint = "https://api.oneflow.com/v1/contracts/{0}/parties";
                public const string ParticipantEndpoint = "https://api.oneflow.com/v1/contracts/{0}/parties/{1}/participants";
                public const string TemplatesEndpoint = "https://api.oneflow.com/v1/templates/{0}";
            }
            public static class PartyTypes
            {
                public const string Individual = "individual";
                public const string Company = "company";
                public const string Ownerside = "ownerside";
            }
            public const string GetTemplatesFilterHeader = "x-oneflow-workspace-id";
            public const string TemplateIdHeader = "x-oneflow-template-id";
            public const string SchemaIdHeader = "schema_id";
            public const string ParticipantTypeHeader = "participant_type";
            public const string UserEmailOverrideHeader = "x-oneflow-user-email-override";
            public const string UserEmailHeader = "x-oneflow-user-email";
            public const string CreatorHeaderValue = "__CREATOR__";
            public static readonly Regex EmailRegex = new Regex("^((([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+(\\.([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+)*)|((\\x22)((((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(([\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x7f]|\\x21|[\\x23-\\x5b]|[\\x5d-\\x7e]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(\\\\([\\x01-\\x09\\x0b\\x0c\\x0d-\\x7f]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF]))))*(((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(\\x22)))@((([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.)+(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.?$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
           
            public static class ExecuteAsOptions
            {
                public const string Creator = "Contract creator";
                public const string CurrentUser = "Connection user";
            }
            #region dynamic schemas
            public const string IndividualPartyDynamicSchema = @"{
                                                                      ""type"": ""object"",
                                                                      ""properties"": {
                                                                        ""country_code"": {
                                                                          ""type"": ""string"",
                                                                          ""x-ms-summary"": ""Party Country Code"",
                                                                          ""x-ms-visibility"": ""advanced"",
                                                                          ""description"": ""ISO Code of the country, e.g., SE. required.""
                                                                        },                                                                       
                                                                        ""participant"": {
                                                                          ""type"": ""object"",
                                                                          ""properties"": {
                                                                            ""delivery_channel"": {
                                                                              ""type"": ""string"",
                                                                              ""required"": [
                                                                               ""true""
                                                                              ],
                                                                              ""description"": ""Method of delivering the contract to participants. (Email/SMS/Same device/None/Custom value)"",
                                                                              ""x-ms-summary"": ""Delivery Channel"",                                                                             
                                                                              ""enum"": [
                                                                                ""email"",
                                                                                ""sms"",
                                                                                ""same_device"",
                                                                                ""none""
                                                                              ]
                                                                            },
                                                                            ""email"": {
                                                                              ""type"": ""string"",
                                                                              ""description"": ""Required if the Delivery channel or Sign method is set to Email."",
                                                                              ""x-ms-summary"": ""Participant Email""
                                                                            },
                                                                            ""identification_number"": {
                                                                              ""type"": ""string"",
                                                                              ""x-ms-summary"": ""Participant Identification Number"",
                                                                              ""x-ms-visibility"": ""advanced"",
                                                                              ""description"": ""e.g., Personal number or SSN""
                                                                            },
                                                                            ""name"": {
                                                                              ""type"": ""string"",
                                                                              ""x-ms-visibility"":""important"",
                                                                              ""x-ms-summary"": ""Participant Name""
                                                                            },
                                                                            ""phone_number"": {
                                                                              ""type"": ""string"",
                                                                              ""x-ms-summary"": ""Participant Phone Number"", 
                                                                              ""description"": ""Required if the Delivery channel or Sign method is set to SMS.""
                                                                            },
                                                                            ""sign_method"": {
                                                                              ""type"": ""string"",
                                                                              ""x-ms-summary"": ""Participant Sign Method"",
                                                                              ""x-ms-visibility"":""advanced"",
                                                                              ""description"": ""Required if the participant is a signatory."",
                                                                              ""enum"": [
                                                                                ""standard_esign"",
                                                                                ""sms"",
                                                                                ""swedish_bankid"",
                                                                                ""norwegian_bankid"",
                                                                                ""danish_nemid"",
                                                                                ""finnish_bankid""
                                                                              ]
                                                                            },
                                                                            ""signatory"": {
                                                                              ""type"": ""boolean"",
                                                                              ""x-ms-visibility"":""advanced"",
                                                                              ""x-ms-summary"": ""Signatory?"",
                                                                              ""description"": ""Specify if the participant can sign the contract. If Yes, the Sign method and Permissions fields are required""
                                                                            },
                                                                            ""title"": {
                                                                              ""type"": ""string"",
                                                                              ""x-ms-summary"": ""Participant Title"",
                                                                              ""x-ms-visibility"":""advanced""
                                                                            },
                                                                            ""two_step_authentication_method"":{
                                                                              ""type"": ""string"",
                                                                              ""x-ms-visibility"": ""advanced"",
                                                                              ""x-ms-summary"": ""Participant 2FA Method"",
                                                                              ""required"": [
                                                                               ""true""
                                                                              ],
                                                                              ""description"": ""Email/SMS/None"",
                                                                              ""enum"": [
                                                                                ""email"",
                                                                                ""sms"",
                                                                                ""none""
                                                                              ]
                                                                            },
                                                                            ""_permissions"": {
                                                                              ""type"": ""object"",
                                                                              ""x-ms-summary"": ""Permissions:"",
                                                                              ""x-ms-visibility"":""advanced"",
                                                                              ""description"": ""Defines permissions for the contract participant."",
                                                                              ""properties"": {
                                                                                ""contract:update"": {
                                                                                  ""type"": ""boolean"",
                                                                                  ""x-ms-summary"": ""Can Update Contract?"",
                                                                                  ""description"": ""Specify if the participant can update the contract.""
                                                                                }
                                                                              }
                                                                            }
                                                                          }
                                                                        },
                                                                        ""type"": {
                                                                          ""type"": ""string"",
                                                                          ""default"": ""individual"",
                                                                          ""x-ms-visibility"": ""internal"",
                                                                          ""required"": [
                                                                            ""true""
                                                                          ],
                                                                          ""title"": """"
                                                                        }
                                                                      },
                                                                      ""required"": [
                                                                        ""type"",
                                                                        ""name"",
                                                                        ""delivery_channel"",
                                                                        ""two_step_authentication_method""
                                                                      ]
                                                                    }";
            public const string CompanyPartyDynamicSchema = @"{
                                                                  ""type"": ""object"",
                                                                  ""properties"": {
                                                                    ""country_code"": {
                                                                      ""type"": ""string"",
                                                                      ""x-ms-summary"": ""Party Country Code"",
                                                                      ""x-ms-visibility"": ""advanced"",
                                                                      ""description"": ""ISO Code of the country, e.g., SE. required""
                                                                    },
                                                                    ""identification_number"": {
                                                                      ""type"": ""string"",
                                                                      ""x-ms-summary"": ""Party Identification Number"",
                                                                      ""x-ms-visibility"": ""advanced"",
                                                                      ""description"": ""e.g., Personal number or SSN""
                                                                    },
                                                                    ""name"": {
                                                                      ""type"": ""string"",
                                                                      ""required"": [
                                                                        ""true""
                                                                      ],
                                                                      ""x-ms-summary"": ""Party Name"",
                                                                      ""x-ms-visibility"": ""important"",
                                                                      ""description"": ""The name of the party.""
                                                                    },
                                                                    ""participant"": {
                                                                          ""type"": ""object"",
                                                                          ""properties"": {
                                                                            ""delivery_channel"": {
                                                                              ""type"": ""string"",
                                                                              ""required"": [
                                                                                ""true""
                                                                              ],
                                                                              ""description"": ""Method of delivering the contract to participants. (Email/SMS/Same device/None/Custom value)"",
                                                                              ""x-ms-summary"": ""Delivery Channel"",
                                                                              ""enum"": [
                                                                                ""email"",
                                                                                ""sms"",
                                                                                ""same_device"",
                                                                                ""none""
                                                                              ]
                                                                            },
                                                                            ""email"": {
                                                                              ""type"": ""string"",
                                                                              ""description"": ""Required if the Delivery channel or Sign method is set to Email."",
                                                                              ""x-ms-summary"": ""Participant Email""
                                                                            },
                                                                            ""identification_number"": {
                                                                              ""type"": ""string"",
                                                                              ""x-ms-visibility"": ""advanced"",
                                                                              ""x-ms-summary"": ""Participant Identification Number"",
                                                                              ""description"": ""The date of birth, SSN, personal number, etc., of the participant.""
                                                                            },
                                                                            ""name"": {
                                                                              ""type"": ""string"",
                                                                              ""required"": [
                                                                                ""true""
                                                                              ],
                                                                              ""x-ms-summary"": ""Participant Name"",
                                                                              ""x-ms-visibility"": ""important""
                                                                            },
                                                                            ""phone_number"": {
                                                                              ""type"": ""string"",
                                                                              ""x-ms-summary"": ""Participant Phone Number"", 
                                                                              ""description"": ""Required if the Delivery channel or Sign method is set to SMS.""
                                                                            },
                                                                            ""sign_method"": {
                                                                              ""type"": ""string"",
                                                                              ""x-ms-summary"": ""Participant Sign Method"",
                                                                              ""x-ms-visibility"": ""advanced"",
                                                                              ""description"": ""Required if the participant is a signatory."",
                                                                              ""enum"": [
                                                                                ""standard_esign"",
                                                                                ""sms"",
                                                                                ""swedish_bankid"",
                                                                                ""norwegian_bankid"",
                                                                                ""danish_nemid"",
                                                                                ""finnish_bankid""
                                                                              ]
                                                                            },
                                                                            ""signatory"": {
                                                                              ""type"": ""boolean"",
                                                                              ""x-ms-summary"": ""Signatory?"",
                                                                              ""x-ms-visibility"": ""advanced"",
                                                                              ""description"": ""Specify if the participant can sign the contract. If Yes, the Sign method and Permissions fields are required""
                                                                            },
                                                                            ""title"": {
                                                                              ""type"": ""string"",
                                                                              ""x-ms-summary"": ""Participant Title"",
                                                                              ""x-ms-visibility"": ""advanced""
                                                                            },
                                                                            ""two_step_authentication_method"":{
                                                                              ""type"": ""string"",
                                                                              ""x-ms-summary"": ""Participant 2FA Method"",
                                                                              ""x-ms-visibility"": ""advanced"",
                                                                              ""description"": ""Email/SMS/None"",
                                                                              ""enum"": [
                                                                                ""email"",
                                                                                ""sms"",
                                                                                ""none""
                                                                              ]
                                                                            },
                                                                            ""_permissions"": {
                                                                              ""type"": ""object"",
                                                                              ""x-ms-summary"": ""Permissions:"",
                                                                              ""x-ms-visibility"": ""advanced"",
                                                                              ""description"": ""Defines permissions for the contract participant. Required if participant is Signatory."",
                                                                              ""properties"": {
                                                                                ""contract:update"": {
                                                                                  ""type"": ""boolean"",
                                                                                  ""x-ms-summary"": ""Can Update Contract?"",
                                                                                  ""description"": ""Specify if the participant can update the contract.""
                                                                                }
                                                                              }
                                                                            }
                                                                          }
                                                                        },                                                                    
                                                                    ""type"": {
                                                                      ""type"": ""string"",
                                                                      ""default"": ""company"",
                                                                      ""x-ms-visibility"": ""internal"",
                                                                      ""required"": [
                                                                        ""true""
                                                                      ],
                                                                      ""title"": """"
                                                                    }
                                                                  },
                                                                  ""required"": [
                                                                    ""type""
                                                                  ]
                                                                }";
            public static readonly string ExecuteAsResponse = @"{
                                                                 ""data"":
                                                                    [
                                                                        {""key"": ""Contract creator"", ""value"" : ""Contract creator""}, 
                                                                        {""key"":""Connection user"", ""value"":""Connection user""}
                                                                    ]
                                                                }";
            #endregion
        }
        public class Models
        {
            public class Party
            {
                /// <summary>
                ///  The country of the company.
                /// </summary>       
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string country_code { get; set; }

                /// <summary>
                /// Whether the party belongs to the current user or not.
                /// </summary>
                /// 
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public bool? my_party { get; set; }

                /// <summary>
                /// The ID of the company.
                /// </summary>         
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int? id { get; set; }

                /// <summary>
                /// The date of birth, SSN, personal ID or similar of a company.
                /// </summary> 
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string identification_number { get; set; }

                /// <summary>
                /// The name of the company.
                /// </summary>     
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string name { get; set; }

                /// <summary>
                /// Participants of the company
                /// </summary>             
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public List<Participant> participants { get; set; }

                /// <summary>
                /// Individual participant, only used when type is individual
                /// </summary>                
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public Participant participant { get; set; }

                /// <summary>
                /// Party type - company or individual
                /// </summary>                
                public string type { get; set; }

                public override bool Equals(object obj)
                {
                    return obj is Party party &&
                            NullStringComparer.NullEqualsEmptyComparer.Equals(identification_number, party.identification_number) &&
                            NullStringComparer.NullEqualsEmptyComparer.Equals(name, party.name) &&
                            NullStringComparer.NullEqualsEmptyComparer.Equals(type, party.type) &&
                            // if country code is empty in the input, it defaults to a predefined country code in the application, 
                            // so just ignore empty country codes during comparison if it's not set.
                            (!String.IsNullOrEmpty(country_code) ? true :
                              NullStringComparer.NullEqualsEmptyComparer.Equals(country_code, party.country_code));

                }

                public override int GetHashCode()
                {
                    int hashCode = 1786558932;
                    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(country_code);
                    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(identification_number);
                    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
                    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(type);
                    return hashCode;
                }

                public override string ToString()
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"country_code - {this.country_code}");
                    sb.AppendLine($"identification_number - {this.identification_number}");
                    sb.AppendLine($"name - {this.name}");
                    sb.AppendLine($"type - {this.type}");
                    sb.AppendLine($"my_party - {this.my_party}");
                    sb.AppendLine($"id - {this.id}");

                    return sb.ToString();
                }
                public void AlignParticipants()
                {
                    if (Constants.PartyTypes.Individual.Equals(type, StringComparison.OrdinalIgnoreCase)) return;
                    if (Constants.PartyTypes.Company.Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        this.participants = new List<Participant>() { this.participant };
                        this.participant = null;
                    }
                }
            }

            public class Participant
            {
                //workaround for Power Automate bug with a nested object in array element.
                [JsonProperty("_permissions/contract:update", NullValueHandling = NullValueHandling.Ignore)]
                bool? updatePermission { get; set; }

                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public ParticipantPermissions _permissions { get; set; }
                /// <summary>
                /// One of <see cref="DeliveryChannelCode"/>
                /// </summary>

                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string delivery_channel { get; set; }

                /// <summary>
                /// The participant's email address.
                /// </summary>
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string email { get; set; }

                /// <summary>
                /// The ID of the participant.
                /// </summary>
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public int? id { get; set; }

                /// <summary>
                /// The date of birth, SSN, personal number, etc of the participant.
                /// </summary>
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string identification_number { get; set; }

                /// <summary>
                /// The name of the participant.
                /// </summary>
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string name { get; set; }

                /// <summary>
                /// The mobile phone number of the participant.
                /// </summary>
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string phone_number { get; set; }

                /// <summary>
                /// One of <see cref="SignMethodCode"/>.
                /// </summary>
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string sign_method { get; set; }

                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string title { get; set; }

                /// <summary>
                /// One of <see cref="TwoFAMethodCode"/>
                /// </summary>
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public string two_step_authentication_method { get; set; }

                [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                public bool signatory { get; set; }

                [System.Runtime.Serialization.OnDeserialized]
                internal void OnDeserializedMethod(System.Runtime.Serialization.StreamingContext context)
                {
                    if (_permissions == null && updatePermission.HasValue)
                    {
                        _permissions = updatePermission.Value;
                    }
                }

                [System.Runtime.Serialization.OnSerializing]
                internal void OnSerializingMethod(System.Runtime.Serialization.StreamingContext context)
                {
                    if (updatePermission.HasValue) updatePermission = null;
                }

                public override bool Equals(object obj)
                {
                    return obj is Participant participant &&
                           EqualityComparer<ParticipantPermissions>.Default.Equals(_permissions, participant._permissions) &&
                           NullStringComparer.NullEqualsEmptyComparer.Equals(delivery_channel, participant.delivery_channel) &&
                           NullStringComparer.NullEqualsEmptyComparer.Equals(email, participant.email) &&
                           NullStringComparer.NullEqualsEmptyComparer.Equals(identification_number, participant.identification_number) &&
                           NullStringComparer.NullEqualsEmptyComparer.Equals(name, participant.name) &&
                           NullStringComparer.NullEqualsEmptyComparer.Equals(phone_number, participant.phone_number) &&
                           NullStringComparer.NullEqualsEmptyComparer.Equals(sign_method, participant.sign_method) &&
                           NullStringComparer.NullEqualsEmptyComparer.Equals(title, participant.title) &&
                           NullStringComparer.NullEqualsEmptyComparer.Equals(two_step_authentication_method, participant.two_step_authentication_method) &&
                           signatory == participant.signatory;
                }

                public override int GetHashCode()
                {
                    int hashCode = -1207568244;
                    hashCode = hashCode * -1521134295 + EqualityComparer<ParticipantPermissions>.Default.GetHashCode(_permissions);
                    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(delivery_channel);
                    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(email);
                    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(identification_number);
                    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
                    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(phone_number);
                    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(sign_method);
                    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(title);
                    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(two_step_authentication_method);
                    hashCode = hashCode * -1521134295 + signatory.GetHashCode();
                    return hashCode;
                }
            }

            public class ParticipantPermissions
            {
                [JsonProperty("contract:update")]
                public bool ContractUpdate { get; set; }

                public static implicit operator ParticipantPermissions(bool updatePermission)
                {
                    return new ParticipantPermissions()
                    {
                        ContractUpdate = updatePermission
                    };
                }
            }

            public class Template
            {
                public int id { get; set; }
                public TemplateType template_type { get; set; }
            }

            public class TemplateType
            {
                public string extension_type { get; set; }
                public string id { get; set; }
                public string name { get; set; }
            }
        }

        public class Responses
        {
            public class GetPartiesResponse
            {
                public IEnumerable<Models.Party> data { get; set; }
            }
        }
        public class ScriptException : Exception
        {
            public ScriptException(HttpStatusCode statusCode, string message, Exception innerException = null)
                : base(message, innerException)
            {
                StatusCode = statusCode;
            }

            public HttpStatusCode StatusCode { get; }

            public override string ToString()
            {
                return ($"Error  while execuring custom script: {Message}. {Environment.NewLine} Stack trace: {StackTrace}.");
            }
        }

        public class NullStringComparer : EqualityComparer<string>
        {
            public static IEqualityComparer<string> NullEqualsEmptyComparer { get; } = new NullStringComparer();

            public override bool Equals(string x, string y)
            {
                // equal if string.Equals(x, y)
                // or both StringIsNullOrEmpty
                return String.Equals(x, y, StringComparison.Ordinal)
                    || (String.IsNullOrEmpty(x) && String.IsNullOrEmpty(y));
            }

            public override int GetHashCode(string obj)
            {
                if (String.IsNullOrEmpty(obj))
                    return 0;
                else
                    return obj.GetHashCode();
            }
        }

        #endregion
    }
    