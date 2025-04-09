# BitlyIP
Bitly is the most widely trusted link management platform in the world. By using the Bitly API, you will exercise the full power of your links through automated link customization, mobile deep linking, and click analytics.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
A free or paid [Bitly plan](https://bitly.com/pages/pricing).

## Obtaining Credentials
This connector uses an access token. In your profile settings, there is a section called API under Developer Settings. On this page, the first option is to generate an access token. With this generated access token, you will use this as part of the Authorization header for this connector. When you authenticate the connector, you will need to enter it as 'Bearer [Your access token]' - make sure there is a space between bearer and your token.

## Supported Operations
### Get group metrics by referring networks
Returns metrics by referring networks for the specified group's links.
### Get group tags
Returns the tags currently used in the specified group. Maximum 1000.
### Get group clicks
Get number of clicks on Bitlinks in a group.
### Get group metrics by devices
Returns the device types generating click traffic to the specified group's links.
### Get group metrics by countries
Returns the geographic origins of click traffic by country for the specified group.
### Get group
Returns details for a group.
### Update group
Updates the details of a group.
### Get group metrics by cities
Returns the geographic origins of click traffic by city for the specified group.
### Get group shorten counts
Returns all the shorten counts for a group.
### Get groups
Returns a list of groups in the organization.
### Get group preferences
Returns preferences for the specified group.
### Update group preferences
Updates preferences for a group.
### Get organization shorten counts
Returns the shorten counts for a specific organization over a specified time period.
### Get organization
Retrieve details for the specified organization.
### Get plan limits
Returns all plan limits and counts available for an organization.
### Get organizations
Retrieve a list of organizations.
### Get user
Returns information for the current authenticated user.
### Update user
Update fields in the user.
### Get platform limits
Fetch all platform limits and counts available for an organization.
### Get custom Bitlinks for BSDs
Gets custom Bitlinks for an organization filtered by BSD(s).
### Get BSDs
Fetch all Branded Short Domains.
### Get metrics for Bitlink by countries
Returns the country origins of click traffic for the specified link.
### Expand Bitlink
Returns the short link and long URL for the specified link.
### Get clicks summary for Bitlink
Returns the click counts for the specified link rolled up into a single field.
### Get Bitlink
Returns information for the specified link.
### Update Bitlink
Updates fields in the specified link.
### Get metrics for Bitlink by referring domains
Returns the referring domain click counts for the specified link.
### Get Bitlink QR code
Generates a QR code for a Bitlink.
### Get metrics for Bitlink by cities
Returns the city origins of click traffic for the specified link.
### Get metrics for Bitlink by devices
Returns the device types generating click traffic to the specified link.
### Get metrics for Bitlink by referrers by domains
Returns click metrics grouped by referrers for the specified link.
### Get sorted Bitlinks
Returns a list of Bitlinks sorted by group.
### Create full Bitlink
Converts a long URL to a Bitlink and sets additional parameters.
### Get clicks for Bitlink
Returns the click counts for the specified link in an array based on a date.
### Get Bitlinks by group
Returns a paginated collection of Bitlinks for a group.
### Update Bitlinks by group
Bulk update can add or remove tags or archive up to 100 links at a time; The response includes a list of Bitlink IDs that were updated.
### Get metrics for Bitlink by referrers
Returns referrer click counts for the specified link.
### Create Bitlink
Converts a long URL to a Bitlink.
### Get custom Bitlink metrics by destination
Returns click metrics for the specified link by its historical destinations.
### Get clicks for custom Bitlink
Returns the click counts for the specified link. This returns an array with clicks based on a date.
### Get custom Bitlink
Returns the details and history of the specified link.
### Update custom Bitlink
Move a keyword (or custom back-half) to a different Bitlink.
### Add custom Bitlink
Add a keyword (or custom back-half) to a Bitlink with a Custom Domain. This endpoint can also be used for initial redirects to a link.
### Get channels
Returns the channels available to a user.
### Create channel
Creates a new channel.
### Get campaign
Returns details for a campaign.
### Update campaign
Updates a campaign's details.
### Get campaigns
Returns the campaigns for the current authenticated user.
### Create campaign
Creates a new campaign.
### Get channel
Returns a channel's details.
### Update channel
Updates an existing channel.
### Get webhook
Returns a webhook.
### Delete webhook
Deletes a webhook.
### Update webhook
Update a webhook
### Verify webhook
Sends ping event to test webhook configuration.
### Create webhook
Creates a webhook.
### Get webhooks
Fetch all webhooks available for an Organization.
### Bulk add Bitlinks to channels
Bulk add Bitlinks to multiple campaign channels.
### Get OAuth App
Retrieve the details for the provided OAuth App client ID.

## Known Issues and Limitations
There are no known issues at this time.
