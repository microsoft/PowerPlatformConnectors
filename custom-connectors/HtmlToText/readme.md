# Title
HtmlToText is a simple custom connector to convert HTML to plain text.

## Prerequisites

## Connector Documentation
The HtmlToText custom connector is an alternative to the [Content Conversion (Preview) connector](https://docs.microsoft.com/en-us/connectors/conversionservice/)
It is not meant to be reverse compatible with the Content Conversion (Preview) connector.
The HtmlToText custom connector by default will:
* Remove all table tags and content from HTML
* Replace br tags with newline.
* Replace br, div, header, p, hr, li, ol, ul blocks with newline.
* Remove HTML tags.

## Known Issues and Limitations

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.