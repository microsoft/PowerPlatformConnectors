# Google BigQuery

BigQuery is a completely serverless and cost-effective enterprise data warehouse. It has built-in machine learning and BI that works across clouds, and scales with your data.

BigQuery's serverless architecture lets you use SQL queries to analyze your data. You can store and analyze your data within BigQuery or use BigQuery to assess your data where it lives.

[BigQuery - Google Cloud](https://cloud.google.com/bigquery)

## Publisher: Ashwani Kumar

## Prerequisites

Have full/owner permission to the Google BigQuery Project and Dataset to the same user account

## Supported Operations

The connector supports the following operations:

- `Get Data`: This action will return your BigQuery data in JSON format by providing 'projectId' and 'datasetId'

## Obtaining Credentials

You will need to generate an OAuth 2.0 client ID and secret for authentication. You will need to setup your OAuth Consent Screen before creating the credentials. More information about the setup is located here. [Using OAuth 2.0 to Access Google APIs](https://developers.google.com/identity/protocols/oauth2) 

## Known issues and limitations

There are no known issues and limitations.

## Deployment Instructions

Use the connector and choose OAuth 2.0 authentication as the authentication type.
