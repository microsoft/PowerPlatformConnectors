# TxtSync Connector
TxtSync enables applications to send and receive SMS Text Messages while tracking open rates and delivery reports. Please go to https://txtsync.com/microsoft-flow for more information and to create your account

### Pre-requisites
To use the connector, you need to have a valid Client Key and Client Secret which can be found by going to: [TxtSync Flow/PowerApps Integration](https://app.txtsync.com/integrations/flow).

Billing for your Account can also be done via Microsoft Azure which can be found at: [TxtSync Azure](https://azuremarketplace.microsoft.com/en-us/marketplace/apps/txtsync.txtsync_transact)

### Supported Actions
`Add a Contact`
Adds a contact and applies tags

`Delete Contact`	
Deletes a contact

`Delete Contact By External Reference`	
Deletes a contact by the external system reference

`Get Contact By External Reference`	
Get contact by external system reference

`Search Contacts`	
Search for contacts by name or number

`Send an SMS`	
Sends out a single SMS to an E164 number

`Send Bulk SMS`	
Send SMS by List of Numbers or Tag Names

`Update Contact`	
Updates a Contact

`Update Contact By External Reference`	
Updates a contact by the external system reference

### Supported Triggers


`Inbound SMS Notification`	
Raises a notification for an inbound SMS

`Outbound SMS Notification`	
Raises a notification for an outbound SMS. 
###### Please note to protect you from potentially causing an infinite loop of sending SMS the Outbound SMS trigger is not invoked when sending an SMS from Flow


