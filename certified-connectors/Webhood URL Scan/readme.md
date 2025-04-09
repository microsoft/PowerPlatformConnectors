# Webhood URL Scanner connector
Webhood is a self-hosted URL scanner used by threat hunters and security analysts for analyzing phishing and malicious sites. This connector allows you to control scans using Azure Logic Apps and Power Automate.

## Publisher: Webhood

## Prerequisites
* Webhood instance

The Webhood instance can be deployed using the [Webhood URL Scanner](https://webhood.io/docs/deployment) deployment instructions. The connector is tested with the latest version of Webhood.

* API key

The API key is used to authenticate the connector to the Webhood instance. See [Obtaining Credentials](#obtaining-credentials) for instructions on how to create an API key.

* Valid https certificate for the Webhood instance if using HTTPS (required for Power Automate)

## Supported Operations

### Create a new scan
This operation creates a new scan. The scan is performed asynchronously and the results are available after the scan is completed. The scan is performed using the default configuration of the Webhood instance.

### Parameters
| Name | Type | Required | Description | Example |
| --- | --- | --- | --- | --- |
| url | string | Yes | URL to scan | https://example.com |

### Get past scans, optionally filter by status
This operation returns a list of past scans. The list can be optionally filtered by status.

### Parameters
| Name | Type | Required | Description | Example |
| --- | --- | --- | --- | --- |
| status | done\|error\|pending\|running | No | Status to filter by | done |

### Get scan by ID
This operation returns a scan by ID. The scan ID is returned by the `Create a new scan` operation.

### Parameters
| Name | Type | Required | Description | Example |
| --- | --- | --- | --- | --- |
| scanId | string | Yes | Scan ID | sryxg94j62elcpt |

### Get screenshot by scan ID
This operation returns a screenshot of the site. The scan ID is returned by the `Create a new scan` operation.

Note that the screenshot is only available if the scan is completed successfully. There is currently no way to display the image in Power Automate. Simple workaround is to use the `Get scan by ID` action to get the `url` and open it in a browser. See [Known Issues and Limitations](#known-issues-and-limitations) for more information.

### Parameters
| Name | Type | Required | Description | Example |
| --- | --- | --- | --- | --- |
| scanId | string | Yes | Scan ID | sryxg94j62elcpt |


## Obtaining Credentials
1. Login to your Webhood instance with an admin account.
2. Go to `Settings` -> `Accounts` -> `API Tokens` to create a new API key.
3. Select `Add Token` and select `scanner` as the role.
4. Copy the generated API key (`Token`) and use it as the `API Key` in the connector.

The API key will be displayed only once as it is not stored in your Webhood instance.

Note that all API keys expire after 365 days. You can create a new API key at any time. If you want to revoke an API key, you can delete it from the Webhood instance by selecting `Revoke`. We recommend you note down ID of the token so that you can identify it later.

## Known Issues and Limitations
### Displaying screenshot
The `Get screenshot by scan ID` returns a screenshot of the site. However, there is currently no way to display the image in Power Automate. Simple workaround is to use the `Get scan by ID` action to get the `url` and open it in a browser.

## Deployment Instructions
To deploy this connector, follow the instructions in [Deploying a custom connector](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli#deploying-a-custom-connector). Then run the following command to create the connector:

```
paconn create --api-prop apiProperties.json --api-def apiDefinition.swagger.json 
```