# Intercom

Intercom is a customer communications platform that combines a help desk, AI chatbot (Fin), proactive support, and customer engagement tools. This connector enables Power Automate flows to manage conversations, contacts, and tickets in your Intercom workspace — powering support alerting, ticket escalation, and customer feedback workflows.

## Publisher

### Aaron Mah

## Prerequisites

1. Sign up for an Intercom workspace. A free development workspace is available at [app.intercom.com/admins/sign_up/developer](https://app.intercom.com/admins/sign_up/developer).
2. Go to the **Developer Hub** → click **New App** → name it → click **Create app**.
3. Navigate to **Configure > Authentication**.
4. Toggle ON these permissions: *Read conversations*, *Read and write conversations*, *Read users and companies*, *Read and write users*, *Read tickets*, *Read and write tickets*, *Read admins*.
5. Copy the **Access Token** displayed on the page.
6. In Power Automate, create a new Intercom connection and paste the access token.

## Supported Operations

### Search Conversations
Search for conversations using filters such as state, creation date, assignee, or tags.

### Get Conversation
Retrieve a single conversation by ID, including its full message history.

### Reply to Conversation
Send a reply to an existing conversation as an admin.

### Search Contacts
Search for contacts (users and leads) using filters such as email, name, or creation date.

### Get Contact
Retrieve a single contact by ID, including all profile data and custom attributes.

### Create Contact
Create a new contact (user or lead) in Intercom.

### Search Tickets
Search for tickets using filters such as state, creation date, or ticket type.

### Get Ticket
Retrieve a single ticket by ID, including its type, state, and custom attributes.

### Create Ticket
Create a new ticket in Intercom linked to an existing contact.

### Close Conversation
Close an open conversation as an admin, with an optional closing comment.

### Update Ticket
Update a ticket's state, assignment, or custom attributes.

### List Tags
Retrieve all tags defined in the Intercom workspace.

### List Admins
Retrieve all admins and teammates in the Intercom workspace.

## API Documentation

Visit [Intercom Developer Docs](https://developers.intercom.com/docs/references/2.11/introduction) for further details.

## Known Issues and Limitations

- **Rate limits:** Intercom enforces rate limits of 1,000 API calls per minute for most plans. Exceeding this returns HTTP 429.
- **UNIX timestamps:** All date/time fields (created_at, updated_at, etc.) are returned as UNIX timestamps (seconds since epoch). Use Power Automate expressions to convert to human-readable dates.
- **Search endpoints use POST:** Unlike typical REST APIs, Intercom's search endpoints (conversations, contacts, tickets) use POST with a JSON body rather than GET with query parameters.
- **Development workspace limits:** Free development workspaces are limited to 20 users/leads, US region only, and have a watermarked Messenger.
- **Ticket type IDs:** When creating tickets, you must provide a ticket_type_id. Find this in Intercom Settings > Tickets > Ticket types.
- **No connector-native triggers:** This version does not include polling triggers. Use the built-in Recurrence trigger with Search Conversations or Search Tickets actions to poll for new items.

## License

Distributed under the MIT License.
