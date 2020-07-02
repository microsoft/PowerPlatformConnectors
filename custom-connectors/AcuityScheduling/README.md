
### NOTE
> This is a *sample* connector.  The connector is provided here with the intent to illustrate certain features and functionality around connectors.

## Acuity Scheduling (Sample) Connector
The Acuity Scheduling sample connector enables you to create new clients and view blocked times in your calendar

## Prerequisites
You will need the following:
* An Acuity Scheduling **Powerhouse** account (Powerhouse is required to access the Acuity Scheduling API) `https://acuityscheduling.com/signup.php`
* Your [User ID and API Key](https://secure.acuityscheduling.com/app.php?action=settings&key=api)
* The [Power Platform CLI Tool](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli)

## Downloading & Deploying the Connector
1. Clone the PowerPlatformConnectors GitHub repository
2. Open a terminal, then change to the `AcuityScheduling` directory, found in `samples` of the cloned repository
3. Run `paconn login`, then follow the authentication steps
4. Once authenticated, run `paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json`
5. Select the target environment for your connector
6. Create a new flow in Power Automate, or a new Power App, using the connector. When prompted, create a new connection with your [User ID and API Key](https://secure.acuityscheduling.com/app.php?action=settings&key=api). Your **UserID** will be the username, and your **API Key** will be the password. 


## Supported Actions
The Acuity Scheduling sample connector currently supports the following actions:
* `CreateNewClient`:  Create a new client in your client list
    - See supported parameters [here](https://developers.acuityscheduling.com/reference#post-clients)
* `GetBlockedTimes`: Get the blocked times on your Acuity calendar(s)
    - See supported parameters [here](https://developers.acuityscheduling.com/reference#blocks)

