# Voice Monkey

Voice Monkey enhances Amazon Alexa's functionality by enabling external triggering of Alexa Routines and offering features like custom Text To Speech announcements and media playback. It integrates with platforms such as IFTTT, Zapier, and Home Assistant, facilitating Alexa's connection with a variety of applications and smart devices. These integrations collectively enrich the smart home experience, making Alexa a more versatile and dynamic tool for home automation.

## Publisher: Richard Wilson

## Prerequisites

To use Voice Monkey, you'll need to create a Voice Monkey account. Sign up on the [Voice Monkey website](https://voicemonkey.io/), where you can choose between the free and paid plans. The free plan covers essential features, while the paid plan offers advanced functionalities for more extensive use cases.

## Supported Operations

The Voice Monkey integration supports a variety of operations to enhance your interaction with Alexa devices:

### 1. Trigger a Voice Monkey Routine

This operation allows you to remotely trigger a pre-defined Alexa routine via Voice Monkey.

- **Input Properties:**
  - **Device ID:** The unique identifier of the Alexa device where the routine will be triggered.
  - **Routine Details:** Specify the particular routine to be triggered.

### 2. Make an Announcement

Enables you to send custom TTS announcements to your Alexa devices.

- **Input Properties:**
  - **Device ID:** The unique identifier of the Alexa device for the announcement.
  - **Announcement Text:** The text to be converted to speech.
  - **Voice:** (Optional) The voice to be used for the announcement.
  - **Language:** (Optional) Language code to enhance the announcement's clarity.
  - **Chime:** (Optional) Sound to play at the start of the announcement.
  - **Audio:** (Optional) URL of an audio file to play.
  - **Background Audio:** (Optional) Background audio file URL to play alongside the text to speech.
  - **Website:** (Optional) URL of a website to open on screen-enabled Echo devices.
  - **No Background:** (Optional) Set to true to not show Voice Monkey logo or image on screen-enabled devices.
  - **Image:** (Optional) URL of an image file to display.
  - **Media Width:** (Optional) Width of the image file.
  - **Media Height:** (Optional) Height of the image file.
  - **Media Scaling:** (Optional) Scaling option for the image.
  - **Media Alignment:** (Optional) Alignment of the image within the container.
  - **Media Radius:** (Optional) Clipping radius for image corners.
  - **Video:** (Optional) URL of a video file to display.
  - **Video Repeat:** (Optional) Number of times the video should loop.
  - **Character Display:** (Optional) Text to display on devices like the Echo Dot With Clock.

### 3. Trigger a Flow

Start a specific Voice Monkey Flow based on your defined criteria.

- **Input Properties:**
  - **Device ID:** The unique identifier of the Alexa device.
  - **Flow ID:** The numeric ID of the Flow you wish to initiate.

## Obtaining Credentials

1. **Sign Up for Voice Monkey:**
   - Visit [Voice Monkey's Website](https://voicemonkey.io) and sign up for an account.

2. **Access Your Dashboard:**
   - Once registered, navigate to the Dashboard page on Voice Monkey.

3. **Get Your Tokens:**
   - Find your Access token and Secret token within the Dashboard. These tokens are essential for using the Voice Monkey connector.

Please ensure that you keep your tokens secure and do not share them publicly.

## Known Issues and Limitations

Currently, there are no known issues or limitations with the Voice Monkey integration. Always refer to this section for the most up-to-date information regarding the connector's performance and compatibility.