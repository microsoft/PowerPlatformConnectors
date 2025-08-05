# Trade.gov

The Trade.gov connector provides access to a range of public U.S. Department of Commerce datasets, including trade events, export service providers, ITA office locations, de minimis thresholds, and trade leads.

## Publisher: Dan Romano, IDR Consultants

## Prerequisites
A valid API key from the U.S. Department of Commerce Developer Hub is required.

## Obtaining Credentials
Visit [https://developer.trade.gov](https://developer.trade.gov) to register and obtain an API key.

## Supported Operations

### Search Trade Events
Retrieve upcoming trade events including conferences, webinars, and missions organized by ITA and partners.

### Search Business Service Providers
Access a directory of vetted service providers that support U.S. exporters globally.

### Search Consolidated Screening List
Search denied and restricted parties across BIS, OFAC, and State Department lists.

### Search De Minimis Thresholds
Retrieve customs and VAT thresholds for over 80 countries.

### Search ITA Office Locations
Find U.S. Commercial Service and export assistance centers globally.

### Search Trade Leads
Access contract opportunities, tenders, and leads for U.S. exporters.

## Known Issues and Limitations
- Date parameters must be formatted as YYYY-MM-DD.
- The connector only surfaces active entities from screening lists.
- Keyword search may not match across all fields for all endpoints.