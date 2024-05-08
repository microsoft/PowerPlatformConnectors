# Litera Copilot Plugin
## Overview
The Litera Copilot Plugin allows users to search for legal matters, and retrieve both the narrative (that is, a descriptive overview) and detail for a specific matter. The plugin works in conjunction with the API for Litera's Lattice application, which offers a proprietary UI to execute the same natural language searches and retrieve matter data from a legal firm's matter data store. As a Copilot plugin, the Litera plugin brings matter data to the core Microsoft applications used by attorneys.


## Prerequisites/How to use the Plugin
Users can leverage the Litera plugin only if they have a Lattice user account. Since Lattice is an enterprise product, users must be authenticated against their firm's Entra tenant. As such, the following steps must be followed for a user to utilize the Lattice plugin:
1. The user's firm must purchase Litera Lattice and subsequently supply Litera with its Entra Tenant ID. (For Microsoft Copilot Plugin Certification Testing, please provide the appropriate Microsoft Entra Tenant ID.) Litera will then give the firm the API key needed to create a connection to the Copilot plugin.
2. The user must attempt to log into the Lattice application (For Microsoft Copilot Plugin Certification Testing, login at https://qa.lattice.lmscloudlab.com/.)
3. During the login process, the user will be prompted to grant consent to use their credentials. Upon granting consent, they will have access to both Litera Lattice and the Litera plugin.

 
## Supported Triggers
 
* None
 

## Supported Actions
The following actions are supported:
* `GetMatterList`:
  Allows the user to search for legal matters. The action returns a list of matter names and matter IDs that satisfy the specified user natural language request for matters.
* `GetMatterNarrative`:
  Allows the user to retrieve a matter narrative -- that is, the descriptive overview -- for the specified matter. Requires a client-matter number, for example "9902-20025". Returns the narrative text as a json string.
* `GetMatterDetail`:
  Allows the user to retrieve the details of a matter, including matter type, client, open/close dates, responsible attorney, and office, for the specified matter. Requires a client-matter number, for example "9902-20025". Returns the matter detail as a json string.