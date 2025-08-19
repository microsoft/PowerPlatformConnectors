## nBold Connector

### Publisher: nBold

## Overview
The **nBold** connector lets you build automation with Teams, channels, sites, Planner, and more to streamline collaboration and ensure governance. nBold allows you to create Teams templates with channels, file templates, folder structures, Planner boards, and more, enabling scalable team creation from templates.

## Prerequisites
To use this connector, you will need:
- A Microsoft Power Apps or Power Automate premium plan for at least one account.
- A valid trial or paid subscription to nBold.
- nBold installed and connected to your tenant.

## Supported Operations

### Triggers
- **When a Team Creation Approval is Requested**: Triggers when a team creation approval request is submitted.
- **When a Team Creation is Approved**: Triggers when a team creation request is approved.
- **When a Team Creation is Rejected**: Triggers when a team creation request is rejected.
- **When a Team is Created Using a Collaboration Template**: Triggers when a team is successfully created from a collaboration template.

### Actions
- **Create a Team Based on a Collaboration Template**: Creates a new team using a predefined collaboration template.
- **Retrieve Templates**: Fetches a list of available collaboration templates.
- **Get Team Details**: Retrieves details of a specific team.
- **Archive a Team**: Archives a team to restrict activity.
- **Unarchive a Team**: Restores an archived team to active status.
- **Invite a Member to a Team**: Adds a new member to a specified team.
- **Get the Channels of a Team**: Retrieves all channels within a team.
- **Create a Channel in a Team**: Creates a new channel in a specified team.
- **Get the Primary Channel of a Team**: Fetches the default primary channel of a team.
- **Create a Tab in a Team Channel**: Adds a tab to a specific channel in a team.

For more details, visit the [nBold API Documentation](https://docs.nbold.co/api).

## Obtaining Credentials
This connector requires OAuth authentication. To obtain credentials:
1. Go to the connector and sign in.
2. Grant the necessary consent to the app.
3. Authentication will be completed, and the connector will be ready for use.

## Getting Started
To get started with the nBold connector:
1. Ensure that you have the prerequisites in place.
2. Configure authentication by signing in and granting the required permissions.
3. Set up automation workflows using triggers and actions within Power Automate or Power Apps.

## Known Issues and Limitations
- The connector requires a valid nBold subscription; free access is not available.
- Some operations may require additional Microsoft Graph API permissions.
- Large-scale automation may require performance tuning depending on the number of requests.

## Frequently Asked Questions

### Why can't I create a team?
Ensure that you have the correct permissions and that your nBold subscription is active. Verify that your authentication setup is complete.

### How do I manage team approval requests?
Use the triggers provided to automate approval workflows in Power Automate.

## Deployment Instructions
This is a certified connector available in Microsoft Power Automate and Power Apps. No manual deployment is required. Simply search for **nBold** in the connector list and start using it in your workflows.