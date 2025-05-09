# Desk365
Desk365 is a cloud-based modern helpdesk for the Microsoft 365 workplace that lets you deliver outstanding customer service through channels like Microsoft Teams, Email, Web Forms/Widgets and more. Automate repetitive work and save time with Desk365’s intuitive, feature-rich web app that comes with a unified inbox to manage all your customer conversations.
## Publisher: Kani Technologies Inc

## Prerequisites
You will need to have an active [Desk365](https://www.desk365.io/pricing/) subscription to leverage this connector.

## Supported Operations

### When a Ticket is Created
This will trigger when a ticket is created.

### When a Ticket is Updated
This will trigger when a ticket is updated.

### When a Note is added to a Ticket
This will trigger when a note is added to a ticket.

### When a Reply is added to a Ticket
This will trigger when a reply is added to a ticket.

### Create a Ticket
Action used to create a new ticket.

### Update a Ticket
Action used to update a specific ticket.

### Add a note to a ticket
Action used to add a note to a specific ticket by specifying the ticket number.
You can also make the note public or private.

### Get Ticket
Action used to get a specific ticket by specifying the ticket number.

### Get all tickets
Action used to get all tickets.
You can also customize your query based on filters.

## Obtaining Credentials
You will need an API Key to access the Desk365 API. Please contact help@desk365.io to request access.

## Known Issues and Limitations
By default, the operation "Get all tickets" do not include the description of the ticket in the response. However, you can choose to include it by setting the "Include Description" parameter to "Yes".
