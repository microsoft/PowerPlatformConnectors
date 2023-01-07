# Postman

Postman is an API platform for building and using APIs. Postman simplifies each step of the API lifecycle and streamlines collaboration.

## Publisher: Fördős András

## Obtaining Credentials

The service needs a registered user and his/her valid API key.

1. Sign up to Postman: [https://www.postman.com/](https://www.postman.com/)
2. Generate API key: [https://web.postman.co/settings/me/api-keys](https://web.postman.co/settings/me/api-keys)

## Supported Operations

### Get authenticated user
Gets information and usage details about the authenticated user.

### List all workspace
List all workspaces available for the authenticated user.

### Get a workspace
Gets information about a specific workspace.

### Create a workspace
Creates a new workspace for the authenticated user.

### List all environments
Get information about all of your environments.

### Get an environment
Gets information about a specific environment.

### List all collections
List all of your subscribed collections.

### Get a collection
Gets information about a specific postman collection.

### Import OpenAPI
Import an OpenAPI (or swagger) definition to your selected workspace.

## Known Issues and Limitations

Current version of the connector only supports a subset of the available service endpoints and response data. Contact me if you see a need to bring in any of the other available ones and we can collaborate!

The underlying service itself has some limitations: [https://learning.postman.com/docs/developer/intro-api/#rate-limits](https://learning.postman.com/docs/developer/intro-api/#rate-limits)
