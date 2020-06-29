
## Motimate Connector
Motimate is the result of more than 10 years of working with some of Scandinavia’s largest companies in improving their internal communication, increasing their sales and refining their employee training. We provide an award winning SAAS-solution making internal communication and corporate training fun and easy.
This connector is an easy to use alternative for using our API to automate tasks such as user registration or group management.
You can read more about Motimate [here](https://about.motimateapp.com/)

## Pre-requisites
You will need the following to use the Motimate connector:
* A subdomain to motimateapp.com
* An admin account to your organization
* A ClientID to the Motimate API

## Using the Motimate Connector

### Shared input parameters
Certain parameters are shared across most, if not all, tasks available in the Motimate connector. These are:
* `Subdomain`: This is your organization's subdomain to motimateapp.com. If your organization uses 'example.motimateapp.com' to log in to Motimate, the subdomain would be 'example'. The parameter is required for all operations in order to route your request to the correct organization's API.
* `Auth`: This parameter is required for all operations except the 'POST Token' operation. Use 'Bearer ' + the 'access_token' output from the 'POST Token' in all subsequent operations in order to authenticate your request to the API.

### Authentication
The first Motimate operation in any given flow must be acquiring an access token from the 'POST Token' operation. Acquiring the token only needs to be done once per flow. After you acquire the token, you pass the value 'Bearer ' along with the token in all subsequent Motimate operations.

## Supported Operations
The connector supports the following operations:

### Token
* `POST token`: Acquire an access token

### Users
* `GET users`: Get all users in the organization
* `GET users`: Get a given user by user_id
* `POST users`: Create a new user
* `PATCH users`: Update a user by user_id
* `DELETE users`: Delete a user by user_id

### Groups
* `GET groups`: Get all groups in the organization
* `GET groups`: Get a given group by group_id
* `POST groups`: Create a new group
* `PATCH groups`: Update a group by group_id
* `DELETE groups`: Delete a group by group_id

### Positions
* `GET positions`: Get all positions in the organization
* `GET positions`: Get a given group by position_id
* `POST positions`: Create a new position
* `PATCH positions`: Update a position by position_id
* `DELETE positions`: Delete a position by position_id