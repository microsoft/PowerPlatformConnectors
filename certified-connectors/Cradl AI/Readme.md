## Cradl AI

Cradl AI is a no-code AI platform for automating internal document workflows. Cradl AI enables you to:

🚀️ Create customized AI models for any document type n any Latin-based language. <br />
👍 Deploy a fully fledged *human-in-the-loop* validation UI with one click <br />
🎉️ Automatically re-train and improve your model based on feedback from end users.<br />

## Publisher: Cradl AI

## Prerequisites

A free Cradl AI account. If you don't already have one, you can sign up for free [here](https://app.cradl.ai/signup).

## Supported Operations

This connector supports the following operations:

### Create document
Create a new document.

### Parse document with human-in-the-loop

Parse a document with *Flows*. This operation runs asynchronous.

### Parse document

Parse a document by calling the model directly. This operation runs synchronously.

## Obtaining Credentials

Log into Cradl AI, then go to [API -> New API key](https://app.cradl.ai/appclients) to create API credentials. Create a new Power Automate connection and copy `Credentials` to the *Client Credentials* field.

## Getting Started

This quick start guide aims to provide a basic overview how Cradl AI can be integrated in a Power Automate workflow. Please refer to the [official documentation](https://docs.cradl.ai/) for up-to-date documentation.

#### 1. Set up a flow

*Cradl AI Flows* enables you to automate internal document processes in a simple, effective and unified way. It reduces the risk of using AI models in production since you can send uncertain documents to a *human-in-the-loop* when necessary. By adjusting your flow's *confidence thresholds*, you can decide when a document should be sent to manual verification and when it's allowed to pass straight through. To create a new flow, go to [Flows -> New flow](https://app.cradl.ai/flows).

### 2. Configure a webhook trigger

The easiest way to retreive the processed documents in Power Automate is by setting up a webhook trigger. Open your Cradl AI Flow and, in the *Export* section, select *Webhook* from the list of available integrations. Copy the HTTP POST URL from your webhook trigger in Power Automate and paste it into the Webhook integration in Cradl AI.

## Known Issues and Limitations

See [API Limits](https://docs.cradl.ai/reference/quotas).

## Frequently Asked Questions

### Which document formats are supported?

JPEG, PNG, PDF and TIFF.

### How many models do I need?

One per _document process_. For example, if you want to automate an expense approval process where you process receipts, invoices and airline tickets, we recommend using one model even if you process multiple document types with different layouts.

### Where is my data stored?

Please refer to our to our [Data Processing Agreement](https://docs.cradl.ai/legal/dpa) and [Privacy Policy](https://docs.cradl.ai/legal/privacy-policy) for more information about how personal data is processed.

## Deployment Instructions

Refer the documentation [here](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.
