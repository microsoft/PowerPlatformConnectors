# Ideal Postcodes Connector

This connector helps you perform UK address lookups 

## Prerequisites

1. An Ideal Postcodes account `https://ideal-postcodes.co.uk/users/sign_up`
2. An Ideal Postcodes API Key

## Setup

1. Clone the PowerPlatform Connectors GitHub repository
2. Open a terminal session and change to the IdealPostcodes directory within the samples directory
3. Run `paconn login` and follow authentication steps
4. Run `paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json`
5. Select your target enivronment
6. Create a Flow or PowerApp and when asked to create a connection enter the API key obtained from the Ideal Postcodes dashboard

## Supported Actions

- Address Search
- Get Address by UDPRN
- Get Addresses by Post Code
- Lookup UDPRN
- Lookup UDPRM
