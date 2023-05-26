# Brønnøysundregistrene
Brønnøysundregistrene is a state administrative agency consisting of 18 different state electronic registers. 
The purpose is to create order and simplification for the business community and for the public.

## Publisher
### Ahmad Najjar

## Prerequisites
No prerequisites are needed. The data provided by Brønnøysundregistrene is totally free.

## Supported Operations
### [Get all or Search entities](https://data.brreg.no/enhetsregisteret/api/docs/index.html#enheter-sok)
Returns all or search entities by name, registration date and/or bankruptcy. Results can be sorted. 

### [Get entity by organization number](https://data.brreg.no/enhetsregisteret/api/docs/index.html#enheter-oppslag)
Returns a single entity by organization number (Existing or Deleted)

### [Get entity roles](https://data.brreg.no/enhetsregisteret/api/docs/index.html#_roller)
Returns all roles for a specific entity by organization number

### [Get all or search sub entities](https://data.brreg.no/enhetsregisteret/api/docs/index.html#underenheter-sok)
Returns all or search sub entities by name. Results can be sorted. 

### [Get sub entity by organization number](https://data.brreg.no/enhetsregisteret/api/docs/index.html#underenheter-oppslag)
Returns a single sub entity by organization number (Existing or Deleted)

### [Get entities updates](https://data.brreg.no/enhetsregisteret/api/docs/index.html#oppdaterte-enheter)
Search for recently updated entities by date or/and update id in the registry.

### [Get sub entities updates](https://data.brreg.no/enhetsregisteret/api/docs/index.html#oppdaterte-underenheter)
Search for recently updated sub entities by date or/and update id in the registry.

## API Documentation
Visit Brønnøysundregistrene [Open data - Entities Registry](https://data.brreg.no/enhetsregisteret/api/docs/index.html) documentation page for further details.

## Known Issues and Limitations
Some operations returns HTTP 400 error if no data is found: (Most likely wrong organization number)
* Get entity by organization number
* Get sub entity by organization number
* Get entity roles

## Deployment Instructions
Import the swagger file in target Power Automate or Power Apps environment.

#### Not all operations provided by Brønnøysundregistrene are part of the first IP connector submission. I will keep adding/updating/supporting this connector based on your feedback/requests :)