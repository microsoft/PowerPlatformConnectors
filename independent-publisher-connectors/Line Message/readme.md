# Line Message API Connector

The connector allows you to send various types of messages (text, sticker, image, etc.) to users, groups, or rooms through a LINE Bot. You can also retrieve user profile information.

## Publisher: Felaray Ho

## Prerequisites

To use this connector, you need a [LINE Official Account](https://manager.line.biz/) and a channel on the [LINE Developers Console](https://developers.line.biz/en/).

For a detailed guide, please see [Getting started with the Messaging API](https://developers.line.biz/en/docs/messaging-api/getting-started).

## Getting your credentials

This connector uses a Channel Access Token for authentication.

1.  Log in to the [LINE Developers Console](https://developers.line.biz/console/).
2.  Select the provider and then the channel for your LINE Bot.
3.  Navigate to the **Messaging API** tab.
4.  At the bottom of the page, you will find the **Channel access token** section. Issue a token (a long-lived one is recommended for most use cases).
5.  Copy this token. You will need it when setting up the connection.

**Important:** When pasting the token into the connection setup, paste **only the token itself**. Do not include the "Bearer " prefix, as the connector handles this automatically.

For more details, see the official documentation on [Channel access tokens](https://developers.line.biz/en/docs/messaging-api/channel-access-tokens/).

## Supported Operations

The connector supports the following operations. For all "Send" actions, you can send the message to a User ID, Group ID, or Room ID. You can also send up to 5 message objects in a single action.

*   `Get User Profile`: Retrieves a user's profile information, including their display name, picture URL, and status message.
*   `Send Text Message`: Sends a simple text message. You can include emojis.
*   `Send Sticker Message`: Sends a LINE sticker. You need to provide the `Package ID` and `Sticker ID`.
*   `Send Image Message`: Sends an image. Requires a URL for the full-size image and a preview image.
*   `Send Video Message`: Sends a video. Requires a URL for the video file and a preview image.
*   `Send Audio Message`: Sends an audio file. Requires a URL for the audio file and its duration in milliseconds.
*   `Send Location Message`: Sends a message with a map location, including title, address, and coordinates.

## Usage Example (in Power Automate)

**Scenario:** Send a LINE notification when a new task is created in Microsoft Planner.

1.  **Trigger:** Use the "When a new task is created" trigger from the Planner connector.
2.  **Action:** Add a new step and search for "Line Message".
3.  **Select Action:** Choose the **Send Text Message** action.
4.  **Configure:**
    *   In the `to` field, enter the target User ID, Group ID, or Room ID you want to send the message to.
    *   In the `messages_text` field, compose your message using dynamic content from the Planner trigger, for example: "New task created: [Title]".
5.  Save and run the flow. Now, every new task in Planner will trigger a LINE message.

## Known Issues and Limitations

*   **API Rate Limits:** The LINE Messaging API has rate limits. For example, push messages are limited to 2,000 requests per second. Exceeding these limits will result in an error.
*   **Message Quotas:** Your LINE Official Account plan determines the number of free push messages you can send per month. Additional messages may incur costs. Please check your plan details on the [LINE Official Account Manager](https://manager.line.biz/).