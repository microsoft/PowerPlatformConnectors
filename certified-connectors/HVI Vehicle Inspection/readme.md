## HVI Inspection Specific Vehicle
## Name of connector 
Particular Vehicle Inspection Data
HVI, is an inspection information capture technology. HVI customer can bring inspection and maintenance process data to Microsoft apps.
HVI collects fleet breakdown information in Realtime and via the custom connector, such information can be retrieved.
All inspection information is available for any fleet vehicle or construction equipment. 

## Description( Information about Hvi NEED TO CHECK)
HVI, is an inspection information capture technology. HVI customer can bring inspection and maintenance process data to Microsoft apps. 
HVI collects fleet breakdown information in Realtime and via the custom connector, such information can be retrieved.
All inspection information is available for any fleet vehicle or construction equipment. 

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature.
* HVI Subscription Account
* Authentication API KEY(Which is basically HVI Account password).

## How to get an Hvi Account?
-Visit the HVI Official website https://heavyvehicleinspection.com/
-Sign up free for HVI account. Click on Web portal login button on the top-right corner.
-You can login with Gmail or manually type the credentials.
-You are all set with a new Hvi Account.


## Supported Operations
The connector supports the following operations:
* `Particular Vehicle Inspection Data`: Get all the Inspection data of a particular vehicle in a time Frame that is provided.

 
## Parameters
* `sv`: Default value is 1.0.
* `Email ID`: User HVI email that is unique for every user.
* `API KEY`: It is provided by the HVI support Team at the time of account creation that is used to uniquely identify the users.
* `Vehicle Number`: Vehicle number for which inspection data is needed.
* `Start Date`: Starting Date from which user want the inspection detail of the Vehicle.
* `End Date`: Ending Date from within which user want the inspection detail of the vehicle. 
