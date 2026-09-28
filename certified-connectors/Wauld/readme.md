# Wauld

Wauld is a digital credentialing platform that helps organisations create, issue, manage, share, and verify digital badges and certificates. The Wauld connector enables users to issue credentials from automated flows and start flows when new credentials are issued in Wauld.

## Publisher: Wauld

## Prerequisites

To use the Wauld connector, you need:

- An active Wauld account.
- Access to the Wauld workspaces, engagements, and documents that will be used in the flow.
- A valid access token from Wauld.
- At least one configured credential document in Wauld before using the trigger or action.

## Supported Triggers

The connector supports the following trigger:

### New Credential Issued

Starts a flow when a new credential is issued in Wauld.

When configuring the trigger, enter the valid account id to finish set up.

The trigger returns the issued credential data made available by Wauld, including recipient and credential information that can be mapped to later steps in the flow.

## Supported Operations

The connector supports the following operation:

### Issue Credential

Issues a Wauld credential to a recipient.

When configuring the action, select the relevant:

- Workspace
- Engagement
- Document

Then map the recipient information and any required document attributes. The available fields may vary depending on the selected Wauld document.

## Obtaining Credentials

The Wauld connector uses secure access token authentication.

To obtain an access token:

1. Sign in to your Wauld account.
2. Open Integrations.
3. Select Power Automate.
4. Generate a new access token.
5. Copy the token and store it securely.
6. Paste the access token exactly as generated.

The access token provides access according to the permissions of the associated Wauld account. Do not share the token publicly or commit it to a source-code repository.

If a token is regenerated or revoked in Wauld, connections using the previous token must be updated with the new token.

## Frequently Asked Questions

https://help.wauld.com/en/help/articles/8431658-power-automate-integration

## Known Issues and Limitations

- The initial connector version supports one trigger and one action.
- The New Credential Issued trigger only processes credentials issued after the flow is enabled. It does not return historical issuance events.
- The trigger is scoped to the selected Wauld account. Any credential issued within that account will trigger the flow in Power Automate. To trigger the flow only for a specific document or engagement, use Power Automate's built-in Condition control to filter the trigger response.
- The Issue Credential action issues one credential per action execution.
- Required action fields depend on the configuration of the selected Wauld document.
- If a selected workspace, engagement, or document is removed or the user's access changes, the flow may fail and require reconfiguration.
- A revoked or regenerated Wauld access token causes authentication failures until the Power Automate connection is updated.
- Credential voiding, revocation, resending, updating, and historical credential lookup are not supported in the initial connector version.
- The connector can access only the Wauld resources available to the account associated with the access token.

## Deployment Instructions

Refer the documentation [here](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.
