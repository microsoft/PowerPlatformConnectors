## CivicPlus Transform Connector

Empowers business users to create and deploy digital forms for enterprise and government organizations as web and native apps, while allowing developers to customize, extend, or harness the forms through their own custom apps. Connect your forms with Power Automate to allow easy submission of your form data into your backend systems or databases without the need to write complex integration code.

## Pre-requisites

- You will need to log into [CivicPlus Transform Productivity Suite](console.transform.civicplus.com) using your email address.
- Navigate to the `Developer Keys` section of the console and create a new API key with the `Forms` permission toggled on.

## Documentation

For further information on how to setup our connector, please refer to our support article [here](https://support.oneblink.io/support/solutions/articles/42000047071).

## Supported Operations

The connector supports the following operations:

`FormSubmissionMetaWebhookTrigger`: Creates a OneBlink Form Submission Meta Webhook

Returns:

| Name          | Path         | Type    |
| ------------- | ------------ | ------- |
| Form Id       | formId       | integer |
| Submission Id | submissionId | string  |

`GetFormSubmissionData`: Allows for OneBlink Form Submission data to be retrieved

Parameters:

| Name          | Key          | Required | Type    | Description                               |
| ------------- | ------------ | -------- | ------- | ----------------------------------------- |
| Form Id       | formId       | true     | integer | ID of the form being retrieved            |
| Submission Id | submissionId | true     | string  | ID of the form submission being retrieved |

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.

## Further Support

For further support, you can contact us at support@oneblink.io - we're always happy to help.
