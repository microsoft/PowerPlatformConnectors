Using the Mavim iMprove Power Platform connector you are able to Create; Retrieve; Update and Delete topic. Updating fields and Create; Retrieve; Update and Delete relations.

The Mavim topics are stored in a hierarchical manner. Where the "Mavim database" is called the root topic. Below the root topic three children exists, where the Yellow cabinet is the working area; The green cabinet contains the created (read only) versions (as children) and the purple cabinet contains the (read only) imported versions (as children).

Only in the Yellow cabinet you can change the topics; fields and relations. All the topics; relations and fields have the unique identifier called DCV code. Only the root topic has a static indentifier (d0c2v0). All other items getting a generated number provided by the Mavim database.

Please note that for some topics (like "Recycle Bin" and "Relation Categories") business rules apply.

To see the data in a more visual way you could login into the Mavim iMprove web application using your credentials also used for the Microsoft Power Platform and the following URL: https://improve.mavimcloud.com/

## Prerequisites
To be able to use Mavim Power Platform connector you require to have a Mavim Cloud database and an Azure Active Directory login from your own tenant.
 - You need the Mavim Cloud database identifier (dbid);
 - The user account should have an valid e-mail address linked to his account.

## How to get credentials
During the on-boarding process we ask for an Azure Active Directory login name. This will be registered as the admin user of the Mavim cloud database. When the Cloud database is ready, Mavim will provide the database identifier also called dbid.

## Getting started with your connector
The action GetRootTopic is to get the starting point of you hierarchical tree. This will return the root topic and all its properties. Simular actions are GetTopic for getting a specific topic; GetTopicChildren to get all the children topics; GetSiblings to get all the siblings of a topic.

All calls to the actions require the dbid to connect to your own Mavim database and a langage code. Supported data languages for the API are English (en) and Dutch (nl).
Topics in the Mavim database have an unique DCV-code, this is the indentifier of a specific topic also called topicid.

### MavimiMprove.GetTopicRoot(dbid,datalanguage);
Get the root topic.
#### Response (example)
```
{
    "business": {
        "canCreateChildTopic": false,
        "canCreateTopicAfter": false,
        "canDelete": false,
        "isReadOnly": true
    },
    "dcv": "d0c2v0",
    "hasChildren": true,
    "icon": "TreeIconID_DbGreen",
    "isInRecycleBin": false,
    "name": "Mavim Database",
    "orderNumber": -1,
    "typeCategory": "mavimElementContainer"
}
```
### MavimiMprove.GetTopic(dbid,datalanguage,topicId)
Get a specific topic by the DCV indentifier.
#### Response (example)
```
    "business": {
        "canCreateChildTopic": true,
        "canCreateTopicAfter": true,
        "canDelete": true,
        "isReadOnly": false
    },
    "dcv": "d16730231c28607v0",
    "hasChildren": true,
    "icon": "TreeIconID_YellowOpen",
    "isInRecycleBin": false,
    "name": "PowerApps topic",
    "orderNumber": 20,
    "parent": "d16730231c24017v0",
    "resources": [
        {
            "Value": "subTopics"
        },
        {
            "Value": "fields"
        }
    ],
    "typeCategory": "mavimElementContainer"
}
```
### MavimiMprove.GetTopicChildren(dbid, datalanguage, topicId)
Get the children topics of the topic
#### Response (example)
```
[
    {
        "business": {
            "canCreateChildTopic": true,
            "canCreateTopicAfter": true,
            "canDelete": true,
            "isReadOnly": false
        },
        "dcv": "d16730231c28632v0",
        "hasChildren": false,
        "icon": "TreeIconID_YellowOpen",
        "isInRecycleBin": false,
        "name": "Subtopic 1",
        "orderNumber": 1,
        "parent": "d16730231c28607v0",
        "typeCategory": "unknown"
    },
   {
        "business": {
            "canCreateChildTopic": true,
            "canCreateTopicAfter": true,
            "canDelete": true,
            "isReadOnly": false
        },
        "dcv": "d16730231c28633v0",
        "hasChildren": false,
        "icon": "TreeIconID_YellowOpen",
        "isInRecycleBin": false,
        "name": "Subtopic 2",
        "orderNumber": 2,
        "parent": "d16730231c28607v0",
        "typeCategory": "unknown"
    }
]
```

### Other supported actions are
#### Topic
- CreateTopicAfter
- CreateChildTopic
- UpdateTopic
- DeleteTopic

#### Topic structure
- MoveTopicToTop
- MoveTopicToBottom
- MoveTopicUp
- MoveTopicDown
- MoveTopicLevelUp
- MoveTopicLevelDown
- GetPathToRoot

#### Fields
- GetTopicFields
- GetFieldByDcvAndFieldsetId
- UpdateFields
- UpdateBooleanSingleField
- UpdateTextSingleField
- UpdateTextMultiField
- UpdateNumberSingleField
- UpdateNumberMultiField
- UpdateDecimalSingleField
- UpdateDecimalMultiField
- UpdateDateSingleField
- UpdateDateMultiField
- UpdateListSingleField
- UpdateRelationshipSingleField
- UpdateRelationshipMultiField
- UpdateRelationshipListField
- UpdateSingleHyperlinkField
- UpdateMultiHyperlinkField

#### Relations
- GetTopicRelations
- SaveRelation
- DeleteRelation
- GetRelationCategories

#### Other
- GetTopicCharts
- GetTopicTypes
- GetTopicIcons
- Version

## Known issues and limitations

- Updating a topic description can only be done by our WOPI (Office for the web) API. On the My Mavim Portal you can find more information under the Mavim Developer API.
- The Mavim iMprove API supports the data languages English and Dutch.
- At the moment we only allow one tenant to access the Mavim cloud database. Guest users are not supported as of now.

## Common errors and remedies

### 401 Unathorized
- Check if the users has access to the Mavim cloud database. Go to the user management of Mavim iMprove to ensure the correct users have access.
- Check if the user account has a valid e-mail adres.

### 404 Not found
- Check if the topic exists in the Mavim cloud database. You could use Mavim iMprove or the Mavim Manager to valdidate if it still exists.

## FAQ

**Where can I add additional users?**
Add additional users using the user management section in the Mavim iMprove application. Here you can add all the users that need access. Providing access to Mavim iMprove means that the user is allowed to use Mavim iMprove and the access to the API.

**Serivce/API accounts**
Access to the Mavim database is only allowed by connecting with a named user. Mavim does not provide a clientid and secret authentication method to connect to the API.
