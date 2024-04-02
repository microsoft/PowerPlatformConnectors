# Ant Text Automation

Revolutionize your email workflow with Ant Text Automation, the powerful Microsoft Power Automate connector. Craft dynamic and visually stunning email templates effortlessly, using Ant Text's intuitive interface, and utilize them later with Ant Text Automation in your Power Automate workflows.

## Publisher: Insight Office

## Prerequisites
You will need the following to proceed:
* Ant Text Business Subscription
* Email associated with Ant Text Business subscription
* Ant Text Business License Key Number
* Give consent in your Outlook client to Ant Text
* Key phrase to send emails using the connector

## Supported Operations
The connector supports the following operations:
### `GetAntTextTemplates`
Get a full list of Ant Text Email Templates under your Ant Text Business licensed email.

### `PostAntTextEmail`
Send Ant Text Email Template with up to 20 merge fields.

## Obtaining Credentials
After purchasing the Ant Text Business subscription, your email will be associated with it, and you will be provided with a license key number. To use the connector, you will need this email and license key number. In the email, you will also receive a keyphrase to use within the connector.

## Getting Started
In order to use this connector, you must have a paid Ant Text Business subscription.
The email used for subscription and the sender of the email has to be the same.
You cannot use a non-Ant Text Business subscription's email address when sending out Ant Text Email with this connector.

Start by signing up for a 30-day trial of Ant Text through [Microsoft AppSource](https://appsource.microsoft.com/en-us/product/office/WA104381167).

Want a personalized Power Automate version for testing?
Simply send us an email to support@anttext.com, and we will get you started!

## Known Issues and Limitations
At the moment, Ant Text Automation has only one functionality - sending emails. There is a hidden operation to receive the list of your templates. Instructions for using the connector have to be followed as provided; otherwise, you might experience issues. Merge fields have to be used according to instructions as well. You need to make yourself familiar with Ant Text first before using Ant Text Automation.

## Deployment Instructions
There are two ways of deploying Ant Text Automation as a custom connector - manually or through CLI.
* For manual deployment, please follow [these instructions](https://nsightoffice-my.sharepoint.com/:b:/g/personal/support_anttext_com/EYbiEBpr5BVCu6SWbPYbwpwBZhRJxw-JrsalPBx8lSeo6g).
* For CLI deployment, follow [this document](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli).