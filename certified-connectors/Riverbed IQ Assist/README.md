# Riverbed IQ Assist
Riverbed IQ, Riverbed’s SaaS-based Unified Observability service, uses automated investigative workflows, 
called runbooks, to enable faster, easier root cause analysis. The no-code runbooks play a significant role in 
automating the troubleshooting processes. In fact, it mimics an organization’s troubleshooting workflows to 
automate the collection of incident details.

## Publisher: Riverbed Technology

## Prerequisites
You need to have access to the Riverbed IQ product.

## Supported Operations

### FindUserEndpoint
This operation searches for a user endpoint based on username, email or display name.

### CreateTicket
This operation creates a help desk ticket for a device.

### GetRemediationRunStatus
This operation fetches the remediation run status given a remediation run ID.

### StartRemediationOnUserEndpoint
This operation starts a remediation on a user device.

### DiagnoseUserEndpoint
This operation runs endpoint analysis to diagnose a device issue.

### SuggestUserEndpointRemediation
This operation suggests a remediation for a device based on diagnosis or symptoms.

### GetOperationId
This operation retrieves the status of a long-running operation initiated by one of the other operations.

## Known Issues and Limitations
The current version is limited to executing pre-mapped, built-in runbooks. Customization support for user-generated runbooks will be added in a future release.

## Deployment Instructions
Please use [the paconn cli tool](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.

## Support
For further support, please contact support@riverbed.com.
