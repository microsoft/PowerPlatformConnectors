# Kit (Independent Publisher)

Kit (formerly ConvertKit) is an email marketing platform built specifically for creators. This connector allows you to automate subscriber management, tagging, form subscriptions, sequences, broadcasts, and more through Power Automate, Power Apps, Logic Apps, and Copilot Studio.

## Publisher: Steve Mordue

[Steve Mordue](https://github.com/forceworks) | [Forceworks](https://forceworks.com)

## Prerequisites

- A Kit account with an eligible plan that supports API access ([pricing](https://kit.com/pricing))
- A Kit V4 API Key

## Obtaining Credentials

### How to get your Kit V4 API Key

1. Log in to your Kit account at [app.kit.com](https://app.kit.com)
2. Navigate to **Settings** (gear icon in the sidebar)
3. Select **Developer** from the settings menu
4. Under **API Keys**, click **Add a new key**
5. Give your API key a descriptive name (e.g., "Power Automate Integration")
6. Copy the generated API key immediately - you won't be able to see it again
7. Store the key securely - you'll need it when creating a connection in Power Automate

> **Important:** Keep your API key secret. Do not share it publicly or commit it to source control.

## Supported Operations

### Subscribers

| Operation | Description |
|-----------|-------------|
| **List Subscribers** | Get a paginated list of all subscribers with optional filters |
| **Get Subscriber** | Retrieve details for a specific subscriber by ID |
| **Create Subscriber** | Create a new subscriber or update existing (upsert behavior) |
| **Update Subscriber** | Update subscriber information |
| **Unsubscribe Subscriber** | Unsubscribe a subscriber from your account |

### Tags

| Operation | Description |
|-----------|-------------|
| **List Tags** | Get all tags in your account |
| **Create Tag** | Create a new tag |
| **Update Tag** | Rename an existing tag |
| **List Subscribers for Tag** | Get all subscribers with a specific tag |
| **Tag Subscriber** | Add a tag to a subscriber by ID |
| **Tag Subscriber by Email** | Add a tag to a subscriber by email address |
| **Remove Tag from Subscriber** | Remove a tag from a subscriber |

### Forms

| Operation | Description |
|-----------|-------------|
| **List Forms** | Get all forms in your account |
| **List Subscribers for Form** | Get subscribers who signed up through a form |
| **Add Subscriber to Form** | Add an existing subscriber to a form |

### Sequences

| Operation | Description |
|-----------|-------------|
| **List Sequences** | Get all email sequences in your account |
| **List Subscribers for Sequence** | Get subscribers enrolled in a sequence |
| **Add Subscriber to Sequence** | Add a subscriber to an email sequence |

### Broadcasts

| Operation | Description |
|-----------|-------------|
| **List Broadcasts** | Get all broadcasts in your account |
| **Get Broadcast** | Get details of a specific broadcast |
| **Create Broadcast** | Create a new draft broadcast |
| **Update Broadcast** | Update an existing draft broadcast |
| **Delete Broadcast** | Delete a draft broadcast |

### Segments

| Operation | Description |
|-----------|-------------|
| **List Segments** | Get all segments in your account |

### Custom Fields

| Operation | Description |
|-----------|-------------|
| **List Custom Fields** | Get all custom fields |
| **Create Custom Field** | Create a new custom field for subscribers |

### Webhooks

| Operation | Description |
|-----------|-------------|
| **List Webhooks** | Get all webhook subscriptions |
| **Create Webhook** | Subscribe to Kit events |
| **Delete Webhook** | Remove a webhook subscription |

### Account

| Operation | Description |
|-----------|-------------|
| **Get Account** | Get account information and statistics |

## Webhook Event Types

When creating webhooks, you can subscribe to these event types:

- `subscriber.subscriber_activate` - Subscriber confirms their subscription
- `subscriber.subscriber_unsubscribe` - Subscriber unsubscribes
- `subscriber.subscriber_bounce` - Email bounces
- `subscriber.subscriber_complain` - Subscriber marks email as spam
- `subscriber.form_subscribe` - Subscriber signs up through a form (requires `form_id`)
- `subscriber.course_subscribe` - Subscriber added to sequence (requires `sequence_id`)
- `subscriber.course_complete` - Subscriber completes sequence (requires `sequence_id`)
- `subscriber.link_click` - Subscriber clicks a link (requires `initiator_value`)
- `subscriber.product_purchase` - Subscriber purchases product (requires `product_id`)
- `subscriber.tag_add` - Tag added to subscriber (requires `tag_id`)
- `subscriber.tag_remove` - Tag removed from subscriber (requires `tag_id`)
- `purchase.purchase_create` - New purchase recorded

## Known Issues and Limitations

1. **Rate Limits:** Kit API allows 120 requests per 60-second rolling window. The connector does not implement automatic retry logic for 429 (rate limit) responses.

2. **Pagination:** Kit uses cursor-based pagination. Use the `after` parameter with the `end_cursor` from the previous response to get the next page.

3. **Bulk Operations:** This connector does not include Kit's bulk endpoints. For large batch operations, consider making multiple individual calls or using Kit's native bulk API directly.

4. **Broadcast Sending:** The Create Broadcast operation creates a draft. You cannot send broadcasts directly via API - they must be scheduled or sent manually in the Kit dashboard.

5. **Custom Fields Limit:** Kit accounts are limited to 140 custom fields.

6. **Plan Requirements:** API access requires an eligible Kit plan. Free plans may have limited or no API access.

## Frequently Asked Questions

### Why is my subscriber showing as "inactive"?

Subscribers created via API start as inactive until they confirm their email (double opt-in). To bypass this, create the subscriber with `state: "active"`, though this should only be done when you have prior consent.

### How do I filter subscribers by tag?

Use the "List Subscribers for Tag" operation with the tag ID to get all subscribers who have that specific tag.

### Can I send emails through this connector?

You can create draft broadcasts, but sending must be done through the Kit dashboard or by setting up automated sequences/visual automations in Kit itself.

### What's the difference between forms and sequences?

- **Forms** are opt-in points where subscribers join your list
- **Sequences** are automated email series sent over time to subscribers

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.

## Further Reading

- [Kit API Documentation](https://developers.kit.com/api-reference/overview)
- [Kit Help Center](https://help.kit.com)
- [Kit Developer Community](https://kit.typeform.com/to/f8urvmPe)
