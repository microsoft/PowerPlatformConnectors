# Softools Connector

[Softools](https://www.softools.net) is a leading NoCode platform for building business process applications. 

# Pre-requisites
Users require an Enterprise License or have a Softools site to use our apis.

An API Key is required to call the API, this can be found under ```My Profile``` on your Softools Site.

An ```App Identifier``` is required from most API calls. 

# Supported API Calls
```Get a record```
Use to get an app from a Softools App. 

```Patch a record```
Use to update a record. 

```Post a record```
Use to create a new record.

```Archive a record```
Use to archive a record.

```Unarchive a record```
Use to unarchive a record.

```Delete a record```
You must call archive first.
Use to delete a record

```Get report records```
Returns all records and properties that are defined in a report.
Filter using odata ```[TextField] eq "test"``` or ```[NumberField] eq 1```
Order by ```[TextField] desc``` or ```[NumberField] asc```
Skip and Top should be used to limit the data returned.

```Get records```
This will return all properties on a record, we recommend using get report records over this.

```Get app fields```
Returns the apps defined in the app

# Support Site
Rate limits can be found [here](https://developer.softools.net/Rate-limits)

More support documentation can be found [here](https://support.softools.net)
