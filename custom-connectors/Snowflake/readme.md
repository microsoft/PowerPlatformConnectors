# Snowflake

This connector is based on the [Snowflake SQL REST API](https://docs.snowflake.com/en/developer-guide/sql-api/index.html).

## Publisher: Rene Koch

## Obtaining Credentials
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

## Supported Operations

Submit SQL Statement for Execution

Check the Status and Get Results

Cancel the Execution of a Statement


## Known Issues and Limitations
1. If you get a 500 response when creating a new connection, that is a transient error. Please wait a few minutes and try again.
2. If you get a 401 response and your Host field in Step 1 follows this format "orgname-accountname," replace the Host field with your "locator" URL.
3. The connector may time out with large query results. 

## Frequently Asked Questions
1. How can the connector be used within Power Apps?
Currently, Power Apps does not support dynamic schema. You can still use the connector from Power Apps by calling a flow from the app instead of directly from an  app. 
