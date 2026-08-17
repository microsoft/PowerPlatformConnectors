# BBC News

## Publisher - Dan Romano (swolcat)

## Prerequisites

There are no prerequisites to use this connector.

## Supported Operations

The connector provides the following operations via a Vercel app hosted by another developer, Sayad Uddin Tahsin. His GitHub page can be found [here](https://github.com/Sayad-Uddin-Tahsin/BBC-News-API).

### Get News by Topic

Retrieve BBC news articles filtered by topic or category (e.g., *world*, *technology*, *sport*).

### Get Latest News

Retrieve the most recent BBC news articles across all available categories.

### Get Languages

List all supported BBC News languages and their corresponding codes.

## Getting Started

The BBC News API is a publicly available, unauthenticated REST API that provides BBC articles in structured JSON format.

Base URL: https://bbc-news-api.vercel.app/api

You can view full documentation and live examples here:  
[https://bbc-news-api.vercel.app/documentation](https://bbc-news-api.vercel.app/documentation)

You can call the following endpoints directly from Power Automate, Power Apps, or Logic Apps:

| Operation | Example Request |
|------------|-----------------|
| **Get News by Topic** | `https://bbc-news-api.vercel.app/api/news?topic=world&limit=5` |
| **Get Latest News** | `https://bbc-news-api.vercel.app/api/latest?limit=5&lang=en` |
| **Get Languages** | `https://bbc-news-api.vercel.app/api/languages` |

Each operation returns JSON-formatted data with fields such as `title`, `description`, `link`, `category`, `published`, and `thumbnail`.

## Obtaining Credentials

No authentication is required. This connector uses **No Auth** for public access.

## Known Issues and Limitations

- The API is unofficial and may occasionally experience rate limits or temporary downtime.
- Some endpoints may not support all BBC categories or languages.
- Response formats are subject to change if the public API is updated by the maintainers.
- An official API does exist for the BBC; however, it is only open to BBC employees at this time.
