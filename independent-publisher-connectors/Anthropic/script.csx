public class Script : ScriptBase
{ 
  public override async Task<HttpResponseMessage> ExecuteAsync() 
  { 
    // Manipulate the request data as applicable before setting it back 
    var requestContentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false); 
    var requestContentAsJson = JObject.Parse(requestContentAsString); 
    // Edit the prompt field 
    var prompt = requestContentAsJson.GetValue("prompt").ToString(); 
    prompt = "\n\nHuman: " + prompt + "\n\nAssistant: "; 
    requestContentAsJson["prompt"] = prompt; 
    this.Context.Request.Content = CreateJsonContent(requestContentAsJson.ToString()); 
    // Make the actual API request 
    var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false); 
    return response; 
  }
}