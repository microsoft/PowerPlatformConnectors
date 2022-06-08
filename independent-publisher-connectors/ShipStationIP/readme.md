# ShipStation IP
The API is a great way to get data directly to and from ShipStation, like creating orders, updating products, and querying order, shipment and customer data.

The API is available for any plan and allows you to interface with the ShipStation platform. The API can be used to automate many tasks including:
* Managing Orders
* Managing Shipments
* Creating Shipping Labels
* Retrieving Shipping Rates

## Publisher: Kristian Matthews

## Prerequisites
ShipStation uses the ISO 8601 combined format for DateTime stamps being submitted to and returned from the API. Please be sure to submit all DateTime values as follows:

yyyy-mm-dd hh:mm:ss (24 hour notation). Example: 2016-11-29 23:59:59

The time zone represented in all API responses is PST/PDT. Similarly, ShipStation asks that you make all time zone convertions and submit any DateTime requests in PST/PDT.

## Supported Operations
Required. Describe actions, triggers, and other endpoints.​

### Stores

#### List Marketplaces
Lists the marketplaces that can be integrated with ShipStation.

#### List Stores
Retrieve the list of installed stores on the account.

#### Refresh Stores
Initiates a store refresh.

## Obtaining Credentials
Use your ShipStation API Key as the username and API Secret as the password. You can find your API Key as the username and API Secret under Settings.

## Known Issues and Limitations
