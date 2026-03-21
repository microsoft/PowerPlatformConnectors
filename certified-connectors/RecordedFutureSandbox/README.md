# Recorded Future Sandbox
The Recorded Future Sandbox Connector enables security and IT teams to analyze and understand URLs, which provides safe and immediate behavioral analysis of URLs, helping contextualize key artifacts in an investigation, and leading to faster triage. Through this connector, organizations can incorporate the malware analysis sandbox into automated workflows with applications such as Microsoft Defender for Endpoint and Microsoft Sentinel.

## Publisher: Recorded Future

## Prerequisites

To enable the Recorded Future Sandbox for Microsoft Azure integration, you must have a Recorded Future API token. If you are using the Enterprise Sandbox, you need one additional Enterprise Sandbox API token.

## How to obtain Recorded Future API token

Go to https://app.recordedfuture.com, click on Integration Center, and search for "Microsoft Sentinel For Sandbox". If you are an admin for your Recorded Future Enterprise, you can issue an API key from there.

Any questions about API tokens not covered by the above can be sent via email to our support team, support@recordedfuture.com.


<a id="how_to_contact_Recorded_Future"></a>
## How to obtain Recorded Future Enterprise Sandbox API token
If you are an Enterprise Sandbox customer, you need an additional API key.

To obtain the Enterprise Sandbox API token, sign in with your Recorded Future account [here](https://sandbox.recordedfuture.com/). Click on your account settings in the upper right corner. There you can find your API key in API Access.

If you were not able to sign in and obtain the Sandbox API token, request the token via this [Integration Support Ticket form](https://support.recordedfuture.com/hc/en-us/requests/new?ticket_form_id=360004119534) or support@recordedfuture.com.

## Supported Operations
This connector is used to submit URLs and files to Recorded Future Sandbox and then retrieve the summary and the report of the sample. The connector has no triggers and four actions:
1. Submit file sample - A file is submitted to the Sandbox. Returns an overview of the submission, including sample ID.
2. Submit URL sample - A URL is submitted to the Sandbox. Returns an overview of the submission, including sample ID.
3. Get the summary - Returns a short summary of the submission, including the status of the full report
4. Get the full report - Returns the full report

## Known Issues and Limitations
N/A
