# Dexcom Community Connector
The Dexcom Community Connector enables the retrieval of a user's estimated glucose value (EGV) data, bringing the ability to monitor and share a Diabetic's glucose levels. You can find out more about the [Dexcom Developer API](https://developer.dexcom.com/overview) directly on their website.


## Pre-requisites
To use this connector, you need the following
1. A Microsoft Power Apps or Power Automate plan with custom connector feature
2. A Dexcom Developer account
3. An App created on your account
4. The Client ID and Client Secret from your App on your Dexcom account  
5. Ensure that you have clicked ['Apply for Upgrade'](https://developer.dexcom.com/user/me/apps) on your Dexcom App Account to use production data.

## Obtaining Credentials

To obtain the credentials you need to firstly create an app on the  [Dexcom Developer API](https://developer.dexcom.com/overview) website. 

1. Open the ['My Apps'](https://developer.dexcom.com/user/me/apps) page
2. Click 'Add an App'
3. Provide a name for your App
4. Click Create
5. Add a redirect URL of: https://global.consent.azure-apim.net/redirect
6. Click 'Save'

Your authentication credentials are now listed under 'Authentication'.

## API documentation
[Dexcom Rest API Documentation](https://developer.dexcom.com/overview)

## Supported Operations
The connector supports the following operations:
* [EGVS](https://developer.dexcom.com/get-egvs)
	- The /egvs endpoint enables retrieval of a user's estimated glucose value (EGV) data, including trend and status information.


## Known Issues and Limitations

Full Access is not required for apps to access production data. All Limited Access apps can have up to 20 authorized users, thus enabling Registered Developers to test their prototype apps with multiple users without going through the upgrade process. This data access still requires user authentication and HIPAA authorization via the OAuth 2.0 process, which is described in the [Authentication](https://developer.dexcom.com/authentication) section of the Dexcom API site.

## Frequently Asked Questions

### Does Dexcom provide a test environment for analyzing returned results from an API call?

Dexcom provides a sandbox API environment which you can make API requests to. To access this you can change the endpoint, security values as well as the action calls to https://sandbox-api.dexcom.com. If you would like to change back to the production endpoint you will need to change to https://api.dexcom.com.
