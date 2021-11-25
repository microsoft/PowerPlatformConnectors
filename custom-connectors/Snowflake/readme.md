This is an experimental connector for Snowflake based on the new Snowflake REST API. It is based on OAUTH with Azure AD but it should also work with other OAUTH providers when the configuration parameters are adapted manually. The connector provides the rows of the resultset as objects with properties for each column rather and not as array of strings. This more convenient to use in power automate.

## Supported Operations
The connector supports the following operations:
- Submit SQL Statement for Execution
- Check the Status and Get Results
- Cancel the Execution of a Statement

## Getting Help or Providing Feedback
If you have any issues, requests for functionality, or have general feedback, please fill out this [form](aka.ms/snowflakeconnector) and we will get back to you. 

## Pre-requisites
Set up Azure AD authentication for Snowflake by following the steps [here](https://docs.snowflake.com/en/user-guide/oauth-azure.html). 

Steps 1, 3, and 4 are setup configuration required on the Snowflake server. 
Step 2 is required for the connector. The values from this step will be used when setting up the connector.

## Using the Connector
1. To use this connector, go to Power Automate and click **Data** on the left navigation page. Then, click **Custom connectors**.
2. Wait for the page to load. Then, click **+New custom connector**. 
3. From the dropdown, select **Import from GitHub**. For **Connector Type**, choose **Custom**. For **Branch**, choose **dev**. For **Connector**, choose **Snowflake**.
4. Click **Continue.** You will now be taken the the Custom Connector UI, which will populate the connector files, including the code file, into the UI.
5. In **Step 1: General**, make sure to replace the "YourInstance" in the **Host URL** field with your own instance.
6. Go to **Step 2: Security** and choose **OAuth 2.0** from the dropdown. Then, fill in the following fields from the values in Pre-requisities:
-	Client ID: OAUTH_CLIENT_ID
-	Client Secret: OAUTH_CLIENT_SECRET
-	Resource URI: Application ID URI
7. Go to **Step 3: Definition** and review the operations. Then, go to **Step 4: Code** to view the code, which should already be enabled. Now, click "Create Connector."
8. Now, you can go to test your connector in **Step 5: Test**, if you'd like.
9. Begin using your custom connector in your environment to build apps and flows! 

## Sample Flow


