# Loripsum
Loripsum.net has an API to generate placeholder text. Sometimes you need lists, headings, long paragraphs, etc. Loripsum.net uses the full text of Cicero's "De finibus bonorum et malorum" to make sure you get a different placeholder text every single time.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
There are no prerequisites needed for this connector.

## Obtaining Credentials
This connector does not use authentication, so no credentials are needed.

## Supported Operations
### Get some placeholder text
Get some placeholder text. Additional parameters can be used to specify the output, using a '%2F'-separated string:
- integer - The number of paragraphs to generate.
- short, medium, long, verylong - The average length of a paragraph.
- decorate - Add bold, italic and marked text.
- link - Add links.
- ul - Add unordered lists.
- ol - Add numbered lists.
- dl - Add description lists.
- bq - Add blockquotes.
- code - Add code samples.
- headers - Add headers.
- allcaps - Use ALL CAPS.
- prude - Prude version.
- plaintext - Return plain text, no HTML.

## Known Issues and Limitations
There are no known issues at this time.
