
## DataScope Forms Connector
DataScope Forms connector allows you to receive data form the DataScope Mobile App and web platform.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* [DataScope Forms Account](https://mydatascope.com) to [generate an API Key](https://mydatascope.com/webhooks#api)

## Supported Operations
**Forms: New Answer** - Triggers when a new answer is submitted. Only one active Trigger per form.
**Findings: New Finding** - Triggers when a finding is created. Only one active Trigger per account.
**Findings: Changed Status** - Triggers when a finding changes its status. Only one active Connection per account.
**Tasks: New Assigned Task** - Triggers when a new task is assigned. Only one active Connection per form.
**Forms: New PDF** - Triggers when a new PDF is generated (by email backup or autonotify). Only one active Connection per form.
**Forms: Status Changed** - Triggers when a form answer changes its status/state. Only one active Connection per form.
**Signatures: New Completed Signature** - Triggers when a PDF is fully signed. Only one active Connection per form.
**Signatures: New Rejected Signature** - Triggers when a new PDF is rejected. Only one active Connection per form.
**Signatures: Updated Signature** - Triggers when a PDF’s signatures are updated. Only one active Connection per form.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.


