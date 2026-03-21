# Highspot MCP Connector

The Highspot MCP Connector leverages the Model Context Protocol (MCP) to provide a secure, seamless bridge between Microsoft 365 Copilot and Highspot. By integrating Highspot directly into your AI workflows, your team can access critical sales content, knowledge, insights, and actions without leaving their conversation.

## Publisher: Highspot

## Prerequisites

To use this connector, you need:

1. An active Highspot account with API access
2. OAuth 2.0 credentials (Client ID and Client Secret) from your Highspot account
3. Appropriate permissions configured in Highspot for MCP access

## Obtaining Credentials

1. Log in to your Highspot account
2. Navigate to your account settings or API configuration section
3. Register a new OAuth 2.0 application
4. Note down the Client ID and Client Secret
5. Configure the redirect URI as provided during connection setup

## Supported Operations

### Invoke Highspot MCP

Invokes the Highspot MCP endpoint to interact with Highspot through the Model Context Protocol.

**Parameters:**
- Request body containing MCP protocol messages

**Returns:**
- MCP protocol response from Highspot

## Known Issues and Limitations

- This connector requires valid OAuth 2.0 credentials from Highspot
- The MCP protocol is used for AI agent interactions

## Frequently Asked Questions

### How do I get my Client ID and Client Secret?

Contact your Highspot administrator or visit the Highspot developer portal to register an OAuth application and obtain your credentials.

### What scopes are required?

The connector requires `mcp:read` and `mcp:write` scopes for full functionality.

## Deployment Instructions

Please use the instructions provided in the [Power Platform Connectors CLI](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector.

## Support

For support, please contact:
- **Email:** support@highspot.com
- **Website:** https://www.highspot.com/contact/

## Legal

- **Privacy Policy:** https://www.highspot.com/privacy/
- **Terms of Service:** https://www.highspot.com/terms/

