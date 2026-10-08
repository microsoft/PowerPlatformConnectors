# Jordan Open Data (Independent Publisher)

Jordan Open Data is the national open government data portal of the Hashemite Kingdom of Jordan, published at [opendata.gov.jo](https://opendata.gov.jo). It hosts several thousand datasets from more than 150 government agencies, covering areas such as health, education, agriculture, transport, tourism, the economy and the environment, with metadata in both English and Arabic. This connector lets Power Automate flows, Power Apps, and Microsoft Copilot Studio agents search the dataset catalogue, read dataset and resource metadata, browse publishing agencies, thematic groups, tags and licences, and query the tabular rows held in the portal DataStore, using the CKAN Action API that powers the portal.

## Publisher: Dan Romano

**Stack Owner:** Hashemite Kingdom of Jordan

## Prerequisites

There are no licensing or account prerequisites for this connector. Jordan Open Data is a public open data service, and every operation in this connector reads data that is published for public use.

You will need:

- A Power Automate, Power Apps, or Microsoft Copilot Studio environment in which to create the connection.
- The name or identifier of the dataset, organization, group or resource you want to read, for any operation that works on a single item. The **Search datasets**, **List organizations**, **List groups** and **List dataset names** operations return these identifiers.

## Obtaining Credentials

No credentials are required. The Jordan Open Data portal serves every read operation in this connector anonymously, so this connector uses no authentication and the connection is created without entering a key, a user name, or a password.

When you create a connection in Power Automate, Power Apps, or Microsoft Copilot Studio, select **Create** and the connection is ready to use immediately.

The portal does issue API tokens to registered users, but the [portal API guide](https://opendata.gov.jo/en/api-guide/) states that a token is required only for the dataset creation and update actions, which are restricted to authorised government publishers. This connector is read-only and therefore sends no token.

## Supported Operations

| Operation | Description |
|-----------|-------------|
| **SearchDatasets** | Searches the catalogue and returns the matching datasets with their full metadata, resources and tags. |
| **GetDataset** | Retrieves a single dataset by its name or identifier, including its bilingual title and description, publishing agency, licence, tags, groups and resources. |
| **ListDatasetNames** | Returns the names of the datasets published on the portal as a plain list of identifiers. |
| **ListOrganizations** | Returns the government agencies that publish datasets on the portal, with the title, description, logo and dataset count of each. |
| **GetOrganization** | Retrieves a single publishing organization by its name or identifier. |
| **ListGroups** | Returns the thematic groups used to categorise datasets, such as agriculture, health and education. |
| **GetGroup** | Retrieves a single thematic group by its name or identifier. |
| **GetResource** | Retrieves the metadata of one downloadable file or link attached to a dataset, including its download URL, format and size. |
| **SearchResources** | Searches the downloadable resources published on the portal using a field and value expression, such as `format:CSV`. |
| **SearchDatastoreRecords** | Returns the tabular rows held in the DataStore for a resource, together with the name and data type of every column. |
| **ListTags** | Returns the keyword tags applied to datasets across the portal, optionally filtered by a search word. |
| **ListLicenses** | Returns the licences that datasets can be published under, including the Open Jordanian Licence. |

## Getting Started

A typical flow that reads real data uses three operations in sequence:

1. Call **Search datasets** with a **Search Query** such as `health`, or a **Filter Query** such as `res_format:CSV`, to find the datasets you are interested in.
2. Read the **Resources** collection of a result and take the **Resource ID** of the file you want, checking that its **DataStore Active** property is `true`.
3. Call **Search DataStore records** with that resource identifier to return the actual rows of the table.

To restrict a search to a single publishing agency, call **List organizations**, take the **Name** of the agency, and pass it to **Search datasets** as a **Filter Query** of `organization:<name>`. The same pattern works for file formats with `res_format:CSV` and for other indexed fields.

If a resource is not loaded into the DataStore, its **URL** property still gives you a direct download link, which you can pass to an HTTP action or a **Create file** action to retrieve the file itself.

## Known Issues and Limitations

- **The API accepts POST only.** Every operation in this connector is a POST request whose options are sent in the request body, which is how the connector is defined. The portal rejects GET requests to the Action API with HTTP 400 and the message `Please use POST method for your request`, even though the portal's own API guide shows GET examples. No action is needed by the user, but this is worth knowing when comparing the connector with the published documentation or with other CKAN portals, most of which do accept GET.
- **Authentication.** This connector uses no authentication because every read operation on the portal is public. OAuth is not supported for Independent Publisher connectors at this time.
- **Read-only.** The portal also exposes dataset creation and update actions, which require an API token that is issued only to authorised government publishers. Those actions are deliberately not included, so this connector cannot create, update, or delete anything on the portal.
- **DataStore columns vary by resource.** Each resource defines its own table, so the columns inside a DataStore row are not fixed and cannot be described in the connector definition. The rows are returned as raw JSON. Read the **Columns** collection returned by **Search DataStore records** to see the column names and data types, then add a **Parse JSON** action in your flow to turn the rows into typed dynamic content.
- **Not every resource is queryable.** Only resources whose **DataStore Active** property is `true` can be read with **Search DataStore records**. At the time of writing the portal holds roughly 650 DataStore tables, which is a minority of the resources in the catalogue. For any other resource, download the file from its **URL** property instead.
- **Resource search needs a field and value expression.** The **Search Query** option of **Search resources** must name the field being searched, for example `format:CSV` or `name:budget`. A bare search word such as `budget` is rejected by the portal with HTTP 409.
- **Faceting is not available.** The portal returns an empty facet collection for catalogue searches, so facet counts are not exposed by this connector. Use **List organizations**, **List groups** and **List tags** to enumerate the values you can filter on, and the **Filter Query** option to apply them.
- **Listing the datasets of an organization or group.** The portal does not return the member datasets as part of **Get organization** or **Get group**. To list them, call **Search datasets** with a **Filter Query** of `organization:<name>`.
- **Metadata quality varies.** The catalogue contains a small number of placeholder and test records left over from portal migration, with values such as `test` in the title and author fields. Filter these out in your flow if they affect your results, and prefer the **Modified** date and the **Resource Count** as indicators of a genuine dataset.
- **Bilingual metadata.** Titles, descriptions, tags and licence names are published in separate English and Arabic properties, such as **Title** and **Title (Arabic)**. Not every dataset populates both, so check for an empty value before displaying one language.
- **Optional metadata fields.** Some declared properties, such as **Dataset Year** and **Uploaded File Name**, are populated on only a subset of records and are absent from the response for the rest. Handle them as optional values in your flow.
- **Automated clients may be blocked.** The portal sits behind a web application firewall that rejects some automated clients with HTTP 451 based on the user agent of the request. Power Platform's own user agent is accepted, so this does not affect the connector, but it can affect local testing with tools and scripts that send a recognisably scripted user agent.
- **No published rate limit.** The portal does not document a request quota and returns no rate limit headers. Flows that run on a short recurrence should still be designed to make as few calls as possible, because an undocumented limit may be applied at any time.
- **Privacy policy page.** The portal publishes a privacy policy page, but at the time of writing its content is a placeholder that reads "Content will be displayed as it becomes available".
