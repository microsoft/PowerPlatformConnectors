# SnowflakeTestApp.Tests

Integration tests for the Snowflake Test App that verify the endpoint's functionalities.

## Test Coverage

This test suite provides comprehensive coverage for all Snowflake V2 connector endpoints:

### Core Endpoints
- **TestConnectionEndpointIntegrationTest.cs** - Tests `/testconnection` endpoint for connection validation
- **DatasetEndpointIntegrationTest.cs** - Tests `/datasets` endpoint for listing datasets
- **DataSetsMetadataEndpointIntegrationTest.cs** - Tests `/$metadata.json/datasets` endpoint for datasets metadata

### Table Operations
- **TableEndpointIntegrationTest.cs** - Tests `/datasets/{dataset}/tables` endpoint for listing tables
- **TableMetadataEndpointIntegrationTest.cs** - Tests `/$metadata.json/datasets/{dataset}/tables/{table}` endpoint for table metadata

### Data Operations (CRUD)
- **TableDataEndpointIntegrationTest.cs** - Tests table data endpoints for:
  - GET `/datasets/{dataset}/tables/{table}/items` - List items
  - GET `/datasets/{dataset}/tables/{table}/items/{id}` - Get specific item
  - POST `/datasets/{dataset}/tables/{table}/items` - Create new item
  - PATCH/PUT `/datasets/{dataset}/tables/{table}/items/{id}` - Update item
  - DELETE `/datasets/{dataset}/tables/{table}/items/{id}` - Delete item

### SQL Operations
- **SqlEndpointIntegrationTest.cs** - Tests SQL API endpoints for:
  - POST `/sql` - Execute SQL statements
  - POST `/sql/{statementHandle}` - Get query results
  - POST `/sql/{statementHandle}/cancel` - Cancel running queries

Each test file includes both positive (with authentication) and negative (without authentication, with invalid parameters) test cases to ensure comprehensive endpoint validation.

## Quick Start

### Prerequisites

Before running tests, ensure you have:
- .NET SDK installed
- Valid Snowflake account credentials
- OAuth bearer token (see "Generating OAuth Tokens" section below)

### Method 1: Visual Studio (Recommended)

1. **Open Solution**
   - Launch Visual Studio
   - Open `dirs.sln` in the `Snowflake V2` directory

2. **Configure Test Settings**
   - Update `TestData.cs` and `ConnectionParametersProviderMock.cs` with your Snowflake connection details
   - Set `DefaultBearerToken` with your OAuth token

3. **Start Application**
   - Set `SnowflakeTestApp` as the startup project
   - Run the application (F5 or Ctrl+F5)
   - Verify it's accessible at `https://localhost:44362`

4. **Run Tests**
   - Open **Test Explorer** (Test → Test Explorer)
   - Click **Run All Tests** to run the entire test suite
   - View results and detailed output in Test Explorer

### Method 2: Command Line

1. **Configure Test Settings** (One-time setup)
   ```bash
   # Edit TestData.cs to set your configuration
   # Update DefaultBearerToken and connection parameters
   ```

2. **Start Application**
   ```bash
   # Build and run the app
   dotnet build
   dotnet run --project SnowflakeTestApp
   ```

3. **Run Tests** (in a new terminal)
   ```bash
   # Run all tests
   dotnet test
   
   # Run specific test project
   dotnet test SnowflakeTestApp.Tests/SnowflakeTestApp.Tests.csproj
   
   # Run tests with detailed output
   dotnet test --logger "console;verbosity=detailed"
   
   # Run specific test class
   dotnet test --filter "FullyQualifiedName~SqlEndpointIntegrationTest"
   ```

## Configuration Setup

### 1. Update Test Configuration

The test configuration is centralized in the `TestData.cs` file. Update the following constants with your Snowflake environment details:

```csharp
public static class TestData
{
    // Application URL (usually doesn't need to change)
    public const string BaseUrl = "https://localhost:44362";
    
    // Snowflake Connection Details - UPDATE THESE
    public static string DefaultSnowflakeHostname = "your-account.region.cloud-provider.snowflakecomputing.com";
    public static string DefaultDatabase => "DATAVERSE";
    public static string DefaultSchema => "PUBLIC";
    public static string DefaultWarehouse => "XSMALL";
    public static string DefaultRole => "SYSADMIN";
    
    // Authentication - UPDATE THIS
    public static string DefaultBearerToken => "your-oauth-bearer-token-here";
    
    // Test Data Configuration
    public const string DefaultDataset = "default";
    public const string DefaultTable = "CUSTOMERS";
}
```

### 2. Connection Parameters

The mock connection parameters are defined in `ConnectionParametersProviderMock.cs`. These should match your `TestData.cs` configuration to ensure consistency across all tests.

### 3. Security Notes

- **Never commit actual tokens to version control**
- Consider using environment variables or user secrets for production scenarios
- The bearer token is stored in TestData class for development convenience
- Ensure your test account has appropriate permissions for the specified database, schema, and warehouse

## Troubleshooting

### Common Issues

