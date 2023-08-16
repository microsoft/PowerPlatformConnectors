# FCA-Connector

The Financial Conduct Authority (FCA) is a financial regulatory body in the United Kingdom. Their website allows people to search the Financial Services Register for firms and individuals and the activities firms have permissions for. The Financial Services Register is a public record of firms, individuals and other bodies that are authorised by the Prudential Regulation Authority (PRA) and/or FCA. The website can be used by anyone, looking to gather information.

# Publisher: Gulshan Khurana

## Prerequisites

You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An API key obtained [here](https://register.fca.org.uk/Developer/s/) and the email address used for registration

## Supported Operations

The connector supports the following operations:
* `Common Search`: Search the Financial Services Register for a firm or an individual by name or unique reference number


## Obtaining Credentials

An API key and email address is required for this connector to work. You can sign up for free with an account on FCA's website [here](https://register.fca.org.uk/Developer/s/registernewuser)

## Known issues and limitations

There are no known issues with this connector. There are functions in the API not implemented in this connector and I will continue to work on these if I can resolve the functions/syntax.

## Deployment Instructions

Upload the connector and create a connection using the API key along with the registered email address from the prerequisites.
