# Mailchimp Marketing Connector for Power Automate

A comprehensive guide to setting up and using the Mailchimp Marketing custom connector for Microsoft Power Automate.

---

## Table of Contents

1. [Overview](#overview)
2. [Setup](#setup)
   - [Prerequisites](#prerequisites)
   - [Get Your Mailchimp API Key](#get-your-mailchimp-api-key)
   - [Install from AppSource](#install-from-appsource)
   - [Create Connection](#create-connection)
   - [Test the Connection](#test-the-connection)
3. [Data Model](#data-model)
   - [Marketing List](#marketing-list)
   - [Marketing List Member](#marketing-list-member)
   - [Campaign](#campaign)
   - [Campaign Activity](#campaign-activity)
   - [Contact](#contact)
   - [Relationships](#relationships)
4. [Key Concepts](#key-concepts)
   - [Audiences vs Lists](#audiences-vs-lists)
   - [Subscriber Hash](#subscriber-hash)
   - [Tags vs Segments](#tags-vs-segments)
   - [Sync Modes](#sync-modes)
   - [Subscription Status](#subscription-status)
   - [Merge Fields](#merge-fields)
   - [Campaign Types](#campaign-types)
5. [Actions Reference](#actions-reference)
   - [Audience Operations](#audience-operations)
   - [Member Operations](#member-operations)
   - [Segment Operations](#segment-operations)
   - [Tag Operations](#tag-operations)
   - [Campaign Operations](#campaign-operations)
   - [Report Operations](#report-operations)
   - [Automation Operations](#automation-operations)
   - [Batch Operations](#batch-operations)
   - [Template Operations](#template-operations)
6. [Pagination](#pagination)
   - [Basic Pattern](#basic-pattern)
   - [Operations Supporting Pagination](#operations-supporting-pagination)
7. [Import Flows](#import-flows)
   - [Import Audiences](#import-audiences)
   - [Import Segments and Tags](#import-segments-and-tags)
   - [Import Campaigns](#import-campaigns)
   - [Import Subscribers](#import-subscribers)
   - [Import Engagement](#import-engagement)
   - [Initial Sync Order](#initial-sync-order)
8. [Sync Flows](#sync-flows)
   - [Sync Marketing List to Mailchimp](#sync-marketing-list-to-mailchimp)
   - [Sync Safety](#sync-safety)
   - [Contact Deletion Cleanup](#contact-deletion-cleanup)
9. [Common Patterns](#common-patterns)
   - [Trigger Customer Journeys](#trigger-customer-journeys)
   - [Bulk Operations with Batch](#bulk-operations-with-batch)
10. [Error Handling](#error-handling)
11. [Best Practices](#best-practices)
12. [Quick Reference](#quick-reference)

---

## Overview

This connector provides comprehensive access to the Mailchimp Marketing API, including:

- **Audience Management** - Create, read, and manage audiences (lists)
- **Member Operations** - Add, update, upsert, and manage subscribers
- **Tag Management** - Apply and remove tags for segmentation
- **Segment Support** - Import and sync segment members
- **Campaign Management** - Create, send, schedule, and manage email campaigns
- **Engagement Tracking** - Get opens, clicks, unsubscribes per campaign
- **Automation Support** - Access classic automation workflows
- **Batch Operations** - Bulk processing for high-volume scenarios
- **Reports & Analytics** - Campaign performance data

### Comparison with Standard Mailchimp Connector

| Feature | Standard Connector | This Connector |
|---------|-------------------|----------------|
| Add/Update Members | ✅ | ✅ |
| Campaign Reports | ❌ | ✅ |
| Open/Click Details | ❌ | ✅ |
| Unsubscribe Tracking | ❌ | ✅ |
| Member Activity | ❌ | ✅ |
| Tag Management | ❌ | ✅ |
| Segment Members | ❌ | ✅ |
| Automations | ❌ | ✅ |
| Batch Operations | ❌ | ✅ |
| Send/Schedule Campaigns | ❌ | ✅ |

---

## Setup

### Prerequisites

- Mailchimp account (Free or any paid plan)
- Microsoft Power Automate license
- Admin access to install solutions (for AppSource install)

### Get Your Mailchimp API Key

1. Log into [mailchimp.com](https://mailchimp.com)
2. Click your **profile icon** in the bottom left corner
3. Navigate to **Account & billing**
4. Click **Extras** → **API keys**
5. Click **Create A Key**
6. Copy the generated API key

**Example API Key Format:**
```
a1b2c3d4e5f6g7h8i9j0k1l2m3n4o5p6-us21
```

The suffix after the dash (`us21`) is your datacenter - the connector extracts this automatically.

### Install from AppSource

1. Go to Microsoft AppSource
2. Search for "RapidStart Mailchimp"
3. Click **Get it now**
4. Select your environment
5. Complete the installation

The solution includes:
- Custom connector
- Dataverse tables (Marketing List, Marketing List Member, Campaign, Campaign Activity)
- Cloud flows
- Model-driven app forms and views

### Create Connection

1. Open Power Automate
2. Go to **My flows** → Open any Mailchimp flow
3. The flow will prompt you to create a connection
4. Enter your Mailchimp API key
5. Click **Create**

The connection is now available for all Mailchimp flows.

### Test the Connection

1. Open the **Import Audiences** flow
2. Click **Run**
3. Check that Marketing List records are created in Dataverse

---

## Data Model

### Marketing List

Represents a Mailchimp audience, tag, or segment in CRM.

| Field | Type | Description |
|-------|------|-------------|
| Name | Text | Display name |
| fw_syncmode | Choice | Tag (307990000), Audience (307990001), Segment (307990002) |
| fw_mailchimpaudienceid | Text | Mailchimp audience ID |
| fw_mailchimptagid | Text | Tag ID (for Tag mode) |
| fw_mailchimpsegmentid | Text | Segment ID (for Segment mode) |
| fw_syncenabled | Yes/No | Controls outbound sync |

### Marketing List Member

Junction table linking Contacts to Marketing Lists.

| Field | Type | Description |
|-------|------|-------------|
| Name | Text | Auto-generated |
| fw_marketinglist | Lookup | Marketing List |
| fw_contact | Lookup | Contact |
| fw_email | Text | Email address |
| fw_subscriberhash | Text | MD5 hash |
| fw_status | Choice | Subscribed, Unsubscribed, Cleaned, Pending, Transactional |
| fw_memberrating | Integer | Mailchimp member rating (1-5) |
| fw_avgopenrate | Decimal | Average open rate |
| fw_avgclickrate | Decimal | Average click rate |
| fw_vip | Yes/No | VIP status |
| fw_signupdate | DateTime | Signup timestamp |
| fw_lastchanged | DateTime | Last changed in Mailchimp |
| fw_lastsynced | DateTime | Last sync timestamp |

### Campaign

Represents a Mailchimp campaign.

| Field | Type | Description |
|-------|------|-------------|
| Name | Text | Campaign title |
| fw_mailchimpcampaignid | Text | Mailchimp campaign ID |
| fw_marketinglist | Lookup | Associated Marketing List |
| fw_subjectline | Text | Email subject |
| fw_status | Choice | Draft, Paused, Scheduled, Sending, Sent |
| fw_sendtime | DateTime | Send timestamp |
| fw_emailssent | Integer | Total emails sent |
| fw_opens | Integer | Total opens |
| fw_clicks | Integer | Total clicks |
| fw_unsubscribes | Integer | Total unsubscribes |
| fw_openrate | Decimal | Open rate |
| fw_clickrate | Decimal | Click rate |

### Campaign Activity

Tracks individual engagement events.

| Field | Type | Description |
|-------|------|-------------|
| Name | Text | Auto-generated |
| fw_campaign | Lookup | Campaign |
| fw_contact | Lookup | Contact |
| fw_activitytype | Choice | Opened (307990000), Clicked (307990001), Unsubscribed (307990002) |
| fw_urlclicked | Text | URL (for clicks) |
| fw_activitydate | DateTime | When activity occurred |

### Contact

Standard Dataverse Contact table with added fields:

| Field | Type | Description |
|-------|------|-------------|
| fw_subscriberhash | Text | MD5 hash of lowercase email |

### Relationships

**Marketing List Member:**
- fw_marketinglist → Parental (deleting list cascades to members)
- fw_contact → Referential (requires cleanup flow)

**Campaign Activity:**
- fw_campaign → Parental (deleting campaign cascades to activities)
- fw_contact → Referential (requires cleanup flow)

**Campaign:**
- fw_marketinglist → Referential (one campaign per audience)

> **Note:** Dataverse allows only one Parental relationship per table. Use the Contact Deletion Cleanup flow to handle orphan records.

---

## Key Concepts

### Audiences vs Lists

Mailchimp uses "Audience" and "List" interchangeably:

| Term | Meaning |
|------|---------|
| Audience | A collection of contacts (subscribers) |
| List | Same as Audience (legacy term) |
| list_id | The unique identifier for an audience |

Most Mailchimp accounts have one main audience with tags/segments for organization.

### Subscriber Hash

Some API operations require a `subscriber_hash` instead of an email address. This is the MD5 hash of the lowercase email.

**Example:**
```
Email: User@Example.com
Lowercase: user@example.com
MD5 Hash: b58996c504c5638798eb6b511e6f49af
```

**Tip:** Use the **UpsertListMember** operation instead - it accepts the email directly and handles the hash internally.

### Tags vs Segments

| Feature | Tags | Segments |
|---------|------|----------|
| Purpose | Manual labels | Dynamic filters |
| Assignment | Via API or UI | Automatic based on criteria |
| Use Case | "VIP", "Event Attendee" | "Opened last 5 campaigns" |
| API Support | Full CRUD | Read members only |
| Member Retrieval | Filter in flow | GetSegmentMembers endpoint |

**Tags:** Use for CRM-driven segmentation. You can apply/remove tags via API and trigger Customer Journeys with "Tag Added" events.

**Segments:** Use for Mailchimp-driven dynamic segmentation. Import segment members to CRM using the dedicated GetSegmentMembers endpoint.

### Sync Modes

Marketing Lists support three sync modes:

| Mode | Value | Use Case |
|------|-------|----------|
| Tag | 307990000 | Static list synced via tag |
| Audience | 307990001 | Full audience sync |
| Segment | 307990002 | Dynamic segment sync |

**Tag Mode:**
- Creates/manages a tag in Mailchimp
- Members are filtered by tag during import
- Adding Contact to Marketing List applies tag in Mailchimp
- Best for CRM-driven segmentation

**Audience Mode:**
- Syncs entire Mailchimp audience
- All members imported regardless of tags
- Best for full audience management

**Segment Mode:**
- Imports members of a Mailchimp segment
- Read-only (segments defined in Mailchimp)
- Best for importing dynamic segments

### Subscription Status

| Status | Description | CRM Value |
|--------|-------------|-----------|
| `subscribed` | Active subscriber, receives emails | 307990000 |
| `unsubscribed` | Opted out, won't receive emails | 307990001 |
| `cleaned` | Hard bounced, removed from sending | 307990002 |
| `pending` | Awaiting double opt-in confirmation | 307990003 |
| `transactional` | Non-marketing, transactional only | 307990004 |

> **Important:** Mailchimp is the master for unsubscribe status. Never re-subscribe contacts from CRM - this violates compliance.

### Merge Fields

Merge fields store subscriber data beyond email. Default fields:

| Field | Tag | Description |
|-------|-----|-------------|
| First Name | FNAME | Subscriber's first name |
| Last Name | LNAME | Subscriber's last name |
| Address | ADDRESS | Mailing address |
| Phone | PHONE | Phone number |

**In API requests:**
```json
{
  "merge_fields": {
    "FNAME": "John",
    "LNAME": "Smith"
  }
}
```

### Campaign Types

| Type | Description |
|------|-------------|
| `regular` | Standard email campaign |
| `plaintext` | Text-only email |
| `absplit` | A/B split test |
| `rss` | RSS-driven campaign |
| `variate` | Multivariate test |

---

## Actions Reference

### Audience Operations

#### GetLists
Retrieves all audiences in the account.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| count | integer | Records to return (default: 100, max: 1000) |
| offset | integer | Records to skip for pagination |

**Response:**
```json
{
  "lists": [
    {
      "id": "1a2b3c4d5e",
      "name": "Newsletter Subscribers",
      "stats": {
        "member_count": 5420,
        "unsubscribe_count": 123,
        "open_rate": 0.42
      }
    }
  ],
  "total_items": 1
}
```

#### GetList
Retrieves a specific audience by ID.

**Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| list_id | string | Yes | The audience ID |

#### CreateList
Creates a new audience.

**When to use:** Rarely - most accounts use one audience with tags.

---

### Member Operations

#### GetListMembers
Retrieves members of an audience.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| list_id | string | The audience ID |
| count | integer | Records to return (max: 1000) |
| offset | integer | Records to skip |
| status | string | Filter: subscribed, unsubscribed, cleaned, pending |
| since_last_changed | string | ISO 8601 date filter |

**Response:**
```json
{
  "members": [
    {
      "id": "abc123",
      "email_address": "john@example.com",
      "status": "subscribed",
      "merge_fields": {
        "FNAME": "John",
        "LNAME": "Smith"
      },
      "stats": {
        "avg_open_rate": 0.45,
        "avg_click_rate": 0.12
      },
      "tags": [
        { "id": 123, "name": "VIP" }
      ]
    }
  ],
  "total_items": 5420
}
```

#### GetListMember
Retrieves a specific member by subscriber hash.

**Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| list_id | string | Yes | The audience ID |
| subscriber_hash | string | Yes | MD5 hash of lowercase email |

#### AddListMember
Adds a new member to an audience.

**Note:** If member already exists, this will return an error. Use **UpsertListMember** instead.

#### UpsertListMember ⭐ RECOMMENDED
Adds a new member or updates existing (upsert).

**Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| list_id | string | Yes | The audience ID |
| subscriber_hash | string | Yes | MD5 hash of lowercase email |
| body | object | Yes | Member details |

**Request Body:**
```json
{
  "email_address": "john@example.com",
  "status_if_new": "subscribed",
  "merge_fields": {
    "FNAME": "John",
    "LNAME": "Smith"
  }
}
```

#### UpdateListMember
Updates an existing member (partial update).

#### DeleteListMember
Archives a member from the audience (soft delete).

#### UpdateMemberTags ⭐ KEY OPERATION
Adds or removes tags from a member.

**Request Body:**
```json
{
  "tags": [
    { "name": "VIP", "status": "active" },
    { "name": "Old Tag", "status": "inactive" }
  ]
}
```

#### GetMemberActivity
Gets activity history for a member.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| list_id | string | The audience ID |
| subscriber_hash | string | MD5 hash of lowercase email |
| count | integer | Records to return |
| offset | integer | Records to skip |

#### GetMemberActivityFeed
Gets detailed activity feed for a member.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| list_id | string | The audience ID |
| subscriber_hash | string | MD5 hash of lowercase email |
| count | integer | Records to return |
| offset | integer | Records to skip |

---

### Segment Operations

#### GetSegments
Retrieves all segments for an audience.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| list_id | string | The audience ID |
| count | integer | Records to return |
| offset | integer | Records to skip |

**Response:**
```json
{
  "segments": [
    {
      "id": "12345",
      "name": "Engaged Subscribers",
      "member_count": 1250,
      "type": "saved",
      "created_at": "2024-01-15T10:00:00+00:00"
    }
  ],
  "total_items": 5
}
```

#### GetSegmentMembers ⭐ NEW
Retrieves members of a specific segment.

**Parameters:**
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| list_id | string | Yes | The audience ID |
| segment_id | string | Yes | The segment ID |
| count | integer | No | Records to return (max: 1000) |
| offset | integer | No | Records to skip |

**Response:**
```json
{
  "members": [
    {
      "id": "abc123",
      "email_address": "john@example.com",
      "status": "subscribed",
      "merge_fields": { "FNAME": "John" }
    }
  ],
  "total_items": 1250
}
```

**When to use:** Import segment members to CRM. Unlike tags, segment membership is not included in GetListMembers response, so this dedicated endpoint is required.

---

### Tag Operations

#### GetListTags
Retrieves all tags for an audience.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| list_id | string | The audience ID |
| count | integer | Records to return |
| offset | integer | Records to skip |

**Response:**
```json
{
  "tags": [
    { "id": 123, "name": "VIP" },
    { "id": 124, "name": "Newsletter" }
  ],
  "total_items": 2
}
```

---

### Campaign Operations

#### GetCampaigns
Retrieves all campaigns.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| count | integer | Records to return |
| offset | integer | Records to skip |
| status | string | Filter: save, paused, schedule, sending, sent |
| since_send_time | string | ISO 8601 date filter |
| list_id | string | Filter by audience |

#### GetCampaign
Retrieves a specific campaign.

#### CreateCampaign
Creates a new campaign.

#### UpdateCampaign
Updates a campaign.

#### DeleteCampaign
Deletes a campaign.

#### GetCampaignContent
Retrieves the HTML/text content of a campaign.

#### SetCampaignContent
Sets the content of a campaign.

#### SendCampaign ⭐
Sends a campaign immediately.

#### ScheduleCampaign
Schedules a campaign for a specific time.

#### SendTestEmail
Sends a test email for a campaign.

---

### Report Operations

#### GetReports
Retrieves reports for all campaigns.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| count | integer | Records to return |
| offset | integer | Records to skip |
| since_send_time | string | ISO 8601 date filter |

#### GetCampaignReport ⭐
Retrieves detailed report for a specific campaign.

#### GetCampaignOpenDetails ⭐
Retrieves who opened a campaign.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| campaign_id | string | The campaign ID |
| count | integer | Records to return |
| offset | integer | Records to skip |

#### GetCampaignClickDetails ⭐
Retrieves which URLs were clicked in a campaign.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| campaign_id | string | The campaign ID |
| count | integer | Records to return |
| offset | integer | Records to skip |

#### GetClickLinkMembers ⭐
Retrieves who clicked a specific link.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| campaign_id | string | The campaign ID |
| link_id | string | The link ID |
| count | integer | Records to return |
| offset | integer | Records to skip |

#### GetCampaignUnsubscribes ⭐
Retrieves who unsubscribed from a campaign.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| campaign_id | string | The campaign ID |
| count | integer | Records to return |
| offset | integer | Records to skip |

#### GetCampaignRecipients
Retrieves the list of recipients for a campaign.

#### GetEmailActivity
Retrieves email activity for a campaign.

---

### Automation Operations

#### GetAutomations
Retrieves all classic automations.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| count | integer | Records to return |
| offset | integer | Records to skip |
| status | string | Filter: save, paused, sending |

**Note:** Returns Classic Automations only, not Customer Journeys.

#### GetAutomation
Retrieves a specific automation.

#### GetAutomationEmails
Retrieves all emails in an automation workflow.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| workflow_id | string | The automation ID |
| count | integer | Records to return |
| offset | integer | Records to skip |

#### GetAutomationEmail
Retrieves a specific email in an automation.

---

### Batch Operations

#### CreateBatch
Creates a batch operation for bulk processing.

**When to use:** High-volume operations (>1000 records).

**Request Body:**
```json
{
  "operations": [
    {
      "method": "PUT",
      "path": "/lists/1a2b3c4d5e/members/hash123",
      "operation_id": "upsert_1",
      "body": "{\"email_address\":\"user@example.com\",\"status_if_new\":\"subscribed\"}"
    }
  ]
}
```

#### GetBatch
Gets the status of a batch operation.

#### GetBatches
Gets all batch operations.

---

### Template Operations

#### GetTemplates
Retrieves all templates.

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| count | integer | Records to return |
| type | string | Filter: user, base, gallery |

---

## Pagination

### Basic Pattern

Use Do Until loop with offset variable for any operation returning >1000 records.

```
Initialize variable: var_Offset (Integer) = 0
Initialize variable: var_HasMore (Boolean) = true

Do Until: var_HasMore equals false
│
├── API Call (count: 1000, offset: var_Offset)
│
├── Apply to each: body/members (or body/lists, etc.)
│   └── [Process each item]
│
└── Condition: length(body/members) less than 1000
    ├── Yes: Set var_HasMore = false
    └── No: Set var_Offset = add(var_Offset, 1000)
```

**Key Points:**
- Set count to 1000 (maximum)
- Check if returned items < 1000 to detect last page
- Reset offset before each new loop (e.g., per Marketing List)

### Operations Supporting Pagination

All these operations support count and offset parameters:

| Operation | Array Property |
|-----------|----------------|
| GetLists | lists |
| GetListMembers | members |
| GetSegmentMembers | members |
| GetSegments | segments |
| GetListTags | tags |
| GetCampaigns | campaigns |
| GetReports | reports |
| GetCampaignRecipients | sent_to |
| GetCampaignOpenDetails | members |
| GetCampaignClickDetails | urls_clicked |
| GetClickLinkMembers | members |
| GetCampaignUnsubscribes | unsubscribes |
| GetEmailActivity | emails |
| GetAutomations | automations |
| GetAutomationEmails | emails |
| GetMemberActivity | activity |
| GetMemberActivityFeed | activity |

---

## Import Flows

### Import Audiences

Imports Mailchimp audiences as Marketing Lists (Audience mode).

```
Manual Trigger
│
├── GetLists (count: 100)
│
└── Apply to each Audience
    │
    ├── List Marketing Lists
    │   Filter: fw_mailchimpaudienceid eq 'audience.id'
    │
    └── Condition: Exists?
        ├── Yes: Update row (Name, stats)
        └── No: Add row
            - Name: audience.name
            - fw_syncmode: 307990001 (Audience)
            - fw_mailchimpaudienceid: audience.id
            - fw_syncenabled: No
```

### Import Segments and Tags

Imports Mailchimp segments and tags as Marketing Lists. Static segments (tags) are imported with Tag mode, saved/dynamic segments with Segment mode.

```
Manual Trigger
│
├── GetLists (count: 100)
│
└── Apply to each Audience
    │
    ├── GetSegments (count: 1000)
    │
    └── Apply to each Segment
        │
        ├── List Marketing Lists
        │   Filter: fw_mailchimpsegmentid eq 'segment.id'
        │
        ├── Condition: type equals 'static'?
        │   │
        │   ├── Yes (Tag):
        │   │   └── Condition: Exists?
        │   │       ├── Yes: Update row
        │   │       └── No: Add row
        │   │           - Name: Tag - segment.name
        │   │           - fw_syncmode: 307990000 (Tag)
        │   │           - fw_mailchimptagid: segment.id
        │   │           - fw_mailchimpsegmentid: segment.id
        │   │           - fw_mailchimpaudienceid: audience.id
        │   │           - fw_syncenabled: No
        │   │
        │   └── No (Segment):
        │       └── Condition: Exists?
        │           ├── Yes: Update row
        │           └── No: Add row
        │               - Name: Segment - segment.name
        │               - fw_syncmode: 307990002 (Segment)
        │               - fw_mailchimpsegmentid: segment.id
        │               - fw_mailchimpaudienceid: audience.id
        │               - fw_syncenabled: No
```

> **Note:** Mailchimp returns tags as static segments in the GetSegments API. This flow handles both, setting the appropriate Sync Mode based on the segment type.

### Import Campaigns

Imports Mailchimp campaigns with pagination.

```
Manual Trigger
│
├── Initialize var_Offset (Integer) = 0
├── Initialize var_HasMore (Boolean) = true
│
├── Do Until: var_HasMore equals false
│   │
│   ├── GetCampaigns (count: 1000, offset: var_Offset)
│   │
│   ├── Apply to each Campaign
│   │   │
│   │   ├── Condition: recipients.list_id not empty?
│   │   │   ├── Yes: Find Marketing List by audience ID
│   │   │   └── No: Set var_MarketingListId = null
│   │   │
│   │   ├── List Campaigns
│   │   │   Filter: fw_mailchimpcampaignid eq 'campaign.id'
│   │   │
│   │   └── Condition: Exists?
│   │       ├── Yes: Update row
│   │       └── No: Add row
│   │           - fw_mailchimpcampaignid: campaign.id
│   │           - fw_marketinglist: var_MarketingListId
│   │           - [other fields]
│   │
│   └── Condition: length(campaigns) < 1000
│       ├── Yes: Set var_HasMore = false
│       └── No: Set var_Offset = add(var_Offset, 1000)
```

### Import Subscribers

Imports Mailchimp subscribers as Contacts and Marketing List Members with support for all three sync modes.

```
Manual Trigger (Marketing List ID)
│
├── Get Marketing List
│
├── Initialize var_SyncMode = fw_syncmode
├── Initialize var_AudienceId = fw_mailchimpaudienceid
├── Initialize var_SegmentId = fw_mailchimpsegmentid
├── Initialize var_Offset (Integer) = 0
├── Initialize var_HasMore (Boolean) = true
├── Initialize var_ProcessedContacts (Array) = []
│
├── Condition: var_AudienceId empty?
│   └── Yes: Terminate
│
├── Do Until: var_HasMore equals false
│   │
│   ├── Condition: var_SyncMode equals Audience (307990001)?
│   │   ├── Yes: GetListMembers (list_id, count: 1000, offset)
│   │   └── No: GetSegmentMembers (list_id, segment_id, count: 1000, offset)
│   │
│   ├── Compose - Members (unify output)
│   │   coalesce(GetListMembers.members, GetSegmentMembers.members, createArray())
│   │
│   ├── Apply to each Member (Concurrency: Off)
│   │   │
│   │   ├── Find or Create Contact
│   │   │   - Match by email (toLower)
│   │   │   - Update: firstname, lastname, subscriberhash
│   │   │   - Set var_ContactId
│   │   │
│   │   ├── Condition: Already Processed?
│   │   │   contains(var_ProcessedContacts, var_ContactId)
│   │   │   ├── Yes: Skip
│   │   │   └── No:
│   │   │       ├── Append var_ContactId to var_ProcessedContacts
│   │   │       ├── Check existing Marketing List Member
│   │   │       │   Filter: contact + marketinglist
│   │   │       └── Condition: Member Exists?
│   │   │           ├── Yes: Update row (status, rating, stats)
│   │   │           └── No: Create row
│   │   │
│   │   └── Update: lastsynced = utcNow()
│   │
│   └── Condition: length(Compose) < 1000
│       ├── Yes: Set var_HasMore = false
│       └── No: Set var_Offset = add(var_Offset, 1000)
```

> **Note:** Tag and Segment modes both use GetSegmentMembers since Mailchimp stores tags as static segments. Only Audience mode uses GetListMembers to retrieve all members.

> **Important:** Set Apply to each concurrency to Off (sequential) to ensure the duplicate check array works correctly.

### Import Engagement

Imports opens, clicks, and unsubscribes for a campaign.

```
Manual Trigger (Campaign ID)
│
├── Get Campaign
│
├── Initialize var_Offset variables for each section
├── Initialize var_HasMore variables for each section
│
├── --- OPENS ---
├── Do Until: var_OpenHasMore equals false
│   ├── GetCampaignOpenDetails (count: 1000, offset: var_OpenOffset)
│   ├── Apply to each Open
│   │   ├── Skip if email empty
│   │   ├── Find Contact by email
│   │   ├── Skip if not found
│   │   ├── Check for duplicate Campaign Activity
│   │   └── Create Campaign Activity (Type: Opened)
│   └── Pagination check
│
├── --- CLICKS ---
├── Do Until: var_ClickHasMore equals false
│   ├── GetCampaignClickDetails (count: 1000, offset: var_ClickOffset)
│   ├── Apply to each URL
│   │   ├── GetClickLinkMembers (with pagination)
│   │   └── Apply to each Member
│   │       ├── Find Contact, check duplicate
│   │       └── Create Campaign Activity (Type: Clicked, URL)
│   └── Pagination check
│
└── --- UNSUBSCRIBES ---
    ├── Do Until: var_UnsubHasMore equals false
    │   ├── GetCampaignUnsubscribes (count: 1000, offset: var_UnsubOffset)
    │   ├── Apply to each Unsubscribe
    │   │   ├── Find Contact, check duplicate
    │   │   ├── Create Campaign Activity (Type: Unsubscribed)
    │   │   └── Update Marketing List Member status
    │   └── Pagination check
```

### Initial Sync Order

For new customers with existing Mailchimp data, run imports in this order:

1. **Import Mailchimp Audiences** → Creates Marketing Lists (Audience mode)
2. **Import Mailchimp Segments and Tags** → Creates Marketing Lists (Tag and Segment modes)
3. **Import Mailchimp Campaigns** → Creates Campaign records
4. **Import Mailchimp Subscribers** → Creates Contacts + Marketing List Members
5. **Import Mailchimp Engagement** → Creates Campaign Activity records

> **Important:** Marketing Lists and Campaigns must exist before importing Subscribers and Engagement.

---

## Sync Flows

### Sync Marketing List to Mailchimp

Pushes CRM Marketing List Members to Mailchimp.

```
Trigger: Manual (or scheduled)
│
├── Get Marketing List
│
├── Condition: Sync Enabled AND has Audience ID?
│   └── No: Terminate
│
├── List Marketing List Members
│   Filter: fw_marketinglist eq 'Marketing List ID'
│
└── Apply to each Member
    │
    ├── Get Contact
    │
    ├── Condition: Has Email AND Subscriber Hash?
    │   └── No: Skip
    │
    ├── Condition: Status equals Subscribed (307990000)?
    │   └── No: Skip (never push non-subscribed)
    │
    ├── UpsertListMember
    │   - list_id: var_AudienceId
    │   - subscriber_hash: Contact's hash
    │   - body:
    │       email_address: Contact email
    │       status_if_new: subscribed
    │       merge_fields:
    │         FNAME: Contact firstname
    │         LNAME: Contact lastname
    │
    ├── Condition: Tag Mode?
    │   └── Yes: UpdateMemberTags
    │       tags: [{ name: var_TagName, status: active }]
    │
    ├── Delay: 1 second (rate limiting)
    │
    └── Update Marketing List Member
        - fw_lastsynced: utcNow()
```

### Sync Safety

**Critical safeguards to prevent compliance violations:**

1. **Sync Enabled Flag**
   - Default: No (disabled)
   - Must be explicitly enabled per Marketing List
   - Prevents accidental sync of import-only lists

2. **Subscribed-Only Sync**
   - Only push contacts with Status = Subscribed
   - Never re-subscribe unsubscribed contacts
   - Mailchimp is master for unsubscribe status

3. **Validate Before Sync**
   - Check Audience ID exists
   - Check email and subscriber hash present
   - Skip invalid records

**Sync Enabled Check Expression:**
```
or(
  empty(outputs('Get_Marketing_List')?['body/fw_mailchimpaudienceid']),
  equals(outputs('Get_Marketing_List')?['body/fw_syncenabled'], false)
)
```

### Contact Deletion Cleanup

Handles orphan Marketing List Members and Campaign Activities when a Contact is deleted.

```
Trigger: When a row is deleted (Contacts)
│
├── Initialize var_ContactId
│   Value: triggerOutputs()?['body/contactid']
│
├── List Marketing List Members
│   Filter: _fw_contact_value eq var_ContactId
│
├── Apply to each Member
│   └── Delete row (Marketing List Member)
│
├── List Campaign Activities
│   Filter: _fw_contact_value eq var_ContactId
│
└── Apply to each Activity
    └── Delete row (Campaign Activity)
```

> **Note:** This flow is required because fw_contact uses Referential relationship (not Parental).

---

## Common Patterns

### Trigger Customer Journeys

Use tags to trigger Mailchimp Customer Journeys from CRM.

**Why Tags?** Customer Journeys have limited API support, but you can trigger them with "Tag Added" as the starting point.

**Setup:**
1. In Mailchimp, create Customer Journey with trigger: "Tag Added: Journey Trigger"
2. In CRM, create Marketing List with Sync Mode = Tag

**Flow:**
```
Trigger: When Contact added to Marketing List

Get Marketing List
  Check Sync Mode = Tag

Calculate subscriber hash

UpsertListMember (ensure contact exists in audience)

UpdateMemberTags
  tags: [{ name: Marketing List name, status: active }]

Result: Contact enters Customer Journey automatically
```

### Bulk Operations with Batch

For syncing 10,000+ contacts efficiently.

```
Get all contacts to sync

Split into chunks of 500

For each chunk:
  Build operations array:
    Loop contacts in chunk:
      Add: {
        method: PUT,
        path: /lists/{list_id}/members/{hash},
        body: JSON member data
      }
  
  CreateBatch with operations array
  Store batch ID

Poll until complete:
  Do Until all batches finished:
    GetBatch for each stored ID
    Check status = finished
    Wait 30 seconds
```

---

## Error Handling

### Common Errors

| Error | Cause | Solution |
|-------|-------|----------|
| 401 Unauthorized | Invalid API key | Check API key is correct |
| 404 Not Found | Invalid list_id or member | Verify IDs exist |
| 400 Bad Request | Invalid email or data | Validate input data |
| Member Exists (400) | Using AddMember for existing | Use UpsertListMember instead |
| Compliance State (400) | Member is cleaned/bounced | Cannot re-subscribe cleaned members |
| Rate Limit (429) | Too many requests | Add delays, use batch operations |

### Power Automate Error Handling

**Scope with Configure Run After:**
```
Scope: Try
  [API operations]
  
Scope: Catch
  Configure run after: Try has failed
  
  Compose: outputs('Failed_Action')
  Log error details
  Increment error counter
```

---

## Best Practices

### Performance

1. **Use pagination** - Always implement Do Until for operations that may exceed 1000 records
2. **Use Batch for bulk** - Don't loop 1000+ individual API calls
3. **Use Upsert over Add** - Handles both new and existing
4. **Add rate limiting** - 1 second delay in loops for large lists
5. **Cache IDs** - Don't call GetLists repeatedly

### Data Integrity

1. **Validate emails** - Check format before API calls
2. **Lowercase emails** - For consistent hashing
3. **Mailchimp owns unsubscribes** - Never re-subscribe from CRM
4. **Track sync dates** - Use fw_lastsynced field
5. **Handle orphans** - Use Contact Deletion Cleanup flow

### Rate Limits

| Limit | Value |
|-------|-------|
| Requests per second | ~10 |
| Batch operations | 500 per batch |
| List results | 1000 max per request |

### Security

1. **Secure API keys** - Use connection, not hardcoded
2. **Sync Enabled flag** - Prevent accidental mass sync
3. **Audit connections** - Review who has access
4. **Monitor usage** - Watch for unusual activity

---

## Quick Reference

### Finding IDs

| To Find | Method |
|---------|--------|
| Audience ID | GetLists → lists[].id |
| Campaign ID | GetCampaigns → campaigns[].id |
| Segment ID | GetSegments → segments[].id |
| Tag ID | GetListTags → tags[].id |
| Link ID | GetCampaignClickDetails → urls_clicked[].id |

### Status Mappings

**Mailchimp → CRM (Import):**
| Mailchimp | CRM Value |
|-----------|-----------|
| subscribed | 307990000 |
| unsubscribed | 307990001 |
| cleaned | 307990002 |
| pending | 307990003 |
| transactional | 307990004 |

**CRM → Mailchimp (Sync):**
```
switch(fw_status,
  307990000, 'subscribed',
  307990001, 'unsubscribed',
  307990002, 'cleaned',
  307990003, 'pending',
  307990004, 'transactional',
  'subscribed'
)
```

### Sync Mode Values

| Mode | Value | Field Used |
|------|-------|------------|
| Tag | 307990000 | fw_mailchimptagid |
| Audience | 307990001 | fw_mailchimpaudienceid |
| Segment | 307990002 | fw_mailchimpsegmentid |

### Lookup Field Expressions

**Setting lookup with variable:**
```
fw_marketinglists(@{variables('var_MarketingListId')})
```

**Setting lookup with null handling:**
```
if(empty(var_MarketingListId), null, concat('fw_marketinglists(', var_MarketingListId, ')'))
```

### Coalesce for Switch Outputs

When using Switch with different API calls:
```
coalesce(
  outputs('Get_audience_members')?['body/members'],
  outputs('Get_segment_members')?['body/members']
)
```

---

## Support

- **Connector Issues:** connect@forceworks.com
- **Mailchimp API Docs:** [mailchimp.com/developer/marketing/api](https://mailchimp.com/developer/marketing/api/)
- **Power Platform Connectors:** [GitHub Repository](https://github.com/microsoft/PowerPlatformConnectors)

---

*Last updated: January 2026*
