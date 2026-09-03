# LegiScan (Independent Publisher)

LegiScan is a legislative tracking and data service that provides real-time information on state and federal legislation. This connector allows users to programmatically access bill information, session data, people profiles, voting records, search results, and monitor states at the local level for all 50 U.S. states and Congress.

## Publisher: Dan Romano (swolcat)

## Prerequisites

You will need a [LegiScan Civic API key](https://legiscan.com/civic_api) to use this connector. Sign up via the LegiScan website to obtain access credentials.

The connector currently supports the following operations:

### Sessions & Metadata
- **GetSessionList**: Retrieves a list of all available sessions for each supported state and Congress.
- **GetMasterListRaw**: Returns a raw dictionary of all bills for a given session.
- **GetDatasetList**: Lists datasets of bulk legislative data for all states.
- **GetDataset**: Returns metadata for a specific dataset.
- **GetDatasetRaw**: Retrieves the full dataset contents for bulk processing.

### Bill Information
- **GetBill**: Returns structured information about a specific bill.
- **GetBillText**: Returns the full text of a specific bill.
- **GetAmendment**: Returns the content of a specific amendment.
- **GetSupplement**: Retrieves a supplement or attachment to a bill.

### Voting and People
- **GetRollCall**: Returns voting results for a specific roll call.
- **GetPerson**: Retrieves information about a legislator, sponsor, or associated person.
- **GetSessionPeople**: Returns all persons associated with a specific session.
- **GetSponsoredList**: Returns a list of bills sponsored by a person.

### Search
- **GetSearchResults**: Returns structured search results for a query.
- **GetSearchRawResults**: Returns raw search results suitable for diffing or auditing.

### Monitoring
- **GetMonitorList**: Returns the structured list of monitored bills.
- **GetMonitorListRaw**: Returns a lightweight version of the monitor list for change detection workflows.
- **SetMonitor**: Sets or updates the list of monitored bills for your account.

## Obtaining Credentials

1. Go to [https://legiscan.com/](https://legiscan.com/).
2. Click on "Generate Key!"
3. On the next page, find the "Sign up here" link or visit https://legiscan.com/user/register
4. After approval, you will receive an API key to use in the connector.

## Getting Started

1. Obtain an API key from LegiScan.
2. Read the docs: https://legiscan.com/misc/LegiScan_API_User_Manual.pdf

## Known Issues and Limitations

- Rate-limited depending on your API subscription tier.
- Full bill text may not be immediately available after introduction.
- Some state-level data may lag or be incomplete depending on legislature update frequency.

## Frequently Asked Questions

**Q:** Does this connector support federal legislation?

**A:** Yes, LegiScan includes U.S. Congress in addition to all 50 states.

**Q:** Can I track bill progress?

**A:** Yes, each bill includes a status and history of actions.

**Q:** Does this connector support state legislation?

**A:** Yes, LegiScan includes legislation activity for all 50 US states.
