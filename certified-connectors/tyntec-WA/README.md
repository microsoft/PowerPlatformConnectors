## Tyntec Conversations API - Send template WhatsApp message

Tyntec Conversations API allows you to send template WhatsApp messages.

Create conversational experiences with your customers right on the world’s number one messaging app (over 2 billion monthly active users). Leveraging WhatsApp’s end-to-end encryption and rich features (images, videos, audios, documents, interactive buttons, etc.), you can remove friction from customer onboarding, notifications and support communication.

### Use Cases of this connector
-   Use this connector in PowerApps Workflows (great for Marketing, CRM or Sales)
-   Send SMS messages using Azure Logic Apps
-   Build SMS support in your Microsoft Power Automate automatizations

And many more!


## Pre-requisites
You will need the following to proceed:
-   A Microsoft Power Apps or Power Automate plan with custom connector feature
-   [tyntec API Key](http://my.tyntec.com/api-settings)
-   [WhatsApp Business account number](https://www.tyntec.com/docs/whatsapp-business-api-account-information-get-started#toc--whatsapp-business-account-) provide by tyntec
-   [WhatsApp Message Template](https://www.tyntec.com/docs/whatsapp-business-api-account-information-get-started#toc-message-templates) approved by WhatsApp


## Supported requests
- **Send Free-form Audio Message** Sends an audio message using [Conversations API](https://api.tyntec.com/reference/#conversations-send-messages-send-a-message).
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **url** - URL of the audio file
- **Send Free-form Image Message** Sends an image message.
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **url** - URL of the image file
- **Send Free-form Contact Message** Sends a contact message
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **contacts addresses city** - Contact's city
  - **contacts addresses country** - Contact's country
  - **contacts addresses countryCode** - Contact's country code
  - **contacts addresses street** - Contact's street
  - **contacts addresses type** - Type of Address (Work, for example)
  - **contacts addresses zip** - Contact's ZIP
  - **formattedName** - Contact's formated name
  - **lastName** Contact's last name
- **Send Free-form Document Message** Sends a document message.
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **url** - URL of the file
  - **caption** - caption of the file
  - **filename** - filename of the file
- **Send Free-form List Message** Sends a list message.
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **type** - Header's type (text)
  - **text** - Header's text
  - **type** - Body's type (text) - required
  - **text** - Body's text - required
  - **type** - Footer's type 
  - **text** - Footer's text
  - **title** - List name
  - **sections title** - Name of section
  - **sections rows payload** - Payload for section
  - **sections rows title** - Title for section
  - **sections rows description** - Description for section
- **Send Free-form Location Message** Sends a location message.
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **longitude** - Longtitude of your location
  - **latitude** - Latitude of your location
  - **name** - Name of your location
  - **address** - full address of your location with commas as separators
- **Send Free-form Product List** Sends a product list
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **type** - Header's type (text)
  - **text** - Header's text
  - **type** - Body's type (text) - required
  - **text** - Body's text - required
  - **type** - Footer's type 
  - **text** - Footer's text
  - **catalogId** - Catalog ID
  - **sections items productId** - Product ID
- **Send Free-form Quick Reply** Sends a quick reply message
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **type** - Header's type (text)
  - **text** - Header's text
  - **type** - Body's type (text) - required
  - **text** - Body's text - required
  - **type** - Footer's type 
  - **text** - Footer's text
  - **buttons payload** - Payload of your button
  - **buttons title** - Title of your button
- **Send Free-form Single Product** Sends a single product 
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **type** - Header's type (text)
  - **text** - Header's text
  - **type** - Body's type (text) - required
  - **text** - Body's text - required
  - **type** - Footer's type 
  - **text** - Footer's text
  - **catalogId** - Catalog ID
  - **sections items productId** - Product ID
- **Send Free-form Sticker Message** Sends Sticker Message
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **url** - URL of the sticker file
- **Send Free-form Text Message** Sends text message 
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **text** - Your text message
- **Send Free-form Video Message** Sends a video message.
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **url** - URL of the video file
  - **caption** - caption of the video file
- **Send Template Document Message** - Sends template message with document
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **templateId** - ID of your template
  - **templateLanguage** - language code of your template
  - **header type** - type of the header (document)
  - **header url** - URL to your document file
  - **body type** - type of your body argument - text
  - **body text** - text of your body argument
- **Send Template Dynamic URL Button** - Sends template message with URL Buttons
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **templateId** - ID of your template
  - **templateLanguage** - language code of your template
  - **body type** - type of your body argument - text
  - **body text** - text of your body argument
  - **button type** - type of your button argument 
  - **button index** - index of your button argument 
  - **button text** - text of your button argument
- **Send Template Image Message** - Sends template message with image
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **templateId** - ID of your template
  - **templateLanguage** - language code of your template
  - **header type** - type of the header (image)
  - **header url** - URL to your image file
  - **body text** - text of your body argument
- **Sends Template Location Message** - Sends template message with location
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **templateId** - ID of your template
  - **templateLanguage** - language code of your template
  - **header longitude** - longtitude of your location
  - **header latitude** - latitude of your location
  - **header name** - Name of your location
  - **header address** - Address of your location
  - **body type** - type of your body argument - text
  - **body text** - text of your body argument
- **Send Template Quick Reply Button** - Sends template message with Quick reply Buttons
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **templateId** - ID of your template
  - **templateLanguage** - language code of your template
  - **body type** - type of your body argument - text
  - **body text** - text of your body argument
  - **button type** - type of your button argument 
  - **button index** - index of your button argument 
  - **button payloada** - payload of your button argument
- **Send Template Text Message** - Sends template message with text arguments
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **templateId** - ID of your template
  - **templateLanguage** - language code of your template
  - **header type** - type of the header (text)
  - **header text** - text in header
  - **body type** - type of your body argument - text
  - **body text** - text of your body argument
- **Send Template Video Message** - Sends template message with video
  - **from** - Your WhatsApp Business Account number (Sender)
  - **to** - Receiver's phone number
  - **templateId** - ID of your template
  - **templateLanguage** - language code of your template
  - **header type** - type of the header (video)
  - **header url** - URL to your image file
  - **body text** - text of your body argument
  - **body type** - type of your body argument - text

- **Status Check**
    -   To make a successful status check, please, provide the connector with the following values
        -   **id** - messageID of your message (_returned after each request_)
- **Validate your WABA number**
  - To validate your WABA number, please, provide the connector with the following values:
        -  **WhatsAppBusinessNumber** - Your WhatsApp Business Account number 
## Supported Triggers

- **Incoming Message** Trigger any flow on new incoming messages
    - **Response Fields**
        - **messageId** (the message ID)
        - **channel** - the channel on which the message was received (viber, whatsapp or sms)
        - **from** - the sender's phone number
        - **to** - the reciever's phone number (you)
        - **receivedAt** - when was the message received
        - **text** - the text of the message
        - **file** - any attached file to the message
        - **event** - on which event the webhook was triggered
        - **timestamp** - the timestamp of the message


## How to get API key 
Please [sign up for a free account](https://www.tyntec.com/create-account). In your account, select the [API settings](http://my.tyntec.com/api-settings) and copy your API key.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.
