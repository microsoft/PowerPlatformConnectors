// Custom code for the PDF Blocks connector. Two operations need shaping that
// Swagger alone cannot express; every other operation is codeless and never
// reaches this file (see `scriptOperations` in apiProperties.json).
//
// MergeDocumentsArray (request shaping): the flow passes an array of documents,
// but the merge endpoint expects a multipart/form-data body with one `file`
// part per document. We decode each base64 document and rebuild the request as
// multipart before forwarding it.
//
// The split_* operations (response shaping): the API returns several PDF
// documents as a compact ZIP archive (its default multi-output format).
// Requesting the API's JSON envelope instead would base64-inflate every part on
// the metered API hop, so we keep the ZIP there and expand it locally into a
// `documents` array the flow can iterate.
//
// Runs on .NET Standard 2.0 with the connector's supported namespaces only:
// https://learn.microsoft.com/en-us/connectors/custom-connectors/write-code
public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        switch (this.ResolveOperationId())
        {
            case "MergeDocumentsArray":
                return await this.HandleMergeArrayOperation().ConfigureAwait(false);

            case "SplitBySize":
            case "SplitByGroups":
            case "SplitAtPage":
            case "SplitByPageCount":
                return await this.HandleSplitOperation().ConfigureAwait(false);

            default:
                // Not a split operation — forward the request untouched. With the
                // scriptOperations allowlist this branch should never be reached.
                return await this.Context
                    .SendAsync(this.Context.Request, this.CancellationToken)
                    .ConfigureAwait(false);
        }
    }

    // The operations this script shapes. Kept in sync with the switch in
    // ExecuteAsync and with `scriptOperations` in apiProperties.json; also used
    // to disambiguate the OperationId below.
    private static readonly string[] ScriptedOperationIds =
    {
        "MergeDocumentsArray",
        "SplitBySize",
        "SplitByGroups",
        "SplitAtPage",
        "SplitByPageCount",
    };

    // Power Automate hands us a plain OperationId in most regions but a
    // base64-encoded one in some. We cannot tell them apart by trying to decode
    // and catching the failure: several plain ids are *also* valid base64.
    // "SplitByPageCount" is sixteen base64-alphabet characters, so it decodes to
    // twelve bytes of garbage instead of throwing — the operation then matches no
    // case and silently falls through to the untouched passthrough, returning the
    // raw ZIP. So trust the raw value when it already names a scripted operation,
    // and only fall back to a base64 decode that *also* yields a scripted one.
    private string ResolveOperationId()
    {
        var operationId = this.Context.OperationId;

        if (Array.IndexOf(ScriptedOperationIds, operationId) >= 0)
        {
            return operationId;
        }

        try
        {
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(operationId));
            if (Array.IndexOf(ScriptedOperationIds, decoded) >= 0)
            {
                return decoded;
            }
        }
        catch (FormatException)
        {
            // Not base64 — fall through to the raw value.
        }

        return operationId;
    }

    private async Task<HttpResponseMessage> HandleMergeArrayOperation()
    {
        // The flow supplies a JSON body: { "documents": [ ... ] } where each
        // element is either a bare base64 string or an object carrying the
        // content (see the per-element handling below). The merge endpoint
        // instead wants a multipart/form-data body with one `file` part per
        // document, so we translate the request here. The `file` field repeats
        // (the API accepts any number of them), which avoids the ten-slot
        // file_1..file_10 cap of the codeless MergeDocuments action.
        var requestBody = this.Context.Request.Content == null
            ? string.Empty
            : await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);

        JObject input;
        try
        {
            input = string.IsNullOrWhiteSpace(requestBody)
                ? new JObject()
                : JObject.Parse(requestBody);
        }
        catch (JsonReaderException)
        {
            return this.CreateBadRequest("The request body must be a JSON object with a 'documents' array.");
        }

        if (!(input["documents"] is JArray documents) || documents.Count == 0)
        {
            return this.CreateBadRequest("Provide at least one document in the 'documents' array.");
        }

        var multipart = new MultipartFormDataContent();
        var index = 0;

        foreach (var document in documents)
        {
            index++;

            // Accept either a bare base64 string (the document content) or an
            // object carrying it under `fileContent` (+ optional `fileName`) —
            // the field names Power Automate file actions and the rest of the
            // connector use. Anything else yields a clean 400 rather than an
            // unhandled exception that would surface as a 500 — reading
            // `element["fileContent"]` off a bare string token would throw.
            string content;
            string name = null;

            switch (document.Type)
            {
                case JTokenType.String:
                    content = (string)document;
                    break;

                case JTokenType.Object:
                    var documentObject = (JObject)document;
                    content = (string)documentObject["fileContent"];
                    name = (string)documentObject["fileName"];
                    break;

                default:
                    return this.CreateBadRequest(
                        $"Document {index} must be a base64 string or an object with a 'fileContent' property.");
            }

            if (string.IsNullOrEmpty(content))
            {
                return this.CreateBadRequest($"Document {index} is missing its 'fileContent'.");
            }

            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(content);
            }
            catch (FormatException)
            {
                return this.CreateBadRequest($"Document {index} has 'fileContent' that is not valid base64.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = $"document_{index}.pdf";
            }

            var part = new ByteArrayContent(bytes);
            part.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            // Every part shares the field name `file`; the API reads the repeated
            // field as an ordered list, preserving the array order.
            multipart.Add(part, "file", name);
        }

        this.Context.Request.Content = multipart;

        // The swagger path is /merge_documents/array (a connector-only variant);
        // point the request at the real /merge_documents endpoint. Rewriting the
        // path segment preserves the scheme and host, including a custom `domain`.
        this.Context.Request.RequestUri = new Uri(
            this.Context.Request.RequestUri.AbsoluteUri.Replace("/merge_documents/array", "/merge_documents"));

        // The connector's global `produces` is application/octet-stream, which the
        // API does not negotiate (it would answer 406), so ask for the PDF.
        this.Context.Request.Headers.Accept.Clear();
        this.Context.Request.Headers.Accept.ParseAdd("application/pdf");

        return await this.Context
            .SendAsync(this.Context.Request, this.CancellationToken)
            .ConfigureAwait(false);
    }

    private HttpResponseMessage CreateBadRequest(string message)
    {
        var body = new JObject
        {
            ["message"] = message,
        };

        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent(body.ToString());
        return response;
    }

    private async Task<HttpResponseMessage> HandleSplitOperation()
    {
        // Force the compact ZIP archive. The connector's global `produces` is
        // application/octet-stream, which the API does not negotiate (it would
        // answer 406), so we must set an Accept it recognises.
        this.Context.Request.Headers.Accept.Clear();
        this.Context.Request.Headers.Accept.ParseAdd("application/zip");

        var apiResponse = await this.Context
            .SendAsync(this.Context.Request, this.CancellationToken)
            .ConfigureAwait(false);

        // Surface validation and other API errors to the flow unchanged.
        if (!apiResponse.IsSuccessStatusCode)
        {
            return apiResponse;
        }

        var zipBytes = await apiResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

        var documents = new JArray();

        using (var zipStream = new MemoryStream(zipBytes))
        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
        {
            foreach (var entry in archive.Entries)
            {
                // Skip directory entries (their Name is empty).
                if (string.IsNullOrEmpty(entry.Name))
                {
                    continue;
                }

                using (var entryStream = entry.Open())
                using (var buffer = new MemoryStream())
                {
                    entryStream.CopyTo(buffer);

                    documents.Add(new JObject
                    {
                        ["name"] = entry.Name,
                        ["content"] = Convert.ToBase64String(buffer.ToArray()),
                        ["content_type"] = "application/pdf",
                    });
                }
            }
        }

        var body = new JObject
        {
            ["documents"] = documents,
        };

        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = CreateJsonContent(body.ToString());
        return response;
    }
}
