# Swagger Converter
Swagger is a suite of tools for API developers from SmartBear Software and a former specification upon which the OpenAPI Specification is based. The Swagger Converter allows conversion of a 1.x or 2.x Swagger definition to the OpenAPI 3.0.1 format.

## Publisher: Fördős András

## Prerequisites
There are no prerequisites to use this service, but it is favorable to get familiar with Swagger and OpenAPI.

## Obtaining Credentials
There are no credentials needed to use this service.

## Supported Operations
### Convert a swagger definition by URL
Converts the supplied payload found under the URL to a 3.0 specification.

### Convert a swagger definition
Converts the supplied payload to a 3.0 specification.

## Known Issues and Limitations
Current version of the connector supports only JSON specifications as the received response format (converted specification).

The same limitation applies to input Swagger definition - only JSON is accepted if directly provided to the connector and not supplied through a URL.

The underlying service is capable of handling YAML too, so if this is a needed feature, get in contact and let's collaborate!
