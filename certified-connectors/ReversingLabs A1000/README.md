# ReversingLabs A1000
ReversingLabs A1000 Malware Analysis Appliance integrates the ReversingLabs game-changing TitaniumCore automated static analysis technology and the TitaniumCloud File Reputation Service database. The REST Services APIs enable analysts to input suspected samples, access unpacked files and view extracted Proactive Threat Indicators (PTIs). The platform performs an in-depth static analysis of a comprehensive array of file types including Windows, Linux, Mac OS, iOS, Android, Windows Mobile, email attachments, documents and firmware.

## Publisher: ReversingLabs

## Prerequisites
Access to an A1000 instance.
- The instance can either be deployed in the cloud or on premise.
- Visit [reversinglabs.com](https://www.reversinglabs.com/products/malware-threat-hunting-and-investigations) to request a demo.

## Supported Operations
`Retrieve the detailed analysis report`: Retrieve a detailed analysis report for local samples.  
`Submit a sample for analysis`: Submit a sample for analysis from a local directory or from a URL.  
`Retrieve the static analysis report`: Retrieve TitaniumCore analysis results for a local sample.  
`Retrieve the dynamic analysis report`: Create and download a PDF or HTLM report for samples that have gone through dynamic analysis in the ReversingLabs Cloud Sandbox.  
`Retrieve summary analysis report`: Retrieve a summary analysis report for local samples.
`Retrieve processing status for files`: Check status of submitted files.  
`Retrieve processing status for URL-s`: Check status of submitted URL-s.  
`Retrieve classification for a sample`: Retrieve classification status for a sample.  
`Create PDF report`: Create a PDF sample analysis report.    
`Check PDF report creation status`: Check the creation status of a requested PDF report.  
`Download PDF Report`: Download the generated PDF analysis Report.  
`Retrieve information for a URL`: Returns network threat intelligence about the provided URL.  
`Retrieve information for a domain`: Returns network threat intelligence about the provided domain.
`Retrieve information for an IP address`: Returns network threat intelligence about the provided IP address.  
`Retrieve IP address resolutions`: Provides a list of IP-to-domain mappings.  
`Retrieve URL-s hosted on the IP address`: Returns a list of URLs hosted on the submitted IP address.  
`Retrieve a list of files from the IP address`: Provides a list of hashes and classifications for files found on the submitted IP address.  
`Perform advanced search`: Search for samples available on the local A1000 instance and TitaniumCloud using the Advanced Search capabilities.  

## Obtaining Credentials
Visit [reversinglabs.com](https://www.reversinglabs.com/products/malware-threat-hunting-and-investigations) to get in contact with ReversingLabs sales representatives.​  
You can also request additional A1000 documentation, if needed.

## Getting Started
To get started, create a new connection towards the A1000 instance using your A1000 token.  
The value of the "Token" field needs to be written in the following form: `"Token 'your_token'"`

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps

