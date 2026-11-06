# CourtListener (Independent Publisher)

CourtListener is a free legal research platform operated by Free Law Project. It provides public access to millions of federal and state court opinions, PACER dockets, docket entries, oral argument recordings, and judge information. This connector enables Power Automate flows and Power Apps to search court dockets, retrieve docket details and filings, search case law opinions, and verify legal citations using the CourtListener REST API v4.

## Publisher: Dan Romano

**Stack Owner:** Free Law Project

## Prerequisites

To use this connector, you need:

- A free CourtListener account at [courtlistener.com](https://www.courtlistener.com)
- A CourtListener API token, available from your account profile

## Obtaining Credentials

1. Go to [courtlistener.com](https://www.courtlistener.com) and select **Sign in / Register**.
2. Create a free account using your email address.
3. After signing in, navigate to your profile page at [courtlistener.com/profile/api-token/](https://www.courtlistener.com/profile/api-token/).
4. Copy the token shown on that page.
5. When creating a connection in Power Automate or Power Apps, paste your token into the **API Key** field using the following format: `Token your_api_token`. The word `Token` followed by a single space must be included before the token value — entering the token alone without this prefix will result in authentication errors.

## Supported Operations

| Operation | Description |
|-----------|-------------|
| **SearchDockets** | Search federal court dockets by case name, docket number, court, or filing date. |
| **GetDocket** | Retrieve a single court docket by its CourtListener numeric ID. |
| **GetDocketEntries** | List docket entries (filings) for a specific court docket, with optional filtering by date or entry number. |
| **SearchOpinions** | Search case law opinions by keyword, court, judge, or date using full-text search. |
| **GetCitation** | Verify one or more legal citations, or extract and verify all citations found in a block of text. |

## Known Issues and Limitations

- **Rate limits:** The CourtListener API enforces the following rate limits for authenticated users: 5 requests per minute, 50 requests per hour, and 125 requests per day. All three limits apply concurrently, and the most restrictive limit controls whether a request is accepted. Flows that call this connector frequently should include delays between actions to stay within these limits.
- **Authentication:** This connector uses API token authentication. OAuth is not supported for Independent Publisher connectors at this time.
- **RECAP Fetch:** The RECAP Fetch endpoint, which retrieves documents directly from PACER on demand, is not included in this connector. Accessing PACER documents through RECAP Fetch requires a separate PACER account and incurs PACER fees. Users who need this capability should access it directly via the CourtListener website or API.
- **Pagination:** All list operations return paginated results. Use the **Cursor** parameter with the value from the **Next Page URL** field in a prior response to retrieve subsequent pages.
- **Constructing full URLs:** The `URL Path` field returned by all operations is a relative path (for example, `/opinion/10863230/case-name/`). To produce a clickable link, prepend `https://www.courtlistener.com` to this value in your flow. Opinion URLs follow the pattern `https://www.courtlistener.com/opinion/{id}/{case-name-slug}/`.
- **Coverage:** CourtListener's opinion database covers federal courts comprehensively and many state courts, but coverage varies by jurisdiction. Not all courts or time periods are available. Use the [CourtListener coverage page](https://www.courtlistener.com/coverage/) to verify coverage for a specific court before building a flow that depends on it.
