# Microsoft School Data Sync CSV Upload

Simplify class management in Office 365. School Data Sync reads rosters from your SIS and creates classes and groups for Microsoft Teams, Intune for Education, and third party applications.

Connect to School Data Sync V2 to upload school and roster information, manage your sync profile.

## Supported Operations

The connector supports the following operations:

* `GetDelegatedToken`: Get the Delegated Token to communicate with School Data Sync.
* `CheckValidationResult`: Check to see if the validation result of the uploaded CSV files returned errors.
* `CallGetuploadsession`: Get the session for the CSV file upload.
* `GetDataconnectorList`: Get the DataConnector list.
* `CallValidate`: Call the operation to validate the uploaded CSV files.
