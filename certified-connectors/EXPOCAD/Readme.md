
## EXPOCAD Connector
EXPOCAD is a Professional Event Floor Plan Management Software Specifically Created for Show Organizers and Operations Teams to Manage Exhibitors and Exposition Space. Multi-Event Management - Any Venue - Any Size - Indoors or Out

## Prerequisites
You will need the following to proceed:
* Your API Key to authenticate your connection (API Key)
* Your organization account name for use with EXPOCAD (ClientName)
* Your event database name for use with EXPOCAD (databaseName)


## Supported Actions
The connector supports the following operations:
* `Add a child exhibitor to the specified booth` : When adding a child exhibitor, the child exhibitor must already exist.
* `Add a new booth class` : Adds a new booth class to the supplied event.
* `Add a new exhibitor` : Adds a new exhibitor to the supplied event.
* `Add a new rate plan` : Adds a new rate plan to the supplied event.
* `Apply a single booth class to the specified booth` : Applys a single booth class to the specified booth in the supplied event.
* `Change the number of the specified booth` : Changes the number of the specified booth in the supplied event
* `Combine the specified booths into a new booth` : Combines the specified booths into a new booth in the supplied event.
* `Convert Held booth to Rented` : Rents the specified held booth to the held exhibitor in the supplied event. This operation will fail if the booth is currently held to a temporary exhibitor.
* `Convert Rented booth to Hold` : Converts the specified rented booth to a booth on hold to the same exhibitor in the supplied event.
* `Delete a booth class` : Deletes a booth class in the supplied event.
* `Delete an exhibitor` : Deletes an exhibitor in the supplied event.
* `Delete the specified booths` : Deletes the specified booths in the supplied event.
* `Get a collection of all pavilions` : Returns a collection of all pavilions in the supplied event.
* `Get a collection of all rate plans` : Returns a collection of all rate plans in the supplied event.
* `Get a collection of all showinshows` : Returns a collection of all showinshows in the supplied event.
* `Get a collection of transactions` : Parameters startDate and endDate allow you to filter by a date range. If you do decide to filter by a range, you must supply at least one valid date. Valid dates are in the format of: YYYY-MM-DD
* `Get a list of all assigned request items` : Returns a list of all assigned request items. If any parameter is omitted, that filter will not be applied
* `Get a list of payments in the Expocad Event` : Returns list of payments in the Expocad Event.For Non-Credit Card Payments, The Check Number is returned in the CCLast4 field
* `Get All Available Booths` : Returns a collection of all available booths in the supplied event.
* `Get All Booths` : Returns an array of all booths in the supplied event.
* `Get All Events` : Returns a collection of registered events.
* `Get All Exhibitors` : Returns a collection of all exhibitors in the supplied event.
* `Get All Rented Booths` : Returns a collection of all rented booths in the supplied event
* `Get an Exhibitor` : Returns the specified exhibitor.
* `Get Event Information` : Returns basic information about the supplied event.
* `Get Event Statistics` : Returns basic statistics describing the supplied event.
* `Get financial information for the specified booth` : Returns financial information for the specified booth.
* `Get Invoice Details`: Gets details of a single invoice. Either invoiceNo or exhibtitorId is required.
* `Get Specific Booth` : Returns the specified booth.
* `Get the currently set default rate plan` : Returns the currently set default rate plan from the supplied event.
* `Get the master list of payment types` : Returns the master list of payment types.
* `Holds the specified booth` : EXPOCAD allows for a booth to be placed on hold to either an existing Exhibitor (by Id) or to a temporary Exhibitor (by Name). Choose one parameter or the other - 'ExhibitorId' or 'ExhibitorName'. If both paramerters are supplied, the hold will default to the Exhibitor ID.
* `Override the display on drawing property for the specified booth` : Overrides the display on drawing property for the specified booth.
* `Remove a child exhibitor from the specified booth` : Removes a child exhibitor from the specified booth in the supplied event.
* `Remove a single booth class from the specified booth` : Removes a single booth class from the specified booth in the supplied event.
* `Rent a Booth` : Rents the specified booth in the supplied event.
* `Reset the display on drawing override for the specified booth` : Resets the display on drawing override for the specified booth.
* `Return a collection of all booth classes` : Returns a collection of all booth classes in the supplied event.
* `Return the specified booth class` : Returns the specified booth class.
* `Returns the master list of requestable items` : Will return items that match either the glCode or the transactionCode
* `Return all invoices in supplied event`: Returns all invoices in the supplied event.
* `Set the default rate plan` : Sets the default rate plan for the supplied event.
* `Uncombine the specified booth` : Uncombines the specified booth in the supplied event.
* `Undelete the specified booths` : Undeletes the specified booths in the supplied event.
* `Unholds the specified booth` : Unholds the specified booth in the supplied event.
* `Unrent a Booth` : Unrents the specified booth in the supplied event.
* `Update an exhibitor` : Updates an exhibitor in the supplied event.
* `Update the specified booth class` : Updates the specified booth class.

## Supported Triggers
The connector does not have any supported triggers.