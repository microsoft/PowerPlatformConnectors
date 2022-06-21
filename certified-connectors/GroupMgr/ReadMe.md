# GroupMgr

A powerful set of tools, for both IT managers and users, to efficiently create, manage, browse and analyse all groups in your Microsoft 365 environment. People love using Microsoft Teams. However, every time a team is created, a ‘group’ and a lot of underlying assets like mailboxes, SharePoint sites, planners and documents are created as well… Managing all this content is near impossible. GroupMgr is a governance tool which gives you that much needed overview of all existing groups, their users and related assets.

Our app will manage Microsoft 365 groups within your tenant. It provides users with an overview of all the currently available groups and allows them to easily create new ones. In addition, admins will be able to connect approval flows to the creation of new groups. This is done by connecting an approval flow to a group type. Besides, relationship between groups and group types will give a clearer overview of all available groups and makes it easier for users to filter or search them. If a user creates a new group, linked to a group type with an approval flow, the flow must be completed first, before the group itself will be created. In order to set up such approval flows, our custom connector must be used.

The connector will place a webhook on the *Creation* and *Update* part of the CRUD which will trigger the connector when needed. The connector consists of two parts, a trigger and an action. The trigger will fire when an group is created or updated. The action is used to send a reply with the approval result. Based on this approval result, the Microsoft 365 group will either be created or declined.

## Pre-requisites
You will need the following to proceed:
* A Microsoft Power Automate plan with custom connector feature,
* A GroupMgr license,
* GroupMgr API authentication details. 

## Get the API key for the GroupMgr
- [GroupMgr website](https://groupmgr.com) - Further details on how to get the API key.

## Support, policies and more infomartion  : 
You can find these resources [here:](https://groupmgr.com/support)