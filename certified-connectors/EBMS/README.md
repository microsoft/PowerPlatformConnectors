# EBMS
EBMS is an ERP Software enabling small businesses to simplify and automate with one powerful solution. They are able to control inventory, sell product, manage labor, and connect financials.

## Publisher: Eagle Business Software

## Prerequisites
To use this integration, you will need an EBMS account. To learn more, visit our website: [www.EagleBusinessSoftware.com](https://www.eaglebusinesssoftware.com/)

## Obtaining Credentials
For authentication, you need an EBMS API Gateway subscription added to your EBMS account. To activate, contact [your Account Manager](https://www.eaglebusinesssoftware.com/contact-us).

## Connector Documentation
For detailed documentation about the connector please refer to our [online documentation] (https://info.eaglebusinesssoftware.com/standard/search?q=power%20automate%20connector).

## Known Issues and Limitations
- Basic authentication is used and requires an EBMS user license to be configured with read/write security permissions allowed for the entity or properties being modified. (e.g. sales order, purchase order, customer, etc.). For more information, see [EBMS API documentation](https://info.eaglebusinesssoftware.com/EBMS/Modules/API/Default).
- EBMS Data Server behind a firewall (outgoing) or with REST API disabled is not supported.

For issues with EBMS please visit [info.eaglebusinesssoftware.com](https://info.eaglebusinesssoftware.com).

## Supported Operations
### Actions 
1. Create new product
2. Update existing product
3. List product categories

### Triggers
1. When task is created
2. When customer is created
3. When product is created
4. When sales order is created 

## Deployment Instructions (as a custom connector)
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.