# Criteo API. 

Criteo provides online display advertisements. Using this API, you can manage your audiences and advertisers. 

## Publisher: Microsoft

## Prerequisites
* A Criteo [account](https://www.criteo.com/) and corresponding administrator credentials.
* A Criteo developer [account](https://developers.criteo.com/) 

## Supported Operations
* `Get audiences` : Get a list of all the audiences for the user or for the given advertiser id
* `Create audience` : Create an Audience for an Advertiser
* `Delete identifiers` : Delete all identifiers from an Audience
* `Modify audience users` : Add/remove users to or from an audience
* `Delete audience` : Delete an audience by id
* `Modify audience` : Update user audience specified by the audience id
* `Get advertisers` : Fetch the portfolio of Advertisers for this account

## Obtaining Credentials
Authentication is provided via OAuth2 flow. Please follow Criteo's [guide](https://developers.criteo.com/marketing-solutions/docs/oauth-app-implementation) to create an app and obtain the clientId and clientSecret
