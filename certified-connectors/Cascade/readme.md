# Cascade Connector

Cascade is a cloud-based strategic planning and execution platform for teams to plan, execute, measure, and adapt their strategy all in one place. Connect your Cascade workspace to the rest of your tools to always have the most up-to-date and relevant data that pertains to your strategy.

## Prerequisites

To use this integration, you will need a Cascade Enterprise account to generate the necessary API token.

## Obtaining Credentials / API Token

For authentication, you need to use an API token. To learn how to generate an API token please visit this help article:

[How to generate an API access token](https://help.cascade.app/en/articles/3309052-how-to-generate-an-api-access-token)

## How to create a connection to Cascade's Power Automate connector

1. Select a Cascade action to use
2. Enter a 'Connection name', this will be the displayed connection name you will use to connect to Cascade
3. Within the 'API Key' field you must enter - 'Bearer YourAPIToken'
   - Where 'YourAPIToken' token above is replaced with your API Token from your Cascade Instance

## Features

Use the Cascade Power Automate connector to connect to your Cascade instance. It is perfect to

## Supported Operations

The connector supports the following operations:

- `Get all Goals`: Returns all Goals within an instance.
- `Create a Goal`: Creates a new Goal within an instance.
- `Get single Goal`: Returns a single Goal based off of its Goal ID.
- `Delete Goal`: Deletes a Goal based off of its Goal ID.
- `Update a Goal`: Updates an existing Goal's details based off of its Goal ID.
- `Get all Risks`: Returns all Risks within an instance.
- `Create a Risk`: Creates a new Risk on an existing Cascade Goal.
- `Get single Risk`: Returns a single Risk based off of its Risk ID.
- `Delete Risk`: Deletes a Risk based off of its Risk ID.
- `Update Risk`: Updates an existing Risk's details based off of its Risk ID.
- `Get all Tasks`: Returns all Tasks within an instance.
- `Create a Task`: Creates a new Task on an existing Cascade Goal.
- `Get single Task`: Returns a single Task based off of its Task ID.
- `Delete Task`: Deletes a Task based off of its Task ID.
- `Update Task`: Updates an existing Task's details based off of its Task ID.
- `Get all Updates`: Returns all Updates within an instance.
- `Create an Update`: Creates a new Update on an existing Cascade Goal.
- `Get single Update`: Returns a single Update based off of its Update ID.
- `Delete Update`: Deletes an Update based off of its Update ID.
- `Update an Update`: Revises an existing Update's details based off of its Update ID.

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
