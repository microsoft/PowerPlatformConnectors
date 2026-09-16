# Palestine Open Data (Independent Publisher)

Palestine Open Data is the national open government data portal of the State of Palestine, published at opendata.ps. It holds datasets from Palestinian government ministries and institutions covering education, health, agriculture and other areas, most of it published in Arabic. This connector searches the dataset catalogue, reads dataset, publishing organisation and resource metadata, lists the licences and topic groups in use, and queries the tabular rows held in the portal DataStore.

## Publisher: Dan Romano

**Stack Owner:** State of Palestine

## Prerequisites

You need a Microsoft Power Apps or Power Automate plan with custom connector capability. You do not need an account on opendata.ps and you do not need an API key, because the portal serves its data API anonymously.

## Obtaining Credentials

No credentials are required. The Palestine Open Data portal exposes its API without authentication, so the connector uses no authentication and there is nothing to configure when you create a connection.

## Supported Operations

### Search datasets

Searches the catalogue and returns the matching datasets with their full metadata, resources and tags. Use the Search Query option for free text and the Filter Query option to restrict results to a publishing organisation, a file format or another indexed field. Dataset titles and descriptions are published mainly in Arabic.

### Search datasets (advanced)

Performs the same search but sends the options in a request body rather than in the query string, which is easier when a query or filter value is long or contains a lot of punctuation.

### Get dataset

Returns the full metadata of a single dataset, including every resource attached to it, its tags, its publishing organisation and its licence. Supply either the dataset name used in the portal address or the dataset identifier.

### List dataset names

Returns the short name of every dataset published on the portal as a plain list of text values. This list also fills the Dataset ID or Name picker on the Get dataset operation.

### Search dataset suggestions

Returns datasets whose name or title begins with the text you supply, as a short suggestion list. Use it to look up a dataset name before calling Get dataset. An empty search text returns no suggestions, so always supply at least one character.

### List organisations

Returns the short name of every government ministry, authority and municipality that publishes data on the portal.

### Get organisation

Returns the details of a single publishing organisation, including its display title, its description and the number of datasets it has published. The Organisation ID or Name option is a picker sourced from Search organisation suggestions.

### Search organisation suggestions

Returns publishing organisations whose name or title matches the text you supply, with the short name and the full title of each one. An empty search text returns organisations without filtering, so this operation can also be used to list them with their titles.

### Get resource

Returns the metadata of a single resource, which is one downloadable file or linked service belonging to a dataset. This includes the download address, the file format, the update frequency and the data quality score the portal has assigned to the resource.

### Search resources

Searches the resources of the portal by the value of a single resource field, written as field:value. Unlike some other CKAN portals in this connector family, this operation's index is populated and returns real matches.

### List resource views

Returns the views configured for one resource. A view is a preview the portal renders for a resource, such as a data table, a chart or a map.

### Search DataStore records

Returns the individual rows held in the DataStore for one resource, together with the name and data type of every column. Only resources whose DataStore Active property is true can be queried this way.

### Search DataStore records (advanced)

Returns DataStore rows using a request body, which lets you supply the Column Filters option as a structured object so you can filter on exact column values.

### Search DataStore with SQL

Runs a read only SQL query against the DataStore and returns the rows it produces. Name the resource identifier in double quotation marks as the table, for example `SELECT * FROM "e83f763b-b7d7-479e-b172-ae981ddc6de5" LIMIT 10`. This is the most flexible way to aggregate, join or filter DataStore data.

### List groups

Returns the short name of every topic group defined on the portal, such as education, health or agriculture. Unlike some other CKAN portals in this connector family, this portal uses real topic groups rather than placeholders.

### List tags

Returns every keyword used across the datasets of the portal as a plain list of text values. Most tags are published in Arabic.

### List licences

Returns every licence the portal offers for its datasets, with the identifier that appears in the Licence ID property of a dataset and the address of the full licence text. Some licence titles are published in Arabic.

### Search file format suggestions

Returns the file formats used by resources on the portal that match the text you supply, such as csv, xlsx or geojson. This is a discovery aid that reports the raw format labels recorded against resources.

## Testing These Operations

The table below lists a known-good input for every operation, verified live against opendata.ps. To try one yourself, open the connector in Power Automate or Power Apps, go to its **Test** tab (or **Edit** the connection on an existing one), select the operation, enter the value shown in the **Test input** column into the named field, and select **Test operation**.

