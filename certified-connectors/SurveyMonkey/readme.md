# SurveyMonkey
SurveyMonkey is the world's leading provider of web-based survey solutions. These solutions are used by companies, organizations, and individuals to gather the insights they need to make more informed decisions.

## Prerequisites
To use this integration, you will need a SurveyMonkey account. 

## Publisher: Microsoft

## Obtaining Credentials
A SurveyMonkey account (trial or paid) is needed to register an application. You can start a trial [here](https://www.surveymonkey.com/). After that you can go to the [developer page](https://developer.surveymonkey.com/api/v3/#registering-an-app) and register your application there. From there you can grab the app key and secret.

## Supported Operations
#### Get a survey
Get details of a specific survey.

#### Schedule an existing invite message
Send or schedule to send an existing message to all message recipients.

#### When a new collector response is added
Triggers a new flow when a response is added (paid account only).

#### When a new survey response is added
Triggers a new flow when a survey response is added (paid account only).

#### When a new survey is created
Triggers a new flow when a survey is created.

#### When a new collector is created
Triggers a new flow when a collector is created.


## Known issues and limitations
The connector is based on integration with [surveymonkey.com](https://www.surveymonkey.com/) portal accounts. Other region-specific instances are not supported.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps