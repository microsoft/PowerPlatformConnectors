# Microsoft School Data Sync CSV Upload

Simplify class management in Office 365. School Data Sync reads rosters from your SIS and creates classes and groups for Microsoft Teams, Intune for Education, and third party applications.

Connect to School Data Sync V2 to upload school and roster information, manage your sync profile.

## Publisher: Microsoft

## Prerequisites

Create an app in the tenant's Azure Active Directory with Edu permissions for rostering.​

## Supported Operations

### GetDelegatedToken

Get the Delegated Token to communicate with School Data Sync.

### GetDataconnectorList

Get the DataConnector list.

### CallGetuploadsession

Get the session for the CSV file upload.

### CallValidate

Call the operation to validate the uploaded CSV files.

### CheckValidationResult

Check to see if the validation result of the uploaded CSV files returned errors.

## Obtaining Credentials

Create an app in the tenant's Azure Active Directory and generate a secrect.

## Known Issues and Limitations

N/A

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps
