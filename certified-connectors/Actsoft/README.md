# Actsoft TeamWherx
TeamWherx allows you to create and capture digital form information, capture employee timekeeping information, and manage work orders with a dispatching system.

This connector uses the [TeamWherx APIs]( https://developer.wfmplatform.com/) to make it easy to interact with information captured using the application.  You can utilize various triggers or actions in the connector to easily extract data from TeamWherx and integrate with other applications.

# Publisher
Actsoft, Inc
# Prerequisites
In order to use this connector, you will need the following:

- An active TeamWherx Account
- TeamWherx API Key
- Microsoft Power Apps or Power Automate plan

# Supported Operations

## Triggers
- When a form record is created or updated
- When a timekeeping record is created or updated
- When an order is created or updated
- When an order status is updated

## Actions
### Forms
- Create a form record
- Get a single form record
- Get a list of form records

### Timekeeping
- Get a single timekeeping record
- Get a list of timekeeping records

### Custom Lists
- Create or update a custom list record
- Delete a custom list record
- Get a single custom list record
- Get a list of custom list records
- Get a list of custom list definitions
- Get a list of custom list fields

### Clients
- Create a client
- Delete a client
- Get a single client record
- Get a list of client records
- Update a client

### Orders
- Create an order
- Get a single order
- Get a list of orders
- Update an order
- Change an order status

### Geofences
- Create a geofence
- Delete a geofence
- Get a single geofence
- Get a list of geofences
- Update a geofence

### GPS Tracking
- Get a list of trips
- Get a list of GPS data records

### Events
- Get a list of events

### Users
- Create a user
- Activate a user
- Deactivate a user
- Update a user
- Update a partial user
- Get a single user
- Get a list of users 

### Vehicles
- Create a vehicle
- Activate a vehicle
- Update a vehicle
- Update partial vehicle information
- Deactivate a vehicle
- Delete a vehicle
- Get a single vehicle
- Get a list of vehicles


## Obtaining Credentials
Developer API portal credentials are managed in the TeamWherx web application. Please contact the application administrator for access.

After credentials are created, the developer portal can be accessed at: [TeamWherx APIs](https://developer.wfmplatform.com/)

## Known Issues and Limitations
- Audio file binary uploads (POST) for all endpoints are limited to MP4 only.
- Dates for all endpoints that require date-time information must be specified according to RFC3339 guidelines, as in the following example: 2021-09-22T02:36:56.52Z. It is not necessary to specify the exact seconds in the timestamp; 00 is acceptable.

## Deployment Instructions
- N/A

## Throttling Limits
| Name	| Calls	| Renewal Period |
| ----- | ------ | ------- |
API calls per connection | 100 | 15 seconds 
Frequency of trigger polls | 1 | 48 seconds

