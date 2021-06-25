# Shopify Admin Store
Manage a shopify store's customers, products, collections and customers using the Shopify Admin API . [Shopify Online Store API](https://shopify.dev/docs/admin-api/rest/reference/online-store)


## Pre-requisites
To use this connector, you need the following
1. A Microsoft Power Apps or Power Automate plan with custom connector feature
2. A Shopify store URL (e.g. storename.myshopify.com )
3. Shopify client credentials - client id and client secret 

###### How to get Client Credenitals
* [Sign Up](https://partners.shopify.com/signup) for Shopify Partner Account.
* [Log In](https://partners.shopify.com/organizations) with your account details and generate credentials by creating an app using: [Generate credentials from your partner dashboard](https://shopify.dev/tutorials/authenticate-a-public-app-with-oauth#generate-credentials-from-your-partner-dashboard) 

## API documentation
[Shopify Rest Admin API](https://shopify.dev/docs/admin-api/rest/reference)

## Supported Operations
The connector supports the following operations:

# Actions:
* [Customer](https://shopify.dev/docs/admin-api/rest/reference/customers)
	- Creates a customer
	- Updates a customer
	- Retrieves all orders belonging to a customer
	- Searches for customers that match a supplied query
	- Retrieves list of customers
* [Order](https://shopify.dev/docs/admin-api/rest/reference/orders) 
	- Create an order
	- Update an order
* [Collect](https://shopify.dev/docs/admin-api/rest/reference/products/collect)
	- Adds a product to a custom collection
* [Product](https://shopify.dev/docs/admin-api/rest/reference/products/product) 
	- Creates a new product
	- Get a single product
	- Updates a new product
	- Retrieves a list of products 

# Triggers:
	- When a collection is created
	- When a customer is created
	- When an order is created
	- When a product has been created


