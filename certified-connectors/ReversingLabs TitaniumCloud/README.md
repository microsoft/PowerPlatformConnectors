# ReversingLabs TitaniumCloud
ReversingLabs TitaniumCloud is a threat intelligence solution providing up-to-date file reputation services, threat classification and rich context on over 10 billion goodware and malware files. A powerful set of REST API query and feed functions deliver targeted file and malware intelligence for threat identification, analysis, intelligence development, and threat hunting services.

## Publisher: ReversingLabs

## Prerequisites
TitaniumCloud account with adequate roles for the desired API-s.
- Visit [reversinglabs.com](https://www.reversinglabs.com/products/file-reputation-service) to request a TitaniumCloud account.

## Supported Operations
`Get file reputation (single query)`: Get information about the malware status of the requested sample.  
`Get file reputation (bulk query)`: Get information about the malware status of requested samples.  
`Get historical multi-AV scan records (single query)`: Provides cross-reference data (AV scanner scanning information, first and last seen date-time (UTC), sample type and size, first and last scanned date, etc.) for a given sample.  
`Get historical multi-AV scan records (bulk query)`: Provides cross-reference data (AV scanner scanning information, first and last seen date-time (UTC), sample type and size, first and last scanned date, etc.) for given samples.  
`Get file analysis (single query)`: Get the analysis results for the requested hash.  
`Get file analysis (bulk query)`: Get the analysis results for the requested hashes.  
`Get file analysis - non-malicious (single query)`: Get a response containing all public knowledge about the given non-malicious sample identified by hash.  
`Get file analysis - non-malicious (bulk query)`: Get a response containing all public knowledge about the given non-malicious samples identified by hash.  
`Get dynamic analysis report (merged)`: This query returns the merged analysis report for the requested sample hash.  
`Get dynamic analysis report (latest)`: This query returns the latest analysis report for the requested sample hash.  
`Get dynamic analysis report (specific)`: This query returns a specific analysis report for the requested sample hash defined by the analysis ID.  
`Get dynamic analysis report for an archive (merged)`: Returns the merged dynamic analysis report for each file within the archive.  
`Get dynamic analysis report for an archive (latest)`: Returns the most recent dynamic analysis report for each file within the archive.  
`Download sample`: Returns the contents of a sample matching the requested hash.  
`Get sample download status`: Returns the file size of samples matching the requested hash values, but only if they are available for download. If the requested samples are not available for download, their size in the response will be returned as -1.  
`Upload sample`: Upload a given sample identified by hash via open stream of POST data.  
`Upload sample metadata`: Upload metadata for the sample identified by hash.  
`Delete sample (single query)`: Deletes a single sample defined by the hash value.  
`Delete samples (bulk query)`: Deletes multiple samples at once defined by the list of hash values in the request payload.  
`Reanalyze sample (single query)`: Sends a sample defined by a hash for rescanning.  
`Reanalyze samples (bulk query)`: Sends multiple samples defined by hashes for rescanning.  
`Subscribe to reputation changes`: Subscribes to a list of samples for which the changed sections (if there are any) will be delivered in the Data Change Feed.  
`Unsubscribe from reputation changes`: Unsubscribes from a list of samples that the user was previously subscribed to.  
`Set start time for reputation changes`: Sets the starting timestamp for the reputation data changes feed.  
`Get reputation data changes`: Returns the next recordset with samples to which the user is subscribed with the starting point defined using the "Set start time for reputation changes" action.  
`Get continuous reputation data changes`: Returns a recordset with samples that the user is subscribed to from the requested timestamp onwards. The timestamp is defined in the request itself.  
`Submit sample for dynamic analysis`: Submits a sample for dynamic analysis.  
`Submit archive for dynamic analysis`: Submits an archive for dynamic analysis.  
`URI to hash search by URI SHA-1 (first page)`: Returns hashes related to the provided URI. This request accepts an URI in the form of a SHA-1 string and returns only the first page of results.  
`URI to hash search by URI SHA-1 (with page parameter)`: Returns hashes related to the provided URI. This request accepts an URI in the form of a SHA-1 string and returns the requested page of results.  
`URI to hash search by URI string (with page parameter)`: Returns hashes related to the provided URI. This request accepts an URI string and returns the requested page of results.  
`Get the URL report`: Returns the classification and reputation report for the submitted URL.  
`List files from a URL`: Retrieve a list of files downloaded from the submitted URL.  
`Get the latest URL analyses (first page)`: Returns the latest completed URL analyses. This action only returns the first page of results.  
`Get the latest URL analyses (with page parameter)`: Returns the latest completed URL analyses. This action returns the requested page of results.  
`Get URL analyses from requested time (first page)`: Returns a list of completed URL analyses, starting from the requested time. This action only returns the first page of results.  
`Get URL analyses from requested time (with page parameter)`: Returns a list of completed URL analyses, starting from the requested time. This action returns the requested page of results.  
`Analyze URL`: Requests an analysis of the submitted URL.  
`Get the domain report`: Returns threat intelligence data for the submitted domain.  
`List files from a domain`: Retrieve a list of files downloaded from the submitted domain.  
`Get URL-s from domain`: Provides a list of URLs associated with the requested domain.  
`Get domain resolutions`: Provides a list of domain-to-IP mappings for the requested domain.  
`Get domain related domains`: Provides a list of domains that have the same top parent domain as the requested domain.  
`Get the IP address report`: Returns threat intelligence data for the submitted IP.  
`List files from an IP address`: Retrieve a list of files downloaded from the submitted IP address.  
`Get URL-s from IP address`: Provides a list of URL-s associated with the requested IP.  
`Get IP address resolutions`: Provides a list of IP-to-domain mappings for the specified IP address.  
`Daily API usage (current user)`: Returns information about daily service usage for the TitaniumCloud account that sent the request.  
`Daily API usage (company)`: Returns information about combined daily service usage for all users in the company.  
`Monthly API usage (current user)`: Returns information about monthly service usage for the TitaniumCloud account that sent the request.  
`Monthly API usage (company)`: Returns information about combined monthly service usage for all users in the company.  
`Date range API usage (current user)`: Returns total usage for all product licenses with a fixed quota over a single date range for the current user.  
`Date range API usage (company)`: Returns information about combined date range service usage for all users in the company.  
`Get active YARA rulesets`: Returns information about the number of active YARA rulesets for the TitaniumCloud account that sent the request.  
`Get API quota limits (current user)`: Returns current quota limits for APIs accessible to the authenticated user.  
`Get API quota limits (company)`: Returns current quota limits for APIs available to all users belonging to the authenticated user’s company.  
`Network Reputation Api`: Provides information regarding the reputation of requested URL, domain or IP address  
`List User Override`: List user overrides for network locations  
`Network Reputation User Override`: Enables URL classification overrides  


## Obtaining Credentials
Visit [reversinglabs.com](https://www.reversinglabs.com/products/file-reputation-service) to get in contact with ReversingLabs sales representatives.​  
You can also request additional TitaniumCloud documentation, if needed.

## Getting Started
To get started, create a new connection towards TitaniumCloud using your credentials.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps

