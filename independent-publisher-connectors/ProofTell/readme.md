# ProofTell
ProofTell checks a phone number, an email address and an IP address, one at a time or together as a 0 to 100 risk score with a verdict and its reasons. This connector puts those checks in a flow: look up a phone number at one of five levels, from a free basic check to a fraud score; verify an email address; look up an IP address; or assess the risk of an identity from any mix of the three. Every check is charged from your ProofTell balance; the sandbox is free.

## Publisher: ProofTell

## Prerequisites
A ProofTell account, created at [prooftell.com](https://prooftell.com/integrations?utm_source=power-automate&utm_medium=integration), and an API key from its dashboard. There is no plan or licence to buy: each live check is charged from the account's balance, and a sandbox key is free.

## Supported Operations
### Look up a phone number
Checks a phone number at the level you choose, each level including the ones before it: Basic (free: valid or not, the formats and the country), Line type (the default: the line type and the carrier the number was allocated to), Current carrier (the carrier the number is on today and whether it was ported), Line status (whether the subscriber is on the network right now) and Fraud score (a 0 to 100 score with its risk level and the number's fraud flags).
### Verify an email address
Verifies an email address: whether it is deliverable, the result and its reason, the flags (disposable, role account, free provider, catch-all) and a likely correction of a misspelt domain. A syntax error, an unknown and a soft bounce are free.
### Look up an IP address
Looks up an IP address: whether it is a Tor exit node, a VPN or a datacenter address, and its country, region, city and network. A private or reserved address is answered and free.
### Assess the risk of an identity
Scores a phone number, an email address and an IP address together, any one of them at least: a 0 to 100 score, a verdict (allow, review or deny) against your organization's thresholds, the reasons that added to the score and each signal's own result. Charged for the signals that answered.
### Get the balance
Returns the account's balance in US dollars (paid, promotional and held amounts) and what this month's checks cost so far. Free; it is also how a connection is tested.

## Obtaining Credentials
1. Sign in to the ProofTell dashboard and open Developers, then Integrations.
2. Create a key named after this platform, for example "Power Automate", and choose its mode: a `pt_test_` key is the free sandbox (simulated, deterministic answers); a `pt_live_` key spends the account's balance.
3. Copy the key: it is shown once.
4. When the connector asks for the API key, paste the key itself: `pt_live_…` or `pt_test_…`.

## Getting Started
Add an action of the connector to a flow, for example "Look up a phone number" after a form submission, pass the phone number from the trigger, and leave Level on Line type. The outputs (Valid, Line type, Country code, Original carrier and the rest) are then available to the next steps. To try the actions without spending anything, connect with a sandbox key and use the documented inputs: `+447700900123` (a valid GB mobile), `jane@example.com` (deliverable), `203.0.113.1` (a clean US address) and `203.0.113.66` (a Tor exit node).

## Known Issues and Limitations
- A sandbox (`pt_test_`) key answers every check with simulated, deterministic results and never contacts a network or a mail server; Get the balance answers the real balance with either key. Use a `pt_live_` key for real answers.
- Every live check is charged from the account's balance at the moment it runs; Basic is free but needs a balance above zero. When the balance cannot cover a check the action fails with 402 `insufficient_balance` and names the amounts to top up.
- The rate limit is about 20 requests a second per key on each server instance, with bursts of 50. Past it an action fails with 429 `rate_limited` and `retry_after` 1: retry after a second (Power Automate's retry policy does this by default).
- Checking a whole file of numbers or addresses is not part of this connector: use the Files page of the ProofTell dashboard.

## Deployment Instructions
Run `paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json` with the [Power Platform Connectors CLI](https://github.com/microsoft/PowerPlatformConnectors/tree/dev/tools/paconn-cli): the properties file carries the policy that sends the key as `Authorization: Bearer <key>`. Importing `apiDefinition.swagger.json` alone in the portal (Custom connectors, New custom connector, Import an OpenAPI file) works too: then add that policy yourself in the Definition step (New policy, Set HTTP header, name `Authorization`, value `Bearer @connectionParameters('api_key')`, action override, on the request). Create a connection with your key and test it with "Get the balance".
