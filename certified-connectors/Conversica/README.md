# Conversica API Connector

The Conversica API facilitates the flow of Lead data from the Customer System to the Conversica Platform, as well as the flow of Lead Update and Message object data from the Conversica Platform back to the Customer System.

## Publisher:

Conversica, Inc

## Prerequisites

A Conversica account is required to use this connector.

## Authentication

### Method

The Conversica API Connector uses HTTPS basic access authentication for the secure transmission of data between the Customer System and the Conversica Platform endpoints.

### API Username & Password

When ready for development, the Customer may contact their Conversica technical account manager to receive a username and password for authentication to the Conversica endpoint.

## Supported Actions

- Create a lead

- Update a lead

## Lead Object

The Lead object includes data relevant for communication between the Conversica Assistant and the Customer’s Leads.
Some keys listed below are optional. However, Conversica recommends including as much information as possible for each Lead, in order to enhance the performance of your Assistant.

| Key            | Type     | Description                                                                                                                                                                     | Required |
| -------------- | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------- |
| apiVersion     | String   | The version number of the Conversica API in use Yes                                                                                                                             |
| id             | String   | The Lead’s unique ID in the Customer’s System (Source ID) Yes                                                                                                                   |
| conversationId | String   | For a particular Conversation, a list or filtered set of Leads (e.g. “campaign” in some CRMs) for the purpose of tailoring messaging to a more specific audience or use case No |
| firstName      | String   | The Lead’s first name, used in messaging Yes                                                                                                                                    |
| email          | String   | The Lead’s primary email address for messaging Yes                                                                                                                              |
| leadSource     | String   | The source of the Lead, as indicated in the Customer’s System and as used by Conversica for either reporting on Lead Source performance or other customizations Yes             |
| leadStatus     | String   | The status of the Lead, as indicated in the Customer’s System and as used by Conversica for intelligent Conversation management (e.g. “New,” “Sold,” or “Lost”) No              |
| optOut         | Boolean  | Indicates whether or not the Lead has opted out of all methods of marketing and contact (“global” opt-out) No                                                                   |
| repId          | String   | The Salesperson’s (Lead owner) unique ID in the Customer’s System No                                                                                                            |
| repName        | String   | The Salesperson’s (Lead owner) name, used in messaging Yes                                                                                                                      |
| clientId       | String   | Unique ID for the Customer in the Customer’s System (Required if the API integration will be used for multiple Conversica Customers) No                                         |
| lastName       | String   | The Lead’s last name No                                                                                                                                                         |
| homePhone      | String   | The Lead’s primary phone number No                                                                                                                                              |
| workPhone      | String   | The Lead’s work phone number No                                                                                                                                                 |
| cellPhone      | String   | The Lead’s cell phone number (Required for SMS) No                                                                                                                              |
| address        | String   | The Lead’s street address No                                                                                                                                                    |
| city           | String   | The Lead’s city No                                                                                                                                                              |
| state          | String   | The Lead’s state No                                                                                                                                                             |
| zip            | String   | The Lead’s zip code No                                                                                                                                                          |
| leadType       | String   | The type of the Lead, as indicated in the Customer’s System (e.g. “Internet,” “Web Form,” or “Phone-In”) No                                                                     |
| date           | Datetime | The date and time the Lead was created in the Customer’s System (UTC RFC 3339 format) No                                                                                        |
| smsOptOut      | Boolean  | Indicates whether or not the Lead has opted out of being contacted by SMS text messaging (Required for SMS Conversations) No                                                    |
| stopMessaging  | Boolean  | If set to “true,” the Assistant will stop listening for responses and will no longer message the Lead No                                                                        |
| skipToFollowup | Boolean  | If set to “true,” the Assistant will wait a few days before sending a follow-up message No                                                                                      |
| repEmail       | String   | The Salesperson’s (Lead owner) email address No                                                                                                                                 |
| smsOptIn       | Boolean  | If set to ‘true,’ indicates the Lead has accepted being contacted by SMS text messaging (Required for SMS Conversations) No                                                     |

## Conversica Support

For additional support,please contact your Conversica technical account manager.
If you do not yet have a technical account manager assigned, feel free to contact the team at apisupport@conversica.com.
