# New York Times
The New York Times allows anyone to search articles by keyword, date, section, and popularity.  This connector allows a Power Platform user to pull this data into their app, flow, or report.

## Publisher
Roy Paar

## Prerequisites
This API uses an API key.  Register on the NYTimes Developer Portal (https://developer.nytimes.com/) for a free account.  Then create an application.  Then add the following three APIs to the application.  Copy and paste the API key into the Power Platform connection.
1. Article Search API
2. Most Popular API
3. Top Stories API

## Supported Operations
### Artitcle Search
Get a list of articles by keyword.  Enter a keyword into the 'q' field and optionally filter by begin_date or end_date.

### Top Stories
Get a list top stories by section.  Available sections are arts, automobiles, books, business, fashion, food, health, home, insider, magazine, movies, nyregion, obituaries, opinion, politics, realestate, science, sports, sundayreview, technology, theater, t-magazine, travel, upshot, us, and world.

### Most Viewed
Get a list of articles by popularity (number of views). Choose a time period of 1, 7, or 30 days.  

## API Documentation
https://developer.nytimes.com/apis

## Known Issues and Limitations
None.
