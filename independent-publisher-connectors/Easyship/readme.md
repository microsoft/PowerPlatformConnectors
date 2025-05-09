# Easyship
Easyship provides a powerful service for you to add hassle free worldwide shipping options to your website and end to end shipping functionality to your warehouse, ERP or platform.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
Use of the Easyship service requires a plan enrollment, with 50 shipments allowed per month on the $0 plan.

## Obtaining Credentials
In your account dashboard, select Connect then New Integration. You will need to register an integration to receive access token for your production and sandbox environments.

## Supported Operations
### Get rates and taxes
Retrieve a list of shipping quotes for a prospective shipment.
### List all shipments
Retrieves a list of all shipments.
### Create a shipment
Creates and retrieves the information about a shipment.
### Buy a shipment label (V1)
Buy and retrieve information about a shipping label.
### Delete a shipment (V1)
Delete and receive confirmation of a shipment deletion.
### Update a shipment
Update a shipment details.
### Get a shipment
Get the details of a shipment.
### Update warehouse state
Update a shipment's warehouse state. Only available for shipments fulfilled at one of the Easyship integrated warehouses.
### Get available pickup slots (V1)
Available pickup slots will be calculated for a specific courier, in local time and for the coming 7 days. A pickup can then be requested using the "Request pickup" action.
### Request a pickup (V1)
When a pickup is created, a pickup request will be made directly to the courier, which will return a reference number. The reference number may be used when contacting the customer service of the courier. A pickup slot must be chosen; upcoming pickup slots may be found using the "Get available pickup slots" action.
### Get checkpoints
Retrieves details of shipment checkpoints.
### Get tracking status
Retrieves the tracking status of a shipment.
### Get item categories
Retrieves a list of shipment categories and the slug to use the category.
### List all boxes
Retrieves a list of all available courier boxes.

## Known Issues and Limitations
Some of these actions are only available through the V1 version of the API. These actions will have (V1) at the end of the action name.
