public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Use the context to forward/send an HTTP request. Turn of caching
        var cacheControlHeader = new CacheControlHeaderValue(){NoStore=true, NoCache=true};
        this.Context.Request.Headers.CacheControl = cacheControlHeader;
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                
        string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);

        try 
        {
            JObject surveyJson;
            string newResponceContent = responseString;
            bool contentChanged = false;

            if (this.Context.OperationId == "GetSurveyPublicationSchema")
            {
                var arrayResponse = JArray.Parse(responseString);
                surveyJson = arrayResponse.First as JObject;
                newResponceContent = this.GetSurveySchema(surveyJson);
                contentChanged = true;
            }
            else if (this.Context.OperationId == "GetExecutionById")
            {
                newResponceContent = this.GetSurveyData(responseString);
                contentChanged = true;
            }
            else 
            { 
                //Don`t do any thing for other operations
            }

            if (contentChanged)
            {
                response.Content = CreateJsonContent(newResponceContent);
                response.StatusCode = HttpStatusCode.OK;
            }
        }
        catch
        {
            //Don`t to transformation
        }

        return response;
    }

    /// <summary>
    ///  The method transforms data from WebAPI to more plain structure, that suitable for Flows
    /// </summary>
    public string GetSurveyData(string surveyExecutionData)
    {
        JArray surveyExecutionArrayJson = JArray.Parse(surveyExecutionData);
        JObject surveyExecutionJson = (JObject)surveyExecutionArrayJson.First();
        JObject resultJson = new JObject();
        resultJson["Id"] = surveyExecutionJson["id"];
        resultJson["surveyId"] = surveyExecutionJson["surveyId"];
        resultJson["SurveyName"] = (string)surveyExecutionJson.SelectToken("$.surveyPublication.survey.name");
        resultJson["Date"] = surveyExecutionJson["executionDate"];
        resultJson["userId"] = surveyExecutionJson["userId"];
        List<JToken> questions = surveyExecutionJson.SelectTokens("$.pages..parameters[:10000]").ToList();

        int i = 0;
        foreach (JObject question in questions)
        {
            i++;
            int order = i;
            string questionText = question.Value<string>("name");
            string questionType = (string)question.SelectToken("parameterType.code");
            resultJson[$"Question{order}"] = questionText;

            switch (questionType)
            {
                case "CHR":
                case "NUM":
                case "SCO":
                case "RAD":
                    resultJson[$"Answer{order}"] = question["value"];
                    break;
                case "CHK":
                    JObject selectedOptions = new JObject();
                    resultJson[$"Answer{order}"] = selectedOptions;
                    JArray optionsJson = (JArray)question["options"];

                    int optionNumber = 0;
                    foreach (JObject optionJson in optionsJson)
                    {
                        optionNumber++;
                        selectedOptions[$"Option{optionNumber}Text"] = optionJson["text"];
                        selectedOptions[$"Option{optionNumber}Checked"] = optionJson["checked"];
                        selectedOptions[$"Option{optionNumber}Id"] = optionJson["id"];
                    }
                    break;
                //case "IMG":
                default:
                    break;
            }

        }

        string result = resultJson.ToString(); ;
        return result;
    }

    /// <summary>
    /// The method generate a schema of survey to support Flow Designer
    /// </summary>
    public string GetSurveySchema(JObject surveyJson)
    {
        JObject resultJson = new JObject();
        JObject firstLevelJson = new JObject();
        firstLevelJson["type"] = "object";
        firstLevelJson["description"] = $"Survey execution (object)";

        JObject resultBodyJson = new JObject();
        SetSimpleProperty(resultBodyJson, "Id", "Execution Id", "Execution Id", "string", null);
        SetSimpleProperty(resultBodyJson, "SurveyId", "Surver Id", "Surver Id", "string", null);
        SetSimpleProperty(resultBodyJson, "SurveyName", "Survey name", "Survey name", "string", null);
        SetSimpleProperty(resultBodyJson, "Date", "Execution date", "Execution date", "string", "date-time");
        SetSimpleProperty(resultBodyJson, "UserId", "User Id", "User Id", "string", null);

        List<JToken> questions = surveyJson.SelectTokens("$..parameters[:10000]").ToList();
        int i = 0;

        foreach (JObject question in questions)
        {
            i++;
            int order = i;
            string questionText = question.Value<string>("name");
            string questionType = (string)question.SelectToken("parameterType.code");
            SetSimpleProperty(resultBodyJson, $"Question{order}", $"Question {order} Text", questionText, "string", null);

            switch (questionType)
            {
                case "CHR":
                    SetSimpleProperty(resultBodyJson, $"Answer{order}", $"Question {order} Answer (Text)", questionText, "string", null);
                    break;
                case "NUM":
                    SetSimpleProperty(resultBodyJson, $"Answer{order}", $"Question {order} Answer (Numeric)", questionText, "number", null);
                    break;
                case "SCO":
                    SetSimpleProperty(resultBodyJson, $"Answer{order}", $"Question {order} Answer (Scale)", questionText, "integer", null);
                    break;
                case "RAD":
                    JObject answerJson1 = SetSimpleProperty(resultBodyJson, $"Answer{order}", $"Question {order} Answer (Single Choice)", questionText, "object", null);
                    JObject radJson = new JObject();
                    SetSimpleProperty(radJson, "id", $"Selected choice id", questionText, "string", null);
                    SetSimpleProperty(radJson, "text", $"Selected choice title", questionText, "string", null);
                    answerJson1["properties"] = radJson;
                    break;
                case "CHK":
                    JObject answerJson2 = SetSimpleProperty(resultBodyJson, $"Answer{order}", $"Question {order} Answer (Checkbox)", questionText, "object", null);

                    JObject answerJson2Options = new JObject();
                    JArray optionsJson = (JArray)question["options"];

                    int optionNumber = 0;
                    foreach (JObject optionJson in optionsJson)
                    {
                        optionNumber++;
                        string optionText = optionJson.Value<string>("text");
                        SetSimpleProperty(answerJson2Options, $"Option{optionNumber}Text", $"Option {optionNumber} text", $"{optionText}{Environment.NewLine}{questionText}", "string", null);
                        SetSimpleProperty(answerJson2Options, $"Option{optionNumber}Checked", $"Option {optionNumber} is selected",$"Is selected '{optionText}'{Environment.NewLine}{questionText}", "boolean", null);
                        SetSimpleProperty(answerJson2Options, $"Option{optionNumber}Id", $"Option {optionNumber} id", $"Option {optionNumber} ID{Environment.NewLine}{questionText}", "boolean", null);
                    }

                    answerJson2["properties"] = answerJson2Options;
                    break;
                //case "IMG":
                default:
                    break;
            }
        }

        firstLevelJson["properties"] = resultBodyJson;
        resultJson["schemaDefinition"] = firstLevelJson;
        return resultJson.ToString();
    }

    public JObject SetSimpleProperty(JObject parentObject, string paramName, string title, string description, string typeName, string format=null)
    {
        JObject jObject = new JObject();
        jObject["type"] = typeName;
        jObject["description"] = description;
        if (string.IsNullOrEmpty(format)) { } else {jObject["format"] = format; }
        if (string.IsNullOrEmpty(title)) { } else { jObject["title"] = title; }

        parentObject[paramName] = jObject;
        return jObject;
    }
}
