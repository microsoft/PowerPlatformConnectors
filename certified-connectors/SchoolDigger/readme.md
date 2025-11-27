## SchoolDigger
SchoolDigger gives you immediate access to over 120,000 U.S. K-12 Schools and 18,500 U.S. School Districts. Information includes school name and address, student demographics over time, student/teacher ratios, test scores at the school building and grade level, and much more! See our full documentation at [developer.schooldigger.com](https://developer.schooldigger.com/)


## Pre-requisites
You will need an account to proceed:
* Choose a plan at [developer.schooldigger.com](https://developer.schooldigger.com/) and sign up
* Once you are signed up, copy down your appID and appKey
also needed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* The Power platform CLI tools

## Supported Operations
The connector supports the following operations:

* ```Get a school list```: Look for schools by name, location (lat/long, city, zip, and more), district, etc.
* ```Get a district list```: Look for schools districts by name, location (lat/long, city, zip, and more), district, etc.
* ```Get school detail```: Returns a detailed record for one school
* ```Get school district detail```: Returns a detailed record for one district
* ```Get a SchoolDigger school ranking list```: Returns a SchoolDigger school ranking list for a particular state and level (elementary, middle, high)
* ```Get a SchoolDigger district ranking list```: Returns a SchoolDigger district ranking list for a particular state
* ```School list autocomplete```: Returns a simple and quick list of schools for use in a client-typed autocomplete.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps

## Further Support
For further support, checkout our [developer website](https://developer.schooldigger.com) or [contact us](mailto:api@schooldigger.com)