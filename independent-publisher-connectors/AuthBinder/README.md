AuthBinder
Overview
AuthBinder is an AI agent audit platform. It captures telemetry from your AI agents — tool calls, external connections, actions taken, and approvals — and delivers a structured risk and compliance report.
Use this connector to log events from your Copilot Studio agents or Power Automate flows directly to AuthBinder for analysis. Every tool call, API connection, or automated action your agent performs can be captured, risk-scored, and reviewed by an analyst.
Prerequisites
Before using this connector you will need:
An AuthBinder account. Sign up at authbinder.com
An active audit package (Baseline or Business)
Your AuthBinder API key and Tenant ID, provided after purchase
How to get credentials
Visit authbinder.com and select an audit package
Complete checkout
Your Tenant ID and API key are displayed immediately after payment and included in your confirmation email
Your API key starts with `ab_live_` — keep it secure and do not share it
Supported operations
Log Agent Event
Send a single telemetry event from your AI agent to AuthBinder. Use this action each time your agent calls a tool, accesses an external service, or performs an action you want to audit.
Required inputs:
Tenant ID — your AuthBinder tenant identifier
Agent ID — a consistent identifier for your agent
Tool — the name of the tool or action called (e.g. `email_sender`, `crm_lookup`)
Optional inputs:
Destination — the external system the tool connected to (e.g. `gmail`, `sharepoint`)
Action — the type of action performed (e.g. `read`, `write`, `send`, `delete`)
Human Approved — whether a human explicitly approved this action before execution
Success — whether the tool call completed successfully
User ID — the user who triggered the agent action
Duration — how long the tool call took in milliseconds
Timestamp — when the event occurred (defaults to current time if omitted)
Output:
Event ID — unique identifier for the logged event
Risk Level — AuthBinder's risk assessment: `low`, `medium`, `high`, or `critical`
Timestamp — when the event was recorded
Log Agent Events (Batch)
Send up to 500 telemetry events in a single call. Use this when your agent performs multiple actions in sequence and you want to log them together efficiently.
Get Connection Status
Check whether your agent is successfully connected to AuthBinder and how many events have been received. Use this to verify your integration is working correctly.
Example use case — Copilot Studio agent audit
A business uses a Copilot Studio agent to handle customer service requests. The agent can look up customer records in Dynamics 365, send emails, and update cases.
With this connector added to the agent's Power Automate flow:
Each time the agent calls a tool, a `Log Agent Event` action fires automatically
AuthBinder receives the event, scores its risk level, and stores it
At the end of the audit window, an AuthBinder analyst reviews all activity
The customer receives a structured risk report covering tool usage, unapproved actions, and behavioural anomalies
Supported frameworks
This connector works with any Copilot Studio agent or Power Automate flow. For Python-based agents using LangChain, CrewAI, AutoGen, LlamaIndex, or Haystack, use the AuthBinder Python SDK instead.
Known limitations
Maximum 500 events per batch call
Events must include a valid Tenant ID matching your authenticated API key
API keys are scoped to a single tenant — do not share keys across organisations
Audit window duration depends on your package: 48 hours (Baseline) or 7 days (Business)
Connector configuration
Parameter	Required	Description
API Key	Yes	Your AuthBinder API key starting with `ab_live_`
API documentation
For API documentation and integration guides, contact info@getauthbinder.com
Support
For connector support, contact info@getauthbinder.com
For general account support, visit authbinder.com
Privacy policy
authbinder.com/privacy
Terms of service
authbinder.com/terms
