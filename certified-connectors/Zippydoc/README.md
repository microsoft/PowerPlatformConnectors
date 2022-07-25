# ZIPPYDOC Connector
ZIPPYDOC allows to merge, transform, filter, extract, report and analyze structured and table based data and to flexibly create data flows. Our connector helps to integrate ZIPPYDOC with third party services.

## Publisher
ZippyDoc GmbH

## Prerequisites
You will need to [Install Excel Add-In](https://appsource.microsoft.com/en-in/product/office/WA200003023).


## Supported Operations
The Connector supports the following operations:
* `Execute`: [Action] Execute all tasks according to their order inside specified flow.
* `Upload Table`: [Action] Upload table in JSON format to specified flow.
* `Get Table By ID`: [Action] Retrieve content of specified table (using table id) in JSON format.
* `Get Table By Name`: [Action] Retrieve content of specified table (using table name) in JSON format.
* `On table changed`: [Trigger] Get notification when a table content has been changed.

## Obtaining Credentials
This connector uses API key authentication. You have to login into ZIPPYDOC excel Add-In and find your API key in user settings. In case of any trouble please contact support@zippydoc.eu.

## Known Issues and Limitations
N/A

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.