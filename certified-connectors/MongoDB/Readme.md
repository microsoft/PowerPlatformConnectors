# MongoDB Connector
MongoDB is the leading modern, general purpose, document-based, distributed database, designed to unleash the power of software and data for developers and the applications they build.
The [MongoDB Data API ](https://www.mongodb.com/data-api/l) provides you REST access to your data in [MongoDB Atlas ](https://www.mongodb.com/atlas),the database-as-a-service offering by MongoDB. The API includes endpoints that create, read, update, delete, and aggregate documents in your cluster.

## Publisher: MongoDB

## Prerequisites
1. [Create an MongoDB Atlas Cluster](https://www.mongodb.com/docs/atlas/tutorial/create-new-cluster/)

Get started by creating a [MongoDB Atlas Account](https://www.mongodb.com/cloud/atlas/register) with a running Cluster, hosting the Database that you want to connect to from the Power Platform.  

2. [Enable the Data API](https://www.mongodb.com/docs/atlas/api/data-api/#1.-enable-the-data-api)

3. [Create a Data API Key](https://www.mongodb.com/docs/atlas/api/data-api/#2.-create-a-data-api-key)

## Supported Operations
Perform CRUD operations and aggregations on your data in minutes. Below are teh supported operations:
1. [Find a Single Document](https://www.mongodb.com/docs/atlas/api/data-api-resources/#find-a-single-document)
2. [Find Multiple Documents](https://www.mongodb.com/docs/atlas/api/data-api-resources/#find-multiple-documents)
3. [Insert a Single Document](https://www.mongodb.com/docs/atlas/api/data-api-resources/#insert-a-single-document)
4. [Insert Multiple Documents](https://www.mongodb.com/docs/atlas/api/data-api-resources/#insert-multiple-documents)
5. [Update a Single Document](https://www.mongodb.com/docs/atlas/api/data-api-resources/#update-a-single-document)
6. [Update Multiple Documents](https://www.mongodb.com/docs/atlas/api/data-api-resources/#update-multiple-documents)
7. [Replace a Single Document](https://www.mongodb.com/docs/atlas/api/data-api-resources/#replace-a-single-document)
8. [Delete a Single Document](https://www.mongodb.com/docs/atlas/api/data-api-resources/#delete-a-single-document)
9. [Delete Multiple Documents](https://www.mongodb.com/docs/atlas/api/data-api-resources/#delete-multiple-documents)
10. [Run an Aggregation Pipeline](https://www.mongodb.com/docs/atlas/api/data-api-resources/#run-an-aggregation-pipeline)

Read the full documentation of the operations and their examples [here](https://www.mongodb.com/docs/atlas/api/data-api-resources/#data-api-resources)

## Getting Started
1. In the Base URL field in the General tab, replace the placeholder **"_Data API App ID_"** with the App ID from the URL endpoint under **Data API** tab in your Atlas cluster.  
2. For some APIs which expect a Json request or response , the layout/sample of the json needs to be added using the **"Import from Sample"** option for the Request or Response under the **Definition** tab.

## Known Limitations

1. Data API logs all requests and stores the logs for 30 days.They can be viewed from the **Data API** screen in the **Logs** tab
2. The Data API has restrictions in terms of the size of the Request and the Response body and the response time which is detailed [here](https://www.mongodb.com/docs/atlas/api/data-api/#request-limitations)

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps

