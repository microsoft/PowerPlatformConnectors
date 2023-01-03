# Coupa
See all of your business spend in one place with Coupa to make cost control, compliance and anything spend management related easier and more effective.
This API provides robust access to read, edit, or integrate your data with the Coupa platform

## Publisher: NovaGL

## Prerequisites

Login to your Coupa instance with an administrator\integration administrator account
-	Click Setup > OAuth2/OpenID Connect Clients
-	Create Client 
-	Grant Type: Authorization Code
-	Name: A descriptive name such as “Power Automate Connector”
-	Redirect URIs: https://global.consent.azure-apim.net/redirect
-	Select relevant scope such as core.common.read

![image](https://user-images.githubusercontent.com/16315601/210454583-f5083eac-ffec-457d-8dfc-1f452afdbf76.png)


## Power Automate

Login to Power Automate 
-	Click Data > Custom Connectors
-	Click New custom connector > Create from blank

### General


* 	Give it a descriptive name such as Coupa API
* 	Upload an image such as the Coupa logo
* 	Add a description eg
    - 	RESTful API that provides robust access to read, edit, or integrate your data with the Coupa platform.
* 	Add you instance url eg https://instance.coupahost.com


### Security

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

### Definition
The definition screen is broken down into four main sections and two areas.

You have the choice of using the UI area which allows you to enter data into the four section individually

You can also the text editor which is called the Swagger editor to enter everything in one go.

Some sections are only be created in the UI or the Swagger editor

#### Getting Started
You can vist your local instance to find out a list of all available API Commands.
https://instance.coupahost.com/api_docs/0

#### Getting Started
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


**Response**
This section is not required its useful however to fill it in as it allows future steps to have access to the response object

Click default > import from sample
Paste in the body the response you get from postman

#### Triggers
This is not currently used in the Coupa API, but potential uses are polling for changes

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

### Test

Click Create Connector
Click New connection
Assuming you have entered everything correctly in the security screen it should open up a Coupa permission screen.

Press allow to enable the OAuth 2.0 connection from Power Automate to Coupa

If everything is done correctly in the definition screen pressing Test Operation should provide you with a list of contracts

### Disclaimer

Power Automate has a limit of 512 objects. I've tried as far as I can to list all the common objects.
If what you require is missing please send me a note and I will see what I can do
