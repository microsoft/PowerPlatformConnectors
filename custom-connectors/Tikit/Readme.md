# Tikit [Sample] Connector 

 
 

Tikit supplies a basic ticket management system. Using this API, you can create tickets and files associated with it. Very often, you may want to use the business with tickets. This is the solution for it. 

 
 

# Publisher: Celebal Technologies 

 
 

Celebal Technologies is a premier software services company in the field of Data Science, Big Data, and Enterprise Cloud.  

 
 

# Prerequisites 

 
 

You will need the following prerequisites to go ahead: 

 
 

* A Microsoft Power Apps or Power Automate plan with custom connector feature. 

* An API Key to use with custom connector. 

* The Power Platform CLI (Command Line Interface) tools. 

 
 

# Supported operations 

 
 

The connector supports the following operations: 

 
 

* File: This let you add files to the previously added tickets. 

    * AddFile: Add file to the specified ticket. 

    * GetFile: Get file with a specified file id. 

    * GetFiles: Get all the files with the help of apikey previously got. 

    * UpdateFile: Update the ticket file with the help of unique fileId. 

 
 

* Ticket: This lets you add tickets in the database. 

    * GetTickets: Gets all the tickets. 

    * AddTicket: Add ticket to the database. 

    * UpdateTicket: Update the ticket with the help of unique ticket id. 

    * GetTicket: Get ticket with the help of unique id of the ticket. 

 
 

# Building the tools: Obtaining the credentials 

 
 

Since Tikit is a ticket management system and is secured by API key do first you need an api key to interact with it. You will find more information about this [here](https://tikit/azurewebsites.net) and follow the steps below: 

 
 

1. Get your API Key by registering yourself on [official](https://tikit.azurewebsites.net) website as a user. 

2. Add your [API Key](https://tikit.azurewebsites.net/APIKey) after registering as a user.  

3. Setup power automate and power apps for use with the tikit api. 

 
 

At this point you will be able to access the connector with the api key previously got by the website. 

 
 

# Known Issue and Limitations 

 
 

None. 

 
 
 

# Deployment Instructions 

 
 

You may use deploy connector by following of way: 

 
 

> #### Microsoft Power Platform Connector CLI.  

>By using [paconn](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) cli by using following commands. 

>- Install python and install paconn. 

>- Download connector files from source. 

>- Use paconn to create a connector in environment of your choice. 

 
 
 
 

# Frequently Asked Questions 

- Question: After deploying connector to my respective environment connector trigger is not working? 

    - Answer: You need to change the parameters in the custom connector. Under definition section of your custom connector update the trigger configuration and select 'callbackURL' as a parameter. 

- Question: How do I get API Key to use this connector. 

    - Answer: You may get the connector API Key by registering yourself with the [portal](https://tikit.azurewebsites.net/). 

 
 

 