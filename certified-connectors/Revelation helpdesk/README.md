
## Revelation Helpdesk Connector
The Revelation helpdesk connector allows you to create and update items in your helpdesk such as tickets, clients, users and assets by connecting to the Revelation helpdesk API using OAuth authentication. You can also take advantage of the extensive list of triggers allowing you to integrate your business processes based on events that occur in Revelation helpdesk.


## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A Revelation helpdesk account. If you are not yet signed up for Revelation helpdesk you can [get a free 30-day trial here](https://revelationhelpdesk.com/prime-free-trial#form)


### How to get credentials
To use the connector you will need the URL to your Revelation helpdesk instance and you will need to have a Super Admin account.
If you have signed up for a Trial the URL will be 'https://trial.revelationhelpdesk.com' and your username will be the email address you used to sign up. You will be promped to change your default password the first time you sign in to your trial account.
  

## Supported actions
The connector supports the following actions:
* `Log a new Ticket`: Create a new ticket
* `Reassign a Ticket`: Assigns an existing ticket to a user or queue
* `Add an Action`: Adds a support staff, private or end user action to an existing ticket
* `Set Ticket Priority`: Set the priority of an existing ticket
* `Set Ticket Status`: Set the status of an existing ticket
* `Set Ticket Type`: Set the type of an existing ticket
* `Create Client`: Create a new client
* `Create Office`: Create a new office
* `Create User`: Create a new user account
* `Create Asset`: Create a new asset
* `Find a User`: Find an existing user by any field
* `Search for Assets`: Find existing assets by any field
* `Update Asset`: Update an existing asset's details
* `Get an existing Asset`: Get an existing asset by id
* `Get an existing Ticket`: Get an existing ticket by Ticket # (id)


## Supported triggers
The connector supports the following triggers:
* `When a Ticket is logged`: Get notified when a new ticket is created
* `When a Ticket is updated`: Get notified when an action is added to an existing ticket
* `When a Ticket is closed`: Get notified when a ticket is closed
* `When a Ticket is reopened`: Get notified when a closed ticket is reopened
* `When a Ticket is at risk`: Get notified when a ticket is at risk of going late
* `When a Ticket is due`: Get notified when a ticket has gone late (SLA missed)
* `When a Ticket is deleted`: Get notified when a ticket is deleted
* `When a Client is created`: Get notified when a client is created
* `When a Client is updated`: Get notified when an existing client's details are updated
* `When a Office is created`: Get notified when an office is created
* `When a Office is updated`: Get notified when an existing office's details are updated
* `When a User is created`: Get notified when a user is created
* `When a User is updated`: Get notified when an existing user's details are updated
* `When a Asset is created`: Get notified when an asset is created
* `When a Asset is updated`: Get notified when an existing asset's details are updated
