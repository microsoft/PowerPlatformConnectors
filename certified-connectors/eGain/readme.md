# eGain - Connector Usage Guide

## Overview

The eGain connector is designed to integrate seamlessly with Microsoft Copilot Studio agents, providing powerful knowledge management capabilities through two distinct eGain instances. This connector enables agents to search knowledge bases, generate AI-powered responses, and provide accurate citations with automatic URL generation.

## Prerequisites

Before using the eGain connector, ensure you have the following prerequisites in place:

1. **eGain Instance Requirements**: You need to have an eGain instance running on Rigel or R21.x.x version or higher.

2. **Client Application Configuration**: A client application must be created in your eGain instance with appropriate permissions for the actions you plan to use. The client application should have:
   - **Portal permissions** for knowledge base search operations
   - **AI permissions** for generative AI operations
   
   For detailed information on creating and configuring client applications, refer to the [eGain Client Application documentation](https://cdn.egain.cloud/21.21.4/help/en/us/administration/about_client_application.htm).

3. **Knowledge Portal Setup**: A knowledge portal must be pre-configured in your eGain instance to enable content search and retrieval. The portal serves as the foundation for knowledge base operations and content management.
   
   For information on setting up knowledge portals, refer to the [eGain Portals documentation](https://cdn.egain.cloud/21.21.4/help/en/us/knowledge/portals_about.htm).

## Agent Configurations

### 1. eGain Demo Agent
- **API Host**: `api.egain.cloud`
- **Instance URL**: [https://egainr21demo.egain.cloud](https://egainr21demo.egain.cloud)
- **Authentication**: Security enabled with Bearer token authentication
- **Custom Topics**:
  - **eGain Search**: Uses `Kb-search` operation
  - **Generative**: Uses `Generative-V3` operation

### 2. eGain Connector Assistant
- **API Host**: `api.ai.egain.cloud`
- **Instance URL**: [https://ai.egain.cloud/s5fa](https://ai.egain.cloud/s5fa)
- **Authentication**: Security enabled with Bearer token authentication
- **Custom Topics**:
  - **Generative**: Uses `Generative-V3` operation
  - **eGainSearch**: Uses `Search-V3` operation

## Security and Authentication

**Important**: The eGain connector itself does not require any authentication configuration. However, the eGain APIs are secured and support generic OAuth 2.0 authentication, which is configured in the Security tab of your Copilot Studio agents.

Both eGain instances have security enabled, ensuring that all API calls are properly authenticated. The connector automatically handles:

- **OAuth 2.0 Support**: Generic OAuth 2.0 authentication configured in Copilot Studio Security tab
- **Bearer Token Authentication**: Automatically adds `Bearer ` prefix if not present
- **Header Management**: Properly manages Authorization headers
- **Parameter Sanitization**: Removes authentication parameters before forwarding to eGain APIs
- **Error Handling**: Graceful handling of authentication failures

## Operations and Usage

### Operation 1: Search-V3
**Used by**: eGain Connector Assistant (eGainSearch topic)

**Purpose**: Advanced search using eGain's V3 search API with enhanced result processing and automatic citation URL generation.

**Configuration**:
- **API Host**: `api.ai.egain.cloud`
- **Endpoint**: `/search/v3`
- **Method**: POST

**Sample Request**:
```json
{
  "q": "How to reset password for user accounts?",
  "baseUrl": "https://ai.egain.cloud/s5fa/kb/ebank,
  "shortName": "content",
  "authToken": "your-bearer-token-here",
  "$attribute": "snippet",
  "$lang": "en-us",
  "$pagenum": 1,
  "$pagesize": 10,
  "Accept": "application/json",
  "Accept-language": "en-us",
  "portalId": "202400000001000"
}
```

**Copilot Studio Configuration**:
```
$attribute: snippet
$lang: en-us
$pagenum: 1
$pagesize: 10
Accept: application/json
Accept-language: en-us
authToken: =System.User.AccessToken
baseUrl: https://ai.egain.cloud/s5fa/kb/ebank
portalId: "202400000001000"
query: =Topic.Var1_SearchQuestion
shortName: content
```

**Sample Response**:
```json
{
  "results": [
    {
      "alternateId": "KB-12345",
      "title": "Password Reset Procedures",
      "url": "https://ai.egain.cloud/s5fa/kb/ebank/content/KB-12345/Password-Reset-Procedures",
      "snippet": "To reset a user password, follow these steps...",
      "relevanceScore": 0.95,
      "lastModifiedDate": "2024-01-15T10:30:00Z"
    },
    {
      "alternateId": "KB-12346",
      "title": "Account Security Guidelines",
      "url": "https://ai.egain.cloud/s5fa/kb/ebank/content/KB-12346/Account-Security-Guidelines",
      "snippet": "Best practices for maintaining account security...",
      "relevanceScore": 0.87,
      "lastModifiedDate": "2024-01-10T14:20:00Z"
    }
  ],
  "totalCount": 2,
  "query": "How to reset password for user accounts?"
}
```

### Operation 2: Kb-search
**Used by**: eGain Demo Agent (eGain Search topic)

**Purpose**: Knowledge base article search with enhanced metadata processing and automatic URL generation.

**Configuration**:
- **API Host**: `api.egain.cloud`
- **Endpoint**: `/search/kb`
- **Method**: POST

**Sample Request**:
```json
{
  "q": "troubleshooting network connectivity issues",
  "baseUrl": "https://egainr21demo.egain.cloud/kb",
  "shortName": "ebank",
  "authToken": "your-bearer-token-here",
  "$attribute": "snippet",
  "$lang": "en-us",
  "$pagenum": 1,
  "$pagesize": 10,
  "Accept": "application/json",
  "Accept-language": "en-us",
  "portalId": "202200000001170"
}
```

**Copilot Studio Configuration**:
```
$attribute: snippet
$lang: en-us
$pagenum: 1
$pagesize: 10
Accept: application/json
Accept-language: en-us
authToken: =System.User.AccessToken
baseUrl: https://egainr21demo.egain.cloud/kb
portalId: "202200000001170"
q: =Topic.Var1_SearchQuestion
shortName: ebank
```

**Sample Response**:
```json
{
  "article": [
    {
      "id": "202200000021450",
      "name": "Network Troubleshooting Guide",
      "url": "https://egainr21demo.egain.cloud/kb/content/202200000021450/Network-Troubleshooting-Guide",
      "snippet": "Common network connectivity issues and their solutions...",
      "createdDate": "2022-11-11T14:21:48Z",
      "lastModifiedDate": "2023-10-02T16:46:58Z",
      "hasAttachments": false
    },
    {
      "id": "202200000019399",
      "name": "WiFi Connection Problems",
      "url": "https://egainr21demo.egain.cloud/kb/content/202200000019399/WiFi-Connection-Problems",
      "snippet": "Step-by-step guide to resolve WiFi connectivity issues...",
      "createdDate": "2022-09-28T22:24:43Z",
      "lastModifiedDate": "2025-08-07T15:22:02Z",
      "hasAttachments": true
    }
  ],
  "isSpellingCorrected": false,
  "paginationInfo": {
    "count": 15,
    "pagenum": 1,
    "pagesize": 10
  },
  "query": "troubleshooting network connectivity issues"
}
```

### Operation 3: Generative-V3
**Used by**: Both agents (Generative topic)

**Purpose**: AI-powered response generation with intelligent citation management, duplicate detection, and configurable filtering.

**Configuration**:
- **eGain Demo Agent**: `api.egain.cloud`
- **eGain Connector Assistant**: `api.ai.egain.cloud`
- **Endpoint**: `/generative/v3`
- **Method**: POST

**Sample Request**:
```json
{
  "q": "What are the steps to configure a new user account with proper security settings?",
  "baseUrl": "https://egainr21demo.egain.cloud/kb",
  "shortName": "ebank",
  "minScore": 0.3,
  "maxReferences": 5,
  "authToken": "your-bearer-token-here",
  "Accept": "application/json",
  "languageCode": "en-us",
  "portalId": "202200000001170"
}
```

**Copilot Studio Configuration**:
```
Accept: application/json
authToken: =System.User.AccessToken
baseUrl: https://egainr21demo.egain.cloud/kb
languageCode: en-us
portalId: 202200000001170
q: =Topic.Var1_UserQuestion
shortName: ebank
minScore: 0.3
maxReferences: 5
```

**Sample Response**:
```json
{
  "Content": "To configure a new user account with proper security settings, follow these comprehensive steps:\n\n**1. Account Creation**\n- Create the user account in the system [1]\n- Set up initial login credentials [2]\n- Configure basic profile information [3]\n\n**2. Security Configuration**\n- Enable two-factor authentication [4]\n- Set password complexity requirements [5]\n- Configure access permissions based on role [6]\n\n**3. Verification and Testing**\n- Test login functionality [7]\n- Verify security settings are active [8]\n- Conduct security audit [9]\n\n**References:**\n1. [User Account Setup Guide](https://egainr21demo.egain.cloud/kb/ebank/content/USR-001/User-Account-Setup-Guide)\n2. [Login Credentials Management](https://egainr21demo.egain.cloud/kb/ebank/content/USR-002/Login-Credentials-Management)\n3. [Profile Configuration](https://egainr21demo.egain.cloud/kb/ebank/content/USR-003/Profile-Configuration)\n4. [Two-Factor Authentication Setup](https://egainr21demo.egain.cloud/kb/ebank/content/SEC-001/Two-Factor-Authentication-Setup)\n5. [Password Policy Configuration](https://egainr21demo.egain.cloud/kb/ebank/content/SEC-002/Password-Policy-Configuration)",
  "Sources": [
    {
      "Id": "1",
      "Name": "User Account Setup Guide",
      "Url": "https://egainr21demo.egain.cloud/kb/ebank/content/USR-001/User-Account-Setup-Guide",
      "Score": 0.95
    },
    {
      "Id": "2",
      "Name": "Login Credentials Management",
      "Url": "https://egainr21demo.egain.cloud/kb/ebank/content/USR-002/Login-Credentials-Management",
      "Score": 0.92
    },
    {
      "Id": "3",
      "Name": "Profile Configuration",
      "Url": "https://egainr21demo.egain.cloud/kb/ebank/content/USR-003/Profile-Configuration",
      "Score": 0.88
    }
  ],
  "Score": 0.92,
  "ReferenceCount": 9
}
```

## Agent Setup and Configuration

### eGain Demo Agent Setup

1. **Create Custom Connector**:
   - Import the OpenAPI definition
   - Set API host to `api.egain.cloud`
   - Configure authentication with Bearer token

2. **Configure Custom Topics**:
   - **eGain Search Topic**: Map to `Kb-search` operation
   - **Generative Topic**: Map to `Generative-V3` operation

3. **Authentication Setup**:
   - Obtain Bearer token from eGain Demo instance
   - Configure in connector authentication settings
   - Test connection before deploying

### eGain Connector Assistant Setup

1. **Create Custom Connector**:
   - Import the OpenAPI definition
   - Set API host to `api.ai.egain.cloud`
   - Configure authentication with Bearer token

2. **Configure Custom Topics**:
   - **eGainSearch Topic**: Map to `Search-V3` operation
   - **Generative Topic**: Map to `Generative-V3` operation

3. **Authentication Setup**:
   - Obtain Bearer token from eGain Connector Assistant instance
   - Configure in connector authentication settings
   - Test connection before deploying

## Best Practices for Copilot Agents

### 1. Topic Design
- **Clear Intent Recognition**: Design topics to clearly identify when to use each operation
- **Context Awareness**: Use conversation context to determine appropriate search parameters
- **Fallback Handling**: Implement fallback responses when no results are found

### 2. Response Processing
- **Citation Integration**: Leverage the automatic URL generation for rich responses
- **Content Formatting**: Use the structured response format for consistent presentation
- **Error Handling**: Implement graceful error handling for API failures

### 3. Security Considerations
- **Token Management**: Implement secure token storage and rotation
- **Access Control**: Ensure proper access controls for different user roles
- **Audit Logging**: Log API calls for security monitoring

### 4. Performance Optimization
- **Caching**: Implement appropriate caching for frequently accessed content
- **Rate Limiting**: Respect eGain API rate limits
- **Response Filtering**: Use minScore and maxReferences parameters effectively

## Troubleshooting

### Common Issues

**Authentication Errors**:
- Verify Bearer token is valid and not expired
- Check API host configuration matches the intended eGain instance
- Ensure proper token format in connector settings

**Empty Results**:
- Verify search queries are specific enough
- Check if content exists in the knowledge base
- Review minScore settings for generative responses

**URL Generation Issues**:
- Ensure baseUrl and shortName parameters are correctly configured
- Verify eGain instance URLs are accessible
- Check for special characters in article names

### Debugging Steps

1. **Test Individual Operations**: Use Power Automate to test each operation separately
2. **Check Response Format**: Verify response structure matches expected schema
3. **Validate Authentication**: Ensure tokens are properly configured
4. **Review Logs**: Check connector execution logs for detailed error information

## Advanced Features

### Duplicate Detection
The connector automatically removes duplicate citations based on reference ID and name combinations, ensuring clean reference lists in generative responses.

### Score-Based Filtering
Configure minimum score thresholds and maximum reference counts to control response quality and length:
- **minScore**: Filter out low-relevance citations (default: 0.01)
- **maxReferences**: Limit number of references (default: 10)

### URL Customization
Support for custom URL construction using baseUrl and shortName parameters:
- **baseUrl**: Your eGain instance base URL
- **shortName**: URL path segment for content access

## Support and Maintenance

### Regular Maintenance
- **Token Rotation**: Regularly update authentication tokens
- **API Monitoring**: Monitor API usage and performance
- **Content Updates**: Keep knowledge base content current

### Support Resources
- **eGain Documentation**: Refer to eGain's official API documentation
- **Microsoft Copilot Studio**: Use Microsoft's connector troubleshooting guides
- **Community Support**: Leverage Microsoft Power Platform community resources

## Version History

- **v1.0.0**: Initial release with Search-V3, Kb-search, and Generative-V3 operations
- Added automatic citation linking and duplicate detection
- Implemented configurable score filtering and reference limits
- Enhanced security with proper authentication handling

---

*This documentation is designed to help developers and administrators effectively implement and maintain the eGain connector in Microsoft Copilot Studio agents.*