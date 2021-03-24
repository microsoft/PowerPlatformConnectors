## Plumsail Documents Connector
Plumsail Documents provides a powerful and very extensive REST API.  Using this API, you can generate documents in Microsoft PowerAutomate, Azure Logic Apps or Power Apps. 

## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A valid [Plumsail Account](https://account.plumsail.com)
* The Power platform CLI tools

### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --icon [Path to icon.png]
```

## Supported Operations
Checkout full API reference on [Plumsail Documents Documentation](https://plumsail.com/docs/documents/v1.x/index.html)