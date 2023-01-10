# Proposal for WebEx Custom Connector using WebEx API # 

Team 18 from UCL Computer Science propose to build a custom connector for WebEx while working alongside the NHS.

Our aim is to allow doctors to create meetings via PowerApp and send them via SMS or WhatsApp to the patients for remote diagnosis session, which would make the process simpler. 

## Publisher: University College London ##

## Prerequisites ##
WebEx account
Register as a WebEx Developer 
Create a WebEx Integration: Go to https://developer.webex.com/my-apps/new > Create Integration.

## Main API Call ##

POST /v1/meetings
{
    "title": "Example Daily Meeting",
    "agenda": "Example Agenda",
    "password": "BgJep@43",
    "start": "2019-11-01 20:00:00",
    "end": "2019-11-01 21:00:00",
    "timezone": "Asia/Shanghai",
    "recurrence": "FREQ=DAILY;INTERVAL=1;COUNT=10",
    "enabledAutoRecordMeeting": false,
    "allowAnyUserToBeCoHost": false,
    "enabledJoinBeforeHost": false,
    "enableConnectAudioBeforeHost": false,
    "joinBeforeHostMinutes": 0,
    "excludePassword": false,
    "publicMeeting": false,
    "reminderTime": 10,
    "unlockedMeetingJoinSecurity": "allowJoin",
    "enableAutomaticLock": false,
    "automaticLockMinutes": 0,
    "allowFirstUserToBeCoHost": false,
    "allowAuthenticatedDevices": false,
    "invitees": [
        {
            "email": "john.andersen@example.com",
            "displayName": "John Andersen",
            "coHost": false
        },
        {
            "email": "brenda.song@example.com",
            "displayName": "Brenda Song",
            "coHost": false
        }
    ],
    "sendEmail": true,
    "hostEmail": "john.andersen@example.com",
    "siteUrl": "site4-example.webex.com",
    "registration": {
        "requireFirstName": "true",
        "requireLastName": "true",
        "requireEmail": "true",
        "requireCompanyName": "true",
        "requireCountryRegion": "false",
        "requireWorkPhone": "true",
        "customizedQuestions": [
            {
                "question": "What color do you like?",
                "required": true,
                "type": "checkbox",
                "options": [
                    {
                        "value": "green"
                    },
                    {
                        "value": "black"
                    },
                    {
                        "value": "yellow"
                    },
                    {
                        "value": "red"
                    }
                ],
                "rules": [
                    {
                        "condition": "notEquals",
                        "value": "red",
                        "result": "reject",
                        "matchCase": "true",
                        "order": 2
                    }
                ]
            },
            {
                "question": "Project Team",
                "type": "singleLineTextBox",
                "maxLength": 120
            },
            {
                "question": "How are you",
                "type": "multiLineTextBox"
            },
            {
                "question": "Dropdownlist Q",
                "type": "dropdownList",
                "options": [
                    {
                        "value": "A1"
                    },
                    {
                        "value": "A2"
                    }
                ]
            },
            {
                "question": "weather",
                "required": false,
                "type": "radioButtons",
                "maxLength": 120,
                "options": [
                    {
                        "value": "sunny"
                    },
                    {
                        "value": "rain"
                    }
                ]
            }
        ],
        "rules": [
            {
                "question": "lastName",
                "condition": "endsWith",
                "value": "tom",
                "result": "reject",
                "matchCase": "false",
                "order": 1
            }
        ]
    },
    "integrationTags": [
        "dbaeceebea5c4a63ac9d5ef1edfe36b9",
        "85e1d6319aa94c0583a6891280e3437d",
        "27226d1311b947f3a68d6bdf8e4e19a1"
    ],
    "simultaneousInterpretation": {
        "enabled": true,
        "interpreters": [
            {
                "languageCode1": "en",
                "languageCode2": "de",
                "email": "marcus.hoffmann@example.com",
                "displayName": "Hoffmann"
            },
            {
                "languageCode1": "en",
                "languageCode2": "fr",
                "email": "antoine.martin@example.com",
                "displayName": "Martin"
            }
        ]
    },
    "enabledBreakoutSessions": true,
    "breakoutSessions": [
        {
            "name": "Breakout Session 1",
            "invitees": [
                "rachel.green@example.com",
                "monica.geller@example.com"
            ]
        },
        {
            "name": "Breakout Session N",
            "invitees": [
                "ross.geller@example.com",
                "chandler.bing@example.com"
            ]
        }
    ],
    "trackingCodes": [
        {
            "name": "Department",
            "value": "Engineering"
        },
        {
            "name": "Division",
            "value": "Full-time"
        }
    ]
}


POST /v1/meetingInvitees
{
    "email": "john.andersen@example.com",
    "displayName": "John Andersen",
    "meetingId": "870f51ff287b41be84648412901e0402",
    "coHost": false,
    "hostEmail": "brenda.song@example.com",
    "panelist": false,
    "sendEmail": true
}

POST /v1/meetingInvitees
{
    "email": "john.andersen@example.com",
    "displayName": "John Andersen",
    "meetingId": "870f51ff287b41be84648412901e0402",
    "coHost": false,
    "hostEmail": "brenda.song@example.com",
    "panelist": false,
    "sendEmail": true
}

We will use this format of API call to the WebEx API in order to create remote diagnosis session meetings via PowerApp.

## Supported Operations ##

The endpoints will be the patient and the PowerApp where the nurse/GP will be able to create meetings and send out invites via email or WhatsApp.

## Obtaining credentials ##

The connection between the Power Platform and WebEx API would be made secure by the using Bearer Token, for which we have obtained the key, and intend to use it for every call made to the API. 
