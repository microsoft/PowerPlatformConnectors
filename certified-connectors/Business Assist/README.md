
## Business Assist Power Connector
Business Assist uses Machine Learning models which will help in improving the customer experience and provide customer success. This will help you optimize your customer support that can help you forecast call volumes, analyze customer feedback, and provide self-help support for faster service. You can integrate with any platform they already use, allowing them to create their own technology stack based on both preference and value.  

## Publisher
Microsoft Corporation

## Prerequisites
You will need the following to proceed:

* A Microsoft PowerApps or Microsoft Power Automate plan with custom connector feature.
* An Azure subscription.

## Create Azure AD app 
APIs used by the connector are secured by Azure Active Directory (AD).
* Register your application using [Azure Portal](https://portal.azure.com), by following the steps [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app).
* Create a new user and set the password.

## Create service principal for Business Assist
Make a POST call to MS Graph API with Business Assist app Id (2b8844d8-6c87-4fce-97a0-fbec9006e140) as a payload to create service principal.
POST https://graph.microsoft.com/v1.0/servicePrincipals
Content-type: application/json
{
  "appId": "2b8844d8-6c87-4fce-97a0-fbec9006e140"
}

## Supported Operations
The connector supports the following operations:

### Create Forecast
Forecast support case volume allows you to confidently forecast future support case volume based on historical data. To process forecast share input historical support case volume data in the JSON format. You need to share the seasonality based on what cadence you want to get forecast results. You can retrieve the forecast output results in the JSON file format.

### Create Text Analytics
Feedback text analytics allows you to identify key topics and sentiments from large number of free text responses from any customer/employee surveys. Allowing you to understand crucial pain points of your customers and improve the overall experience.

### Get Self-help Insights
Increase end user satisfaction and reduce your helpdesk costs by integrating with Microsoft 365 self-help, created by product teams and powered by machine learning. M365 Self-help includes instant answers, walk-through solutions and top web search results. Provide end users with instant resolution to common issues.

## Build Connection to Business Assist Connector

1. Navigate to [https://make.powerapps.com/](https://make.powerapps.com/)

2. Select Connections on the left pane.

3. Create a new connection
	- Search for 'Business Assist'.
	- Click + icon of the Business Assist connector
	- Click Create button in the popup
	- Login with the user credentials that you created in the previous section

4. Add a manual flow to test the connector
	- Create a new instant cloud flow
	- Search for Business Assist 
	- Add any action (Forecast/Text Analytics/Self-help) and test with sample input.

## Supported Operations
The BA API Power Connector supports the following operations:

* `Create Forecast Job`: Create a support ticket forecasting job based on your organization's data
* `Get Forecast Status`: Check the status of a previously submitted forecasting job
* `Get Forecast Result`: Retrieve the results of a completed forecasting job
* `Create Text Analytics Job`: Create a job to analyze a set of submitted text
* `Get Text Analytics Status`: Check on the status of a previously submitted text analysis job
* `Get Text Analytics Result`: Retrieve the result of a completed text analysis job
* `Create Self-Help Query`: Type query related to M365 product, and you will get the insights on how you can troubleshoot the issue and web search article links to learn more on how to address the issue.
