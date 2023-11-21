# NHTSA vPIC
The NHTSA Product Information Catalog Vehicle Listing (vPIC) Application Programming Interface (API) provides different ways to gather information on Vehicles and their specifications. The vPIC Dataset is populated using the information submitted by the Motor Vehicle manufacturers through the 565 submittals. All the information on how a VIN is assigned by the manufacturer is captured in this catalog and used to decode a VIN and extract vehicle information.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
There are no prerequisites to using this connector.

## Obtaining Credentials
There are no credentials needed for this connector.

## Supported Operations
### Decode VIN
This action returns the decoded VIN.
### Decode WMI
This action returns the decoded World Manufacturer Identifier (WMI). WMIs may be put in as either 3 characters representing VIN position 1-3 or 6 characters representing VIN positions 1-3 & 12-14. Example "JTD", "1T9131".
### Get WMIs for Manufacturer
Provides information on the all World Manufacturer Identifier (WMI) for a specified Manufacturer. Only WMI registered in vPICList are displayed. If Query is a number - method will do exact match on Manufacturer's Id. If Query is a string - it will look for manufacturers whose name is LIKE the provided name (it accepts a partial manufacturer name as an input). Multiple results are returned in case of multiple matches. If Vehicle Type is a number - method will do exact match on Vehicle Type's Id. If Vehicle Type is a string - it will look for Vehicle Type whose name is LIKE the provided name (it accepts a partial Vehicle Type name as an input). Multiple results are returned in case of multiple matches. Both parameters are optional but at least one must be provided.
### Get all makes
Retrieves a list of all the makes available in vPIC Dataset.
### Get parts
Get a list of parts with letter date. Up to 1000 results will be returned at a time.
### Get all manufacturers
Retrieves a list of all the Manufacturers available in vPIC Dataset. Parameter "Manufacturer Type" allows to filter the list based on manufacturer type (Incomplete Vehicles, Completed Vehicle Manufacturer, Incomplete Vehicle Manufacturer, Intermediate Manufacturer, Final-Stage Manufacturer, Alterer, Replica Vehicle Manufacturer, or any part of it). Results are provided in pages of 100 items.
### Get manufacturer details
Retrieves the details for a specific manufacturer that is requested. If query is a number - method will do exact match on Manufacturer's Id. If query is a string - it will look for manufacturers whose name is LIKE the provided name (it accepts a partial manufacturer name as an input). Multiple results are returned in case of multiple matches.
### Get makes for manufacturer by manufacturer name
Retrieves all the Makes in the vPIC dataset for a specified manufacturer that is requested. If query is a number - method will do exact match on Manufacturer's Id. If query is a string - it will look for manufacturers whose name is LIKE the provided name (it accepts a partial manufacturer name as an input). Multiple results are returned in case of multiple matches. Manufacturer name can be a partial name, or a full name for more specificity (e.g., "988", "HONDA", "HONDA OF CANADA MFG., INC.", etc.).
### Get makes for manufacturer by manufacturer name and year
Retrieves all the Makes in the vPIC dataset for a specified manufacturer and whose Year From and Year To range cover the specified year If supplied parameter is a number - method will do exact match on Manufacturer's Id. If supplied parameter is a string - it will look for manufacturers whose name is LIKE the provided name (it accepts a partial manufacturer name as an input). Multiple results are returned in case of multiple matches. Manufacturer can be idenfitied by Id, a partial name, or a full name (e.g., "988", "HONDA", "HONDA OF CANADA MFG., INC.", etc.).
### Get makes by vehicle type name
Retrieves all the Makes in the vPIC dataset for a specified vehicle type whose name is LIKE the vehicle type name in vPIC Dataset. Vehicle Type name can be a partial name, or a full name for more specificity (e.g., "Vehicle", "Moto", "Low Speed Vehicle", etc.).
### Get vehicle types for make
Retrieves all the Vehicle Types in the vPIC dataset for a specified Make whose name is LIKE the make name in vPIC Dataset. Make can be a ID, partial name, or a full name for more specificity (e.g., "Merc", "Mercedes Benz", etc.).
### Get equipment plant codes
Retrieves assigned equipment plant codes.
### Get models for make
Retrieves the models in the vPIC dataset for a specified make whose name is LIKE the make in vPIC Dataset. Make can be a partial, or a full for more specificity (e.g., "Harley", "Harley Davidson", etc.).
### Get models for make a combination of year and vehicle type
Retrieves the models in the vPIC dataset for a specified year and make whose name is LIKE the make in vPIC Dataset. Make can be a partial, or a full for more specificity (e.g., "Harley", "Harley Davidson", etc.) Model year is integer (greater than 1995). Vehicle Ttpe name can be a partial name, or a full name for more specificity (e.g., "Vehicle", "Moto", "Low Speed Vehicle", etc.).
### Get vehicle variables
Retrieves a list of all the vehicle related variables that are in vPIC dataset. Information on the name, description and the type of the variable is provided.
### Get vehicle variable values
Retrieves a list of all the accepted values for a given variable that are stored in vPIC dataset. Search parameter can either be a Variable Name ("battery type" in first example, please use full name, not just part of it), or Variable ID ("2" in second example). This applies to only "Look up" type of variables.


## Known Issues and Limitations
There are no known issues at this time.
