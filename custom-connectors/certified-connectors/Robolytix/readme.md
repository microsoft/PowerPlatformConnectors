
## Robolytix connector
Robolytix is the key online analytic and monitoring tool for Robotic Process Automation using Sonar technology to evaluate, audit, monitor and improve performance of robots operating in any application, RPA platform or custom solution.

This connector provides methods for validating API credentials, sending Sonar data to Robolytix and accessing related enumerations.

## Pre-requisites
It is suggested to register a new free account at [Robolytix.com](https://robolytix.com).

You will also need the following to proceed:

* A Microsoft Power Apps or Microsoft Power Automate plan with custom connector feature
* An Azure subscription
* The Power platform CLI tools


## API documentation
Detailed documentation is located in [Robolytix SDK](https://github.com/robolytix/robolytix-sdk) repository.


## Supported Operations
Connector from external platform usually implements only one action called Sonar. This action calls **/messages** endpoint. There are two endpoints **/listprocesses** and **/listtypes** that serve enumerations for main call. The rest of the endpoints are just for validation connection and authorization.


## How to get credentials
1. Go to webpages [Robolytix.com](https://robolytix.com)
2. Sign-up for a new account
3. Generate API Key in **Settings -> API Keys** menu
4. Use API Key in related connection


## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.

