# Leave Dates

Leave Dates simplifies leave management by providing an API for scheduling, tracking, and reporting employee time off. This API enables seamless integration with HR systems, making leave tracking more efficient for teams and organizations.

## Publisher: Tiago Ramos (novalogica)

## Prerequisites
To use this API, you need the following:

- A Leave Dates account. Sign up here: [Leave Dates](https://www.leavedates.com/)
- An Bearer Token for authentication

## Obtaining Credentials
- Configure your Bearer Token in your account settings at [Leave Dates API Key](https://www.leavedates.com/)

## Authentication
All requests require an Bearer Token, passed in the `Authorization` header:
```http
Authorization: Bearer YOUR_BEARER_TOKEN
```

## Supported Actions
The Leave Dates API supports various actions for managing companies, employees, and leave requests.

### 1. Companies
- **List Companies**: Retrieves a list of all companies associated with the authenticated user.
- **List Departments**: Retrieves a list of departments for a given company.
- **List Leave Types**: Fetches all available leave types within a company.

### 2. Reports
- **Get Allowance Summary Report**: Get allowance summary report of the company by allowance type, calendar and allowance unit.

### 3. Leaves
- **Request Leave**: Creates a new leave request.
- **Get Leave Details**: Retrieves details of a specific leave request.
- **Approve Leave**: Approves a leave request.
- **Cancel Leave**: Cancels an existing leave request.
- **Update Leave**: Update an existing leave request.

### 4. Employees
- **List Employees**: Retrieves all employees in a company.
- **Add Employee**: Adds a new employee to a company.
- **Delete Employee**: Delete a employee.
- **Update Employee**: Update a employee.
- **Get Details Of An Employee**: All details for requested employee.

## Other Actions
- More endpoints will be available in upcoming updates. Stay tuned!

## Known Issues and Limitations
There are no known issues at the time of publishing.

## Error Codes
| Status Code | Meaning |
|------------|---------|
| 400 | Bad Request |
| 401 | Unauthorized |
| 404 | Not Found |
| 500 | Internal Server Error |