# FBI Most Wanted

This service offers an extensive database on the FBI's most wanted individuals and art crimes, providing detailed profiles, appearance descriptions, crime details, and available rewards. It's designed for public safety awareness, aiding identification, and reporting of suspects.

## Publisher: Richard Wilson

## Prerequisites

None

## Supported Operations

### List Recent Wanted

Get a listing of people wanted by the FBI. This operation allows filtering and sorting to find specific individuals.

- **Inputs**:
  - `Page Size`: Number of items to return per page. Default is 20.
  - `Page`: Page number of result listing. Default is 1.
  - `Sort On`: Specify the field to sort on, either `publication` or `modified`.
  - `Sort Order`: Specify the order to sort by, either `desc` or `asc`.
  - `Title`: Title of the wanted person.
  - `Field Office`: Filter by responsible field office. Options include `albany`, `albuquerque`, `anchorage`, etc.
  - `Person Classification`: Classification of the person, such as `Main`, `Victim`, or `Accomplice`.
  - `Poster Classification`: Filter by the classification of the poster, including `default`, `ten`, `terrorist`, etc.
  - `Status`: Filter by the status of the person, including `captured`, `recovered`, `located`, etc.
- **Outputs**:
  - `Total Records`: Total number of wanted records available.
  - `Current Page Number`: The page number of the current result set.
  - `List of Wanted Individuals`: A collection of wanted individuals.

### List Art Crimes

Get a listing of national art theft. This operation provides details on art crimes, including the art piece involved and the status of the crime.

- **Inputs**:
  - `Page Size`: Enter the number of crimes to return per page (50 is the maximum). Default is 20.
  - `Page`: Results page you would like to retrieve.
  - `Sort By`: Specifies the field to sort results on. Options are `publication` or `modified`.
  - `Sort Order`: Specifies the direction of sorting results. Options are `desc` or `asc`.
  - `Title`: Filter on the title of the art.
  - `Category`: Filter on the category of the art.
  - `Maker`: Filter on who made the art.
  - `Reference Number`: Filter on the art crime reference number.
- **Outputs**:
  - `Total Art Crimes`: Total number of art crimes tracked.
  - `Page Number`: Current page of art crime listings.
  - `List of Art Crimes`: A collection of art crimes.

### Get Data for Wanted Person

Retrieve information on a specific wanted person by their unique identifier.

- **Inputs**:
  - `UID`: Enter the unique identifier (UID) for the person.
- **Outputs**:
  - Detailed information about the wanted person, including their title, description, images, and status among other details.

### Get Data for Art Crime

Retrieve detailed information on a specific art crime by its unique identifier.

- **Inputs**:
  - `UID`: Enter the unique identifier (UID) for the art crime.
- **Outputs**:
  - Detailed information about the art crime, including the title, description, images, category, and status among other details.

## Obtaining Credentials

No authentication is required to access the FBI Most Wanted API. This service is publicly accessible, allowing users to query information without the need for API keys or OAuth tokens.

## Known Issues and Limitations

Currently, no known issues or limitations exist. Always refer to this section for updated information.
