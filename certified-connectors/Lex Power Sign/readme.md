# Lex Power Sign connector

Lex Persona is a European qualified trust services provider offering a range of electronic signature solutions. With Lex Enterprise, you have access to all the eIDAS signature levels, along with a large number of signer authentication methods. Your signature books are embedded in digital workflows. Automate connections between Lex Enterprise and all your tools to eliminate repetitive tasks, gain efficiency, and accelerate your business.

## Prerequisites

You will need to have the following in order to proceed:

- Lex Enterprise subscription. Use the trial plan to test

The environment requirements are:

- A Microsoft Power Apps or Power Automate plan with custom connector feature
- The Power platform CLI tools

## Get a Lex Enterprise subscription

Please contact our sales representatives at commercial@lex-persona.com

# Setting up the Lex Enterprise backend url

By default, the connector is configured to use our test backend (sgs-wm-test01.sunnystamp.com). If you want to pair the connector with your own Lex Enterprise tenant, edit the apiDefinition.swagger.json file and replace the test system url by your tenant url:

    -   "host": "yourtenant.lexpersona.com",

## Deploying the sample

First login to your PowerPlatform:

    paconn login

Then create the new custom connector:

    paconn create --api-prop apiProperties.json --api-def apiDefinition.swagger.json

## Supported operations

The connector supports the following operations:

- <mark style="background-color: dimgray">Retrieve consent page</mark> : Retrieve a given consent page information
- <mark style="background-color: dimgray">Search consent pages</mark> : Search consent pages, by name for example
- <mark style="background-color: dimgray">Create contact</mark> : Create a contact
- <mark style="background-color: dimgray">Retrieve contact</mark> : Retrieve a given contact information
- <mark style="background-color: dimgray">Delete contact</mark> : Delete a contact
- <mark style="background-color: dimgray">Update contact</mark> : Update a contact information
- <mark style="background-color: dimgray">Search contacts</mark> : Search contacts, by last name for example
- <mark style="background-color: dimgray">Retrieve the metadata mapping</mark> : Retrieve the metadata mapping of your tenant
- <mark style="background-color: dimgray">Create signature profile</mark> : Create a signature profile
- <mark style="background-color: dimgray">Retrieve signature profile</mark> : Retrieve a given signature profile information
- <mark style="background-color: dimgray">Update signature profile</mark> : Update a signature profile information
- <mark style="background-color: dimgray">Search signature profiles</mark> : Search signature profiles, by name for example
- <mark style="background-color: dimgray">Retrieve me</mark> : Retrieve the connected user information (the user id is needed to create a workflow)
- <mark style="background-color: dimgray">Create workflow</mark> : Create a workflow
- <mark style="background-color: dimgray">Retrieve workflow</mark> : Retrieve a given workflow information
- <mark style="background-color: dimgray">Delete workflow</mark> : Delete a workflow
- <mark style="background-color: dimgray">Update workflow</mark> : Update a workflow information
- <mark style="background-color: dimgray">Search workflows</mark> : Search workflows, by name for example
- <mark style="background-color: dimgray">Retrieve consent page url</mark> : Retrieve the url of the consent page for a given signer
- <mark style="background-color: dimgray">Download evidences file</mark> : Download the evidences file for a workflow
- <mark style="background-color: dimgray">Create part</mark> : Create a document part
- <mark style="background-color: dimgray">Create document</mark> : Create a document from parts
- <mark style="background-color: dimgray">Retrieve document</mark> : Retrieve a document information
- <mark style="background-color: dimgray">Delete document</mark> : Delete a document
- <mark style="background-color: dimgray">Update document</mark> : Update a document information
- <mark style="background-color: dimgray">Search documents</mark> : Search documents, by file name for example
- <mark style="background-color: dimgray">Download a workflow documents</mark> : Download the documents of a given workflow
- <mark style="background-color: dimgray">Create metadata layout</mark> : Create a metadata layout
- <mark style="background-color: dimgray">Retrieve metadata layout</mark> : Rertieve a given metadata layout information
- <mark style="background-color: dimgray">Update metadata layout</mark> : Update a given metadata layout information
- <mark style="background-color: dimgray">Search metadata layouts</mark> : Search metadata layouts, by name for example









