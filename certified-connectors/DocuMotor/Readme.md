# DocuMotor Connector
DocuMotor provides powerful document generation capabilities based on input JSON data. Using this connector you are able to generate:
* Word documents
* PowerPoint presentations
* Excel spreadsheets
* PDFs

## Publisher: Omnidocs

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A DocuMotor tenant
* A template

## Supported Operations
The connector supports the following operations:

### Generate a document 
A document is generated based on the input data, Template ID, accept type (native or PDF), and optionally Stage ID for versioned templates. The output file type depends on which type of template you are using.

## Obtaining Credentials
The service uses Basic authentication in the form of the Unit ID, API Secret.

### Acquiring the Unit ID
Once logged in to your tenant in DocuMotor, navigate to the Unit and access the "Management" page from the menu, from here navigate to "Details". Your Unit ID will be listed and can be copied using the "Copy"-button.

### Acquiring the API Secret
Access the "Management" page from the menu, from here navigate to "API Secrets". Either copy an existing API secret from the list, or create a new secret by entering a name and clicking the "New Secret"-button.

## Getting Started

### Acquiring the Template ID
Access the list of templates ("Templates") from the menu and navigate to the template you wish to generate from the file list. Once you have navigated to the template open the template editor by clicking on the name of the template.
In the Template editor click the "Snippets"-button in the top bar.
Select the Template ID, which is the ID inside the API url shown:  https://documotor.com/api/template/{TemplateID}/generate.

### (Acquiring the Stage ID)
This step is only needed if you are using a versioned Template.
Access the "Management" page from the menu, from here navigate to "Stages". Either copy an existing Stage ID from the list, or create a new stage by entering a name and clicking the "New Stage"-button.

## Known Issues and Limitations
Currently not supporting self-hosted DocuMotor instances.

## Frequently Asked Questions

### How do I sign up for a test tenant?
Reach out to Omnidocs at info@omnidocs.com.

## Deployment Instructions
Run the following commands and follow the prompts:

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.