# Farsight DNSDB Connector

Farsight Security DNSDB® is the world’s largest DNS intelligence database that provides a fact-based view of the configuration of the global Internet infrastructure. DNSDB leverages Farsight’s Security Information Exchange (SIE) data-sharing platform and is engineered and operated by leading DNS experts. Farsight collects, filters, and verifies Passive DNS data from its global sensor array. DNSDB is the highest-quality and most comprehensive DNS intelligence data service of its kind.

## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* Farsight DNSDB API Key

## Supported Operations
The connector supports the following operations:
* `RRSet Lookup`: The “rrset” lookup queries DNSDB’s RRset index, which supports “forward” lookups based on the owner name of an RRset.

* `RRSet Lookup with RRType`: The “rrset” lookup queries DNSDB’s RRset index, which supports “forward” lookups based on the owner name of an RRset.

* `RRSet Lookup with RRType and Bailiwick`: The “rrset” lookup queries DNSDB’s RRset index, which supports “forward” lookups based on the owner name of an RRset.

* `RData Lookup`: The “rdata” lookup queries DNSDB’s Rdata index, which supports “inverse” lookups based on Rdata record values.

* `RData Lookup with RRType`: The “rdata” lookup queries DNSDB’s Rdata index, which supports “inverse” lookups based on Rdata record values.

* `Flexible Search`: Flexible Search adds both Regular Expressions and Globbing support to the DNSDB API to expand the types of search queries and add more control to searches.

* `Service Limits`: Retrieve service limits

## Access Your API Key
Register for a free API key at https://www.farsightsecurity.com/solutions/dnsdb/ .

## Support and documentation: 
For all the support requests and general queries you can contact support@farsightsecurity.com or [contact us](https://www.farsightsecurity.com/about-farsight-security/contacts/)
