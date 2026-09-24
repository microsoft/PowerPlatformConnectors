# Miro V2

Miro is the leading digital whiteboarding and visual collaboration platform used by cross-functional teams for design thinking, retrospectives, agile planning, brainstorming, and diagramming. This connector enables Power Automate flows to create and manage Miro boards, add content items (sticky notes, cards, frames, text, connectors), and share boards with collaborators — all using the modern Miro V2 REST API.

## Publisher
### Aaron Mah

## Prerequisites

1. A [Miro account](https://miro.com/signup/) (free tier is sufficient for API access).
2. A **Developer team** in Miro — create one at [https://miro.com/app/dashboard/?createDevTeam=1](https://miro.com/app/dashboard/?createDevTeam=1) if you don't have one.
3. A Miro OAuth app with the following configuration:
   - Go to [Your Apps](https://miro.com/app/settings/user-profile/apps) and click **Create new app**.
   - Check scopes: `boards:read` and `boards:write`.
   - Set Redirect URI to `https://global.consent.azure-apim.net/redirect`.
   - Copy the **Client ID** and **Client Secret** from the App Credentials section.
4. In Power Automate, create a new Miro V2 connection using those credentials and authorize when prompted.

## Supported Operations

### List Boards
Retrieves a list of boards accessible to the current user. Supports filtering by team, searching by name, sorting, and pagination.

### Get Board
Retrieves details for a specific board by its ID, including owner, team, policy, and membership information.

### Create Board
Creates a new Miro board with the specified name and settings. Optionally set a description and target team.

### Copy Board
Creates a copy of an existing board, including all its content. Useful for creating boards from templates.

### Update Board
Updates the name, description, or settings of an existing board.

### Create Sticky Note
Creates a sticky note on a board with the specified text and color. Supports shape selection (square or rectangle), fill color, text alignment, positioning, and placement inside frames.

### Create Card
Creates a card item on a board with title, description, assignee, and due date. Cards are the primary structured-data item on Miro boards.

### Create Frame
Creates a frame on a board to visually group and organize items. Use the returned Frame ID as the parent.id when creating items inside the frame.

### Create Text
Creates a text item on a board with the specified content. Supports HTML formatting, custom colors, font sizes, and text alignment.

### Create Connector
Creates a line connecting two items on a board. Supports captions, stroke color, width, and style (normal, dashed, dotted).

### List Board Items
Retrieves all items on a board, optionally filtered by item type (sticky_note, card, shape, text, image, frame, etc.). Supports cursor-based pagination.

### Get Item
Retrieves details for a specific item on a board by its ID, including type-specific data, position, and timestamps.

### Share Board
Invites one or more users to a board by email with a specified role (viewer, commenter, or editor). Optionally include a custom invitation message.

## API Documentation
Visit [Miro Developer Platform](https://developers.miro.com/reference/api-reference) for further details.

## Known Issues and Limitations

- **Developer team limits**: Developer teams are limited to 3 boards and 5 collaborators, with a watermark on boards.
- **Free plan limits**: Free accounts can have up to 3 editable boards; additional boards become view-only.
- **Rate limits**: Miro uses a credit-based rate limiting system (100,000 credits/minute). Read operations cost 50 credits; create operations cost 500 credits. Rate limits are subject to change.
- **Board ID encoding**: Board IDs may contain special characters (e.g., `=`). Ensure proper URL encoding.
- **Copy board uses PUT**: The Copy Board operation uses an HTTP PUT method (not POST), which is unusual but matches the Miro API specification.
- **No triggers in this version**: This connector provides actions only. Trigger support (polling or webhook-based) is planned for a future version.
- **Existing V1 connector**: An earlier Miro connector using the deprecated V1 API exists. This V2 connector is the modern replacement with broader coverage.

## License
Distributed under the MIT License.
