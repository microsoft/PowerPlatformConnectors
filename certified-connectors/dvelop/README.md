## d.velop Connector
d.velop is a provider of ECM solutions (Enterprise Content Management). Use this connector to connect your flow with services from the d.velop platform or your d.velop system.


## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A d.velop platform subscription with booked app "d.velop connect for integration platforms"
* The Power platform CLI tools

## Building the connector 
To proceed we need a d.velop platform instance, an API-Key for the authentication and the custom connector.

### Set up d.velop cloud platform instance
To use the custom connector we need to create a d.velop cloud platform instance and create an API-Key for the authentication.

**Booking d.velop cloud platform instance**
Visit https://store.d-velop.de and book the service d.velop documents. It is free for 30 days.
During the booking process you have to define your custom d.velop domain URl.

**Create API-Key for authentication**
Visit https://<d.velop cloud domain>.d-velop.cloud/identityprovider/config/apikey and create an API-Key for a user with whose rights the actions and triggers should be executed.  

#### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

## Supported Triggers
As part of this sample following triggers are supported:
* `d.velop event (Webhook)`: Triggers when the defined event occurs in the d.velop platform

## Supported Actions
As part of this sample following actions are supported:
* `Store document`: Stores a document in d.velop documents
* `Execute an action`: Executes the selected action in the selected d.velop environment
