using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SnowflakeTestApp.Tests.Infrastructure;

namespace SnowflakeTestApp.Tests
{
    /// <summary>
    /// Base class for integration tests providing common functionality and test data management.
    /// </summary>
    [TestClass]
    public abstract class BaseIntegrationTest
    {
        private const int APPLICATION_HEALTH_CHECK_TIMEOUT_SECONDS = 5;
        private const string BEARER_TOKEN_CONFIGURATION_ERROR = 
            "Bearer token not configured. Please update ConnectionParametersProviderMock.TestBearerToken with a valid OAuth bearer token. See README.md for instructions.";
        private const string APPLICATION_NOT_RUNNING_ERROR = 
            "SnowflakeTestApp is not running at {0}. Please start the application before running tests. Error: {1}";

        protected static AccessTokenService AccessTokenService;
        protected string BaseUrl => TestData.BaseUrl;
        protected HttpClient HttpClient;
        protected static TestDataSeeder DataSeeder;
        protected static List<TestDataRecord> SeededTestData => DataSeeder?.SeededRecords ?? new List<TestDataRecord>();
        public TestContext TestContext { get; set; }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            InitializeAccessTokenService();
            InitializeTestDataSeeder();
            SeedTestData();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            CleanupTestResources();
        }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            HttpClient = CreateHttpClient();
        }

        [TestCleanup] 
        public virtual void TestCleanup()
        {
            HttpClient?.Dispose();
        }

        protected string GetTestToken()
        {
            var service = new AccessTokenService(TestData.TenantId, TestData.ClientId, TestData.ClientSecret, TestData.Scope);
            var token = service.GetAccessTokenAsync().GetAwaiter().GetResult();
            return token;
        }

        protected void EnsureApplicationIsRunning()
        {
            try
            {
                ValidateApplicationHealth();
            }
            catch (Exception ex)
            {
                Assert.Inconclusive(string.Format(APPLICATION_NOT_RUNNING_ERROR, BaseUrl, ex.Message));
            }
        }

        protected T DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = response.Content.ReadAsStringAsync().Result;
            
            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (JsonException ex)
            {
                var errorMessage = $"Failed to deserialize response content as {typeof(T).Name}. Content: {content}. Error: {ex.Message}";
                Assert.Fail(errorMessage);
                return default(T);
            }
        }

        protected StringContent CreateJsonContent(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        }

        protected async Task<List<TestDataRecord>> FetchActualDataFromDatabase(string tableName = null)
        {
            using (var httpClient = CreateHttpClient())
            {
                var dataSeeder = new TestDataSeeder(httpClient, TestData.BaseUrl, AccessTokenService);
                return await dataSeeder.FetchTestDataFromDatabase(tableName);
            }
        }

        protected async Task<TestDataRecord> FetchActualRecordById(int id, string tableName = null)
        {
            using (var httpClient = CreateHttpClient())
            {
                var dataSeeder = new TestDataSeeder(httpClient, TestData.BaseUrl, AccessTokenService);
                return await dataSeeder.FetchTestRecordById(id, tableName);
            }
        }

        protected void ValidateDataMatches(List<TestDataRecord> expectedRecords, List<TestDataRecord> actualRecords, string message = null)
        {
            var assertionMessage = message ?? "Database records should match seeded test data";
            
            Assert.AreEqual(expectedRecords.Count, actualRecords.Count, $"{assertionMessage}: Record count mismatch");
            
            ValidateIndividualRecords(expectedRecords, actualRecords, assertionMessage);
        }

        protected void ValidateRecordMatches(TestDataRecord expected, TestDataRecord actual, string message = null)
        {
            var assertionMessage = message ?? $"Record with ID {expected?.Id} should match expected values";
            
            Assert.IsNotNull(actual, $"{assertionMessage}: Record not found in database");
            Assert.AreEqual(expected, actual, assertionMessage);
        }
        private static void InitializeAccessTokenService()
        {
            AccessTokenService = new AccessTokenService(TestData.TenantId, TestData.ClientId, TestData.ClientSecret, TestData.Scope);
        }
        private static void InitializeTestDataSeeder()
        {
            var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(TestData.DefaultTimeoutSeconds)
            };
            
            DataSeeder = new TestDataSeeder(httpClient, TestData.BaseUrl, AccessTokenService);
        }

        private static void SeedTestData()
        {
            try
            {
                var success = DataSeeder.EnsureTestTableExistsAndSeed(TestData.DefaultTable, TestData.DefaultDataset)
                    .GetAwaiter().GetResult();
                
                if (!success)
                {
                    throw new InvalidOperationException($"Failed to setup test table '{TestData.DefaultTable}'");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Test data seeding failed: {ex.Message}", ex);
            }
        }

        private static void CleanupTestResources()
        {
            try
            {
                DataSeeder?.CleanupTestTable(TestData.DefaultTable).GetAwaiter().GetResult();
                DataSeeder?.Dispose();
            }
            catch (Exception)
            {
                // Ignore cleanup errors to prevent masking test failures
            }
        }

        private HttpClient CreateHttpClient()
        {
            return new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(TestData.DefaultTimeoutSeconds)
            };
        }

        private void ValidateApplicationHealth()
        {
            using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(APPLICATION_HEALTH_CHECK_TIMEOUT_SECONDS) })
            {
                var response = client.GetAsync(BaseUrl).Result;
            }
        }

        private void ValidateIndividualRecords(List<TestDataRecord> expectedRecords, List<TestDataRecord> actualRecords, string assertionMessage)
        {
            foreach (var expected in expectedRecords)
            {
                var actual = actualRecords.FirstOrDefault(r => r.Id == expected.Id);
                
                Assert.IsNotNull(actual, $"{assertionMessage}: Record with ID {expected.Id} not found in database");
                Assert.AreEqual(expected, actual, $"{assertionMessage}: Record with ID {expected.Id} does not match expected values");
            }
        }
    }
} 