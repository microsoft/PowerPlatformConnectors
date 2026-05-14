# Bluesky API Connector

- **This custom connector allows Power Platform apps to interact with Bluesky's API, supporting operations for managing feeds, conversations, notifications, video uploads, and account data.

## Publisher: Dan Romano

## Prerequisites

- **To use this connector, you will need:

    - **A Bluesky account.
    - **API access to Bluesky with App Password.

## Supported Operations

- **This connector includes the following operations:

### Feed Endpoints

- **GET /app.bsky.feed.getTimeline — Retrieve a user's timeline posts.
- **GET /app.bsky.feed.getFeed — Retrieve posts from a specific feed or tag.
- **GET /app.bsky.feed.searchPosts — Search for posts matching a query.
- **GET /app.bsky.feed.getActorFeeds — Retrieve feeds posted by a specific actor.
- **GET /app.bsky.feed.getActorLikes — Retrieve posts liked by a specific actor.
- **GET /app.bsky.feed.getAuthorFeed — Retrieve posts from a specific author's feed.
- **GET /app.bsky.feed.getFeedGenerator — Retrieve a generated feed for a user.
- **GET /app.bsky.feed.getFeedGenerators — Retrieve metadata about all feed generators.
- **GET /app.bsky.feed.getFeedSkeleton — Retrieve a minimal skeletal version of a feed.
- **GET /app.bsky.feed.getListFeed — Retrieve posts from a specific list.
- **GET /app.bsky.feed.getPosts — Retrieve a list of specific posts by their IDs.
- **GET /app.bsky.feed.getPostThread — Retrieve the thread of posts related to a specific post.
- **GET /app.bsky.feed.getLikes — Retrieve users who liked a specific post.
- **GET /app.bsky.feed.getQuotes — Retrieve posts that quote a specific post.
- **GET /app.bsky.feed.getRepostedBy — Retrieve users who reposted a specific post.
- **GET /app.bsky.feed.getSuggestedFeeds — Retrieve suggested feeds for the user.
- **POST /app.bsky.feed.sendInteractions — Send interactions (like or repost) for a post.

### Actor Endpoints

- **GET /app.bsky.actor.getPreferences — Retrieve the user's preferences.
- **GET /app.bsky.actor.getProfile — Retrieve the profile of a specific user.
- **GET /app.bsky.actor.getProfiles — Retrieve multiple user profiles.
- **GET /app.bsky.actor.getSuggestions — Retrieve account suggestions for the user.
- **POST /app.bsky.actor.putPreferences — Update the user's preferences.
- **GET /app.bsky.actor.searchActors — Search for user accounts by query.
- **GET /app.bsky.actor.searchActorsTypeahead — Retrieve search suggestions for user accounts.

### Graph Endpoints

- **GET /app.bsky.graph.getActorStarterPacks — Retrieve starter packs of accounts for a new user.
- **GET /app.bsky.graph.getKnownFollowers — Get followers known to the authenticated user.
- **GET /app.bsky.graph.getFollowers — Retrieve a list of followers for a user.
- **GET /app.bsky.graph.getFollows — Retrieve users followed by a specific user.
- **GET /app.bsky.graph.getBlocks — Retrieve users blocked by the authenticated user.
- **GET /app.bsky.graph.getList — Retrieve a specific list of users.
- **GET /app.bsky.graph.getLists — Retrieve all lists for the authenticated user.
- **GET /app.bsky.graph.getListBlocks — Retrieve users blocked within a specific list.
- **GET /app.bsky.graph.getListMutes — Retrieve muted lists for the authenticated user.
- **GET /app.bsky.graph.getMutes — Retrieve muted users.
- **GET /app.bsky.graph.getRelationships — Retrieve relationships between the authenticated user and specified accounts.
- **GET /app.bsky.graph.getStarterPack — Retrieve a single starter pack for a new user.
- **GET /app.bsky.graph.getStarterPacks — Retrieve all starter packs for new users.
- **GET /app.bsky.graph.getSuggestedFollowsByActor — Retrieve suggested accounts to follow, filtered by an actor.
- **POST /app.bsky.graph.muteActor — Mute a specific actor.
- **POST /app.bsky.graph.unmuteActor — Unmute a specific actor.
- **POST /app.bsky.graph.muteActorList — Mute a specific list of actors.
- **POST /app.bsky.graph.unmuteActorList — Unmute a specific list of actors.
- **POST /app.bsky.graph.muteThread — Mute an entire thread of posts.
- **POST /app.bsky.graph.unmuteThread — Unmute an entire thread of posts.

### Labeler Endpoints

- **GET /app.bsky.labeler.getServices — Retrieve the list of labeler services available to the user.

### Notification Endpoints

- **GET /app.bsky.notification.getUnreadCount — Retrieve the count of unread notifications.
- **GET /app.bsky.notification.listNotifications — List notifications for the authenticated user.
- **POST /app.bsky.notification.putPreferences — Update notification preferences.
- **POST /app.bsky.notification.registerPush — Register for push notifications.
- **POST /app.bsky.notification.updateSeen — Mark notifications as seen.

### Video Endpoints

- **GET /app.bsky.video.getJobStatus — Retrieve the status of a video upload job.
- **GET /app.bsky.video.getUploadLimits — Retrieve the video upload limits for the user.
- **POST /app.bsky.video.uploadVideo — Upload a video file.

### `chat.bsky` Endpoints

- **DELETE /chat.bsky.actor.deleteAccount — Permanently delete an actor's account and associated data.
- **GET /chat.bsky.actor.exportAccountData — Export account data for a specific actor.
- **DELETE /chat.bsky.convo.deleteMessageForSelf — Delete a message from the user's view only.
- **GET /chat.bsky.convo.getConvo — Retrieve a specific conversation.
- **GET /chat.bsky.convo.getConvoForMembers — Get a conversation between specific members.
- **GET /chat.bsky.convo.getLog — Retrieve the log of a conversation.
- **GET /chat.bsky.convo.getMessages — Retrieve messages within a conversation.
- **POST /chat.bsky.convo.leaveConvo — Leave a conversation.
- **GET /chat.bsky.convo.listConvos — List all conversations for a user.
- **POST /chat.bsky.convo.muteConvo — Mute notifications for a conversation.
- **POST /chat.bsky.convo.sendMessage — Send a message in a conversation.
- **POST /chat.bsky.convo.sendMessageBatch — Send multiple messages in a batch.
- **POST /chat.bsky.convo.unmuteConvo — Unmute notifications for a conversation.
- **POST /chat.bsky.convo.updateRead — Mark a conversation as read.
- **GET /chat.bsky.moderation.getActorMetadata — Retrieve metadata associated with an actor's moderation status.
- **GET /chat.bsky.moderation.getMessageContext — Retrieve context for a message's moderation.
- **POST /chat.bsky.moderation.updateActorAccess — Update access permissions for a specified actor.

### `com.atproto.admin` Endpoints

- **DELETE /com.atproto.admin.deleteAccount — Permanently delete an account.
- **POST /com.atproto.admin.disableAccountInvites — Disable invites for an account.
- **POST /com.atproto.admin.disableInviteCodes — Disable invite codes.
- **POST /com.atproto.admin.enableAccountInvites — Enable invites for an account.
- **GET /com.atproto.admin.getAccountInfo — Get information about a specific account.
- **GET /com.atproto.admin.getAccountInfos — Retrieve information about multiple accounts.
- **GET /com.atproto.admin.getInviteCodes — Retrieve invite codes for an account.
- **GET /com.atproto.admin.getSubjectStatus — Retrieve status information for a subject.
- **GET /com.atproto.admin.searchAccounts — Search for accounts.
- **POST /com.atproto.admin.sendEmail — Send an email to an account.
- **POST /com.atproto.admin.updateAccountEmail — Update the email address associated with an account.
- **POST /com.atproto.admin.updateAccountHandle — Update the handle associated with an account.
- **POST /com.atproto.admin.updateAccountPassword — Update the password for an account.
- **POST /com.atproto.admin.updateSubjectStatus — Update status for a subject.

### `com.atproto.identity` Endpoints

- **GET /com.atproto.identity.getRecommendedDidCredentials — Retrieve recommended DID credentials.
- **POST /com.atproto.identity.requestPlcOperationSignature — Request a signature for a PLC operation.
- **GET /com.atproto.identity.resolveHandle — Resolve an account handle.
- **POST /com.atproto.identity.signPlcOperation — Sign a PLC operation.
- **POST /com.atproto.identity.submitPlcOperation — Submit a PLC operation.
- **POST /com.atproto.identity.updateHandle — Update the handle for an identity.

### `com.atproto.label` Endpoints

- **GET /com.atproto.label.queryLabels — Query labels for content or actors.

### `com.atproto.moderation` Endpoints

- **POST /com.atproto.moderation.createReport — Create a report for moderation.
- **GET /com.atproto.moderation.getActorMetadata — Retrieve metadata for moderation purposes.

### `com.atproto.repo` Endpoints

- **POST /com.atproto.repo.applyWrites — Apply writes to a repository.
- **POST /com.atproto.repo.createRecord — Create a record in a repository.
- **DELETE /com.atproto.repo.deleteRecord — Delete a record from a repository.
- **GET /com.atproto.repo.describeRepo — Describe a repository.
- **GET /com.atproto.repo.getRecord — Retrieve a record by ID.
- **POST /com.atproto.repo.importRepo — Import data into a repository.
- **GET /com.atproto.repo.listMissingBlobs — List missing blobs.
- **GET /com.atproto.repo.listRecords — List all records in a repository.
- **POST /com.atproto.repo.putRecord — Update a record in the repository.
- **POST /com.atproto.repo.uploadBlob — Upload a binary blob to a repository.

### `com.atproto.server` Endpoints

- **POST /com.atproto.server.activateAccount — Activate an account.
- **GET /com.atproto.server.checkAccountStatus — Check account status.
- **POST /com.atproto.server.confirmEmail — Confirm an email address.
- **POST /com.atproto.server.createAccount — Create a new account.
- **POST /com.atproto.server.createAppPassword — Create an app-specific password.
- **POST /com.atproto.server.createInviteCode — Create a single invite code.
- **POST /com.atproto.server.createInviteCodes — Create multiple invite codes.
- **POST /com.atproto.server.createSession — Create a session.
- **POST /com.atproto.server.deactivateAccount — Deactivate an account.
- **DELETE /com.atproto.server.deleteAccount — Permanently delete an account.
- **DELETE /com.atproto.server.deleteSession — Delete a session.
- **GET /com.atproto.server.describeServer — Describe server information.
- **GET /com.atproto.server.getAccountInviteCodes — Retrieve invite codes for an account.
- **GET /com.atproto.server.getServiceAuth — Retrieve service authentication info.
- **GET /com.atproto.server.getSession — Retrieve session information.
- **GET /com.atproto.server.listAppPasswords — List app-specific passwords.
- **POST /com.atproto.server.refreshSession — Refresh a session.
- **POST /com.atproto.server.requestAccountDelete — Request deletion of an account.
- **POST /com.atproto.server.requestEmailConfirmation — Request email confirmation.
- **POST /com.atproto.server.requestEmailUpdate — Request email update.
- **POST /com.atproto.server.requestPasswordReset — Request password reset.
- **POST /com.atproto.server.reserveSigningKey — Reserve a signing key.
- **POST /com.atproto.server.resetPassword — Reset password.
- **POST /com.atproto.server.revokeAppPassword — Revoke an app-specific password.
- **POST /com.atproto.server.updateEmail — Update the email associated with an account.

### `com.atproto.sync` Endpoints

- **GET /com.atproto.sync.getBlob — Retrieve a blob by ID.
- **GET /com.atproto.sync.getBlocks — Retrieve blocks by ID.
- **GET /com.atproto.sync.getLatestCommit — Get the latest commit.
- **GET /com.atproto.sync.getRecord — Retrieve a record for synchronization.
- **GET /com.atproto.sync.getRepo — Retrieve repository data for sync.
- **GET /com.atproto.sync.getRepoStatus — Retrieve repository status.
- **GET /com.atproto.sync.listBlobs — List blobs in a repository.
- **GET /com.atproto.sync.listRepos — List repositories for synchronization.
- **POST /com.atproto.sync.notifyOfUpdate — Notify of an update in the system.
- **POST /com.atproto.sync.requestCrawl — Request a crawl for synchronization.

### `tools.ozone.communication` Endpoints

- **POST /tools.ozone.communication.createTemplate — Create a communication template.
- **DELETE /tools.ozone.communication.deleteTemplate — Delete a communication template.
- **GET /tools.ozone.communication.listTemplates — List communication templates.
- **POST /tools.ozone.communication.updateTemplate — Update a communication template.

