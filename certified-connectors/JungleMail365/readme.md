## JungleMail 365 Connector

JungleMail 365 Connector extends the capabilities of JungleMail and allows to automate certain tasks such as:

1.  Newsletter approval using Power Automate (Flow): https://help-junglemail365.enovapoint.com/article/811-newsletter-approval-with-power-automate
2.  Creating a newsletter archive in SharePoint using Power Automate (Flow): https://help-junglemail365.enovapoint.com/article/814-archiving-newsletters-with-power-automate
3.  Exporting analytics and usage statistics using Power Automate (Flow) or Power Apps

## Pre-requisites

JungleMail 365 app must be added to user's SharePoint tenant. https://help-junglemail365.enovapoint.com/article/628-installing-from-microsoft-appsource
User must be a licensed user in JungleMail for Office 365.

## API documentation

https://docs.microsoft.com/en-us/connectors/junglemail365/

## Supported Operations

Triggers:

When a newsletter is submitted: This operation triggers a flow when a JungleMail 365 newsletter is submitted.
When newsletter processing is completed: This operation triggers a flow when a JungleMail 365 newsletter processing is completed.
When newsletter processing is started: This operation triggers a flow when a JungleMail 365 newsletter processing is started.

Actions:

Create newsletter: Create JungleMail 365 newsletter.
Approve newsletter: Approve and execute JungleMail 365 newsletter.
Reject newsletter: Reject JungleMail 365 newsletter.
Get analytics log: Returns data of the requested analytics data type of each JungleMail newsletter recipient.
Get analytics report: Returns numeric values of newsletter analytics such as email opens, link clicks, read rate, etc.
Get newsletter details: Returns Newsletter data such as creation date, newsletter title, newsletter subject, newsletter content, etc.
Get newsletters: Returns newsletters based on daterange or limited number of newsletters (up to 200)
Get processed emails list: Used with When newsletter processing is completed trigger. Filter the processed newsletters to return either sent, skipped, failed, or completed emails.
Get unsubscribes: Returns the unsubscribed emails.

Full documentation of the operations: https://docs.microsoft.com/en-us/connectors/junglemail365

## How to get credentials

User must be a licensed user in JungleMail for Office 365.
Access is gained through the API key, it can be generated in JungleMail for Office 365 settings. More info here: https://help-junglemail365.enovapoint.com/article/808-power-automate-integration

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps
