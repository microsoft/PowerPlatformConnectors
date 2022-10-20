# Vocean Connector
---
Vocean is the space for co-creation. This is where ideas meet and grow strong together. Here you can harness the intelligence of collectives, empower people and determine the way forward. For more information, visit [Vocean](https://vocean.com/ "Vocean website"). With the API provided by Vocean you can retrieve information that participants have contributed with in different activities. It is also possible to instead let a trigger inform you, for example, when an idea is added, or you are added to a Vocean activity.

## Publisher: Vocean AB

## Prerequisites
---
You must be a member of a Vocean space, appointed with a Vocean Premium Leader license. If you are not a member of a Vocean space yet you can easily sign up for a space of your own [here](https://vocean.com/pricing/ "Sign up for Vocean").
	
## Restrictions
---
Within Vocean you can be a member of several spaces, the Vocean Connector allows you to reach all your spaces where you have Vocean Premium Leader rights. However, to be able to retrieve data from an activity you must also be a leader of the specified activity. (If you have a Leader license in a Vocean space you can access its [members page](https://app.vocean.com/#/network/members) and see if you have the Premium Leader license flag enabled on your membership.)

## Getting Started
---
* Call the relevant action from the Connector (for example, `Get ideas` for an Innovate activity) and in relevant cases choose the activity in the dropdown. Note: you then need to first choose the network to get a dropdown for activities. 

When needed: 
* Create an activity.  
* Invite others to participate and await their contributions. 

## Supported Operations
---
The connector supports the following actions:
* `Get spaces`: Get available spaces in Vocean
* `Get activities`: Get available activities in Vocean, for the specified network
* `Get ideas`: Get all the ideas and related data from a specified Innovate activity in Vocean
* `Get votes`: Get all the votes from a specified Vote activity in Vocean
* `Get explore responses`: Get all the responses from a specified Explore activity in Vocean
* `Add idea`: Create an idea in a specific Vocean activity
* `Add ideas`: Create ideas in a specific Vocean activity

The connector supports the following triggers:
* `When an idea is added or interacted with`: Triggers when an idea is added, or something happens to an existing idea
* `When an activity's, or a workboard's, availability changes for the current user`: Triggers when something happens to an activity, or a workboard, that affects the current user's possibility to interact with the activity or workboard

## Known Limitations
---
The Data API has restrictions in terms of the size of the Response and returns a maximum of 1,000 _items_ for the ‘Get’ actions. Note that the number of votes matches the number of respondents in Vote activities, while each idea and each response to a question in an Explore activity count as a separate item and the number of _respondents_ might be far less than 1,000 if each respondent contributes with more than one idea or if there are more than one question in the Explore activity. (For Explore responses the maximum is actually slightly increased if 1,000 is not evenly dividable by the number of questions in the activity.)

The dropdowns for spaces and activities are limited to 100 items.