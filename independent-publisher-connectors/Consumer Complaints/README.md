# Consumer Complaints

The Consumer Financial Protection Bureau, a U.S. government agency, ensures fair treatment by financial institutions. The Consumer Complaint Database collects and publishes complaints about financial products and services after company responses, confirmations, or 15 days. Complaints referred to other regulators, like those about institutions with assets under $10 billion, are not published. The database updates daily.

## Publisher: Richard Wilson

## Prerequisites

To use this connector, you must have an API key. You can obtain an API key by creating an account on the Consumer Financial Protection Bureau website and requesting access to the Consumer Complaint Database API.

## Supported Operations

### Search Consumer Complaints

Search the contents of the consumer complaint database.

- **Inputs**:

| Name                     | Description                                                                                       |
|--------------------------|---------------------------------------------------------------------------------------------------|
| Search Term              | Return results containing specific term.                                                          |
| Field                    | Specify which field is searched if "Search Term" has a value. Defaults to "Complaint What Happened".|
| Start Index              | Return results starting from a specific index, only if format parameter is not specified.         |
| Result Size              | Limit the size of the results.                                                                    |
| Sort Order               | Return results sorted in a particular order.                                                      |
| No Aggregations          | True means no aggregations will be included, False means aggregations will be included.           |
| No Highlight             | True means no highlighting will be included, False means highlighting will be included.           |
| Companies                | Filter the results to only return these companies.                                                |
| Public Responses         | Filter the results to only return these types of public response by the company.                  |
| Max Company Received Date| Return results with date < Max Company Received Date (i.e. 2017-03-04).                           |
| Min Company Received Date| Return results with date >= Min Company Received Date (i.e. 2017-03-04).                          |
| Company Responses        | Filter the results to only return these types of response by the company.                         |
| Consumer Consents        | Filter the results to only return these types of consent consumer provided.                       |
| Consumer Disputed        | Filter the results to only return the specified state of consumer disputed, i.e. yes, no.         |
| Max Date Received        | Return results with date < Max Date Received (i.e. 2017-03-04).                                   |
| Min Date Received        | Return results with date >= Min Date Received (i.e. 2017-03-04).                                  |
| Has Narrative            | Filter the results to only return the specified state of whether it has narrative in the complaint.|
| Issues                   | Filter the results to only return these types of issue and subissue.                              |
| Products                 | Filter the results to only return these types of product and subproduct.                          |
| States                   | Filter the results to only return these states.                                                   |
| Submission Methods       | Filter the results to only return these types of way consumers submitted their complaints.        |
| Tags                     | Filter the results to only return these types of tag.                                             |
| Timely Response          | Filter the results to show whether a response was timely.                                         |
| Zip Codes                | Filter the results to only return these zip codes.                                                |

- **Outputs**:

| Name         | Description                  |
|--------------|------------------------------|
| SearchResult | The search results, including hits and aggregations. |

### Suggest Possible Searches

Suggest additional search terms based upon filters.

- **Inputs**:

| Name          | Description                       |
|---------------|-----------------------------------|
| Result Size   | Limit the size of the results.    |
| Suggestion Text | Text to use for suggestions.    |

- **Outputs**:

| Name          | Description                 |
|---------------|-----------------------------|
| SuggestResult | Array of suggestions.       |

### Suggest Possible Companies

Provide a list of companies that match complaints in the database.

- **Inputs**:

