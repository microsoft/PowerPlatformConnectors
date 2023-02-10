# Originality.AI
This is a simple connector that allows users to access their Originality.AI data from Logic App, and Power Platform and Dynamics 365 applications. With Originality.AI, users can easily check for plagiarism and detect AI-written content.

## Publisher: Osazee Odigie

## Prerequisites
To use the connector, you will have to complete the following steps:
1. Create an account on [https://app.originality.ai/](https://app.originality.ai/) and verify your account.
2. Click on **API Access** under **Menu**.  
3. Create an API key on the **My Keys** table by clicking the **Create New Key** button.
4. To start making requests: Create a header key named X-OAI-API-KEY and use your api key as the value in your application of choice
5. API Endpoint: [https://api.originality.ai/api/v1](https://api.originality.ai/api/v1)
6. Note that all request responses are returned in JSON

## Supported Operations
### Get - Credit Balance
Fetch the current credit balance of your Originality.ai account.

### Get - Credit Usage
Fetch your credit usage from your last 100 scans.

### Get - Payments
Fetch your last 100 credit purchases.

### Post - AI Detection
Run an AI Detection scan for a full piece of content.

### Post - Url AI Detection
Run an AI Detection scan on a full webpage.

## Known Issues and Limitations
No identified issue yet. <br/>
However, there is a set API usage limit clearly defined in the [terms of use](https://krozu.com/terms). You would need to contact Krozu via email: **support@krozu.com** if needs differs from the limitation.

## API Documentation
API: [https://app.originality.ai/api-access](https://app.originality.ai/api-access)
<br/>
Terms of Use: [https://originality.ai/terms-and-conditions/](https://originality.ai/terms-and-conditions/)
