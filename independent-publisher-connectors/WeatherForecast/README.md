# Weather Forecast Connector
This connector will help you have weather forecast of cities you look for. It can give accurate results of upto 3 days in advance.

## Publisher: Haimantika Mitra

## Prerequisites
You will need the following:
- A Microsoft Power Apps or Power Automate plan with custom connector feature
- An account on https://openweathermap.org/
- [API Key] (https://openweathermap.org/)

## Obtaining Credentials
- Sign in to https://openweathermap.org/.
- Locate 'My API keys' under your profile.
- Generate keys. Use the generated key for aunthentication

## Supported Operations
This connector will help you have weather forecast of the requested city. It returns the following data:
- coordinates
- temperature (max,min)
- pressure
- humidity
- sea level
- ground level
- visibility
- clouds
- wind (speed,deg, gust)
- sunrise, sunset

## Known Issues and Limitations
Limitation:
- You can query only one city at a time.

## Deployment Instructions
1. Clone the PowerPlatformConnectors GitHub repository
2. Open a terminal, then change to the WeatherForecast directory, found in samples of the cloned repository
3. Run `paconn login`, then follow the authentication steps
4. Once authenticated, run paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
5. Select the target environment for your connector
6. Create a new flow in Power Automate, or a new Power App, using the connector. When prompted, create a new connection with your API Key and city whose weather forecast you want to know. 'q' will be the city, along with the country code, and your 'appid' will be the authentication key.