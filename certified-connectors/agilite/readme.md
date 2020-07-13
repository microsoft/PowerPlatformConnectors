
## Agilite Connector
A certified connector that provides seamless interactions with Agilit-e APIs.


## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with certified connector feature
* A registered [Agilit-e](https://agilite.io) account and API Key
* The Power platform CLI tools


## How to get credentials
<<Instructions on how user can get credentials to test connector in Flow/LogicApps/PowerApps>> 


## API documentation
Click [here](https://docs.agilite.io/reference) to access online API Documentation


## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Flow and PowerApps


## Supported Operations
The connector supports the following operations:
* `executeBPM`: Submits an option in its current workflow step. Depending on the business rules, the record wil be progressed to the next step in the process and return information about the next step to the calling api.
* `getBPMByProfileKey`: Get the full BPM Profile with configuration and options for the process.
* `getBPMRecordState`: Get BPM Record State for one or more records to include the current status, who is responsible for the record, the workflow history and other information to describe the current state and rules for the record.
* `registerBPMRecord`: Register new BPM Record. The record is assigned with a recordID that is used for the record life cycle to identify the record, it's progress, history and status at all time.
* `getKeywordsByProfileKey`: Returns Key/Value pairs of a specific Keyword profile based on the 'profile-key' provided.
* `getKeywordsLabelByValue`: Returns a Keyword Label based on the 'profile-key' and 'value-key' provided.
* `getKeywordProfileKeysByGroup`: Returns all Keyword profile keys based on the 'group-name' provided.
* `getKeywordsValueByLabel`: Returns a Keyword Value based on the 'profile-key' and 'label-key' provided.
* `generateNumber`: Generates a unique number based on 'profile-key' provided.
* `assignRole`: Assign user specified role that is applicable to a specific record. An example of this would be a document reviewer or a leave approver.
* `getAssignedRoles`: Get responsible person(s) for previously assigned role for a specific record.
* `getRole`: et responsible person(s) that are assigned to roles in the Agilit-e admin portal. These roles are not only relevant to specific records or business processes and are more typically used for company wide roles in the organization.
* `executeTemplates`: Processes and returns a Template from Agilit-e based on the 'profile-key' provided.