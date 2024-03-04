# Korto
Use this connector to communicate with your instance of the Korto Record Management Systems API v2.0.

## Publisher: Korto OÜ

## Pre-requisites
[Contact us](https://korto.io/contact-us/) to get your instance of the Korto application environment.
After setting up your Korto environment, you'll be able to get the necessary Korto API URL and API Key to create the connector connection.

## Supported Operations

### GetTag
Get a Tag definition from Korto.
You can choose to identify the Tag by it's internal Korto ID or by name.
### CreateTag
Create a Tag definition in Korto.
You'll need to specify the Tag name, type (system tag, label tag, action tag, ...) and value type (string, decimal, date, ...).
### DeleteTag
Delete a Tag definition from Korto.
This will remove the Tag definition and all it's references on all the Records.
You can choose to identify the Tag by it's internal Korto ID or by name.

### GetRecord
Get a Record definition, with all the included Tags.
You can choose to identify the Record by it's internal Korto ID or by an external ID.
### DownloadRecord
Download the content of the file asociated with a Korto Record.
You can choose to identify the Record by it's internal Korto ID or by an external ID.
### CreateRecord
Create a new Record in Korto.
This operation needs an input, from an output of another connector that can read a file content.
### DeleteRecord
Delete a record from Korto, with all the included Tags.
Tag definitions will remain in the system, only the values associated with the Record will be deleted.
You can choose to identify the Record by it's internal Korto ID or by an external ID.

### AddTagToRecord
Add a new Tag to an existing Record.
The Record and Tag definitions need to exist in Korto.
You can choose to identify the Record by it's internal Korto ID or by an external ID.
For the Tag, you need to specify the Tags name and value.
### UpdateTagOnRecord
Update an existing Tag on a existing Record.
The Record and Tag definitions need to exist in Korto.
You can choose to identify the Record by it's internal Korto ID or by an external ID.
For the Tag, you need to specify the Tags name and it's new value.
### DeleteTagFromRecord
Delete an existing Tag from an existing Record.
This operation will not delete the definition of the Tag from the system, but will delete the Tag value associated with the Record.

## Obtaining Credentials
For authentication, you need to use an API Key. You can create keys in the Admin section, within your Korto application instance.

## Known Issues and Limitations
Currently, there are no reported issues and limitations.

## Deployment Instructions
Please use [these Microsoft instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.