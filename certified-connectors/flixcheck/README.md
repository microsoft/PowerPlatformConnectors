
## Flixcheck Connector
Flixcheck provides a powerful and very extensive REST API. Using this API, you can create and manage checks. This connector exposes a subset of these APIs as operations in PowerAutomate.

## Pre-requisites
You will need the following to proceed:
* Register at https://app.flixcheck.com/register

### Deploying
Run the following commands and follow the prompts:

```paconn
paconn create --settings settings.json --secret <client_secret>
```

## Supported Operations
The connector supports the following operations:
* `Subscribe to Check opened Event`: Subscribe to Check opened Event via Webhook trigger
* `Subscribe to Check completely finished Event`: Subscribe to Check completely finished Event via Webhook trigger
* `Subscribe to Check delivered Event`: Subscribe to Check delivered Event via Webhook trigger
* `Subscribe to Check created Event`: Subscribe to Check created Event via Webhook trigger
* `Get Check by ID`: Get Check by checkId via Action
* `Get Checks from Folder`: Get Checks filtered by folderId via Action
* `Create Check`: Create Check via Action
* `Get Template by ID`: Get Template by templateId via Action
* `Get Templates from all Folders`: Get Templates from all Folders via Action
* `Get Folders`: Get Folders by Type via Action




