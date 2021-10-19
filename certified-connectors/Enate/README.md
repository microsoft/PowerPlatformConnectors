
## Enate

The Enate Connector allows seamless integration with Enate’s enterprise workflow engine. It allows Power Automate Flows to get work from the Enate platform, edit the data, add/download files, update the Enate platform and subscribe to the webhook triggers.  We can also create new work items on the Enate platform using these actions in the connector.

## Pre-requisites

You will need the following to proceed:

- `Access to the Enate Instance which is hosted in cloud`

## Supported Operations

The Enate Connector currently supports the following operations available from Enate Platform:

Enate Platform – Provides a service for work management of Bots and robots. It consists of several APIs that helps in various operations of Enate data. It receives requests and sends responses using HTTPS protocols with body content in JSON format

## Supported Actions
- `Authenticate` : Action which will authenticate to the Enate Platform and returns the token.
- `GetCase` : Action which will get the Case details from the Enate Platform.
- `GetFileTags` : Action which will get the details related to the file tags from the Enate Platform. 
- `GetFileTag` : Action which will get the details related to that file tag from the Enate Platform.
- `GetContactTags` : Action which will get the details related to the contact tags from the Enate Platform. 
- `GetContactTag` : Action which will get the details related to that contact tag from the Enate Platform. 
- `GetAction` : Action which will get the Action details from the Enate Platform. 
- `GetTicket` : Action which will get the Ticket details from the Enate Platform. 
- `GetLaunchableActions` : Action which will get the details related to the launchable actions available in the case from the Enate Platform.
- `CreateSubCase` : Action which will create a sub case in the Enate Platform.
- `GetCommunications` : Action which will get the details of all the communications in the packet from the Enate Platform.
- `GetCommunication` : Action which will get the details of that communication from the Enate Platform.
- `GetMoreWork` : Action which will get the work available in the Enate Platform. 
- `IsWorkAvailable` : Action which will check if work is available in the Enate Platform.
- `RejectWork` : Action which will allows a robot to reject a piece of work so that it can be retrieved by a Human user
- `GetScheduleDetailsOfCase` : Action which will get the schedule details associated with the case attribute from the Enate platform
- `LaunchAdhocAction` : Action which will launch an adhoc action in the Enate platform
- `SearchForContact` : Action which searches for an Employee or External contact with the supplied search criteria.
- `CreatePacket` : Action which will create a new packet but does not start the process
- `SetAssignee` : Action which will set the assignee user for the packet 
- `GetWebhookSubscriptions` : Action which will get a list of all current WebHook subscriptions in the Enate platform
- `GetWebhookSubscription` : Action which will get a specific WebHook subscription by GUID
- `UnsubscribeWebHookSubscription` : Action which will permanently deletes a WebHook subscription
- `UpdateWebhookSubscription` :Action which updates an existing webhook subscription
- `GetServiceLines` : Action which gets a list of service lined available in the Enate platform
- `CreateCase` : Action which creates a case in the Enate platform
- `UpdateCase` : Action which will update the case and all its data supplied
- `CreateTicket`: Action which will create a ticket in the Enate platform
- `GetContexts` : Action which returns a list of Contexts for a Case or Ticket
- `GetServiceMatrix` : Action which returns the service matrix details
- `UpdateTicket` : Action which will update all its ticket details supplied
- `UpdateAction`: Action which will update all the data supplied
- `SendEmailCommunication` : Action which sends an email from a packet
- `AddPacketCommunication` : Action which Writes a new Packet Communication of type note or Self-Service comment to the Packet based on the authenticated user type
- `SetToDoCase` : Action which will set the status of the case to ToDo
- `UpdateChecklist` : Action which will update an individual checklist item on an Action
- `AttachFile` : Action which will attach a file to the packet in the Enate
- `SetToDoTicket` : Action which will set the status of the ticket to ToDo
- `SetToResolvedCase` : Action which resolves a Case where all the Actions and Sub Cases have already completed, but the Case requires manually resolving.
- `SetToResolvedTicket` : Action which will set the ticket status to resolved
- `ActionResolveSuccessfully` : Action which resolves an Action after it has been successfully completed.

## Obtaining Token

The user must use Authenticate action before using any other actions. The Authenticate action will request for the URL, InstanceName, Username and Password for the Enate Instance for authenticating. This will return the AuthToken which can further be used in all other actions.

![image](https://user-images.githubusercontent.com/91666311/137901482-0c24fe20-0fc9-4ed4-bbf6-d29bab0a8885.png)

## Sample Flow

A sample flow which will get the case details from the Enate platform - The HostUrl, InstanceName, Username and password are stored in the variables to provide in the Authenticate action which will return the token that can be used in further actions . The GetCase action takes the Case GUID as an input and returns all the details related to the case.

![image](https://user-images.githubusercontent.com/91666311/137902399-47a5a7d5-fc62-4a30-9885-ae1890fe132c.png)



