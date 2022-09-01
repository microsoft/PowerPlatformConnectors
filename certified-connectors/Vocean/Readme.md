# Vocean Connector
---
Vocean is a platform for digital co-creation with the vison to _let every voice matter_ and to unleash the power of the many. With Vocean you can include more individuals in innovative processes and by doing so you will get a better basis for making decisions and a wider support for the result. For more information, visit [Vocean](https://vocean.com/ "Vocean website"). With the API provided by Vocean you can retrieve information that participants have contributed with in different activities.

## Publisher: Vocean AB

## Prerequisites
---
You must be a member of a Vocean account, appointed with a Vocean Premium Leader license. If you aren't a member of a Vocean account yet you can easily sign up for an account of your own [here](https://vocean.com/pricing/ "Sign up for Vocean").
	
## Restrictions
---
Within Vocean you can be a member of several accounts, the Vocean Connector allows you to reach all your accounts. However, to be able to retrieve data you must be a leader of the specified activity and have Vocean Premium Leader rights in that account. (If you have a Leader license in a Vocean account you can access its [members page](https://app.vocean.com/#/network/members) and see if you have the Premium Leader license flag enabled on your membership.)

## Geting Started
---
* Create an activity. Invite others to participate and await their contributions. 
* Call the relevant action from the Connector (e.g. `GetIdeas` for an Innovate activity) and provide the activity id. The activity id is found from the activity's URL as the trailing guid after _/activity/_.

## Supported Operations
---
The connector supports the following actions:
* `GetIdeas`: Get all the ideas for a specified Innovation activity in Vocean
* `GetVotes`: Get all the votes for a specified Vote activity in Vocean
* `GetExploreResponses`: Get all the responses for a specified Explore activity in Vocean

## Known Limitations
---
The Data API has restrictions in terms of the size of the Response and returns a maximum of 1000 _items_. Note that the number of votes matches the number of respondents in Vote activities, while each idea and each response to a question in an Explore activity count as a separate item and the number of _respondents_ might be far less than 1000 if each respondent gives more than one idea or if there are more than one question in the Explore activity. (For Explore responses the maximum is actually slightly increased if 1000 is not evenly dividable by the number of questions in the activity.)