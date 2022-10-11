## Lasso X Connector

Enables customers of Lasso X to utilize public data regarding companies, people, and production units from the Danish Business Authority, CVR, directly in Power Automate.

## Pre-requisites

- You will need to sign up for an account with [Lasso X](https://www.lassox.com).
- You will need to obtain your [API Key](https://docs.lassox.com/gettingstarted/#authentication).

## Documentation

For general information on the API offerings of Lasso X, visit our [documentation](https://docs.lassox.com).

## Supported Operations

The connector supports the following operations:

`CompanyInformation`: Retrieves information regarding a company. Takes a CVR number as input.
`RelatedProductionUnits`: Retrieves information regarding Production units related to a company. Takes CVR Number as input, and optionally whether to only include companies at same address or not at the same address as the company.
`CompanyLatestReport`: Retrieves the company's latest financial report. Takes a CVR number as input.
`PersonInformation`: Retrieves information regarding a person. Takes a unit number as input.
`ProductionUnitInformation`: Retrieves information regarding a production unit. Takes a Production Unit Number (P number) as input.

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.

## Further Support

For further support, you can contact us at support@lassox.com or at our [webpage](https://lassox.com/support/tech-support), and we'll be happy to help.
