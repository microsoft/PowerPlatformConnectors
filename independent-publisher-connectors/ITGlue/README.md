# ITGlue
IT Glue is an IT documentation software for MSPs, co-managed IT teams, as well as IT departments. This connector uses your ITGlue API Key to access various resources.

## Publisher: Nirmal

## Prerequisites
To use this connector, you need the following

- API Key from ITGlue Portal
- Account Region base Url

The base URL for all endpoints and methods is: 

```
https://api.itglue.com
```

Partners with an account in the EU data center will use:

```
https://api.eu.itglue.com
```

Partners with an account in the Australia data center will use:

```
https://api.au.itglue.com
```


## Obtaining Credentials
- All requests require an API key that will be linked to your IT Glue account. Trying to access the API with an incorrect key will result in an HTTP 403 error.

To create one or more API keys, sign in to IT Glue with an Administrator role and generate your keys from Account > Settings > API Keys. Note that you can't view a key again after it has been generated, so store your keys as passwords in IT Glue or somewhere else safe. API keys do not expire, but they can be revoked anytime from Account > Settings > API Keys.

For more details, Please visit https://api.itglue.com/developer/

### Supported Operations

#### 1. List Organization Types

- Returns a list of organization types in your account.

 #### 2. Create an Organization Type

- Creates a new organization type in your account. Returns the created object if successful.

 #### 3. Get an Organization Type

- Returns the details of an existing organization type.

 #### 4. Update an Organization Type

- Updates an organization type in your account. Returns 422 Bad Request error if trying to update an externally synced record.

 #### 5. List Configuration Types

- Returns a list of configuration types in your account.

 #### 6. Create a Configuration Type

- Creates a new configuration type in your account. Returns the created object if successful.

 #### 7. Get a Configuration Type

- Returns the details of an existing configuration type.

 #### 8. Update a Configuration Type

- Updates a configuration type in your account. Returns 422 Bad Request error if trying to update an externally synced record.

 #### 9. List Configuration Statuses

- Returns a list of configuration statuses in your account.

 #### 10. Create a Configuration Status

- Creates a new configuration status in your account. Returns the created object if successful.

 #### 11. Get a Configuration Status

- Returns the details of an existing configuration status.

 #### 12. Update a Configuration Status

- Updates a configuration status in your account. Returns 422 Bad Request error if trying to update an externally synced record. - Success

 #### 13. List Countries

- Returns a list of supported countries.

 #### 14. Get a Country

- Returns the details of one of the supported countries."

 #### 15. List Flexible Asset Types

- Returns a list of flexible asset types in your account.

 #### 16. Create a Flexible Asset Type

- Creates a new flexible asset type in your account. 

 #### 17. Get a Flexible Asset Type

- Returns the details of an existing flexible asset type.

 #### 18. Update a Flexible Asset Type

- Updates the details of an existing flexible asset type in your account
 #### 19. List Domains

- Returns a list of domains for all organizations or for a specified organization.

 #### 20. List Flexible Asset Fields

- Returns a list of fields for the specified flexible asset type.

## Known Issues and Limitations
There are no known issues at time of publishing.

## Rate Limits
Rate limiting in the IT Glue API should be encountered only in rare circumstances. Currently, the limit is 10k requests per account per day and a variable limit of between 1 to 10 requests per second. A 429 Too Many Requests error code will be returned for rate limited requests.

## Deployment Instructions
Follow the instructions provided on the [Power Automate blog](https://flow.microsoft.com/en-us/blog/import-a-connector-from-github-as-a-custom-connector/).

