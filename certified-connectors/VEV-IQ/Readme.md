# VEV-IQ
The Custom Connector for the VEV-IQ Charging Station Supervision Platform allows you to seamlessly integrate and retrieve data from your charging station management system into the Power Platform. This connector enables you to access transaction data, consumption records, user information, and more on a per-tenant basis.

## Publisher: VEV PLATFORM SERVICES FRANCE

## Prerequisites
* A VEV-IQ tenant
* A VEV-IQ account, you can get a subscription directly via [VEV](https://www.vev.com/)
* A VEV-IQ [API Key and a User API account](https://flint-cousin-851.notion.site/Generate-your-TOKEN-API-3025ff2726be4fc4a0e61aa02a50c5e7?pvs=4)

The environment requirements are:
* A Microsoft Power Apps or Power Automate plan with custom connector feature

## Supported Operations
The connector supports the following operations:
* `List sites`: Lists all the sites in your tenant
* `List charging stations`: Lists all the charging stations in your tenant
* `Get transactions`: Gets all transactions in your tenant
* `Get completed transactions`: Gets all completed transactions in your tenant
* `Get active transactions`: Gets all active transactions in your tenant
* `List users`: Lists all users in your tenant
* `List tags`: Lists all tags in your tenant
* `List assets`: Lists all assets in your tenant
* `List car catalogs`: List all car catalogs in your tenant
* `List car makers`: Lists all car makers in your tenant
* `List cars`: Lists all cars on your tenant
* `List site areas`: Lists all site areas in your tenant
* `Get statistics charging stations consumption`: Gets statistics charging stations consumption in your tenant
* `Get statistics charging stations inactivity`: Gets statistics charging stations inactivity in your tenant
* `Get statistics charging stations usage`: Gets statistics charging stations usage in your tenant
* `Get statistics charging station transaction`: Gets statistics charging stations transactions in your tenant
* `Get statistics users consumption`: Gets statistics users consumption in your tenant
* `Get statistics users inactivity`: Gets statistics users inactivity in your tenant
* `Get statistics users usage`: Gets statistics users usage in your tenant
* `Get statistics users transaction`: Gets statistics users transactions in your tenant
* `Get statistics users pricing`: Gets statistics users pricing in your tenant
* `Get invoices`: Gets invoices information from your tenant


## Obtaining Credentials
To access the API securely and retrieve a bearer token, you can use Postman, a popular API testing tool. Follow these steps to obtain a bearer token using the provided [API Sign in endpoint](https://rest.vev-iq.com/v1/auth/signin) and request body:

Install and Open Postman:
If you don't have Postman installed, you can download it from [Postman's official website](https://www.postman.com/downloads/). Once installed, open Postman.

Create a New Request:
Click on the "New" button in Postman to create a new request.

Select the Request Type:
Choose the HTTP request type "POST" from the dropdown menu next to the URL input field.

Enter the API URL:
In the URL field, enter the [API Sign in endpoint](https://rest.vev-iq.com/v1/auth/signin).

Enter Request Body:
Click on the "Body" tab below the headers section. Choose the "raw" option and set the format to "JSON (application/json)".

Paste the following JSON request body into the input field:
{
  "email": "api.user@domain.com",
  "password": "Your-API-Password",
  "acceptEula": true,
  "tenant": "Your-Tenant-Name"
}

Replace "api.user@domain.com", "Your-API-Password", and "Your-Tenant-Name" with your actual credentials and tenant information.

Send the Request:
Click the "Send" button to submit the request to the API.

Retrieve Bearer Token:
Once the request is sent successfully, you'll receive a response from the API. Look for the "token" field in the response body. This is the bearer token you need to authenticate subsequent requests.

Use Bearer Token:
Copy the bearer token from the response. You can now use this token as an "Authorization" header in your subsequent API requests.

Remember that you can retrieve bearer tokens eonly if you have a dedicated API user in the VEV-IQ environment. Please contact your administrator to do so.

This process allows you to retrieve a bearer token using Postman, which can then be used to securely access the API endpoints in your custom connector.

## Known Issues and Limitations
There is a Limit of 100 number of records on each Get query. 
The data records you get depends on your user role and permission.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.