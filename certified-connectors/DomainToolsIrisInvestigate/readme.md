# DomainTools Iris investigate

Map connected infrastructure to get ahead of threats. The Iris Investigate API delivers dozens of domain name attributes on every result including Risk Score, DNS, Whois, SSL, and more. It enables easy pivoting through different domain name attributes and exposes meaningful insights with connection counts on most data fields. It is best suited for human-scale interactions.

## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* DomainTools API Username
* DomainTools API Key

## Supported Operations
The connector supports the following operations:
* `Investigate Domain`:  Retrieves the infrastructure and whois data associated with a domain or comma-separated list of up to 100 domains.

* `Pivot by MX IP`: Returns up to 500 domains served by a given mail server IP. Use the optional 'active' and 'date updated after' parameters to pre-filter the result set.

* `Pivot by Nameserver IP Address`: Returns up to 500 domains served by a provided nameserver IP. Use the optional 'active' and 'date updated after' parameters to pre-filter the result set.

* `Pivot by Registrant Name`: Returns up to 500 domains exactly matching the provided Whois registrant field. Use the optional 'active' and 'date updated after' parameters to pre-filter the result set.

* `Pivot by Registrant Org`: Returns up to 500 domains exactly matching the provided Whois registrant organization field. Use the optional 'active' and 'date updated after' parameters to pre-filter the result set.

* `Pivot by SSL Hash`: Returns up to 500 domains with a SSL certificate matching a provided SHA-1 hash. Use the optional 'active' and 'date updated after' parameters to pre-filter the result set.

* `Pivot MX Host`: Returns up to 500 domains with a mail server on a provided domain name. Use the optional 'active' and 'date updated after' parameters to pre-filter the result set.

* `Pivot Nameserver Host`: Returns up to 500 domains served by a provided nameserver host. Use the optional 'active' and 'date updated after' parameters to pre-filter the result set.

* `Pivot SSL Email`: Returns up to 500 domains with a given email address on the SSL certificate. Use the optional 'active' and 'date updated after' parameters to pre-filter the result set.

* `Retrieve Account Information`: Returns information of the active API endpoints, rate limits and usage for an account.

* `Return Domains from Search Hash`: Import up to 500 domains from Iris Investigate into the Sentinel platform. Export your investigation by search hash (Iris Investigate -> Search -> Export). Use the optional 'active' and 'date updated after' parameters to pre-filter the result set.

* `Return Tagged With All`: Retrieve up to 500 domains tagged within the Iris Investigate UI. Given a comma-separated list of tags, returns domains that are tagged with ALL of the tags. Use the optional 'active' and 'date updated after' parameters to pre-filter the result set.

* `Return Tagged With Any`: Retrieve up to 500 domains tagged within the Iris Investigate UI. Given a comma-separated list of tags, returns domains that are tagged with ANY of the tags. Use the optional 'active' and 'date updated after' parameters to pre-filter the result set.

* `Reverse Email`: Returns up to 500 domains with an email address on the most recently available Whois record, DNS SOA record or SSL certificate. Use the optional 'active' and 'date updated after' parameters to pre-filter the result set.

* `Reverse Email Domain`: Returns up to 500 domains with the domain portion of an email address on the most recently available Whois or DNS SOA record. Use the optional 'active' and 'date updated after' parameters to pre-filter the result set.

* `Reverse IP`: Returns up to 500 domains that last resolved to a given IPv4 address an active DNS check. Use the optional 'active' and 'date updated after' parameters to pre-filter the result set.


## Support and documentation: 
For all the support requests and general queries you can contact enterprisesupport@domaintools.com or [contact us](https://www.domaintools.com/integrations)
