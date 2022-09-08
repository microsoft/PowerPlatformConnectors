# CloudConvert

[CloudConvert](https://cloudconvert.com) is a file converter service - more than 200 different audio, video, document, ebook, archive, image,
spreadsheet and presentation formats supported. The connector allows to convert files, compress files and create website
screenshots using the CloudConvert service.

## Publisher: Lunaweb GmbH

## Prerequisites

In order to use this connector you require either a free or paid [account](https://cloudconvert.com/register) for CloudConvert.

## Supported Operations

### Convert File

Convert one input file to another output format.
To show all advanced conversion options, bot the **Input Format** and **Output Format** parameters need to be selected.

### Compress File

Reduce and optimize the file size of PDF, PNG and JPG files.

### Create Website Screenshot

Capture a screenshot from an URL as PDF, PNG or JPG.

### Merge Files

Merge at least 2 files into one PDF. Any input format which is supported by CloudConvert can be merged.

## Obtaining Credentials
A CloudConvert account (free or paid) is required to register the OAuth Client. To get a Client ID and Client Secret, follow these steps:

1. Go to your [Dashboard](https://cloudconvert.com/dashboard)
2. Go to *Authorization* -> *OAuth clients*
3. Click *Create New Client*
4. Enter a Name and `https://global.consent.azure-apim.net/redirect` as redirect URL.
5. Your **Client ID** and **Client Secret** will show up.

## Known Issues and Limitations

* The file size of the input file is currently limited to 50MB.

## API Documentation
https://cloudconvert.com/api/v2

## Deployment Instructions
Run the following commands and follow the prompts:
```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --icon icon.png
```