
# Exasol - The analytics database connector
Exasol is an analytics-focused parallelized relational database management system (RDBMS).
Our superfast data analytics platform gives you the power, flexibility and scalability to meet the demands of your long-term data strategy.  
Connect to an Exasol database to create, read, update and delete data.

## Publisher: Exasol AG
## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan
* An Exasol database
* Exasol REST API set up, internet facing

## Supported Operations
The connector supports the following actions:

### `GetTables` 
Returns a list of the tables visible to the database user.

### `GetRows`
Returns the rows from a table (based on filter conditions).

### `InsertRow`
Insert a row into a table.

### `UpdateRows`
Update row(s) within a table.

### `DeleteRows`
Delete row(s) within a table.

### `ExecuteQuery`
Run a custom query to fetch data, for more advanced scenarios.

### `ExecuteNonQuery`
Run a custom command, for more advanced scenarios.
## Obtaining Credentials
Currently the connector uses an API Key as the default authentication method.   
The API key(s) can be set in the Exasol REST API which you're also required to set up to use this connector.
## Getting Started
You should first set up the Exasol REST API (see: https://github.com/exasol/exasol-rest-api).

The connector itself takes 2 parameters when creating a new connection:

`Host`: Where your REST API is hosted (this can be an IP address or DNS name).

`API Key`: This is one of the authentication keys you've configured to gain access to the REST API.


Note: You can still edit these 2 values afterwards.

Please see the user guide for more detailed instructions and a full tutorial [here](https://github.com/exasol/power-apps-connector/blob/main/doc/user_guide/user_guide.md).

## Known Issues and Limitations
Using a on-premises data gateway is currently not supported.

## Deployment Instructions
Please follow [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.

## Further support and documentation


Questions and issues can be filed [here](https://github.com/exasol/power-apps-connector/issues).