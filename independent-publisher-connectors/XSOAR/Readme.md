## XSOAR Connector
XSOAR provides a security orchestration, automation, and remediation platform.  Using this integration you can submit data to XSOAR for usage in security automations.

## Publisher: Landon Chelf | FIS

## Prerequisites
You will need the following to proceed:
* A XSOAR with an integration instance available over HTTPS via [TODO HERE].
* An API key for the integration instance

## Supported Operations
The connector supports the following operation:
* `Send to XSOAR`: Send JSON data to XSOAR

## Obtaining Credentials
API keys are created by configuring an integration instance within XSOAR.

## Known issues and limitations
### Limitations
* Only supports API Key based authentication.
* Only works with integrations instances configured to be exposed over HTTPS via server rerouting using the instance.execute.external config setting in XSOAR.
### Issues
There are no known issues at this time.
