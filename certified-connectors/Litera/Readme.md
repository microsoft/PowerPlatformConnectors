# Title
The Litera Search connector allows users to issue natural language searches for legal matters, and retrieve both the narrative and detail for a specific matter. The plugin works in conjunction with the API for Litera's Lattice application, which offers a proprietary UI to execute the same natural language searches and retrieve matter data from a legal firm's matter data store. As a Copilot plugin, the Litera plugin brings matter data to the core Microsoft applications used by attorneys.

## Publisher: Publisher's Name
Litera

## Prerequisites
* Users can leverage the Litera Search plugin only if they have a Lattice user account. Since Lattice is an enterprise product, users must be authenticated against their firm's Entra tenant. As such, the user's firm must purchase Litera Lattice and subsequently supply Litera with its Entra Tenant ID. (For Microsoft Copilot Plugin Certification Testing, please provide the appropriate Microsoft Entra Tenant ID.) Litera will then give the firm the API key needed to create a connection to the Copilot plugin.

## Supported Operations
There are no supported triggers.  The following actions are supported:
### `GetMatterList`:
  Allows the user to search for legal matters. The action returns a list of matter names and matter IDs that satisfy the specified user natural language request for matters.
### `GetMatterNarrative`:
  Allows the user to retrieve a matter narrative -- that is, the descriptive overview -- for the specified matter. Requires a client-matter number, for example "9902-20025". Returns the narrative text as a json string.
### `GetMatterDetail`:
  Allows the user to retrieve the details of a matter, including matter type, client, open/close dates, responsible attorney, and office, for the specified matter. Requires a client-matter number, for example "9902-20025". Returns the matter detail as a json string.
  
## Obtaining Credentials
1. See the Prerequisites section above for the prerequisite to obtaining credentials.
2. The user must attempt to log into the Lattice application using their appropriate Microsoft Entra credentials (For Microsoft Copilot Plugin Certification Testing, login at https://qa.lattice.lmscloudlab.com/.)
3. During the login process, the user will be prompted to grant consent to use their credentials. Upon granting consent, they will have access to both Litera Lattice and the Litera Search plugin.

## Known Issues and Limitations
Search criteria are limited to those supported by Litera Lattice: 
* Client Matter Number 
* Matter Type (hierarchical) 
* Industry (hierarchical) 
* Matter Practice Group 
* Matter Location 
* Client
* Open Date 
* Close Date 
* Timekeeper
* Responsible Attorney
* Billing Attorney
* Matter Office 
* Matter Kind (open/closed) 
* Total Hours (secured) 
* Total Billing (secured) 
* Adjudicator (judge name)
* Forum

For any other criteria, we run a semantic search against matter narrative to stack-rank for relevance.

## Deployment Instructions
Not Applicable.