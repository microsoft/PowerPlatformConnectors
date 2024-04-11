# QPP NextGen
Quark Publishing Platform (QPP) NextGen is a cloud-based component content management system (CCMS) for automating the publishing of highly regulated enterprise content. The QPP NextGen connector helps move data from your content ecosystem in both directions to simplify processes and accelerate time to market.


## Publisher: Quark Software Inc.
https://www.quark.com/

## Prerequisites
 - You need a QPP-NG environment (v3.3 above) and QPP-NG licenses to use our [Connector](https://www.quark.com/products/quark-publishing-platform).
 - You need to have an existing client (Connection), preferably separate, created on the tenants for which they want the actions and triggers of this connector to interact. More details can be found in the Obtaining Credentials section.
 - Understand how to use power automate and get value using dynamic content or from other actions response using query creator.

## Supported Operations
Below is the list of operations supported by the QPP-NG connector – 

### Get User by Name
This operation retrieves user information based on the username. 

### Is Group Member
This operation checks if a user is a member of a specified group based on user Id and group Id.

### Get Group Members
This operation will return a list of users and all their metadata specific to the group Id.

### Get Group By Name
This operation will return a group (group details) specific to the group name.

### Get User
This operation retrieves detailed information about a specific user. 

### Get Asset
This operation fetches information about a specific asset. More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_getAsset).

### Get Collection Groups
This operation retrieves the routing groups associated with a collection, workflow, and status. More information on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_getCollectionGroups).


### Set Attribute Values
This operation sets the attribute values for a specific entity or object. Please use Legacy designer view for best experience as there may be issues in new designer due to ongoing development. More information on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_setAttributeValues).

| Parameter          | Definition                                                     | Example                                                         	   |
|--------------------|---------------------------------------------------------------|---------------------------------------------------------------------|
| Impersonation User | Username for the QPP NG user that needs to be impersonated.	 | admin                                                        	   |
| assetId            | QPP platform Asset Id.                                        | {Asset Id here}                                              	   |
| Attribute Array    | An array of attributes or Single attribute                    | Click on the Switch to Detail Input button and paste the JSON code. |

Copy and paste the below JSON format using the online JSON compiler to the Body param in the SetAttributeValues action. 

The below example is to set Routed to Value to an asset
```yaml
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
]
```

`Note:` Please replace the values in the above example with your original, personalized information –
 - **"value": "DocumentToBeCheckedInName.xml":** Full name for the asset to be checked-in
 - **"id": 1:** Collection Id where the asset can be checked-in
 - **"name": "Home":** Collection name here where the asset can be checked-in
 - **"id": 110452:** Id of the content type for which the asset needs to be checked-in as
 - **"name": "Smart Document":** Name of the content type for which the asset needs to be checked-in as
 - **"value": "{DocumentToBeCheckedInName}":** Original name of the asset, if any
 - **"value": "xml":** extension required here 

### Get Asset Download Info
This operation provides download information for a specific asset, which can be used together with Download Blob Action to down the asset stream. More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_getAssetDownloadInfo).

### Get Child Asset Relations
This operation retrieves the child asset relations for a specified parent asset. More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_getChildAssetRelations).

### Get Attribute Values by Name
This operation retrieves attribute values based on the attribute name. More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_getAttributeValuesByName).

### Upload File (Blob)
This operation uploads a file or binary large object (Blob) to the system using the specified context and file identifier. It is mostly used to upload files into an existing blob location, often made by InitCheckin action. It further returns ContextId and FileIdentifier along with the blob upload URL. Please be careful as it’s time-bound and will expire in 120 secs. The input can be converted to XML if the document to be checked in is an XML file for Input Stream Resource.
Examples of Param value for this action are given below – 
 - **File Identifier:** _split(outputs('Initiate_New_Check-in')?['body/uploadUrl'],'/')[5]_
 - **Context Id:** _split(outputs('Initiate_New_Check-in')?['body/uploadUrl'],'/')[4]_ 

More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_upload).

### Get Token as per Grant Type
This operation obtains an access token based on the specified params and grant type. This action is provided to fetch tokens for actions like download blob as input param, as it doesn’t use the authentication process as other actions. 
 - **Client Id:** You can navigate to your desired QPP-NG tenants **Admin** app and then **Settings > Connections**. Here they can create a new connection or use existing ones. Just enter the Connection name for Client Id. 
 - **Client Secret:** Like Client Id, navigate to **Connections** and just enter **Secret** for the existing connection. Regenerate the secret if you don’t have it. Secrets should be stored for existing connections securely, managed by admin. Check with admin before regenerating the secret. 
 - **Grant Type:** Keep this value as "password".
 - **Username:** Username for existing QPP-NG user for which connection is being used.
 - **Password:** Password for the QPP-NG user.

 ### Download File (Blob)
