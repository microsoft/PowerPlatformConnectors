
## Business Assist Power Connector
Business Assist uses Machine learning based tools originally developed internally at Microsoft to automate common customer success-oriented tasks.  They are now being made available to help customers and partners achieve their goals to reduce costs, listen more efficiently to customers, and respond to their needs more quickly. These tools will allow large scale data analysis to be performed quickly using the low code solution. You can integrate with any platform you already use, allowing you to create your own technology stack based on both preference and value. The Business Assist connector give you tools to better understand your customers' wants and needs and respond to them more quickly.

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
Customer service organizations need to reliably predict service call volumes to allocate resources and set staffing levels appropriately. Analyzing past trends and volume levels manually is a time-consuming task. Using machine learning can speed up the process considerably. The Business Assist Forecast API brings the ability to quickly and confidently forecast future support case volume based on historical data.

Forecast support case volume allows you to confidently forecast future support case volume based on historical data. To process forecast share input historical support case volume data in the JSON format. You need to share the seasonality based on what cadence you want to get forecast results. You can retrieve the forecast output results in the JSON file format.

### Create Text Analytics
Business Assist Text Analytics service can reduce time and operation costs in feedback data analysis for open-text questions significantly. Our APIs conform to REST architectural style and allow for interaction with RESTful web services.

The Topic Clustering helps to identify and cluster similar responses together and highlight the topics that require your attention most. Feedback text analytics allows you to identify key topics and sentiments from large number of free text responses from any customer/employee surveys. Allowing you to understand crucial pain points of your customers and improve the overall experience.

### Get Self-help Insights
Organizations can improve their customers' experience by helping users solve issues on their own without ever needing to contact support. Business Assist Self Help helps find self-serve solutions from Microsoft's extensive library of support articles.

Increase end user satisfaction and reduce your helpdesk costs by integrating with Microsoft 365 self-help, created by product teams, and powered by machine learning. M365 self-help combines user intent (how users express their problem) with what we know about users to show the right self-help solution at the right time. It also returns top web search results to increase self-help success.


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
