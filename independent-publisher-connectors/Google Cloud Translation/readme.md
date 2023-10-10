# Google Cloud Translation
Cloud Translation enables your websites and applications to dynamically translate text programmatically through an API. Translation uses a Google pre-trained or a custom machine learning model to translate text. By default, Translation uses a Google pre-trained Neural Machine Translation (NMT) model, which Google updates on semi-regular cadence when more training data or better techniques become available.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
This connector requires the [Basic Cloud Translation](https://cloud.google.com/translate/docs/editions) edition. This edition handles primarily casual user-generated content, such as chat, social media, or comments.

## Obtaining Credentials
You will need to generate an OAuth 2.0 client ID and secret for authentication. You will need to setup your OAuth Consent Screen before creating the credentials. More information about the setup is located [here](https://cloud.google.com/docs/authentication/end-user#creating_your_client_credentials).

## Supported Operations
### Translate text
Translates input text, returning translated text.
### Detect language
Detects the language of text within a request.
### Get language list
Returns a list of supported languages for translation.

## Known Issues and Limitations
There are no known issues at this time.
