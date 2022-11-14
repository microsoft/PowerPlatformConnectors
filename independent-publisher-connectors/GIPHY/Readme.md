
## GIPHY
GIPHY API helps user to seamlessly integrate their apps with the largest GIF and Sticker Library in the world.

## Publisher
Priyanshu Srivastav<br>
*Email : priyanshusrivastav2002@gmail.com<br>*
*LinkedIn : https://www.linkedin.com/in/priyanshu-srivastav-b067241ba/*

## Prerequisites
You need to get the API key from [GIPHY Website](https://developers.giphy.com/). The API Key is available publicly.

### Deploying the sample
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json --secret <client_secret>
```

## Supported Operations
The connector supports the following operations:
* `Get-GIF`: Returns a list of GIFs matching the search string provided as input

## Parameters

parameter | required  | description
--- | --- | ---
`API Key` | Yes | The API Key for the GIPHY API
`q` | Yes | Search query term or phrase
`limit` | No | The maximum number of records to return
`offset` | No | An optional results offset
`rating` | No | Filters results by rating (g/pg/pg-13/r)
`lang` | No | Specify default country for regional content

## API Documentation
Visit [GIPHY API Reference](https://developers.giphy.com/docs/api#quick-start-guide) for further details.
