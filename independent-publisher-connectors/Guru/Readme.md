# Guru

Guru is a knowledge management platform that helps teams capture, organize, and share verified information. This connector enables you to search, create, update, and verify knowledge cards, browse collections and folders, and manage tag categories — all from within your automated workflows.

## Publisher
### Aaron Mah

## Prerequisites

To use this connector, you need:

1. A **Guru account** — sign up at [app.getguru.com](https://app.getguru.com). API access may require a paid plan (All-in-One at $15/user/month). See [Guru Pricing](https://www.getguru.com/pricing) for details.
2. A **User API Token**:
   1. Log into Guru at [app.getguru.com](https://app.getguru.com)
   2. Navigate to **Manage → Apps & Integrations → API Access**
   3. Click **Generate User Token** and give it a name
   4. Copy the token immediately — it cannot be viewed again
3. Your **Guru email address** (the one you use to log in)
4. In Power Automate, enter your Guru email as the **Username** and the API token as the **Password**

Your API token inherits the permissions of your Guru user account. Most connector operations work with Author-level permissions or above.

## Supported Operations

### Search Cards
Search for cards using free-text terms and Guru Query Language filters. This is the primary way to list and find cards — Guru has no dedicated list endpoint.

### Get Card
Retrieve a single card by ID with full content, verification state, and collection info.

### Create Card
Create a new knowledge card in a collection with title, HTML content, and sharing options.

### Update Card
Update an existing card's title, content, or sharing status.

### Verify Card
Mark a card as verified (trusted), confirming its content is accurate and up-to-date.

### Unverify Card
Remove verification from a card, marking it as needing review.

### Archive Card
Archive (soft-delete) a card, removing it from active views. Archived cards can still be found with the Show Archived option in Search Cards.

### List Collections
Get all knowledge collections accessible to the authenticated user. Useful for populating dropdowns when creating cards.

### Get Collection
Get details of a specific knowledge collection by ID.

### Get Folder
Retrieve details of a specific folder by ID. Folders are the organizational unit within collections.

### Get Folder Items
List all cards and subfolders within a specific folder.

### List Tag Categories
Get all tag categories and their tags for the team. Requires a Team ID.

### List Card Versions
Get all historical versions of a card to track changes over time.

## API Documentation

Visit [Guru Developer Docs](https://developer.getguru.com/docs/getting-started) for further details on the API and its capabilities.

## Known Issues and Limitations

- **No card list endpoint** — Guru does not provide a `GET /cards` endpoint. Use the Search Cards operation to list and find cards.
- **Search pagination** — Search results are limited to 50 items per page. Use the `maxResults` parameter to control page size. Cursor-based pagination via `Link` headers is supported by the API but not exposed in this connector.
- **HTML content** — Card content (`content` field) is returned as HTML. Ensure your flow handles HTML formatting appropriately.
- **Rate limits** — Guru API rate limits are not publicly documented. If you encounter 429 errors, add delays between requests.
- **API token permissions** — Your token inherits your Guru user role. Admin operations (analytics, team management) require Admin-level access.
- **Archived cards** — The Archive Card operation performs a soft delete. Cards can be found again using Search Cards with `showArchived=true`.
- **Team ID required** — The List Tag Categories operation requires a Team ID parameter. You can obtain your Team ID from the Guru API by calling `GET /teams`.

## License

Distributed under the MIT License.
