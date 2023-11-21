public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Check if the operation ID matches what is specified in the OpenAPI definition of the connector
        return await this.HandleForwardAndTransformOperation().ConfigureAwait(false);
    }
    private async Task<HttpResponseMessage> HandleForwardAndTransformOperation()
    {
        // Use the context to forward/send an HTTP request
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        // Do the transformation if the response was successful, otherwise return error responses as-is
        if (response.IsSuccessStatusCode && this.Context.OperationId == "GenerateAnswer")
        {
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            
            // Example case: response string is some JSON object
            var body = JObject.Parse(responseString);
            
            var answers = (body["answers"] as JArray);

            var _previousLowScoreVariationMultiplier = 0.7;
            var _maxLowScoreVariationMultiplier = 1.0;
            var MaximumScoreForLowScoreVariation = 0.95;
            var MinimumScoreForLowScoreVariation = 0.2;
            var suggestions = new JArray();
            var topAnswerScore = (double)answers[0]["confidenceScore"];

            var prevScore = topAnswerScore;
            if (topAnswerScore > MinimumScoreForLowScoreVariation && topAnswerScore <= MaximumScoreForLowScoreVariation)
            {
                suggestions.Add(answers[0]);

                for (var i = 1; i < answers.Count(); i++)
                {
                    bool check1 = ((prevScore - (double)answers[i]["confidenceScore"]) < (_previousLowScoreVariationMultiplier * Math.Sqrt(prevScore)));
                    bool check2 = ((topAnswerScore - (double)answers[i]["confidenceScore"]) < (_maxLowScoreVariationMultiplier * Math.Sqrt(topAnswerScore)));
                    if (check1 && check2)
                    {
                        prevScore = (double)answers[i]["confidenceScore"];
                        suggestions.Add(answers[i]);
                    }
                }
            }

            if (answers != null)
            {
                foreach (JObject answer in answers)
                {
                    // Keep one question and remove the rest.
                    if (answer["questions"]?.Count() > 1)
                    {
                        var questions = new JArray(answer["questions"][0]);
                        answer.Remove("questions");
                        answer.Add("questions", questions);
                    }
                }
            }

            var numberOfAmbiguousQuestions = suggestions?.Count();
            if (numberOfAmbiguousQuestions >= 2)
            {
                body.Add("isUserQuestionAmbiguous", true);
            }
            else
            {
                body.Add("isUserQuestionAmbiguous", false);
            }
            
            response.Content = CreateJsonContent(body.ToString());
        }

        return response;
    }
}
