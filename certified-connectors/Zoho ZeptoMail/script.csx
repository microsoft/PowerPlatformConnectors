public class Script: ScriptBase {
  public override async Task < HttpResponseMessage > ExecuteAsync() {
    var dict = new Dictionary < string,
      string > ();

    if (this.Context.OperationId == "SendMail") {
      JObject jObject = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync());
      string mailType = (string) jObject["mailtype"];
      if (mailType == "text") {
        jObject.Add(new JProperty("textbody", (string) jObject["htmlbody"]));
        jObject.Remove("htmlbody");
      }
      JObject fromDetails = (JObject) jObject["from"];
      JObject fromDetail = (JObject) fromDetails["from-detail"];

      fromDetails.Remove("from-detail");
      fromDetails.Add(new JProperty("address", ((string) fromDetail["from-prefix"]) + "@" + ((string) fromDetail["from-domain"])));

      jObject.Remove("mailtype");
      this.Context.Request.Content = CreateJsonContent(jObject.ToString());
      HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
      return response;

    } else if (this.Context.OperationId == "SendTemplateMail") {
      JObject jObject = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync());
      JArray merge_key_details = (JArray) jObject["merge_key_detail"];
      if (merge_key_details != null) {
        JObject mergekeys = new JObject();
        for (int i = 0; i < merge_key_details.Count; i++) {
          JObject merge_key_detail = (JObject) merge_key_details[i];
          mergekeys.Add((string) merge_key_detail["key"], merge_key_detail["value"]);
        }
        jObject.Remove("merge_key_detail");
        jObject.Add(new JProperty("merge_info", mergekeys));
      }
      JObject fromDetails = (JObject) jObject["from"];
      JObject fromDetail = (JObject) fromDetails["from-detail"];
      fromDetails.Remove("from-detail");
      fromDetails.Add(new JProperty("address", ((string) fromDetail["from-prefix"]) + "@" + ((string) fromDetail["from-domain"])));

      this.Context.Request.Content = CreateJsonContent(jObject.ToString());
      HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
      return response;

    } else if (this.Context.OperationId == "GetMailTemplate") {
      HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
      string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
      JObject result = JObject.Parse(responseString);

      JArray dataArray = (JArray) result["data"];
      JArray mail_templates = new JArray();

      if (dataArray != null) {
        for (int i = 0; i < dataArray.Count; i++) {

          JObject dataElement = (JObject) dataArray[i];
          if (dataElement != null) {
            JArray templateArray = (JArray) dataElement["data"];
            if (templateArray != null) {
              for (int j = 0; j < templateArray.Count; j++) {
                mail_templates.Add((JObject) templateArray[j]);
              }

            }
          }
        }
        result.Remove("data");
        result.Add(new JProperty("mail_templates", mail_templates));

      }
      response.Content = CreateJsonContent(result.ToString());
      return response;
     }
    HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
    errorResponse.Content = CreateJsonContent("{\"message\": \"Unsupported Operation\"}");
    return errorResponse;

  }
}