# iManage Work for Admins

iManage is the industry-leading provider of document and email management solutions for knowledge workers. iManage platform organizes and secures the information in documents and emails, so professionals can search for what they need, act on it, and collaborate more effectively. The iManage Work connector for Admins supports automation of iManage Administrator features that are available in Control Center. For example, updating metadata fields, adding users, and so on.

## Publisher: iManage LLC

## Prerequisites

The connector is available for all iManage Work customers connecting to cloudimanage.com. First, the iManage Work Admin for Power Automate application will need to be enabled by an administrator of your iManage Work environment. Once enabled, you will need the specific URL for your environment and a login account and password under which the connector can execute actions. Actions respect the same permissions as applied in Work, so for many of these actions you must have NRT Admin or Tier 1 or Tier 2 support. For more information see the FAQ.

## Supported Operations

### Add folder

Adds a new folder under a workspace, tab or folder.

### Add shortcuts to user's My Matters

Adds workspace shortcuts to a user's 'My Matters'. If a Category ID is specified, shortcuts are placed under that category.

### Add tab

Adds a new tab under a workspace.

### Assign user to library

Assigns an existing user to a library.

### Create alias for custom or property lookup

Creates a custom property alias for custom1 through custom12, custom29, and custom30.

### Create user

Creates user and assigns the user to the preferred library.

### Get lookup aliases

Gets the list of lookup aliases for the specified lookup field ID.

### Update custom field

Updates a custom field description or enabled state for custom1 through custom12, custom29, and custom30.

### Get library roles

Gets the list of user roles for a library.

### Get My Matters categories

Gets categories created in 'My Matters'. Administrators can specify a user ID to get 'My Matters' categories for that user.

## Obtaining Credentials

If you are an existing iManage Work user, provide your iManage Work credentials to login. Otherwise, contact your System Administrator for assistance.

## Known Issues and Limitations

For a list of known issues and limitations, please visit https://docs.imanage.com/power-automate/index.html.

## Frequently Asked Questions

For a list of Frequently Asked Questions, please visit https://docs.imanage.com/power-automate/index.html.

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
