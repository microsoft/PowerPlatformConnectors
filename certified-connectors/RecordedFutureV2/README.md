The Recorded Future integration allows real-time security intelligence to be integrated into popular Microsoft services like Sentinel, Defender ATP, and others. This empowers our clients to maximize their existing security investments, ensuring they have real-time intelligence to secure their cloud environments and reduce risk to the organization. The Recorded Future connector for Microsoft Azure enables access to dedicated actions for pulling Recorded Future indicators (IP, Domain, URL, Hash, Vulnerabilities), associated context (Risk Score, Risk Rules, High Confidence Links, and an Intelligence Card Link), and Recorded Future alerts.

## Whats new?

This new version of the connector improves the enrichment actions by replacing the Related Entity section with High Confidence Evidence Based Links. (More details on links can be found here: https://support.recordedfuture.com/hc/en-us/articles/1500006698521-Links).

## Why a new connector?

This connector uses a new underlying API specifically built to serve the connector. The new API requires different API tokens for authentication and we do not want to break usage for existing clients.

## Prerequisites

To enable the Recorded Future for Microsoft Azure integration, users must be provisioned a Recorded Future API token. Please reach to your account manager to obtain the necessary API token.

## How to get credentials

Prior to use of the Recorded Future integration for Microsoft Azure, users must provision an API token from their account manager or from within the Recorded Future portal necessary for the integration.

1. Login to the Recorded Future Portal (https://app.recordedfuture.com). Click on the menu in the upper right and choose “User Settings”.

2. On the User Settings menu, choose the “API Access” section and click the “Generate New API Token” link.

3. Provide a name for your token, select a “Description” of “Microsoft Azure”, and then click the “Create” button.  Save the API token that is generated, since you will configure it within the Microsoft Azure connector for the integration.


## Known issues and limitations

N/A
