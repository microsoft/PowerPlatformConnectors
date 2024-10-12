
# Snowflake API Reference
This connector is based on the [Snowflake SQL REST API](https://docs.snowflake.com/en/developer-guide/sql-api/index.html). Snowflake enables data storage, processing, and analytic solutions that are faster, easier to use, and more flexible than traditional offerings. The connector uses the Snowflake REST API V2 to submit synchronous and asynchronous queries and retrieve corresponding results.

## Publisher: Snowflake

## Prerequisites

- Users must have Microsoft Entra ID for the external authentication.
- Users must have a premium Power Apps license.
- Users must have Snowflake account.

## Supported Operations

### Submit SQL Statement for Execution

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Submit SQL statement for execution on snowflake.

### Check the Status and Get Results

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Check the status of the execution and get the results, by providing the statementHandle.

### Cancel the Execution of a Statement

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Cancels the execution of a statement, by providing the statementHandle.

### Convert result set rows from array to objects

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Converts result set rows from array of string to JSON object.

## Obtaining Credentials

Set up Azure AD authentication for Snowflake by following these steps:

1. In [Step 1: Configure the OAuth Resource in Azure AD](https://docs.snowflake.com/en/user-guide/oauth-azure.html#configure-the-oauth-resource-in-azure-ad), follow steps 1-10 and define the scope as `SESSION:ROLE-ANY` by following these [instructions](https://docs.snowflake.com/en/user-guide/oauth-azure.html#using-any-role-with-external-oauth).
2. In [Step 2: Create an OAuth Client in Azure AD](https://docs.snowflake.com/en/user-guide/oauth-azure.html#create-an-oauth-client-in-azure-ad), follow steps 1-13.
3. Navigate to **Authentication** -> **Platform configurations** -> **Add a platform** -> **Add** "https://global.consent.azure-apim.net/redirect/snowflakepa" -> Click **Save**. Ensure that the redirect URL is set in the Snowflake OAuth Client and not the Snowflake OAuth Resource.
4. Go to the resource created in Step 1 and go to **Expose an API** -> **Add a client application** -> **Add** your `APPLICATION_CLIENT_ID` from earlier in step 3 above -> Click **Save**
5. Follow [Step 3: Collect Azure AD Information for Snowflake](https://docs.snowflake.com/en/user-guide/oauth-azure.html#collect-azure-ad-information-for-snowflake) entirely. 
6. Copy and paste the text below into your Snowflake worksheet, which is where you execute your queries in Snowflake. Before you execute the query, make sure you make the following replacements so that your query succeeds.

    - In Microsoft Azure, go to your Snowflake OAuth Resource app and click on **Endpoints**. 
    - To get the AZURE_AD_ISSUER in line 5, copy the link in the **Federation metadata document** field and open the link in a new tab. Copy the entityID link which should something look like this: `https://sts.windows.net/90288a9b-97df-4c6d-b025-95713f21cef9/`. Paste it into the query  and make sure you have a `/` before the last quotation mark and that you keep the quotation marks. 
    - To get the Keys URL in line 6, copy the link in the **OpenID Connect metadata document** field and open the link in a new tab. Copy the jwks_uri which should look something like this: `https://login.microsoftonline.com/90288a9b-97df-4c6d-b025-95713f21cef9/discovery/v2.0/keys`. Paste it into the query and make sure you keep the quotation marks.
    - Replace the Audience List URL in line 7 with `Application ID URI` from Step 1. Keep the quotation marks.   
    - If your Snowflake account uses the same email address as your Microsoft Azure account, then replace `login_name` in line 9 with `email_address`. If not, keep it as is and do not type in your login name. Keep the quotation marks.
    - Make sure you've set your role as `ACCOUNTADMIN`. Now you can execute your query. 

``` text
CREATE SECURITY INTEGRATION <integration name>
       type = external_oauth
       enabled = true
       external_oauth_type = azure
       external_oauth_issuer = '<AZURE_AD_ISSUER>'     
       external_oauth_jws_keys_url = '<Keys URL from step 6>'
       external_oauth_audience_list = ('[Application ID URI from registered resource app in Azure]')
       external_oauth_token_user_mapping_claim = 'upn'
       external_oauth_snowflake_user_mapping_attribute = 'login_name'
       external_oauth_any_role_mode = 'ENABLE';
```

## Getting Started

1. Add Snowflake connector in the Power App or Flow.
2. Enter connection credentials as below:\
       - Client Id: Snowflake OAuth Client ID from registered client app in Azure.\
       - Client Secret: Snowflake OAuth Client secret from registered client app in Azure.\
       - Resource URL: Application ID URI from registered resource app in Azure.
3. Click on Sign in button and provider user credentials.

## Known Issues and Limitations

1. If you get a 500 response when creating a new connection, that is a transient error. Please wait a few minutes and try again.
2. If you get a 401 response and your Host field in Step 1 follows this format "orgname-accountname," replace the Host field with your "locator" URL.
3. The connector may time out with large query results. This is due to a general limitation that a custom connector must finish all operations, including fetching the data from snowflake is a total of 5 seconds as documented here.
    >[Microsoft Custom Code FAQ](https://learn.microsoft.com/en-us/connectors/custom-connectors/write-code#custom-code-faq)
    >
    >Q: Are there any limits?<br/>
    >A:Yes. Your script must finish execution within 5 seconds and the size of your script file can’t be more than 1 MB.
4. Snowflake does not currently support variable binding in multi-statement SQL requests. This behavior passes through to the connector.
5. Fields with true Null values are omitted from the result set of a query returned via a custom connector. Fields with quoted null strings ("null") are returned as expected. This is due to the serialization settings of the APIM layer that inherently lives underneath the custom connector as part of the power platform. Refer to the [Snowlake API Reference](#snowflake-api-reference) documentation for more details about the Nullable setting.

## Frequently Asked Questions

1. How can the connector be used within Power Apps?
Currently, Power Apps does not support dynamic schema. You can still use the connector from Power Apps by calling a flow from the app instead of directly from an  app. 

## Deployment Instructions

1. Download the connector source code from GitHub repository.
2. Follow the instructions provided in [Create a New Custom Connector](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli#create-a-new-custom-connector) to deploy the connector in your favourite Power Platform Environment.
3. Once the connector is deployed, follow the instructions in [Update an existing custom connector](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli#update-an-existing-custom-connector) to update the connector with the latest changes.
4. Follow the steps mentioned in [Obtaining Credentials](#obtaining-credentials) section to get credentials.
5. Test the connector with the above obtained credentials.

## Version History

### September 2024

- Fix issue where **Submit SQL Statement for Execution** and **Check the Status and Get Results** actions were not handling fields with an object or array schema type.
- Resolve issue where multi-statement count parameter was not being handled by the **Submit SQL Statement for Execution** action.

### August 2024

#### Fixes

- Resolve issue where null values for numeric columns would result in an error when calling **Submit SQL Statement for Execution** action or the **Convert result set rows from array to objects** action.
- Resolve issue where multi-partition results are not properly converted to JSON objects when calling the **Convert result set rows from array to objects** action.
- Resolve issue where the **Convert result set rows from array to objects** action would not properly convert the result set rows to JSON objects.

#### Breaking Changes

- The **Convert result set rows from array to objects** action is deprecated. This behavior has been integrated into the **Submit SQL Statement for Execution** and **Check the Status and Get Results** actions.
- The **Check the Status and Get Results** action now requires the **DataSchema** input parameter to be provided
