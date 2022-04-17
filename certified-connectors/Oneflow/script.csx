using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oneflow.PowerApps.CustomCode.SupportingClasses;

namespace Oneflow.PowerApps.CustomCode
{
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
                [Operations.PartyCreate] = HandlePartyCreate
            };

            _schemaMappings = new Dictionary<string, string>(){
                ["Company"] = Constants.CompanyPartyDynamicSchema,
                ["Individual"] = Constants.IndividualPartyDynamicSchema
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

        private async Task<HttpResponseMessage> HandleGetDynamicSchema(IScriptContext ctx){
            var req = ctx.Request;
            var schemaId = req.Headers.First(x => 
                string.Equals(x.Key,"schema_id", StringComparison.OrdinalIgnoreCase))
                .Value?.First();
            if (string.IsNullOrEmpty(schemaId)){
                var defaultResponse = new HttpResponseMessage(HttpStatusCode.OK);
                defaultResponse.Content = CreateJsonContent("{}");
                return defaultResponse;
            }
            if (!_schemaMappings.ContainsKey(schemaId)){
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

              // for individual parties, just straight up create an individual participant.
              if (inputParty.type == Constants.PartyTypes.Individual)
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
              
              if (!getPartiesResponse.IsSuccessStatusCode){
                  return getPartiesResponse;
              }
              debugInfo.AppendLine("converting get party response.");
              var getPartiesResponseJson = await getPartiesResponse.Content.ReadAsStringAsync();
              
              var existingParties = JsonConvert.DeserializeObject<Responses.GetPartiesResponse>(getPartiesResponseJson);

              int matchingPartyId = 0;
              foreach (Models.Party existingParty in existingParties.data){
                if (existingParty.Equals(inputParty)) {
                  matchingPartyId = existingParty.id.Value;
                  break;
                }
              }

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
            if (participant == null){
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
        
        private void CopyHeaders (HttpRequestMessage copyFrom, HttpRequestMessage copyTo)
        {
          if (copyFrom == null || copyTo == null){
            throw new ScriptException(HttpStatusCode.BadRequest, "CopyHeaders - one of the input parameters is null.");
          }

            foreach (var header in copyFrom.Headers)
            {
                copyTo.Headers.Add(header.Key, header.Value);
            }
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

        }

        public static class Constants
        {
          public static class Requests
          {
            public const string PartyEndpoint = "https://api.oneflow.com/v1/contracts/{0}/parties";
            public const string ParticipantEndpoint = "https://api.oneflow.com/v1/contracts/{0}/parties/{1}/participants";
          }
          public static class PartyTypes
          {
            public const string Individual = "individual";
            public const string Company = "company";
          }
            public const string GetTemplatesFilterHeader = "x-oneflow-workspace-id";

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
                           country_code == party.country_code &&
                           identification_number == party.identification_number &&
                           name == party.name &&                           
                           type == party.type;
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

                public void AlignParticipants(){
                  if (String.Equals("individual", type, StringComparison.InvariantCultureIgnoreCase)) return;
                  if (String.Equals("company", type, StringComparison.InvariantCultureIgnoreCase)){
                    this.participants = new List<Participant>(){this.participant};
                    this.participant = null;
                  }
                }
            }

          public class Participant
          {
                //workaround for Power Automate bug with a nested object in array element.
                [JsonProperty("_permissions/contract:update", NullValueHandling = NullValueHandling.Ignore)]
                bool? updatePermission {get; set;}

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
                           delivery_channel == participant.delivery_channel &&
                           email == participant.email &&
                           identification_number == participant.identification_number &&
                           name == participant.name &&
                           phone_number == participant.phone_number &&
                           sign_method == participant.sign_method &&
                           title == participant.title &&
                           two_step_authentication_method == participant.two_step_authentication_method &&
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

          public class ParticipantPermissions {
                [JsonProperty("contract:update")]
                public bool ContractUpdate { get; set; }

                public static implicit operator ParticipantPermissions (bool updatePermission)
                {
                  return new ParticipantPermissions(){
                    ContractUpdate = updatePermission
                  };
                }
          }
        }

        public class Responses
        {
          public class GetPartiesResponse
          {
            public IEnumerable<Models.Party> data {get; set;}
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
        #endregion
    }
}
