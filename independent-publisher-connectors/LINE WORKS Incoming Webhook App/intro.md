# LINE WORKS Incoming Webhook (Independent Publisher)

LINE WORKS is a business chat and collaboration service that is widely used in Japan. This connector posts notification messages to LINE WORKS talk rooms through the LINE WORKS Incoming Webhook app, so flows can send alerts and updates without an API app or tokens.

## Publisher: iwaohig

## Prerequisites

- A LINE WORKS tenant on the Free, Standard or Advanced plan.
- The Incoming Webhook app installed from the LINE WORKS App Directory, with a webhook URL added for the target talk room. For setup steps, see the [Incoming Webhook documentation](https://developers.worksmobile.com/jp/docs/webhook).

## Obtaining Credentials

This connector does not use authentication. Each request is routed by the webhook ID, which is the last part of the webhook URL (`https://webhook.worksmobile.com/message/{webhookId}`). Anyone who has the webhook ID can post to the talk room, so treat it as a secret.

## Supported Operations

### Send message

Sends a message to the talk room that is linked to the webhook ID. The message has an optional title, the message text (which can mention members with `<m userId="user@example.com">`), and an optional link button with a label and a URL.

## Example Scenarios

- Send monitoring alerts and deployment results to an operations talk room.
- Notify a sales team about new orders or inquiries.
- Remind members about approvals or timesheet deadlines.

## Known Issues and Limitations

- A tenant can add up to 5 webhook URLs.
- The rate limit is 120 requests per minute per LINE WORKS domain, shared by all webhook URLs. Requests over the limit return HTTP 429.
- The Incoming Webhook app sends text messages only. It cannot send images or files, or use other message types.
