## Azure Cognitive Services Batch Speech-to-text Connector

The Speech Services batch transcription API is a cloud-based service that provides batch speech recognition asynchronous processing over provided audio contents. This connector exposes these functions as operations in Microsoft Power Automate and Power Apps.

## Pre-requisites

You will need the following to proceed:

- A Microsoft Power Apps or Microsoft Power Automate plan with custom connector feature
- The Power platform CLI tools
- A Microsoft Azure Cognitive Services speech resource.

## Deploying the connector

Run the following command and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```

## Supported Operations

The connector supports the following operations:

- `Create transcription`: Creates a new transcription.
- `Get transcription`: Gets the transcription identified by the given ID.
- `Get transcription files`: Gets the files of the transcription identified by the given ID.
- `Delete transcription`: Deletes the specified transcription task.
