The Soft1 Connector provides an API to work with Soft1 objects in order to create automated workflows between Soft1 applications and our favourite apps and services. 

## Prerequisites

To use this integration, you will need access to a Soft1 installation with Soft1 Microsoft Power Connector module enabled. To make a connection, select Sign In. You will be prompted to provide your Soft1 Serial Number, Username and Password. After the connection is created you're ready to start using this integration.

## How to get credentials

To obtain credentials a web account must be configured with access to the Soft1 Microsoft Power Connector module -
this account (Username and Password) and your installation serial number will be used for
authentication by the Soft1 connector.

## Getting started with your connector

Soft1 Connector provides four triggers :

    * When a record is created or modified
    * When a record is deleted
    * When a record is modified
    * When a record is created

Each trigger returns a key (id) of the specified of object that was created, modified or deleted, but not its data. In order to fetch the data of this object you have to use the corresponding Get Action filling the field "KEY" with the key returned by the trigger. For example, when a new customer is inserted in Soft1, our trigger "When a record is created" returns the key of the new record and then the Action "Get Customer" returns the customer's data.