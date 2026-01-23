# Kit Power Platform Connector - Installation & Usage Guide

## Table of Contents

1. [Overview](#overview)
2. [Prerequisites](#prerequisites)
3. [Getting Your Kit API Key](#getting-your-kit-api-key)
4. [Installing the Custom Connector](#installing-the-custom-connector)
5. [Creating a Connection](#creating-a-connection)
6. [Using the Connector in Power Automate](#using-the-connector-in-power-automate)
7. [Using the Connector in Power Apps](#using-the-connector-in-power-apps)
8. [Operation Reference](#operation-reference)
9. [Common Use Cases & Flow Examples](#common-use-cases--flow-examples)
10. [Working with Pagination](#working-with-pagination)
11. [Troubleshooting](#troubleshooting)
12. [Best Practices](#best-practices)

---

## Overview

The Kit connector enables integration between Microsoft Power Platform (Power Automate, Power Apps, Logic Apps, and Copilot Studio) and Kit (formerly ConvertKit), the email marketing platform built for creators.

### What You Can Do

- **Manage Subscribers:** Create, update, list, and unsubscribe contacts
- **Organize with Tags:** Create tags and apply them to subscribers for segmentation
- **Track Form Signups:** Monitor and manage form submissions
- **Automate Sequences:** Add subscribers to email sequences programmatically
- **Create Broadcasts:** Draft email broadcasts for your audience
- **React to Events:** Use webhooks to trigger flows when subscribers take actions

### Connector Specifications

| Property | Value |
|----------|-------|
| **Connector Type** | Independent Publisher (Custom) |
| **Authentication** | API Key (Header) |
| **API Version** | Kit V4 |
| **Base URL** | `https://api.kit.com/v4` |
| **Rate Limit** | 120 requests per 60 seconds |

---

## Prerequisites

Before installing the connector, ensure you have:

### Kit Requirements

- [ ] A Kit account at [kit.com](https://kit.com)
- [ ] A Kit plan that supports API access (check [pricing](https://kit.com/pricing))
- [ ] Access to Kit's Developer Settings

### Microsoft Requirements

- [ ] A Microsoft 365 or Power Platform account
- [ ] Access to Power Automate or Power Apps
- [ ] Permission to create custom connectors in your environment
- [ ] Power Automate Premium license (custom connectors require premium)

---

## Getting Your Kit API Key

### Step 1: Access Kit Settings

1. Log in to your Kit account at [app.kit.com](https://app.kit.com)
2. Click the **gear icon** (⚙️) in the left sidebar to open Settings

![Kit Settings Location](https://via.placeholder.com/600x300?text=Kit+Settings+Sidebar)

### Step 2: Navigate to Developer Settings

1. In the Settings menu, scroll down and click **Developer**
2. You'll see the Developer Settings page with API Keys section

### Step 3: Create a New API Key

1. Under **V4 API Keys**, click **"Add a new key"**
2. Enter a descriptive name for your key:
   - Example: `Power Automate Integration`
   - Example: `Production - CRM Sync`
3. Click **Create**

### Step 4: Copy and Save Your Key

⚠️ **IMPORTANT:** The API key is only shown once!

1. Copy the generated API key immediately
2. Store it securely (password manager, Azure Key Vault, etc.)
3. If you lose the key, you'll need to create a new one

```
Example API Key Format:
kit_xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
```

### Security Best Practices

| Do | Don't |
|----|-------|
| ✅ Store keys in a password manager | ❌ Share keys in emails or chat |
| ✅ Use different keys for different integrations | ❌ Commit keys to source control |
| ✅ Rotate keys periodically | ❌ Use the same key everywhere |
| ✅ Delete unused keys | ❌ Share keys with unauthorized users |

---

## Installing the Custom Connector

### Method 1: Import via Power Automate Portal (Recommended)

#### Step 1: Download Connector Files

Ensure you have these files:
- `apiDefinition.swagger.json`
- `apiProperties.json`

#### Step 2: Open Power Automate

1. Go to [make.powerautomate.com](https://make.powerautomate.com)
2. Sign in with your Microsoft account
3. Select the appropriate **Environment** from the top-right dropdown

#### Step 3: Navigate to Custom Connectors

1. In the left sidebar, expand **More**
2. Click **Discover all**
3. Under **Data**, click **Custom connectors**

![Custom Connectors Navigation](https://via.placeholder.com/600x200?text=More+→+Discover+All+→+Custom+Connectors)

#### Step 4: Create New Custom Connector

1. Click **+ New custom connector** (top right)
2. Select **Import an OpenAPI file**
3. Enter connector name: `Kit`
4. Click **Import** and select `apiDefinition.swagger.json`
5. Click **Continue**

#### Step 5: Configure General Settings

On the **General** tab:

| Field | Value |
|-------|-------|
| Icon | Upload a Kit logo (optional) |
| Icon background color | `#da3b01` |
| Description | Kit email marketing platform connector |
| Scheme | HTTPS |
| Host | `api.kit.com` |
| Base URL | `/v4` |

#### Step 6: Configure Security

On the **Security** tab:

1. Authentication type: **API Key**
2. Parameter label: `API Key`
3. Parameter name: `X-Kit-Api-Key`
4. Parameter location: **Header**

![Security Configuration](https://via.placeholder.com/600x250?text=API+Key+Authentication+Settings)

#### Step 7: Review Operations

On the **Definition** tab:
- Review the imported operations
- All 30 operations should be listed
- No changes needed if import was successful

#### Step 8: Create the Connector

1. Click **Create connector** (top right)
2. Wait for the confirmation message
3. The connector is now available in your environment

---

### Method 2: Import via Power Platform CLI

For developers who prefer command-line tools:

#### Step 1: Install Power Platform CLI

```bash
# Using npm
npm install -g pac

# Or download from Microsoft
# https://docs.microsoft.com/en-us/power-platform/developer/cli/introduction
```

#### Step 2: Authenticate

```bash
pac auth create --environment "https://yourorg.crm.dynamics.com"
```

#### Step 3: Create Connector

```bash
# Navigate to connector folder
cd kit-connector

# Create the connector
pac connector create --api-definition apiDefinition.swagger.json --api-properties apiProperties.json
```

---

### Method 3: Import via Solution

For ALM and deployment across environments:

#### Step 1: Create a Solution

1. Go to [make.powerapps.com](https://make.powerapps.com)
2. Select **Solutions** from left sidebar
3. Click **+ New solution**
4. Name it `Kit Integration`
5. Select a publisher and click **Create**

#### Step 2: Add Custom Connector to Solution

1. Open your solution
2. Click **+ Add existing** → **Automation** → **Custom connector**
3. If connector exists, select it
4. If not, click **+ New custom connector** and follow Method 1

#### Step 3: Export and Import

```
Export: Solutions → Kit Integration → Export → Managed/Unmanaged
Import: Target Environment → Solutions → Import → Select zip file
```

---

## Creating a Connection

After installing the connector, create a connection to authenticate:

### Step 1: Test from Custom Connector

1. Go to **Custom connectors** → **Kit**
2. Click the **pencil icon** to edit
3. Go to **Test** tab
4. Click **+ New connection**

### Step 2: Enter API Key

1. In the connection dialog, paste your Kit V4 API Key
2. Click **Create connection**

### Step 3: Verify Connection

1. Back on the Test tab, select your connection
2. Expand **GetAccount** operation
3. Click **Test operation**
4. You should see your Kit account information returned

**Success Response Example:**
```json
{
  "account": {
    "name": "My Creator Business",
    "plan_type": "creator",
    "primary_email_address": "hello@mybusiness.com"
  },
  "user": {
    "name": "John Creator",
    "email_address": "john@mybusiness.com"
  }
}
```

---

## Using the Connector in Power Automate

### Creating Your First Flow

#### Example: New Subscriber Welcome Notification

**Trigger:** When an HTTP request is received (or any trigger)
**Action:** Create subscriber in Kit and send Teams notification

##### Step 1: Create Flow

1. Go to **My flows** → **+ New flow** → **Automated cloud flow**
2. Name: `Kit - New Subscriber Welcome`
3. Choose trigger (e.g., When an item is created in SharePoint)

##### Step 2: Add Kit Action

1. Click **+ New step**
2. Search for **Kit**
3. Select **Create a subscriber**

##### Step 3: Configure Action

| Field | Value |
|-------|-------|
| Email Address | `@{triggerOutputs()?['body/Email']}` |
| First Name | `@{triggerOutputs()?['body/FirstName']}` |
| State | `active` |

##### Step 4: Add Notification

1. Add another step: **Post message in a chat or channel (Teams)**
2. Configure the message with subscriber details

##### Step 5: Save and Test

1. Click **Save**
2. Click **Test** → **Manually** → **Test**
3. Provide test input and verify execution

---

### Working with Dynamic Content

Kit operations return structured data you can use in subsequent steps:

#### Subscriber Object Fields

```
subscriber.id           → Unique subscriber ID
subscriber.first_name   → First name
subscriber.email_address → Email address
subscriber.state        → Status (active, inactive, etc.)
subscriber.created_at   → Creation timestamp
subscriber.fields       → Custom field values (object)
```

#### Using in Expressions

```
// Get subscriber ID from Create Subscriber response
outputs('Create_a_subscriber')?['body/subscriber/id']

// Get custom field value
outputs('Get_Subscriber')?['body/subscriber/fields/Company']

// Check if subscriber is active
if(equals(outputs('Get_Subscriber')?['body/subscriber/state'], 'active'), 'Yes', 'No')
```

---

## Using the Connector in Power Apps

### Adding the Connector

1. Open your app in Power Apps Studio
2. Click **Data** in the left panel
3. Click **+ Add data**
4. Search for **Kit**
5. Select the Kit connector
6. Choose your connection

### Calling Operations

#### List All Tags

```powerapps
// In a button's OnSelect or screen's OnVisible
ClearCollect(
    colTags,
    Kit.ListTags({per_page: 100}).tags
)
```

#### Create a Subscriber

```powerapps
// In a submit button's OnSelect
Set(
    varNewSubscriber,
    Kit.CreateSubscriber({
        email_address: txtEmail.Text,
        first_name: txtFirstName.Text,
        state: "active"
    })
);

If(
    !IsBlank(varNewSubscriber.subscriber.id),
    Notify("Subscriber created!", NotificationType.Success),
    Notify("Error creating subscriber", NotificationType.Error)
)
```

#### Display Subscribers in a Gallery

```powerapps
// Screen OnVisible
ClearCollect(colSubscribers, Kit.ListSubscribers({per_page: 50}).subscribers)

// Gallery Items property
colSubscribers

// Labels in gallery
ThisItem.email_address
ThisItem.first_name
ThisItem.state
```

### Handling Pagination in Power Apps

```powerapps
// Initial load
ClearCollect(
    colSubscribers,
    Kit.ListSubscribers({per_page: 100})
);

// Load more (in a "Load More" button)
Collect(
    colSubscribers,
    Kit.ListSubscribers({
        per_page: 100,
        after: Last(colSubscribers.subscribers).id
    }).subscribers
)
```

---

## Operation Reference

### Subscribers

| Operation | Method | Description |
|-----------|--------|-------------|
| `ListSubscribers` | GET | Get paginated list of subscribers |
| `GetSubscriber` | GET | Get single subscriber by ID |
| `CreateSubscriber` | POST | Create or update subscriber (upsert) |
| `UpdateSubscriber` | PUT | Update existing subscriber |
| `UnsubscribeSubscriber` | POST | Unsubscribe a subscriber |

#### ListSubscribers Parameters

| Parameter | Type | Description |
|-----------|------|-------------|
| `status` | string | Filter: active, inactive, bounced, complained, cancelled |
| `email_address` | string | Filter by exact email |
| `created_after` | datetime | Filter by creation date |
| `created_before` | datetime | Filter by creation date |
| `per_page` | integer | Results per page (max 100) |
| `after` | string | Pagination cursor |

#### CreateSubscriber Body

```json
{
  "email_address": "subscriber@example.com",
  "first_name": "Jane",
  "state": "active",
  "fields": {
    "Last name": "Doe",
    "Company": "Acme Inc"
  }
}
```

---

### Tags

| Operation | Method | Description |
|-----------|--------|-------------|
| `ListTags` | GET | Get all tags |
| `CreateTag` | POST | Create a new tag |
| `UpdateTag` | PUT | Rename a tag |
| `ListSubscribersForTag` | GET | Get subscribers with a tag |
| `TagSubscriber` | POST | Add tag to subscriber by ID |
| `TagSubscriberByEmail` | POST | Add tag to subscriber by email |
| `RemoveTagFromSubscriber` | DELETE | Remove tag from subscriber |

---

### Forms

| Operation | Method | Description |
|-----------|--------|-------------|
| `ListForms` | GET | Get all forms |
| `ListSubscribersForForm` | GET | Get subscribers from a form |
| `AddSubscriberToForm` | POST | Add subscriber to form |

---

### Sequences

| Operation | Method | Description |
|-----------|--------|-------------|
| `ListSequences` | GET | Get all sequences |
| `ListSubscribersForSequence` | GET | Get subscribers in a sequence |
| `AddSubscriberToSequence` | POST | Add subscriber to sequence |

---

### Broadcasts

| Operation | Method | Description |
|-----------|--------|-------------|
| `ListBroadcasts` | GET | Get all broadcasts |
| `GetBroadcast` | GET | Get broadcast details |
| `CreateBroadcast` | POST | Create draft broadcast |
| `UpdateBroadcast` | PUT | Update draft broadcast |
| `DeleteBroadcast` | DELETE | Delete draft broadcast |

#### CreateBroadcast Body

```json
{
  "subject": "Weekly Newsletter",
  "content": "<html><body><h1>Hello {{ subscriber.first_name }}!</h1></body></html>",
  "preview_text": "This week's updates...",
  "description": "Newsletter for week of Jan 20",
  "public": true
}
```

---

### Webhooks

| Operation | Method | Description |
|-----------|--------|-------------|
| `ListWebhooks` | GET | Get all webhooks |
| `CreateWebhook` | POST | Subscribe to events |
| `DeleteWebhook` | DELETE | Unsubscribe from events |

#### Webhook Event Types

| Event | Required Parameter | Description |
|-------|-------------------|-------------|
| `subscriber.subscriber_activate` | - | Subscriber confirms |
| `subscriber.subscriber_unsubscribe` | - | Subscriber unsubscribes |
| `subscriber.subscriber_bounce` | - | Email bounces |
| `subscriber.subscriber_complain` | - | Marked as spam |
| `subscriber.form_subscribe` | `form_id` | Form submission |
| `subscriber.course_subscribe` | `sequence_id` | Added to sequence |
| `subscriber.course_complete` | `sequence_id` | Completed sequence |
| `subscriber.link_click` | `initiator_value` | Clicked link |
| `subscriber.product_purchase` | `product_id` | Purchased product |
| `subscriber.tag_add` | `tag_id` | Tag added |
| `subscriber.tag_remove` | `tag_id` | Tag removed |
| `purchase.purchase_create` | - | New purchase |

---

## Common Use Cases & Flow Examples

### Use Case 1: Sync CRM Contacts to Kit

**Scenario:** When a new contact is created in Dynamics 365, add them to Kit with appropriate tags.

```
Trigger: When a record is created (Dataverse - Contact)
    ↓
Action: Create a subscriber (Kit)
    - Email: Contact Email
    - First Name: Contact First Name
    - State: inactive (double opt-in)
    ↓
Condition: Is Customer = Yes?
    ↓ Yes
Action: Tag subscriber by email (Kit)
    - Tag ID: [Customer Tag ID]
    - Email: Contact Email
    ↓ No
Action: Tag subscriber by email (Kit)
    - Tag ID: [Lead Tag ID]
    - Email: Contact Email
```

### Use Case 2: Welcome Sequence Enrollment

**Scenario:** When someone fills out a specific form, add them to a welcome sequence.

```
Trigger: When a new response is submitted (Microsoft Forms)
    ↓
Action: Create a subscriber (Kit)
    - Email: Response Email
    - First Name: Response Name
    - State: active
    ↓
Action: Add subscriber to sequence (Kit)
    - Sequence ID: [Welcome Sequence ID]
    - Email: Response Email
    ↓
Action: Post message (Teams)
    - Message: "New subscriber enrolled in welcome sequence: {email}"
```

### Use Case 3: Tag Based on Purchase

**Scenario:** When an order is placed in your e-commerce system, tag the customer in Kit.

```
Trigger: When an HTTP request is received (Webhook from e-commerce)
    ↓
Parse JSON: Extract order details
    ↓
Action: Create a subscriber (Kit)
    - Email: Customer Email
    - Fields: {"Product Purchased": "@{Product Name}"}
    ↓
Action: Tag subscriber by email (Kit)
    - Tag ID: [Customers Tag ID]
    - Email: Customer Email
    ↓
Condition: Order Total > 100?
    ↓ Yes
Action: Tag subscriber by email (Kit)
    - Tag ID: [VIP Customers Tag ID]
```

### Use Case 4: Daily Subscriber Report

**Scenario:** Send a daily Teams message with new subscriber count.

```
Trigger: Recurrence (Daily at 9 AM)
    ↓
Action: List subscribers (Kit)
    - Created After: addDays(utcNow(), -1)
    - Status: active
    ↓
Compose: Count subscribers
    - Expression: length(outputs('List_subscribers')?['body/subscribers'])
    ↓
Action: Post adaptive card (Teams)
    - Title: "Daily Kit Subscriber Report"
    - Body: "New subscribers in last 24 hours: {count}"
```

### Use Case 5: Unsubscribe Sync

**Scenario:** When someone unsubscribes in Kit, update your CRM.

```
Trigger: When an HTTP request is received
    - Configure as Kit webhook endpoint
    ↓
Parse JSON: Webhook payload
    ↓
Condition: Event = subscriber.subscriber_unsubscribe?
    ↓ Yes
Action: List records (Dataverse)
    - Filter: email eq '{subscriber_email}'
    ↓
Action: Update a record (Dataverse)
    - Email Opt Out: Yes
    - Updated: utcNow()
```

---

## Working with Pagination

Kit API uses cursor-based pagination for list operations.

### Understanding the Response

```json
{
  "subscribers": [...],
  "pagination": {
    "has_previous_page": false,
    "has_next_page": true,
    "start_cursor": "abc123",
    "end_cursor": "xyz789",
    "per_page": 50
  }
}
```

### Paginating in Power Automate

#### Method 1: Do Until Loop

```
Initialize variable: allSubscribers (Array)
Initialize variable: cursor (String) = ""
Initialize variable: hasMore (Boolean) = true
    ↓
Do Until: hasMore equals false
    ↓
    Action: List subscribers (Kit)
        - per_page: 100
        - after: @{variables('cursor')}
        ↓
    Append to array: allSubscribers
        - Value: @{outputs('List_subscribers')?['body/subscribers']}
        ↓
    Set variable: hasMore
        - Value: @{outputs('List_subscribers')?['body/pagination/has_next_page']}
        ↓
    Set variable: cursor
        - Value: @{outputs('List_subscribers')?['body/pagination/end_cursor']}
```

#### Method 2: Get First Page Only

For most use cases, the first page (up to 100 results) is sufficient:

```
Action: List subscribers (Kit)
    - per_page: 100
    - status: active
```

---

## Troubleshooting

### Common Errors

#### 401 Unauthorized

**Cause:** Invalid or missing API key

**Solutions:**
1. Verify API key is correct (check for extra spaces)
2. Ensure you're using a V4 API key (not V3)
3. Create a new connection with fresh key
4. Check that your Kit plan supports API access

#### 404 Not Found

**Cause:** Resource doesn't exist

**Solutions:**
1. Verify the ID (subscriber_id, tag_id, etc.) is correct
2. Check if the resource was deleted
3. Ensure you're using the correct endpoint

#### 422 Unprocessable Entity

**Cause:** Validation error in request

**Solutions:**
1. Check email address format
2. Verify required fields are provided
3. Review the error message for specific field issues

#### 429 Too Many Requests

**Cause:** Rate limit exceeded (120 requests/60 seconds)

**Solutions:**
1. Add delays between requests: `delay(outputs('...'), 'PT1S')`
2. Reduce frequency of scheduled flows
3. Batch operations where possible
4. Implement exponential backoff

### Debugging Tips

#### Enable Run History

1. Go to flow details
2. Click on a failed run
3. Expand each action to see inputs/outputs
4. Check the error message in failed actions

#### Test in Connector

1. Edit the custom connector
2. Go to Test tab
3. Test individual operations
4. View raw request/response

#### Check Kit Dashboard

1. Log into Kit
2. Verify subscribers, tags, etc. exist
3. Check for any account limitations

### Error Response Format

Kit returns errors in this format:

```json
{
  "errors": [
    "Email address is invalid",
    "First name is too long"
  ]
}
```

---

## Best Practices

### Performance

| Recommendation | Reason |
|----------------|--------|
| Use `per_page: 100` for large lists | Reduces number of API calls |
| Cache tag/form IDs | Avoid repeated lookups |
| Use specific filters | Reduce response size |
| Implement pagination only when needed | Most flows need only first page |

### Reliability

| Recommendation | Reason |
|----------------|--------|
| Add error handling (Scope + Configure run after) | Graceful failure handling |
| Use retry policies on HTTP actions | Handle transient failures |
| Log important operations | Debugging and auditing |
| Test with real data | Catch edge cases |

### Security

| Recommendation | Reason |
|----------------|--------|
| Use Azure Key Vault for API keys | Centralized secret management |
| Limit connection sharing | Principle of least privilege |
| Rotate API keys periodically | Reduce exposure risk |
| Monitor flow runs | Detect anomalies |

### Maintainability

| Recommendation | Reason |
|----------------|--------|
| Use descriptive flow names | Easy identification |
| Add comments to complex expressions | Future reference |
| Document custom connector changes | Version control |
| Test after Kit API updates | Ensure compatibility |

---

## Appendix

### Sample Custom Field Values

When creating or updating subscribers, you can set custom fields:

```json
{
  "fields": {
    "Last name": "Smith",
    "Company": "Acme Corp",
    "Phone": "+1-555-0123",
    "Birthday": "March 15",
    "Source": "Power Automate Sync",
    "Lead Score": "85",
    "Interests": "Marketing, Automation"
  }
}
```

> **Note:** Custom field names must match exactly (case-sensitive) with fields in your Kit account.

### Webhook Payload Examples

#### subscriber.subscriber_activate

```json
{
  "subscriber": {
    "id": 12345,
    "first_name": "Jane",
    "email_address": "jane@example.com",
    "state": "active",
    "created_at": "2025-01-20T10:30:00Z",
    "fields": {}
  }
}
```

#### subscriber.tag_add

```json
{
  "subscriber": {
    "id": 12345,
    "email_address": "jane@example.com",
    "state": "active"
  },
  "tag": {
    "id": 67890,
    "name": "Customer"
  }
}
```

### Useful Expression Examples

```javascript
// Format date for Kit (ISO 8601)
formatDateTime(utcNow(), 'yyyy-MM-ddTHH:mm:ssZ')

// Get yesterday's date
addDays(utcNow(), -1)

// Check if array is empty
empty(outputs('List_subscribers')?['body/subscribers'])

// Get first subscriber from list
first(outputs('List_subscribers')?['body/subscribers'])

// Count items in response
length(outputs('List_tags')?['body/tags'])

// Concatenate first and last name
concat(triggerBody()?['FirstName'], ' ', triggerBody()?['LastName'])

// Convert to lowercase for email
toLower(triggerBody()?['Email'])
```

---

## Support & Resources

### Official Resources

- [Kit API Documentation](https://developers.kit.com/api-reference/overview)
- [Kit Help Center](https://help.kit.com)
- [Kit Status Page](https://status.kit.com)

### Power Platform Resources

- [Custom Connector Documentation](https://docs.microsoft.com/en-us/connectors/custom-connectors/)
- [Power Automate Community](https://powerusers.microsoft.com/t5/Power-Automate-Community/ct-p/MPACommunity)
- [Power Platform CLI](https://docs.microsoft.com/en-us/power-platform/developer/cli/introduction)

### Reporting Issues

For connector issues:
1. Check this troubleshooting guide first
2. Search [Power Platform Community](https://powerusers.microsoft.com)
3. For Kit API issues, contact [Kit Support](https://kit.com/support)
4. For connector bugs, open an issue on the [GitHub repository](https://github.com/microsoft/PowerPlatformConnectors)

---

*Last Updated: January 2025*
*Connector Version: 1.0.0*
*Kit API Version: V4*
