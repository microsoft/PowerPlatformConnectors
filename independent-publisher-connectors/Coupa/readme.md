# Coupa
See all of your business spend in one place with Coupa to make cost control, compliance and anything spend management related easier and more effective.
This connector provides robust access to read, edit, or integrate your data with the Coupa platform

## Publisher: NovaGL

## Prerequisites

*   Go to {instance}.coupahost.com (replacing the word instance with your instance name)
*   Login with an administrator\integration administrator account
*	Click Setup > OAuth2/OpenID Connect Clients
*	Create Client 
*	Grant Type: Authorization Code
*	Name: A descriptive name such as “Power Automate Connector”
*	Redirect URIs: https://global.consent.azure-apim.net/redirect
*	Select relevant scope such as core.common.read

![image](https://user-images.githubusercontent.com/16315601/210454583-f5083eac-ffec-457d-8dfc-1f452afdbf76.png)

Once saved open up the newly saved connector and copy the details mentioned on the screen
![image](https://user-images.githubusercontent.com/16315601/212443775-50ed7b37-74f1-4488-9f8a-62a0ea618a1a.png)

For more information Please see here - https://compass.coupa.com/en-us/products/core-platform/integration-playbooks-and-resources/integration-knowledge-articles/oauth-2.0-getting-started-with-coupa-api

## Power Automate

Login to Power Automate 
*	Click Data > Custom Connectors
*	Click New custom connector > Create from blank

### General


* 	Give it a descriptive name such as Coupa API
* 	Upload an image such as the Coupa logo
* 	Add a description eg
    - 	RESTful API that provides robust access to read, edit, or integrate your data with the Coupa platform.
* 	Add you instance url eg https://instance.coupahost.com


## Supported Operations

### Contracts - Get Contracts
The following is an example of how to get contracts

**General**

| Summary | Description | Operation ID |
| ------------- |-------------| -----|
| Contracts - Get Contract by ID    | Get Contract by ID | Contract_GET|

**Request**

Click Import from sample

| Verb | URL| Headers |
| ------------- |-------------| -----|
|` GET `| /api/contracts?return_object=limited&limit=1&offset=50&dir=desc| Accept application/json|

This will fill in query and header elements that can be filled in when you run the follow.

Since some of them have predefined values we can fill them in prior

Eg click return_object > edit

| Title        | Input|           
| ------------- |-------------|
| Dropdown type        | Static|
| Values        | "shallow", "limited", "none"|

Now click on Accept under Headers and press edit


| Title        | Input|           
| ------------- |-------------|
| Default value       | application/json|
| Is required?| Yes|
| Visibility | Internal|


### Addresses - Create Address

The following examples explains how to create a ship to address via the Coupa API

Scopes: `core.common.read core.common.write`

**General**
| Title|  Value |
|---|---|
|  Summary |  Addresses - Create Address |
| Description  |  Create Address |
| Operation ID|  Address_Create |

**Request**

Import from Sample
| Title|  Value |
|---|---|
|  Verb|  `POST`|
|  URL|  `/api/addresses/`|
| Headers |  `Accept application/json` (don’t forget to default the value)|

**Body**
```json
{
  "name": "string",
  "location-code": "string",
  "street1": "string",
  "street2": "string",
  "city": "string",
  "state": "string",
  "postal-code": "string"
} 
```
**Response**

Click import from sample and copy the above body into the body field.

### Addresses - Update Address

This example is quite similar to the above except for the fact we are updating not creating

Scopes: `core.common.read core.common.write`

**General**
| Title|  Value |
|---|---|
|  Summary |  Addresses - Update an Address |
| Description  |  Update Address |
| Operation ID|  Address_Update |

**Request**

Import from Sample
| Title|  Value |
|---|---|
|  Verb|  `POST`|
|  URL|  `/api/addresses/{id}`|
| Headers |  `Accept application/json` (don’t forget to default the value)|

**Body**
```json
{
  "name": "string",
  "location-code": "string",
  "street1": "string",
  "street2": "string",
  "city": "string",
  "state": "string",
  "postal-code": "string"
} 
```
**Response**

Click import from sample and copy the above body into the body field.


### Invoices - Delete Invoice by ID

To delete an invoice we have to make sure its in new status and able to be deleted

Scopes: `core.invoice.delete`

**General**
| Title|  Value |
|---|---|
|  Summary |  Invoices - Delete Invoices by ID |
| Description  | Delete Invoices by ID |
| Operation ID|  Invoices_Delete |

**Request**

Import from Sample
| Title|  Value |
|---|---|
|  Verb|  `DELETE`|
|  URL|  `/api/invoices/{id}`|
| Headers |  `Accept application/json` (don’t forget to default the value)|
| Body | Leave empty |

**Response**

Not valid for this request

## Obtaining Credentials

| Title        | Result |           
| ------------- |:-------------:|
| Authentication type       | OAuth 2.0 |

OAuth 2.0
Copy the following information from the Coupa OAuth client page

| Title        | Input|           
| ------------- |-------------|
| Identity Provider       | Generic Oauth 2|
| Client id      | Coupa Identifier|
| Client secret| Coupa secret|
| Authorization URL| https://instance.coupahost.com/oauth2/authorizations/new|
| Token URL|  https://instance.coupahost.com/oauth2/token|
| Refresh URL|  https://instance.coupahost.com/oauth2/token|
| Scope | Oidc Scopes without commas e.g  core.common.read core.contracts.read|

![image](https://user-images.githubusercontent.com/16315601/210454637-4d47e655-eb02-4e66-8a6c-19b96726e68c.png)

## Getting Started
You can vist your local instance to find out a list of all available API Commands.
https://instance.coupahost.com/api_docs/0


#### Triggers
This is not currently used in this Coupa connector, but potential uses are polling for changes

#### References 
If you have multiple actions that have similar objects in the request or response object you can reference them using the Swagger editor

#### Policies
Policies allow tasks to be automated an example would be to automate the addition of the Accept header.

Rather than changing each request individually we can create a policy to update all of them at once.

| Title        | Input|           
| ------------- |-------------|
| Name| Set ACCEPT Header|
| Template | Set HTTP Header|
| Operations| Leave blank as this will select all of them|
| Header name | Accept|
| Header value      | application/json|
| Action if header exists      | override|
| Run policy on      | Request|


## Known Issues and Limitations

Power Automate has a limit of 512 objects. I've tried as far as I can to list all the common objects.
If what you require is missing please send me a note and I will see what I can do.

## Frequently Asked Questions

**My results don't match my postman output**
This connector uses the User scope meaning that all output relates to that user. 
To see results from a system level you will have to use another method with `client credentials` authorizations method.
