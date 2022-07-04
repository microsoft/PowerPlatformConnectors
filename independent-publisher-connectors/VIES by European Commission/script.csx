#r "Newtonsoft.Json"

using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.IO;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        
        return await this.HandleForwardOperation().ConfigureAwait(false);

    }

    private async Task<HttpResponseMessage> HandleForwardOperation()
    {
        HttpResponseMessage response;
        var requestBody = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var data = JObject.Parse(requestBody);
        var countryCode = ((string)data["countryCode"]).Trim();
        var vatNumber = ((string)data["vatNumber"]).Trim().Replace(" ", string.Empty).Replace("-", string.Empty).Replace(".", string.Empty);
        
        var url = "http://ec.europa.eu/taxation_customs/vies/services/checkVatService";
        var xml = @"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'><s:Body><checkVat xmlns='urn:ec.europa.eu:taxud:vies:services:checkVat:types'><countryCode>"+countryCode+"</countryCode><vatNumber>"+vatNumber+"</vatNumber></checkVat></s:Body></s:Envelope>";

        try
        {
            using (var client = new WebClient())
            {
                var doc = new XmlDocument();
                doc.LoadXml(client.UploadString(url, string.Format(xml, countryCode, vatNumber)));
                var xmlResponse = doc.SelectSingleNode("//*[local-name()='checkVatResponse']") as XmlElement;
                if (xmlResponse == null)
                {
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    response.Content = CreateJsonContent(xmlResponse.ToString());
                    return response;
                }

                var newResult = new JObject
                {
                    ["countryCode"] = xmlResponse["countryCode"].InnerText,
                    ["vatNumber"] = xmlResponse["vatNumber"].InnerText,
                    ["name"] = xmlResponse["name"]?.InnerText,
                    ["address"] = xmlResponse["address"]?.InnerText,
                    ["requestDate"] = xmlResponse["requestDate"]?.InnerText,
                    ["valid"] = xmlResponse["valid"]?.InnerText,
                };
                
                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = CreateJsonContent(newResult.ToString());
                return response;
            }
        }
        catch (Exception e)
        {
            response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = CreateJsonContent(e.Message);
            return response;
        }
    }
}