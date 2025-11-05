# Azure Blob Storage Index Tags

Azure Blob Storage Index Tags connector allows you to view, manage, and query blob index tags. Blob index tags provide data management and discovery capabilities using key-value tag attributes on your blob storage resources.

## Publisher

Power Platform Community

## Prerequisites

To use this connector, you will need:

* An Azure subscription
* An Azure Storage Account with blob storage
* Appropriate permissions to read and write blob index tags (Storage Blob Data Owner or Storage Blob Data Contributor role)
* Azure AD app registration with delegated permissions to Azure Storage (for OAuth authentication)

## Supported Operations

This connector supports the following operations based on the [Azure Blob Storage REST API](https://docs.microsoft.com/en-us/rest/api/storageservices/blob-service-rest-api):

### Get Blob Tags
Retrieves all user-defined tags for a specified blob. This operation returns all the tags associated with a blob.

**Parameters:**
- Container Name: The name of the container
- Blob Name: The name of the blob (including path if in a virtual directory)

### Set Blob Tags
Sets user-defined tags for a specified blob. This operation replaces all existing tags on the blob.

**Parameters:**
- Container Name: The name of the container
- Blob Name: The name of the blob (including path if in a virtual directory)
- Tags: An array of tag objects, each containing a Key and Value

**Example Tags JSON:**
```json
{
  "Tags": {
    "TagSet": {
      "Tag": [
        {
          "Key": "Environment",
          "Value": "Production"
        },
        {
          "Key": "Department",
          "Value": "Finance"
        }
      ]
    }
  }
}
```

### Find Blobs by Tags (Container Level)
Finds all blobs in a specific container that match a tag filter expression.

**Parameters:**
- Container Name: The name of the container to search
- Tag Filter Expression: A SQL-like expression to filter blobs (e.g., `"Environment='Production' AND Status='Active'"`)
- Max Results: (Optional) Maximum number of results to return (default: 5000)
- Marker: (Optional) Continuation token for pagination

### Find Blobs by Tags (Account Level)
Finds all blobs across all containers in the storage account that match a tag filter expression.

**Parameters:**
- Tag Filter Expression: A SQL-like expression to filter blobs (e.g., `"Environment='Production' AND Status='Active'"`)
- Max Results: (Optional) Maximum number of results to return (default: 5000)
- Marker: (Optional) Continuation token for pagination

## Obtaining Credentials

### Azure AD Authentication Setup

1. **Create an Azure AD App Registration:**
   - Navigate to Azure Active Directory > App registrations
   - Click "New registration"
   - Provide a name (e.g., "Power Platform Blob Tags Connector")
   - Set redirect URI as: `https://global.consent.azure-apim.net/redirect`
   - Click "Register"

2. **Configure API Permissions:**
   - In your app registration, go to "API permissions"
   - Click "Add a permission"
   - Select "Azure Storage"
   - Choose "Delegated permissions"
   - Select `user_impersonation`
   - Click "Add permissions"
   - Grant admin consent if required

3. **Note the Application (client) ID:**
   - Copy the "Application (client) ID" from the Overview page
   - You'll need to update this in the `apiProperties.json` file

4. **Assign Storage Roles:**
   - Navigate to your Storage Account
   - Go to "Access Control (IAM)"
   - Add role assignment
   - Select "Storage Blob Data Contributor" or "Storage Blob Data Owner"
   - Assign to your user account or service principal

### Connector Setup

When creating a connection:
1. Enter your Storage Account Name (without the `.blob.core.windows.net` suffix)
2. Complete the OAuth authentication flow
3. Grant consent to access your storage account

## Tag Query Syntax

When using the "Find Blobs by Tags" operations, you can use SQL-like expressions:

**Basic Operators:**
- Equality: `"TagName='Value'"`
- AND: `"Tag1='Value1' AND Tag2='Value2'"`
- OR: `"Tag1='Value1' OR Tag2='Value2'"`
- Greater than/Less than: `"NumericTag>100"`

**Examples:**
- Find production blobs: `"Environment='Production'"`
- Find active production blobs: `"Environment='Production' AND Status='Active'"`
- Find blobs from specific departments: `"Department='Finance' OR Department='HR'"`

## Known Issues and Limitations

* **Tag Limits:**
  - Maximum 10 tags per blob
  - Tag keys: 1-128 characters
  - Tag values: 0-256 characters
  - Tag keys and values are case-sensitive

* **Query Limitations:**
  - Maximum 5000 results per query (use marker for pagination)
  - Tag queries may take time to reflect recently set tags (eventual consistency)

* **Authentication:**
  - Requires OAuth 2.0 with Azure AD
  - Users must have appropriate RBAC permissions on the storage account
  - Shared Key authentication is not supported in this connector (use OAuth only)

* **API Version:**
  - This connector uses Azure Blob Storage REST API version 2020-10-02
  - Blob index tags require storage accounts with hierarchical namespace disabled or API version 2020-04-08 or later

## API Documentation

For more information on Azure Blob Storage Index Tags:
- [Blob Index Tags Overview](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-manage-find-blobs)
- [Get Blob Tags REST API](https://docs.microsoft.com/en-us/rest/api/storageservices/get-blob-tags)
- [Set Blob Tags REST API](https://docs.microsoft.com/en-us/rest/api/storageservices/set-blob-tags)
- [Find Blobs by Tags REST API](https://docs.microsoft.com/en-us/rest/api/storageservices/find-blobs-by-tags)

## Deployment Instructions

1. Clone or download the connector files
2. Update the `clientId` in `apiProperties.json` with your Azure AD app registration client ID
3. Import the connector to your Power Platform environment:
   - Go to Power Automate or Power Apps
   - Navigate to Data > Custom connectors
   - Click "New custom connector" > "Import an OpenAPI file"
   - Upload the `apiDefinition.swagger.json` file
   - Configure the connector settings
   - Create a connection using your storage account name

## Support and Contributions

This is a community-maintained connector. For issues, questions, or contributions, please visit the [Power Platform Connectors GitHub repository](https://github.com/microsoft/PowerPlatformConnectors).
