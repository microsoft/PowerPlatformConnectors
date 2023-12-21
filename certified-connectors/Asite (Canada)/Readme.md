# Title
Asite is an open construction platform that enables organizations with comprehensive range of solutions connect dispersed teams across the lifecycle of capital assets by collaborate, plan, design, and build with seamless information sharing across the entire supply chain which helps capital project owners stay at the forefront of innovation, maintaining a golden thread of information.

The Asite connector helps to build a connection between two systems for file exchange. The connector collaborates both systems by uploading and downloading files based on triggers.

## Publisher: Publisher's Name
Asite Solutions Limited

## Prerequisites
- An active Asite CDE Subscription
- An active Microsoft Power Automate subscription
- An active workflow configuration in Asite platform, configured workflow trigger with the type "Microsoft Flow"

## Supported Operations

The connector supports the following operations:

- `Select Project Name`: It will list out all the Asite's projects where you have access.
- `Select Folder Name`: It will list out all the accessible folders based on your access from the selected Project.
- `When a workflow is triggered on file(s)`: This operation triggers a flow when a file is uploaded/updated on the project. The trigger is fired to include sub-folders based on the workflow configuration.
- `Get file content`: Retrieves the file content from Asite.
- `List of configured Triggers from Asite Platform`: To display list of configured Triggers from Asite Platform
- `Delete configured trigger`: To Delete configured trigger.
- `Get Dynamic Schema based on project and folder`: Get Dynamic Schema based on project and folder.
- `Set file metadata`: Retrieves standard and custom metadata.
- `Create file`: Upload a file in Asite project folder.
- `Get dynamic schema based on project Id and trigger Id`: Get dynamic schema based on project Id and trigger Id.
- `Select Folder Name`: List out all the accessible folders based on your access from the above selected Project.
- `When a workflow is triggered on App(s)`: This operation triggers a flow when a form is created/updated on the project. Configure separate flows for each App.
- `Get dynamic schema based on project Id and trigger Id`: Get dynamic schema based on project Id and trigger Id


## Obtaining Credentials
An owner of the organization must log into Asite and authorize the connection.

## Connector Documentation
For detailed documentation around the connector please refer to
https://help.asite.com/en/collections/3576597-understanding-asite-integration-via-microsoft-power-automate-flow

## Known Issues and Limitations
For issues with Asite please try this link first: https://help.asite.com/en/collections/3576597-understanding-asite-integration-via-microsoft-power-automate-flow


## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.
