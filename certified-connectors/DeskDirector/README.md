# DeskDirector
DeskDirector automates and accelerates ticket-based workflows between Users and Techs working IT services. The DeskDirector connector is a deep and powerful integration into our rich ticketing platform. All phases of a ticket’s life cycle can be automated and enhanced for superior service experience.

## Publisher: DeskDirector Limited

## Prerequisites
You will need an active DeskDirector subscription with Power Automate connector feature, you can read our pricing [here](https://www.deskdirector.com/pricing).

## Supported Operations
View the documentation for the supported operations [here](https://help.deskdirector.com/article/zfmuvlcui1-desk-director-power-platform-connector).

## Obtaining Credentials
When setting up a new connection, you will be asked to provide an API key, the key can be generated or retrived from your admin portal, see documentation [here](https://help.deskdirector.com/article/fheiam50fg-developer-corner#api_key).

## Getting Started
View the documentation for getting started [here](https://help.deskdirector.com/article/u8uqhpn5rs-get-started-with-the-desk-director-power-automate-connector).

## Known Issues and Limitations
* The DeskDirector connector is not fully supported in Azure Logic Apps due to the lack of support for dynamic schema. 

## Deployment Instructions
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```
