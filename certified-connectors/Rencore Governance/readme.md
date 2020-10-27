# Rencore Governance

Organizations using Office 365 struggle with controlling their tenants. With Rencore Governance, organizations can **understand the current state** of their tenants, **monitor its most important aspects** and **automate handling issues**. Get started by signing up at [app.rencore.com](https://app.rencore.com).

## Pre-requisites

To use this connector, you need an account on [Rencore Governance](https://app.rencore.com).

## Supported Operations

### Automate handling policy violations in Office 365

This connector introduces a trigger that allows you to automate handling issues that Rencore Governance detected in your tenant. After detecting an issue in your tenant, Rencore Governance will trigger any flow that uses this connector, passing the following information:

- which resources violate the particular policy
- which of these resources are enabled or disabled
- who in your organization uses or owns these non-compliant resources
- how many resources have been analyzed in total
- how many resources do not comply with the particular policy
- how many users are affected

## How to get credentials

Start by signing in to [Rencore Governance](https://app.rencore.com) using your existing work account. After connecting to your Office 365 tenant and enabling one or more policies to track, you can use this connector by signing in using the same account and selecting the policy that you want to automate handling.

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps
