# Calendly

Calendly helps you easily schedule meetings based on participants availability.

This connector allows you to subscribe to `invitee.created` and `invitee.canceled` webhook events.

## Publisher: Calendly

## Prerequisites

- A Calendly [Professional plan subscription or higher](https://calendly.com/pricing).
- A personal access token associated with a Calendly organization admin or owner.

## Supported Triggers

The connector supports the following triggers:

- `invitee.created`: Triggers when an event is scheduled.
- `invitee.canceled`: Triggers when an event is canceled.

## Obtaining Credentials

Follow the procedures below to generate a personal access token and authenticate an internal or private application with the Calendly API v2. Do not share your personal access token with public sources or reuse it across applications.

To further secure your personal access tokens, we do not display or store them in your Calendly account. After generation, they’re unretrievable.

_Note: The personal access token must be created by an organization owner or admin_

- Go to the [Integrations Page](https://calendly.com/integrations).
- Select the API & Webhooks tile
- If you have no prior personal access tokens, select Get a token now under Personal Access Tokens.
- If you already have a token, select Generate new token under Your personal access tokens.
- At Create your personal access token, create an identifiable name for your token and select Create Token, then Copy token.

## Get started with your connector

- Select `Create Webhook Subscription` from the list of Calendly triggers.
- In the `events` dropdown menu select the events you would like to subscribe to.

## Known Issues and Limitations

- The personal access token must be created by an organization admin or owner.
- User scoped webhook subscriptions are not supported at this time. The webhook subscription will be fired when an event is scheduled or canceled with any user in the Calendly organization.

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
