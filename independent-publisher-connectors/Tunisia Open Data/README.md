# Tunisia Open Data (Independent Publisher)

Tunisia Open Data is the national open government data portal of the Republic of Tunisia. The public site at data.gov.tn is a content-managed shell; the dataset catalogue behind it runs CKAN at catalog.data.gov.tn, and that is what this connector targets. The catalogue holds datasets from Tunisian ministries, agencies and municipalities covering the economy, agriculture, local affairs, health, education and other areas, published mainly in French with parallel Arabic fields. This connector searches the dataset catalogue, reads dataset, publishing organisation and resource metadata, and lists the licences and topic groups in use.

## Publisher: Dan Romano

**Stack Owner:** Republic of Tunisia

## Prerequisites

You need a Microsoft Power Apps or Power Automate plan with custom connector capability. You do not need an account on catalog.data.gov.tn and you do not need an API key, because the portal serves its data API anonymously.

## Obtaining Credentials

No credentials are required. The Tunisia Open Data catalogue exposes its CKAN API without authentication, so the connector uses no authentication and there is nothing to configure when you create a connection.

## Supported Operations

Each operation carries a **Try it** line with an input verified live against catalog.data.gov.tn on 28 August 2026. To run one, open the connector in Power Automate or Power Apps, go to its **Test** tab, select the operation, enter the values shown and select **Test operation**. Any counts given are the live figures at the time of writing.

### Search datasets

Searches the catalogue and returns the matching datasets with their full metadata, resources and tags. Use the Search Query option for free text and the Filter Query option to restrict results to a publishing organisation, a file format or another indexed field. Dataset titles and descriptions are published mainly in French.

**Try it:** Search Query = `tunis`. Returns the datasets whose metadata matches "tunis". Leave Search Query empty to browse the whole catalogue (about 2,900 datasets), 20 per page.

### Search datasets (advanced)

Performs the same search but sends the options in a request body rather than in the query string, which is easier when a query or filter value is long or contains a lot of punctuation.

**Try it:** Search Options body with Search Query (`q`) = `tunis`. Same result as the query-string search above, sent as a request body.

### Get dataset

Returns the full metadata of a single dataset, including every resource attached to it, its tags, its publishing organisation and its licence. Supply either the dataset name used in the portal address or the dataset identifier.

**Try it:** Dataset ID or Name = `arrete-de-fixation-du-tarif-des-taxes-que-les-collectivites-locales-commune-beni-khiar`. Returns the local-tax tariff order dataset for the commune of Beni Khiar.

### List dataset names

Returns the short name of every dataset published on the portal as a plain list of text values. This list also fills the Dataset ID or Name picker on the Get dataset operation.

**Try it:** No parameters. Returns roughly 2,900 short names such as `recensement-2014` and `budget-2020`.

### Search dataset suggestions

Returns datasets whose name or title begins with the text you supply, as a short suggestion list. Use it to look up a dataset name before calling Get dataset. An empty search text returns no suggestions, so always supply at least one character.

**Try it:** Search Text = `2017-gouvernorats-fosda-fonds`. Returns the dataset whose name begins with that text.

### List organisations

Returns the short name of every government ministry, agency and municipality that publishes data on the portal.

**Try it:** No parameters. Returns roughly 220 organisation short names, such as `oep`, `anme` and `anpe`.

### Get organisation

Returns the details of a single publishing organisation, including its display title, its description and the number of datasets it has published. The Organisation ID or Name option is a picker sourced from Search organisation suggestions.

**Try it:** Organisation ID or Name = `crda-kairouan`. Returns the Regional Agricultural Development Commission of Kairouan record.

### Search organisation suggestions

Returns publishing organisations whose name or title matches the text you supply, with the short name and the full title of each one. An empty search text returns organisations without filtering, so this operation can also be used to list them with their titles.

**Try it:** Search Text = `crda-kairouan`. Returns the organisation whose name matches "crda-kairouan".

### Get resource

Returns the metadata of a single resource, which is one downloadable file belonging to a dataset. This includes the download address, the file format and the media type the portal has recorded.

**Try it:** Resource ID = `49f0900d-ce2a-4e84-ae79-ed0ced1317ed`. Returns that resource's metadata, including its download URL and format. Take any resource ID from the Resources list of a Get dataset or Search datasets response.

### Search resources

Searches the resources of the portal by the value of a single resource field, written as field:value, for example `format:CSV` or `name:budget`. Only stored resource fields such as format, name, description and url can be searched, and the query must be written as `field:value` — a bare word is rejected.

