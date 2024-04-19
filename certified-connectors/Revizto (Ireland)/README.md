# Title
Revizto unifies BIM intelligence and makes it immediately accessible and actionable for the entire project team. With Revizto's advanced Issue Tracker project team members can identify and manage model-based issues in the 3D space and 2D sheets, including addressing clash groups. Revizto provides unified access to a project's data both for 2D and 3D workflows, so anyone can use it depending on project requirements.

Revizto Connector allows you to seamlessly extract information about your projects, teams, and issue history from Revizto. This capability enables the utilization of Revizto data, and, if necessary, its integration with other data sources, to generate custom reports and dashboards featuring compelling visuals in Power BI. Revizto Connector streamlines the process of linking both applications with just a few clicks, eliminating the need to compose API requests.
## Publisher: Publisher's Name
Revizto SA
## Prerequisites
Before you use Revizto Connector, ensure that the following conditions are met:
* You are a registered Revizto user.
* You are a member of an active Revizto+ license with API access in the Ireland region.
## Supported operations
Revizto Connector supports the following operations:
| Name |  Description  | API endpoint |
| :--- | :--- | :--- |
| Get current user's licenses | Gets the licenses available to the current user. | /user/licenses |
| Get license projects | Gets the projects available in a license. | /project/list/{licenseUuid}/paged |
| Get project members | Gets the list of project members. | /project/{projectUiud}/team |
| Get issues (no filter) | Gets a list of project issues.  | /project/{projectUuid}/issue-filter/filter_all |
| Get issues | Gets a detailed list of project issues. | /project/{projectUuid}/issue-filter/filter |
| Get issue comments | Gets the issue comments that were added on the specified date or later. | /project/{issueUuid}/comments |
## Obtaining credentials
To connect to Revizto from PowerBI Desktop:
1. In the list of connectors, select *Revizto (Ireland)* and click *Connect*. You will be redirected to the Revizto Workspace web page.
1. Click *Revizto login* or *SSO* (ask your license administrator which method will work for your license).
1. Select the license region: Europe (Ireland).
1. Sign in.
## Getting started
After you sign in, perform the following steps:
1. Click *Connect*. This will open the *Navigator* window.
1. In the left pane, select a license, and then select a project under this license.
1. Under the project, select tables.
1. Click *Load*.
Now you can set up relationships between tables or work with each one individually.
## Known issues and limitations
The following limitations apply:
| Name | Calls | Renewal period |
| :--- | :--- | :--- |
| API calls per connection | 50 | 60 seconds |
## Frequently asked questions
### The list of licences in PowerBI is empty
Ensure that you are signed in to the correct region and that Revizto Connector is configured to use that region (check whether the region specified in the connector's name matches the region that you selected during the authorization).
### I can't find my license in the list
Ensure that you signed in to Revizto using the method provided by your license administrator. Different Revizto licenses might require different authentication methods
## Deployment Instructions
Required. Add instructions on how to deploy this connector as custom connector.
1. Copy the "RevCon-ireland.mez" file into [Documents]/Power BI Desktop/Custom Connectors.
2. Check the option (Not Recommended) Allow any extension to load without validation or warning in Power BI Desktop (under File | Options and settings | Options | Security | Data Extensions).
3. Restart Power BI Desktop.
4. In Power BI Desktop, click on Get Data, go to Other and navigate to Revizto (Ireland).
5. Select the Revizto (Ireland) connector and click "Connect" button. 
