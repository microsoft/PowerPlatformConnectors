# Gravity Forms Power Automate PRO

Connect Gravity Forms to Power Automate with this professional plugin and connector by Reenhanced. You will be able to trigger Power Automate flows when your forms are submitted and access any previous entries through the actions. New features are in active development in this well supported plugin and connector.

## Publisher: Reenhanced LLC
Reenhanced has built an API on top of Gravity Forms designed to connect Gravity Forms to the Power Platform. This is not useless middleware, but is instead a sensible and secure set of extensions to the Gravity Forms API that allows for direct communication between your WordPress site and Power Automate. This is a certified connector and is fully supported by Reenhanced.

## Prerequisites
A paid license for Gravity Forms is not required, but is highly recommended. The Gravity Forms Power Automate pro plugin requires a valid license for use. Purchase your license at reenhanced.com.

## Supported Operations

### Trigger: When a Gravity Forms form is submitted
Send the complete entry to Power Automate. Your form fields are available as dynamic content for use in any additional action.

### Trigger: When a Gravity Forms entry is updated
Whenever a Gravity Forms entry is updated, send the complete entry to Power Automate. Your form fields are available as dynamic content for use in any additional action. Updates include entry metadata and any changes to the form fields.

### Trigger: When a Gravity Forms entry is deleted
Whenever a Gravity Forms entry is deleted, send the complete entry to Power Automate. Your form fields are available as dynamic content for use in any additional action.

### Trigger: When a Gravity Forms entry status changes
Whenever a Gravity Forms entry status changes, send the complete entry to Power Automate. Your form fields are available as dynamic content for use in any additional action. Additionally, both the old and new status are available as dynamic content.

### Trigger: When a Gravity Forms entry approval status changes (GravityView)
GravityView provides a way to approve or reject entries. Whenever an entry's approval status changes, send the complete entry to Power Automate. Your form fields are available as dynamic content for use in any additional action. Additionally, the new status is available as dynamic content.

### Trigger: When a Gravity Forms entry payment status changes
Whenever a Gravity Forms entry payment status changes, send the complete entry to Power Automate. Your form fields are available as dynamic content for use in any additional action. The payment action is available as dynamic content.

### Action: Get Gravity Forms form entries
Get a list of entries from any form and use them in your flow. Each form field is available as a dynamic content item.

### Action: Get Gravity Forms form entry
Get a single entry from any form and use it in your flow. Each form field is available as a dynamic content item.

### Action: Update Gravity Forms form entry
Update a single entry from any form. Each form field is available as a dynamic content item. Empty fields will not be updated unless they are provided in the request.

### Action: Delete Gravity Forms form entry
Delete a single entry from any form. Each form field is available as a dynamic content item.

### Action: Create Gravity Forms form entry
Create a new entry in any form. Each form field is available as a dynamic content item. Empty fields will not be created unless they are provided in the request. Entry is not validated prior to creation.

### Action: Validate Gravity Forms form entry
Validate a new entry in any form. Each form field is available as a dynamic content item. Returns validation errors if any are found. Does not create the entry.

### Action: Get Gravity Forms entry notes
Get a list of notes from any entry and use them in your flow. Each note is available as a dynamic content item.

### Action: Create Gravity Forms entry note
Create a new note in any entry. Each note field is available as a dynamic content item.

### Action: Download Gravity Forms entry file upload
Download a file upload given the attachment URL. This is useful for sending the file to another service or for use in a flow. Uses built-in authentication so it will not conflict with other plugins that may provide additional site security.

### Action: Delete Gravity Forms entry file upload
Delete a file upload given the attachment URL. This is useful for cleaning up after a flow or for use in a flow. Uses built-in authentication so it will not conflict with other plugins that may provide additional site security.

### Action: Delete all file uploads from a Gravity Forms entry
Delete all file uploads from an entry. This is useful for cleaning up after a flow or for use in a flow. Uses built-in authentication so it will not conflict with other plugins that may provide additional site security.

## Obtaining Credentials
To use this connector you need to install the Gravity Forms Power Automate Pro plugin, available for purchase at reenhanced.com. When you install and activate the plugin, if you reload the Gravity Forms > Settings > Power Automate Pro page you will see the three items required to connect your site to Power Automate:

1. *License Key:* This is your license key for the plugin
2. *Secret Key:* A randomly generated secret known only to your WordPress site and Power Automate
3. *Site URL:* This is the full url to access your WordPress site and is needed so Power Automate knows how to connect to your WordPress installation.

## Getting Started
Once you have installed the plugin and created a connection you can start an automated flow that is triggered by submission of a Gravity Forms form or use any other trigger.

## Known Issues and Limitations

We cannot guarantee compatibility with other plugins that modify the Gravity Forms API. If you are using a plugin that modifies the Gravity Forms API, you may experience issues with the connector.

## Deployment Instructions
This is a certified connector and can be used immediately, but you will need to install the WordPress plugin on any site that you wish to connect to Power Automate.

