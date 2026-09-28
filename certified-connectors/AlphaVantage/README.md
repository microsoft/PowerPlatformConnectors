# Alpha Vantage Connector

## Publisher: Alpha Vantage Inc.

Access 200+ financial market data functions through Model Context Protocol (MCP). Alpha Vantage provides real-time and historical data for stocks, forex, cryptocurrencies, technical indicators, fundamental analysis, and economic indicators.

## Prerequisites

You will need an Alpha Vantage API key to use this connector. Get your free API key at: https://www.alphavantage.co/support/#api-key

## Supported Operations

The connector uses Model Context Protocol (MCP) JSON-RPC interface to provide dynamic access to all Alpha Vantage functions. AI agents and applications can discover available tools and resources automatically.

### Core Operations

- **Execute MCP Request**: Send JSON-RPC requests to the Alpha Vantage MCP server
  - `tools/list`: Discover all available financial data functions (200+)
  - `tools/call`: Execute specific financial data queries
  - `resources/list`: List available data resources
  - `resources/read`: Read resource data

## Available Tools (Examples)

The MCP server exposes 200+ tools including:

### Stock Market Data
- TIME_SERIES_INTRADAY, TIME_SERIES_DAILY, TIME_SERIES_WEEKLY, TIME_SERIES_MONTHLY
- GLOBAL_QUOTE - Real-time stock quotes
- MARKET_STATUS - Current market status worldwide

### Technical Indicators
- SMA, EMA, MACD, RSI, STOCH, ADX, CCI, AROON, BBANDS
- 50+ technical analysis indicators

### Fundamental Data
- COMPANY_OVERVIEW - Company information and financial ratios
- INCOME_STATEMENT, BALANCE_SHEET, CASH_FLOW
- EARNINGS, EARNINGS_CALENDAR
- DIVIDENDS, SPLITS

### Options Data
- REALTIME_OPTIONS - Real-time options chains
- HISTORICAL_OPTIONS - Historical options data

### News & Intelligence
- NEWS_SENTIMENT - Market news with sentiment analysis
- EARNINGS_CALL_TRANSCRIPT - Earnings call transcripts

### Forex & Crypto
- FX_DAILY, FX_WEEKLY, FX_MONTHLY - Foreign exchange rates
- DIGITAL_CURRENCY_DAILY, DIGITAL_CURRENCY_WEEKLY, DIGITAL_CURRENCY_MONTHLY
- CURRENCY_EXCHANGE_RATE

### Commodities & Economic Data
- WTI, BRENT, NATURAL_GAS - Energy prices
- COPPER, ALUMINUM - Metal prices
- REAL_GDP, CPI, INFLATION, UNEMPLOYMENT, FEDERAL_FUNDS_RATE

## Getting Started

### 1. Obtain API Key
Sign up for a free API key at https://www.alphavantage.co/support/#api-key

### 2. Create Connection
In your Power Platform application, create a new connection using your API key.

### 3. Discover Available Tools
Use the `tools/list` method to see all available functions:

```json
{
  "jsonrpc": "2.0",
  "method": "tools/list",
  "params": {},
  "id": 1
}
```

### 4. Call a Tool
Use `tools/call` to execute a specific function:

```json
{
  "jsonrpc": "2.0",
  "method": "tools/call",
  "params": {
    "name": "TIME_SERIES_DAILY",
    "arguments": {
      "symbol": "IBM",
      "outputsize": "compact",
      "datatype": "json"
    }
  },
  "id": 2
}
```

## Example Use Cases

### Stock Price Tracker
Monitor real-time stock prices and create alerts for price movements.

### Technical Analysis Dashboard
Build dashboards showing technical indicators (RSI, MACD, Bollinger Bands) for portfolio stocks.

### Portfolio Performance Monitor
Track portfolio performance with real-time quotes, dividends, and splits data.

### Market News Aggregator
Aggregate and analyze market news sentiment for specific stocks or sectors.

### Economic Indicator Tracker
Monitor economic indicators (GDP, CPI, unemployment) and correlate with market movements.

### Forex Trading Assistant
Track currency exchange rates and create forex trading workflows.

## Rate Limits

- **Free tier**: 25 API requests per day
- **Premium tiers**: Higher rate limits available at https://www.alphavantage.co/premium/

## Known Issues and Limitations

- Rate limits apply based on your subscription tier
- Some premium-only features require paid API keys
- Historical intraday data availability varies by symbol

## Deployment Instructions

This connector is submitted for Microsoft certification to be available in:
- Power Automate
- Power Apps
- Logic Apps
- Microsoft Copilot Studio

Once certified, users can add it from the official connector catalog.

## Support

### Alpha Vantage Support
- Website: https://www.alphavantage.co
- Email: support@alphavantage.co
- Documentation: https://www.alphavantage.co/documentation/
- MCP Server: https://mcp.alphavantage.co/mcp

### Connector Issues
For issues specific to this Power Platform connector, please contact support@alphavantage.co

## Additional Resources

- API Documentation: https://www.alphavantage.co/documentation/
- MCP Server Documentation: https://github.com/alphavantage/alpha_vantage_mcp
- Community Forum: https://www.alphavantage.co/community/
- FAQ: https://www.alphavantage.co/support/#support

## Changelog

### Version 1.0.0 (Initial Release)
- MCP-based connector providing access to 200+ Alpha Vantage API functions
- Support for stocks, technical indicators, fundamentals, forex, crypto, commodities, and economic data
- Automatic tool discovery via Model Context Protocol
- JSON-RPC interface for flexible querying
