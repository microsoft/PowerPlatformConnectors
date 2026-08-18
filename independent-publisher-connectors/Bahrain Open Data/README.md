# Bahrain Open Data (Independent Publisher)

Bahrain Open Data is the national open data portal of the Kingdom of Bahrain, operated by the Information & eGovernment Authority. It publishes hundreds of government datasets covering areas such as population, health, education, justice, municipal services, tourism, and the environment, in both English and Arabic. This connector lets Power Automate flows, Power Apps, and Microsoft Copilot Studio agents search the dataset catalogue, read and filter the records inside a dataset, enumerate facet values, list dataset attachments, and export catalogue or dataset content as files, using the Opendatasoft Explore API v2.1 that powers the portal.

## Publisher: Dan Romano

**Stack Owner:** Information & eGovernment Authority, Kingdom of Bahrain

## Prerequisites

There are no licensing or account prerequisites for this connector. The Bahrain Open Data portal is a public, open data service, and every operation in this connector reads data that is published for public use.

You will need:

- A Power Automate, Power Apps, or Microsoft Copilot Studio environment in which to create the connection.
- The identifier of the dataset you want to query, for any operation that works on a single dataset. The **List datasets** operation returns these identifiers, and the **Dataset ID** field offers the first 100 datasets in a picker.

## Obtaining Credentials

No credentials are required. The Bahrain Open Data portal serves the Explore API v2.1 anonymously, so this connector uses no authentication and the connection is created without entering a key, a user name, or a password.

When you create a connection in Power Automate, Power Apps, or Microsoft Copilot Studio, select **Create** and the connection is ready to use immediately.

The underlying Opendatasoft platform can also accept an optional API key for access to restricted datasets on portals that publish them. The Bahrain Open Data portal exposes its catalogue publicly, so no key is needed, and this connector does not send one.

## Supported Operations

| Operation | Description |
|-----------|-------------|
| **ListDatasets** | Lists the datasets published on the portal, with their metadata and field schema, filtered and sorted with the ODSQL query options. |
| **GetDataset** | Retrieves the metadata and field schema of a single dataset, including its title, publisher, theme, licence, update frequency, and record count. |
| **ListRecords** | Lists the records held in a dataset, with full ODSQL support for filtering, field selection, aggregation, and sorting. |
| **GetRecord** | Retrieves a single record from a dataset by its record identifier. |
| **ListCatalogFacets** | Lists the facet values available across the whole catalogue, such as themes, publishers, and keywords, with a count of datasets for each value. |
| **ListDatasetFacets** | Lists the facet values available within one dataset, with a count of records for each value. |
| **ListDatasetAttachments** | Lists the files attached to a dataset, such as documentation or source spreadsheets, with the download URL and media type of each. |
| **ListCatalogExportFormats** | Lists the file formats that the whole catalogue can be exported to, with the download link for each format. |
| **ListDatasetExportFormats** | Lists the file formats that the records of a dataset can be exported to, with the download link for each format. |
| **ExportCatalog** | Exports the dataset catalogue as a file in a chosen format, such as CSV, XLSX, RDF, or DCAT. |
| **ExportRecords** | Exports the records of a dataset as a file in a chosen format, such as CSV, XLSX, JSON, GeoJSON, or Parquet, without the record limit that applies to **ListRecords**. |

## Getting Started

A typical flow uses two operations in sequence:

1. Call **ListDatasets** with a **Filter** such as `theme like 'Health'` to find the datasets you are interested in, and read the **Dataset ID** from the results.
2. Call **ListRecords** with that dataset identifier, and use the **Filter**, **Select Fields**, **Group By**, and **Order By** options to return exactly the rows you need.

The **Filter**, **Select Fields**, **Group By**, and **Order By** options all accept expressions written in the Opendatasoft Query Language (ODSQL). For example, `year > 2020 AND indicator like 'waste'` is a valid **Filter**, and `sum(value) as total` combined with a **Group By** of `year` aggregates records by year. The ODSQL reference is published in the [Opendatasoft Explore API documentation](https://help.opendatasoft.com/apis/ods-explore-v2/).

To move a whole dataset into a file, a SharePoint library, or Excel, use **ExportRecords** rather than paging through **ListRecords**. The export operations return the raw file content, which you can pass straight into a **Create file** action.

## Known Issues and Limitations

- **Rate limits.** The portal allows 5,000 API calls per day per client, and the quota resets at midnight UTC. The remaining allowance is returned in the `X-RateLimit-Remaining` response header. When the quota is exhausted, the portal returns HTTP 429. Flows that run on a short recurrence should be designed to stay well inside this allowance.
- **Authentication.** This connector uses no authentication because the portal is public. OAuth is not supported for Independent Publisher connectors at this time.
- **Read-only.** The Explore API supports the HTTP GET method only, so this connector can read and export data but cannot create, update, or delete anything on the portal.
- **Record fields vary by dataset.** Each dataset defines its own schema, so the fields inside a record are not fixed and cannot be described in the connector definition. The record fields are returned as raw JSON. Use **GetDataset** to read the field names and types of a dataset, then add a **Parse JSON** action in your flow to turn the records into typed dynamic content.
- **Result size limits on list operations.** **ListRecords** and **ListDatasets** return a maximum of 100 items per call, or 20,000 items when a **Group By** expression is used, and the sum of **Limit** and **Offset** must remain below 10,000. Use **ExportRecords** or **ExportCatalog** when you need the whole dataset or catalogue.
- **Record identifiers.** The **GetRecord** operation needs a record identifier, which the portal does not return by default. Set the **Select Fields** option of **ListRecords** to `recordid` to retrieve the identifier of each record, then pass that value to **GetRecord**.
- **Geographic export formats need geographic data.** The `shp`, `kml`, `fgb`, `gpx`, and `ov2` values of **Export Format** work only for datasets that contain a geographic field. Selecting one of them for a dataset without geographic data returns HTTP 400. Use **ListDatasetExportFormats** to see the formats a particular dataset supports. Most datasets on this portal are statistical tables without geographic fields.
- **Dataset picker coverage.** The **Dataset ID** field lists the first 100 datasets returned by the catalogue. The portal publishes more datasets than that, so for a dataset outside the first 100, enter its identifier manually. The identifier appears in the dataset URL, immediately after `/datasets/`.
- **CSV options apply to CSV exports only.** The **CSV Field Delimiter**, **CSV List Separator**, **Quote All Strings**, and **Include Byte Order Mark** options on the export operations take effect only when the **Export Format** is `csv`, and are ignored for every other format. Note that the default field delimiter is a semicolon, so set the delimiter to a comma if your flow expects comma-separated values.
- **Format-specific export options are not exposed.** The API accepts a small number of extra options that apply only to the Parquet, GPX, and DCAT Application Profile exports, such as the Parquet compression codec and the GPX name field. These are not exposed by this connector because they are not useful in Power Platform scenarios. The formats themselves can still be selected in the **Export Format** field.
- **Language of metadata.** Dataset titles, descriptions, and field labels are published in English and Arabic, and some datasets carry Arabic labels even when their identifiers are in English. Use the **Language** option to control the language used when formatting values.
- **Export file size.** An export with the default **Export Limit** of -1 returns every record in the dataset. For large datasets this can produce a file big enough to hit the message size limits of Power Automate, so set an explicit **Export Limit**, or filter the export with the **Filter** option, when working with the largest datasets on the portal.
