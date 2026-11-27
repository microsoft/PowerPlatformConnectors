# Morocco Open Data (Independent Publisher)

Morocco Open Data is the national open government data portal of the Kingdom of Morocco, published at data.gov.ma. It holds 666 datasets from ministries, regional agencies, courts and other public bodies, covering areas such as health, justice, agriculture, insurance and urban planning. This connector searches the dataset catalogue, reads dataset, publishing organisation and resource metadata, reads the field schema a dataset was published against, and lists the licences and topic groups in use. Dataset and organisation metadata is published in French and Arabic.

## Publisher: Dan Romano

**Stack Owner:** Kingdom of Morocco

## Prerequisites

You need a Microsoft Power Apps or Power Automate plan with custom connector capability. You do not need an account on data.gov.ma and you do not need an API key, because the portal serves its data API anonymously.

## Obtaining Credentials

No credentials are required. The Morocco Open Data portal exposes its API without authentication, so the connector uses no authentication and there is nothing to configure when you create a connection.

## Supported Operations

### Search datasets

Searches the catalogue and returns the matching datasets with their full metadata, resources and tags. Use the Search Query option for free text and the Filter Query option to restrict results to a publishing organisation, a file format or another indexed field.

### Search datasets (advanced)

Performs the same search but sends the options in a request body rather than in the query string, which is easier when a query or filter value is long or contains a lot of punctuation.

### Get dataset

Returns the full metadata of a single dataset, including every resource attached to it, its tags, its publishing organisation and its licence.

### List dataset names

Returns the short name of every dataset published on the portal as a plain list of text values. This list also fills the Dataset ID or Name picker on the Get dataset operation.

### Search dataset suggestions

Returns datasets whose name or title begins with the text you supply, as a short suggestion list. Use it to look up a dataset name before calling Get dataset. An empty search text returns no suggestions, so always supply at least one character.

### Get dataset field schema

Returns the definition of every field a dataset can be published with on this portal, including which fields are required and which carry a French or Arabic translation. Useful for understanding the shape of a dataset's Additional Metadata before parsing it.

### List organisations

Returns the short name of every government ministry, authority and agency that publishes data on the portal.

### Get organisation

Returns the details of a single publishing organisation, including its display title, its description and the number of datasets it has published. Organisation titles on this portal are usually the organisation's acronym, with the full name in the Description property.

### Search organisation suggestions

Returns publishing organisations whose name or title matches the text you supply, with the short name and the full title of each one. An empty search text returns organisations without filtering, so this operation can also be used to list them with their titles.

### Get resource

Returns the metadata of a single resource, which is one downloadable file belonging to a dataset. This includes the download address, the file format and the internet media type of the resource.

### Search resources

Searches the resources of the portal by the value of a single resource field, written as field:value.

### List resource views

Returns the views configured for one resource. A view is a preview the portal renders for a resource, such as the built-in data table explorer.

### List groups

Returns the short name of every topic group defined on the portal, such as sante, education or agriculture.

### List tags

Returns every keyword used across the datasets of the portal as a plain list of text values. See the Known Issues and Limitations section below before relying on this operation.

### List licences

Returns every licence the portal offers for its datasets, with the identifier that appears in the Licence ID property of a dataset and the address of the full licence text.

### Search file format suggestions

Returns the file formats used by resources on the portal that match the text you supply, such as csv, xlsx or pdf.

## Testing These Operations

The table below lists a known-good input for every operation, verified live against data.gov.ma. To try one yourself, open the connector in Power Automate or Power Apps, go to its **Test** tab (or **Edit** the connection on an existing one), select the operation, enter the value shown in the **Test input** column into the named field, and select **Test operation**.

| Operation | Test input | How to test |
|---|---|---|
| **SearchDatasets** | No parameters required | Returns the first page of the catalogue, 20 datasets by default. |
| **SearchDatasetsAdvanced** | No parameters required | Same search as above, sent as an empty request body — returns the same first page of the catalogue. |
| **GetDataset** | Dataset ID or Name = `activites-sectorielles-agriculture-et-peche` | Returns the full metadata and resource list for that dataset. |
| **ListDatasetNames** | No parameters required | Returns the short name of every dataset on the portal. |
| **SearchDatasetSuggestions** | Search Text = `assurance-maladie-obligatoire` | Returns dataset suggestions whose name or title begins with that text. |
| **GetDatasetSchema** | Schema Type = `dataset` | Returns the full field schema datasets are published against. This option is fixed and sent automatically, since calling this operation without it is rejected by the portal's own firewall. |
| **ListOrganizations** | No parameters required | Returns the short name of every publishing organisation. |
| **GetOrganization** | Organisation ID or Name = `cnss` | Returns the details of the Caisse Nationale de Sécurité Sociale organisation record. |
| **SearchOrganizationSuggestions** | Search Text = `cnss` | Returns organisations whose name or title matches "cnss". |
| **GetResource** | Resource ID = `33262496-7479-48ab-9ee5-8d584b85efc4` | Returns the metadata of that resource, including its download URL and format. |
| **SearchResources** | Field Query = `format:CSV` | Returns resources whose format field equals CSV — the query must be written as `field:value`, a bare word is rejected. |
| **ListResourceViews** | Resource ID = `33262496-7479-48ab-9ee5-8d584b85efc4` | Returns the views (if any) configured for that resource. |
| **ListGroups** | No parameters required | Returns the portal's 23 topic groups. |
| **ListTags** | No parameters required | Returns the portal's global tag list. See Known Issues and Limitations below. |
| **ListLicenses** | No parameters required | Returns every licence the portal offers. |
| **SearchFormatSuggestions** | Search Text = `csv` | Returns file formats matching "csv". |

## Getting Started

Start with **Search datasets** and leave the Search Query option empty to browse the catalogue, or supply a French or Arabic term to narrow it down. Each dataset returned carries a Resources list. Take the Resource ID of any resource and pass it to **Get resource** for its full metadata, or to **List resource views** to see how the portal previews it.

To work through a single publishing body instead, call **List organisations**, pass one of the returned short names to **Search datasets** as the Filter Query option in the form `organization:cnss`, and then follow the same path into the resources.

To understand the shape of a dataset's metadata before parsing it in a flow, call **Get dataset field schema** first. It reports every field the portal allows a dataset to carry, including which ones are required and which carry a French or Arabic translation.

## Known Issues and Limitations

- **The portal is French-first, with Arabic as a genuine second language.** Dataset and organisation titles, descriptions and notes are published as separate French and Arabic fields, not just a single mixed-language title. Organisation titles are usually the organisation's acronym (for example CNSS), with the full name in the Description property, so an organisation record can look terse until you read that field.

- **DataStore querying is not available.** The portal enables CKAN's DataStore extension, but a check across every dataset on the portal found no resource with it actually turned on. This connector does not include DataStore query operations as a result. If the portal begins populating its DataStore, this may be added in a future update.

- **The global tag list is empty.** List tags returns no results, even though individual datasets carry real tags of their own, visible in a dataset's Tags property. This looks like an index that was never built on this deployment rather than a portal with no tags. There is no workaround through this API; tags are only visible per dataset.

- **Get dataset field schema always sends the same option.** The Schema Type option is fixed to `dataset` and sent automatically, because calling this operation without any value is rejected by the portal's own web application firewall rather than returning an error from the CKAN API itself.

- **No published rate limit.** The portal does not document a rate limit and does not return any rate limit headers. No throttling was observed during testing, but the limits are not guaranteed, so leave a short delay between calls when you loop over many datasets or resources.

- **No authentication and no write operations.** The connector is read only. It cannot create, update or delete datasets, resources or rows, and it cannot read private datasets.

- **Resource sizes are reported to 32-bit precision.** The Size In Bytes property of a resource is a standard 32-bit integer on this portal, which is large enough for every file size observed during testing but should be kept in mind if a future upload exceeds roughly 2 GB.
