# USAJOBS

[USAJOBS.gov](https://www.usajobs.gov/) is the Federal Government's official job board, providing free access to thousands of job opportunities across hundreds of federal agencies and organizations. As the primary source for federal job listings, applications, and employment opportunity information, [USAJOBS.gov](https://www.usajobs.gov/) offers a diverse range of positions to job seekers. With over 17 million users, it helps connect candidates with federal careers, ensuring fair and equal opportunity in the hiring process.

## Publisher: Richard Wilson

## Prerequisites

To use this connector, you must obtain an API key after registering on the USAJOBS website. This API key will be used to authenticate requests to the USAJOBS API.

## Supported Operations

### Search for jobs

Search USAJOBS for job opportunity announcements.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Keyword`                      | Search based on a keyword.                                             |
  | `Position Title`               | Search for job announcements containing this title.                    |
  | `Remuneration Minimum Amount`  | Minimum salary for the job.                                            |
  | `Remuneration Maximum Amount`  | Maximum salary for the job.                                            |
  | `Pay Grade High`               | Maximum pay grade for the job.                                         |
  | `Pay Grade Low`                | Minimum pay grade for the job.                                         |
  | `Job Category Code`            | Job category series code.                                              |
  | `Location Name`                | Location name for the job.                                             |
  | `Posting Channel`              | Posting channel for the job.                                           |
  | `Organization`                 | Agency subelement code.                                                |
  | `Position Offering Type Code`  | Position offering type code.                                           |
  | `Travel Percentage`            | Travel percentage for the job.                                         |
  | `Position Schedule Type Code`  | Position schedule type code.                                           |
  | `Relocation Indicator`         | Indicates if the job offers relocation.                                |
  | `Security Clearance Required`  | Security clearance required for the job.                               |
  | `Supervisory Status`           | Supervisory status of the job.                                         |
  | `Date Posted`                  | Number of days since the job was posted.                               |
  | `Job Grade Code`               | Grade code for the job.                                                |
  | `Sort Field`                   | Field to sort the results by.                                          |
  | `Sort Direction`               | Direction to sort the results.                                         |
  | `Page`                         | Page number for the results.                                           |
  | `Results Per Page`             | Number of results per page, up to 500.                                 |
  | `Who May Apply`                | Candidate designation for the job.                                     |
  | `Radius`                       | Radius for location-based search.                                      |
  | `Fields`                       | Level of detail in the job data.                                       |
  | `Salary Bucket`                | Salary range grouping.                                                 |
  | `Grade Bucket`                 | Grade level grouping.                                                  |
  | `Hiring Path`                  | Hiring paths for the job.                                              |
  | `Mission Critical Tags`        | Mission-critical tags for the job.                                     |
  | `Position Sensitivity`         | Position sensitivity and risk level.                                   |
  | `Remote Indicator`             | Indicates if the job is remote.                                        |

- **Outputs**:

  Returns a list of job opportunities matching the search criteria, including details such as job title, location, organization, and application links.

### List academic honors

Returns a list of academic honors values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of academic honors and corresponding codes.

### List academic levels

Returns a list of academic levels values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of academic levels and corresponding codes.

### List action codes

Returns a list of action codes values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of action codes and corresponding codes.

### List agency subelements

Returns a list of agencies and the corresponding organization codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of agency subelements and corresponding codes.

### List announcement closing types

Returns a list of announcement closing types values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of announcement closing types and corresponding codes.

### List applicant suppliers

Returns a list of applicant suppliers values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of applicant suppliers and corresponding codes.

### List application statuses

Returns a list of application status values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of application status values and corresponding codes.

### List countries

Returns a list of countries.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of countries.

### List country subdivisions

Returns a list of states, provinces, or territories within countries.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |
  | `Country`                      | The 3 character ISO Code.                                              |

- **Outputs**:

  Returns a list of country subdivisions.

### List cyber work groupings

Returns a list of cyber work grouping values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of cyber work groupings and corresponding codes.

### List cyber work roles

Returns a list of cyber work role values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of cyber work roles and corresponding codes.

### List degree type codes

Returns a list of degree type codes values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of degree type codes and corresponding codes.

### List disabilities

Returns a list of disabilities values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of disabilities and corresponding codes.

### List documentations

Returns a list of documentations values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of documentations and corresponding codes.

### List document formats

Returns a list of document formats values and the corresponding codes.

-

 **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of document formats and corresponding codes.

### List ethnicities

Returns a list of ethnicities values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of ethnicities and corresponding codes.

### List federal employment statuses

Returns a list of federal employment statuses values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of federal employment statuses and corresponding codes.

### List GSA geographic locations

Returns a list of GSA geographic location values.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of GSA geographic locations and corresponding codes.

### List hiring paths

Returns a list of hiring paths and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of hiring paths and corresponding codes.

### List key standard requirements

Returns a list of key standard requirements values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of key standard requirements and corresponding codes.

### List language codes

Returns a list of language values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of language codes and corresponding values.

### List language proficiencies

Returns a list of language proficiencies values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of language proficiencies and corresponding codes.

### List location expansions

Returns a list of location expansions values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of location expansions and corresponding codes.

### List military status codes

Returns a list of military status values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of military status codes and corresponding values.

### List mission critical codes

Returns a list of mission critical values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of mission critical codes and corresponding values.

### List occupational series

Returns a list of job categories and the corresponding job category code.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of occupational series and corresponding codes.

### List pay plans

Returns a list of pay plans and the corresponding pay plan code.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of pay plans and corresponding codes.

### List position offering types

Returns a list of position offering types values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of position offering types and corresponding codes.

### List position opening statuses

Returns a list of position opening status values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of position opening status codes and corresponding values.

### List position schedule types

Returns a list of position schedule types values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of position schedule types and corresponding codes.

### List postal codes

Returns a list of postal code values.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of postal codes.

### List race codes

Returns a list of race values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of race codes and corresponding values.

### List referee type codes

Returns a list of referee type values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of referee type codes and corresponding values.

### List remuneration rate interval codes

Returns a list of remuneration rate interval values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of remuneration rate interval values and corresponding codes.

### List required standard documents

Returns a list of required standard documents values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of required standard documents and corresponding codes.

### List security clearances

Returns a list of security clearances values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of security clearances and corresponding codes.

### List service types

Returns a list of service types values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of service types and corresponding codes.

### List special hirings

Returns a list of special hirings values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of special hirings and corresponding codes.

### List travel percentages

Returns a list of travel percentages values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of travel percentages and corresponding codes.

### List who may apply

Returns a list of who may apply values and the corresponding codes.

- **Inputs**:

  | Name                           | Description                                                            |
  |--------------------------------|------------------------------------------------------------------------|
  | `Last Modified`                | Only return items modified on or after the date supplied.              |

- **Outputs**:

  Returns a list of who may apply and corresponding codes.

## Obtaining Credentials

To obtain credentials for accessing the USAJOBS API, you must register on the [USAJOBS Developer Portal](https://developer.usajobs.gov/apirequest/) and request an API key. This key will be used in the header of your API requests for authentication.

## Known Issues and Limitations

Currently, no known issues or limitations exist. Always refer to this section for updated information.
