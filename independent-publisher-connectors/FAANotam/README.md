# FAANotamCRM
This connector uses your API Key to retrieve NOTAM (Notice to Airmen) database. This will allow the user to retrieve all data from the Notams database that is 
available. However, the user must register to get access to the FAA site as well as receive approval from the FAA in order to use the NOTAM api. Please make sure you use this connector and set limitations (as you would with any other API).

##Publisher: Falana Kidd, Double Axis

##Prerequisites
The user must register for a user account on this site: https://api.faa.gov/s/first-time-user. 

##Obtaining Credentials
After you register, login and then request access from the FAA for access to the selected API. 
The API key is generated and given to you from the FAA. This key will only give you access to the scope specified, which is the Notam scope. 


##Supported Operations
#Operation 1
Get Notams
This will allow you to retrieve all Notams available. You will then have to filter the data.

##Getting Started
Directions: After your account is approved, the FAA will issue an API key.
1. Create a connection reference in Power Automate
2. Add your assigned API key to the new connection reference
3. Use this connection reference with the FAA connector. See figure 1 and figure 2. The following example demonstrates adding data from Notams to an activity record. 
However, you have the abiliy to add this data to an entity record.

##Known Issues and Limitations
In the event the NOTAM database is down for enhancements, then the connector may not function as expected. Wait until you are notified that the database back up and running and then proceed to run your power automate flow again.

##Frequently Asked Questions
None

##Deployment Instructions
None

