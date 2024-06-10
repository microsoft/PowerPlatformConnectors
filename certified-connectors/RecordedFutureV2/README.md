# Recorded Future V2
The Recorded Future integration allows real-time security intelligence to be integrated into popular Microsoft services like Sentinel, Defender ATP, and others. This empowers our clients to maximize their existing security investments, ensuring they have real-time intelligence to secure their cloud environments and reduce risk to the organization. The Recorded Future connector for Microsoft Azure enables access to dedicated actions for pulling Recorded Future indicators (IP, Domain, URL, Hash, Vulnerabilities), associated context (Risk Score, Risk Rules, High Confidence Links, and an Intelligence Card Link), Recorded Future Alerts, Playbooks Alerts and Detection Rules.

## Publisher: Recorded Future

## Whats new?
- Added Recorded Future Playbook Alerts actions
- New V2 Recorded Future Alerts actions 
- Added Recorded Future Detection Rules actions
- Added IntelligenceCloud parameter
- Added HTML response to lookup actions

## Prerequisites

To enable the Recorded Future for Microsoft Azure integration, users must be provisioned a Recorded Future API token. Please reach to your account manager to obtain the necessary API token.

## How to get credentials

Prior to use of the Recorded Future integration for Microsoft Azure, users must provision an API token from their account manager or from within the Recorded Future portal necessary for the integration.

1. Login to the Recorded Future Portal (https://app.recordedfuture.com). Click on the menu in the upper right and choose “User Settings”.

2. On the User Settings menu, choose the “API Access” section and click the “Generate New API Token” link.

3. Provide a name for your token, select a “Description” of “Microsoft Azure”, and then click the “Create” button.  Save the API token that is generated, since you will configure it within the Microsoft Azure connector for the integration.

## Supported Operations
This connector is used to pull Recorded Future indicators, alerts, playbook alerts, and detection rules :
1. IP Enrichment - Enrich an IP with Recorded Future data. 
2. Domain Enrichment - Enrich a domain with Recorded Future data. 
3. URL Enrichment - Enrich a URL with Recorded Future data.
4. Hash Enrichment - Enrich a hash with Recorded Future data. 
5. Vulnerability Enrichment - Enrich a vulnerability with Recorded Future data. 
6. Search Alert Notification - List Alert Notifications by a set of search parameters. 
7. Get Alert Notification by ID - Get the alert details of a triggered alert
8. Search Alert Rules - List alert rules by name 
9. Search Alert Notification (Deprecated) - Deprecated
10. Get Alert Notification by ID (Deprecated) - Deprecated
11. Search Playbook Alerts - List playbook alerts based on a set of search parameters
12. Get Playbook Alert by ID - Get the alert details of a playbook alert
13. Search Detection Rules - Get detection rules matching a search filter
14. Recorded Future RiskLists and SCF Download - Download Recorded Future Risk Lists and Security Control Feeds
15. SOAR API - Multi-Entitiy Enrichment - Enrich multiple entities at once (Specific Access is Required)

## Known issues and limitations

N/A
