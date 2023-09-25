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

|Operation             |Description |
|----------------------|------------------------------------------------------------------------------------------------------------------------------------------|
|Retrieve consent page |[Retrieve a given consent page information](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#retrieve-consent-page) |
|Search consent pages |[Search consent pages, by name for example](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#search-consent-pages) |
|Create contact |[Create a contact](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#create-contact) |
|Retrieve contact | [Retrieve a given contact information](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#retrieve-contact) |
|Delete contact |[Delete a contact](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#delete-contact) |
|Update contact |[Update a contact information](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#update-contact) |
|Search contacts |[Search contacts, by last name for example](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#search-contacts) |
|Retrieve the metadata mapping |[Retrieve the metadata mapping of your tenant](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#retrieve-metadata-mapping) |
|Create signature profile |[Create a signature profile](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#create-signature-profile) |
|Retrieve signature profile |[Retrieve a given signature profile information](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#retrieve-signature-profile) |
|Update signature profile |[Update a signature profile information](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#update-signature-profile) |
|Search signature profiles |[Search signature profiles, by name for example](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#search-signature-profiles) |
|Retrieve me |[Retrieve the connected user information (the user id is needed to create a workflow)](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#retrieve-me) |
|Create workflow  |[Create a workflow](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#create-workflow) |
|Retrieve workflow |[Retrieve a given workflow information](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#retrieve-workflow) |
|Delete workflow |[Delete a workflow](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#delete-workflow) |
|Update workflow|[Update a workflow information](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#update-workflow) |
|Search workflows |[Search workflows, by name for example](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#search-workflows) |
|Retrieve consent page url |[Retrieve the url of the consent page for a given signer](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#create-workflow-invite) |
|Download evidence files |[Download the evidence files for a workflow](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#download-evidence-files) |
|Create part |[Create a document part](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#create-parts) |
|Create document | [Create a document from parts](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#create-document) |
|Retrieve document |[Retrieve a document information](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#retrieve-document) |
|Delete document |[Delete a document](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#delete-document) |
|Update document |[Update a document information](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#update-document) |
|Search documents |[Search documents, by file name for example](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#search-documents) |
|Download a workflow documents |[Download the documents of a given workflow](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#download-workflow-documents)
|Create metadata layout |[Create a metadata layout](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#create-layout) |
|Retrieve metadata layout |[Rertieve a given metadata layout information](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#retrieve-layout) |
|Update metadata layout |[Update a given metadata layout information](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#update-layout) |
|Search metadata layouts |[Search metadata layouts, by name for example](https://sgs-demo-test01.sunnystamp.com/wm-docs/api.html#search-layouts) |

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











