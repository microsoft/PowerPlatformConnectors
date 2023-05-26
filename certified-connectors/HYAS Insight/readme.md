# HYAS Insight Connector

HYAS Insight integration to Microsoft Azure Sentinel provides direct, high volume access to HYAS Insight data. It enables investigators and analysts to understand and defend against cyber adversaries and their infrastructure.

## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* HYAS Insight API Key

## Supported Operations
The connector supports the following operations:
* `Get Mobile Geolocation Information`: Returns a list of mobile geolocation information for the provided IPV4 address or IPV6 address.
* `Get Sinkhole Information`: Returns Sinkhole Information for the provided IPV4 address.
* `Get Passive DNS Information`: Returns Passive DNS Information for the provided IPV4 or Domain.
* `Get Dynamic DNS Information`: Returns Dynamic DNS Information for the provided IP address or Domain or Email address.
* `Get Passive Hash Information`: Returns Passive Hash Information for the provided IPV4 or Domain.
* `Get SSL Certificate Information`: Returns SSL Certificate Information for the provided Hash or Domain or IP address.
* `Get Whois Information`: Returns Whois Information for the provided Domain or Email address or Phone Number.
* `Get C2 Attribution Information`: Returns C2 Attribution Information for the provided Domain or IP address or Email address or SHA256.
* `Get Malware Sample Information`: Returns Malware Information for the provided Hash.
* `Get Malware Sample Record Information`: Returns Malware Sample Records for the provide MD5 or Domain or IPV4 address.
* `Get Open Source Indicators Information`: Returns a list of threat or intel indicators from open sources for the provided IPV4 address or IPV6 address or Domain or SHA1 or SHA256 or MD5.
* `Get Current Whois Information`: Returns Current Whois Information for the provided Domain.


## Support and documentation: 
For all the support requests and general queries you can contact support@hyas.com or visit [contact-us](https://www.hyas.com/contact)
