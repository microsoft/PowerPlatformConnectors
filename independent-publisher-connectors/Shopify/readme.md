# Shopify
This connector allows you to query your Shopify store Orders and Order related information. This information is provided on a store level. To connect to Shopify you will need to setup an application access token from the Shopify admin portal.

## Publisher: Ray Bennett (MSFT)

## Prerequisites
No prerequisites are required for this connector.

## Supported Operations

### RetrieveAllOrders
Retrievees a list of orders that match the specified criteria. You an use the filter conditions to retrieve only the orders that you want.

### UpdateAnOrder
Updates an order by passing the ID of the order and a JSON body representing the elements to update.

### CancelAnOrder
Cancels an order by ID.

### RetrieveListOfCustomers
Retrieves a list of ccustomers that match the specified criteria.

## Obtaining Credentials
Authentication is done by registering an application within your Shopify admin portal. This application will need the scopes to Read and Write orders as well as Read customers.

After your application and scope are created, you will need to generate an access token that will then be used for authentication.

## Known Issues and Limitations
No known issues or limitations.