# 1ToolRing
1ToolRing is a pay per call tool catalog for AI agents and flows. One API key and one prepaid balance give access to third party tools for search and SEO data, maps and places, e-commerce prices, market data and FX, web pages, weather and more. Every tool shows its price before it runs, and a failed run is not charged.

## Publisher: Khairul Muhtadin (Khaisa Studio)

## Prerequisites
You need a 1ToolRing account and an API key. Sign up at [app.1toolring.com](https://app.1toolring.com/). Running a tool spends from your prepaid balance. Discovering and inspecting tools, reading runs and searching the price list are free.

## Supported Operations
The usual flow is Discover tools, then Inspect tool to read the input schema and price, then Run tool.

### Discover tools
Find tools for a job described in plain words, with their ids and prices. This call is free.

### Inspect tool
Read the input schema, exact price, notes and health of one tool before running it. This call is free.

### Run tool
Run a tool with its inputs. The run is charged at the price shown by Inspect tool, and a failed run is not charged.

### Get tool run
Get the status and result of one of your runs. Use it to poll a run that answered with status running. This call is free.

### Search price list
Search the public price list of tools by words. This call is free.

## Obtaining Credentials
1. Sign in at [app.1toolring.com](https://app.1toolring.com/).
2. Open [Keys](https://app.1toolring.com/keys) and create an API key.
3. Paste the key into the API Key field when you create the connection. The connector sends it in the `x-api-key` header.

## Known Issues and Limitations
- Run tool spends from your prepaid balance at the price shown by Inspect tool. Top up on the Billing page in the dashboard.
- The `output` of a run is passed through from the tool provider as is, so its shape depends on the tool. Use Parse JSON with the tool's documented output if you need typed fields.
- A slow tool answers with status `running` (HTTP 202). Poll it with Get tool run every few seconds until the status is `completed` or `failed`.
- A single run has a spending cap. A run above it fails with `RUN_TOO_LARGE` and nothing is charged.

## Documentation
- Product: https://1toolring.com
- API reference: https://docs.1toolring.com/reference/tools/
- Price list: https://1toolring.com/tools/

## Deployment Instructions
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```
