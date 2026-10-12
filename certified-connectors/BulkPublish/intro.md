## BulkPublish

BulkPublish is a social media scheduling platform. It publishes posts to Facebook, Instagram, X, LinkedIn, TikTok, YouTube, Threads, Bluesky, Pinterest, Google Business Profile, Mastodon, Reddit, Discord, Telegram, Tumblr and Snapchat.

With this connector you can create, schedule, update, publish, retry and delete posts, upload media, and check publish status in your flows. For example, you can post new SharePoint items to social media or send a Teams alert when a post fails.

### Prerequisites
- A BulkPublish account (https://bulkpublish.com) with API access
- An API key from https://app.bulkpublish.com/developer

### How to get credentials
Sign in to BulkPublish, open https://app.bulkpublish.com/developer, and create an API key. Copy the key (it starts with `bp_`) and paste it when you create the connection.

### Get started with your connector
1. Add a BulkPublish action and create a connection with your API key.
2. Choose **Create a post**, pick your channels, type the content, and set the status to `scheduled` with a time, or to `draft`.
3. Follow it with **Publish a post** if you want it to go live right away.

### Known issues and limitations
There are no triggers yet because BulkPublish has no webhooks. To check status on a schedule, use Recurrence plus **List posts**.

### Common errors and remedies
- **401**: the API key is wrong or was revoked. Create a new one.
- **400 VALIDATION_ERROR**: a character limit or required field failed. Check the error message.
- **409 PUBLISH_IN_PROGRESS**: the post is being published, so it can't be deleted yet.
- **422**: you've hit your plan quota.

### FAQ
**Can I post to several networks at once?** Yes. Pass several channel IDs in one Create a post call.
