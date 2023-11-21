# My Acclaro
The My Acclaro client portal gives you on-demand access to the tools and reports you need to request, manage and review your translations.

Easy to use and navigate, our translation management platform streamlines translation workflows for your enterprise — from order creation through review.

**Translation Order Management:**
* Continuous Translation API for custom system integration.
* Request quotes quickly.
* Create orders and share files/content easily for faster turnarounds.
* Access your translation memories and glossaries anytime.
* View up-to-the-minute translation work status.
* Gain visibility into your dedicated project team.
* Manage your translation budget and invoices.
* Receive notifications when orders are ready.

**Translation Review Management:**
* Manage your translation review tasks in one place.
* Give your reviewers access to a secure, dedicated review portal.
* Collaborate with reviewers without leaving the platform.


## Publisher: Acclaro Inc.
### Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature.
* A My Acclaro account.

## Supported Operations
The connector supports the following operations:
### Working with Orders
* **Create Order:** Creates an Order, if "string" added as parameter, then the Order takes strings rather than files.
* **add target lang:** Adds a target language to the specified Order.
* **add lang pair:** Adds a language pair to the specified Order.
* **get order details:** Gets the specified Order details for a specified Order.
* **get all order details:** Gets all Order details.
* **set order comment:** Sets a comment for the specified Order.
* **get order comments:** Gets the specified Order comments.
* **submit order:** Submits the specified Order for preparation and then translation.
### Working with files:
* **send file:** Sends a source file for translation.
* **send reference file:** Sends a reference file (e.g. a styleguide) for a particular language.
* **get file:** Gets a file based on its ID.
* **get file info:** Gets the information of a file based on its ID.
### Working with strings:
* **post string:** Posts a string for translation.
* **get string info:** Gets the string information and the translated string once completed.
* **get string:** Gets a string based on its ID.
### Working with quotes:
* **request quote:** Requests a quote for the Order.
* **get quote details:** Gets the Quote status and details for the Order.
* **quote decision:** Approves/declines the quoted price for the Order.

## Obtaining Credentials 
You need to be provided with a username and password for [My Acclaro](https://my.acclaro.com). To request the credentials, please contact support@acclaro.com or go [here](https://info.acclaro.com/my-acclaro-registration).

Once you have been set up, log into the Portal and get your API key (a bearer token).

### Steps to obtain the token
1. Log into the portal: https://my.acclaro.com
2. On the top menu, go to 'API > Token'
3. On the new screen you will find a field called 'Your Web Token', you can copy it to the clipboard.
4. Add the token preceeded by the word 'Bearer: '.
  e.g. if your token is "abcd1234ZXYF", then you should input the API key at PowerAutomate as "Bearer: abcd1234ZXYF".

## Known Issues and Limitations
* The maximum amount of results per page is 50, it is recommended to paginate those queries that are expected to return more than 50 results.
* The maximum amount of requests per minute allowed for each IP is 180; please contact support if this causes a limitation for you.

## Deployment Instructions
1. At custom connectors, import the API specification as an openAPI file: `apiDefinition.swagger.json`.
2. Assign a name and a logo for the connector.
3. Click on "Save connector".

## Frequently Asked Questions
1. Does Acclaro provide a staging environment to test the API?

*Acclaro provides a sandbox API environment which you can make API requests to. To access this environment you can change the endpoint to 'apisandbox.acclaro.com' and obtain a Bearer token for this environment accordingly. If you would like to change back to the production endpoint you will need to change to 'my.acclaro.com'.*

2. Does Acclaro charge any fees for using the My Acclaro Portal?

*No, Acclaro charges for the services requested through the portal My Acclaro, but the use of the portal and its API is free for all Acclaro customers.*