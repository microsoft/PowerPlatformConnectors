# Hermoso

Hermoso is an AI marketing agent. It researches the ads your competitors run, creates image and video ads in your brand's style, schedules and publishes posts to the social channels you own, and manages ad campaigns on the ad accounts you connect.

## Publisher: Hermoso

## Prerequisites

- A Hermoso account. You can create one at https://app.hermoso.ai.
- Some features (research and AI generation) draw on your Hermoso plan balance. Scheduling, publishing, campaign management and analytics are included on every plan.
- Microsoft Copilot Studio with generative orchestration and MCP tools enabled in your environment.
- To publish or manage ads, connect your own social and ad accounts inside Hermoso first. The tools act only on accounts you connected.

## Obtaining credentials

The server uses OAuth 2.0 (authorization code). No API key is entered by hand. When you create the connection you are sent to the Hermoso sign-in page, you sign in and approve access, and Hermoso returns a token scoped to your account. Tokens refresh automatically. You can revoke access at any time from your Hermoso account settings.

## Getting started

Add Hermoso as a tool on your agent, create a connection, and sign in when prompted. Some prompts to try:

- "Find the competitors of allbirds.com and show the longest running ads each one has on Meta."
- "Search TikTok and Reddit for what people say about protein pancakes and list the hooks that repeat."
- "What is in my publishing queue this week?"
- "Which hooks worked across my published posts, channel by channel?"
- "Which accounts are connected to my Hermoso workspace?"

## Supported tools

The authoritative list is whatever the server returns from `tools/list` for your account. The tools fall into these areas.

| Area | What the agent can do |
| --- | --- |
| Research | Find competitors from a website, read ads from the Meta, Google and LinkedIn ad libraries, search organic TikTok, Instagram, YouTube, Reddit and Threads, and summarize the angles that repeat. |
| Create | Plan and render image and video ads in your saved brand style, make thumbnails, and edit or reformat video. |
| Publish | Post or schedule to the channels you connected, read the queue, reschedule or cancel a post. |
| Advertise | Build and manage campaigns on the ad platforms you connect. New campaigns are created paused. |
| Measure | Read post performance and ad reports for connected accounts. |
| Workspace | List and switch brands, check connected channels, credits and jobs. |

Every tool carries annotations. Read-only tools are marked read-only, and tools that change or delete data are marked as not read-only so Copilot Studio can ask for confirmation first. Actions that spend money or publish publicly ask for explicit confirmation.

## Known issues and limitations

- Image and video generation can take longer than a single call. Those tools return a job id, and the agent reads the result with the jobs tools.
- Publishing and campaign tools return an error until the matching account is connected in Hermoso.
- Research and generation require a Hermoso plan balance.

## Support

- Documentation: https://hermoso.ai/docs/mcp/
- Contact: hello@hermoso.ai
- Privacy policy: https://hermoso.ai/privacy
- Terms: https://hermoso.ai/terms
