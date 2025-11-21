# ACT Azure OpenAI (Independent Publisher)

## Overview  
The ACT Azure OpenAI (Independent Publisher) custom connector enhances the available functions and capabilities to easily interact with Azure AI Foundry Open AI models including advanced functionalities like using the Response API with chaining, files, etc. .

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

In addition, it can also be used generate codes and images using the create endpoint, chain the responses into a thread.

## Features  
### Authentication & Connection  
This connector uses API key authentication to connect to an Azure OpenAI Rest API in Azure Foundry?.  
To set up the connection:  
1. Obtain an api key and endpoint from Azure AI Foundry.  
2. In the connector in your environment, create a new connection and provide the key endpoint/host.  
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
1. In Power Automate / Power Apps, add the connector **ACT Azure OpenAI (Independent Publisher)**.  
2. Choose the connection you created above.  
3. Insert the action **Create Response**, configure parameters such as api-version, x-ms-oai-image-generation-deployment and body.  
4. Run the action.  

## Known Issues & Limitations  
- No known limitations beyond Microsoft's documented limitations of the Azure OpenAI API

## Support & Feedback  
If you encounter issues or would like to provide feedback, please contact:  
**contact@actvelocity.com**

