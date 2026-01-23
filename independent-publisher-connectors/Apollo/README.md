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
- **Reveal Personal Emails**: Include personal email addresses in results (default: false)
- **Reveal Phone Numbers**: Include phone numbers in results (default: false)

### Enrich Account
Enrich an organization by domain. Returns comprehensive company details including industry, employee count, revenue, funding history, technology stack, and departmental headcount breakdown.

**Inputs:**
- **Domain** (required): Company website domain (e.g., microsoft.com)

### Bulk Enrich Contacts
Enrich multiple contacts in a single request. Match by email, or by first name, last name, and domain combination.

**Inputs:**
- **Queries** (required): Array of contact queries containing email or name/domain combinations
- **Reveal Personal Emails**: Include personal email addresses in results (default: false)
- **Reveal Phone Numbers**: Include phone numbers in results (default: false)

### Bulk Enrich Accounts
Enrich multiple organizations in a single request by domain.

**Inputs:**
- **Queries** (required): Array of objects containing domain values

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

1. Trigger: When a new response is submitted (Microsoft Forms)
2. Action: Enrich Contact (Apollo Enrichment)
   - Email: Form response email field
   - Reveal Phone Numbers: true
3. Action: Create item (SharePoint)
   - Map Apollo outputs (name, title, company, phone) to SharePoint columns

### Example: Enrich Leads in Dataverse (Dynamics 365 / RapidStart CRM)

Use this connector to automatically enrich leads and accounts in Dataverse-based CRM applications like Dynamics 365 Sales or RapidStart CRM.

1. Trigger: When a row is added (Microsoft Dataverse)
   - Table: Leads
   - Scope: Organization
2. Condition: Check if email is not empty
3. Action: Enrich Contact (Apollo Enrichment)
   - Email: Lead email address
   - Reveal Phone Numbers: true
4. Action: Enrich Account (Apollo Enrichment)
   - Domain: Extract domain from email (use expression: `split(triggerOutputs()?['body/emailaddress1'], '@')[1]`)
5. Action: Update a row (Microsoft Dataverse)
   - Table: Leads
   - Row ID: Lead ID from trigger
   - Job Title: `person.title`
   - Company Name: `organization.name`
   - Business Phone: `person.phone_numbers[0].sanitized_number`
   - No. of Employees: `organization.estimated_num_employees`
   - Industry: `organization.industry`
   - LinkedIn: `person.linkedin_url`

This pattern works with any Dataverse-based application, allowing sales teams to focus on selling rather than manual data entry.

## Known Issues and Limitations

- **Rate Limits**: Apollo API has rate limits based on your subscription tier. Implement appropriate delays when processing large batches.
- **Credit Usage**: Each enrichment request consumes API credits. Revealing phone numbers uses additional credits.
- **Bulk Limits**: Bulk operations support a maximum of 10 queries per request.
- **Match Quality**: Not all lookups will return results. Email-based matching typically has higher success rates than name/domain matching.
- **Data Freshness**: Apollo data is updated regularly but may not reflect very recent job changes.

## API Documentation

For detailed API documentation, visit: https://apolloio.github.io/apollo-api-docs/

## Frequently Asked Questions

**Q: What Apollo subscription do I need?**
A: API access is available on paid Apollo plans. Check Apollo's pricing page for current plan details and API access levels.

**Q: How do I know if a lookup was successful?**
A: Successful lookups return a person or organization object. If no match is found, these objects will be null or empty.

**Q: Can I use this connector with Power Apps?**
A: Yes, this connector works with both Power Automate and Power Apps.

**Q: What happens if I exceed my API rate limit?**
A: Apollo will return a 429 error. Implement retry logic with exponential backoff for production flows.

## Deployment Instructions

Please use the instructions on [Microsoft Power Platform Connectors](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate.
