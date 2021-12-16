## My Acclaro Connector (Independent Publisher)
The My Acclaro client portal gives you on-demand access to the tools and reports you need to request, manage and review your translations.

Easy to use and navigate, our translation management platform streamlines translation workflows for your enterprise — from order creation through review.

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* A My Acclaro account

## Obtaining Credentials 
You need to be provided with a username and password for My Acclaro.

Once you have been set up, log into the Portal and get your API key (a bearer token).

## Supported Operations
The connector supports the following operations:
* Orders:
   * create-order: Creates an Order, if "string" added as parameter, then the Order takes strings rather than files.
   * add-target-lang: Adds a target language to the specified Order.
   * add-lang-pair: Adds a language pair to the specified Order.
   * get-order-details: Gets the specified Order details.
   * get-all-order-details: Gets all Order details.
   * set-order-comment: Sets a comment for the Order
   * get-order-comments: Gets the Order comments
   * submit-order: Submits the Order for preparation and then translation.
* Files:
   * send-file: Sends a source file for translation."
   * send-reference-file: Sends a reference file (e.g. a styleguide) for a particular language."
   * get-file: Gets a file based on its ID."
   * get-file-info: Gets the information of a file based on its ID."
* Strings:
   * post-string: Posts a string for translation."
   * get-string-info: Gets the string information and the translated string once completed."
* Quotes:
   * request-quote: Requests a quote for the Order."
   * get-quote-details: Gets the Quote status and details for the Order."
   * quote-decision: Approves/declines the quoted price for the Order."
