public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try
        {
            if ("ValidateNotification".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
            {
                return await this.ValidateNotification().ConfigureAwait(false);
            }
            else if("WebhookNotification".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
            {
                return await this.WebhookNotification().ConfigureAwait(false);
            }
            else  if("GetEntities".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
            {              
                return await this.GetDynamicResponseJsonBody(this.GetEntities).ConfigureAwait(false);         
            }
            else if("GetSchema".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
            {
                return await this.GetDynamicResponseJsonBody(this.GetResponseSchema).ConfigureAwait(false);
            }
            else if("GetWriteSchema".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
            {
                return await this.GetDynamicRequestBody().ConfigureAwait(false);
            }
            else if("GetZellisObjects".Equals(this.Context.OperationId, StringComparison.OrdinalIgnoreCase))
            {
                return await this.GetZellisObjects().ConfigureAwait(false);
            }
            else
            {
                return await this.SubscribeTrigger().ConfigureAwait(false);
            }
        }
        catch (ConnectorException ex)
        {
            var response = new HttpResponseMessage(ex.StatusCode);
            response.Content = CreateJsonContent(ex.Message);
            return response;
        }
        
    }

    private string getRequestHeaderValue(string headerName) {
        IEnumerable<string> headerValues = this.Context.Request.Headers.GetValues(headerName);
        var headerValue = headerValues.FirstOrDefault();
        return headerValue;
    }

    private Uri manipulateUrl(Uri uri)
    {
        var noLastSegment = uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped);
		for (int i = 0; i < uri.Segments.Length - 1; i++)
		{
			noLastSegment += uri.Segments[i];
		}
		noLastSegment = noLastSegment += "$metadata";

        var uriBuilder = new UriBuilder(noLastSegment);
        return uriBuilder.Uri;
    }

    private JObject getBaseArrayTypePropertyDefinition(string description, string title, string format)
	{
        var baseTypePropertyDefinition =  new JObject
        {
            ["title"] = title,
            ["type"] = "array",
            ["items"] = new JObject
            {
                ["type"] = format,
            },
            ["description"] = description
        };
		
		return baseTypePropertyDefinition;
	}

    private JObject getBaseTypePropertyDefinition(string description, string title, string type, string format)
	{
        var baseTypePropertyDefinition =  new JObject
        {
            ["title"] = title,
            ["type"] = type,
            ["format"] = format,
            ["description"] = description
        };
		
		return baseTypePropertyDefinition;
	}

    private JObject getBaseTypePropertyDefinition(string description, string title, string type, string format, string[] enums)
	{
        var enumsJArray = new JArray(enums);

		var baseTypePropertyDefinition =  new JObject
        {
            ["title"] = title,
            ["type"] = type,
            ["format"] = format,
            ["enum"] = enumsJArray,
            ["description"] = description
        };
		
		return baseTypePropertyDefinition;
	}

    private JObject getJObject(string description)
	{
		var basePropertyDefinition = new JObject
      	{
        	["description"] = description,
      	};
		
		return basePropertyDefinition;
	}

    private Dictionary<JObject, List<string>> getFieldsJObject(Dictionary<string, string[]> fields)
    {
        List<string> requiredFieldsList = new List<string>();
        var fieldsObject = new JObject();
        foreach (var field in fields)
		{
            if ("array".Equals(field.Value[1])) 
            {
                fieldsObject.Merge(new JObject()
                {
                    [field.Key] = getBaseArrayTypePropertyDefinition(field.Value[0], field.Value[0], field.Value[2]).DeepClone(),
                });
            }
            else if (string.IsNullOrEmpty(field.Value[3])) 
            {
                fieldsObject.Merge(new JObject()
                {
                    [field.Key] = getBaseTypePropertyDefinition(field.Value[0], field.Value[0], field.Value[1], field.Value[2]).DeepClone(),
                });
            }
            else{
                fieldsObject.Merge(new JObject()
                {
                    [field.Key] = getBaseTypePropertyDefinition(field.Value[0], field.Value[0], field.Value[1], field.Value[2], 
                        field.Value[3].Split('|')).DeepClone(),
                });
            }
            	
            if ("required".Equals(field.Value[4]))
			{
				requiredFieldsList.Add(field.Key);
			}        
		}
        Dictionary<JObject, List<string>> fieldsJObject = new Dictionary<JObject, List<string>>();
        fieldsJObject.Add(fieldsObject, requiredFieldsList);
        return fieldsJObject;
    }

    private JObject getRequstJObject(JObject fieldsObject)
	{
        var newBody = new JObject
		{
			["schema"] = new JObject
			{
                ["type"] = "object",
                ["properties"] = fieldsObject
			},
		};
        return newBody;
    }

    private JObject getRequstJObject(JObject fieldsObject, String[] requiredFieldsArray)
	{
        var requiredFieldsJArray = new JArray(requiredFieldsArray);

        var newBody = new JObject
		{
			["schema"] = new JObject
			{
                ["type"] = "object",
                ["properties"] = fieldsObject,
                ["required"] = requiredFieldsJArray,
			},
		};
        return newBody;
    }

    private Func<String, JObject> getRequestBodyFunction(string functionName)
    {
        Dictionary<string, Func<String, JObject>> functions = new Dictionary<string, Func<String, JObject>>();
        functions.Add("AbsenceDailyDetails", this.GetAbsenceDailyDetailsRequestBody);
        functions.Add("AbsenceHeaders", this.GetAbsenceHeadersRequestBody);
        functions.Add("BankAccounts", this.GetBankAccountsRequestBody);
        functions.Add("CostCentres", this.GetCostCentresRequestBody);
        functions.Add("CostCentreSplits", this.GetCostCentreSplitsRequestBody);
        functions.Add("FixedPayElements", this.GetFixedPayElementsRequestBody);
        functions.Add("Grades", this.GetGradesRequestBody);
        functions.Add("Jobs", this.GetJobsRequestBody);
        functions.Add("Locations", this.GetLocationsRequestBody);
        functions.Add("OperatorBackgroundMessage", this.GetOperatorBackgroundMessageRequestBody);   
        functions.Add("OperatorMessage", this.GetOperatorMessageRequestBody);
        functions.Add("ParentalLeaveKITDays", this.GetParentalLeaveKITDaysRequestBody);
        functions.Add("Posts", this.GetPostsRequestBody);
        functions.Add("ProspectiveWorkers", this.GetProspectiveWorkersRequestBody);
        functions.Add("SharedParentalLeave", this.GetSharedParentalLeaveRequestBody);
        functions.Add("StructureUnit", this.GetStructureUnitRequestBody);
        functions.Add("TemporaryPayElements", this.GetTemporaryPayElementsRequestBody);
        functions.Add("UserDefinedFields", this.GetUserDefinedFieldsRequestBody);
        functions.Add("Workers", this.GetWorkersRequestBody);  
        functions.Add("WorkerAttainments", this.GetWorkerAttainmentsRequestBody); 
        functions.Add("WorkerAttendances", this.GetWorkerAttendancesRequestBody);
        functions.Add("WorkerDrivingLicences", this.GetWorkerDrivingLicencesRequestBody);        
        functions.Add("WorkerNICategories", this.GetWorkerNICategoriesRequestBody);
        functions.Add("WorkPatterns", this.GetWorkPatternsRequestBody);
        functions.Add("WorkerParentalLeave", this.GetWorkerParentalLeaveRequestBody);
        functions.Add("WorkerPassportVisas", this.GetWorkerPassportVisasRequestBody);
        functions.Add("WorkerPensionSchemes", this.GetWorkerPensionSchemesRequestBody);
        functions.Add("WorkerPosts", this.GetWorkerPostsRequestBody);        
        functions.Add("WorkerPRSIDetails", this.GetWorkerPRSIDetailsRequestBody);
        functions.Add("WorkerRelationships", this.GetWorkerRelationshipsRequestBody);
        functions.Add("WorkerTaxCodeHistories", this.GetWorkerTaxCodeHistoriesRequestBody);
        functions.Add("WorkerUSCDetails", this.GetWorkerUSCDetailsRequestBody);
        return functions[functionName];
    }
    
    private async Task<HttpResponseMessage> ValidateNotification()
    {
        var h1_signature = this.Context.Request.Headers.TryGetValues("x-zip-signature", 
                out var tokens) ? tokens.FirstOrDefault() : null;
        var accessKey = this.Context.Request.Headers.TryGetValues("x-access-key", out
            var keys) ? keys.FirstOrDefault() : null;
        this.Context.Request.Headers.Remove("x-access-key");
            
        var content = await this.Context.Request.Content.ReadAsStringAsync()
            .ConfigureAwait(false);
        
        
        var result = JObject.Parse(content); 
        var payload = result["payload"];
        
        var source_result = JObject.Parse(payload.ToString());
        source_result.Value<JObject>("data").Remove("additionalDetails");
        var source_event = Regex.Replace(source_result.ToString(), 
            "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
            
        var hmacHashed = "";
        using (HMACSHA512 hmacsha512 = new HMACSHA512(Encoding.ASCII
            .GetBytes(accessKey.ToString())))
        {
                hmacHashed = BitConverter.ToString(hmacsha512.ComputeHash(Encoding
            .ASCII.GetBytes(source_event))).Replace("-","").ToLower(); 
        }
        HttpResponseMessage response;
        if (!h1_signature.Equals(hmacHashed))
        {
            response = new HttpResponseMessage(HttpStatusCode.BadRequest); 
            response.Content = CreateJsonContent("{\"Error\": \"Invalid Request\"}"); 
        }
        else
        {
            response = new HttpResponseMessage(HttpStatusCode.OK);
        }
        return response;
    }

    private async Task<HttpResponseMessage> WebhookNotification()
    {
        var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        var appUri = query.Get("appUri");
        appUri = Encoding.UTF8.GetString(Convert.FromBase64String(appUri));
        using var appRequest = new HttpRequestMessage(HttpMethod.Post, appUri);
        var notification = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var result = JArray.Parse(notification);

        var h1_eventType = this.Context.Request.Headers.TryGetValues("aeg-event-type", 
            out var eventTypes) ? eventTypes.FirstOrDefault() : null;
        var h1_signature = this.Context.Request.Headers.TryGetValues("x-zip-signature", 
            out var tokens) ? tokens.FirstOrDefault() : null;
        var accessKey = this.Context.Request.Headers.TryGetValues("x-access-key", out
            var keys) ? keys.FirstOrDefault() : null;
        this.Context.Request.Headers.Remove("x-access-key");

        if ("SubscriptionValidation".Equals(h1_eventType))
        {              
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var validationResponse = result[0]["data"]["validationCode"];
        
            var newResult = new JObject
            {
                ["validationResponse"] = validationResponse,
            };
            response.Content = CreateJsonContent(newResult.ToString());
            return response;        
        }
        else if (!string.IsNullOrEmpty(h1_signature))
        {                  
            var arr_source_result = JArray.Parse(notification);               
            var source_result = arr_source_result[0];
            source_result.Value<JObject>("data").Remove("additionalDetails");
            var source_event = Regex.Replace(source_result.ToString(), 
                "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
            
            var hmacHashed = "";
            using (HMACSHA512 hmacsha512 = new HMACSHA512(Encoding.ASCII
                .GetBytes(accessKey)))
            {
                hmacHashed = BitConverter.ToString(hmacsha512.ComputeHash(Encoding
                .ASCII.GetBytes(source_event))).Replace("-","").ToLower(); 
            }
            
            if (!h1_signature.Equals(hmacHashed))
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest); 
                response.Content = CreateJsonContent("{\"Error\": \"Invalid Request\"}");
                return response;
            }
            else
            {
                appRequest.Content = CreateJsonContent(result.ToString());
                return await this.Context.SendAsync(appRequest, this.CancellationToken).ConfigureAwait(false);
            }
        }
        else
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest); 
            response.Content = CreateJsonContent($"Unknown request{h1_eventType}"); 
            return response;
        }
    }

    private async Task<HttpResponseMessage> SubscribeTrigger()
    {
        var content = await this.Context.Request.Content.ReadAsStringAsync()
            .ConfigureAwait(false);
        var original = JObject.Parse(content);
        var paApps = original["URL"]?.ToString();
        var paAppsBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(paApps ?? string.Empty));
        var notificationProxyUri = this.Context.CreateNotificationUri($"/WebhookNotification?appUri={paAppsBase64}");

        // Calling of Connector Action is not working and hence below line is commented
        //original["URL"] = notificationProxyUri.AbsoluteUri;
        this.Context.Request.Content = CreateJsonContent(original.ToString());
        // Use the context to forward/send an HTTP request
        HttpResponseMessage response = await this.Context.SendAsync(
            this.Context.Request,
            this.CancellationToken
        ).ConfigureAwait(continueOnCapturedContext: false);

        // Do the transformation if the response was successful
        if (response.IsSuccessStatusCode)
        {
            var responseString = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(continueOnCapturedContext: false);

            var result = JObject.Parse(responseString); 

            Uri loc = new Uri(string.Format("{0}/{1}",
                    this.Context.OriginalRequestUri.ToString(),
                    result.GetValue("SubscriptionId").ToString()));

            response.Headers.Location = loc;
        }
        return response;        
    }

    private async Task<HttpResponseMessage> GetDynamicResponseJsonBody(Func<XElement, String, JObject> transformationFunction)
    {
        var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        var entityName = query.Get("entity");
        this.Context.Request.RequestUri = manipulateUrl(this.Context.Request.RequestUri);
        using var userInfoRequest = new HttpRequestMessage(HttpMethod.Get, 
        this.Context.Request.RequestUri);
        userInfoRequest.Headers.Authorization = this.Context.Request.Headers.Authorization;
        userInfoRequest.Headers.Add("Ocp-Apim-Subscription-Key", getRequestHeaderValue("Ocp-Apim-Subscription-Key"));
        using var userInfoResponse = await this.Context.SendAsync(userInfoRequest, this.CancellationToken).ConfigureAwait(false);
        var xml = await userInfoResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        XElement root = XElement.Parse(xml);
        XNamespace edmx = "http://docs.oasis-open.org/odata/ns/edmx";
        XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";
        XElement dataServices = root.Element(edmx + "DataServices");
        XElement schema = dataServices.Element(edm + "Schema");

        var body = transformationFunction(schema, entityName);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = CreateJsonContent(body.ToString());
        return response;
    }

    private JObject GetEntities(XElement schema, String entityName)
    {       
        XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";
        XElement entityContainer = schema.Element(edm + "EntityContainer");
        IEnumerable<string> entitySets =
            from el in entityContainer.Elements(edm + "EntitySet")
                let entitySet = (string)el.Attribute("Name")
                orderby entitySet
                select entitySet;
        var entities = new JArray { entitySets.Select(item => new JObject
        {
                ["name"] = item,
                ["value"] = item,
        })};

        var body = new JObject
        {
            ["Entities"] = entities
        };
        return body;
    }

    private JObject GetResponseSchema(XElement schema, String entityName)
    {       
        XNamespace edm = "http://docs.oasis-open.org/odata/ns/edm";
        XElement entityContainer = schema.Element(edm + "EntityContainer");
        IEnumerable<string> entityList =
            from el in entityContainer.Elements(edm + "EntitySet")
			    where ((string)el.Attribute("Name")).Equals(entityName)
			    let entities = (string)el.Attribute("EntityType")
                orderby entities
                select entities;
        var entityType = entityList.FirstOrDefault().Split('.').LastOrDefault();
        IEnumerable<IEnumerable<XElement>> propertyList =
            from el in schema.Elements(edm + "EntityType")
			    where ((string)el.Attribute("Name")).Equals(entityType)
			    let properties = el.Elements(edm + "Property")
                orderby properties
                select properties;

        var itemProperties = new JObject();

        foreach (var item in propertyList.FirstOrDefault())
		{
    		switch (item.Attribute("Type").Value) 
			{
				case "Edm.String":
				case "Edm.Boolean":
				case "Edm.Byte":
				case "Edm.Date":
				case "Edm.DateTime":
				case "Edm.Decimal":
				case "Edm.Double":
				case "Edm.Single":
				case "Edm.Guid":
				case "Edm.Int16":
				case "Edm.Int32":
				case "Edm.Int64":
				case "Edm.SByte":
				case "Edm.TimeOfDay":
				case "Edm.DateTimeOffset":
				default:
					itemProperties[item.Attribute("Name").Value] = getJObject(item.Attribute("Name").Value);
					break;
			}
		}

        var newBody = new JObject
		{
			["schema"] = new JObject
			{
                ["type"] = "object",
                ["properties"] = new JObject
                {
                    ["value"] = new JObject
                    {
                        ["type"] = "array",
                        ["items"] = new JObject
                        {
                            ["type"] = "object",
                            ["properties"] = itemProperties,
                        },
                    },
                    ["@odata.nextLink"] = new JObject
                    {
                        ["type"] = "string",
                        ["description"] = "Next Page Link"
                    },
                },
			},
		};

        return newBody;
    }

    private async Task<HttpResponseMessage> GetZellisObjects()
    {
        var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        var filter = query.Get("$filter");
        var expand = query.Get("$expand");
	    var order = query.Get("$orderby");
        var top = query.Get("$top");
        var skiptoken = query.Get("$skiptoken");
        var select = query.Get("$select");
        var uri = this.Context.Request.RequestUri;
        var modUri = uri.GetLeftPart(UriPartial.Path);

        if(!string.IsNullOrEmpty(filter)) 
        {
            modUri+=$"?$filter={filter}";
            if(!string.IsNullOrEmpty(expand)) 
            {
                modUri+=$"&$expand={expand}";
            }
        }
        else if(!string.IsNullOrEmpty(expand)) 
        {
            modUri+=$"?$expand={expand}";
        }

	    if(!string.IsNullOrEmpty(order))
        {
            if(!string.IsNullOrEmpty(filter) || !string.IsNullOrEmpty(expand))
            {
                modUri+=$"&$orderby={order}";
            }
            else
            {
                modUri+=$"?$orderby={order}";  
            }
        }

        if(!string.IsNullOrEmpty(top))
        {
            if(!string.IsNullOrEmpty(filter) || !string.IsNullOrEmpty(expand) || !string.IsNullOrEmpty(order))
            {
                modUri+=$"&$top={top}";
            }
            else
            {
                modUri+=$"?$top={top}";  
            }
        }

        if(!string.IsNullOrEmpty(skiptoken))
        {
            if(!string.IsNullOrEmpty(filter) || !string.IsNullOrEmpty(expand) || !string.IsNullOrEmpty(order) || !string.IsNullOrEmpty(top))
            {
                modUri+=$"&$skiptoken={skiptoken}";
            }
            else
            {
                modUri+=$"?$skiptoken={skiptoken}";  
            }
        }

        if(!string.IsNullOrEmpty(select))
        {
            if(!string.IsNullOrEmpty(filter) || !string.IsNullOrEmpty(expand) || !string.IsNullOrEmpty(order) || !string.IsNullOrEmpty(top)
             || !string.IsNullOrEmpty(skiptoken))
            {
                modUri+=$"&$select={select}";
            }
            else
            {
                modUri+=$"?$select={select}";  
            }
        }

        var uriBuilder = new UriBuilder(modUri);
        this.Context.Request.RequestUri = uriBuilder.Uri;
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
    }

    private async Task<HttpResponseMessage> GetDynamicRequestBody()
    {
        var query = HttpUtility.ParseQueryString(this.Context.Request.RequestUri.Query);
        var entityName = query.Get("entity");
        var operation = query.Get("operation");
        return await this.GetDynamicRequestJsonBody(getRequestBodyFunction(entityName), operation).ConfigureAwait(false);
    }
    
    private async Task<HttpResponseMessage> GetDynamicRequestJsonBody(Func<String, JObject> transformationFunction, String operation)
    {
        var body = transformationFunction(operation);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = CreateJsonContent(body.ToString());
        return response;
    }

    private JObject GetWorkerAttainmentsRequestBody(String operation)
    {      
        string[] mandatoryRequiredFields = { "WorkerId", "AttainmentGroupId", "GroupText", "AttainmentSubjectId", "SubjectText", "AttainmentTypeId", 
            "TypeText", "Status" };

        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
        fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
        fields.Add("AttainmentGroupId", new string[]{ "AttainmentGroupId", "integer", "int32", "", "required" });
        fields.Add("GroupText", new string[]{ "Group Text", "string", "", "", "required" });
		fields.Add("AttainmentSubjectId", new string[]{ "AttainmentSubjectId", "integer", "int32", "", "required" });
        fields.Add("SubjectText", new string[]{ "Subject Text", "string", "", "", "required" });
        fields.Add("AttainmentTypeId", new string[]{ "AttainmentSubjectId", "integer", "int32", "", "required" });
        fields.Add("TypeText", new string[]{ "Type Text", "string", "", "", "required" });
        fields.Add("Status", new string[]{ "Status", "string", "", "", "required" });
        fields.Add("AttainmentDate", new string[]{ "Attainment Date", "string", "date", "", "" });
        fields.Add("ReviewedDate", new string[]{ "Reviewed Date", "string", "date", "", "" });
        fields.Add("NextReviewDate", new string[]{ "Next Review Date", "string", "date", "", "" });
        fields.Add("AttainmentReviewedId", new string[]{ "AttainmentReviewedId", "integer", "int32", "", "" });
        fields.Add("AttainmentEstablishmentId", new string[]{ "AttainmentEstablishmentId", "integer", "int32", "", "" });
        fields.Add("AttainmentAchievementGradeId", new string[]{ "AttainmentAchievementGradeId", "integer", "int32", "", "" });
        fields.Add("AttainmentRequirementGradeId", new string[]{ "AttainmentRequirementGradeId", "integer", "int32", "", "" });
        fields.Add("OriginalDate", new string[]{ "Original Date", "string", "date", "", "" });
        fields.Add("TargetDate", new string[]{ "Target Date", "string", "date", "", "" });
        fields.Add("AttainmentSourceId", new string[]{ "AttainmentSourceId", "integer", "int32", "", "" });
        fields.Add("AttainmentPriorityId", new string[]{ "AttainmentPriorityId", "integer", "int32", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, "PATCH".Equals(operation, StringComparison.OrdinalIgnoreCase) ? 
            mandatoryRequiredFields : fieldsJObject.Value.ToArray());
    }

    private JObject GetWorkerAttendancesRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
        fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
        fields.Add("AttendanceTypeId", new string[]{ "AttendanceTypeId", "integer", "int32", "", "required" });
		fields.Add("AttendanceCategoryId", new string[]{ "AttendanceCategoryId", "integer", "int32", "", "required" });
        fields.Add("StartDate", new string[]{ "Start Date", "string", "date", "", "required" });
        fields.Add("AttendanceDuration", new string[]{ "Attendance Duration", "string", "", "", "" });
        fields.Add("PostId", new string[]{ "PostId", "integer", "int32", "", "" });
        fields.Add("CostCentreId", new string[]{ "CostCentreId", "integer", "int32", "", "" });
        fields.Add("InputCash", new string[]{ "Input Cash", "string", "", "", "" });
		fields.Add("Calculate", new string[]{ "Calculate", "boolean", "", "", "" });
        fields.Add("Comments", new string[]{ "Comments", "string", "", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
    }
    
    private JObject GetUserDefinedFieldsRequestBody(String operation)
    {  
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
	fields.Add("EntityId", new string[]{ "EntityId", "integer", "int32", "", "" });
	fields.Add("FieldId", new string[]{ "FieldId", "integer", "int32", "", "" });
	fields.Add("FieldValue", new string[]{ "Field Value", "string", "", "", "" });

        var fieldsJObject = getFieldsJObject(fields).First();

        return getRequstJObject(fieldsJObject.Key);
    }

    private JObject GetWorkersRequestBody(String operation)
    {  
        string[] mandatoryRequiredFields = { "WorkerNumber" };

        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
        fields.Add("WorkerNumber", new string[]{ "Worker Number", "string", "", "", "required" });
		fields.Add("TitleId", new string[]{ "TitleId", "integer", "int32", "", "", "required" });
		fields.Add("FirstForename", new string[]{ "First Name","string", "", "", "required" });
        fields.Add("Surname", new string[]{ "Surname", "string", "", "", "required" });
		fields.Add("BirthDate", new string[]{ "Birth Date", "string", "date", "", "required" });
		fields.Add("GenderId", new string[]{ "GenderId", "integer", "int32", "", "required" });
        fields.Add("PayGroupId", new string[]{ "PayGroupId", "integer", "int32", "", "required" });
		fields.Add("PayGroupEffectiveDate", new string[]{ "PayGroupEffectiveDate", "string", "date", "", "required" });
		fields.Add("OriginalJoinReasonId", new string[]{ "OriginalJoinReasonId", "integer", "int32", "", "required" });
        fields.Add("OriginalStartDate", new string[]{ "Original Start Date", "string", "date", "", "required" });
		fields.Add("CurrentJoinReasonId", new string[]{ "CurrentJoinReasonId", "integer", "int32", "", "required" });
		fields.Add("CurrentStartDate", new string[]{ "Current Start Date", "string", "date", "", "required" });
        fields.Add("Age", new string[]{ "Age", "integer", "int32", "", "" });
        fields.Add("OtherForenames", new string[]{ "Other First Name", "string", "", "", "" });
        fields.Add("PreviousSurname", new string[]{ "Previous Surname", "string", "", "", "" });
        fields.Add("KnownAs", new string[]{ "Known As", "string", "", "", "" });
        fields.Add("Initials", new string[]{ "Initials", "string", "", "", "" });
        fields.Add("NationalInsuranceNo", new string[]{ "National Insurance Number", "string", "", "", "" });
        fields.Add("PersonalEmail", new string[]{ "Personal Email Address", "string", "", "", "" });
        fields.Add("BusinessEmail", new string[]{ "Business Email Address", "string", "", "", "" });
        fields.Add("WorkTelephoneNumber", new string[]{ "Work Telephone Number", "string", "", "", "" });
        fields.Add("WorkTelephoneExtension", new string[]{ "Work Telephone Extension", "string", "", "", "" });
        fields.Add("HomeTelephoneNumber", new string[]{ "Home Telephone Number", "string", "", "", "" });
        fields.Add("MobileTelephoneNumber", new string[]{ "Mobile Telephone Number", "string", "", "", "" });
        fields.Add("CountryOfBirthId", new string[]{ "CountryOfBirthId", "integer", "int32", "", "" });
        fields.Add("MaritalStatusId", new string[]{ "MaritalStatusId", "integer", "int32", "", "" });
        fields.Add("MaritalStatusEffectiveDate", new string[]{ "MaritalStatusEffectiveDate", "string", "date", "", "" });
        fields.Add("ReligionId", new string[]{ "ReligionId", "integer", "int32", "", "" });
        fields.Add("EthnicOriginId", new string[]{ "EthnicOriginId", "integer", "int32", "", "" });
        fields.Add("LeaveDate", new string[]{ "Leave Date", "string", "date", "", "" });        
        fields.Add("LeaveReasonId", new string[]{ "LeaveReasonId", "integer", "int32", "", "" });
        fields.Add("DeathDate", new string[]{ "Death Date", "string", "date", "", "" });
        fields.Add("Suspend", new string[]{ "Suspend", "string", "", "", "" });   
        fields.Add("ROIPensionTracingNo", new string[]{ "ROI Pension Tracing Number", "string", "", "", "" });   
        fields.Add("ROIEmploymentIdentifier", new string[]{ "ROI Employment Identifier", "string", "", "", ""  });           
        fields.Add("Rehire", new string[]{ "Rehire", "string", "", "", "" });    
        fields.Add("IsROIDirector", new string[]{ "ROI Director", "boolean", "", "", "" });  
        fields.Add("ROIDirectorTypeId", new string[]{ "ROIDirectorTypeId", "integer", "int32", "", "" });   
        fields.Add("EmployeeTypeId", new string[]{ "EmployeeTypeId", "integer", "int32", "", "" });  

        Dictionary<string, string[]> addressFields = new Dictionary<string, string[]>();
        addressFields.Add("LineOne", new string[]{ "Address Line One", "string", "", "", "" });
		addressFields.Add("LineTwo", new string[]{ "Address Line Two", "string", "", "", "" });
		addressFields.Add("LineThree", new string[]{ "Address Line Three", "string", "", "", "" });
        addressFields.Add("LineFour", new string[]{ "Address Line Four", "string", "", "", "" });
        addressFields.Add("PostCode", new string[]{ "Post Code", "string", "", "", "" });
		addressFields.Add("Municipality", new string[]{ "Municipality", "string", "", "", "" });
		addressFields.Add("Region", new string[]{ "Region", "string", "", "", "" });
        addressFields.Add("CountryId", new string[]{ "CountryId", "integer", "int32", "", "" });

        Dictionary<string, string[]> countryFields = new Dictionary<string, string[]>();
        countryFields.Add("Code", new string[]{ "Code", "string", "", "", "" });

        var fieldsJObject = getFieldsJObject(fields).First();
        var fieldsObject = fieldsJObject.Key;
        var addressObject = getFieldsJObject(addressFields).First();
        
        var address = new JObject
		{
			["Address"] = new JObject
			{
                ["type"] = "object",
                ["properties"] = addressObject.Key,
                ["description"] = "Address",
			},
		};

        var countryObject = getFieldsJObject(countryFields).First();
        
        var country = new JObject
		{
			["Country"] = new JObject
			{
                ["type"] = "object",
                ["properties"] = countryObject.Key,
                ["description"] = "Country",
			},
		};

        address.Merge(country);

        fieldsObject.Merge(address);

        return getRequstJObject(fieldsObject, "PATCH".Equals(operation, StringComparison.OrdinalIgnoreCase) ? 
            mandatoryRequiredFields : fieldsJObject.Value.ToArray());
    }

    private JObject GetAbsenceHeadersRequestBody(String operation)
    {       
        string[] mandatoryRequiredFields = { "WorkerId", "AbsenceTypeId" };

        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
        fields.Add("PostId", new string[]{ "PostId", "integer", "int32", "", "", "" });
		fields.Add("AbsenceStart", new string[]{ "Absence Start Date", "string", "date", "", "required" });
        fields.Add("AbsenceStartTime", new string[]{ "Absence Start Time", "string", "", "", "" });
        fields.Add("AbsenceEnd", new string[]{ "Absence End Date", "string", "date", "", "required" });
        fields.Add("AbsenceEndTime", new string[]{ "Absence End Time", "string", "", "", "" });
        fields.Add("AbsenceTypeId", new string[]{ "AbsenceTypeId", "integer", "int32", "", "required" });
        fields.Add("NoProc", new string[]{ "No Proc", "boolean", "", "", "" });
		fields.Add("AbsenceCode", new string[]{ "Absence Code", "string", "", "", "" });
        fields.Add("AbsenceReasonId", new string[]{ "Absence Reason", "string", "", "", "" });
        fields.Add("LinkDate", new string[]{ "Link Date", "string", "date", "", ""  });
        fields.Add("AbsenceCertificateTypeId", new string[]{ "AbsenceCertificateTypeId", "integer", "int32", "", "" });
        fields.Add("CertProd", new string[]{ "Cert Prod", "boolean", "", "", "" });
        fields.Add("UnitsReq", new string[]{ "Units Required", "integer", "int32", "", "" });
        fields.Add("AbsenceSubTypeId", new string[]{ "AbsenceSubTypeId", "integer", "int32", "", "" });
        fields.Add("CostCentreId", new string[]{ "CostCentreId", "integer", "int32", "", "" });
        fields.Add("AbsenceReasonTypeId", new string[]{ "AbsenceReasonTypeId", "integer", "int32", "", "" });
        fields.Add("AbsenceCauseId", new string[]{ "AbsenceCauseId", "integer", "int32", "", "" });
        fields.Add("CertExpiry", new string[]{ "Cert Expiry", "string", "date", "", "" });
        fields.Add("HealthSafety", new string[]{ "Health Safety", "boolean", "", "", "" });
        fields.Add("ReturnWkIntvw", new string[]{ "Return Wk Intvw", "string", "date", "", "" });
        fields.Add("DueMatchingDate", new string[]{ "Due Matching Date", "string", "date", "", "" });
        fields.Add("Action", new string[]{ "Action", "string", "", "", "" });

        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, "PATCH".Equals(operation, StringComparison.OrdinalIgnoreCase) ? 
            mandatoryRequiredFields : fieldsJObject.Value.ToArray());
    }

    private JObject GetAbsenceDailyDetailsRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
        fields.Add("PostId", new string[]{ "PostId", "integer", "int32", "", "" });
        fields.Add("AbsenceStart", new string[]{ "Absence Start Date", "string", "date", "", "required" });
        fields.Add("AbsenceStartTime", new string[]{ "Absence Start Time", "string", "", "", "" });
        fields.Add("AbsenceEnd", new string[]{ "Absence End Date", "string", "date", "", "required" });
        fields.Add("AbsenceEndTime", new string[]{ "Absence End Time", "string", "", "", "" });

        Dictionary<string, string[]> absenceDailyDetailsFields = new Dictionary<string, string[]>();
        absenceDailyDetailsFields.Add("Date", new string[]{ "Date", "string", "date", "", "required" });
		absenceDailyDetailsFields.Add("HoursPerDay", new string[]{ "Hours Per Day", "integer", "int32", "", "required" });
        absenceDailyDetailsFields.Add("NonWorkingHrsInDay", new string[]{ "Non Working Hrs In Day", "integer", "int32", "", "required" });
		absenceDailyDetailsFields.Add("IsWorkDay", new string[]{ "Working Day", "boolean", "", "", "required" });
		absenceDailyDetailsFields.Add("IsPayQualifyingDay", new string[]{ "Pay Qualifying Day", "boolean", "", "", "required" });
        absenceDailyDetailsFields.Add("IsEntitlementQualifyingDay", new string[]{ "Entitlement Qualifying Day", "boolean", "", "", "required" });
        absenceDailyDetailsFields.Add("IsSSPQualifyingDay", new string[]{ "SSP Qualifying Day", "boolean", "", "", "required" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
        var fieldsObject = fieldsJObject.Key;
        var absenceDailyDetailsObject = getFieldsJObject(absenceDailyDetailsFields).First();
        var requiredFieldsJArray = new JArray(absenceDailyDetailsObject.Value.ToArray());

        var absenceDailyDetails = new JObject
		{
			["AbsenceDailyDetails"] = new JObject
			{
                ["type"] = "array",
                ["items"] = new JObject
                {
                    ["type"] = "object",
                    ["properties"] = absenceDailyDetailsObject.Key,
                    ["required"] = requiredFieldsJArray,
                },
                ["description"] = "Absence Daily Details",
			},
		};

        fieldsJObject.Value.Add("AbsenceDailyDetails");

        fieldsObject.Merge(absenceDailyDetails);
       
        return getRequstJObject(fieldsObject, fieldsJObject.Value.ToArray());
    }

    private JObject GetBankAccountsRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
        fields.Add("AccountTypeId", new string[]{ "AccountTypeId", "integer", "int32", "", "required" });
        fields.Add("AccountName", new string[]{ "Account Name", "string", "", "", "required" });
        fields.Add("AccountNo", new string[]{ "Account Number", "string", "", "", "required" });
        //fields.Add("BranchDetailId", new string[]{ "BranchDetailId", "integer", "int32", "BRANCHSORTCODE|BIC", "required" });
        fields.Add("BranchDetailId", new string[]{ "BranchDetailId", "integer", "int32", "", "required" });
        fields.Add("SortCode", new string[]{"Branch Sort Code", "string", "", "", "" });
        fields.Add("Bic", new string[]{ "BIC", "string", "", "", "" });
        fields.Add("AccountIBAN", new string[]{ "Account IBAN", "string", "", "", "" });
        fields.Add("AccountRollNo", new string[]{ "Account Roll Number", "string", "", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
    }

    private JObject GetCostCentresRequestBody(String operation)
    {       
        string[] mandatoryRequiredFields = { "Code" };

        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("Code", new string[]{ "Code", "string", "", "", "required" });
        fields.Add("ShortDescription", new string[]{ "Short Description", "string", "", "", "required" });
        fields.Add("LongDescription", new string[]{ "Long Description", "string", "", "", "required" });
		fields.Add("ObsoleteDate", new string[]{ "Obsolete Date", "string", "date", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, "PATCH".Equals(operation, StringComparison.OrdinalIgnoreCase) ? 
            mandatoryRequiredFields : fieldsJObject.Value.ToArray());
    }

    private JObject GetCostCentreSplitsRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
        fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "" });
        fields.Add("PostId", new string[]{ "PostId", "integer", "int32", "", "" });
        fields.Add("StartDT", new string[]{ "Start Date", "string", "date", "", "required" });
        fields.Add("EndDT", new string[]{ "End Date", "string", "date", "", "" });
		fields.Add("CostSource", new string[]{ "Cost Source", "string", "", "", "required" });

        Dictionary<string, string[]> costCentreSplitFields = new Dictionary<string, string[]>();
        costCentreSplitFields.Add("CostCentreId", new string[]{ "CostCentreId", "integer", "int32", "", "required" });
        costCentreSplitFields.Add("EesPercentage", new string[]{ "EES Percentage", "string", "", "", "required" });
        costCentreSplitFields.Add("ErsPercentage", new string[]{ "ERS Percentage", "string", "", "", "required" });
        
        var fieldsJObject = getFieldsJObject(fields).First();        
        var fieldsObject = fieldsJObject.Key;
        var costCentreSplitObject = getFieldsJObject(costCentreSplitFields).First();
        var requiredFieldsJArray = new JArray(costCentreSplitObject.Value.ToArray());

        var costCentreSplit = new JObject
		{
			["CostCentreSplit"] = new JObject
			{
                ["type"] = "array",
                ["items"] = new JObject
                {
                    ["type"] = "object",
                    ["properties"] = costCentreSplitObject.Key,
                    ["required"] = requiredFieldsJArray,
                },
                ["description"] = "Cost Centre Split",
			},
		};

        fieldsJObject.Value.Add("CostCentreSplit");

        fieldsObject.Merge(costCentreSplit);
       
        return getRequstJObject(fieldsObject, fieldsJObject.Value.ToArray());
    }

    private JObject GetFixedPayElementsRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
        fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
        fields.Add("PayElementId", new string[]{ "PayElementId", "integer", "int32", "", "", "required" });
		fields.Add("StartDate", new string[]{ "Start Date", "string", "date", "", "required" });
        fields.Add("EndDate", new string[]{ "End Date", "string", "date", "", "" });
        fields.Add("Amount", new string[]{ "Amount", "number", "float", "", "" });
        fields.Add("Rate", new string[]{ "Amount", "number", "float", "", "" });
        fields.Add("PostId", new string[]{ "PostId", "integer", "int32", "", "" });
        fields.Add("CostCentreId", new string[]{ "CostCentreId", "integer", "int32", "", "" });
        fields.Add("PayMethodId", new string[]{ "PayMethodId", "integer", "int32", "", "" });
        fields.Add("PayReasonId", new string[]{ "PayReasonId", "integer", "int32", "", "" });
        fields.Add("ThirdPartyId", new string[]{ "ThirdPartyId", "integer", "int32", "", "" });
        fields.Add("Units", new string[]{ "Units", "integer", "int32", "", "" });
        fields.Add("Arrears", new string[]{ "Arrears", "number", "float", "", "" });
        fields.Add("UnitsArrears", new string[]{ "UnitsArrears", "number", "float", "", "" });
        fields.Add("IsSuspended", new string[]{ "Suspended", "boolean", "", "", "" });
		fields.Add("ReferenceNo", new string[]{ "Reference Number", "string", "", "", "" });
        fields.Add("ErsCostCentreId", new string[]{ "ErsCostCentreId", "string", "", "", "" });
        fields.Add("ErsAmount", new string[]{ "RS Amount", "number", "float", "", "" });
        fields.Add("ErsPercentage", new string[]{ "ERS Percentage", "number", "float", "", "" });
        fields.Add("ErsArrears", new string[]{ "ERS Arrears", "number", "float", "", "" });
        fields.Add("EesPercentage", new string[]{ "EES Percentage", "number", "float", "", "" });
        fields.Add("ReducingBal", new string[]{ "Reducing Balance", "number", "float", "", "" });
        fields.Add("ReducingBalOv", new string[]{ "Reducing Balance Override", "number", "float", "", "" });     
        fields.Add("ReducingBalPeriods", new string[]{ "Reducing Balance Periods", "integer", "int32", "", "" });
        fields.Add("TableId", new string[]{ "Table Id", "string", "", "", "" });
        fields.Add("TableRow", new string[]{ "Table Row", "string", "", "", "" });
        fields.Add("TableColumn", new string[]{ "Table Column", "string", "", "", "" });
        fields.Add("ForeignCurr", new string[]{ "Foreign Currency", "string", "", "", "" });
        fields.Add("BankAccountNo", new string[]{ "Bank Account Number", "string", "", "", "" });
        fields.Add("BankAccountName", new string[]{ "Bank Account Name", "string", "", "", "" });
        fields.Add("BankAccountTypeCode", new string[]{ "Bank Account Type Code", "string", "", "", "" });
        fields.Add("BankAccountSort", new string[]{ "Bank Account Sort Code", "string", "", "", "" });
        fields.Add("BankIban", new string[]{ "Bank IBAN", "string", "", "", "" });
        fields.Add("BsRollNo", new string[]{ "BS Roll Number", "string", "", "", "" });
        fields.Add("BankCountryId", new string[]{ "BankCountryId", "integer", "int32", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
    }

    private JObject GetGradesRequestBody(String operation)
    {       
        string[] mandatoryRequiredFields = { "Code" };

        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("Code", new string[]{ "Code", "string", "", "", "required" });
        fields.Add("GradeShortDesc", new string[]{ "Short Description", "string", "", "", "required" });
        fields.Add("GradeLongDesc", new string[]{ "Long Description", "string", "", "", "required" });
        fields.Add("GradeStartDate", new string[]{ "Start Date", "string", "date", "", "required" });
        fields.Add("GradeObsoleteDate", new string[]{ "Obsolete Date", "string", "date", "", "" });
        fields.Add("GradeGroup", new string[]{ "Group", "string", "", "", "" });
        fields.Add("GradeSortCode", new string[]{ "Sort Code", "integer", "int32", "", "" });
        fields.Add("GradeSecurityLevel", new string[]{ "Security Level", "integer", "int32", "", "" });
        fields.Add("GradeTeachFlag", new string[]{ "Teach Flag", "string", "", "", "" });
        fields.Add("GradeHayFlag", new string[]{ "Hay Flag", "string", "", "", "" });
        fields.Add("GradeHesaFlag", new string[]{ "Hesa Flag", "string", "", "", "" });
        fields.Add("NationalPayFrameInd", new string[]{ "National Pay Frame Indicator", "string", "", "", "" });
        fields.Add("RegionalPaySpine", new string[]{ "Regional Pay Spine", "string", "", "", "" });
        fields.Add("13thSalaryInd", new string[]{ "13th Salary Indicator", "string", "", "", "" });
        fields.Add("SuppressRateCalcMsg", new string[]{ "Suppress Rate Calculation Message", "string", "", "", "" });
        fields.Add("GradeSpinalAutoCreate", new string[]{ "Grade Spinal Auto Create", "string", "", "", "" });
        fields.Add("GradeOnboardCampaignId", new string[]{ "Grade Onboard Campaign Id", "string", "", "", "" });
        fields.Add("GradeHistTableId", new string[]{ "Grade History Table Id", "string", "", "", "" });
        fields.Add("GradeHistStartDate", new string[]{ "Grade History Start Date", "string", "date", "", "" });
        fields.Add("GradeHistEndDate", new string[]{ "Grade History End Date", "string", "date", "", "" });
        fields.Add("GradeHistMaxPoint", new string[]{ "Grade History Max Point", "integer", "int32", "", "" });
        fields.Add("GradeHistMinPoint", new string[]{ "Grade History Min Point", "integer", "int32", "", "" });
        fields.Add("GradeHistIncrementFlag", new string[]{ "Grade History Increment Flag", "string", "", "", "" });
        fields.Add("GradeHistIncrementStep", new string[]{ "Grade History Increment Step", "string", "", "", "" });
        fields.Add("GradeHistPayScale", new string[]{ "Grade History Pay Scale", "string", "", "", "" });
        fields.Add("GradeRespCategory", new string[]{ "Grade Resp Category", "string", "", "", "" });
        fields.Add("GradeRespCatMaxPoints", new string[]{ "Grade Resp Cat Max Points", "integer", "int32", "", "" });
        fields.Add("GradeRespCatPriority", new string[]{ "Grade Resp Category Priority", "integer", "int32", "", "" });
        fields.Add("GradeRespCatIncrFlag", new string[]{ "Grade Resp Category Incremental Flag", "string", "", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, "PATCH".Equals(operation, StringComparison.OrdinalIgnoreCase) ? 
            mandatoryRequiredFields : fieldsJObject.Value.ToArray());
    }

    private JObject GetJobsRequestBody(String operation)
    {       
        string[] mandatoryRequiredFields = { "Code" };

        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("Code", new string[]{ "Code", "string", "", "", "required" });
        fields.Add("JobShortDesc", new string[]{ "Short Description", "string", "", "", "required" });
        fields.Add("JobLongDesc", new string[]{ "Long Description", "string", "", "", "required" });
        fields.Add("JobStartDate", new string[]{ "Start Date", "string", "date", "", "required" }); 
        fields.Add("JobObsoleteDate", new string[]{ "Obsolete Date", "string", "date", "", "" });
        fields.Add("JobGrade", new string[]{ "Job Grade", "string", "", "", "" });
        fields.Add("JobSalaryMin", new string[]{ "Job Salary Min", "string", "", "", "" });
        fields.Add("JobSalaryMax", new string[]{ "JobSalaryMax", "string", "", "", "" });
        fields.Add("JobSecurityLevel", new string[]{ "Job Security Level", "string", "", "", "" });
        fields.Add("JobType", new string[]{ "Job Type", "string", "", "", "" });
        fields.Add("JobSortCode", new string[]{ "Job Sort Code", "string", "", "", "" });
        fields.Add("JobRole", new string[]{ "Job Role", "string", "", "", "" });
        fields.Add("JobIrregClaim", new string[]{ "Job Irregular Claim", "string", "", "", "" });
        fields.Add("JobExpenseClaim", new string[]{ "Job Expense Claim", "string", "", "", "" });
        fields.Add("JobInternetProfile", new string[]{ "Job Internet Profile", "string", "", "", "" });
        fields.Add("JobIntranetProfile", new string[]{ "Job Intranet Profile", "string", "", "", "" });
        fields.Add("JobOnboardProfile", new string[]{ "Job Onboard Profile", "string", "", "", "" });
        fields.Add("JobRtiPayFlag", new string[]{ "Job RTI Pay Flag", "string", "", "", "" });
        fields.Add("JobOnboardCampaignId", new string[]{ "Job Onboard Campaign Id", "string", "", "", "" });
        fields.Add("JobServConStartDate", new string[]{ "Job Service Condition Start Date", "string", "date", "", "" });
        fields.Add("JobServConEndDate", new string[]{ "Job Service Condition End Date", "string", "date", "", "" });
        fields.Add("ServiceConditionId", new string[]{ "ServiceConditionId", "integer", "int32", "", "" });
        fields.Add("JobFteStartDate", new string[]{ "Job FTE Start Date", "string", "date", "", "" });        
        fields.Add("JobFteAuthDate", new string[]{ "Job FTE Auth Date", "string", "date", "", "" });
        fields.Add("JobFteHours", new string[]{ "Job FTE Hours", "string", "", "", "" });
        fields.Add("JobFteWpy", new string[]{ "Job FTE WPY", "string", "", "", "" });
        fields.Add("JobFteAuthBody", new string[]{ "Job FTE Auth Body", "string", "", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, "PATCH".Equals(operation, StringComparison.OrdinalIgnoreCase) ? 
            mandatoryRequiredFields : fieldsJObject.Value.ToArray());
    }

    private JObject GetLocationsRequestBody(String operation)
    {       
        string[] mandatoryRequiredFields = { "Code" };

        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("Code", new string[]{ "Code", "string", "", "", "required" });
        fields.Add("ShortDescription", new string[]{ "Short Description", "string", "", "", "required" });
        fields.Add("LongDescription", new string[]{ "Long Description", "string", "", "", "required" });
        fields.Add("StartDate", new string[]{ "Start Date", "string", "date", "", "required" });
        fields.Add("Region", new string[]{ "Region", "string", "", "", "" });
        fields.Add("LocLevel2", new string[]{ "Location Level 2", "string", "", "", "" });        
        fields.Add("ObsoleteDate", new string[]{ "Obsolete Date", "string", "date", "", "" });
        fields.Add("WeightingCategory", new string[]{ "Weighting Category", "string", "", "", "" });
        fields.Add("TrainingRegion", new string[]{ "Training Region", "string", "", "", "" });
        fields.Add("AssessmentCentre", new string[]{ "Assessment Centre", "string", "", "", "" });
        fields.Add("Zone", new string[]{ "Zone", "string", "", "", "" });
        fields.Add("DefaultParkingLevy", new string[]{ "Default Parking Levy", "string", "", "", "" });
        fields.Add("AddressLineOne", new string[]{ "Address Line One", "string", "", "", "" });
        fields.Add("AddressLineTwo", new string[]{ "Address Line Two", "string", "", "", "" });
        fields.Add("AddressLineThree", new string[]{ "Address Line Three", "string", "", "", "" });
        fields.Add("AddressLineFour", new string[]{ "Address Line Four", "string", "", "", "" });
        fields.Add("AddressPostCode", new string[]{ "Address Post Code", "string", "", "", "" });
        fields.Add("AddressCountryId", new string[]{ "AddressCountryId", "integer", "int32", "", "" });
        fields.Add("AddressMunicipality", new string[]{ "Address Municipality", "string", "", "", "" });
        fields.Add("AddressRegion", new string[]{ "Address Region", "string", "", "", "" });
        fields.Add("Email", new string[]{ "Email", "string", "", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, "PATCH".Equals(operation, StringComparison.OrdinalIgnoreCase) ? 
            mandatoryRequiredFields : fieldsJObject.Value.ToArray());
    }

    private JObject GetOperatorBackgroundMessageRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("ItemID", new string[]{ "Item ID", "string", "", "", "required" });
		fields.Add("CurrentCount", new string[]{ "Current Count", "integer", "int32", "", "required" });
		fields.Add("Status", new string[]{ "Status", "string", "", "FAILED|COMPLETE|STARTING|STOPPED|INITIATING|ABORTED|PROGRESS", "required" });
		fields.Add("FeedbackText", new string[]{ "Feedback Text", "string", "", "", "" });
        fields.Add("EndDate", new string[]{ "End Date", "string", "date", "", "" });
        fields.Add("EndTime", new string[]{ "End Time", "string", "", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
    }

    private JObject GetOperatorMessageRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("ToOperator", new string[]{ "To Operator", "string", "", "", "required" });
        fields.Add("MessageSubject", new string[]{ "Message Subject", "string", "", "", "required" });
        fields.Add("MessageBody", new string[]{ "Message Body", "string", "", "", "required" });
        fields.Add("TaskID", new string[]{ "Task ID", "string", "", "", "required" });
        fields.Add("MessageLevel", new string[]{ "Message Level", "string", "", "", "required" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
    }

    private JObject GetParentalLeaveKITDaysRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
		fields.Add("ExpectedDueDate", new string[]{ "Expected Due Date", "string", "date", "", "required" });
		fields.Add("KITSplitDayDate", new string[]{ "KIT Split Day Date", "string", "date", "", "required" });
        fields.Add("KITSplitDayType", new string[]{ "KIT Split Day Type", "string", "", "", "required" });
        fields.Add("KITSplitDayAmt", new string[]{ "KIT Split Day Amount", "number", "float", "", "" });
        fields.Add("KITSplitOffsetAmt", new string[]{ "KIT Split Offset Amount", "number", "float", "", "" });
        fields.Add("DeriveKitSplitOs", new string[]{ "Derive KIT Split Os", "string", "", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
    }

    private JObject GetPostsRequestBody(String operation)
    {       
        string[] mandatoryRequiredFields = { "Code" };

        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("Code", new string[]{ "Code", "string", "", "", "required" });
        fields.Add("PostShortDescription", new string[]{ "Short Description", "string", "", "", "required" });
        fields.Add("PostLongDescription", new string[]{ "Long Description", "string", "", "", "required" });
        fields.Add("PostStartDate", new string[]{ "Start Date", "string", "date", "", "required" });   
        fields.Add("JobId", new string[]{ "JobId", "integer", "int32", "", "required" });
        fields.Add("WorkPatternId", new string[]{ "WorkPatternId", "integer", "int32", "", "required" });
        fields.Add("PostObsoleteDate", new string[]{ "Obsolete Date", "string", "date", "", "" });
        fields.Add("PostGradeStartDate", new string[]{ "Grade Start Date", "string", "date", "", "" });
        fields.Add("PostGradeEndDate", new string[]{ "Grade End Date", "string", "date", "", "" });
        fields.Add("PostLocationStartDate", new string[]{ "Location Start End Date", "string", "date", "", "" });
        fields.Add("LocationId", new string[]{ "LocationId", "integer", "int32", "", "" });
        fields.Add("TitleDate", new string[]{ "Title Date", "string", "date", "", "" });
        fields.Add("TitleShortDescription", new string[]{ "Title Short Description", "string", "", "", "" });
        fields.Add("TitleLongDescription", new string[]{ "Title Long Description", "string", "", "", "" });
        fields.Add("FteStartDate", new string[]{ "FTE Start Date", "string", "date", "", "" });
        fields.Add("FteHeadCount", new string[]{ "FTE Head Count", "string", "", "", "" });
        fields.Add("FteHours", new string[]{ "FTE Hours", "string", "", "", "" });
        fields.Add("FteOverrideHours", new string[]{ "FTE Override Hours", "string", "", "", "" });
        fields.Add("PostClass", new string[]{ "Post Class", "string", "", "", "" });
        fields.Add("PostInternetProfile", new string[]{ "Post Internet Profile", "string", "", "", "" });
        fields.Add("PostIntranetProfile", new string[]{ "Post Intranet Profile", "string", "", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, "PATCH".Equals(operation, StringComparison.OrdinalIgnoreCase) ? 
            mandatoryRequiredFields : fieldsJObject.Value.ToArray());
    }

    private JObject GetProspectiveWorkersRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
        fields.Add("WorkerNumber", new string[]{ "Worker Number", "string", "", "", "required" });
		fields.Add("TitleId", new string[]{ "TitleId", "integer", "int32", "", "", "required" });
		fields.Add("FirstForename", new string[]{ "First Name","string", "", "", "required" });
        fields.Add("Surname", new string[]{ "Surname", "string", "", "", "required" });
		fields.Add("BirthDate", new string[]{ "Birth Date", "string", "date", "", "required" });
		fields.Add("GenderId", new string[]{ "GenderId", "integer", "int32", "", "required" });
        fields.Add("ExternalApplicantNumber", new string[]{ "External Applicant Number", "string", "", "", "required" });        
        fields.Add("JobOfferNumber", new string[]{ "Job Offer Number", "string", "", "", "required" });
        fields.Add("JobOfferTitle", new string[]{ "Job Offer Title", "string", "", "", "required" });
        fields.Add("ConfirmedStartDate", new string[]{ "Confirmed Date", "string", "date", "", "required" });
        fields.Add("OtherForenames", new string[]{ "Other First Name", "string", "", "", "" });
        fields.Add("PreviousSurname", new string[]{ "Previous Surname", "string", "", "", "" });
        fields.Add("KnownAs", new string[]{ "Known As", "string", "", "", "" });
        fields.Add("HomeEmailAddress", new string[]{ "Home Email Address", "string", "", "", "" });
        fields.Add("WorkEmailAddress", new string[]{ "Work Email Address", "string", "", "", "" });
        fields.Add("WorkTelephoneNumber", new string[]{ "Work Telephone Number", "string", "", "", "" });
        fields.Add("HomeTelephoneNumber", new string[]{ "Home Telephone Number", "string", "", "", "" });
        fields.Add("MobileTelephoneNumber", new string[]{ "Mobile Telephone Number", "string", "", "", "" });
        fields.Add("LineOne", new string[]{ "Address Line One", "string", "", "", "" });
		fields.Add("LineTwo", new string[]{ "Address Line Two", "string", "", "", "" });
		fields.Add("LineThree", new string[]{ "Address Line Three", "string", "", "", "" });
        fields.Add("LineFour", new string[]{ "Address Line Four", "string", "", "", "" });
        fields.Add("PostCode", new string[]{ "Post Code", "string", "", "", "" });
		fields.Add("AddressMunicipality", new string[]{ "Municipality", "string", "", "", "" });
		fields.Add("Region", new string[]{ "Region", "string", "", "", "" });
        fields.Add("AddressCountryId", new string[]{ "AddressCountryId", "integer", "int32", "", "" });
        fields.Add("PayGroupId", new string[]{ "PayGroupId", "integer", "int32", "", "" });
        fields.Add("PostId", new string[]{ "PostId", "integer", "int32", "", "" });
        fields.Add("GradeId", new string[]{ "GradeId", "integer", "int32", "", "" });
        fields.Add("LocationId", new string[]{ "LocationId", "integer", "int32", "", "" });
        fields.Add("StartReasonId", new string[]{ "StartReasonId", "integer", "int32", "", "" });
        fields.Add("PositionStatusId", new string[]{ "PositionStatusId", "integer", "int32", "", "" });
        fields.Add("WorkPatternId", new string[]{ "WorkPatternId", "integer", "int32", "", "" });
        fields.Add("ServiceConditionId", new string[]{ "ServiceConditionId", "integer", "int32", "", "" });
        fields.Add("ContractHours", new string[]{ "Contract Hours", "number", "float", "", "" });
        fields.Add("FixedSalary", new string[]{ "Fixed Salary", "number", "float", "", "" });
        fields.Add("SalaryPayElementId", new string[]{ "SalaryPayElementId", "integer", "int32", "", "" });
        fields.Add("HourlyRate", new string[]{ "Hourly Rate", "number", "float", "", "" });
        fields.Add("HourlyPayElementId", new string[]{ "HourlyPayElementId", "integer", "int32", "", "" });
        fields.Add("ProjectedEndDate", new string[]{ "Projected End Date", "string", "date", "", "" });
        fields.Add("ProjectedEndReasonId", new string[]{ "ProjectedEndReasonId", "number", "float", "", "" });
        fields.Add("CountryOfBirthId", new string[]{ "CountryOfBirthId", "integer", "int32", "", "" });
        fields.Add("NationalInsuranceNo", new string[]{ "National Insurance Number", "string", "", "", "" });      
        fields.Add("NationalityCitizenshipId", new string[]{ "NationalityCitizenshipId", "integer", "int32", "", "" });
        fields.Add("PassportNumber", new string[]{ "Passport Number", "string", "", "", "" });
        fields.Add("PassportExpiryDate", new string[]{ "Passport Expiry Date", "string", "date", "", "" });   
        fields.Add("PassportCountryOfIssueId", new string[]{ "PassportCountryOfIssueId", "integer", "int32", "", "" });
        fields.Add("VisaNumber", new string[]{ "Visa Number", "string", "", "", "" });
        fields.Add("VisaIssueDate", new string[]{ "Visa Issue Date", "string", "date", "", "" }); 
        fields.Add("VisaExpiryDate", new string[]{ "Visa Expiry Date", "string", "date", "", "" });   
        fields.Add("VisaCountryIssueId", new string[]{ "VisaCountryIssueId", "integer", "int32", "", "" });
        fields.Add("BranchSortCode", new string[]{ "Branch Sort Code", "string", "", "", "" });
        fields.Add("AccountNo", new string[]{ "Account Number", "string", "", "", "" });        
        fields.Add("AccountName", new string[]{ "Account Name", "string", "", "", "" });
        fields.Add("AccountRollNo", new string[]{ "Account Roll Number", "string", "", "", "" });
        fields.Add("AccountIBAN", new string[]{ "Account IBAN", "string", "", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
    }

    private JObject GetSharedParentalLeaveRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
        fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
		fields.Add("ExpDueDate", new string[]{ "Expected Due Date", "string", "date", "", "required" });
		fields.Add("AbsStartDate", new string[]{ "Absence Start Date", "string", "date", "", "required" });        
		fields.Add("AbsEndDate", new string[]{ "Absence End Date", "string", "date", "", "" });
        fields.Add("AbsType", new string[]{ "Absence Type", "integer", "int32", "", "required" });        
		fields.Add("ActDate", new string[]{ "Birth/Placement Date", "string", "date", "", "" });
        fields.Add("MatAdpt", new string[]{ "Maternity/Adoption", "string", "", "MATERNITY|ADOPTION", "" });  
        fields.Add("MaxLvWks", new string[]{ "Max Leave Weaks", "string", "", "", "" });         
        fields.Add("RtwFlag", new string[]{ "Return To Work", "boolean", "", "", "" });
        fields.Add("CurtDate", new string[]{ "Curtailment Date", "string", "date", "", "" });
        fields.Add("IntRtwDate", new string[]{ "Interim RTW Date", "string", "date", "", "" });
        fields.Add("MppAppEndDate", new string[]{ "MPP-APP End Date", "string", "date", "", "" });
        fields.Add("LvStat", new string[]{ "LV Status", "string", "", "Y|N", "" });  
        fields.Add("MothDec", new string[]{ "Mother Deceased", "string", "", "Y|N", "" });  
        fields.Add("PartDec", new string[]{ "Partner Deceased", "string", "", "Y|N", "" });  
        fields.Add("MortInd", new string[]{ "Mortality Indicator", "string", "", "Y|N", "" });  
        fields.Add("NormWkPayAmt", new string[]{ "Normal Week Pay Amount", "string", "", "Y|N", "" });  
        fields.Add("NotPayShpl", new string[]{ "Not Pay Shpl", "string", "", "Y|N", "" });  
        fields.Add("StartYrPrd", new string[]{ "Start Year Prd", "string", "", "", "" }); 
        fields.Add("PayMthd", new string[]{ "Pay Method", "string", "", "", "" }); 
        fields.Add("CntHrs", new string[]{ "Cnt Hrs", "string", "", "", "" }); 
        fields.Add("DiffDailyRate", new string[]{ "Diff. Daily Rate", "string", "", "", "" }); 
        fields.Add("PartForename", new string[]{ "Part First Name", "string", "", "", "" }); 
        fields.Add("PartSurname", new string[]{ "Part Surname", "string", "", "", "" }); 
        fields.Add("PartNiNo", new string[]{ "Part NI No", "string", "", "", "" }); 
        fields.Add("PartAddr1", new string[]{ "Part Address 1", "string", "", "", "" }); 
        fields.Add("PartAddr2", new string[]{ "Part Address 3", "string", "", "", "" }); 
        fields.Add("PartAddr3", new string[]{ "Part Address 3", "string", "", "", "" }); 
        fields.Add("PartAddr4", new string[]{ "Part Address 4", "string", "", "", "" }); 
        fields.Add("PartTown", new string[]{ "Part Town", "string", "", "", "" }); 
        fields.Add("PartCounty", new string[]{ "Part County", "string", "", "", "" }); 
        fields.Add("PartCountry", new string[]{ "Part Country", "string", "", "", "" }); 
        fields.Add("PartPstCd", new string[]{ "Part Post Code", "string", "", "", "" }); 
        fields.Add("PartErName", new string[]{ "Part ER Name", "string", "", "", "" }); 
        fields.Add("PartErAddr1", new string[]{ "Part ER Address 1", "string", "", "", "" }); 
        fields.Add("PartErAddr2", new string[]{ "Part ER Address 3", "string", "", "", "" }); 
        fields.Add("PartErAddr3", new string[]{ "Part ER Address 3", "string", "", "", "" }); 
        fields.Add("PartErAddr4", new string[]{ "Part ER Address 4", "string", "", "", "" }); 
        fields.Add("PartErTown", new string[]{ "Part ER Town", "string", "", "", "" }); 
        fields.Add("PartErCounty", new string[]{ "Part ER County", "string", "", "", "" }); 
        fields.Add("PartErCountry", new string[]{ "Part ER Country", "string", "", "", "" }); 
        fields.Add("PartErPstCd", new string[]{ "Part ER Post Code", "string", "", "", "" }); 
        fields.Add("AdptAgcyName", new string[]{ "Adpt Agency Name", "string", "", "", "" }); 
        fields.Add("AdptAgcyAddr1", new string[]{ "Adpt Agency Address 1", "string", "", "", "" }); 
        fields.Add("AdptAgcyAddr2", new string[]{ "Adpt Agency Address 3", "string", "", "", "" }); 
        fields.Add("AdptAgcyAddr3", new string[]{ "Adpt Agency Address 3", "string", "", "", "" }); 
        fields.Add("AdptAgcyAddr4", new string[]{ "Adpt Agency Address 4", "string", "", "", "" }); 
        fields.Add("AdptAgcyTown", new string[]{ "Adpt Agency Town", "string", "", "", "" }); 
        fields.Add("AdptAgcyCounty", new string[]{ "Adpt Agency County", "string", "", "", "" }); 
        fields.Add("AdptAgcyCountry", new string[]{ "Adpt Agency Country", "string", "", "", "" }); 
        fields.Add("AdptAgcyPstCd", new string[]{ "Adpt Agency Post Code", "string", "", "", "" }); 
        fields.Add("StatPaid", new string[]{ "Stat Paid", "integer", "int32", "", "" });
        fields.Add("OccPaid", new string[]{ "Occ Paid", "integer", "int32", "", "" });
        fields.Add("StatPaidTo", new string[]{ "Stat Paid To", "integer", "int32", "", "" });
        fields.Add("OccPaidTo", new string[]{ "Occ Paid To", "integer", "int32", "", "" });
        fields.Add("PaidToDate", new string[]{ "Paid To Date", "string", "date", "", "" });
        fields.Add("ServStartDate", new string[]{ "Serv Start Date", "string", "date", "", "" });
        fields.Add("CerRecDate", new string[]{ "Cer Rec Date", "string", "date", "", "" });
        fields.Add("FinRtwDate", new string[]{ "Fin Rtw Date", "string", "date", "", "" });
        fields.Add("ExpLvDate", new string[]{ "Exp LV Date", "string", "date", "", "" });
        fields.Add("RevocDate", new string[]{ "Revoc Date", "string", "date", "", "" });
        fields.Add("QwStartDate", new string[]{ "QW Start Date", "string", "date", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
    }

    private JObject GetStructureUnitRequestBody(String operation)
    {       
        string[] mandatoryRequiredFields = { "Code" };

        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("Code", new string[]{ "Code", "string", "", "", "required" });
        fields.Add("StrShortDesc", new string[]{ "Short Description", "string", "", "", "required" });
        fields.Add("StrLongDesc", new string[]{ "Long Description", "string", "", "", "required" });
        fields.Add("StrObsoleteDate", new string[]{ "Obsolete Date", "string", "date", "", "" }); 
        fields.Add("StrSlipMsgId", new string[]{ "Slip Message Id", "integer", "int32", "", "" });
        fields.Add("StrEmployer", new string[]{ "Employer", "string", "", "", "" });  
        fields.Add("StrOnCostPerc", new string[]{ "On Cost Percentage", "integer", "int32", "", "" });
        fields.Add("StrOnCostInd", new string[]{ "On Cost Indicator", "string", "", "", "" });
        fields.Add("StrEmpCode", new string[]{ "Employee Code", "string", "", "", "" });
        fields.Add("StrErsCode", new string[]{ "ERS Code", "string", "", "", "" });
        fields.Add("StrVatCode", new string[]{ "VAT Code", "string", "", "", "" });
        fields.Add("StrOnCostCode", new string[]{ "On Cost Code", "string", "", "", "" });
        fields.Add("StrErsContraCode", new string[]{ "ERS Contra Code", "string", "", "", "" });
        fields.Add("StrOnCostContraCode", new string[]{ "On Cost Contra Code", "string", "", "", "" });
        fields.Add("StrEesCostSpreadMethod", new string[]{ "EES Cost Spread Method", "string", "", "", "" });
        fields.Add("BankSortCode", new string[]{ "Bank Sort Code", "string", "", "", "" });
        fields.Add("AccountNo", new string[]{ "Account Number", "string", "", "", "" });
        fields.Add("AccountName", new string[]{ "Account Name", "string", "", "", "" });
        fields.Add("BankAccountType", new string[]{ "Bank Account Type", "string", "", "", "" });
        fields.Add("AccountRollNo", new string[]{ "Account Roll Number", "string", "", "", "" });
        fields.Add("BankCountry", new string[]{ "Bank Country", "string", "", "", "" });
        fields.Add("AccountIBAN", new string[]{ "Account IBAN", "string", "", "", "" });
        fields.Add("BICcode", new string[]{ "BIC Code", "string", "", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, "PATCH".Equals(operation, StringComparison.OrdinalIgnoreCase) ? 
            mandatoryRequiredFields : fieldsJObject.Value.ToArray());
    }

    private JObject GetTemporaryPayElementsRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
		fields.Add("PayElementId", new string[]{ "PayElementId", "integer", "int32", "", "required" });
		fields.Add("BatchId", new string[]{ "Batch", "string", "", "", "required" });
		fields.Add("StartDate", new string[]{ "Start Date", "string", "date", "", "required" });
		fields.Add("EndDate", new string[]{ "End Date", "string", "date", "", "" });
		fields.Add("PostId", new string[]{ "PostId", "integer", "int32", "", "" });
		fields.Add("CostCentreId", new string[]{ "CostCentreId", "integer", "int32", "", "" });
        fields.Add("Amount", new string[]{ "Amount", "number", "float", "", "" });
		fields.Add("ErsAmount", new string[]{ "ERS Amount", "number", "float", "", "" });
        fields.Add("Rate", new string[]{ "Rate", "number", "float", "", "" });
        fields.Add("AdvancePeriods", new string[]{ "Units", "integer", "int32", "", "" });
        fields.Add("Units", new string[]{ "Units", "integer", "int32", "", "" });
		fields.Add("TaxPeriod", new string[]{ "Tax Period", "string", "", "", "" });
		fields.Add("TaxYear", new string[]{ "Tax Year", "string", "", "", "" });
        fields.Add("OverrideAction", new string[]{ "Override Action", "string", "", "", "" });
        fields.Add("WorkerAmountTableCode", new string[]{ "Worker Amount Table Code", "string", "", "", "" });
        fields.Add("WorkerAmountRowCoord", new string[]{ "Worker Amount Row Coord", "string", "", "", "" });
        fields.Add("WorkerAmountColCoord", new string[]{ "Worker Amount Col Coord", "string", "", "", "" });
        fields.Add("EmployerAmountTableCode", new string[]{ "Employer Amount Table Code", "string", "", "", "" });
        fields.Add("EmployerAmountRowCoord", new string[]{ "Employer Amount Row Coord", "string", "", "", "" });
        fields.Add("EmployerAmountColCoord", new string[]{ "Employer Amount Col Coord", "string", "", "", "" });
        fields.Add("UnitsTableCode", new string[]{ "Units Table Code", "string", "", "", "" });
        fields.Add("UnitsRowCoord", new string[]{ "Units Row Coord", "string", "", "", "" });
        fields.Add("UnitsColCoord", new string[]{ "Units Col Coord", "string", "", "", "" });
        fields.Add("RateTableCode", new string[]{ "Rate Table Code", "string", "", "", "" });
        fields.Add("RateRowCoord", new string[]{ "Rate Row Coord", "string", "", "", "" });
        fields.Add("RateColCoord", new string[]{ "Rate Col Coord", "string", "", "", "" });

        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
    }

    private JObject GetWorkPatternsRequestBody(String operation)
    {       
        string[] mandatoryRequiredFields = { "Code", "WorkDays" };

        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("Code", new string[]{ "Code", "string", "", "", "required" });
        fields.Add("ShortDescription", new string[]{ "Short Description", "string", "", "", "required" });
        fields.Add("LongDescription", new string[]{ "Long Description", "string", "", "", "required" });
        fields.Add("StartDate", new string[]{ "Start Date", "string", "date", "", "required" }); 
        fields.Add("WeeklyHours", new string[]{ "Weekly Hours", "string", "", "", "required" });
        fields.Add("WeeksPerYear", new string[]{ "Weeks Per Year", "string", "", "", "required" });  
        fields.Add("ObsoleteDate", new string[]{ "Obsolete Date", "string", "date", "", "" });
        fields.Add("MyViewRota", new string[]{ "MyView Rota", "string", "", "", "" });
        fields.Add("ClearWorkSession", new string[]{ "Clear Work Session", "string", "", "", "" });

        Dictionary<string, string[]> workDaysFields = new Dictionary<string, string[]>();
        workDaysFields.Add("WorkDayNo", new string[]{ "Work Day Number", "string", "", "", "required" });
		workDaysFields.Add("WorkDayFlag", new string[]{ "Work Day Flag", "string", "", "", "required" });

        Dictionary<string, string[]> sessionsFields = new Dictionary<string, string[]>();
        sessionsFields.Add("DayNo", new string[]{ "Day Number", "string", "", "", "required" });
		sessionsFields.Add("StartTime", new string[]{ "Start Time", "string", "", "", "required" });
        sessionsFields.Add("EndTime", new string[]{ "End Time", "string", "", "", "required" });

        var fieldsJObject = getFieldsJObject(fields).First();
        var fieldsObject = fieldsJObject.Key;
        var workDaysObject = getFieldsJObject(workDaysFields).First();
        var requiredWorkDayFieldsJArray = new JArray(workDaysObject.Value.ToArray());
        var sessionsObject = getFieldsJObject(sessionsFields).First();
        var requiredSessionsFieldsJArray = new JArray(sessionsObject.Value.ToArray());
        
        var workDays = new JObject
		{
			["WorkDays"] = new JObject
			{
                ["type"] = "array",
                ["items"] = new JObject
                {
                    ["type"] = "object",
                    ["properties"] = workDaysObject.Key,
                    ["required"] = requiredWorkDayFieldsJArray,
                },
                ["description"] = "Work Days",
			},
		};
        fieldsJObject.Value.Add("WorkDays");
        fieldsObject.Merge(workDays);

        var sessions = new JObject
		{
			["Sessions"] = new JObject
			{
                ["type"] = "array",
                ["items"] = new JObject
                {
                    ["type"] = "object",
                    ["properties"] = sessionsObject.Key,
                    ["required"] = requiredSessionsFieldsJArray,
                },
                ["description"] = "Sessions",
			},
		};
        fieldsJObject.Value.Add("Sessions");
        fieldsObject.Merge(sessions);
       
        return getRequstJObject(fieldsObject, "PATCH".Equals(operation, StringComparison.OrdinalIgnoreCase) ? 
            mandatoryRequiredFields : fieldsJObject.Value.ToArray());
    }

    private JObject GetWorkerDrivingLicencesRequestBody(String operation)
    {       
        string[] mandatoryRequiredFields = { "WorkerId", "EffectiveDate", "DrivingLicenceNumber", "DrivingLicenceCountryId" };

        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
        fields.Add("EffectiveDate", new string[]{ "Effective Date", "string", "date", "", "required" });
        fields.Add("DrivingLicenceNumber", new string[]{ "Driving Licence Number", "string", "", "", "required" });
        fields.Add("DrivingLicenceCountryId", new string[]{ "DrivingLicenceCountryId", "integer", "int32", "", "required" });
        fields.Add("DrivingLicenceRegionId", new string[]{ "DrivingLicenceRegionId", "integer", "int32", "", "" });
        fields.Add("ExpiryDate", new string[]{ "Expiry Date", "string", "date", "", "" });
        fields.Add("LicenceClasses", new string[]{ "Licence Classes", "array", "string", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, "PATCH".Equals(operation, StringComparison.OrdinalIgnoreCase) ? 
            mandatoryRequiredFields : fieldsJObject.Value.ToArray());
    }

    private JObject GetWorkerNICategoriesRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
        fields.Add("StartDate", new string[]{ "Start Date", "string", "date", "", "required" });
        fields.Add("NICategoryId", new string[]{ "NICategoryId", "integer", "int32", "", "required" });
        fields.Add("PostId", new string[]{ "PostId", "integer", "int32", "", "" });
        fields.Add("nicategoryId", new string[]{ "nicategoryId", "integer", "int32", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
    }

    private JObject GetWorkerPRSIDetailsRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
        fields.Add("EffectiveDate", new string[]{ "Effective Date", "string", "date", "", "required" });
        fields.Add("PRSIClassId", new string[]{ "PRSIClassId", "integer", "int32", "", "required" });
        fields.Add("IsLevyExempt", new string[]{ "Levy Exempt", "boolean", "", "", "" });
        fields.Add("IsErsExempt", new string[]{ "ERS Exempt", "boolean", "", "", "" });
        fields.Add("IsExpatEmployeeExempt", new string[]{ "Expat Employee Exempt", "boolean", "", "", "" });
        fields.Add("IsCommunityParticipant", new string[]{ "Community Participant", "boolean", "", "", "" });
        fields.Add("PRSIExemptionReasonId", new string[]{ "PRSIClassId", "integer", "int32", "", "" });
        fields.Add("prsiclassId", new string[]{ "prsiclassId", "integer", "int32", "", "" });     
        fields.Add("prsiexemptionReasonId", new string[]{ "prsiexemptionReasonId", "integer", "int32", "", "" });      
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
    }

    private JObject GetWorkerParentalLeaveRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
        fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
		fields.Add("ExpectedDueDate", new string[]{ "Expected Due Date", "string", "date", "", "required" });
		fields.Add("PayStartDate", new string[]{ "Pay Start Date", "string", "date", "", "required" });
		fields.Add("AbsenceTypeId", new string[]{ "AbsenceTypeId", "integer", "int32", "", "required" });
        fields.Add("LoadStatus", new string[]{ "Load Status", "string", "", "", "" });  
		fields.Add("ActualDate", new string[]{ "Actual Date", "string", "date", "", "" });   
		fields.Add("OccPayStartDate", new string[]{ "Occ PayStart Date", "string", "date", "", "" });     
        fields.Add("StartYearPeriod", new string[]{ "Start Year Period", "string", "", "", "" });
        fields.Add("PlPayMethodId", new string[]{ "PlPayMethodId", "integer", "int32", "", "" });
        fields.Add("ContractHours", new string[]{ "Contract Hours", "number", "float", "", "" });
        fields.Add("ServiceStartDate", new string[]{ "Service Start Date", "string", "date", "", "" });
        fields.Add("Matb1FormReceivedDate", new string[]{ "Matb1 Form Received Date", "string", "date", "", "" });
        fields.Add("MotherDeceased", new string[]{ "Mother Deceased", "boolean", "", "", "" });
        fields.Add("PartnerDeceased", new string[]{ "Partner Deceased", "boolean", "", "", "" });
        fields.Add("MortalityIndId", new string[]{ "Mortality Indicator Id", "integer", "int32", "", "" });
        fields.Add("RtwFlag", new string[]{ "Return To Work", "boolean", "", "", "" });
        fields.Add("CurtailmentDate", new string[]{ "Curtailment Date", "string", "date", "", "" });
        fields.Add("InterimRtwDate", new string[]{ "Interim Rtw Date", "string", "date", "", "" });
        fields.Add("MppAppEndDate", new string[]{ "Mpp App End Date", "string", "date", "", "" });
        fields.Add("FinalRtwDate", new string[]{ "Final Rtw Date", "string", "date", "", "" });
        fields.Add("PaternityRtwDate", new string[]{ "Paternity Rtw Date", "string", "date", "", "" });
        fields.Add("ExpectedLeaveDate", new string[]{ "Expected Leave Date", "string", "date", "", "" });
        fields.Add("RevocationDate", new string[]{ "Revocation Date", "string", "date", "", "" });
        fields.Add("AverageWkPayAmt", new string[]{ "Average Week Pay Amount", "number", "float", "", "" });
        fields.Add("NormalWkPayAmt", new string[]{ "Normal Week Pay Amount", "number", "float", "", "" });        
        fields.Add("StatLump", new string[]{ "Stat Lump", "integer", "int32", "", "" });
        fields.Add("OccLump", new string[]{ "Occ Lump", "integer", "int32", "", "" });
        fields.Add("QwStart", new string[]{ "Qw Start", "string", "date", "", "" });
        fields.Add("StatPaid", new string[]{ "Stat Paid", "integer", "int32", "", "" });
        fields.Add("OccPaid", new string[]{ "Occ Paid", "integer", "int32", "", "" });
        fields.Add("PaidToDate", new string[]{ "Paid To Date", "string", "date", "", "" });
        fields.Add("StatPaidTo", new string[]{ "Stat Paid To", "integer", "int32", "", "" });
        fields.Add("OccPaidTo", new string[]{ "Occ Paid To", "integer", "int32", "", "" });
        fields.Add("DiffDailyRate", new string[]{ "Diff. Daily Rate", "boolean", "", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
    }

    private JObject GetWorkerPassportVisasRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
        fields.Add("PassportNumber", new string[]{ "Passport Number", "string", "", "", "" });
        fields.Add("PassportExpiryDate", new string[]{ "Passport Expiry Date", "string", "date", "", "" });
        fields.Add("PassportCountryIssueId", new string[]{ "PassportCountryIssueId", "integer", "int32", "", "" });
        fields.Add("PassportVerifiedDate", new string[]{ "Passport Verified Date", "string", "date", "", "" });
        fields.Add("VisaNumber", new string[]{ "Visa Number", "string", "", "", "" });
        fields.Add("VisaCountryIssueId", new string[]{ "VisaCountryIssueId", "integer", "int32", "", "" });
        fields.Add("VisaIssueDate", new string[]{ "Visa Issue Date", "string", "date", "", "" });
        fields.Add("VisaExpiryDate", new string[]{ "Visa Expiry Date", "string", "date", "", "" });
        fields.Add("VisaTierId", new string[]{ "VisaTierId", "integer", "int32", "", "" });
        fields.Add("VisaCategoryId", new string[]{ "VisaCategoryId", "integer", "int32", "", "" });
        fields.Add("VisaRestrictionsId", new string[]{ "VisaRestrictionsId", "string", "integer", "int32", "" });
        fields.Add("VisaVerifiedDate", new string[]{ "Visa Verified Date", "string", "date", "", "" });
        fields.Add("VisaUKEntryDate", new string[]{ "Visa UK Entry Date", "string", "date", "", "" });
        fields.Add("NationalityCitizenshipId", new string[]{ "NationalityCitizenshipId", "integer", "int32", "", "" });
        fields.Add("SecondNationalityCitizenshipId", new string[]{ "WorkerId", "integer", "int32", "", "" });
        fields.Add("CertificateOfSponsorshipNumber", new string[]{ "Certificate Of Sponsorship Number", "string", "", "", "" });
        fields.Add("CertificateOfSponsorshipIssueDate", new string[]{ "Certificate Of Sponsorship Issue Date", "string", "date", "", "" });
        fields.Add("CertificateOfSponsorshipExpiryDate", new string[]{ "Certificate Of Sponsorship Expiry Date", "string", "date", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
    }

    private JObject GetWorkerPensionSchemesRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
        fields.Add("PensionSchemeId", new string[]{ "PensionSchemeId", "integer", "int32", "", "required" });
        fields.Add("SchemeJoinedDate", new string[]{ "Scheme Joined Date", "string", "date", "", "" });
        fields.Add("SchemeLeftDate", new string[]{ "Scheme Left Date", "string", "date", "", "" });
        fields.Add("MemberNumber", new string[]{ "Member Number", "string", "", "", "" });
        fields.Add("MinimumPensGross", new string[]{ "Minimum Pens Gross", "string", "", "", "" });
        fields.Add("SchemeRefNumber", new string[]{ "Scheme Ref Number", "string", "", "", "" });
        fields.Add("SchemeLeaverReasonId", new string[]{ "PensionSchemeId", "integer", "int32", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
    }

    private JObject GetWorkerPostsRequestBody(String operation)
    {       
        string[] mandatoryRequiredFields = { "WorkerId", "PostId", "PostStartDate" };

        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
		fields.Add("PostId", new string[]{ "PostId", "integer", "int32", "", "required" });
		fields.Add("IsMainPost", new string[]{ "Main Post", "boolean", "", "", "" });
		fields.Add("PostStartDate", new string[]{ "Post Start Date", "string", "date", "", "required" });
		fields.Add("PostEndDate", new string[]{ "Post End Date", "string", "date", "", "" });
		fields.Add("PostJoinReasonId", new string[]{ "PostJoinReasonId", "integer", "int32", "", "required" });
		fields.Add("PostLeaveReasonId", new string[]{ "PostLeaveReasonId", "integer", "int32", "", "" });
        fields.Add("GradeId", new string[]{ "GradeId", "integer", "int32", "", "required" });
		fields.Add("GradeStartDate", new string[]{ "Grade Start Date", "string", "date", "", "required" });
        fields.Add("GradeEndDate", new string[]{ "Grade End Date", "string", "date", "", "" });
        fields.Add("GradeOverrideReasonId", new string[]{ "GradeOverrideReasonId", "integer", "int32", "", "" });
        fields.Add("GradeOverrideDate", new string[]{ "Grade Override Date", "String", "date", "", "" });
		fields.Add("GradeOverrideStep", new string[]{ "Grade Override Step", "string", "", "", "" });
		fields.Add("GradeReasonId", new string[]{ "GradeReasonId", "integer", "int32", "", "required" });
        fields.Add("GradeCurrentPoint", new string[]{ "Grade Current Point", "string", "", "", "" });
        fields.Add("GradeBasicSalary", new string[]{ "Grade Basic Salary", "string", "", "", "" });
        fields.Add("GradeCarryForward", new string[]{ "Grade Carry Forward", "boolean", "", "", "" });
        fields.Add("ServiceConditionId", new string[]{ "ServiceConditionId", "integer", "int32", "", "required" });
        fields.Add("ServiceConditionStartDate", new string[]{ "Service Condition Start Date", "string", "date", "", "required" });
        fields.Add("ServiceConditionEndDate", new string[]{ "Service Condition End Date", "string", "date", "", "" });
        fields.Add("PositionStatusId", new string[]{ "PositionStatusId", "integer", "int32", "", "required" });
        fields.Add("PositionStatusStartDate", new string[]{ "Position Status Start Date", "string", "date", "", "required" });
        fields.Add("PositionStatusEndDate", new string[]{ "Position Status End Date", "string", "date", "", "" });
        fields.Add("WorkPatternId", new string[]{ "WorkPatternId", "integer", "int32", "", "required" });
        fields.Add("WorkPatternStartDate", new string[]{ "Work Pattern Start Date", "string", "date", "", "required" });
        fields.Add("WorkPatternEndDate", new string[]{ "Work Pattern End Date", "string", "date", "", "" });
        fields.Add("WorkPatternStartDay", new string[]{ "Work Pattern Start Day", "integer", "int32", "", "required" });
        fields.Add("WorkPatternNoTimesFlag", new string[]{ "Work Pattern No Times Flag", "boolean", "", "", "" });
        fields.Add("WorkPatternReasonId", new string[]{ "WorkPatternReasonId", "integer", "int32", "", "required" });
        fields.Add("ContractHours", new string[]{ "Contract Hours", "integer", "int32", "", "required" });
        fields.Add("ContractHoursStartDate", new string[]{ "Contract Hours Start Date", "string", "date", "", "required" });
        fields.Add("ContractHoursEndDate", new string[]{ "Contract Hours End Date", "string", "date", "", "" });
        fields.Add("ContractHoursFTE", new string[]{ "Contract Hours FTE", "integer", "int32", "", "" });
        fields.Add("ContractHoursWeeksPerYear", new string[]{ "Contract Hours Weeks Per Year", "integer", "int32", "", "" });
        fields.Add("LocationId", new string[]{ "LocationId", "integer", "int32", "", "required" });
        fields.Add("LocationStartDate", new string[]{ "Location Start Date", "string", "date", "", "required" });
        fields.Add("LocationEndDate", new string[]{ "Location End Date", "string", "date", "", "" });
        fields.Add("LocationReasonId", new string[]{ "LocationReasonId", "integer", "int32", "", "required" });
        fields.Add("MultiHours", new string[]{ "Multi Hours", "integer", "int32", "", "" });
        fields.Add("MultiWeeks", new string[]{ "Multi Weeks", "integer", "int32", "", "" });
        fields.Add("OccupancyTypeId", new string[]{ "OccupancyTypeId", "integer", "int32", "", "" });
        fields.Add("ProjectedEndDate", new string[]{ "Projected End Date", "string", "date", "", "" });
        fields.Add("HesaFlag", new string[]{ "Hesa Flag", "boolean", "", "", "" });
        fields.Add("ApprenticeStartDate", new string[]{ "Apprentice End Date", "string", "date", "", "" });
        fields.Add("ApprenticeEndDate", new string[]{ "Apprentice End Date", "string", "date", "", "" });
        fields.Add("ApprenticeNIFlag", new string[]{ "Apprentice NI Flag", "boolean", "", "", "" });

        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, "PATCH".Equals(operation, StringComparison.OrdinalIgnoreCase) ? 
            mandatoryRequiredFields : fieldsJObject.Value.ToArray());
    }

    private JObject GetWorkerRelationshipsRequestBody(String operation)
    {       
        string[] mandatoryRequiredFields = { "WorkerId", "RelationshipTypeId" };

        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
		fields.Add("RelationshipTypeId", new string[]{ "RelationshipTypeId", "integer", "int32", "", "required" });
        fields.Add("RelationshipWorkerId", new string[]{ "RelationshipWorkerId", "integer", "int32", "", "" });
		fields.Add("WorkerRelationshipId", new string[]{ "WorkerRelationshipId", "integer", "int32", "", "" });
        fields.Add("IsPartner", new string[]{ "Is Partner", "string", "", "", "" });
        fields.Add("IsEmergencyContactRestricted", new string[]{ "Is Emergency Contact Restricted", "string", "", "", "" });
        fields.Add("Surname", new string[]{ "Surname", "string", "", "", "" });
        fields.Add("RelationshipDetail", new string[]{ "Relationship Detail", "string", "", "", "" });
        fields.Add("BirthName", new string[]{ "Birth Name", "string", "", "", "" });
		fields.Add("TitleId", new string[]{ "TitleId", "integer", "int32", "", "" });
        fields.Add("FirstForename", new string[]{ "First Forename", "string", "", "", "" });
        fields.Add("OtherForenames", new string[]{ "Other Forenames", "string", "", "", "" });
        fields.Add("PreviousSurname", new string[]{ "Previous Surname", "string", "", "", "" });
        fields.Add("KnownAsForename", new string[]{ "Known As Forename", "string", "", "", "" });
        fields.Add("KnownAsSurname", new string[]{ "Known As Surname", "string", "", "", "" });
        fields.Add("GenderId", new string[]{ "GradeId", "integer", "int32", "", "" });
		fields.Add("BirthDate", new string[]{ "Birth Date", "string", "date", "", "" });
        fields.Add("DeathDate", new string[]{ "Death Date", "string", "date", "", "" });
        fields.Add("CountryOfBirthId", new string[]{ "CountryOfBirthId", "integer", "int32", "", "" });
		fields.Add("NationalityCitizenshipId", new string[]{ "NationalityCitizenshipId", "integer", "int32", "", "required" });
        fields.Add("NationalInsuranceNo", new string[]{ "National Insurance Number", "string", "", "", "" });

        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, "PATCH".Equals(operation, StringComparison.OrdinalIgnoreCase) ? 
            mandatoryRequiredFields : fieldsJObject.Value.ToArray());
    }

    private JObject GetWorkerTaxCodeHistoriesRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
        fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });        
		fields.Add("EffectiveDate", new string[]{ "Effective Date", "string", "date", "", "required" });
		fields.Add("TaxCodeSourceId", new string[]{ "TaxCodeSourceId", "integer", "int32", "", "required" });        
		fields.Add("TaxBasis", new string[]{ "Tax Basis", "string", "", "", "" });
		fields.Add("TaxCode", new string[]{ "Tax Code", "string", "", "", "required" });
        fields.Add("TaxLetter", new string[]{ "Tax Letter", "string", "", "", "" });        
		fields.Add("TaxCredit", new string[]{ "Tax Credit", "number", "float", "", "" }); 
        fields.Add("CutOff", new string[]{ "Cut Off", "integer", "int32", "", "" }); 
        fields.Add("Year", new string[]{ "Year", "string", "", "", "" });   
        fields.Add("Exempt", new string[]{ "Exempt", "string", "", "", "" });   
        fields.Add("Emergency", new string[]{ "Emergency", "integer", "int32", "", "" }); 
		fields.Add("ExclusionOrderEffectiveDate", new string[]{ "Exclusion Order Effective Date", "string", "date", "", "" });

        Dictionary<string, string[]> taxCodeP45DetailFields = new Dictionary<string, string[]>();
        taxCodeP45DetailFields.Add("PreviousPay", new string[]{ "Previous Pay", "number", "float", "", "" });
		taxCodeP45DetailFields.Add("PreviousTax", new string[]{ "Previous Tax", "number", "float", "", "" });
		taxCodeP45DetailFields.Add("P45LeaveDate", new string[]{ "P45 Leave Date", "string", "date", "", "" });
        taxCodeP45DetailFields.Add("P45LeaveYear", new string[]{ "P45 Leave Year", "string", "", "", "" });
        taxCodeP45DetailFields.Add("P45LeavePeriod", new string[]{ "P45 Leave Period", "integer", "int32", "", "" });
		taxCodeP45DetailFields.Add("P45LeaveFrequency", new string[]{ "P45 Leave Frequency", "string", "", "", "" });
		taxCodeP45DetailFields.Add("P46StatementClass", new string[]{ "P46 Statement Class", "string", "", "", "" });
		taxCodeP45DetailFields.Add("P45TaxCode", new string[]{ "P45 Tax Code", "string", "", "", "" });
		taxCodeP45DetailFields.Add("P45TaxBasis", new string[]{ "P45 Tax Basis", "string", "", "", "" });
		taxCodeP45DetailFields.Add("P45PayEOffice", new string[]{ "P45 Pay EOffice", "string", "", "", "" });
		taxCodeP45DetailFields.Add("P45PayERef", new string[]{ "P45 Pay ERef", "string", "", "", "" });
		taxCodeP45DetailFields.Add("P45EarlierYear", new string[]{ "P45 Earlier Year", "boolean", "", "", "" });
		taxCodeP45DetailFields.Add("StudentLoan", new string[]{ "Student Loan", "boolean", "", "", "" });
		taxCodeP45DetailFields.Add("StudentLoan2", new string[]{ "Student Loan 2", "boolean", "", "", "" });
		taxCodeP45DetailFields.Add("StudentLoan4", new string[]{ "Student Loan 4", "boolean", "", "", "" });
		taxCodeP45DetailFields.Add("PostGraduateLoan1", new string[]{ "Post Graduate Loan1", "boolean", "", "", "" });
		taxCodeP45DetailFields.Add("SuspendEDI", new string[]{ "Suspend EDI", "boolean", "", "", "" });
        taxCodeP45DetailFields.Add("P45StarterDeclarationId", new string[]{ "P45StarterDeclarationId", "integer", "int32", "", "" });
		taxCodeP45DetailFields.Add("CitizenEEA", new string[]{ "Citizen EEA", "boolean", "", "", "" });
		taxCodeP45DetailFields.Add("EPM6Member", new string[]{ "EPM6 Member", "boolean", "", "", "" });
        taxCodeP45DetailFields.Add("P45PrevAnnPen", new string[]{ "P45 Prev Ann Pen", "integer", "int32", "", "" });
		taxCodeP45DetailFields.Add("P45BereavedInd", new string[]{ "P45 Bereaved Indicator", "boolean", "", "", "" });
		taxCodeP45DetailFields.Add("P45AlsoEmployed", new string[]{ "P45 Also Employed", "boolean", "", "", "" });
        taxCodeP45DetailFields.Add("IEP45PreviousPay", new string[]{ "IE P45 Previous Pay", "integer", "int32", "", "" });
        taxCodeP45DetailFields.Add("IEP45PreviousTax", new string[]{ "IE P45 Previous Tax", "integer", "int32", "", "" });

        Dictionary<string, string[]> p45StarterDeclarationFields = new Dictionary<string, string[]>();
        p45StarterDeclarationFields.Add("Code", new string[]{ "P45 Starter Code", "string", "", "", "" });

        Dictionary<string, string[]> taxCodeSourceFields = new Dictionary<string, string[]>();
        taxCodeSourceFields.Add("Code", new string[]{ "Tax Source Code", "string", "", "", "" });

        var fieldsJObject = getFieldsJObject(fields).First();
        var fieldsObject = fieldsJObject.Key;
        var p45StarterDeclarationObject = getFieldsJObject(p45StarterDeclarationFields).First();

        var p45StarterDeclaration = new JObject
		{
			["P45StarterDeclaration"] = new JObject
			{
                ["type"] = "object",
                ["properties"] = p45StarterDeclarationObject.Key,
                ["description"] = "P45 Starter Declaration",
			},
		};

        var taxCodeP45DetailObject = getFieldsJObject(taxCodeP45DetailFields).First();
        
        var taxCodeP45Detail = new JObject
		{
			["TaxCodeP45Detail"] = new JObject
			{
                ["type"] = "object",
                ["properties"] = taxCodeP45DetailObject.Key,
                ["description"] = "Tax Code P45 Detail",
			},
		};
        taxCodeP45Detail.Merge(p45StarterDeclaration);
        
        fieldsObject.Merge(taxCodeP45Detail);

        var taxCodeSourceObject = getFieldsJObject(taxCodeSourceFields).First();
        
        var taxCodeSource = new JObject
		{
			["TaxCodeSource"] = new JObject
			{
                ["type"] = "object",
                ["properties"] = taxCodeSourceObject.Key,
                ["description"] = "Tax Code Source",
			},
		};

        fieldsObject.Merge(taxCodeSource);
       
        return getRequstJObject(fieldsObject, fieldsJObject.Value.ToArray());
    }

    private JObject GetWorkerUSCDetailsRequestBody(String operation)
    {       
        Dictionary<string, string[]> fields = new Dictionary<string, string[]>();
		fields.Add("WorkerId", new string[]{ "WorkerId", "integer", "int32", "", "required" });
        fields.Add("EffectiveDate", new string[]{ "Effective Date", "string", "date", "", "required" });
        fields.Add("USCChangeSourceId", new string[]{ "USCChangeSourceId", "integer", "int32", "", "required" });
        fields.Add("IsUSCExempt", new string[]{ "USC Exempt", "boolean", "", "", "" });
        fields.Add("IsUSCAnnualisedCalc", new string[]{ "USC Annualised Calc", "boolean", "", "", "" });
        fields.Add("BroughtInUSCGross", new string[]{ "Brought In USC Gross", "string", "", "", "" });
        fields.Add("BroughtInUSCDeduction", new string[]{ "Brought In USC Deduction", "string", "", "", "" });
        fields.Add("USCSubsidiaryEmploymentIndicator", new string[]{ "USC Subsidiary Employment Indicator", "string", "", "", "" });
        fields.Add("USCCutOff1", new string[]{ "USC Cut Off 1", "string", "", "", "" });
        fields.Add("USCCutOff2", new string[]{ "USC Cut Off 3", "string", "", "", "" });
        fields.Add("USCCutOff3", new string[]{ "USC Cut Off 3", "string", "", "", "" });
        fields.Add("USCCutOff4", new string[]{ "USC Cut Off 4", "string", "", "", "" });
        fields.Add("USCCutOff5", new string[]{ "USC Cut Off 5", "string", "", "", "" });
        fields.Add("USCRate1", new string[]{ "USC Rate 1", "string", "", "", "" });
        fields.Add("USCRate2", new string[]{ "USC Rate 2", "string", "", "", "" });
        fields.Add("USCRate3", new string[]{ "USC Rate 3", "string", "", "", "" });
        fields.Add("USCRate4", new string[]{ "USC Rate 4", "string", "", "", "" });
        fields.Add("USCRate5", new string[]{ "USC Rate 5", "string", "", "", "" });
        
        var fieldsJObject = getFieldsJObject(fields).First();
       
        return getRequstJObject(fieldsJObject.Key, fieldsJObject.Value.ToArray());
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