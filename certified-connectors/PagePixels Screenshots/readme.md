# PagePixels Screenshots Connector

The **PagePixels Screenshots** connector allows you to programmatically capture website screenshots with advanced control over rendering, geolocation, device emulation, and AI-based image analysis. Simulate user interactions like filling out forms, logging in, or clicking buttons before taking a screenshot. You can also render custom HTML to capture your emails and analyze screenshots using AI prompts, all from real geographic locations using a global residential IP proxy network.

## Publisher: PagePixels LLC

## Prerequisites

To use this connector, you must have a [PagePixels account](https://pagepixels.com). Registration is free, and no credit card or phone number is required.

## Supported Operations

This connector provides the following operations:

### 1. **QuickSnap** – Take a Screenshot of a Web Page

Captures an instant screenshot of any public web page. Offers advanced customization options, including:

* Full-page, viewport-only, or selector-based captures
* Multi-step actions (form submissions, navigation, etc.)
* Blocking of ads, trackers, and cookie banners
* Geolocation and device emulation
* Header and cookie injection
* Output in JPEG, PNG, or WebP formats

📘 [API Documentation](https://pagepixels.com/app/screenshots-api-documentation)

---

### 2. **RealLocationScreenshot** – Take a Screenshot from a Real Geographic Location

Capture screenshots from real IPs in over 200 locations worldwide (countries, states, and major cities). Ideal for:

* Localization and SERP testing
* Region-specific content verification
* Compliance audits

Specify the `proxy_server` to select the desired location (e.g., `USA`, `Germany`, `Tokyo`, `California`, etc.).

---

### 3. **SnapHtml** – Take a Screenshot of Custom HTML

Render and capture screenshots from raw HTML you provide. Great for:

* Email rendering
* Custom reports based on API data
* Marketing previews

Full support for JavaScript, CSS injection, and multi-step logic.

---

### 4. **AiAnalysisScreenshot** – Take a Screenshot and Analyze It with AI

Capture a screenshot and have it automatically analyzed by AI using a custom prompt. Useful for:

* Summarizing content
* Extracting data
* Running accessibility or design audits
* Validating UI/UX heuristics

No external AI key required, just include a prompt of up to 2000 characters in length.

---

## Obtaining Credentials

This connector uses OAuth2 for authentication. You can register at [pagepixels.com](https://pagepixels.com) and use your credentials during the first connection setup in Power Platform.

## Getting Started

1. Navigate to [pagepixels.com](https://pagepixels.com) and sign up for an account.
2. In Power Automate or Logic Apps, search for **PagePixels Screenshots** and add the connector to your flow.
3. During setup, authenticate via the OAuth2 login window.
4. Choose the operation you wish to perform and configure it with URL, HTML content, or AI prompt.

📘 For feature-specific options like multi-step actions, localization settings, and rendering options, refer to the [PagePixels API documentation](https://pagepixels.com/app/screenshots-api-documentation).

## Known Issues and Limitations

* **Screenshots of authenticated pages** may require multi-step flows with login forms, which can be complex. If you run into any trouble, we can help, please contact support. 

## Frequently Asked Questions

**Q: Can I take screenshots from a specific city or state?**
A: Yes! Use the `proxy_server` parameter in `RealLocationScreenshot`. Over 200 locations are supported.

**Q: Is JavaScript rendered in the screenshot?**
A: Yes. Pages are rendered in a headless browser with full JS support.

**Q: Can I capture just a part of the page?**
A: Yes. Use the `selectors` parameter to specify a CSS selector.

**Q: Does the AI analysis require any OpenAI API keys?**
A: No. Everything is handled internally by PagePixels. Just pass your `ai_prompt`.

**Q: How do I block ads or trackers?**
A: Use the `no_ads`, `no_tracking`, and `no_cookie_banners` parameters.

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate and Power Apps.

## Support

For help, bug reports, or feature requests, visit:
👉 [https://pagepixels.com/support](https://pagepixels.com/support)