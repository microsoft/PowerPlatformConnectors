
## DocuMotor Connector
DocuMotor provides powerful document generation capabilities based on input JSON data through it's API. Using this API you are able to generate:
* Word documents
* PowerPoint presentations
* Excel spreadsheets
* PDFs

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A DocuMotor tenant
* A template

## Building the connector 
Since the DocuMotor APIs requires an authentication token in the form of Basic, we first need to acquire our Unit ID, API Secret, and Template ID. For versioned templates we will also need to find the Stage ID. After acquiring this, you can create and test the DocuMotor connector.

### 1. Acquiring the Unit ID
Once logged in to your tenant in DocuMotor, navigate to the Unit and access the "Management" page from the menu, from here navigate to "Details". Your Unit ID will be listed and can be copied using the "Copy"-button.

### 2. Acquiring the API Secret
Access the "Management" page from the menu, from here navigate to "API Secrets". Either copy an existing API secret from the list, or create a new secret by entering a name and clicking the "New Secret"-button.

### 3. Acquiring the Template ID
Access the list of templates ("Templates") from the menu and navigate to the template you wish to generate from the file list. Once you have navigated to the template open the template editor by clicking on the name of the template.
In the Template editor click the "Snippets"-button in the top bar.
Select the Template ID, which is the ID inside the API url shown:  https://documotor.com/api/template/{TemplateID}/generate

### (4. Acquring the Stage ID)
This step is only needed if you are using a versioned Template.
Access the "Management" page from the menu, from here navigate to "Stages". Either copy an existing Stage ID from the list, or create a new stage by entering a name and clicking the "New Stage"-button.

### Deploying the connector
Run the following commands and follow the prompts:

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.

## Supported Operations
The connector supports the following operations:
* `Generate a document`: A document is generated based on the input data and template id.