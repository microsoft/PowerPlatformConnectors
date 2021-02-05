## InfoShare Connector Description

* Kendox InfoShare provides a powerful and very extensive REST API. Using this API, you can create and manage documents and processes within InfoShare. Together with the Kendox InfoShare Power Automate Connector you can streamline
  content processes across a widespread application landscape thus avoiding information silos.
* The InfoShare Connector provides you actions for managing documents and processes.

## Prerequisites

* A license to use this connector available via sales365@kendox.com.
* Kendox InfoShare with the WCF service endpoints reachable online.
* Kendox InfoShare login credentials. 

## How to get credentials?

* The connector API Key can be purchased via e-mail request using sales365@kendox.com. The WCF service endpoint will be provided from Kendox (Cloud) or from a Kendox partner, who implemented the respective InfoShare instance.

## How to use the connector?

First you have to create a connection to the connector. For the connection, an API Key is required. You will get it from sales365@kendox.com. The first action is always the Logon or LogonWithHashedPasswort action. These actions return a connection Id, which is required for all further actions. If the connection Id is not be used for more than 10 minutes, the connection Id expires and you have to re-logon with the Logon or LogonWithHashedPasswort action.
