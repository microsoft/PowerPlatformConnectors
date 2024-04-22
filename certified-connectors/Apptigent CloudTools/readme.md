## Apptigent CloudTools for Salesforce

Automate critical business processes with seamless Salesforce integration. Easily connect any data source with your Salesforce org to retrieve customer information, close deals, create records, convert leads, run bulk jobs, execute quick actions, upload files, associate documents with records, update feeds, retrieve linked files, and much more. You can even execute advanced queries and seaches using SOQL, SOSL, and GraphQL. CloudTools for Salesforce Sales Cloud unlocks the potential of your enterprise CRM data by making it easily accessible from any app or workflow.  

## Prerequisites

CloudTools requires a valid Salesforce license, installation of the CloudTools managed package in a Salesforce org (any edition), and a current CloudTools subscription (billed separately). For step-by-step instructions on how to get started visit https://www.apptigent.com/solutions/cloudtools/configuration.

## Supported Operations

Please visit the [CloudTools documentation page](https://www.apptigent.com/resources/documentation/cloudtools-for-salesforce-sales-cloud/) for a complete list of available actions. To learn more about each one and how to use it in your apps, forms and flows, visit our [Tutorials page](https://www.apptigent.com/resources/tutorials/). 

## Obtaining credentials

Generate a license (API) key by following these simple steps:

1. Install the CloudTools managed package via the Salesforce App Exchange.
2. Launch the CloudTools configuration page by selecting the "Apptigent CloudTools" link in the App Launcher.
3. Follow the on-screen instructions to create a connected app and generate a license key. 
4. Copy the key and provide it in the new connection dialog in Power Apps, Power Automate, or Azure Logic Apps.

## Getting Started

Add a CloudTools action to any flow or app. Configure the inputs and test the action to view the results. Use the examples in the documentation linked above to include  the action output data in additional flow steps, fields, or forms within your application.

## Known issues and limitations

Installing and configuring CloudTools for Salesforce Sales Cloud requires System Administrator permissions in Salesforce. Contact your Salesforce administrator for assistance if you do not have sufficient permissions within your org.

## Deployment Instructions

CloudTools actions are available in the actions panel within Power Automate and using the dotted notation expression editor in Power Apps. To deploy CloudTools as a customer connector within your environment, create a new Custom Connector using the Import Swagger File method, provide the file URL on the [CloudTools documentation page](https://www.apptigent.com/resources/documentation/cloudtools-for-salesforce-sales-cloud/), complete the wizard, and publish the connector. 

NOTE: If you choose the manual deployment method, be sure to check the [CloudTools documentation page](https://www.apptigent.com/resources/documentation/cloudtools-for-salesforce-sales-cloud/) regularly for updates.
