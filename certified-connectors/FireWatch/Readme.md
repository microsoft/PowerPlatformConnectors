# FireWatch
Connect to the FireWatch service and request data for analytics.

## Publisher
[LearnPro Group](https://learnprogroup.com/)

## Prerequisites
1. An active FireWatch deployment is required. [Learn more about FireWatch](https://learnprogroup.com/firewatch/).
2. Your organisation's URL that is hosting the FireWatch service is required to connect to FireWatch in the connector.
3. An API key is also required to connect and authenticate. The API key will grant access to your FireWatch account in the connector.
4. Users of this FireWatch connector will also need permission to access to FireWatch's Analytics Views, which define data search queries.
5. The user will also need to have (an) existing Analytics View(s). Analytics Views are user-defined, so the connector will not be able to execute any views if the user has none.

## Obtaining an API Key
Users can generate Personal Access Tokens (API keys) from within their user settings in FireWatch. If you need assistance creating a FireWatch user account, accessing an existing account, or generating an API key, please contact your FireWatch administrator. Alternatively, contact support@firewatch.co.uk to reach LearnPro Group for assistance.

## Supported Operations
- `List the user's Analytics Views` - Retrieve a list of all Analytics Views that the user has access to. This simply serves as a visualisation to the user which Analytics Views they can execute; only one of them should be executed at the user's request, and not all.
- `Execute an Analytics View` - Execute one of the Analytics Views and retrieve the results.