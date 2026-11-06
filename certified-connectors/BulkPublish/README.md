# BulkPublish
BulkPublish is a social media scheduling tool. It lets you create, schedule, publish and track posts on 16 social networks from one place, including Facebook, Instagram, X, LinkedIn, TikTok, YouTube, Threads, Bluesky and Pinterest.

## Publisher: BulkPublish

## Prerequisites
You need a BulkPublish account on a plan that includes API access, and at least one connected social channel.

## Supported Operations
### List channels
Lists your connected social media accounts.
### List posts
Lists posts, optionally filtered by status or channel.
### Create a post
Creates a draft or scheduled post on one or more channels.
### Get a post
Gets a post, including its publish status, URL and any error on each platform.
### Update a post
Updates a post's content, schedule, channels or media.
### Delete a post
Deletes a post.
### Publish a post
Publishes an existing post immediately.
### Retry a failed post
Retries the platforms that failed for a post.
### Upload media
Uploads an image or video (up to 100MB) to your media library.

## Obtaining Credentials
Sign in to BulkPublish, open [app.bulkpublish.com/developer](https://app.bulkpublish.com/developer), create an API key, and paste it (`bp_...`) into the connection dialog. The connector adds the `Bearer` prefix.

## Getting Started
1. Create a connection with your API key.
2. Use **List channels** to get channel IDs, or pick them from the dropdown in **Create a post**.
3. To publish right away, call **Create a post** with status `draft`, then **Publish a post** with the returned ID.
4. To react to published or failed posts, use a **Recurrence** trigger followed by **List posts** filtered by status. See Known Issues.

## Known Issues and Limitations
- The connector has no triggers. BulkPublish has no webhooks, and its list endpoint has no state or "changed since" parameter, so it can't support Power Automate polling triggers yet. Use Recurrence plus List posts instead.
- Uploads are limited to 100MB. Larger videos need the multipart endpoints, which this connector doesn't expose.
- The API returns 429 when you send requests too fast. The spec doesn't publish the exact limit.

## Deployment Instructions
`paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --icon icon.png`

`icon.png` is a 230×230 PNG on brand color `#FFDE00`. `iconBrandColor` in `apiProperties.json` is `#FFDE00`.