| Name                     | Description                                                                                       |
|--------------------------|---------------------------------------------------------------------------------------------------|
| Suggestion Text          | Text to use for suggestions.                                                                      |
| Result Size              | Limit the size of the results.                                                                    |
| Public Responses         | Filter the results to only return these types of public response by the company.                  |
| Max Company Received Date| Return results with date < Max Company Received Date (i.e. 2017-03-04).                           |
| Min Company Received Date| Return results with date >= Min Company Received Date (i.e. 2017-03-04).                          |
| Company Responses        | Filter the results to only return these types of response by the company.                         |
| Consumer Consents        | Filter the results to only return these types of consent consumer provided.                       |
| Consumer Disputed        | Filter the results to only return the specified state of consumer disputed, i.e. yes, no.         |
| Max Date Received        | Return results with date < Max Date Received (i.e. 2017-03-04).                                   |
| Min Date Received        | Return results with date >= Min Date Received (i.e. 2017-03-04).                                  |
| Has Narrative            | Filter the results to only return the specified state of whether it has narrative in the complaint.|
| Issues                   | Filter the results to only return these types of issue and subissue.                              |
| Products                 | Filter the results to only return these types of product and subproduct.                          |
| States                   | Filter the results to only return these states.                                                   |
| Submission Methods       | Filter the results to only return these types of way consumers submitted their complaints.        |
| Tags                     | Filter the results to only return these types of tag.                                             |
| Timely Response          | Filter the results to show whether a response was timely.                                         |
| Zip Codes                | Filter the results to only return these zip codes.                                                |

- **Outputs**:

| Name          | Description                 |
|---------------|-----------------------------|
| SuggestResult | Array of suggestions.       |

### Suggest Possible Zip Codes

Provide a list of zip codes that match complaints in the database.

- **Inputs**:

| Name                     | Description                                                                                       |
|--------------------------|---------------------------------------------------------------------------------------------------|
| Suggestion Text          | Text to use for suggestions.                                                                      |
| Result Size              | Limit the size of the results.                                                                    |
| Public Responses         | Filter the results to only return these types of public response by the company.                  |
| Max Company Received Date| Return results with date < Max Company Received Date (i.e. 2017-03-04).                           |
| Min Company Received Date| Return results with date >= Min Company Received Date (i.e. 2017-03-04).                          |
| Company Responses        | Filter the results to only return these types of response by the company.                         |
| Consumer Consents        | Filter the results to only return these types of consent consumer provided.                       |
| Consumer Disputed        | Filter the results to only return the specified state of consumer disputed, i.e. yes, no.         |
| Max Date Received        | Return results with date < Max Date Received (i.e. 2017-03-04).                                   |
| Min Date Received        | Return results with date >= Min Date Received (i.e. 2017-03-04).                                  |
| Has Narrative            | Filter the results to only return the specified state of whether it has narrative in the complaint.|
| Issues                   | Filter the results to only return these types of issue and subissue.                              |
| Products                 | Filter the results to only return these types of product and subproduct.                          |
| States                   | Filter the results to only return these states.                                                   |
| Submission Methods       | Filter the results to only return these types of way consumers submitted their complaints.        |
| Tags                     | Filter the results to only return these types of tag.                                             |
| Timely Response          | Filter the results to show whether a response was timely.                                         |
| Zip Codes                | Filter the results to only return these zip codes.                                                |

- **Outputs**:

| Name          | Description                 |
|---------------|-----------------------------|
| SuggestResult | Array of suggestions.       |

### Get Consumer Complaint by ID

Get complaint details for a specific ID.

- **Inputs**:

| Name          | Description                 |
|---------------|-----------------------------|
| Complaint ID  | ID of the complaint.        |

- **Outputs**:

| Name         | Description                    |
|--------------|--------------------------------|
| Complaint    | The complaint details.         |

### Get the State by State Information

Get complaint information broken down by states.

- **Inputs**:

