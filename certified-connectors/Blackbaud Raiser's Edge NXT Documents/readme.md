## Blackbaud Raiser's Edge NXT Documents Connector

[Raiser's Edge NXT](https://www.blackbaud.com/products/blackbaud-raisers-edge-nxt) is a comprehensive cloud-based fundraising and donor management software solution built specifically for nonprofits and the entire social good community.  

This connector is built on top of Blackbaud's [SKY API](https://developer.blackbaud.com/skyapi), and provides functionality to create document locations for use with physical attachments within the system.

## API Documentation
https://developer.blackbaud.com/skyapi

## Pre-requisites
To create a custom version of this connector, you will need to:
* [Sign in](https://signin.blackbaud.com) (or create) a Blackbaud ID.
* [Sign up](https://developer.blackbaud.com/signup) for a developer account using your Blackbaud ID.
* Request a [subscription](https://developer.blackbaud.com/subscriptions) to the SKY API to get an API key (will be used in the next steps).
* Register an [application](https://developer.blackbaud.com/apps) to get OAuth client ID/secret credentials (will be used in the next steps).

## Configure the connector properties
Using the values obtained in the previous step, update the following placeholder values in the `apiProperties.json` file:
* PLACEHOLDER_CLIENTID
* PLACEHOLDER_API_KEY

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.

Don't forget to specify the `--secret <client_secret>` value (obtained in the previous steps) when running the `paconn create` commamnd.

## Further Support
For further support, please visit our developer community at https://community.blackbaud.com/developer