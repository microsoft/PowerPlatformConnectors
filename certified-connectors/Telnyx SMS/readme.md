# Telnyx
Telnyx Suite is a comprehensive cloud communication platform that provides communication capabilities via voice, SMS, fax, and IP services. Telnyx provides global high-speed connections for low latency, high uptime service. The Telnyx Suite includes SIP trunking, messaging API, voice API, lookup API, programmable fax API, and wireless services.

## Publisher: Telnyx LLC

## Prerequisites
Sign-up free from https://telnyx.com/ to create an API key. For more information on creating the API keys are available from https://support.telnyx.com/en/articles/4305158-api-keys-and-how-to-use-them

## Supported Operations
### Send a message
Send a message with a Phone Number, Alphanumeric Sender ID, Short Code or Number Pool.

## Obtaining Credentials
Review the following guide for detailed instructions for obtaining Telnyx API Key: https://support.telnyx.com/en/articles/4305158-api-keys-and-how-to-use-them

The Telnyx API V2 uses API Keys to authenticate requests. You can view and manage your API Keys by logging into your Mission Control Portal account and navigating to the Auth V2 tab in the "Auth" section. For information regarding API V1 Authentication please refer to here.

Your API Keys carry many privileges, so be sure to keep them secure! Do not share your secret API Keys in publicly accessible areas such as GitHub, client-side code, and so forth.

To use your API Key, assign it in your SDK as shown in this quickstart guide section. Using our RESTful API, you can also make requests by passing the API Key in the Authorization header: Authorization: Bearer <YOUR_API_KEY>".

All API requests must be made over HTTPS. Calls made over plain HTTP will fail. API requests without authentication will also fail.

## Getting Started
For detailed setup information please go to: https://developers.telnyx.com/docs/v2/messaging/quickstarts/portal-setup

### Sign Up for a Telnyx Mission Control Portal Account
Head to telnyx.com/sign-up to sign up for your free Telnyx account. It’ll give you access to our Mission Control Portal where you can buy numbers, set up and manage messaging profiles, and more.

A free credit will be automatically applied to your account upon signup. To enter a promo code, click on My Account in the top-right portal dropdown.

### Buy an SMS-capable Phone Number with Telnyx
To use the Messaging API, you’ll need an SMS-capable phone number purchased from, or ported into, the Telnyx platform. If purchasing a new number, select the SMS number feature when searching. In general, numbers that are ported in will be messaging-capable.

Within the Numbers section of the Telnyx Portal, you can search for, buy and provision new numbers – or port existing numbers.

### Create a Messaging Profile
Messaging Profiles are the simplest way for you to configure your inbound and outbound messaging settings. An SMS-capable phone number is SMS-enabled by assigning it to a Messaging Profile.

Configure your profile to send and receive SMS messages.

### Assign Your Phone Number to Your Messaging Profile
Once you’ve created a messaging profile, you’ll need to add your number to that profile.

## API Documentation
https://developers.telnyx.com/

## Known Issues and Limitations
None.

## Deployment Instructions
Review the [following documentation](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) for deploying the Telnyx connector as a custom connector in Microsoft PowerAutomate and Power Apps