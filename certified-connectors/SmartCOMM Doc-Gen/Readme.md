# SmartCOMM On Demand Generation Connector
Smart Communications is a leading cloud-based platform for enterprise customer communications. As the only cloud solution ranked as a Leader in Gartner’s Magic Quadrant for Customer Communications Management, more than 350 global brands – many in the world’s most highly regulated industries – rely on Smart Communications to make multi-channel customer communications more meaningful, while also helping them simplify their processes and operate more efficiently.

The SmartCOMM On Demand Connector for Power Apps includes a pre-built integration for producing Customer Communications. In addition, the connector provides RESTful webservice calls to SmartCOMM for on-demand use cases.

For this connector the region is fixed to ap10-sb (ap-southeast-2 - Sydney), so only ap10-sb.smartcommunications.cloud can be connected.

## Publisher: Smart Communications

## Features
The connector has the following features:
* Includes input parameters for setting various SmartCOMM Generation options
* Accepts transaction Data (in both XML or JSON format)
* Generates Communication(s) which can be presented back to the Power App or pushed to external systems (E.g. SharePoint)

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A SmartCOMM subscription / instance set up for your organization with user access
* You will need to work with your Smart Communications contact to set up a document template to generate documents from.
* To configure your component settings - you will need to know the ‘BatchConfigResID’ or SmartCOMM Template Selector resource ID. Ask your Smart Communications contact to help you find this.
* Setup your SmartCOMM instance with an OAuth 2.0 Client (Authorization Code grant type)

## Supported Operations
The connector supports the following operations:
### `GenerateDocument`
Generates response including the base64 encoded byte[] output. Output supports different types of channel output. (e.g. SMS, PRINT, TEXT, XML, TML, XSLFO, HTML)

## Supported Parameters
The connector supports the following parameters:
### `includeDocumentData`
Optional query parameter. Boolean flag to determine whether the document data is included in the response. Possible values are true (Stateless) or false (Stateful). If true, the document data will be included in the response. The document data returned in the response will be Base64 encoded.
### `batchConfigResId`
The CMS ID of the Template Selector resource. The Template Selector is used to map the transaction data to the Template and Data Model.
### `projectId`
Resource versions referenced in this specified project will take precedence. If this value is not specified, the latest available resource version will be used. This should only be used in the development or testing scenarios. (optional)
### `transactionData`
The sample data. The data will be transmitted to SmartCOMM in Base64 format, but the encoding is handled by a script, the connector parameter should be a simple JSON without Base64 encoding.
### `transactionRange`
If the sample data contains more than one transaction, this value specifies which transaction to generate the document (optional).
### `transactionDataType`
The content type of the transaction data. Can be 'application/xml' or 'application/json'. Defaults to 'application/xml' (optional).
### `Properties - name`
The property name (A list of optional configuration properties). E.g. `merge.pdf`
### `Properties - value`
The property value (A list of optional configuration properties). E.g. `true`

## Obtaining Credentials
1. Contact your SmartCOMM User Admin to create your user access
2. Once your user access created, you will receive an email to activate your account
3. Follow the instructions to activate your account
4. In the Authorization Code flow, the SmartCOMM user that owns the resources in the SmartCOMM cloud authorizes clients to access their user account. Users will access the redirect URL to login the tenancy. Users can enter the their user name and credentials to access the tenancy for the first attempt. (the following attempts will pop up the access code page directly)
5. Accept the request to retrieve the access code, by click the Accept button
6. Clients then use access tokens to access the resources, not the user's credentials.

## Getting Started
1. Provide the OAuth 2.0 Client details - Client ID, Client Secret, Authorization URL and Token URL
2. Provide the Power Apps generated Redirect URL (e.g. https://global.consent.azure-apim.net/redirect)
3. Create a Data Connection to the SmartCOMM instance using a registered user account
4. Supply Request URL to generateDocument endpoint. E.g. https://ap10-sb.smartcommunications.cloud/one/oauth2/api/v10/job/generateDocument
5. Supply required parameters (defined below).
6. Supply optional parameters as required
7. SmartCOMM On Demand Generation connector generates the communication(s) and returns the response (including the base64 encoded output) to the Power App
8. The communication(s) can then be displayed in the Power App for download, archived (e.g. SharePoint, S3 bucket) or pushed to an output delivery service e.g. SNS, Outlook, mailhouse etc.

## Known Issues and Limitations
1. Rate limiting (check with your SmartCOMM Admin for more details as this is part of the Licence Agreement)
2. Individual maximum request size is 10MB

## Frequently Asked Questions

### Where Can I find further information about the `GenerateDocument` service?
This can be found here - https://ap10-sb.smartcommunications.cloud/one/apiViewer/#!api=doc-gen&operation=generateDocumentNonMultiPart&resource=Job

### Is data encrypted using this connector?
Yes, if HTTPS is configured the connection will be encrypted end to end.

### How can I get a SmartCOMM user account?
Contact your SmartCOMM user Admin.

### What's the authorization type?
OAuth 2.0

### How can I get the OAuth2 Client ID and secret?
Contact your SmartCOMM admin or refer to https://ap10-sb.smartcommunications.cloud/one/help/oneplatform/en/index.htm#EditClient.htm

### How can I get the authorization code?
Refer to section 'How to get credentials' above for full details.

### How can I get support for this connector?
Contact support@smartcommunications.com for all support enquiries

### Where can I find more information about the SmartCOMM product?
https://www.smartcommunications.com

### How can I get the batchConfigResId?
Contact your SmartCOMM SME or refer to https://ap10-sb.smartcommunications.cloud/one/help/oneplatform/en/index.htm#ResourceProperties.htm. 

### How can I get the projectId?
Contact your SmartCOMM SME or refer to https://ap10-sb.smartcommunications.cloud/one/help/oneplatform/en/index.htm#ResourceProperties.htm.

### How soon will I get the response?
The response time can vary based on your network speed, payload size and resource complexity, etc. SmartCOMM recommends to run a performance baseline to collect the maximum response time and average response time

### How can I get the output from the response?
The successful response includes the base64 encoded byte[] output. To view the output, actions have to be taken to decode the byte[] output and save the copy in a format as specified in the "mimeType". For more details please refer to https://ap10-sb.smartcommunications.cloud/one/apiViewer/#!api=doc-gen&operation=generateDocumentNonMultiPart&resource=Job

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.

## Further information
* Further information regarding the functionality of the SmartCOMM API (specifically `GenerateDocument`) can be found here - https://ap10-sb.smartcommunications.cloud/one/apiViewer/#!api=doc-gen&operation=generateDocumentNonMultiPart&resource=Job
* Ensure HTTPS is configured for end to end encryption