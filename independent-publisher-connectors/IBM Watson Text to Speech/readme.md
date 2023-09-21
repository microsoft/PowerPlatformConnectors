# **IBM Watson Text to Speech**

The IBM Watson™ Text to Speech service provides APIs that use IBM's speech-synthesis capabilities to synthesize text into natural-sounding speech in a variety of languages, dialects, and voices. The service supports at least one male or female voice, sometimes both, for each language. The audio is streamed back to the client with minimal delay.

### Publishers: Lucas Titus, Kin Cheung, Ivan Leong, Andrew Lau

# Pre-requisites

- A Microsoft Power Apps or Power Automate plan.
- An [IBM Cloud](https://cloud.ibm.com) account with access to Watson services.
- A provisioned [Text to Speech](https://cloud.ibm.com/catalog/services/text-to-speech) resource.

# Obtaining Credentials

Retrieve your IBM Watson Text to Speech API key and service URL from the **manage** resource page as seen in the image below.

![Credentials](https://i.gyazo.com/5e1d0845b57591a821ddf225eba38a19.png)

Copy your API key and URL and paste them in the connector connection configuration.

# Supported Operations

### [Synethesize](https://cloud.ibm.com/apidocs/text-to-speech#synthesize)
Synthesizes text to audio that is spoken in the specified voice. The service bases its understanding of the language for the input text on the specified voice. Use a voice that matches the language of the input text.

### [Get pronunciation](https://cloud.ibm.com/apidocs/text-to-speech#getpronunciation)
Gets the phonetic pronunciation for the specified word. You can request the pronunciation for a specific format. You can also request the pronunciation for a specific voice to see the default translation for the language of that voice or for a specific custom model to see the translation for that model.


### [List voices](https://cloud.ibm.com/apidocs/text-to-speech#listvoices)
Lists all voices available for use with the service. The information includes the name, language, gender, and other details about the voice. The ordering of the list of voices can change from call to call; do not rely on an alphabetized or static list of voices. To see information about a specific voice, use the Get a voice.


### [Get a voice](https://cloud.ibm.com/apidocs/text-to-speech#getvoice)
Gets information about the specified voice. The information includes the name, language, gender, and other details about the voice. Specify a customization ID to obtain information for a custom model that is defined for the language of the specified voice. To list information about all available voices, use the List voices method.

# Known Issues and Limitations

Currently, the audio file from the **Synethesize** operation is returned as a base64 string to allow support in Canvas apps. This might cause issues with other services.