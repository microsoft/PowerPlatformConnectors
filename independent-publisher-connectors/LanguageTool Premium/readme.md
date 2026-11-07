# LanguageTool Premium

Check texts for style and grammar issues with [LanguageTool](https://languagetool.org/). This connector authenticates with your LanguageTool Premium account (username + API key), giving you authenticated access, including the Premium rules and higher limits of your plan. For anonymous, unauthenticated access use the free **LanguageTool** connector instead.

## Publisher: Fördős András

## Prerequisites

A LanguageTool account with API access. Authenticated access (and Premium checks/limits) requires a LanguageTool Premium subscription.

## Obtaining Credentials

1. Sign in to your [LanguageTool account](https://languagetool.org/).
2. Open **Account settings > API access** ([direct link](https://languagetool.org/editor/settings/api)).
3. Note your **Username** (the email address of your account) and copy your **API key**.
4. When you create a connection, enter your **Username** and **API key**. The connector sends them with every request; no other setup is required.

## Supported Operations

### Check a text
Check a text with LanguageTool for possible style and grammar issues, and get suggested replacements.

### Get a list of supported languages
Get a list of supported languages and their variants.

## Known Issues and Limitations

- This connector uses authenticated (account-based) access against `api.languagetoolplus.com`. The results, rules, and rate limits available depend on your LanguageTool plan.
- Either `text` or `data` must be provided when checking a text; `data` lets you mark which parts are text and which are markup.
- **Free and Premium access are published as two separate connectors, for a technical reason.** LanguageTool's free tier requires that *no* credentials are sent, while Premium requires a username and API key on *every* request. A single connector cannot cover both cleanly: the credentials are added to the request by a connector-wide policy, which requires the connection fields to always be present. A field left blank for the free tier is dropped from the connection by the platform, which makes the policy fail (`Property with name username is not present in connection parameters`); a required field, in turn, cannot be left blank. Because the two tiers are irreconcilable at the connection level, this **LanguageTool Premium** connector handles authenticated (Premium) access, and the separate free **LanguageTool** connector handles anonymous access.
