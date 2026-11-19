# Tomba

Tomba finds, verifies, and enriches professional email addresses, phone numbers, and company data. Use this connector to search a domain for contacts, find a person's email address, check that an address can receive mail, and look up company profiles and technologies.

## Publisher: Tomba technology web service LLC

## Prerequisites

You need a [Tomba account](https://app.tomba.io/) with an API key and secret. A free plan is available; limits and credits depend on your plan.

## Supported Operations

### Find email

#### Search domain

Finds the email addresses Tomba has for a company, by domain or company name.

#### Find email address

Finds a person's professional email address from their name and their company's domain or name.

#### Find article author

Finds the email address of an article's author from the article's URL.

#### Find email from LinkedIn profile

Finds the email address that belongs to a LinkedIn profile.

#### Get email count

Counts the email addresses Tomba has for a domain.

#### Get email format

Returns the patterns a domain uses to build its email addresses.

#### Get email sources

Lists the web pages where Tomba found an email address.

### Verify

#### Verify email address

Checks whether an email address can receive mail.

#### Get domain status

Checks whether a domain belongs to a webmail or disposable email provider.

### Enrich

#### Enrich email address

Returns the person and company behind an email address.

#### Enrich person

Returns a person's profile from their email address.

#### Enrich company

Returns a company's profile from its domain.

#### Enrich person and company

Returns the person and company profiles for an email address in one call.

### Phone

#### Find phone number

Finds a phone number from an email address, a domain, or a LinkedIn profile.

#### Validate phone number

Checks whether a phone number is valid and returns its formats, line type, carrier, region, and time zones.

### Companies

#### Search companies

Searches Tomba's company database with a text query, structured filters, or both.

#### Find similar companies

Finds companies similar to a domain or a company name.

#### Get website technologies

Lists the technologies a website uses.

#### Suggest companies

Suggests companies, with their domains and logos, for a partial company name.

#### Get employee locations

Counts a company's employees by country.

### Account

#### Get API usage

Returns your API usage.

#### Get request logs

Returns your recent API requests.

### Leads and flags

#### List leads

Lists your leads, optionally from one lead list.

#### Create lead

Adds a lead to one of your lead lists.

#### List flags

Lists the reports of incorrect data you've submitted, with their status.

#### Flag incorrect data

Reports incorrect data, such as an email address that bounced.

## Obtaining Credentials

The connector authenticates with an API key and an API secret:

1. Sign in to Tomba and open the [API keys](https://app.tomba.io/api) page.
2. Create a key, or copy an existing one.
3. Copy your secret from the same page.
4. When you create a connection, enter both values.

See [Authentication](https://docs.tomba.io/authentication) for key expiry and rotation.

## Getting Started

1. Add a Tomba action to your flow and create a connection with your key and secret.
2. Start with **Find email address**: enter a domain, such as `stripe.com`, and a person's first and last name.
3. Pass the address to **Verify email address** to check that it can receive mail.

## Known Issues and Limitations

- **A successful response can mean no result.** Check for this before you use the result:
    - **Find email address** and **Find email from LinkedIn profile** return an empty `email`.
    - **Enrich person**, **Enrich company**, and **Enrich person and company** set the `errors` field.
    - **Find phone number** and **Validate phone number** can return `data` as an empty array instead of an object. Test it with an expression such as `empty(body('Find_phone_number')?['data'])` before you read its fields, because reading a field of an array can fail the step.
- **Validate phone number can fail on a bad number.** A number with fewer than 7 digits, or whose country can't be determined, fails with status 422, so handle that failure in your flow.
- **Only one result from LinkedIn and phone lookups.** The API's `full` option returns an array of results, which the connector can't describe alongside the single-result form, so these actions don't offer it.
- **Empty fields show warnings in the test pane.** When a number, boolean, or array field is `null`, the test pane reports a type mismatch against `string`. The field is correct, and flows receive the `null`.
- **Actions use your Tomba credits.** Most lookups cost credits, and **Enrich Mobile** costs extra. See [Credits and usage](https://docs.tomba.io/usage-and-quotas).
- **Rate limits are shared.** Every key and connection on an account counts toward the same limits, which depend on your plan. A request over a limit fails with status 429. See [Rate limits](https://docs.tomba.io/rate-limits).
- **Long requests can time out.** Tomba recommends a client timeout of at least 180 seconds, because searches and verifications run in real time. Power Automate limits outbound synchronous requests to 120 seconds. For large volumes, use [bulk jobs](https://docs.tomba.io/bulks) in the Tomba dashboard.
- **Create lead requires an email address and a list.** The API accepts leads without them, but the connector requires both.
- **Account details and API keys aren't available.** The connector leaves out these endpoints because they return your API secret and keys, which would appear in flow run history.

## Frequently Asked Questions

### Why does the connection ask for two values?

Tomba needs both the API key and the API secret on every request. The connector sends them in the `X-Tomba-Key` and `X-Tomba-Secret` headers.

### Does the connector support OAuth?

Not at this time. Connections use an API key and secret.

## Deployment Instructions

Run these commands from this folder with the [Power Platform Connectors CLI](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy the connector as a custom connector:

```sh
paconn login
paconn create --api-prop apiProperties.json --api-def apiDefinition.swagger.json --icon icon.png
```

`paconn` can't update a connector whose `apiProperties.json` has a `stackOwner` property. To update the connector later, deploy from a copy of the file without it.
