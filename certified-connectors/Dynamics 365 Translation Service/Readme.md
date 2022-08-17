## Dynamics 365 Translation Service Connector

This connector provides actions for Translation, Regeneration, and Alignment powered by Dynamics 365 Translation service.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An account with access to Dynamics Lifecycle Services

### Deploying the sample

Refer to [this documention](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) for guidance on deploying this connector as a custom connector. 

You will need to register an application inside AAD. For guidance registering an application, see [this documention](https://docs.microsoft.com/en-us/graph/auth-register-app-v2).

API Permissions:
 * Dynamics Lifecycle services API
   *  user_impersonation scope is required


## Supported Operations
The connector supports the following (user-facing) operations:
* `Translate`: Submit a translation request.
* `Align`: Submit an alignment request.
* `Regenerate`: Submit a regeneration request. Returns the file content of the regeneration output. 
* `Retrieve`: Polls the status of a translation request.
* `Download Translation`: Downloads the output of a completed translation request.
* `Download Alignment`: Downloads the output of a completed alignment request.




