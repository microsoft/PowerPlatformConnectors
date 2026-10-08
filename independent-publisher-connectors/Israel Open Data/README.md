# Israel Open Data (Independent Publisher)

Israel Open Data is the national open government data portal of the State of Israel, published at data.gov.il. It holds roughly 1,200 datasets from about 61 government ministries, authorities and municipalities, covering transport, health, statistics, mapping, environment, water, finance and other areas. This connector searches the dataset catalogue, reads dataset, publishing organisation and resource metadata, lists the licences in use, and queries the tabular rows held in the portal DataStore.

## Publisher: Dan Romano

**Stack Owner:** State of Israel

## Prerequisites

You need a Microsoft Power Apps or Power Automate plan with custom connector capability. You do not need an account on data.gov.il and you do not need an API key, because the portal serves its data API anonymously.

## Obtaining Credentials

No credentials are required. The Israel Open Data portal exposes its API without authentication, so the connector uses no authentication and there is nothing to configure when you create a connection.

## Supported Operations

### Search datasets

Searches the catalogue and returns the matching datasets with their full metadata, resources and tags. Use the Search Query option for free text and the Filter Query option to restrict results to a publishing organisation, a file format or another indexed field.

### Search datasets (advanced)

Performs the same search but sends the options in a request body rather than in the query string, which is easier when a query or filter value is long or contains a lot of punctuation.

This operation also carries the **Format Filter** option, which is the recommended way to find datasets by file type and is not available on Search datasets. Format Filter is a pick list of the twenty file formats actually in use on the portal: CSV, XLSX, XLS, PDF, JSON, GeoJSON, XML, KML, KMZ, SHP, GDB, DOC, DOCX, TXT, ZIP, RAR, JPEG, RSS, Web Service and Link. Choosing CSV returns only datasets that publish at least one CSV resource. You may choose more than one format, in which case a dataset must contain a resource in every format you chose, so picking CSV and PDF together returns only datasets that have both. Leave the option empty to search without filtering by format. Format Filter can be combined with the Filter Query option, for example to return only the CSV datasets of a single ministry.

### Get dataset

Returns the full metadata of a single dataset, including every resource attached to it, its tags, its publishing organisation and its licence.

### List dataset names

Returns the short name of every dataset published on the portal as a plain list of text values. This list also fills the Dataset ID or Name picker on the Get dataset operation.

### Search dataset suggestions

Returns datasets whose name or title begins with the text you supply, as a short suggestion list. Use it to look up a dataset name before calling Get dataset. Note that an empty search text returns no suggestions, so always supply at least one character.

### List organisations

Returns the short name of every government ministry, authority and municipality that publishes data on the portal.

### Get organisation

Returns the details of a single publishing organisation, including its display title, its description and the number of datasets it has published. The Organisation ID or Name option is a picker that lists every organisation by its full title.

### Search organisation suggestions

Returns publishing organisations whose name or title matches the text you supply, with the short name and the full title of each one. An empty search text returns organisations without filtering, so this operation can also be used to list them with their titles.

### Get resource

Returns the metadata of a single resource, which is one downloadable file or linked service belonging to a dataset. This includes the download address, the file format, the update frequency and the data quality score the portal has assigned to the resource.

### Search resources

Searches the resources of the portal by the value of a single resource field, written as field:value. See the Known Issues and Limitations section below before using this operation.

### List resource views

Returns the views configured for one resource. A view is a preview the portal renders for a resource, such as a data table, a chart or a map.

### Search DataStore records

Returns the individual rows held in the DataStore for one resource, together with the name and data type of every column. Only resources whose DataStore Active property is true can be queried this way.

### Search DataStore records (advanced)

Returns DataStore rows using a request body, which lets you supply the Column Filters option as a structured object so you can filter on exact column values.

### Search DataStore with SQL

Runs a read only SQL query against the DataStore and returns the rows it produces. Name the resource identifier in double quotation marks as the table, for example `SELECT * FROM "e83f763b-b7d7-479e-b172-ae981ddc6de5" LIMIT 10`. This is the most flexible way to aggregate, join or filter DataStore data.

### List groups

Returns the short name of every group defined on the portal. The portal categorises its datasets by publishing organisation rather than by group, so only a small number of placeholder groups exist.

### List tags

Returns every keyword used across the datasets of the portal as a plain list of text values. Most tags are published in Hebrew.

### List licences

Returns every licence the portal offers for its datasets, with the identifier that appears in the Licence ID property of a dataset and the address of the full licence text.

### Search file format suggestions

Returns the file formats used by resources on the portal that match the text you supply, such as csv, xlsx or geojson. This is a discovery aid that reports the raw format labels recorded against resources. To filter a search by format, use the Format Filter option on Search datasets (advanced) instead, because that option uses the exact format values the catalogue index recognises.

## Getting Started

Start with **Search datasets** and leave the Search Query option empty to browse the catalogue, or supply a Hebrew or English term to narrow it down. Each dataset returned carries a Resources list. Take the Resource ID of a resource whose DataStore Active property is true and pass it to **Search DataStore records** to read the actual rows of data.

To work through a single publishing body instead, call **List organisations**, pass one of the returned short names to **Search datasets** as the Filter Query option in the form `organization:lamas`, and then follow the same path into the resources.

## Known Issues and Limitations

- **The portal content is mainly in Hebrew.** Dataset titles, descriptions, tag names, organisation titles and some licence titles are published in Hebrew. Dataset and organisation short names are usually Latin transliterations or English keywords, so they are the most reliable values to filter and match on. Plan for right-to-left text when you display these values.

- **The portal website is currently unavailable.** As of August 2026 the data.gov.il website returns an error page on every address, including the home page. Only the data API at `https://data.gov.il/api/3/action` is reachable, and it is fully working. The About and Terms pages published with this connector are the portal's own genuine pages and are listed in its live sitemap, but they will not load until the website is restored.

- **The dataset picker lists every dataset.** The Dataset ID or Name option on Get dataset is filled from the full catalogue, so the picker holds about 1,200 entries listed by short name. Use Search dataset suggestions or Search datasets when it is easier to find a dataset by searching than by scrolling.

- **Search resources returns no matches.** The resource index behind this operation is not populated on this portal, so the operation succeeds and returns a count of zero for every query, even for file formats that clearly exist in the catalogue. To find resources, use Search datasets or Get dataset and read the Resources list of each dataset. The operation is included because it is a standard read operation of the portal platform and may begin returning results if the index is rebuilt.

- **Some portal operations are not available and are therefore not included.** The portal status, the dataset activity stream, the organisation activity stream and the dataset schema operations all return an error rather than data on this portal, so this connector does not expose them.

- **Licence flags are returned as text.** The Covers Content, Covers Data, Covers Software and Is Generic properties of a licence are returned as the text `True` or `False` rather than as true or false values. Compare them as text.

- **DataStore rows have no fixed shape.** Every resource in the DataStore has its own set of columns, so the Rows output of the DataStore operations cannot be given fixed properties. Read the Columns output of the same response to discover the column names and data types of the resource you are querying, then address the row values by those names.

- **DataStore rows are always returned as objects.** The record format is fixed so that the response shape stays predictable. The comma separated value and tab separated value output modes offered by the portal platform are not exposed.

- **SQL queries are read only and carry an internal column.** Search DataStore with SQL accepts SELECT statements only, and the resource identifier must be wrapped in double quotation marks to be used as the table name. Rows returned by this operation also carry a `_full_text` column that the portal uses internally for searching, which you can ignore.

- **Dataset suggestions need at least one character.** Search dataset suggestions matches on the beginning of a dataset name or title and returns nothing at all for an empty search text, unlike the organisation and file format suggestion operations, which both return results when the search text is empty.

- **Groups are not used on this portal.** List groups returns only two placeholder groups, because the portal organises its catalogue by publishing organisation instead.

- **The Format Filter option is only on the advanced dataset search.** The portal accepts the underlying filter list on a request body but rejects it on a query string, so Search datasets does not offer the option. To filter by format on Search datasets, type the filter yourself into the Filter Query option in the form `res_format:CSV`. Note that these format values are case sensitive, so `res_format:csv` in lower case matches nothing.

- **Search file format suggestions does not match the Format Filter list.** The suggestion operation reports the raw format labels recorded against resources, which are lower cased and include a number of one-off and malformed entries. The Format Filter option instead offers the twenty values the catalogue index actually recognises. Use Format Filter to filter, and treat the suggestion operation purely as a way to see what formats exist.

- **No paging cursor on the catalogue search.** Search datasets returns at most 1,000 datasets in one call. Use the Skip Count option together with the Number of Results option to page through a larger result set, and read the Total Matches output to work out how many pages you need.

- **No published rate limit.** The portal does not document a rate limit and does not return any rate limit headers. No throttling was observed during testing, but the limits are not guaranteed, so leave a short delay between calls when you loop over many datasets or resources.

- **No authentication and no write operations.** The connector is read only. It cannot create, update or delete datasets, resources or rows, and it cannot read private datasets.

- **Organisation details are returned without the internal portal secret.** The portal includes an internal secret field in its organisation record. That field is deliberately left out of this connector, so it is not returned to your flows or apps.
