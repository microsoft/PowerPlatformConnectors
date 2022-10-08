## The Open Movie Database
The OMDb API is a RESTful web service to obtain movie information, all content and images on the site are contributed and maintained by users.

## Publisher
Aaryan Arora

## Prerequisites
You need to get the API key from [ORB Website](http://www.omdbapi.com/) or [here](http://omdbapi.com/apikey.aspx). The API Key is available publicly.

### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

## Supported Operations
The connector supports the following operations:
* `Search By Title`: List keys in the specified vault
* `Search By ID`: Gets the public part of a stored key
* `Search by Keyword`: Creates a new key

## Parameters

parameter | required  | description
--- | --- | ---
`API Key` | Yes | The API Key for the OMDb API
`i` | No | A valid IMDb ID
`s` | No | Movie title to search for
`type` | No | Type of result to return
`page` | No | Page number to return

Please note while both "i" and "s" are optional at least one argument is required

## API Documentation
Visit [OMDb reference page](http://www.omdbapi.com/) for further details.

## Known Issues and Limitations
No Issues and Limitations yet.