**Try it:** Field Query = `format:csv`. Returns the CSV resources (about 1,695). A bare word with no colon returns HTTP 409.

### List resource views

Returns the views configured for one resource. A view is a preview the portal renders for a resource, such as a data table explorer, a chart or a PDF viewer.

**Try it:** Resource ID = `49f0900d-ce2a-4e84-ae79-ed0ced1317ed`. Returns the views configured for that resource, such as the built-in data table explorer.

### List groups

Returns the short name of every topic group defined on the portal, such as `affaires-locales`, `commerce` or `agriculture-ressources-hydrauliques-et-peche-maritime`.

**Try it:** No parameters. Returns the 23 topic group short names.

### List tags

Returns every keyword used across the datasets of the portal as a plain list of text values, most of them in French.

**Try it:** No parameters. Returns roughly 4,400 tags.

### List licences

Returns every licence the portal offers for its datasets, with the identifier that appears in the Licence ID property of a dataset and the address of the full licence text. Licence titles carry a parallel Arabic title.

**Try it:** No parameters. Returns 6 licences, including the Tunisian National Open Public Data Licence (`licence-nationale-de-données-publiques-ouvertes`), which most datasets use.

### Search file format suggestions

Returns the file formats used by resources on the portal that match the text you supply, such as csv, xlsx, pdf or json. Use a returned format with the Filter Query option of the Search datasets operation, for example `res_format:CSV`.

**Try it:** Search Text = `csv`. Returns `csv`. `p` returns `pdf`, `png` and similar; `j` returns `json`.

## Getting Started

Start with **Search datasets** and leave the Search Query option empty to browse the catalogue, or supply a French term to narrow it. Each dataset returned carries a Resources list of downloadable files (CSV, XLSX, PDF and others). Take a Resource ID from that list and pass it to **Get resource** for the download URL, or to **List resource views** to see how the portal previews it.

To work through a single publishing body instead, call **List organisations**, pass one of the returned short names to **Search datasets** as the Filter Query option in the form `organization:oep`, and follow the same path into the resources.

## Known Issues and Limitations

- **The API is on a different host from the public site.** `data.gov.tn` is a content-managed site whose `/api/3/action/` path returns HTML. The CKAN API is the separate `catalog.data.gov.tn` host. This connector already targets the correct host; it is called out here because the obvious URL does not work.

- **Content is French-first with parallel Arabic fields.** Datasets, resources and organisations carry both a French field (`title`, `notes`, `name`, `description`) and an Arabic sibling (`title_ar`, `notes_ar`, `name_ar`, `description_ar`). Publishers do not always fill the Arabic fields, so an Arabic value can be blank or can simply repeat the French text. The French field and the short `name` are the most reliable values to match on.

- **The portal's firewall throttles unusual clients.** Requests from tools whose User-Agent header does not look like a browser (for example a bare scripting client) are slowed to a stall after a short burst. Requests from a browser, and from Power Platform's own connector runtime, are not affected. If you script against the same backend outside Power Platform, send a browser-like User-Agent and space your calls.

- **DataStore query operations are excluded.** The portal has the CKAN DataStore extension enabled, but no resource on the catalogue has been loaded into it (`datastore_active` is false on every resource checked, and a `datastore_search` call against a real resource returns "resource not found"). The three DataStore operations other CKAN connectors offer — Search DataStore records, the request-body variant, and Search DataStore with SQL — are therefore left out of this connector rather than shipped as dead endpoints. Resources are file downloads; use the Download URL from Get resource to fetch the data.

- **The dataset field schema operation is excluded.** This portal loads the CKAN `scheming` extension but does not expose its `scheming_dataset_schema_show` endpoint (it returns "not found"), so the Get dataset field schema operation that the Morocco connector in this family offers is not included here. The custom dataset fields it would describe (National Theme, the Geographic and Coverage properties) are still returned inline on each dataset.

- **Most resources report no size.** The Size In Bytes property is empty on the large majority of resources. It is declared as nullable for that reason.

- **Custom metadata fields are often blank.** The National Theme, Geographic Level, Geographic Name, Coverage Start Date, Coverage End Date and Update Frequency properties exist on every dataset but are frequently empty, because publishers rarely fill them.

- **No dedicated privacy or terms page.** Only the portal's About page resolves. The Website metadata field points at `data.gov.tn/fr` and the Privacy policy field at the catalogue's About page.

- **No published rate limit.** The portal does not document a rate limit and does not return rate limit headers. Leave a short delay between calls when you loop over many datasets or resources.

- **No authentication and no write operations.** The connector is read only. It cannot create, update or delete datasets, resources or rows, and it cannot read private datasets.
