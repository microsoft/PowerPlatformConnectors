# MiniSoup HTML Parser

## Summary

MiniSoup is a lightweight HTML parsing library for Power Automate, inspired by Beautiful Soup. It enables direct HTML parsing, data extraction, and web analysis within Power Automate flows without requiring external services.

## Prerequisites

There are no prerequisites for using this connector as it doesn't require authentication.

## Publisher

Shogo Shindo

## Supported Operations

### Fetch HTML
Fetches HTML content from a specified URL.

### Select Elements
Selects HTML elements matching the provided selector.

### Extract Values
Extracts specific attribute values from HTML elements matching the provided selector.

### Find All Elements
Finds all HTML elements matching the specified tag name and optional attributes.

### Parse Table
Parses an HTML table into structured data with headers and rows.

## Obtaining Credentials

This connector does not require authentication.

## Known Issues and Limitations

- Processing very large HTML documents (multiple MB) may hit timeout limits
- JavaScript-generated content on websites cannot be parsed (only static HTML)
- Limited support for complex CSS selectors and XPath expressions

## Frequently Asked Questions

### Can I use this to scrape websites?
Yes, but please ensure you comply with the terms of service of the websites you're accessing.

### Does this work with dynamic content?
No, this connector can only parse static HTML content that is returned from the initial page load.

### Is there a limit to how much data I can process?
Power Automate has a 2-minute timeout limit for connector operations, which may be reached with very large HTML documents.