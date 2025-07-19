# PagePixels Screenshots connector

With PagePixels Screenshots you can:

* Easily capture full-page, viewport-only, or selector based screenshots
* Simulate user interactions before taking your screenshot (click buttons and links, fill out forms, and login to websites before taking your screenshot)
* Render custom HTML as an image / screenshot (perfect for screenshotting emails and generating custom reports from API data)
* Take screenshots from real geographic locations around the world via our residential IP proxy network (great for testing localization and SERP)
* Perform an automatic AI-powered analysis on your screenshot.

Check out the full suite of PagePixels features here: [https://pagepixels.com/app/screenshots-api-documentation](Documentation)

## Pre-requisites

You will need a PagePixels account to use this connector. PagePixels is free to start. No credit card or phone number required.

To get started, sign up at:
[https://pagepixels.com](https://pagepixels.com)

## Supported Operations

The connector provides several actions for quickly capturing screenshots and analyzing web content.

### Take a Screenshot of a web page – `QuickSnap`

Captures an instant screenshot of any URL with

* Full page, view-port, or selected element captures
* Multi-Step functionality (click buttons and links, fill out forms, login to websites before taking your screenshot)
* Blocking of ads, trackers, or cookie banners, and
* Device emulation and localization

See all available features in the [https://pagepixels.com/app/screenshots-api-documentation](API Documentation).

---

### Take a Screenshot of a web page from a real location – `RealLocationScreenshot`

Captures a screenshot from a real location within a selected country, state, or city. Perfect for localization and SERP testing.

Common use cases:

* Search engine result testing from various locations
* Geotargeted ad validation
* Regulatory compliance or localization QA

Specify the `proxy_server` value from over 200 available locations, including "USA", "Germany", "Tokyo", "California", and more.

---

### Take a Screenshot of custom HTML – `SnapHtml`

Screenshot raw HTML content that you provide. This is great for capturing emails and custom reports from API data.

Includes:

* Full rendering engine with javascript capabilities.
* Full support for all screenshot options found in the [https://pagepixels.com/app/screenshots-api-documentation](PagePixels Documentation).

---

### Take a Screenshot and analyze the image with AI – `AiAnalysisScreenshot`

This action captures a webpage and then analyzes it using AI. The analysis is based on the user's provided prompt to direct the AI. No external API AI keys are required. 

Use it to:

* Extract specific data from the webpage.
* Generate page summaries
* Identify UI elements
* Perform accessibility or marketing checks

Just pass your desired `ai_prompt` (up to 2000 characters), and PagePixels handles the capture + interpretation pipeline.

## Support and Documentation

For all support requests and documentation:
Visit [https://pagepixels.com/support](https://pagepixels.com/support)