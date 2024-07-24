# Open Data Network

The Open Data Network connector enables users to access and utilize government data to improve decision-making and enrich lives. By leveraging data from various government sources, this connector allows developers, businesses, and public sector organizations to integrate valuable information into their applications and services, driving open data standards and best practices. The system has over 700+ Federal, State, and Local agencies contributing data and hosts tens of thousands of datasets. Examples of contributing agencies include the Centers for Disease Control and Prevention (CDC), Environmental Protection Agency (EPA), Federal Bureau of Investigation (FBI), and National Aeronautics and Space Administration (NASA).

## Publisher: Richard Wilson

## Prerequisites

To use this connector, you must have an API key or App Token. You can obtain an API key by creating an account on the Open Data Network website and requesting access to the API. Please see the "Obtaining Credentials" section for detailed instructions on generating App Tokens and API Keys.

## Supported Operations

### Search Catalog

Search for open data network assets using various query parameters.

- **Inputs**:

  | Name               | Description                                                                                           |
  |--------------------|-------------------------------------------------------------------------------------------------------|
  | `Id`               | The unique identifier of an asset                                                                     |
  | `Domain`           | The domain name                                                                                       |
  | `Name`             | The case-insensitive asset name                                                                       |
  | `Category`         | A single category                                                                                     |
  | `Tag`              | A single tag                                                                                          |
  | `Only`             | One of the asset types (e.g., dataset, chart, map)                                                    |
  | `Attribution`      | The case-sensitive name of the attributing entity                                                     |
  | `License`          | The case-sensitive license name                                                                       |
  | `Full-Text Search Query` | Search for assets by any text in their name, description, category, tags, or other fields       |
  | `Min Should Match` | The number or percent of words that must match                                                        |
  | `Parent Id`        | The unique identifier of a parent asset                                                               |
  | `Derived From`     | The unique identifier of an asset derived from another                                                |
  | `Provenance`       | The provenance, either 'official' or 'community'                                                      |
  | `For User`         | The unique identifier of a user or a team                                                             |
  | `Shared To`        | The unique identifier of a user or team to whom assets are shared                                     |
  | `Column Name`      | The name of a column                                                                                  |
  | `Public`           | A true or false value for public or private assets                                                    |
  | `Visibility`       | The visibility status, either 'open' or 'internal'                                                    |
  | `Audience`         | The audience, either 'private', 'site' or 'public'                                                    |
  | `Published`        | A true or false value for published or unpublished assets                                             |
  | `Approval Status`  | The approval status (pending, rejected, approved, not_ready)                                          |
  | `Explicitly Hidden`| A true or false value for hidden or unhidden assets                                                   |
  | `Data Json Hidden` | A true or false value for assets hidden or unhidden from data.json catalog                            |
  | `Derived`          | A true or false value for derived or base assets                                                      |
  | `Order`            | The sort order of the results                                                                         |
  | `Offset`           | Initial starting point for paging (0 by default)                                                      |
  | `Limit`            | Number of results to return (100 by default, up to 10000)                                             |
  | `Scroll Id`        | The identifier for the last asset from the previous results                                           |
  | `Boost Official`   | Multiplier for the relevance score of official documents                                              |
  | `Show Visibility`  | Whether to return visibility information                                                              |

- **Outputs**:

  Returns a list of open data network assets matching the search criteria, including asset details such as name, description, and metadata.

### Search Dataset with SoQL

Retrieve data from a dataset using SoQL query.

- **Inputs**:

  | Name                | Description                                                      |
  |---------------------|------------------------------------------------------------------|
  | `Domain`            | Dataset publishers                                               |
  | `Dataset`           | The unique identifier of the dataset resource                    |
  | `Limit`             | The number of records to return                                  |
  | `Offset`            | The offset for pagination                                        |
  | `Select`            | The set of columns to be returned, similar to a SELECT in SQL    |
  | `Where`             | Filters the rows to be returned, similar to WHERE                |
  | `Order`             | Column to order results on, similar to ORDER BY in SQL           |
  | `Group`             | Column to group results on, similar to GROUP BY in SQL           |
  | `Having`            | Filters the rows that result from an aggregation, similar to HAVING |
  | `Full Text Search`  | Performs a full text search for a value                          |
  | `Full SoQL Query`   | A full SoQL query string, all as one parameter                   |

- **Outputs**:

  Returns the data from the specified dataset resource, based on the SoQL query parameters.

### Get Search Data

Get data for search fields.

- **Inputs**:

  | Name      | Description                                        |
  |-----------|----------------------------------------------------|
  | `Domain`  | The domain of the dataset                          |
  | `Dataset` | The unique identifier of the dataset resource      |
  | `Query`   | The search query to get values                     |
  | `Limit`   | The limit of values to return                      |

- **Outputs**:

  Returns search data values for the specified query parameters.

## Obtaining Credentials

### Generating App Tokens and API Keys

#### What is an App Token?

An Application Token is an alphanumeric string that authorizes you to create an application. App tokens can be used as part of the authentication process to perform read operations through the API. Data & Insights users can leverage the app tokens to reach out to users in case their application is causing too many calls per unit time, and they need to be throttled.

#### What is an API Key?

API Keys can be used to perform read, write, and delete operations through the API. These operations will be available to each user according to their role on the domain they are accessing. An API Key comes with a secret key, which together serve as a proxy for a user's username and password. The advantage of using an API Key + Secret Key is that it allows a user to authenticate without showing their username, and it will not change as the user's Data & Insights password changes.

#### Obtaining an App Token and API Keys

1. Start by logging into your Data & Insights account on any Data & Insights domain, such as [evergreen.data.socrata.com](https://evergreen.data.socrata.com).
2. Navigate to your profile page by selecting the rightmost icon on the header bar.
3. Click on "Your Profile" and select the pencil icon to edit your profile.
4. In the profile settings, navigate to "Developer Settings" for API keys and app tokens.
5. Click "Create New App Token" and fill in the sections for Name and Description. For example, if you will be using this token to authorize automated DataSync updates, you could call the app token “Your Name - DataSync token” and as a description enter “App token used for updating datasets on ‘x’ domain”.

Please note: Your Application Name must be unique across all registered applications on all Data & Insights domains.

## Known Issues and Limitations

Currently, no known issues or limitations exist. Always refer to this section for updated information.
