## Cradl AI

Cradl AI is a no-code AI platform for automating internal document workflows. Cradl AI enables you to:

🚀️ Create customized AI models for any document type n any Latin-based language. <br />
👍 Deploy a fully fledged *human-in-the-loop* validation UI with one click <br />
🎉️ Automatically re-train and improve your model based on feedback from end users.<br />

## Prerequisites

Before you begin, make sure that you have a Cradl AI account. If you don't have one, sign up for free [here](https://app.cradl.ai/signup).

## Quick start

This quick start guide aims to provide a basic overview how Cradl AI can be integrated in a Power Automate workflow. Please refer to the [official documentation](https://docs.cradl.ai/) for up-to-date documentation.

#### 1. Set up a flow

*Cradl AI Flows* enables you to automate internal document processes in a simple, effective and unified way. Furthermore, it reduces the risk of using AI models in production since you can send uncertain documents to a *human-in-the-loop* when necessary. By adjusting your flow's *confidence thresholds*, you can decide when a document should be sent to manual verification and when it's allowed to pass straight through. To create a new flow, go to [Flows -> New flow](https://app.cradl.ai/flows).

### 2. Create a connection

If you have not done so already, go to [API -> New API key](https://app.cradl.ai/appclients) to create API credentials. Copy the `clientId` *and* `clientSecret`to the *Client Credentials* field in the connection. The `clientId` and `clientSecret` should be separated by a colon:

*Client Credentials:*

> \<clientId\>:\<clientSecret\>

### 3. Set up a webhook trigger

The easiest way to retreive the processed documents in Power Automate is by setting up a webhook trigger. Open your Cradl AI Flow and, in the *Export* section, select *Webhook* from the list of available integrations.

## Supported Operations

The connector supports the following operations:

* `Parse document with human-in-the-loop`: Parse a document with *Flows*. This operation runs asynchronous.
* `Parse document`: Parse a document by calling the model directly. This operation runs synchronously.

## Documentation

For more thorough guides and detailed documentation, please refer to the [official documentation](https://docs.cradl.ai/).
