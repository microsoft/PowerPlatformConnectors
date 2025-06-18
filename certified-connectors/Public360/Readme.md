# Public 360 Connector

The Public 360° Connector empowers users to seamlessly archive cases and documents into the Public 360° system as part of automated workflows. It supports archiving content such as social media posts, Outlook items, or documents stored in OneDrive.

This connector is ideal for organizations aiming to integrate Public 360° with their digital recordkeeping and case management processes using tools like Microsoft Power Automate.

## Features

* Archive various types of content into Public 360° directly from cloud platforms.
* Automate case and document management using standardized actions.
* Ensure compliance with recordkeeping policies through consistent metadata and storage rules.

## Prerequisites

To use this connector, ensure the following requirements are met:
* An active Azure subscription
* An authorized user account in your Microsoft Entra ID
* Necessary permissions to access and archive data in Public 360°

## Supported Operations

The connector currently supports the following actions:
* `Create Case`: Registers a new case in Public 360°.
* `Create Document`: Uploads and archives a document to an existing case.
* `Create File`: Attaches a file to a document or stores it directly in Public 360°.

## Notes
* When using the Create Case operation, you should provide either the Responsible Person Id Number, Resposible Person Email or the Responsible Enterprise Number.
* Ensure metadata is correctly mapped for successful archiving.
* Network and authentication configurations (such as firewalls and token access) must allow secure communication with the Public 360° API.
* This connector is using SIF API for communication with Public 360. For more information, visit https://help.360online.com/ReleaseInformation/SIF%20APIs%20-%20Documentation.pdf
