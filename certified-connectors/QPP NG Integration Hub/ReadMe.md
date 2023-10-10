# Quark Publishing Platform NextGen
The Quark Publishing Platform NextGen connector offers a comprehensive suite of actions for seamless integration to the Quark Publishing Platform NextGen (QPP NG). Effortlessly retrieve and manage user metadata, asset information, group memberships, attribute values, and more. Streamline your processes by leveraging a wide range of actions, from checking group memberships to uploading and downloading files. Simplify your QPP NG interactions and enhance your workflows with this versatile and powerful connector.


## Publisher: Quark Software Inc.
https://www.quark.com/

## Prerequisites
- The user Needs a QPPNG enviroment and QPPNG licenses to use Our Connector.
- The User also needs a No expiry PAT to create Connection possibly with Admin priviliges for the Host Env. This can be achived by contacting Support team.(https://quarksoftware.my.site.com).

## Supported Operations
Required. Describe actions, triggers, and other endpoints.​
### GetUserByName
Retrieve user metadata based on the username.

### IsGroupMember
Check if a user is a member of a specific group based on user ID and group ID.

### GetUser
Fetch metadata for a specified user.

### GetAsset
Obtain metadata for a specific asset.

### GetCollectionGroups
Retrieve applicable routing groups for a given collection, workflow, and status.

### SetAttributeValues
Set attribute values for the QPP Platform.

|Parameter|Defintion|Example|
|:----|:----|:----|
|name|Url for the Host environment that This Action needs to points to.|{HostNameHere}.app.quark.com|
|assetId|QPP Platform Asset Id.|{Asset Id here}|
|Attribute Array|An array of attributes or Single attribute|Click on the Switch to detail Input buttom resmbling a file.
Copy and paste the Below json formatted using online json compiler to the Body param in SetAttributeValues action after removing the comments inside //
//Below example is to set Routed to Value to an asset
[
  {
    "attributeId": 25,
    "type": 7,
    "multiValued": true,
    "attributeValue": {
      "domainValues": [
        {
          "id": 5015			//Id For the User or Group Id can be given here. Instead of Id "name" property can be added and used
        }
      ]
    }
  }
]
//Other examples are below
//To set Status for an Asset
[
    {
        "attributeId": 24,
        "attributeName": "Status",
        "type": 7,
        "multiValued": false,
        "attributeValue": {
            "id": 1482,
            "name": "Create",
            "domainId": 6
        }
    }
]|

### GetAssetDownloadInfo
Get a list of users and their metadata specific to a group for asset download.

### GetChildAssetRelations
Obtain relations of child assets for a given asset.

### GetAttributeValuesByName
Retrieve attribute values using attribute names.

### Upload File (Blob)
Upload a blob for a File using the specified context and file identifier. This Action is mostly used to Upload Files into an existing blob location, often made by InitCheckin action, it further returns ContextId and FileIdentifier along with blob upload url. Carefull as its Time bound and will expire in 120 secs. The input can be converted to xml if the document to be checked in is an xml file for Input Stream Resource.

### Download File (Blob)
Download a blob for a file using the specified context and file identifier. This Action is mostly used in conjuction with getAssetDownloadInfo, Publish Asset action as they return the blob location and other attributes in response. Its with the help of this action users can download the asset from the blob. Just like upload its a 2 step process.Carefull as its Time bound and will expire in 120 secs.

### GetPaginatedQueryResultsWithFilters
Get paginated query results along with query details and filters.This Action is used to perform search queries on the QPPNG platform, usually to filter and get data. Given example as default values in action itself is using Query Condition param to query and filter assets on QPPNG based on Attribute Id with a particular value. Users can further change these condition to Customize their search. Sort info param is necessary and has a default value added to sort the search result. All the prefilled data is required ,Rest can be skipped.

### GetWorkflowByName
Retrieve metadata for a specified workflow by its name.

### GetAttributesByName
Retrieve metadata for existing attributes with specific names on the QPP Server.

### CreateAttribute
Creates a new attribute, return its ID, and publish a success message. The Minimum Required params to create an attribute is prefilled in the action default values. By deafult a text attribute is created. Other params can be added to customize the request. Value Type 1 is for text , 6 is for boolean etc.

### AddAttributeToContentTypes
Update attribute mappings for content types and their children using specified attribute maps.

### GetContentTypesInfoByName
Retrieve information about a content type using its name.

### CreateQuery
Create a new query with its definition and details.

|Parameter|Defintion|Example|
|:----|:----|:----|
|Host Name|Url for the Host environment that This Action needs to points to.|{HostNameHere}.app.quark.com|
|Query Name|QPP Platform Asset Id.|{Asset Id here}|
|Query Display Sorting|List of sorting settings. It takes information on what attributes the data from search will be sorted and shown|Click on the Switch to detail Input buttom resmbling a file.
Copy and paste the Below json formatted using online json compiler to the Body param in SetAttributeValues action after removing the comments inside //[
  {
    "columnId": 2,
    "width": 300,
    "attributeColumn": true
  }
]|
|Query Condition|The set of attributes under Query Condition can be used to create conditon in Platform along with query, These filters are used to filter data from search results|[
  {
    "logicalOperator": 1,
    "comparisonOperator": 1,
    "negated": false,
    "parameterized": false,
    "type": 1,
    "attributeId": 1531,
    "value": "SVVnOQW9MEqC9GN5mcsr4GQALPdn"
  }
]|

### GetAllQueries
Retrieve all queries along with their definitions.

### GetAllUsers
Fetch metadata for all users.

### PublishAsset
This request returns the stream after publishing an asset into a specified format using the publishing channel. Download Blob can be used then to further download the Published Output.

