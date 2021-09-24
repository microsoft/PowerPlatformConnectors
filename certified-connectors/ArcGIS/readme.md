ArcGIS for Power Automate allows you to enhance your automated workflows with location intelligence from your ArcGIS organization. 

## Prerequisites

- An ArcGIS account is required. For more information, see [Create account](https://doc.arcgis.com/en/arcgis-online/get-started/create-account.htm) .
- An ArcGIS API key is not required but can be used in place of an ArcGIS account.

## Get started

ArcGIS for Power Automate connects your automated workflows to resources from your ArcGIS organization. Access geoenrichment, geocoding, and routing services to add demographics and location information to your data. You can also use ArcGIS for Power Automate to create feature layers and fetch data from existing feature layers in your ArcGIS organization.

## Considerations

Connecting to your ArcGIS account will consume credits with the actions listed above. ArcGIS connections using OAuth2 have a timeout of 14 days and must be reconnected manually. 

## Throttling limits
Name |	Calls	| Renewal Period
--- | --- | --- 
Geocode addresses	| 150	| Per request
Get data from Feature Layer	| 1000 rows	| Per request
Geometry lookups |	1 geometry | Per request
