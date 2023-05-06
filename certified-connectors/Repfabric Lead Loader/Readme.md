## Repfabric Lead Loader
This connector is used for sending data to Repfabric's backend system, automatically creating new tables and objects related to customers, companies, and "leads". The primary purpose of this connector is to send the data from a .csv file, though this API can be used for other Microsoft services in the future.

## Publisher

Repfabric

## Prerequisites
You will only need access to a sheet template that will provide the correct data for the API to process, and a Power Automate template that uses the connector. [Here](https://repfabric-my.sharepoint.com/:x:/p/jeremy_ramos/EfdJI0HWs3lMi9T7rvNrGVsB83ZmKSciVHeOebrYulcTDw?e=1yafTd) is a Google Sheet example. Until we can publish the template with the certified connector, contact andre@phiquest.io for a Power Automate template.

You will not need credentials for testing this operation as we cannot give credentials for this API to those outside of the organization at the moment. For testing purposes, this API will use it own default credentials that are stored as Basic Auth, to avoid leaking client information.

## Supported Operations

### Contact
* Main Endpoint used for creating Customers and Principals in the Repfabric Database

## Obtaining Credentials
Credentials are managed by the Repfabric web application. Please contact andre@phiquest.io for access.

## Getting Started
There are many columns of data that can be sent, but only a few are necessary. This is the required data that the API needs to take:


    "company" : "Company Inc.",
    "type": "Customer",
    
    "first-name": "John",
    "last-name" : "Doe",

    "principal-name": "Principal Inc.",
    "program": "This is a Readme example"

From here, please provide the data from a csv file and link them to the appropriate key. **The only exceptions are type and program. Type should be defaulted to Customer, and Program can be any test string, but it cannot be empty.**

The connector will return the **status** of the API call, the **data** that displays a message of what the API created, or why it failed, and the **probability** that the principal that was submitted matches the principal in the database, based on string similarity. (If the Principal does not seem to exist in the database, it will state that it has created a new Principal)

You can use the returned information to let the user know of the status of the call, via email and/or updating the csv sheet. This API should only run for about 3 - 10 seconds.

## Known Issues and Limitations
When running the Power Automate template, it may run indefinitely if there's an issue with the configuration, or if the API keeps crashing. We are looking to fix this in a future update.

## Deployment Instructions
N/A