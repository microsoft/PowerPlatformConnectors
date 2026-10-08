using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace SnowflakeTestApp.Tests.Data
{
    /// <summary>
    /// End-to-end SQL-injection tests for the table data (items) endpoint. These fire real HTTP
    /// requests at a running SnowflakeTestApp (which forwards to a live Snowflake instance) and
    /// assert that injection attempts are rejected before any SQL is executed.
    ///
    /// All payloads are read-only and non-destructive.
    /// </summary>
    [TestClass]
    public class TableDataInjectionEndpointIntegrationTest : BaseIntegrationTest
    {
        private const string TestDataset = "default";
        private const string ControlTable = "CUSTOMERS";

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            EnsureApplicationIsRunning();
        }

        /// <summary>
        /// Injection via a nested function in the search-term position of contains/startswith/endswith.
        /// The vulnerable parser rendered <c>tolower('...')</c> as <c>LOWER('...')</c> and
        /// spliced it into the LIKE literal, so the payload closed the literal and appended
        /// <c>UNION SELECT CURRENT_USER()</c>; with <c>$select=NAME</c> the session user came back as a row.
        /// A patched backend must reject the filter with 400 and must not echo the session user.
        /// </summary>
        [DataTestMethod]
        [DataRow("contains(NAME, tolower(' OR 1=0 UNION SELECT CURRENT_USER() --'))")]
        [DataRow("contains(NAME, toupper(' OR 1=0 UNION SELECT CURRENT_USER() --'))")]
        [DataRow("startswith(NAME, tolower(' OR 1=0 UNION SELECT CURRENT_USER() --'))")]
        [DataRow("endswith(NAME, toupper(' OR 1=0 UNION SELECT CURRENT_USER() --'))")]
        public async Task GetItemsEndpoint_WithNestedFunctionLikeSearchTerm_IsRejectedAndDoesNotLeak(string maliciousFilter)
        {
            var currentUser = await GetCurrentSnowflakeUserAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(currentUser), "Precondition: should be able to resolve CURRENT_USER() via the /sql endpoint.");

            AddAuthHeaders();

            var url = $"{BaseUrl}/datasets('{TestDataset}')/tables('{ControlTable}')/items?$select=NAME&$filter={Uri.EscapeDataString(maliciousFilter)}";

            var response = await HttpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            LogResponse($"Malicious $filter '{maliciousFilter}'", response, content);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, $"Nested-function LIKE search term must be rejected with 400. Status: {response.StatusCode}, Body: {content}");

            Assert.IsFalse(content.IndexOf(currentUser, StringComparison.OrdinalIgnoreCase) >= 0, "Response must not contain CURRENT_USER(); its presence means the injected UNION executed.");
        }

        /// <summary>
        /// Same exploit with <c>$count=true</c>, which additionally builds a <c>SELECT COUNT(*)</c> query
        /// from the same filter. It must also be rejected with 400.
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_WithNestedFunctionLikeSearchTermAndCount_IsRejected()
        {
            AddAuthHeaders();

            const string maliciousFilter = "contains(NAME, tolower(' OR 1=0 UNION SELECT CURRENT_USER() --'))";
            var url = $"{BaseUrl}/datasets('{TestDataset}')/tables('{ControlTable}')/items?$count=true&$filter={Uri.EscapeDataString(maliciousFilter)}";

            var response = await HttpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            LogResponse("Malicious $filter with $count", response, content);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, $"Nested-function LIKE search term with $count must be rejected with 400. Status: {response.StatusCode}, Body: {content}");
        }

        /// <summary>
        /// A harmless nested function in the search-term position is rejected the same way: the search
        /// term must be a string literal, regardless of its content.
        /// </summary>
        [DataTestMethod]
        [DataRow("contains(tolower(NAME), tolower('john'))")]
        [DataRow("startswith(NAME, toupper('j'))")]
        [DataRow("contains(NAME, EMAIL)")]
        public async Task GetItemsEndpoint_WithNonLiteralLikeSearchTerm_ReturnsBadRequest(string filter)
        {
            AddAuthHeaders();

            var url = $"{BaseUrl}/datasets('{TestDataset}')/tables('{ControlTable}')/items?$filter={Uri.EscapeDataString(filter)}";

            var response = await HttpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            LogResponse($"Non-literal search term '{filter}'", response, content);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, $"Non-literal LIKE search term must be rejected with 400. Status: {response.StatusCode}, Body: {content}");
        }

        private void LogResponse(string label, HttpResponseMessage response, string content)
        {
            TestContext.WriteLine($"[{label}] HTTP {(int)response.StatusCode} {response.StatusCode}");
            TestContext.WriteLine($"[{label}] Response body: {content}");
        }

        private void AddAuthHeaders()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Resolves the current Snowflake session user via the /sql endpoint. This is the exact value
        /// a successful CURRENT_USER() injection would exfiltrate, used to assert non-leakage.
        /// </summary>
        private static async Task<string> GetCurrentSnowflakeUserAsync()
        {
            var raw = await DataSeeder.ExecuteSqlStatement("SELECT CURRENT_USER() AS USR");
            var json = JObject.Parse(raw);
            return (string)json["Data"]?[0]?["USR"];
        }
    }
}
