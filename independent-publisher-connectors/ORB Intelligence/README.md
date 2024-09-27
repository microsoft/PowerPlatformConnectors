# ORB Intelligence
Orb Intelligence provides clients with a framework of API calls to use the Orb database for their various use cases. The framework comprises five rings, Match, Fetch, Search, Lookalike, and Corporate Tree. The clients can access the various company details from the Orb Database using these commands.

## Publisher
### Aaryan Arora | Ankita Singh

## Prerequisites
You need to get the API key from [ORB API Schema](https://api.orb-intelligence.com/docs/#/) or [ORB Intelligence](http://orb-intelligence.com).
The API Key is available publicly.

## Supported Operations
The connector supports the following operations:
* `Match`: The Match call is used to match a customer record to an Orb profile.
* `Fetch`: The Fetch call retrieves the complete company profile from the Orb database based on the Orb Number.
* `Search`: The Search call is used to retrieve a list of companies from the Orb database for given search criteria.
* `Lookalike`: The Lookalike call identifies a list of companies from the Orb database with similar profiles to a target company
* `Corporate Tree`: Corporate Tree call is used to retrieve the information about a parent and all subsidiaries of a company.
### The Match API
>GET /3/match/
- The Match API call performs a candidate retrieval process, the process by which Orb records are identified as potential matches to the query. The candidate retrieval process can be thought of as throwing a large net out over all of the Orb reference database and pulling it back in to return all the records that could be considered a match based on the input fields provided.
### The Fetch API
>GET /3/fetch/{orb_number}
- The input to the Fetch call is an Orb Number. The Orb Number for a company record is obtained from choosing a candidate from the Match call or a previously identified matched record.
- The Fetch API call will return all the standard fields in the Orb database for a single company record corresponding to the Orb Number.

### Search API
>GET /3/search/
- The Search API lets users build out a list of companies for use cases, such as building out a list of companies in a specific territory or segment.
- In the Search call, if a user specifies that they want to retrieve the complete Orb profiles, the list will include all the Orb attributes associated with the companies in the list. By default, they will receive a summary profile of the companies and select the companies they wish to retrieve the complete profile using the Orb Number and the Fetch call.
### Lookalike API
>GET /3/lookalike/
- Lookalike API Call enables users to expand their total addressable market or evaluate leads by identifying companies that look like existing customers.
- If a user specifies that they want to retrieve the full Orb profiles, then the list will include all the Orb attributes associated with the companies in the list. By default, they will receive a summary profile of the companies, and can select the companies they wish to retrieve the full profile for using the Orb Number and the Fetch call.
### Corporate Tree
>GET /3/corporate_tree/{orb_num}/
- The Corporate Tree API call allows a user to retrieve the full corporate tree of subsidiaries for a given company, starting from its ultimate parent company. The input to the Corporate Tree call is the Orb Number of the target company (mandatory field).
***

## API Documentation
Visit [ORB Intelligence APIs reference](https://api.orb-intelligence.com/docs/#/) page for further details.
***
## Known Issues and Limitations
Not all operations provided by ORB Intelligence are part of the first IP connector submission. We will keep adding/updating/supporting this connector based on your feedback/requests :)
