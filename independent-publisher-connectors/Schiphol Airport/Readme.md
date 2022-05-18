# Schiphol Airport
Schiphol Airport (AMS IATA) is one of the busiest airports in the world. It is 15 km (9.3 mi) southwest of Amsterdam in the Netherlands. 
It serves the city of Amsterdam and the rest of the Netherlands. It is one of the major hubs in Europe with flights between many destinations in Asia, Europe and North America. 
It is home to KLM, the Dutch flag carrier airline. As of 2016, Schiphol airport is the third busiest in Europe behind London Heathrow and Paris Charles de Gaulle and ahead of Frankfurt Airport as measured by passenger numbers.

## Publisher: Michel Gueli

## Prerequisites
You need a Schiphol Developer account. You can get one for free [here](https://developer.schiphol.nl/signup).

## Obtaining Credentials
In all requests both the APP ID and APP KEY have to be provided. These settings are visible in the Dashboard, Show API Keys.

## Supported Operations

### Aircraft Types
Retrieves list of aircrafttypes. [More info](https://developer.schiphol.nl/apis/flight-api/v4/flights?version=latest#!/aircraft-type-controller/retrieveAllAircraftTypesUsingGET)

### Airlines
Retrieves list of airlines. [More info](https://developer.schiphol.nl/apis/flight-api/v4/flights?version=latest#!/airline-controller/retrieveAllAirlinesUsingGET)

### Airline by IATA or ICAO
Retrieves airline based on code (IATA or ICAO). [More info](https://developer.schiphol.nl/apis/flight-api/v4/flights?version=latest#!/airline-controller/retrieveAirlineUsingGET)

### Destinations
Retrieves list of destinations. [More info](https://developer.schiphol.nl/apis/flight-api/v4/flights?version=latest#!/destination-controller/retrieveAllDestinationsUsingGET)

### Destination by IATA 
Retrieves destination based on IATA code. [More info](https://developer.schiphol.nl/apis/flight-api/v4/flights?version=latest#!/destination-controller/retrieveDestinationUsingGET)

### Flights by id
Retrieves a Flight based on flight-id. [More info](https://developer.schiphol.nl/apis/flight-api/v4/flights?version=latest#!/flight-controller/retrieveFlightUsingGET)

### Flights
Retrieves flights for a specific date. [More info](https://developer.schiphol.nl/apis/flight-api/v4/flights?version=latest#!/flight-controller/retrieveFlightsForDateOrPeriodUsingGET_1)

## Deployment Instructions
Upload the connector and authorize by providing your APP ID and APP KEY. 

## Known Issues and Limitations
Note that a maximum of 20 results will be included in each response from the API. When more results are found pagination is applied.