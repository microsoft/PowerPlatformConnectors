// Rewrites requests between the Power Automate designer and https://api.marksign.eu
// Paths starting /power-automate/ are virtual, we have one API endpoint, several designer blocks
public class Script : ScriptBase
{
    private const int ThrowawayLinkMinutes = 1;

    private string bearer;

    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var request = this.Context.Request;
        string token = null;

        if (request.Headers.TryGetValues("Authorization", out var values))
        {
            token = values.FirstOrDefault();
        }

        if (string.IsNullOrEmpty(token))
        {
            return ErrorJson(HttpStatusCode.Unauthorized, "The connection is missing the API key.");
        }

        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = token.Substring(7);
        }

        this.bearer = "Bearer " + token;
        request.Headers.Remove("Authorization");
        request.Headers.TryAddWithoutValidation("Authorization", this.bearer);

        // Cloudflare sends content-encoding: zstd the moment a request offers it, and the flow
        // engine has no decoder for that
        request.Headers.Remove("Accept-Encoding");
        request.Headers.TryAddWithoutValidation("Accept-Encoding", "identity");

        switch (this.Context.OperationId)
        {
            case "UploadDocument":
                return await this.Upload().ConfigureAwait(false);
            case "InviteSigner":
            case "InviteByLink":
                return await this.InviteSigner().ConfigureAwait(false);
            case "WaitForSignature":
            case "WaitForSigner":
                return await this.WaitForSignature().ConfigureAwait(false);
            case "WaitForSignatureEvent":
                return await this.WaitForSignatureEvent().ConfigureAwait(false);
            case "CreateContainer":
                return await this.CreateContainer().ConfigureAwait(false);
            case "CreateSignerLinks":
                return await this.CreateSignerLinks().ConfigureAwait(false);
            case "CreateSharedLink":
                return await this.CreateSharedLink().ConfigureAwait(false);
            case "ValidateDocument":
                return await this.ValidateDocument().ConfigureAwait(false);
            case "ExtendSignatures":
                return await this.ExtendSignatures().ConfigureAwait(false);
            default:
                return await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
        }
    }

    // UploadDocument
    // Uploads one document and prepares it for signing
    // in   query file_name, billing_type, access + raw binary body
    // api  POST /v2/document/upload (multipart, part files[] named from file_name)
    // out  200 {status, data:{uuid, filename}} | 400 input | 502 empty data
    private async Task<HttpResponseMessage> Upload()
    {
        var uri = this.Context.Request.RequestUri;
        var fileName = QueryValue(uri, "file_name");
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return ErrorJson(HttpStatusCode.BadRequest, "The file name is missing.");
        }

        var content = this.Context.Request.Content;
        var bytes = content == null ? null : await content.ReadAsByteArrayAsync().ConfigureAwait(false);
        if (bytes == null || bytes.Length == 0)
        {
            return ErrorJson(HttpStatusCode.BadRequest, "The file content is empty.");
        }

        var fields = new Dictionary<string, string>
        {
            ["billing_type"] = QueryValue(uri, "billing_type", "document_owner")
        };

        var access = QueryValue(uri, "access");
        if (!string.IsNullOrEmpty(access))
        {
            fields["access"] = access;
        }

        var response = await this.SendFile(uri.GetLeftPart(UriPartial.Path), fileName, bytes, fields).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            return response;
        }

        var body = await ParseJson(response).ConfigureAwait(false);
        var first = (body["data"] as JArray)?.FirstOrDefault() as JObject;
        if (first == null)
        {
            return ErrorJson(HttpStatusCode.BadGateway, "The document was not uploaded.");
        }

        return OkJson(new JObject { ["status"] = body["status"], ["data"] = first });
    }

    // InviteSigner, InviteByLink
    // Invites a person to sign, by email or by returning their personal link
    // in   body name, surname, email, language, success_url, purpose, callback_url
    // api  POST /v2/document/{id}/signer (body nested as {signer:{...}}, InviteByLink adds no_email)
    // out  200 {status}, InviteByLink also {data:{invitation_url}} | 400 body
    //      the API response is passed through untouched
    private async Task<HttpResponseMessage> InviteSigner()
    {
        var request = this.Context.Request;
        var flat = await this.ReadBody().ConfigureAwait(false);
        if (flat == null)
        {
            return InvalidJson();
        }

        var signer = new JObject();
        foreach (var field in new[] { "name", "surname", "email", "language", "success_url", "purpose" })
        {
            if (flat[field] != null)
            {
                signer[field] = flat[field];
            }
        }

        if (this.Context.OperationId == "InviteByLink")
        {
            signer["no_email"] = true;
            var uri = request.RequestUri;
            request.RequestUri = new Uri(
                uri.GetLeftPart(UriPartial.Authority) + "/v2/document/" + PathSegment(uri, 3) + "/signer");
        }

        var body = new JObject { ["signer"] = signer };
        if (flat["callback_url"] != null)
        {
            body["callback_url"] = flat["callback_url"];
        }

        request.Content = CreateJsonContent(body.ToString());

        return await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
    }

    // WaitForSignature, WaitForSigner
    // Holds the flow until everyone has signed, or until one named signer has
    // in   path documentId, query timeout_minutes and signer_email, plus attempt and deadline while polling
    // api  GET /document/{id}/check-status (every poll)
    //      GET /v2/document/{id}/signer (every 3rd poll, or every poll with signer_email)
    // out  200 {document_status: signed|rejected|timed_out, rejected_by?, rejection_reason?}
    //      202 + Location + Retry-After (keep polling, the API has no long-running endpoint)
    private async Task<HttpResponseMessage> WaitForSignature()
    {
        var uri = this.Context.Request.RequestUri;
        var authority = uri.GetLeftPart(UriPartial.Authority);
        var documentId = PathSegment(uri, 3);

        var attempt = QueryInt(uri, "attempt", 0);
        var deadline = QueryLong(uri, "deadline", 0L);
        if (deadline == 0)
        {
            // 41760 min = 29 days, capped at 43200 = 30 days, where Power Automate kills the run regardless
            var timeoutMinutes = Math.Min(QueryLong(uri, "timeout_minutes", 41760L), 43200L);
            deadline = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + timeoutMinutes * 60;
        }

        var signerEmail = QueryValue(uri, "signer_email");

        var statusResponse = await this.SendGet(authority + "/document/" + documentId + "/check-status").ConfigureAwait(false);
        if (!statusResponse.IsSuccessStatusCode)
        {
            // A transient 5xx must not end a wait that may have weeks left
            return this.IsTransient(statusResponse, deadline)
                ? this.PollAgain(attempt, deadline, "waiting")
                : statusResponse;
        }

        var statusBody = await ParseJson(statusResponse).ConfigureAwait(false);
        var documentStatus = (string)statusBody?["data"]?["document_status"];

        if (documentStatus == "signed")
        {
            return OkJson(new JObject { ["document_status"] = "signed" });
        }

        // Rejection is only visible on the signers, never in the document status
        if (signerEmail != null || attempt % 3 == 0)
        {
            var signersResponse = await this.SendGet(authority + "/v2/document/" + documentId + "/signer").ConfigureAwait(false);
            if (!signersResponse.IsSuccessStatusCode)
            {
                return this.IsTransient(signersResponse, deadline)
                    ? this.PollAgain(attempt, deadline, "waiting")
                    : signersResponse;
            }

            var signersBody = await ParseJson(signersResponse).ConfigureAwait(false);
            var rejected = (signersBody?["data"] as JArray)?
                .FirstOrDefault(s => string.Equals((string)s["status"], "rejected", StringComparison.OrdinalIgnoreCase));

            if (rejected != null)
            {
                return OkJson(new JObject
                {
                    ["document_status"] = "rejected",
                    ["rejected_by"] = ((string)rejected["name"] + " " + (string)rejected["surname"]).Trim(),
                    ["rejection_reason"] = rejected["rejection_reason"]
                });
            }

            if (signerEmail != null)
            {
                var target = (signersBody?["data"] as JArray)?.FirstOrDefault(s =>
                    string.Equals((string)s["email"], signerEmail, StringComparison.OrdinalIgnoreCase));
                if (target != null && string.Equals((string)target["status"], "signed", StringComparison.OrdinalIgnoreCase))
                {
                    return OkJson(new JObject { ["document_status"] = "signed" });
                }
            }
        }

        if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() >= deadline)
        {
            return OkJson(new JObject { ["document_status"] = "timed_out" });
        }

        return this.PollAgain(attempt, deadline, documentStatus ?? "waiting");
    }

    private bool IsTransient(HttpResponseMessage response, long deadline)
    {
        return (int)response.StatusCode >= 500 && DateTimeOffset.UtcNow.ToUnixTimeSeconds() < deadline;
    }

    private HttpResponseMessage PollAgain(int attempt, long deadline, string status)
    {
        return this.PollingResponse(
            attempt,
            "deadline",
            deadline.ToString(),
            RetryDelaySeconds(attempt),
            new JObject { ["document_status"] = status });
    }

    // Polling state rides in the Location query, undeclared in the swagger
    private HttpResponseMessage PollingResponse(int attempt, string carryName, string carryValue, int retryAfterSeconds, JObject body)
    {
        var location = Regex.Replace(this.Context.OriginalRequestUri.ToString(), @"([?&])attempt=\d+&?", "$1")
            .TrimEnd('?', '&');
        location += (location.Contains("?") ? "&" : "?") + "attempt=" + (attempt + 1);
        if (!location.Contains(carryName + "="))
        {
            location += "&" + carryName + "=" + carryValue;
        }

        var response = new HttpResponseMessage(HttpStatusCode.Accepted);
        response.Headers.Location = new Uri(location);
        response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(retryAfterSeconds));
        response.Content = CreateJsonContent(body.ToString());
        return response;
    }

    // Cumulative, not per-tier: 30s up to the 5-min mark, 1 min to 30 min, 2 min to 2 h, then 5 min
    private static int RetryDelaySeconds(int attempt)
    {
        if (attempt < 10)
        {
            return 30;
        }
        if (attempt < 35)
        {
            return 60;
        }
        if (attempt < 80)
        {
            return 120;
        }
        return 300;
    }

    // WaitForSignatureEvent
    // Wakes the flow when the next signer signs or rejects, inviting nobody
    // in   path documentId, body callbackUrl (the flow's notification URL)
    // api  POST /v2/document/generate-temporary-signing-link (the only way to store callback URLs)
    //      DELETE /v2/document/signing-link/{id} (drops the link that registration forced)
    //      GET /document/{id}/check-status + /v2/document/{id}/signer
    // out  200 {status: ok} | 400 no callbackUrl | the link-creation error if registering failed
    //      already signed or rejected: POSTs the event payload to callbackUrl itself
    private async Task<HttpResponseMessage> WaitForSignatureEvent()
    {
        var input = await this.ReadBody().ConfigureAwait(false);
        var callbackUrl = input == null ? null : (string)input["callbackUrl"];
        if (string.IsNullOrEmpty(callbackUrl))
        {
            return ErrorJson(HttpStatusCode.BadRequest, "The callback URL is missing.");
        }

        var uri = this.Context.Request.RequestUri;
        var authority = uri.GetLeftPart(UriPartial.Authority);
        var documentId = PathSegment(uri, 3);

        var register = this.NewApiRequest(HttpMethod.Post, authority + "/v2/document/generate-temporary-signing-link");
        register.Content = CreateJsonContent(new JObject
        {
            ["documents"] = new JArray(documentId),
            ["callback_url"] = callbackUrl,
            ["reject_callback_url"] = callbackUrl,
            ["expire_after"] = ThrowawayLinkMinutes
        }.ToString());

        var registerResponse = await this.Context.SendAsync(register, this.CancellationToken).ConfigureAwait(false);
        if (registerResponse.IsSuccessStatusCode)
        {
            var created = await ParseJson(registerResponse).ConfigureAwait(false);
            await this.DeleteSigningLinks(created, authority).ConfigureAwait(false);
        }

        // Past events never replay
        var documentStatus = (string)null;
        var statusResponse = await this.SendGet(authority + "/document/" + documentId + "/check-status").ConfigureAwait(false);
        if (statusResponse.IsSuccessStatusCode)
        {
            var status = await ParseJson(statusResponse).ConfigureAwait(false);
            documentStatus = (string)status?["data"]?["document_status"];
        }

        var signers = (JArray)null;
        var signersResponse = await this.SendGet(authority + "/v2/document/" + documentId + "/signer").ConfigureAwait(false);
        if (signersResponse.IsSuccessStatusCode)
        {
            signers = (await ParseJson(signersResponse).ConfigureAwait(false))["data"] as JArray;
        }

        var alreadyRejected = signers != null && signers.Any(s => string.Equals((string)s["status"], "rejected", StringComparison.OrdinalIgnoreCase));
        if (documentStatus == "signed" || alreadyRejected)
        {
            var payload = new JObject { ["uuid"] = documentId, ["status"] = documentStatus };
            if (signers != null)
            {
                payload["members_count"] = signers.Count;
                payload["signed_members_count"] = signers
                    .Count(s => string.Equals((string)s["status"], "signed", StringComparison.OrdinalIgnoreCase));
                payload["members"] = new JArray(signers.Select(s => new JObject
                {
                    ["id"] = s["id"],
                    ["member_name"] = s["name"],
                    ["member_surname"] = s["surname"],
                    ["member_email"] = s["email"],
                    ["status"] = s["status"]
                }));
            }

            var fire = new HttpRequestMessage(HttpMethod.Post, callbackUrl);
            fire.Content = CreateJsonContent(payload.ToString());
            await this.Context.SendAsync(fire, this.CancellationToken).ConfigureAwait(false);
            return OkJson(new JObject { ["status"] = "ok" });
        }

        if (!registerResponse.IsSuccessStatusCode)
        {
            return registerResponse;
        }

        return OkJson(new JObject { ["status"] = "ok" });
    }

    // ValidateDocument
    // Checks a document's signatures and returns the validation report
    // in   path documentId, plus attempt and session while polling
    // api  POST /v3/validation/document/{id} (first call only, returns session_id)
    //      GET /v3/validation/session/{id} (every later poll)
    // out  200 validation report | 202 keep polling | 502 no session | 504 after 60 attempts
    private async Task<HttpResponseMessage> ValidateDocument()
    {
        var uri = this.Context.Request.RequestUri;
        var authority = uri.GetLeftPart(UriPartial.Authority);

        var attempt = QueryInt(uri, "attempt", 0);
        var sessionId = QueryValue(uri, "session");

        if (sessionId == null)
        {
            var startRequest = this.NewApiRequest(
                HttpMethod.Post, authority + "/v3/validation/document/" + PathSegment(uri, 3));

            var startResponse = await this.Context.SendAsync(startRequest, this.CancellationToken).ConfigureAwait(false);
            if (!startResponse.IsSuccessStatusCode)
            {
                return startResponse;
            }

            var started = await ParseJson(startResponse).ConfigureAwait(false);
            sessionId = (string)started?["data"]?["session_id"];
            if (string.IsNullOrEmpty(sessionId))
            {
                return ErrorJson(HttpStatusCode.BadGateway, "The validation session could not be started.");
            }
        }
        else
        {
            var resultResponse = await this.SendGet(authority + "/v3/validation/session/" + Uri.EscapeDataString(sessionId)).ConfigureAwait(false);
            if (!resultResponse.IsSuccessStatusCode)
            {
                return resultResponse;
            }

            var result = await ParseJson(resultResponse).ConfigureAwait(false);
            if (!string.Equals((string)result["status"], "waiting", StringComparison.OrdinalIgnoreCase))
            {
                return OkJson(result);
            }

            // 12 polls at 5s then 48 at 15s is 13 minutes, a stuck session must not poll for weeks
            if (attempt >= 60)
            {
                return ErrorJson(HttpStatusCode.GatewayTimeout, "The validation did not finish in time.");
            }
        }

        return this.PollingResponse(
            attempt,
            "session",
            Uri.EscapeDataString(sessionId),
            attempt < 12 ? 5 : 15,
            new JObject { ["status"] = "waiting" });
    }

    // CreateSignerLinks
    // Creates one personal signing link per signer, for you to deliver
    // in   body documents[], signers[]
    // api  POST /v2/document/generate-temporary-signing-link (body forwarded unchanged)
    // out  200 {status, documents, temporary_signing_links} | 400 body, empty documents, empty signers
    //      the API response is passed through untouched
    private async Task<HttpResponseMessage> CreateSignerLinks()
    {
        var request = this.Context.Request;
        var input = await this.ReadBody().ConfigureAwait(false);
        if (input == null)
        {
            return InvalidJson();
        }

        if (!(input["documents"] is JArray documents) || documents.Count == 0)
        {
            return ErrorJson(HttpStatusCode.BadRequest, "Provide at least one Document ID.");
        }

        if (!(input["signers"] is JArray signers) || signers.Count == 0)
        {
            return ErrorJson(HttpStatusCode.BadRequest, "Provide at least one signer.");
        }

        request.Content = CreateJsonContent(input.ToString());
        return await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
    }

    // CreateSharedLink
    // Creates one link anyone can open, and whoever opens it still signs with their own identity
    // in   body documents[], callback_url, delete_document_after, expire_after
    // api  POST /v2/document/generate-temporary-signing-link (same call, no signers array)
    // out  200 {status, data:{signing_link, signing_link_id, valid_until}} | 400 | 502 no link
    private async Task<HttpResponseMessage> CreateSharedLink()
    {
        var input = await this.ReadBody().ConfigureAwait(false);
        if (input == null)
        {
            return InvalidJson();
        }

        if (!(input["documents"] is JArray documents) || documents.Count == 0)
        {
            return ErrorJson(HttpStatusCode.BadRequest, "Provide at least one Document ID.");
        }

        var authority = this.Context.Request.RequestUri.GetLeftPart(UriPartial.Authority);
        var create = this.NewApiRequest(HttpMethod.Post, authority + "/v2/document/generate-temporary-signing-link");
        create.Content = CreateJsonContent(input.ToString());

        var response = await this.Context.SendAsync(create, this.CancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            return response;
        }

        var created = await ParseJson(response).ConfigureAwait(false);
        var link = (created["temporary_signing_links"] as JArray)?.FirstOrDefault() as JObject;
        if (link == null)
        {
            return ErrorJson(HttpStatusCode.BadGateway, "The link was not created.");
        }

        return OkJson(new JObject
        {
            ["status"] = "ok",
            ["data"] = new JObject
            {
                ["signing_link"] = link["temporary_signing_link"],
                ["signing_link_id"] = link["temporary_signing_link_id"],
                ["valid_until"] = link["valid_until"]
            }
        });
    }

    // CreateContainer
    // Combines several files into one signable container, such as an ADoc or ASiC-E
    // in   body document_type, files[]{file_name, file_content (base64 or {$content}), adoc_file_type}
    // api  POST /v3/file/upload (once per file, sequentially)
    //      POST /v2/document/generate-temporary-signing-link (only endpoint that builds from file UUIDs)
    //      DELETE /v2/document/signing-link/{id} (drops the link that assembly forced)
    // out  200 {status, data: document} | 400 input or wrong document_type | 502 upload failed
    private async Task<HttpResponseMessage> CreateContainer()
    {
        var input = await this.ReadBody().ConfigureAwait(false);
        if (input == null)
        {
            return InvalidJson();
        }

        if (!(input["files"] is JArray files) || files.Count == 0)
        {
            return ErrorJson(HttpStatusCode.BadRequest, "Provide at least one file.");
        }

        var authority = this.Context.Request.RequestUri.GetLeftPart(UriPartial.Authority);
        var documentFiles = new JArray();

        foreach (var file in files)
        {
            var fileName = (string)file["file_name"];
            var content = file["file_content"] is JObject inline ? (string)inline["$content"] : (string)file["file_content"];
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrEmpty(content))
            {
                return ErrorJson(HttpStatusCode.BadRequest, "Every file needs a File Name and File Content.");
            }

            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(content);
            }
            catch (FormatException)
            {
                return ErrorJson(HttpStatusCode.BadRequest, "The content of " + fileName + " is not valid base64.");
            }

            var uploadResponse = await this.SendFile(authority + "/v3/file/upload", fileName, bytes).ConfigureAwait(false);
            if (!uploadResponse.IsSuccessStatusCode)
            {
                return uploadResponse;
            }

            var uploaded = await ParseJson(uploadResponse).ConfigureAwait(false);
            var fileId = (string)((uploaded["data"] as JArray)?.FirstOrDefault()?["uuid"]);
            if (string.IsNullOrEmpty(fileId))
            {
                return ErrorJson(HttpStatusCode.BadGateway, fileName + " was not uploaded.");
            }

            var documentFile = new JObject { ["file_id"] = fileId };
            if (file["adoc_file_type"] != null)
            {
                documentFile["adoc_file_type"] = file["adoc_file_type"];
            }

            documentFiles.Add(documentFile);
        }

        var createRequest = this.NewApiRequest(HttpMethod.Post, authority + "/v2/document/generate-temporary-signing-link");
        createRequest.Content = CreateJsonContent(new JObject
        {
            ["document_type"] = input["document_type"],
            ["document_files"] = documentFiles,
            ["expire_after"] = ThrowawayLinkMinutes
        }.ToString());

        var createResponse = await this.Context.SendAsync(createRequest, this.CancellationToken).ConfigureAwait(false);
        if (!createResponse.IsSuccessStatusCode)
        {
            return createResponse;
        }

        var created = await ParseJson(createResponse).ConfigureAwait(false);
        await this.DeleteSigningLinks(created, authority).ConfigureAwait(false);

        var document = (created["documents"] as JArray)?.FirstOrDefault() as JObject;
        if (document == null)
        {
            return ErrorJson(HttpStatusCode.BadRequest, "The files were not combined into a document. Check the document type.");
        }

        return OkJson(new JObject { ["status"] = "ok", ["data"] = document });
    }

    // ExtendSignatures
    // Upgrades a document's signatures for long-term validity or archiving
    // in   path documentId, body level (LTA, anything else means LT)
    // api  POST /document/{id}/extend-to-lt | /document/{id}/extend-to-lta (empty body)
    // out  200 {status} | 400 body
    //      the API response is passed through untouched
    private async Task<HttpResponseMessage> ExtendSignatures()
    {
        var request = this.Context.Request;
        var input = await this.ReadBody().ConfigureAwait(false);
        if (input == null)
        {
            return InvalidJson();
        }

        var suffix = string.Equals((string)input["level"], "LTA", StringComparison.OrdinalIgnoreCase)
            ? "extend-to-lta"
            : "extend-to-lt";
        var uri = request.RequestUri;

        request.RequestUri = new Uri(
            uri.GetLeftPart(UriPartial.Authority) + "/document/" + PathSegment(uri, 3) + "/" + suffix);
        request.Content = CreateJsonContent("{}");

        return await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
    }

    private async Task DeleteSigningLinks(JObject created, string authority)
    {
        var links = created["temporary_signing_links"] as JArray;
        if (links == null)
        {
            return;
        }

        foreach (var link in links)
        {
            var linkId = (string)link["temporary_signing_link_id"];
            if (string.IsNullOrEmpty(linkId))
            {
                continue;
            }

            await this.Context.SendAsync(
                this.NewApiRequest(HttpMethod.Delete, authority + "/v2/document/signing-link/" + linkId),
                this.CancellationToken).ConfigureAwait(false);
        }
    }

    private async Task<JObject> ReadBody()
    {
        try
        {
            var content = this.Context.Request.Content;
            var body = content != null ? await content.ReadAsStringAsync().ConfigureAwait(false) : null;
            return string.IsNullOrWhiteSpace(body) ? new JObject() : JObject.Parse(body);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private async Task<HttpResponseMessage> SendGet(string url)
    {
        return await this.Context.SendAsync(this.NewApiRequest(HttpMethod.Get, url), this.CancellationToken).ConfigureAwait(false);
    }

    private async Task<HttpResponseMessage> SendFile(string url, string fileName, byte[] bytes, Dictionary<string, string> fields = null)
    {
        var file = new ByteArrayContent(bytes);
        file.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        var form = new MultipartFormDataContent();
        form.Add(file, "files[]", fileName);
        if (fields != null)
        {
            foreach (var pair in fields)
            {
                form.Add(new StringContent(pair.Value), pair.Key);
            }
        }

        var request = this.NewApiRequest(HttpMethod.Post, url);
        request.Content = form;
        return await this.Context.SendAsync(request, this.CancellationToken).ConfigureAwait(false);
    }

    private HttpRequestMessage NewApiRequest(HttpMethod method, string url)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.TryAddWithoutValidation("Authorization", this.bearer);
        request.Headers.TryAddWithoutValidation("Accept-Encoding", "identity");
        return request;
    }

    private static string PathSegment(Uri uri, int index)
    {
        return uri.AbsolutePath.Trim('/').Split('/')[index - 1];
    }

    private static string QueryValue(Uri uri, string name)
    {
        var match = Regex.Match(uri.Query ?? string.Empty, "[?&]" + name + "=([^&]+)");
        return match.Success ? Uri.UnescapeDataString(match.Groups[1].Value) : null;
    }

    private static string QueryValue(Uri uri, string name, string fallback)
    {
        var value = QueryValue(uri, name);
        return string.IsNullOrWhiteSpace(value) ? fallback : value;
    }

    private static int QueryInt(Uri uri, string name, int fallback)
    {
        return int.TryParse(QueryValue(uri, name), out var value) ? value : fallback;
    }

    private static long QueryLong(Uri uri, string name, long fallback)
    {
        return long.TryParse(QueryValue(uri, name), out var value) ? value : fallback;
    }

    private static async Task<JObject> ParseJson(HttpResponseMessage response)
    {
        return JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
    }

    private HttpResponseMessage OkJson(JObject body)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = CreateJsonContent(body.ToString());
        return response;
    }

    private HttpResponseMessage InvalidJson()
    {
        return ErrorJson(HttpStatusCode.BadRequest, "The request body is not valid JSON.");
    }

    private HttpResponseMessage ErrorJson(HttpStatusCode code, string message)
    {
        var response = new HttpResponseMessage(code);
        response.Content = CreateJsonContent(new JObject
        {
            ["status"] = "error",
            ["message"] = message
        }.ToString());
        return response;
    }
}
