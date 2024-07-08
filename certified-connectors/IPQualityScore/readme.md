# IPQS Fraud and Risk Scoring Connector

IPQualityScore (IPQS) provides enterprise grade fraud prevention, risk analysis, and threat detection. Analyze IP addresses, phone numbers, email addresses, and URLs or domains to identify sophisticated bad actors and high risk behavior worldwide.

## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* IPQualityScore API Key

## Supported Operations
The connector supports the following operations:
* `Retrieve IP address reputation data`: This service performs real-time lookups to instantly determine how risky a user, click, or transaction is based on an IP address and optional device information. In addition to analyzing if the IP address is a proxy or VPN, the API returns over 20 relevant data points such as: 

  * Geo location data
  * ISP
  * Connection type
  * Device details
  * Recent reputation activity
  * Overall fraud score
  * Status as a proxy, VPN, or TOR connection
  * Abuse Velocity
  * Other similar data points to classify reputation and risk

* `Retrieve Email address reputation data`: This API provides real-time email address reputation scoring and validation with hundreds of syntax & DNS checks. The API can be leveraged to determine if the email address inbox exists with the mail service provider and is able to accept new messages. In addition, users can determine if the email address has a poor reputation or has recently been associated with abuse or threats. Additional risk scoring can detect disposable and temporary mail services as well as emails with a history of fraudulent behavior online.

* `Retrieve URL (or) Domain reputation data.`: Scans links and domains in real-time to detect suspicious URLs using trusted machine learning models. These machine learning models can accurately identify phishing links, malware URLs, viruses, parked domains, and suspicious URLs with real-time risk scores. In addition, the machine learning models can confidently classify poor reputation domains, suspicious links, and phishing URLs with a real-time API integration.
Features such as parking domain detection, domain spam scores, reputation checks, and domain age, elevates URL intelligence to a whole new level.

* `Retrieve Phone Number reputation data`: Accurately verify phone numbers worldwide and retrieve a combination of carrier and line type details with risk analysis data to assess phone number reputation. IPQS collects phone validation and verification data from a wide variety of carriers and tier 1 telecommunication providers, with support for all regions. Detect inactive and disconnected phone numbers for easy user validation similar to HLR & LRN lookups. Accurately identify virtual and disposable phone numbers along with numbers associated with abusive behavior online.

## Access Your API Key
Register for a free API key at [IPQualityScore.com](https://www.ipqualityscore.com/create-account/azure) or contact your account manager to access your existing API key.

## Support and documentation: 
For all the support requests and general queries you can contact Support@IPQualityScore.com or [contact us](https://www.ipqualityscore.com/contact-us)
