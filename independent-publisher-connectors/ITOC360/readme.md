# ITOC360

ITOC360 is an on-call alert routing and incident management platform for DevOps, SRE, and IT operations teams. It receives alerts from inbound integrations, deduplicates them, applies escalation logic, and notifies the right on-call engineer through their preferred channel. Use this connector to send alert and resolve events to ITOC360 from your flows.

## Publisher: ITOC360

## Prerequisites

You will need an ITOC360 account. You can sign up at [itoc360.com](https://itoc360.com).

## Obtaining Credentials

1. Sign in to your ITOC360 dashboard.
2. Create a new source (for example, a Power Automate source) under **Sources**.
3. Copy the generated source token.
4. When creating a connection in Power Automate, paste the token into the **Source Token** field.

## Supported Operations

### Send event

Sends an alert or resolve event to ITOC360. Set **Event type** to `alert` to trigger a new alert, or `resolve` to resolve an existing one. Events with the same title, host, and source are grouped together, so a `resolve` event closes the matching open alert.

## Known Issues and Limitations

No known issues at this time.
