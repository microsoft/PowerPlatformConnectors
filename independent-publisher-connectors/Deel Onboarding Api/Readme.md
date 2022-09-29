# Deel Onboarding Api Connector
Deel is a platform that streamlines the process on onboarding Employees/Contractors from other countries without breaking local law of those countries.  
The platform handles HR processes from Onboarding, Timesheets, Payroll Processing, Tasks, Invoice Adjustment, Amend Contract and Milestones.
## Deel Open API
This platform provides an open API Specification for intergration for their customers.  
The documentation for the api is provided from [This Path](https://deel.com/docs)  
 ![deel developer image](https://files.readme.io/f74f02c-developers.png)

## Implementation
In this hackathon I will implement atleast three of the various API Endpoints Provided from this platform.

## List of Api
### Authentication
#### Api Tokens
<ol>
<li>Navigate to Apps & Integrations > Developer Center.</li>
<li>Click on the “Generate New Token” button.</li>
<li>In the popup, click the “Generate Token” button to generate a new token. Your newly generated token is visible on the screen.</li>
<li>Make sure to copy and save your token once is generated. You won't be able to see it again!</li>
</ol>

```
curl -X GET 'api.letsdeel.com/rest/v1/contracts' \
-H 'Authorization: Bearer YOUR-TOKEN-HERE'
```

There is support for OATH2 but that would be implemented later

### Timesheet
You can create, update, review and delete timesheets which are used to track the time a particular employee has worked during a certain period.
#### Retrieve contractIdTimeBased
```
curl --location -g --request GET '{{host}}/rest/v1/contracts?limit=2&sort_by=worker_name&order_direction=desc' \
--header 'Authorization: Bearer {{token}}
```
#### Create Timesheet
```
curl --location -g --request POST '{{host}}/rest/v1/timesheets' \
--header 'Authorization: Bearer {{token}}' \
--header 'Content-Type: application/json' \
--data-raw '{
  "data": {
    "contract_id": "{{contractIdTimeBased}}",
    "quantity": 1,
    "description": "worked",
    "date_submitted": "2022-05-16"
  }
}'
```
#### Update a timesheet Entry
##### <strong>Step 1:</strong> Retrieve the `timesheet id`
```
curl --location -g --request GET '{{host}}/rest/v1/contracts/{{contractId}}/timesheets?statuses[]=pending' \
--header 'Authorization: Bearer {{token}}'
```
##### <strong>Step 2:</strong> Update the timesheet entry
```
curl --location -g --request PATCH '{{host}}/rest/v1/timesheets/{{timesheetId}}' \
--header 'Authorization: Bearer {{token}}' \
--header 'Content-Type: application/json' \
--data-raw '{
  "data": {
    "quantity": 1,
    "description": "worked - updated"
  }
}'
```

#### Delete a timesheet Entry
```
curl --location -g --request POST '{{host}}/rest/v1/timesheets/{{timesheetId}}/reviews' \
--header 'Authorization: Bearer {{token}}' \
--header 'Content-Type: application/json' \
--data-raw '{
  "data": {
    "status": "approved",
    "reason": "my reason"
  }
}'
```

#### Review timesheet Entries
```
curl --location -g --request POST '{{host}}/rest/v1/timesheets/{{timesheetId}}/reviews' \
--header 'Authorization: Bearer {{token}}' \
--header 'Content-Type: application/json' \
--data-raw '{
  "data": {
    "status": "approved",
    "reason": "my reason"
  }
}'
```

### Tasks
The Pay-As-You-Go (PAYG) contract is an ongoing contract used to pay contractors based on the working days/hours or tasks they have submitted. The client signs the contract and pays the invoice at the end of a week or a month. The amount invoiced is generated based on the total time a contractor or client submits. This type of contract can be used for work of any scope.

#### Create a new task for your PAYG contractor.
```
curl --location -g --request POST '{{host}}/rest/v1/contracts/{{contractId}}/tasks' \
--header 'Authorization: Bearer {{token}}' \
--header 'Content-Type: application/json' \
--data-raw '{
  "data": {
    "type": "payg_tasks",
    "title": "PAYG Contract",
    "country_code": "US",
    "state_code": "CO",
    "notice_period": 5,
    "job_title": {
      "name": "Software Engineer"
    },
    "seniority": {
      "id": 1
    },
    "client": {
      "legal_entity": {
        "id": 12345
      },
      "team": {
        "id": 45678
      }
    },
    "start_date": "2022-10-10",
    "termination_date": "2023-11-11",
    "scope_of_work": "Create software applications.",
    "compensation_details": {
      "currency_code": "USD",
      "scale": "weekly",
      "frequency": "monthly",
      "cycle_end": 25,
      "cycle_end_type": "DAY_OF_MONTH",
      "payment_due_type": "REGULAR",
      "payment_due_days": 5
    },
    "meta": {
      "documents_required": true
    }
  }
}'
```

#### Review Task
#####  <strong>Step 1:</strong> Retrieve the `taskId`
```
curl --location -g --request GET '{{host}}/rest/v1/contracts/{{contractIdTaskBased}}/tasks' \
--header 'Authorization: Bearer {{token}}' \
--data-raw ''
```
##### <strong>Step 2:</strong> Approve the Task
```
curl --location -g --request POST '{{host}}/rest/v1/contracts/{{contractIdTaskBased}}/tasks/{{taskId}}/reviews' \
--header 'Authorization: Bearer {{token}}' \
--header 'Content-Type: application/json' \
--data-raw '{
  "data": {
    "status": "approved"
  }
}'
```
###### You are also able to review and then approve or decline multiple tasks by specifying the taskId
```
curl --location -g --request POST '{{host}}/rest/v1/contracts/{{contractIdTaskBased}}/tasks/many/reviews' \
--header 'Authorization: Bearer {{token}}' \
--header 'Content-Type: application/json' \
--data-raw '{
  "data": {
    "status": "approved",
    "ids": [
      {{taskId}},
      {{taskId2}}
    ]
  }
}'
```

##### Delete a task
```
curl --location -g --request DELETE '{{host}}/rest/v1/contracts/{{contractIdTaskBased}}/tasks/{{taskId}}' \
--header 'Authorization: Bearer {{token}}' \
--data-raw ''
```
### Invoice Adjustments
#### Create An Invoice adjustment
```
curl --location --request POST 'https://api.letsdeel.com/rest/v1/invoice-adjustments' \
--header 'Authorization: Bearer {token}' \
--header 'Content-Type: application/json' \
--data-raw '{
  "data": {
    "contract_id": "a1b2b3",
    "amount": 1,
    "description": "my bonus",
    "date_submitted": "2022-06-31",
    "type": "bonus"
  }
}'
```

#### Update An Existing Invoice adjustment
#### <strong>Step 1:</strong> Retrieve the `timesheetId`
```
curl --location -g --request GET '{{host}}/rest/v1/contracts/{{contractId}}/timesheets?statuses[]=pending' \
--header 'Authorization: Bearer {token}'
```

#### <strong>Step 2:</strong> Update an existing adjustment
```
curl --location -g --request PATCH '{{host}}/rest/v1/invoice-adjustments/{{timesheetId}}' \
--header 'Authorization: Bearer {token}' \
--header 'Content-Type: application/json' \
--data-raw '{
  "data": {
    "amount": 222,
    "description": "bonus - updated"
  }
}'
```

### Delete an Invoice adjustment
```
curl --location -g --request DELETE '{{host}}/rest/v1/invoice-adjustments/{{timesheetIdVat}}?reason=my reason to delete' \
--header 'Authorization: Bearer {token}'
```

### Approve an invoice adjustment
The invoice adjustment can also be submitted by the worker and you are able to review that invoice adjustment and then either approve it...
```
curl --location -g --request POST '{{host}}/rest/v1/invoice-adjustments/{{timesheetId}}/reviews' \
--header 'Authorization: Bearer {token} ' \
--header 'Content-Type: application/json' \
--data-raw '{
  "data": {
    "status": "approved",
    "reason": "my reason"
  }
}'
```
### Decline an invoice adjustment
... or decline it.
```
curl --location -g --request POST '{{host}}/rest/v1/invoice-adjustments/{{timesheetId}}/reviews' \
--header 'Authorization: Bearer {token}' \
--header 'Content-Type: application/json' \
--data-raw '{
  "data": {
    "status": "declined",
    "reason": "my reason"
  }
}'
```

### Search for an invoice line item
```
curl --location -g --request GET '{{host}}/rest/v1//invoice-adjustments?types[]=bonus' \
--header 'Authorization: Bearer {token}'
```

<em>More Api Specifications will be updated soon...</em>