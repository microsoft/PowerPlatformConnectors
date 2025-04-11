# eBay
eBay facilitates consumer-to-consumer and business-to-consumer sales through its website. This connector allows you to access eBay features and supports actions for
taxonomy, account, metadata, inventory, offers management.  

## Publisher: Artesian Software Technologies LLP

## Prerequisites
You need to have an eBay developer account. You can [get started](https://developer.ebay.com/api-docs/static/gs_ebay-rest-getting-started-landing.html).

## Supported Operations
### Get Default Category Tree ID
A given eBay marketplace might use multiple category trees, but one of those trees is considered to be the default for that marketplace. This call retrieves a reference to the default category tree associated with the specified eBay marketplace ID.

### Get Category Suggestions
This call returns an array of category tree leaf nodes in the specified category tree that are considered by eBay to most closely correspond to the query string q.

### Get Item Aspects For Category
This call returns a list of aspects that are appropriate or necessary for accurately describing items in the specified leaf category. Each aspect identifies an item attribute (for example, color) for which the seller will be required or encouraged to provide a value (or variation values) when offering an item in that category on eBay.

### Get Fulfillment Policies
This method retrieves all the fulfillment policies configured for the marketplace you specify using the marketplace_id query parameter.

### Get Fulfillment Policy
This method retrieves the complete details of a fulfillment policy. Supply the ID of the policy you want to retrieve using the fulfillmentPolicyId path parameter.

### Get Payment Policies
This method retrieves all the payment policies configured for the marketplace you specify using the marketplace_id query parameter.

### Get Payment Policy
This method retrieves the complete details of a payment policy. Supply the ID of the policy you want to retrieve using the paymentPolicyId path parameter.

### Get Return Policies
This method retrieves all the return policies configured for the marketplace you specify using the marketplace_id query parameter.

### Get Return Policy
This method retrieves the complete details of the return policy specified by the returnPolicyId path parameter.

### Create Inventory Location
Use this call to create a new inventory location. In order to create and publish an offer (and create an eBay listing), a seller must have at least one inventory location, as every offer must be associated with a location.

### Get Inventory Locations
This call retrieves all defined details for every inventory location associated with the seller''s account.

### Get Inventory Location
This call retrieves all defined details of the inventory location that is specified by the merchantLocationKey path parameter.

### Get Item Condition Policies
This method returns item condition metadata on one, multiple, or all eBay categories on an eBay marketplace. This metadata consists of the different item conditions (with IDs) that an eBay category supports, and a boolean to indicate if an eBay category requires an item condition.

### Create Or Replace Inventory Item
This call creates a new inventory item record or replaces an existing inventory item record.

### Get Inventory Items
This call retrieves all inventory item records defined for the seller''s account.

### GET Inventory Item
This call retrieves the inventory item record for a given SKU.

### GET Offers
This call retrieves all existing offers for the specified SKU value.

### GET Offer
This call retrieves a specific published or unpublished offer. The unique identifier of the offer (offerId) is passed in the request.

### Create Offer
This call creates an offer for a specific inventory item on a specific eBay marketplace.

### Update Offer
This call updates an existing offer.

### Delete Offer
If used against an unpublished offer, this call will permanently delete that offer. In the case of a published offer (or live eBay listing), a successful call will either end the single-variation listing associated with the offer, or it will remove that product variation from the eBay listing and also automatically remove that product variation from the inventory item group. In the case of a multiple-variation listing, the deleteOffer will not remove the product variation from the listing if that variation has one or more sales. If that product variation has one or more sales, the seller can alternately just set the available quantity of that product variation to 0, so it is not available in the eBay search or View Item page, and then the seller can remove that product variation from the inventory item group at a later time.

### Withdraw Offer
This call is used to end a single-variation listing that is associated with the specified offer. This call is used in place of the deleteOffer call if the seller only wants to end the listing associated with the offer but does not want to delete the offer object. With this call, the offer object remains, but it goes into the unpublished state, and will require a publishOffer call to relist the offer.

### Publish Offer
This call is used to convert an unpublished offer into a published offer, or live eBay listing. The unique identifier of the offer (offerId) is passed in the request.

## Obtaining Credentials
The eBay API exposes functionalities to help developers build client applications. We need to have the appropriate keyset for the production environment.
Follow the [Guided Walkthrough](https://developer.ebay.com/api-docs/static/gs_create-the-ebay-api-keysets.html) to create API keysets. Once keyset is created it appears on the 
Application keys page and on My Account dashboard.

## Getting Started
[Getting Started Reference](https://developer.ebay.com/api-docs/static/gs_ebay-rest-getting-started-landing.html)

## Known Issues and Limitations
No issues and limitations are known at this time.

##Screenshots
![Create Inventory Location](https://astllp-my.sharepoint.com/personal/ajinder_singh_artesian_io/Documents/eBay%20Connector%20Photos/CreateInventoryLocation.png  "Create Inventory Location")

![Create or Replace Inventory Item](https://astllp-my.sharepoint.com/personal/ajinder_singh_artesian_io/Documents/eBay%20Connector%20Photos/CreateOrReplaceInventoryItem.png "Create or Replace Inventory Item")

![Get Fulfillment Policy](https://astllp-my.sharepoint.com/personal/ajinder_singh_artesian_io/Documents/eBay%20Connector%20Photos/GetFulfillmentPolicy.png "Get Fulfillment Policy")

![Connector Test Operation](https://astllp-my.sharepoint.com/personal/ajinder_singh_artesian_io/Documents/eBay%20Connector%20Photos/ConnectorTestOperation.png "Connector Test Operation")