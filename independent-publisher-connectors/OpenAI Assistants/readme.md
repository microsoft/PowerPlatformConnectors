# OpenAI Assistants
OpenAI Assistants allows you to build AI assistants within your own applications. An Assistant has instructions and can leverage models, tools, and knowledge to respond to user queries. The Assistants service currently supports three types of tools: Code Interpreter, Retrieval, and Function calling.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You need an OpenAI account and available tokens to use this connector.

## Obtaining Credentials
Once logged in to your account, you will need to create a new API key.

## Supported Operations
### List assistants
Retrieves a list of assistants.
### Create assistant
Create an assistant with a model and instructions.
### Retrieve assistant
Retrieves information about an assistant.
### Delete assistant
Deletes an assistant.
### Get assistant files
Retrieves a list of assistant files.
### Create assistant file
Create an assistant file by attaching a file to an assistant.
### Retrieve assistant file
Retrieves an assistant file.
### Delete assistant file
Deletes an assistant file.
### Create thread
Creates a thread that assistants can interact with.
### Get thread
Retrieve a thread.
### Delete thread
Deletes a thread.
### Modify a thread
Modifies a thread.
### List messages
Retrieve a list of messages for a given thread.
### Create message
Create a message within a thread.
### Modify message
Modifies a message.
### Retrieve message file
Retrieves a message file from a thread.
### List message files
Retrieves a list of message files for a thread.
### List runs
Retrieve a list of runs for a thread.
### Create run
Creates a run for a thread.
### Get run
Retrieve a run from a thread.
### Modify run
Modifies a run for a thread.
### Submit tool outputs to run
When a run has the status: "requires_action" and required_action.type is submit_tool_outputs, this action can be used to submit the outputs from the tool calls once they're all completed.
### Cancel a run
Cancel a run for a thread that is in progress.
### Create thread and run
Creates a thread and run it in one request.
### Get run step
Retrieve a run step for a thread.
### List run steps
Retrieve a list of run steps for a thread.
### List models
Lists the currently available models, and provides basic information about each one such as the owner and availability.

## Known Issues and Limitations
There are no known issues at this time.
