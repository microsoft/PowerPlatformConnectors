# iObeya Connector

iObeya provides a powerful REST API. Using this API, you can create and manage data on your platform (e.g. board, card, qcd indicators, etc.) in iObeya. This connector exposes a subset of these APIs as operations in Microsoft Flow and PowerApps.

This connector will allow our partners and customers to build and automate on top of iObeya's platform, workflows using Power Automate or Power Apps.

## Pre-requisites
You will need the following to proceed:
* An iObeya platform
* An OAuth Application in iObeya administation : Settings >> API >> OAuth Application
  - you need to provide `https://global.consent.azure-apim.net/redirect` as RedirectUri
* A Microsoft PowerApps or Microsoft Flow plan with custom connector feature

## Supported Operations
The connector supports the following operations:
* `List of rooms`: Retrieve list of your rooms
* `List of boards`: Retrieve list of your boards
* `Create Card`: Create a card on a board
  * Supported card :
    - Standard
    - Activity
    - Feature
    - Story
    - ProblemSolving
    - QCD Action
* `Bulk QCD Indicators Update`: Bulk update your QCD indicators from any source (Excel, SAP, etc..)
* `List activity card`: Retrieve all activity card from a board planning by date range