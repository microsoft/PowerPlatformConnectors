// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeTestApp.Mocks
{
    using System;
    using System.Web;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using SnowflakeV2CoreLogic;

    /// <summary>
    /// Mock implementation of the connection parameters provider.
    /// Provides test values for connection parameters.
    /// Update these values below for your test environment before deployment or debugging.
    /// </summary>
    public class ConnectionParametersProviderMock : IConnectionParametersProvider
    {
        // ====== CONFIGURATION VALUES ======
        // Update these values for your test environment:

        /// <summary>
        /// Snowflake instance hostname (without https://)
        /// REQUIRED: Update with your Snowflake account URL
        /// Format: account.region.cloud_provider.snowflakecomputing.com
        /// Example: "mycompany.us-west-2.aws.snowflakecomputing.com"
        /// </summary>
        public static string TestSnowflakeInstance = "your-account.region.cloud-provider.snowflakecomputing.com";

        /// <summary>
        /// Database name for testing
        /// REQUIRED: Update with your test database name
        /// Common values: "TESTDB", "PUBLIC", "SAMPLE_DATA"
        /// </summary>
        public static string TestDatabase = "DATAVERSE";

        /// <summary>
        /// Schema name for testing
        /// REQUIRED: Update with your test schema name
        /// Common values: "PUBLIC", "INFORMATION_SCHEMA", "TEST_SCHEMA"
        /// </summary>
        public static string TestSchema = "PUBLIC";

        /// <summary>
        /// Warehouse name for testing
        /// REQUIRED: Update with your Snowflake warehouse name
        /// Common values: "COMPUTE_WH", "XSMALL_WH", "TEST_WAREHOUSE"
        /// </summary>
        public static string TestWarehouse = "XSMALL";

        /// <summary>
        /// Role for testing
        /// REQUIRED: Update with your Snowflake role name
        /// Common values: "SYSADMIN", "PUBLIC", "ACCOUNTADMIN", "USERADMIN"
        /// </summary>
        public static string TestRole = "ACCOUNTADMIN";

        // ====== MOCK IMPLEMENTATION ======

        public T GetProperty<T>(string key)
        {
            if (key.Equals(Constants.Server, StringComparison.OrdinalIgnoreCase))
            {
                return (T)Convert.ChangeType(TestSnowflakeInstance, typeof(T));
            }

            if (key.Equals(Constants.Database, StringComparison.OrdinalIgnoreCase))
            {
                return (T)Convert.ChangeType(TestDatabase, typeof(T));
            }

            if (key.Equals(Constants.Role, StringComparison.OrdinalIgnoreCase))
            {
                return (T)Convert.ChangeType(TestRole, typeof(T));
            }

            if (key.Equals(Constants.Warehouse, StringComparison.OrdinalIgnoreCase))
            {
                return (T)Convert.ChangeType(TestWarehouse, typeof(T));
            }

            if (key.Equals(Constants.Schema, StringComparison.OrdinalIgnoreCase))
            {
                return (T)Convert.ChangeType(TestSchema, typeof(T));
            }

            throw new ArgumentException();
        }

        public Uri GetReferrerUri()
        {
            return new Uri("http://localhost");
        }

        public IToken GetToken()
        {
            // Get the token from the request header.
            // For local testing, pass the token through the Authorization header or set it here.
            string authToken = HttpContext.Current.Request.Headers["Authorization"] ?? string.Empty;

            // Remove the "Bearer " prefix if it exists.
            if (authToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                authToken = authToken.Substring("Bearer ".Length).Trim();
            }

            return new TokenMock(authToken);
        }

        public bool PropertyExists(string key)
        {
            return true;
        }

        public bool TryGetProperty<T>(string key, out T value)
        {
            value = default;
            if (key.Equals("$parameterSet", StringComparison.OrdinalIgnoreCase))
            {
                value = (T)Convert.ChangeType("oauthSP", typeof(T));
            }

            return true;
        }
    }

    public class TokenMock : IToken
    {
        private readonly string token;

        public TokenMock() : this(string.Empty) { }

        public TokenMock(string token)
        {
            this.token = token;
        }
        public string AccessToken => token;

        public string TokenType => "AAD";

        public string UId => null;

        public string RefreshChangeStamp => "2";

        public DateTime TokenAcquireTime => DateTime.UtcNow.AddMinutes(-1);
    }
}