# GlobalGiving Project
GlobalGiving connects nonprofits, donors, and companies in nearly every country in the world. We help fellow nonprofits access the funding, tools, training, and support they need to serve their communities.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You will need to [create an account](https://www.globalgiving.org/dy/v2/login/form.html?show=createAccount&fromProtected=false&andthen=/) on the GlobalGiving website.

## Obtaining Credentials
Once logged in, navigate to the [API](https://www.globalgiving.org/dy/v2/user/api/) tab on the My Account page to find your API Key. You will need this key for all operations, in addition to your email address and password to obtain an access token (only needed for some operations).

## Supported Operations
### Get access token
Access tokens are similar to session tokens and allow entering of credentials once per session. Once authenticated, a user is provided with an access token that is submitted with every secure request (those containing '/secure/' in the URL path, e.g. submitting a donation) instead of always submitting a username and password. Access tokens are short-lived and can expire so users need to re-GET a token at the start of each session or if their current access token expires. Note that access token timeouts are individually set for each API user as a database configuration and by default are set to 10 hours.
### Get all project information for a set of project IDs
This action retrieves information for projects filtered by a list of comma separated project ids. The result set is limited to a maximum of 10 results for a request.
### Get all project summary information for set of project IDs
This action retrieves a subset of the full project information for projects filtered by a list of comma separated project ids. The result set is limited to a maximum of 10 results for a request.
### Get all projects for a campaign
This action retrieves information for all projects in specific Campaign. The result set, however, is limited to a maximum of 10 results for every request. If there are more results available, the response will return two extra elements <hasNext> and <nextProjectId>. Note that these two elements are not present if there are no more available results. The <hasNext> element indicates whether there are more results available and the <nextProjectId> element indicates the next project id to include in the query string to retrieve the next set of results.
### Get all projects for a campaign summary
This action retrieves a subset of the full project information for all projects in a specific campaign. The result set, however, is limited to a maximum of 10 results for every request. If there are more results available, the response will return two extra elements <hasNext> and <nextProjectId>. Note that these two elements are not present if there are no more available results. The <hasNext> element indicates whether there are more results available and the <nextProjectId> element indicates the next project id to include in the query string to retrieve the next set of results.
### Get all projects for a country or region
This action retrieves information for all projects in a specific country or region. The result set, however, is limited to a maximum of 10 results for every request. If there are more results available, the response will return two extra elements <hasNext> and <nextProjectId>. Note that these two elements are not present if there are no more available results. The <hasNext> element indicates whether there are more results available and the <nextProjectId> element indicates the next project id to include in the query string to retrieve the next set of results.
### Get all projects for a country or region summary
This action retrieves a subset of the full project information for all projects in a specific country or region. The result set, however, is limited to a maximum of 10 results for every request. If there are more results available, the response will return two extra elements <hasNext> and <nextProjectId>. Note that these two elements are not present if there are no more available results. The <hasNext> element indicates whether there are more results available and the <nextProjectId> element indicates the next project id to include in the query string to retrieve the next set of results.
### Get all projects for a theme
This action retrieves information for all projects in a specific theme. The result set, however, is limited to a maximum of 10 results for every request. If there are more results available, the response will return two extra elements <hasNext> and <nextProjectId>. Note that these two elements are not present if there are no more available results. The <hasNext> element indicates whether there are more results available and the <nextProjectId> element indicates the next project id to include in the query string to retrieve the next set of results.
### Get all projects for a theme summary
This action retrieves a subset of the full project information for all projects in a specific theme. The result set, however, is limited to a maximum of 10 results for every request. If there are more results available, the response will return two extra elements <hasNext> and <nextProjectId>. Note that these two elements are not present if there are no more available results. The <hasNext> element indicates whether there are more results available and the <nextProjectId> element indicates the next project id to include in the query string to retrieve the next set of results.
### Get all projects for an organization
This action retrieves the full project information for all projects belonging to a specific organization. The result set, however, is limited to a maximum of 10 results for every request. If there are more results available, the response will return two extra elements <hasNext> and <nextProjectId>. Note that these two elements are not present if there are no more available results. The <hasNext> element indicates whether there are more results available and the <nextProjectId> element indicates the next project id to include in the query string to retrieve the next set of results.
### Get all projects for an organization summary
This action retrieves the full project information for all projects belonging to a specific organization. The result set, however, is limited to a maximum of 10 results for every request. If there are more results available, the response will return two extra elements <hasNext> and <nextProjectId>. Note that these two elements are not present if there are no more available results. The <hasNext> element indicates whether there are more results available and the <nextProjectId> element indicates the next project id to include in the query string to retrieve the next set of results.
### Get all projects
This action retrieves information for all projects. The result set, however, is limited to a maximum of 10 results for every request. If there are more results available, the response will return two extra elements <hasNext> and <nextProjectId>. Note that these two elements are not present if there are no more available results. The <hasNext> element indicates whether there are more results available and the <nextProjectId> element indicates the next project id to include in the query string to retrieve the next set of results.
### Get all projects summary
This action retrieves a subset of the full project information, for all projects. The result set, however, is limited to a maximum of 10 results for every request. If there are more results available, the response will return two extra elements <hasNext> and <nextProjectId>. Note that these two elements are not present if there are no more available results. The <hasNext> element indicates whether there are more results available and the <nextProjectId> element indicates the next project id to include in the query string to retrieve the next set of results.
### Get all projects IDs
This action retrieves all the available project IDs.
### Get featured projects
This action retrieves information for featured projects. The result set will always return 10 results. The featured projects are rotated hourly based on GlobalGiving ranking, and a "randomization" algorithm.
### Get featured projects summary
This action retrieves summary information for featured projects. The result set will always return 10 results. The featured projects are rotated hourly based on GlobalGiving ranking, and a "randomization" algorithm.
### Get image gallery
This action retrieves any extra images for a specific project. When requesting full <project> information, the <imageGallerySize> indicates the number of images available. If this number is greater than zero you can access the extra images using This action.
### Get organization
This action retrieves information for a specific organization.
### Get all organizations
This action retrieves information for all available organizations. The result set, however, is limited to a maximum of 10 results for every request. If there are more results available, the response will return two extra elements <hasNext> and <nextOrgId>. Note that these two elements are not present if there are no more available results. The <hasNext> element indicates whether there are more results available and the <nextOrgId> element indicates the next organization id to include in the query string to retrieve the next set of results.
### Get outstanding invoices
This action retrieves all outstanding invoices not yet reconciled. This is a secure request therefore, an access token (authentication) is required.
### Get max third party ID
This action retrieves the max (greatest alphabetically) third party transaction id associated with this API key. This is a secure request therefore, an access token (authentication) is required.
### Get region project counts for a specific region
This action retrieves project counts for a specific region.
### Get region project counts
This action retrieves all regions and countries with their associated project counts.
### Get regions
This action retrieves all regions.
### Get specific project
This action displays information for a specific project.
### Get specific project summary
This action displays a subset of the full project information for a specific project.
### Get GlobalGiving themes
This action retrieves all GlobalGiving themes.
### Get themes with project IDs
This action retrieves all GlobalGiving themes with their associated project IDs.
### Search projects
This action allows searching for projects using keywords.
### Submit a donation
This action allows a user to submit a donation request.
### Get gift card designs
When sending out email gift cards a gift card design must be specified. This action retrieves all the available GlobalGiving gift card designs to select from.
### Send a gift card
This action allows a user to send a gift card.
### Order a gift certificate
This action allows a user to submit an order for a gift certificate without submitting payment details. This allows GlobalGiving the ability to immediately generate gift certificates with the understanding that reconciliation of the order will take place at a later date.
### Get gift certificate details
This action allows a user to query the validity and details of a gift certificate.


## Known Issues and Limitations
Connection parameters cannot be used within operations, so when making actions that include your API key, email and password, you should secure those [inputs](https://docs.microsoft.com/en-us/power-automate/guidance/planning/define-input-output#securing-inputs-and-outputs) and [values](https://docs.microsoft.com/en-us/connectors/keyvault/).
