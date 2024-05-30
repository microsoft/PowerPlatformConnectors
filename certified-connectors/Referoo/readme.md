# Referoo – Fast, simple, secure online reference checks

At Referoo, we know it can be difficult to get references fast. Our online reference checking tool stops you spending time chasing referees, collating references, and crossing your fingers with the hope your references are legit. We provide the option of collecting your references via SMS, email or phone, so you can choose how to contact the referee.

This connector allows you to access the features of your Referoo account and supports actions for managing your checks for candidates and their referees, as well as actions for managing account functions such as questionnaires, brands, packages, and teams.

## Publisher: [Referoo](https://www.referoo.com/)

## Prerequisites

You will need the following to proceed:

* A [Referoo user account](https://www.referoo.com.au/web-app/account/user-login.php) on either our Production application or our Sandbox with an API key and secret
* The proper redirect URL set up with your credentials
* The environment hostname for which your credentials are valid (Production or Sandbox server)

## How to get credentials

[Contact the Referoo team](https://www.referoo.com/support/) if you're in need of credentials – they can set them up for you. Be sure you provide the following Redirect URL when requesting credentials: https://global.consent.azure-apim.net/redirect – This will streamline the process of making a successful connection.

## Get started with our connector

There are two different servers available to connect to:

* api.sandbox.referoo.com.au
* api.referoo.com.au

When you're first provided with API credentials, you'll be able to connect to the sandbox server. Once your flow is ready to go live, contact the [Referoo team](https://www.referoo.com/support/) to request to add your credentials to the live server.

### Making a connection
Ensuring you're using the proper server host, provide the connector with your API Key (Client Id) and Secret to the connector, logging in with your Referoo user account credentials when prompted.

## Supported Operations

### Candidates

* **ListCandidates**  Will return a list of candidates linked to your account in order of creation date. A maximum of 50 can be returned at a time, 20 are returned by default.
* **CreateCandidate** Will create a candidate and add them to Referoo. *The candidate will be contacted immediately if they have 'Automatically send requests when candidate is added' enabled on their account.*
* **GetCandidateById** Will return a JSON object containing a single candidate based on the num provided in the path.
* **UpdateCandidateById** Will update a candidate based on the record num passed in the call. Pass a JSON object containing the variables you wish to update.
* **GetCandidatesRefereesById** Will return a JSON object containing all of the referees a candidate has based on the record num provided in the path.
* **EmailCandidateRequestById** Will send the candidate a reference request email.
* **CandidateCommsLogsId** Will return a JSON object containing comms logs related to a candidate.
* **CandidateActionsLogId** Will return a JSON object containing action logs related to a candidate.
* **GetCandidateChecksById** Will return a JSON object containing check records related to a candidate.

### Referees

* **ListReferees** Will return a JSON object containing all of the referees a candidate has based on the record num provided in the path.
* **CreateReferee** Will create a new referee under the candidate num that was passed in the path.
* **GetRefereeById** Will return a JSON object containing a single referee, based on the ID provided in the path.
* **DeleteRefereeById** Will permanently delete a referee, its log records and reference (if one was created).
* **UpdateRefereeById** Will update a referee based on the ID passed in the call. Pass a JSON object containing the variables you wish to update.
* **CreateRefereeQuick** Will create a new candidate and create a new referee for them with one call. You can add up to 5 referee's per candidate. The referee will also be sent a reference request email or SMS.
* **EmailRefereeRequestById** Will send the referee a reference request email.
* **DownloadReportByRefereeId** Will initiate the download of the referee's reference in PDF format using the ID provided in the path.
* **GetAnswersForRefereeId** Will return the answers provided by a referee in their reference, based on the referee ID provided.
* **GetRefereeChecksById** Will return a JSON object containing check records related to a referee.
* **SendRefereeCommsID** Will update a single referee so that their communications are sent.

### Questionnaires

* **ListQuestionnaires** Will return an array of questionnaires linked to this account in order of creation date. *Only public or published questionnaires will be returned – Draft items are not accessible.*
* **GetQuestionnaireById** Will return a JSON object containing a single candidate, based on the num provided in the request.

### Accounts/User

* **GetLoggedInUserDetails** Will return details of of the user who is currently connected.

***Note:** Only a parent account can access the following actions:*

* **ListAccounts** Will return a list of accounts linked to the recruiters parent account in order of last name then first name.
* **CreateAccount** Will create a new child account under the recruiter using the API.
* **GetAccountById** Will return a JSON object containing a single account based on the ID provided in the request.
* **UpdateAccountById** Will update a child account based on the ID passed in the call. *Pass a JSON object containing the variables you wish to update.*

### Brands

* **ListBrands** Will return an object with the name and other details of brands that the user has access to.

### Packages

* **ListPackages** Will return an array of packages linked to this account.
* **GetPackageById** Will return a single package linked to this account using the ID provided in the request.

### Teams

* **ListTeams** Will return an array of teams linked to this account.
* **GetTeamById** Will return a single team linked to this account using the ID provided in the request.

## Known issues and limitations

Referoo's webhook system is currently incompatible with the Referoo connector – We're working hard to roll out the ability to utilize triggers in your flows (such as being alerted when a candidate has added all of their referees).

*As new features roll out on Referoo, our development team will be rolling out both API and Microsoft Power Automate connector updates.*

## Common errors and remedies

### 401 – Unauthorized

Ensure you are connecting with the credentials provided by the Referoo team as well as connecting to the appropriate server for your credentials.

#### Example
Your credentials are valid for the Sandbox application & API but you've selected the Production hostname as the environment.

### 403 – Not Found

The specified resource was not found. Ensure you double-check the num you provided to the action, it may not exist.

#### Example

The `UpdateCandidateById` action was provided with an invalid numeric identifier (num). Verify using the `ListCandidates` action and filter with the email address of the candidate you expected to update.

### 422 – Unprocessable Content
The request was understood, but there was an issue with it. Check with the returned error for specific details.

#### Example
The `CreateCandidate` action was provided with values for both `number_of_references_required` and `maximum_number_of_references` but the `number_of_references_required` was 3 and the `maximum_number_of_references` was 2. This will return a 422 due to the requirement of the maximum parameter being greater than the number of references.

## FAQ

Refer to [Referoo's API Docs](https://api.referoo.com.au/docs/api) for more details on interacting with our connector. 