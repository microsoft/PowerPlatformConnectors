# Knowledgeone RecFind6 

The RecFind 6 Connector provides a way of connecting to a RecFind 6 system from Microsoft’s Power platform.
It is a replacement of the RecFind 6 SharePoint Integration Tool that was available in earlier versions.
It fully supports Microsoft Office 365.

Using the RecFind 6 Connector you may:
* serve a RecFind 6 Search Screen (a "Power App") to users, for example on a SharePoint page, allowing users to search, view and modify records in the RecFind 6 system (using the Mini-API product functionality)
* use RecFind 6 within a "Power Automate Flow", allowing documents to be sent from SharePoint to your RecFind 6 system
* create dashboards showing statistical information based on data stored in your RecFind 6 system

### Publisher: 
Knowledgeone Corporation
 
### Prerequisites

Firstly, you must have a RecFind 6 Connector licence. This product is not a standard feature of RecFind 6 and is purchased separately. Please contact Knowledgeone Corporation Sales if you would like to purchase the RecFind 6 Connector.

You will also need to have the appropriate Microsoft licences for accessing the required Power Platform components.

The RecFind 6 Connector requires that you are running RecFind 6 v2.11 or higher and have applied the RecFind 6 DRM v2.11.1 or higher update.

To install the RecFind 6 Connector software you require a Windows server with the Internet Information Services (IIS) role enabled. 

You must also have Microsoft .NET Framework v4.7.2 installed. Please refer to your operating system documentation for details. If not present, the RecFind 6 Connector installation program will install it.

After installation, you will need to configure your system so that the RecFind 6 Connector site is externally accessible via a HTTPS URL. This permits documents to be sent to your RecFind 6 system and data can be retrieved from the Power Platform.

For all functionality of the RecFind 6 Search Power App to be available, you will also require a RecFind 6 Web Client and Mini-API licence and software installed.


### Supported Operations

`QueryList` Returns a list of all predefined queries that the caller may request.

`QueryData` Provides a list of field names and values corresponding to the requested query and search parameters.  Results can be paged.

`QueryTable` Provides a table of results corresponding to the requested query and search parameters.

`SavedSearch` Provides a table of results corresponding to the requested saved search and search parameters.

`SendFile` Submit file contents and metadata for storage in the RecFind 6 database. Returns a URL to the stored file.

### Obtaining Credentials
Pass the security key in a header named `SecurityKey`.   
Connections to the RecFind 6 Connector require a Security Key to successfully connect. It is critical that this key is changed after installation and kept secure. The Security Key should only be shared with people who require it.

### To set/change your security key:
* Edit the web.config file located in the folder where the RecFind 6 Connector was installed, e.g. "C:\Program Files (x86)\Knowledgeone Corporation\RecFind 6\RF6Connector\web.config"

* Locate the Security Key setting and alter the value:  
`<add key="SecurityKey" value="MySecretKey" />` 
