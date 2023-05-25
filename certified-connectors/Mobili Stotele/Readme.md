# Mobili stotele
Mobili stotele is a mobile virtual PBX service for Tele2 mobile subscribers. This connector lets users to easily send all the data and events about calls, voicemails, contacts and configuration to other services.

## Publisher: Tele2

## Prerequisites
You will need the following to proceed:
* A phone number with Tele2 Mobili stotele subscription
* An API key, that can be generated at https://mobili-stotele.tele2.lt/integration

## Supported Operations
### Get Contacts by ID
Retreive a single employee object by providing an ID.

### Get Group By ID
Retreive a single employee group object by providing an ID.

### Get Call Recording by ID
Retreive a single call recording by providing an ID.

### Get Call History Item By ID
Retreive a single call history entry by providing an ID.

### Get Voicemail Box By ID
Retreive a single voicemail box object by providing an ID.

### Get Voicemail By ID
Retreive a single voicemail object by providing an ID.

### Get Audio Prompt By ID
Retreive a single audio prompt object by providing an ID.

### Get Voip Endpoint By ID
Retreive a single voip endpoint object by providing an ID.

### Dial
Request the start of new call by sending a request to a device.

### Delete Webhook By ID
Delete a single webhook object by providing an ID.

### Get Contacts
Retrieve and maintain a list of all employees. Gets triggered, when new employee is created.

### Get Groups
Retrieve and maintain a list of all employee groups. Gets triggered, when new employee group is created.

### Get Call Recordings
Retrieve and maintain a list of all call recordings. Gets triggered, when new call recording is created.

### Get call History
Retrieve and maintain a list of all call history items. Gets triggered, when new call is registered in call history.

### Get Voicemail Boxes
Retrieve and maintain a list of all voicemail boxes. Gets triggered, when new voicemail box is created.

### Get voicemails
Retrieve and maintain a list of all voicemails. Gets triggered, when new voicemail is received.

### Get Audio Prompts
Retrieve and maintain a list of all audio prompts. Gets triggered, when new audio prompt is created.

### Get Voip Endpoints
Retrieve and maintain a list of all voip endpoints. Gets triggered, when new voip endpoint is created.

### CallStarted webhook callback
Provides event with data payload about the call when event CallStarted event is received during the start of the call.

### CallConnected webhook callback
Provides event with data payload about the call when event CallConnected event is received during a moment when call is answered.

### CallCompleted webhook callback
Provides event with data payload about the call when event CallCompleted event is received during a moment when call has ended.

### VoicemailCreated webhook callback
Provides event with data payload about the voicemail when event VoicemailCreated event is received during a moment when caller has left a voicemail.

### VoicemailDeleted webhook callback
Provides event with data payload about the voicemail when event VoicemailDeleted event is received during a moment when user has deleted a voicemail message.

### LostCallerAdded webhook callback
Provides event with data payload data about the call when event LostCallerCreated event is received during a moment when call does not get answered and gets registered as lost.

### LostCallerUpdated webhook callback
Provides event with data payload data about the call when event LostCallerUpdated event is received during a moment when another unsuccesful call attempt is received from the same number, so lost call record attempts and last_contact fields get updated with a new number and timestamp.

### LostCallerRemoved webhook callback
Provides event with data payload about the call when event LostCallerDeleted event is received during a moment when lost call gets called back from employee or gets answered later in repeated call from the same caller.

### CallRecorded webhook callback
Provides event with data payload about the call when event CallRecorded event is received during a moment when call recording gets created.

### CallTransfered webhook callback
Provides event with data payload about the call when event CallTransfered event is received during a moment when call gets transfered to a third party either by a blind transfer of attended transfer.

## Obtaining Credentials
Use your API key generated at https://mobili-stotele.tele2.lt/integration as a Bearer token:

```
Bearer OnYSrLTH3i2qiyb873au2LNdAajXjf5I
```


## Getting Started
* Create a new api key
* Add IP ACL as `0.0.0.0/0` as the IP can change
* Add all the neccessary api methods that you're going to be using with Microsoft Power automate

## Known Issues and Limitations
* There can be no more than 20 webhooks created at the same time, so if you run into any issues with triggers, please check how many webhooks are under API -> WEB HOOKS in Mobili stotele panel
* Items are not gathered with `Get <anything>` triggers - please note, that only newly created items are gathered with triggers.
* Any authentication  issues - please make sure to configure API key in following format: `Bearer <api_key>`

## Frequently Asked Questions
### What are the benefits of using this connector?
Unlike a normal scenario, where you would develop yourself a solution that gathers data from an API, this connector already provides the middle step between source (being Mobili stotele API) and destination, where the data is sent. For example, by configuring a flow with `CallConnected` trigger, we can send information about all answered calls directly to OneDrive Excel sheet.

### What are the requirements to start using this connector?
You need to be Tele2 carrier subscriber as well as subscriber of the Mobili stotele service.

### What is the difference between webhook callbacks and regular `Get` operations?
`Get` operations maintain a list of particular type of items, for example calls with `Get call History` and every time gathers only newly created call history items. Webhook callbacks, however, get triggered automatically from Mobili stotele service's side when a new event is received. For example, when a new voicemail message is left, Mobili stotele sends an event to connector with data about the new voicemail message.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
