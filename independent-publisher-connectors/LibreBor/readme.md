
## Libre Bor Connector
LibreBor gives you access to more than 5 million company and individual records from the Spanish Register. Using this connector, you can retrieve companies information using their names or NIF (Tax Identification Number), you can validate individual's relations to a company and also validate wether a company is still active or not. The connector also has utilities such as a NIF validator.

## Publisher : Mario Trueba Cantero & Marco Amoedo

## Prerequisites
You will need the following to proceed:
* An account with LibreBor (https://librebor.me/) and an active subscription

## Getting your credentials
Once you have an active account with LibreBor you will be given a username and password to access their services.


## Supported Operations
The connector supports the following operations:
* `Search Company`: Company results including name, slug and province. Use the slug to retrieve all the information for a company.
* `Get company information by slug`: Retrieve company information for specified Slug.
* `Get company information by NIF`: Retrieve company information for specified NIF.
* `Search for a person`: Retrieve list of people that match the keywords in their name. The number of results is limited to 200. Use the 'page' parameter to retrieve more results.
* `Get person information by slug`: Retrieve person information for specified Slug.
* `Validate NIF`: Check whether a NIF is valid or not and return information that can be infered from a NIF number, such as the legal form, company type and register province.
* `Get company incorporation information by Slug`: Retrieve company incorporation information for specified Slug.


## Known Issues and Limitations
None at this time.