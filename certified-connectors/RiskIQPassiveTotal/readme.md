# RiskIQ Illuminate

RiskIQ Illuminate reveals cyber threats relevant to your critical assets through connected digital relationships. It is the only security intelligence solution with tailored attack surface intelligence to uncover exposures, risks, and threats against your unique digital footprint, pinpointing what’s relevant to you—all in one place.

## Pre-requisites

You will need the following to proceed:  
* A Microsoft Power Apps or Power Automate plan with custom connector feature  
* An Azure subscription  
* The Power platform CLI tools  
* [RiskIQ API credentials](https://api.riskiq.net/api/concepts.html)  

## Supported Operations

The connector supports the following operations:

### Account

* `Get account metadata and settings` : Retrieve current account metadata and settings.
* `Get API usage history` : Retrieve the details of API usage history of the account.
* `Get active monitors` : Retrieve the set of active monitors.
* `Get current organization metadata` : Retrieve the details of current organization metadata.
* `Get account and organization quotas` : Retrieve the details of current account and organization quotas.
* `Get sources used for queries` : Retrieve the details of sources being used for queries.
* `Get team activity` : Retrieve the details of team activity.
* `Get items by classification` : Retrieve items with the specified classification.
  
### Actions

* `Delete tags` : Removes tags from an artifact.
* `Get tags` : Get tags from a given artifact.
* `Add tags` : Adds tags to a given artifact.
* `Set tags` : Sets tags to a given artifact.
* `Get bulk classification status` : Retrieve classification statuses for given domains.
* `Set bulk classification status` : Set classification statuses for given domains.
* `Get classification status` : Retrieve classification status for a given domain.
* `Set classification status` : Sets the classification status for a given domain.
* `Get compromised status` : Indicates whether or not a given domain has ever been compromised.
* `Set compromised status` : Sets status for a domain to indicate if it has ever been compromised.
* `Get dynamic DNS status` : Indicates whether or not a domain's DNS records are updated via dynamic DNS.
* `Set dynamic DNS status` : Sets a domain's status to indicate whether or not its DNS records are updated via dynamic DNS.
* `Get monitor status` : Indicates whether or not a domain is monitored.
* `Get sinkhole status` : Indicates whether or not an IP address is a sinkhole.
* `Set sinkhole status` : Sets status for an IP address to indicate whether or not it is a sinkhole.
* `Search tags` : Retrieve artifacts for a given tag.

### Artifact

* `Delete artifacts in bulk` : Delete artifacts in bulk by their artifacts ids.
* `Artifact updates in bulk` : Perform artifact updates in bulk.
* `Create artifacts in bulk` : Create artifacts in bulk with given parameters.
* `Delete artifact with a UUID` : Delete artifact having a certain UUID.
* `Find artifact` : Read existing artifacts. If no filters are passed, this returns all your personal artifacts created by you or your organization.
* `Update artifact` : Update artifact, or toggle monitoring status.
* `Create Artifact` : Create artifact with given parameters.

### Articles

* `Get articles indicators` : Retrieves articles indicators.
* `Get article details` : Retrieves the details of the article specified.
* `Get articles by indicator` : Retrieves all articles containing the indicator specified.
* `Get articles` : Retrieves all articles.

### Data Card

* `Get summary data card` : Retrieves a summary data card associated to the given query.

### Enrichment

* `Get enrichment data` : Get enrichment data for a query.
* `Get malware` : Get malware data for a query.
* `Get OSINT` : Get OSINT data for a query.
* `Get subdomains` : Get subdomains data for a query.

### Services

* `Get the open ports info for the IP address given` : The exposed services endpoints allow you to see services on recently open ports for an IP address.

### Monitor

* `Get alerts associated with an artifact or project` : Retrieve all alerts associated with an artifact or project.

### Project

* `Find project` : Retrieve all information related to project.
* `Create project` : Create project with given parameters.
* `Update project` : Updates a project denoted by project ID.
* `Delete project` : Delete project by project ID.
* `Set project tags` : Set the project tags of given project ID.
* `Add project tags` : Add tags to a project by project ID.
* `Remove project tags` : Remove tags from a project by project ID.

### SSL Certificates

* `Get SSL certificate` : Retrieves an SSL certificate by its SHA-1 hash.
* `Get SSL certificate history` : Retrieves the SSL certificate history for a given certificate SHA-1 hash or IP address.
* `Search SSL certificates` : Retrieves SSL certificates for a given field value.
* `Search SSL certificates by keyword` : Retrieves SSL certificates for a given keyword.

### Tag Artifact

* `Get artifact tags` : Retrieve the tags of an artifact or artifacts.
* `Set artifact tags` : Set the tags of an artifact or artifacts.
* `Update artifact tags` : Add tags to an artifact or artifacts.
* `Remove artifact tags` : Remove a set of tags from an artifact or artifacts.

### Trackers

* `Search trackers that match the criteria` : Retrieves hosts or IP addresses that employ a specific user tracking service.

### Host Attributes

* `Get components` : Retrieves the host attribute components of a query.
* `Get pairs` : Retrieves the host attribute pairs related to the query.
* `Get trackers` : Retrieves the host attribute trackers.
* `Get cookies` : Retrieves the host attribute cookies related to the query.

### Cookies

* `Get addresses by cookie domain` : Searches the cookies addresses information by cookie domain.
* `Get addresses by cookie name` : Searches the addresses information by cookie name.
* `Get hosts by cookie domain` : Searches the cookies hosts information by cookie domain.
* `Get hosts by cookie name` : Searches the hosts information by cookie name.

### Components

* `Get addresses by component name` : Searches the components addresses information by component name.
* `Get hosts by component name` : Searches the components hosts information by component name.

### Passive DNS

* `Get passive DNS` : Retrieves the passive DNS results from active account sources.
* `Get unique passive DNS` : Retrieves the unique passive DNS results from active account sources.
* `Search passive DNS` : Searches the Passive DNS data for a keyword query.

### WHOIS

* `Get WHOIS` : Retrieves the WHOIS data for the specified query.
* `Search WHOIS keyword` : Search WHOIS data for a keyword.
* `Search WHOIS` : Searches WHOIS data by field and query.

### Bulk Enrichment

* `Get enrichment data bulk` : Get bulk enrichment data for many queries.
* `Get malware bulk` : Get bulk malware data for many queries.
* `Get OSINT bulk` : Get bulk OSINT data for many queries.

### Reputation
* `Get reputation` : Retrieves reputation for given query.
### Attack Surface Intelligence
* `Get attack surface` : Finds the Attack Surface information of the given account.
* `Get attack surface priority detail by level` : Finds the Attack Surface Priority Information given the level (low, medium, high) associated to the given account.
* `Get attack surface insight by insight Id` : Finds the Attack Surface Insight Information given the insight ID for the given account
* `Get attack surface third party by vendor Id` : Finds vendors associated with the given vendor id for given account account
* `Get all third party vendors` : Finds all vendors associated with the given account
* `Get attack surface third party priority detail by vendor Id and level` : Finds vendors associated with the given vendor id and priority level for given account account
* `Get attack surface third party insight by vendor Id and insight Id` : Finds vendors associated with the given vendor id and insight ID for given account account
* `Get attack surface vulnerable components` : Finds the Attack Surface Vulnerable Components for the primary vendor.
* `Get attack surface third party vulnerable components` : Finds the Attack Surface Third-Party Vulnerable Components given the vendor ID.
* `Get attack surface vulnerable information` : Finds the Attack Surface Vulnerability Information for the primary vendor for the given account.
* `Get attack surface third party vulnerabilities` : Finds the Attack Surface Third-Party Vulnerability Information given the vendor ID.
* `Get attack surface vulnerability observations` : Finds the Attack Surface Vulnerability Observations for the primary vendor given a CVE.
* `Get attack surface third party vulnerability observations` : Finds the Attack Surface Third-Party Vulnerability Observations given the vendor ID and CVE.
### Intel Profiles
* `Get profile details` : Retrieves the details for the given profile.
* `Get all indicators for given profile` : Retrieves the indicators for the given profile id.
* `Get all profiles` : Retrieves all profiles.
* `Get all profiles by indicator` : Retrieves all profiles containing the given indicator.
 
## How to get credentials

Register for a test API key at [RiskIQ Security Intelligence Services](https://api.riskiq.net/api/concepts.html) or contact your account representative (support@riskiq.com) to identify your existing customer keys.

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps. Additionally, you can leverage this connector within Logic Apps.