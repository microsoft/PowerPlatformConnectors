# US Code of Federal Regulations

The [eCFR.gov](https://www.ecfr.gov/) is the Federal Government's digital version of the Code of Federal Regulations (CFR). This API provides access to structural and content metadata for CFR titles, sections, amendments, and historical versions. It enables civic technologists, legal researchers, and compliance teams to programmatically interact with up-to-date and historical regulatory text.

## Publisher: Dan Romano

## Prerequisites

There are no prerequisites to use this connector. However, to fully utilize the data, you may want to connect output to services such as Azure Blob Storage, Microsoft Fabric Lakehouse, or Power BI.

## Supported Operations

### GetCfrTitles
Returns a list of all CFR titles with metadata.

### GetContentVersionsByTitle
Returns historical content versions for a specific CFR title.

### GetCfrStructureNode
Returns the regulatory structure and content node for a CFR identifier on a given date.

### GetTitleAncestry
Returns the regulatory hierarchy for a given CFR title and date.

### GetAgencies
Returns a list of agencies that appear in the CFR.

### GetCfrCorrections
Returns historical corrections to the CFR.

### GetTitleCorrections
Returns corrections for a specific CFR title.

## Obtaining Credentials

N/A — no authentication is required to use this connector. However, requests may be throttled if accessed too frequently. Users are advised to cache responses or spread out requests when pulling large volumes of data.

## Known Issues and Limitations

- The `/full` endpoint for XML content (e.g., `title-10.xml`) may time out or fail to load in Power Automate due to file size (some exceed 40MB).
- Diffing is not exposed via API — only structural or date-based comparisons can be performed externally.
- No built-in pagination; filtering by date and title is encouraged to minimize payloads.

