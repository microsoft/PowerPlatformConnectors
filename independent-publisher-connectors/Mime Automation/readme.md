# Mime Automation
The Mime Automation connector helps you to extract attachments in a Base-64 formated string together with its file name, which is originating from a Base-64-encoded TNEF or EML file string.

## Publisher: Publisher's Name
Andreas Cieslik, AC Cloud Solutions

## Prerequisites
Required. Any plans or licenses, tools required from the connector.​

## Supported Operations
### Extract files from a TNEF-encoded file
For the "Content" parameter you need to provide a Base64-encoded TNEF file string.
The API service will then extract existing files in an JSON array with file name, mime type and content parameters.
The content will also be a base-64 encoded string of the actual file.
### Extract files from a EML-encoded file
For the "Content" parameter you need to provide a Base64-encoded EML file string.
The API service will then extract existing files in an JSON array with file name, mime type and content parameters.
The content will also be a base-64 encoded string of the actual file.

## Obtaining Credentials
Please go this web site and subscribe to the required plan in the Products => APIs section:
https://www.accloudsolutions.com

Once you have signed up and subscribed to a plan you will get an API key in your portal overview.

When you use this connector you need to provide that API key for the connection to be configured.

## Getting Started
### TNEF Attachment Extraction
Transport Neutral Encapsulation Format or TNEF is a proprietary email attachment format used by Microsoft Outlook and Microsoft Exchange Server. An attached file with TNEF encoding is most often named winmail.dat or win.dat, and has a MIME type of Application/MS-TNEF. The official (IANA) media type, however, is application/vnd.ms-tnef.

Use case:

Some email clients or third party applications that receive emails cannot interpret the TNEF-encoded file that comes from users that are using Microsoft Outlook as their main email client to send their emails with having the TNEF feature activated. With a custom implementation /business process flow that is using our API it would be still possible to parse out and extract the attachted documents in order to store these files on your client or in your third party application.

More information on how Microsoft Outlook can be configured in this regard here:
https://support.microsoft.com/en-us/topic/how-email-message-formats-affect-internet-email-messages-in-outlook-3b2c0536-c1c0-1d68-19f0-8cae13c26722

This API endpoint expects in the request body a JSON with a "content" property
```json
{
    content: "BASE-64 encoded string of a TNEF file"
}
```
For quick testing you can convert your winmail.dat (TNEF encoded file) into a base-64 encoded string with online converter:

https://base64.guru/converter/encode/file

Otherwise you need to implement this conversion logic in your client prior to calling this API endpoint.

As a result this endpoint will return base-64 encoded files and its original file names.

The result schema will be like this:
```json
[
    {
        fileName:"mypicture.jpg",
        content:"/9j/4AAQSkZJRgABAgAAZABkAAD/7AARRHVja3kAAQAEAAAAPAAA/+4AJkFkb2JlAGTAAAAAAQMAFQQDBgoNAAADiAAABUwAAAfnAAALd//bAI..."
    },
    {
        fileName:"readme.txt",
        content:"/9j/4AAQSkZJRgABAgAAZABkAAD/7AARRHVja3kAAQAEAAAAPAAA/+4AJkFkb2JlAGTAAAAAAQMAFQQDBgoNAAADiAAABUwAAAfnAAALd//bAI..."
    }
]
```
To quickly check and convert the result base-64 strings back you could also use this online converter tool:
https://base64.guru/converter/decode/file


### EML Attachment Extraction

Use case

In your business your receive eml files and you want to implement an automation process to automatically process any email attached file(s) for further business logics or for archiving purposes. Then this is the right API for you.

What is the EML file format?

The EML file format is a common file type used for storing email messages. EML stands for "Electronic Mail" and it is a file format typically associated with email clients and systems. It contains the complete content of an email, including the sender and recipient information, subject, message body, attachments, and any other relevant metadata. EML files are usually plain text files with a specific structure that allows them to be easily parsed and interpreted by email applications. They can be opened and viewed by various email clients or imported into different email programs for further use or archiving purposes.

Sample request schema:

This API endpoint expects in the request body a JSON with a "content" property
```json
{
    content: "BASE-64 encoded string of an EML file"
}
```
For quick testing you can convert your *.eml file into a base-64 encoded string with an online converter:

https://base64.guru/converter/encode/file

Otherwise you need to implement this conversion logic in your client prior to calling this API endpoint.
In Microsoft's PowerAutomate or FX either you will get the file content already in base64 or there are functions for conversion available.

As a result this endpoint will return base-64 encoded files and its original file names and the mime (file) type.

Sample result schema:
```json
[
    {
        fileName:"mypicture.jpg",
        mimeType:"image/jpeg",
        content:"/9j/4AAQSkZJRgABAgAAZABkAAD/7AARRHVja3kAAQAEAAAAPAAA/+4AJkFkb2JlAGTAAAAAAQMAFQQDBgoNAAADiAAABUwAAAfnAAALd//bAI…",
    },
    {
        fileName:"readme.txt",
        mimeType:"text/plain",
        content:"/9j/4AAQSkZJRgABAgAAZABkAAD/7AARRHVja3kAAQAEAAAAPAAA/+4AJkFkb2JlAGTAAAAAAQMAFQQDBgoNAAADiAAABUwAAAfnAAALd//bAI…"
    }
]
```
To quickly check and convert the result base-64 strings back you could also use this online converter tool:
https://base64.guru/converter/decode/file


## Known Issues and Limitations
There are no known issues at this time.

