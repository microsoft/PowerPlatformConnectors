
### NOTE
> This is a Secplug scan connector. The connector is provided here with the intent to illustrate certain features and functionality around connectors.

## Secplug Scan Connector
Secplug provides a powerful platform for pluggable security using secplug plugins.



## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An account in secplugs
* The Power platform CLI tools

## Building the connector 
You would need to ensure that you have an account in secplug and using https://secplugs.com/index.php/providers an power automate profile is created.
After that is completed, you can create and test the secplug connector.

### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```

## Supported Operations
The connector supports the following operations:
* `Scan a file`: scans a file, using the configured security provider in secplug profile.
* `Scan an email`: scans a file, using the configured security provider in secplug profile.
