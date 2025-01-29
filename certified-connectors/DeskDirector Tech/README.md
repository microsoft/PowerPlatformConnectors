## DeskDirector Tech Connector
The DeskDirector Tech connector is a tool designed to enhance ticket management and workflow within the DeskDirector advanced ticket management system. It enables efficient tracking and handling of tickets with functionalities such as detailed search filters, full ticket content retrieval, and time entry management.

## Publisher: DeskDirector

## Prerequisites
You will need an active DeskDirector subscription with flow connector feature, you can read our pricing [here](https://www.deskdirector.com/pricing).

## Supported Operations

### Find tickets
Retrieve a list of tickets based on various filter options such as keyword search, status, assignment, task progress, last contact message, creation time, and tags.

### Get ticket full content
Get ticket full content by ticket ID which includes all ticket notes and time entries.

### Find time entries
Find my time entries based on the time period.

## Obtaining Credentials
When setting up a new connection, you will be asked to provide a personal access key, the key can be generated or retrived from your admin portal, see documentation [here](https://help.deskdirector.com/article/sjt29029hg-desk-director-tech-connector#create_a_connection).

## Known Issues and Limitations
Currently, the connector is limited to retrieving and managing existing ticket and time entry data.

## Deployment Instructions
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```
