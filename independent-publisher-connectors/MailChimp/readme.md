# Mailchimp Marketing (Independent Publisher)

Mailchimp is an email marketing platform that allows businesses to manage audiences, create campaigns, and track engagement. This connector provides comprehensive access to Mailchimp's Marketing API, enabling advanced automation scenarios including campaign reporting, engagement tracking, audience management, and automation workflows.

## Publisher: Steve Mordue

[Steve Mordue](https://github.com/forceworks) | [Forceworks](https://forceworks.com)

## Prerequisites

- A Mailchimp account (Free or paid plan)
- A Mailchimp API Key

## Obtaining Credentials

### How to get your API Key:

1. Log into your Mailchimp account at [mailchimp.com](https://mailchimp.com)
2. Click your profile icon in the bottom left corner
3. Navigate to **Account & billing**
4. Click **Extras** > **API keys**
5. Click **Create A Key**
6. Copy the generated API key

### How to find your Datacenter:

Your datacenter is the suffix at the end of your API key. For example, if your API key is `abc123def456-us21`, your datacenter is `us21`.

## Supported Operations

### Audiences (Lists)

| Operation | Description |
|-----------|-------------|
| **GetLists** | Retrieve all audiences in the account |
| **GetList** | Get details for a specific audience |
| **CreateList** | Create a new audience |
| **GetListMembers** | Get members of an audience |
| **GetListMember** | Get a specific member by email hash |
| **AddListMember** | Add a new member to an audience |
| **UpsertListMember** | Add or update a member (upsert) |
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

### Reports & Analytics

| Operation | Description |
|-----------|-------------|
| **GetReports** | Get reports for all campaigns |
| **GetCampaignReport** | Get detailed report for a specific campaign |
| **GetCampaignRecipients** | Get list of recipients for a campaign |
| **GetCampaignOpenDetails** | Get detailed open information (who opened) |
| **GetCampaignClickDetails** | Get click information (which URLs clicked) |
| **GetClickLinkMembers** | Get members who clicked a specific link |
| **GetCampaignUnsubscribes** | Get unsubscribe information for a campaign |
| **GetEmailActivity** | Get email activity for a campaign |

### Automations (Classic)

| Operation | Description |
|-----------|-------------|
| **GetAutomations** | Retrieve all automations |
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

## Known Issues and Limitations

1. **Customer Journeys**: The Mailchimp API has limited support for Customer Journeys (the newer automation system). This connector supports Classic Automations which have full API access.

2. **Rate Limits**: Mailchimp allows approximately 10 API requests per second. For high-volume operations, use the Batch Operations.

3. **Pagination**: Most list operations return a maximum of 1000 records. Use the `offset` parameter to paginate through larger datasets.

4. **Email Hash**: Some member operations require an MD5 hash of the lowercase email address as the subscriber_hash parameter.

5. **Webhook Support**: This connector does not include webhook triggers. Use polling-based triggers in Power Automate for real-time scenarios.

## Frequently Asked Questions

### Q: Does this work with Mailchimp's free plan?
A: Yes, the API is available on all Mailchimp plans including the free tier.

### Q: How do I calculate the subscriber_hash?
A: The subscriber_hash is the MD5 hash of the lowercase email address. In Power Automate, you can use an expression or a Compose action to generate this.

### Q: Can I use this with Customer Journeys?
A: Customer Journeys have limited API support. For automation triggering, we recommend using tags - add a tag via the API and configure your Customer Journey to trigger when that tag is added.

### Q: What's the difference between this and the standard Mailchimp connector?
A: This connector provides significantly more operations including campaign reports, engagement tracking (opens, clicks, unsubscribes), automation management, member activity history, batch operations, and more.

## Deployment Instructions

1. Import the connector to your Power Platform environment
2. Create a new connection using your Mailchimp API Key and Datacenter
3. Test the connection by using the GetLists operation

## Support

For issues with this connector, please contact: steve@forceworks.com

For Mailchimp API documentation, visit: https://mailchimp.com/developer/marketing/api/
