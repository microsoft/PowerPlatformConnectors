This is the private preview version of the Snowflake connector based on the [Snowflake SQL REST API](https://docs.snowflake.com/en/developer-guide/sql-api/index.html). It is based on OAUTH with Azure AD, but it should also work with other OAUTH providers when the configuration parameters are adapted manually. The connector provides the rows of the resultset as objects with properties for each column rather and not as array of strings. 

## Supported Operations
The connector supports the following operations:
- Submit SQL Statement for Execution
- Check the Status and Get Results
- Cancel the Execution of a Statement

## Getting Help or Providing Feedback
If you have any issues, requests for functionality, or have general feedback, please fill out this [form](http://aka.ms/snowflakeconnector) and we will get back to you. 

## Pre-requisites for Using the Connector
Set up Azure AD authentication for Snowflake by following these steps:
1. In [Step 1: Configure the OAuth Resource in Azure AD](https://docs.snowflake.com/en/user-guide/oauth-azure.html#step-1-configure-the-oauth-resource-in-azure-ad), follow steps 1-10 and define the scope as `SESSION:ROLE-ANY` by following these [instructions](https://docs.snowflake.com/en/user-guide/oauth-azure.html#using-any-role-with-external-oauth).
2. Navigate to **Authentication** -> **Platform configurations** -> **Add a platform** -> **Add** "https://global.consent.azure-apim.net/redirect" -> Click **Save**
3. In [Step 2: Create an OAuth Client in Azure AD](https://docs.snowflake.com/en/user-guide/oauth-azure.html#step-2-create-an-oauth-client-in-azure-ad), follow steps 1-13.
4. Go to the resource created in Step 1 and go to **Expose an API** -> **Add a client application** -> **Add** your `APPLICATION_CLIENT_ID` from earlier in step 3 above -> Click **Save**
5. Follow [Step 3: Collect Azure AD Information for Snowflake](https://docs.snowflake.com/en/user-guide/oauth-azure.html#step-3-collect-azure-ad-information-for-snowflake) entirely. 
6. Copy and paste the text below into your worksheet, which is where you execute your queries in Snowflake. Before you execute the query, make sure you make the following replacements.  
A. Replace the `AZURE_AD_ISSUER` in line 5 with the value from step 3 that is similar to `https://sts.windows.net/90288a9b-97df-4c6d-b025-95713f21cef9/`. Keep the quotation marks and make sure you have a `/` before the last quotation mark.  
B. Replace the Keys URL in line 6 with the value from Step 3 that is similar to `https://login.microsoftonline.com/90288a9b-97df-4c6d-b025-95713f21cef9/discovery/v2.0/keys`. Keep the quotation marks.  
C. Replace the Audience List URL in line 7 with `Application ID URI` from Step 1. Keep the quotation marks.   
D. If your Snowflake account uses the same email address as your Microsoft Azure account, then replace `login_name` in line 9 with `email_address`. If not, keep it as is and do not type in your login name. Keep the quotation marks.  
E. Make sure you've set your role as `ACCOUNTADMIN`. Now you can execute your query.  

```
create security integration connector
       type = external_oauth
       enabled = true
       external_oauth_type = azure
       external_oauth_issuer = '<AZURE_AD_ISSUER>'     
       external_oauth_jws_keys_url = 'https://login.windows.net/common/discovery/keys'
       external_oauth_audience_list = ('https://analysis.usgovcloudapi.net/powerbi/connector/snowflake')
       external_oauth_token_user_mapping_claim = 'upn'
       external_oauth_snowflake_user_mapping_attribute = 'login_name'
       external_oauth_any_role_mode = 'ENABLE';
```

Steps 1, 3, and 4 are setup configuration required on the Snowflake server. 
Step 2 is required for the connector. The values from this step will be used when setting up the connector.

## Using the Connector
1. Make sure you've followed the pre-requisites.
2. To use this connector, go to Power Automate and click **Data** on the left navigation page. Then, click **Custom connectors**.
3. Wait for the page to load. Then, click **+New custom connector**. 
4. From the dropdown, select **Import from GitHub**. For **Connector Type**, choose **Custom**. For **Branch**, choose **dev**. For **Connector**, choose **Snowflake**.
5. Click **Continue.** You will now be taken the the Custom Connector UI, which will populate the connector files, including the code file, into the UI.
6. In **Step 1: General**, make sure to replace the "YourInstance" in the **Host URL** field with your own instance. You can learn more about this field [here](https://docs.snowflake.com/en/user-guide/client-redirect.html#introduction-to-client-redirect).
7. Go to **Step 2: Security** and choose **OAuth 2.0** from the dropdown. Then, in the OAuth 2.0 section, choose **Azure Active Directory** from the **Identity Provider** dropdown. 
8. Fill in the following fields from the values in Pre-requisities:
-	Client ID: `OAUTH_CLIENT_ID`
-	Client Secret: `OAUTH_CLIENT_SECRET`
-	Resource URI: `Application ID URI`
9. Go to **Step 3: Definition** and review the operations. Then, go to **Step 4: Code** to view the code, which should already be enabled. Now, click "Create Connector."
10. Now, you can go to test your connector in **Step 5: Test** and create a new connection.
11. Begin using your custom connector in your environment to build apps and flows! 

## Sample Flow


