## ID Analyzer Connector

ID scan technology to extract and verify data from global ID documents including passport, driver license and identification card, verify remote user age and identity with our digital onboarding solutions.

## Prerequisites

You will need the following to proceed:

- [Sign Up](https://www.idanalyzer.com/signup.html) for ID Analyzer account.
- log into ID Analyzer's [web portal](https://portal.idanalyzer.com/) with your account details and click on `API Credentials` to get your private API key.

## Supported Operations

The connector supports the following operations:

#### Core API

- `Core API Setting`: Build your own identity verification application by uploading images of government-issued documents.

#### DocuPass API

- `Creating DocuPass Session`: Create a verification session for every user requiring identity verification.
- `Validating Callback`: To make sure the data was sent from our server.

#### Vault API

- `Get Vault Entry Content`: When entries are added to the vault, both Core API and DocuPass API will return a vault entry identifier named vaultid.
- `List and Filter Entries`: List all the entries stored inside the vault or search for entries with specific values.
- `Update Vault Entry`: Update the value of a single field or values of multiple fields in a vault entry.
- `Delete Entry From Vault`: Delete single or multiple vault entries.
- `Add Image To Vault`: Upload an image and add it to an existing vault entry.
- `Delete Image From Vault`: Delete an image inside a vault entry.
- `Create or update secret value`: Sets a secret in a specified key vault
- `Face Search`: Face search allows you to search the entire vault using an image of a person.
- `Face Search Training`: Before performing face search, you must issue a train command to train the document datasets inside your vault. You may also train your vault data through web portal. The training task is asynchronous, training time depends on the number of vault entries and images. It could take from several seconds to an hour.
- `Training Status`: Check whether vault training is still ongoing or has completed.

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.

## API documentation

API documentation can be found [here](https://developer.idanalyzer.com/index.html).

## Further Support

Feel free to write to us or give us a call for support or further information regarding our service. We will reply to you as soon as possible. But yes, it can take up to 24 hours. For all the support requests and general queries you can contact support@idanalyzer.com
or visit [contact-us](https://www.idanalyzer.com/contact.html).
