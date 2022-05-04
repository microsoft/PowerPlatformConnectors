## Ordnance Survey Places Connector
Find UK addresses instantly online and be confident you're getting the most up-to-date information with Ordnance Survey's address API.

Search by postcode, UPRN, full or partial address and ensure you get the right address first time. 

OS Places API is the UK’s most trusted, most comprehensive online address database. Users can now use Power Platform’s tools to further benefit from Ordnance Survey’s secure, scalable and resilient address look-up API. 

## Prerequisites
OS Places API requires an active license with Ordnance Survey. Customers must either sign up for a free trial or premium  subscriptions to generate access key required login to the API Connector. 
* Subscription plan options can be found here: https://osdatahub.os.uk/plans

## API Documentation
The API documentation can be found here https://osdatahub.os.uk/docs/places/overview

## Data specification
The data behind the API is Ordnance Survey's AddressBase Premium. The documentation about this addressing data can be found here: 
https://www.ordnancesurvey.co.uk/business-government/tools-support/addressbase-premium-support

## Supported Operations
The connector supports the following operations:
* `Find`: A free text search designed for quick address queries
* `Postcode`: A search based on a property's postcode.
* `UPRN`: A search that takes a UPRN as the search parameter
* `Nearest`: Takes a pair of coordinates (X and Y) as an input to determine the closest address
* `Bounding Box`: Takes two points and creates a bounding box. All addresses within this bounding box are then returned.
* `Radius`: Takes a pair of coordinates as the centre for a circle and returns all addresses that are intersected by it
* `Polygon`: A POST Request that takes a geoJSON polygon or multi-polygon object and returns all addresses that are in the object

## Further Support
For further support, you can contact us at technicalservices@os.uk, and we'll be happy to help.
