# Proposal for WebEx Custom Connector using WebEx API # 

Team 18 from UCL Computer Science propose to build a custom connector for WebEx while working alongside the NHS.

Our aim is to allow doctors to create meetings via PowerApp and send them via SMS or WhatsApp to the patients for remote diagnosis session, which would make the process simpler. 

## Publisher: University College London ##

## Obtaining Credentials
- Go to the App you created and login. 
- [Request an OAuth 2.0 Client Id and Client Secret](https://developer.webex.com/my-apps/new/integration)
- On the WebEx Side, add the redirect URL (https://global.consent.azure-apim.net/redirect) and select scopes.
- On the Power Platform side, add (https://webexapis.com/v1/authorize) as authorization URL and add (https://webexapis.com/v1/access_token) as token URL

## Supported Operations
The connector supports the following operations:

* `Create A Meeting`: Creates a meeting on WebEx
* `Read Meetings`: Read all the meetings on the Webex Account
* `Create A Invitee`: Create a invitee to the meeting and automatically send them an invitation email