This operation downloads a file or Blob from the system using the specified context and file identifier. This action is mostly used in conjunction with _getAssetDownloadInfo_ and _publishAsset_ actions as they return the blob location and other attributes in the response. With the help of this action, you can download the asset from the blob. Just like Upload, it’s a 2-step process. Please be careful as it’s time-bound and will expire in 120 secs. 

More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_download).

To use this action, provide the following param's – 
 - **Auth Token for Header:** This action is dependent on the **Get Token as per Grant Type** action to get an authentication token for this param. Considering this action is added before Download (Blob) action and you have kept its name as Get **Token as per Grant Type**, then add this value in the Expression editor next to dynamic content: _outputs('Get_Token_as_per_Grant_Type')?['body/access_token']_. For more information, refer to the **Get Token as per Grant Type** section.
 - **Context Id:** Can be directly added through the previous action as dynamic content. Like GetAssetDownloadInfo will show context Id as dynamic content.
 - **File Identifier:** Need to use an Expression to extract information from the download URL of the previous action. Let's say the previous action is getAssetDownloadInfo, then the expression will become like _split(outputs('Get_Asset_Download_Info')?['body/downloadUrl'],'/')[5]_. 

### Get Paginated Query Results with Filters
This operation retrieves paginated query results based on specified filters. This action is used to perform search queries on the QPP-NG platform, usually to filter and get data. For example, default values in action itself use Query Condition param to query and filter assets on QPP-NG based on Attribute Id with a particular value. You can further change this condition to customize their search. Sort info param is necessary and has a default value added to sort the search result. All the prefilled data is required, the rest can be skipped.

More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_getPaginatedQueryResultsWithFilters).

Note: If there are any Issues after using this action, like not being able to add params, some params missing, or trouble saving or running the flow. Try the old Power Automate UI.

### Get Workflow by Name
This operation retrieves information about a specific workflow by its name.

### Get Attributes by Name
This operation retrieves attributes based on their names on the QPP server. More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_getAttributeByName).

### Create Attribute
This operation creates a new attribute in the system, returns its ID, and publishes a success message. The minimum params required to create an attribute are prefilled in the action default values. By default, a Text attribute is created. Other params can be added to customize the request. Value Type 1 is for the text, 6 for Boolean, etc.

More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_createAttribute).

### Create List Attribute
This operation creates a new list attribute in the system, returns its ID, and publishes a success message. The minimum params required to create an attribute are prefilled in the action default values. By default, a List attribute is created. Other params can be added to customize the request. Select the existing Domain list from the drop-down.

More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_createAttribute).

### Add Attribute to Content Types
This operation adds an attribute to specified content types. More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_addAttributeToContentTypes).

### Get Content Types Info by Name
This operation retrieves information about content types based on their names. More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_getContentTypesInfoByName).

### Create Query
This operation creates a new query in the system with its definition and details. Please use Legacy designer view for best experience as there may be issues in new designer due to ongoing development. More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_createQuery).

Check the table below for the reference purposes –

| Parameter                | Definition                                                                                                         | Example                                                            |
|--------------------------|-------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------|
| Impersonation User       | Username for the QPP NG user that needs to be impersonated.                                                       | admin                                                               |
| Query Name               | QPP platform Asset Id.                                                                                            | {Asset Id here}                                                     |
| Query Display Sorting    | List of sorting settings. It takes information on what attributes the data from the search will be sorted and shown   | Click on the Switch to Detail Input button and paste the JSON code. |

Copy and paste the below JSON formatted using the online JSON compiler to the body param in the Query Display action. 

Click on the **Switch to Detail Input** button and paste the JSON code. 

```yaml
[
   {
        "columnId": 2,
        "width": 300,
        "attributeColumn": true
    }
]
```

Paste in Query condition. The set of attributes under Query Condition can be used to condition the platform along with the query. These filters are used to filter data from search results. In place of attributeId, set your attribute ID and then search against the expected value. 

```yaml
[
   {
        "logicalOperator": 1,
        "comparisonOperator": 1,
        "negated": false,
        "parameterized": false,
        "type": 1,
        "attributeId": 1531,
        "value": "SVVnOQW9MEqC9GN5mcsr4GQALPdn"
    }
]
```

Note: If there are any Issues after using this action, like not being able to add params, some params missing, or trouble saving or running the flow. Try the old Power Automate UI.

### Get All Queries
This operation retrieves all available queries in the system along with their definitions.  

