# Appwrite
With this connector, you can integrate Appwrite backend as a service's offering to cut up to 70% of the time and costs required for building a modern application. It abstracts and simplifies common development tasks behind a REST APIs, to help you develop your app in a fast and secure way.

## Publisher: Osazee Odigie

## Prerequisites
Users will need an **API Key** to authenticate. To create a new API key, go to your API keys tab in your project setting using your Appwrite console and click the 'Add API Key' button.

When adding a new API Key, you can choose which scope to grant your application. If you need to replace your API Key, create a new key, update your app credentials and, once ready, delete your old key.

## Supported Operations
### Get Account
Get currently logged in user data as JSON object.

### Get Browser Icon
You can use this endpoint to show different browser icons to your users. The code argument receives the browser code as it appears in your user [GET /account/sessions](/docs/client/account#accountGetSessions) endpoint. Use width, height and quality arguments to change the output settings.When one dimension is specified and the other is 0, the image is scaled with preserved aspect ratio. If both dimensions are 0, the API provides an image at source quality. If dimensions are not specified, the default size of image returned is 100x100px.

### Get Image from URL
Use this endpoint to fetch a remote image URL and crop it to any image size you want. This endpoint is very useful if you need to crop and display remote images in your app or in case you want to make sure a 3rd party image is properly served using a TLS protocol.When one dimension is specified and the other is 0, the image is scaled with preserved aspect ratio. If both dimensions are 0, the API provides an image at source quality. If dimensions are not specified, the default size of image returned is 400x400px.

### List Databases
Get a list of all databases from the current Appwrite project. You can use the search parameter to filter your results.

### Get Database
Get a database by its unique ID. This endpoint response returns a JSON object with the database metadata.

### Delete Database
Delete a database by its unique ID. Only API keys with with databases.write scope can delete a database.

### Get Collection
Get a collection by its unique ID. This endpoint response returns a JSON object with the collection metadata.

### Create Function
Create a new function. You can pass a list of [permissions](/docs/permissions) to allow different project users or team with access to execute the function using the client API.

### Get HTTP
Check the Appwrite HTTP server is up and responsive.

### Get DB
Check the Appwrite database server is up and connection is successful.

### Get User Locale
Get the current user location based on IP. Returns an object with user country code, country name, continent name, continent code, ip address and suggested currency. You can use the locale header to get the data in a supported language.([IP Geolocation by DB-IP](https://db-ip.com))

## Known Issues and Limitations
No identified issue yet.

## API Documentation
API: [https://appwrite.io/docs/rest/](https://appwrite.io/docs/rest/)
<br/>
Terms of Use: [https://appwrite.io/policy/terms](https://appwrite.io/policy/terms)