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
`Retrieve User Tags for a sample`: Lists existing tags for the requested sample, if there are any.  
`Delete User Tags from a sample`: Removes one or more User Tags from the requested sample.  
`Create User Tags for a sample`: Adds one or more User Tags to the requested sample, regardless of whether the sample already has any tags.  
`Delete the classification for a sample.`: Delete the classification of a sample.  
`Set classification for a sample`: Set the classification of a sample.  
`Retrieve a list of YARA rulesets`: Retrieve a list of YARA rulesets that are on the A1000 appliance.  
`Retrieve the contents of a YARA ruleset`: Retrieve the full contents of the requested ruleset in raw text/plain format.  
`Retrieve YARA matches for specified rulesets`: Retrieve the list of YARA matches (both local and cloud) for requested rulesets.  
`Delete a YARA ruleset`: Delete the specified YARA ruleset and its matches from the appliance.  
`Create or update a YARA ruleset`: Creates a new YARA ruleset if it doesn't exist. If a ruleset with the specified name already exists, a new revision (update) of the ruleset is created.  
`Enable or disable a YARA ruleset`: Enables a previously disabled YARA ruleset, or disables a currently enabled YARA ruleset.  
`Get YARA ruleset synchronization time`: Information about the current synchronization status for TitaniumCloud-enabled rulesets.  
`Set YARA ruleset synchronization time`: Modify the TitaniumCloud synchronization time for TitaniumCloud-enabled YARA rulesets.  
`Check YARA Retro status on the appliance`: Check the status of Local Retro on the A1000 appliance.  
`Start or stop a YARA Local Retro scan`: Allows users to initiate the Local Retro scan on the A1000 appliance, and stop the Local Retro scan that is in progress on the appliance.  
`YARA Cloud Retro status`: Check the status of Cloud Retro for the specified YARA ruleset.  
`Manage YARA Cloud Retro scans`: Start and stop a Cloud Retro scan for a specified ruleset on the A1000 appliance, as well as to clear all Cloud Retro results for the ruleset.  
`List containers for every requested hash`: Get a list of all top-level containers from which the requested sample has been extracted during analysis.  
`Delete a sample`: Delete a sample from A1000.  
`Download files extracted from a local sample`: Download files extracted from the requested sample to the local storage.  
`Reanalyze multiple samples`: Reanalyze multiple samples with selected services.


## Obtaining Credentials
Visit [reversinglabs.com](https://www.reversinglabs.com/products/malware-threat-hunting-and-investigations) to get in contact with ReversingLabs sales representatives.    
You can also request additional A1000 documentation, if needed.

## Getting Started
To get started, create a new connection towards the A1000 instance using your A1000 token.  
The value of the "Token" field needs to be written in the following form: `"Token 'your_token'"`

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps

