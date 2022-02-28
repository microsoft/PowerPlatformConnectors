## PUG Gamified Engagement

The PUG Gamified Engagement Connector for Dynamics 365 enables the first phase of a comprehensive and personalized gamified engagement program for driving customer loyalty. This app comes with out-of-the-box integration into D365 and enables D365 platform customers to create users within a loyalty framework, generate badges based on digital actions, and issue currency for answering calls to action. It is part of PUG's award-winning Picnic Customer Engagement Hub platform which quickly and cost-effectively enables many of the leading global brands to solve their growth-limiting business problems – such as brand apathy, disengagement, loyalty, poor insight and lost revenue. Through the application of advanced gameplay mechanics and motivational psychology, Picnic instantly turns any CRM, loyalty program, website, or application into a richly interactive user experience. Picnic delivers the highest call to action responses while creating deep and lasting emotional connections.


## Prerequisites

To use this connector, you must have an active PUG Picnic Engagement Hub solution. If you won't have an active PUG Picnic Engagement solution and want to learn more, please visit PUG Interactive.

 
## How to get credentials
 
Provide detailed information about how a user can get credentials to leverage the connector. Where possible, this should be step-by-step instructions with links pointing to relevant parts of your website. If your connector doesn't require authentication, this section can be removed.

Customers with an active Picnic Customer Engagement Hub solution will be provided credentials that will enable them to use the connector. Visit [www.puginteractive.com/contact_us](https://www.puginteractive.com/contact_us).

## Getting started with your connector
 
PUG Gamified Engagement gives some type of gamification outcome when some action or set of actions has been undertaken by a user. These actions are called "triggers" in Picnic. The "logic" that is used to express when outcomes occur is specified in a Rule. Triggers for actions can be any type of action that is undertaken in a 3rd party system, while Outcomes can be badges, currency, virtual goods.
 
Using the Connector involves:
1. Determining your business objectives and what types of actions your customers, audience members, and learners will help you reach these objectives.
2. Designing which gamification techniques will best work for your user base, and how these will be integrated into your site or application. More information on how to do this is available from PUG Interactive.
3. Using the Picnic Customer Engagement Hub Campaign Manager, set up rules and outcomes.
4. Integrate the specific features into your app by using the PUG Gamified Engagement Connector for Dynamics 365.
 

## Supported Operations
The connector supports the following operations:

* `Get_Player_Accounts`: Gets the list of Player Currency Accounts for a Player
* `Get_Account_Balance `: Gets the balance of a specific Currency Account for a Player
* `Add_Points `: Adds a specified amount of Currency to a specific Currency account for a Player
* `Remove_Points `: Removes a specified amount of Currency from a specific Currency account for a Player
* `Get_Batch `: Gets a Batch of Items for a Player to select from
* `Issue_Badge `: Issues a specific Badge to a Player
* `List_Player_Badges `: Gets a list of Badge instances a Player has
* `List_Badges `: Gets a list of Badges that are available to be issued to Players
* `Claim_Instance `: Marks a specific Badge instance as claimed
* `Get_Badge `: Gets the details of a specific Badge
* `Get_Badge_Instance `: Gets a Badge instance details that has been given to a Player
* `Get_Badges_Instances `: Gets a list of Badges instances that has been given to a Player
* `Add_Item_Instance `: Adds an instance of an item to a player







