# Seismic Power Automate Configuration connector
The Seismic Power Automate Configuration connector includes actions related to content properties, teamsite data, and privacy management settings.

## Publisher: Seismic​

## Prerequisites
You need a Seismic Premium user account for the tenant with Full Control (System Administrator) permissions.

## Supported Operations

### Content properties - Get values of a content property
Gets the list of content property values for the specified content property.

### Content properties - Add values to a content property
Adds one or more content property values to an existing content property.

### Content properties - Get all
Gets the properties for all built-in and custom content properties configured for the specified TeamSite.

### Content properties - Add a content property
Adds a content property and associates it to an existing TeamSite.

### Email opt-outs - Get list
Gets a list of email address opt-outs.

### Email opt-outs - Add or update emails
Adds or updates the email opt-out preferences for one or more email addresses.

### Email opt-outs - Delete email
Deletes a single email address from the opt-out list.

### Teamsite - Get all teamsites
Gets the full list of TeamSites for the tenant, regardless of user access.

### Teamsite - Get teamsite information
Gets information for the specified TeamSite.

### User - Get teamsites
Gets the list of TeamSites to which the current user has access.

### User - Get profiles
Gets the list of content profiles to which the current user has access.

### Create a webhook subscription
Creates a webhook subscription for the specified event type. Refer to the [Seismic Developer portal](https://developer.seismic.com/seismicsoftware/docs/webhooksoverview) for more information on webhooks.


## Getting Started
The Seismic Power Automate Configuration connector includes the following actions. Each action corresponds to an API endpoint. Refer to the article for each corresponding API endpoint for further information on the fields and properties associated with the action.

* [Content properties - Add a content property](https://developer.seismic.com/seismicsoftware/reference/seismiccontentpropertiesaddacontentproperty)
* [Content properties - Add values to a content property](https://developer.seismic.com/seismicsoftware/reference/seismiccontentpropertiesaddoneormorecontentpropertyvalues)
* [Content properties - Get all](https://developer.seismic.com/seismicsoftware/reference/seismiccontentpropertiesgetcontentproperties)
* [Content properties - Get values of a content property](https://developer.seismic.com/seismicsoftware/reference/seismiccontentpropertiesgetcontentpropertyvalues)
* [Email opt-outs - Add or update emails](https://developer.seismic.com/seismicsoftware/reference/seismicprivacymanagementaddorupdateoneormoreemailsintheoptoutlist)
* [Email opt-outs - Delete email](https://developer.seismic.com/seismicsoftware/reference/seismicprivacymanagementdeleteanemailfromtheoptoutlist)
* [Email opt-outs - Get list](https://developer.seismic.com/seismicsoftware/reference/seismicprivacymanagementgetemailoptoutlist)
* [Teamsite - Get all teamsites](https://developer.seismic.com/seismicsoftware/reference/seismicteamsitesgetalistofallteamsites)
* [Teamsite - Get teamsite information](https://developer.seismic.com/seismicsoftware/reference/seismicteamsitesgetinformationforaspecificteamsite)
* [User - Get profiles](https://developer.seismic.com/seismicsoftware/reference/seismicusersgetlistofmyassignedprofiles)
* [User - Get teamsites](https://developer.seismic.com/seismicsoftware/reference/seismicusersgetmyassignedteamsites)
* [Create a webhook subscription](https://developer.seismic.com/seismicsoftware/docs/webhooksoverview) (Webhook)

## Obtaining Credentials
You need a Seismic Premium user account for the tenant with Full Control (System Administrator) permissions. Contact your Seismic system administrator if you do not have sufficient permissions.

## Known Issues and Limitations
No issues and limitations are known at this time. All APIs operate in accordance with Seismic API policy, including Rate Limits. Please refer to the [Seismic Developer portal](https://developer.seismic.com/) for API specifications, restrictions, and standards.