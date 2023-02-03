# Fliplet integration connector

This custom connector will allow power automate and logic apps user to connect to the Fliplet platform and do a variety of operations on their data sources.

## Prerequisites

In order to use this custom connector, you'll need to create an API token and assign permissions to a specific data source within the Fliplet platform.

1. Go into your app settings and generate an API Token. See Screenshot

![UI Where the API token can be generated](https://help.fliplet.com/wp-content/uploads/sites/2/2020/07/Screenshot-2022-11-09-at-11.15.56.png)

2. Click on "create Token", then after the token is created, click on "Edit" you will get to this screen where you must choose which data source you want your token to access:

![Edit DS permissions](https://iili.io/H1b7CXa.md.png)

3. Click on the button "Manage security rule" Next to the data source you want to access, you'll be taken to this screen where you'll need to select the type of operations you want to perform on your data source:

![Edit DS permissions](https://iili.io/H1bY1Zg.md.png)

4. Once you have selected the permissions, close this overlay and save your settings.

That's it, you're ready to start working with this custom connector!

## Supported Operations

- `getAllApps` Lists all the apps linked to this API token
- `getAppsById` Display information about a specific app targeted by its Id
- `getAllDataSources` Lists all the data sources linked to this API token
- `getDataSourceById` Displays information about a specific data source targeted by its Id
- `getDataSourceEntry` Displays information about a specific data source entry targeted by its Id
- `updateDataSourceEntry` Updates a specific data source entry targeted by its Id using a JSON object
- `deleteDataSourceEntry` Deletes a specific data source entry targeted by its Id
- `getDataSourceEntries` Displays data source data as a JSON object
- `createDataSourceRows` Create a new data source entry using a JSON object