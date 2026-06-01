using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SnowflakeTestApp.Tests.Infrastructure
{
    /// <summary>
    /// Handles creation and seeding of test tables with sample data.
    /// </summary>
    public class TestDataSeeder : IDisposable
    {
        private const string SQL_ENDPOINT = "/sql";
        private const string APPLICATION_JSON = "application/json";
        private const string CREATE_TABLE_SQL_TEMPLATE = @"
                CREATE OR ALTER TABLE {0} (
                    ID NUMBER PRIMARY KEY,
                    NAME VARCHAR(255) NOT NULL,
                    EMAIL VARCHAR(255),
                    PHONE VARCHAR(50),
                    CREATED_DATE TIMESTAMP DEFAULT CURRENT_TIMESTAMP(),
                    IS_ACTIVE BOOLEAN DEFAULT TRUE,
                    BALANCE NUMBER(10,2) DEFAULT 0.00
                )";
        private const string INSERT_DATA_SQL_TEMPLATE = "INSERT INTO {0} (ID, NAME, EMAIL, PHONE, IS_ACTIVE, BALANCE) VALUES {1}";
        private const string TRUNCATE_TABLE_SQL_TEMPLATE = "TRUNCATE TABLE IF EXISTS {0}";
        private const string SELECT_ALL_SQL_TEMPLATE = "SELECT ID, NAME, EMAIL, PHONE, IS_ACTIVE, BALANCE FROM {0} ORDER BY ID";
        private const string SELECT_BY_ID_SQL_TEMPLATE = "SELECT ID, NAME, EMAIL, PHONE, IS_ACTIVE, BALANCE FROM {0} WHERE ID = {1}";

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly AccessTokenService _accessTokenService;

        /// <summary>
        /// Gets the test records that were seeded into the database
        /// This allows tests to validate against the same data structure
        /// </summary>
        public List<TestDataRecord> SeededRecords { get; private set; } = new List<TestDataRecord>();

        public TestDataSeeder(HttpClient httpClient, string baseUrl, AccessTokenService accessTokenService)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
            _accessTokenService = accessTokenService ?? throw new ArgumentNullException(nameof(accessTokenService));
        }

        /// <summary>
        /// Creates test table if it doesn't exist and seeds it with sample data
        /// </summary>
        /// <param name="tableName">Name of the table to create/seed (defaults to TestData.DefaultTable)</param>
        /// <param name="dataset">Dataset name (defaults to TestData.DefaultDataset)</param>
        public async Task<bool> EnsureTestTableExistsAndSeed(string tableName = null, string dataset = null)
        {
            tableName = tableName ?? TestData.DefaultTable;
            dataset = dataset ?? TestData.DefaultDataset;

            try
            {
                await CreateTableIfNotExists(tableName);
                await CleanupTestTable(tableName);
                
                SeededRecords = SampleTestData.GetDefaultTestRecords();
                await SeedTableWithSampleData(tableName, SeededRecords);
                
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not setup test table '{tableName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Seeds the test table with the provided test records
        /// </summary>
        /// <param name="tableName">Name of the table to seed</param>
        /// <param name="records">Test records to insert</param>
        public async Task SeedCustomTestData(string tableName, List<TestDataRecord> records)
        {
            try
            {
                await CreateTableIfNotExists(tableName);
                await CleanupTestTable(tableName);
                SeededRecords = new List<TestDataRecord>(records);
                await SeedTableWithSampleData(tableName, records);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not seed custom test data into table '{tableName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a test table if it doesn't exist using SQL execution
        /// </summary>
        private async Task CreateTableIfNotExists(string tableName)
        {
            var createTableSql = string.Format(CREATE_TABLE_SQL_TEMPLATE, tableName);
            await ExecuteSqlStatement(createTableSql);
        }

        /// <summary>
        /// Seeds the test table with sample data from TestDataRecord objects
        /// </summary>
        private async Task SeedTableWithSampleData(string tableName, List<TestDataRecord> records)
        {
            ValidateRecordsForSeeding(records);

            var values = GenerateInsertValues(records);
            var seedDataSql = string.Format(INSERT_DATA_SQL_TEMPLATE, tableName, string.Join(", ", values));

            await ExecuteSqlStatement(seedDataSql);
        }

        /// <summary>
        /// Executes a SQL statement using the SQL endpoint
        /// </summary>
        public async Task<string> ExecuteSqlStatement(string sqlStatement)
        {
            try
            {
                var request = CreateSqlRequest(sqlStatement);
                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                ValidateSqlResponse(response, responseContent, sqlStatement);

                return responseContent;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to execute SQL statement: {sqlStatement}. Error: {ex.Message}", ex);
            }
        }

        private HttpRequestMessage CreateSqlRequest(string sqlStatement)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}{SQL_ENDPOINT}");
            
            AddRequestHeaders(request);
            AddRequestContent(request, sqlStatement);

            return request;
        }

        private static void ValidateRecordsForSeeding(List<TestDataRecord> records)
        {
            if (records == null || records.Count == 0)
            {
                throw new ArgumentException("No records provided for seeding", nameof(records));
            }
        }

        private static IEnumerable<string> GenerateInsertValues(List<TestDataRecord> records)
        {
            return records.Select(r => 
                $"({r.Id}, '{EscapeSqlString(r.Name)}', '{r.Email}', '{r.Phone}', {FormatBooleanForSql(r.IsActive)}, {r.Balance})"
            );
        }

        private static string EscapeSqlString(string value)
        {
            return value.Replace("'", "''");
        }

        private static string FormatBooleanForSql(bool value)
        {
            return value ? "TRUE" : "FALSE";
        }

        private void AddRequestHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("Authorization", $"Bearer {this.GetAccessToken()}");
            request.Headers.Add("Instance", TestData.DefaultSnowflakeHostname);
            request.Headers.Add("Accept", APPLICATION_JSON);
        }

        private static void AddRequestContent(HttpRequestMessage request, string sqlStatement)
        {
            var sqlPayload = CreateSqlPayload(sqlStatement);
            var json = JsonConvert.SerializeObject(sqlPayload);
            request.Content = new StringContent(json, Encoding.UTF8, APPLICATION_JSON);
        }

        private static object CreateSqlPayload(string sqlStatement)
        {
            return new
            {
                statement = sqlStatement,
                timeout = TestData.DefaultSqlTimeout,
                database = TestData.DefaultDatabase,
                schema = TestData.DefaultSchema,
                warehouse = TestData.DefaultWarehouse,
                role = TestData.DefaultRole
            };
        }

        private static void ValidateSqlResponse(HttpResponseMessage response, string responseContent, string sqlStatement)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"SQL execution failed. Status: {response.StatusCode}, Response: {responseContent}");
            }
        }

        public string GetAccessToken()
        {
            return _accessTokenService.GetAccessTokenAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Cleans up test data by truncating the test table
        /// </summary>
        /// <param name="tableName">Name of the table to cleanup (defaults to TestData.DefaultTable)</param>
        public async Task<bool> CleanupTestTable(string tableName = null)
        {
            tableName = tableName ?? TestData.DefaultTable;

            try
            {
                var truncateSql = string.Format(TRUNCATE_TABLE_SQL_TEMPLATE, tableName);
                await ExecuteSqlStatement(truncateSql);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not cleanup test table '{tableName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Fetches data from the test table and maps it to TestDataRecord objects
        /// This allows for direct comparison between seeded data and actual database content
        /// </summary>
        /// <param name="tableName">Name of the table to fetch data from</param>
        /// <returns>List of TestDataRecord objects representing the actual data in the database</returns>
        public async Task<List<TestDataRecord>> FetchTestDataFromDatabase(string tableName = null)
        {
            tableName = tableName ?? TestData.DefaultTable;

            try
            {
                var sqlStatement = string.Format(SELECT_ALL_SQL_TEMPLATE, tableName);
                var responseContent = await ExecuteSqlStatement(sqlStatement);
                
                return MapSnowflakeResponseToTestDataRecords(responseContent);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not fetch test data from table '{tableName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Fetches a specific test record by ID from the database
        /// </summary>
        /// <param name="id">ID of the record to fetch</param>
        /// <param name="tableName">Name of the table to fetch from</param>
        /// <returns>TestDataRecord if found, null otherwise</returns>
        public async Task<TestDataRecord> FetchTestRecordById(int id, string tableName = null)
        {
            tableName = tableName ?? TestData.DefaultTable;

            try
            {
                var sqlStatement = string.Format(SELECT_BY_ID_SQL_TEMPLATE, tableName, id);
                var responseContent = await ExecuteSqlStatement(sqlStatement);
                
                var records = MapSnowflakeResponseToTestDataRecords(responseContent);
                return records.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not fetch test record with ID {id} from table '{tableName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Maps the Snowflake SQL response to TestDataRecord objects
        /// This handles the JSON response structure from Snowflake
        /// </summary>
        /// <param name="snowflakeResponse">Raw JSON response from Snowflake SQL endpoint</param>
        /// <returns>List of TestDataRecord objects</returns>
        private List<TestDataRecord> MapSnowflakeResponseToTestDataRecords(string snowflakeResponse)
        {
            try
            {
                return JsonConvert.DeserializeObject<SnowflakeResponse>(snowflakeResponse).Data.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to map Snowflake response to TestDataRecord objects. Response: {snowflakeResponse}. Error: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Simple class to represent the Snowflake SQL response structure
        /// </summary>
        private class SnowflakeResponse
        {
            [JsonProperty("Data")]
            public TestDataRecord[] Data { get; set; }
        }

        /// <summary>
        /// Disposes the HttpClient used by this TestDataSeeder
        /// </summary>
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
} 