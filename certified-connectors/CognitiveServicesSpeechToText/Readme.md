## CognitiveServicesSpeechToText

Azure Cognitive Services Batch Speech-to-text Connector is built based on Speech Services batch transcription API which is a cloud-based service that provides batch speech recognition asynchronous processing over provided audio contents. This connector exposes these functions as operations in Microsoft Power Automate and Power Apps.

## Publisher: Speech Services team

Microsoft Azure Cognitive Services Speech Services team​

## Prerequisites

You will need the following to proceed:

- A Microsoft Power Apps or Microsoft Power Automate plan with custom connector feature
- The Power platform CLI tools
- A Microsoft Azure Cognitive Services speech resource.

## Supported Operations

The connector supports the following operations:

### Create transcription

Creates a new transcription.

### Get transcription

Gets the transcription identified by the given ID.

### Get transcription files

Gets the files of the transcription identified by the given ID.

### Delete transcription

Deletes the specified transcription task.

## Obtaining Credentials

Connector will use the key authentication for backend services.

## Known Issues and Limitations

Please refer to the Speech Services batch transcription quotas and limits.

## Deployment Instructions

Run the following command and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```
