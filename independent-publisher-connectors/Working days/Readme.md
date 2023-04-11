# Working days
Working days service. It covers more than 45 countries and 230 regional calendars. It also covers main stock exchanges calendars. The quality of data is the top priority. API offers up to date information by following government announcements regarding public holidays changes.

## Publisher: Tomasz Poszytek

## Prerequisites
No prerequisites.

## Supported Operations
### Add working days
Add (or remove) any number of working days to a given date.

### Analyze
Analyze a period (provide a start_date and an end_date over a given calendar, API responds with the period analysis).

### Get day information
Get detailed information about a specific day.

### List non working days
List the non working days (weekend days, public holidays and custom dates) between 2 dates.

### Add working hours
Add an amount of working time to a given start date/time.

### Add public holidays
Add days where public holidays are taking place to a given date.

### Add weekend days
Add weekends days to a given date.

### Locating a postal address
Sometimes it may be needed to identify which calendar configuration is relevant to a given customer's postal address. This service returns the appropriate country_code & configuration for a given postal adresss.

### Check quota
Check quota of left days to use within the current month and available for the next month.

## Obtaining Credentials
Navigate to https://api.workingdays.org/1.2/get-your-key.php to obtain API key. Each API call consumes part of monthly quota. The days removed from the quota after a call is equal to the length **in days** of the resulting period.
When you send a request like to analyze period between 2019-01-01 and 2019-12-31, it will take off 365 days of the monthly quota because they are 365 days in the year 2019.

## Known Issues and Limitations
No issues or limitations at the moment of api creation.

## Getting Started
Visit https://api.workingdays.org/ to get started and https://api.workingdays.org/1.2/api-test-tool.php to test the api on your own for free. Also, please visit https://api.workingdays.org/1.2/api-countries.php to check available countries and local configurations.
