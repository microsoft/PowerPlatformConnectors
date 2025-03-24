using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (this.Context.OperationId == "FormatQuery")
        {
            var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            JObject data = JObject.Parse(contentAsString);

            JArray columnMetaData = (JArray)data["ColumnMetaData"];
            JArray rows = (JArray)data["Row"];

            JArray result = new JArray();

            foreach (JArray row in rows)
            {
                JObject mappedRow = new JObject();

                for (int i = 0; i < row.Count; i++)
                {
                    JObject column = (JObject)columnMetaData[i];
                    string columnName = (string)column["DefaultValue"];
                    var rowValue = (JToken)row[i];
                    mappedRow[columnName] = rowValue;
                }
                result.Add(mappedRow);
            }
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = CreateJsonContent(result.ToString());
            return response;
        }
        else
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            return response;
        }
    }
}