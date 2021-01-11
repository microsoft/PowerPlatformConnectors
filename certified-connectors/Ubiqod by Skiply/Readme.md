
## Ubiqod by Skiply 
Ubiqod provides a simple and powerful platform to connect your Skiply IoT buttons and Qods to third party platforms. This connector triggers a flow every time data is sent by any of the IoT-devices or Qods belonging to the configured group. 

## Prerequisites 
To proceed, you need: 
* A Microsoft Power Apps or Power Automate plan 
* A Ubiqod subscription 
The Ubiqod API Key can be found in the "Account" section of your Ubiqod backend. 

## Prepare your IoT-devices and Qods 
Before using the Ubiqod trigger in Power Automate, pay attention to the following points: 
* The connector will be linked to a group: make sure at least one group exists in your Ubiqod backend. 
* If an IoT-device or a Qod is not linked to a Code list, the raw value of the code will be sent (or buttons indexes for IoT-devices). 
* If an IoT-device or a Qod is linked to a Code list but the entered code is not defined in the related Code list, the raw value of the code will be sent (or buttons indexes for IoT-devices). 

## Set up the connector 
* Select the trigger "When data is received from devices" 
* Enter your API Key if requested 
* From the Group field, select the Group containing the IoT-devices and/or Qods you want to listen to 
* Then add the steps of your choice 

## Available fields 
The following fields are available for mapping: 
* Timestamp: datetime of Qod scanning or IoT-device message transmission
* Identifier:  identifier of the Qod or IoT-device (s/n)
* Qod or IoT-device label: label of the Qod or IoT-device 
* Site ID: site ID the Qod or IoT-device is linked to 
* Site label: site name the Qod or IoT-device is linked to 
* Customer contact email: email of customer contact defined on Site 
* Customer contact phone: phone number of customer contact defined on Site 
* Site manager email: email of site manager defined on Site 
* Site manager phone: phone number of site manager defined on Site 
* Action value: code label or raw value if no such code defined in the related code list (button index for IoT-devices if no such code defined) 
* GPS condition: 1 if condition is valid, 0 if not -
condition is valid when the user GPS position is under the validation distance from the site position, 0 if not or if the user refused to give its position
* On-device condition: 1 if condition is valid, 0 if not - For IoT-device: condition is valid when magnetic badge has been swiped / For S-Qod: condition is valid when dynamic QR code has been scanned from the device and validated by the Ubiqod server
* Code condition: 1 if condition is valid, 0 if not - condition is valid when the entered code is defined in the related code list
* GPS value: if available, GPS coordinates of the user's position 
* Validation code value: if available, validation code label or raw value if no such code defined in the related code list 
* Number of presses: only for IoT-devices, number of times the button was pressed 
* Custom field_1: only for Qod with a form, value of field #1 
* Custom field_2: only for Qod with a form, value of field #2 
* Custom field_3: only for Qod with a form, value of field #3 
* Custom field_4: only for Qod with a form, value of field #4 

## Normal mode vs code mode (only for IoT-devices)
In the configuration of your IoT-device, you can force the use of “code mode”. 
If you press button 1 and button 2, in “normal mode”, 2 separate requests are sent (one for each button). In “code mode”, the same sequence of presses only sends one request, with code "12" (you can configure the associated label in your Code list). 

## Test the connection 
* Ubiqod accounts include virtual devices which behave exactly the same way as IoT-devices. Press the "play button" in the IoT-device list to launch the simulator. Press one or more buttons, and wait until the 3 green lights stay on for one second (usually 4 seconds after the first press). 

## Supported Operations 
The connector supports only one public operation: 
* DataIn (triggered every time a button sends data)
