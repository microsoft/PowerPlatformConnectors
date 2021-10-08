This is an experimental connector for Snowflake based on the new Snowflake REST API. It is based on OAUTH with Azure AD but it should also work with other OAUTH providers when the configuration parameters are adapted manually. The connector provides the rows of the resultset as objects with properties for each column rather and not as array of strings. This more convenient to use in power automate.

Prerequisite for the installation of the connector is the proper configuration of the Snowflake for OAUTH with an external Azure AD service provider. This configuration process should have produced the following parameters:
-	Client id
-	Secret
-	Resource URL

Installation:
1.	Open apiDefinition.swagger.json, replace @@xxx.snowflakecomputing.com@@, @@https://xxx.snowflakecomputing.com/oauth/token-request@@ with your own values
2.	New Custom Connector => Import an Open API File, select “apiDefinition.swagger.json”
3.	Select "icon.png" with the snowflake logo as a picture if you like on tab 1
4.	Enter client id, secret, resource url on tab 2
5.	Paste the content of file “snowflake_connector_code” into the textbox in step 4 (Code) and make that the checkboxes for “ExecuteSqlStatement” and “GetResults” are checked.

