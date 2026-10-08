# looot
looot gives a flow one key and one prepaid balance for more than 2,500 data endpoints from 90+ providers: work emails, phone numbers, company and people search, Google results, web pages, news and local businesses. The connector searches the catalog, shows the price before a run, and runs an endpoint or a job. A failed call costs nothing.

## Publisher: Walid Boulanouar

## Prerequisites
You need a looot account with a prepaid balance (top up from $5, no subscription) and an agent token.

## Supported Operations
### Search the catalog
Finds endpoints by what you want to do, in plain words. Free. Returns endpoint IDs, providers, categories and estimated prices.

### Get endpoint details
Reads the exact inputs, output and price of an endpoint or a job. Free. Call it before Run an endpoint.

### Run an endpoint
Runs an endpoint, or a job written as `job:people.email.find` so looot picks the provider. It spends prepaid credit. Set an idempotency key and the same key with the same input never pays twice. Set Wait (seconds) to wait for the result, or leave it at 0 and read the result later with Get a run.

### Get a run
Reads one run: status, result and cost.

### Get the balance
Reads the available credit, the reserved credit and the top-up link. Free.

## Obtaining Credentials
The connector uses an API key sent in the `Authorization` header.
1. Sign in at [looot.ai](https://looot.ai) and open the dashboard.
2. Go to Settings, Agent tokens, and create a token. Tick `catalog.read`, `runs.read`, `runs.execute` and `usage.read`.
3. When Power Automate asks for the key, enter `Bearer ` followed by the token, for example `Bearer YOUR_TOKEN`.

## Getting Started
1. Create a connection with your agent token.
2. Run Search the catalog with a query such as `verify an email address`.
3. Run Get endpoint details with an `endpointId` from the search answer, read the input fields and the price.
4. Run Run an endpoint with that `endpointId` and an `input` object that uses the field names from step 3.

## Known Issues and Limitations
- The `result` of a run depends on the endpoint, so its fields are not listed in the response schema. Use the Parse JSON action with a sample from Get endpoint details, or read the fields in a dynamic expression.
- A run that waits longer than the Wait (seconds) value returns a run ID and a `queued` or `running` status. Poll it with Get a run.
- Runs spend real credit. The price is shown by Get endpoint details before you run.

## Frequently Asked Questions
### What does a failed call cost?
Nothing. looot charges only for calls that return a result.

### Where is the full API documentation?
At [docs.looot.ai](https://docs.looot.ai). Support is at [looot.ai/contact](https://looot.ai/contact).
