# Airlabs
Airlabs collects extensive real-time aviation and flight data from thousands of sources to cleanse, aggregate and organize it into comprehensive collections that enable developers to create innovative products and deliver the perfect user experience for travels.

## Publisher: Fördős András

## Prerequisites
An Airlabs account is required. You can sign up for a free plan or select from the paid memberships: [https://airlabs.co/#Pricing](https://airlabs.co/#Pricing)

## Obtaining Credentials
This connector uses API-Key authentication. Once signed up, visit your profile home page to get your API-Key: [https://airlabs.co/account](https://airlabs.co/account)

## Supported Operations

### List real-time flight data
Use flight data api to monitor individual flights, display information in your applications, and guide users through flight preparation. Provide flight data on a map, displaying absolutely all active flights in a convenient format with constantly updated information on status, coordinates, speed, direction and other useful data.

### Get flight information
The detailed Flight Data service is a perfect combination of schedule data, real-time flight data, and aircraft information for a specific scheduled & live flight.

### List airlines
A complete and reliable set of airline data to support your applications and quality of service.

### List airline routes
Together with Schedules you can use data from Airline Routes Database to predict flight schedules for the distant future. Knowing what days of the week and what times a flight departs, you can easily build any connecting routes using discrete mathematics and graph theory methods.

### List airport schedules
The real-time flight schedule database reflects the current status of the departure and boarding queue: each departure or arrival airport displays these schedules in the lounges and terminal or gate areas. Using Flight Schedules, you can create your own time table - in the app, on the site, in watches, bots, and wherever.

### List airports
A complete set of airports database, which will allow you to display destination maps, display the necessary data on your websites or in your applications in the format you want.

### List countries
A complete set of country data to support your application. Use it to integrate data when you need localization or to automatically display local currency.

## Known Issues and Limitations

The current version of the connector is an initial one, meaning it does not provide access to all the service endpoints and response data provided by Airlabs, especially around the paid services. 

Please reach out and connect me when you see the need for any additional extension and let us collaborate!

Important note, that the underlying service has various limitations applied (rates and data) based on your membership.

By design the Airlabs endpoint returns in every response the request parameters too, meaning the api_key can be exposed to your end users if they have access to the raw response in PowerAutomate or PowerApps. This connector does not provide any parsed property to these details, rendering it less accessible - but still available - for end users.