### GetParentAssetRelations
This request will the list of relation of an parent asset relation. In param Child asset id needs to be given whos parent info needs to be fetched.

| Parameter            | Defintion                                                                           | Example                                                                                                                                                                                                                                             |
| -------------------- | ----------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| new_url                 | Url for the Host environment that This Action needs to points to.                   | {HostNameHere}.app.quark.com                                                                                                                                                                                                                        |
| assetId              | Asset Id which needs to be checked Out                                              | 50                                                                                                                                                                                                                                                  |
| AttributeValue Array | Paste the example exactly into the body of the action , after switching to raw view | [ {<br>   "attributeId": 16,<br>   "type": 1,<br>   "attributeValue": {<br>   "value": "c:/checkedoutfiles"<br>  }<br> },<br>  {<br>   "attributeId": 34,<br>   "type": 1,<br>   "attributeValue": {<br>   "value": "REST-CLIENT"<br>  }<br> }<br>] |

### Initiate Check-in and Update
Request will check-in and Update an already exiting asset in QPPNG. Asset Needs to be checked Out before hand. Use checkout action before.

### CheckOut
Marks the asset with the given assetId as Checked Out. This will allow the user to check-in an updated version of the asset with Initiate Check-in and Update action. User needs to add one more required attribute value in the body to checkout asset. Provide value 36 for attribute id, 1 for type and 

### InitNewCheckin
Request will check-in a new QPPNG asset, as per given Qpp attribute values array,For example Required Attribute value are Name,Collection,extension etc to check-in an asset to QPPNG Platform. Upload Blob can be further used to upload file into the given blob location.

|Parameter|Defintion|Example|
|:----|:----|:----|
|name|Url for the Host environment that This Action needs to points to.|{HostNameHere}.app.quark.com|
|createMinorVersion|Variable createMinorVersion should refer to true or false for creating version for checking.|False|
|Asset|Contains asset metadata in the form of array of attribute values.For example Required Attribute value are Name,Collection,extension etc to check-in an xml asset|Copy and paste the Below json formatted using online json compiler to the Body param in Init check-in action after removing the comments inside //
		[
		{
			"attributeId": 2,
			"type": 1,
			"multiValued": false,
			"attributeValue": {
				"value": "DocumentToBeCheckedInName.xml"			//Full name for the Asset to be checked In.
			}
		},
		{
			"attributeId": 55,
			"type": 7,
			"multiValued": false,
			"attributeValue": {
				"id": 1,											//Collection Id here where the asset can be checked In.
				"name": "Home",										//Collection Name here where the asset can be checked In.
				"domainId": 19,
				"accessibleChildrenAvailable": true,
				"collectionTemplate": false,
				"collectionClass": 1416
			}
		},
		{
			"attributeId": 62,
			"type": 7,
			"multiValued": false,
			"attributeValue": {
				"id": 110452,										//Id of Content Type for which the asset needs to be checked-in As.
				"name": "Smart Document",							//Name of Content Type for which the asset needs to be checked-in As.
				"domainId": 25
			}
		},
		{
			"attributeId": 33,
			"type": 1,
			"multiValued": false,
			"attributeValue": {
				"value": "{DocumentToBeCheckedInName}"				//Original Name of the asset if any.
			}
		},
		{
			"attributeId": 11,
			"type": 1,
			"multiValued": false,
			"attributeValue": {
				"value": "xml"										//xtension required here
			}
		}
	]|



## Obtaining Credentials
The User needs a No expiry PAT to create Connection possibly with Admin priviliges for the Host Env. This can be achived by contacting Support team.(https://quarksoftware.my.site.com) . This needs to be done by a System admin and Kept private. The format to enter API token is "Bearer {PAT TOKEN HERE}"

## Getting Started
To Get Started Users can Listen to a wide variety of events like Asset, Collection etc using Built in Trigger. 
- Create a new Automation.
- Select The QPPNG trigger.
- Fill out the Necessary Feilds (Default Examples can be used).
- A No expiry PAT will Also be required to create Connection.

## Known Issues and Limitations
- Wont be able to Update an Already Registered webhook Trigger like Updating Params etc. As Workaround use Use delete webhook action to Delete the existing webhook and Create A new Flow with Webhook.If the Flow is big, Then Export and Import via a package.
- Authentication is PAT based currently , In future authentication will be Oauth 2.0 based. Clients will need a PAT access token to create an Initial Connection.The Current version of connector will not work Properly for Oauth 2.0 based QPPNG enviroments as it will not support No expirey token and connections will expire in 360 seconds.
- Saving Certain Actions with No Params given(Empty) Gives Error (Expects Some value in Body).

## Frequently Asked Questions

### Can I use the actions and Triggers with Different QPPNG enviroments?
Yes its possible, Users just needs to fill Host Name Param As per the Enviroment, and Use appropriate Connections to that enviroment.
### Can you help us if we get stuck or have Questions
Absolutely. Just email support_cases@quark.com and make sure you mention the Microsoft Power Automate Connector when you describe your problem.
### Where can i get more information on the actions and endpoints?
More information on the variaous quark API's can be here, https://developer.quark.com/ Just navigate to Content platform Tab.As this connector only contains a subset of the actual API possible for QPPNG platform, this site can be used to explore and request for more actions and endpoints via logging a feature request at support_cases@quark.com.

## Deployment Instructions
- Clone the PowerPlatformConnectors GitHub repository
- Run paconn login, then follow the authentication steps
- Once authenticated,  Run the folllowing command for Quark Publishing Platform NextGen Connector artifacts, run paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
- Select the target environment for your connector.
- Get the Access token to create the connection for the QPPNG enviroment.