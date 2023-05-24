## Dynamics 365 Translation Service

This connector provides actions for Translation, Regeneration, and Alignment powered by Dynamics 365 Translation Service (DTS). It is designed for Dynamics Lifecycle services users who already use DTS as their translation solution. 

## Publisher: Microsoft Corporation

## Prerequisites
You will need the following to proceed:
* A Microsoft Power Apps or Power Automate plan with custom connector feature.
* An account with access to Dynamics Lifecycle Services.

## Supported Operations
The connector supports the following operations:
### Translate
Submit a translation request. This operation takes native resource files and optionally translation memories and creates a new translation request under your DTS workspace. 
### Align 
If you have files that were previously translated, and you also have corresponding source files, you can use the Align operation to create a TM in XML Localization Interchange File Format (XLIFF).
### Regenerate
Use this operation to regenerate the translated native format files after revising the translation memory files.
### Retrieve 
Use this operation to poll the status of a translation request. Once the request is completed, you can download the output.
### Download Translation Result
Use this operation to download the output of a completed translation request.
### Download Alignment Result
Use this operation to download the output of a completed alignment request.
### Get Languages
This is an internal operation. Use it to get the list of translation languages DTS supports for a given product.
### Products
This is an internal operation. Use it to get the list of products supported by DTS.
### Product Versions
This is an internal operation. Use it to get the list of product versions for a given product supported by DTS.

## Obtaining Credentials
This connector uses OAuth via an AAD app for authentication. Since DTS resides in LCS, you need LCS API permissions in your application. The following procedure guides you through the app registration process for an AAD app for use with this connector.

1. Sign in to the [Azure portal](https://portal.azure.com/) as the user who will be used to communicate with the LCS API.
2. Under **Azure services**, select **App registrations**. 
3. On the **App registrations** page, select **New registration**.
4. On the **Register an application** page, in the **Name** field, enter a name for the app.
5. Under **Supported account types**, select an option to specify which accounts should be supported.
6. Select **Register**.
7. On the page for your new app registration, in the left navigation pane, under **Manage**, select **API permissions**.
8. On the **API permissions** page, select **Add a permission**.
9. On the **APIs my organization uses** tab, find and select the **Dynamics Lifecycle services** API.
10. Select the checkbox for the API permission that has the **user\_impersonation** scope, and then select **Add permissions**.
11. Select the button to grant admin consent for the permissions. When you're prompted to confirm the action, select **Yes**.
12. In the left navigation pane, under **Manage**, select **Authentication**.
13. On the **Authentication** page, under **Advanced settings**, set the **Allow public client flows** option to **Yes**.
14. In the left navigation pane, select **Overview**.
15. The overview page for your app registration shows the client ID. 
16. In the left navigation pane, select **Certificates & secrets**. From here, you can generate the client secret needed to set up the connection.

## Known Issues and Limitations
* Only user interface requests are supported.
* The Align operation returns a URL for the alignment file, but the download alignment operation takes the filename as input. You will need to manipulate the URL to extract the filename before using the Align operation.

## Deployment Instructions
You can deploy this as a custom connector without making any changes to the API definition. In the API properties, you need to supply your app's clientId and clientSecret before attempting to deploy. See the **Obtaining Credentials** section above for guidance on registering  the app. You can then use the `paconn` tool to deploy as a custom connector by [following these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli).







