## VEV-IQ Charging Station data connector
The Custom Connector fo the VEV-IQ Charging Station Supervision Platform allows you to seamlessly integrate and retrieve data from your charging station management system into the Power Platform. This connector enables you to access transaction data, consumption records, user information, and more on a per-tenant basis.


## Prerequisites
You will need the following to proceed:
* Sign in to your VEV-IQ platform or contact an administrator to have one: kinza.benslimane@vev.com
* You can get a subscription directly via https://www.vev.com/
* Having an API user dedicated in your VEV-IQ environment

The environment requirements are:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* The Power platform CLI tools


## Building the connector 
Since VEV-IQ is secured, you first need to authenticate to the VEV-IQ APIs and retrieve your bearer token. After that is completed, you can use the VEV-IQ connector.

### Authenticate to retrieve your Bearer Token using Postman
To access the API securely and retrieve a bearer token, you can use Postman, a popular API testing tool. Follow these steps to obtain a bearer token using the provided API endpoint https://rest.vev-iq.com/v1/auth/signin and request body:

Install and Open Postman:
If you don't have Postman installed, you can download it from [Postman's official website](https://www.postman.com/downloads/). Once installed, open Postman.

Create a New Request:
Click on the "New" button in Postman to create a new request.

Select the Request Type:
Choose the HTTP request type "POST" from the dropdown menu next to the URL input field.

Enter the API URL:
In the URL field, enter the API endpoint https://rest.vev-iq.com/v1/auth/signin.

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



