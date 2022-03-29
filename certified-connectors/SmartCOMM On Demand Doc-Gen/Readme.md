## SmartCOMM On Demand Generation Connector
Smart Communications™ is a leading cloud-based platform for enterprise customer communications. As the only cloud solution ranked as a Leader in Gartner’s Magic Quadrant for Customer Communications Management, more than 350 global brands – many in the world’s most highly regulated industries – rely on Smart Communications to make multi-channel customer communications more meaningful, while also helping them simplify their processes and operate more efficiently.

The SmartCOMM On Demand Connector for Power Apps includes a pre-built integration for producing Customer Communications. In addition, the connector provides RESTful webservice calls to SmartCOMM for on-demand use cases.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A SmartCOMM subscription / instance set up for your organization with user access
* You will need to work with your Smart Communications contact to set up a document template to generate documents from.
* To configure your component settings - you will need to know the ‘BatchConfigResID’ or SmartCOMM Template Selector resource ID. Ask your Smart Communications contact to help you find this.
* Setup your SmartCOMM instance with an OAuth 2.0 Client (Authorization Code grant type)

## Features
The connector has the following features:
* Includes input parameters for setting various SmartCOMM Generation options
* Accepts transaction Data (in both XML or JSON format)
* Generates Communication(s) which can be presented back to the Power App or pushed to external systems (E.g. SharePoint)

### Steps to Generate Communication
1. Provide the host URL of the SmartCOMM instance
2. Provide the OAuth 2.0 Client details - Client ID, Client Secret, Authorization URL and Token URL
3. Provide the Power Apps generated Redirect URL (e.g. https://global.consent.azure-apim.net/redirect)
3. Create a Data Connection to the SmartCOMM instance using a registered user account
4. Supply Request URL to generateDocument endpoint. E.g. https://<RegionServer>.smartcommunications.cloud/one/oauth2/api/v10/job/generateDocument
5. Supply required parameters (defined below). Note the transactionData parameter is required to be encoded as Base64.
6. Supply optional parameters as required
7. SmartCOMM On Demand Generation connector generates the communication(s) and returns the response (including the base64 encoded output) to the Power App
8. The Communication(s) can then be displayed in the Power App for download or pushed to external systems

## Supported Operations
The connector supports the following operations:
* `GenerateDocument`: Generates response including the base64 encoded byte[] output. Output supports different types of channel output. (e.g. SMS, PRINT, TEXT, XML, TML, XSLFO, HTML)

## Supported Parameters
The connector supports the following parameters:
* `includeDocumentData` : Optional query parameter. Boolean flag to determine whether the document data is included in the response. Possible values are true (Stateless) or false (Stateful). If true, the document data will be included in the response. The document data returned in the response will be Base64 encoded.
* `batchConfigResId` - The CMS ID of the Template Selector resource. The Template Selector is used to map the transaction data to the Template and Data Model.
* `projectId` : Resource versions referenced in this specified project will take precedence. If this value is not specified, the latest available resource version will be used. This should only be used in the development or testing scenarios. (optional)
* `transactionData` : The sample data. The data is encoded as Base64.
* `transactionRange` : If the sample data contains more than one transaction, this value specifies which transaction to generate the document (optional).
* `transactionDataType` : The content type of the transaction data. Can be 'application/xml' or 'application/json'. Defaults to 'application/xml' (optional).
* `Properties - name` : The property name (A list of optional configuration properties). E.g. `merge.pdf`
* `Properties - value` : The property value (A list of optional configuration properties). E.g. `true`

## Further information
* Further information regarding the functionality of the SmartCOMM API (specifically `GenerateDocument`) can be found here - https://<RegionServer>.smartcommunications.cloud/one/apiViewer/#!api=doc-gen&operation=generateDocumentNonMultiPart&resource=Job
* Ensure HTTPS is configured for end to end encryption