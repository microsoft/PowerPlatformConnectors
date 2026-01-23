# Mailchimp Marketing (Independent Publisher)

Mailchimp is an email marketing platform that allows businesses to manage audiences, create campaigns, and track engagement. This connector provides comprehensive access to Mailchimp's Marketing API with 40+ operations, enabling advanced automation scenarios including detailed engagement tracking (opens, clicks, unsubscribes), campaign management, audience management, and automation workflows.

## Publisher: Steve Mordue

## Prerequisites

- A Mailchimp account (Free or paid plan)
- A Mailchimp API Key

## Obtaining Credentials

### How to get your API Key:

1. Log into your Mailchimp account at [mailchimp.com](https://mailchimp.com)
2. Click your **profile icon** in the bottom left corner
3. Navigate to **Account & billing**
4. Click **Extras** > **API keys**
5. Click **Create A Key**
6. Copy the generated API key (format: `abc123def456-us21`)

The datacenter (e.g., `us21`) is automatically extracted from your API key - no additional configuration needed.

## Supported Operations

### Audiences (Lists)

| Operation | Description |
|-----------|-------------|
| **GetLists** | Retrieve all audiences in the account |
| **GetList** | Get details for a specific audience |
| **CreateList** | Create a new audience |
| **GetListMembers** | Get members of an audience with pagination |
| **GetListMember** | Get a specific member by email hash |
| **AddListMember** | Add a new member to an audience |
| **UpsertListMember** | Add or update a member (recommended for sync) |
| **UpdateListMember** | Update an existing member |
| **DeleteListMember** | Archive a member from an audience |
| **UpdateMemberTags** | Add or remove tags from a member |
| **GetMemberActivity** | Get activity history for a member |
| **GetMemberActivityFeed** | Get detailed activity feed for a member |
| **GetListTags** | Get all tags for an audience |
| **GetSegments** | Get all segments for an audience |

### Campaigns

| Operation | Description |
|-----------|-------------|
| **GetCampaigns** | Retrieve all campaigns |
| **GetCampaign** | Get details for a specific campaign |
| **CreateCampaign** | Create a new campaign |
| **UpdateCampaign** | Update a campaign |
| **DeleteCampaign** | Delete a campaign |
| **GetCampaignContent** | Get the content of a campaign |
| **SetCampaignContent** | Set the content of a campaign |
| **SendCampaign** | Send a campaign immediately |
| **ScheduleCampaign** | Schedule a campaign for a specific time |
| **SendTestEmail** | Send a test email for a campaign |

### Reports & Engagement Tracking

| Operation | Description |
|-----------|-------------|
| **GetReports** | Get reports for all campaigns |
| **GetCampaignReport** | Get detailed report for a specific campaign |
| **GetCampaignRecipients** | Get list of recipients for a campaign |
| **GetCampaignOpenDetails** | Get who opened a campaign and when |
| **GetCampaignClickDetails** | Get which URLs were clicked |
| **GetClickLinkMembers** | Get who clicked a specific link |
| **GetCampaignUnsubscribes** | Get who unsubscribed and why |
| **GetEmailActivity** | Get detailed email activity for a campaign |

### Automations (Classic)

| Operation | Description |
|-----------|-------------|
| **GetAutomations** | Retrieve all classic automations |
| **GetAutomation** | Get details for a specific automation |
| **GetAutomationEmails** | Get all emails in an automation |
| **GetAutomationEmail** | Get a specific email in an automation |

### Batch Operations

| Operation | Description |
|-----------|-------------|
| **GetBatches** | Get all batch operations |
| **GetBatch** | Get status of a batch operation |
| **CreateBatch** | Create a batch operation for bulk processing |

### Templates

| Operation | Description |
|-----------|-------------|
| **GetTemplates** | Retrieve all templates |

## Comparison with Standard Mailchimp Connector

This connector provides significantly more functionality than the standard Mailchimp connector:

| Feature | Standard | This Connector |
|---------|----------|----------------|
| Add/Update Members | ✅ | ✅ |
| Campaign Reports | ❌ | ✅ |
| Open Details (who opened) | ❌ | ✅ |
| Click Details (which URLs) | ❌ | ✅ |
| Unsubscribe Tracking | ❌ | ✅ |
| Member Activity History | ❌ | ✅ |
| Tag Management | ❌ | ✅ |
| Automations | ❌ | ✅ |
| Batch Operations | ❌ | ✅ |
| Send/Schedule Campaigns | ❌ | ✅ |

## Known Issues and Limitations

1. **Customer Journeys**: The Mailchimp API has limited support for Customer Journeys (the newer automation system). This connector supports Classic Automations which have full API access. To trigger Customer Journeys, use tags - the API can add tags which can trigger Journey entry points.

2. **Rate Limits**: Mailchimp allows approximately 10 API requests per second. For high-volume operations, use the Batch Operations.

3. **Pagination**: Most list operations return a maximum of 1000 records per request. Use the `count` and `offset` parameters to paginate through larger datasets.

4. **Email Hash**: Some member operations require an MD5 hash of the lowercase email address. **Tip:** Use the `UpsertListMember` operation which handles this internally.

5. **Webhook Support**: This connector does not include webhook triggers. Use scheduled flows for polling scenarios.

## Frequently Asked Questions

### Q: Does this work with Mailchimp's free plan?
A: Yes, the API is available on all Mailchimp plans including the free tier.

### Q: How do I calculate the subscriber_hash?
A: The subscriber_hash is the MD5 hash of the lowercase email address. However, we recommend using the `UpsertListMember` operation which doesn't require you to calculate the hash.

### Q: Can I use this with Customer Journeys?
A: Customer Journeys have limited API support. We recommend using tags - add a tag via the `UpdateMemberTags` operation and configure your Customer Journey to trigger when that tag is added.

### Q: What's the difference between this and the standard Mailchimp connector?
A: This connector provides 40+ operations vs the standard connector's ~5, including detailed engagement tracking, automation management, and batch operations.

## Deployment Instructions

1. Import the connector to your Power Platform environment
2. Create a new connection using your Mailchimp API Key
3. Test the connection by using the GetLists operation

## Support

For issues with this connector, please contact: connect@forceworks.com

For Mailchimp API documentation, visit: [mailchimp.com/developer/marketing/api](https://mailchimp.com/developer/marketing/api/)
