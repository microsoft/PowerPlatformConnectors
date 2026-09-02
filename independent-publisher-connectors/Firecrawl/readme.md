# Firecrawl Connector

The Firecrawl connector bridges Power Automate with Firecrawl's web scraping infrastructure. It allows you to automate browser interactions, perform deep website crawls, and execute web searches to convert complex web content into clean, LLM-ready markdown.

## Publisher: Anil Goud Gunda

## Prerequisites
To use this connector, you need a valid Firecrawl account and an active API Key. You can sign up and retrieve your key at [firecrawl.dev](https://firecrawl.dev).

## Supported Operations

### Create Interact Session
Initializes a standalone browser session to be used for interactive automation.

### Run Code
Executes Playwright, Puppeteer, or agent-browser scripts (Python/Node/Bash) within an active browser session.

### Close Session
Destroys an active Interact browser session and releases resources to prevent unnecessary billing.

### Start Crawl
Initiates an asynchronous crawl on a website. Returns a unique Crawl ID used to fetch results once processing is complete.

### Get Crawl Results
Retrieves the status and data (markdown/HTML) for a specific Crawl ID.

### Search & Scrape
Performs a web search based on a query and automatically scrapes the resulting pages into markdown, HTML, or structured JSON.

## Obtaining Credentials
1. Visit [firecrawl.dev](https://firecrawl.dev) and sign up for an account.
2. Navigate to your dashboard and generate an API key.
3. When configuring the connector in Power Automate, paste your API key into the "API Key" field. The connector handles the `Bearer` token authentication automatically.

## Getting Started
1. Add the Firecrawl connector to your Power Automate flow.
2. Authenticate using your API key.
3. For **Interact** actions: Use "Create Interact Session" followed by "Run Code". Add a 5-second delay between these steps to ensure the session initializes.
4. For **Crawl** actions: Use "Start Crawl", followed by a "Do Until" loop that calls "Get Crawl Results" until the status returns as "completed".

## Known Issues and Limitations
*   **Asynchronous Processing:** The "Crawl" operation is asynchronous. You must implement a polling loop (Do Until) in your flow to check for the "completed" status before processing the results.
*   **Session Initialization:** When creating a browser session for code execution, a small delay (approx. 5 seconds) is recommended before running code to ensure the browser instance has fully stabilized.
*   **Rate Limits:** Execution is subject to the rate limits associated with your Firecrawl subscription plan.

## Frequently Asked Questions

### Question 1
**Why does my code execution fail with "TargetClosedError"?**
This usually occurs if the browser session is still initializing or if the session has timed out. Ensure you are using the "Delay" action before running code, or check your "TTL" settings.

### Question 2
**How do I save credits?**
Always ensure you use the "Close Session" action at the end of your flows that use browser sessions. Otherwise, the session will remain active until the `activityTtl` expires, consuming your credits.

## Deployment Instructions
1. Navigate to **Custom Connectors** in the Power Automate portal.
2. Select **New custom connector** and choose **Import an OpenAPI file**.
3. Upload the `apiDefinition.swagger.json` file.
4. On the **Security** tab, select **API Key** authentication.
5. Save the connector and test using the operations listed above.