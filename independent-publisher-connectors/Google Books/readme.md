# Google Books
Google Books provides and open and public library enabling searching information of various volumes while also providing a way to search within publications.

## Publisher: Fördős András

## Prerequisites
A google account is required. Follow official steps: [LINK](https://developers.google.com/books/docs/v1/getting_started#getaccount)

Get familiar with the query language for partial results: [LINK](https://developers.google.com/books/docs/v1/performance#partial-response)

## Obtaining Credentials
This connector uses API Key authentication. To create an API Key, follow the official documentation: [LINK](https://developers.google.com/books/docs/v1/using#APIKey)

## Supported Operations
### Get Volume
Get a specific volume information with the help of a unique ID.
### Search Volumes
Search through and within various volumes with the help of complex queries.

## Known Issues and Limitations

### User location based on IP

Google Books respects copyright, contract, and other legal restrictions associated with the end user's location. As a result, some users might not be able to access book content from certain countries. For example, certain books are "previewable" only in the United States; we omit such preview links for users in other countries. Therefore, the API results are restricted based on your server or client application's IP address.

### Private bookshelves

Current implementation of the custom connector only allows searching through the public volumes and information, not enabling the private "MyLibrary" features of Google Books or access to user data. 

### Limitations of the API itself

There are additional limitations applicable, but they are not limitations of the Custom Connector itself, rather of the underlying service. For example, maximum results allowed to be returned in a search query are 40.

Read more on [LINK](http://developers.google.com/books/docs/v1/using)
