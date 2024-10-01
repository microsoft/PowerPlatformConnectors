# Github as a datasource

This connector allows you to grab a raw MD file from your github repo, and pull it in as data to then be manipulated based on your markdown format.

## Publisher: Nathalie Leenders | Wortell

## Prerequisites

You will need the following to proceed:

- A Microsoft PowerApps or Microsoft Flow plan with custom connector feature
- A premium license

## Supported Operations

The connector supports the following operations:

- `RetrieveData`: Retrieves data from a raw MD file in your GitHub repository.

## Deployment instructions

1. Navigate to the [Power Apps Custom Connectors page](https://make.powerapps.com/)
2. Click on `New custom connector` and select `Import an OpenAPI file`
3. Browse to the location of the `apiDefinition.swagger.json` and `apiProperties.json` files
4. Click `Continue`
5. Update the general information and security details on the following page
6. Click `Create connector`
7. Use the new connector in your Power Apps application

## Usage

There are 5 parameters built in:

1. Github name
2. Repo name
3. Folder name
4. Folder2 name
5. Markdownfile name

- Github name is your user account for github, or from the account you're trying to pull data from.
- Repo name is the name of your repo.
- Foldername is the name of the first folder
- Foldername2 is the underlying folders. If there are more than use use foldername/foldername2/foldername3 in this parameter if needed.
- Take note, it is open source so it doesn't need any authentication on github.

## Known issues and limitations

- The connector is currently in preview and may have some limitations.
