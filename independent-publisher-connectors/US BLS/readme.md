# US Bureau of Labor Statistics (BLS)

The **Bureau of Labor Statistics (BLS) API** provides public access to U.S. labor market and economic data. With this connector, you can query survey metadata and retrieve time series data from BLS datasets directly in Power Platform.

## Publisher: Dan Romano

## Prerequisites
You must have a valid [BLS API key](https://data.bls.gov/registrationEngine/). While some endpoints are available without a key, most advanced features (catalog metadata, calculations, annual averages, aspects) require one.

## Supported Operations

### Surveys
- **Get All Surveys** 
  Retrieve metadata for all BLS surveys.  

- **Get Survey Metadata** 
  Retrieve metadata for a specific survey using its abbreviation (e.g., CPS, CES).

### Series
- **Get Single Series** 
  Retrieve data for a single series ID. Example: `CUUR0000SA0` (Consumer Price Index, All Urban Consumers, U.S. city average, All items).  

- **Get Multiple Series**
  Retrieve data for one or more series by posting a list of series IDs and optional parameters (start year, end year, catalog, calculations, annual averages, aspects).  

- **Get Popular Series**
  Retrieve the most requested BLS series.

## Obtaining Credentials
1. Go to the [BLS API registration page](https://data.bls.gov/registrationEngine/).
2. Submit your email address to receive an API key.
3. Use this key as the `registrationkey` query parameter when calling API endpoints.

## Known Issues and Limitations
- The BLS API occasionally returns `Results` as either an object or an array, depending on the request. The connector normalizes this using a schemaless definition to avoid runtime errors in Power Platform.
- Rate limits apply. If you exceed the threshold, you may receive a `429 Too Many Requests` error.
- Some series IDs require specific survey knowledge; consult the [BLS survey documentation](https://www.bls.gov/developers/api_signature_v2.htm) for details.

## Further Reading
- [BLS Public Data API Documentation](https://www.bls.gov/developers/)
- [BLS Data Finder](https://beta.bls.gov/dataQuery/) (to explore available series IDs)
