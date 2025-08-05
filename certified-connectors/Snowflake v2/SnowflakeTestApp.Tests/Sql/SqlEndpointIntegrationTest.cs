using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;

namespace SnowflakeTestApp.Tests.Sql
{
    /// <summary>
    /// Integration tests for the SQL endpoints.
    /// These tests document the expected behavior and can be used to verify the endpoints manually.
    /// This test class uses the globally seeded CUSTOMERS test table.
    /// </summary>
    [TestClass]
    public class SqlEndpointIntegrationTest : BaseIntegrationTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            EnsureApplicationIsRunning();
        }

        /// <summary>
        /// Test querying the seeded test data with specific customer information
        /// </summary>
        [TestMethod]
        public async Task QuerySeededTestData_WithAuth_ReturnsOK()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Add("Instance", TestData.DefaultSnowflakeHostname);
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var sqlPayload = new
            {
                statement = $"SELECT NAME, EMAIL, BALANCE FROM {TestData.DefaultTable} WHERE NAME = 'John Doe' OR NAME = 'Jane Smith' ORDER BY NAME",
                timeout = TestData.DefaultSqlTimeout,
                database = TestData.DefaultDatabase,
                schema = TestData.DefaultSchema,
                warehouse = TestData.DefaultWarehouse,
                role = TestData.DefaultRole
            };

            var json = JsonConvert.SerializeObject(sqlPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync($"{BaseUrl}/sql", content);
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();

            StringAssert.Contains(responseContent, "John Doe");
            StringAssert.Contains(responseContent, "Jane Smith");
        }

        /// <summary>
        /// Test the POST /sql endpoint without authentication
        /// </summary>
        [TestMethod]
        public async Task ExecuteSqlStatementEndpoint_WithoutAuth_ReturnsBadRequest()
        {
            HttpClient.DefaultRequestHeaders.Add("Instance", TestData.DefaultSnowflakeHostname);
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var sqlPayload = new
            {
                statement = TestData.SampleSqlStatement,
                timeout = TestData.DefaultSqlTimeout,
                database = TestData.DefaultDatabase,
                schema = TestData.DefaultSchema,
                warehouse = TestData.DefaultWarehouse,
                role = TestData.DefaultRole
            };

            var json = JsonConvert.SerializeObject(sqlPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync($"{BaseUrl}/sql", content);
            
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Test the POST /sql endpoint with missing Instance header
        /// </summary>
        [TestMethod]
        public async Task ExecuteSqlStatementEndpoint_WithMissingInstance_ReturnsBadRequest()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var sqlPayload = new
            {
                statement = "SELECT 1 as test_column;",
                timeout = 60
            };

            var json = JsonConvert.SerializeObject(sqlPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync($"{BaseUrl}/sql", content);
            
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Test the POST /sql endpoint with missing Accept header
        /// </summary>
        [TestMethod]
        public async Task ExecuteSqlStatementEndpoint_WithMissingAccept_ReturnsBadRequest()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Add("Instance", TestData.DefaultSnowflakeHostname);

            var sqlPayload = new
            {
                statement = TestData.SampleSqlStatement,
                timeout = TestData.DefaultSqlTimeout,
                database = TestData.DefaultDatabase,
                schema = TestData.DefaultSchema,
                warehouse = TestData.DefaultWarehouse,
                role = TestData.DefaultRole
            };

            var json = JsonConvert.SerializeObject(sqlPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync($"{BaseUrl}/sql", content);
            
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Test the POST /sql endpoint with empty SQL statement
        /// </summary>
        [TestMethod]
        public async Task ExecuteSqlStatementEndpoint_WithEmptyStatement_ReturnsBadRequest()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Add("Instance", TestData.DefaultSnowflakeHostname);
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var sqlPayload = new
            {
                statement = "",
                timeout = TestData.DefaultSqlTimeout,
                database = TestData.DefaultDatabase,
                schema = TestData.DefaultSchema,
                warehouse = TestData.DefaultWarehouse,
                role = TestData.DefaultRole
            };

            var json = JsonConvert.SerializeObject(sqlPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync($"{BaseUrl}/sql", content);
            
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Test the complete SQL execution flow: execute SQL statement then get results using statement handle
        /// </summary>
        [TestMethod]
        public async Task SqlExecutionFlow_ExecuteThenGetResults_ReturnsOk()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Add("Instance", TestData.DefaultSnowflakeHostname);
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // Execute SQL statement
            var sqlPayload = new
            {
                statement = "SELECT 1 as test_column, CURRENT_USER() as current_user",
                timeout = TestData.DefaultSqlTimeout,
                database = TestData.DefaultDatabase,
                schema = TestData.DefaultSchema,
                warehouse = TestData.DefaultWarehouse,
                role = TestData.DefaultRole
            };

            var json = JsonConvert.SerializeObject(sqlPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var executeResponse = await HttpClient.PostAsync($"{BaseUrl}/sql", content);
            
            Assert.AreEqual(HttpStatusCode.OK, executeResponse.StatusCode);
            var executeResponseContent = await executeResponse.Content.ReadAsStringAsync();
            Assert.IsFalse(string.IsNullOrEmpty(executeResponseContent), "Execute response content should not be empty");
            var executeData = JsonConvert.DeserializeObject<JObject>(executeResponseContent);
            Assert.IsNotNull(executeData["Data"], "Response should contain data");
            Assert.AreEqual(1, executeData["Data"][0]["TEST_COLUMN"].Value<int>(), "test_column should equal 1");

            // Extract statement handle from response
            var executeResult = JsonConvert.DeserializeObject<JObject>(executeResponseContent);
            string statementHandle = executeResult["Metadata"]["StatementHandle"].ToString();
            Assert.IsFalse(string.IsNullOrEmpty(statementHandle), "Statement handle should be present in response");

            // Get results using statement handle
            var getResultsContent = new StringContent("{}", Encoding.UTF8, "application/json");
            var resultsResponse = await HttpClient.PostAsync($"{BaseUrl}/sql/{statementHandle}", getResultsContent);
            
            Assert.AreEqual(HttpStatusCode.OK, resultsResponse.StatusCode);
            var resultsResponseContent = await resultsResponse.Content.ReadAsStringAsync();
            Assert.IsFalse(string.IsNullOrEmpty(resultsResponseContent), "Results response content should not be empty");

            // Verify the results contain expected data
            var resultsData = JsonConvert.DeserializeObject<JObject>(resultsResponseContent);
            Assert.IsNotNull(resultsData["Data"], "Response should contain data");
            Assert.IsTrue(JToken.DeepEquals(resultsData["Data"][0], executeData["Data"][0]), "The SQL results do not match the expected data.");
        }
        
        /// <summary>
        /// Test the GET /sql/results endpoint for getting statement results with authentication
        /// </summary>
        [TestMethod]
        public async Task GetResultsEndpoint_WithAuth_ReturnsNotFound()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");

            var response = await HttpClient.PostAsync($"{BaseUrl}/sql/results/test-handle", new StringContent(""));
            
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// Test the POST /sql/cancel endpoint for cancelling statements with authentication
        /// </summary>
        [TestMethod]
        public async Task CancelStatementEndpoint_WithAuth_ReturnsBadRequest()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");

            var response = await HttpClient.PostAsync($"{BaseUrl}/sql/test-handle/cancel", new StringContent(""));
            
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        /// <summary>
        /// Test cancellation of already completed SQL statement - should return 422
        /// </summary>
        [TestMethod]
        public async Task SqlExecutionFlow_CancelCompletedStatement_Returns422()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Add("Instance", TestData.DefaultSnowflakeHostname);
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // Execute quick SQL statement that completes immediately
            var sqlPayload = new
            {
                statement = "SELECT 1",
                timeout = TestData.DefaultSqlTimeout,
                database = TestData.DefaultDatabase,
                schema = TestData.DefaultSchema,
                warehouse = TestData.DefaultWarehouse,
                role = TestData.DefaultRole
            };

            var json = JsonConvert.SerializeObject(sqlPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var executeResponse = await HttpClient.PostAsync($"{BaseUrl}/sql", content);
            
            Assert.AreEqual(HttpStatusCode.OK, executeResponse.StatusCode);
            var executeResponseContent = await executeResponse.Content.ReadAsStringAsync();
            Assert.IsFalse(string.IsNullOrEmpty(executeResponseContent), "Execute response content should not be empty");

            // Extract statement handle from response
            var executeResult = JsonConvert.DeserializeObject<JObject>(executeResponseContent);
            string statementHandle = executeResult["Metadata"]["StatementHandle"].ToString();
            Assert.IsFalse(string.IsNullOrEmpty(statementHandle), "Statement handle should be present in response");

            // Try to cancel already completed statement - should return 422
            var cancelContent = new StringContent("{}", Encoding.UTF8, "application/json");
            var cancelResponse = await HttpClient.PostAsync($"{BaseUrl}/sql/{statementHandle}/cancel", cancelContent);
            
            Assert.AreEqual(422, (int)cancelResponse.StatusCode, "Cancel should return 422 for already completed statement");
        }
    }
} 
