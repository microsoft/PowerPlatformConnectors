## Plumsail Forms Connector
Plumsail Forms allows you to design rich fully responsive web forms and publish them to any websites. This connector provides a trigger for subscribing to form submissions.

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
* `Form is submitted`: a trigger that fires when the specified form is submitted
* `Delete submission`: an action that deletes the specified submission
* `Download attachment`: an action that downloads the specified attachment
* `Delete attachment`: an action that deletes the specified attachment
