# Holopin (Independent Publisher)

Holopins are digital badges, but actually cool. Earn verifiable digital badges for skills, achievements, and all the amazing things you do. Build collections and create your own badge board.

## Publisher: Troy Taylor

## Prerequisites

You must sign up for an account with [Holopin](https://www.holopin.io/).

## Obtaining Credentials

Once logged in to your account, visit your [Integrations page](https://www.holopin.io/@organization/api-keys) and create a new API key.

## Supported Operations

### Badge Operations

#### Issue a badge
Issue a regular (static) badge and retrieve the coupon identifier for the badge. Optionally send an email to the recipient with a claim link.

#### Issue badge with custom image (Beta)
Issue a badge with a custom image per recipient. This is an experimental beta feature that allows you to provide a URL or base64-encoded PNG image.

#### Get badge recipients
Retrieve the profiles of users who claimed a specific badge. Results are paginated with 72 users per page. This is useful for analytics and tracking badge distribution.

### Holobyte Operations

Holobytes are progress points or standalone rewards that can contribute to evolving badges. They come in different icons (lemon, cherry, coffee, starfruit, avocado) and can include rich metadata.

#### Issue a holobyte
Issue a unique holobyte (progress point) for an evolving badge or as a standalone reward. You can specify the recipient by Holopin username, GitHub ID, or Discord ID.

#### Issue a multi-claim holobyte
Issue a holobyte via multi-claim URL for marketing campaigns, Easter eggs, and special occasions. Anyone with the URL can claim the holobyte.

### User Operations

#### Get user's badges
Retrieve a list of all the badges issued to a user by their username. Returns detailed information including badge images, organization details, and descriptions.

#### Get user's board
Retrieve the full board image for a user by their username. This returns the visual representation of all their badges arranged on their board.

### Coupon Operations

#### Get coupon status (Beta)
Check whether a badge coupon has been claimed by a recipient. This is an experimental beta feature that returns claim status, timestamps, and recipient information.

## Known Issues and Limitations

- The custom image endpoint (`/beta/coupon`) is currently in beta and may have limitations or changes in the future.
- The coupon status endpoint (`/beta/coupon`) is also in beta.
- Badge recipient results are limited to 72 users per page - use pagination to retrieve all recipients.
- Holobyte icons are limited to the five available options: lemon, cherry, coffee, starfruit, and avocado.
