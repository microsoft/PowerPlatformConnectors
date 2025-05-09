# ProPublica Campaign Finance
The Campaign Finance API includes information about electronic filings, which are submitted to the Federal Election Commission on nearly every day of the year. The API, which originated at The New York Times in 2008, provides details about specific types of filings, filings for a specific date and a summary of financial information in each filing.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
The actions for this connector should be reviewed on the [ProPublica Campaign Finance API documentation](https://projects.propublica.org/api-docs/campaign-finance/) page.

## Obtaining Credentials
An API key will be emailed to you after submitting the request form on the documentation page.

## Supported Operations
### Search for candidates
Retrieves candidates by first or last name.
### Get a candidate
Retrieves a specific candidate.
### Get top 20 candidates in specific financial category
Retrieves the top 20 candidates within a given financial category for a given campaign cycle.
### Get candidates from a state
Retrieves an array of FEC candidates for a given state (and optional chamber and district).
### Get recently added candidates
Retrieves the 20 most recently added candidates.
### Get recent late contributions
During the last 20 days before a primary or general election, candidate committees must file reports of any contributions of $1,000 or more within 48 hours of receipt. This retrieves the most recent late contributions to candidates.
### Get recent late contributions to a candidate
Retrieves the most recent late contributions to a specific candidate.
### Get recent late contributions to a committee
Retrieves the most recent late contributions to a specific committee.
### Get recent late contributions by date
Retrieves late contributions from a specific date.
### Search for committees
Retrieves a list of committees.
### Get a specific committee
Retrieves a specific FEC committee for a given campaign cycle.
### Get recently added committees
Retrieves the 20 most recently added FEC committees.
### Get recently added independent expenditure-only committees
Retrieves the 20 most recently added FEC independent expenditure-only committees, known as “super PACs”.
### Get committee filings
Retrieves the 20 most recent FEC filings from the specified committee.
### Get leadership committees
Retrieves committees designated as “leadership PACs” by the FEC.
### Search for electronic filings
Retrieves information about FEC reports filed electronically by a committee.
### Get electronic filings by date
Retrieves information about FEC reports filed electronically on a specific date.
### Get electronic filing form types
Retrieves a list of available form types for FEC electronic filings.
### Get electronic filings by type
Retrieves the most recent electronic filings by form type.
### Get summary for a specific presidential electronic filing
Retrieves summary figures from a specific FEC electronic filing (Form 3 filings by presidential candidates only).
### Get recent independent expenditures
Retrieves the 200 most recent independent expenditures and optionally can be offset by multiples of 20.
### Get independent expenditures by date
Retrieves all independent expenditures on a specific date (the date of activity, not the date filed with the FEC).
### Get independent expenditures by specific committee
Retrieves the 20 most recent independent expenditures by a given committee.
### Get independent expenditures that support or oppose a specific candidate
Retrieves the 200 most recent independent expenditures in support of or opposition to a given candidate.
### Get independent expenditures that support or oppose presidential candidates
Retrieves the 200 most recent independent expenditures in support of or opposition to any presidential candidate.
### Get independent expenditures office totals
Retrieves the amount of money spent in independent expenditures for a given office (either House, Senate or President).
### Get independent expenditures race totals for a specific committee
Retrieves the total amounts of money that a given committee has spent on individual races (consisting of a state, office and district) during a cycle.
### Get recent electioneering communications
Retrieves the 20 most recent broadcast advertisements that identify one or more federal candidates (and have aired 30 days before a primary election and 60 days before the general election).
### Get electioneering communications by specific committee
Retrieves the most recent broadcast advertisements by a specific committee that identify one or more federal candidates (and have aired 30 days before a primary election and 60 days before the general election).
### Get electioneering communications by date
Retrieves all broadcast advertisements that identify one or more federal candidates (and have aired 30 days before a primary election and 60 days before the general election) from a specific date.
### Get lobbyist bundlers for a specific committee
Retrieves the most recent lobbyist bundlers reported by a specific committee.

## Known Issues and Limitations
There are no known issues at this time.