| Name                     | Description                                                                                       |
|--------------------------|---------------------------------------------------------------------------------------------------|
| Search Term              | Return results containing specific term.                                                          |
| Field                    | Specify which field is searched if "Search Term" has a value. Defaults to "Complaint What Happened".|
| Companies                | Filter the results to only return these companies.                                                |
| Public Responses         | Filter the results to only return these types of public response by the company.                  |
| Max Company Received Date| Return results with date < Max Company Received Date (i.e. 2017-03-04).                           |
| Min Company Received Date| Return results with date >= Min Company Received Date (i.e. 2017-03-04).                          |
| Company Responses        | Filter the results to only return these types of response by the company.                         |
| Consumer Consents        | Filter the results to only return these types of consent consumer provided.                       |
| Consumer Disputed        | Filter the results to only return the specified state of consumer disputed, i.e. yes, no.         |
| Max Date Received        | Return results with date < Max Date Received (i.e. 2017-03-04).                                   |
| Min Date Received        | Return results with date >= Min Date Received (i.e. 2017-03-04).                                  |
| Has Narrative            | Filter the results to only return the specified state of whether it has narrative in the complaint.|
| Issues                   | Filter the results to only return these types of issue and subissue.                              |
| Products                 | Filter the results to only return these types of product and subproduct.                          |
| States                   | Filter the results to only return these states.                                                   |
| Submission Methods       | Filter the results to only return these types of way consumers submitted their complaints.        |
| Tags                     | Filter the results to only return these types of tag.                                             |
| Timely Response          | Filter the results to show whether a response was timely.                                         |
| Zip Codes                | Filter the results to only return these zip codes.                                                |

- **Outputs**:

| Name         | Description                  |
|--------------|------------------------------|
| StatesResult | The state-by-state information.|

### List Complaint Trends

Return specific aggregations for a search.

- **Inputs**:

| Name                     | Description                                                                                       |
|--------------------------|---------------------------------------------------------------------------------------------------|
| Search Term              | Return results containing specific term.                                                          |
| Field                    | Specify which field is searched if "Search Term" has a value. Defaults to "Complaint What Happened".|
| Companies                | Filter the results to only return these companies.                                                |
| Public Responses         | Filter the results to only return these types of public response by the company.                  |
| Max Company Received Date| Return results with date < Max Company Received Date (i.e. 2017-03-04).                           |
| Min Company Received Date| Return results with date >= Min Company Received Date (i.e. 2017-03-04).                          |
| Company Responses        | Filter the results to only return these types of response by the company.                         |
| Consumer Consents        | Filter the results to only return these types of consent consumer provided.                       |
| Consumer Disputed        | Filter the results to only return the specified state of consumer disputed, i.e. yes, no.         |
| Max Date Received        | Return results with date < Max Date Received (i.e. 2017-03-04).                                   |
| Min Date Received        | Return results with date >= Min Date Received (i.e. 2017-03-04).                                  |
| Focus                    | The name of the product or company on which to focus charts for products and issues.               |
| Has Narrative            | Filter the results to only return the specified state of whether it has narrative in the complaint.|
| Issues                   | Filter the results to only return these types of issue and subissue.                              |
| Lens                     | The data lens through which to view complaint trends over time.                                   |
| Products                 | Filter the results to only return these types of product and subproduct.                          |
| States                   | Filter the results to only return these states.                                                   |
| Submission Methods       | Filter the results to only return these types of way consumers submitted their complaints.        |
| Sub Lens                 | The sub-lens through which to view complaint trends over time.                                    |
| Sub Lens Depth           | The top X trend sub aggregations will be returned, where X is the supplied sub_lens_depth.         |
| Tags                     | Filter the results to only return these types of tag.                                             |
| Timely Response          | Filter the results to show whether a response was timely.                                         |
| Trend Depth              | The top X trend aggregations will be returned, where X is the supplied trend_depth.                |
| Trend Interval           | The interval of time to use for trends aggregations histograms.                                   |
| Zip Codes                | Filter the results to only return these zip codes.                                                |

- **Outputs**:

| Name         | Description                  |
|--------------|------------------------------|
| TrendsResult | The trends result.           |

## Obtaining Credentials

This API is anonymous and does not require authentication.

## Known Issues and Limitations

Currently, no known issues or limitations exist. Always refer to this section for updated information.
