# Federal Reserve Economic Data (FRED)
FRED contains frequently updated US macro and regional economic time series at annual, quarterly, monthly, weekly, and daily frequencies. FRED aggregates economic data from a variety of sources- most of which are US government agencies. The economic time series in FRED contain observation or measurement periods associated with data values.

## Publisher: Troy Taylor, Hitachi Solutions

## Prerequisites
There are no prerequisites for this API other than obtaining an API key.

## Obtaining Credentials
You will need to register on the Federal Reserve Board of St. Louis website for an [API key](https://fred.stlouisfed.org/docs/api/api_key.html).

## Supported Operations

### Get a category
Get a category.
### Get the child categories
Get the child categories for a specified parent category.
### Get the related categories for a category
A related category is a one-way relation between 2 categories that is not part of a parent-child category hierarchy. Most categories do not have related categories.
### Get the series in a category
Get the series in a category.
### Get the tags for a category
Get the FRED tags for a category.
### Get the related tags for tags within a category
Get the related FRED tags for one or more FRED tags within a category. FRED tags are attributes assigned to series. For this request, related FRED tags are the tags assigned to series that match all tags in the parameters.
### Get all releases of economic data
Get all releases of economic data.
### Get release dates for all releases of economic data
Get release dates for all releases of economic data.
### Get a release of economic data
Get a release of economic data.
### Get release dates for a release of economic data
Get release dates for a release of economic data. Note that release dates are published by data sources and do not necessarily represent when data will be available on the FRED or ALFRED websites.
### Get the series on a release of economic data
Get the series on a release of economic data.
### Get the sources for a release of economic data
Get the sources for a release of economic data.
### Get the tags for a release
Get the FRED tags for a release.
### Get the related tags for one or more tags within a release
Get the related FRED tags for one or more FRED tags within a release.
### Get release table trees for a given release
Get release table trees for a given release. Note that release dates are published by data sources and do not necessarily represent when data will be available on the FRED or ALFRED websites.
### Get an economic data series
Get an economic data series.
### Get the categories for an economic data series
Get the categories for an economic data series.
### Get the observations or data values for an economic data series
Get the observations or data values for an economic data series.
### Get the release for an economic data series
Get the release for an economic data series.
### Get economic data series that match search text
Get economic data series that match search text.
### Get the related tags for one or more  tags matching a series search
Get the related FRED tags for one or more FRED tags matching a series search.
### Get the tags for a series search
Get the FRED tags for a series search.
### Get the tags for a series
Get the FRED tags for a series.
### Get economic data series sorted by when observations were updated
Get economic data series sorted by when observations were updated on the FRED server. Results are limited to series updated within the last two weeks.
### Get the dates in history when a series' data values were revised
Get the dates in history when a series' data values were revised or new data values were released. Vintage dates are the release dates for a series excluding release dates when the data for the series did not change.
### Get all sources of economic data
Get all sources of economic data.
### Get a source of economic data
Get a source of economic data.
### Get the releases for a source
Get the releases for a source.
### Get all tags
Get FRED tags. Optionally, filter results by tag name, tag group, or search. FRED tags are attributes assigned to series.
### Get the related tags for one or more tags
Get the related FRED tags for one or more FRED tags. Optionally, filter results by tag group or search.  FRED tags are attributes assigned to series. Related FRED tags are the tags assigned to series that match all tags in the tag_names parameter and no tags in the exclude_tag_names parameter.
### Get the series matching tags
Get the series matching all tags in the tag_names parameter and no tags in the exclude_tag_names parameter.

## Known Issues and Limitations
There are no issues or limitations none at this time.
