# LINE WORKS Incoming Webhook (Independent Publisher)

LINE WORKS is a business chat and collaboration service. This connector posts notification messages to LINE WORKS talk rooms through the LINE WORKS Incoming Webhook app. Each message has a title and a body, and can include a link button.

## Publisher: iwaohig

## Prerequisites

- A LINE WORKS tenant on the Free, Standard or Advanced plan.
- The Incoming Webhook app installed from the LINE WORKS App Directory, with a webhook URL added for the target talk room. For setup steps, see the [Incoming Webhook documentation](https://developers.worksmobile.com/jp/docs/webhook).

## Obtaining Credentials

This connector does not use authentication. Each request is routed by the webhook ID, which is the last part of the webhook URL:

```
https://webhook.worksmobile.com/message/{webhookId}
```

Anyone who has the webhook ID can post to the talk room, so treat it as a secret. If it leaks, delete the webhook URL in LINE WORKS and add a new one.

## Supported Operations

### Send message

Sends a message to the talk room that is linked to the webhook ID.

| Parameter | Required | Description |
|---|---|---|
| Webhook ID | Yes | The ID at the end of the webhook URL. |
| Title | No | The message title. If it is empty, the title set for the webhook in LINE WORKS is used. |
| Message Text | Yes | The message text. To mention a member, use `<m userId="user@example.com">`. |
| Button Label | No | The text shown on the link button. Set it together with Button URL. |
| Button URL | No | The URL that opens when the link button is selected. Set it together with Button Label. |

On success, the operation returns `code` (200) and `description` (`success`).

## Known Issues and Limitations

- A tenant can add up to 5 webhook URLs.
- The rate limit is 120 requests per minute per LINE WORKS domain, shared by all webhook URLs. Requests over the limit return HTTP 429.
- The Incoming Webhook app sends text messages only. It cannot send images or files, or use other message types.
- Messages are posted by the Incoming Webhook app, not by a member, and members cannot reply to the app.