- **"Test fails with 401 Unauthorized"**: Check that your bearer token is valid and not expired
- **"Connection refused to localhost:44362"**: Ensure SnowflakeTestApp is running before executing tests
- **"Table not found" errors**: Verify your database and schema configuration in TestData.cs
- **"Warehouse not found" errors**: Confirm the warehouse name exists and your account has access
- **SSL/TLS errors**: Make sure you're using the correct Snowflake hostname format


## Generating OAuth Tokens

This guide walks you through setting up OAuth authentication for Snowflake using the Authorization Code Grant flow.

### Prerequisites

- **ACCOUNTADMIN** role access for creating security integrations
- A non-admin user account for testing (default role must not be ACCOUNTADMIN, SECURITYADMIN, or ORGADMIN)
- URL encoding tool (e.g., [urlencoder.io](https://urlencoder.io) or Postman)

### Step 1: Create OAuth Security Integration

Create a Security Integration using the **ACCOUNTADMIN** role:

```sql
CREATE SECURITY INTEGRATION POWER_APPS_TOKEN
TYPE = OAUTH
ENABLED = TRUE
OAUTH_CLIENT = CUSTOM
OAUTH_CLIENT_TYPE = 'CONFIDENTIAL'
OAUTH_REDIRECT_URI = 'https://localhost.com'
OAUTH_ISSUE_REFRESH_TOKENS = TRUE
OAUTH_REFRESH_TOKEN_VALIDITY = 86400;
```

> **Note:** The `OAUTH_REDIRECT_URI` is where Snowflake will redirect the authorization code. We're using `https://localhost.com` for demonstration purposes.

### Step 2: Gather OAuth Configuration Details

1. **Get integration details:**
   ```sql
   DESC SECURITY INTEGRATION POWER_APPS_TOKEN;
   ```
   
   Record the following values:
   - `OAUTH_CLIENT_ID`
   - `OAUTH_REDIRECT_URI`
   - `OAUTH_AUTHORIZATION_ENDPOINT`
   - `OAUTH_TOKEN_ENDPOINT`

2. **Get client secret:**
   ```sql
   SELECT SYSTEM$SHOW_OAUTH_CLIENT_SECRETS('POWER_APPS_TOKEN');
   ```
   
   This returns 2 secrets - record either one as `OAUTH_CLIENT_SECRET`.

### Step 3: Request Authorization Code

1. **Prepare the authorization URL:**
   
   First, URL-encode these parameters:
   - `OAUTH_CLIENT_ID`
   - `OAUTH_REDIRECT_URI`
   
   Then construct the authorization URL:
   ```
   <OAUTH_AUTHORIZATION_ENDPOINT>?response_type=code&client_id=<encoded_OAUTH_CLIENT_ID>&redirect_uri=<encoded_OAUTH_REDIRECT_URI>
   ```

2. **Complete the authorization flow:**
   - Navigate to the authorization URL in your browser
   - Log in with a non-admin Snowflake user
   - Review and accept the consent prompt
   - After consent, you'll be redirected to the `OAUTH_REDIRECT_URI`

3. **Extract the authorization code:**
   
   The redirect URL will contain the authorization code:
   ```
   https://localhost.com/?code=029118413715B55DxxxxxxxxD3AD952F484380839
   ```
   
   Save the `code` parameter value for the next step.

### Step 4: Exchange Authorization Code for Access Token

Use the authorization code to get an access token:

```bash
curl -X POST \
  -H "Content-Type: application/x-www-form-urlencoded;charset=UTF-8" \
  --user "<OAUTH_CLIENT_ID>:<OAUTH_CLIENT_SECRET>" \
  --data-urlencode "grant_type=authorization_code" \
  --data-urlencode "code=<AUTHORIZATION_CODE>" \
  --data-urlencode "redirect_uri=<OAUTH_REDIRECT_URI>" \
  <OAUTH_TOKEN_ENDPOINT>
```

> **Important:** Use the original (non-encoded) values for `OAUTH_CLIENT_ID`, `OAUTH_CLIENT_SECRET`, and `OAUTH_REDIRECT_URI` in this request.

**Response:** You'll receive an access token and refresh token in the response.

### Step 5: Refresh Access Token (Optional)

Access tokens expire after 600 seconds. Use the refresh token to get a new access token without user interaction:

```bash
curl -X POST \
  -H "Content-Type: application/x-www-form-urlencoded;charset=UTF-8" \
  --user "<OAUTH_CLIENT_ID>:<OAUTH_CLIENT_SECRET>" \
  --data-urlencode "grant_type=refresh_token" \
  --data-urlencode "refresh_token=<REFRESH_TOKEN>" \
  --data-urlencode "redirect_uri=<OAUTH_REDIRECT_URI>" \
  <OAUTH_TOKEN_ENDPOINT>
```

### Security Notes

- Keep client secrets secure and never expose them in client-side code
- Access tokens are short-lived (10 minutes) by design
- Use refresh tokens to maintain long-term access without user re-authentication
- Always use HTTPS for production redirect URIs

## Test Data Infrastructure

The test project includes comprehensive data seeding functionality for tests that require sample data:

### Base Classes

- **`BaseIntegrationTest`**: Basic test functionality without automatic data seeding

### TestDataSeeder Class

The `TestDataSeeder` class in the `Infrastructure` folder handles:
- Creating test tables with proper schema
- Seeding tables with sample data
- Cleaning up test data between tests
- Providing access to seeded records for validation

### Default Test Table Schema

Tests using data seeding will automatically create a `CUSTOMERS` table with this structure:

```sql
CREATE OR ALTER TABLE CUSTOMERS (
    ID NUMBER PRIMARY KEY,
    NAME VARCHAR(255) NOT NULL,
    EMAIL VARCHAR(255),
    PHONE VARCHAR(50),
    CREATED_DATE TIMESTAMP DEFAULT CURRENT_TIMESTAMP(),
    IS_ACTIVE BOOLEAN DEFAULT TRUE,
    BALANCE NUMBER(10,2) DEFAULT 0.00
)
```

### Sample Test Data

The seeder creates 10 sample customer records including:
- John Doe (john.doe@example.com, $1,500.50)
- Jane Smith (jane.smith@example.com, $2,750.00)
- Bob Johnson (bob.johnson@example.com, $890.25)
- And 7 additional test customers...

### Using Data Seeding in Tests

```csharp
[TestClass]
public class MyDataTest : BaseIntegrationTestWithDataSeeding
{
    [TestMethod]
    public async Task TestWithSeededData()
    {
        // RequireTestData() ensures test data is available
        RequireTestData();
        
        // Access seeded records for validation
        var expectedRecords = TestDataSeeder.SeededRecords;
        Assert.AreEqual(10, expectedRecords.Count);
        
        // Test your endpoint with seeded data
        var response = await HttpClient.GetAsync($"{BaseUrl}/datasets('default')/tables('CUSTOMERS')/items");
        response.EnsureSuccessStatusCode();
        
        // Validate response against seeded data
        var content = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(content.Contains("John Doe"));
    }
}
```

### Custom Test Data

Override the seeding behavior for specific test needs:

```csharp
[TestMethod]
public async Task TestWithCustomData()
{
    var customRecords = new List<TestDataRecord>
    {
        new TestDataRecord { Id = 1, Name = "Custom User", Email = "custom@test.com", Balance = 100.00m }
    };
    
    await TestDataSeeder.SeedCustomTestData("CUSTOMERS", customRecords);
    
    // Your test code here
}
```

### Data Cleanup

Tests inheriting from `BaseIntegrationTestWithDataSeeding` automatically:
1. Clean up test tables before each test
2. Seed fresh data for each test
3. Dispose of resources after test completion

## Development Guidelines

## Project Structure

The test project is organized by functionality with clear separation of concerns:

```
SnowflakeTestApp.Tests/
├── Connection/                                    # Connection & Authentication Tests
│   └── TestConnectionEndpointIntegrationTest.cs  # Tests connection validation endpoint
├── Data/                                         # Data Operations Tests (CRUD)
│   ├── DatasetEndpointIntegrationTest.cs         # Tests dataset listing endpoints
│   ├── TableDataEndpointIntegrationTest.cs       # Tests table CRUD operations
│   └── TableEndpointIntegrationTest.cs           # Tests table listing endpoints
├── Infrastructure/                               # Test Infrastructure & Utilities
│   ├── TestDataSeeder.cs                        # Handles test data creation/seeding
│   ├── TestDataModel.cs                         # Test data models and records
│   ├── TestDataValidationIntegrationTest.cs     # Validates test data infrastructure
│   └── ODataResponseModels.cs                   # Response models for OData endpoints
├── Metadata/                                     # Schema & Metadata Tests
│   ├── DataSetsMetadataEndpointIntegrationTest.cs # Tests dataset metadata endpoints
│   └── TableMetadataEndpointIntegrationTest.cs   # Tests table metadata endpoints
├── Sql/                                          # SQL Execution Tests
│   └── SqlEndpointIntegrationTest.cs             # Tests direct SQL execution
├── BaseIntegrationTest.cs                        # Base classes for all integration tests
├── TestData.cs                                   # Configuration constants and test data
├── GlobalSuppressions.cs                         # Code analysis suppressions
├── SnowflakeTestApp.Tests.csproj                # Project configuration
└── README.md                                     # This documentation
```

### Key Files and Their Purpose

| File/Directory | Purpose |
|----------------|---------|
| **`TestData.cs`** | Central configuration for connection details, endpoints, and test constants |
| **`BaseIntegrationTest.cs`** | Base class providing common functionality for all integration tests |
| **`Infrastructure/TestDataSeeder.cs`** | Handles automatic creation and seeding of test tables with sample data |
| **`Infrastructure/TestDataModel.cs`** | Defines data models used across tests (TestDataRecord, etc.) |
| **Connection/** | Tests for connection validation and authentication |
| **Data/** | Tests for all CRUD operations on datasets and tables |
| **Metadata/** | Tests for schema and metadata retrieval operations |
| **Sql/** | Tests for direct SQL query execution and management |



