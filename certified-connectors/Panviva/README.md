## Panviva Connector

Panviva is an omnichannel knowledge management solution. Our priority is to aid our clients in keeping their employees happy, so that they can focus on ensuring their customers are happy. Digitally transforming organisations for the last 20 years, we're now helping our customers to deliver knowledge nuggets to their employees and customers, whether they are in the home or the office - anytime and anywhere.

## Prerequisites

You will need the following to proceed:

- A Microsoft Power Apps or Power Automate plan with custom connector feature
- A Panviva subscription
- The Power Platform CLI tools

## Building the connector

Since the Panviva APIs are secured, you will need

1. Access to a Panviva instance (also known as a tenant)
2. A developer account on the Panviva developer portal [https://dev.panviva.com](https://dev.panviva.com)
3. An active Panviva API subscription (also known as an API plan) and valid Panviva API credentials
   After that is completed, you can create and test the connector.

### Deploying the sample

Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```

## Supported Actions

The connector supports the following actions:

- **Search Operations**
  - `Search`: Search documents, folders, and files (external documents) for a term and return paginated results
  - `Search Artefacts`: Return search results for a given query
- **Live Operations**
  - `Live CSH`: Perform a CSH keyword search and present the results on an active user's Panviva client
  - `Live Document`: Present a document on an active user's Panviva client
  - `Live Search`: Search and present the results on an active user's Panviva client
- **Document Operations**
  - `Document`: Return a document using the document ID provided
  - `Document Containers`: Return a list of containers using the document ID provided
  - `Document Container Relationships`: List parent-child relationships between the containers of a Panviva document
  - `Container`: Return a container using the container ID provided
  - `Document Translations`: List all translations (per language and locale) of a Panviva document
- **Resource Operations**
  - `Artefact`: Return an artefact using the ID provided
  - `Artefact Categories`: List all available artefact categories
  - `File`: Retrieve a file (external document) from Panviva
  - `Folder`: Return information about a Panviva folder and references to each of its direct children
  - `Folder Children`: Get all the immediate children of a Panviva folder, not including grandchildren. Children can be folders, documents, or files (external documents)
  - `Folder Translations`: Get all the translations of a Panviva folder, along with each translated folders respective children
  - `Folder Root`: Get the root/home folder in all of Panviva, which can be drilled into using the Get Folder Children endpoint. Note this endpoint was formerly referred to as the 'Folder Search' endpoint
  - `Image`: Retrieve an image from Panviva. Image data is represented as a base64 string
