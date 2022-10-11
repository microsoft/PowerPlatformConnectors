# JIRA
JIRA is a software development tool for agile teams to plan, track, and release world-class software. Connecting JIRA issues to the rest of your tools helps break down barriers and unleash the potential of your team.

## Prerequisites
To use this integration, you will need a Jira account. 

## Obtaining Credentials
For authentication, you need to use an API token. To get a token, go to [this link](https://id.atlassian.com/manage/api-tokens).

## Known Issues and Limitations
- Basic authentication with passwords is deprecated. For more information, see [jira API documentation](https://developer.atlassian.com/cloud/jira/platform/deprecation-notice-basic-auth-and-cookie-based-auth/).
- JIRA Server behind a firewall or with REST API disabled is not supported.
- When creating a connection to JIRA Cloud, you need to use a valid email address for username. Otherwise, the connection will not be established, although it looks like it's successful.
- Usernames in JIRA Cloud are deprecated and cannot be used anymore for fields such as Reporter. For more information, see [Atlassian Cloud documentation](https://developer.atlassian.com/cloud/jira/platform/deprecation-notice-user-privacy-api-migration-guide/).
- As per the [Jira API documentation](https://developer.atlassian.com/cloud/jira/platform/rest/v2/#api-rest-api-2-project-search-get), jira API returns maximum 50 projects, so top 50 projects will be listed under dropdown in actions/triggers.
- Pagination was implemented on [Get projects](#get-projects). It will return all projects.
- The [Create a new issue (V2)](#create-a-new-issue-(v2)-(preview)) action supports only simple field types such as 'string', 'number', 'date', and 'datetime' in the dynamic schema. If project configuration has a required fields of complex data types, the operation will fail with the error: "Field '{key}' of type '{type}' is not supported". To workaround this, please change project fields configuration and make these fields not required.
- The `Project` property expected by the connector's actions and triggers should be filled using one of the following options:
    - Pick a project from the project picker. Note that JIRA Cloud connections show only top 50 projects in the picker.
    - Use an output from the [Get projects](#get-projects) action (project's `Project Key` property).

## Deployment Instructions
Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as custom connector in Microsoft Power Automate and Power Apps.
