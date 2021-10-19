Enate 
The Enate Connector allows seamless integration with Enate’s enterprise workflow engine. It allows Power Automate Flows to get work from the Enate platform, edit the data, add/download files, update the Enate platform and subscribe to the webhook triggers.  We can also create new work items on the Enate platform using these actions in the connector.
Prerequisites
You will need the following to proceed:
•	Access to the Enate Instance which is hosted in cloud.
Supported Operations
The Enate Connector currently supports the following operations available from Enate Platform:
•	Enate Platform – Provides a service for work management of Bots and robots. It consists of several APIs that helps in various operations of Enate data. It receives requests and sends responses using HTTPS protocols with body content in JSON format.
•	Supported Actions
o	Authenticate: Action which will authenticate to the Enate Platform and returns the token.
o	GetCase: Action which will get the Case details from the Enate Platform.
o	GetFileTags: Action which will get the details related to the file tags from the Enate Platform. 
o	GetFileTag: Action which will get the details related to that file tag from the Enate Platform.
o	GetContactTags: Action which will get the details related to the contact tags from the Enate Platform. 
o	GetContactTag: Action which will get the details related to that contact tag from the Enate Platform. 
o	GetAction: Action which will get the Action details from the Enate Platform. 
o	GetTicket: Action which will get the Ticket details from the Enate Platform. 
o	GetLaunchableActions: Action which will get the details related to the launchable actions available in the case from the Enate Platform.
o	CreateSubCase: Action which will create a sub case in the Enate Platform.
o	GetCommunications: Action which will get the details of all the communications in the packet from the Enate Platform.
o	GetCommunication: Action which will get the details of that communication from the Enate Platform.
o	GetMoreWork: Action which will get the work available in the Enate Platform. 
o	IsWorkAvailable: Action which will check if work is available in the Enate Platform.
o	RejectWork: Action which will allows a robot to reject a piece of work so that it can be retrieved by a Human user
o	GetScheduleDetailsOfCase: Action which will get the schedule details associated with the case attribute from the Enate platform
o	LaunchAdhocAction: Action which will launch an adhoc action in the Enate platform
o	SearchForContact: Action which searches for an Employee or External contact with the supplied search criteria.
o	CreatePacket: Action which will create a new packet but does not start the process
o	SetAssignee: Action which will set the assignee user for the packet 
o	GetWebhookSubscriptions: Action which will get a list of all current WebHook subscriptions in the Enate platform
o	GetWebhookSubscription: Action which will get a specific WebHook subscription by GUID
o	UnsubscribeWebHookSubscription: Action which will permanently deletes a WebHook subscription
o	UpdateWebhookSubscription:Action which updates an existing webhook subscription
o	GetServiceLines: Action which gets a list of service lined available in the Enate platform
o	CreateCase: Action which creates a case in the Enate platform
o	UpdateCase: Action which will update the case and all its data supplied
o	CreateTicket: Action which will create a ticket in the Enate platform
o	GetContexts: Action which returns a list of Contexts for a Case or Ticket
o	GetServiceMatrix: Action which returns the service matrix details
o	UpdateTicket: Action which will update all its ticket details supplied
o	UpdateAction: Action which will update all the data supplied
o	SendEmailCommunication: Action which sends an email from a packet
o	AddPacketCommunication: Action which Writes a new Packet Communication of type note or Self-Service comment to the Packet based on the authenticated user type
o	SetToDoCase: Action which will set the status of the case to ToDo
o	UpdateChecklist: Action which will update an individual checklist item on an Action
o	AttachFile: Action which will attach a file to the packet in the Enate
o	SetToDoTicket: Action which will set the status of the ticket to ToDo
o	SetToResolvedCase: Action which resolves a Case where all the Actions and Sub Cases have already completed, but the Case requires manually resolving.
o	SetToResolvedTicket: Action which will set the ticket status to resolved
o	ActionResolveSuccessfully: Action which resolves an Action after it has been successfully completed.
Obtaining Token
The user must use Authenticate action before using any other actions. The Authenticate action will request for the URL, InstanceName, Username and Password for the Enate Instance for authenticating. This will return the AuthToken which can further be used in all other actions.
Sample Flow
1.	A sample flow which will get the case details from the Enate platform – the HostUrl, InstanceName, Username and Password which is required for Authenticate action is stored in the variable, the token obtained as an output is also stored in the variable which can be further used for other actions. The GetCase action will take the Case GUID as an input and returns us all the details related to the case.


