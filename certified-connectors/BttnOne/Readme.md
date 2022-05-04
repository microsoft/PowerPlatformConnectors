## Bttn ONE Connector
The magical bttn that automates your business, improves your customer experience, and so much more…   
This connector is for the bttn ONE. The next generation of Internet of Things buttons.

## How to get a Bttn
Visit bt.tn for more information and how to order one of our unique IoT buttons!

## Prerequisites
You will need the following to proceed:
* A bttn ONE device.
* A Bttn account.
* A Power Automate action created on the customer portal.

## Authentication 
Connect this connector to your Bttn account using Oauth. You are automatically forwarded to the Bttn login page.

## Supported Operations
The connector supports the following operations:
* `getActionConfigurations`: Lists all your Power Automate actions configured on the Bttn portal.
* `bttnWebhook`: Sets a webhook for your Power Automate flow on a configured Power Automate Action. This webhook wil be executed when the bttn ONE is pressed.

# Bttn ONE Connector
The magical bttn that automates your business, improves your customer experience, and so much more…   
This connector is for the bttn ONE. The next generation of Internet of Things buttons.

## Publisher: Bttn

## Prerequisites
You will need the following to proceed:
* A bttn ONE device.
* A Bttn account.
* A Power Automate action created on the customer portal.
 
## Supported Operations
* Create webhook

###  Create Webhook
This action creates a webhook to your (pre-configured) bttn action which allows you to trigger your Power Automate Flow.

## Obtaining Credentials
Credentials can be obtained when you register to the Bttn platform and get your hands on our Bttn ONE device.

## Known Issues and Limitations
You are only able to configure one Action to one webhook. If you require multiple triggers you will need to create multiple actions on our portal.
Please name them correctly so you can identify your actions

## Deployment Instructions
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --icon icon.png
(https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli)
