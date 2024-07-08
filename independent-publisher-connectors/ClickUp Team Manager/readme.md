# ClickUp Team Manager

ClickUp is a productivity platform that helps teams manage projects and tasks. Leveraging the API in App or Automation, allows users to create spaces, folders, and lists.

## Publisher: Duke DeVan - Hitachi Solutions

## Prerequisites
To use the connector you will need:
- ClickUp account

## Supported Operations

### Get Teams
Returns every team in organization.

### Create A Folder
Creates a new folder in designated space.

### Create A Space
Creates a new space in designated team.

### Create A List
Creates a new list in designated folder.

## Obtaining Credentials

Sign in to ClickUp account and take the following steps.
1. Go to settings
  - Select Integrations
  - Select ClickUp API
  -	Select Create an App
    -	Name App
    -	Provide redirect url - global.consent.azure-apim.net
  - Select Create App – you will receive AppID and App Secret
2.Generate Code
  - Navigate to [https://app.clickup.com/api?client_id={client_id}&redirect_uri={redirect_url}](https://app.clickup.com/api?client_id={client_id}&redirect_uri={redirect_url})
  - Select Workspace
  - Select Connect
  - Copy Code from address bar – paste into notepad or text editor will be needed to generate authorization token
3. Use Code to generate Authorization Token (auth token does not expire)
  - Navigate to [https://api.clickup.com/api/v2/oauth/token?client_id={client_id}&client_secret{client_secret}=&code={code}](https://api.clickup.com/api/v2/oauth/token?client_id={client_id}&client_secret{client_secret}=&code={code})
  - Copy Authorization token and paste into text editor – will be used to create connection in power platform.
4. Create Connection (see deployment instructions)

## Known Issues and Limitations
No known issues or limitations at this point.
