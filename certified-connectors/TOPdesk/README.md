
TOPdesk Connector 1.0
TOPdesk is a market leading ITSM tool, used by thousands of companies world wide. TOPdesk delivers a very extensive API, which entails endpoints to almost all data in TOPdesk.
Due to a large amount of functionalities and endpoints, the TOPdesk Connector 1.0, is scaled down to a bare minimum. This means the connector consists at its current stage, of Supporting files and Incident management, two of the core modules in TOPdesk.
Supporting-files gives you the possibility of create, edit and delete persons, operators, branches and locations inside your TOPdesk, and the incident management API, makes you able to automate and integrate the with first-line and second-line incidents.

Prerequisites
You will need the following to proceed:

A TOPdesk Subscription
Operator Login for TOPdesk, with permissions to access the Supporting files and Incident management API
A base 64 encoded Authorization key, generated from the Operator Login.
A Power platform subscription


Set up a TOPdesk Connector

Create an operator account in TOPdesk, with permissions to access all data in Supporting Files, and in Incident Management. 

Make the account able to login, and give it a login name and password.

Login to the account in TOPdesk, and press the account settings in the upper right corner.

In the area called Application Passwords, press the Add button, and give the application password an Application Name. This name is only used, in order to identify which application password, that is used for what.

Choose an expire data of the application password.

Click then create, and save the generated password temporary, since the application password only is visible in TOPdesk once.

To add TOPdesk as connection, create a new connection, choose TOPdesk and add. your entire host name "nameOfCompany.TOPdesk.com".

afterwards, you write the LoginName and then add the generated application password from TOPdesk.

You will now be able to draw, update and post information into your TOPdesk environment.


The connector supports the following operations:

The entire supporting files API: https://developers.topdesk.com/explorer/?page=supporting-files
The entire incident management API: https://developers.topdesk.com/explorer/?page=incident