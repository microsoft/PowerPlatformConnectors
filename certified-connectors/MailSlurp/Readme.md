# MailSlurp Connector for Power Automate

The MailSlurp connector brings powerful email and SMS testing capabilities directly into your Power Automate flows. It allows you to create inboxes, receive OTPs, read messages, and automate verification steps without writing code. This connector is ideal for QA teams, citizen developers, and enterprise automation builders who need to test workflows that depend on email or SMS delivery.

> Get your free [API KEY here](https://app.mailslurp.com/)

## What MailSlurp Provides

MailSlurp is a hosted email and SMS API platform used by more than 100,000 developers and enterprise teams. It enables fully automated email and SMS interactions for testing, monitoring, onboarding, login flows, and business processes.

## Key Features
Build, test, and automate SMS and email using MailSlurp's powerful connector features:

### Create inboxes instantly
Generate real email inboxes on demand for any workflow. Perfect for sign-up flows, password resets, and account provisioning.

### Automate OTP and verification tests
Receive emails or SMS messages, extract OTP codes, and feed them into your automated workflows without manual steps.

### Wait for messages during a flow
Use wait operations to pause your flow until an email or SMS arrives. Great for testing multi-step verification.

### Read and parse messages
Get subject, body, attachments, and metadata. MailSlurp can extract important values like tokens, links, or numeric codes.

### Temporary or persistent inboxes
Use inboxes for short-lived test runs or create long-term inboxes for business processes.

### Enterprise-grade reliability
Hosted on AWS across multiple regions with secure storage, rate limits, and high throughput. Supports API keys, audit logs, and compliance needs.

## Why Power Automate Users Benefit

Power Automate has limited built-in capabilities for handling dynamic email and SMS verification flows. MailSlurp fills this gap so that you can automate real world scenarios reliably.

Perfect for:
  - Testing and validating sign-up flows
  - Verifying password reset functionality
  - Simulating new user onboarding
  - Automating internal QA and UAT processes
  - Monitoring login flows using OTP codes
  - Integrating with Power Apps, Dynamics, and SharePoint apps
  - Building end-to-end RPA workflows that depend on email or SMS delivery

## Benefits for Power Automate teams
MailSlurp will make your QA, testing, and automations faster and more reliable:

### No more manual testing
Stop creating test accounts by hand. MailSlurp automates inbox creation and email/SMS retrieval.

### Works without writing code
Everything is drag-and-drop. No JSON parsing or HTTP calls required.

### Fast and scalable
Create thousands of inboxes and run parallel test flows.

### Secure
Use Power Automate connections to store your API key securely.

### Supports both email and SMS
Automate any verification channel your app uses.

## Typical Use Cases
Here is how you can use MailSlurp today:

### 1. Automated sign-up testing
  - Create inbox
  - Fill form with inbox address
  - Wait for OTP
  - Extract OTP
  - Submit OTP
  - Validate success

### 2. Password reset scenarios
  - Trigger reset
  - Wait for reset email
  - Parse reset token
  - Follow reset link
  - Assert outcome

### 3. Compliance and monitoring
  - Confirm transactional emails are being delivered
  - Validate templates
  - Ensure login flows are stable

### 4. RPA and integration flows
  - Integrate with internal apps that use email-based approval
  - Test and automate customer onboarding paths

## Getting Started

To use this connector you will need:

  1.	A MailSlurp [account](https://app.mailslurp.com)
  2.	An [API key](https://app.mailslurp.com) (stored securely in your connection settings)
  3.	A flow that needs email or SMS automation

The connector exposes actions to create inboxes, wait for emails or SMS messages, read message content, and manage inboxes.

## Documentation

Full API documentation is available at: [https://docs.mailslurp.com](https://docs.mailslurp.com)

For advanced use cases, you can combine Power Automate with [MailSlurp SDKs](https://docs.mailslurp.com/sdks/) in C Sharp, Python, Java, or TypeScript.
