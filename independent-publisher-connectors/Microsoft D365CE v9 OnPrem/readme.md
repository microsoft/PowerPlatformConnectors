Microsoft D365CE v9 OnPrem
Microsoft Dynamics 365 Customer Engagement v9 is a CRM platform created by Microsoft.  This connector allows connection to a non-IFD on-premises environment using the web API through a data gateway.

Publisher
Roy Paar

Prerequisites
This connector uses Windows authentication.  You must first install an on-premises data gateway in order to access your on-premises D365 CE environment via API.  Follow documentation here. https://docs.microsoft.com/en-us/data-integration/gateway/service-gateway-onprem
Once your gateway/cluster has been installed on-premises, it will be visible in the Power Platform under Dataverse > Gateways.
After deploying this connector to your cloud tenant, you must modify the following values in the General tab of the connector page:
-Check the "Connect via on-premises data gateway" box (you will choose the gateway later when you create a connection)
-Replace the "Host" value with your D365 on-premises address using the format in the example (NOTE: D365 CE on-premise does not need to be internet-facing i.e. IFD)
-Replace the "Base URL" value with your D365 on-premises org name (get this value from Customizations > Developer Resources > Service Root URL)
On the Security tab, ensure Windows Authentication is selected.
Update the Connector in the upper right corner to save your changes.
On the Test tab, create a new connection.  In the authentication screen, enter the Windows Authentication credentials required by the on-premises environment and choose your gateway.  Manually input a contact GUID from the D365 CE online environment and test the service.  Navigate to the on-premises environment and validate that the contact has been created with the same GUID.
Perform another test with the same GUID, using different data (update scenario).  Validate the the existing contact on-premises reflects your change.
At this point you have successfully connected to your on-premises D365 CE instance and can now leverage this custom connector in Power Automate / Logic Apps.  In your flow, add a new action and choose the "Custom" tab to find the connector.

Supported Operations
Upsert Contact (Patch)
Calls the D365CE v9 web API to either insert or update a contact in your on-premises environment based on the same action in D365 CE online.
Once connector has been installed in your cloud environment and connection to on-premises environment has been established, use this connector in Power Automate or Logic Apps to perform the on-premises update based on any trigger (i.e. cloud contact phone number is changed).
Currently only the following contact data points are available to update on-premises.
-firstname
-lastname
-middlename
-birthdate
-customertypecode
-emailaddress1
-emailaddress2
-telephone1
-telephone2
-telephone3
-mobilephone
-address1_line1
-address1_line2
-address1_city
-address1_stateorprovince
-address1_postalcode
-address1_county

Upsert Account (Patch)
Calls the D365CE v9 web API to either insert or update an account in your on-premises environment based on the same action in D365 CE online.
Once connector has been installed in your cloud environment and connection to on-premises environment has been established, use this connector in Power Automate or Logic Apps to perform the on-premises update based on any trigger (i.e. cloud account name is changed).
Currently only the following account data points are available to update on-premises.
-name
-address1_line1
-address1_line2
-address1_city
-address1_stateorprovince
-address1_postalcode
-address1_county

Upsert Lead (Patch)
Calls the D365CE v9 web API to either insert or update a lead in your on-premises environment based on the same action in D365 CE online.
Once connector has been installed in your cloud environment and connection to on-premises environment has been established, use this connector in Power Automate or Logic Apps to perform the on-premises update based on any trigger (i.e. cloud lead phone number is changed).
Currently only the following lead data points are available to update on-premises.
-fullname
-emailaddress1
-telephone1

API Documentation
Visit MS Docs site for D365 CE API usage. https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/update-delete-entities-using-web-api#upsert-a-table

Known Issues and Limitations
None.