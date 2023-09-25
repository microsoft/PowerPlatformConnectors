# Lex Power Sign connector

Lex Persona is a European qualified trust services provider offering a range of electronic signature solutions. With Lex Enterprise, you have access to all the eIDAS signature levels, along with a large number of signer authentication methods. Your signature books are embedded in digital workflows. Automate connections between Lex Enterprise and all your tools to eliminate repetitive tasks, gain efficiency, and accelerate your business.

# Publisher

Lex Persona (www.lex-persona.com)

# Prerequisites

You will need to have the following in order to proceed:

- Lex Enterprise subscription. Use the trial plan to test

The environment requirements are:

- A Microsoft Power Apps or Power Automate plan with custom connector feature
- The Power platform CLI tools

# Supported operations

## Retrieve consent page
Retrieve a given consent page information

## Search consent pages
Search consent pages, by name for example

## Create contact
Create a contact

## Retrieve contact
Retrieve a given contact information

## Delete contact</mark> : Delete a contact

## Update contact</mark>
Update a contact information

## Search contacts
Search contacts, by last name for example

## Retrieve the metadata mapping
Retrieve the metadata mapping of your tenant

## Create signature profile
 Create a signature profile

## Retrieve signature profile
Retrieve a given signature profile information

## Update signature profile
Update a signature profile information

## Search signature profiles
 Search signature profiles, by name for example

## Retrieve me
Retrieve the connected user information (the user id is needed to create a workflow)

## Create workflow 
Create a workflow

## Retrieve workflow
Retrieve a given workflow information

## Delete workflow
Delete a workflow

## Update workflow
Update a workflow information

## Search workflows
Search workflows, by name for example

## Retrieve consent page url
Retrieve the url of the consent page for a given signer

## Download evidences file
Download the evidences file for a workflow

## Create part
Create a document part

## Create document
 Create a document from parts

## Retrieve document
Retrieve a document information
## Delete document
Delete a document

## Update document
Update a document information

## Search documents
Search documents, by file name for example

## Download a workflow documents
Download the documents of a given workflow

## Create metadata layout
Create a metadata layout

## Retrieve metadata layout
Rertieve a given metadata layout information

## Update metadata layout
Update a given metadata layout information

## Search metadata layouts
Search metadata layouts, by name for example

# Obtaining credentials

Please contact our sales representatives at commercial@lex-persona.com to get a Lex Enteprise subscription or an account on our trial systems.

Connect to the Lex Enterprise portal with your account, and generate an API Key.

# Known issues and limitation

N/A

# Deployment instructions

By default, the connector is configured to use our trial backend (sgs-wm-test01.sunnystamp.com). If you want to pair the connector with your own Lex Enterprise tenant, edit the apiDefinition.swagger.json file and replace the test system url by your tenant url:

    -   "host": "yourtenant.lexpersona.com",

Then login to your PowerPlatform:

    paconn login

And create the new custom connector:

    paconn create --api-prop apiProperties.json --api-def apiDefinition.swagger.json











