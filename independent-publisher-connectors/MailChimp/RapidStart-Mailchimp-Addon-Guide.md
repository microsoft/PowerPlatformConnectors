# RapidStart CRM Mailchimp Addon

Complete Mailchimp integration for RapidStart CRM. Import audiences, sync contacts, and track campaign engagement—all from within your CRM.

---

## Table of Contents

1. [Overview](#overview)
2. [Installation](#installation)
3. [Initial Setup](#initial-setup)
   - [Create Mailchimp Connection](#create-mailchimp-connection)
   - [Run Initial Import](#run-initial-import)
4. [Data Model](#data-model)
   - [Marketing List](#marketing-list)
   - [Marketing List Member](#marketing-list-member)
   - [Campaign](#campaign)
   - [Campaign Activity](#campaign-activity)
5. [Sync Modes](#sync-modes)
   - [Audience Mode](#audience-mode)
   - [Tag Mode](#tag-mode)
   - [Segment Mode](#segment-mode)
6. [Included Flows](#included-flows)
   - [Import Flows](#import-flows)
   - [Sync Flows](#sync-flows)
   - [Utility Flows](#utility-flows)
7. [Using the Integration](#using-the-integration)
   - [View Marketing Lists](#view-marketing-lists)
   - [View Subscribers](#view-subscribers)
   - [View Campaigns](#view-campaigns)
   - [View Engagement](#view-engagement)
   - [Sync Contacts to Mailchimp](#sync-contacts-to-mailchimp)
8. [Configuration](#configuration)
   - [Enable Outbound Sync](#enable-outbound-sync)
   - [Schedule Automated Imports](#schedule-automated-imports)
   - [Customize Field Mappings](#customize-field-mappings)
9. [Triggering Customer Journeys](#triggering-customer-journeys)
10. [Troubleshooting](#troubleshooting)
11. [Support](#support)

---

## Overview

The RapidStart CRM Mailchimp Addon provides bi-directional synchronization between your CRM and Mailchimp:

**Import from Mailchimp:**

- Audiences, Tags, and Segments as Marketing Lists
- Subscribers as Contacts and Marketing List Members
- Campaigns with performance metrics
- Engagement data (opens, clicks, unsubscribes)

**Sync to Mailchimp:**

- Push CRM contacts to Mailchimp audiences
- Apply tags to trigger Customer Journeys
- Keep contact data synchronized

**What's Included:**

- Custom Mailchimp connector
- 4 Dataverse tables
- 8 pre-built Cloud Flows
- Model-driven app forms and views

---

## Installation

### Prerequisites

- RapidStart CRM installed
- Mailchimp account (Free or paid)
- Power Automate license (included with most Microsoft 365 plans)
- Environment admin access

### Install from AppSource

1. Go to [Microsoft AppSource](https://appsource.microsoft.com)
2. Search for **"RapidStart Mailchimp"**
3. Click **Get it now**
4. Sign in with your Microsoft 365 account
5. Select your environment
6. Review permissions and click **Install**

Installation takes 5-10 minutes. You'll receive an email when complete.

### Verify Installation

1. Open [make.powerapps.com](https://make.powerapps.com)
2. Select your environment
3. Go to **Solutions**
4. Confirm **RapidStart Mailchimp** appears in the list
5. Go to **Flows** and verify 8 Mailchimp flows are present

---

## Initial Setup

### Create Mailchimp Connection

**Get your Mailchimp API Key:**

1. Log into [mailchimp.com](https://mailchimp.com)
2. Click your **profile icon** (bottom left)
3. Go to **Account & billing**
4. Click **Extras** → **API keys**
5. Click **Create A Key**
6. Copy the key (format: `abc123def456-us21`)

**Create the connection:**

1. Open [make.powerautomate.com](https://make.powerautomate.com)
2. Go to **Connections**
3. Click **+ New connection**
4. Search for **Mailchimp Marketing**
5. Enter your API key
6. Click **Create**

### Run Initial Import

Run the import flows in this order:

| Step | Flow                               | Creates                                 |
| ---- | ---------------------------------- | --------------------------------------- |
| 1    | Import Mailchimp Audiences         | Marketing Lists (Audience mode)         |
| 2    | Import Mailchimp Segments and Tags | Marketing Lists (Tag and Segment modes) |
| 3    | Import Mailchimp Campaigns         | Campaign records                        |
| 4    | Update Subscribers                 | Contacts + Marketing List Members       |
| 5    | Import Mailchimp Engagement        | Campaign Activity records               |

**To run flows 1-3 (Import Audiences, Segments and Tags, Campaigns):**

1. Go to [make.powerautomate.com](https://make.powerautomate.com)
2. Navigate to **My flows**
3. Find the flow (e.g., "Import Mailchimp Audiences")
4. Click the flow name to open it
5. Click **Run** → **Run flow**
6. Wait for completion before running the next flow

**To run flow 4 (Update Subscribers):**

1. Open **RapidStart CRM**
2. Navigate to **Marketing** → **Marketing Lists**
3. Either:
   - Open a Marketing List record and click **Flow** → **Update Subscribers**, OR
   - Select multiple Marketing Lists in the list view and click **Flow** → **Update Subscribers** to run for all selected
4. The flow runs once per selected record

**To run flow 5 (Import Mailchimp Engagement):**

1. Open **RapidStart CRM**
2. Navigate to **Marketing** → **Campaigns**
3. Either:
   - Open a Campaign record and click **Flow** → **Import Mailchimp Engagement**, OR
   - Select multiple Campaigns in the list view and click **Flow** → **Import Mailchimp Engagement** to run for all selected
4. The flow runs once per selected record

> **Note:** Import Subscribers and Import Engagement run from records because they need to know which Marketing List or Campaign to import for.

---

## Data Model

### Marketing List

Represents a Mailchimp audience, tag, or segment.

| Field                 | Description                               |
| --------------------- | ----------------------------------------- |
| Name                  | Display name                              |
| Sync Mode             | Tag, Audience, or Segment                 |
| Mailchimp Audience ID | Links to Mailchimp audience               |
| Mailchimp Tag ID      | Links to Mailchimp tag (Tag mode)         |
| Mailchimp Segment ID  | Links to Mailchimp segment (Segment mode) |
| Sync Enabled          | Controls outbound sync to Mailchimp       |
| Member Count          | Number of members                         |

### Marketing List Member

Links Contacts to Marketing Lists (junction table).

| Field           | Description                                |
| --------------- | ------------------------------------------ |
| Marketing List  | Parent Marketing List                      |
| Contact         | Linked Contact                             |
| Email           | Email address                              |
| Subscriber Hash | MD5 hash for Mailchimp API                 |
| Status          | Subscribed, Unsubscribed, Cleaned, Pending |
| Member Rating   | Mailchimp rating (1-5 stars)               |
| Avg Open Rate   | Historical open rate                       |
| Avg Click Rate  | Historical click rate                      |
| VIP             | VIP status in Mailchimp                    |
| Last Synced     | Last sync timestamp                        |

### Campaign

Represents a Mailchimp email campaign.

| Field          | Description                     |
| -------------- | ------------------------------- |
| Name           | Campaign title                  |
| Marketing List | Associated audience             |
| Subject Line   | Email subject                   |
| Status         | Draft, Scheduled, Sending, Sent |
| Send Time      | When the campaign was sent      |
| Emails Sent    | Total recipients                |
| Opens          | Total opens                     |
| Clicks         | Total clicks                    |
| Unsubscribes   | Total unsubscribes              |
| Open Rate      | Percentage opened               |
| Click Rate     | Percentage clicked              |

### Campaign Activity

Individual engagement events.

| Field         | Description                      |
| ------------- | -------------------------------- |
| Campaign      | Parent Campaign                  |
| Contact       | Who engaged                      |
| Activity Type | Opened, Clicked, or Unsubscribed |
| URL Clicked   | Link URL (for clicks)            |
| Activity Date | When it happened                 |

---

## Sync Modes

Marketing Lists support three sync modes, each serving different use cases.

### Audience Mode

Syncs the entire Mailchimp audience.

**Use when:**

- You want all subscribers in CRM
- You have a single primary audience
- You don't need tag-based segmentation

**How it works:**

- Import: All audience members are imported
- Sync: CRM contacts are added to the audience

**Created by:** Import Mailchimp Audiences flow

### Tag Mode

Syncs contacts with a specific Mailchimp tag.

**Use when:**

- You want CRM-driven segmentation
- You need to trigger Customer Journeys
- You have multiple segments within one audience

**How it works:**

- Import: Only members with the tag are imported
- Sync: Contacts are added to audience AND tag is applied

**Created by:** Import Mailchimp Segments and Tags flow (for static segments/tags)

**Example:** Create a "VIP Customers" tag in Mailchimp. Import creates a Marketing List with Tag mode. Adding a Contact to this list applies the VIP tag in Mailchimp.

### Segment Mode

Imports members of a Mailchimp segment (read-only).

**Use when:**

- You want to import dynamic Mailchimp segments
- Segments are defined by Mailchimp criteria
- You don't need to push changes back

**How it works:**

- Import: Segment members are imported
- Sync: Not applicable (segments are Mailchimp-managed)

**Created by:** Import Mailchimp Segments and Tags flow (for saved/dynamic segments)

**Example:** Mailchimp has a segment "Opened last 5 campaigns". Import brings these engaged contacts into CRM for sales follow-up.

---

## Included Flows

### Import Flows

| Flow                               | Purpose                                        | Trigger                     |
| ---------------------------------- | ---------------------------------------------- | --------------------------- |
| Import Mailchimp Audiences         | Creates Marketing Lists from audiences         | Manual                      |
| Import Mailchimp Segments and Tags | Creates Marketing Lists from segments and tags | Manual                      |
| Import Mailchimp Campaigns         | Creates Campaign records                       | Manual                      |
| Update Subscribers                 | Creates Contacts and Marketing List Members    | Manual (per Marketing List) |
| Import Mailchimp Engagement        | Creates Campaign Activity records              | Manual (per Campaign)       |

### Sync Flows

| Flow                             | Purpose                          | Trigger                     |
| -------------------------------- | -------------------------------- | --------------------------- |
| Sync Marketing List to Mailchimp | Pushes CRM contacts to Mailchimp | Manual (per Marketing List) |

### Utility Flows

| Flow                    | Purpose                                     | Trigger   |
| ----------------------- | ------------------------------------------- | --------- |
| Contact Deleted Cleanup | Removes orphan records when Contact deleted | Automatic |

---

## Using the Integration

### View Marketing Lists

1. Open **RapidStart CRM**
2. Navigate to **Marketing** → **Marketing Lists**
3. Views available:
   - **All Marketing Lists** - Everything
   - **Audience Lists** - Audience mode only
   - **Tag Lists** - Tag mode only
   - **Segment Lists** - Segment mode only
   - **Sync Enabled** - Lists configured for outbound sync

### View Subscribers

**From a Marketing List:**

1. Open the Marketing List record
2. Click the **Members** tab
3. See all Marketing List Members with status, rating, and engagement stats

**From a Contact:**

1. Open the Contact record
2. Click the **Marketing** tab
3. See all Marketing Lists the contact belongs to

### View Campaigns

1. Navigate to **Marketing** → **Campaigns**
2. Open a Campaign to see:
   - Performance metrics (opens, clicks, unsubscribes)
   - Associated Marketing List
   - Campaign Activity timeline

### View Engagement

**From a Campaign:**

1. Open the Campaign record
2. Click the **Activity** tab
3. See all opens, clicks, and unsubscribes

**From a Contact:**

1. Open the Contact record
2. Click the **Marketing** tab
3. See Campaign Activities showing their engagement history

### Sync Contacts to Mailchimp

**Prerequisites:**

- Marketing List has **Sync Enabled** = Yes
- Marketing List has a valid **Mailchimp Audience ID**
- Contact has an email address

**To sync:**

1. Add Contact(s) to the Marketing List
2. Open the Marketing List record
3. Click **Run Flow** → **Mailchimp - Sync Marketing List**
4. Confirm and wait for completion

**What happens:**

- Contact is added/updated in Mailchimp audience
- If Tag mode: tag is applied to the contact
- Marketing List Member's **Last Synced** is updated

---

## Configuration

### Enable Outbound Sync

By default, Marketing Lists are import-only. To enable syncing CRM contacts to Mailchimp:

1. Open the Marketing List
2. Set **Sync Enabled** = Yes
3. Save

> **Important:** Only enable sync for lists you want to push to Mailchimp. This prevents accidentally syncing test data or import-only lists.

### Schedule Automated Imports

To keep data fresh, schedule imports to run automatically:

1. Copy the flow (e.g., **Mailchimp - Import Campaigns**)
2. Click **Edit**
3. Delete the Manual trigger
4. Add **Recurrence** trigger:
   - Interval: 1
   - Frequency: Day
   - At: 6:00 AM
5. Save

**Recommended schedule:**

| Flow               | Frequency |
| ------------------ | --------- |
| Import Audiences   | Weekly    |
| Import Tags        | Weekly    |
| Import Segments    | Weekly    |
| Import Campaigns   | Daily     |
| Import Subscribers | Daily     |
| Import Engagement  | Daily     |

### Customize Field Mappings

To sync additional fields between CRM and Mailchimp:

**Import (Mailchimp → CRM):**

1. Open the **Update Subscribers** flow
2. Find the **Create Contact** or **Update Contact** action
3. Add field mappings from the Mailchimp member response:
   - `merge_fields/PHONE` → Business Phone
   - `merge_fields/COMPANY` → Company Name
   - Custom merge fields as needed

**Sync (CRM → Mailchimp):**

1. Open the **Sync Marketing List** flow
2. Find the **UpsertListMember** action
3. Add merge_fields to the body:
   
   ```json
   {
   "email_address": "@{contact.emailaddress1}",
   "status_if_new": "subscribed",
   "merge_fields": {
    "FNAME": "@{contact.firstname}",
    "LNAME": "@{contact.lastname}",
    "PHONE": "@{contact.telephone1}",
    "COMPANY": "@{contact.companyname}"
   }
   }
   ```

> **Note:** Merge field tags (FNAME, PHONE, etc.) must match your Mailchimp audience settings.

---

## Triggering Customer Journeys

Use Tag mode Marketing Lists to trigger Mailchimp Customer Journeys from CRM.

### Setup in Mailchimp

1. Go to **Automations** → **Customer Journeys**
2. Create a new journey
3. Set starting point: **Tag added**
4. Select or create a tag (e.g., "CRM - New Lead")
5. Build your journey steps
6. Activate the journey

### Setup in CRM

1. Run **Import Tags** to create the Marketing List
2. Or manually create a Marketing List:
   - Sync Mode: Tag
   - Mailchimp Audience ID: Your audience
   - Mailchimp Tag ID: The tag ID
   - Sync Enabled: Yes

### Trigger the Journey

1. Add a Contact to the Marketing List
2. Run **Sync Marketing List** flow
3. Contact enters the Customer Journey in Mailchimp

**Automation options:**

- Create a flow that adds Contacts to Marketing Lists based on criteria
- Trigger sync when a Contact reaches a certain lead score
- Sync when a Contact's status changes

---

## Troubleshooting

### Flow Fails with "Unauthorized"

**Cause:** Invalid or expired API key

**Solution:**

1. Go to **Connections**
2. Find the Mailchimp connection
3. Click **...** → **Delete**
4. Create a new connection with a fresh API key

### Contacts Not Syncing

**Check:**

1. Marketing List has **Sync Enabled** = Yes
2. Marketing List has **Mailchimp Audience ID** populated
3. Contact has an email address
4. Marketing List Member status is **Subscribed**

> **Note:** Unsubscribed, Cleaned, and Pending contacts are never synced to Mailchimp to maintain compliance.

### Duplicate Contacts Created

**Cause:** Email matching failed

**Solution:**

1. Ensure Contact emails are consistent (no duplicates)
2. The import flow matches by email - if no match found, a new Contact is created
3. Merge duplicate Contacts manually, then re-run import

### Import Takes Too Long

**Cause:** Large audience (>10,000 members)

**Solution:**

- Flows handle pagination automatically (1000 records per batch)
- Large imports may take 30+ minutes
- Consider running during off-hours
- Check flow run history for progress

### "Compliance State" Error

**Cause:** Trying to re-subscribe a cleaned/bounced contact

**Solution:**

- Contacts with status "Cleaned" cannot be re-subscribed
- This is Mailchimp enforcing email deliverability rules
- Remove the cleaned contact from the Marketing List

### Orphan Records After Deleting Contact

**Cause:** Contact Deleted Cleanup flow not running

**Solution:**

1. Verify the flow is turned on
2. Check flow run history for errors
3. Manually delete orphan Marketing List Members and Campaign Activities

---

## Support

**Documentation:**

- [Mailchimp API Reference](https://mailchimp.com/developer/marketing/api/)
- [Power Automate Documentation](https://docs.microsoft.com/power-automate/)

**Contact:**

- Email: connect@forceworks.com
- Website: [forceworks.com](https://www.forceworks.com)

**Found a bug?**

- Use the in-app feedback button
- Include flow run ID and error message

---

*Version 1.0 | January 2026*
