
## AIHY My Hospitals Connector
The My Hospitals API currently allows you to search for specific data across the MyHospitals national hospital reporting platform. This data provides information about your local hospital, hospitals within your region, state or territory and nationally.

To avoid the need for programming, the API also returns some data in machine readable CSV format and/or XLSX format.

You can see what information is available from the API, and how you can filter and search for specific information, by exploring the [interactive home page](https://myhospitalsapi.aihw.gov.au/).

## Prerequisites
You will need the following to proceed
 A Microsoft Power Apps or Power Automate plan with custom connector feature
 The Power platform CLI tools

## Publisher: Paul Culmsee

## Building the connector 
The API is an open read-only API and requires no authentication. This will be reviewed regularly and, depending on usage, an API key system may be implemented.

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```

## Supported Operations
The connector supports the following operations
 `Get Flat Formatted Data` Gets formatted data in a flattened structure with each item corresponding to a single data point. Data is restricted to within a single measure category and can be optionally filtered by measure, reporting unit code, reporting unit type and reporting dates. Matching within each filter is by logical disjunction (OR) and matching across filters is by logical conjunction (AND). NOTE: There is currently a pagination restriction on this query that restricts requests to a maximum of 1000 results. Use skip and top to control this pagination. Sorting is not currently available
 `Get Flat Data` Gets data in a flattened structure with each item corresponding to a single data point. Data is restricted to within a single measure category and can be optionally filtered by measure, reporting unit type and reporting dates. Matching within each filter is by logical disjunction (OR) and matching across filters is by logical conjunction (AND). NOTE: There is currently a pagination restriction on this query that restricts requests to a maximum of 1000 results. Use skip and top to control this pagination. Sorting is not currently available
 `Get Caveats` Gets a list of caveats
 `Get Single Caveat` Gets a caveat matching the supplied caveat code
 `Get DataSets` Gets a list of datasets, optionally filtered by a list of measure and reported measure codes. Matching within each filter is by logical disjunction (OR) and matching across filters is by logical conjunction (AND)
 `Get Single Dataset` Gets a single dataset matching the supplied dataset id.
 `Get Dataset Items` Gets a list of data items for a dataset,, optionally filtered, using logical disjunction (OR), by a list of reporting unit codes
 `Get Measure Categories` Gets a list of measure categories
 `Get Single Measure Category` Gets a single measure category matching the supplied measure category code
 `Get Measures for a Measure Category` Gets a list of measures for the specified measure category code
 `Get Reported Measure Categories` Gets list of all reported measure categories available
 `Get Reported Measure Categories for Measure Category Code` Gets a list reported measure categories matching the supplied reported measure category codes. NOTE: Reported measure codes are not constrained to be unique in the system
 `Get Reported Measures for Reported Measure Category` Gets a list of all reported measures for a specified reported measure category code
 `Get Reported Measures` Gets list of reported measures, optionally filtered by a list of measure and reported measure category codes. Matching within each filter is by logical disjunction (OR) and matching across filters is by logical conjunction (AND)
 `Get Single Reported Measure` Gets a single reported measure
 `Get Data Items for Reported Measure` Gets a list of all data items for the specified reported measure
 `Get Reporting Units` Gets list of all reporting units, optionally filtered, using logical disjunction (OR), by a list of reporting unit type codes
 `Get Single Reporting Unit` Gets a single reporting unit matching the supplied reporting unit code
 `Get Data Items for Reporting Unit` Gets a list of all data items for the specified reporting unit
 `Get Measures with Data Available for Reporting Unit` Get list of all measures that have data available for the specified reporting unit
 `Get Brick Codes for Reporting Unit` Gets list of all brick codes available for the specified reporting unit
 `Get Reporting Unit Types` Gets a list of reporting unit types
 `Get Single Reporting Unit Type` Gets a single reporting unit type matching the supplied reporting unit type code
 `Get Dictionary of Reporting Unit Codes and Available Bricks` Gets a dictionary of reporting units codes and their available bricks, for the specified reporting unit type
 `Get Measures` Gets list of all measures, optionally filtered, using logical disjunction (OR), by a list of measure category codes
 `Get Reporting Units Available for Measure` Gets a list of all reporting units that have data available for the specified measure
 `Get Data Items for Measure` Gets a list of all data items for the specified measure
 `Get Measure Downloads Available` Gets dictionary, indexed by the measure download code, of all available measure download codes and descriptions that can be used in the measure download API call
 `Get Measure Download` Gets a measure download, in XLSX format, for the supplied measure download code
 `Get Measure Across Reporting Unit Download` Gets a measure across reporting unit download, in XLSX format, for the supplied measure download code
 `Get Reporting Unit Datasheet Codes Available` Gets dictionary, indexed by the datasheet code, of all available reporting unit datasheet codes and descriptions that can be used in the reporting unit download API call
 `Get Reporting Unit Mapping Download` Gets the reporting unit mapping download, in XLSX format
 `Get Reporting Unit Data Download` Gets a reporting unit data download, in XLSX format, for the supplied reporting unit code and datasheet code
 `Get Simple Downloads Available` Gets dictionary, indexed by the simple download code, of all available simple download codes and descriptions that can be used in the simple download API call
 `Get Simple Download` Gets a simple download, in XLSX format, for the supplied download code
 
## Known Issues and Limitations
* Currently there is no imposed rate limiting, however the API usage will be monitored and limiting may be imposed if there are access issues. 

* Material that can be copied or downloaded from this website has been released under a Creative Commons BY 3.9 (CC-BY 3.0) license. Further information can be found at https://www.aihw.gov.au/copyright.

* Currently there is no pagination built into the API.

* The API requests are heavily cached, and the cache is updated daily and/or whenever new data is published.

* The API is available for use at any time. However, the API may be disabled at any time by the AIHW for maintenance or other reasons.





