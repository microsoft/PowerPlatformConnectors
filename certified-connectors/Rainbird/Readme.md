# Rainbird Connector

Rainbird is a no-code intelligent automation platform that enables you to automate complex decisions.
This connector enables you to interact with knowledge maps you’ve built in Rainbird.


## Prerequisites


You will need the following to proceed:

-A Microsoft Power Apps or Power Automate plan with custom connector feature
-A Rainbird account 
-The Power platform CLI tools


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

Generate an evidence-based on a fact ID. Knowledge inferred by Rainbird always come with data to explain the decision-making process. This data is made accessible through this action.

### Version:

Return the current Rainbird's inference engine version.