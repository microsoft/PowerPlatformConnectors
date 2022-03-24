# myStrom

myStrom is a Swiss company producing smart home devices. Company produces smart bulbs, led strips, switches and controllers.  

## Publisher: Tomasz Poszytek

## Prerequisites

You have to create an account at myStrom. You can sign up for free [here](https://mystrom.ch/cockpit/en/). Also it is good to install myStrom app ([Google Play](https://play.google.com/store/apps/details?id=com.cogniance.asoka.srs.android), [App Store](https://apps.apple.com/en/app/mystrom-mobile/id546326128)), for extended control of devices and scenes creation.

## Supported Operations

### Authentication

Obtain authentication token by providing your login and password. Authentication token is required by all other requests.

### Get devices

Get list of all user's registered devices.

### Get device

Get details of the specific user's device by providing its Device ID.

### Toggle device

Change user's device state, like turn it on, off, toggle, change color, dim or ramp values.

### Get scenes

Get list of all user's scenes.

### Execute scene

Execute scene by ID. This will trigger all actions defined for the scene.

### Create webhook

Create webhook for a device.

### Get webhook

Get webhook defined for a device.

### Delete webhook

Delete device's webhook.

## Obtaining Credentials

To get authentication token you need to execute action **Authenticate** providing your e-mail and password used to create your myStrom account. The authentication token is permanent for each account, therefore once you obtain it, you can save it in eg. Azure Key Vault and then provide to other actions from the service.

## Known Issues and Limitations

Colors' values that can be provided to the **Toggle device** action must be formatted using **HSV** model eg. 0;100;100 (for red). No other issues and limitations are known at this time.

## Getting Started

You can visit [myStrom REST API v2](https://mystrom.ch/api/v2/) to test endpoints yourself. 

## Disclaimer

myStrom documentation for the endpoints is rather laconic, therefore despite the fact some endpoints offer more parameters, I was not able to find out what data should be provided to make them work. Also when I was testing them with my own devices, especially smart wifi bulb, I didn't observe any reactions, thus decided to add only those methods and parameters that actually did work.