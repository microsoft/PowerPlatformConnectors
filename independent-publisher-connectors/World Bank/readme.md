# World Bank Data Catalog (Independent Publisher)

This is a custom connector for the [World Bank API](https://datahelpdesk.worldbank.org/knowledgebase/articles/898599), created using the Independent Publisher connector framework for Microsoft Power Platform.

The World Bank API provides access to a catalog of global development datasets, economic indicators, and country-level statistics.

## Publisher: Dan Romano (IDR Consultants)

## Prerequisites

- Microsoft Power Platform environment (Power Apps or Power Automate)
- No authentication is required for this connector.

## Getting Started

- [World Bank documentation](https://datacatalog.worldbank.org/)
- [World Bank API Help](https://datahelpdesk.worldbank.org/knowledgebase/articles/889392-about-the-indicators-api-documentation)
- [How to paginate the response](https://developments.substack.com/p/institutionalized)

## Supported Operations

### Country & Region Endpoints

1.) List All Countries

	GET /v2/country
	Retrieves a list of all countries with metadata (ISO codes, region, income level, etc.).

2.) Get Country by Code

	GET /v2/country/{countryCode}
	Retrieves detailed info for a specific country.

3.) List All Regions

	GET /v2/region
	Retrieves a list of all regions.

4.) Get Region by Code

	GET /v2/region/{regionCode}
	Retrieves detailed info for a specific region.

5.) List All Income Levels

	GET /v2/incomelevel
	Retrieves a list of all income levels.

6.) Get Income Level by Code

	GET /v2/incomelevel/{incomeLevelCode}
	Retrieves detailed info for a specific income level.

7.) List All Lending Types

	GET /v2/lendingtype
	Retrieves a list of all lending types.

8.) Get Lending Type by Code

	GET /v2/lendingtype/{lendingTypeCode}
	Retrieves detailed info for a specific lending type.

### Indicator Endpoints

9.) List All Indicators

	GET /v2/indicator
	Retrieves all available development indicators.

10.) Get Indicator by Code

	GET /v2/indicator/{indicatorCode}
	Retrieves metadata for a specific indicator.

11.) Get Indicator Data for All Countries

	GET /v2/country/all/indicator/{indicatorCode}
	Retrieves data for a specific indicator across all countries.

12.) Get Indicator Data for a Country

	GET /v2/country/{countryCode}/indicator/{indicatorCode}
	Retrieves time series data for a specific indicator in one country.

13.) Get Indicator Data by Region

	GET /v2/region/{regionCode}/indicator/{indicatorCode}
	Retrieves indicator data scoped to a specific region.

14.) Get Indicator Data by Income Level

	GET /v2/incomelevel/{incomeLevelCode}/indicator/{indicatorCode}
	Retrieves indicator data scoped to an income level.

15.) Get Indicator Data by Lending Type

	GET /v2/lendingtype/{lendingTypeCode}/indicator/{indicatorCode}
	Retrieves indicator data scoped to a lending category.

### Topic & Source Endpoints

16.) List All Topics

	GET /v2/topic
	Retrieves a list of development topics (e.g. education, health).

17.) Get Topic by ID

	GET /v2/topic/{topicId}
	Retrieves metadata for a specific topic.

18.) List All Data Sources

	GET /v2/source
	Retrieves a list of data providers (e.g. IMF, UN, World Bank).

19.) Get Source by ID

	GET /v2/source/{sourceId}
	Retrieves metadata for a specific source.

20.) Get Indicators by Source

	GET /v2/source/{sourceId}/indicator
	Returns all indicators published by a specific source.

## Known Issues and Limitations

- World Bank endpoints do not offer pagination. Refer to the documentation [here](https://developments.substack.com/p/institutionalized) for guidance on how to paginate the response.

