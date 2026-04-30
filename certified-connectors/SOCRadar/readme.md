# SOCRadar Threat Intelligence

[SOCRadar](https://socradar.io) is an Extended Threat Intelligence (XTI) platform combining digital risk protection, external attack surface management and cyber threat intelligence in one SaaS solution. This connector lets you query SOCRadar threat intelligence in real time, retrieve dark web findings, and ingest curated IOC feeds into Microsoft Defender, Sentinel and Power Platform flows.

## Publisher: SOCRadar Inc.

## Prerequisites
- Active SOCRadar subscription (Standard Licensed APIs entitlement)
- SOCRadar API key (Settings → API Options)
- Your SOCRadar Company ID (numeric)
- Power Platform / Logic Apps environment

## Supported Operations
- Query Threat Intelligence (IOC lookup or boolean query)
- Get Threat Details (full content by hash)
- Get IOC Feed (collection-based)
- Get PII Exposure findings
- Get Black Market findings
- Get Botnet Data findings
- Get IM Content findings (Telegram/Discord)
- Get Suspicious Content findings

## Obtaining Credentials
1. Sign in at https://platform.socradar.com
2. Go to Settings → API & Integrations → API Options
3. Generate an API key and copy it
4. Note your Company ID from the dashboard URL

## Getting Started
1. In Power Automate or Logic Apps, add an action and search for "SOCRadar".
2. Provide your Company ID and API Key when creating the connection.
3. Choose an operation and pass the indicator value.

## Fetching Multiple IOC Collections

The `GetIOCFeed` operation fetches one collection at a time (each collection is a distinct URL). To pull data from up to ~40 collections in a single flow:

1. **Create an array variable** in your Power Automate flow named `CollectionList` with the following structure (add one item per collection):
   ```json
   [
     { "uuid": "a1b2c3d4-0000-0000-0000-111122223333", "name": "Ransomware IPs" },
     { "uuid": "b2c3d4e5-0000-0000-0000-222233334444", "name": "Phishing Domains" }
   ]
   ```
2. Add an **Apply to each** action and set the input to `CollectionList`.
3. Inside the loop, add a **Get IOC feed for a collection** action:
   - **Collection UUID** → `items('Apply_to_each')?['uuid']`
   - **Collection name** → `items('Apply_to_each')?['name']`  *(optional — echoed back in the response for traceability)*
4. Append each response to a result array or process inline.

This pattern supports any number of collections without flow duplication.

## Known Issues and Limitations
- All endpoints rate-limited to 1 request/second per API key.
- The `query` parameter is limited to 500 characters.
- Dark web endpoints require Digital Risk Protection license.

## FAQ
**Q: Which Microsoft products work with this?**
A: Power Automate, Logic Apps, Microsoft Sentinel (Playbooks), Microsoft Defender XDR (Custom Detection automations).

**Q: How do I report issues?**
A: https://socradar.io/support or support@socradar.io
