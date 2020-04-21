# RiskIQ Internet Intelligence

RiskIQ Internet Intelligence provides organizations with historic insights about Internet infrastructure, so they can enrich security-based information such as logs, alerts and incidents. RiskIQ has been collecting Internet data for over 10 years by using a combination of web crawlers, global proxies and mass portscanning of the Internet. To learn more about RiskIQ, visit our [website](https://www.riskiq.com) or register for a free account on our analyst platform, [PassiveTotal](https://community.riskiq.com/).

## Pre-requisites

You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* An Azure subscription
* The Power platform CLI tools
* [RiskIQ API credentials](https://api.riskiq.net/api/concepts.html)

## Supported Operations

The connector supports the following operations:
* `Passive DNS`: Search across historic DNS data to identify overlaps between infrastructure hosting.
* `WHOIS`: Search across current and historic WHOIS records to identify shared registrations through common attributes like email addresses, phone numbers and organization names.
* `SSL Certificates`: Search across current and historic SSL Certificate records and their hosting history.
* `Web Components`: Reveal technologies used for hosting and web applications for particular infrastructure.
* `Host Pairs`: Identify relationships that exist between websites through various causes such as iFrames, script references, images and CSS.
* `Trackers`: Reveal technologies used for in web applications for particular infrastructure.
* `Cookies`: Identify cookies deployed from web applications for particular infrastructure.

## How to get credentials

Register for a test API key at [RiskIQ Security Intelligence Services](https://api.riskiq.net/api/concepts.html) or contact your account representative (support@riskiq.com) to identify your existing customer keys.

## Deployment instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps. Additionally, you can leverage this connector within Logic Apps.
