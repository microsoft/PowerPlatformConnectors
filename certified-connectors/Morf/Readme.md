## Morf Connector
Morf is a lightweight API-driven platform for automatically generating forms that can easily be embedded into existing systems, apps, and sites. Morf provides a REST API that can be used to dynamically generate user interfaces and capture information to power your digital processes.

## Publisher: AFTIA Solutions

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with a custom connector feature
* The Power platform CLI tools
* A Morf API Key

## Supported Operations
The connector currently supports the following operations:

### Render (V1)
Render a new Morf data capture experience. Call this API with a Morf form definition and optionally data to receive a prefilled form document that can be presented to a user in a browser context.

The operation accepts:

* `Form`: A Morf form definition (JSON)
* `Data`: *(Optional)* A JSON data payload used to prefill the form

and returns:

* `Form`: A portable rendered form document (HTML) that can be rendered in browser environments.

### Transform JSONata (V1)
Transform JSON data using JSONata. Call this API with a JSON payload and a [JSONata](https://jsonata.org/) query or transformation expression to receive the modified data or query result

The operation accepts:

* `JSON Data`: A JSON data payload being submitted for querying or transformation
* `JSONata Expression`: A [JSONata](https://jsonata.org/) query or transformation expression to execute against the JSON data payload

and returns:

* `Result`: Result of the expression execution containing a string or a stringified object (JSON).

## Obtaining Credentials
To get started, head over to our [Morf Editor](https://editor.getmorf.io/) and request access keys. You will be granted one (1) site and one (1) API key. As described in our [authentication documentation](https://github.com/aftialabs/morf-docs/blob/main/guides/Authentication.md), use the provided API key when configuring your new Power Platform connection. Our free usage terms are available [here](https://github.com/aftialabs/morf-docs/blob/main/guides/termsandconditions.md#product-description).

## Getting Started
1. To get started, start by creating a Morf form definition. This can be done by using our [Morf Editor](https://editor.getmorf.io/) and creating a new form from scratch or by converting an existing document.
2. With your form definition handy, you can now create a new Power Platform Flow leveraging the Morf Render operation.
3. Configure a new connection if one is not present by adding your Morf API key to the connection configuration. 
4. Configure the action to pass your form definition and optionally data to it, and after invoking your flow you will receive a rendered Morf form.
5. This form can be sent to a user by using many different delivery mechanisms or it can be presented to a user directly using the HTTP Response action.
6. For more information on submitting a form to a Power Platform Flow, have a look at our [documentation](https://github.com/aftialabs/morf-docs/blob/main/guides/PowerAutomate.md).

## Known Issues and Limitations
* Free tier users may experience slower response times during periods of increased usage due to provisioning standards

## Frequently Asked Questions

### How are new forms created?
This can be done by using our [Morf Editor](https://editor.getmorf.io/) and creating new forms from scratch or by converting existing documents.

### Can Morf forms be embedded?
Absolutely. Morf forms can be embedded into any web property without the use of inline frames (`iframe`). Get started by heading over to our [Morf Editor](https://editor.getmorf.io/) to generate an embed tag. 

### What makes Morf forms different?
Morf forms use a concept called generational interfaces which allows form definitions to be dynamically modified in real-time when calling our render APIs. This allows for advanced dynamic behavior as part of render logic and operations.

### How do I submit a Morf form?
Morf forms can be submitted to any API that accepts a POST request. Information on submissions can be found [here](https://github.com/aftialabs/morf-docs/blob/main/guides/Submission.md).

### Can Morf forms be submitted to a Power Platform Flow?
Yes! Follow this [guide](https://github.com/aftialabs/morf-docs/blob/main/guides/PowerAutomate.md) to set up your own submission flow.

## Deploying the connector
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```