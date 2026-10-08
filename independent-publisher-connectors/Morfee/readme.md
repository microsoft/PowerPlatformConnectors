# Morfee

Morfee turns a saved HTML template plus JSON into a finished PDF document: a quote, an invoice, a work order, a certificate. Paste your HTML once, point at the fields that come from your data, and every call after that is one POST request.

## Publisher: furo solutions

## Prerequisites
Use of the service requires an account at [Morfee](https://morfee.furo.solutions/sign-up). A 14-day trial is available with no card required.

## Obtaining Credentials
Once signed in, go to the API keys page at [morfee.furo.solutions/keys](https://morfee.furo.solutions/keys) and create a key. A key belongs to one organization and reaches nothing outside it. The key is shown once, so store it when it is created.

## Supported Operations
### List templates
Every template in your library, with its id, name and how many fields it has. Start here: you need a template id to render a document.
### Render a document
Renders one document from a saved template and your data. With the default response format the answer is a JSON object holding a download link, valid for an hour, which is what a workflow tool wants: a link survives being passed between steps, raw PDF bytes do not.

## Known Issues and Limitations
Making a template (pasting the HTML and pointing at the fields) happens in the Morfee builder at [morfee.furo.solutions](https://morfee.furo.solutions), not through this connector: that step needs a person looking at the document, not an API call. This connector renders documents from templates already saved there.
