# Philips HUE

Philips HUE allows you to control smart home devices, like lights, switches, plugs and more built by Philips.
The connector is using API V2 endpoints.

## Publisher: Tomasz Poszytek

## Prerequisites

You have to create a developer account at Philips HUE. You can sign up for free [here](https://developers.meethue.com/).

## Obtaining Credentials

### Hue Remote API

The **Hue Remote API** is a definition of an app that you will use to authorize all requests from Power Automate. To set it up navigate to [your apps](https://developers.meethue.com/my-apps/) and create new **Hue Remote API** to use with Power Automate. Use ``https://global.consent.azure-apim.net/redirect`` as the Callback URL. After **appid** is created you will be able to obtain your **Client ID** and **Client Secret** values.

### HUE application key

**HUE application key** is mandatory to provide during connection creation. Without the valid application key you won't be able to access any data from your account. The HUE application key must be generated once and it is not expiring. **The process consist of two steps**. You need to perform first a PUT request and then POST request to generate it:

1. Download and install Postman application from [here](https://www.postman.com/downloads/). You can as well use any other software allowing you to make HTTP calls.
2. Import **PUT request** to Postman:

```curl --location --request PUT 'https://api.meethue.com/route/api/0/config' \
--header 'Content-Type: application/json' \
--header 'Authorization: Bearer YOURTOKEN' \
--data-raw '{"linkbutton":true}'
```
3. Set up Authorization using oauth 2.0. To do that follow steps described in [Postman Learning Center](https://learning.postman.com/docs/sending-requests/authorization/#oauth-20). Use the following values:

    1. Auth URL: https://api.meethue.com/v2/oauth2/authorize
    2. Access Token URL: https://api.meethue.com/v2/oauth2/token
    3. Client ID: Available under HUE Remote API created as a Prerequisite: https://developers.meethue.com/my-apps/
    4. Client Secret: Available under HUE Remote API created as a Prerequisite: https://developers.meethue.com/my-apps/

4. Import **POST request** to Postman and choose configured in step 3. oauth 2.0 as its Authorization:

```curl --location --request POST 'https://api.meethue.com/route/api' \
--header 'Content-Type: application/json' \
--header 'Authorization: Bearer YOURTOKEN' \
--data-raw '{"devicetype":"YOUR NAME HERE"}'
```

5. Execute PUT request and after that POST request. **Copy "username" value** returned by POST request.

You're now ready to create a connection for Philips HUE connector in Power Automate.

## Supported Operations

### Get lights

Get list of your devices having lightning capabilities.

### Get light

Get details of a specific device having lightning capabilities.

### Execute light

Execute device having lightning capabilities.

### Get devices

Get list of all your devices.

### Get device

Get details of a specific device.

### Execute device

Execute a specific device.

### Get rooms

Get list of rooms (groups).

### Get scenes

Get a list of all scenes. Scenes are used to store and recall settings for a group of lights.

### Get scene

Get details of a specific scene.

### Delete scene

Delete specific scene.

## Known Issues and Limitations

Action's **Execute light** parameter "Action" should contain value only for devices supporting it, e.g. light bulbs. Using it with other device types will complete request successfully although will return status code 207 instead 200.

## Disclaimer

Current version of the connector does not cover all possible actions available under Philips HUE API v2 collection. I have built those I found the most important. Please let me know via https://aka.ms/poszytek what other actions you would like to have and I will update the connector.