### Get All Users
This operation retrieves information about all users in the system. 

### Get Publishing Status
This operation retrieves all available metadata for the assets being published currently. Possible return values are _PUBLISHING_IN_PROGESS_, _PUBLISHING_FAILED_, _PUBLISHING_COMPLETED_. This action is used with the Publish Asset Async action to keep checking the status of publishing. 

More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_getPublishingStatus).

### Publish Asset Async
This operation initiates the context Id for the asset that is being published using the Publishing Channel Id mentioned. This action along with Get Publishing Status can be used to keep checking the status of Publishing for the context Id. The context Id itself mentions the location where the published output will be available once the publishing process is completed.

More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_publishAssetAsync).

Follow the steps given below to configure fetching published output using Publish Asset Async, Get Publishing Status, and Download Blob operations together. 
 - Configure Publish Asset Async operation, provide Asset Id & Publishing Channel Id. 
 - Also create a string variable and name it - Publishing Status. 
 - Add a do-while OOTB operation and provide values as follows: 
    - **Loop Until:** Add dynamic content of Publishing status variable, select Is Equal from the middle drop-down. 
    - Set the value equal to as _(PUBLISHING_COMPLETE)_. 
    - Additionally, as per user, Timeout on **Do-until** can be set, count as **15**, Timeout as **PT3M**. 
 - Add Get Publishing Status from our connector, refer to the Get Publishing Status section to configure the operation. 
 - Add Set Variable operation and then provide the below values as an expression for this operation – _(body(‘Get_Publishing_Status’)?[‘publishingStatus’])_. This will fetch the Publishing Status and set it on the Publishing Status variable, which is also used to check against our loop-breaking condition of the while loop.
 - Now add a condition here and set the condition’s Expression as variables _(‘PublishingStatus’)_ is equal to _PUBLISHING_COMPLETE_. 
 - Now, for the True branch, add Get Token as per grant type operation. 
 - Add Download Blob operation. 
 - Now add the following expression in the Download Blob operation, since get Publishing Status doesn’t provide any dynamic content, use this expression for inputs. 
    - **Context Id:** _split(body(‘Get_Publishing_Status’)?[‘publishingOutputDownloadInfo’][0]?[‘downloadUrl’],’/’)[4]_ 
    - **File Identifier:** _split(body(‘Get_Publishing_Status’)?[‘publishingOutputDownloadInfo’][0]?[‘downloadUrl’],’/’)[5]_ 

Following the above steps, you will be able to fetch the published asset as a stream using async. 

### Get Parent Asset Relations
This operation retrieves the parent's asset relations for a specified child asset. In param, Child asset Id needs to be given whose parent info needs to be fetched. More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_getParentAssetRelations).

### Initiate Check-in and Update
This operation initiates the check-in and update process for a specific asset in QPP-NG. The asset needs to be checked out beforehand. Use checkout action before. More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_initCheckin). 

### Check Out
This operation checks out a specific asset for editing. Please use Legacy designer view for best experience as there may be issues in new designer due to ongoing development. More information can be found on the [Quark developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_checkOut). 

| Parameter              | Definition                                                                             | Example                  |
|------------------------|----------------------------------------------------------------------------------------|--------------------------|
| Impersonation User     | Username for the QPP NG user that needs to be impersonated.                            | admin                    |
| assetId                | Asset Id which needs to be checked out                                                 | {Asset Id here}          |
| AttributeVaue Array    | Paste the example exactly into the body of the action, after switching to raw view    | Example is shown below   |

```yaml
[
   {
        "attributeId": 16,
        "type": 1,
        "attributeValue": {
            "value": "c:/checkedoutfiles"
        }
    },
    {
        "attributeId": 34,
        "type": 1,
        "attributeValue": {
            "value": "REST-CLIENT"
        }
    }
]
```

### Init New Checkin
This operation initiates a new check-in process for an asset. The request will check-in a new QPP-NG asset, as per the given QPP attribute values array. For example, the Required Attribute values are Name, Collection, Extension, etc. to check-in an asset to the QPP-NG Platform. Upload Blob can be further used to upload files into the given blob location. 

Please use Legacy designer view for best experience as there may be issues in new designer due to ongoing development. 

