
## Cloudmersive Security
[Security Threat Detection API](https://cloudmersive.com/security-threat-detection-api) is critical for detecting and blocking the most common security threats to your business.  Cloudmersive Security Threat Detection provides coverage for the most common types of security threats in one powerful connector.  Stateless high-security processing ensures fast performance and strong security.


## Pre-requisites
N/A


## API documentation
[Security Threat Detection API](https://api.cloudmersive.com/docs/security.asp) is available on the Cloudmersive website


## Supported Operations

### Automatically detect threats in an input string
Auto-detects a wide range of threat types in input string, including Cross-Site Scripting (XSS), SQL Injection (SQLI), XML External Entitites (XXE), Server-side Request Forgeries (SSRF), and JSON Insecure Deserialization (JID).

### Detect Insecure Deserialization JSON (JID) attacks in a string
Detects Insecure Deserialization JSON (JID) attacks from text input.

### Check text input for SQL Injection (SQLI) attacks
Detects SQL Injection (SQLI) attacks from text input.

### Protect text input from Cross-Site-Scripting (XSS) attacks through normalization
Detects and removes XSS (Cross-Site-Scripting) attacks from text input through normalization. Returns the normalized result, as well as information on whether the original input contained an XSS risk.

### Protect text input from XML External Entity (XXE) attacks
Detects XXE (XML External Entity) attacks from XML text input.

### Check a URL for Server-side Request Forgery (SSRF) threats
Checks if an input URL is at risk of being an SSRF (Server-side request forgery) threat or attack.

### Check if IP address is a known threat
Check if the input IP address is a known threat IP address. Checks against known bad IPs, botnets, compromised servers, and other lists of threats.

### Check if IP address is a Bot client threat
Check if the input IP address is a Bot, robot, or otherwise a non-user entity. Leverages real-time signals to check against known high-probability bots.

### Check if IP address is a Tor node server
Check if the input IP address is a Tor exit node server. Tor servers are a type of privacy-preserving technology that can hide the original IP address who makes a request.

## How to get credentials
- [Register](https://account.cloudmersive.com/signup) for a Cloudmersive Account
- [Sign In](https://account.cloudmersive.com/login) with your Cloudmersive Account and click on API Keys

Here you can create and see your API key(s) listed on the API Keys page.  Simply copy and paste this API Key into the Cloudmersive Security Connector.

Now you are ready to start using the Cloudmersive Security Connector.


## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps

