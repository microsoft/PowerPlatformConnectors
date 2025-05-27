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
    /// Provides test values for connection parameters.Update these to your test values before deployment or debugging.
    /// </summary>
    public class ConnectionParametersProviderMock : IConnectionParametersProvider
    {
        public T GetProperty<T>(string key)
        {
            if (key.Equals(Constants.Server, StringComparison.OrdinalIgnoreCase))
            {
                string testServer = "tlpycol-taa70859.snowflakecomputing.com";
                return (T)Convert.ChangeType(testServer, typeof(T)); ;
            }

            if (key.Equals(Constants.Database, StringComparison.OrdinalIgnoreCase))
            {
                string testDatabase = "SNOWFLAKE_TEST_DATA";
                return (T)Convert.ChangeType(testDatabase, typeof(T)); ;
            }

            if (key.Equals(Constants.Role, StringComparison.OrdinalIgnoreCase))
            {
                string testRole = "ACCOUNTADMIN";
                return (T)Convert.ChangeType(testRole, typeof(T)); ;
            }

            if (key.Equals(Constants.Warehouse, StringComparison.OrdinalIgnoreCase))
            {
                string testWarehouse = "COMPUTE_WH";
                return (T)Convert.ChangeType(testWarehouse, typeof(T)); ;
            }

            if (key.Equals(Constants.Schema, StringComparison.OrdinalIgnoreCase))
            {
                string testSchema = "PUBLIC";
                return (T)Convert.ChangeType(testSchema, typeof(T)); ;
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