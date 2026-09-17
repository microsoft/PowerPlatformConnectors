# Calendly

Calendly is a scheduling automation platform that helps individuals, teams, and organizations schedule meetings without back-and-forth emails. This connector enables Power Automate users to list and manage scheduled events, inspect invitees, check availability, generate single-use booking links, and cancel events — complementing the certified connector's trigger-focused coverage.

## Publisher

### Aaron Mah

## Prerequisites

1. A [Calendly](https://calendly.com) account (free or paid).
2. A **Personal Access Token** (PAT):
   - Log in to [calendly.com](https://calendly.com/login).
   - Navigate to **Integrations** → **API & Webhooks** ([direct link](https://calendly.com/integrations/api_webhooks)).
   - Under "Personal Access Tokens," click **Generate new token**.
   - Name your token (e.g., `Power Automate`) and click **Create Token**.
   - **Copy the token immediately** — it is shown only once.
   - Paste the token into the Power Automate connection dialog.
3. Some features (webhooks, routing forms) require a **Professional+** plan.

## Supported Operations

### Get Current User
Returns the authenticated user's profile including their URI, organization, and scheduling URL. Call this first to obtain the user URI needed by most other operations.

### List Scheduled Events
Lists scheduled events for the authenticated user with optional filtering by status, date range, and invitee email. Use for daily digests or event lookups.

### Get Scheduled Event
Returns the full details of a specific scheduled event by its UUID, including location, conferencing details, calendar event references, and host memberships.

### List Event Invitees
Lists all invitees for a specific scheduled event, including their contact info, custom question responses, cancel/reschedule URLs, and payment details.

### Get Event Invitee
Returns full details of a specific invitee for a scheduled event, including UTM tracking parameters and rescheduling history.

### Cancel Scheduled Event
Cancels a scheduled event with an optional cancellation reason sent to all participants. Useful for auto-canceling meetings when conditions change.

### Create Scheduling Link
Creates a single-use scheduling link for an event type, ideal for personalized outreach. Each link can only be used once.

### List Available Times
Returns available time slots for a specific event type within a date range. Enables smart scheduling flows that check availability before suggesting times.

### List Event Types
Lists all event types (meeting templates) for the authenticated user, including duration, kind, and scheduling URLs.

### Get User Busy Times
Returns time ranges where the user is busy (has conflicting calendar events) within a specified window. Useful for conflict detection.

### List Availability Schedules
Returns the user's configured availability schedules including working hours rules and time-off overrides.

## API Documentation

Visit [Calendly Developer Docs](https://developer.calendly.com/api-docs) for further details.

## Known Issues and Limitations

- Calendly uses **full URIs** as resource identifiers (e.g., `https://api.calendly.com/users/ABCDEF123`). You must pass the complete URI for `user`, `event_type`, and similar parameters — not just a UUID.
- For real-time event notifications, use the certified Calendly connector's webhook triggers (requires Professional+ plan). This IP connector provides action operations only.
- API rate limit: **Unlimited** for most endpoints, but the Scheduling API (not included in this connector) has a 2 req/min limit.
- Personal Access Tokens are shown only once at creation. If lost, generate a new one.
- The `event_type_available_times` endpoint returns slots up to a maximum configurable look-ahead window set in your Calendly event type settings.

## License

Distributed under the MIT License.