### `tools.ozone.moderation` Endpoints

- **POST /tools.ozone.moderation.emitEvent — Emit a moderation event.
- **GET /tools.ozone.moderation.getEvent — Retrieve a specific moderation event.
- **GET /tools.ozone.moderation.getRecord — Retrieve a moderation record.
- **GET /tools.ozone.moderation.getRecords — List moderation records.
- **GET /tools.ozone.moderation.getRepo — Retrieve moderation information for a repository.
- **GET /tools.ozone.moderation.getRepos — Retrieve moderation data for multiple repositories.
- **GET /tools.ozone.moderation.queryEvents — Query moderation events.
- **GET /tools.ozone.moderation.queryStatuses — Query moderation statuses.
- **POST /tools.ozone.moderation.searchRepos — Search repositories for moderation.

### `tools.ozone.server` Endpoints

- **GET /tools.ozone.server.getConfig — Retrieve server configuration information.

### `tools.ozone.set` Endpoints

- **POST /tools.ozone.set.addValues — Add values to a set.
- **DELETE /tools.ozone.set.deleteSet — Delete a specified set.
- **DELETE /tools.ozone.set.deleteValues — Delete specific values from a set.
- **GET /tools.ozone.set.getValues — Retrieve values from a set.
- **GET /tools.ozone.set.querySets — Query sets based on filters.
- **POST /tools.ozone.set.upsertSet — Create or update a set.

### `tools.ozone.signature` Endpoints

- **GET /tools.ozone.signature.findCorrelation — Find correlations related to a signature.
- **GET /tools.ozone.signature.findRelatedAccounts — Find accounts related to a signature.
- **GET /tools.ozone.signature.searchAccounts — Search for accounts by signature.

### `tools.ozone.team` Endpoints

- **POST /tools.ozone.team.addMember — Add a member to a team.
- **DELETE /tools.ozone.team.deleteMember — Remove a member from a team.
- **GET /tools.ozone.team.listMembers — List members in a team.
- **POST /tools.ozone.team.updateMember — Update team member information.

### Unspecced Endpoints

- **POST /app.bsky.unspecced.uploadBlob — Upload a binary blob to the user's account.
- **GET /app.bsky.unspecced.getBlob — Retrieve a binary blob from the user's account.


## Obtaining Credentials

- Go to the [Bluesky Developer Portal](https://bsky.app/settings).
- Generate an App Password under "Account Settings."
- Save the App Password for use in connector setup.


## Getting Started

- **Some endpoints require specific parameters (such as actor/user Ids for examples).

    - **Authenticate: Start by authenticating with your Bluesky credentials.
    - **Choose Endpoint: Select the desired endpoint, such as retrieving a feed or listing notifications.
    - **Set Parameters: Provide the required parameters like user IDs, limits, or query strings.
    - **Execute Request: Run the request to retrieve data or perform actions within your Power Platform app.

## Known Issues and Limitations

- **Uses API Token, not OAuth 2.0. [Bluesky docs on OAuth 2.0](https://docs.bsky.app/docs/advanced-guides/oauth-client).

### Frequently Asked Questions

- **Q: How do I generate an App Password?**  
  A: Visit the [Bluesky Developer Portal](https://bsky.app/settings) and generate a new App Password. Use the password to generate a token.

- **Q: What are the API rate limits?**  
  A: Bluesky imposes a rate limit of 100 requests per minute per account.

- **Q: What are some useful terms and definitions, such as actor or DID?**  
  **A:** Useful terms to know:
    - **Actor:** An entity, typically a user, that interacts with the system. This can represent a person, organization, or automated system.
    - **Starter Pack:** A curated set of suggested accounts or content designed to help new users get started on the platform.
    - **Signature:** A cryptographic proof used to verify the authenticity and integrity of a transaction or operation within the system.
    - **PLC (Personal Linked Chain):** A blockchain-like structure used to maintain a verifiable history of account operations (e.g., handle updates, key rotations). It ensures transparency and trustworthiness.
    - **DID (Decentralized Identifier):** A globally unique identifier for an actor that is not tied to a centralized registry. DIDs are foundational to the decentralized nature of Bluesky.


### Support

- **dan.romano@swolcat.com or torin@imp.sh (original owner)

