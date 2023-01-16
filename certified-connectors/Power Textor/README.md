# Power Textor Connector

With the Power Textor connector, send SMS messages to your customers and get push notifications when a text message is received. Build brand loyalty by a one-on-one conversation with customers with Power Textor actions that support a range of text message sending options; including on-demand SMS, scheduled SMS, event reminders, and Google Review requests. Each functionality is available for an individual number or a group of numbers.

## Publisher: Imperium Dynamics

## Prerequisites

Subscribe to a Power Textor plan `https://powertextor.com/#pricing`.

## Supported Operations

### When a text (SMS) message is received (Trigger)

This operation triggers a flow when a new text message arrives. The flow gets information of sender number, text message body, sender location and address details, recipient number, and recipient location and address details.

### Send text (SMS) message to multiple contacts

Send a bulk text (SMS) message to multiple Power Textor Contacts at once.

### Schedule review text (SMS) message for multiple contacts

Schedule review messages in bulk for Power Textor contacts by specifying the date and time on which the messages should be sent. Send pin location of your business or service area within the text message.

### Send text (SMS) message to multiple groups

Send a bulk text (SMS) message to multiple Power Textor groups at once.

### Send review text (SMS) message to multiple groups

Send review text messages to multiple Power Textor groups to get feedback from customers. Send the pin location of your business or service area with in the text messages.

### Schedule text (SMS) message for a contact

Schedule a text (SMS) message for a Power Textor contact and specify the scheduled date and time on which the text message should be sent.

### Schedule text (SMS) message for multiple groups

Schedule a bulk text message to send to Power Textor groups, by configuring the date and time when the text message should be sent.

### Schedule review text (SMS) message for a contact

Schedule a review message for a Power Textor contact by specifying the date and time on which the message should be sent. Send the pin location of your business or service area within the text message.

### Schedule review text (SMS) message for multiple groups

Schedule review messages in bulk for Power Textor groups by specifying the date and time on which the messages should be sent. Send the pin location of your business or service area within the text message.

### Send text (SMS) message to a contact

Send a text message to a Power Textor Contact.

### Schedule text (SMS) message for multiple contacts

Schedule a bulk text message to multiple contacts by configuring the date and time when the text message should be sent.

### Send review text (SMS) message to a contact

Send a review text message to a Power Textor contact to get feedback from a customer. Send the pin location of your business or service area within the text message.

### Send review text (SMS) message to multiple contacts

Send a review text message to Power Textor contacts to get feedback from customers. Send the pin location of your business or service area within the text messages.

### Schedule text (SMS) message event reminder to a contact

Schedule an event reminder to a Power Textor contact by specifying event date. Provide the number of days before the event day and the time when the text should be sent.

### Schedule text (SMS) message event reminder to multiple contacts

Schedule a bulk event reminder to Power Textor contacts by specifying the event date. Provide the number of days before the event day and the time when the texts should be sent.

### Schedule text (SMS) message event reminder to multiple groups

Schedule a bulk event reminder to Power Textor group(s) by specifying event date. Provide the number of days before the event day and the time when the text should be sent.

### Send text (SMS) message to a number

Send a text message to a number directly, without specifying the name of the user, while the contact is automatically created in Power Textor.

### Schedule review text (SMS) message for a group

Schedule a review message for a Power Textor group by specifying the date and time on which the message should be sent. Send the pin location of your business or service area within the text message.

### Schedule text (SMS) message for a group

Schedule a bulk text message to group by configuring the date and time when the text message should be sent.

### Schedule text (SMS) message event reminder to a group

Schedule a bulk event reminder to a Power Textor group by specifying the event date. Provide the number of days before the event day and the time when the text should be sent.

### Send review text (SMS) message to a group

Send a review text message to a Power Textor group to get the feedback of a customer. Send the pin location of your business or service area within the text message.

### Send text (SMS) message to a group

Send a bulk text (SMS) message to a PowerTextor group at once.

### Send text (SMS) message to a new group

Create a new group to send a text message to using Power Textor. The text message will be sent, while the group is automatically created and saved in Power Textor.

### Schedule a text (SMS) message for a new group

Create a new group, and schedule a text message for the group using Power Textor, in the same flow.

### Schedule text (SMS) message event reminder to a new group

Create a group to schedule a text message event reminder using Power Textor while the group is automatically created and saved in Power Textor.

### Send text (SMS) message to a new contact

Create a new contact, and send a text message to this contact using Power Textor, in the same flow.

### Schedule review text (SMS) message to a new contact

Schedule a text message to a new contact based on time and date using Power Textor, while the contact is automatically created and saved in Power Textor.

### Send review text (SMS) message to a new contact

Create a contact to send a review text message using Power Textor, while the contact is automatically created in Power Textor.

### Schedule review text (SMS) message to a new group

Schedule a text message to a new group based on time and date using Power Textor, while the group is automatically created and saved in Power Textor.

### Send review text (SMS) message to a new group

Create a group to send a review text message using Power Textor while the group is automatically created and saved in Power Textor.

### Schedule a text (SMS) message for a new contact

Create a new contact and schedule a text message for the contact using Power Textor in the same flow.

### Create a Contact

Create a new contact from a flow to save the contact in Power Textor.

### Schedule text (SMS) message event reminder to a new contact

Create a contact to schedule a text message event reminder using Power Textor, while the contact is automatically created in Power Textor.

### Update a Power Textor Contact

Update the contact name of an existing contact in Power Textor. The contact name in Power Textor is changed to the new name as the flow runs and succeeds.

### Send text (SMS) message to multiple numbers

Send a text to multiple contact numbers directly, while their numbers are saved in Power Textor.

### Send a text (SMS) message

Send text message to a number directly, without specifying name of the user, while the contact is automatically created in PowerTextor.

### Send an MMS

Send an MMS to a number directly, without specifying name of the user, while the contact is automatically created in PowerTextor.

### Send an MMS to a group

Send an MMS to a PowerTextor group at once.

### Send an MMS to a contact

Send an MMS to a Power Textor Contact.


## Obtaining Credentials

Power Textor custom connector uses API key authentication. When creating a connection for the Power Textor custom connector, an API key needs to be provided. This API key can be obtained through the Power Textor website, the steps for which have been outlined in the following section.

1. Login to Power Textor.
2. Go to **API Keys** section.
3. Generate a new API Key by clicking **New** button.
4. Enter a name to identify your API Key.
5. Select the period of validity for the API Key.
6. Click **Save**.
7. Copy the key displayed in **API Key** field.

## Known Issues and Limitations

No issues.

## Deployment Instructions

Following steps are required to deploy this connector:

1. Go to `https://make.powerapps.com/`
2. Go to **Custom Connectors** section under **Dataverse** tab
3. Click **New custom connector** button
4. Select **Import an OpenAPI file**
5. Add a name for the connector
6. Select **Import**
7. Select Swagger API definition file (`apiDefinition.swagger.json`)
8. Click **Continue**
9. Share connector with users and/or editors
