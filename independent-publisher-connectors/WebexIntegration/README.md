# WebEx Custom Connector # 

Team 18 from UCL Computer Science propose to build a custom connector for WebEx while working alongside the NHS.

Our aim is to allow doctors to create meetings via PowerApp and send them via email or WhatsApp to the patients for remote diagnosis session, which would make the process simpler. 

## Publisher: University College London -- Oscar Hui, Zakariya Fakira, Satbir Virdi, Chaohui Wang ##

## Prerequisites ##
Cisco Webex Developer Account (https://developer.webex.com/)
Cisco Webex User Account (Account generated from Cisco Webex Sandbox Developer Account also work)

## Supported Operations
The connector supports creat a meeting, read meetings and create invitee to Webex meeting.

### Operation 1 `Create a meeting`
This operation creates a meeting on Webex based on the details given from the user.

### Operation 2 `Read meetings`
This operation read all the meetings that is scheduled on the Webex account (including those not generated from this connector)

### Operation 3 `Create an invitee`
This operation allows user to create a new invitee to the meeting, the api would send an invitation email to the invitee to join the meeting.

### Operation 4 `Edit a meeting`
This operation allows user to edit a meeting with its parameters.

### Operation 5 `Delete a meeting`
This operation allows user to delete a meeting.

## Obtaining Credentials
- Go to the App you created and login. 
- [Request an OAuth 2.0 Client Id and Client Secret](https://developer.webex.com/my-apps/new/integration)
- On the WebEx Side, add the redirect URL (https://global.consent.azure-apim.net/redirect) and select scopes.
- On the Power Platform side, add (https://webexapis.com/v1/authorize) as authorization URL and add (https://webexapis.com/v1/access_token) as token URL

## Known Issues and Limitations
N/A
