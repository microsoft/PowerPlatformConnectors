# Recorded Future V2
The Recorded Future integration allows real-time security intelligence to be integrated into popular Microsoft services like Sentinel, Defender ATP, and others. This empowers our clients to maximize their existing security investments, ensuring they have real-time intelligence to secure their cloud environments and reduce risk to the organization. The Recorded Future connector for Microsoft Azure enables access to dedicated actions for pulling Recorded Future indicators (IP, Domain, URL, Hash, Vulnerabilities), associated context (Risk Score, Risk Rules, High Confidence Links, and an Intelligence Card Link), Recorded Future Alerts, Playbooks Alerts, Threat Map, Threat Indicators and Detection Rules.

## Publisher: Recorded Future

## Whats new?
- Recorded Future's Threat Actor Threat Map
- Recorded Future's Malware Threat Map
- Recorded Future's Threat Indicators for Actors
- Recorded Future's Threat Indicators for Malware

## Prerequisites

To enable the Recorded Future for Microsoft Azure integration, users must be provisioned a Recorded Future API token. Please reach to your account manager to obtain the necessary API token.

## How to get credentials

Recorded Future requires API keys to communicate with our API. To obtain API keys: [Start a 30-day free trial of Recorded Future for Microsoft Sentinel](https://go.recordedfuture.com/microsoft-azure-sentinel-free-trial?utm_campaign=&utm_source=microsoft&utm_medium=gta) or visit [Recorded Future Requesting API Tokens](https://support.recordedfuture.com/hc/en-us/articles/4411077373587-Requesting-API-Tokens) (Require Recorded Future Login) and request API token for ```Recorded Future for Microsoft Sentinel``` or/and ```Recorded Future Sandbox for Microsoft Sentinel```.

## Supported Operations
This connector is used to pull Recorded Future indicators, alerts, playbook alerts, threat map, threat indicators and detection rules:
1. Recorded Future RiskLists and SCF Download - Download Recorded Future Risk Lists and Security Control Feeds
2. IP Enrichment - Enrich an IP with Recorded Future data.
3. Domain Enrichment - Enrich a domain with Recorded Future data.
4. URL Enrichment - Enrich a URL with Recorded Future data.
5. Hash Enrichment - Enrich a hash with Recorded Future data.
6. Vulnerability Enrichment - Enrich a vulnerability with Recorded Future data.
7. SOAR API - Multi-Entitiy Enrichment - Enrich multiple entities at once (Specific Access is Required)
8. Search Triggered Alerts - List Alert Notifications by a set of search parameters.
9. Get Triggered Alerts by ID - Get the alert details of a triggered alert
10. Search Alert Rules - List alert rules by name
11. Search Alert Notification (Deprecated) - Deprecated
12. Get Alert Notification by ID (Deprecated) - Deprecated
13. Search Playbook Alerts - List playbook alerts based on a set of search parameters
14. Get Playbook Alert by ID - Get the alert details of a playbook alert
15. Fetch Threat Map actors - Fetch Threat Map data for the enterprise's primary organization with filters.
16. Fetch Threat Map malware - Fetch Threat Map data for the enterprise's primary organization with filters.
17. Fetch Threat indicators for Actors in STIX format - Fetch Threat Indicators for Actors in STIX format.
18. Fetch Threat Indicators for Malware in STIX format - Fetch Threat Indicators for Malware in STIX format.
19. Search Detection Rules (Preview) - Get detection rules matching a search filter

## Examples of Solutions for Microsoft Sentinel
Install guide of solutions using this connector: [Recorded Future Solutions for Microsoft Sentinel](https://github.com/Azure/Azure-Sentinel/blob/master/Solutions/Recorded%20Future/readme.md)


## Known issues and limitations

N/A
