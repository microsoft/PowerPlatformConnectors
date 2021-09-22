ClickUp Team Manager

ClickUp is a productivity platform that helps teams manage projects and tasks. Leveraging the API in App or Automation, allows users to create spaces, folders, and lists. 

Publisher: Duke DeVan - Hitachi Solutions

Prerequisites
To use the connector you will need:
•	ClickUp account 

Supported Operations

1. Get Teams
  Returns every team in organization.

2. Create A Folder
  Creates a new folder in designated space.

3. Create A Space
  Creates a new space in designated team.

4. Create A List
  Creates a new list in designated folder.

Obtaining Credentials

Sign in to ClickUp account and take the following steps.
1.	Go to settings
  a.	Select Integrations
  b.	Select ClickUp API
  c.	Select Create an App
    i.	Name App
    ii.	Provide redirect url - global.consent.azure-apim.net
  d.	Select Create App – you will receive AppID and App Secret
2.	Generate Code
  a.	Navigate to https://app.clickup.com/api?client_id={client_id}&redirect_uri={redirect_url}
  b.	Select Workspace
  c.	Select Connect
  d.	Copy Code from address bar – paste into notepad or text editor will be needed to generate authorization token
3.	Use Code to generate Authorization Token (auth token does not expire)
  a.	Navigate to https://api.clickup.com/api/v2/oauth/token?client_id={client_id}&client_secret{client_secret}=&code={code}
  b.	Copy Authorization token and paste into text editor – will be used to create connection in power platform.
4. Create Connection (see deployment instructions)

Known Issues and Limitations
No known issues or limitations at this point.

Deployment Instructions
1. paconn login
2. paconn create --api-prop [Path to apiProperties.json] --api-def [Path to apiDefinition.swagger.json] --icon [Path to icon.png]

Once connector has been downloaded to environment create connection using authentication token by
•	Edit connector
•	Select “Test” tab
•	"+ New Connection"
•	Enter Authorization token
