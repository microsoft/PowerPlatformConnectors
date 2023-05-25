##**##HTML To PDF By Pascalcase** 

This connector can overcome the limitation of the native one drive HTML to PDF convertor which has 2mb HTML size limit, there are not many options in the market if one wants to find a way to convert more than 2mb HTML. We think for the cost which we are offering, its the cheapest in the market, without any compromises being made to the capabilities.

## Publisher: Pascalcase Software Pvt.Ltd

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

# HTML To PDF By Pascalcase
Convert HTML to PDF connectors

1. Users can use this connector by searching "HTML to PDF by pascalcase" in powerautomate "add an action".

2. Once selected they need to provide the content type {i.e. application/json}, the HTML string and the Key{ provided by pascalcase software pvt.ltd}.

3. The response is then sent back to power automate as a bytes array which can then be used as file content.

4. If the users do not have a key the response will be limited to 1 page of PDF, if the users have the key the response will be limited to 5pages and maximum HTML file of 20mb can be processed by the connector.


## Known issues and limitations

This connector can process maximum 15MB of HTML and can generate maximum 5 pages of the PDF. If there is a HTML which is more than 5pages it will descard the remaining pages and send output of only 5pages.

## Common errors and remedies

If there are any issues with the connector i.e. HTTPS timeout i.e 2min then reduce the size of the HTML and try again. 

## FAQ

1.  Can I use the connector without key?
    Yes you can, but the generated PDF will be of only 1 page.

2.  What is the maximum size of PDF which can be converted?
    Maximum 15MB of PDF can be converted currently.

3.  How many pages does the licences version of the app can process.
    Maximum 5pages of PDF can be processed currently by our system.
