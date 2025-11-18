# SVC Azure OpenAI (Independent Publisher)

## Overview  
The SVC Azure OpenAI (Independent Publisher) custom connector allows makers to easily interact with our company’s SVC Azure AI Foundry REST API.

With this connector you can

- Create an AI Response to a query
- Get a given response
- Delete a given response
- Returns a list of input items for a given response.
- Returns a list of files.
- Upload a file that can be used across various endpoints
- Returns information about a specific file.
- Delete a file and remove it from all vector stores.
- Returns the contents of the specified file.
- Transcribes audio into the input language.
- Generates audio from the input text.

## Features  
### Authentication & Connection  
This connector uses API key authentication to connect to our company’s SVC Azure AI Foundry REST API.  
To set up the connection:  
1. Obtain an api key from from <link or instructions>.  
2. In the connector in your environment, create a new connection and provide the key.  
3. Once connected, all actions/triggers will work as described.

### Actions & Triggers  
Here are the actions provided:  
- **Create Response** – Create an AI Response to a query 
- **Get Response** – Get a given response 
- **Delete Response** – Delete a given response
- **ListInputItems** – Returns a list of input items for a given response.
- **Files_List** – Returns a list of files.
- **Files_Upload** – Upload a file that can be used across various endpoints
- **Files_Get** – Returns information about a specific file.
- **Files_Delete** – Delete a file and remove it from all vector stores.
- **Files_GetContent** – Returns the contents of the specified file.
- **Transcript** – Transcribes audio into the input language.
- **TextToSpeech** – Generates audio from the input text.



### Getting Started  
1. In Power Automate / Power Apps, add the connector **SVC Azure OpenAI (Independent Publisher)**.  
2. Choose the connection you created above.  
3. Insert the action **Create Response**, configure parameters such as api-version, x-ms-oai-image-generation-deployment and body.  
4. Run the action.  

## Known Issues & Limitations  
- Transcript action only works for certain audio file types

## Support & Feedback  
If you encounter issues or would like to provide feedback, please contact:  
**svc_act1@ascension-team.com**

