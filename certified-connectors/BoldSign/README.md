# BoldSign

BoldSign is a secure, easy-to-use electronic signature solution that provides legally binding ways to send, sign, and manage documents.

## Publisher: Syncfusion, Inc.

## Prerequisites

You need an active BoldSign account to access the e-signature platform and utilize its features through the connector. If you don't have an account yet, sign up for BoldSign at [BoldSign website](https://www.boldsign.com/).

## Supported Operations

### Send document using template with recipient

Send a document out for signature using a specified template to recipients.

### Download document

Downloads the PDF file of the given document ID.

### Download audit trail

Download the audit trail of a completed document.

### Get document status

Retrieves the status and other information of a document.

### Event triggers

The list of supported trigger are:

- Sent
- Signed
- Completed
- Declined
- Revoked
- Reassigned
- Expired
- Viewed
- Authentication Failed
- Delivery Failed
- Send Failed
- Template Send Failed
- Draft Created

## Obtaining Credentials

BoldSign connector uses the OAuth 2.0 authentication and all you need is a BoldSign account to get started.

## Known Issues and Limitations

The rate limiting will be applied as per the BoldSign API rate limits and your subscription plan. For more information, please refer to the [BoldSign API documentation](https://developers.boldsign.com/api-overview/rate-limit/?region=us).

While sending a signature request, you can set the `sandbox` parameter to `true` to send the request in the sandbox mode. This will allow you to test the signature request without consuming any credits and they will not be legally binding.

|                        | Sandbox                             | Live                          |
| ---------------------- | ----------------------------------- | ----------------------------- |
| **API Requests Limit** | Maximum of 50 per hour              | Higher limit of 2000 per hour |
| **Document Lifespan**  | Automatically deleted after 14 days | Never deleted automatically   |
| **PDF Watermarks**     | Test watermarks, not legally valid  | No watermarks, legally valid  |

## Frequently Asked Questions

### How to switch regions?

BoldSign operates in two regions: US and EU. The connector seamlessly supports both regions, allowing you to select your preferred region during the connection creation process.

## Deployment Instructions

If you would like to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps, please follow the instructions provided in the [official documentation](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli). Also, please replace the `clientId` in the `apiProperties.json` file with your own credentials and the `clientSecret` with your own secret.
