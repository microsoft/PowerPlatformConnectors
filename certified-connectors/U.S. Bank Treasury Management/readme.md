# U.S. Bank Treasury Management
U.S. Bank provides a treasury management api's to create flexible and seamless banking experiences for your organization. Easily connect your ERP and other treasury applications to our Corporate Account Information and RTP® – Real-Time Payments services. Instantly access company accounts, transaction data and initiate and manage real-time payments through The Clearing House RTP® network.

## Publisher
U.S. Bank

## Prerequisites
* An account on the U.S. Bank developer portal.
* A completed Portal Access form that’s returned to U.S. Bank API Onboarding team.
* API credentials from the U.S. Bank API Onboarding team.
* SinglePoint user credentials from the U.S. Bank Treasury Management implementation team.

## Supported Operations
| Operation Name                            | Description                                                                                                                                        |
| ----------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------- |
| **InitiateRTPCreditTransfer**                | Initiate an RTP credit transfer Transaction                                                                            |
| **GetRTPCreditTransfer**      | Retrieve status and details of a RTP credit transfer Transaction sent                                                     |
| **InitiateRFP**                | Initiate an RTP request for payment (RFP)                                                                |
| **GetRFP**       | Retrieve status and details of an RFP sent                                                                         |
| **GetRTPEligibility**         | Check if receiver's bank is part of the RTP Network                                                                         |
| **GetAccountsList**    | Get list of accounts for the authorized customer                                                               |
| **GetAccountBalancesOfCurrentDay**         | Get account balances for an array of accounts for current-day                                                |
| **GetAccountBalancesOfPreviousDay**  | Get account balances for an array of accounts for previous-day                                                         |
| **GetTransactionsOfCurrentDay**         | Get transactions of an account for current-day                                          |
| **GetTransactionsOfPreviousDay**  | Get transactions of an account for prior business days over a date range                                                |

## Obtaining Credentials
1. Sign up to create a U.S Bank Developer Portal account on our [Developer Portal](https://developer.usbank.com/). Signing up is easy with on-screen instructions that guide you along the way. If you need help, contact us.

2. [Contact us](https://developer.usbank.com/contact) to request the keys. Enter the message below in the “Message” field on the [Contact us](https://developer.usbank.com/contact) form.

    “I want to access this API from a Microsoft connector.”


3. You’ll receive a Portal Access Form from [apionboarding@usbank.com](mailto:apionboarding@usbank.com) Complete the Portal Access Form and return it to U.S. Bank at [apionboarding@usbank.com](mailto:apionboarding@usbank.com).

4. Enter the following text in the “Describe how and why your business wants to consume U.S. Bank APIs and/or related data” field on the Portal Access Form:

    “I want to access the APIs through the Microsoft connector.” 

    To use actions related to Corporate Account Information, you must request access to the Corporate Account Information service. To use actions related to RTP® - Real-Time Payments, you must request access to the RTP® - Real-Time Payments service. 

5. The U.S. Bank API Onboarding Team will assign a U.S. Bank Treasury Management Consultant to your organization. The U.S. Bank Treasury Management Consultant or the U.S. Bank API Onboarding Team will contact you to discuss next steps.  

6. Confirm you want to use the connector in the discovery call with the U.S. Bank API Onboarding Team and the U.S. Bank Treasury Sales Team. 

7. Once confirmed, U.S Bank kicks off the implementation process. The API Onboarding Team will send your API credentials to you. A U.S. Bank Treasury Management implementation coordinator will send your SinglePoint credentials to your organization’s SinglePoint administrator.

8. You’re now ready to use the connector.

## Known Issues and Limitations
* OAuth MFA required, our API Onboarding team will coordinate generating your SinglePoint and OAuth registrations

## Deployment Instructions
* N/A
