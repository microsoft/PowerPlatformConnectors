# Title : World Academia
The primary purpose of this connector is to provide easy and efficient access to university-related data for educational institutions, researchers, students, and anyone interested in information about universities worldwide. 

## Publisher: Kelcho Tech​

## Prerequisites
You will need the following to use the World Academia connector:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* The tool is free to use no authentication or licenses is required.​

## Supported Operations
The connector will enable users to retrieve data such as lists of universities, university details, continents, countries, and more
### /api/v1/continentsdata
Get all continents and their metadata

### /api/v1/continents
Get a list continents

## /api/v1/continent/{continentName}
Get a list countries in a continent.​

## /api/v1/sch
Get all universities and their metadata

## /api/v1/sch/count
Get the number of  universities available

## /api/v1/sch/countries
Get all universities and their metadata

## /api/v1/sch/countriescodes
Get a list of countriesCodes with universities

## /api/v1/sch/country/{countryName}
Get all universities in a country via country name

## /api/v1/sch/countrycode/{countryCode}
Get all universities in a country via country code

## /api/v1/schlist
Get a list of all universities

## /api/v1/sch/university/{universityName}
Get all university details via university name

## Obtaining Credentials
The is no authentication needed to access this connector​

## Getting Started
Optional. How to get started with your connector.

## Known Issues and Limitations
The connector may not cover all universities in the world yet but we are working day and night to achieve this goal.​

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Flow and PowerApps

