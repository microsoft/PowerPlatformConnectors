# Folderit DMS

Folderit is a document management system (DMS) for organizing business documents. Find records, create folders, upload and download files, add document versions, read and update metadata, and queue configured document workflows using your Folderit permissions.

## Release status

This documentation accompanies the certification candidate. The connector is currently a private development preview, not a certified public connector. Check the [current integration status](https://www.folderit.com/blog/a-comprehensive-guide-to-folderits-multifaceted-integrations/#power-automate) before planning deployment.

## Prerequisites

- A Folderit account with API access and permission to use the intended documents and folders.
- A Microsoft license that permits custom connectors in the intended environment.
- An administrator-approved connector installation while the candidate remains private.

## Connect to Folderit

Create a Folderit connection from the installed connector, sign in with your Folderit user account, and authorize it. The connection acts with that user's existing permissions. Select the account and folder appropriate to your flow. Store credentials only in the authorized platform configuration.

## Document management actions

The candidate provides 19 actions covering accounts and folders, document search, record and file details, version history, metadata, file upload and download, and configured manual workflow starts. It currently supplies actions, with no Folderit event trigger. Use another approved trigger or an explicitly designed polling flow.

### Retrieve a document for the next step

1. Add **Download file** and select the account and exact file UID.
2. Read `fileContent`, `contentType`, `byteLength`, and `sha256` from the response.
3. For an action expecting binary content, map `base64ToBinary(body('Download_file')?['fileContent'])`.

A manual development flow passed the download through this conversion and preserved all 261 bytes of a binary test fixture. This verifies that mapping; it does not verify a complete Outlook, SharePoint, Teams, or other external-application workflow.

### Add a document revision

Use **Add file version** with the exact existing file UID. Map base64 file content and the file name. A successful revision keeps the file UID and returns a new version UID. Disable automatic retries for uploads and revision writes, and deduplicate source events before creating documents.

### Use metadata and document workflows

Discover field UIDs and types with **List metadata fields**. **Update metadata** accepts supported single-value scalar fields and verifies them by independent readback. Datetime writes and multiple-value fields are excluded from this candidate. A failed backend batch can leave some values changed; inspect the record before retrying.

**List startable workflows** returns exact manual start options. **Start workflow** queues the selected option. Queuing is not evidence that a workflow finished. Ten successful calls queued an inspected synthetic workflow with no notification or write steps. Downstream workflow completion is not independently asserted.

## Limits and troubleshooting

- File transfers are limited to 10 MiB in this candidate.
- Discovery lists for accounts and metadata are capped at 1,000 items; other list actions expose page bounds.
- HTTP 202 from download means the file is being prepared. Retry the read later.
- Use the documented Folderit token endpoint for refresh. The earliest development connection remained usable after more than 65 minutes without reconnecting; token exchange logs were not independently inspected.
- The final connector passed at least ten successful, schema-valid live calls for each of its 19 operations. Its managed connector solution passed Solution Checker with zero issues.
- The saved certification flow successfully downloaded and converted the binary fixture with an exact SHA-256 match.
- Independent customer permission isolation, deliberate revocation and near-limit transfer checks remain additional acceptance work. Microsoft certification and public publication are not yet complete.

For support or a scoped integration evaluation, [contact Folderit](https://www.folderit.com/contact/). Review the [Folderit API](https://api.folderit.com/) for current contracts.
