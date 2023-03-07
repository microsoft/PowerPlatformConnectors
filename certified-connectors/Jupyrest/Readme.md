Jupyrest is a service that can run notebooks with predefined input and output contracts.

## Prerequisites
You will need the following to use the connector:
* A Juprest instance ([Installing the App on Customer Subscription](https://msdata.visualstudio.com/CosmosDB/_git/LivesiteNotebooks?path=/docs/marketplace.md&_a=preview&anchor=installing-the-app-on-customer-subscription))
* An Azure Active Directory Application
	* This is your Juprest client AAD app for submitting request to Juprest service, which is also provided when you create your Juprest instance following above doc.

## How to get credentials
Your AAD client app created in the prerequisites has the access to your Jupyrest instance and will be used to create a connection of Jupyrest connector.

## Getting Started with your connector
You will need to enter your Jupyrest instance, AAD client ID and client secret to start using the connector. 
* For the Jupyrest instance, use the url of your Function App like https://cdbkeplerprod.azurewebsites.net
* For the AAD client ID and secret, use the AAD mentioned in the prerequisites.
* For the ResourceUrl, use the url of your Function App. e.g. https://cdbkeplerprod.azurewebsites.net

## Known issues and limitations
"Upload a notebook to Synapse" does not support uploading a existing notebook execution given an excutionId.

## Common errors and remedies
Contact us via [kepleruser@service.microsoft.com](mailto:kepleruser@service.microsoft.com) in case of errors and questions.

## FAQ
Contact us via [kepleruser@service.microsoft.com](mailto:kepleruser@service.microsoft.com) in case of errors and questions.