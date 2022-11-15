## Jupyrest Connector
Jupyrest is a service that can turn a notebook into a REST API with predefined input and output contracts.


## Prerequisites
You will need the following to use the connector:
* A Juprest instance ([Installing the App on Customer Subscription](https://msdata.visualstudio.com/CosmosDB/_git/LivesiteNotebooks?path=/docs/marketplace.md&_a=preview&anchor=installing-the-app-on-customer-subscription))
* An Azure Active Directory Application
	* This is your Juprest client AAD app for submitting request to Juprest service, which is also provided when you create your Juprest instance following above doc.


## Getting Started 
You will need to enter your Jupyrest instance, AAD client ID and client secret to start using the connector. 
* For the Jupyrest instance, use the url after "https://" like cdbkeplerprod.azurewebsites.net
* For the AAD client ID and secret, use the AAD mentioned in the prerequisites.


## Supported Operations
The connector supports the following operations:
* `List notebooks`: Lists notebooks in the specified Jupyrest instance
* `Get notebook metadata`: Gets the notebook metadata including input parameters
* `Execute a notebook`: Invoke a notebook


