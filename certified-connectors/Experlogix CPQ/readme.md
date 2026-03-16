# Experlogix CPQ Connector
Experlogix CPQ (Configure, Price, Quote) helps sales teams quickly and accurately build quotes for complex products and services. Using a logic-based rules engine, Experlogix CPQ software guides users through selecting the right configuration options, applies pricing automatically and prevents incompatible variations from being selected — all through deep integration with leading CRM and ERP systems like Microsoft Dynamics 365 and Salesforce.

## Publisher: Experlogix

## Prerequisites
You need to have access to an Experlogix CPQ project.

## Connector capabilities
The Experlogix CPQ Power Platform connector supports configuration lifecycle operations, model metadata discovery, and MCP server communication for agentic scenarios.

## Supported operations

### Get Configuration Xml (`GetConfigurationXml`)
Reads and returns the configuration XML associated with a provided record in the host system.
- Required inputs: `type`, `id`
- Typical output: `configurationXml`, `configureUrl`, `lineItemIds`, `success`, `messages`

### Create Configuration From Copy (`CreateConfigurationFromCopy`)
Creates a new configuration by copying an existing source configuration to a target record.
- Required inputs: `targetId`, `sourceId`, `type`
- Optional input: `lineItemIds`

### Update the Configuration (`UpdateConfiguration`)
Updates a record's configuration using a complete XML payload.
- Required inputs: `id`, `type`, `configurationXml`

### Create the Configuration (`CreateConfiguration`)
Creates a new configuration for a record using a complete XML payload.
- Required inputs: `id`, `type`, `configurationXml`

### Create Configuration From Changes (`CreateConfigurationFromChanges`) **New**
Creates a new configuration by applying a list of configuration changes.
- Required inputs: `id`, `type`, `seriesId`, `modelId`
- Optional input: `changes[]`

### Update Configuration From Changes (`UpdateConfigurationFromChanges`) **New**
Updates an existing configuration by applying a list of configuration changes.
- Required inputs: `id`, `type`
- Optional input: `changes[]`

### Get Model Metadata (`GetModelMetadata`)
Returns model metadata to help build guided configuration experiences.
- Optional input: `relevantCategories[]`
- Typical output: `Categories`, `CategoryOptions`, `CategoryProperties`, `success`, `messages`

## Change-based configuration payloads
The `changes[]` collection supports granular configuration updates via `ChangeConfig` entries, including values such as:
- `type` (for example: `setProperty`, `selection`, `recalc`)
- `configLineIndex`
- `changeType` (for selection changes)
- `categoryId`, `optionId`, `selectionIndex`
- `propertyId`, `value`

Use change-based operations when you want to apply targeted updates without sending full configuration XML.

## Known Issues and Limitations
The current version only support connecting to Microsoft Dynamics 365 CE and Microsoft Finance and Operations. Support for other CPQ host systems will be added in the future.

## Support
For further support, please contact support@experlogix.com
