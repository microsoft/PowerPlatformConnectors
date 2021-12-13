This is the private preview version of the Snowflake connector based on the [Snowflake SQL REST API](https://docs.snowflake.com/en/developer-guide/sql-api/index.html). It is based on OAUTH with Azure AD, but it should also work with other OAUTH providers when the configuration parameters are adapted manually. The connector provides the rows of the resultset as objects with properties for each column rather than an array of strings. 

## Supported Operations
The connector supports the following operations:
- Submit SQL Statement for Execution
- Check the Status and Get Results
- Cancel the Execution of a Statement

## Getting Help or Providing Feedback
If you have any issues, requests for functionality, or have general feedback, please fill out this [form](http://aka.ms/snowflakeconnectorfeedback) and we will get back to you. 

## Pre-requisites for Using the Connector
Set up Azure AD authentication for Snowflake by following these steps:
1. In [Step 1: Configure the OAuth Resource in Azure AD](https://docs.snowflake.com/en/user-guide/oauth-azure.html#step-1-configure-the-oauth-resource-in-azure-ad), follow steps 1-10 and define the scope as `SESSION:ROLE-ANY` by following these [instructions](https://docs.snowflake.com/en/user-guide/oauth-azure.html#using-any-role-with-external-oauth).
2. In [Step 2: Create an OAuth Client in Azure AD](https://docs.snowflake.com/en/user-guide/oauth-azure.html#step-2-create-an-oauth-client-in-azure-ad), follow steps 1-13.
3. Navigate to **Authentication** -> **Platform configurations** -> **Add a platform** -> **Add** "https://global.consent.azure-apim.net/redirect" -> Click **Save**. Ensure that the redirect URL is set in the Snowflake OAuth Client and not the Snowflake OAuth Resource.
4. Go to the resource created in Step 1 and go to **Expose an API** -> **Add a client application** -> **Add** your `APPLICATION_CLIENT_ID` from earlier in step 3 above -> Click **Save**
5. Follow [Step 3: Collect Azure AD Information for Snowflake](https://docs.snowflake.com/en/user-guide/oauth-azure.html#step-3-collect-azure-ad-information-for-snowflake) entirely. 
6. Copy and paste the text below into your Snowflake worksheet, which is where you execute your queries in Snowflake. Before you execute the query, make sure you make the following replacements so that your query succeeds.
A. In Microsoft Azure, go to your Snowflake OAuth Resource app and click on **Endpoints**. 
B. To get the AZURE_AD_ISSUER in line 5, copy the link in the **Federation metadata document** field and open the link in a new tab. Copy the entityID link which should something look like this: `https://sts.windows.net/90288a9b-97df-4c6d-b025-95713f21cef9/`. Paste it into the query  and make sure you have a `/` before the last quotation mark and that you keep the quotation marks. 
C. To get the Keys URL in line 6, copy the link in the **OpenID Connect metadata document** field and open the link in a new tab. Copy the jwks_uri which should look something like this: `https://login.microsoftonline.com/90288a9b-97df-4c6d-b025-95713f21cef9/discovery/v2.0/keys`. Paste it into the query and make sure you keep the quotation marks.  
D. Replace the Audience List URL in line 7 with `Application ID URI` from Step 1. Keep the quotation marks.   
E. If your Snowflake account uses the same email address as your Microsoft Azure account, then replace `login_name` in line 9 with `email_address`. If not, keep it as is and do not type in your login name. Keep the quotation marks.  
F. Make sure you've set your role as `ACCOUNTADMIN`. Now you can execute your query.  

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

## Using the Connector
1. Make sure you've followed the pre-requisites.
2. To use this connector, go to Power Automate and click **Data** on the left navigation page. Then, click **Custom connectors**.
3. Wait for the page to load. Then, click **+New custom connector**. 
4. From the dropdown, select **Import from GitHub**. For **Connector Type**, choose **Custom**. For **Branch**, choose **dev**. For **Connector**, choose **Snowflake**.
5. Click **Continue.** You will now be taken the the Custom Connector UI, which will populate the connector files, including the code file, into the UI.
6. In **Step 1: General**, make sure to replace the "YourInstance" in the **Host URL** field with your own instance. You can learn more about this field [here](https://docs.snowflake.com/en/user-guide/client-redirect.html#introduction-to-client-redirect).
7. Go to **Step 2: Security** and choose **OAuth 2.0** from the dropdown. Then, in the OAuth 2.0 section, choose **Azure Active Directory** from the **Identity Provider** dropdown. 
8. Fill in the following fields from the values in Pre-requisities:
-	**Client ID**: `OAUTH_CLIENT_ID`
-	**Client Secret**: `OAUTH_CLIENT_SECRET`
-	**Resource URI**: `Application ID URI`
The **Redirect UR**L will be blank for now.
10. Go to **Step 3: Definition** and review the operations. Then, go to **Step 4: Code** to view the code, which should already be enabled. Now, click "Create Connector."
11. Go back to **Step 2: Security** and copy the value in **Redirect URL**. In the Microsoft Azure portal, go to your app registrations and select the Snowflake OAuth Client app. On the left sidebar, click on **Authentication** > **+Add a platform** > Paste your value into the **Redirect URIs field** >> Click **Configure**.
12. Now, you can go to test your connector in **Step 5: Test** and create a new connection.
13. Begin using your custom connector in your environment to build apps and flows! 

## Known Issues
1. If you get a 500 response when creating a new connection, that is a transient error. Please wait a few minutes and try again.
2. If you get a 401 response and your **Host** field in Step 1 follows this format "orgname-accountname," replace the **Host** field with your "locator" URL. 
