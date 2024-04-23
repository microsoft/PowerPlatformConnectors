# Autenti E-Signature Standard Workflow Connector for Power Automate

## Description

The Autenti E-Signature Standard Workflow Connector enables integration with the Autenti platform, offering functionalities related to managing processes of electronic signing of documents within Microsoft Power Automate workflows.

## Key Features

- **Creating document process drafts**: Allows the creation of document process drafts in Autenti.
- **Listing documents**: Enables retrieving a list of running document signing processes or processes not yet started (drafts).
- **Adding files to document processes**: Allows for the addition of files to an existing document process drsft.
- **Managing process participants**: Enables adding and managing participants in document processes.

## Requirements

- An account on the Autenti platform.
- OAuth application configuration in Autenti to obtain `clientId` and `clientSecret`.
- To unlock all features, take advantage of the offer [Autenti Azure Marketplace](https://azuremarketplace.microsoft.com/en-us/marketplace/apps?search=Autenti&page=1).

## Configuration

1. **Registering an application in Autenti**: Go to [Autenti API](https://autenti.com/en/contact) to register your application and obtain OAuth identifiers (clientId and clientSecret).
2. **Configuring the connector in Power Automate**: During the connector setup in Power Automate, enter the obtained OAuth identifiers.

## Usage Examples

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

## Support

For questions or problems, contact [Autenti support](https://autenti.com/en/contact).

## License

License information for this connector can be found in the [Autenti privacy policy](https://autenti.com/en/terms-and-conditions/privacy-policy).

## Contribution

We encourage contributions and suggestions for improvements to this connector. Please submit any proposals through the GitHub issue tracking system.

## About the Author

This connector was developed by [AUTENTI Sp. z o.o.](https://autenti.com), a provider of advanced solutions for electronic document signing, remote identification and registered electronic delivery.
