# Mobsim Send SMS
Communicate with your customers in an assertive and direct way using the "MOBSIM - Send SMS" Connector. SMS - with high delivery and opening rates - makes it possible to send promotional and transactional campaigns, alerts, reminders, notifications, invites and more, allowing for a complete experience with personalized messages and the convenience of scheduled deliveries to mobile phone users.

## Publisher: MOBSIM Comunicação Mobile SMS

## Prerequisites
All requests must include an access key, obtained through the top menu "Customer Name" > "Profile" > "API Key" field.

## Supported Operations
Send SMS: Send SMS messages through your MOBSIM account

### Operation 1
Send SMS

## Obtaining Credentials
The first step to obtain the API KEY is to firm a commercial contract with prepaid or postpaid payment with **MOBSIM COMUNICAÇÃO MOBILE**.

It's necessary to manifest the wish to use the API, so that the necessary settings are applied. The contact must be made through
the e-mail "contato@mobsim.com.br" or through MOBSIM's Company WhatsApp +55(11)93735-2809 (Number subjected to change). After hiring the **MOBSIM** services, the 
Technical Team will create an account in the **MOBSIM Manager** system, and send the credentials through e-mail.

## Known Issues and Limitations
It's possible to generate up to 10 send threads at the same time, generating a perfomance up to 10.000 sends per requisition.

We recommend that there be a spacing of 30 seconds between each batch of 10 send threads of 1.000 numbers, or, for better understanding, 20.000 SMS/Minute.

Individual calls with only 1 number per requisition are recommended to be made up to 1.500 send threads per minute.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate.