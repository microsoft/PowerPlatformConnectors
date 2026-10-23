# Libya Open Data (Independent Publisher)

Libya Open Data is the national open government data portal operated by the Libyan General Information Authority (الهيئة العامة للمعلومات), published at opendata.gia.gov.ly. It holds datasets from Libyan government institutions covering the economy, population, education, health, oil and gas, electricity, water resources and other areas, most of it published in Arabic. This connector searches the dataset catalogue, reads dataset, publishing organisation and resource metadata, lists the licences and topic groups in use, and queries the tabular rows held in the portal DataStore.

## Publisher: Dan Romano

**Stack Owner:** General Information Authority

## Prerequisites

You need a Microsoft Power Apps or Power Automate plan with custom connector capability. You do not need an account on opendata.gia.gov.ly and you do not need an API key, because the portal serves its data API anonymously.

## Obtaining Credentials

No credentials are required. The Libya Open Data portal exposes its API without authentication, so the connector uses no authentication and there is nothing to configure when you create a connection.

## Supported Operations

Each operation below carries a **Try it** line with a known-good input, verified live against opendata.gia.gov.ly on 28 August 2026. To run one, open the connector in Power Automate or Power Apps, go to its **Test** tab, select the operation, enter the values shown and select **Test operation**. Any counts given are the live figures at the time of writing and will drift as the portal grows.

### Search datasets

Searches the catalogue and returns the matching datasets with their full metadata, resources and tags. Use the Search Query option for free text and the Filter Query option to restrict results to a publishing organisation, a file format or another indexed field. Dataset titles and descriptions are published mainly in Arabic.

**Try it:** No parameters. Returns the whole catalogue (about 127 datasets), 20 per page. Add a Search Query such as `نفط` (Arabic for "oil") or a Filter Query such as `organization:gaz` to narrow it.

### Search datasets (advanced)

Performs the same search but sends the options in a request body rather than in the query string, which is easier when a query or filter value is long or contains a lot of punctuation.

**Try it:** Empty Search Options body (`{}`). Returns the whole catalogue, sent as a request body. Add `fq` such as `organization:gaz` inside the body to filter.

### Get dataset

Returns the full metadata of a single dataset, including every resource attached to it, its tags, its publishing organisation and its licence. Supply either the dataset name used in the portal address or the dataset identifier.

**Try it:** Dataset ID or Name = `c8cecaca-5897-4d14-8acf-54ef0d116b44`. Returns the "Digital government indicators in Libya (EGDI 2024)" dataset (short name `gov66`) with its DataStore-backed resource. The short name `gov66` works here too.

### List dataset names

Returns the short name of every dataset published on the portal as a plain list of text values. This list also fills the Dataset ID or Name picker on the Get dataset operation.

**Try it:** No parameters. Returns roughly 134 short names such as `gov66`, `emp3` and `gaz5`.

### Search dataset suggestions

Returns datasets whose name or title begins with the text you supply, as a short suggestion list. Use it to look up a dataset name before calling Get dataset. An empty search text returns no suggestions, so always supply at least one character.

**Try it:** Search Text = `libya`. Returns datasets whose name or title contains "libya", such as `numerical-distribution-of-libyan-deaths`. An Arabic search text matches on the Arabic titles.

### List organisations

Returns the short name of every government ministry, authority and information centre that publishes data on the portal.

**Try it:** No parameters. Returns the 16 organisation short names, such as `idce`, `labor` and `oil`.

### Get organisation

Returns the details of a single publishing organisation, including its display title, its description and the number of datasets it has published. The Organisation ID or Name option is a picker sourced from Search organisation suggestions.

**Try it:** Organisation ID or Name = `gaz`. Returns that organisation's record with its Arabic title and dataset count.

### Search organisation suggestions

Returns publishing organisations whose name or title matches the text you supply, with the short name and the full title of each one. An empty search text returns organisations without filtering, so this operation can also be used to list them with their titles.

**Try it:** Search Text = `gaz`. Returns the organisation whose name matches `gaz`. Leave Search Text empty to get the first organisations with their Arabic titles.

### Get resource

Returns the metadata of a single resource, which is one downloadable file or linked service belonging to a dataset. This includes the download address, the file format and the media type the portal has recorded for the resource.

**Try it:** Resource ID = `5109b2f8-9cd7-48ce-ab75-bf94b6738d62`. Returns that resource's metadata, including its download URL, format and size in bytes. Take any resource ID from the Resources list of a Get dataset or Search datasets response.

### Search resources

Searches the resources of the portal by the value of a single resource field, written as field:value, for example `format:CSV` or `name:population`. Only stored resource fields such as format, name, description and url can be searched, and the query must be written as `field:value` — a bare word is rejected.

**Try it:** Field Query = `format:CSV`. Returns the 23 CSV resources. A bare word with no colon, such as `population`, returns HTTP 409.

### List resource views

Returns the views configured for one resource. A view is a preview the portal renders for a resource, such as a data table, a chart or a map.

**Try it:** Resource ID = `057984a8-2e16-4fb0-a89a-de879c904876`. Returns the one `datatables_view` configured for that resource.

### Search DataStore records

Returns the individual rows held in the DataStore for one resource, together with the name and data type of every column. Only resources whose DataStore Active property is true can be queried this way, and only a small number of resources on this portal are loaded into the DataStore.

**Try it:** Resource ID = `057984a8-2e16-4fb0-a89a-de879c904876`. Returns the EGDI and OSI indicator rows with three columns (`_id`, `indecator_name`, `indecator_value`). The other two DataStore-backed resources are `238b663d-7223-45f9-bbf9-b06d09adbe32` and `7cde6c25-82ec-443e-9189-3d4e3399fa0a`.

### Search DataStore records (advanced)

Returns DataStore rows using a request body, which lets you supply the Column Filters option as a structured object so you can filter on exact column values.

**Try it:** Query Options body with Resource ID = `057984a8-2e16-4fb0-a89a-de879c904876` and Records Format = `objects`. Same rows as above; add a Column Filters object such as `{"indecator_name":"..."}` to match an exact value.

### List groups

Returns the short name of every topic group defined on the portal, such as economy, population, health, education, oil and electricity. Most datasets on the portal are assigned to one or more of these groups.

**Try it:** No parameters. Returns the 33 group short names.

### List tags

Returns every keyword used across the datasets of the portal as a plain list of text values. Most tags are published in Arabic.

**Try it:** No parameters. Returns the 24 tags currently in use.

### List licences

Returns every licence the portal offers for its datasets, with the identifier that appears in the Licence ID property of a dataset and the address of the full licence text. Some licence titles are published in Arabic.

**Try it:** No parameters. Returns 15 licences, including `notspecified`, `cc-by`, `cc-zero` and `other-open` (the one most datasets use).

### Search file format suggestions

Returns the file formats used by resources on the portal that match the text you supply, such as xlsx, csv, json or xml. Use a returned format with the Filter Query option of the Search datasets operation, for example `res_format:XLSX`.

**Try it:** Search Text = `csv`. Returns `csv`. Other text returns the matching formats in use — `xls` returns `xlsx` (the portal's most common format), `j` returns `json`, `x` returns `xml`.

## Getting Started

Start with **Search datasets** and leave the Search Query option empty to browse the catalogue, or supply an Arabic or English term to narrow it down. Each dataset returned carries a Resources list. Most resources on this portal are spreadsheet or document downloads; where a resource has its DataStore Active property set to true, take its Resource ID and pass it to **Search DataStore records** to read the actual rows of data.

To work through a single publishing body instead, call **List organisations**, pass one of the returned short names to **Search datasets** as the Filter Query option in the form `organization:idce`, and then follow the same path into the resources.

## Known Issues and Limitations

- **The API is served under a `/dashboard` path.** The portal's data API base is `https://opendata.gia.gov.ly/dashboard/api/3/action`, not `.../api/3/action` at the site root. The site root serves only the public HTML portal. This connector already targets the correct path; it is called out here because the root path silently returns an HTML page rather than an API error.

- **The portal content is Arabic-first.** Dataset titles, descriptions, tag names, organisation titles and DataStore column names are published in Arabic. Dataset and organisation short names (`name`) are the most reliable values to filter and match on. Plan for right-to-left text when you display these values.

- **Several file formats are in use.** Resources are published mainly as XLSX, with smaller numbers of CSV, JSON and XML files. Filter on the Filter Query option in the form `res_format:XLSX` (or `res_format:CSV`) if you need to restrict a dataset search to one format.

- **Only a few resources are in the DataStore.** Most resources are file downloads that have not been loaded into the tabular DataStore, so their DataStore Active property is false and the Search DataStore records operations will not return rows for them. Check the DataStore Active property on a resource before calling those operations.

- **DataStore SQL is not available.** The portal has the DataStore enabled but the SQL query endpoint (`datastore_search_sql`) is switched off, so this connector does not offer a SQL operation. Use Search DataStore records with the Column Filters, Sort Order and Columns options instead.

- **DataStore rows have no fixed shape.** Every resource in the DataStore has its own set of columns, so the Rows output of the DataStore operations cannot be given fixed properties. Read the Columns output of the same response to discover the column names and data types of the resource you are querying, then address the row values by those names.

- **DataStore rows are always returned as objects.** The record format is fixed so that the response shape stays predictable. The comma separated value and tab separated value output modes offered by the portal platform are not exposed.

- **Dataset suggestions need at least one character.** Search dataset suggestions matches on the beginning of a dataset name or title and returns nothing at all for an empty search text.

- **No published rate limit.** The portal does not document a rate limit and does not return any rate limit headers. No throttling was observed during testing, but the limits are not guaranteed, so leave a short delay between calls when you loop over many datasets or resources.

- **No authentication and no write operations.** The connector is read only. It cannot create, update or delete datasets, resources or rows, and it cannot read private datasets.

- **Some metadata fields can be inconsistent in type.** A small number of fields, such as a DataStore query's estimated row count (`total_estimation_threshold`), have been observed returned as different data types depending on the query. These fields are declared without a fixed type in this connector so that either shape passes through without error.
