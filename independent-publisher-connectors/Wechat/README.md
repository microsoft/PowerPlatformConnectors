# Proposal for WeChat Custom Connector
Team 18 from UCL Computer Science propose to build a custom connector for Wechat while building a reminder solution for Medical and Health Services in Asia.

Our aim is to provide an easy implementation for Medical and Health Services in Asia to automatically send out reminders to patients for their upcoming doctor’s appointment via Wechat Official Account using Power Automate. Our Power App will be designed to provide insights about appointments. The App would also allow nurses to send e-prescription reference numbers to the patients via Wechat Official acccont and allows the patient to show it in pharmacies.

### Publisher: University College London

### Prerequisites
Wechat Official account <br>
Get Wechat Official account verified <br>
Template for reminder and e-prescription messages

### Main API Call

POST /v1/messages <br>
{
"touser":"OPENID",
"template_id":"templateidtypedhere",
"url":"http://weixin.qq.com/download",  
"client_msg_id":"MSG_000001",
"data":{
"first": {
"value":"main_message",
"color":"#173177"
},
"keyword1":{
"value":"appintmentID_or_referenceID",
"color":"#173177"
},
"keyword2": {
"value":"date_and_time",
"color":"#173177"
},
"keyword3": {
"value":"hospital_name",
"color":"#173177"
},
"remark":{
"value":"doctor_name！",
"color":"#173177"
}
}
}
<br>
<br>
We will use this format of API call to the Wechat Official Account Messaging API in order to send a message from the Power Platform to the patient via Wechat.

### Supported Operations
The endpoints will be the Wechat account of the patient (the number or account associated with the patient in the Medical and Health Services database) and the PowerApp where the nurse will be able to view the sent messages and messages to be sent. The actions will include sending a variety of formatted messages from the Medical and Health Services official Wechat account to the patients associated number which will include the patient’s appointment date, type, location and so on.

### Operation 1
The trigger to the automatic messaging system would be a query to database of the Medical and Health services in Asia which would automatically send messages everyday when it is a week to the appointment. These Wechat reminders would be a good way of reminding the patients for their upcoming appointment. Sending such reminders will trigger webhook http request to the PowerApp and allow logging of what messages were sent and when.

### Operation 2
The trigger for the prescription sharing facility would be the input and submission of an input form on the Power App by healthcare staff. This message would then be sent across to the patient’s phone using Wechat and would hence be a way for the patient to show the reference code to the pharmacy to get their medicine through a simpler process.

### Obtaining credentials
The connection between the Power Platform and Wechat API would be made secure by the use of OAuth 2.0, for which we have obtained the key, and intend to use it for every call made to the API.

We have made an authorisation request to Wechat (the resource owner) and have been granted this. We will then use this authorisation grant (our OAuth key) to make the relevant calls to the Wechat API which will allows us to use their resources every time they successfully check and validate our OAuth key.

