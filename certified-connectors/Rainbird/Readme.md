# Rainbird

Rainbird is a no-code intelligent automation platform that enables you to automate complex decisions.
This connector enables you to interact with knowledge maps you’ve built in Rainbird.

## Publisher: Rainbird Technologies Ltd

## Prerequisites

You will need the following to proceed:
-A Rainbird account (a free community account, or an enterprise licenced account)
-A Microsoft Power Apps or Power Automate plan


## Supported Operations


### Start:

Create a new session ID on a knowledge map. This first step will allow you to make further actions against your Rainbird model, such as injecting facts or doing a query.

### Inject:

Inject data in a knowledge map against a specific session ID. This is usually performed prior to a query. It's a way to upload data into a Rainbird model.

### Query:

Ask a question to a knowledge map. This action triggers Rainbird's decision engine. It performs the automated decisioning and then provides a final answer. If Rainbird needs more information to come to a conclusion, it will ask a question.

### Response:

Give an answer back to Rainbird. When Rainbird comes back with a question to a query, it expects a user or a system to input more data. This action provides a response back.

### Undo:

Undo a previous action. This action removes the last action performed. It removes the last piece of data given to Rainbird.

### Evidence:

Generate evidence based on a fact ID. Knowledge inferred by Rainbird always comes with data to explain the decision-making process. This data is made accessible through this action.

### Version:

Return the current Rainbird's inference engine version.


## Obtaining Credentials

To create a connection with Rainbird, you'll need your account API key. You can find it in your account settings (in the account menu or in the publish menu of any knowledge map).
Use your API key as a username, and use any character as a password.

## Known Issues and Limitations

This version of the Rainbird connector doesn't allow you to access your evidence tree with the evidence key. You will need to enable your models to publicly share links to evidence and interaction data if you want to generate accessible evidence tree links. You can set this in the publish menu of any knowledge map.

## Deployment Instructions

To deploy this connector as a custom connector you need to go into the "custom connector" page of Power automate, and follow these instructions:
-Create a new custom connector by importing an OpenAPI file. Use the "apiDefinition.swagger.json" from this project.
-Once in the connector creation wizard, go to "3-Definition" and create a new policy. Name it "dynamic host" and choose "Set host URL" as a template.
-Two more options should appear. In "Operations" select all the existing operations (7 in total). In "Url Template", enter the following "https://@queryParameters('environment')" (without the double quotes).
-You now need to validate your connector. Click on "Create connector" and use Rainbird in your flows to automate complex decisions.
