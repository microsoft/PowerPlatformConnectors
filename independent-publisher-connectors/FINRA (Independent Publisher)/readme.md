## FINRA Connector

The FINRA Data Connector provides access to a wealth of financial market data maintained by the Financial Industry Regulatory Authority (FINRA). This includes information on OTC market activity, trading volume, and regulatory filings. The connector supports endpoints for Equity and Fixed Income data, offering freemium access to critical financial insights.

## Publisher

Dan Romano (swolcat)

## Prerequisites

- Bookmark and use this link: https://developer.finra.org/docs
- Authorization: https://developer.finra.org/docs#getting_started-api_platform_basics-authorization
- FINRA support: https://developer.finra.org/support
- Connect to FINRA via Postman: https://developer.finra.org/UsingPostmantocalltheFINRAAPIPlatform
- Fees: Free accounts are available with restrictions. Premium accounts: https://developer.finra.org/fees

## Obtaining Credentials

To access the FINRA data services, you will need an API key. You can obtain this by registering on the FINRA API portal and requesting access. It is important to note that FINRA requires OAuth2 authentication. Follow the instructions provided in the FINRA documentation.

- Instructions: https://developer.finra.org/docs
- Further instructions: https//developments.substack.com/p/paper-trails
- It is recommended to create two credentials, one for Public datasets and another for Mock datasets.

## Supported Opeartions

Freemium access includes: Summaries from Equity endpoints (Weekly, Monthly, OTC)

Action 1 - Blocks Summary - This endpoint provides a comprehensive summary of weekly trading activity in the equity markets, including both ATS and non-ATS trades.

Action 2 - Monthly Summary - This endpoint delivers a detailed monthly summary of trading activities in the equity markets, focusing on OTC trades.

Action 3 - OTC Block Summary - This endpoint offers a summary of significant block trades in the OTC market, highlighting large volume transactions.

## Known Issues and Limitations

There are no known issues at this time but paging datasets is recommended for handling large result sets.

## Frequently Asked Questions

### Does this contain stock market data?

No. Use the SEC Independent Publisher connector from either Dan Romano or Gulshan Khurana.

### What does this do then?

The output for each endpoint is a delimited data containing comprehensive summaries of weekly, monthly, and significant block trading activities in the equity markets, helping users analyze market trends and movements.

## Deployment Instructions

None to note.

