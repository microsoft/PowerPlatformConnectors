# Synthesia
Synthesia STUDIO lets you create one-off videos and test different types of videos before automating the process through the service. Also, Synthesia STUDIO lets you define templates with personalization variables that you can call later through the service.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
This connector requires an account and a paid subscription with [Synthesia](https://www.synthesia.io/).

## Obtaining Credentials
Once logged into your account, go to Settings and in the API Keys area, create a new API key.

## Supported Operations
### List videos
Retrieve a list of all videos created via the API or STUDIO.
### Create a video
Creates a video based on the request parameters.
### Get video status
Queries the video status. A video can be in status `in_progress' from 10 minutes up to 40+. Once a video is completed, you will be able to see a download URL that you can use to download the .mp4 file.
### Delete video
Delete a video.
### Update a video
Patch the parameters of an existing video.
### List templates
Retrieve a list of all templates in Synthesia STUDIO, as well as the variables that may be provided when using them.
### Get template
Retrieve the information on a template. In particular, you may use it to find out the variables available for customization.
### Create a video from a template
Create a video based on a template created in Synthesia STUDIO.

## Known Issues and Limitations
There are no known issues at this time.
