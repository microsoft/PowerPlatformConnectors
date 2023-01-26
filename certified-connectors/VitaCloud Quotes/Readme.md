# VitaCloud Quotes Connector
VitaCloud Quotes API provides a curated set of quotes by famous authors and celebrities, tagged with various themes such as faith, god, happiness, hope, humour, inspiration, knowledge, life lessons, love, motivation, philosophy, poetry, relationship, religion, romance, science, time, truth, and wisdom. VitaCloud Quotes API connector is a wrapper around the VitaCloud Quotes API that enables bringing the tailored, daily and random VitaCloud Quotes into your workspace using Microsoft Power Automate, Microsoft Power Apps, and Azure Logic Apps in your Azure and Office 365 subscription. 

## Publisher: VitaCloud Limited
<a href="https://www.vitacloud.co.uk" target="_blank">VitaCloud Website</a><br/>
<a href="https://portal.vitacloud.co.uk/home" target="_blank">VitaCloud API Portal</a><br/>
<a href="mailto:support@vitacloud.co.uk">VitaCloud Support</a><br/>

## Prerequisites
You will need the following to proceed:
* A VitaCloud API Base URL
* A VitaCloud API Key
* A Microsoft Power Apps or Power Automate plan with custom connector feature
* The Power platform CLI tools

## Supported Operations
The connector supports the following operations

* `Get authors`: Get all authors
* `Get author`: Get a specific author by `authortag`
* `Get themes`: Get all themes
* `Get theme`: Get a specific theme by `themetag`
* `Get quotes`: Get 5 random quotes
* `Get quotes by number`: Get the specified number of random quotes (max 10)
* `Get quotes by author`: Get 5 random quotes for specific author by `authortag`
* `Get quotes by author and number`: Get the specified number of random quotes for specific author by `authortag` (max 10)
* `Get quotes by theme`: Get 5 random quotes for specific theme by `themetag`
* `Get quotes by theme and number`: Get the specified number of random quotes for specific theme by `themetag` (max 10)
* `Get random quote`: Get a random quote
* `Get today's quote`: Get today's quote

## Obtaining Credentials
### Steps to generate the API Base URL and API Key

1. Sign up or log in to the <a href="https://portal.vitacloud.co.uk/home" target="_blank">VitaCloud API Portal</a><br/>
<img src="https://vitacloud.blob.core.windows.net/company/portalhome.JPG" alt="Portal home" width="1200"/>

2. Subscribe to one of the free or paid products from the Product catalog<br/>
<img src="https://vitacloud.blob.core.windows.net/company/products.JPG" alt="Product catalog" width="1200"/>

3. Select the subscribed product in the portal<br/>
<img src="https://vitacloud.blob.core.windows.net/company/selectproduct.jpg" alt="Select subscribed product" width="1200"/>

4. Copy the **Base Product URL** and **API Key** from the **Authentication** tab<br/> 
<img src="https://vitacloud.blob.core.windows.net/company/authentication.jpg" alt="Authentication" width="800"/>

> The **Base Product URL** and **API Key** will be required for connecting to the Quotes connector in Power Automate and Power Apps.

## Known Issues and Limitations

* Please note some functionality may be limited based on the subscribed product. E.g. if only **Today's Quote** product is subscribed, only the **Get Today's Quote** action will work in the connector. 
* We recommend subscribing to the **Quotes by themes and authors** product to unlock all options in the connector.
* If number of quotes is not specified for operations requiring them, then at most 5 quotes will be returned in the response.
* Number of quotes returned will be limited to a maximum of 10.
* Some authors may not have a corresponding Wikipedia link.
* Author images are subject to change, but will maintain the same size and aspect ratio.
* If an incorrect Base Product URL or API Key is specified, HTTP status code `404 - not found` will be returned, alongwith a response as below <br/>
`{"requestId": long string, "message": "No data product found. Please contact your data provider"}` <br/>
Please update the connection with the correct **Base Product URL** and **API Key** from the API portal.

* If an action is used that is not included in the product corresponding to the Base Product URL, HTTP status code `403 - forbidden` will be returned, alongwith a response as below <br/>
`{"requestId": long string, "message": "Endpoint not available"}` <br/>
Update the connection with the **Base Product URL** corresponding to the product containing the requested endpoint.

## Frequently Asked Questions
* **How can I track the number of calls made for the subscribed product?**<br/>
In the <a href="https://portal.vitacloud.co.uk/home" target="_blank">VitaCloud API Portal</a>, select the subscribed product.<br/>
<img src="https://vitacloud.blob.core.windows.net/company/selectproduct.jpg" alt="select action" width="800"/><br/>
The **Overview** tab for the selected product will show the number of API calls made against the available quota.

* **How can I consume the APIs directly in a website or app?**<br/>
Download the OpenAPI 3.0 specification for your subscribed endpoints on the **Overview** tab for your product. Please include the API key in the header for your request to the endpoints. Select an endpoint in the **Endpoints** tab to try out a request in the portal. You can also copy code snippets available in Curl, JavaScript, Python, Go, Ruby and PhP from the portal.

* **What themes and authors are included?**
The Quotes connector `Get Themes` action provides over 20 themes to choose from, including popular ones like humour, wisdom, science, inspiration, philosophy and happiness. The `Get Authors` action provides details for over 700 authors, personalities and celebrities. Most endpoints also include authors' images and wikipedia links, where available.<br/>
<img src="https://vitacloud.blob.core.windows.net/company/authors1.png" alt="authors" width="400"/>
<img src="https://vitacloud.blob.core.windows.net/company/themes.png" alt="themes" width="400"/>
<img src="https://vitacloud.blob.core.windows.net/company/authors2.png" alt="authors" width="400"/>

* **Where can I reach out if I have a product query or require support?**<br/>
Please send your query to <a href="mailto:support@vitacloud.co.uk">VitaCloud Support</a> or fill out the contact form on the <a href="https://www.vitacloud.co.uk" target="_blank">VitaCloud Website</a>.

## Deployment Instructions
Run the following commands and follow the prompts:

```paconn
paconn create --api-def apiDefinition.swagger.json --api-prop apiProperties.json
```