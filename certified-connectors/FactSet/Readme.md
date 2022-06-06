# FactSet

Prepare for client meetings using the FactSet connector to access key company information in a custom branded, presentation ready PDF. Stay on top of relevant news and events for companies of interest and highlight important topics to drive your discussion with FactSet Signals. Alternatively, use Digital Cards to collaborate with colleagues and send summaries of actionable insights via Teams.

### Publisher: FactSet Research Systems

### Pre-Requisites

You will need the following to proceed

* A Microsoft Power Automate plan with premium connector access
* FactSet API Access

### Accessing FactSet APIs

FactSet's API Program is designed to support digital transformation journeys through flexible access to data in a secure and consistent manner. To learn more about FactSet APIs and gain access to them, please click here : [FactSet Developer Portal](https://developer.factset.com/learn/getting-started)

### Supported Operations

This connector supports the following operations

* `Signals - Get Headlines` : Fetch Signals event headlines based on the filtering criteria
* `Signals - Get Details` : Fetch Signals event headlines plus all additional event details based on the filtering criteria
* `Signals - Adaptive Card` : Fetch Microsoft's Adaptive Cards, which includes headlines and event details data plus hyperlinks to FactSet reports, based on the filtering criteria
* `NER - Entities` : Extract named entities from document text.
* `BookBuilder - Get Book List` : Retrieves the list of books that were previously created and are available in the client's book library
* `BookBuilder - Get Template List` : A template is a predefined list of content to be compiled in a PDF. Templates need to defined/created in FactSet workstation.
* `BookBuilder - Create Book from Template` : This endpoint retrieves book status, book name, and book ID for ticker requested in JSON format.
* `BookBuilder - Get PDF` : This endpoint will return the PDF output given a book_id.

### FactSet Support

Please contact this email address in case of issues : WorkflowAutomation@factset.com