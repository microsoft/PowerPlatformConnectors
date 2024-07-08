## Companies House Connector

Companies House is a UK govnerment Organisation and website that allows people to look up companies, people at companies, review information, file informations about those companies and more. The website can be used by anyone, looking to gather information.

## Publisher: Matt Collins-Jones

## Prerequisites

You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A Companies House API key obtained [here](https://developer.company-information.service.gov.uk/get-started)

## Supported Operations

The connector supports the following operations:
* `Find Company By Number`: Find a Company By their Company Number
* `List Person of Significant Control`: List Persons of Significant Control by Company Number
* `List Statements for Person of Significant Control`: List Statements for Person of Significant Control
* `Individual PSC`: Find information about a Person of Significan Control by Company Number and PCS ID
* `UK Establishments`: Lists UK Establishment Companies
* `Officer Appointment by Officer Id`: Officer Appointment by Officer Id
* `Filing History By Number and Id`: Filing History By Number and Id
* `Charges By Company Number`: Company Charges By Company Number
* `Charges By Number and Charge Id`: Company Charges By Number and Charge Id
* `Address By Number`: Comapny Address by Company Number
* `Company Officers By Company Number`: Company Officers By Company Number
* `Company Officers By Number and Appointment Id`: Company Officers By Number and Appointment Id
* `Filing History By Company Number`: Filing History By Company Number

## Obtaining Credentials

An API key is required for this connector to work. You can sign up for free with an account on Companies Houses' website [here](https://developer.company-information.service.gov.uk/get-started)

## Known issues and limitations

There are no known issues with this connector. There are functions in the API not implemented in this connector and I will continue to work on these if I can resolve the functions/syntax.

## Deployment Instructions

Upload the connector and create a connection using the API key from the prerequisites.