| Operation | Test input | How to test |
|---|---|---|
| **SearchDatasets** | Search Query = `humanitarian`, Number of Results = `20` | Returns up to 20 datasets whose metadata matches "humanitarian". |
| **SearchDatasetsAdvanced** | Search Options body with Search Query = `Health Facilities` | Same search as above, sent as a request body instead of query parameters — use this when a query string would be awkward. |
| **GetDataset** | Dataset ID or Name = `census-of-agriculture-2020` | Returns the full metadata and resource list for that dataset. |
| **ListDatasetNames** | No parameters required | Returns the short name of every dataset on the portal. |
| **SearchDatasetSuggestions** | Search Text = `agriculture` | Returns datasets whose name or title begins with "agriculture". |
| **ListOrganizations** | No parameters required | Returns the short name of every publishing organisation. |
| **GetOrganization** | Organisation ID or Name = `pmof` | Returns the details of the Palestinian Ministry of Finance organisation record. |
| **SearchOrganizationSuggestions** | Search Text = `pcbs` | Returns organisations whose name or title matches "pcbs" (the Palestinian Central Bureau of Statistics). |
| **GetResource** | Resource ID = `8932e162-67a3-43e8-8329-99f1af64106c` | Returns the metadata of that resource, including its download URL and format. |
| **SearchResources** | Field Query = `format:CSV` | Returns resources whose format field equals CSV — the query must be written as `field:value`, a bare word is rejected. |
| **ListResourceViews** | Resource ID = `8932e162-67a3-43e8-8329-99f1af64106c` | Returns the views (if any) configured for that resource. |
| **SearchDatastoreRecords** | Resource ID = `d6da5817-dd92-4510-b83b-e1e8f16601ba`, Search Query = `Jenin` | Returns the DataStore rows of that resource that mention "Jenin". |
| **SearchDatastoreRecordsAdvanced** | Resource ID = `d6da5817-dd92-4510-b83b-e1e8f16601ba`, Search Query = `Jenin` | Same query as above, sent as a request body — use this when you also need the Column Filters option. |
| **SearchDatastoreWithSql** | SQL Query = `SELECT * FROM "d6da5817-dd92-4510-b83b-e1e8f16601ba"` | Runs the SQL statement against the resource's DataStore table. The resource identifier must be wrapped in double quotation marks, as it is used as the table name. |
| **ListGroups** | No parameters required | Returns the portal's 19 topic groups. |
| **ListTags** | No parameters required | Returns every keyword used across the catalogue. |
| **ListLicenses** | No parameters required | Returns every licence the portal offers. |
| **SearchFormatSuggestions** | Search Text = `csv` | Returns file formats matching "csv". This portal's datasets use exactly one format (CSV), so this is the only value worth searching for — searching for another format such as `xlsx` or `pdf` correctly returns no matches. |

## Getting Started

Start with **Search datasets** and leave the Search Query option empty to browse the catalogue, or supply an Arabic or English term to narrow it down. Each dataset returned carries a Resources list. Take the Resource ID of a resource whose DataStore Active property is true and pass it to **Search DataStore records** to read the actual rows of data.

To work through a single publishing body instead, call **List organisations**, pass one of the returned short names to **Search datasets** as the Filter Query option in the form `organization:pmof`, and then follow the same path into the resources.

## Known Issues and Limitations

- **The portal content is genuinely Arabic-first.** Dataset titles, descriptions, tag names, organisation titles and even DataStore column names are published in Arabic, not just supplementary fields. Dataset and organisation short names (`name`) are the most reliable values to filter and match on. Plan for right-to-left text when you display these values.

- **Only one file format is in use.** Every resource on the portal is published as CSV. The Search resources and Search file format suggestions operations work correctly but will only ever surface CSV, so no format picker or filter enum is offered by this connector — filter on `res_format:CSV` in the Filter Query option if you need to be explicit.

- **No dedicated privacy or terms page exists on the portal.** Only `/about` resolves; `/privacy-policy`, `/terms`, `/terms-and-conditions` and their English-prefixed equivalents all return 404. The Website and Privacy policy metadata fields on this connector both point to the About page.

- **DataStore rows have no fixed shape.** Every resource in the DataStore has its own set of columns, so the Rows output of the DataStore operations cannot be given fixed properties. Read the Columns output of the same response to discover the column names and data types of the resource you are querying, then address the row values by those names.

- **DataStore rows are always returned as objects.** The record format is fixed so that the response shape stays predictable. The comma separated value and tab separated value output modes offered by the portal platform are not exposed.

- **SQL queries are read only.** Search DataStore with SQL accepts SELECT statements only, and the resource identifier must be wrapped in double quotation marks to be used as the table name.

- **Dataset suggestions need at least one character.** Search dataset suggestions matches on the beginning of a dataset name or title and returns nothing at all for an empty search text.

- **No published rate limit.** The portal does not document a rate limit and does not return any rate limit headers. No throttling was observed during testing, but the limits are not guaranteed, so leave a short delay between calls when you loop over many datasets or resources.

- **No authentication and no write operations.** The connector is read only. It cannot create, update or delete datasets, resources or rows, and it cannot read private datasets.

- **Some metadata fields can be inconsistent in type.** A small number of fields, such as a DataStore query's estimated row count, have been observed returned as different data types (for example a number in one response and a string in another) depending on the query. These fields are declared without a fixed type in this connector so that either shape passes through without error.
