## GAC Connect2All
GAC Connect2All integrates the Microsoft Power Platform to Business Central and makes adaptations and the extensibility to connect virtual any Datasource to your business processes in Business Central possible. You do not need to be a Professional developer to seamlessly extend Business Central to interact to other business application and Datasources. GAC Connect2All delivers your business the scalability and flexibility to create smart integration solutions you need today and tomorrow.

## Pre-requisites
The user, who wants to use to the Connect2All connector in Power Automate,  must meet the requirements below:

 - The user must have a valid BC 365 license.
 - The user must have a paid subscription to at least one (or more) of the following GAC products:
   - Trade365 Starter
   - Trade365 Plus
   - Trade365 Enterprise
   - Production 365 Starter
   - Production 365 Plus
   - Production 365 Enterprise
   - PSA 365 Starter
   - PSA 365 Plus
   - PSA 365 Enterprise
   - GAC Integration platform

## Supported Operations
The connector currently supports the following operations:  

**Data Operations**
- **Get data:** This operation is used to retrieve data from Business Central for specific table(s).
- **Get data by id:** This operation is used to retrieve a specific record from Business Central.
- **Get data (complex):** This operation is used to retrieve complex objects from Business Central.
- **Insert data:** This operation is used to insert data into one ore more tables in Business Central.
- **Update data:** This operation is used to update data in a Business Central table.
- **Delete data:** This operation is used to delete data from a Business Central table.

**Media Operations**
 - **Get media:** This operation is used to retrieve a media value from Business Central.
 - **Update media:** This operation is used to update a media value from Business Central.
 - **Delete media:** This operation is used to delete a media value from Business Central.

**Report Operations**
- **Get Reports:** This operation is used to retrieve a report in various formats from Business Central.

**Action Operations**
- **Perform an action:** This operation is used to schedule an action in the task scheduler in Business Central.

**Xml Operations**
- **Get xml:** This operation is used to retrieve and transform data to XML from Business Central.
- **Get xml by id:** This operation is used to retrieve a specific record and transform it to XML from Business Central.
- **Get xml with metadata:** This operation is used to 'get xml' but wraps it in metadata as result.
- **Get xml by id with metadata:** This operation is used to 'get xml by id' but wraps it in metadata as result.
- **Process xml:** This operation is used to process a xml and write it's data to various mapped tables in Business Central.

**Csv Operations**
- **Get csv:** This operation is used to retrieve and transform data to csv from Business Central.
- **Get csv by id:** This operation is used to retrieve a specific record and transform it to csv from Business Central.
- **Get csv with metadata:** This operation is used to 'get csv' but wraps it in metadata as result.
- **Get csv by id with metadata:** This operation is used to 'get csv by id' but wraps it in metadata as result.
- **Process csv:** This operation is used to process a csv and write it's data to various mapped tables in Business Central.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps
