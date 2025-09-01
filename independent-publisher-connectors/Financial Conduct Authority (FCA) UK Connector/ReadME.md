# FCA Connector

The Financial Conduct Authority (FCA) is a financial regulatory body in the United Kingdom. Their website allows people to search the Financial Services Register for firms and individuals and the activities firms have permissions for. The Financial Services Register is a public record of firms, individuals and other bodies that are authorised by the Prudential Regulation Authority (PRA) and/or FCA. The website can be used by anyone, looking to gather information.

# Publisher: Gulshan Khurana

## Prerequisites

You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An API key obtained [here](https://register.fca.org.uk/Developer/s/) and the email address used for registration

## Supported Operations

The connector supports the following operations:
* `Common Search`: Search the Financial Services Register for a firm or an individual or a fund (collective investment scheme) by name or unique reference number
*  `Find Individuals Details`: Get information about an Individual associated with its Individual Reference Number
*  `Find Firm Details`: Get information about a specific firm using its Firm Reference Number
*  `Find Product Details`: Get information about a specific product using its Product Reference Number
*  `Subfund Details`: Get information about subfund of a product by using its Product Reference Number
*  `Product Other Name Details`: Get information about other names of a product by using its Product Reference Number
*  `Individual Disciplinary History`: Get information about Disciplinary History on an Individual using its Individual Reference Number
*  `Firm Other Names`: Get information about the other names used by the firms using its Firm Reference Number
*  `Firm Address`: Get information about a specific firm using its Firm Reference Number
*  `Firm Individuals`: Get information about the Individuals associated with a firm using its Firm Reference Number
*  `Firm Activities and Permissions`: Get information about the activities and permissions associated with a specific firm using its Firm Reference Number
*  `Firm Requirements`: Get information about the requirements associated with a specific firm using its Firm Reference Number
*  `Firm Requirements Investment Types`: Get information about the investment types associated with a specific firm requirement using its Firm Reference Number and Requirement Reference
*  `Firm Regulators`: Get information about a regulator on a firm using its Firm Reference Number
*  `Firm Passport`: Get information about a passport details by using its Firm Reference Number
*  `Firm Passport Permission`: Get information about a passport permission by using its Firm Reference Number and Country
*  `Firm Exclusions`: Get information about an Exclusion on a firm using its Firm Reference Number
*  `Firm Disciplinary History`: Get information about a Disciplinary History on a firm using its Firm Reference Number
*  `Firm Waiver`: Get information about a Waiver on a firm using its Firm Reference Number


## Obtaining Credentials

An API key and email address is required for this connector to work. You can sign up for free with an account on FCA's website [here](https://register.fca.org.uk/Developer/s/registernewuser)

## Known issues and limitations

There are no known issues with this connector. There are functions in the API not implemented in this connector and I will continue to work on these if I can resolve the functions/syntax.

## Deployment Instructions

Upload the connector and create a connection using the API key along with the registered email address from the prerequisites.

Go to the 'Definition' section of the connector (in edit mode), click on 'X-Auth-Email' policy under 'Policies' and ensure that, in 'Operations' field in the Policy Details that emerge on the screen, all operations listed there are ticked/checked.
