## Peakboard Connector
The Peakboard Connector offers the possibility to receive the alerts, sent to your Peakboard Hub Online group, in Microsoft Power Automate
for further processing.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A Peakboard Hub Online subscription
* One or more Peakboard Boxes

## Building the connector 
When using it for the first time, the Peakboard Connector expects you to create a connection using a group key. Here you have to input the group key of the Peakboard Hub Online group, you want to connect with Power Automate. Notice that only the Peakboard Boxes belonging to this group will be connected to Power Automate. 

### Access Peakboard Hub Online group key
On the Peakboard Hub Online website navigate to user icon in the top right corner like described on our [help site](https://help.peakboard.com/hub/en-hub_connectpbdesigner.html). Click it and select 'Show Group Key'. Now a pop-up containing your group key should show up. 


## Supported Operations
The connector supports the following operations:
* `Receive alerts`: Receive alerts, sent from a Peakboard Box, in Power Automate

## Known issues and limitations
Only alerts from Peakboard Boxes belonging to the Peakboard Hub Online group you connected to Power Automate will be forwarded.