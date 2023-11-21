# App Power Forms Connector

Forms Connector from [App Power Solutions LLC](https://apppower.net) provides a suite of actions and triggers to easily include external stakeholders in your business processes. The connector makes it very easy and straight forward to create a form within the designer resulting in a URL that you can send to the external party. 

There are two different ways you can create the form. 

1. Use action *Create App Power Easy form* to first create an empty form to which you can then add fields one by one in the subsequent steps
2. Use action *Create new form from Adaptive Card* to use an Adaptive Card JSON string to create the form

## Prerequisites

### Installing the custom connector

You need Power Platform CLI tools to install the custom connector files to your environment.

Run the following command and follow the prompts:

```
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```

### Acquiring license

If you want to start with a free trial license, you can go ahead and create a new App Power Forms connection by following these steps. The trial key will be sent to you during the process. You can also acquire a paid license from [App Power](https://apppower.net) website.

1. Browse to Connections section under Power Apps or Power Automate site (e.g. https://us.flow.microsoft.com/ or https://make.powerapps.com/)
1. Click **+ New connection**
1. Search for App Power Forms connector an click on the plus icon next to it
1. You are no presented with a dialog allowing you to enter an API key. However, you can also type in your e-mail address to have a trial key delivered right into you inbox
1. Once you've typed in you e-mail address, go ahead and click **Create**
1. You will now receive a notification *Check you inbox for new trial key!*
1. Go ahead and check you inbox. You should have received an e-mail with a subject *Your trial key for App Power Forms*
1. Copy the trial key in the e-mail and paste it in the new connection dialog (where you just typed in your e-mail address)
1. Click **Create** and a new connection is successfully created!

The trial license is intended for testing only and can't be used in production. You can purchase a production license at https://apppower.net.

## Actions

The following actions are available in App Power Forms connector.

### Create new App Power Easy form

Create a new App Power Easy form. After this action you can add any number of fields to the form by adding field specific actions.


|Property|Required|Description|
|---|---|---|
|Form name|Yes|Unique name for the form. This text is not visible to the end user. The value is used to provide dynamic metadata for the maker in the flow designer.|
|Form description|No|This text is displayed at the beginning of the form with some emphasis compared to other form content.|
|Thank you text|No|This text is displayed to the end user after submitting the form. If nothing is specified text *Thank you!* is displayed.|

### Add new field to App Power Easy form

Use this action to add a new field to the form created in an earlier step. The fields will be visible on the form in the order they are added.

|Property|Required|Description|
|---|---|---|
|Form ID|Yes|Id of the form this field is added to. This is typically *Card ID* value of *Create new App Power Easy form* action defined earlier in the flow.|
|Form name|Yes|Name of the form this field is added to. The value selected from the list typically matches the *Form name* value of *Create new App Power Easy form* action defined earlier in the flow. The value is used to provide dynamic metadata for the maker in the flow designer.|
|Field name|Yes|The name of the field being added to the form. Field name is displayed on the form as label for the input control.|
|Field type|Yes|the type of the field being added to the form. Based on the selected type some additional fields are displayed in the action. Available field types are: *Text input, Date select, Boolean, Option list, Static text, Hidden value* and *button*.

### Wait for form response

Use this action to wait until a specific form is submitted. You can use this action for both Easy Forms and forms based on Adaptive Card JSON.

|Property|Required|Description|
|---|---|---|
|Card ID|Yes|ID of the card or form to wait for. The value of this property is typically *Card ID* output of the *Create new App Power Easy form* action defined earlier in the flow.|
|Card name|Yes|Name of the form. The value selected from the list typically matches the *Form name* value of *Create new App Power Easy form* action defined earlier in the flow. The value is used to provide dynamic metadata for the maker in the flow designer.|

When a response is received, the submission data is removed from the database. This means that no data entered by the person making the submission is persisted after the submission is consumed by this action or by *When form is submitted* trigger.

### Get form as Adaptive Card JSON

Get Easy Form as Adaptive Card JSON. You can use the JSON to post the card in Teams as an Adaptive Card, for example.

|Property|Required|Description|
|---|---|---|
|Card Id of the form|Yes|Give the Card Id of the form definition to get the JSON from.|

### Create new form from Adaptive Card

Another way of creating new forms is to use Adaptive Card JSON. You have to use some external tool - like [Adaptive Cards Designer](https://adaptivecards.io/designer/) - to create the desired JSON.

|Property|Required|Description|
|---|---|---|
|Card name|Yes|Unique name for the card. This text is not visible to the end user. The value is used to provide dynamic metadata for the maker in the flow designer.|
|Adaptive Card JSON|Yes|Adaptive Card JSON that will be rendered to the end user.|
|Card displayed after submit|No|Adaptive Card JSON that will be displayed to the end user after submitting the card. If this is left blank, a simple *Thank you!* text is displayed.|

## Triggers

The following triggers are available in App Power Forms connector.

### When form is submitted

This trigger is fired whenever there are new submissions for the specified form.

|Property|Required|Description|
|---|---|---|
|Form name|Yes|Choose the form to get submissions from. Unlike *Wait for form response* action described earlier, this trigger listens for **all** submissions for the specified *Form name*. Therefore it is possible that multiple submissions are returned at a time and you have to use looping construct or similar mechanism to process individual submissions.|

All submissions are removed from the database when they are returned by the trigger.

## Field types

When you have created a form with *Create new App Power Easy form* action you can start adding fields to it using *Add new field to App Power Easy form* action. All available field types are explained in this section.

### Text input

Text input maps to [Input.Text](https://adaptivecards.io/explorer/Input.Text.html) Adaptive Card input. When you select this field type, you are presented with the following additional properties.

|Property|Description|
|---|---|
|Is required|Whether this is a required field.|
|Multiple lines of text|Whether this is a single-line text field or is the user allowed to enter multiple lines of text.|
|Default value|The field is prefilled with the value given here.|

### Date select

 Date select maps to [Input.Date](https://adaptivecards.io/explorer/Input.Date.html) Adaptive Card input. When you select this field type, you are presented with the following additional properties.

|Property|Description|
|---|---|
|Is required|Whether this is a required field.|
|Default date|The value of the field is prefilled with the date given here. The value must be in ISO format, e.g. `2022-01-10`. You can also use `utcNow()` expression to use the current date.|
|Min date|The earliest date the user is allowed to choose.|
|Max date|The latest date the user is allowed to choose.|

### Boolean

 Boolean maps to [Input.Toggle](https://adaptivecards.io/explorer/Input.Toggle.html) Adaptive Card input. When you select this field type, you are presented with the following additional properties.

|Property|Description|
|---|---|
|Is selected by default|Whether this field is checked by default.|

### Option list

Option list maps to [Input.ChoiceSet](https://adaptivecards.io/explorer/Input.ChoiceSet.html) Adaptive Card input. When you select this field type, you are presented with the following additional properties.

|Property|Description|
|---|---|
|Allow multiple selections|Whether the end user can choose more than one option from the list.|
|Is required|Whether this is a required field.|
|Default selected option|The option that is preselected on the form. The text entered in this field must match one of the options of the field.|
|Options|List of options available to the end user to choose from.|

### Static text

Static text maps to [TextBlock](https://adaptivecards.io/explorer/TextBlock.html) Adaptive Card element. When you select this field type, you are presented with the following additional properties.

|Property|Description|
|---|---|
|Text to display|The static text displayed to the end user.|
|Highlight text on form|If selected *Yes*, this static text is highlighted on the form.|

This field is not an input element on the form. It can be used to provide information and instructions to the end user.

### Hidden value

 When you select this field type, you are presented with the following additional properties.

|Property|Description|
|---|---|
|Value|The value stored along with the field.|

Hidden values are not accessible to the end user in any way. If you are using *When form is submitted* trigger to collect responses from your form, this field is a good way to pass some additional business data to the trigger flow from the originating flow.

### Button

Button maps to either [Action.Submit](https://adaptivecards.io/explorer/Action.Submit.html) or [Action.OpenUrl](https://adaptivecards.io/explorer/Action.OpenUrl.html) Adaptive Card action. When you select this field type, you are presented with the following additional properties.

|Property|Description|
|---|---|
|URL|If given, a new browser tab or window is opened when the user clicks on the button. Otherwise the form is submitted.|

The *Field name* value is used as the button text on the form.

Buttons are positioned at the bottom of the form regardless of when they are added to the form in your form creation flow.

## Known issues

- If you add a field to a form and then remove it in the designer, the field is still visible in the dynamic content picker. However when the flow is run the value will always be empty.
- Currently there is no way to remove forms that have been added during flow design. 