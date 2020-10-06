
## Ubiqod by Skiply
Ubiqod provides a simple and powerfull platform to connect your Skiply connected buttons or QR Codes to third party platforms. 
This connector will trigger a flow each time data is sent by a device belonging to the configured group.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan
* An Ubiqod subscription with Skiply buttons activated
The Ubiqod API Key can be found in the "Account" section of your Ubiqod backend.

## Prepare your device
Before adding the Ubiqod trigger in Power Automate, pay attention to the following points:
* The connector will be linked to a group. Make sure at least one group exists in your Ubiqod backend. 
* If your device is not linked to a Label Template, the default label will apply.

## Set up the connector
* Add the trigger "When data is received from devices"
* Enter your API Key if requested
* From the list of Group Names, choose the Group that should contain the devices you want to listen to
* Add the steps of your choice

## Available fields
The following fields are available for mapping:
* Device Name: Device Name as defined in the Ubiqod platform. This name is also visible on the device itself.
* Device Label: Device Label as defined in the Ubiqod platform. Can be an indoor location or any other element that idetifies the device.
* Request Date: Date of the request from Ubiqod server.
* Action Label: Label of the action. Can be the label of a button, or the label of a code, if code mode is activated.
* Times Button Pressed: For non-code mode, indicates the number of times the same button has been pressed. Always at 1 in code mode.
* Site Id: Ubiqod ID of the site associated with the device.
* Site Label: Ubiqod label of the site associated with the device.
* Badge: Will be 1 if badge was used, 0 otherwise.

## Normal mode vs code mode
In the configuration of your device, you can force the use of code mode.
If you press button 1 and button 2, in normal mode, 2 separate requests will be sent (one for each button). In code mode, this will only send one request, with code "12" (you can configure the associated Label in your Label Template).

## Test the connection
* Tests accounts include virtual devices which behave in exactly the same way as physical devices. Press the "calculator button" in the device list to launch the simulator. Press one or more buttons, and wait until the 3 green lights stay on for one second (usually 10 seconds after the first press).

## Supported Operations
The connector supports only one public operation:
* DataIn (triggered each time that a button send data)
