# Annature

Annature is an Australia's leading digital signature provider, purpose built for all industries. It's an ISO 27001 certified technology company that builds digital signature, identity verification, and payment collection software.

Consumers from small businesses to enterprises, use our software to manage their digital transactions online. This connectors supports only core operations like Getting Documents, Creating Envelope, Sending to recipients. 

 
## Publisher: Strategik

## Prerequisites

To use this connector, you need the following

- A Microsoft Power Apps or Power Automate plan with custom connector feature
- You need to have an Annature account configured with APIKeys.


## Obtaining Credentials

- To get started you will need to create a free account in order to obtain a pair of API keys. [sign up](https://dashboard.annature.com.au/sign-up) 
- Deploy the Connector and Configure the Data Connections using the APIKeys
- For more details checkout this [article](https://www.strategik.com.au/post/using-annature-with-the-power-platform)


### Supported Operations 
Annature API has more than 40 operations. Currently this connector supports the most important operations related to Documents and Envelopes.

## Actions

#### 1. Documents
- Get Documents : Get documents created for an envelope.
- Get Document : Get a single document created for an envelope 

####  2. Envelopes
- Create Envelope : Creates a New Envelope
- Get Envelopes : Get envelopes. 
- Get Envelope : Get the details of an existing envelope
- Send Envelope : Sends an existing draft envelope to all eligible recipients.
- Void Envelope : Voids an envelope cancelling all outstanding signature requests and preventing the envelope from being completed

####  3. Recipients
- Get Recipient : Get details of a Recipient based on Id

####  4. Fields
- Get Fields : Get fields from an envelope
- Get Field : Get a single field created for an envelope

#### 5. Templates
- Get Templates : Get templates defined in your annature account
- Get Template : Get the template based on template Id.
- Use a Template : Create a Envelope based on a specific template Id.

####  6. Others
- Get Accounts : Get accounts from your annature account
- Get Organization : Get the details of your organisation.

## Triggers 

### Endpoints : OnDocumentEvent

Annature uses webhooks to notify your application via HTTP POST when an event happens on envelopes sent from your organisation.

Please read about webhook responses here https://documentation.annature.com.au/#/webhooks/responding


## Known Issues and Limitations
There are no known issues at time of publishing.


## Deployment Instructions
Follow the instructions provided on the [Power Automate blog](https://flow.microsoft.com/en-us/blog/import-a-connector-from-github-as-a-custom-connector/).
