# kwtSMS for Microsoft Power Automate

[![OpenAPI Validation](https://github.com/boxlinknet/kwtsms-mspowerauto/actions/workflows/validate.yml/badge.svg)](https://github.com/boxlinknet/kwtsms-mspowerauto/actions/workflows/validate.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Power Automate](https://img.shields.io/badge/Power%20Automate-Independent%20Publisher-blue)](https://learn.microsoft.com/en-us/connectors/custom-connectors/certification-submission-ip)
[![kwtSMS](https://img.shields.io/badge/kwtSMS-SMS%20Gateway-FFA200)](https://www.kwtsms.com)

## About kwtSMS

[kwtSMS](https://www.kwtsms.com) is a Kuwait-based SMS gateway that delivers messages to 200+ countries. Built for businesses that need reliable A2P (Application-to-Person) messaging: OTP verification, transactional alerts, marketing campaigns, and notifications. Full Arabic language support, pre-approved sender IDs, and a straightforward REST JSON interface.

## About this integration

A Microsoft Power Automate custom connector that brings kwtSMS into your automated workflows. Send SMS messages directly from Power Automate flows triggered by events in SharePoint, Dynamics 365, Outlook, Teams, or any of the 1,000+ connectors in the Power Platform ecosystem.

Submitted as an [Independent Publisher](https://learn.microsoft.com/en-us/connectors/custom-connectors/certification-submission-ip) connector.

**Publisher:** kwtSMS

## Features

- **Send SMS** - Send messages to one or up to 200 recipients in a single action. Supports English, Arabic, and Unicode content.
- **Check Balance** - Monitor your available SMS credits and total purchased credits from within your flows.
- **List Sender IDs** - Retrieve all approved sender IDs on your account for dynamic sender selection.
- **Check Coverage** - List active country codes to verify delivery availability before sending.
- **Validate Numbers** - Validate phone numbers in bulk. Returns numbers grouped as valid (OK), format error (ER), or no route (NR).
- **Test Mode** - Built-in test mode on Send SMS. Messages are queued but not delivered, credits are recoverable.

## Prerequisites

You need a kwtSMS account with API access enabled. [Sign up at kwtsms.com](https://www.kwtsms.com).

## Obtaining Credentials

1. Log in to your kwtSMS account at [kwtsms.com](https://www.kwtsms.com)
2. Your API credentials are the same username and password you use to log in
3. You need at least one approved Sender ID. The default testing sender is `KWT-SMS`

**Note:** Your API username is NOT your mobile number. Use the account username and password.

## Supported Operations

| Operation | Description |
|-----------|-------------|
| **Send SMS** | Send a message to one or more recipients (max 200 per request). Optional test mode. |
| **Check Balance** | Get available and total purchased SMS credits. |
| **List Sender IDs** | Get all approved sender IDs for your account. |
| **Check Coverage** | List active country codes available for sending. |
| **Validate Numbers** | Validate phone numbers. Returns OK, ER (format error), NR (no route) arrays. |

## Important Notes

- Phone numbers must be in international format, digits only (for example `96598765432`). No `+` prefix, no spaces, no dashes.
- Sender ID is case sensitive. `KWT-SMS` is for testing only, register a private sender ID for production use.
- English messages: 160 characters per page. Arabic messages: 70 characters per page.
- Messages must not contain emojis or HTML tags.
- Maximum 200 numbers per Send SMS request. For larger lists, split into multiple requests.
- The `unix-timestamp` in responses uses GMT+3 (Asia/Kuwait) server time, not UTC.
- Set test mode to 1 during development. Messages are queued but not delivered, and credits can be recovered by deleting from the sending queue in your kwtSMS dashboard.

## Known Issues and Limitations

- Delivery reports (DLR) are not available for Kuwait numbers, only for international numbers.
- The default `KWT-SMS` sender ID may experience delays and is blocked on some carriers. Use a private sender ID for production.
- There is a 15-second minimum between messages to the same phone number (ERR028).

## Links

- [kwtSMS Website](https://www.kwtsms.com)
- [kwtSMS Integrations](https://www.kwtsms.com/integrations.html)
- [kwtSMS Support](https://www.kwtsms.com/support.html)
- [Power Automate Custom Connectors](https://learn.microsoft.com/en-us/connectors/custom-connectors/)
- [Independent Publisher Program](https://learn.microsoft.com/en-us/connectors/custom-connectors/certification-submission-ip)

