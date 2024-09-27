public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        string advancedInfoHeader = "advancedInfo";
        bool advancedInfo = true;
        if (this.Context.Request.Headers.Contains(advancedInfoHeader))
        {
            string headerValue = this.Context.Request.Headers.GetValues(advancedInfoHeader).FirstOrDefault(p => !string.IsNullOrEmpty(p));
            advancedInfo = (!string.IsNullOrEmpty(headerValue)) && bool.TryParse(headerValue, out bool value) ? value : false;
            this.Context.Request.Headers.Remove(advancedInfoHeader);
        }

        // Use the context to forward/send an HTTP request. Turn of caching
        var cacheControlHeader = new CacheControlHeaderValue() { NoStore = true, NoCache = true };
        this.Context.Request.Headers.CacheControl = cacheControlHeader;
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);

        try
        {
            // Do the transformation if the response was successful, otherwise return error responses as-is
            JObject surveyJson;
            string newResponceContent = responseString;
            bool contentChanged = false;

            if (this.Context.OperationId == "GetSurveyPublicationSchema")
            {
                var arrayResponse = JArray.Parse(responseString);
                surveyJson = arrayResponse.First as JObject;
                newResponceContent = this.GetSurveySchema(surveyJson, advancedInfo);
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
                case "RAD":
                    resultJson[$"Answer{order}"] = question["value"];
                    break;
                case "NUM":
                case "SCO":
                    resultJson[$"Answer{order}"] = question["value"];
                    resultJson[$"Answer{order}Str"] = Convert.ToString(question["value"]);
                    break;
                case "CHK":
                    JObject selectedOptions = new JObject();
                    resultJson[$"Answer{order}"] = selectedOptions;
                    JArray optionsJson = (JArray)question["options"];

                    bool addOtherOption = ((bool)((JValue)question["addOtherOption"]));
                    JObject otherOption = (JObject)question["otherOption"];

                    if (addOtherOption && otherOption != null)
                    {
                        optionsJson.Add(otherOption);
                    }

                    int optionNumber = 0;
                    List<string> selectedValues = new List<string>();

                    foreach (JObject optionJson in optionsJson)
                    {
                        optionNumber++;
                        string optionText = (string)optionJson["text"];
                        bool optionChecked = (bool)optionJson["checked"];
                        optionText = string.IsNullOrEmpty(optionText) && addOtherOption ? (string)optionJson["label"] : optionText;

                        selectedOptions[$"Option{optionNumber}Text"] = optionText;
                        selectedOptions[$"Option{optionNumber}Checked"] = optionChecked;
                        selectedOptions[$"Option{optionNumber}CheckedStr"] = (optionChecked) ? "Yes" : "No";
                        selectedOptions[$"Option{optionNumber}Id"] = optionJson["id"];

                        if (optionChecked) { selectedValues.Add(optionText); }
                    }

                    selectedOptions["SelectedOptions"] = string.Join(", ", selectedValues);
                    break;
                //case "IMG":
                case "SEC":
                    i--;
                    break;
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
    public string GetSurveySchema(JObject surveyJson, bool advancedInfo)
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
            int questionOrder = i;
            string questionText = question.Value<string>("name");
            string questionType = (string)question.SelectToken("parameterType.code");
            SetSimpleProperty(
                resultBodyJson,
                $"Question{questionOrder}",
                $"{questionText} (Question, {this.GetQuestionTypeDisplayName(questionType)})",
                $"Text of question {questionOrder}: {questionText} ({this.GetQuestionTypeDisplayName(questionType)})",
                "string", null);

            JObject answerJson = SetSimpleProperty(
                resultBodyJson,
                $"Answer{questionOrder}",
                $"{questionText} (Answer, {this.GetQuestionTypeDisplayName(questionType)})",
                $"Answer ({this.GetQuestionTypeForFlow(questionType)}) for question {questionOrder}: {questionText}",
                this.GetQuestionTypeForFlow(questionType), null);

            switch (questionType)
            {
                case "CHR":
                    break;
                case "NUM":
                case "SCO":
                    SetSimpleProperty(
                        resultBodyJson,
                        $"Answer{questionOrder}Str",
                        $"{questionText} (Answer, String value of {this.GetQuestionTypeDisplayName(questionType)})",
                        $"Answer (String) for question {questionOrder}: {questionText}",
                        "string", null);
                    break;
                case "RAD":
                    JObject radJson = new JObject();
                    SetSimpleProperty(radJson, "id", $"Selected choice id",
                        $"Question {questionOrder} (Single Choice). Id of selected option.{Environment.NewLine}Question text: {questionText}", "string", null);
                    SetSimpleProperty(radJson, "text", $"Selected choice title",
                        $"Question {questionOrder} (Single Choice). Text of selected option.{Environment.NewLine}Question text: {questionText}", "string", null);
                    answerJson["properties"] = radJson;
                    break;
                case "CHK":

                    JObject answerJson2Options = new JObject();
                    JArray optionsJson = (JArray)question["options"];
                    bool addOtherOption = ((bool)((JValue)question["addOtherOption"]));
                    JObject otherOption = (JObject)question["otherOption"];

                    if (addOtherOption && otherOption != null)
                    {
                        optionsJson.Add(otherOption);
                    }

                    SetSimpleProperty(answerJson2Options, $"SelectedOptions", $"Answer-Selected options(text)",
                        $"Question {questionOrder} (checkbox). Answer (string)-selected options, separated by comma{Environment.NewLine}Question text: {questionText}", "string", null);

                    int optionNumber = 0;

                    if (advancedInfo)
                    {
                        foreach (JObject optionJson in optionsJson)
                        {
                            optionNumber++;
                            string optionText = optionJson.Value<string>("text");
                            if (string.IsNullOrEmpty(optionText) && addOtherOption)
                            {
                                optionText = "Other option(it will  be set by user)";
                            }

                            string optionId = optionJson.Value<string>("id");
                            SetSimpleProperty(answerJson2Options, $"Option{optionNumber}Text", $"Option {optionNumber} text",
                                $"Question {questionOrder} (checkbox). Text of option {optionNumber}{Environment.NewLine}Option text: {optionText}{Environment.NewLine}Question text: {questionText}", "string", null);

                            SetSimpleProperty(answerJson2Options, $"Option{optionNumber}CheckedStr", $"Option {optionNumber} is selected (text)",
                                $"Question {questionOrder} (checkbox). Answer (string)-is option {optionNumber} selected{Environment.NewLine}Option text: {optionText}{Environment.NewLine}Question text: {questionText}", "string", null);

                            SetSimpleProperty(answerJson2Options, $"Option{optionNumber}Checked", $"Option {optionNumber} is selected (boolean)",
                                $"Question {questionOrder} (checkbox). Answer (boolean)-is option {optionNumber} selected{Environment.NewLine}Option text: {optionText}{Environment.NewLine}Question text: {questionText}", "boolean", null);

                            SetSimpleProperty(answerJson2Options, $"Option{optionNumber}Id", $"Option {optionNumber} id",
                                $"Question {questionOrder} (checkbox). Id of option {optionNumber}{Environment.NewLine}Id:{optionId}{Environment.NewLine}Option text: {optionText}{Environment.NewLine}Question text: {questionText}", "string", null);
                        }
                    }

                    answerJson["properties"] = answerJson2Options;
                    break;
                //case "IMG":
                case "SEC":
                    i--;
                    break;
                default:
                    break;
            }
        }

        firstLevelJson["properties"] = resultBodyJson;
        resultJson["schemaDefinition"] = firstLevelJson;
        return resultJson.ToString();
    }

    public JObject SetSimpleProperty(JObject parentObject, string paramName, string title, string description, string typeName, string format = null)
    {
        JObject jObject = new JObject();
        jObject["type"] = typeName;
        jObject["description"] = description;
        if (string.IsNullOrEmpty(format)) { } else { jObject["format"] = format; }
        if (string.IsNullOrEmpty(title)) { } else { jObject["title"] = title; }

        parentObject[paramName] = jObject;
        return jObject;
    }

    public string GetQuestionTypeForFlow(string questionType)
    {
        if (string.Equals(questionType, "CHR")) { return "string"; }
        if (string.Equals(questionType, "NUM")) { return "number"; }
        if (string.Equals(questionType, "SCO")) { return "integer"; }
        return "object";
    }

    public string GetQuestionTypeDisplayName(string questionType)
    {
        if (string.Equals(questionType, "CHR")) { return "Text"; }
        if (string.Equals(questionType, "NUM")) { return "Numeric"; }
        if (string.Equals(questionType, "SCO")) { return "Scale"; }
        if (string.Equals(questionType, "RAD")) { return "Single Choice"; }
        if (string.Equals(questionType, "CHK")) { return "Checkbox"; }
        return questionType;
    }
}
