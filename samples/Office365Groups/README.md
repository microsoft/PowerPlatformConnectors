
### NOTE
> This is a *sample* connector.  The connector is provided here with the intent to illustrate certain features and functionality around connectors.

# Office365 Groups Connector (Sample)
Office 365 Groups lets you manage group membership and calendar events in your organization using your Office 365 account. You can perform various actions such as get group roster, add or remove members and create group events. 

Public Connector: https://docs.microsoft.com/en-us/Connectors/office365groups/

##  Setup

1. [Enable authentication in Azure Active Directory
](https://docs.microsoft.com/en-us/connectors/custom-connectors/azure-active-directory-authentication#enable-authentication-in-azure-active-directory) to setup authentication for your custom connector via AAD. Keep note of the `AAD application ID` and `Your-Base64-Key`.
2. Replace `aad_application_id` in apiProperties.json file with your `AAD application ID`.
3. Use CLI tool `paconn` to create a new custom connector. Enter `Your-Base64-Key` when prompted for the "OAuth2 client secret".
4. [Set the reply URL in Azure](https://docs.microsoft.com/en-us/connectors/custom-connectors/azure-active-directory-authentication#set-the-reply-url-in-azure), you can find the redirect URL in the security page of your custom connector.

## Supported Actions

The following actions are supported:

* `List all groups`: List all the groups available in an organization.

* `List groups that I am a member of`: List all groups that you are a member of.

* `List group members`: List all members in the given group and their details such as name, title, email, etc.

* `Join group`:  Add yourself to a group.

* `Remove member from group`: Remove a member from a group.

* `Create group`: Create a new group.

* `Delete group`: Delete a group.


Please note that each of these operations would succeed only if you have sufficient permissions to execute that operation on the group.
