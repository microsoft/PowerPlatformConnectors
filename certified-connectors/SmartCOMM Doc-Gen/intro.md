Smart Communications™ is a leading cloud-based platform for enterprise customer communications. As the only cloud solution ranked as a Leader in Gartner’s Magic Quadrant for Customer Communications Management, more than 350 global brands – many in the world’s most highly regulated industries – rely on Smart Communications to make multi-channel customer communications more meaningful, while also helping them simplify their processes and operate more efficiently.

The SmartCOMM On Demand Connector for Power Apps includes a pre-built integration for producing Customer Communications. The connector provides RESTful webservice calls to SmartCOMM for on-demand use cases.

The connector support the SmartCOMM `GenerateDocument` operation which generates a response including the base64 encoded byte[] output. Output supports different types of channel output. (e.g. SMS, PRINT, TEXT, XML, TML, XSLFO, HTML)

## Prerequisites

In order to use the SmartCOMM component in Power Apps, you will need a few things completed first:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A SmartCOMM instance set up for your organization with user access
* You will need to work with your Smart Communications contact to set up a document template to generate documents from.
* To configure your component settings - you will need to know the ‘BatchConfigResID’ or SmartCOMM Template Selector resource ID. Ask your Smart Communications contact to help you find this.
* Setup your SmartCOMM instance with an OAuth 2.0 Client (Authorization Code grant type)

## How to get credentials

1. Contact your SmartCOMM User Admin to create your user access
2. Once your user access created, you will receive an email to activate your account
3. Follow the instructions to activate your account
4. In the Authorization Code flow, the SmartCOMM user that owns the resources in the SmartCOMM cloud authorizes clients to access their user account. Users will access the redirect URL to login the tenancy. Users can enter the their user name and credentials to access the tenancy for the first attempt. (the following attempts will pop up the access code page directly)
5. Accept the request to retrieve the access code, by click the Accept button
6. Clients then use access tokens to access the resources, not the user's credentials.

## Get started with your connector

1. Provide the host URL of the SmartCOMM instance
2. Provide the OAuth 2.0 Client details - Client ID, Client Secret, Authorization URL and Token URL
3. Provide the Power Apps generated Redirect URL (e.g. https://global.consent.azure-apim.net/redirect)
3. Create a Data Connection to the SmartCOMM instance using a registered user account
	* We need a complete document generator setup at SmartCOMM, which contains at least a template selector, a template and a data model.
4. Create Connector
	* required parameters:
		* Region
			* RegionServer for exmaple: eu10-sb
		* Client Id
			* It is the same as in the Provide the OAuth 2.0 Client details
		* Client Secret
			* It is the same as in the Provide the OAuth 2.0 Client details
5. Supply Request URL to generateDocument endpoint. E.g. https://<RegionServer>.smartcommunications.cloud/one/oauth2/api/v10/job/generateDocument
	* required parameters:
		* Include Document Data 
			* yes as usual
		* transactionData (Json or XML)
			* no need for base64 encoding, because it is done by the connector
			* it should be the same format as in SmartComm
		* batchConfigResId 
			* Resource ID of the Smartcomm Template Selector
		* projectId
			* default empty, but have to be filled in when the SmartComm project is not released
		* transactionRange
			* it can be empty, but when filled in it should show what indexes should be used from the array
		* transactionDataType
			* application/json or application/xml
			* it is the format of the transactionData parameter
		* extra params like merge.pdf can be read on SmartComm API description
			* https://<server>.smartcommunications.cloud/one/apiViewer/
			* Where <server> is the Smart Communications server you're using (NA1.smartcommunications.cloud, EU1.smartcommunications.cloud, and so on).
6. SmartCOMM On Demand Generation connector generates the communication(s) and returns the response (including the base64 encoded output) to the Power App
	* Returns an envelope array, which contains the base64 encoded documents.
	* The document usualy inside of <documentEnvelope><envelopes><envelope><primaryChannel><data>string</data>
7. The communication(s) can then be displayed in the Power App for download, archived (e.g. SharePoint, S3 bucket) or pushed to an output delivery service e.g. SNS, Outlook, mailhouse etc.

## Known issues and limitations

1. Rate limiting (check with your SmartCOMM Admin for more details as this is part of the Licence Agreement)
2. Individual maximum request size is 10MB

## Common errors and remedies

1. Ensure `transactionDataType` set to 'application/json' when using JSON payload.
2. Ensure specify `projectId` when you want to limit the scope within a SmartCOMM project
3. Ensure `includeDocumentData` set to true when you need th output to be included in the response
4. Refer to Common error codes section below.

## Common error codes

* HTTP 403 -  Generate Document has failed
This error returns `id` (the SmartCOMM error code) and the error `msg` (the SmartCOMM error description)

* HTTP 500 - Internal Server Error
We also recommend that in the event of an error when you call the service, you wait five seconds before retrying. On multiple continuous failures, your service should alert users and stop submitting that call, as your request may be invalid.

* HTTP 429 - Too Many Requests
If you get an HTTP 429 response, you should wait 60 seconds, then retry your request.

* HTTP 503 - Service Unavailable
An HTTP 503 response is sent during maintenance. You should wait 120 seconds then retry your request. Maintenance windows could last an hour or more during a milestone upgrade.

## FAQ
`Provide a breakdown of frequently asked questions and their respective answers here. This can cover FAQs about interacting with the underlying service or about the connector itself.`

* Where Can I find further information about the `GenerateDocument` service? -  can be found here - https://<RegionServer>.smartcommunications.cloud/one/apiViewer/#!api=doc-gen&operation=generateDocumentNonMultiPart&resource=Job

* Is data encrypted using this connector? - Yes, if HTTPS is configured the connection will be encrypted end to end.

* How can I get a SmartCOMM user account? - Contact your SmartCOMM user Admin.

* What's the authorization type? - OAuth 2.0

* How can I get the OAuth2 Client ID and secret? - Contact your SmartCOMM admin or refer to https://<RegionServer>.smartcommunications.cloud/one/help/oneplatform/en/index.htm#EditClient.htm

* How can I get the authorization code? - Refer to section 'How to get credentials' above for full details.

* How can I get support for this connector? - Contact support@smartcommunications.com for all support enquiries

* Where can I find more information about the SmartCOMM product? - https://www.smartcommunications.com

* How can I get the batchConfigResId? - Contact your SmartCOMM SME or refer to https://<RegionServer>.smartcommunications.cloud/one/help/oneplatform/en/index.htm#ResourceProperties.htm. 

* How can I get the projectId? - Contact your SmartCOMM SME or refer to https://<RegionServer>.smartcommunications.cloud/one/help/oneplatform/en/index.htm#ResourceProperties.htm.

* How soon will I get the response? - The response time can vary based on your network speed, payload size and resource complexity, etc. SmartCOMM recommends to run a performance baseline to collect the maximum response time and average response time

* How can I get the output from the response? - The successful response includes the base64 encoded byte[] output. To view the output, actions have to be taken to decode the byte[] output and save the copy in a format as specified in the "mimeType". For more details please refer to https://<RegionServer>.smartcommunications.cloud/one/apiViewer/#!api=doc-gen&operation=generateDocumentNonMultiPart&resource=Job