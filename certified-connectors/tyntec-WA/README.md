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
-   **Send WhatsApp** using tyntec Conversations API [reference](https://api.tyntec.com/reference/#conversations-send-messages-send-a-message)
    -   To make a successful request, please, populate the followings fields:
        -   **to** - receiver's phone number in _international_ form without leading 00 (_E.g. 4989202451100_)
        -   **from** - your WhatsApp Business account number number
        -   **templateId** - the name of your approved template
        -   **language-policy** - language policy of your template
        -   **language-code** - language code of your template
        -   **components type** - use **body** if you are sending text. Rich media use **header**
        -   **type of parameter** - use **text** if sending text, rich media uses **media**
        -   **parameter-url** - input the source url
        -   **parameter-type** - type of used media - support **image**, **document** and **video**
        -   **parameter-filename** - filename for your rich media
        -   **parameter-text** - input your text message
        -   _if your template uses more parameters, repeat **parameter-type** and **parameter-text** for each parameter your template requires_
    

        
        
- **Status Check**
    -   To make a successful status check, please, provide the connector with the following values
        -   **id** - messageID of your message (_returned after each request_)


## How to get API key 
Please [sign up for a free account](https://www.tyntec.com/create-account). In your account, select the [API settings](http://my.tyntec.com/api-settings) and copy your API key.

## Deployment instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.
