# Linkly
Linkly is a link shortener and click tracker. Create branded short links on your own domain, add UTM parameters and retargeting pixels, report conversions, and pull click analytics grouped by country, platform, referrer or campaign.

## Publisher: Chris Muktar

## Prerequisites
A [Linkly](https://linklyhq.com) account. The free plan is enough to try the connector.

## Obtaining Credentials
This connector uses an API key.

1. Sign in to [Linkly](https://app.linklyhq.com).
2. Open **Settings** and then **API**.
3. Copy your API key.

When you create a connection, enter the key in the form `Bearer YOUR_API_KEY` (the word Bearer, a space, then the key).

Most actions also need a **Workspace ID**. Run the **List workspaces** action once to find it, or copy it from the same API settings page.

## Supported Operations
### List workspaces
Returns every workspace the API key can access. Use it to find the workspace ID that other actions require.
### Create or update a link
Creates a new short link, or updates an existing one when a link ID is supplied. Supports custom domains and slugs, UTM parameters, retargeting pixels, expiry rules and Open Graph metadata.
### Get a link
Retrieves a single link by its ID, including its configuration and click statistics.
### List links
Returns a paginated list of the links in a workspace, with click statistics. Supports search and sorting.
### Delete a link
Permanently deletes a single link from a workspace.
### Get click analytics
Returns click counts over time for a workspace or for specific links, optionally filtered by date range, country, browser, platform, referrer or ISP.
### Get click counts by dimension
Returns click counts grouped by a dimension such as country, city, platform, referrer or UTM parameter, ordered by count descending.
### List custom domains
Returns the custom domains configured for a workspace. These domain names can be used when creating links.
### Report a conversion
Records a conversion such as a purchase, sign-up or custom event, and attributes it to the link that was clicked when an attribution token is supplied.
### List conversions
Returns the most recent conversions recorded in the workspace.

## Known Issues and Limitations
- Real-time click webhooks are not exposed as triggers in this version. Use Linkly's webhook feature with an HTTP-triggered flow instead.
- Bulk link creation, link export and domain management are available in the Linkly API but are not included in this connector yet.
- The API is rate limited; the grouped click counts action returns a 429 response when the limit is exceeded.

## Frequently Asked Questions
### Where do I find the Workspace ID?
Run **List workspaces**, or open **Settings > API** in Linkly where the workspace ID is shown next to the API key.

### How do I use a custom domain?
Add the domain in Linkly first, then pass its name in the **Domain** field of **Create or update a link**. **List custom domains** returns the names available in a workspace.

## Deployment Instructions
Use [these instructions](https://learn.microsoft.com/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Power Automate and Power Apps.
