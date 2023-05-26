# SECIB

SECIB provides a REST API which can be used to manage your SECIB's data (such as your case files, your calendar, your documents, etc).

The SECIB connector uses that REST API to allow SECIB's users to automate their workflow. For instance, you can trigger a worklflow when a new case file is created in SECIB neo and then process this case file before sending an email notification with the case file's information to other SECIB's users.

## Prerequisites

You will need the following to proceed :

- A Microsoft Power Apps or Microsoft Power Automate plan enabling the use of Premium connectors
- A SECIB neo/air or a Gestisoft licence. Please go to <https://www.secib.fr/> for further information.

## Supported Triggers

The connector supports the following triggers :

### Case File

- `DossierCreated`: Triggers a flow when a case file is created in SECIB neo/air or in Gestisoft.

## Supported Operations

The connector supports the following operations :

### Case File

- `GetDossierById`: allows the recovery of a case file which is corresponding to an identifier specified in parameter.

### Law firm

- `GetCabinets`: Retrieving each accessible firm for the current user.

### View

- `(V1) GetViewByViewName`: Retrieve data from a view in the SECIB database.
- `(V2) GetViewByViewName`: Retrieve data from a view in the SECIB database, using a where clause.
- `GetViewDynamicSchema`: Retrieve the schema of a view in the SECIB database (operation used within GetViewByName to retrieve the dynamic schema of the operation's response).

### Webhook

- `DeleteWebhook`: Operation used to delete a webhook subscription.

### Time Spent

- `SaveTempsPasse`: Let a user encode a time spent which will be then saved into the application.

## Obtaining Credentials

Required. Explain the authentication method and how to get the credentials.​

The connector needs authentication with an Azure Active Directory account and your tenant must be granted with access to our web API. You can contact us at **support.flow@secib.fr** for more information.

## Known Issues and Limitations

Version 2.0 **doesn't support user authentication**. To put it another way, when you are using the SECIB connector this is **on behalf of a whole law firm**.

## Frequently Asked Questions

This section is empty. You can submit your questions to **support.flow@secib.fr**.

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Flow and PowerApps

Connect with SECIB Support to request Client Id and Client Secret for authorization

## Support

For any questions please contact SECIB Support [here](https://support.secib.fr/) or send us a mail at **support.flow@secib.fr**.
