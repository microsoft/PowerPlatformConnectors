# Lang.ai Connector
Lang.ai supercharges your customer service platform by automating time-consuming manual tasks like tagging, prioritization and routing.
Unsupervised AI that literally takes minutes to implement as you build your tags combining concepts visually instead of spending months using a traditional AI approach.

Lang.ai connector provides a powerful REST API.  Using this API, you can send new texts to an specific project in Lang.ai to analyze, retrieve list of projects from your Lang.ai account and list all the tags in one specific project.


## Prerequisites
You will need the following to proceed:
* A Lang.ai susbscription (API token)
* At least 1 project created in Lang.ai vía web platform


## How to get credentials?
* Contact Lang.ai representatives by requesting a demo at https://lang.ai/demo
* Send a message to sales@lang.ai asking for an access, our team will get back to you as soon as possible.
* Once you’ve subscribed to Lang.ai, you’ll be able to generate a token to use for the connector.

## Set up a project in Lang.ai
In order to use Lang.ai connector to automate the analysis in your text processes, you will need to follow these steps:

1. Create a project
You will need to create a project in Lang.ai uploading a dataset with your historical data, using Lang.ai web platform. You can read more about this [here](https://help.lang.ai/en/articles/2694436-creating-projects-and-uploading-datasets) 

2. Create classification model and tags
Once you have uploaded the data, it's time to create a classification model to analyze new text in real-time. This must be done using Tags. You can read more about this [here](https://help.lang.ai/en/articles/3175538-using-tags) 

3. Connect and analyze in real-time
In order to connect your own classification model wiht your text process you will need to create an API token to use it. You can generate an API token in `Settings > Channels > API > Token Access`

## Supported Operations
The connector supports the following operations:
* `Analyze documents`: Returns the classification for a given document and the specified project.
* `Save documents`: Saves a given document into the specified project. It supports passing metadata that can be later used in the project's Dashboard. New metadata values passed via API will be available to use in the project's setup section.
* `List projects`: Returns the list of projects.
* `List project tags`: Returns the list of the project's tags.




