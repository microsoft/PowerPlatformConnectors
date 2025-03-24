# German Federal Parliament

Access German Federal Parliament legislative data, documents, and activities.

## Publisher: Dan Romano (swolcat)

## Prerequisites

To use this connector, you will need:

- A **Microsoft Power Apps** or **Power Automate** plan that includes custom connector capabilities.
- Personal or public API key. See "Obtaining Credentials."

## Obtaining Credentials

- No authentication is required. The API is publicly available via API key for 2025: I9FKdCn.hbfefNWCY336dL6x62vfwNKpoN2RZ1gp21
- To obtain your own license key that is valid for ten years, submit an email request to the service: parlamentsdokumentation@bundestag.de

## Supported Operations

This connector supports the following operations:

### Parliamentary Processes
- **Retrieve all parliamentary processes** – Get a list of all legislative processes in the Bundestag.
- **Retrieve a specific parliamentary process** – Fetch metadata for a specific legislative process.

### Process Steps
- **Retrieve all process steps** – Get a list of all steps within legislative processes.
- **Retrieve a specific process step** – Fetch details of a specific step within a legislative process.

### Parliamentary Documents
- **Retrieve all parliamentary documents** – Get a list of all documents (Drucksachen) related to legislation.
- **Retrieve a specific document** – Fetch metadata for a specific parliamentary document.

### Plenary Protocols
- **Retrieve all plenary protocols** – Get a list of all Bundestag plenary sessions.
- **Retrieve a specific plenary protocol** – Fetch details of a specific plenary session.

### Activities
- **Retrieve all parliamentary activities** – Get a list of all recorded activities in the Bundestag.
- **Retrieve a specific parliamentary activity** – Fetch metadata for a specific activity.

### Persons
- **Retrieve all persons** – Get a list of all politicians and relevant individuals in the Bundestag.
- **Retrieve a specific person** – Fetch metadata for a specific person.

## Getting Started

1. **Add the custom connector** – Import the provided Swagger 2.0 definition into Power Automate or Power Apps.
2. **Set up a flow or app** – Use the available actions to fetch Bundestag data based on legislative processes, documents, activities, or persons. Use this API key for the entire year of 2025: I9FKdCn.hbfefNWCY336dL6x62vfwNKpoN2RZ1gp21
3. **Use the results** – Connect responses to your Power BI reports, automate notifications, or integrate with your workflows.
4. **Create your own API key by submitting an email request to the service: parlamentsdokumentation@bundestag.de. They are valid for ten years.
5. Additional resources:
	- [Bundestag API Swagger Documentation](https://search.dip.bundestag.de/api/v1/swagger-ui/)
	- [Microsoft Power Automate Custom Connectors](https://learn.microsoft.com/en-us/connectors/custom-connectors/)
	- [API General Info](https://dip.bundestag.de/%C3%BCber-dip/hilfe/api)
	- [Official PDF Documentation via the Bundestag API](https://dip.bundestag.de/api/v1/docs/pdf)

## Known Issues and Limitations

- **Data Availability** – Some older documents or legislative processes may not be available.
- **Rate Limits** – The Bundestag API may enforce request limits for large queries.
- **Pagination Handling** – Responses use a cursor-based pagination system for large datasets.

## Frequently Asked Questions (FAQ)

### Does this API require authentication?
No. The Bundestag API is publicly accessible and does not require authentication. Use this API key for 2025: I9FKdCn.hbfefNWCY336dL6x62vfwNKpoN2RZ1gp21. You can apply for own API key by sending an email to parlamentsdokumentation@bundestag.de.

### How often is the data updated?
The data is updated based on parliamentary activities, which means some documents may have delays before appearing in the API.

### Can I use this in Power BI?
Yes. You can integrate the Bundestag API with Power BI using Power Automate to fetch and structure the data. Power BI can transform the array of objects into a tabular format for visualization.
