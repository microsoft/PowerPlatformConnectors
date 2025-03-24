## Pdfless Connector
With Pdfless, design beautiful document templates based on powerful of html/css then generate your own invoices, catalogs, complex documents with pagination and more… using payload data in JSON format.

## Prerequisites
You will need the following to proceed:
* A [Pdfless](https://www.pdfless.com) account
* A designed document template. Please refer to the [documentation](https://docs.pdfless.com/quickstart) to create a new template or follow the steps in next section.

### Create a document template from Pdfless dashboard
You first need to register in Pdfless. Once connected to dashboard, create a new `Document template` :
    - Entering a `title` for naming your document template
    - Selecting a `Startup template` of pre-defined document templates. You can create your document template from scratch using `Blank page` template.

From html editor, add custom variables to render dynamic your document. You can read more about this [here](https://docs.pdfless.com/expressions) 
For UI customization of your document (page format, orientation, fonts...), read the following [section](https://docs.pdfless.com/page-rules)

### Obtaining API key
From Pdfless dashboard, go to `Workspace settings` tab and you will find out API key in `Api Credentials` section.
##### NOTE
> API key is specific by workspace. If you have upgraded plan, you are able to create more than one workspace.

## Supported Operations
The connector supports the following operation:
* `Create a new PDF document`: Create a dynamic PDF document based on selected template identifier and defined payload.

### `Create a new PDF document` Connector
This connector generates a PDF document through a template and data.

|Property|Required|Description|
|---|---|---|
|`Template`|Yes|Select a document template of workspace.|
|`Data payload`|Yes|If your document contains variables, enter data in JSON format corresponding to defined variables. You can read more about this [here](https://docs.pdfless.com/expressions).|

## Known issues

- `Create a new PDF document` connector list at most 100 document templates. If you have more than 100 document templates, you can insert directly document template id into the form.

## Deployment Instructions
To deploy the Pdfless connector as a custom connector, please follow these instructions:

1. In Power Automate, search for the "Pdfless" connector.
2. Fill in the API key