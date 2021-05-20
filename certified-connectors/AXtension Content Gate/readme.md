## AXtension Content Gate Connector
AXtension Content Gate helps you find all relevant content within a split second, regardless of storage location. Connect your Microsoft Dynamics 365 environment with AXtension Content Gate and create a single point-of-truth for all content in your organization.

## Prerequisites
You will need the following to proceed:
* An AXtension Content Gate environment. [More information](https://www.axtension.com/solutions/content-gate/)

## Documentation
Create a connection and use the dns subdomain of the environment to connect to (https://{dns-subdomain}.content-gate.com). For example, if the environment you want to connect to is located at https://contoso.content-gate.com, use 'contoso' (no quotes) when creating a connection.

## Supported Operations
The following operations are supported as part of the AXtension Content Gate connector.

### Store Content

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
