# Tvarka Sign MCP

Tvarka Sign MCP lets an agent send documents for signature and bring back the signed result. The agent drafts a contract from text, or takes a file or a link, and sends it to one or more people. Tvarka Sign emails each signer. A signer signs with a simple e-signature on the page, or with a qualified electronic signature through their eID (Smart-ID, Mobile-ID or an ID card). The agent follows each signing, sends reminders, and returns the signed PDF and its evidence package.

Every signature is made by a person. No tool signs on anyone's behalf.

## Publisher: UAB Socialiniai algoritmai

Tvarka Sign is a service of UAB Socialiniai algoritmai, Lithuania. Website: https://tvarka.eu/

## Prerequisites

1. A Tvarka Sign account. Sign up at https://tvarka.eu/
2. Your Tvarka API key (see below).

QES use requires the signer's own supported QES credential and compatible signing middleware for the chosen method. The API key alone does not provide QES capability. Ordinary e-signatures (`level: esign`) do not require QES credentials or middleware.

## How to get your API key

1. Sign in to the Tvarka Sign app at https://tvarka.eu/sign/app/
2. Open Settings.
3. Click Show or Copy beside API key. Every active member of a workspace with Sign access has a key. It starts with `tsk_live_`.
4. When you add Tvarka Sign MCP to an agent, paste the key into the API key field.

The key acts as you, in one workspace. It is sent in the `X-Api-Key` header. Revoke or replace it under Settings, Manage access.

## Tools

The agent discovers the tools at run time through `tools/list`.

| Tool | What it does |
|---|---|
| `send_document` | Sends a document for signature in one call. The document can be Markdown text (rendered to PDF), a public link, base64 bytes or a document already in the workspace. Signers can sign in parallel or in a fixed order. |
| `create_document` | Turns Markdown text into an A4 PDF in the workspace and returns a preview link. |
| `list_documents` | Lists workspace documents that can be sent for signature. |
| `request_signatures` | Sends a workspace document for signature to one or more people. |
| `get_signing` | Returns the state of a signing and each signer. Once completed, it returns a download link for the signed document, and for e-signatures a link to the evidence package. |
| `list_signings` | Lists signings sent with this key, newest first. |
| `add_signer` | Adds a signer to a pending qualified signing. |
| `remind_signer` | Sends a signer their invitation again. |
| `cancel_signing` | Withdraws every open invitation and closes the signing. |
| `list_templates` | Lists the workspace's saved templates with their signer roles. |
| `send_from_template` | Sends a saved template for signature in one call. |
| `list_form_templates` | Lists the workspace's published forms and their fields. |
| `send_forms` | Asks one person to fill forms on their own phone and sign them. |
| `get_form_request` | Returns the state of a form request and, once submitted, its values. |
| `sign_now` | Lets the person who owns the key sign a workspace PDF now with Smart-ID or Mobile-ID on their own phone. The agent shows the verification code. The person checks it and enters their PIN. |
| `get_signature` | Returns the state of a `sign_now` signature and, once signed, a download link. |

## Signature types

- E-signature (`level: esign`): each signer reads the PDF and signs on the page, optionally behind an access code or an SMS code. Billed once per completed document.
- Qualified signature (`level: qualified`, the default): each signer signs with their eID. Billed per signature.

## Example prompts

- "Send this NDA to anna@example.com and tom@example.com for e-signature, Anna first."
- "Draft a one-page service agreement between our company and Example Ltd and send it to jonas@example.com for signature."
- "Which of my signings sent this week are still waiting, and who has not signed?"
- "Remind the last signer on the supply contract."
- "Download the signed lease and its evidence package."

## Data and safety

- The key reaches only the workspace it belongs to.
- Tools that change state (sending, adding a signer, reminding, cancelling, signing) are marked as such, so the agent asks the user before it calls them. Read-only tools are marked read-only.
- Signed documents are returned as download links that expire after one hour.
- Documents and data are stored on servers in the European Union.

## Known issues and limitations

- A document sent inline as base64 can be up to 15 MiB.
- `sign_now` works only for a person with a Lithuanian personal code on their Tvarka account and a Smart-ID or Mobile-ID. For anyone else it returns a clear error, and `send_document` is used instead.
- An e-signature has a fixed list of signers. To change it, cancel the signing and send it again.
- Each request must finish within 120 seconds. Large documents are best sent as a link.
- MCP actions are discovered by agents and are not selectable as ordinary actions in the Power Automate flow designer. Use the connector Test tab with a raw JSON-RPC request or an MCP-enabled agent to exercise the tools.

## Support

Email info@tvarka.eu. API documentation: https://sign-api.tvarka.eu/docs

## Deployment instructions

Add Tvarka Sign MCP as a tool to an agent, create a connection with your API key, and let the agent call the tools.
