# API Indonesia (Independent Publisher)

## What the service is

API Indonesia is one REST API and one remote MCP server for Indonesian public data taken from official sources: administrative regions down to villages and postal codes, region boundaries as GeoJSON, national holidays and cuti bersama, prayer times and Hijri dates, schools with NPSN, universities, hospitals, BMKG earthquakes, weather and warnings, air quality, Bank Indonesia exchange rates, provincial minimum wage, NIK and NPWP format checks, QRIS parsing, rupiah formatting and terbilang, halal certification (BPJPH), BPOM, DJKI trademarks, OJK lists, food prices, Transjakarta, KBBI, Al-Quran and Alkitab. Site: https://apiindonesia.id. 84 MCP tools. Made by Khaisa Studio (Pontianak, Indonesia).

## Why a Power Platform connector

Makers building agents and flows for Indonesian users currently scrape government sites or hand-write HTTP actions to answer questions like "what is the postal code for this kelurahan", "is 25 December a holiday", "what time is Maghrib in Bandung", "what is the provincial minimum wage this year". This connector wraps those lookups as named operations with a stable JSON envelope, so a Copilot Studio agent or a Power Automate flow can call them directly.

## Proposed operations

REST, JSON, authenticated with an API key in the `x-api-key` header (the same header the public API documents).

1. `ListProvinsi` / `GetProvinsi` - GET /api/v1/wilayah/provinsi[/{id}]: provinces and their codes.
2. `ListKabupaten` / `ListKecamatan` / `ListKelurahan` - the region tree down to village level, with postal codes.
3. `ListHolidays` - GET /api/v1/hari-libur: national holidays and cuti bersama by year.
4. `GetPrayerTimes` - prayer times for a city and date.
5. `GetEarthquakes` - latest BMKG earthquakes.
6. `GetExchangeRates` - Bank Indonesia exchange rates.
7. `ValidateNik` / `FormatTerbilang` / `ParseQris` - validation and formatting helpers.

The full REST surface is documented by the service's own OpenAPI document at https://apiindonesia.id/openapi.json, and the operation subset above is a starting point for review; I am happy to widen or narrow it.

## Authentication

- API key: create one at https://dashboard.apiindonesia.id/ and send it as the `x-api-key` header (apiKey, header).
- OAuth 2.1 (DCR + PKCE S256) is also supported on the MCP endpoint at https://mcp.apiindonesia.id/mcp, published as `id.apiindonesia/api-indonesia` in the official MCP Registry.

## Pricing and availability

Sign-up gives 3,000 free credits with no card. A successful request costs 2 credits (Rp2); failed requests are not billed. Available worldwide, with documentation in Indonesian and English.

## What I can provide during certification

- Complete OpenAPI 2.0 definition and apiProperties.json.
- A test API key with credits for the certification reviewers.
- Public documentation at https://docs.apiindonesia.id (agent index: https://docs.apiindonesia.id/llms.txt) and a public examples repository at https://github.com/khmuhtadin/api-indonesia-examples.

## Publisher

Khaisa Studio, Pontianak, Indonesia. Submitted as an Independent Publisher connector by the service owner.
