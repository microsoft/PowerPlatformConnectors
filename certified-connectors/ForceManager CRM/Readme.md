# ForceManager CRM Connector for Power Automate

The ForceManager CRM Connector for Power Automate is designed to streamline the tasks of field sales representatives, focusing on sales enhancement and customer relationship management. This connector facilitates seamless integration with ForceManager's comprehensive CRM functionalities.

## Publisher: ForceManager CRM

Published by ForceManager, a leader in CRM solutions tailored for sales efficiency and customer relationship management.

## Prerequisites

Users must have an active subscription to ForceManager CRM and Power Automate to utilize this connector.

Our clients from Starter plan or above can start to use the connector straight away, since they have access to the API credentials needed to setup the connector. Any Essential plan customer has to upgrade its plan to the Starter or Professional one; this can be done autonomously via the billing section available in the ForceManager CRM webapp.

Any new customer instead must undergo a registration process which is available on our [website](https://www.forcemanager.com/signup/) or via any of our mobile applications downloadable in the [App Store](https://apps.apple.com/it/app/forcemanager-crm-mobile/id945076174) and [Google Play](https://play.google.com/store/apps/details?id=com.tritiumsoftware.forcemanager&hl=it&gl=US).

## Supported Operations

The connector offers a range of operations including:

-   **ListBranches**: Retrieves all branches.
-   **ListCountries**: Fetches all countries.
-   **ListCurrencies**: Lists all currencies.
-   **ListTimezones**: Provides a list of timezones.
-   **ListContactTypes**: Retrieves contact types.
-   **ListContacts**: Lists all contacts.
-   **CreateContact**: Creates a new contact.
-   **GetContact**: Retrieves a specific contact.
-   **UpdateContact**: Updates a contact.
-   **DeleteContact**: Deletes a contact.
-   **ContactWebhooks**: Sets webhooks for contact updates.
-   **ListOpportunityTypes**: Retrieves opportunity types.
-   **ListOpportunityStatuses**: Lists opportunity statuses.
-   **ListOpportunities**: Lists all opportunities.
-   **CreateOpportunity**: Creates a new opportunity.
-   **GetOpportunity**: Retrieves a specific opportunity.
-   **UpdateOpportunity**: Updates an opportunity.
-   **DeleteOpportunity**: Deletes an opportunity.
-   **OpportunityWebhooks**: Sets webhooks for opportunity updates.
-   **ListActivityTypes**: Lists activity types.
-   **ListUsers**: Lists all users.
-   **ListAccountTypes**: Retrieves account types.
-   **ListAccountStatuses**: Lists account statuses.
-   **ListAccountSegments**: Lists account segments.
-   **ListAccounts**: Lists all accounts.
-   **CreateAccount**: Creates a new account.
-   **GetAccount**: Retrieves a specific account.
-   **UpdateAccount**: Updates an account.
-   **DeleteAccount**: Deletes an account.
-   **AccountWebhooks**: Sets webhooks for account updates.
-   **ListSalesOrderStatuses**: Lists opportunity statuses.
-   **ListSalesOrders**: Lists all sales orders.
-   **CreateSalesOrder**: Creates a new sales order.
-   **GetSalesOrder**: Retrieves a specific sales order.
-   **UpdateSalesOrder**: Updates a sales order.
-   **DeleteSalesOrder**: Deletes a sales order.
-   **SalesOrderWebhooks**: Sets webhooks for sales order updates.
-   **ListSalesOrdersLines**: Lists all sales orders lines.
-   **CreateSalesOrderLine**: Creates a new sales order line.
-   **GetSalesOrderLine**: Retrieves a specific sales order line.
-   **UpdateSalesOrderLine**: Updates a sales order line.
-   **DeleteSalesOrderLine**: Deletes a sales order line.

This comprehensive list covers a wide range of functionalities provided by the connector, ensuring effective integration with the ForceManager CRM system.

## Obtaining Credentials

Users need to authenticate via Public and Private keys obtained from their ForceManager CRM account.

Generating the API keys needed for the authentication process can be done or via the settings page in the [ForceManager CRM webapp](https://app.forcemanager.net) or via the [ForceAdmin](https://admin.forcemanager.net) tool, as per it is reported in our [public documentation](https://developer.forcemanager.com/) page of ForceManager CRM APIs. The API keys generation provides a public and private keys that are necessary along the connector authentication step meant for creating a connection in Power Automate.

## Known Issues and Limitations

List actions of core entities return a default number of 50 records. To change this counter or download any remaining records iteratively the `count` header and `page` query parameter have to be modified.

Power Automate actions are subject to the same rate limits imposed to public APIs usage, that is a maximum number of 4 or 13 requests per seconds depending on the API credentials plan of each customer.

## Deployment Instructions

To deploy this connector:

1. Access your Power Automate environment.
2. Navigate to 'Custom Connectors'.
3. Import the provided connector definition.
4. Enter the required authentication details.
5. Test the connector to ensure functionality.
