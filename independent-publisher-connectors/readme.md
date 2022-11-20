# Proposal for WhatsApp Custom Connector using WhatsApp Cloud API # 

Team 18 from UCL Computer Science propose to build a custom connector for WhatsApp while working alongside the NHS.

Our aim is to allow the NHS to automatically send out reminders to patients for their upcoming doctor’s appointment on WhatsApp using Power Automate.  Our Power App will be designed such that it provides insights about appointments but also allows nurses to send the e-prescriptions reference number sent out to the pharmacies, to the patient through WhatsApp. 

## Publisher: University College London ##

## Prerequisites ##
WhatsApp account
Register as a Meta Developer 
Enable two-factor authentication for your account 
Create a Meta App: Go to developers.facebook.com > My Apps > Create App. Select the "Business" type and follow the prompts on your screen.


## Main API Call ##

POST /v1/messages
{
    "preview_url": false | true,
    "recipient_type": "individual",
    "to": "whatsapp-id",
    "type": "text",
    "text": {
        "body": "your-text-message-content"
    }
}

We will use this format of API call to the WhatsApp Cloud API in order to send a message from the Power Platform to the patient.

## Supported Operations ##

The endpoints will be the WhatsApp account of the patient (the number associated with the patient in the NHS database) and the PowerApp where the nurse will be able to view the sent messages and messages to be sent. The actions will include sending a variety of formatted messages from the NHS business account to the patients associated number which will include the patient’s appointment date, type, location and so on.

## Operation 1 ##

The trigger to the automatic messaging system would be a query to the NHS database which would cause messages to be sent daily whenever the appointment is within a week. These WhatsApp notifications would be a good way of reminding the patients of their upcoming appointment. Sending such reminders will trigger webhook http request to the PowerApp and allow logging of what messages were sent and when.

## Operation 2 ##

The trigger for the prescription sharing facility would be the input and submission of an input form on the Power App by the NHS staff. This message would then be sent across to the patient’s phone using WhatsApp and would hence be a way for the patient to know where to find their prescription reference and when they go to the pharmacy.

## Obtaining credentials ##

The connection between the Power Platform and WhatsApp API would be made secure by the use of OAuth 2.0, for which we have obtained the key, and intend to use it for every call made to the API. 

We have made an authorisation request to WhatsApp/Meta (the resource owner) and have been granted this. We will then use this authorisation grant (our OAuth key) to make the relevant calls to the WhatsApp API which will allows us to use their resources every time they successfully check and validate our OAuth key.
