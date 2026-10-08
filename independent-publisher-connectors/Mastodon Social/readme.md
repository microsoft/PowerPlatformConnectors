# Mastodon Social (Independent Publisher)
Mastodon is a free, open-source social network server based on ActivityPub. It is designed to be self-hosted and can be used to create a decentralized social network. This API allows you to interact with Mastodon instances and perform various actions such as posting statuses, following users, and managing your account.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You must sign up for an account with [Mastodon Social](https://mastodon.social/).

## Obtaining Credentials
Once logged in to your account, create a new application in the [Development Settings](https://mastodon.social/settings/applications) page. You will need to grant the following scopes to the app: read profile write follow push admin:read admin:write

## Supported Operations
### Register an account
Creates a user and account records. Returns an account access token for the app that initiated the request. The app should save this token for later, and should wait for the user to confirm their account by clicking a link in their email inbox.
### Register an account
Creates a user and account records. Returns an account access token for the app that initiated the request. The app should save this token for later, and should wait for the user to confirm their account by clicking a link in their email inbox.
### Verify account credentials
Test to make sure that the user token works.
### Update account credentials
Update the user's display and preferences.
### Get account
View information about a profile.
### Get account statuses
Statuses posted to the given account.
### Get account followers
Accounts which follow the given account, if network is not hidden by the account owner.
### Get account following
Accounts which the given account is following, if network is not hidden by the account owner.
### Get account featured tags
Tags featured by this account.
### Get account lists
User lists that you have added this account to.
### Get identity proofs
Identity proofs for a given account.
### Follow account
Follow the given account. Can also be used to update whether to show reblogs or enable notifications.
### Unfollow account
Unfollow the given account.
### Block account
Block the given account. Clients should filter statuses from this account if received (e.g. due to a boost in the Home timeline).
### Unblock account
Unblock the given account.
### Mute account
Mute the given account. Clients should filter statuses and notifications from this account, if received (e.g. due to a boost in the Home timeline).
### Unmute account
Unmute the given account.
### Feature account on your profile
Add the given account to the user's featured profiles (featured profiles are currently shown on the user's own public profile).
### Unfeature account from profile
Remove the given account from the user's featured profiles.
### Set account note
Sets a private note on a user.
### Check relationships to other accounts
Find out whether a given account is followed, blocked, muted, etc.
### Search for accounts
Search for matching accounts by username or display name.
### Get bookmarks
Statuses the user has bookmarked.
### Get favourites
Statuses the user has favourited.
### Get account mutes
Accounts the user has muted.
### Get account blocks
Get blocked users.
### Get domain blocks
View domains the user has blocked.
### Delete domain block
Remove a domain block, if it exists in the user's array of blocked domains.
### Create domain block
Block a domain to: hide all public posts from it, hide all notifications from it, remove all followers from it, prevent following new users from it (but does not remove existing follows).
### Get filters
Retrieve a list of filters.
### Create a filter
Creates a new filter.
### Get filter
Get one filter.
### Delete filter
Delete a filter.
### Update filter
Update a filter.
### Report an account
File a report.
### Get follow requests
Pending follows.
### Accept follow
Accept the follow request from the given account.
### Reject follow
Reject the follow request from the given account.
### Get endorsements
Accounts that the user is currently featuring on their profile.
### Get featured tags
View your featured tags.
### Create featured tag
Create a feature tag.
### Unfeature tag
Unfeature a tag.
### Get featured tag suggestions
Shows your 10 most-used tags, with usage history for the past week.
### Get preferences
Get user preferences.
### Get suggestions
Accounts the user has had past positive interactions with, but is not yet following.
### Delete suggestion
Delete user suggestion.
### Post a new status
Publish a new status with the given parameters.
### Get status
Retrieve a status by identifier.
### Delete a status
Deletes an existing status.
### Get parent and child statuses in context
Get parent and child statuses in context.
### Get reblogged by
View who boosted a given status.
### Get favourited by
View who favourited a given status.
### Add status to favourites
Add a status to your favourites list.
### Remove status from favourites
Remove a status from your favourites list.
### Reblog a status
Reshare a status.
### Unreblog a status
Undo a reshare of a status.
### Bookmark a status
Privately bookmark a status.
### Unbookmark a status
Remove a status from your private bookmarks.
### Mute a conversation
Do not receive notifications for the thread that this status is part of. Must be a thread in which you are a participant.
### Unmute a conversation
Unmute notifications for a conversation.
### Pin a status
Feature one of your own public statuses at the top of your profile.
### Unpin a status
Unfeature a status from the top of your profile.
### Create an attachment
Creates an attachment to be used with a new status.
### Get attachment
Get an attachment.
### Update attachment
Update an attachment, before it is attached to a status and posted.
### Get poll
View a poll.
### Vote on poll
Vote on a poll.
### Get scheduled statuses
View scheduled statuses.
### Get scheduled status
View a single scheduled status.
### Delete scheduled status
Cancel a scheduled status.
### View scheduled status
View a single scheduled status.
### View public timeline
View public statuses.
### View hashtag timeline
View public statuses containing the given hashtag.
### View home timeline
View statuses from followed users.
### View list timeline
View statuses in the given list timeline.
### Show conversations
Show conversation.
### Remove conversation
Remove a conversation from your history.
### Mark conversation as read
Mark a conversation as read.
### Get lists
Fetch all lists that the user owns.
### Delete a list
Delete a list.
### Create a new list
Create a new list.
### Change a list
Change the title of a list, or which replies to show.
### Get a specific list
View a specific list.
### Get accounts in list
View accounts in list.
### Delete accounts from list
Remove accounts from the given list.
### Add accounts to list
Add accounts to the given list. Note that the user must be following these accounts.
### Get saved timeline positions
Get saved timeline positions.
### Get saved timeline position
Get saved timeline position.
### View notifications
Notifications concerning the user. This API returns Link headers containing links to the next/previous page. However, the links can also be constructed dynamically using query params and identifier values.
### View notification
View information about a notification with a given identifier.
### Clear notifications
Clear all notifications from the server.
### Dismiss notification
Clear a single notification from the server.
### View push subscription
View the push subscription currently associated with this access token.
### Delete push subscription
Updates the current push subscription. Only the data part can be updated. To change fundamentals, a new subscription must be created instead.
### Add push subscription
Add a Web Push API subscription to receive notifications. Each access token can have one push subscription. If you create a new subscription, the old subscription is deleted.
### Update push subscription
Updates the current push subscription. Only the data part can be updated. To change fundamentals, a new subscription must be created instead.
### Perform a search
Perform a search for content.
### View server information
Obtain general information about the server.
### List of connected domains
Domains that this instance is aware of.
### Weekly activity
Instance activity over the last 3 months, binned weekly.
### Get trending tags
Tags that are being used more frequently within the past week.
### Get directory
List accounts visible in the directory.
### View custom emojis
Returns custom emojis that are available on the server.
### Get accounts
View accounts matching certain criteria for filtering, up to 100 at a time. Pagination may be done with the HTTP Link header in the response.
### View account
View admin-level information about the given account.
### Perform action against account
Perform an action against an account and log this action in the moderation history.
### Approve account
Approve the given local account if it is currently pending approval.
### Reject account
Reject the given local account if it is currently pending approval.
### Enable account
Re-enable a local account whose login is currently disabled.
### Unsilence account
Unsilence a currently silenced account.
### Unsuspend account
Unsuspend a currently suspended account.
### View reports
View all reports. Pagination may be done with HTTP Link header in the response.
### View report
View information about the report with the given identifier.
### Claim report
Claim the handling of this report to yourself.
### Unassign report
Unassign a report so that someone else can claim it.
### Resolve report
Mark a report as resolved with no further action taken.
### Reopen report
Reopen a currently closed report.
### View announcements
See all currently active announcements set by admins.
### Dismiss announcement
Allows a user to mark the announcement as read.
### Delete reaction to announcement
Undo a react emoji to an announcement.
### React to announcement
Allows a user to mark the announcement as read.
### View identity proofs
View identity proofs.
### Get OpenGraph data for URL
Get OpenGraph metadata for a URL.
### Delete profile avatar
Deletes the avatar associated with the user's profile.
### Delete profile header
Deletes the header image associated with the user's profile.

## Known Issues and Limitations
There are no known issues at this time.
