# Square
Square helps millions of sellers run their business - from secure credit card processing to point of sale solutions.

## Publisher: Troy Taylor

## Prerequisites
Use of this connector will require permission from the owner of a seller account for production use with OAuth 2. You should also sign up for a [Square Developer account](https://developer.squareup.com/us/en) to gain access to a sandbox environment, also using OAuth 2.

## Obtaining Credentials
In your Square Developer account, you will need to create an application registration to be granted a sandbox OAuth 2 application ID and access token. You will also need to set the OAuth redirect URL to https://global.consent.azure-apim.net/redirect. Once you are ready to use the connector in production, the owner will need to register a production application with the same redirect URL.

## Supported Operations

### Apple Pay
#### Register domain for Apple Pay
Activates a domain for use with Apple Pay on the Web and Square. A validation is performed on this domain by Apple to ensure that it is properly set up as an Apple Pay enabled domain.
### Bank Accounts
#### List bank accounts
Returns a list of BankAccount objects linked to a Square account.
#### Get bank account by V1 ID
Returns details of a BankAccount identified by V1 bank account ID.
#### Get bank account
Returns details of a BankAccount linked to a Square account.
### Bookings
#### Create booking (Beta)
Creates a booking.
#### Search availability (Beta)
Searches for availabilities for booking.
#### Retrieve business booking profile (Beta)
Retrieves a seller's booking profile.
#### List team member booking profiles (Beta)
Lists booking profiles for team members.
### Cards
#### List cards
Retrieves a list of cards owned by the account making the request.
#### Create card
Adds a card on file to an existing merchant.
#### Retrieve card
Retrieves details for a specific Card.
#### Disable card
Disables the card, preventing any further updates or charges. Disabling an already disabled card is allowed but has no effect.
### Cash Drawers
#### List cash drawer shifts
Provides the details for all of the cash drawer shifts for a location in a date range.
#### Retrieve cash drawer shift
Provides the summary details for a single cash drawer shift.
#### List cash drawer shift events
Provides a paginated list of events for a single cash drawer shift.
### Catalog
#### Batch delete catalog objects
Deletes a set of CatalogItems based on the provided list of target IDs and returns a set of successfully deleted IDs in the response.
#### Batch retrieve catalog objects
Returns a set of objects based on the provided ID. Each CatalogItem returned in the set includes all of its child information including: all of its CatalogItemVariation objects, references to its CatalogModifierList objects, and the ids of any CatalogTax objects that apply to it.
#### Batch upsert catalog objects
Creates or updates up to 10,000 target objects based on the provided list of objects. The target objects are grouped into batches and each batch is inserted/updated in an all-or-nothing manner. If an object within a batch is malformed in some way, or violates a database constraint, the entire batch containing that item will be disregarded. However, other batches in the same request may still succeed. Each batch may contain up to 1,000 objects, and batches will be processed in order as long as the total object count for the request (items, variations, modifier lists, discounts, and taxes) is no more than 10,000.
#### Catalog info
Retrieves information about the Square Catalog API, such as batch size limits that can be used by the BatchUpsertCatalogObjects endpoint.
#### List catalog
Returns a list of CatalogObjects that includes all objects of a set of desired types (for example, all CatalogItem and CatalogTax objects) in the catalog. ListCatalog does not return deleted catalog items.
#### Upsert catalog object
Creates or updates the target CatalogObject.
#### Retrieve catalog object
Returns a single CatalogItem as a CatalogObject based on the provided ID. The returned object includes all of the relevant CatalogItem information including: CatalogItemVariation children, references to its CatalogModifierList objects, and the ids of any CatalogTax objects that apply to it.
#### Delete catalog object
Deletes a single CatalogObject based on the provided ID and returns the set of successfully deleted IDs in the response. Deletion is a cascading event such that all children of the targeted object are also deleted. For example, deleting a CatalogItem will also delete all of its CatalogItemVariation children.
#### Search catalog objects
Searches for CatalogObject of any type by matching supported search attribute values, excluding custom attribute values on items or item variations, against one or more of the specified query filters.
#### Update item modifier lists
Updates the CatalogModifierList objects that apply to the targeted CatalogItem without having to perform an upsert on the entire item.
#### Update item taxes
Updates the CatalogTax objects that apply to the targeted CatalogItem without having to perform an upsert on the entire item.
### Checkouts
#### Create checkout
Links a checkoutId to a checkout_page_url that customers are directed to in order to provide their payment information using a payment processing workflow.
### Customers
#### List customer groups
Retrieves the list of customer groups of a business.
#### Create customer group
Creates a new customer group for a business.
#### Retrieve customer group
Retrieves a specific customer group.
#### Delete customer group
Deletes a customer group as identified by the group_id value.
#### Update customer group
Updates a customer group.
#### List customer segments
Retrieves the list of customer segments of a business.
#### Retrieve customer segment
Retrieves a specific customer segment.
#### List customers
Lists customer profiles associated with a Square account. Under normal operating conditions, newly created or updated customer profiles become available for the listing operation in well under 30 seconds. Occasionally, propagation of the new or updated profiles can take closer to one minute or longer, especially during network incidents and outages.
#### Create customer
Creates a new customer for a business.
#### Search customers
Searches the customer profiles associated with a Square account using a supported query filter.  Calling SearchCustomers without any explicit query filter returns all customer profiles ordered alphabetically based on given_name and family_name.
#### Retrieve customer
Returns details for a single customer.
#### Delete customer
Deletes a customer profile from a business. This operation also unlinks any associated cards on file.
#### Update customer
Updates a customer profile. To change an attribute, specify the new value. To remove an attribute, specify the value as an empty string or empty object.
#### Remove group from customer
Removes a group membership from a customer. The customer is identified by the customer_id value and the customer group is identified by the group_id value.
#### Add group to customer
Adds a group membership to a customer. The customer is identified by the customer_id value and the customer group is identified by the group_id value.
### Devices
#### List device codes
Lists all DeviceCodes associated with the merchant.
#### Create device code
Creates a DeviceCode that can be used to login to a Square Terminal device to enter the connected terminal mode.
#### Get device code
Retrieves DeviceCode with the associated ID.
### Disputes
#### List disputes
Returns a list of disputes associated with a particular account.
#### Retrieve dispute
Returns details about a specific dispute.
#### Accept dispute
Accepts the loss on a dispute. Square returns the disputed amount to the cardholder and updates the dispute state to ACCEPTED. Square debits the disputed amount from the seller’s Square account. If the Square account does not have sufficient funds, Square debits the associated bank account.
#### List dispute evidence
Returns a list of evidence associated with a dispute.
#### Create dispute evidence text
Uploads text to use as evidence for a dispute challenge.
#### Retrieve dispute evidence
Returns the evidence metadata specified by the evidence ID in the request URL path. You must maintain a copy of the evidence you upload if you want to reference it later.  You cannot download the evidence after you upload it.
#### Delete dispute evidence
Removes specified evidence from a dispute.
#### Submit evidence
Submits evidence to the cardholder's bank. Before submitting evidence, Square compiles all available evidence. This includes evidence uploaded using the CreateDisputeEvidenceFile and CreateDisputeEvidenceText endpoints and evidence automatically provided by Square, when available.
### Gift Cards
#### List gift card activities (Beta)
Lists gift card activities. By default, you get gift card activities for all gift cards in the seller's account. You can optionally specify query parameters to filter the list. For example, you can get a list of gift card activities for a gift card, for all gift cards in a specific region, or for activities within a time window.
#### Create gift card activity (Beta)
Creates a gift card activity.
#### Retrieve gift card from GAN (Beta)
Retrieves a gift card using the gift card account number (GAN).
#### Retrieve gift card from nonce (Beta)
Retrieves a gift card using a nonce (a secure token) that represents the gift card.
#### Link customer to gift card (Beta)
Links a customer to a gift card
#### Unlink customer from gift card (Beta)
Unlinks a customer from a gift card.
#### Retrieve gift card (Beta)
Retrieves a gift card using its ID.
### Inventory
#### Retrieve inventory adjustment
Returns the InventoryAdjustment object with the provided adjustment_id.
#### Batch change inventory
Applies adjustments and counts to the provided item quantities. On success: returns the current calculated counts for all objects referenced in the request.
#### Batch retrieve inventory changes
Returns historical physical counts and adjustments based on the provided filter criteria.
#### Batch retrieve inventory counts
Returns current counts for the provided CatalogObjects at the requested Locations.
#### Retrieve inventory physical count
Returns the InventoryPhysicalCount object with the provided physical_count_id.
#### Retrieve inventory transfer (Beta)
Returns the InventoryTransfer object with the provided transfer_id.
#### Retrieve inventory count
Retrieves the current calculated stock count for a given CatalogObject at a given set of Locations.
### Invoices
#### List invoices
Returns a list of invoices for a given location.
#### Create invoice
Creates a draft invoice for an order created using the Orders API. A draft invoice remains in your account and no action is taken. You must publish the invoice before Square can process it (send it to the customer's email address or charge the customer’s card on file).
#### Search invoices
Searches for invoices from a location specified in the filter.
#### Get invoice
Retrieves an invoice by invoice ID.
#### Delete invoice
Deletes the specified invoice. When an invoice is deleted, the associated order status changes to CANCELED. You can only delete a draft invoice (you cannot delete a published invoice, including one that is scheduled for processing).
#### Update invoice
Updates an invoice by modifying fields, clearing fields, or both.
#### Cancel invoice
Cancels an invoice. The seller cannot collect payments for the canceled invoice.
#### Publish invoice
Publishes the specified draft invoice. After an invoice is published, Square follows up based on the invoice configuration. For example, Square sends the invoice to the customer's email address, charges the customer's card on file, or does nothing. Square also makes the invoice available on a Square-hosted invoice page.
### Labor
#### List break types
Returns a paginated list of BreakType instances for a business.
#### Create break type
Creates a new BreakType. A BreakType is a template for creating Break objects.
#### Get break type
Returns a single BreakType specified by id.
#### Delete break type
Deletes an existing BreakType. A BreakType can be deleted even if it is referenced from a Shift.
#### Update break type
Updates an existing BreakType.
#### Create shift
Creates a new Shift. A Shift represents a complete workday for a single employee
#### Search shifts
Returns a paginated list of Shift records for a business.
#### Get shift
Returns a single Shift specified by id.
#### Delete shift
Deletes a Shift.
#### Update shift
Updates an existing Shift.
#### List team member wages
Returns a paginated list of TeamMemberWage instances for a business.
#### Get team member wage
Returns a single TeamMemberWage specified by id.
#### List workweek configs
Returns a list of WorkweekConfig instances for a business.
#### Update workweek config
Updates a WorkweekConfig.
### Locations
#### List locations
Provides information of all locations of a business.
#### Create location (Beta)
Creates a location.
#### Retrieve location
Retrieves details of a location. You can specify "main" as the location ID to retrieve details of the main location.
#### Update location (Beta)
Updates a location.
### Loyalty
#### Create loyalty account
Creates a loyalty account.
#### Search loyalty accounts
Searches for loyalty accounts in a loyalty program.
#### Retrieve loyalty account
Retrieves a loyalty account.
#### Accumulate loyalty points
Adds points to a loyalty account.
#### Adjust loyalty points
Adds points to or subtracts points from a buyer's account. Use this endpoint only when you need to manually adjust points.
#### Search loyalty events
Searches for loyalty events. A Square loyalty program maintains a ledger of events that occur during the lifetime of a buyer's loyalty account. Each change in the point balance (for example, points earned, points redeemed, and points expired) is recorded in the ledger. Using this endpoint, you can search the ledger for events.
#### Retrieve loyalty program
Retrieves the loyalty program in a seller's account, specified by the program ID or the keyword main.  Loyalty programs define how buyers can earn points and redeem points for rewards.
#### Calculate loyalty points
Calculates the points a purchase earns. An application might call this endpoint to show the points that a buyer can earn with the specific purchase.
#### Create loyalty reward
Creates a loyalty reward. After a reward is created, the points are locked and not available for the buyer to redeem another reward.
#### Search loyalty rewards
Searches for loyalty rewards in a loyalty account.
#### Retrieve loyalty reward
Retrieves a loyalty reward.
#### Delete loyalty reward
Deletes a loyalty reward. Returns the loyalty points back to the loyalty account. You cannot delete a reward that has reached the terminal state (REDEEMED).
#### Redeem loyalty reward
Redeems a loyalty reward. After the reward reaches the terminal state, it cannot be deleted. In other words, points used for the reward cannot be returned to the account.
### Merchants
#### List merchants
Returns Merchant information for a given access token.
#### Retrieve merchant
Retrieve a Merchant object for the given merchant_id.
### Mobile
#### Create mobile authorization code
Generates code to authorize a mobile application to connect to a Square card reader. Authorization codes are one-time-use and expire 60 minutes after being issued.
### Orders
#### Create order
Creates a new order that can include information about products for purchase and settings to apply to the purchase.
#### Batch retrieve orders
Retrieves a set of orders by their IDs. If a given order ID does not exist, the ID is ignored instead of generating an error.
#### Calculate order (Beta)
Enables applications to preview order pricing without creating an order.
#### Clone order (Beta)
Creates a new order, in the DRAFT state, by duplicating an existing order. The newly created order has only the core fields (such as line items, taxes, and discounts) copied from the original order.
#### Search orders
Search all orders for one or more locations. Orders include all sales, returns, and exchanges regardless of how or when they entered the Square ecosystem (such as Point of Sale, Invoices, and Connect APIs).
#### Retrieve order
Retrieves an Order by ID.
#### Update order (Beta)
Updates an open order by adding, replacing, or deleting fields. Orders with a COMPLETED or CANCELED state cannot be updated.
#### Pay order (Beta)
Pay for an order using one or more approved payments or settle an order with a total of 0. The total of the payment_ids listed in the request must be equal to the order total. Orders with a total amount of 0 can be marked as paid by specifying an empty array of payment_ids in the request.
### Payments
#### List payments
Retrieves a list of payments taken by the account making the request. Results are eventually consistent, and new payments or changes to payments might take several seconds to appear.
#### Create payment
Creates a payment using the provided source. You can use this endpoint to charge a card (credit/debit card or Square gift card) or record a payment that the seller received outside of Square (cash payment from a buyer or a payment that an external entity processed on behalf of the seller).
#### Cancel payment by idempotency key
Cancels (voids) a payment identified by the idempotency key that is specified in the request. Use this method when the status of a CreatePayment request is unknown (for example, after you send a CreatePayment request, a network error occurs and you do not get a response). In this case, you can direct Square to cancel the payment using this endpoint. In the request, you provide the same idempotency key that you provided in your CreatePayment request that you want to cancel. After canceling the payment, you can submit your CreatePayment request again. Note that if no payment with the specified idempotency key is found, no action is taken and the endpoint returns successfully.
#### Get payment
Retrieves details for a specific payment.
#### Update payment
Updates a payment with the APPROVED status. You can update the amount_money and tip_money using this endpoint.
#### Cancel payment
Cancels (voids) a payment. You can use this endpoint to cancel a payment with the APPROVED status.
#### Complete payment
Completes (captures) a payment. By default, payments are set to complete immediately after they are created.
### Refunds
#### List payment refunds
Retrieves a list of refunds for the account making the request. Results are eventually consistent, and new refunds or changes to refunds might take several seconds to appear.
#### Refund payment
Refunds a payment. You can refund the entire payment amount or a portion of it. You can use this endpoint to refund a card payment or record a refund of a cash or external payment.
#### Get payment refund
Retrieves a specific refund using the refund_id.
### Sites
#### List sites
Lists the Square Online sites that belong to a seller.
#### Retrieve snippet
Retrieves your snippet from a Square Online site. A site can contain snippets from multiple snippet applications, but you can retrieve only the snippet that was added by your application.
#### Delete snippet
Removes your snippet from a Square Online site.
#### Upsert snippet
Adds a snippet to a Square Online site or updates the existing snippet on the site. The snippet code is appended to the end of the head element on every page of the site, except checkout pages. A snippet application can add one snippet to a given site.
### Subscriptions
#### Create subscription
Creates a subscription for a customer to a subscription plan. If you provide a card on file in the request, Square charges the card for the subscription. Otherwise, Square bills an invoice to the customer's email address. The subscription starts immediately, unless the request includes the optional start_date. Each individual subscription is associated with a particular location.
#### Search subscriptions
Searches for subscriptions. Results are ordered chronologically by subscription creation date. If the request specifies more than one location ID, the endpoint orders the result by location ID, and then by creation date within each location. If no locations are given in the query, all locations are searched.
#### Retrieve subscription
Retrieves a subscription.
#### Update subscription
Updates a subscription. You can set, modify, and clear the subscription field values.
#### Cancel subscription
Sets the canceled_date field to the end of the active billing period. After this date, the status changes from ACTIVE to CANCELED.
#### List subscription events
Lists all events for a specific subscription.
#### Resume subscription
Resumes a deactivated subscription.
### Team Members
#### Create team member
Creates a single TeamMember object.
#### Bulk create team members
Creates multiple TeamMember objects.
#### Bulk update team members
Updates multiple TeamMember objects.
#### Search team members
Returns a paginated list of TeamMember objects for a business.
#### Retrieve team member
Retrieves a TeamMember object for the given TeamMember id.
#### Update team member
Updates a single TeamMember object.
#### Retrieve wage setting
Retrieves a WageSetting object for a team member specified by TeamMember id.
#### Update wage setting
Creates or updates a WageSetting object.
### Terminals
#### Create terminal checkout
Creates a Terminal checkout request and sends it to the specified device to take a payment for the requested amount.
#### Search terminal checkouts
Retrieves a filtered list of Terminal checkout requests created by the account making the request.
#### Get terminal checkout
Retrieves a Terminal checkout request by checkout_id.
#### Cancel terminal checkout
Cancels a Terminal checkout request if the status of the request permits it.
#### Create terminal refund
Creates a request to refund an Interac payment completed on a Square Terminal.
#### Search terminal refunds
Retrieves a filtered list of Interac Terminal refund requests created by the seller making the request.
#### Get terminal refund
Retrieves an Interac Terminal refund object by ID.
#### Cancel terminal refund
Cancels an Interac Terminal refund request by refund request ID if the status of the request permits it.

## Known Issues and Limitations
There are some actions that have '(Beta)' appended to the action name. These actions may not return all fields while still in beta.
