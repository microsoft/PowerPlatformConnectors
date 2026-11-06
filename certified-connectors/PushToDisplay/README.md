# PushToDisplay Connector

PushToDisplay lets you send real-time display updates to your boards. Every device linked to a board - phones, tablets, TVs, or any screen running the PushToDisplay app - updates instantly when an update is delivered.

This connector exposes a single operation, `Send an update to devices`, which sends a styled message to one of your boards. Content is delivered in real time to every device linked to that board.

## Prerequisites

You will need the following to proceed:

- A PushToDisplay account with at least one board.See [PushToDisplay](https://pushtodisplay.com) for details.
- PushToDisplay app installed on your devices.

## How to get started

1. Create a new connection to the PushToDisplay connector via **PushToDisplay account (OAuth2)** and sign in.
2. Add the **Send an update to devices** action to your flow or app.
3. Enter the **Blocks** message text, and optionally the **Board ID** (or leave it empty to use your default board), panel, alignment, and styling fields.
4. Run it. Every device linked to the board updates instantly.

See the [HTTP API - Send update](https://pushtodisplay.com/docs/http-api/send-update) reference for all request fields and response formats.

## Supported Operations

The connector supports the following operations:

- `Send an update to devices`: Send a styled message to a board. Supports panel-targeted routing (`panelId`), full-panel mode, alignment, content spacing, and per-block text styling (size, weight, color, background). Returns `messageId`, `enqueuedAtUtc`, and `userId`.

### Request fields

| Field                | Required | Description                                                                                                                                             |
| -------------------- | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Board ID             | No       | The board to update. If omitted, your default board is used. Find the Board ID in the PushToDisplay app or web portal.                                  |
| Blocks               | Yes      | One or more content blocks. Each block contains a message with optional text size, font weight, text color, and text background color.                  |
| Target Panel         | No       | Which panel to update (1-4). Only needed for multi-panel layouts such as Side by Side or 2x2 Grid. Defaults to panel 1.                                 |
| Full Panel Mode      | No       | When enabled, the message fills the entire panel area. Intended for bold, full-screen style messages.                                                   |
| Content Spacing      | No       | Spacing between lines of text: compact, standard, or spacious. Defaults to standard.                                                                    |
| Horizontal Alignment | No       | Horizontal alignment of the message text: start, center, or end. Defaults to center.                                                                    |
| Vertical Alignment   | No       | Vertical alignment of the message text: start, center, or end. Defaults to center.                                                                      |
| Background Color     | No       | Background color for the update area in hex format (for example, #0F172A). Covers the message row, or the entire panel when Full Panel Mode is enabled. |

## How to get help

- [PushToDisplay API documentation](https://pushtodisplay.com/docs)
- [HTTP API - Send update](https://pushtodisplay.com/docs/http-api/send-update)
- [PushToDisplay](https://pushtodisplay.com) · [Terms](https://pushtodisplay.com/terms) · [Privacy](https://pushtodisplay.com/privacy)

## Known Issues and Limitations

- **Board selection is by ID.** The connector does not provide a dynamic board picker; you must enter the Board ID as free text. Find the ID in the PushToDisplay app or web portal.
- **Colors are hex strings.** Background and text colors are entered as hex values (for example, #FFFFFF); there is no native color picker.
