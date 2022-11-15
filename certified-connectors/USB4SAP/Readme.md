# USB4SAP
USB4SAP is one single API for any SAP data source to be consumed as a REST or RFC enabled API

## Publisher: Ecoservity

## Prerequisites
User must have backend agent installed on the SAP system as described in the Deployment instructions below
User must enter a valid SAP user ID & password.
For REST based access, a NetWeaver Gateway or equivalent REST API provider should be connected to backend SAP system.
Follow this [guide](https://github.com/ecsadm/usb4sap/blob/main/KB%20024%20-%20ODDI%20License%20Installation_v2.1.pdf) for prerequisites.

## Supported Operations
This query supports GET operation from SAP backend to provision data from tables, views, reports, queries, ALV, CDS, or any other data container.


## Obtaining Credentials
This API uses Basic Authentication

## Known Issues and Limitations
REST API supports data limits as specified under REST API protocols
Large datasets extraction may result in timeouts as configured in the REST API systems


## Deployment Instructions

Follow this [link](https://github.com/ecsadm/usb4sap/blob/main/KB%20024%20-%20ODDI%20License%20Installation_v2.1.pdf) for the installation of ODDI for SAP.
