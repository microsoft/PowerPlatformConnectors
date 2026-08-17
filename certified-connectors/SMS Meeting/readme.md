# SMS Meeting - Power Automate Connector

**SMS Meeting** allows you to automate SMS sending, notifications, reminders, and contact management directly from Microsoft Power Automate, Power Apps, and Logic Apps.

## Publisher: Ventus

## Prerequisites

- An active SMS Meeting account.
- An active SMS Meeting API subscription (paid plan) from the client portal: [my.sms-meeting.com](https://my.sms-meeting.com).
- An API key (Secret Key) generated in your client portal.

## Supported Operations

### Send an SMS (`CreateSms`)
Send an SMS to one or more recipients. You can specify the content, sender, scheduled date, etc.

### List all SMS (`GetAllSms`)
Retrieve the list of all sent or scheduled SMS messages.

### List all contacts (`GetContacts`)
Display the contacts available in your SMS Meeting account.

### Create a contact (`CreateContact`)
Add a new contact to your SMS Meeting address book.

### List templates (`GetTemplates`)
Show the list of available message templates.

### Use contact lists
Create, update, delete, and manage your contact lists for bulk sending.

## How to Get Your Credentials (API Key)

1. Log in to your client portal: [my.sms-meeting.com](https://my.sms-meeting.com).
2. Subscribe to the "SMS Meeting API" plan if you have not already done so.
3. Go to the **API Keys / Secret Key** section.
4. Generate a secret key for your license.
5. Enter this key when creating the connection in Power Automate.

## Getting Started

- Add the custom connector to your Power Platform environment.
- When first using, enter your API key ("Secret Key").
- Use the provided actions in your flows to automate SMS sending and management.

## Known Issues and Limitations

- This connector requires an active API subscription.
- The SMS sending quota depends on your plan.
- All required fields must be provided (especially the SMS type: `confirmation` or `reminder`).
- Advanced management (templates, contacts) requires the appropriate license permissions.

## Frequently Asked Questions

### Can I send an SMS to multiple numbers in a single action?
Yes, simply use the Power Automate loop feature and call the "Send SMS" action for each number.

### What should I do if my API key does not work?
Check that your API plan is active and that the key has not been revoked. Generate a new key from your client portal if necessary.

## Deployment Instructions

1. Add the `apiDefinition.swagger.json` and `apiProperties.json` files to the required folder.
2. Follow the Microsoft procedure to [submit a certified or custom connector](https://learn.microsoft.com/en-us/connectors/custom-connectors/submit-certification).
3. Once approved, the connector will be available in Power Automate for all your users.

---

Support & contact: [hello@sms-meeting.com](mailto:hello@sms-meeting.com)  
Website: [www.sms-meeting.com](https://www.sms-meeting.com)
