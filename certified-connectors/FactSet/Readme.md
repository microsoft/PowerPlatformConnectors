# FactSet

Automate your workflows using the FactSet Connector to stay on top of relevant news and events for companies of interest, highlight important topics to drive your discussions, prepare for meetings, and more.

## Publisher: FactSet Research Systems

## Prerequisites

You will need the following to proceed

* A Microsoft Power Automate plan with premium connector access
* FactSet API Access

## Supported Operations

This connector supports the following operations

* `Signals - Get Headlines` : Fetch Signals event headlines based on the filtering criteria
* `Signals - Get Details` : Fetch Signals event headlines plus all additional event details based on the filtering criteria
* `Signals - Adaptive Card` : Fetch Microsoft's Adaptive Cards, which includes headlines and event details data plus hyperlinks to FactSet reports, based on the filtering criteria
* `NER - Entities` : Extract named entities from document text.
* `BookBuilder - Get Book List` : Retrieves the list of books that were previously created and are available in the client's book library
* `BookBuilder - Get Template List` : A template is a predefined list of content to be compiled in a PDF. Templates need to defined/created in FactSet workstation.
* `BookBuilder - Create Book from Template` : This endpoint retrieves book status, book name, and book ID for ticker requested in JSON format.
* `BookBuilder - Get PDF` : This endpoint will return the PDF output given a book_id.
* `Symbology - Get Symbology` : Translate Entity ID and market security symbols into FactSet Permanent Security Identifiers.

## Obtaining Credentials

FactSet's API Program is designed to support digital transformation journeys through flexible access to data in a secure and consistent manner. To learn more about FactSet APIs and gain access to them, please click here : [FactSet Developer Portal](https://developer.factset.com/learn/getting-started).

## Known Issues and Limitations

No known issues or limitations

## Frequently Asked Questions

### Do you have a support email to contact in case of issues?
Contact this email address in case of issues with the FactSet connector : WorkflowAutomation@factset.com

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate.