# Carbon Intensity
This connector uses National Grid’s Carbon Intensity API to provide an indicative trend of regional carbon intensity of the electricity system in Great Britain (GB).

## Publisher
### Hasan Ünlü

## Prerequisites
Carbon Intensity API does not require authentication to use.

## Supported Operations

### Intensity
Gets Carbon Intensity data for current half hour. All times provided in UTC (+00:00).

### Intensity Factors
Gets Carbon Intensity factors for each fuel type.

### Generation
Gets generation mix for current half hour. All times provided in UTC (+00:00).

### Regional
#### England
Gets Carbon Intensity data for current half hour for England. All times provided in UTC (+00:00).
#### Scotland
Gets Carbon Intensity data for current half hour for Scotland. All times provided in UTC (+00:00).
#### Wales
Gets Carbon Intensity data for current half hour for Wales. All times provided in UTC (+00:00).

## API Documentation
[Carbon Intensity APIs reference](https://api.carbonintensity.org.uk/)

## Known Issues and Limitations
Not all operations provided by Carbon Intensity are part of the current connector submission.
