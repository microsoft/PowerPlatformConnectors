# Mark Sign

Mark Sign is a qualified electronic signature service compliant with the eIDAS regulation. A flow can send a document for signing, wait until it is signed, then download the signed file and validate it.

## Publisher: Mark ID, UAB

## Prerequisites

You need a Mark Sign API access token. See Obtaining Credentials below.

## Supported Operations

### Document: Upload
Uploads a document (PDF, ADoc, BDoc or ASiC-E) and prepares it for signing. The file name becomes the document title.

### Signing: Invite by email
Invites a person to sign; Mark Sign sends the invitation email. Calling it again for the same person sends a reminder.

### Signing: Invite by URL
Adds a signer without sending an email and returns their personal **Invitation URL** for you to deliver.

### Signing: Wait for all signers
Holds the flow until the document is signed or rejected, or ends with the `timed_out` status when the waiting time (29 days by default) runs out.

### Signing: Wait for one signer
Holds the flow until one particular signer, chosen by email, signs. Any rejection also ends the wait.

### Signing: Wait for next signer
Resumes the flow the moment the next signer signs or rejects, with no polling.

### Document: Get status
Checks the overall signing status of the document.

### Document: Get signers
Lists each signer's status, including the rejection reason if a signer rejected.

### Document: Download
Downloads the document with all signatures collected so far.

### Document: List
Lists the documents in your workspace, with text search and paging.

### Document: Remove
Removes a document from the workspace.

### Document: Upload as container
Combines several files into one signable container document (ADoc, BDoc or ASiC-E). Billing follows the Mark Sign default, where each signer pays.

### Signing: Create signer links
Creates a personal signing link for each signer, to deliver yourself. The links expire, can require identity verification, and are the only signing pages with a reject option.

### Signing: Create shared link
Creates one link anyone can open to sign the documents, each under their own verified identity.

### Signing: Delete link
Deletes a signer link or shared link before it expires.

### Document: Extend signature validity
Upgrades the document signatures for long-term validity (LT) or long-term archiving (LTA).

### Document: Validate
Checks the signatures of a document and returns the full validation report.

## Obtaining Credentials

Contact Mark Sign customer support at [pagalba@marksign.lt](mailto:pagalba@marksign.lt) to request an API access token for your workspace. When creating a connection in Power Automate, paste the token into the "API Key" field.

The token is tied to one workspace, so every document created or listed through the connection belongs to that workspace.

## Getting Started

The simplest signing flow:

1. **Document: Upload** (for example, the file content and name tokens from SharePoint or OneDrive).
2. **Signing: Invite by email**. Mark Sign emails the invitation.
3. **Signing: Wait for next signer**. The flow pauses here until the signing happens.
4. **Document: Download** and store it where you need it.

With several signers, use **Signing: Invite by email** for each person, then one **Signing: Wait for all signers** step before the download. To deliver the links yourself instead of Mark Sign emails, use **Signing: Invite by URL** or **Signing: Create signer links** and send each person their link with your own Outlook or Teams action.

## Known Issues and Limitations

- The connector does not provide flow triggers; use the wait operations inside a flow instead.
- A document has one notification registration. Do not run two **Signing: Wait for next signer** steps on the same document at the same time. The second replaces the first, and the first step then waits until it times out. Filling a **Callback URL** field on any block replaces the registration too.
- Signers can reject a document only on a **Signing: Create signer links** page (with **Show Reject Button** on); email invitations, invitation URLs and shared links offer no reject option.
- Mark Sign notifies only about signatures. When an invite block uses a **Purpose** of `review` or `confirmation`, that person's action never wakes **Signing: Wait for next signer**; check their status with **Document: Get signers** instead.
- **Document: Download** returns the file content only; take the file name from **Document: List** or from your upload step.
- A flow run can last at most 30 days (a Power Automate limit), so no wait can outlive that. **Signing: Wait for all signers** and **Signing: Wait for one signer** end with the `timed_out` status after 29 days by default. For **Signing: Wait for next signer**, set a **Timeout** (for example `P29D`) in the action settings and add a branch configured to run after the action **has timed out**. If the 30-day run limit hits first, Power Automate cancels the whole run and no branch handles it.
- **Document: List** does not support filtering by status.
- **Document: Upload** accepts one file per call; loop over attachments to upload several. **Document: Upload as container** is meant for a few files; combining many large files in one call can exceed the processing time limit.

## Deployment Instructions

Run `paconn create --api-prop apiProperties.json --api-def apiDefinition.swagger.json --script script.csx --icon icon.png` to deploy this connector as a custom connector in your Power Platform environment. See [paconn documentation](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) for details.

## Support

For issues with the connector or the Mark Sign API, contact [pagalba@marksign.lt](mailto:pagalba@marksign.lt).
