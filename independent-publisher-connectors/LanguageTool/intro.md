# LanguageTool

LanguageTool is a free and open-source grammar, style, and spell checker for English, Spanish, and more than 20 other languages. This connector uses the **free, anonymous** tier of LanguageTool — no account or credentials are required. For authenticated access with Premium rules and higher limits, use the separate **LanguageTool Premium** connector.

## Publisher

Fördős András (Independent Publisher)

## Prerequisites

None. This connector uses LanguageTool's free, anonymous tier — no account or credentials are required.

## Supported Operations

### Check a text
Check a text with LanguageTool for possible style and grammar issues, and get suggested replacements.

### Get a list of supported languages
Get a list of supported languages and their variants.

## Known Issues and Limitations

- This connector uses the free, anonymous tier of LanguageTool against `api.languagetoolplus.com`; the available rules and rate limits are those of the free service.
- Either `text` or `data` must be provided when checking a text; `data` lets you mark which parts are text and which are markup.
- **Free and Premium access are published as two separate connectors, for a technical reason.** LanguageTool's free tier requires that *no* credentials are sent, while Premium requires a username and API key on *every* request. A single connector cannot cover both cleanly, so this **LanguageTool** connector handles anonymous (free) access, and the separate **LanguageTool Premium** connector handles authenticated access with Premium rules and higher limits.
