# Content Rabbit

Content Rabbit is a headless social-scheduling service. This connector wraps its public REST API so you can schedule posts, publish them to several social networks, and read their analytics from Power Automate, Power Apps, and Logic Apps.

## Publisher: Pooria Arab

## Prerequisites

You need a Content Rabbit account on a paid plan and an API key. Plans start at $12 per month, billed annually. Sign up at [contentrabbitai.com](https://contentrabbitai.com).

## Obtaining Credentials

1. Sign in to Content Rabbit.
2. Go to **Settings > Team > API & Integrations**.
3. Select **Generate API Key**.
4. Copy the key. Content Rabbit shows it once, so store it before you leave the page.
5. Paste the bare key when you create the connection. Do not add a `Bearer` prefix — the connector adds it for you.

## Supported Operations

### List Accounts

Returns the social accounts connected to your team, with the platform and handle for each. Use it to find the platform value that `Create Post` expects.

### List Posts

Returns your scheduled and published posts. Filter by `status` or `platform`, and page through the results with `limit` and `cursor`.

### Create Post

Creates a post. Set `scheduledAt` to schedule it for later, or leave it out to keep the post as a draft. Takes the target platform, the content, and optional media IDs.

### Get Analytics

Returns post-level analytics for the team. Narrow the range with `start` and `end`, and set `timezone` to read the numbers in a local day boundary.

### Create Media Upload URL

Returns a signed, short-lived upload URL. Upload the file to that URL, then pass the returned `id` to `Create Post` as a media ID.

## Known Issues and Limitations

The connector covers scheduling, publishing, and analytics. It does not connect a social account — do that in the Content Rabbit dashboard first, then the account appears in `List Accounts`.

Analytics are as fresh as each network's own reporting. Some platforms delay their figures by several hours.
