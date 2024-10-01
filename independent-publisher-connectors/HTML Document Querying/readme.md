## HTML Document Querying
This connector can query HTML documents. It used the HTML Agility Pack to interact with the provided HTML and uses XPath queries to extract information from the provided HTML document. The HTML document can also be downloaded from a HTTP address first.

## Publisher: Remy Blok

## Prerequisites
There are no prerequisities for this connector

## Supported Operations

### Execute Query to HTML document
Queries the provided HTML document with the provided xpath queries.

### Execute Query to downloaded HTML document
Downloads a HTML document from the provided URL. Then queries the downloaded HTML document with the provided xpath queries.

## Obtaining Credentials
Anonymous authentication

## Known Issues and Limitations
When downloading an HTML document is needs to be accessable without credentials. Just a plain HTTP get request need to result in the document. 