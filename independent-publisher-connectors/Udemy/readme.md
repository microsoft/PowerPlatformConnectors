# Udemy
[Udemy](https://www.udemy.com) is an online learning and teaching marketplace.

## Publisher: Nanddeep Nachan, Smita Nachan

## Prerequisites
You need to have an Udemy account. You can [sign up](https://www.udemy.com/join/signup-popup/) for free.

## Supported Operations
### Get Courses
Returns list of courses.

### Get Course Details
Returns course with specified pk.

### Get Course Reviews
Returns list of course reviews.

### Get Public Curriculum Items
Returns list of curriculum items.

## Obtaining Credentials
The Udemy Affiliate API exposes functionalities of Udemy to help developers build client applications and integrations with Udemy.
To create an Affiliate API client, Sign up on [Udemy](www.udemy.com) and go to the [API Clients](https://www.udemy.com/user/edit-api-clients) page in your user profile. Once your Affiliate API client request is approved, your newly created Affiliate API client will be active and your bearer token will be visible on the [API Clients](https://www.udemy.com/user/edit-api-clients) page.

## Getting Started
Visit [Udemy Affiliate API](https://www.udemy.com/developers/affiliate/) documentation page for further details. You may need to sign in to Udemy to view it.

## Known Issues and Limitations
Udemy Affiliate APIs are protected with [Cloudflare](https://www.cloudflare.com/) captcha solution. The API requests from the Custom Connector UI and Power Apps might get blocked due to this. To overcome this limitation, you may consider [using Privacy Pass with Cloudflare](https://support.cloudflare.com/hc/en-us/articles/115001992652-Using-Privacy-Pass-with-Cloudflare).

On the other hand, the connector works fine with Power Automate.

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
