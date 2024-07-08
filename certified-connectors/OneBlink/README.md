# OneBlink Connector

Empowers business users to create and deploy digital forms for enterprise and government organizations as web and native apps, while allowing developers to customize, extend, or harness the forms through their own custom apps. Connect your forms with this connector to allow easy submission of your form data into your backend systems or databases without the need to write complex integration code.

## Pre-requisites

- You will need to log into [OneBlink Console](https://console.oneblink.io) using your email address.
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

`GetFormSubmissionAttachment`: Retrieves attachments associated with a OneBlink form submission

Parameters:

| Name          | Key          | Required | Type    | Description                                     |
| ------------- | ------------ | -------- | ------- | ----------------------------------------------- |
| Form Id       | formId       | true     | integer | ID of the form the attachment was uploaded with |
| Attachment Id | attachmentId | true     | string  | ID of the uploaded attachment                   |

`GenerateFormSubmissionPDF`: Allows for Form Submission PDF to be generated

Parameters:

| Name                         | Key                      | Required | Type    | Description                                                                     |
| ---------------------------- | ------------------------ | -------- | ------- | ------------------------------------------------------------------------------- |
| Form Id                      | formId                   | true     | integer | ID of the form that was submitted                                               |
| Submission Id                | submissionId             | true     | string  | ID of the form submission                                                       |
| Include Submission Id In PDF | includeSubmissionIdInPdf | true     | boolean | The submission identifier can be included at the bottom of each page in the PDF |

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.

## Further Support

For further support, you can contact us at support@oneblink.io - we're always happy to help.
