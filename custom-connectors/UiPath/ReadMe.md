## Author:
UiPath - [UiPath Website](https://uipath.com)
## Contact:
<integrations-apps@uipath.com>

## Version: 
0.5 - Private Preview

## Description
This is the private preview version of the UiPath connector for Power Automate.
With this version only automation cloud is supported.

## Supported Operations
The connector supports the following operations:
- Run Jobs
- Add Queue Items

## Getting Help or Providing Feedback
If you have any issues, requests for functionality, or have general feedback, please <integrations-apps@uipath.com>.

## Pre-requisites
Please make sure you have an automation cloud account. Feel free to [sign up](https://www.uipath.com/developers/studio-download) for a trial.
This connector is using OAUTH and you will need to configure an [External app](https://docs.uipath.com/automation-cloud/docs/managing-external-applications) in your automation cloud.

## Using the Connector
1. To use this connector, go to Power Automate and click **Data** on the left navigation page. Then, click **Custom connectors**.
2. Wait for the page to load. Then, click **+New custom connector**. 
3. From the dropdown, select **Import from GitHub**. For **Connector Type**, choose **Custom**. For **Branch**, choose **dev**. For **Connector**, choose **UiPath**.
4. Click **Continue.** You will now be taken the the Custom Connector UI, which will populate the connector files, including the code file, into the UI.
5. In **Step 1: General**, make sure to replace the "YourInstance" in the **Host URL** field with your own instance. You can learn more about this field [here](https://docs.uipath.com/connectors/docs/power-automate-about).
6. Go to **Step 2: Security** and in the OAuth 2.0 section, insert into **Client ID** the value from  **Automation Cloud -External Apps - AppID** field. Do the same for **Client Secret**.
7. Go to **Step 3: Definition** and review the operations. Then, go to **Step 4: Code** to view the code, which should already be enabled. Now, click "Create Connector."
9. Now, you can go to test your connector in **Step 5: Test**, if you'd like.
10. Begin using your custom connector in your environment to build apps and flows! 
