# Starmind
Unlock the right expertise exactly when you need it.  
Starmind’s advanced knowledge engine identifies experts within your organization and instantly connects you to their insights—seamlessly integrating human intelligence into your daily workflows.

The Starmind Connector enables seamless integration with Starmind’s knowledge engine, allowing users to access verified answers and to connect with experts across their organisation. By connecting to Starmind, Power Automate workflows or Copilot agents can intelligently retrieve responses to business-critical questions, surface expert knowledge, and streamline decision-making processes.

## Publisher: Publisher's Name
Starmind

## Prerequisites
To use this integration, you will need a Starmind network prepared for the integration. Please, request the setup via the Starmind [technical support channel](https://starmind.atlassian.net/servicedesk/customer/portal/2).
The connector can be used with any Starmind plan.

## Supported Operations

### FindExpert
Finds experts based on the provided content.

### FindQuestion
Finds questions based on the provided query parameters.

### GetQuestion
Retrieves the complete details for a question, including solutions, comments, and concepts by its ID.

### PostQuestion
Creates a new question draft.

### PublishQuestion
Publishes question draft.

### ResolveUser
Resolves the globalUserId to user object. Provide the graphQL query to retrieve the user object as follows

```graphql
query getUser($globalUserId: UUID!) {
  user(globalUserId: $globalUserId) {
    globalUserId
    firstname
    lastname
    email
    languageId
    avatarUrl
    company
    department
    position
    location
    about
    countryId
    created
    dateDeleted
    isFederated
  }
}
```

## Obtaining Credentials
To obtain an API key for Starmind, you need to contact your Starmind account manager or support team via our [technical support channel](https://starmind.atlassian.net/servicedesk/customer/portal/2)

## Known Issues and Limitations
This connector authenticates exclusively via API key.
- Access is performed through a technical user account.
- OAuth authentication in the context of the signed-in user is not supported.

## Frequently Asked Questions

### What is required to use this connector with a Starmind network?
To use this integration, you will need a Starmind network prepared for the integration. Please, request the setup via the Starmind [technical support channel](https://starmind.atlassian.net/servicedesk/customer/portal/2).

### How can I get a new API key?
To obtain an API key for Starmind, you need to contact your Starmind account manager or support team via our [technical support channel](https://starmind.atlassian.net/servicedesk/customer/portal/2)

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector in Microsoft Power Automate.

