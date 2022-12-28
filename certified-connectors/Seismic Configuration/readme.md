# Seismic Configuration
Seismic configuration connector provides set of actions required for adding or updating content properties, fetching teamsites and updating email opt-outs.

## Publisher: Seismic​

## Prerequisites
You must have user created in Seismic tenant.

## Supported Operations
### Get Content Property Values
Provides the list of content property values for a given content property. This will only return values for properties that have a defined list of values. Properties which have no defined domain of values and are filled in freeform by users are not provided here.

### Add Content Property Values
Add one or more content property values to an existing content property. Property values cannot be added to properties where an existing domain of values does not exist. Property values are case sensitive.#### Warning:Adding a domain of values to an existing property that does not have an domain of values can result in a loss of data. If this is desired please log into Seismic and create a domain of values to enable this functionality.

### Get Content Properties
Provides the list of content properties defined within the system.

### Add Content Property
Add a content property and associate it to an existing teamsite. Provide a set of property values in a single step using the `domainOfValues` object.

### Get Gdpr Emails
Get a list of registered opt-outs.

### Add or update Gdpr Emails
This action allows one or more email addresses to be added or updated in the tracking opt-out list. If the email address isn't already in the list, it will be added otherwise it will be updated.

### Delete Gdpr Email
Deletes a single email address from the opt-out list

### Get Teamsites
Provides the full list of teamsites that are available in the current tenant.

### Get Teamsite Details
Get the details of a given teamsite.

### Get User Teamsites
Get the current user's assigned teamsites.

### Get User Profiles
Get the list of content profiles that the current user has access to.

## Getting Started
For more information follow https://developer.seismic.com/seismicsoftware/reference/seismiccontentpropertiesgetcontentpropertyvalues

## Obtaining Credentials
You must have user created in Seismic tenant. If your user is not present, please ask tenant admin to create user for you.

## Known Issues and Limitations
No issues and limitations are known at this time.

