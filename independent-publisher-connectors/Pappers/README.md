# Pappers
Pappers is an API that allows you to collect all legal and financial information on the companies of your choice (statutes, corporate accounts, trademarks, managers, shareholders).
Refine your analysis with our advanced search engine.

## Publisher: Allan De Castro

## Prerequisites

You need to have a Pappers API Key. You can [register](https://www.pappers.fr/inscription) to get your key (a free developer plan is available).

## Supported Operations

The connector supports the following operations:

- `Retrieves all the information available on a company.`: This action allows you to search for all the information available for a company based on the SIRET or SIREN.
- `Search for companies matching criteria.`: This action allows you to perform a multi-criteria search to display a list of matching companies.


## Obtaining Credentials

### Pappers side
Through the [API Key Management page](https://www.pappers.fr/mon-compte/api), you can grab the API Key.

### Custom connector side
Once you are configuring the connector on the Power Platform.
You will have to use the **API Key** in order to validate the security stage in the process.

## Known issues and limitations

There are no known issues and limitations (except for those related to your subscribed plan).

## Deployment Instructions

Upload the connector and choose no authentication as the authentication type.