# Business Solver
Business Solver is a benefits administration and engagement platform that deliver an industry-leading benefits experience for an organization and its employees.The Business Solver connector enables fetching of the employee's benifits.

## Publisher: HR Copilot

## Prerequisites
> To access the Business Solver connector you need to generate access token and authenticate with Authorization header, x-Amz-Date, X-Amz-Singnature while establishing the connection with the connector.

## Supported Operations

- **`EmployeeId`**
  - *Inputs:* `coNum` `companyEmployeeId`
  - *HTTP Verb:* `GET`
  - *Description:* Retrieves the Id and Company Id of the Employee based on the input parameters.

- **`Employees`**
  - *HTTP Verb:* `GET`
  - *Description:* Retrieves the details of the Employee.

- **`EmployeesCoverage`**
  - *HTTP Verb:* `GET`
  - *Description:* Retrieves the coverage details of the Employee.

- **`EmployeesDependents`**
  - *HTTP Verb:* `GET`
  - *Description:* Retrieves the Dependents details of the Employee.


## Obtaining Credentials
Make an Api call to the endpoint "https://qa.benefitsolver.com/benefits/rest/token" with an API testing Tool (ex: Postman, Insomnia) with the Authorization type as AWS V4 by entering the "Access Key ID" and "Secret Access Key".

## Known Issues and Limitations
"Authorization header", "x-Amz-Date", "X-Amz-Singnature" fields are required to establish a connection with the connector.


