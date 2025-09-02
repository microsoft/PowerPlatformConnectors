namespace SnowflakeTestApp.Tests
{
    /// <summary>
    /// Common test data and constants used across integration tests.
    /// Configuration values come from ConnectionParametersProviderMock.cs
    /// </summary>
    public static class TestData
    {
        public const string TenantId = "<TenantId>";
        public const string ClientId = "<ClientId>";
        public const string ClientSecret = "<ClientSecret>";
        public const string Scope = "<Scope>";

        public const string BaseUrl = "https://localhost:44362";
        public const int DefaultTimeoutSeconds = 30;
        public const string DefaultDataset = "default";
        public const string DefaultTable = "CUSTOMERS";
        public const string MockStatementHandle = "01b1ebc6-0000-7065-0000-438300e4e0c6";
        public const int DefaultSqlTimeout = 30;
        public const string SampleSqlStatement = "SELECT 1 as test_column, CURRENT_TIMESTAMP() as current_time, CURRENT_USER() as current_user";

        public static string DefaultSnowflakeHostname = "your-account.region.cloud-provider.snowflakecomputing.com";// ConnectionParametersProviderMock.TestSnowflakeInstance;
        public static string DefaultDatabase => "DATAVERSE"; // ConnectionParametersProviderMock.TestDatabase;
        public static string DefaultSchema => "PUBLIC";// ConnectionParametersProviderMock.TestSchema;
        public static string DefaultWarehouse => "XSMALL"; //ConnectionParametersProviderMock.TestWarehouse;
        public static string DefaultRole => "ACCOUNTADMIN"; // ConnectionParametersProviderMock.TestRole;
        
    }
} 