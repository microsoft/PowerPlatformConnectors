# TPSClear (Independent Publisher)

TPSClear screens UK phone numbers against the Telephone Preference Service (TPS) and Corporate TPS (CTPS) registers, the UK do-not-call lists that outbound callers must respect under PECR. This connector screens numbers in bulk from a flow, keeps your own suppression and consent lists next to the register checks, fetches past screening records as audit evidence, and reads your usage. Register data is supplied through our licensed data partners.

## Publisher: Steffen Hoyemsvoll

## Prerequisites

- A TPSClear account, created by magic link sign in at [tpsclear.co.uk/account](https://tpsclear.co.uk/account).
- An API key. A test key (`tpsc_test_...`) screens against fixture data for free and is enough to build and test a flow. A live key (`tpsc_live_...`) screens against register data and is funded by a prepaid credit pack or a plan on the TPSClear account behind it.

## Supported Operations

### Screen phone numbers

Screens up to 200 UK phone numbers in one call and returns a status per number: `clean`, `tps-listed`, `ctps-listed`, `unknown`, `suppressed` or `consented`. Each distinct valid UK number is one chargeable check. The record ID for the call is returned in the `X-Request-Id` response header.

### Get a screening record

Fetches a past screening call by its record ID, with the per number results as they were at the time of the call.

### Get usage

Returns the current fair use window for the key and rolling 30 day totals for the account. Read only and free; it is also the connection test.

### List suppressed numbers

Returns the size of your suppression list and its 100 most recent entries.

### Add numbers to the suppression list

Adds up to 10,000 numbers, each with an optional reason and the date and time of the opt out. A number on the list comes back from Screen phone numbers as `suppressed`, is not screened against the registers and is not charged.

### Remove a number from the suppression list

Takes one number off the suppression list, with an optional note.

### List consented numbers

Returns the size of your consent list and its 100 most recent entries.

### Add numbers to the consent list

Adds up to 10,000 numbers you hold consent to call, each with an optional reason (the consent evidence) and the date and time consent was given. A number on the list comes back from Screen phone numbers as `consented`, is not screened against the registers and is not charged. A number on both lists comes back `suppressed`.

### Remove a number from the consent list

Takes one number off the consent list when consent is withdrawn, with an optional note.

## Obtaining Credentials

1. Go to [tpsclear.co.uk/account](https://tpsclear.co.uk/account) and sign in with your email address. A magic link is sent; there is no password.
2. On the account page, create an API key. Choose a test key to build your flow for free, or a live key to screen against the registers.
3. Copy the key. It is shown once.
4. When creating a connection, paste the key on its own into the **API Key** field. The connector adds the `Bearer` prefix for you; do not type it.

The key is sent in the `Authorization` header on every call.

## Getting Started

1. Create a connection with your API key. The connection test calls Get usage, which is free.
2. Add **Screen phone numbers** to a flow and supply one or more numbers. A single number is a one item array.
3. Read each result's `status`. Only `clean` means the number was screened and is on neither register. Filter on `equals(item()?['status'], 'clean')` rather than on "does not contain listed", because that lets `unknown` through.
4. Keep the record ID. In a flow expression it is `outputs('Screen_phone_numbers')?['headers']?['X-Request-Id']`. Store it against the contact or the campaign; it addresses the stored record for Get a screening record.

With a test key, `+441134960002` comes back `tps-listed`, `+442079460018` comes back `ctps-listed` and `+441134960001` comes back `clean`. These are Ofcom drama ranges, so no real subscriber is behind them.

## Known Issues and Limitations

- **Batch size.** Screen phone numbers takes up to 200 numbers per call. For a larger list, split it with a Select or Chunk expression and call the action once per chunk.
- **Status `unknown` is not `clean`.** It covers both a number that is not a valid UK line and a register check that failed. Treat it as not screened.
- **Checks age.** The registers change daily. Screen a list again before calling if the check is 27 or more days old; the `checkedAt` field on each result is the time to measure from.
- **HTTP 429 has three meanings.** The `error` field is `rate_limited` when a retry after the `Retry-After` header will succeed. It is `allowance_reached` or `daily_cap_reached` when the account's plan allowance or daily cap is spent, and a retry will not clear those; the default retry policy on the action will retry anyway, so check the `error` field in a run that fails on 429.
- **Test keys are read only on lists.** The four add and remove operations on the suppression and consent lists return HTTP 403 `test_key_read_only` on a test key; the two list reads work on either. Use a live key for list changes; they are free and need no credits.
- **Limited keys.** A key created with only some scopes returns HTTP 403 `insufficient_scope` on the actions outside them, and a key with an IP allowlist returns 403 `ip_not_allowed` from any other address. Calls from this connector come from Microsoft data centre addresses, so use a key with no IP allowlist, or one that covers the Power Platform outbound addresses for your region.
- **List reads are capped.** List suppressed numbers and List consented numbers return the 100 most recent entries. The `count` field is the full size of the list.
- **No trigger.** The connector has actions only. It screens a list before a campaign and writes the verdict back; it does not act at the moment a call is dialled.
- **Fair use limits.** Each key has a lookup budget per window and a requests per minute limit. Get usage shows the window; a 429 with `rate_limited` carries `Retry-After`.

## Frequently Asked Questions

### Does a screen tell me whether I may call the number?

It tells you whether the number is on the TPS or CTPS register, and whether it is on your own suppression or consent list. Only `clean` means on neither register. Whether you may call also depends on the consent and legitimate interest rules in PECR for your situation, which the service does not judge.

### Which numbers are charged?

Each distinct valid UK number sent to Screen phone numbers is one chargeable check, unless it is on your suppression or consent list, in which case it is answered from the list for free. Duplicates in one call, numbers that are not valid UK numbers, and checks that failed on our side are not charged. Everything else in the connector is free.

## Deployment Instructions

Import `apiDefinition.swagger.json` as a custom connector (OpenAPI 2.0), then import `apiProperties.json` or recreate its single policy by hand: a **Set HTTP header** policy on the request, header `Authorization`, value `Bearer @connectionParameters('api_key')`, action override. Create a connection with a TPSClear API key and run Get usage to confirm it.

## API documentation

[tpsclear.co.uk/developers](https://tpsclear.co.uk/developers)
