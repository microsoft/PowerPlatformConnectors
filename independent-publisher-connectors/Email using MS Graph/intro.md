# Graph Mail API (Independent Publisher)

## Overview

The Graph Mail API connector enables users to send emails using the Microsoft Graph API. This connector provides seamless integration for sending emails from specified Microsoft 365 accounts directly within Power Automate, Azure Logic Apps, and Power Apps. 
By leveraging Microsoft Graph API, it facilitates efficient email delivery using a service principle and application authentication, making it ideal for automating communication workflows and notifications.

## Key Features

- **Send Emails:** Sends emails from a specified user account through Microsoft Graph, with control over email content, recipients, and message format.
- **Save to Sent Items:** Option to save sent emails in the user’s Sent Items folder for easy tracking and reference.
- **Secure Authentication:** Supports OAuth2 allowing to send emails without delegating to a user.

## Operations

### Send an Email
- **Summary:** Sends an email from a specified user account.
- **Endpoint:** `POST /users/{user-email}/sendMail`
- **Required Parameters:**
  - `user-email`: The email address of the user sending the email.
  - `Content-Type`: Specifies the format of the email content (default: `application/json`).
  - `message`: Email details, including subject, body (with support for both text and HTML content), recipients, and optional settings for saving the email to Sent Items.

### Security and Permissions
- **OAuth Scopes Required:** `Mail.Send`
- **Supported Authentication Flows:** OAuth2 authorization.

## Collaboration
If you’re interested in contributing or collaborating on this connector, feel free to reach out! This connector has been developed to simplify email automation tasks for users working in Power Platform and Azure ecosystems.

**Contact Information:** ian@tweed.technology
