# Cognizant Automation Center
Cognizant Automation Center I&O is an integrated portfolio of Bots, services IPs, platform and solutions that brings the best of the breed Automated solutions. It is an AIOps platform that integrates with several systems in a complex IT ecosystem and facilitates transition Eliminate-Automation–Modernize using Devops SRE Operate framework. Cognizant Automation Center Integrates systems of engagement, systems of record, systems of monitoring & assurance and systems that execute Automation actions.

## Publisher: Cognizant

## Prerequisites

User should have valid Cognizant Automation Center Subscription

## Supported Operations
This Connector supports the following operation:

**Activity Operations**
- **Create Generic Activity:** This operation is used to create generic activity.
- **Read Activity Attributes** This operation is used to fetch all the activity attributes.
- **Create Activity Additional Attribute** This operation is used to insert or update additioanl attribute of an activity.
- **Link as Child Activity** This operation is used to link activity as child.
- **Create Activity Log** This operation is used to insert the log in Activity.

**Action Operations**
- **Trigger Derived Action:** This operation is used to trigger the derived action.
- **Get Action Status:** This operation is used to get the executed action status.
- **Update Action Output:** This operation is used to update executed action output.

**IS Operations**
- **Get IS Execution Status:** This operation is used to get the IS status.
- **Trigger IS:** This operation is used to trigger IS.

## Obtaining Credentials

This Connector will use the jwt token based authentication. JWT token can be generate by navigating to Administration -> Customer Data -> API Tokens inside the AutomationCenter application.

## Known Issues and Limitations

- **User not found in cache:** Navigate to API token and click cache token to resolve this issue.

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.