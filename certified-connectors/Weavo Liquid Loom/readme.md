# Weavo Liquid Loom
The Weavo Liquid Loom connector is a powerful tool designed to streamline data conversion and transformation processes across different formats. Utilizing the [Liquid Template Language](https://shopify.github.io/liquid/), this connector enables precise and custom data transformations, ensuring seamless integration and communication within automated workflows.

## Publisher: Weavo

## Prerequisites
Before you begin, ensure you have the following:

- A valid Microsoft Power Apps or Power Automate plan with custom connector capabilities or an Azure subscription with access to Azure Logic Apps.
- An active Weavo Liquid Loom subscription for your organization and an ApiKey which can be generated through the Weavo Liquid Loom home page: https://www.weavo.net/signup-for-liquid-loom. A 'forever free plan' is available.

## Supported Operations

### JSON to JSON
Transform a JSON object to another JSON object using a Liquid Template.

### JSON to XML
Transform a JSON object to an XML object using a Liquid Template.

### JSON to Text
Transform a JSON object to text using a Liquid Template. Same as the two above, but without any validation on output type.

### XML to JSON
Transform a XML object to a JSON object using a Liquid Template.

### XML to XML
Transform a XML object to another XML object using a Liquid Template.

### XML to Text
Transform a XML object to text using a Liquid Template. Same as the two above, but without any validation on output type.

### CSV to JSON
Transform a CSV to a JSON object using a Liquid Template.

### CSV to XML
Transform a CSV to an XML object using a Liquid Template.

### CSV to Text
Transform a CSV to text using a Liquid Template. Same as the two above, but without any validation on output type.

## Obtaining Credentials
- An active Weavo Liquid Loom subscription for your organization and an ApiKey which can be generated through the Weavo Liquid Loom home page: https://www.weavo.net/signup-for-liquid-loom. A 'forever free plan' is available.

## Known Issues and Limitations
No known issues or limitations at the moment.

## Deployment Instructions
* In the [Power Automate portal](https://make.powerautomate.com/), create a new flow or edit an existing one. 

* Add a new action to your flow and in the "Choose an operation" menu, search for "Weavo Liquid Loom" under the Premium tab. 

* Select your preferred action.

* You will be prompted to supply an ApiKey you create before. This Connection will be saved by Power Automate, and available for use in future flows.

* To build your liquid template, you can use the [Liquid Loom Generator](https://www.weavo.dev) to generate a template, test it, and then copy it into the Liquid Template field in the connector.
