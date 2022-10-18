
## Business Assist Power Connector
Business Assist APIs is used internally at Microsoft for managing the M365 support scenarios. The APIs are using Machine Learning models which will help in improving the customer experience and provide customer success. It will help to reduce the support cost and provide a better customer success experience with faster turnaround time.

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

## Supported Operations
The connector supports the following operations:

### Create Forecast
To process forecast share input historical support case volume data in the JSON format. You need to share the seasonality based on what cadence you want to get forecast results. You can retrieve the forecast output results in the JSON file format.

### Create Text Analytics
To process open free text from responses share input verbatim feedback data in the JSON format. You need to have minimum 500 responses to initiate an api call to retrieve the topic clustered with sentiments as an output results in the JSON file format.

### Get Self-help Insights
Empower your support personnel with instant access to Microsoft 365 insights, created by product teams and powered by machine learning, to assist end users in the moment of need.


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



