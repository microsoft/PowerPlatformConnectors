
### NOTE
> This is a Scancloud APIs connector. The connector is provided here with the intent to illustrate certain features and functionality around connectors.

## Scancloud APIs Connector
Scancloud APIs provides a powerful platform for pluggable security using Scancloud APIs plugins.



## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An account in Scancloud
* The Power platform CLI tools

## Building the connector 
You would need to ensure that you have an account in secplugs and using https://scancloud.io/index.php/providers an power automate profile is created.
After that is completed, you can create and test the secplugs connector.

### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```

## Supported Operations
The connector supports the following operations:
* `Scan a file`: scans a file, using the configured security provider in scancloud profile.
* `Scan an email`: scans a file, using the configured security provider in scancloud profile.
