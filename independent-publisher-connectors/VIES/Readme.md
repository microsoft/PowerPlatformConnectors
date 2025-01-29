# VIES
VIES (VAT Information Exchange System) is an electronic mean of validating VAT-identification numbers of economic operators registered in the European Union for cross border transactions on goods or services. You can verify the validity of a VAT number issued by any Member State / Northern Ireland by selecting that Member State / Northern Ireland from the drop-down menu provided, and entering the number to be validated.

## Publisher: Tomasz Poszytek

## Prerequisites
No prerequisites.

## Supported Operations
### Check VAT validity
Action looks up data in VIES database to see, if for given country's ISO code and VAT number there is a registered company. If data is valid, service will return "true" along with company's details. Otherwise it will return false. 

## Obtaining Credentials
No credentials needed. 

## Known Issues and Limitations
Sometimes requests to VIES may take more time than expected which may lead to timeouts. If you face such issues, try building requests to VIES in a loop running until a correct response is received. Apart from that, no other issues or limitations are known at the moment of api creation.

## Changelog

1.0: Initial release of the Independent Publisher Connector.
2.0: Updates to the connector caused by changes in VIES endpoint.

## Getting Started
Visit https://ec.europa.eu/taxation_customs/vies/vatRequest.html to test the api on your own.