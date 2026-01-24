# Apollo Enrichment

Apollo.io is a sales intelligence platform with a database of over 275 million verified B2B contacts. This connector provides access to Apollo's enrichment APIs, allowing you to look up detailed information about people and companies directly within your Power Automate flows.

## Publisher: Steve Mordue

[Steve Mordue](https://github.com/forceworks) | [Forceworks](https://forceworks.com)

## Prerequisites

- An Apollo.io account ([sign up here](https://www.apollo.io/sign-up))
- An Apollo API key with appropriate permissions

## Supported Operations

### Enrich Contact
Enrich a single contact by email address. Returns person details including name, job title, phone numbers, LinkedIn profile, and current organization information.

**Inputs:**
- **Email** (required): Email address to match
- **Reveal Personal Emails** (advanced): Include personal email addresses in results (default: false)
- **Reveal Phone Numbers** (advanced): Include phone numbers in results (default: false)

### Enrich Account
Enrich an organization by domain. Returns comprehensive company details including industry, employee count, revenue, funding history, technology stack, and departmental headcount breakdown.

**Inputs:**
- **Domain** (required): Company website domain (e.g., microsoft.com)

### Bulk Enrich Contacts
Enrich up to 10 contacts in a single API call.

**Inputs:**
- **Contacts** (required): Array of objects, each containing an `email` property
- **Reveal Personal Emails** (advanced): Include personal email addresses in results (default: false)
- **Reveal Phone Numbers** (advanced): Include phone numbers in results (default: false)

**Example input:**
```json
[
  {"email": "john@microsoft.com"},
  {"email": "jane@google.com"},
  {"email": "bob@apple.com"}
]
```

### Bulk Enrich Accounts
Enrich up to 10 organizations in a single API call.

**Inputs:**
- **Domains** (required): Array of domain strings

**Example input:**
```json
["microsoft.com", "google.com", "apple.com"]
```

## Individual vs Bulk Operations

| Factor | Individual | Bulk |
|--------|------------|------|
| API calls | 1 per record | 1 per 10 records |
| HTTP overhead | Higher | Lower |
| Rate limit efficiency | Less efficient | More efficient |
| Credits consumed | Same (1 per record) | Same (1 per record) |
| Error handling | Per-record control | All or nothing |
| Complexity | Simpler | Requires array handling |

**When to use Individual:**
- On-demand enrichment (button click on a form)
- Per-record error handling needed
- Simple flows without batch processing

**When to use Bulk:**
- Scheduled batch enrichment jobs
- Importing/processing lists
- When minimizing API calls matters

## Obtaining Credentials

1. Log in to your Apollo.io account
2. Navigate to **Settings** → **Integrations** → **API Keys**
3. Click **Create New Key**
4. Give your key a descriptive name
5. Copy the API key and store it securely

For more information, see [Apollo's API documentation](https://apolloio.github.io/apollo-api-docs/).

## Getting Started

1. Create a new flow in Power Automate
2. Add a new action and search for "Apollo"
3. Select the desired operation (Enrich Contact, Enrich Account, etc.)
4. When prompted, create a new connection by entering your Apollo API key
5. Configure the input parameters and use the outputs in subsequent flow steps

### Example: Enrich a Contact from a Form Submission

```
Trigger: When a new response is submitted (Microsoft Forms)
    ↓
Action: Enrich Contact (Apollo Enrichment)
    - Email: Form response email field
    - Reveal Phone Numbers: true
    ↓
Action: Create item (SharePoint)
    - Map Apollo outputs (name, title, company, phone) to columns
```

### Example: Enrich New Leads in Dataverse

```
Trigger: When a row is added (Microsoft Dataverse)
    - Table: Leads
    - Scope: Organization
    ↓
Condition: Email is not empty
    ↓
Action: Enrich Contact (Apollo Enrichment)
    - Email: Lead email address
    ↓
Action: Enrich Account (Apollo Enrichment)
    - Domain: split(triggerOutputs()?['body/emailaddress1'], '@')[1]
    ↓
Action: Update a row (Microsoft Dataverse)
    - Job Title: body('Enrich_Contact')?['person']?['title']
    - Company Name: body('Enrich_Account')?['organization']?['name']
    - Business Phone: first(body('Enrich_Contact')?['person']?['phone_numbers'])?['sanitized_number']
    - No. of Employees: body('Enrich_Account')?['organization']?['estimated_num_employees']
    - Industry: body('Enrich_Account')?['organization']?['industry']
    - LinkedIn: body('Enrich_Contact')?['person']?['linkedin_url']
```

### Example: Batch Enrich Accounts (Bulk)

```
Trigger: Scheduled (Recurrence)
    ↓
Action: List rows (Dataverse)
    - Table: Accounts
    - Filter: fw_lastenriched eq null
    - Top count: 10
    ↓
Action: Select
    - From: outputs('List_rows')?['body/value']
    - Map: Extract domain from websiteurl
    ↓
Action: Bulk Enrich Accounts (Apollo Enrichment)
    - Domains: body('Select')
    ↓
Action: Apply to each
    - Loop through results and update accounts
```

## Building Arrays for Bulk Operations

**For Bulk Enrich Accounts (simple string array):**

In the Domains field, switch to code view and enter:
```json
["microsoft.com", "google.com", "apple.com"]
```

Or use a Select action to extract domains from a Dataverse query.

**For Bulk Enrich Contacts (array of objects):**

Use a Select action:
- **From:** Your array of contacts
- **Map:** `{"email": "@{item()?['emailaddress1']}"}`

Or use Compose:
```json
[
  {"email": "john@example.com"},
  {"email": "jane@example.com"}
]
```

## Known Issues and Limitations

- **Rate Limits**: Apollo API has rate limits based on your subscription tier. Implement appropriate delays when processing large batches.
- **Credit Usage**: Each enrichment request consumes API credits. Revealing phone numbers and personal emails uses additional credits.
- **Bulk Limits**: Bulk operations support a maximum of 10 records per request. Use `chunk()` function to process larger lists.
- **Match Quality**: Not all lookups will return results. Email-based matching typically has higher success rates.
- **Data Freshness**: Apollo data is updated regularly but may not reflect very recent job changes.
- **Plan Restrictions**: Search endpoints (People Search, Organization Search, Job Postings, News) require higher-tier Apollo plans.

## Chunking Large Lists

To process more than 10 records with bulk operations:

```
Compose: chunk(body('Select'), 10)
    ↓
Apply to each: outputs('Compose')
    ↓
    Bulk Enrich Accounts
        - Domains: items('Apply_to_each')
    ↓
    Delay: 1 second (avoid rate limits)
```

## API Documentation

For detailed API documentation, visit: https://apolloio.github.io/apollo-api-docs/

## Frequently Asked Questions

**Q: What Apollo subscription do I need?**
A: Enrichment endpoints are available on most paid Apollo plans. Search endpoints require higher tiers. Check Apollo's pricing page for current details.

**Q: How do I know if a lookup was successful?**
A: Successful lookups return a `person` or `organization` object. If no match is found, these will be null.

**Q: Can I use this connector with Power Apps?**
A: Yes, this connector works with both Power Automate and Power Apps.

**Q: What happens if I exceed my API rate limit?**
A: Apollo returns a 429 error. Implement retry logic with exponential backoff, or add Delay actions between calls.

**Q: Why use Bulk instead of a loop with Individual?**
A: Bulk reduces HTTP overhead and is more efficient with rate limits. Use it for batch jobs. Individual is simpler for on-demand, single-record enrichment.

## Deployment Instructions

Please use the instructions on [Microsoft Power Platform Connectors](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate.
