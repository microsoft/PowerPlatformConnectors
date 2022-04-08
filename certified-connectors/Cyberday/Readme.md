# Cyberday
Cyberday is an information security management system, where one key part is managing the data assets of a company. Often the most important data asset are the data systems used to process data and run operations.

This connector allows you to fetch and create data systems inside your Cyberday account and integrate with
other products in your organisation.

## Publisher: Agendium Ltd

## Prerequisites
A Cyberday account is required to use this connector. Signing up can be done either at https://www.cyberday.ai/trial or directly through Microsoft Teams app store.

## Supported Actions

### List data systems
Fetch a list of all data systems and their main information from your Cyberday account.

![](https://uploads-ssl.webflow.com/5ebe2d9ae83a62f96fed82de/623aee026970a12dbed5c0cb_Screenshot%202022-03-23%20at%2011.48.45.png)

### Create a new data system
Add a new data system directly to your Cyberday account.

## Obtaining Credentials
In order to use this connector you will need your organisation's API key from your Cyberday account. This can be found in the "Settings" -> "Integration settings" section inside Cyberday.

**N.b.!** Settings are only available for highest (admin) level users in a Cyberday account.

**To fetch your API key**

1. Go to Settings
2. Expand "Integration settings"
3. Turn the "API Access" switch ON
4. Copy the key shown below 

![](https://uploads-ssl.webflow.com/5ebe2d9ae83a62f96fed82de/623aef556f0b8deaefb0c849_Screenshot%202022-03-23%20at%2011.54.57.png)

## Known issues and limitations

The connector is currently limited to the most important data assets in Cyberday, but will be expanded to more documentation according to user feedback.

## Deploying the connector
Run the following command and follow the prompts:

```paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json```

You can find more details about deploying this as custom connector in Microsoft Power Automate and Power Apps [at the following page](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli).
