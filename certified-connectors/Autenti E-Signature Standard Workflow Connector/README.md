# Autenti E-Signature Standard Workflow Connector for Power Automate

The Autenti E-Signature Standard Workflow Connector enables integration with the Autenti platform, offering functionalities related to managing processes of electronic signing of documents within Microsoft Power Automate workflows.

## Publisher: Autenti Sp. z o.o.

## Prerequisites

- An account on the Autenti platform.
- OAuth application configuration in Autenti to obtain `clientId` and `clientSecret`.
- To unlock all features, take advantage of the offer [Autenti Azure Marketplace](https://autenti.com/p-a).

## Supported Operations

### List Documents
This action provides an interface to list all documents within the system, allowing for incremental synchronization of document statuses and details between the Autenti platform and the client's system. Essential for maintaining up-to-date document workflows.

### Create Document Process Draft
Initiate the creation of a document process draft, which serves as the preliminary setup for document workflows before finalization and execution. This step is critical for configuring document parameters and participants.

### Add Participant to Document Process
Enables the addition of participants to an ongoing document process, facilitating the inclusion of new signatories or viewers as the document progresses through various stages. Essential for dynamic workflow adjustments.

### Get Document Process Files
Retrieve all files associated with a specific document process. This action is vital for accessing document attachments and related files necessary for review and processing.

### Add File to Document Process
Allows users to upload and attach files to a document process. This capability is crucial for including necessary documentation such as contracts or supplementary materials in the signing process.

### Perform Action on Document Process
Execute specific actions on a document process, such as starting the signing process, withdrawing documents, or sending reminders to participants. This functionality is key to managing and steering document flows effectively.

### Get Document Process Details
Obtain comprehensive details about a specific document process, including participant information, document status, and history. This detailed overview is crucial for monitoring and managing the lifecycle of document processes.

### Retrieve Document Process File Content
Download the content of files associated with a document process. This function is essential for participants who need to review documents in detail before proceeding with signing or approval.

### Document Change (Webhook)
Set up notifications for any changes in document status or modifications within the document process. This webhook ensures that all stakeholders are immediately informed of updates, maintaining workflow continuity.

## Obtaining Credentials

1. **Registering an application in Autenti**: Go to [Autenti API](https://autenti.com/en/contact) to register your application and obtain OAuth identifiers (clientId and clientSecret).
2. **Configuring the connector in Power Automate**: During the connector setup in Power Automate, enter the obtained OAuth identifiers.\

## Deployment Instructions
Please use the [instructions on Microsoft Power Platform Connectors CLI](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate (Flow) and Power Apps.

## Getting Started

The Autenti E-Signature Connector can automate various document and signature processes, integrating seamlessly into your existing workflows. Below are practical examples of how you can leverage this connector:

### Automating Contract Signing Process

**Scenario**: You need a reliable method to send contracts for electronic signing whenever a new client is onboarded.

**Workflow**:
1. **Trigger**: The process begins when a new client form is submitted, capturing client details.
2. **Action**: Use the "Create Document Process Draft" action to create a new document process in Autenti, uploading the contract template personalized with the client's details.
3. **Action**: Add the client as a participant in the document process using the "Add Participant to Document Process" action.
4. **Action**: Start the signing process by invoking the "Perform Action on Document Process" action.
5. **Trigger**: Use the "Document Change" trigger to monitor the status of the document process.
6. **Action**: Once the document is fully signed, automatically send a notification email to the relevant internal team for further processing.

### HR Onboarding Documents

**Scenario**: Automate the distribution and signing of HR documents for new employees.

**Workflow**:
1. **Trigger**: The workflow starts when a new employee record is added to the HR system.
2. **Action**: For each required document (e.g., employment contract, confidentiality agreement), create a document process draft and upload the documents.
3. **Action**: Add the new employee as a participant to each document process.
4. **Action**: Trigger the signing process for each document sequentially or simultaneously, depending on the need.
5. **Trigger**: Use the "Document Change" to monitor when each document is signed.
6. **Action**: After all documents are signed, compile signed documents and store them in the company's document management system.

### Automated Legal Review Requests

**Scenario**: You want to streamline the process for requesting and obtaining legal reviews of contracts before sending them out for signature.

**Workflow**:
1. **Trigger**: The process is triggered when a contract is marked as "Ready for Review" in a project management tool.
2. **Action**: Create a document process draft for the contract and use "Add File to Document Process" to upload the contract.
3. **Action**: Add the legal team as participants with the role of "Reviewer."
4. **Action**: Notify the legal team to review the contract through the "Perform Action on Document Process" action.
5. **Trigger**: Monitor the document status using "Document Change" to see when the review is complete.
6. **Action**: Once reviewed, automatically notify the project manager and update the project status in the management tool.

### Client Approval Workflows for Creative Agencies

**Scenario**: A creative agency requires a fast and efficient way to get client approvals on artwork or marketing materials.

**Workflow**:
1. **Trigger**: Initiate the process when the design team uploads final artwork files to a shared drive.
2. **Action**: Create a new document process for each artwork file, adding them to the document process.
3. **Action**: Add clients as participants with "Viewer" or "Approver" roles.
4. **Action**: Begin the approval process, allowing clients to view and approve directly within Autenti.
5. **Trigger**: Monitor approval status through "Document Change" triggers.
6. **Action**: Once approval is received, automate the next steps in the project workflow, such as sending the artwork to print or publishing it online.

These examples showcase the versatility of the Autenti E-Signature Connector in Power Automate, enabling organizations to automate complex document workflows, reduce manual tasks, and accelerate decision-making processes.

## Known Issues and Limitations

Currently, there are no known issues. If any issues arise, please contact [Autenti support](mailto:support@autenti.com).

## About the Author

This connector was developed by [AUTENTI Sp. z o.o.](https://autenti.com), a provider of advanced solutions for electronic document signing, remote identification and registered electronic delivery.
