# ProPublica Congress
Using the Congress API, you can retrieve legislative data from the House of Representatives, the Senate and the Library of Congress. The API, which originated at The New York Times in 2009, includes details about members, votes, bills, nominations and other aspects of congressional activity.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
The actions for this connector should be reviewed on the [ProPublica Congress API documentation page](https://www.propublica.org/datastore/api/propublica-congress-api).

## Obtaining Credentials
An API key will be emailed to you after submitting the request form on the documentation page.

## Supported Operations
### Get a list of members
To get a list of members of a particular chamber in a particular Congress, use the following URI structure. The results include all members who have served in that congress and chamber, including members who are no longer in office. To filter the list to only active members (or to see members who have left), use the in office attribute.
### Get a specific member
This action gets biographical and Congressional role information for a particular member of Congress.
### Get new members
This action gets a list of the most recent new members of the current Congress.
### Get current members by state or district
This action gets biographical and Congressional role information for a particular member of Congress.
### Get members leaving office
This action gets a list of members who have left the Senate or House or have announced plans to do so.
### Get a specific member's vote positions
This action gets the most recent vote positions for a specific member of the House of Representatives or Senate.
### Compare two members vote positions
This action compares two members’ vote positions in a particular Congress and chamber. Responses include four calculated values, showing the number and percentage of votes in which the members took the same position or opposing positions.
### Compare two members' bill sponsorships
This action compares bill sponsorship between two members who served in the same Congress and chamber.
### Get recent bills by a specific member
This action gets the 20 bills most recently introduced or updated by a particular member.
### Get quarterly office expenses by a specific house member
The House of Representatives publishes quarterly reports detailing official office expenses by lawmakers. The Congress API has data beginning in the third quarter of 2009. The most recent quarter may not be available until several months after it ends.
### Get quarterly office expenses by category for a specific house member
The House of Representatives publishes quarterly reports detailing official office expenses by lawmakers. The Congress API has data beginning in the third quarter of 2009. This action gets the amount a given lawmaker spent during a specified year and quarter in a specified category.
### Get quarterly office expenses for a specific category
This action gets the amount spent by individual lawmakers in a specified category during a specified year and quarter by category.
### Get recent privately funded trips
This action gets a list of privately funded trips reported by all House members and staff in a particular Congress.
### Get recent privately funded trips by a specific house member
This action gets a list of privately funded trips reported by a specific House member, including staff travel, in a particular Congress.
### Search bills
This action searches the title and full text of legislation by keyword to get the 20 most recent bills. Searches cover House and Senate bills from the 113th Congress through the current Congress (117th). If multiple words are given (e.g. query=health care) the search is treated as multiple keywords using the OR operator. Quoting the words (e.g. query="health care") makes it a phrase search. Search results can be sorted by date (the default) or by relevance, and in ascending or descending order.
### Get recent bills
This action gets summaries of the 20 most recent bills by type. For the current Congress, “recent bills” can be one of four types. For previous Congresses, “recent bills” means the last 20 bills of that Congress.
### Get recent bills by a specific subject
This action gets the 20 most recently updated bills for a specific legislative subject. Results can include more than one Congress.
### Get upcoming bills
This action gets details on bills that may be considered by the House or Senate in the near future, based on scheduled published or announced by congressional leadership. The bills and their potential consideration are taken from the House Majority Leader and floor updates from Senate Republicans.
### Get a specific bill
This action gets details about a particular bill, including actions taken and votes. The attributes house_passage_vote and senate_passage_vote are populated (with the date of passage) only upon successful passage of the bill.
### Get amendments for a specific bill
This action gets Library of Congress-assigned subjects about a particular bill. This request returns the 20 most recent results and supports paginated requests.
### Get subjects for a specific bill
This action gets Library of Congress-assigned subjects about a particular bill. This request returns the 20 most recent results and supports paginated requests.
### Get related bills for a specific bill
This action gets Library of Congress-identified related bills for a particular bill. This request returns the 20 most recent results and supports paginated requests.
### Get cosponsors for a specific bill
This action gets information about the cosponsors of a particular bill.
### Get recent votes
This action returns recent votes from the House, Senate or both chambers. It returns the 20 most recent results, sorted by date and roll call number, and you can paginate through votes using the offset query string parameter that accepts multiples of 20.
### Get a specific roll call vote
This action gets a specific roll-call vote, including a complete list of member positions.
### Get votes by type
This action returns vote information in four categories: missed votes, party votes, lone no votes and perfect votes.
### Get votes by date
This action gets all votes in a particular date range (fewer than 30 days).
### Get Senate nomination votes
This action gets Senate votes on presidential nominations. This request returns the 20 most recent results and supports paginated requests.
### Get recent personal explanations
This actions gets lists of personal explanations. Lawmakers, mostly in the House but also in the Senate, can make personal explanations for missed or mistaken votes in the Congressional Record. These explanations can refer to a single vote or to multiple votes.
### Get recent personal explanations by a specific member
This action gets recent personal explanations by a specific member.
### Get list of committees
This action gets a list of Senate, House or joint committees, including their subcommittees.
### Get a specific committee
This action gets information about a single Senate or House committee, including the members of that committee.
### Get recent committee hearings
This action gets a list of 20 upcoming Senate or House committee meetings.
### Get hearings for a specific committee
This actions gets a list of hearings for a specific Senate or House committee.
### Get a specific subcommittee
This action gets information about a single Senate or House subcommittee, including the members of that subcommittee.
### Get recent official communications
This action gets lists of official communications to Congress from the president, executive branch agencies and state legislatures to congressional committees.
### Get recent official communications by category
This action gets lists of official communications to Congress in a specific category. It returns the 20 most recent results for the specified type: ec (Executive Communication), pm (Presidential Message) or pom (Petition or Memorial).
### Get recent official communications by date
This action gets lists of official communications to Congress on a specific date.
### Get a specific nomination
This action gets details about a particular presidential civilian nomination.
### Get recent House and Senate floor actions
This action gets the latest actions from the House or Senate floor.
### Get House and Senate floor actions by date
This action gets actions from the House or Senate floor on a particular date.
### Get recent lobbying representation filings
This action gets the most recent lobbying representation filings.
### Search lobbying representation filings
This actions gets the 20 most recent lobbying representation filings for a given search term.
### Get a specific lobbying representation filing
This action gets a specific lobbying representation filing.

## Known Issues and Limitations
There are no known issues at this time.
