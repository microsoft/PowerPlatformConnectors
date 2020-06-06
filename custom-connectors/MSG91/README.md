## MSG91 for SMS Text & Voice
API for SMS Text, Email & Voice. MSG91 delivers over 1 Billion SMS text messages every month. Trusted by 5,000+ companies.


## Pre-requisites
You will need the following to proceed:
* A MSG91 subscription
* API Key from MGS91 dashboard



### Creating an custom connector

1. Create an account on https://msg91.com website.
2. Go to Dashboards and Flows & APIs. 
3. Create a new API key. You can use this API key to make connection in Power Automate.
4. Make sure you whitelist the IP. Otherwise, API calls will fail. For testing, you can disable additonal security so that all the IPs are white-listed. But we strongly recommend to enable after testing is completed.

## Supported Operations
The connector supports the following operations:
* `SendSMS`: Sends an SMS text message to either one phone number or multiple phone numbers or to a group. 
* `AddGroup`: Creates a new group or a list to store contacts.
* `AddContact`: Adds a new contact to an existing group.
* `DeleteGroup`: Deletes a group.
* `ListContact`: Lists contacts of a group.
* `EditContact`: Updates an existing contact.
* `ListGroup`: Lists all the groups.
* `DeleteContact`: Deletes a contact.


## Support 
Contact support@pascalcase.com 

Created by https://pascalcase.com in collaboration with https://msg91.com

