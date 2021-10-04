# Notion
Notion is an application that provides components such as notes, databases, kanban boards, wikis, calendars and reminders.These components and systems can be used individually, or in collaboration with others.

## Publisher
### Chandra Sekhar & Harshini Varma

## Prerequisites
You need to have a Notion [Personal/Enterprise account](https://www.notion.so/signup) click here for  [Pricing](https://www.notion.so/pricing).

## Supported Operations
### [Notion Operations](https://developers.notion.com/)
#### List all users
Returns a paginated list of Users for the workspace. 

#### Retrieve a block
Retrieves a Block object using the ID specified.

#### Retrieve a database
Retrieves a Database object using the ID specified.

#### Retrieve block children
Returns a paginated array of child block objects contained in the block using the ID specified. 

#### Retrieve a user
Retrieves a User using the ID specified.

#### Search
Searches all pages and child pages that are shared with the integration. (The results may include databases)

#### Update a block
Updates the content for the specified block_id based on the block type.



## Upcoming Planned Features
#### Query a Database (Planned)
Gets a list of Pages contained in the database, filtered and ordered according to the filter conditions and sort criteria provided in the request. 
 
#### Create a Database (Planned)
Creates a database as a subpage in the specified parent page, with the specified properties schema.
 
#### Update Database (Planned)
Updates an existing database as specified by the parameters.
 
#### Retrieve a page (Planned)
Retrieves a Page object using the ID specified.
 
#### Create a page (Planned)
Creates a new page in the specified database or as a child of an existing page.
 
#### Update page (Planned)
Updates page property values for the specified page. 
 
#### Append block children (Planned)
Creates and appends new children blocks to the parent block_id specified.
 
#### Delete a block (Planned)
Sets a Block object, including page blocks, to archived: true using the ID specified. 


## API Documentation
Visit [Start building with the Notion API ](https://developers.notion.com/) page for further details.

Visit [change log](https://developers.notion.com/changelog) for API Updates

## Known Issues and Limitations
To ensure a consistent developer experience for all API users, the Notion API is rate limited and basic size limits apply to request parameters.

### Rate Limit
* Rate-limited requests will return a "rate_limited" error code (HTTP response status 429).
* The rate limit for incoming requests is an average of 3 requests per second. Some bursts beyond the average rate are allowed.
* Integrations should accommodate variable rate limits by handling HTTP 429 responses and backing off (or slowing down) the speed of future requests.
* A common way to implement this is using one or several queues for pending requests, which can be consumed by sending requests as long as Notion does not respond with an HTTP 429.

### Size limits
Notion limits the size of certain parameters, and the depth of children in requests.
* A requests that exceeds any of these limits will return "validation_error" error.
* In addition to the property limits given, payloads have a maximum size of 1000 block elements and 500kb.

### Request size limits

These limits apply to requests sent to Notion's API only. There are different limits on the number of relations and people mentions returned by the API.

#### Not all operations provided by Notion are part of the first IP connector submission. We will keep adding/updating/supporting this connector based on your feedback/requests :)


<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.