# PoliteMail
Measure results with powerful email metrics, including opens, read-time and click through rates plus valuable analytics to help you gain the insights you need to improve your communications. Gain the comms analytics Viva doesn’t provide and improve your communications strategy with data. This connector provides the tools to complete everyday operations with simplicity and ease.

## Publisher: PoliteMail Software


## Prerequisites
To use this connector, you will need the following:
- A subscription with PoliteMail; you can sign up [Here](https://politemail.com/products-page/)
- A valid login, either username/password or personal access token


## Supported Operations
`Create a campaign` : This action will create a Campaign.

`Create a contact` : This action will create a contact.

`Create a list`	: This action allows you to create a list.

`Get contacts from a list` : This action will retrieve all contacts from a list.

`Add emails to a list` : This action allows you to add contacts to a PoliteMail list by email address.

`Remove emails from list` : This action allows you to remove contacts from a PoliteMail list by email address.

`Send a message` : This allows you to send a tracked message using PoliteMail.

`Create a template`	: This action allows you to create a template.

`Upload a SmartAttachment` : This action allows you to upload a file as a Smart Attachment.

`Upload an image` : This action will upload an image to the PoliteMail server.


## Obtaining Credentials
- If your PoliteMail platform is configured to use `PASSWORD` as the authentication mode, then you will enter the username for the `Username` field and your password for the `Password` field.
- If your PoliteMail platform is configured to use `AZUREAD`, `SAML` or `WINDOWNS` as the authentication mode, you will need to generate a personal access token to create the connection. 
  - This can be completed under PoliteMail --> Account --> Personal Access Tokens --> New
  - Then on creation of the connection for the connector, place the access token created above in both the `Username` and `Password` fields.

## Deployment Instructions
To deploy this connector as a custom connector you need to go into the `Custom Connector` page of Power automate, and follow these instructions: 
- Create a new custom connector by importing an OpenAPI file. Use the "apiDefinition.swagger.json" from this project.
- On the `General` tab, update the section labeled `Host` with the full URL of your PoliteMail platform. (i.e., https://your-name.pmail2.com)
- The rest of the configuration is optional, and you can now select 'Create Connector' and begin using your custom PoliteMail Connector.

## Known Issues and Limitations
There are no knowing issues so far.