# iMIS
iMIS is the world’s only Engagement Management System (EMS), purpose-built to meet the unique needs of your organization and to support your continuous performance improvement. The iMIS connector allows you to manage and connect your iMIS data to thousands of other online services.

## Publisher
Computer System Innovations, Inc.

*Stack Owner: Advanced Solutions International, Inc.*

## Prerequisites
This connector is intended for customers with active iMIS subscriptions or installations. Please see our documentation for a list of iMIS connector prerequisites: https://csi.ms/paprerequisites

## Supported Operations

### Triggers

| Name | Description |
|------|-------------|
| When an activity is added | Occurs when an activity is added into iMIS. (Does not fire on updates or deletes.) |
| When an order is added | Occurs when any order (that is added to the Orders table) is added into iMIS. (Does not fire on updates or deletes.) |

### Actions

| Name | Description |
|------|-------------|
Find contacts | Search for contact records, optionally filtering by some search criteria
Get a contact | Get a single contact by iMIS ID
Get a contact's picture | Get the profile picture for a contact record by iMIS ID
Create a contact | Create a new contact (person) record
Update a contact | Update fields on a person's record
Get all addresses for a contact | Get a list of addresses for a contact
Get an address for a contact | Get an address by iMIS ID and address purpose
Update a contact's mailing address | Update fields on a contact's mailing address
Create an organization | Create a new contact (organization) record
Update an organization | Update fields on an organization's record
Find activities | Search for activities, optionally filtering by some criteria
Get an activity by sequence | Get a single activity record by its sequence (SEQN) number
Create an activity | Create a new activity record
Update an activity | Update fields on an existing activity record
Execute an IQA | Execute an IQA (parameters are supported) and return the results for subsequent actions
Find panel records | Search for panel records or business objects, filter by any property on the panel
Get a single-instance panel record | Get a single-instance panel record by iMIS ID and panel name
Update a single-instance panel record | Update fields on a single-instance panel record
Delete a single-instance panel record | Delete a single-instance panel record for a given iMIS ID
Get a multi-instance panel record | Get a single multi-instance panel record by iMIS ID and Ordinal (or other primary key)
Create a multi-instance panel record | Create a new multi-instance panel record
Update a multi-instance panel record | Update fields on an existing multi-instance panel record
Delete a multi-instance panel record | Delete a multi-instance panel record by iMIS ID and Ordinal (or other primary key)

## Obtaining Credentials
Please refer to our documentation on how to obtain an API Key here: https://csi.ms/paapikey

## Getting Started
To get started with the iMIS connector, please see our Getting Started guide here: https://csi.ms/pagettingstarted

## Known Issues and Limitations
For documentation on known limitations, issues, and troubleshooting steps, please see: https://csi.ms/palimitations

## Deployment Instructions
It is recommended to use the officially published iMIS connector where possible. To deploy the iMIS connector as a custom connector, please follow these instructions: https://csi.ms/pacustominstall
