## GreenSign

GreenSign's electronic signature software streamlines document workflows and boosts productivity by eliminating the need for traditional paper-based processes.

## Pre-requisites

In order to use this connector, you will need an account with GreenSign. you can sign up [here](https://app.greensign.io/signup).

Once you've signed up, you can use GreenSign in PowerAutomate.

If you're an organization using GreenSign, you can [contact us](https://www.greensign.io/contact-us) for enabling your Power Automate integration.

## Obtaining Credentials.

This connector uses `Oauth` authentication. When creating a new flow (in Power Apps/Logic Apps), you'll be required to log in to GreenSign. Signup with GreenSign if you haven't already or if you're an organization member ask your GreenSign administrator to invite you to your organization's GreenSign account.

## Supported Operations

This connector supports the following operations:

* `Create envelope`: Create a new blank envelope.
* `Add envelope recipients`: Add recipients in an envelope.
* `Send Envelope`: Send an existing envelope for signing.
* `List templates`: List owned templates in a specified organization.
* `Create envelope using template with recipients`: Create a new envelope using a specified template and specify recipients.
* `Get document signing actions from envelope`: Get document signing actions from envelope.
* `Add documents to an envelope`: Add documents to an envelope.
* `Get recipient info from envelope`: Get recipient info from envelope.
* `Get document info from envelope`: Get document info from envelope.
* `Populate signing actions in an envelope`: Populate signing actions with data for a recipient in an envelope.

It also supports the following triggers:

* `When an envelope status changes`: Triggered when the status of an envelope changes to the one you specify.

For more information on parameters accepted/required by the connector's operations, please refer to our [documentation](https://developers.africastalking.com/docs/sms/overview).

## Known Issues and Limitations

For issues with GreenSign please email us at [support@greensign.io](support@greensign.io)

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
