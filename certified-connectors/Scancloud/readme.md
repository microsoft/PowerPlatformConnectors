
## Scancloud Connector
Scancloud provides a powerful platform for pluggable security using Scancloud plugins.

## Publisher: Scancloud


## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An account in Scancloud
* The Power platform CLI tools

## Supported Operations
The connector supports the following operations:
* `Scan a file`: scans a file, using the configured security provider in scancloud profile.
* `Scan an email`: scans a file, using the configured security provider in scancloud profile.

## Obtaining Credentials
You can obtain key using a valid account from ScanCloud Profile.

## Known Issues and Limitations
N/A

### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```