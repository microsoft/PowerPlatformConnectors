# Microsoft Acronyms
You can use the Microsoft Search service in Microsoft Graph to search, add, and update acronyms in your organization.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
This connector requires the use of a Microsoft work account.

## Obtaining Credentials
You will need an Azure Entra ID app registration client ID and client secret, along with the delegated app permissions for SearchConfiguration.ReadWrite.All and Acronym.Read.All.

## Supported Operations
### Search acronyms
Retrieve a list of acronyms based on the search query.
### List acronyms
Retrieve a list of the acronym and their properties.
### Create acronym
Creates a new acronym.
### Get acronym
Read the properties and relationships of an acronym.
### Delete acronym
Deletes an acronym.
### Update acronym
Update the properties of an acronym.

## Known Issues and Limitations
This connector uses the Graph beta API endpoints, as that is the newest product version.
