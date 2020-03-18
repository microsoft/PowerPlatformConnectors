## Outlook Tasks connector (Preview)
This connector provides actions against the Outlook Tasks APIs.

### Preview
This connector currently only supports operations against Task Groups and Task Folders.
Support for Tasks themselves will be open-sourced soon.

## API documentation
Detailed documentation of the target APIs can be found at [Outlook Task REST API reference](https://docs.microsoft.com/en-us/previous-versions/office/office-365-api/api/version-2.0/task-rest-operations).


## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.

In order to deploy as a custom connector, you will need to have a registered Microsoft Application. For information on how to register your app, see [Register an application with the Microsoft identity platform](https://docs.microsoft.com/en-us/graph/auth-register-app-v2).

Required scopes:
- `offline_access`
- `https://outlook.office.com/tasks.readwrite`

You will need to supply your app's `clientId` and `clientSecret` in order to deploy your custom connector.
