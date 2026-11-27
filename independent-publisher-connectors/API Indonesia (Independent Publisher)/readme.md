# API Indonesia
API Indonesia is one API for Indonesian public data from official sources: administrative regions down to villages and postal codes, region boundaries, national holidays and cuti bersama, prayer times and Hijri dates, schools, universities, hospitals, BMKG earthquakes, weather and warnings, air quality, Bank Indonesia exchange rates, minimum wages, transport (Transjakarta, toll roads, airports, ports, stations), trademarks, BPOM and halal records, NIK and NPWP format checks, QRIS parsing and rupiah helpers.

## Publisher: Khairul Muhtadin (Khaisa Studio)

## Prerequisites
You need an API Indonesia account and an API key. Sign up at [dashboard.apiindonesia.id](https://dashboard.apiindonesia.id/). New accounts get 3,000 free credits with no card. A successful request costs 2 credits, and failed requests are not billed.

## Supported Operations
The connector has 149 operations, grouped by dataset.

### Dataset metadata
- **List dataset metadata**: Returns metadata for every registered dataset. This endpoint does not require an API key.
- **Retrieve dataset metadata**: Returns snapshot and provenance metadata for one dataset.

### Regions (wilayah)
- **List provinces**: Returns a list of all provinces in Indonesia.
- **Retrieve a province**: Returns a single province by its region code.
- **List regencies or cities in a province**: Nested alias for listing regencies/cities by province code.
- **List regencies or cities**: Returns a list of regencies and cities, optionally filtered by province.
- **Retrieve a regency or city**: Returns a single regency/city by its region code.
- **List districts in a regency or city**: Nested alias for listing districts by regency/city code.
- **List districts**: Returns a list of districts, optionally filtered by regency/city.
- **Retrieve a district**: Returns a single district by its region code.
- **List villages in a district**: Nested alias for listing villages and customary villages by district code.
- **List villages**: Returns a list of villages complete with postal codes, optionally filtered by district.
- **Retrieve a village**: Returns a single village by its region code.
- **Search regions**: Searches region names across all levels at once (province, regency, district, village). The level field indicates the region level of each result.

### Public holidays
- **List public holidays**: Returns a list of public holidays, sorted by date. Can be filtered by year and month.
- **Upcoming holidays**: Returns up to 10 upcoming public holidays, counted from today.
- **Retrieve a holiday**: Returns a single public holiday by ID.

### Prayer times
- **List prayer time cities**: Returns the regencies/cities that have prayer time data. Use the regency_id from this response with the schedule endpoints.
- **Daily prayer times**: Returns one day of prayer times for a given regency/city.
- **Monthly prayer times**: Returns a full month of prayer times for a given regency/city, sorted by date.

### Schools
- **List schools**: Returns a paginated list of schools with name/NPSN search plus region, level, status, and accreditation filters.
- **Search schools**: Searches schools by name. Returns up to 50 results.
- **Retrieve a school**: Returns a single school by NPSN, including province, regency/city, district, and village names.

### Universities
- **List universities**: Returns a paginated, filterable list of higher-education institutions, including province and regency/city names.
- **Search universities**: Searches institutions by name or abbreviation. Returns up to 50 results.
- **Retrieve a university**: Returns a single institution by ID, including province and regency/city names.

### Hospitals
- **List hospitals**: Compact list with name/code search and region, type, class, ownership, and BLU filters. Use the detail endpoint for service, staff, bed, and facility collections.
- **Search hospitals**: Searches hospitals by name/code with the same filters as the list endpoint. Returns up to 50 results.
- **Retrieve a hospital**: Returns a full profile by internal ID or SIRS code, including available bed, service, staff, facility, provenance, and synchronization details.

### Earthquakes (BMKG)
- **Latest earthquakes**: Returns the 15 most recent earthquakes, newest first.
- **Felt earthquakes**: Returns the 15 most recent felt earthquakes (BMKG term for quakes reported felt by the public), complete with the MMI scale per area.
- **Earthquake history**: Returns earthquakes within a given date range, up to 100 records, newest first.

### Weather forecast (BMKG)
- **Village weather forecast**: Returns a 3-day weather forecast for a single village. Choose between adm4 (village code) or kabupaten_id to fetch a representative forecast for the regency.
- **Search forecast location**: Searches for villages or cities that have weather forecast data by name. Returns a list of adm4 codes usable in the main endpoint.

### Weather warnings (BMKG)
- **List weather warnings**: Returns all currently active weather warnings. Can be filtered by province name.

### Postal codes
- **List postal codes**: Returns paginated postal codes with explicit Kemendagri administrative codes. BPS code fields and filters are available for 96% of records (official SIG BPS 2025 bridge). A small number of newer villages have no BPS counterpart yet.
- **Look up a region by postal or government code**: Exact lookup by postal or Kemendagri code. At least one filter is required. Lookup by BPS code also works for the roughly 96% of records that have a BPS counterpart.
- **Search postal codes**: Searches postal codes by village name or postal code number.
- **Retrieve a postal code**: Returns a single postal code entry by ID.

### Exchange rates
- **Latest exchange rate**: Returns the latest exchange rate for a given currency pair.
- **Historical exchange rates**: Returns historical exchange rates for a given date range.

### Quran
- **List surah**: Returns the complete list of 114 Al-Quran surahs, ordered by number.
- **Retrieve a surah**: Returns the metadata of a single surah by number (1–114).

### Bible (Alkitab)
- **List Bible books**: Returns all 66 Bible books with chapter counts and testament classification (OT/NT).
- **List Bible books by testament**: Filter books by testament: PL (Old Testament) or PB (New Testament).
- **Read a Bible chapter**: Returns all verses in a chapter. Use book abbreviation (e.g. Gen, John, Ps) or full name.
- **Read a Bible verse**: Returns a single verse by book, chapter, and verse number.
- **Search Bible verses**: Search for a word or phrase across all 31,104 Bible verses. Supports pagination via limit and offset.

### Dictionary (KBBI)
- **Search KBBI words**: Search KBBI dictionary by keyword. Supports pagination.
- **Look up a KBBI word**: Returns all KBBI entries for a specific word (some words have multiple entries).
- **Random KBBI words**: Returns random words from KBBI. Useful for games or learning apps.

### Vehicle plates
- **List vehicle plate codes**: Returns all 61 vehicle plate codes, sorted by code.
- **Retrieve a plate code**: Returns details for a single plate code.

### Provincial minimum wage (UMP)
- **List provincial minimum wages (UMP)**: All provincial minimum wage records. Use query params to filter.
- **Provincial minimum wage for all years**: All UMP records for one province, sorted by latest year.
- **Provincial minimum wage for one year**: A single UMP record for a specific province and year.

### Validation
- **List Indonesian banks**: Returns bank codes with valid account number lengths.
- **Validate NIK**: Checks 16-digit NIK format and extracts region/birth date codes. Not Dukcapil verification.
- **Validate NPWP**: Checks 15-digit NPWP format. Not DJP verification.

### Rupiah and QRIS helpers
- **Format Rupiah**: Formats a number as Indonesian Rupiah string.
- **Parse QRIS**: Decodes a QRIS EMVCo payload string.
- **Parse Rupiah**: Parses rupiah text, such as Rp1.500.000, into a number.
- **Spell out Rupiah**: Spells out a rupiah amount in Indonesian words (terbilang).
- **Convert to dynamic QRIS**: Adds an amount to static QRIS and recalculates CRC.

### Bank Indonesia rates
- **Latest Bank Indonesia rates**: Latest transaction rates for USD/EUR/SGD (default) or other currencies.
- **Bank Indonesia rate history**: Daily rate history for one currency (max 366 days).

### Halal supervisors (BPJPH)
- **Search halal supervisors**: Searches halal supervisors (penyelia halal) registered with BPJPH by business or supervisor name.
- **Retrieve a halal supervisor**: Gets one halal supervisor record by its id_penyelia UUID.

### BPOM products
- **Search BPOM registered products**: Search registered medicine/traditional/cosmetic products.
- **Retrieve a BPOM product by NIE**: Get product by NIE (registration number).

### Hijri calendar
- **Convert a Gregorian date to Hijri**: Convert one Gregorian date to Hijri.
- **Hijri calendar for a year**: Full date mapping for one Gregorian year (currently 2026).

### OJK investment lists
- **List legal investment products**: Returns a list of legal mutual fund products registered with OJK. Supports pagination and name search.
- **Investment product detail**: Returns details of a single mutual fund product by ID.
- **List legal investment apps**: Returns a list of legal investment apps registered with OJK. Supports pagination and name search.
- **Investment app detail**: Returns details of a single investment app by ID.
- **List illegal investments**: Returns a list of illegal investments flagged by OJK. Each entry may include aliases, addresses, phone numbers, emails, websites, entity type, activity types, and a description. Supports pagination and name search.
- **Illegal investment detail**: Returns details of a single illegal investment by ID.

### Astronomy
- **Sunrise and sunset**: Returns sunrise, sunset, solar noon, and day length for a given lat/lon and date. Handles polar night / midnight sun.
- **Twilight times**: Returns civil, nautical, and astronomical twilight boundaries for photography/observation.
- **Moon phase**: Returns the moon phase (illumination, phase name, age, distance) for a given date.
- **Upcoming moon phases**: Returns the next major moon phases (new/first quarter/full/last quarter), up to 12 ahead.
- **Eclipse predictions**: Returns upcoming solar & lunar eclipses (approximation based on saros/draconic cycles). Dates & kinds are indicative.

### Trademarks (DJKI)
- **Search registered trademarks**: Returns registered trademarks matching a keyword. Search covers the mark-name column; use optional parameters to filter by owner, NICE class, or search within the owner column instead.
- **Trademark detail**: Returns details of a single trademark by application number. If the number only appears in the publication journal (not yet registered), the response still returns 200 with journal data plus a journal_events list.
- **Search the trademark journal**: Returns trademark applications published in the DJKI Berita Resmi Merek journal. Coverage is a rolling window of recent editions (roughly twice a month, last few months). Use it to monitor lookalike filings during the opposition window.

### Region boundaries
- **List region boundaries**: Lists the active region boundaries with their metadata.
- **Region boundary metadata**: Gets the source, license and GeoJSON URL of one region boundary.
- **Region boundary GeoJSON**: GeoJSON application/geo+json; X-Data-* headers carry source, version, license and attribution. Illustrative geometry.

### Air quality
- **List air quality readings for today**: Lists the air quality readings cached today for Indonesian cities. The list may be empty early in the day.
- **Latest air quality for a city**: Gets the latest air quality reading for a city by part of its name. Returns 404 when the city has no reading.

### Food prices
- **Food price series overview**: Returns a summary of every series (consumer/producer level x national/province region): commodity count, row count, and the available month range.
- **Monthly food prices**: Returns monthly average food prices, filterable by level, region, commodity, province, and month range. Prices are integers in rupiah; "-" in the source becomes null.
- **Food price anomaly index**: Strategic commodity price-anomaly indices (QIPA, AIPA, IFPA) with status Alert (Low/High Price) or Normal. From National Food Agency monthly releases (check the latest date in the response); useful to flag abnormal price spikes or drops.

### Transjakarta
- **List Transjakarta routes**: Returns the 240 Transjakarta routes. - Short and long names - Service description (BRT, integrated public transport, Mikrotrans, Transjabodetabek, tour bus) - GTFS route type - Corporate colour with its text colour - The number of unique halte the route serves, counted across both directions, not the number of rows.
- **Retrieve a Transjakarta route**: One route in full. - The fares that apply to it (from fare_rules) - Its departure headways (per trip) - The halte list of both directions: "ke" for direction_id 0 and "kembali" for direction_id 1, each ordered by ascending stop_sequence - An unknown route answers 404 NOT_FOUND - The official release carries no street geometry, so per-segment travel distances are not available here.
- **List Transjakarta stops**: Returns the 8,091 Transjakarta halte with code, name, coordinates, and GTFS stop type. The q filter partially matches name, code, or halte ID.
- **Nearest Transjakarta stops**: Finds the halte around a coordinate, nearest first, with the distance in metres (distance_m). - A bounding box is applied first and the exact great-circle distance second, so a 5 km radius stays cheap - Radius defaults to 1,000 metres (maximum 5,000) and limit to 20 (maximum 100) - A missing or non-numeric lat/lng, a radius outside 1-5,000, or a limit outside 1-100 answers 400 VALIDATION_ERROR.
- **Retrieve a Transjakarta stop**: One halte plus the routes that call there, one row per route and direction pair (direction_id). A halte served in both directions appears twice, and a tourist halte can be served by several routes. An unknown halte answers 404 NOT_FOUND.
- **List Transjakarta fares**: The six GTFS fares with their rupiah prices. - FP: Rp3,500, the flat Transjakarta fare - GR: Rp0, free on selected routes - PP: Rp20,000, plus PP2/PP3 for airport services - How many transfers are allowed (null means the fare covers a single journey) - This endpoint does not paginate, so meta carries the attribution only.
- **Transjakarta departure headways**: Departure frequencies from GTFS frequencies.txt. - One row per trip serving that route, with the operating window and the headway in seconds (900 seconds means a bus every 15 minutes) - The route filter narrows the result to one route; a route without frequency rows answers an empty list, not a 404 - Times are returned as the source "HH:MM:SS" strings, so night services can exceed 24:00:00.

### Points of interest
- **Search and list POI**: POI list ordered by name. - The q filter matches the START of the name, case-insensitively (q=masjid matches "Masjid Istiqlal", q=istiqlal does not) - category takes a key from /api/v1/poi/categories - locality (alias kota) matches the city/regency exactly as written in the Overture address, e.g. "Jakarta Selatan" or "Surabaya" - Filters can be combined - per_page is capped at 100 and the offset at 5,000.
- **Nearest POI**: POI around a coordinate, nearest first, with the distance in metres (distance_m). - Optionally filtered by category and minimum confidence (min_confidence) - Radius defaults to 1,000 metres (maximum 5,000) and limit to 20 (maximum 100) - A missing or non-numeric lat/lon, a radius outside 1-5,000, a limit outside 1-100, or a min_confidence outside 0-1 answers 400 VALIDATION_ERROR - lng is accepted as an alias for lon.
- **List POI categories**: Every Overture category present in the Indonesian data (1,250 categories), most POI first, with basic_category, taxonomy hierarchy and POI count. The q filter partially matches the category key.
- **Retrieve a POI**: One POI by its Overture ID (GERS ID, stable across releases). An unknown ID answers 404 NOT_FOUND.

### IP geolocation
- **Look up an IP address**: Details for one IP address. - Country (ISO 3166-1 alpha-2 code), continent code, region, city, latitude/longitude - The ASN with its organisation name, and the DB-IP range the address falls in - IPv6 can be written directly in the path; IPv4-mapped addresses (::ffff:a.b.c.d) are looked up as IPv4 - An invalid address answers 400 and an address with no matching range answers 404; neither is charged.
- **Look up the caller IP address**: Same as /ip/{ip}, for the address that called the API (the CF-Connecting-IP header). The answer still comes from the DB-IP tables, not from Cloudflare's own geolocation. Never edge-cached. On the RapidAPI surface the caller address is the RapidAPI gateway, so this endpoint answers 400 UNSUPPORTED there; use /ip/{ip}.
- **Look up many IP addresses**: Up to 100 addresses in one call, answered in input order. - An invalid or uncovered address gets a per-item error object instead of failing the whole call - Priced per CALL, not per IP: 100 credits for 1 to 100 addresses (1 credit per IP on a full batch) - For fewer than 50 addresses, /ip/{ip} (2 credits per call) is cheaper - A body with no valid address answers 400 and is not charged.

### Legal entity identifiers (LEI)
- **Search legal entities**: Searches the 1,644 entities with an Indonesian legal or headquarters address (golden copy snapshot of 1 November 2026). - q matches the legal name and other/transliterated names through a full-text index (word prefixes) - The provinsi filter applies to both the legal and headquarters address - Some GLEIF records still use retired island codes (for example ID-JW for Java) and those are not rolled up into provinces.
- **Retrieve an LEI**: One entity in full. - Legal and headquarters addresses - Registration number (registration_authority.id from the GLEIF authority list + entity_id) - Registration status and managing LOU - direct_parent and ultimate_parent (null when no relationship record exists) - An LEI with the wrong shape or an invalid ISO 17442 checksum answers 400 INVALID_LEI; a valid LEI outside the dataset answers 404.
- **List child entities of an LEI**: Entities that report this LEI as their direct parent (type=direct, the default), ultimate parent (type=ultimate), or either (type=all). For a parent abroad, only the children held in this dataset (entities with an Indonesian address) are listed.

### Countries, cities and time zones
- **List countries**: Returns the 252 countries and territories in GeoNames countryInfo, ordered by name: ISO2, ISO3, numeric code, capital, continent, area, population, currency, calling code (without +), languages, TLD, postal code format and neighbours.
- **Retrieve a country**: One country by ISO2 or ISO3 code (case-insensitive), with its time zones and the number of its cities in the cities5000 data. An unknown code answers 404 NOT_FOUND.
- **Search cities**: Cities from GeoNames cities5000, largest population first. q matches the PREFIX of the name or ASCII name, case-insensitively ("sao" finds São Paulo); translated names (e.g. Indonesian names for foreign cities) are not available yet.
- **Nearest cities**: Cities around a coordinate, nearest first, with the great-circle distance in kilometres (distance_km). Radius defaults to 50 km (maximum 500) and limit to 20 (maximum 100). Searches across the 180° meridian are handled.
- **Retrieve a city**: One city by geonameid, with the country name, elevation, DEM, GeoNames modification date and its time zone offsets. An unknown id answers 404 NOT_FOUND.
- **List time zones**: All 418 time zones in GeoNames timeZones.txt (or one country with country), with the GMT offset on 1 January, the DST offset on 1 July of the same year (offset_year) and the raw offset without DST, in hours.
- **Time zone with local time**: One IANA time zone with the GeoNames offsets and the CURRENT local time, computed per request from the runtime IANA data (not cached): local_time, utc_offset and is_dst. The id may contain slashes (Asia/Jakarta) and is case-insensitive.

### Toll roads
- **List toll roads**: Returns the 77 toll roads in the BPJT list with operator (BUJT), status (operasi or operasi sebagian), total and operating length, provinces, operating decree date, investment value when available, gate count, BPJT road code, type (intercity/urban) and the PSN flag. Ordered by name.
- **Retrieve a toll road**: One toll road with its history (sejarah: operating decree and PPJT from BPJT, plus the Wikipedia opening date when available), its sources, every toll gate on the road and the BPJT tariffs for it. An unknown road answers 404 NOT_FOUND.
- **List toll gates**: Returns the 558 toll gates with coordinates, toll road, province, regency/city, operator and validation status. Filters can be combined.
- **Nearest toll gates**: Finds the toll gates around a coordinate, nearest first, with the distance in metres (distance_m). Radius defaults to 10,000 metres (maximum 50,000) and limit to 10 (maximum 50); a missing or non-numeric lat/lng, a radius outside 1-50,000 or a limit outside 1-50 answers 400 VALIDATION_ERROR.
- **Retrieve a toll gate**: One gate with its validation details: number of merged OSM points, distance to the OSM toll carriageway (jarak_ke_tol_m), validation notes (checks passed and confirming sources) and the OSM element IDs. An unknown gate answers 404 NOT_FOUND.
- **Toll tariffs**: Returns the 1,485 tariff rows from the BPJT "Data Tarif Jalan Tol" sheet: origin, destination and the rupiah tariff for classes I, II-III and IV-V (classes II and III, and IV and V, share a tariff as in the source). Tariffs change by ministerial decree, so check BPJT for adjustments after the snapshot date.

### Airports
- **List airports**: Returns the 440 airports with ICAO/IATA codes, region, coordinates, elevation, size, scheduled service, international status and validation status. Filters can be combined.
- **Nearest airports**: Finds airports around a coordinate, nearest first, with the distance in metres (distance_m). Radius defaults to 50,000 metres (maximum 300,000) and limit to 5 (maximum 50); invalid parameters answer 400 VALIDATION_ERROR.
- **Retrieve an airport**: One airport by ICAO code, IATA code or id, with its runways (direction, length, width, surface), operator when available, Wikidata ID, validation notes (distances between sources and to the official position) and coordinate source. An unknown code answers 404 NOT_FOUND.

### Ports
- **List ports**: Returns the 1,082 sea and ferry ports with hierarchy or class, region, UN/LOCODE, coordinates when validated and validation status. Filters can be combined.
- **Nearest ports**: Finds ports with coordinates around a coordinate, nearest first, with the distance in metres (distance_m). Radius defaults to 25,000 metres (maximum 200,000) and limit to 10 (maximum 50); ports without coordinates are not included. Invalid parameters answer 400 VALIDATION_ERROR.
- **Retrieve a port**: One port by id or UN/LOCODE, with its 2017/2027/2037 hierarchy, World Port Index details when available, validation notes, coordinate source and legal basis. An unknown id answers 404 NOT_FOUND.

### Bus terminals
- **List bus terminals**: Returns the 614 bus terminals with type (A or null), managing BPTD for type A, region, coordinates when validated and validation status. Filters can be combined.
- **Nearest terminals**: Finds terminals with coordinates around a coordinate, nearest first, with the distance in metres (distance_m). Radius defaults to 10,000 metres (maximum 100,000) and limit to 10 (maximum 50); terminals without coordinates are not included. Invalid parameters answer 400 VALIDATION_ERROR.
- **Retrieve a terminal**: One terminal with address (when available), Wikidata ID, validation notes, coordinate source and legal basis. An unknown terminal answers 404 NOT_FOUND.

### Train stations
- **List stations**: Returns the 1,129 stations with code, type, operator, Daop/Divre, status, region, coordinates and validation status. The jenis filter also matches services, so jenis=krl includes heavy-rail stations served by KRL.
- **Nearest stations**: Finds stations around a coordinate, nearest first, with the distance in metres (distance_m). Only active stations by default; status=semua includes inactive ones. Radius defaults to 5,000 metres (maximum 100,000) and limit to 10 (maximum 50). Invalid parameters answer 400 VALIDATION_ERROR.
- **Retrieve a station**: One station by KAI station code or ID, with its service list, elevation, Wikidata ID, validation notes and coordinate source. When a code is shared by more than one row, the active station wins. An unknown code answers 404 NOT_FOUND.

### Regency and city minimum wage (UMK)
- **List regency or city minimum wages (UMK)**: Returns regency and city minimum wages for the requested year, defaulting to the latest year in the dataset. Province filter, name search and value sort can be combined; the result count is in meta.total.
- **Retrieve one regency or city minimum wage**: Returns the latest wage for one regency/city code, with the validation note and the history of earlier years (empty when only one year exists). A region that only uses the provincial UMP answers 404.

### HS codes (BTKI)
- **List BTKI chapters**: Returns the 99 BTKI chapters with their section, Indonesian and English descriptions and the number of tariff lines per chapter. Useful for building navigation before filtering codes.
- **List HS codes**: Returns eight-digit HS codes with their description, heading, chapter and MFN duty. q accepts a 2 to 8 digit code (dots ignored) or words from the description; the bab and pos filters can be combined.
- **Retrieve one HS code**: Returns one HS code with its MFN duty, the validation note and its chapter detail. The code may be written with dots (8517.13.00) or without; an unknown code answers 404.

### Electricity and fuel prices
- **Electricity tariffs in force**: Returns electricity tariffs in force on a date (default today, WIB), one row per class and power limit. The golongan filter matches partially; kategori filters household, business, industrial or government classes.
- **Electricity tariff history for one class**: Returns every tariff period for one class, ordered by class, power limit and period start. The golongan parameter is required; an unknown class answers 404.
- **Subsidised fuel prices in force**: Returns subsidised and assigned fuel prices in force on a date (default today, WIB), ordered by product, with the unit, the selling area and the legal basis.
- **Fuel price history**: Returns every fuel price period, or a single product when produk is given (partial match, e.g. pertalite), ordered by product then start date.

### Public facilities
- **Categories and facility counts**: Returns the six categories with their facility counts and subcategory breakdown. Useful for building a filter menu before calling the list endpoint.
- **List facilities**: Returns facilities matching the filters, ordered by name. At least one of kategori, provinsi, kabupaten or q is required; without a filter the answer is 400, so the whole table is never pulled at once.
- **Nearest facilities**: Finds facilities around a coordinate, nearest first, with the distance in metres (distance_m). Radius defaults to 2,000 metres (maximum 50,000) and limit to 10 (maximum 50). Facilities without coordinates are not returned.
- **Retrieve one facility**: Returns one facility with its category-specific attributes, the OpenStreetMap and Wikidata ids, and the validation notes. An unknown id answers 404.

## Obtaining Credentials
1. Sign up or sign in at [dashboard.apiindonesia.id](https://dashboard.apiindonesia.id/).
2. Create an API key in the dashboard.
3. Paste the key into the API Key field when you create the connection. The connector sends it in the `x-api-key` header.

## Known Issues and Limitations
- Each successful request uses credits from your balance. When the balance runs out, requests answer HTTP 402 until you top up.
- Validate NIK checks the 16 digit format and reads the region and birth date codes. Validate NPWP checks the 15 digit format. Neither is verified against Dukcapil or DJP records.
- Some datasets are refreshed on a schedule, so the newest official data can take a short time to appear. Each response includes its source.
- Account, API key and billing endpoints of the service are not part of this connector.

## Documentation
- Product: https://apiindonesia.id
- Documentation: https://docs.apiindonesia.id
- OpenAPI document: https://apiindonesia.id/openapi.json

## Deployment Instructions
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```
