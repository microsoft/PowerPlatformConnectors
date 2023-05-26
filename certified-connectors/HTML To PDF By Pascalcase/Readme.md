## HTML To PDF By Pascalcase 

HTML to PDF connector by Pascalcase can overcome the limitation of the native one drive HTML to PDF convertor which has 2mb HTML size limit.

## Publisher: 

Pascalcase

## Prerequisites

The only prerequisite is the user should be able to use Power automate flow.  

## How to get credentials

To get full access to the tool, please contact this email : support@pascalcase.com

There will be a key/Cipher{eg - y9XSXTizWPQVfq1MYbSR!709 dumy key}} given to you which can be used inside the power automate flow as a parameter, there is montly subscription of $50 USD which lets users convert their HTML to PDFs and a Maximum of 500 calls a month per key. Maximum size of the HTML should not be more than 15mb and maximum this tool can process 5 pages of the PDF.

## Get started with your connector

1.	Search for html to pdf converter in choose an operation area. Select the pascalcase connector. Refer Image – H1
 
 ![image](https://github.com/VikrantUpadhyay1/HTMLToPDFCustomConnector/assets/122965309/5d5e13f1-8877-4798-9f68-16c20f83767f)
 
Image – H1
2.	Provide content type, html string/code and the key {can be null}.The key will be provided by pascalcase software pvt.ltd to the users if they want to generate more than 1 page of PDF, else the generated PDF will be of 1 page. By default, the content type and html string will have some values in them, users can change the html value based on their desire. Refer Image – H2

 ![image](https://github.com/VikrantUpadhyay1/HTMLToPDFCustomConnector/assets/122965309/32eed87d-0055-478a-8c6a-3305f9899396)
Image – H2

3.	Once run the response will be a bytes array which can be used in any file content parameter either for Dataverse or for SharePoint. Refer Image – H2
 
![image](https://github.com/VikrantUpadhyay1/HTMLToPDFCustomConnector/assets/122965309/3c51d113-031f-4fb5-9629-b93e400b66c2)

Image – H3

## Known issues and limitations

This connector can create a maximum of 5 PDF pages and process up to 15MB of HTML. If an HTML file has more than 5 pages, the extra pages will be discarded, and just 5 pages will be sent as output.

## Common errors and remedies

If there are any issues with the connector i.e. HTTPS timeout(2min) then reduce the size of the HTML and try again. 

## FAQ

1.  Can I use the connector without key?

    Yes you can, but the generated PDF will be of only 1 page.

2.  What is the maximum size of PDF which can be converted?

    Maximum 15MB of PDF can be converted currently.

3.  How many pages does the licences version of the app can process?

    Maximum 5pages of PDF can be processed currently by our system.
