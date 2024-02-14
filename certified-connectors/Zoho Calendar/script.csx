public class Script: ScriptBase {
  public override async Task < HttpResponseMessage > ExecuteAsync() {
    if (this.Context.OperationId == "New_Event") {
      try {
        JObject eventData = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync());
        HttpRequestMessage request = this.Context.Request;
        var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
        uriBuilder.Query = "";
        bool isAllDay = (eventData["isallday"] != null) ? (bool) eventData["isallday"] : false;
        eventData["dateandtime"] = getConvertedDateTime((JObject) eventData["dateandtime"], isAllDay);
        request.RequestUri = uriBuilder.Uri;
        var content = new MultipartFormDataContent();
        content.Add(new StringContent(eventData.ToString()), "eventdata");
        request.Content = content;
        HttpResponseMessage response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        return await getEventResponse(response);
      } catch (Exception e) {
        if (e.Message == "Invalid_Date") {
          return getErrorMessage(HttpStatusCode.BadRequest, "Invalid date format");
        }
      }
    } else if (this.Context.OperationId == "Delete_Event") {
      HttpRequestMessage request = this.Context.Request;
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      uriBuilder.Query = "";
      request.RequestUri = uriBuilder.Uri;
      HttpRequestMessage eventRequest = CloneHttpRequest(request, HttpMethod.Get);
      HttpResponseMessage eventResponseMsg = await this.Context.SendAsync(eventRequest, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

      string responseString = await eventResponseMsg.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
      JObject eventResp = JObject.Parse(responseString);
      JArray events = (JArray) eventResp["events"];
      if (events != null && events.Count > 0) {
        JObject eventObj = (JObject) events[0];
        if (events.Count > 1 || eventObj.ContainsKey("rrule")) {
          return getErrorMessage(HttpStatusCode.BadRequest, "Cannot delete this event because it is a recurring event.");
        }
        
        string etag = (string) eventObj["etag"];
        request.Headers.TryAddWithoutValidation("etag", etag);
      } else {
        return getErrorMessage(HttpStatusCode.BadRequest, "Event resource not found");
      }
      HttpResponseMessage response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
      return response;
    } else if (this.Context.OperationId == "Get_Event_List") {
      HttpRequestMessage request = this.Context.Request;
      var query = System.Web.HttpUtility.ParseQueryString(request.RequestUri.Query);
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      uriBuilder.Query = "";
      this.Context.Request.RequestUri = uriBuilder.Uri;
      HttpResponseMessage response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
      return response;

    } else if (this.Context.OperationId == "Get_Event") {
      HttpRequestMessage request = this.Context.Request;
      var query = System.Web.HttpUtility.ParseQueryString(request.RequestUri.Query);
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      uriBuilder.Query = "";
      this.Context.Request.RequestUri = uriBuilder.Uri;
      HttpResponseMessage response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

      return await getEventResponse(response);
    } else if (this.Context.OperationId == "Get_Calendar_Details") {
      HttpRequestMessage request = this.Context.Request;
      var query = System.Web.HttpUtility.ParseQueryString(request.RequestUri.Query);
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      uriBuilder.Query = "";
      this.Context.Request.RequestUri = uriBuilder.Uri;
      HttpResponseMessage response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

      return await getCalendarResponse(response);
    } else if (this.Context.OperationId == "Search_Events") {
      try {
            HttpRequestMessage request = this.Context.Request;
      var query = System.Web.HttpUtility.ParseQueryString(request.RequestUri.Query);
      var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
      query.Remove("cuid");
      query["start"] = getConvertedTime(query["start"], false, true);
      if (query["end"] != null && !String.IsNullOrEmpty(query["end"])) {
        query["end"] = getConvertedTime(query["end"], false, true);
      }
      uriBuilder.Query = query.ToString();
      this.Context.Request.RequestUri = uriBuilder.Uri;
      HttpResponseMessage response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);

      return await getEventResponse(response);} catch (Exception e) {
        if (e.Message == "Invalid_Date") {
          return getErrorMessage(HttpStatusCode.BadRequest, "Invalid date format");
        }
      }
    } else if (this.Context.OperationId == "Update_Event") {
      try {
        HttpRequestMessage request = this.Context.Request;
        var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
        uriBuilder.Query = "";
        request.RequestUri = uriBuilder.Uri;
        HttpRequestMessage eventRequest = CloneHttpRequest(request, HttpMethod.Get);
        HttpResponseMessage eventResponseMsg = await this.Context.SendAsync(eventRequest, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        JObject eventData = JObject.Parse(await this.Context.Request.Content.ReadAsStringAsync());
        string responseString = await eventResponseMsg.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
        JObject eventResp = JObject.Parse(responseString);
        JArray events = (JArray) eventResp["events"];
        JObject respObj = JObject.Parse("{}");
        string etag = "";
        if (events != null && events.Count > 0) {

          JObject eventObj = (JObject) events[0];
          etag = (string) eventObj["etag"];
        }
        var content = new MultipartFormDataContent();
        eventData["etag"] = etag;
        bool isAllDay = (eventData["isallday"] != null) ? (bool) eventData["isallday"] : false;
        eventData["dateandtime"] = getConvertedDateTime((JObject) eventData["dateandtime"], isAllDay);
        content.Add(new StringContent(eventData.ToString()), "eventdata");
        request.Content = content;
        HttpResponseMessage response = await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        return await getEventResponse(response);
      } catch (Exception e) {
        if (e.Message == "Invalid_Date") {
          return getErrorMessage(HttpStatusCode.BadRequest, "Invalid date format");
        }
      }
    }

    return null;
  }
  public static JObject getConvertedDateTime(JObject eventData, bool isAllDay) {
    JObject covertedDateObj = JObject.Parse("{}");
    bool hasTimeZone = eventData.ContainsKey("timezone") && !string.IsNullOrEmpty((string) eventData["timezone"]);
    covertedDateObj["start"] = getConvertedTime(eventData["start"].Value < string > (), isAllDay, !hasTimeZone);
    covertedDateObj["end"] = getConvertedTime(eventData["end"].Value < string > (), isAllDay, !hasTimeZone);
    if (hasTimeZone) {
      covertedDateObj["timezone"] = (string) eventData["timezone"];
    }
    return covertedDateObj;
  }
  public static string getConvertedTime(string value, bool isDateFormat, bool isOffsetNeeded) {

    string outputDTFormat = "yyyyMMddTHHmmss";
    string outputDFormat = "yyyyMMdd";
    String[] datetimeformatWZ = {
      "yyyy/MM/dd'T'HH:mm:sszzz",
      "yyyy/MM/dd'T'HH:mm:ssZ",
      "yyyy-MM-dd'T'HH:mm:sszzz",
      "yyyy-MM-dd'T'HH:mm:ssZ",
      "yyyyMMdd'T'HHmmssZ",
      "yyyyMMdd'T'HHmmsszzz"
    };
    String[] datetimeformat = {
      "yyyy/MM/dd'T'HH:mm:ss",
      "yyyy-MM-dd'T'HH:mm:ss",
      "yyyyMMdd'T'HHmmss"
    };
    String[] dateformat = {
      "yyyy/MM/dd",
      "yyyy-MM-dd",
      "yyyyMMdd"
    };
    DateTime newDate;
    DateTime convertedDate;
    bool hasOffset = false;

    if (DateTime.TryParseExact(value, datetimeformat, null, System.Globalization.DateTimeStyles.None, out convertedDate)) {
      if (isOffsetNeeded) {
        newDate = convertedDate.ToUniversalTime();
      } else {
        newDate = convertedDate;
      }
    } else if (DateTime.TryParseExact(value, datetimeformatWZ, null, System.Globalization.DateTimeStyles.None, out convertedDate)) {
      newDate = convertedDate.ToUniversalTime();
      hasOffset = true;
    } else if (DateTime.TryParse(value, out convertedDate)) {
      newDate = convertedDate.ToUniversalTime();
      hasOffset = true;
    } else if (DateTime.TryParseExact(value, dateformat, null, System.Globalization.DateTimeStyles.None, out convertedDate)) {
      newDate = convertedDate;
    } else if (long.TryParse(value, out long timeinmillis)) {
      newDate = DateTimeOffset.FromUnixTimeMilliseconds(timeinmillis).UtcDateTime;
      hasOffset = true;
    } else {
      throw new Exception("Invalid_Date");
    }
    if (isDateFormat) {
      return newDate.ToString(outputDFormat);
    } else if (isOffsetNeeded || hasOffset) {
      return newDate.ToString(outputDTFormat + "Z");
    } else {
      return newDate.ToString(outputDTFormat);
    }

  }
  public static HttpRequestMessage CloneHttpRequest(HttpRequestMessage request, HttpMethod httpMethod) {
    HttpRequestMessage clone = new HttpRequestMessage(httpMethod, request.RequestUri);

    clone.Version = request.Version;
    foreach(KeyValuePair < string, object > prop in request.Properties) {
      clone.Properties.Add(prop);
    }
    foreach(KeyValuePair < string, IEnumerable < string >> header in request.Headers) {
      clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
    }
    clone.Headers.TryAddWithoutValidation("Content-Type", "application/octet-stream");
    return clone;
  }
  public static async Task < HttpResponseMessage > getEventResponse(HttpResponseMessage response) {
    return await convertArrayToObject(response, "events");
  }
  public static async Task < HttpResponseMessage > getCalendarResponse(HttpResponseMessage response) {
    return await convertArrayToObject(response, "calendars");
  }
  public static async Task < HttpResponseMessage > convertArrayToObject(HttpResponseMessage response, string key) {
    if (response.IsSuccessStatusCode) {
      string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
      JObject arrayResp = JObject.Parse(responseString);
      JArray items = (JArray) arrayResp[key];
      JObject respObj = JObject.Parse("{}");
      if (items != null && items.Count > 0) {

        JObject itemObj = (JObject) items[0];
        response.Content = new StringContent(itemObj.ToString(), System.Text.Encoding.UTF8, "application/json");
      }
    }
    return response;
  }
  public static HttpResponseMessage getErrorMessage(HttpStatusCode errorCode, string description) {
    JObject errorObj = new JObject();
    JArray errArr = new JArray();
    JObject errDetailObj = new JObject();
    errDetailObj["description"] = description;
    errArr.Add(errDetailObj);
    errorObj["error"] = errArr;
    HttpResponseMessage errorResponse = new HttpResponseMessage(errorCode);
    errorResponse.Content = CreateJsonContent(errorObj.ToString());
    return errorResponse;
  }
}