## Slascone Connector
SLASCONE helps vendors license and monitor their products, with minimal engineering effort. In the cloud. This connector enables easy connectivity to third party software.

## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A Slascone subscription with admin rights

## Building the connector 
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```

To use the Connector you need to copy your API Key and IsvId in the Slascone portal.

## Supported Operations
The connector supports the following operations:
* `Create Customer`: Create a Customer in Slascone
* `Update Customer`: Update a Customer in Slascone
* `Customer is created`: This operation is triggered when a Customer is created in Slascone