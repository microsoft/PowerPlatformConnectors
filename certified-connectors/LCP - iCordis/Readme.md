# LCP - iCordis
With the LCP - iCordis connector you can transfer data to the e-loket forms on your website.

## Publisher: LCP nv

## Prerequisites
You will need the following to proceed:
* An account on the LCP (iCordis) backoffice
* Client/Secret to login in to iCordis

## Supported Operations
The connector supports the following operations:
### `Get submission(s) for an iCordis e-loket form`
Get the submissions for a given form in json-format. Parameters determine what data returns.
#### Parameters
   * `formid`: Required parameter. The ID of the form to get the submissions for.
   * `entryid`: Optional parameter. The entry ID of the submission to get.
   * `lastsynch`: Optional parameter. Date-Time from which the submissions must be collected. When empty, all submissions are returned.
   * `includeFiles`: Optional parameter. When true, the attachments will be included as base64. Otherwise, a link to the attachment will be provided.
   * `includePDF`: Optional parameter. When true, a PDF will be included with the form and the entered values (in base64).
   * `includeHTML`: Optional parameter. When true, a HTML file will be included with the form and the entered values (in base64).
   * `page`: Optional parameter. When included, only 10 submissions will be returned. This parameter can then be used to get the next page. Otherwise, all submissions will be returned.  

### `Delete trigger`
Unsubscribe from the webhook trigger for the given form.
#### Parameters
* `formid`: Required parameter. The ID of the form to unsubscribe from.
* `webhookURL`: Required parameter. The subscriber url to delete.
## Supported Triggers
The connector supports the following triggers:  
* `When a new submission is created for the e-loket form`: Get a notification when a new submission is created for the e-loket form and receive the ID of the new submission.

## Obtaining Credentials
1. Create an account in the CBO (https://#yourcbolink#/module/index/504)
2. Send a request to helpdesk@lcp.be to give the created account access to the API.

## Known Issues and Limitations
Only LCP customers with the e-loket functionality can use this connector. 

## Deployment Instructions
Please use [these instructions](https://learn.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.



