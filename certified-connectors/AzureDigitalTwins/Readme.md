
### NOTE
> This is a Azure Digital Twins connector. The connector is provided here with the intent to illustrate certain features and functionality around connectors.

## Azure Digital Twins Connector
Azure Digital Twins is an Internet of Things (IoT) platform that enables you to create a digital representation of real-world things, places, business processes, and people. Gain insights that help you drive better products, optimize operations and costs, and create breakthrough customer experiences.



## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* The Power platform CLI tools
* Azure Digital Twin instance

## Building the connector 
Since ADT APIs are secured by Azure Active Directory (AD), we first need to set up a few thing in Azure AD so that our connectors can securely access the Digital Twins.  After that is completed, you can create and test the sample connector.

### Set up an Azure Digital Twins application for your custom connector:
We first need to create and load the Digital twin instance. You can follow the steps here:
* https://azure.microsoft.com/en-us/products/digital-twins/ 
 
The connector offers an power platform interface to the Azure Digital Twin APIs: https://learn.microsoft.com/en-us/rest/api/azure-digitaltwins/. Users can now interact with their twin data through this connector and build applications in the power platforms. 

### Supported Operations

The connector supports the following operations:

* `Twins APIs`: https://learn.microsoft.com/en-us/rest/api/digital-twins/dataplane/twins 
* `Query APIs`: https://learn.microsoft.com/en-us/rest/api/digital-twins/dataplane/query 





