public class Script: ScriptBase {
  public override async Task < HttpResponseMessage > ExecuteAsync() {
    if (this.Context.OperationId == "AirmeetTriggers") {
      await this.UpdateNameRequest().ConfigureAwait(false);
    } else {
      await this.UpdateRequest().ConfigureAwait(false);
    }
    var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(false);
    response.Headers.Location = new Uri(string.Format("https://api-gateway.airmeet.com/prod/platform-integration/v1/webhook-deregister"));
    return response;
  }
  private async Task UpdateNameRequest() {
    var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
    var contentAsJson = JObject.Parse(contentAsString);
    var triggerMetaInfoId = (string) contentAsJson["triggerMetaInfoId"];

    if (triggerMetaInfoId == "trigger.airmeet.attendee.added") {
      contentAsJson["name"] = "ZAPIER_AIRMEET_CREATED";
      contentAsJson["description"] = "Airmeet Created trigger subscription for MICROSOFT_DYNAMICS";
    }
    if (triggerMetaInfoId == "trigger.airmeet.created") {
      contentAsJson["name"] = "ZAPIER_AIRMEET_CREATED";
      contentAsJson["description"] = "Airmeet Created trigger subscription for MICROSOFT_DYNAMICS";
    }
    if (triggerMetaInfoId == "trigger.airmeet.registrant.added") {
      contentAsJson["name"] = "ZAPIER_REGISTRANT_ADDED";
      contentAsJson["description"] = "Registrant Created trigger subscription for MICROSOFT_DYNAMICS";
    }
    if (triggerMetaInfoId == "trigger.airmeet.started") {
      contentAsJson["name"] = "ZAPIER_AIRMEET_STARTED";
      contentAsJson["description"] = "Airmeet started trigger subscription for MICROSOFT_DYNAMICS";
    }
    if (triggerMetaInfoId == "trigger.airmeet.finished") {
      contentAsJson["name"] = "ZAPIER_AIRMEET_FINISHED";
      contentAsJson["description"] = "Airmeet finished trigger subscription for MICROSOFT_DYNAMICS";
    }
    if (triggerMetaInfoId == "trigger.airmeet.reminder") {
      contentAsJson["name"] = "ZAPIER_AIRMEET_REMINDER";
      contentAsJson["description"] = "Airmeet reminder trigger subscription for MICROSOFT_DYNAMICS";
    }
    if (triggerMetaInfoId == "trigger.airmeet.attendee.joined") {
      contentAsJson["name"] = "ZAPIER_REGISTRANT_JOINED";
      contentAsJson["description"] = "Attendee Joined trigger subscription for MICROSOFT_DYNAMICS";
    }
    this.Context.Request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
    this.Context.Request.Content = CreateJsonContent(contentAsJson.ToString());
  }
  private async Task UpdateRequest() {
    string content = string.Empty;
    this.Context.Request.Headers.TryGetValues("x-access-key", out
      var airmeetAccessKey);
    this.Context.Request.Headers.TryGetValues("x-secret-key", out
      var airmeetSecretKey);
    using
    var userInfoRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.airmeet.com/api/v1/auth");
    userInfoRequest.Headers.TryAddWithoutValidation("X-Airmeet-Access-Key", airmeetAccessKey);
    userInfoRequest.Headers.TryAddWithoutValidation("X-Airmeet-Secret-Key", airmeetSecretKey);
    userInfoRequest.Headers.TryAddWithoutValidation("Content-Type", "application/json");
    var newResult = new JObject {};
    userInfoRequest.Content = CreateJsonContent(newResult.ToString());
    using
    var userInfoResponse = await this.Context.SendAsync(userInfoRequest, this.CancellationToken).ConfigureAwait(false);
    content = await userInfoResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
    var jsonContent = JObject.Parse(content);
    var token = (string) jsonContent["token"];
    if (string.IsNullOrEmpty(token)) {
      token = string.Empty;
    }
    this.Context.Request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
    this.Context.Request.Headers.TryAddWithoutValidation("X-Airmeet-Access-Token", token);
  }
}