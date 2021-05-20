## AXtension Content Gate Connector
AXtension Content Gate helps you find all relevant content within a split second, regardless of storage location. Connect your Microsoft Dynamics 365 environment with AXtension Content Gate and create a single point-of-truth for all content in your organization.

## Prerequisites
You will need the following to proceed:
* An AXtension Content Gate environment. [More information](https://www.axtension.com/solutions/content-gate/)

## Documentation
Create a connection and use the dns subdomain of the environment to connect to (https://{dns-subdomain}.content-gate.com). For example, if the environment you want to connect to is located at https://contoso.content-gate.com, use 'contoso' (no quotes) when creating a connection.

## Supported Operations
The following operations are supported as part of the AXtension Content Gate connector.

### Execute Query
Executes a query against an AXtension Content Gate environment. Loads content and business entity information attached to the content items.

| Field                 | Description |
|-----------------------|-------------|
| Provider Reference Id | The connector reference id of the business entity that will be used as the query context. |
| External Type         | The type that is used in the external system for the business entity that will be used as the query context. |
| External Id           | The identifier that is used in the external system for the business entity that will be used as the query context. |
| View                  | The view that will be used to execute the query. [optional] |

### Get Content
Downloads the actual binary content for a given item.

| Field   | Description |
|---------|-------------|
| Id      | The identifier of the content item to download. |
| Mode    | The mode in which the content should be loaded. Leave empty to get the original file. Provide 'pdf' (without quotes) for pdf previews. |
| Context | The context in which the content should be downloaded. Preview-first-page only works if previewer caching is enabled. (possible values: preview, preview-full, preview-first-page). |

### List Business Entity Connectors
Lists all the business entity connnectors that are available in the system.

_No additional inputs required_

### Store Content
Stores new Content in an AXtension Content Gate environment.

| Field                      | Description |
|----------------------------|-------------|
| Title                      | The title of of the content to store. |
| Content Type               | The type of content to store. |
| Content                    | The actual content (binary / weblink) to store.  |
| File Name                  | The file name (if applicable). |
| Content Category           | The content category name or id that the content will be stored as. |
| Business Entity Connector  | The connector reference id of content to store. |
| Business Entity Type       | The business entity type of the entity to attach the content to. |
| Business Entity Identifier | The external identifier of the business entity to store. |
| _Additional Fields_        | Any additional fields defined as content entity properties are displayed as well. |

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.

## Further Support
For further support, you can contact us at support@axtension.com - we're always happy to help.
