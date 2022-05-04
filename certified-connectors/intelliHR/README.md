## intelliHR Power Platform Connector
With this connector you can subscribe to events that occur within your intelliHR tenacy and automate workflows within the Microsoft Power Platform and its connectors using data from your tenacy.

## Prerequisites
You will need the following to proceed:
* An existing intelliHR tenant (including the tenant ID)
* An intelliHR API key [(how to generate an API key)](https://developers.intellihr.io/#auth)

## Setting up the connector 
There is two crucial steps to enabling the connector:

### 1. Adding the tenant header.
To identify the data associated with your requests, you must include the tenant ID on each request the connector makes.  During configuration, you will be prompted to add your Tenant name to the connector.  You can find your tenant name as part of your intelliHR domain address, eg https://[tenant-name].intellihr.net.

### 2. Configuring your API key
When you first use the connector, you will be prompted to supply an API key.  These can be produced inside your intelliHR account with appopriate levels of access.  It's important to be aware that during the API key input, you are expected to prepend 'Bearer ' (including the space) before your key.

```Bearer XYZ```

Omitting this will cause 'invalid API key' errors to return for your requests.

## Supported Operations

### Business Entities
Business Entities represent the legal entities which exist within the organisation. A Business Entity can be used as a filter for automation logic (eg Pulse configuration) and as a dimension in the analytics tools. Every Job in the system belongs to a Business Entity.
- List all Business Entities
- Find a Business Entity by ID

### Business Units
The administrative units that exist within the organisation. Used to render the org chart business unit view. For example, "Finance", "HR", or "Information Technology". Business Units can be configured as text strings with parent -> child relationships. Every Job in the system belongs to a Business Unit.
- List all Business Units
- Create a new Business Unit
- Find a Business Unit by ID
- Delete a Business Unit by ID
- Update a Business Unit by ID

### Custom Field Definitions
Custom Field Definitions can be used to attach extra data fields to particular models in the system such as Training.
- List all Custom Field Definitions
- Find a Custom Field Definition by ID

### Default Remuneration Components
Default Remuneration Components are the organization-wide remuneration components for both additions and breakdowns.
List all Default Remuneration Components
Find a Default Remuneration Component by ID

### Employment Conditions
Employment conditions are the conditions under which someone is paid for their remuneration schedule. Every Job in the system belongs to a Employment Condition or the No Employment Condition Employment Condition.
- List all employment conditions
- Find an Employment Condition by ID

### Jobs
The Job object is the core data object for past, current and future roles that a Person holds in the organisation. A Person can have more than one Job at a point in time to represent secondment / role sharing.
The data returned from this endpoint represents the data at this current time, which may change over time due to scheduled changes. This endpoint returns the Job's Remuneration Schedule.
- List all Jobs
- Create a new Job
- Find a Job by ID
- Patch an existing Job

### End Job
- Set and finalise the end date of a job. Finalising a job changes it to a past job on the end date.

### Locations
Locations can be configured as text strings with parent -> child relationships. For example, Australia -> Brisbane -> Spring Hill Head Office. These locations are used for filtering in analytics. Every Job in the system belongs to a Location or the No Location Location.
- List all locations
- Find a Location by ID

### Pay Grades
Pay Grades are optional attributes applied to a job to provide a salary range for analytical purposes. These are configurable for admins in the intelliHR application. Every Job in the system belongs to a Pay Grade or the No Pay Grade Pay Grade.
- List all pay grades
- Find a Pay Grade by ID

### People
People represent the base objects in the intelliHR data model. All past, present and future employees should be created as a Person and then have a Job created which references that Person object. Contact information for the employee (eg emergency contact, phone numbers, email addresses, etc) is referenced to the Person object. While Jobs can have scheduled updates which will take effect in the future, People are always static in the system and will not change unless updated.
- List all people
- Create a new person record
- Find a Person by ID
- Update a Person by ID

### Person Documents
Person Document is an object which represents the document stored in the "Documents" tab in Person Profile in intelliHR system. You can upload a document through this API by following the steps below.

1. Create a presigned upload URL which enables you to upload a document to S3 by using Create a presigned upload URL endpoint.
2. Upload a document to the S3 presigned URL with HTTP PUT method.
3. Mark the upload status successful with Update a Document endpoint.

- Create a presigned upload URL
- Update a document by id

### Recruitment Sources
Recruitment sources refer to the origin of this employee, such as a job board or a recruitment agency. These are configured as text strings in the intelliHR application and are used as filters for analytical purposes. Recruitment Sources can be configured as text strings with parent -> child relationships.
- List all recruitment sources
- Find a Recruitment Source by ID

### Training Providers
Training Providers are used in the system to document what/who provided the Training.
- List all Training Providers
- Find a Training Provider by ID

### Trainings
Trainings are used in the system to store details about individual training events.
- List all Trainings
- Create a Training record
- Find a Training record by ID
- Delete a Training record by ID
- Update a Training record by ID

### Training Types
Training Types are used in the system to document the category of a Training record.
- List all Training Types
- Find a Training Type by ID

### Work Classes
Work Classes are a sub-class of Work Types. For example, the Permanent Work Type has both Full-Time and Part-Time Work Classes. These are used for to configure automation and analytics filters. Every Job in the system belongs to a Work Class.
- List all work classes
- Find a Work Class by ID

### Work Rights
Work Rights are the organization-wide work rights.
- List all Work Rights
- Find a Work Right by ID

### Work Types
Work Types represent the primary work classification for a job. It is one of the types Permanent, Fixed Contract, Unpaid or Temporary/Casual. This attribute is always used with the Work Class attribute which is a sub class of Work Type. Every Job in the system belongs to a Work Type.
- List all Work Types
- Find a Work Type by ID

### Webhook Events
Webhook Events are hardcoded events which occur within the intelliHR platform which can be subscribed to using Webhooks.
As an example, person.created is a Webhook Event, which can trigger a Webhook when a Person is created within the platform.
- Find a Webhook Event by ID

### Webhooks
Webhooks are used in the system to allow developers and integration platforms to subscribe to a Webhook Event, such as a person.updated or person.created event.
When the Webhook Event is triggered, the system will send a payload to the endpoint specified when creating the Webhook.
- Create a webhook and subscribe to a type of event in your tenant
- Find a Webhook by ID
- Delete a Webhook by ID
- Update a Webhook by ID
