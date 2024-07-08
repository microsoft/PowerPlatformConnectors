# Entersoft Connector
Entersoft Connector unleashes the power of Entersoft Business Suite family products (either on-premise or on cloud) and makes them available in a well defined set of operations, objects and services that can be used in Logic Apps, Power Automate and Power Apps. Furthermore, Entersoft Connector exposes a set of business-level Trigger Events that can be used to initiate automated workflows.

Entersoft Connector relies on the [Entersoft Web API Documentation](https://api.entersoft.gr/help) to deliver an easy way to interact with the Entersoft Business Suite's business objects, datasets, automations, business rules, embedded workflows, operations, reports and services. You may use Entersoft Connector in almost any flow or application you are developing on the Microsoft Power Apps platorm, in Logic Apps and Power Automate exploiting the complete functionality of Entersoft Business Suite ERP, Entersoft CRM, Entersoft Warehouse Management, Entersoft Retail, Entersoft Enterprise Mobile Applications, Entersoft Proof Of Delivery, etc.

As events take place in the back-end of Entersoft Business Suite family products, these can trigger and initiate complex workflows on the Microsoft Power App platform, so you can achieve a high degree of automation, and integrate with any of the other systems and services that are present in the Power Apps connector community.

# Prerequisites
You will need the following to proceed:
- A Microsoft Power Apps or Power Automate paln
- A licensed Entersoft Business Suite family product (with either SAAS or Perpetual license) running either on-premise or on-cloud, version 5.X or higher.
- An active Subscription to [Entersoft Cloud Store](https://www.cloud.entersoft.eu)

# Supported Operations
Entersoft Connector for Microsoft Power Apps, Logic Apps, Microsoft Power Flow exposes a rich set of the operations, services and triggers of the connected Entersoft Business Suite family product. Through these operations, complex Microsoft Power platform workflows and applications of any type can be developed that take advantage of the connected Entersoft ERP, CRM, WMS, Retail, Mobile, MRPII services and objects / entities. These operations fall into the following main categories:

## Entity / Object Retrieve
Entersoft Connector makes available the complete object model of all the *public* entities and objects of the connected Entersoft Business Suite family product under the **security & access rights context** of the *signed in user*. No matter the sub-system to which the entity or the object belongs to (i.e. Financials, Sales CRM, Service CRM, Core, Warehouse Management System Request Orders, Shipments, etc.), all of them can be retrieved by either the **External Identification Code** known as *Code* or by the **Internal Unique Identifier** known as *GID*.

In order to provide a seamless interface ot the *user* of the Entersoft Connector, all of those services that retrieve entities / objects take a **PK** as an input paramter of type *string* that can be either the *Code* or the *GID* value that identifies the entity.

For example, should someone needs to retrieve the Entersoft's CRM ESTMTask object by either the Code or the GID -depending on what it is available in his/her implementation of the workflow context- he/she should use the **ESTaskManagement_ESTMTask** operation of the Entersoft Power Connector. Similarly, if we need to get a Sales Order object, in order to orchestrate an *e-Shop Complaint Resolution* automated workflow on Microsoft Power Flow platform, assuming that the Sales Order Code is in the subject of an incoming email, we should use the **ESFinancials_ESFIDocumentTrade** operation to get the full Entersoft Business Suite Sales Order object.

For the complete list of the Entities / Objects and the corresponding retrieval operations you may consult [Entersoft Web API Documentation](https://api.entersoft.gr/help). These operations are grouped based on the subsystem the entities belong to, i.e. ESFInancials, ESTMTask Management, ES Material Managment, Warehouse Management, Field Service, xVan, Field Sales, etc.

## Create, Update & Delete Entities / Objects
Entersoft Connector exposes a set of operations for **Createing**, **Updating / Modifying** and **Deleting** entities / objects of the Entersoft Business Suite subsystems, assuming that the *signed in user* has the appropriate security and access rights for performing such operations. As for any of the operations available for use under the context of Entersoft Connector and the underlying Entersoft Web API, all are subject to the daomain rules and customizarion & parameterization that is in place at the back-end Entersoft Business Suite family product implemented at the back-end *Business Layer* aka Domain Level. Especially for the **ESEntity_DeleteEntityByID** and **ESEntity_DeleteEntityByType** operations, as an additional extra security assurance, the *signed-in User* **must** belong to the Entersoft Business Security Group **ES_API_ALLOW_DELETE** (*this can be done through the Entersoft Business Suite Security Administration Panel*).

## List of Entities / Objects 
Entersoft Connector provides an easy to use set of operations that are to be used for the retrieval of a set of objects / entities of a given type that meet a given set of filter conditions, with project a given set of attributes and returned in a given order. Since the number of objects / entities can be large, **server-side paging** is fully supported and imposed by Entersoft Connector (page-size cannot be larger than 500). As in the case of any of the operations exposed by Entersoft Connector and the underlying Entersoft Web API, the **security and the access rights** of the *signed in user* is always in place and active. 

So if someone in the context of worklow implementation needs to get the last 5 Sales Orders in the system then he/she should use the **ESEntity_EntitiesByType** operation with parameter *EntityType* equal to *ESFIDocumentTrade*, parameter *OrderByFields* equal to *["-ADRegistrationDate"]* and *PageSize* equal to *5*.

## Entity / Objects Automations
**Entersoft Automations** are among the most important and powerful customization engines of the Entersoft Business Suite family products. There are several types of **Entersoft Automations** namely: Entity, DataSet, Context-less and Business Rules. All of them have been supported and made avaliable to Entersoft Connector and can be used in any Microsoft Power platform Workflow and Applications. The *Entity* type automations can be involved through the operations:
- [EntityAutomationNew](https://api.entersoft.gr/swagger/ui/index#!/ESEntity/ESEntity_EntityAutomationNew)
- [EntityAutomationUpdate](https://api.entersoft.gr/swagger/ui/index#!/ESEntity/ESEntity_EntityAutomationUpdate)
- [EntityAutomationUpdateByCode](https://api.entersoft.gr/swagger/ui/index#!/ESEntity/ESEntity_EntityAutomationUpdateByCode)

The rest of the Entersoft Automation types are documented in the **ESPlatformServices** section of [Entersoft Web API Documentation](https://api.entersoft.gr).

## Platform Services / Operations
Entersoft Connector allows for the execution and retrieval of the *named* reports and datasets that are defined at the back-end Entersoft Business Suite family product that the connector is connected to. The most frequently used operations, exposed by the Entersoft Connector, for this task are: **[ESRPC_GetPQData](https://api.entersoft.gr/swagger/ui/index#!/ESPlatformServices/ESRPC_GetPQData)** and **[ESRPC_GetPQData2](https://api.entersoft.gr/swagger/ui/index#!/ESPlatformServices/ESRPC_GetPQData2)**. Since these operations can return a large amount of data, **Server-Side Paging** is supported and imposed by Entersoft Connector (maximum PageSize is 500 entries). 

In order to better address the needs for a powerful integration to Entersoft Business Suite ERP's subsystem, with respect to *finance type of documents* Entersoft Connector exposes the operation [Import/Update Document](https://api.entersoft.gr/swagger/ui/index#!/ESPlatformServices/ESRPC_FIImportDocument). 

Entersoft Business Suite has an extended and powerful logging system that relies on Microsoft SQL Server RDBMS and its being widely-used to store and document events, errors, informational messages and warnings that relate to anything that has to do with Entersoft Business Suite run-time. Logging into this subsystem, has been added as 1st class citizen operation - **[Log](https://api.entersoft.gr/swagger/ui/index#!/ESPlatformServices/ESRPC_Log)** to Entersoft Connector at the application level so that the integrators can log events, messages, warnings, informations that generated in the context of the workflows and apps that Entersoft Connector is being used.

There are several other operations available under this category that are documented in [Entersoft Web API Documentation](https://api.entersoft.gr). 

## Collaboration Services / Operations
Entersoft Connector exposes the *collaboration* services that are bult-in into the connected Entersoft Business Suite family product for messaging and collaboration. Operations supported are:
- BoadcastMessage a message to the logged on User(s)
- Send an Email through the Entersoft's Business Suite mailing system
- Send an SMS through the configured at the back-end SMS provider
- Send a Viber message (either promotional or textual) through the back-end configured Viber gateway provider

## Triggers / Web Hooks
Entersoft Connector provides Entersoft Business Suite events as the entry point for defining **Triggers** on the Microsoft Power Apps & Flows platform. Through these events and triggers, Entersoft Business Suite becomes an *Initiator*, a key-player in initiating workflows that can involve any action of any of the available connectors on the Microsoft Power Platform,  breaking the barriers of Entersoft Business Suite eco-system.

The most important and frequently used *Events* (and therefore the corresponding *Triggers*) are:
### Entersoft Business Events 
These are high-level opinitated events that span across all Entersoft Business Suite subsystems. The events exposed are listed below:
- HighValueSalesOrder
- HighValuePurchaseOrder
- OrderReadyToShip
- SaleOrderOnHold
- OrderSaleCanceled
- PartiallyCanceledOrder
- SaleOrderDelete
- HighValueCustomerCreditNote
- NewDocumentOfType
- NewCRMTaskOfType
- CRMTaskExpired
- HighValuePayment
- HighValuePaymentWithCheck
- HighValuePaymentTaxes
- HighValueCollection
- HighValueCollectionWithCheque
- StockFallingBelowThreshold
- RequestToApprove
- RequestHasBeenApproved

### Entersoft Entity / Object Events
These are events that are fired automatically by Entersoft Business Suite when a CRUD operation takes place on Entity / Object of the Entesoft Business Suite Object model
- Create
- Update
- Delete
- CreateOrUpdate

### Entersoft Proof Of Delivery Events
These are events that are fired automatically by Entersoft Business Suite: Proof Of Delivery SAAS service when a state of a Delivery changes:
- Initial
- Arrived
- Started
- Delivered
- Cancelled
- All

### Entersoft System Events
These are events fired that are automatically by Entersoft Business Suite when an event of type System, Service Status and Health related occurs.
- RestartAppServer
- UpgradeStart
- UpgradeEnd
- UpgradeErr
- CustomVerChanged
- ReCache
- NoAvailableLicense
- AppServerStart
- AppServerStop
- RFRequestAssistance
- UserLockedOut
- DBCheckStart
- DBCheckEnd
- DBCheckSErr
- Other

# API Documentation
An extensive documentation of the supported actions, operations & triggers as well as the models for input and utput parameters can be found on the [Entersoft Web API Documentation](https://api.entersoft.gr/help).

** All of the operations that are listed in the [Entersoft Web API Documentation](https://api.entersoft.gr/help) are available on Entersoft Connector.**