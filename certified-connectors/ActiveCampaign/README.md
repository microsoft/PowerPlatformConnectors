
# ActiveCampaign

This connector allows you to integrate specific workflows with ActiveCampaign. You can use this connector to extract data from a data source and upload specific information to your ActiveCampaign account.

## Publisher: Microsoft

## Prerequisites

* An ActiveCampaign [account](https://www.activecampaign.com/) and corresponding administrator credentials.
* An ActiveCampaign [API Key](https://help.activecampaign.com/hc/articles/207317590-Getting-started-with-the-API#how-to-obtain-your-activecampaign-api-url-and-key) and REST Endpoint hostname.

## Obtaining Credentials

Please follow [the instructions](https://help.activecampaign.com/hc/articles/207317590-Getting-started-with-the-API#how-to-obtain-your-activecampaign-api-url-and-key) from ActiveCampaign's documentation on how to find your Api Key and REST Endpoint hostname.

## Supported Operations

### Bulk import contacts

You can bulk import contacts from a source into your ActiveAccount list. The action takes a list of contacts and performs a bulk upload into the specified list. You will need an [ActiveCampaign List ID](https://help.activecampaign.com/hc/en-us/articles/360000030559-How-to-create-a-list-in-ActiveCampaign) to use this operation.

## Known issues and limitations

The number of contacts that you can export to ActiveCampaign depends on your contract with ActiveCampaign.
