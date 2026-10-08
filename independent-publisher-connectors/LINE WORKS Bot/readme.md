## LINE WORKS Bot Connector

This connector enables you to send messages to LINE WORKS users and chat rooms using the LINE WORKS Bot API with OAuth 2.0 authentication.

## Publisher: iwaohig

## Prerequisites

Before using this connector, you need to:

1. Have a LINE WORKS administrator account with Developer permissions
2. Create a Bot in LINE WORKS Developer Console and complete setup in Admin Console

### Creating a Bot in LINE WORKS

To create and configure a Bot:

1. **Add Bot to Tenant**
   - Follow the instructions at: https://developers.worksmobile.com/jp/docs/bot#step-one-add-to-tenant

2. **Add Bot to Domain**
   - Complete the setup by following: https://developers.worksmobile.com/jp/docs/bot#step-two-add-to-domain

3. **Note Important Information**
   After Bot creation, note down:
   - Bot ID

### OAuth 2.0 Configuration

For OAuth 2.0 authentication setup, refer to:
https://developers.worksmobile.com/jp/docs/auth

When configuring the connector in Power Platform:
1. The system will generate a redirect URL automatically
2. Copy this redirect URL and add it to your LINE WORKS Bot's OAuth 2.0 settings
3. Ensure the required scope `bot` is selected for sending messages

## Obtaining Credentials

The connector uses OAuth 2.0 authentication. You'll need:

- **Bot ID**: Found in your Bot's basic information
- **Client ID**: Found in Developer Console Client App configuration  
  See: https://developers.worksmobile.com/jp/docs/developer-console#configuration-page

## Supported Operations

This connector currently supports **text messages only**. For details, see: https://developers.worksmobile.com/jp/docs/bot-send-text

### Send Message to User
Send a text message to a specific LINE WORKS user.

**Parameters:**
- Bot ID: Your Bot's ID
- User ID or Email: The recipient's user ID or email address
- Message: The text message to send (max 2,000 characters)

### Send Message to Channel
Send a text message to a LINE WORKS channel (chat room).

**Parameters:**
- Bot ID: Your Bot's ID
- Channel ID: The target channel's ID
- Message: The text message to send (max 2,000 characters)

## API Limits

For message sending rate limits and other API restrictions, refer to:
https://developers.worksmobile.com/jp/docs/rate-limits

Message size limit: 2,000 characters

## Known Issues and Limitations

1. **Current Version Limitations:**
   - Only text messages are supported
   - No support for images, files, or rich message templates (planned for future versions)
   - No support for receiving messages (this is an action-only connector)

2. **Authentication:**
   - OAuth 2.0 token expiration is handled automatically

3. **User Identification:**
   - When sending to users, you can use either the user ID or email address
   - The Bot must have permission to send messages to the user

## Getting Channel IDs

To get a channel ID:
1. Add your Bot to the target channel
2. Use LINE WORKS user interface or API to retrieve channel information
3. Channel IDs are in the format: `7e7f0b1c-c3b6-4e59-3dcf-3c284ab95d99`

## Troubleshooting

### Common Errors:

1. **401 Unauthorized**
   - Check if your OAuth credentials are correct
   - Ensure the Bot has the required `bot` scope

2. **403 Forbidden**
   - Verify the Bot is added to the target channel
   - Check if the Bot has permission to message the user

3. **404 Not Found**
   - Confirm the user ID/email or channel ID is correct
   - Ensure the target exists in your LINE WORKS organization

4. **429 Too Many Requests**
   - You've exceeded the rate limit
   - Wait for the limit to reset (hourly)

## Future Enhancements

Planned features for future versions:
- Image and file message support
- Rich message templates
- Sticker support
- Message with buttons and actions
- Bulk messaging capabilities

## Support

For connector-specific issues, contact: iwaohig@gmail.com

For LINE WORKS Bot API documentation, visit: https://developers.worksmobile.com/jp/docs/bot-api