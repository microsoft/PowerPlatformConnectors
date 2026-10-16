# Wavelength
Wavelength turns call transcripts, meeting notes, and process descriptions into editable process flow diagrams. Use this connector to create flows from text, find and read flows, export and summarize flows, post flows to chat channels, and start a workflow when a flow changes.

## Publisher: MVR Capital Holdings LLC

## Prerequisites
You must have a Wavelength account. A free account is available at https://wavelengthflow.com. Plan limits apply. The Free plan saves up to 5 flows.

## Supported Operations
### List flows
Lists the saved flows of the signed-in user, with the most recent change first.
### Create a flow from text
Creates and saves a process flow from a transcript, call notes, or a description of a process.
### Search flows
Searches the saved flows of the signed-in user by title, step labels, and transcript text.
### Get a flow
Gets the steps, links, Mermaid diagram, and open link of one saved flow.
### Export a flow
Exports one saved flow as Mermaid, SVG, draw.io XML, or JSON text.
### Summarize a flow
Writes a recap email, a statement of work outline, or a list of systems in scope from one saved flow.
### Send a flow to a channel
Posts a flow summary and link to a Slack, Microsoft Teams, Google Chat, or Discord channel, or to a webhook that the user set up in Wavelength.
### When a flow is created or updated
Starts when a flow is created or changed. The trigger checks for changes at a fixed interval.

## Obtaining Credentials
The connector uses OAuth 2.0. When you create a connection, sign in with your Wavelength account and approve access. No API key is necessary.

## Getting Started
1. Create a free account at https://wavelengthflow.com.
2. In Power Automate, add a Wavelength action, for example "Create a flow from text".
3. Select "Sign in with your Wavelength account" and approve the access.
4. Put call notes or a process description in the Text field. The action returns the new flow and its open link.
5. Add "Summarize a flow" to write a recap email from the new flow.

## Known Issues and Limitations
1. Plan limits on the number of saved flows apply to "Create a flow from text".
2. "Send a flow to a channel" needs a channel that the user set up in Wavelength Settings first.
3. The trigger "When a flow is created or updated" checks for changes at a fixed interval, so it does not start at once.

## Frequently Asked Questions
### Does Wavelength use my data to train AI models?
No. See https://wavelengthflow.com/ai-and-data.html.
### Where can I get support?
Send an email to support@wavelengthflow.com.

## Deployment Instructions
Use the Power Platform CLI (paconn) to deploy this connector as a custom connector: `paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --script script.csx`. Then set the OAuth client ID and secret on the Security tab.
