# Secret Server

Delinea Secret Server is an enterprise-grade, privileged access management solution that is quickly deployable and easily managed. With Secret Server, you can automatically discover and manage your privileged accounts through an intuitive interface, protecting against malicious activity, enterprise-wide.

## Prerequisites

* Install Secret Server using this [link](https://docs.delinea.com/secrets/current/getting-started-tutorial/2-installation)
* Get valid license using this [link](https://docs.delinea.com/secrets/current/secret-server-setup/licensing/index.md)
* Create a [service account](https://docs.delinea.com/secrets/current/api-scripting/sdk-cli/index.md#setup_procedure) on a Secret Server.
* [Create a Secret](https://docs.delinea.com/secrets/current/secret-management/procedures/creating-secrets/index.md) in a Secret Server.

## Deploy and Configure the Connector

* Use CLI tool **paconn** to create a new custom connector.
* Run the following commands

  `paconn create --api-prop apiProperties.json --api-def apiDefinition.swagger.json`
* Configure the Secret server URL in Custom Connector.

## Supported Operations

The connector supports the following operations:

1. [GetToken](https://updates.thycotic.net/secretserver/restapiguide/11.1.7/OAuth/#operation/OAuth2Service_Authorize): Retrieve an access token for use with other API requests or refresh an access token.
1. [GetSecretById](https://updates.thycotic.net/secretserver/restapiguide/11.1.7/TokenAuth/#operation/SecretsService_GetSecret): Get a single secret by ID.
1. [GetTemplateById](https://updates.thycotic.net/secretserver/restapiguide/11.1.7/TokenAuth/#operation/SecretTemplatesService_GetV2): Get a single secret template by ID.
