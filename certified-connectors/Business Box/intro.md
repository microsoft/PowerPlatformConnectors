# Business Box

[Business Box](https://marubox.jp) is online booking software for small businesses in Japan: salons, clinics, studios, tutors and consultants publish a booking page, take payments and send reminders in Japanese or English. This connector lets you list event types and open times, create and cancel bookings, send single use booking links, and start a flow the moment a booking is created, rescheduled, cancelled or marked as a no show.

## Publisher: App Dev Plus G.K.

## Prerequisites

- A Business Box account. The API and webhooks are available on every plan, including the free plan. Sign up at [marubox.jp](https://marubox.jp).
- A Business Box API key (see Obtaining Credentials).
- A Power Automate, Power Apps or Logic Apps plan that includes premium connectors.

## Supported Operations

### When a booking event occurs

Trigger. Starts the flow when a booking is created, rescheduled, cancelled or marked as a no show. Choose one or more events, or `*` for every event. The trigger output is the full booking, and for a reschedule also the previous start and end time. Paid bookings fire `booking.created` only after payment succeeds.

### List event types

Lists the event types (the things customers can book), with name, duration, price, visibility and booking link.

### List available start times

Lists the start times that can be booked for an event type between two dates (at most 31 days), after working hours, calendar conflicts, buffers, notice and daily limits are applied.

### List bookings

Lists bookings, newest first, filtered by status, event type, customer email, start time or last change time. Supports paging with Cursor.

### Get a booking

Gets one booking by its ID.

### Create a booking

Books a time for a customer, exactly as adding a booking by hand in Business Box does. The customer receives a confirmation email unless Send Confirmation is set to No. No card payment is taken. A time that is no longer free fails with `409 slot_unavailable`; Business Box never double-books.

### Cancel a booking

Cancels a booking. The customer is emailed, and a paid booking is refunded in full. Cancelling an already cancelled booking returns it unchanged.

### Create a single use booking link

Creates a booking link that works once and then expires (after 14 days by default). Useful for replying to an enquiry from email, a form or a CRM.

### Get the connected account

Returns the account the API key belongs to and the permissions the key carries. Used to test the connection.

## Obtaining Credentials

The connector uses a personal API key.

1. Sign in to Business Box and open **Settings → Integrations → API keys**.
2. Name the key (for example "Power Automate") and choose what it may do:
   - **Read only**: list event types, list and read bookings, check availability.
   - **Read and write**: everything above plus create and cancel bookings, create booking links and subscribe to webhooks. The trigger and the create and cancel actions need this.
3. Select **Create key** and copy the key straight away. It starts with `bbk_` and is shown only once.
4. When you create the connection, paste the key into **API key**. Paste only the key; the connector sends it as `Authorization: Bearer <key>`.

Keys do not expire. Revoke a key in the same screen to disconnect it immediately.

## Getting Started

1. Add the **When a booking event occurs** trigger and choose `booking.created`.
2. Add an action from another connector, for example post the customer name, event type and start time to a Teams channel, or add a row to an Excel table.
3. Make a test booking on your Business Box booking page. The flow runs within seconds.

Times are ISO 8601 in UTC. Use the **Convert time zone** action to show them in Japan time (Tokyo Standard Time).

## Known Issues and Limitations

- Each API key may make 120 requests per minute. Above that the service returns `429` with a `Retry-After` header.
- An account can have at most 25 webhook subscriptions. Each flow that uses the trigger uses one subscription while it is turned on.
- Webhook deliveries can arrive more than once and out of order. Use the Event ID output to ignore duplicates.
- A subscription whose deliveries fail 20 times in a row over at least three days is switched off and the account owner is emailed. It can be switched back on in **Settings → Integrations → Webhooks**.
- Money amounts are integers in the currency's minor unit. Japanese yen has no minor unit, so `5000` means ¥5,000.
- The connector cannot edit event types, availability or the account profile; those are not part of the public API.
- **Create a booking** does not take card payments, even for paid event types.

## Frequently Asked Questions

### Which permissions does the key need?

Read only is enough for the list and get actions. The trigger, **Create a booking**, **Cancel a booking** and **Create a single use booking link** need a Read and write key. A call without the needed permission fails with `403 insufficient_scope`.

### Where is the API reference?

At [marubox.jp/developers](https://marubox.jp/developers/). The OpenAPI document is at https://api.marubox.jp/v1/openapi.json.

### How do I get help?

See [marubox.jp/support](https://marubox.jp/support) or email support@marubox.jp.

## Resources

- Developer documentation: https://marubox.jp/developers/
- Webhooks: https://marubox.jp/developers/webhooks/
- Authentication and API keys: https://marubox.jp/developers/authentication/
- Support: https://marubox.jp/support
