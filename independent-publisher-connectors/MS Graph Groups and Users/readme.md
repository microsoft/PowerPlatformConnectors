# MS Graph Groups and Users
The MS Graph Groups and Users connector utilizes Microsoft Graph to help retrieve Azure AD groups, members (users) and license details. Users can utilize this connector in their app or a flow.

## Publisher: Jay Jani

## Prerequisites
Azure Subscription and Azure AD App registration with specific permissions.

## Supported Operations
### List Users
Retrieve all users in the organization (AAD Tenant).

### List Groups By Display Name Search
Retrieve groups by searching group's display name with specific keywords.

### List Subscribed Skus
Retrieve details of organization's subscribed license plan skus.

### List Direct Group Members
Retrieve direct members of a group with count.

### Get Member License Details
Retrieve group member(user)'s license details.

### Get Group Properties
Retrieve properties and relationships of an AAD group.

### Get Member Groups
Group memberships for a user (member). This operation will retrieve list of AAD groups the member belongs to.

## Obtaining Credentials
Refer: https://docs.microsoft.com/en-us/graph/auth-v2-service#authentication-and-authorization-steps
1) Register your app (https://docs.microsoft.com/en-us/graph/auth-v2-service#1-register-your-app)
2) Add Credentials (https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app#add-a-client-secret)
3) Configure app permissions
4) Get administrator consent

Note: For steps #3 and #4
You'll add following Graph API Delegated permissions and get administrator consent:
•	User.Read— This permission is normally already granted
•	User.Read.All — “Grant Admin consent for <your org>”
•	Directory.Read.All — “Grant Admin consent for <your org>”
•	GroupMember.Read.All — “Grant Admin consent for <your org>”


## API Documentation
https://graph.microsoft.com

## Known Issues and Limitations
None.
