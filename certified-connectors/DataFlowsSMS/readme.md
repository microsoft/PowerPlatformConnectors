# DataFlows SMS Connector

DataFlows SMS is an Australian bulk SMS platform that enables businesses to send SMS messages, receive inbound SMS, and track delivery status. This connector integrates DataFlows SMS with Microsoft Power Automate, Power Apps, and Logic Apps.

## Publisher: DataFlows Pty Ltd

## Prerequisites

- A DataFlows SMS account ([Sign up here](https://sms.dataflows.com.au/register))
- API Token from Developer Settings
- SMS credits in your account

## Supported Operations

### Actions

| Operation | Description |
|-----------|-------------|
| **Send SMS** | Send SMS message to single or multiple recipients with optional scheduling |
| **Get SMS Status** | Get details and delivery status of a specific SMS message |
| **List SMS Messages** | Get a paginated list of all SMS messages for your account |
| **Get Account Balance** | Retrieve current SMS credit balance |
| **Get Account Profile** | Get account profile information |
| **Get Available Sender IDs** | Get list of active sender IDs available for the account |

### Triggers

| Trigger | Description |
|---------|-------------|
| **When an SMS is received** | Triggers when an inbound SMS is received on your virtual number |
| **When SMS delivery status changes** | Triggers when an outbound SMS delivery status changes (sent, delivered, failed) |

## Obtaining Credentials

1. Log in to your DataFlows SMS account at [sms.dataflows.com.au](https://sms.dataflows.com.au)
2. Navigate to **Developer** > **HTTP API**
3. Copy your **API Token**
4. Use this token when creating a connection in Power Automate

## Getting Started

### Send an SMS

1. Add the **Send SMS** action to your flow
2. Enter the recipient phone number (with country code, e.g., 61412345678)
3. Select a Sender ID from the dropdown
4. Choose message type (plain or unicode)
5. Enter your message content

### Receive SMS Trigger

1. Add the **When an SMS is received** trigger to start your flow
2. The trigger will fire automatically when an SMS arrives at your virtual number
3. Use the dynamic content (From Number, Message Content, etc.) in subsequent actions

## Known Issues and Limitations

- Maximum 160 characters per SMS for plain text (longer messages split into multiple parts)
- Sender ID must be pre-approved for alphanumeric IDs
- Rate limit: 600 requests per minute
- Australian numbers require +61 country code format

## Frequently Asked Questions

### How do I get a virtual number for receiving SMS?
Contact DataFlows support at support@dataflows.com.au to provision a virtual number for your account.

### What SMS gateways are supported?
DataFlows supports multiple carriers including Twilio, Plivo, ClickSend, and direct Australian carrier connections.

### How much does each SMS cost?
Pricing varies by destination. Check your account dashboard for current rates.

## Deployment Instructions

This connector is available in the Power Platform connector gallery. Search for "DataFlows SMS" when adding a connection.

For custom deployment:
1. Download the connector files
2. Import via Power Platform admin center
3. Configure the connection with your API token

## Support

- **Email**: support@dataflows.com.au
- **Website**: [dataflows.com.au](https://dataflows.com.au)
- **Documentation**: [sms.dataflows.com.au/developers/http-docs](https://sms.dataflows.com.au/developers/http-docs)
