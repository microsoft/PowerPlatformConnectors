# WooCommerce
WooCommerce is a free, open-source commerce platform for WordPress. It empowers anyone, anywhere, to sell anything – with unlimited extensibility, flexibility, and control. Using this connector you can access all built-in resources for WooCommerce and if enabled, Subscription and Coupon resources. You can easily extend the WordPress plugin to support your own resources.

## Publisher: Reenhanced
Reenhanced is committed to providing the best possible experience using WordPress with Power Automate including WooCommerce.

## Prerequisites
In order to use this plugin, you must have the following:

- An active WooCommerce installation
- Install the Power Automate for WooCommerce plugin 
- REST API Credentials created in WooCommerce

## Supported Operations

### Trigger: When a WooCommerce event happens
Select the type of WooCommerce resource, then select the event that will fire your trigger. The triggered resource is returned from this operation.

### Find a list of the selected item type
Pick the resource you want to work with, and then fill in the request to retrieve a filtered list of items.

### Create a new item of the specified type
Pick the resource and then fill in the dynamic properties to create a new item of the selected type

### Get a single item of the selected type
Pick the resource and specify the id to retrieve a specific record

### Update an item of the selected type
Pick the resource and then fill in the dynamic properties to update a new item of the selected type. Leave blank anything you don't want to change.

### Delete an item of the specified type
Delete an item by the id specified.

### Perform a batch operation
Perform a create, update, or delete on up to 100 items at a time.

## Obtaining Credentials
In order to authenticate with your WooCommerce store, you must create a REST API key that will be used to communicate.

## Known Issues and Limitations
We have identified an issue with deleting webhooks. Old webhooks must be removed manually until this issue is resolved.

## Deployment Instructions
Use the copy from github operation to customize this connector to your needs. Please note you will need the WooCommerce plugin installed.
