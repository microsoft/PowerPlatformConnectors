## HTML To PDF By Pascalcase 

[HTML to PDF connector](https://pascalcase.com/Home/HTMLToPDFConverter) by [Pascalcase](https://pascalcase.com/) converts HTML to PDF and can overcome the limitation of the native OneDrive HTML to PDF convertor which has 2MB HTML size limit.

## Publisher: 

Pascalcase

## Prerequisites

The only prerequisite is the user should be able to use Power automate flow.  

## How to get credentials


$${\color{lightgreen}This \space tool \space is \space free \space to \space use \space if \space you \space just \space have \space HTML \space to \space PDF \space conversion \space of \space one \space page.}$$

Please send an email to : support@pascalcase.com if you want full access to the tool.

You will be given a key/cipher, such as the y9XSXTizWPQVfq1MYbSR!709 dummy key, which will be used inside the power automate flow as a parameter for a full version of the connector.

## Get started with your connector

1.	Search for HTML to PDF by pascalcase in 'add an actions', select the pascalcase connector. Refer Image – H1
 
![image](https://github.com/microsoft/PowerPlatformConnectors/assets/122965309/263f26aa-4f26-4318-a814-c587ac4c2136)

$${\color{lightblue}Image \space – \space H1}$$

2.	Provide HTML string/code and the key **{can be null}**.The key will be provided by [pascalcase software pvt.ltd](https://pascalcase.com/Home/HTMLToPDFConverter) to the users if they want to generate more than 1 page of PDF, else the generated PDF will be of 1 page. By default, the HTML string will have some values in it, users can change the HTML value based on their desire. Refer Image – H2

 ![image](https://github.com/microsoft/PowerPlatformConnectors/assets/122965309/944d34bd-14b5-4b40-8301-42de8c23bdb1)
 
$${\color{lightblue}Image \space – \space H2}$$

3.	Once the flow is run, the response will be a bytes array which can be used in any file content parameter either for Dataverse or for SharePoint. Refer Image – H3
 
![image](https://github.com/microsoft/PowerPlatformConnectors/assets/122965309/a9dd5a1d-867d-45fb-9318-d1364046dedb)

$${\color{lightblue}Image \space – \space H3}$$

## Known issues and limitations

This connector can create a maximum of 5 PDF pages and process up to 15MB of HTML. If an HTML file has more than 5 pages, the extra pages will be discarded, and first 5 pages will be sent as output.

## Common errors and remedies

If there are any issues with the connector i.e. HTTPS timeout(2min), then reduce the size of the HTML and try again. 

## FAQ

1.  Can I use the connector without key?

    Yes you can, but the generated PDF will be of only one page.

2.  What is the maximum size of PDF which can be converted?

    Maximum 15MB of PDF can be converted currently.

3.  How many pages does the licences version of the app can process?

    Maximum 5 pages of PDF can be processed currently by our system.
