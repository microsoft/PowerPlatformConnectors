## iAuditor connector
iAuditor API allows you to retrieve data from your organization's iAuditor platform. You can automate your workflows from exporting inspection reports to creating corrective actions for your team to complete. Please note that the list of supported operations below is only a subset of the functionalities on offer for the iAuditor API. You can see the full iAuditor API documentation on our [developer guide](https://developer.safetyculture.io/).

## Prerequisites
You will need the following to proceed:
* [iAuditor Premium subscription](https://safetyculture.com/pricing/) to [generate API tokens](https://support.safetyculture.com/integrations/how-to-get-an-api-token/)
* API client

## Supported Operations
The connector supports the following operations:

### Inspections
* `Search modified inspections`: Retrieve the list of inspections filtered by dates, template, completion status and more.
* `Get a specific inspection`: Retrieve the full data of an inspection.
* `Export an inspection report`: Submit an export request for an inspection report into the selected format.
* `Check inspection export status`: Check the inspection export status.
* `Export an audit report (legacy)`: Submit an export request for an audit into the format passed as a query parameter. Please note this export request is for the classic report format.
* `Check the status of the export request submitted earlier (legacy)`: Check whether the download report submitted earlier is ready for download.
* `Download an audit report (legacy)`: Download the audit report submitted earlier.
* `Retrieve an inspection web report link`: If a link has previously been generated for this inspection, it is returned. Otherwise a new link is generated.
* `Disable an inspection web report link`: Disable the web report link to an inspection.
* `Set the archived state of an inspection`: Choose 'Yes' to archive an inspection and 'No' to restore an archived inspection.
* `Get an inspection media`: Get a media file from an inspection.

### Actions
* `Search actions`: Find actions related based upon their status, priority, assignees, item, inspection and more.
* `Create an action`: Create a new Action.
* `Update an action`: Update an existing Action.
* `Delete an action`: Delete an existing Action.