# Bluesky API Connector - Non-App.Bsky Endpoints

- **This custom connector allows Power Platform apps to interact with Bluesky’s API, supporting operations for managing actors, moderation, labels, identity, synchronization, and administrative controls outside the `app.bsky` namespace.

## Supported Endpoints

To use this connector, you will need:

- **A Bluesky account.
- **API access to Bluesky with an App Password.

## Obtaining Credentials

- **Go to the [Bluesky Developer Portal](https://docs.bsky.app).
- **Register your App Password in Settings.

- **Steps:
- **Use the CreateSession endpoint to authenticate and retrieve your accessJwt (access token) and refreshJwt (refresh token).	
- **Use the RefreshSession endpoint when your token expires to retrieve a new accessJwt and refreshJwt.

## Supported Operations

- **This connector includes the following operations:

### `chat.bsky` Endpoints

- **chat.bsky.actor.deleteAccount** – Permanently delete an actor's account and associated data.
- **chat.bsky.actor.exportAccountData** – Export account data for a specific actor.
- **chat.bsky.convo.deleteMessageForSelf** – Delete a message from the user's view only.
- **chat.bsky.convo.getConvo** – Retrieve a specific conversation.
- **chat.bsky.convo.getConvoForMembers** – Get a conversation between specific members.
- **chat.bsky.convo.getLog** – Retrieve the log of a conversation.
- **chat.bsky.convo.getMessages** – Retrieve messages within a conversation.
- **chat.bsky.convo.leaveConvo** – Leave a conversation.
- **chat.bsky.convo.listConvos** – List all conversations for a user.
- **chat.bsky.convo.muteConvo** – Mute notifications for a conversation.
- **chat.bsky.convo.sendMessage** – Send a message in a conversation.
- **chat.bsky.convo.sendMessageBatch** – Send multiple messages in a batch.
- **chat.bsky.convo.unmuteConvo** – Unmute notifications for a conversation.
- **chat.bsky.convo.updateRead** – Mark a conversation as read.
- **chat.bsky.moderation.getActorMetadata** – Retrieve metadata associated with an actor's moderation status.
- **chat.bsky.moderation.getMessageContext** – Retrieve context for a message's moderation.
- **chat.bsky.moderation.updateActorAccess** – Update access permissions for a specified actor.

### `com.atproto.admin` Endpoints

- **com.atproto.admin.deleteAccount** – Permanently delete an account.
- **com.atproto.admin.disableAccountInvites** – Disable invites for an account.
- **com.atproto.admin.disableInviteCodes** – Disable invite codes.
- **com.atproto.admin.enableAccountInvites** – Enable invites for an account.
- **com.atproto.admin.getAccountInfo** – Get information about a specific account.
- **com.atproto.admin.getAccountInfos** – Retrieve information about multiple accounts.
- **com.atproto.admin.getInviteCodes** – Retrieve invite codes for an account.
- **com.atproto.admin.getSubjectStatus** – Retrieve status information for a subject.
- **com.atproto.admin.searchAccounts** – Search for accounts.
- **com.atproto.admin.sendEmail** – Send an email to an account.
- **com.atproto.admin.updateAccountEmail** – Update the email address associated with an account.
- **com.atproto.admin.updateAccountHandle** – Update the handle associated with an account.
- **com.atproto.admin.updateAccountPassword** – Update the password for an account.
- **com.atproto.admin.updateSubjectStatus** – Update status for a subject.

### `com.atproto.identity` Endpoints

- **com.atproto.identity.getRecommendedDidCredentials** – Retrieve recommended DID credentials.
- **com.atproto.identity.requestPlcOperationSignature** – Request a signature for a PLC operation.
- **com.atproto.identity.resolveHandle** – Resolve an account handle.
- **com.atproto.identity.signPlcOperation** – Sign a PLC operation.
- **com.atproto.identity.submitPlcOperation** – Submit a PLC operation.
- **com.atproto.identity.updateHandle** – Update the handle for an identity.

### `com.atproto.label` Endpoints

- **com.atproto.label.queryLabels** – Query labels for content or actors.

### `com.atproto.moderation` Endpoints

- **com.atproto.moderation.createReport** – Create a report for moderation.
- **com.atproto.moderation.getActorMetadata** – Retrieve metadata for moderation purposes.

### `com.atproto.repo` Endpoints

- **com.atproto.repo.applyWrites** – Apply writes to a repository.
- **com.atproto.repo.createRecord** – Create a record in a repository.
- **com.atproto.repo.deleteRecord** – Delete a record from a repository.
- **com.atproto.repo.describeRepo** – Describe a repository.
- **com.atproto.repo.getRecord** – Retrieve a record by ID.
- **com.atproto.repo.importRepo** – Import data into a repository.
- **com.atproto.repo.listMissingBlobs** – List missing blobs.
- **com.atproto.repo.listRecords** – List all records in a repository.
- **com.atproto.repo.putRecord** – Update a record in the repository.
- **com.atproto.repo.uploadBlob** – Upload a binary blob to a repository.

### `com.atproto.server` Endpoints

- **com.atproto.server.activateAccount** – Activate an account.
- **com.atproto.server.checkAccountStatus** – Check account status.
- **com.atproto.server.confirmEmail** – Confirm an email address.
- **com.atproto.server.createAccount** – Create a new account.
- **com.atproto.server.createAppPassword** – Create an app-specific password.
- **com.atproto.server.createInviteCode** – Create a single invite code.
- **com.atproto.server.createInviteCodes** – Create multiple invite codes.
- **com.atproto.server.createSession** – Create a session.
- **com.atproto.server.deactivateAccount** – Deactivate an account.
- **com.atproto.server.deleteAccount** – Permanently delete an account.
- **com.atproto.server.deleteSession** – Delete a session.
- **com.atproto.server.describeServer** – Describe server information.
- **com.atproto.server.getAccountInviteCodes** – Retrieve invite codes for an account.
- **com.atproto.server.getServiceAuth** – Retrieve service authentication info.
- **com.atproto.server.getSession** – Retrieve session information.
- **com.atproto.server.listAppPasswords** – List app-specific passwords.
- **com.atproto.server.refreshSession** – Refresh a session.
- **com.atproto.server.requestAccountDelete** – Request deletion of an account.
- **com.atproto.server.requestEmailConfirmation** – Request email confirmation.
- **com.atproto.server.requestEmailUpdate** – Request email update.
- **com.atproto.server.requestPasswordReset** – Request password reset.
- **com.atproto.server.reserveSigningKey** – Reserve a signing key.
- **com.atproto.server.resetPassword** – Reset password.
- **com.atproto.server.revokeAppPassword** – Revoke an app-specific password.
- **com.atproto.server.updateEmail** – Update the email associated with an account.

### `com.atproto.sync` Endpoints

- **com.atproto.sync.getBlob** – Retrieve a blob by ID.
- **com.atproto.sync.getBlocks** – Retrieve blocks by ID.
- **com.atproto.sync.getLatestCommit** – Get the latest commit.
- **com.atproto.sync.getRecord** – Retrieve a record for synchronization.
- **com.atproto.sync.getRepo** – Retrieve repository data for sync.
- **com.atproto.sync.getRepoStatus** – Retrieve repository status.
- **com.atproto.sync.listBlobs** – List blobs in a repository.
- **com.atproto.sync.listRepos** – List repositories for synchronization.
- **com.atproto.sync.notifyOfUpdate** – Notify of an update in the system.
- **com.atproto.sync.requestCrawl** – Request a crawl for synchronization.

### `tools.ozone.communication` Endpoints

- **tools.ozone.communication.createTemplate** – Create a communication template.
- **tools.ozone.communication.deleteTemplate** – Delete a communication template.
- **tools.ozone.communication.listTemplates** – List communication templates.
- **tools.ozone.communication.updateTemplate** – Update a communication template.

### `tools.ozone.moderation` Endpoints

- **tools.ozone.moderation.emitEvent** – Emit a moderation event.
- **tools.ozone.moderation.getEvent** – Retrieve a specific moderation event.
- **tools.ozone.moderation.getRecord** – Retrieve a moderation record.
- **tools.ozone.moderation.getRecords** – List moderation records.
- **tools.ozone.moderation.getRepo** – Retrieve moderation information for a repository.
- **tools.ozone.moderation.getRepos** – Retrieve moderation data for multiple repositories.
- **tools.ozone.moderation.queryEvents** – Query moderation events.
- **tools.ozone.moderation.queryStatuses** – Query moderation statuses.
- **tools.ozone.moderation.searchRepos** – Search repositories for moderation.

### `tools.ozone.server` Endpoints

- **tools.ozone.server.getConfig** – Retrieve server configuration information.

### `tools.ozone.set` Endpoints

- **tools.ozone.set.addValues** – Add values to a set.
- **tools.ozone.set.deleteSet** – Delete a specified set.
- **tools.ozone.set.deleteValues** – Delete specific values from a set.
- **tools.ozone.set.getValues** – Retrieve values from a set.
- **tools.ozone.set.querySets** – Query sets based on filters.
- **tools.ozone.set.upsertSet** – Create or update a set.

### `tools.ozone.signature` Endpoints

- **tools.ozone.signature.findCorrelation** – Find correlations related to a signature.
- **tools.ozone.signature.findRelatedAccounts** – Find accounts related to a signature.
- **tools.ozone.signature.searchAccounts** – Search for accounts by signature.

### `tools.ozone.team` Endpoints

- **tools.ozone.team.addMember** – Add a member to a team.
- **tools.ozone.team.deleteMember** – Remove a member from a team.
- **tools.ozone.team.listMembers** – List members in a team.
- **tools.ozone.team.updateMember** – Update team member information.

All other endpoints not listed can be found here (non-app.bsky): https://docs.bsky.app/docs/category/http-reference

### Authentication

- **The connector uses Bearer Authentication. The accessJwt is added automatically to the Authorization header for API calls after session creation.

#### API Key

- **The connector requires an API Key for additional validation in all requests.
- **Description: The Bluesky API Key is generated from your Bluesky account and must be added during connector setup.
