# Experlogix CPQ Connector
Experlogix CPQ (Configure, Price, Quote) helps sales teams quickly and accurately build quotes for complex products and services. Using a logic-based rules engine, Experlogix CPQ software guides users through selecting the right configuration options, applies pricing automatically and prevents incompatible variations from being selected — all through deep integration with leading CRM and ERP systems like Microsoft Dynamics 365 and Salesforce.

## Publisher: Experlogix

## Prerequisites
You need to have access to an Experlogix CPQ project.

## Supported Operations

### ConfigurationXml
This operation will read and return the configuration XML associated to the provided record in the host system.

### CreateConfigurationFromCopy
This operation will create a new configuration by copying the current configuration at the specified source record to the target record.

### UpdateConfiguration
This operation will update the configuration to the provided record in the host system.

### CreateConfiguration
This operation will create a new configuration to the provided record in the host system.

## Known Issues and Limitations
The current version only support connecting to Microsoft Dynamics 265 CE and Microsoft Finance and Operations. Support for other CPQ host systems will be added in the future.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.

## Support
For further support, please contact support@experlogix.com