# Introduction

BoldSign is a secure, easy-to-use electronic signature solution that provides legally binding ways to send, sign, and manage documents.

## Prerequisites

You need an active BoldSign account to access the e-signature platform and utilize its features through the connector. If you don't have an account yet, sign up for BoldSign at [BoldSign website](https://www.boldsign.com/).

## How to get credentials

BoldSign connector uses the OAuth 2.0 authentication and all you need is a BoldSign account to get started.

## Throttling Limits and Sandbox mode

The rate limiting will be applied as per the BoldSign API rate limits and your subscription plan. For more information, please refer to the [BoldSign API documentation](https://developers.boldsign.com/api-overview/rate-limit/?region=us).

While sending a signature request, you can set the `sandbox` parameter to `true` to send the request in the sandbox mode. This will allow you to test the signature request without consuming any credits and they will not be legally binding.

|                        | Sandbox                             | Live                          |
| ---------------------- | ----------------------------------- | ----------------------------- |
| **API Requests Limit** | Maximum of 50 per hour              | Higher limit of 2000 per hour |
| **Document Lifespan**  | Automatically deleted after 14 days | Never deleted automatically   |
| **PDF Watermarks**     | Test watermarks, not legally valid  | No watermarks, legally valid  |

## Deployment instructions

If you would like to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps, please follow the instructions provided in the [official documentation](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli). Also, please replace the `clientId` in the `apiProperties.json` file with your own credentials and the `clientSecret` with your own secret.
