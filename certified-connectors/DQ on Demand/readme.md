# DQ on Demand Connector

DQ on Demand is a Data Quality as a Service (DQaaS) Data Quality Improvement Platform that is designed to make the task of data management simpler.

DQ on Demand is for those who are not prepared to accept the high cost of low-quality data, are frustrated with differing levels of quality, conflicting rules and levels of and data governance.

## Publisher: DQ Global

## Pre-requisites

You will need the following to proceed:

* A Microsoft Power Automate plan with premium connector feature
* A DQ on Demand account
* A Client Id & Client Secret
* Credits for the service

## Obtaining Credentials

[Sign up](https://shop.dqglobal.com/my-account/) for a new account.
Upon signing up your trial period will automatically start, and you will be given a small sum of credits so that you can get started straight away.

Register an [application](https://shop.dqglobal.com/my-account/apps/) to get a Client Id & Client Secret.

**Note**: Make sure to set the Redirect URL to: `https://global.consent.azure-apim.net/redirect`

## API Documentation

The API Documentation can be found [here](https://dqondemand.docs.apiary.io/).

## Support

For all support requests and general queries you can contact support@dqglobal.com or visit our [support portal](https://www.dqglobal.com/support/).

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-gb/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.

Replace the `[[PLACEHOLDER_CLIENT_ID]]` value in the apiProperties file with your Client Id (obtained in the previous steps).

Don't forget to specify the `--secret <client_secret>` value with your Client Secret (obtained in the previous steps) when running the `paconn create` command.
