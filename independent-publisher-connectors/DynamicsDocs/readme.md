# DynamicDocs
DynamicDocs provides a quick way to create bespoke PDF documents in bulk with the ability to include graphics and logic in the documents. Not only can you create good looking documents using LaTeX, you can also create custom charts and include conditional logic into the documents with custom built-in functions. This allows you to create truly dynamic documents which only depend on the data payload.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
You will need to sign up for an account with DynamicDocs.

## Obtaining Credentials
In your account dashboard, under Account Details you can generate an API key, also called Adv-Security-Token.

## Supported Operations
### Create a document from a template
Send dynamic data and receive back a protected URL link to download the document.

## Known Issues and Limitations
DynamicDocs uses AWS for document creation and storage. You will need to use HTTP actions to receive the status and download the document.