More information can be found on the [Quark Developer portal](https://qppngsdk-rel3x.app.quark.com/Documentation/platformServices/restapi/api.html#Quark_application_API_3_0_0_SNAPSHOT_initNewCheckin).

| Parameter              | Definition                                                                                                                                                               | Example                    |
|------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------|
| Impersonation User     | Username for the QPP NG user that needs to be impersonated.                                                                                                              | admin                      |
| createMinorVersion     | Variable createMinorVersion should refer to True or False for creating a version for checking                                                                              | False                      |
| Asset                  | Contains asset metadata in the form of an array of attribute values. For example, required Attribute values are Name, Collection, Extension, etc. to check-in an XML asset   | The Example is shown below     |

```yaml
[
    {
        "attributeId": 2,
        "type": 1,
        "multiValued": false,
        "attributeValue": {
            "value": "DocumentToBeCheckedInName.xml"
        }
    },
    {
        "attributeId": 55,
        "type": 7,
        "multiValued": false,
        "attributeValue": {
            "id": 1,
            "name": "Home",
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
            "id": 110452,
            "name": "Smart Document",
            "domainId": 25
        }
    },
    {
        "attributeId": 33,
        "type": 1,
        "multiValued": false,
        "attributeValue": {
            "value": "{DocumentToBeCheckedInName}"
        }
    },
    {
        "attributeId": 11,
        "type": 1,
        "multiValued": false,
        "attributeValue": {
            "value": "xml"
        }
    }
]
```

**Note:** Please replace the values in the above example with your original, personalized information –
 - **"value": "DocumentToBeCheckedInName.xml":** Full name for the asset to be checked-in
 - **"id": 1:** Collection Id where the asset can be checked-in
 - **"name": "Home":** Collection name here where the asset can be checked-in
 - **"id": 110452:** Id of the content type for which the asset needs to be checked-in as
 - **"name": "Smart Document":** Name of the content type for which the asset needs to be checked-in as
 - **"value": "{DocumentToBeCheckedInName}":** Original name of the asset, if any
 - **"value": "xml":** extension required here

## Webhook-trigger

Fill in the required fields (default examples can be used). For example, if you want to listen to an event when an asset is added.
- **Impersonation User:** Username of the user on behalf of which trigger will be registered. You can mention any existing username here from the tenant for which your connection is set. 
- **Webhook Name:** Unique name that doesn’t already exist. To know existing registered webhooks for any QPP-NG tenant, you can add or delete webhooks action, if the connection to that QPP-NG tenant is correct, it will show you the list. 
- **Webhook Type:** Should be kept as Event. 
- **Object Type:** For listening to an Asset Added event, you need to select Asset here, similarly, if it was any other event like Attribute added. You should select Attribute. The first name of the event name should be set here. 
- **Change Type:** From the drop down, you should select the _ASSET_ADDED_ change type as per the example. But the value here should be as per the Object selected, like for object ASSET. Only acceptable values will be starting from ASSET for change type. 
- **Version:** Can be kept at 1.0 (webhook service version). 
- **Filter Criteria:** In case you want to filter events, then you can enter a value here in the following Lucene pattern (https://lucenetutorial.com/lucene-query-syntax.html). For example, to filter events of assets added for only smart document content type. The filter value will be Content\ Type:Smart\ Document, where Content\ type is a QPP-NG attribute, and it could also be any other QPP-NG attribute here for the asset. 

In case you want to update the webhook-trigger details or connections once the webhook has been already registered, then follow the steps below. This is a workaround, as the native way to update webhook-trigger will give you errors.
 - Delete the existing trigger, and create a new trigger from our connector with all the details. Once the flow is saved an error might be shown on the flow details page. If the error is (401:Unauthorized), then this can be ignored, as once you start Receiving events this will go away. 
 - Another way of updating webhooks can be to Click on your existing flow, go to flow details, and then click on turn off the flow.
 - Once the flow is turned off, open the flow, delete the old webhook-trigger, create a new one from our connector, fill out the new details, and then save the flow.
 - Now return to the flow details page and turn on flow.
 - In case you don't receive events, please check if there are any issues QPPNG tenant & webhooks, Also check if the credentials & webhook details entered are correct.

More information about Webhooks can be found on the [Quark Developer portal](https://developer.quark.com/v3/api/web-hook/).

## Obtaining Credentials
While creating a new connection with our connector, you will see 3 params. Details for acquiring each are shared below. Creating and managing QPP-NG connections should be done by the admin user. Please check with your QPP-NG environment admin. 

You can also name your credentials appropriately to easily identify them later. Go to **Connections** in Power Automate, search for the connector name, and edit the existing connection, then provide an appropriate name. 

 - **Client ID:** You can navigate to your desired QPP-NG tenants Admin app and then **Settings > Connections**. Here they can create a new connection or use an existing one. Just enter the connection name for Client Id. 
 - **Client Secret:** Like Client Id, navigate to **Connections** and just enter **Secret** for the existing connection. Regenerate the secret if you don’t have it. Secrets should be stored for existing connections securely, managed by admin. Check with admin before regenerating the secret. 
 - Host name: The host name for the QPPNG tenant you want to create connections to interact with our connector. example (yourtenantname.app.quark.com).Should be visible the URL, when you access your QPPNG environment.

After creating Credentials, you can test the credentials to see if credentials are correct.

Simple actions like getUser can be used from our connector in a manual flow, Test the flow with default fields.

While creating a connection for the first time, some error codes that can be faced are as below:
 - **Invalid host URL (500):** Check your host name for the QPPNG environment again, the client id and secret should belong to this connection.
 - **Unauthorized/unauthorized_client (401):** For Invalid client or Invalid client credentials, ensure the client Id and client secret are valid and belong to the host name being used.


## Getting Started
To get started, you can listen to a wide variety of events of Type Asset, Collections, etc. from your QPP-NG tenant using a built-in trigger. 
 - Create a new Automation flow in Power Automate. 
 - Select our QPP-NG Connector and then choose a trigger. 
 - Fill in the required fields. Refer to the Webhook-trigger section. 
 - Select the appropriate Connection for the QPP-NG tenant where you want to register webhook. You can also create new connections. Follow the Steps given in the **Obtaining Credentials** section. 
 - Save the flow. This will execute the registration request in the back. When you return to the flow details page(or click the back button) you should see a green underline at the top mentioning that everything went well and flow is ready to be triggered.
 - Once your flow runs, you can further get the webhook response from the flow run, and use the response to generate dynamic content. For this, you can use parse JSON OOTB action. 
 - In case this is the first webhook with this change type (event) registered on the QPP-NG tenant, you need to wait for 15 minutes before the webhook is activated, and then you can expect the flow to be triggered.

If there are errors in the flow checker with the webhook trigger, the Power Automate will show red in the flow checker. Please open it and check the error. Below are some of the errors you may encounter, and the steps to mitigate them. 

 - **Webhook with name already exists** 
 Either provide a unique name for the webhook or use the Delete Webhook operation with the same connection in another manual flow. Select the webhook with the same name and delete it. More information about Webhooks can be found on the [Quark Developer portal](https://developer.quark.com/v3/api/web-hook/).
 - **Bad Request(400)** 
 Please check the Params provided as per the documentation and error details. 
 - **Resource not found (404) & Location header is missing in subscribe request** 
 Please delete the flow, and then create a new webhook trigger and flow. If the flow is big, duplicate the flow, while enabling the flow and saving, Power Automate will try to do a fresh webhook registration. 
 

## Known Issues and Limitations
 - Currently, you won't be able to update params for an already registered webhook trigger. As a workaround, delete the current webhook operation from flow and add a new QPP-NG webhook trigger in its place. To delete the previously registered webhook from the QPP-NG environment, create a new manual flow with the delete operation, select the previous webhook from the drop-down, and run the flow. If this workaround doesn’t work and the flow is big, then duplicate the flow, and after updating webhook details, save and enable it. This will also create a new webhook registration successfully. 
 - If the QPP-NG webhook service can’t invoke the registered webhook-trigger for 3 times, then the webhook will be disabled. 
 - Saving certain actions with no params given (empty) gives an error (expect some value in the body). 
 - If this is the first webhook with this change type (event), you need to wait for 15 minutes before the webhook is activated and then you can expect the flow to be triggered.
 - These actions have been tested with Old Power Automate UI, If for some actions the UI is not behaving properly or not showing all params. Switch to the old UI from the top right toggle button.
 - Please use the Legacy designer view for the best experience as there may be issues with the new designer due to ongoing development.

## Frequently Asked Questions

### Can I use the actions and triggers with different QPP-NG environments? 
Yes, it’s possible. You just need to fill in the Host Name param as per the environment and use appropriate connections to that environment. 
### Can you help us if we get stuck on something or have doubts? 
Absolutely. Just email [email protected] and make sure you mention the Microsoft Power Automate Connector when you describe your problem. 
### Where can I get more information on the actions and endpoints? 
More information on the various Quark APIs can be found [here](https://developer.quark.com/). Just navigate to the Content Platform tab. As this connector only contains a subset of the actual API possible for the QPP-NG platform, this site can be used to explore and request more actions and endpoints via logging a feature request at [email protected]. 

## Deployment Instructions
 - Clone the PowerPlatformConnectors GitHub repository. 
 - Run the paconn login, then follow the authentication steps. 
 - Once authenticated, run the following command for Quark Publishing Platform - NextGen Connector artifacts, run _paconncreate-s [path to settings.json file]_ 
 - Select the target environment for your connector. 
 - Get the access token to create the connection for the QPP-NG environment. 