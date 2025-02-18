# nBold Connector

## Overview
The **nBold** connector let you build automation with teams, channels, sites, planner and more to streamline collaboration, and better govern your collaboration environment. nBold let's you build teams templates with channels, Files templates, Folder structures, Planner board and more; Automate the creation of teams from templates at scale. 

## Features
- **Create Teams from Template**: create new team with Channels, Files, Folders, Planner, lists and more.
- **Approve or Reject Team Requests**: Approve or Reject team creation requests
- **Manage Teams**: Retrieve team details, archive teams, and access collaboration templates.
- **Security & Compliance**: Ensure proper governance and compliance across Microsoft Teams.

## Prerequisite
You will need the following 
- A Microsoft Power Apps or Power Automate premium plan for 1 account only.
- A valid Trial or a Paid subscription to nBold.
- nBold installed and connected to your tenant.


## Authentication
This connector requires OAuth authentication. To obtain credentials:
1. Go to the connector and sign-in. 
2. Give the consent to the app
3. Everything is working

## Operations

### Triggers
- **When a Team Creation Approval is Requested**: Triggers when a team creation approval request is submitted.  
- **When a Team Creation is Approved**: Triggers when a team creation request is approved.  
- **When a Team Creation is Rejected**: Triggers when a team creation request is rejected.  
- **When a Team is Created Using a Collaboration Template**: Triggers when a team is successfully created from a collaboration template.  

### Actions
- **Create a Team Based on a Collaboration Template**: Creates a new team using a predefined collaboration template.  
- **Retrieve Templates**: Fetches a list of available collaboration templates.  
- **Get Team Details**: Retrieves details of a specific team.  
- **Archive a Team**: Archives a team to restrict activity.  
- **Unarchive a Team**: Restores an archived team to active status.  
- **Invite a Member in a Team**: Adds a new member to a specified team.  
- **Get the Channels of a Team**: Retrieves all channels within a team.  
- **Create a Channel in a Team**: Creates a new channel in a specified team.  
- **Get the Primary Channel of a Team**: Fetches the default primary channel of a team.  
- **Create a Tab in a Team Channel**: Adds a tab to a specific channel in a team.  

For more details, visit [nBold API Documentation](https://docs.nbold.co/api).