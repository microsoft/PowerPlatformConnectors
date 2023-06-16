# Unique Connector
The Unique connector is intended to allow you to customize the recording post-processing flow by triggering actions when a webhook notification from Unique is received. 

## Publisher: Unique
Th connetor has been developed and is maintained by Unique AG development team.

## Prerequisites
- A Microsoft Power Apps or Power Automate plan with custom connector feature
- An Azure subscription

## Supported Operations
The connector supports the following operations:
* `Transcription ready trigger`: Get a notification whenever the transcription of a recording is ready and post-processing of the recording is available. 
* `Analyze`: Get an analysis the speaking time per recording participant and compute overall sentiment analysis. 

## Obtaining Credentials
Calls to Unique API require an API Key provided to you on request to our customer success team.  


## Known Issues and Limitations
This connector is a work in progress and will be gradually updated with new actions on an ongoing basis. 

## Deployment Instructions
This connector is intended to be used in Logic Apps. 
