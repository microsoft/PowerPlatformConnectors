# Shopify Admin Content
Manage a shopify store's content including articles, blogs, pages and comments using the Shopify Admin API . [Shopify Online Store API](https://shopify.dev/docs/admin-api/rest/reference/online-store)


## Pre-requisites
To use this connector, you need the following
1. A Microsoft Power Apps or Power Automate plan with custom connector feature
2. A Shopify store URL (e.g. storename.myshopify.com )
3. Shopify client credentials - client id and client secret 

###### How to get Client Credenitals
* [Sign Up](https://partners.shopify.com/signup) for Shopify Partner Account.
* [Log In](https://partners.shopify.com/organizations) with your account details and generate credentials by creating an app using: [Generate credentials from your partner dashboard](https://shopify.dev/tutorials/authenticate-a-public-app-with-oauth#generate-credentials-from-your-partner-dashboard) 

## API documentation
[Shopify Rest Admin API](https://shopify.dev/docs/admin-api/rest/reference/online-store)

## Supported Operations
The connector supports the following operations:
* [Article](https://shopify.dev/docs/admin-api/rest/reference/online-store/article)
	- Retrieve a list of all articles from a blog
	- Receive a single article
	- Create an article for a blog
	- Update an article
	- Delete an article
* [Blog](https://shopify.dev/docs/admin-api/rest/reference/online-store/blog) 
	- Retrieve a list of all blogs
	- Retrieve a single blog
	- Create a new blog
	- Modify an existing blog
	- Remove an existing blog
* [Comment](https://shopify.dev/docs/admin-api/rest/reference/online-store/comment)
	- Retrieves a list of comments
	- Retrieves a single comment by its ID
	- Creates a comment for an article
	- Updates a comment of an article
	- Marks a comment as spam
	- Removes a comment
* [Page](https://shopify.dev/docs/admin-api/rest/reference/online-store/page) 
	- Create a new page
	- Updates a page
	- Get a list of all pages
	- Get a single page
	- Count all pages for a shop
    - Delete a page
       


