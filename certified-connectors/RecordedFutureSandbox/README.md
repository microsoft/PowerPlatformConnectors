# Recorded Future Sandbox
The Recorded Future Sandbox Connector enables security and IT teams to analyze and understand URLs, which provides safe and immediate behavioral analysis of URLs, helping contextualize key artifacts in an investigation, and leading to faster triage. Through this connector, organizations can incorporate the malware analysis sandbox into automated workflows with applications such as Microsoft Defender for Endpoint and Microsoft Sentinel.

## Publisher: Recorded Future

## Prerequisites

To enable the Recorded Future Sandbox for Microsoft Azure integration, users must be provisioned two API tokens, one Recorded Future API token and one Sandbox API token. Please reach out to your account manager to obtain the necessary API token.

## How to obtain Recorded Future API token

Recorded Future clients interested in API access for custom scripts or to enable a paid integration can request an API Token via this [Integration Support Ticket form](https://support.recordedfuture.com/hc/en-us/requests/new?ticket_form_id=360004119534).  Please fill out the following fields, based on intended API usage.

Recorded Future API Services - Choose if your token is pertaining to one of the below Recorded Future API offerings:
- Connect API
- Entity Match API
- List API
- Identity API (Note:  Identity API is included with a license to Identity Intelligence Module)
- Detection Rule API
- Playbook Alert API (currently in Beta)

Integration Partner Category - Choose if your token is pertaining to a supported partner integration offering:
- Premier Integrations
- Partner Owned Integrations
- Client Owned Integration
- Intelligence Card Extensions

Select Your Problem - Choose "Upgrade" or "New Installation"

Note that for API access to enable a paid integration, Recorded Future Support will connect with your account team to confirm licensing and ensure the token is set up with the correct specifications and permissions.

Additional questions about API token requests not covered by the above can be sent via email to our support team, support@recordedfuture.com.


<a id="how_to_contact_Recorded_Future"></a>
## How to obtain Recorded Future Sandbox API token
To obtain the Sandbox API token sign in with your Recorded Future account [here](https://sandbox.recordedfuture.com/). Click on your account settings in the upper right corner. There you can find your API key in API Access.

If you were not able to sign in and obtain the Sandbox API token, request the token via this [Integration Support Ticket form](https://support.recordedfuture.com/hc/en-us/requests/new?ticket_form_id=360004119534) or support@recordedfuture.com.

## Supported Operations
This connector is used to submit URLs and files to Recorded Future Sandbox and then retrieve the summary and the report of the sample. The connector has no triggers and four actions:
1. Submit file sample - A file is submitted to the Sandbox. Returns an overview of the submission, including sample ID.
2. Submit URL sample - A URL is submitted to the Sandbox. Returns an overview of the submission, including sample ID.
3. Get the summary - Returns a short summary of the submission, including the status of the full report
4. Get the full report - Returns the full report

## Known Issues and Limitations
N/A
