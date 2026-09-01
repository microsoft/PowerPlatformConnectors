# Staffbase connector

With the Staffbase connector for Power Automate you bridge the gap between different tools and systems and include the Staffbase platform in your automated workflows. Leverage the power of automated workflows that include your employee app or intranet to automate processes, run tasks on a schedule, or notify users as events take place.

## Prerequisites

* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A Staffbase license for your organization
* The role of administrator for your organization on the Staffbase platform
* The [API token](https://support.staffbase.com/hc/en-us/articles/360015755691) to configure authentication
* [Information](https://support.staffbase.com/hc/en-us/articles/4404976026386) on which Staffbase infrastructure your application is hosted on

## Obtaining Credentials

For authentication between your Staffbase platform and Power Automate, you need an API token. You can [generate an API token](https://support.staffbase.com/hc/en-us/articles/360015755691) from the Staffbase  Studio.

## Getting Started

Configure the connector with basic authentication and the hosting URL.

1. In Power Automate, navigate to **Connectors**.
2. Search for **Staffbase connector**.
3. Click on the Staffbase connector. A dialog to configure the connector opens.
4. In the **API Token** field, add the API token using the following syntax: `Basic [API-Token]`

> **Note:** You need to add an empty space between `Basic` and the API token.

In the **Host ID** field, enter the identifier for the infrastructure where your Staffbase platform is hosted:
* German infrastructure: **de1**
* International infrastructure: **us1**

For detailed information, visit the [Staffbase Support Portal](https://support.staffbase.com/hc/en-us/articles/360017381759).

## Token Permissions

| Level | Suitable For |
|---|---|
| **Read-only** | Analytics, Tags, Post/Channel/User/Space search operations |
| **Editorial** | Notifications, creating/updating Posts, Pages, Campaigns |
| **Admin** | Users (create/update/delete), Groups, Spaces, CSV Import, File Management, Quick Links, Media (upload/delete) |

## Supported Operations

### Notifications

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Send Notification | POST | /branch/notifications | Important |

### Posts & Channels

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Channels: Get list | GET | /channels | Important |
| Channels: Get branch channels | GET | /branch/channels | Important |
| Channels: Get by ID | GET | /channels/{channelID} | Important |
| Channels: Get posts in channel | GET | /channels/{channelID}/posts | Important |
| Channels: Create post in channel | POST | /channels/{channelID}/posts | Important |
| Posts: Get all | GET | /posts | Important |
| Posts: Create post | POST | /posts | Important |
| Posts: Get by ID | GET | /posts/{pageID} | Important |
| Posts: Update | PUT | /posts/{pageID} | Important |
| Posts: Delete | DELETE | /posts/{pageID} | Advanced |
| Posts: Search | GET | /posts/search | Important |
| Posts: Resend notifications | POST | /posts/{pageID}/notifications | Important |
| Posts: Get acknowledgements | GET | /posts/{pageID}/acknowledgements | Advanced |

### Comments

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Comments: Get all | GET | /comments | Important |

### Media

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Media: Get all | GET | /media | Important |
| Media: Upload | POST | /media | Important |
| Media: Get by ID | GET | /media/{mediumID} | Important |
| Media: Delete | DELETE | /media/{mediumID} | Advanced |
| Media: Replace | PUT | /media/{mediumID} | Important |
| Media: Search | GET | /media/search | Important |
| Media: Publish | POST | /media/publish | Important |

### Users

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Users: Get all | GET | /users | Important |
| Users: Create user | POST | /users | Important |
| Users: Get by ID | GET | /users/{userID} | Important |
| Users: Update | PUT | /users/{userID} | Important |
| Users: Delete | DELETE | /users/{userID} | Advanced |
| Users: Partial update | PATCH | /users/{userID} | Important |
| Users: Search | GET | /users/search | Important |
| Users: Sync | GET | /users/sync | Important |
| Users: Get groups | GET | /users/{userID}/groups | Important |
| Users: Search groups | GET | /users/{userID}/groups/search | Important |
| Users: Get visible groups | GET | /users/{userID}/groups/visible | Important |
| Users: Delete sessions | DELETE | /users/{userID}/sessions | Advanced |
| Users: Send recovery email | POST | /users/{userID}/recovery | Important |

### Pages

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Pages: Get list | GET | /pages | Important |
| Pages: Create | POST | /pages | Important |
| Pages: Sync (external system sync) | GET | /pages/sync | Advanced |
| Pages: Get by ID | GET | /pages/{pageId} | Important |
| Pages: Update | PUT | /pages/{pageId} | Important |
| Pages: Delete | DELETE | /pages/{pageId} | Advanced |

### Groups

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Groups: Get list | GET | /groups | Important |
| Groups: Create | POST | /groups | Important |
| Groups: Search | GET | /groups/search | Important |
| Groups: Get by ID | GET | /groups/{groupId} | Important |
| Groups: Partial update | PATCH | /groups/{groupId} | Important |
| Groups: Delete | DELETE | /groups/{groupId} | Advanced |
| Groups: Search members | GET | /groups/{groupId}/members/search | Important |
| Groups: Add or remove members | PATCH | /groups/{groupId}/members | Important |

### Spaces

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Spaces: Get list | GET | /spaces | Important |
| Spaces: Create | POST | /spaces | Important |
| Spaces: Search | GET | /spaces/search | Important |
| Spaces: Get by ID | GET | /spaces/{spaceId} | Important |
| Spaces: Update | PUT | /spaces/{spaceId} | Important |
| Spaces: Delete | DELETE | /spaces/{spaceId} | Advanced |

### Campaigns

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Campaigns: List | GET | /campaigns | Important |
| Campaigns: Create | POST | /campaigns | Important |
| Campaigns: Get by ID | GET | /campaigns/{campaignId} | Important |
| Campaigns: Update | PUT | /campaigns/{campaignId} | Important |
| Campaigns: Delete | DELETE | /campaigns/{campaignId} | Advanced |
| Campaigns: List references | GET | /campaigns/{campaignId}/references | Important |
| Campaigns: Add reference | POST | /campaigns/{campaignId}/references | Important |
| Campaigns: Delete reference | DELETE | /campaigns/{campaignId}/references/{sourceType}/{sourceId} | Advanced |

### Tags

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Tags: Get user profile tags | GET | /tags | Important |

### Quick Links

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Quick Links: Get all | GET | /branch/quicklinks | Important |
| Quick Links: Create | POST | /branch/quicklinks | Important |
| Quick Links: Get by ID | GET | /branch/quicklinks/{quickLinkId} | Important |
| Quick Links: Update | POST | /branch/quicklinks/{quickLinkId} | Important |
| Quick Links: Delete | DELETE | /branch/quicklinks/{quickLinkId} | Advanced |

### Page Templates

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Templates: Get list | GET | /templates | Important |
| Templates: Create | POST | /templates | Important |
| Templates: Get by ID | GET | /templates/{templateId} | Important |
| Templates: Update | PUT | /templates/{templateId} | Important |
| Templates: Delete | DELETE | /templates/{templateId} | Advanced |

### Installations

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Installations: Search | GET | /installations/search | Important |

### Analytics

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Analytics: Post performance rankings | GET | /branch/analytics/posts/rankings | Advanced |
| Analytics: Post performance timeseries | GET | /branch/analytics/posts/timeseries | Advanced |
| Analytics: Content performance rankings | GET | /branch/analytics/contents/rankings | Advanced |
| Analytics: Chat activity timeseries | GET | /branch/analytics/chats/timeseries | Advanced |
| Analytics: User engagement timeseries | GET | /branch/analytics/v2/users/timeseries | Advanced |

### Email Performance

| Operation | Method | Path | Visibility |
|---|---|---|---|
| Email Performance: List emails | GET | /email-performance/emails | Advanced |
| Email Performance: Get email metadata | GET | /email-performance/emails/{emailId} | Advanced |
| Email Performance: Get click rate | GET | /email-performance/{emailId}/clicks | Advanced |
| Email Performance: Get open rate | GET | /email-performance/{emailId}/opens | Advanced |
| Email Performance: Get recipient count | GET | /email-performance/{emailId}/recipient-count | Advanced |
| Email Performance: Get top clicked links | GET | /email-performance/{emailId}/top-clicked-links | Advanced |
| Email Performance: Get engagement trend | GET | /email-performance/{emailId}/engagement-trend | Advanced |
| Email Performance: Get total activity over time | GET | /email-performance/{emailId}/total-activity-over-time | Advanced |
| Email Performance: Get unique activity over time | GET | /email-performance/{emailId}/unique-activity-over-time | Advanced |
| Email Performance: Get audience groups | GET | /email-performance/{emailId}/user-groups | Advanced |

### CSV Import

| Operation | Method | Path | Visibility |
|---|---|---|---|
| CSV Import: List imports | GET | /users/imports | Important |
| CSV Import: Upload CSV file | POST | /users/imports | Important |
| CSV Import: Get import by ID | GET | /users/imports/{importId} | Important |
| CSV Import: Update import status | PATCH | /users/imports/{importId} | Important |
| CSV Import: Delete import | DELETE | /users/imports/{importId} | Advanced |
| CSV Import: Get processing status | GET | /users/imports/{importId}/status | Important |
| CSV Import: Get configuration | GET | /users/imports/{importId}/config | Advanced |
| CSV Import: Set configuration | PATCH | /users/imports/{importId}/config | Advanced |

### File Management

| Operation | Method | Path | Visibility |
|---|---|---|---|
| File Management: Create collection | POST | /medialibrary/collections | Important |
| File Management: List collections | GET | /medialibrary/collections | Important |
| File Management: List all collections (admin) | GET | /medialibrary/collections/all | Advanced |
| File Management: Get collection | GET | /medialibrary/collections/{collectionId} | Important |
| File Management: Update collection | PUT | /medialibrary/collections/{collectionId} | Important |
| File Management: Delete collection | DELETE | /medialibrary/collections/{collectionId} | Important |
| File Management: Add files to collection | POST | /medialibrary/collections/{collectionId}/entries | Important |
| File Management: List files in collection | GET | /medialibrary/collections/{collectionId}/entries | Important |
| File Management: Remove files from collection | POST | /medialibrary/collections/{collectionId}/entries/delete | Important |
| File Management: Search media files | POST | /medialibrary/entries | Important |
| File Management: Associate file with media library | PUT | /medialibrary/entries/{mediumId} | Important |

## Known Issues and Limitations

* The Staffbase connector does not support all Staffbase API features.
* Search Settings API locale transformation is not supported.
* For more detailed information on Staffbase APIs, visit the [Staffbase Developer Portal](http://developers.staffbase.com).

## Frequently Asked Questions

### What business workflows can be automated using the Staffbase connector?

Many business processes can be automated using the Staffbase connector. For example, notify your employees directly in their employee app or intranet as and when actions take place in other tools. Learn more about such business use cases [here](https://support.staffbase.com/hc/en-us/articles/360017639140).

### Are there tutorials to help me get started with an automated workflow using the Staffbase connector?

Staffbase offers tutorials that use our Forms plugin and the Staffbase connector to automate your facility management. Learn all about how to set it up in this [section](https://support.staffbase.com/hc/en-us/sections/360004870179).

## Deployment Instructions

Run the following commands and follow the prompts:
```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```