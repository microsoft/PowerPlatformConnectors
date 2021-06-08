

# SeeBotRun Link Connector

The SeeBotRun Link connector allows organizations to create and managed short URLs, using the associated domains with their accounts.  The connector is powered by the [SeeBotRun Link REST API](https://seebotrun.readme.io/).


## Prerequisites

You will need the following to proceed:
- A Microsoft Power Apps or Power Automate plan with custom connector feature.
- An existing account with SeeBotRun and access to the Link platform. [Find out how to create an account](https://www.seebot.run/link/).
- The Power platform CLI tools.


## Building the connector

### Account Setup
- Have an existing account with SeeBotRun (see prerequisites).
- Once logged in, click the Support link in the header, select Link as the application, and include the message "Requesting API key for Power Automate"
- An API key, User Token, and User ID will be sent to you via email, securely.  

### Deploying the Sample
Run the following command and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```


## Supported Operations
- `List Domains`: List all domains available to the account.
- `List Links`: List all links available to the account, for the specified domain.
- `Create Link`: Create a new link (default or predefined types only).
- `Update Link`: Update an existing link based on ID.
- `Delete Link`: Delete an existing link based on ID.
- `Create/Update Predefined Link`: Create or Update a predefined (second level) link, based on the provided link ID and key.
- `Delete Predefined Link`: Delete an existing predefined link, based on the provided link ID and key.


## Known issues and limitations

- Our connector only supports the key elements for links when creating/editing.  Tags and Marketing details are not included (to manage these, please use the administration application directly).
- Domains, Tags, and Reports are not currently supported as part of this connector.
