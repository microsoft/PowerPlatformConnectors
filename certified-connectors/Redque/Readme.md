# Redque
Redque provides a powerful REST API.  Using this API, you can manage your documents, accounts, folders, enums and configurations in a Redque system.  Very often, you may want to leverage those functions in your accounting application or in your process automation.

## Publisher: Redque s.r.o.

## Prerequisites

You will need the following to proceed:

* A Redque subscription or Redque demo account

## Getting started

* Prepare your Redque account or create a new one at https://register.redque.com

## Obtaining Credentials

You can obtain credentials by creating an account at [Redque registration](https://register.redque.com/)

## Deployment Instructions

* In the [Power Automate portal](https://make.powerautomate.com/), create a new flow or edit an existing one. 

* Add a new action to your flow and in the "Choose an operation" menu, search for "Redque" under the Premium tab. 

* Select your preferred action.

* You will be prompted to supply a Username and Password you registered before. This Connection will be saved by Power Automate, and available for use in future flows.

* That's it! You can now use Redque actions in all your Power Automate flows. 

## Supported Operations

The connector supports the following operations:

### Configuration

* `Return document to issuer config`: Retrieve current configuration for return to sender action

* `Update template`: Update template for return o issuer function

### Documents

* `Get image`: Gets link to an image of a given page

* `Return to issuer`: Returns document to the issuer for corrections

* `List documents`: List filtered documents based on parameters

* `Export document`: Export document to accounting provider

* `Download document`: Downloads document's file or gets URL

* `Get document's metadata`: Gets document's metadata

* `Update document`: Updates document

* `Lock document`: Locks (opens) document preventing other users from modifying it

* `Unlock document`: Unlocks locked (closes) document to allow other users to modify its contents

* `Grant access`: Grants document access to specified user

* `Revoke access`: Removes access to the document for specified user

* `Upload document`: Uploads the document and schedules it for processing

### Enums

* `Return enums`: Returns list of all enums

* `Create new enum`: Creates new enum

* `Return specific enum`: Returns specific enum

* `Update specific enum`: Update specific enum

* `Deletes enum`: Deletes enum with specified ID

### Extraction

* `Extract document`: Requests extraction of a document. After document is retrieved it is scheduled for deletion

* `Get extracted document`: Extracted document is marked for removal and will be deleted within several hours

### Folder

* `Create folder`: Creates a new folder

* `Download folder`: Downloads all documents in a folder as a single archive

* `Remove folder`: Removes existing folder

* `Find folder`: Finds folder by ID

* `Update folder data`: Updates folder data

### Tenant

* `Get license`: Returns license information

* `Get token`: Returns current registration token or nothing if token is not active

* `Generate token`: Generates new registration token

* `Remove token`: Removes registration token

### Users

* `Filter users`: Gets the list of user by criteria

* `Change password`: Changes user's password

* `Change user's password`: Changes password for specified user. Requires user management permission

* `Delete user`: Deletes user

* `Find user`: Finds user

* `Update user`: Partially updates specified user

* `Add permissions`: Grants permissions

* `Remove permissions`: Revokes document permissions

## Frequently Asked Questions

### Does Redque connector support full maintenance of Redque system?

#### Yes, Redque connector includes all of the benefits of Redque API.

## Known Issues and Limitations

There are no knowing issues so far.