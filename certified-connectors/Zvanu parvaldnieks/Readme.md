# Zvanu parvaldnieks

Zvanu Parvaldnieks is a mobile virtual PBX service for LMT mobile subscribers.
This connector allows Zvanu parvaldnieks subscribers to easily connect call and configuration data to other services.

## Authentication 

Use your API key generated at `https://zvanuparvaldnieks.lmt.lv/integration` as a Bearer token:

```
Bearer OnYSrLTH3i2qiyb873au2LNdAajXjf5I
```

## Supported Operations

The connector supports the following operations:
### Actions
* `Get Contacts by ID` - Retreive a single employee object by providing an ID.
* `Get Group By ID` - Retreive a single employee group object by providing an ID.
* `Get Call Recording by ID` - Retreive a single call recording by providing an ID.
* `Get Call History Item By ID` - Retreive a single call history entry by providing an ID.
* `Get Voicemail Box By ID` - Retreive a single voicemail box object by providing an ID.
* `Get Voicemail By ID` - Retreive a single voicemail object by providing an ID.
* `Get Audio Prompt By ID` - Retreive a single audio prompt object by providing an ID.
* `Get Voip Endpoint By ID` - Retreive a single voip endpoint object by providing an ID.
* `Dial` - Request the start of new call by sending a request to a device.
### Triggers
* `Get Contacts` - Retrieve and maintain a list of all employees. Gets triggered, when new employee is created.
* `Get Groups` - Retrieve and maintain a list of all employee groups. Gets triggered, when new employee group is created.
* `Get Call Recordings` - Retrieve and maintain a list of all call recordings. Gets triggered, when new call recording is created.
* `Get call History` - Retrieve and maintain a list of all call history items. Gets triggered, when new call is registered in call history.
* `Get Voicemail Boxes` - Retrieve and maintain a list of all voicemail boxes. Gets triggered, when new voicemail box is created.
* `Get voicemails` - Retrieve and maintain a list of all voicemails. Gets triggered, when new voicemail is received.
* `Get Audio Prompts` - Retrieve and maintain a list of all audio prompts. Gets triggered, when new audio prompt is created.
* `Get Voip Endpoints` - Retrieve and maintain a list of all voip endpoints. Gets triggered, when new voip endpoint is created.
* `CallStarted webhook callback` - Provides event with data payload about the call when event CallStarted event is received during the start of the call.
* `CallConnected webhook callback` - Provides event with data payload about the call when event CallConnected event is received during a moment when call is answered.
* `CallCompleted webhook callback` - Provides event with data payload about the call when event CallCompleted event is received during a moment when call has ended.
* `VoicemailCreated webhook callback` - Provides event with data payload about the voicemail when event VoicemailCreated event is received during a moment when caller has left a voicemail.
* `VoicemailDeleted webhook callback` - Provides event with data payload about the voicemail when event VoicemailDeleted event is received during a moment when user has deleted a voicemail message.
* `LostCallerAdded webhook callback` - Provides event with data payload data about the call when event LostCallerCreated event is received during a moment when call does not get answered and gets registered as lost.
* `LostCallerUpdated webhook callback` - Provides event with data payload data about the call when event LostCallerUpdated event is received during a moment when another unsuccesful call attempt is received from the same number, so lost call record attempts and last_contact fields get updated with a new number and timestamp.
* `LostCallerRemoved webhook callback` - Provides event with data payload about the call when event LostCallerDeleted event is received during a moment when lost call gets called back from employee or gets answered later in repeated call from the same caller.
* `CallRecorded webhook callback` - Provides event with data payload about the call when event CallRecorded event is received during a moment when call recording gets created.
* `CallTransfered webhook callback` - Provides event with data payload about the call when event CallTransfered event is received during a moment when call gets transfered to a third party either by a blind transfer of attended transfer.