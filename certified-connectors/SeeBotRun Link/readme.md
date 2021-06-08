

# SeeBotRun Link Connector

The SeeBotRun Link connector allows organizations to create and managed short URLs, using the associated domains with their accounts.


## Prerequisites

An existing account with SeeBotRun and access to the Link platform. [Find out how to create an account](https://www.seebot.run/link/)


## How to get credentials?

- Have an existing account with SeeBotRun (see prerequisites).
- Once logged in, click the Support link in the header, select Link as the application, and include the message "Requesting API key for Power Automate"
- An API key, User Token, and User ID will be sent to you via email, securely.  


## Supported Operations
- **Create/Update/Delete Links**  Links can be created, updated, and deleted via the connector.  Note: When updating links created outside of the connector, any tag and marketing details will be removed.
- **Create/Update/Delete Predefined Links** Predefined links (second level links) can be created under a link when the link type is set to `predefined`.  Predefined links are key based; the system will create the link if the key is not already taken and update the link if it is found.


## Known issues and limitations

- Our connector only supports the key elements for links when creating/editing.  Tags and Marketing details are not included (to manage these, please use the administration application directly).
- Domains, Tags, and Reports are not currently supported as part of this connector.
