# iObeya

This connector exposes a subset of the powerful iObeya REST API endpoints as operations available for both Microsoft Power Automate and Power Apps. It allows you to build and automate workflows using these two services on top of your iObeya platform.

## Publisher: iObeya

iObeya boosts the agility of your organization by bringing teams together on a single visual collaboration platform ready to support all your Visual Management practices. In compliance with your strictest security requirements as validated by an ISO-27001 certification, iObeya can be connected to any data source to create a visual cockpit. This gives teams the ability to visualize business data for better decision making and quicker problem solving.

## Prerequisites
You will need the following to use this connector:
* An iObeya platform with an Enterprise subscription plan
* A Microsoft Power Apps or Power Automate plan allowing you to use custom connectors
* A dedicated OAuth application endpoint for external clients declared via the iObeya platform   administration interface: 
  * As a platform admin user connected to the iObeya administration interface, click on the SETTINGS > API 
  * Create a new OAuth application endpoint by clicking on the “Add OAuth application” button
  * Configure the OAuth application and provide https://global.consent.azure-apim.net/redirect as a RedirectUri

## Supported Operations
The iObeya connector supports the following operations:

### List of rooms
Retrieve list of your rooms

### List of boards
Retrieve list of your boards

### Create Card
Create a card on a board.  
Supported types of card :

* Standard
* Activity
* Feature
* Story
* ProblemSolving
* QCD Action

### QCD Indicators Bulk Update
Bulk update your QCD indicators from various data sources (Excel, SAP, etc..)

### List activity card
Retrieve all activity cards by date range from a planning board

## Contact Us

Don’t hesitate to contact us if you have any question, or you want to share your feedback regarding our connector via integrations-support@iobeya.com.