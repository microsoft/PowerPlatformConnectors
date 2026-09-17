# Send to Gestioo

Gestioo helps organizations manage, classify and archive their documents in a secure vault. This connector deposits PDF and Word documents with the user's authorized classification and access groups, requests archival, and checks the final result.

## Publisher

Gestioo

## Prerequisites

- An active Gestioo organization and a Microsoft work account authorized by that organization to create documents and access its vault.
- A Power Automate license that covers Premium connectors, and permission to use the connector under your organization's data policies.
- For a local Windows file server, a configured Microsoft on-premises data gateway and File System connection, or an existing desktop flow that supplies the file content. The gateway accesses your local files; the Gestioo connector calls Gestioo over HTTPS.

The document transfer does not require a Dataverse database or Power BI. Power BI can report on an import register maintained by your own workflow.

## Supported operations

| Operation | Purpose |
| --- | --- |
| Get deposit options | Get the access groups, classifications, administrative units and upload limit available to the signed-in user. |
| Create deposit | Upload a PDF or DOCX with its metadata and a stable idempotency key. Returns a deposit receipt. |
| Get deposit status | Check a receipt created by this user and connector. |
| Archive deposit | Request archival of the deposited document. Repeat this action when processing is still in progress. |

## Getting started

1. Select **Send to Gestioo**, create a connection and sign in with your authorized Microsoft work account. Your Gestioo organization is resolved from that account. An administrator may need to approve access under your tenant's consent policy.
2. Run **Get deposit options** and choose the destination classification and access groups. Supply an event date when the selected classification requires it.
3. Read the file content and create a durable idempotency key for this file version. Store that key before the first upload and reuse it for retries.
4. Run **Create deposit** with the original file name, title, metadata and file bytes encoded once as base64. When the source connector returns a binary object, use its `$content` value. Store the returned `depositId`.
5. Call **Archive deposit** with that receipt. If the status is `receiving` or `processing`, wait for `retryAfterSeconds` (15 seconds by default) and call **Archive deposit** again. Bound the loop, for example to 240 iterations and one hour.
6. Only `archived` confirms completion. Record the receipt and archive timestamp in your import register. On `failed`, `interrupted`, or timeout, keep the original and investigate the existing receipt.

Archival seals the document and is irreversible. Configure automatic archival only for files your organization has approved for that destination. Limit initial imports to one file at a time and protect file inputs and outputs in flow run history.

## Known limitations

- PDF and DOCX are supported. Gestioo accepts original files up to 50 MiB; upstream connectors and gateways can impose lower limits. Check the strictest limit in the entire flow before uploading.
- Word documents are converted to PDF. The original DOCX is not retained as the archived source; keep the original file in your source system.
- The connector transfers existing files and metadata. It does not scan a file server, diagnose its infrastructure, or decide a records classification by itself.
- A status of `processing` does not mean that the document is in the vault. **Get deposit status** only observes progress; **Archive deposit** requests the seal once processing permits it.
- Reusing a key with different bytes or metadata returns `409 idempotency_conflict`. Do not generate a new key automatically after a timeout or a lost response.
- Access is checked on every call. Removing the user's rights or group membership can stop a previously connected flow.
- The Power Platform environment and source system determine the Microsoft-side data location. Connector certification does not guarantee that the whole workflow is hosted exclusively in Quebec.

## Troubleshooting

| Response | Action |
| --- | --- |
| 401 | Reconnect with an authorized Microsoft work account. |
| 403 | Ask your Gestioo administrator to verify the account, document and vault permissions, and selected access groups. |
| 404 | Check the receipt and use the same user and connector that created it. |
| 409 | Preserve the existing receipt and idempotency key. Investigate the reported conflict or document state. |
| 413 or 415 | Check file size, extension and actual content. |
| 429 or 503 | Wait and retry with the same receipt or the same idempotency key and payload. |

## Support

Contact [Gestioo support](https://gestioo.co/contact) or email support@gestioo.co. Provide the operation, timestamp and safe error code. Do not send credentials or document contents in a support request.

[Terms of use](https://gestioo.co/conditions-utilisation) · [Privacy policy](https://gestioo.co/politique-confidentialite)

## Public development configuration

The public API properties use the placeholder `YOUR_CLIENT_ID`. Contact Gestioo support for an authorized development registration and register the exact redirect URL generated for that custom connector. Production OAuth credentials are supplied privately to Microsoft through Partner Center and must never be committed to this repository.
