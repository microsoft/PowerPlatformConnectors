# Linkly
Linkly is a link shortener and click tracker. It creates branded short links on your own domain, tracks every click with country, platform and referrer breakdowns, fires retargeting pixels, and records conversions back to the link that produced them. This connector lets flows create and manage links, pull click analytics and report conversions in a Linkly workspace.

## Publisher: Linkly

## Prerequisites
A [Linkly](https://linklyhq.com) account. The free plan is enough to use every action in this connector. Reporting and listing conversions also requires the Linkly conversion tracking feature to be enabled on the workspace.

## Supported Operations
### List workspaces
Returns the workspaces available to this connection. Use it to find the workspace that other actions require.
### Create or update a link
Creates a new short link, or updates an existing one when a link ID is supplied. Supports custom domains and slugs, UTM parameters, retargeting pixels and Open Graph metadata.
### Get a link
Retrieves a single link by its ID, including its destination, short URL and configuration.
### List links
Returns a paginated list of the links in a workspace, with click statistics. Supports search and sorting.
### Delete a link
Moves a link to the trash. Its short URL stops redirecting, and the link can be restored later from the trash.
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

## Obtaining Credentials
This connector uses OAuth 2.0. No API key is needed.

1. If you do not have a Linkly account yet, sign up at [https://linklyhq.com](https://linklyhq.com). The free plan works.
2. When you create a connection, choose **Sign in with Linkly** and sign in with your Linkly account.
3. Choose the workspace this connection should use when prompted, then approve the request.

The connection is bound to the workspace you chose. To work in another workspace, create a second connection.

## Getting Started
1. Create a connection as described above.
2. Add the **Create or update a link** action to a flow, enter the destination URL and, optionally, a custom domain and slug. The action returns the short URL and the link ID.
3. Use **List links**, **Get click analytics** or **Get click counts by dimension** to read click data. Actions that take a **Workspace** let you pick it from a list, which is populated by **List workspaces**.
4. To attribute purchases or sign-ups to links, pass the `linkly_cid` value from your landing page URL to **Report a conversion** as the attribution token.

## Known Issues and Limitations
- A connection is bound to a single Linkly workspace, the one chosen when signing in. Create one connection per workspace.
- **Report a conversion** and **List conversions** require the Linkly conversion tracking feature. Workspaces without it receive an error from these actions.
- Real-time click webhooks are not exposed as triggers. Use Linkly's webhook feature with an HTTP-triggered flow instead.
- Bulk link creation, link export and domain management are available in the Linkly API but are not included in this connector.
- The API is rate limited. **Get click counts by dimension** returns a rate-limit error when the limit is exceeded; retry after a short delay.

## Frequently Asked Questions
### Which workspace do my actions run in?
Actions that take a **Workspace** parameter show a list of the workspaces available to the connection. Actions without one, such as **Get a link**, **Report a conversion** and **List conversions**, use the workspace bound to the connection.

### How do I use a custom domain?
Add the domain in Linkly first, then pass its name in the **Domain** field of **Create or update a link**. **List custom domains** returns the names available in a workspace.

### Does Delete a link remove the link permanently?
No. The link is moved to the trash and stops redirecting. It can be restored from the trash in Linkly.

## Deployment Instructions
1. In the maker portal, open **Custom connectors**, choose **New custom connector** and then **Import an OpenAPI file**. Select `apiDefinition.swagger.json` from this folder.
2. On the **Security** tab, choose **OAuth 2.0** with the **Generic Oauth 2** identity provider. Enter the client ID and client secret issued by Linkly, set the authorization URL to `https://app.linklyhq.com/oauth/authorize`, and set both the token URL and refresh URL to `https://app.linklyhq.com/oauth/token`.
3. Create the connector, then copy the redirect URL shown on the Security tab and register it with Linkly for that client ID.
4. Create a connection by signing in with your Linkly account, then test an action such as **List workspaces**.

You can also deploy with the [paconn](https://learn.microsoft.com/connectors/custom-connectors/paconn-cli) command line tool using the `apiDefinition.swagger.json` and `apiProperties.json` files in this folder, after replacing the client ID placeholder in `apiProperties.json` with the one issued by Linkly.
