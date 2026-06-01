<a href="https://flotiq.com/">
    <img src="https://editor.flotiq.com/fonts/fq-logo.svg" alt="Flotiq logo" title="Flotiq" align="right" height="60" />
</a>

# Flotiq Connector for Microsoft Power Platform

[Flotiq](https://flotiq.com) is an API-first, headless Content Management System (CMS) that enables developers and content creators to manage and publish content through automatically generated REST APIs. It supports low-code and no-code integrations across various platforms.

This connector lets you easily integrate Flotiq with Microsoft services such as **Power Apps**, **Logic Apps**, or **Power Automate**, allowing seamless data flow between Flotiq and other systems.

---

## Prerequisites

Before you begin, make sure you have:

- A [Flotiq account](https://editor.flotiq.com/register?utm_campaign=flotiq_powerapps_connector&utm_medium=referral&utm_source=microsoft) (free plan available)
- At least one **Content Type Definition (CTD)** created in Flotiq
- An API key with access to the CTD
- Access to Microsoft Power Apps or a Microsoft Azure subscription (for Logic Apps)

---

## Getting Started

1. **Create a Content Type Definition in Flotiq**  
   Follow the guide in [our documentation](https://flotiq.com/docs/panel/content-types/#creating-content-type-definitions) or start with a ready-made example like `"Event"`:

   ![Creating content type in Flotiq](https://api.flotiq.com/image/0x0/_media-bd6699ad-8940-4315-92aa-898ce0a4d091.png)

2. **Generate a scoped API key**  
   Navigate to the API Keys section in Flotiq and [create a new key](https://flotiq.com/docs/API/#user-defined-api-keys).  
   We recommend limiting its scope to only the CTD and operations (e.g., Create, Update) required by the connector.

   ![Scoped API keys in Flotiq](https://api.flotiq.com/image/0x0/_media-12fd9d37-408a-4b1c-b138-ff71296c2811.png)

3. **Copy the API key**  
   You'll use this key to authorize requests from Power Platform to Flotiq.

---

## Using the Connector in Logic Apps

1. **Find and authorize the connector**  
   In Logic Apps, search for "Flotiq" among available connectors.  
   Paste your API key when prompted to authorize the connection.

   ![flotiq connector authorization](https://api.flotiq.com/image/0x0/_media-4eb8ec6b-ae57-4efb-9a0a-28ec1acbe700.png)

2. **Use the `Create Content Object` action**  
   Choose your target Content Type Definition from the dropdown.  
   Once selected, a dynamic form will appear, letting you map all the CTD’s fields.

   ![Creating event in Flotiq from outlook](https://api.flotiq.com/image/0x0/_media-cb00a2f9-e895-4378-9ec4-52e35670f1ce.png)

3. **Continue building your Logic App**  
   You can now connect Flotiq with other Microsoft services in your workflow.

---

Got questions? Visit [flotiq.com](https://flotiq.com) or reach out via our support channels.
