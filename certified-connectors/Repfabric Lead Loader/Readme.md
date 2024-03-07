## Repfabric Lead Loader
This connector is used for sending new company, contact, opportunity and other forms of data to create new records in your Repfabric system. This connector lets you to send the data from an excel file, csv, online source such as a website contact form, and other Microsoft services. Common uses of this connector include uploading lists of leads, first time buy reports, and construction project lists so they become immmediately available to your sales team.

## Publisher

Repfabric

## Prerequisites
You will only need access to an excel sheet template (or online web form) that will provide the required data for the Repfabric API, and a Power Automate template (from the store) that uses the connector. Please find example excel templates in the Repfabric customer portal documentation and tutorials or contact support@repfabric.com. Similarly, you can use any excel sheet as long as the information is correctly mapped into the connnector.

You must be a Repfabric, Distifabric or Manufabric subscriber. Contact sales@repfabric.com to join.
You will also need your Login to your Repfabric system for using this connector.

For Excel sheet connections, you must have the Power Automate Flow add-in installed in your Excel program (available in the Get Add-ins Excel button) and a Power Automate license for any user of the spreadsheet that needs to run the flow.

## Supported Operations

### Contact
* Main Endpoint used for creating Companies in the Repfabric Database

## Obtaining Credentials
Credentials are managed by the Repfabric web application and assigned to you. Please contact your system administrator or support@repfabric.com if you do not know your credentials.

## Getting Started
There are many columns of company, contact, and opportunity data that can be sent, but only a few are necessary. This is the minimum required data that the API needs to take in:


    "company" : "Company Inc.",
    "type": "Customer",
    
    "first-name": "John",
    "last-name" : "Doe",

    "principal-name": "Manufacturer Inc.",
    "program": "Topic of my opportunity",

Please provide the data from an excel or csv file and link them to the appropriate key mapping in the connector. **Company Type and Program (Opportunity Topic)are required. Company Type should match a company type loaded into Repfabric, and Program cannot be empty if you are creating an opportunity.**

Based on the completeness of your required fields, this connector can be used to create a single company without any contacts or opportunities, create companies and contacts, or create companies, contacts and opportunities.  If for example you do not complete the required fields of a company, neither contacts nor opportunities will be created since they are "child" records to the company that was not created.  Similarly, if you do not complete all required fields of an opportunity, but do so for the companies and the contact, only company and contact will be created but the opportunity will fail creation. Therefore this connection can be used to create new companies in Repfabric that stand alone, new companies and contacts only, or new companies, contacts and opportunities.

In order to prevent duplicate company or contact creation, the API will provide similarity analysis to match a new record to an existing company or contact if found.  If the company or contact is already found in Repfabric, only the opportunity will be created (as long as all opportunity required fields are present). If the company is found, but the contact is not found in Repfabric, only the contact and opportunity will be created under the found company.

The connector will return the status of the API call, the data that displays messages of what the API created (company, contact, opportunity, etc), or why it failed.  
You can use the returned information to inform the user of the success or failure of the call, via email and/or updating the input excel or csv sheet. This update process should only run for about 3 - 15 seconds.

## Known Issues and Limitations
When running a template, it may run indefinitely if there's an issue with the configuration, or if the API keeps crashing due to malformed input information. We will add more features to defend against this in a future update.

Adding additional columns to source spreadsheets that are connected into a Power Automate flow may take several minutes before they become available in the Power Automate design tool.

Special characters cannot be used in input column headers of your Excel sheet due to limitations of processing them through Power Automate.

## Deployment Instructions
Please see your Repfabric tutorial or training sites for detailed deployment instructions or contact support@repfabric.com
