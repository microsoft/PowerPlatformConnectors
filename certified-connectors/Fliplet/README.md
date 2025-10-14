
# Fliplet

This custom connector will allow power automate and logic apps user to connect to the Fliplet platform and do a variety of operations on their data sources.

## Publisher: Fliplet

Fliplet is a no code app platform where users can build web apps and native apps.

## Prerequisites

The main pre-requisite is being able to generate an API token in Fliplet studio. To do this, your licence level should be:

- Your app should be on the private+ licence OR
- Your org is an entreprise client

Please check with our customer success team if you have any questions.

## Supported Operations

### `GetAllApps`

Lists all the apps linked to this API token

### `GetAllDataSources`

Lists all the data sources linked to this API token

### `GetDataSourceById`

Displays information about a specific data source targeted by its Id

### `GetDataSourceEntry`

Displays information about a specific data source entry targeted by its Id

### `UpdateDataSourceEntry`

Updates a specific data source entry targeted by its Id using a JSON object

### `DeleteDataSourceEntry`

Deletes a specific data source entry targeted by its Id

### `GetDataSourceEntries`

Displays data source data as a JSON object

### `CreateDataSourceRows`

Create a new data source entry using a JSON object

## Obtaining Credentials

In order to use this custom connector, you'll need to create an API token and assign permissions to a specific data source within the Fliplet platform.

1 - Go into your app settings and generate an API Token. See Screenshot

![UI Where the API token can be generated](https://help.fliplet.com/wp-content/uploads/sites/2/2020/07/Screenshot-2022-11-09-at-11.15.56.png)

2 - Click on "create Token", then after the token is created, click on "Edit" you will get to this screen where you must choose which data source you want your token to access:

![Edit DS permissions](https://iili.io/H1b7CXa.md.png)

3 - Click on the button "Manage security rule" Next to the data source you want to access, you'll be taken to this screen where you'll need to select the type of operations you want to perform on your data source:

![Edit DS permissions](https://iili.io/H1bY1Zg.md.png)

4 - Once you have selected the permissions, close this overlay and save your settings.

## Getting Started

You can get started with this connector by creating a simple flow in power automate:

- List all apps
- List all data sources
- List data in data sources
- Copy this data in an excel table or a sharepoint list

## Known Issues and Limitations

This connector is focused on operations in data sources, the limitations are:

- `POST`, `PUT` and `DELETE` actions targets individual entries, therefore the user may need to use the "Apply to each" action in power automate to cycle through entries
- `POST`, `PUT` actions rely on a specific JSON format, else the data sources can't interpret the data correctly, see examples in the Swagger definition
- The operation `CreateDataSourceRows` requires the `append` constant to be set on `TRUE`, else the data in the data source will be replaced.

## Deployment Instructions

There are two ways to deploy this connector:

- Creating a new connector from scratch in power automate/logic apps and importing the [Swagger (OpenAPI definition)](https://app.swaggerhub.com/apis/SuperOuss/FlipletRestAPI/0.2#/default/getAllApps)
- Once certified, users can browse to this connector directly in Power automate