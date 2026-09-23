// Answers GetFilterSchema with the trigger's filter form; every other
// operation is forwarded untouched. Generated from filter-schema.json by
// generate-script-csx.py - do not edit by hand.

public class Script : ScriptBase
{
    private const string FilterSchemaOperationId = "GetFilterSchema";

    private const string EmptySchema =
        @"{""schema"":{""type"":""object"",""properties"":{}}}";

    private const string EnabledSchema =
        @"{""schema"":{""type"":""object"",""properties"":{""filter_match"":{""type"":""string"",""title"":""Combine filters with"",""description"":""Whether a meeting has to match every filter you fill in, or just one of them."",""enum"":[""all"",""any""],""default"":""all"",""x-ms-summary"":""Combine filters with"",""x-ms-enum-values"":[{""displayName"":""Match all filters (AND)"",""value"":""all""},{""displayName"":""Match any filter (OR)"",""value"":""any""}]},""title_contains"":{""type"":""array"",""title"":""Meeting title contains"",""description"":""Only meetings whose title contains any of these. Capitalisation is ignored."",""maxItems"":20,""items"":{""type"":""string"",""title"":""Value"",""description"":""One value"",""x-ms-summary"":""Value""},""x-ms-summary"":""Meeting title contains""},""title_starts_with"":{""type"":""array"",""title"":""Meeting title starts with"",""description"":""Only meetings whose title begins with any of these."",""maxItems"":20,""items"":{""type"":""string"",""title"":""Value"",""description"":""One value"",""x-ms-summary"":""Value""},""x-ms-summary"":""Meeting title starts with""},""title_equals"":{""type"":""array"",""title"":""Meeting title is exactly"",""description"":""Only meetings whose title is exactly one of these."",""maxItems"":20,""items"":{""type"":""string"",""title"":""Value"",""description"":""One value"",""x-ms-summary"":""Value""},""x-ms-summary"":""Meeting title is exactly""},""host_contains"":{""type"":""array"",""title"":""Meeting host contains"",""description"":""Only meetings hosted by one of these people. Whole email addresses, or @external, @internal, @domain(acme.com)."",""maxItems"":20,""items"":{""type"":""string"",""title"":""Value"",""description"":""One value"",""x-ms-summary"":""Value""},""x-ms-summary"":""Meeting host contains""},""host_not_contains"":{""type"":""array"",""title"":""Meeting host not contains"",""description"":""Skip meetings hosted by any of these people. Whole email addresses, or @external, @internal, @domain(acme.com)."",""maxItems"":20,""items"":{""type"":""string"",""title"":""Value"",""description"":""One value"",""x-ms-summary"":""Value""},""x-ms-summary"":""Meeting host not contains""},""participants_contains"":{""type"":""array"",""title"":""Meeting participants contains"",""description"":""Only meetings one of these people attended. Whole email addresses, or @external, @internal, @domain(acme.com). Your own address cannot be used - you are in every meeting this can fire for."",""maxItems"":20,""items"":{""type"":""string"",""title"":""Value"",""description"":""One value"",""x-ms-summary"":""Value""},""x-ms-summary"":""Meeting participants contains""},""participants_not_contains"":{""type"":""array"",""title"":""Meeting participants not contains"",""description"":""Skip meetings any of these people attended. Whole email addresses, or @external, @internal, @domain(acme.com). Your own address cannot be used."",""maxItems"":20,""items"":{""type"":""string"",""title"":""Value"",""description"":""One value"",""x-ms-summary"":""Value""},""x-ms-summary"":""Meeting participants not contains""}}}}";

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (string.Equals(this.OperationId(), FilterSchemaOperationId,
                          StringComparison.OrdinalIgnoreCase))
        {
            return this.FilterSchemaResponse();
        }

        return await this.Context
            .SendAsync(this.Context.Request, this.CancellationToken)
            .ConfigureAwait(false);
    }

    private string OperationId()
    {
        var raw = this.Context.OperationId;
        if (string.IsNullOrEmpty(raw))
        {
            return string.Empty;
        }

        try
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(raw));
        }
        catch (FormatException)
        {
            return raw;
        }
    }

    private HttpResponseMessage FilterSchemaResponse()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = CreateJsonContent(this.FiltersEnabled() ? EnabledSchema : EmptySchema);
        return response;
    }

    private bool FiltersEnabled()
    {
        var query = this.Context.Request.RequestUri?.Query ?? string.Empty;
        foreach (var pair in query.TrimStart('?').Split('&'))
        {
            var parts = pair.Split(new[] { '=' }, 2);
            if (parts.Length != 2 || !string.Equals(parts[0], "enabled",
                                                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var value = Uri.UnescapeDataString(parts[1]);
            return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)
                || value == "1"
                || string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }
}
