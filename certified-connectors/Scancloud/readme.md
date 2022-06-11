
## Scancloud 
ScanCloud is a unique security platform that allows you to customise and combine hosted security scanners and use them to protect your assets.We provide ready-made, open source apps & plugins, Kits and an API to use the security scanners you configure in our cloud.You can then use the ScanCloud portal monitor activity, review performance and provide centralised management. Using this connector customers can directly get files and emails scanned by selected scanner from Scancloud.

## Publisher: Scancloud
scancloud

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

## Deployment Instructions
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```