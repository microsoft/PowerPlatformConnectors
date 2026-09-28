# Power Automate Integration

Connect Instafill.ai with Microsoft Power Automate to automate your document workflows. Fill forms from CRM data, convert incoming flat PDFs automatically, trigger Teams notifications when a fill completes, and more.

---

## Supported triggers and actions

### Triggers

| Name                     | Type              | Description                                                                                              |
|--------------------------|-------------------|----------------------------------------------------------------------------------------------------------|
| **When an event occurs** | Trigger (Instant) | Fires when a form is filled or a flat PDF is converted to fillable. Select the event type to listen for. |

### Actions

| Name                       | Type   | Description                                                    | Required input |
|----------------------------|--------|----------------------------------------------------------------|----------------|
| **Fill Form**              | Create | Fills a PDF form with data from provided sources using AI.     | Form ID        |
| **Get Fill Job Status**    | Search | Returns the status and result of a fill job.                   | Session ID     |
| **List Forms**             | Search | Returns all forms in the current workspace.                    | —              |
| **Get Form**               | Search | Returns details for a specific form.                           | Form ID        |
| **Convert PDF**            | Create | Converts a flat PDF into a fillable form.                      | PDF File       |
| **Get Conversion Status**  | Search | Returns the current status and result of a conversion job.     | Job ID         |
| **Check if PDF Is Flat**   | Create | Checks whether a PDF is flat or already fillable.              | PDF File       |

---

## Getting started

Before you open Power Automate, generate an API key in your Instafill.ai account:
**Settings → Workspace → [Manage API Keys](https://instafill.ai/settings/workspace/api)**

> API keys are shown only once. Copy it before closing the page.

Then, in Power Automate:

1. Go to **Custom connectors** and find **Instafill.ai**.
2. Create a new **connection** and enter your API key as `Bearer YOUR_API_KEY`.
3. Create a new **flow**, add an Instafill.ai trigger or action, and configure it.

---

## Example workflows

### Get notified in Teams when a form is filled

**Instafill.ai** (When an event occurs → form_filled) → **Microsoft Teams** (Post Message)

Post a message with the filled PDF link to your team channel the moment filling finishes.

### Auto-save filled forms to SharePoint

**Instafill.ai** (When an event occurs → form_filled) → **SharePoint** (Create File)

Keep filled forms organized in a document library without manual downloads.

### Automatically convert flat PDFs from incoming emails

**Outlook** (When a new email arrives) → **Instafill.ai** (Check if PDF Is Flat) → **Condition** (Only continue if flat) → **Instafill.ai** (Convert PDF)

Detect and convert flat PDFs from incoming emails.

### Track every fill in Excel

**Instafill.ai** (When an event occurs → form_filled) → **Excel Online** (Add a Row)

Keep a record of every filled form with timestamps and download links.

---

## How conversion works in Power Automate

Conversion is asynchronous. When you trigger **Convert PDF**, Instafill.ai starts processing and returns a job ID immediately — the converted form isn't ready yet.

There are two ways to handle this in a flow:

- **Use the When an event occurs trigger** (event type: `form_converted`) in a separate flow. This trigger fires automatically when conversion finishes, so you don't need to poll for the result.
- **Add a Get Conversion Status step** after Convert PDF to check whether the job is complete before continuing the flow.

For most workflows, the trigger is the simpler option.

---

## Troubleshooting

### API key is invalid

Copy the full key from [Manage API Keys](https://instafill.ai/settings/workspace/api). If the key was lost before copying, generate a new one — existing keys can't be retrieved. Make sure to include the `Bearer ` prefix (with a space) when entering the key.

### Trigger is not firing

Make sure the flow is turned on and the correct event type is selected. Check flow run history for errors.

### Convert PDF returns a job ID but no form

Conversion runs asynchronously. Use **Get Conversion Status** to poll the result, or use the **When an event occurs** trigger (event type: `form_converted`) in a separate flow.

### Connection test fails

Verify that the API key is correct and prefixed with `Bearer `. Check that your Instafill.ai account is active.

---

## FAQ

### Do I need a paid plan?

API access is available on all paid Instafill.ai plans. Power Automate requires a Premium license to use custom connectors.

### Are there limits on how many flows I can create?

No limits from Instafill.ai. Power Automate plan limits apply.

### Does it work with multi-page PDFs?

Yes. Each conversion or fill counts as one operation regardless of how many pages the PDF has.

### Will more triggers and actions be added?

Yes. The integration is actively expanding alongside the Instafill.ai API.

---

## Support

- **Instafill.ai** — chat widget at [instafill.ai](https://instafill.ai) or email [support@instafill.ai](mailto:support@instafill.ai)
- **Power Automate** — [Microsoft Power Automate support](https://make.powerautomate.com)
- **API docs** — [docs.instafill.ai](https://docs.instafill.ai)
