# Flixcheck Power Automate Connector

Flixcheck is a web tool that optimizes communication with customers by transforming analog forms into fully digital processes. With this connector, you can react to key lifecycle events of a check (e.g., created, delivered, opened, completed) and use the transmitted data within Microsoft Power Automate to build automated workflows. In addition, the connector enables you to create checks directly from your automated processes.

## Publisher: Flixcheck GmbH

## Prerequisites

- A valid Flixcheck account  
- Access to Microsoft Power Automate  
- API access enabled in your Flixcheck account  
- A valid API key generated in the Flixcheck portal  

## Supported Operations

This connector provides trigger-based operations that allow you to subscribe to different check lifecycle events. When a selected event occurs in Flixcheck, the corresponding trigger starts your Power Automate flow and provides the related check data.

### Check Created (Trigger)

Subscribes to the event that is fired when a new check is created in Flixcheck. The trigger provides the check data payload, which can be used in subsequent actions within your flow.

### Check Delivered (Trigger)

Subscribes to the event that is fired when a check has been successfully delivered to the recipient. The trigger returns the related check data for further processing.

### Check Opened (Trigger)

Subscribes to the event that is fired when a recipient opens a check. The trigger provides the check data payload, enabling you to react immediately (e.g., notify internal teams or update records).

### Check Completely Finished (Trigger)

Subscribes to the event that is fired when a check has been fully completed by the recipient. The trigger returns the complete check data, allowing you to process submitted information, store results, or initiate follow-up actions.

## Obtaining Credentials

The Flixcheck connector uses OAuth2 authentication.
 
To authenticate:
 
1. When creating a new connection in Power Automate, select the Flixcheck connector.
2. Click “Sign in”.
3. You will be redirected to the Flixcheck login page.
4. Log in with your Flixcheck account credentials.
5. Grant the required permissions to Power Automate.
 
After successful authentication, the connection will be established and can be used in your flows.

## Getting Started

1. Import or create the Flixcheck custom connector in Power Automate.
2. Create a new connection using your Flixcheck OAuth2 authentication.
3. Create a new automated cloud flow.
4. Select one of the Flixcheck triggers (e.g., “Check Completely Finished”).
5. Add follow-up actions, such as creating a record in another system, sending a notification, or storing submitted data.
6. Save and test your flow.

Once activated, your flow will automatically run whenever the selected check event occurs in Flixcheck.

## Known Issues and Limitations

- The connector currently supports event-based triggers only.  
- Each trigger subscription is tied to the configured connection.  
- A valid and active Flixcheck account is required at all times.  

## Frequently Asked Questions

### How do I know if my trigger is working correctly?

You can test your flow in Power Automate and perform the corresponding action in Flixcheck (e.g., create or complete a check). If configured correctly, the flow run history will show the received event and payload.

### Can I use multiple triggers in one flow?

Yes. You can create multiple flows using different Flixcheck triggers or combine additional logic within a single flow, depending on your automation requirements.

## Deployment Instructions

To deploy this connector as a custom connector in Power Automate:

1. Open Power Automate and navigate to “Custom connectors”.
2. Select “Import an OpenAPI file” (or the appropriate import option).
3. Upload the connector definition file.
4. Configure the security settings to use API key authentication.
5. Create a new connection using your Flixcheck API key.
6. Test the connector and publish it within your environment.
7. Use the connector in your flows.