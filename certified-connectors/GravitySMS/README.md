# Gravity SMS

Send and receive SMS messages through RingCentral using the Gravity SMS connector for Microsoft Power Automate, Power Apps, and Logic Apps.

## Prerequisites

- A [Gravity SMS](https://gravitysms.com) account with an active subscription
- A RingCentral account connected through the Gravity SMS dashboard
- An API key from the Gravity SMS dashboard

## Getting your API key

1. Sign in to your Gravity SMS dashboard at [app.gravitysms.com](https://app.gravitysms.com)
2. Go to **Integrations** and activate the **Power Automate** integration
3. Copy the API key — you'll use this when creating a connection in Power Automate

## Supported operations

### Actions

| Operation                        | Description                                                        |
| -------------------------------- | ------------------------------------------------------------------ |
| **Send an SMS message**          | Queue an outbound SMS for delivery through your RingCentral number |
| **Get message delivery status**  | Check the current status of a sent message                         |
| **List message history**         | Retrieve a filtered, paginated list of SMS messages                |
| **Check RingCentral connection** | Verify your RingCentral account is connected and active            |

### Triggers

| Trigger                     | Description                                                  |
| --------------------------- | ------------------------------------------------------------ |
| **When an SMS is received** | Fires when an inbound SMS arrives on your RingCentral number |

## Creating a connection

When adding the Gravity SMS connector to a flow, you'll be prompted to enter your API key. Paste the key from your dashboard and the connection is ready to use.

## Known issues and limitations

- SMS messages are limited to 1,600 characters
- Phone numbers must be in E.164 format (e.g. +14155551234)
- The inbound SMS trigger requires a connected RingCentral account

## Support

- Website: [gravitysms.com](https://gravitysms.com)
- Documentation: [docs.gravitysms.com](https://docs.gravitysms.com)
- Privacy policy: [gravitysms.com/privacy](https://gravitysms.com/privacy)
- Terms of service: [gravitysms.com/terms](https://gravitysms.com/terms)
