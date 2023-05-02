# WhatsApp

Allows the user to send automated WhatsApp messages through a flow on Power Automate. Custom message templates can be sent and variables within the JSON body can be set. These variables can be in the form of an image header and/or a text variable in the body.

## Publishers: Zakariya Fakira, Satbir Virdi, Oscar Hui, Chaohui Wang

## Prerequisites

- WhatsApp Business Account
- Meta developer account

1. Creating the Meta App

   Follow the steps on the following link to easily create a Meta Developer account: https://developers.facebook.com/docs/development/register/

2. Creating the Meta App

   Steps defined in detail by Meta: https://developers.facebook.com/docs/development/create-an-app/

   To get started, create an app on the meta app dashboard.

   Select the developer console page from the WhatsApp business account.

   From there, select create app and follow the setup process.

3. Using the Phone Number ID

   Once the above has been completed, the phone number ID which can be viewed on the Meta App can be used to complete the URL request path in the API call. This can be set through the connector.

4. Adding recipients to the list of contacts

   Only phone numbers registered to the Meta App can be contacted using the connector so it is key that these phone numbers are added to the list. This can be done by heading to the ‘Getting Started’ page of the Meta App and selecting ‘Manage Phone Number List’ in the recipient drop down list.

5. Creating custom message templates

   Once everything is set up, you can choose to create your own message templates to be sent from your WhatsApp Business account. These message templates will need to be approved by Meta before you can use it in the connector.

   For that, follow the steps, thoroughly explained by Meta on this website: [Create message templates for your WhatsApp Business account | Meta Business Help Centre (facebook.com)](https://www.facebook.com/business/help/2055875911147364?id=2129163877102343).

More information about the WhatsApp Business Cloud API can be found on https://developers.facebook.com/docs/whatsapp/cloud-api.

## Supported Operation

The connector supports the following operations:

- Send Message: Sends a message to the recipient provided that the latter is found in the list of phone numbers for the Meta Account.

Important Note on how to consume the connetor in a flow: If the user wishes to send text-only or image+text message templates, a slight variation needs to be made in the Power Automate Flow.

If the user is using text-only, they can simply disregard the 'link' field in the flow.

If the user is using image+text, they can use the 'link' field to specify the image URL.

It may happen that the user uses different templates at once, and this has to be taken into account when using the connector. Should the user need both a text-only and an image+text template simultaneously, they should use the connector in 2 separate flows.

## Obtaining Credentials

### Generating the permanent token

By default Meta uses a temporary token that expires in 24 hours. This temporary token can be used as the API key but will have to be updated within 24 hours.

To get a permanent token, follow the steps from the following link: [Permanent token for WhatsApp Cloud API: How to generate? | Pepper Cloud Blog](https://blog.peppercloud.com/generate-permanent-token-for-whatsapp-cloud-api/)

## Known Issues And Limitations

Recipient is unable to reply to the messages as a webhook hasn’t been configured on the connector.

For now the connector can only be used in a Power Automate Flow as Power Apps doesn’t recognize the innermost nested arrays found in the JSON body. By setting the raw body, it would work. In Power Automate, it works perfectly.
