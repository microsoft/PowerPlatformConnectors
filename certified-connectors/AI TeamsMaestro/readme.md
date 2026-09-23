# AI TeamsMaestro

AI TeamsMaestro joins your meetings across Microsoft Teams, Google Meet, and Zoom, transcribes them, and writes a structured summary with action items. This connector starts a flow the moment either one is ready, so meeting notes reach the tools your team already works in without anyone copying them across by hand.

## Publisher: Maestro Labs

## Prerequisites

- An AI TeamsMaestro account on a plan that includes outbound meeting events. The connection is made against your own account, and a flow sees the meetings that account records.
- Permission to create flows in your environment. Custom connectors are a premium feature of Power Platform, so each user of a flow built on this connector needs a licence that covers premium connectors.

## Supported Operations

### When a meeting is transcribed

Triggers once for each meeting AI TeamsMaestro finishes transcribing. Carries the meeting details, who attended, and the full transcript — both as one block of text and split into individual utterances. Fires earlier than the summary trigger, and fires even for meetings that are never summarized.

### When a meeting is summarized

Triggers once for each meeting AI TeamsMaestro finishes summarizing. Carries the same meeting details plus the summary text and any action items found, each with the person it was assigned to.

Both triggers report the same **Meeting ID**, so a flow can match one meeting's transcript to its summary. Both are webhook triggers: saving a flow registers a subscription, and deleting or turning the flow off removes it.

The connector also carries three operations that are not maker-visible and exist to support that lifecycle: **Stop sending meeting events** unsubscribes when a flow is removed, **Get the connected account** backs the connection test, and **Get the filter form** is answered at design time by the connector's custom code to render the filter fields described below.

## Obtaining Credentials

Sign in with the Microsoft account you use for AI TeamsMaestro. Creating the connection opens an AI TeamsMaestro sign-in page, and you are asked to approve the connection once. There is no API key to copy and no password to store — access can be withdrawn from AI TeamsMaestro at any time, which stops delivery immediately.

Your account has to already exist. Connecting does not create one: if the Microsoft account you choose has never used AI TeamsMaestro, the sign-in page says so and stops.

## Getting Started

Create an automated cloud flow and pick one of the two triggers. To post every summary into a channel:

1. Add **When a meeting is summarized** as the trigger.
2. Add the action that posts a message.
3. In the message, pick **Title**, **Summary text** and **Ended at** from the dynamic content list.
4. Save the flow.

### Running a flow for only some meetings

Tick **Apply filters to selected meetings** on the trigger and the filter boxes appear:

- *Meeting title contains*, *Meeting title starts with*, *Meeting title is exactly*
- *Meeting host contains*, *Meeting host not contains*
- *Meeting participants contains*, *Meeting participants not contains*

Each box takes several values and matches if any one of them does. Title values are text and ignore capitalisation. Host and participant values are whole email addresses.

**Combine filters with** decides whether a meeting has to match every box you filled in, or just one.

Your own address cannot be used in the participant boxes — you are in every meeting the flow can fire for.

## Known Issues and Limitations

- Meeting events are only sent for meetings AI TeamsMaestro recorded. Meetings held without it produce nothing.
- A meeting that produces too little speech is transcribed but never summarized, so a flow that must run for every meeting should use the transcribed trigger.
- **Segments** is empty when a transcript came from the meeting's own captions rather than from a recording — those carry no per-utterance detail. **Transcript text** always holds the conversation, so read that when segments is empty.
- **Organizer** is empty for a meeting where nobody was recorded as the host, which happens when AI TeamsMaestro joined a link it was not invited to.
- Speaker labels in a transcript are not identities. They need not match any attendee name, and they carry no email address.
- Only some field and comparison pairings are allowed. A title can use *contains*, *starts with* or *is exactly*; a host or participant can use *contains* or *not contains*. The filter boxes only offer the valid pairs.
- There is no way to preview a trigger's output in the designer before the trigger has run.

## Frequently Asked Questions

### Will an old meeting trigger my flow?

No. Only meetings that finish after the flow is turned on are delivered.

### Can two flows listen to the same event?

Yes. Each flow holds its own subscription and receives its own copy.

### What happens when I delete or turn off a flow?

The subscription behind it is removed and delivery stops. Turning the flow back on creates a new one.

### Why is the summary formatted oddly?

Summary text is Markdown. Use an action that renders Markdown, or convert it, if the destination expects plain text or HTML.

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.

Supply your own OAuth client id and client secret when creating the connector. The `clientId` in `apiProperties.json` is a placeholder, `(REPLACE_WITH_CLIENT_ID)`, and no client secret is included in this repository.
