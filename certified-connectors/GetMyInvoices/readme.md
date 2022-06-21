
# GetMyInvoices
GetMyInvoices makes effective invoices processing easy indeed.

## Pre-requisites
This is our website url. (https://www.getmyinvoices.com)
You can sign up a new trial account, and can login with that account.
After login, please go to "Documents" menu on the left sidebar.
Here you can see invoice documents of our GetMyInvoices App, and you can see also "UPLOAD DOCUMENT" button in the top.
You can upload a new invoice documents by clicking this button.
After uploading invoice documents, you can get these invoices one by one by using our connector.
(we can get only last invoice by using this connector.)

## How to get credentials
We don't need any authentication process in our connector.
We will use "api_key" in the post body for connecting to our GetMyInvoices App.
In order to get a "api_key", you can go to this page. (https://login.getmyinvoices.com/api_access.php)
Here you can see existing "api_key" and you can also create a new "api_key".
You can also add a new "api_key" with Permission (for example, Full-permission).

## API documentation
https://api.getmyinvoices.com/accounts/v2/doc/

## Known issues and limitations
As I mentioned above, our connector will be used for getting last invoice from GetMyInvoices.
(We can get the following values of last invoice.
Invoice Id, Company Uid, Company Name, Document Type, Document Date, Payment Status, ...)
This means if you run our connector once, then it will take one invoice data, and last invoice id will be increased.
Next, if you run our connector again, then we will get next invoice data according to last invoice id, and last invoice id will be also increased.
In this way, last invoice id will be increased, if last invoice id is reached to limit, then connector will return empty data.

## Further Support
For further support, please contact support@getmyinvoices.com
