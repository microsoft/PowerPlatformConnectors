

## Assently E-Sign Connector
The Assently E-Sign Connector makes functionality of Assently E-Sign available for use in Microsoft Power Automate (Flow).


## Pre-requisites
An Assently E-Sign Account with Microsoft Flow add-on. [Create a free Assently E-Sign Trial Account](https://app.assently.com/user/signup).


## How to get credentials
When the Microsoft Flow add-on is enabled by the Administrator on your Assently E-Sign Account, a Personal Access Token can be generated under your user profile.


## API Documentation
See Assently E-Sign's [API Documentation](https://app.assently.com/api).

## Deployment Instructions
Please use the [instructions on Microsoft Power Platform Connectors CLI](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate (Flow) and Power Apps.

## Supported Operations

- **When a new case is Created from template:** This trigger waits for a new case to be Created from a template.
- **When a case is Sent:** This trigger waits for a case to be Sent.
- **When a case is Recalled:** This trigger waits for a case to be recalled.
- **When a case is Finished and the receipt is available:** This trigger waits for a case to Finish. The receipt is available at this stage.
- **When a case is Rejected:** This trigger waits for a case to be Rejected.
- **When a case is Expired:** This trigger waits for a case to Expire.
- **When a case is Deleted:** This trigger waits for a case to be Deleted.
- **When a Signature is added to a case:** This trigger waits for a Signature to be added to a case.
- **When an Appoval is requested on a case:** This trigger waits for an Appoval to be requested on a case.
- **When a case is Reminded:** This trigger waits for a Reminder to be sent for a case.
- **When a case's metadata is Updated:** This trigger waits for metadata of a case to be updated.
- **When a case is Signed by all parties:** This trigger when all parties of a case has signed. At this stage Receipt document is not generated yet.
- **When an Approval is added to a case:** This trigger waits for an Approval to be added to a case.
- **When a Rejection is added to a case:** This trigger waits for a Rejection to be added to a case.
- **When a case is Approved by all approvers:** This trigger triggers when all approvers have approved a case.

- **Get a Case:** By this action you can get a Case by its CaseId.
- **Find and list Cases:** By this action you can find and List cases based upon the input parameters. More info at https://app.assently.com/api#Findandlistcases
- **Find and list templates:** By this action you can find and List templates based upon the input parameters. More info at https://app.assently.com/api#Findandlisttemplates
- **Create a case:** By this action you can create a new Case in your account
- **Create Case from Template:** By this action you can create a new Case from Template
- **Update a Case:** By this action you can update properties and collections of a case. It is recommended to use GetCase action before making an update. Collections: missing items will be removed, others updated or added. Documents collection: Only filename and formfields can be changed. To modify size, hash or data, the document must be removed first and a new document (with a new id) must be added.
- **Update Case Metadata:** By this action you can update case metadata regardless of the case status. Existing metadata will be replaced with new metadata. Metadata cannot be complex objects.
- **Send a Case:** By this action you can change the status of the case to Sent, making it available for signing. In order to send a case the parameters Parties, Documents and AllowedSignatureTypes must be specified on the case. If notifications are enabled, parties will be notified.
- **Request Approval:** By this action you can set a case to require approval before it is sent. Sends a request to approver stakeholders to approve and send the case. Approvals are requested in the name of the API user.
- **Send a reminder:** By this action you can send reminders to all parties that have not yet signed. If signing order is enforced, only the next party in turn will be reminded.
- **Delete a Case:** By this action you can delete a Case permanently. If the case is sent, it will be recalled prior to deletion. 
- **Recall a case:** By this action you can recall a case if it is already sent. Finished cases cannot be recalled.
- **Get a case by temporary id:** By this action you can get a case by its temporaryId. A temporary id is a 4+ digit number that is only valid for 24 hours.
- **Get file content of a Case Document:** By this action you can get file of a case by caseId and documentId.
