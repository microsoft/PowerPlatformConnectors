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
        var xml = @"<?xml version='1.0' encoding='utf-8'?><soap:Envelope xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'><soap:Body><checkVat xmlns='urn:ec.europa.eu:taxud:vies:services:checkVat:types'><countryCode>"+countryCode+"</countryCode><vatNumber>"+vatNumber+"</vatNumber></checkVat></soap:Body></soap:Envelope>";

        try
        {
            using (var client = new WebClient())
            {
                var doc = new XmlDocument();
                doc.LoadXml(client.UploadString(url, string.Format(xml, countryCode, vatNumber)));
                // Parse incoming XML to turn it into valid JSON response
                var xmlResponse = doc.SelectSingleNode("//*[local-name()='checkVatResponse']") as XmlElement;
                var xml_countryCode = xmlResponse.SelectSingleNode("//*[local-name()='countryCode']") as XmlElement;
                var xml_vatNumber = doc.SelectSingleNode("//*[local-name()='vatNumber']") as XmlElement;
                var xml_requestDate = doc.SelectSingleNode("//*[local-name()='requestDate']") as XmlElement;
                var xml_vatValid = doc.SelectSingleNode("//*[local-name()='valid']") as XmlElement;
                var xml_companyName = doc.SelectSingleNode("//*[local-name()='name']") as XmlElement;
                var xml_companyAddress = doc.SelectSingleNode("//*[local-name()='address']") as XmlElement;

                if (xmlResponse == null)
                {
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    response.Content = CreateJsonContent(xmlResponse.ToString());
                    return response;
                }

                // Building JSON for response
                var newResult = new JObject
                {
                    ["countryCode"] = xml_countryCode.InnerText,
                    ["vatNumber"] = xml_vatNumber.InnerText,
                    ["name"] = xml_companyName.InnerText,
                    ["address"] = xml_companyAddress.InnerText,
                    ["requestDate"] = xml_requestDate.InnerText,
                    ["valid"] = xml_vatValid.InnerText,
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
