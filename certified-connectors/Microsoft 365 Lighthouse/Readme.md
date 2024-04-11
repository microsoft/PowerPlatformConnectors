# Microsoft 365 Lighthouse

This connector enables Microsoft 365 Lighthouse users to integrate with the Microsoft 365 Lighthouse, allowing them to manage multiple customer tenants efficiently. With this connector, users can automate tasks, gather insights, and perform actions across all managed tenants across Microsoft 365 Lighthouse.

## Publisher: Microsoft​

## Prerequisites

- Access to Microsoft 365 Lighthouse.
- Multifactor authentication (MFA) enabled.
- Power Automate set up within your organization.

## Supported Operations

### List Aggregated Policy Compliances

Get a list of aggregated Policy Compliances and their properties.

### List Alerts

Get a list of alerts.

### List Alert Logs

Get a list of all Tenant Alert Logs.

### List Baseline Summaries

Get management Template Collection Tenant Summaries.

### List Tenants

Get a list of the tenants and their properties.

### List Tenant Details

Get a list of tenants and their detailed information.

### List Tenant Usage

Get a list of service usages for each tenant.

### List Baseline Task Summaries

Get a list of the tasks for each baseline.

### List Cloud PC Connections

Get a list of all the Cloud PC connections.

### List Cloud PC Devices

Get a list of all Cloud PC devices.

### List Conditional Access Policy Coverages

Get a list of all conditional access policy coverages.

### List Credential User Registrations Summaries

Get a summary of all user registrations.

### List Device Compliance Summaries

Get a list of all Device Compliance Policy Setting State Summaries.

### List Device Compliances

Get a list of all Managed Device Compliances.

### List My Roles

Get a list of my roles.

### List Tenants Customized Information

Get a list of all customized information regarding my Tenants.

### List Tenant Tags

Get a list of all the tenant tags in my account.

### List Windows Device Malware States

Get a list of all the Windows devices malware states.

### List Windows Protection States

Get a list of all Windows protection states.

### List Device App Performances

Get a list of all devices' app performances.

### List App Performances

Get a list of all app performances.

### List Device Health Statuses

Get a list of all device health statuses.

### List Management Template Steps

Get a list of all management template steps.

## Obtaining Credentials

Access to Microsoft 365 Lighthouse is required to register this connector. Please refer to the [Microsoft 365 Lighthouse](https://learn.microsoft.com/en-us/microsoft-365/lighthouse/m365-lighthouse-requirements?view=o365-worldwide) documentation regarding access requirements. This connector uses Microsoft Azure Active Directy Open Authentication and multifactor authentiation (MFA). During authorization, users must first select their Microsoft partner account that has a conditional access policy assigned to it that requires MFA for cloud apps. After authenticating via Azure Active Directory and completing MFA, a global admin must approve AAD permissions that will be used by the connector.

## Known Issues and Limitations

- Connector action is doesn't list all items: This occurs due to the nature of the Microsoft Graph API. You can increase the number of returned items by turning on pagination for that action and indicating the threshold of returned items. Use of $skipToken is not supported.
- This Connector is limited to the actions listed above. All other Microsoft 365 Lighthouse API endpoints are currently not supported. You can request additional features via the [Microsoft 365 Lighthouse Feedback Portal](https://feedbackportal.microsoft.com/feedback/forum/d6a21c42-fe1b-ec11-b6e7-0022481f8472)

## Deployment Instructions

Please use [these instructions](https://cloudpartners.transform.microsoft.com/download?assetname=assets/M365-Lighthouse-and-Power-Platform-Integration-Guide.pdf&download=1) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
