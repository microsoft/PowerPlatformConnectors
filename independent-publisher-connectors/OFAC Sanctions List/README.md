# OFAC Sanctions List

The U.S. Department of the Treasury's Office of Foreign Assets Control (OFAC) administers and enforces economic and trade sanctions. It publishes the Specially Designated Nationals and Blocked Persons (SDN) List and the Consolidated Sanctions List, and it makes both available for download through the Sanctions List Service at `sanctionslistservice.ofac.treas.gov`.

This connector lists the sanctions lists and sanctions programs that OFAC publishes, downloads the published data files in their CSV, XML and compressed forms, and retrieves the most recent list of changes so a flow can react when a designation is added, changed or removed. The connector returns the published files as they are. It does not perform name matching, scoring or screening.

## Publisher: Dan Romano

## Prerequisites

You need a Microsoft Power Platform environment that can use premium connectors. No account, sign up or API key is required. The Sanctions List Service is a public service.

## Obtaining Credentials

None. This connector uses no authentication. Every operation calls a public endpoint of the OFAC Sanctions List Service anonymously.

## Supported Operations

| Operation | Description |
| --- | --- |
| **List sanctions lists** | Returns the names of the sanctions lists OFAC publishes, such as the SDN List and the Consolidated List. |
| **List sanctions programs** | Returns the sanctions program codes OFAC uses to tag list entries, such as `CUBA`, `IRAN` or `LIBYA2`. |
| **Download a sanctions list file** | Downloads one published data file and returns its contents. Choose from the SDN and Consolidated primary files, the alternate name, address and remarks files, the advanced XML files, the fixed-width file and the compressed XML archives. |
| **Get the latest list changes** | Returns the most recent delta file, an XML document listing the entries added, changed or removed in the latest publication. |
| **Get list changes for a date** | Returns a `publications` array of the delta publications OFAC issued on a given date, each with a publication ID and the time it was published. The array is empty when nothing was published that day. |

## Known Issues and Limitations

- **The connector returns files, not structured records.** CSV, XML and fixed-width content comes back as a single body that your flow must parse. The flat CSV files have a fixed, documented column layout; the advanced XML files follow OFAC's published schema.
- **Payloads can be large.** The SDN advanced XML and the Consolidated files run to several megabytes. A single download can approach the response size and the request timeout limits that apply to connectors, so prefer the smallest file that carries the data you need.
- **Downloads are served through a redirect.** *Download a sanctions list file* and *Get the latest list changes* are answered by the service with a redirect to time limited cloud storage on a separate host. The connector follows that redirect for you with custom code and returns the file contents in one call. You do not see or handle the storage URL.
- **The latest changes are returned as text, not parsed XML.** *Get the latest list changes* returns the delta document as a single string so it passes through exactly as OFAC published it, against OFAC's versioned schema. Read individual entries with the `xml()` and `xpath()` expressions. See *Working with the delta file* below.
- **No screening.** This connector retrieves official list data only. It does not match names, calculate similarity scores or tell you whether a given party is sanctioned. Use a dedicated screening service for that.
- **File names can change.** OFAC can rename or retire published files without notice. Use *List sanctions lists* and *List sanctions programs* to check current values, and expect the file list in *Download a sanctions list file* to need occasional updates.
- **Consolidated data is split.** There is no single `CONSOLIDATED.CSV`. The Consolidated List in CSV form is spread across `CONS_PRIM.CSV`, `CONS_ADD.CSV`, `CONS_ALT.CSV` and `CONS_COMMENTS.CSV`, which join on the primary record number.
- **Rate limits.** OFAC publishes no rate limits for the Sanctions List Service. Keep scheduled calls to a reasonable frequency; the lists are typically republished on business days.
- **OAuth is not used.** Independent publisher connectors do not support OAuth, and this service does not require it.

## Testing These Operations

All values below were verified against the live service on 2026-08-30.

| Operation | Test input | Expected result |
| --- | --- | --- |
| List sanctions lists | none | An array beginning `SDN List`, `Non-SDN Palestinian Legislative Council List`, `FSE List`, ... |
| List sanctions programs | none | An array of program codes such as `CUBA`, `IRAN`, `LIBYA2`, `RUSSIA-EO14024`. |
| Download a sanctions list file | `fileName` = `SDN.CSV` | The SDN primary CSV, one quoted record per line, first field the record number. |
| Download a sanctions list file | `fileName` = `CONS_PRIM.CSV` | The Consolidated primary names CSV. |
| Download a sanctions list file | `fileName` = `SDN_ADVANCED.XML` | The advanced-format SDN XML document. |
| Get the latest list changes | none | The latest delta document, returned as a string, dated on or near the last business day. See *Working with the delta file*. |
| Get list changes for a date | `year` = `2026`, `month` = `1`, `day` = `16` | `{ "publications": [ { "publicationID": 803, "datePublished": "2026-01-16T10:01:44.740051" } ] }`. A date with no publication returns `{ "publications": [] }`. |

To filter downloaded data to one country program, first call *List sanctions programs*, pick the relevant code (for example `LIBYA2`), download `SDN.CSV`, and keep the rows whose program column contains that code.

## Working with the delta file

*Get the latest list changes* returns the delta as a text string. In a flow, the action output is already the plain XML document. The backslash escapes you see under **Show raw outputs** in the run history (`\"`, `\r\n`) are how the value is
carried over the wire; they are not part of the string. You only reintroduce escaping if you pass the raw string into another step that serializes it again, such as dropping it straight into a *Compose* that feeds a JSON body. Convert it with `xml()` first and work with the result instead.

Two things to know before writing an expression:

- The delta document declares a **default XML namespace**
  (`xmlns="https://www.treasury.gov/ofac/DeltaFile/1.0"`). A plain path such as
  `//entity` matches nothing. Match on the local element name instead, with
  `*[local-name()='entity']`.
- Each `entity` element carries an `action` attribute of `add`, `change` or
  `remove`. Use it to route the change.

Assuming the action is named *Get the latest list changes*:

| Goal | Expression |
| --- | --- |
| Convert the string to XML | `xml(body('Get_the_latest_list_changes'))` |
| Publication date | `xpath(xml(body('Get_the_latest_list_changes')), "string(//*[local-name()='datePublished'])")` |
| Number of entities added | `xpath(xml(body('Get_the_latest_list_changes')), "count(//*[local-name()='entity'][@action='add'])")` |
| All entity IDs in the delta | `xpath(xml(body('Get_the_latest_list_changes')), "//*[local-name()='entity']/@id")` |
| Primary full names | `xpath(xml(body('Get_the_latest_list_changes')), "//*[local-name()='entity']//*[local-name()='formattedFullName']/text()")` |

`xpath()` returns an array of XML nodes. Wrap the call in an *Apply to each* to step through the matches, and call `xpath(item(), "string(.)")` inside the loop to read a node's text value.

### Coming in release 2: a parsed summary

A future release adds **Get the latest list changes summary**
(`GetLatestChangesSummary`). It parses the delta in the connector and returns a flat JSON object: the publication date and type, and one record per changed entity with its ID, action (`add`, `change` or `remove`), primary name, entity type, sanctions programs and sanctions lists. The full delta document is still included as a `raw` field. Use that operation when you want the common fields without writing `xpath()`, and keep using *Get the latest list changes* when you need the complete document. This operation is not available yet.
