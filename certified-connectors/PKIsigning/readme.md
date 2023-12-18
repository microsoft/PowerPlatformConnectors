
## PKIsigning Platform Connector
The PKIsigning platform provides an extensive REST API. Using this API, you can manage all requests and their contained documents and requests for signing, approving and downloading requests. 

## Publisher: SBRS B.V.
### PKIsigning is a trademark of SBRS B.V.

## Prerequisites
You will need the following to proceed:
* A paid PKIsigning professional or specialist subscription 

## Supported Operations

### Actions

#### Get actor data by document
List all actors that are linked to a document within a request.

#### Create actor
Create a new actor and add it to a specific document in a request.

#### Get actor data
Retrieve all information on a specific actor of a specific document in a request.

#### Delete actor
Remove a specified actor from a document in a request.

#### Update actor
Update information on a specified actor of a document in a request.

#### Get actor data by request
List all actors that are linked to a request regardless of the document they are linked to.

#### Resend invite
Resends the invite for the current actor in line for a specific request.

#### Withdraw invite
Withdraws the invite for the current actor in line for a specific request.

#### Create document
Add a new document to an existing request.

#### Get document data
Retrieve all information of a specific document in a request.

#### Delete document
Remove a specific document from a request.

#### Update document
Update name and requestname of a specific document.

#### Get workgroups for organisation
Retrieve a list of all workgroups for a specific organisation.

#### Get workgroups for user
Returns a list of all workgroups for a specific user of a specific organisation.

#### Create request
Create a new request without any documents.

#### Get request data
Retrieve all information that is available on a request including all documents and actors.

#### Delete a request
Permanently delete a specific request.

#### Update request
Update data for a specific request.

#### Download request
Downloads a requests and its contents as a .zip file.

#### Send request
Starts the workflow and sends an invite to the first actor in line for a specific request.

### Triggers
Triggers will be sent shortly after the subscribed event occurs.

#### Register a new webhook for this organisation
Register a new trigger that fires upon statusupdates of all dossiers in the specified organisation.

## Obtaining credentials
Please contact [PKIsigning](https://pkisigning.nl) for a subscribtion to our platform. Every user can create flows after signing in with his or her credentials.

## Getting started
### Subscribe to status updates
1. Start with creating a flow that starts with a trigger to be notified of status updates. 
2. If the status value in the notification is `Completed`, use `Download dossier` to obtain the signed documents.

### Invite persons to sign a document
1. Create a new dossier, store the returned ID to keep track of dossiers in the PKIsigning platform and correlation to your own system.
3. Add one or more documents, a document should contain a placeholder text for us to find the location where to create a signature placeholder.
4. Add one or more signers to the documents, every signer should have a unique placeholder text.
5. Start the workflow to send out invites to actors.
6. When all actors have signed, a status update is sent, see `Subscribe to status updates`.

## Known issues and limitations
* Triggers should be created with the value `status` for events parameter.
* Statuses are not reported yet for every signing.

## Deployment instructions
PKIsigning platform connector requires a valid `Client id` and `Client secret` to create a connector.
These properties can be found in the key vault and should only be set in the custom connector's authentication settings.

1. Run:
	```paconn
	paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
	```

2. Login to PowerAutomate
3. Navigate to the new connector
4. Under "Security" update the `Client id` and `Client secret`
5. Update the connector