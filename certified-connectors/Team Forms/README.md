# Team Forms

Teams Forms brings digital forms into Microsoft Teams. The software will empower your teams to build and deliver forms from within the productivity tools that they already know and trust. Unlike many other forms solutions on the market, Team Forms talks directly with your Teams SharePoint so that data captured by forms remains in your ownership and never leaves your trusted office 365 environment

## Publisher: VP Labs

VP Labs is an independent software vendor providing Team Forms as a software service.

## Prerequisites

Team Forms offers a free and paid subscription to our service.To get started you simply need a Microsoft Office 365 work or school account with access to Microsoft Teams. For more information on our licensing please visit our [pricing page]('https://teamforms.app/pricing').

## Supported Operations

---

### `Trigger` When a new form response is submitted

Triggered a flow when a response is submitted in Team Forms. This trigger will fire immediately after a form has been uploaded to SharePoint. You can use the output of the trigger to access fields within the form response.

### `Action` Teams

Returns all teams that the authenticated user is a member of.

### `Action` Forms

Returns all the forms for a given team.

### `Action` Form

Returns details for a specific form.

### `Action` Files

Returns metadata for all files/attachments associated with a form response. Users can use the id from the response with the [SharePoint actions]('https://learn.microsoft.com/en-us/connectors/sharepointonline/') to get the file contents.

### `Action` Get Pdf

Returns details of the pdf file generated for a given form response.

### `Action` Get Pdf Content

Returns the pdf file generated for a given form response.

### `Action` Response

Returns details of a form response.

### `Action` Response Schema

This action is used internally to dynamically get a form response schema so flow can provide intuitive response fields. You cannot call this action directly.

### `Action` Delete Subscription

This action is used internally to delete subscription triggers when flow trigger actions are updated or deleted.

## Obtaining Credentials

To use Team Forms simply login using your existing Microsoft Office work or school account and provide it access to SharePoint and Teams.

## Known Issues and Limitations

Usage volume of this connector will be limited based on your Team Forms licensing.
