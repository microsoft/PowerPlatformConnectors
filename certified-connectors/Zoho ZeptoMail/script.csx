﻿public class Script: ScriptBase {
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
     }  else if(this.Context.OperationId == "Processed_Mail_Stats"){
        HttpRequestMessage request = this.Context.Request;
        var query = System.Web.HttpUtility.ParseQueryString(request.RequestUri.Query);
        var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
        string[] events = {"hb","sb","sent"};
        for (int i = 0; i < events.Length; i++) {
            query.Add("events",events[i]);
        }
        uriBuilder.Query = query.ToString();
        this.Context.Request.RequestUri = uriBuilder.Uri;
        HttpResponseMessage response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        if (response.IsSuccessStatusCode) {
            string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            JsonSerializerSettings settings = new JsonSerializerSettings
                                            {
                                                DateParseHandling = DateParseHandling.None
                                            };
            JObject result = (JObject)JsonConvert.DeserializeObject(responseString,settings);
            JObject dataObj = (JObject) result["data"];
            JObject statsObj = (JObject) dataObj["stats"];
            JObject resultObj = new JObject();
            for (int i = 0; i < events.Length; i++) {
                resultObj[events[i]]=getStatDataForEvent(statsObj,events[i],(int)dataObj[events[i]]);
            }
            response.Content = CreateJsonContent(resultObj.ToString());
            return response;

        }
        return response;

    }
    HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
    errorResponse.Content = CreateJsonContent("{\"message\": \"Unsupported Operation\"}");
    return errorResponse;

  }

public static JObject getStatDataForEvent(JObject statsObj, string eventKey,int count) {
    JObject eventStatObj = new JObject();
    JArray dataArray = new JArray();
    if(statsObj.ContainsKey(eventKey)){
        JArray eventArr = (JArray) statsObj[eventKey];
        for (int eventIndex = 0; eventIndex < eventArr.Count; eventIndex++) {
            JArray eventDataArr = (JArray) eventArr[eventIndex];
            JObject eventDataObj = new JObject();
            eventDataObj["date"] = (string)eventDataArr[0];
            Debug.WriteLine(eventDataArr[0]);
            eventDataObj["count"] = eventDataArr[1];
            dataArray.Add(eventDataObj);
        }

    }
    eventStatObj["stats"] = dataArray;
    eventStatObj["count"] = count;
    return eventStatObj;
  }

}