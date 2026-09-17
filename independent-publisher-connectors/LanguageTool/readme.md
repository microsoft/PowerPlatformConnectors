# LanguageTool
LanguageTool is a free and open-source grammar, style, and spell checker, currently working for English, Spanish and 20 other languages. It also provides subscription based additional premium features, such as personal dictionaries or additional error checks.

This connector uses the **free, anonymous** tier of LanguageTool — no account or credentials are required. For authenticated access with Premium rules and higher limits, use the separate **LanguageTool Premium** connector.

## Publisher: Fördős András

## Prerequisites
There are no prerequisites to use this service.

## Obtaining Credentials
There are no credentials needed to use this service. It uses LanguageTool's free, anonymous tier.

## Supported Operations
### Check a text
The main feature of the service - check a text with LanguageTool for possible style and grammar issues.

### Get a list of supported languages
List the available supported languages and their variants by the service.

## Known Issues and Limitations
This connector uses only the free, anonymous tier of LanguageTool. For authenticated access — including Premium rules and the higher rate limits of a paid plan — use the separate **LanguageTool Premium** connector, which takes a username and API key. The two tiers are published as separate connectors because LanguageTool's free tier requires that *no* credentials are sent, while Premium requires them on *every* request, and a single connector cannot cover both cleanly.

The underlying service has limitations, described here: [https://languagetool.org/http-api/#/default](https://languagetool.org/http-api/#/default)
