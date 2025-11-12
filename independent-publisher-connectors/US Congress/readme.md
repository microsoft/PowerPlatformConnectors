# US Congress API (Independent Publisher)

## Publisher
Dan Romano (swolcat)

## Prerequisites
To use this connector, you need:
- A **Congress.gov API key**, which you can obtain by signing up at [Congress.gov API](https://api.congress.gov/).
- Access to **Power Automate**, **Power Apps**, or **Logic Apps** to integrate the API into your workflows.

## Obtaining Credentials
The **US Congress API** requires an API key for authentication.
1. Go to [Congress.gov API](https://api.congress.gov/) and register for an account.
2. Once registered, generate an **API key** from the developer portal.
3. When setting up the connector in Power Platform, **enter your API key** in the authentication settings.

## Supported Operations

### Bills

- Get All Bills – Retrieve a list of bills sorted by the latest action.
	- GET /bill

- Get Bills by Congress – Retrieve bills filtered by a specific Congress.
	- GET /bill/{congress}

- Get Bills by Type – Retrieve bills filtered by Congress and bill type (House or Senate).
	- GET /bill/{congress}/{billType}

- Get Bill Details – Retrieve detailed information for a specific bill.
	- GET /bill/{congress}/{billType}/{billNumber}

- Get Bill Actions – Retrieve the list of actions taken on a specific bill.
	- GET /bill/{congress}/{billType}/{billNumber}/actions

- Get Bill Amendments – Retrieve all proposed amendments for a specific bill.
	- GET /bill/{congress}/{billType}/{billNumber}/amendments

- Get Bill Committees – Retrieve committees associated with a specific bill.
	- GET /bill/{congress}/{billType}/{billNumber}/committees

- Get Bill Cosponsors – Retrieve the list of cosponsors for a bill.
	- GET /bill/{congress}/{billType}/{billNumber}/cosponsors

- Get Related Bills – Retrieve bills that are related to a specific bill.
	- GET /bill/{congress}/{billType}/{billNumber}/relatedbills

- Get Bill Subjects – Retrieve the legislative subjects assigned to a bill.
	- GET /bill/{congress}/{billType}/{billNumber}/subjects

- Get Bill Summaries – Retrieve summaries explaining a bill’s content.
	- GET /bill/{congress}/{billType}/{billNumber}/summaries

- Get Bill Text Versions – Retrieve different text versions of a bill.
	- GET /bill/{congress}/{billType}/{billNumber}/text

- Get Bill Titles – Retrieve all official titles assigned to a bill.
	- GET /bill/{congress}/{billType}/{billNumber}/titles

### Amendments

- Get All Amendments – Retrieve a list of amendments sorted by the latest action.
	- GET /amendment

- Get Amendments by Congress – Retrieve amendments filtered by a specific Congress.
	- GET /amendment/{congress}

- Get Amendments by Type – Retrieve amendments filtered by Congress and amendment type.
	- GET /amendment/{congress}/{amendmentType}

- Get Amendment Details – Retrieve detailed information for a specific amendment.
	- GET /amendment/{congress}/{amendmentType}/{amendmentNumber}

- Get Amendment Actions – Retrieve the list of actions taken on a specific amendment.
	- GET /amendment/{congress}/{amendmentType}/{amendmentNumber}/actions

- Get Amendment Cosponsors – Retrieve the list of cosponsors for an amendment.
	- GET /amendment/{congress}/{amendmentType}/{amendmentNumber}/cosponsors

- Get Related Amendments – Retrieve a list of amendments related to a specific amendment.
	- GET /amendment/{congress}/{amendmentType}/{amendmentNumber}/amendments

- Get Amendment Text Versions – Retrieve different text versions of an amendment (available from the 117th Congress onwards).
	- GET /amendment/{congress}/{amendmentType}/{amendmentNumber}/text

### Summaries

- Get All Summaries – Retrieve a list of bill summaries sorted by the latest update.
	- GET /summaries

- Get Summaries by Congress – Retrieve summaries filtered by a specific Congress.
	- GET /summaries/{congress}

- Get Summaries by Bill Type – Retrieve summaries filtered by Congress and bill type.
	- GET /summaries/{congress}/{billType}

### Congress

- Get All Congress Sessions – Retrieve a list of congressional sessions.
	- GET /congress

- Get Congress Session Details – Retrieve details about a specific Congress session.
	- GET /congress/{congress}

- Get Current Congress Details – Retrieve details about the current Congress.
	- GET /congress/current

### Members

- Get All Members – Retrieve a list of all congressional members.
	- GET /member

- Get Member Details – Retrieve details about a specific congressional member.
	- GET /member/{bioguideId}

- Get Member Sponsored Legislation – Retrieve a list of bills sponsored by a specific member.
	- GET /member/{bioguideId}/sponsored-legislation

- Get Member Cosponsored Legislation – Retrieve a list of bills cosponsored by a specific member.
	- GET /member/{bioguideId}/cosponsored-legislation

- Get Members by Congress – Retrieve a list of members from a specific Congress.
	- GET /member/congress/{congress}

- Get Members by State – Retrieve a list of members representing a specific state.
	- GET /member/{stateCode}

- Get Members by State & District – Retrieve a list of members filtered by state and district.
	- GET /member/{stateCode}/{district}

- Get Members by Congress, State, & District – Retrieve a list of members filtered by Congress, state, and district.
	- GET /member/congress/{congress}/{stateCode}/{district}

### Committees

- Get All Committees – Retrieve a list of all congressional committees.
	- GET /committee

- Get Committees by Chamber – Retrieve committees filtered by House or Senate.
	- GET /committee/{chamber}

- Get Committees by Congress – Retrieve committees filtered by a specific Congress.
	- GET /committee/{congress}

- Get Committees by Congress & Chamber – Retrieve committees filtered by a specific Congress and chamber.
	- GET /committee/{congress}/{chamber}

- Get Committee Details – Retrieve details about a specific congressional committee.
	- GET /committee/{chamber}/{committeeCode}

- Get Committee Bills – Retrieve legislation associated with a specific committee.
	- GET /committee/{chamber}/{committeeCode}/bills

- Get Committee Reports – Retrieve reports published by a specific committee.
	- GET /committee/{chamber}/{committeeCode}/reports

- Get Committee Nominations – Retrieve nominations associated with a specific committee.
	- GET /committee/{chamber}/{committeeCode}/nominations

- Get House Committee Communications – Retrieve House communications associated with a committee.
	- GET /committee/{chamber}/{committeeCode}/house-communication

- Get Senate Committee Communications – Retrieve Senate communications associated with a committee.
	- GET /committee/{chamber}/{committeeCode}/senate-communication

### Committee Reports

- Get All Committee Reports – Retrieve a list of committee reports.
	- GET /committee-report

- Get Committee Reports by Congress – Retrieve committee reports filtered by a specific Congress.
	- GET /committee-report/{congress}

- Get Committee Reports by Type – Retrieve reports filtered by Congress and report type.
	- GET /committee-report/{congress}/{reportType}

- Get Committee Report Details – Retrieve details about a specific committee report.
	- GET /committee-report/{congress}/{reportType}/{reportNumber}

- Get Committee Report Text – Retrieve text versions of a specific committee report.
	- GET /committee-report/{congress}/{reportType}/{reportNumber}/text

### Committee Prints

- Get All Committee Prints – Retrieve a list of all committee prints.
	- GET /committee-print

- Get Committee Prints by Congress – Retrieve committee prints filtered by a specific Congress.
	- GET /committee-print/{congress}

- Get Committee Prints by Chamber – Retrieve committee prints filtered by Congress and chamber.
	- GET /committee-print/{congress}/{chamber}

- Get Committee Print Details – Retrieve details about a specific committee print.
	- GET /committee-print/{congress}/{chamber}/{jacketNumber}

- Get Committee Print Text – Retrieve text versions of a specific committee print.
	- GET /committee-print/{congress}/{chamber}/{jacketNumber}/text

### Committee Meetings

- Get All Committee Meetings – Retrieve a list of all committee meetings.
	- GET /committee-meeting

- Get Committee Meetings by Congress – Retrieve committee meetings filtered by a specific Congress.
	- GET /committee-meeting/{congress}

- Get Committee Meetings by Chamber – Retrieve committee meetings filtered by Congress and chamber.
	- GET /committee-meeting/{congress}/{chamber}

- Get Committee Meeting Details – Retrieve details about a specific committee meeting.
	- GET /committee-meeting/{congress}/{chamber}/{eventId}

### Hearings

- Get All Hearings – Retrieve a list of all hearings.
	- GET /hearing

- Get Hearings by Congress – Retrieve hearings filtered by a specific Congress.
	- GET /hearing/{congress}

- Get Hearings by Chamber – Retrieve hearings filtered by Congress and chamber.
	- GET /hearing/{congress}/{chamber}

- Get Hearing Details – Retrieve details about a specific hearing.
	- GET /hearing/{congress}/{chamber}/{jacketNumber}

### Congressional Record

- Get All Congressional Records – Retrieve a list of congressional record issues sorted by most recent.
	- GET /congressional-record

### Daily Congressional Record

- Get All Daily Congressional Records – Retrieve a list of daily congressional record issues sorted by most recent.
	- GET /daily-congressional-record

- Get Daily Congressional Records by Volume – Retrieve daily congressional records filtered by a specific volume number.
	- GET /daily-congressional-record/{volumeNumber}

- Get Daily Congressional Records by Issue – Retrieve daily congressional records filtered by volume number and issue number.
	- GET /daily-congressional-record/{volumeNumber}/{issueNumber}

- Get Daily Congressional Record Articles – Retrieve a list of articles for a specific daily congressional record issue.
	- GET /daily-congressional-record/{volumeNumber}/{issueNumber}/articles

### Bound Congressional Record

- Get All Bound Congressional Records – Retrieve a list of bound congressional records sorted by most recent.
	- GET /bound-congressional-record

- Get Bound Congressional Records by Year – Retrieve bound congressional records filtered by a specific year.
	- GET /bound-congressional-record/{year}

- Get Bound Congressional Records by Month – Retrieve bound congressional records filtered by year and month.
	- GET /bound-congressional-record/{year}/{month}

- Get Bound Congressional Records by Day – Retrieve bound congressional records filtered by year, month, and day.
	- GET /bound-congressional-record/{year}/{month}/{day}

### House Communications

- Get All House Communications – Retrieve a list of all House communications.
	- GET /house-communication

- Get House Communications by Congress – Retrieve House communications filtered by a specific Congress.
	- GET /house-communication/{congress}

- Get House Communications by Type – Retrieve House communications filtered by Congress and communication type.
	- GET /house-communication/{congress}/{communicationType}

- Get House Communication Details – Retrieve details about a specific House communication.
	- GET /house-communication/{congress}/{communicationType}/{communicationNumber}

### House Requirements

- Get All House Requirements – Retrieve a list of House requirements.
	- GET /house-requirement

- Get House Requirement Details – Retrieve details about a specific House requirement.
	- GET /house-requirement/{requirementNumber}

- Get Matching Communications for House Requirement – Retrieve a list of communications that match a specific House requirement.
	- GET /house-requirement/{requirementNumber}/matching-communications

### Senate Communications

- Get All Senate Communications – Retrieve a list of all Senate communications.
	- GET /senate-communication

- Get Senate Communications by Congress – Retrieve Senate communications filtered by a specific Congress.
	- GET /senate-communication/{congress}

- Get Senate Communications by Type – Retrieve Senate communications filtered by Congress and communication type.
	- GET /senate-communication/{congress}/{communicationType}

- Get Senate Communication Details – Retrieve details about a specific Senate communication.
	- GET /senate-communication/{congress}/{communicationType}/{communicationNumber}

### Nominations

- Get All Nominations – Retrieve a list of nominations sorted by the date received from the President.
	- GET /nomination

- Get Nominations by Congress – Retrieve nominations filtered by a specific Congress.
	- GET /nomination/{congress}

- Get Nomination Details – Retrieve details about a specific nomination.
	- GET /nomination/{congress}/{nominationNumber}

- Get Nominees for a Nomination – Retrieve a list of nominees for a specific nomination.
	- GET /nomination/{congress}/{nominationNumber}/{ordinal}

- Get Nomination Actions – Retrieve the list of actions taken on a specific nomination.
	- GET /nomination/{congress}/{nominationNumber}/actions

- Get Nomination Committees – Retrieve the list of committees associated with a specific nomination.
	- GET /nomination/{congress}/{nominationNumber}/committees

- Get Nomination Hearings – Retrieve the list of printed hearings associated with a specific nomination.
	- GET /nomination/{congress}/{nominationNumber}/hearings

### Treaties

- Get All Treaties – Retrieve a list of treaties sorted by the date of the last update.
	- GET /treaty

- Get Treaties by Congress – Retrieve treaties filtered by a specific Congress.
	- GET /treaty/{congress}

- Get Treaty Details – Retrieve details about a specific treaty.
	- GET /treaty/{congress}/{treatyNumber}

- Get Partitioned Treaty Details – Retrieve details about a specified partitioned treaty.
	- GET /treaty/{congress}/{treatyNumber}/{treatySuffix}

- Get Treaty Actions – Retrieve the list of actions taken on a specific treaty.
	- GET /treaty/{congress}/{treatyNumber}/actions

- Get Partitioned Treaty Actions – Retrieve the list of actions on a specified partitioned treaty.
	- GET /treaty/{congress}/{treatyNumber}/{treatySuffix}/actions

- Get Treaty Committees – Retrieve the list of committees associated with a specific treaty.
	- GET /treaty/{congress}/{treatyNumber}/committees

## Known Issues and Limitations
- The API is **read-only**; it does not allow users to submit or modify legislative data.
- Some endpoints may have **rate limits**, which can affect bulk requests.
- Data updates may experience **minor delays**, as they depend on when Congress.gov refreshes its records.
- Some endpoints require **date parameters**, and incorrect formatting may result in errors.

## Frequently Asked Questions

### **1. Can I retrieve past Congress sessions?**
Yes! The API provides access to **historical legislative data**. You can specify a **Congress session number** as a parameter to filter results.

### **2. Does the API include voting records?**
Currently, the Congress.gov API does not provide detailed voting records, but it does include **bill actions** that reference major votes.

### **3. How often is the data updated?**
Congress.gov updates its data **daily**, but the timing may vary depending on legislative activity.

## Deployment Instructions
1. **Import the connector** into Power Automate or Power Apps.
2. **Configure authentication** by entering your API key in the connector settings.
3. **Use the available actions** to fetch legislative data in your workflows.
4. **Test and refine your integration** to ensure it meets your reporting needs.


## Additional Resources

- [Congress.gov API Documentation](https://api.govinfo.gov/docs/)
- [Congress.gov Developer Portal](https://api.congress.gov/)
- [US Congress API Overview](https://www.congress.gov/developers/)
- [US Congress GitHub Repo](https://github.com/LibraryOfCongress/api.congress.gov/)
- API keys and user registration follow the [data.gov privacy policy.](https://data.gov/privacy-policy/)
- API content follows the [Library of Congress privacy policy.](https://www.loc.gov/legal/privacy-policy/)

