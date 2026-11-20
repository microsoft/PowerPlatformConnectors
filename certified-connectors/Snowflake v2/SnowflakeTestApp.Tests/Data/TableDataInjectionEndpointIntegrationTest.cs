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
    /// assert that identifier/clause injection attempts are rejected before any SQL is executed.
    ///
    /// The primary case reproduces a vulnerability confirmed against the live environment: passing
    /// <c>(SELECT CURRENT_USER()) t</c> as the "table name" to
    /// <c>GET /datasets('default')/tables('{table}')/items</c> caused the unpatched backend to build
    /// <c>SELECT * FROM (SELECT CURRENT_USER()) t ...</c> and return the Snowflake session user.
    /// A patched backend must reject the request (via <c>EnsureValidQualifiedSnowflakeIdentifier</c>)
    /// and must never echo the exfiltrated value. Identifier, clause and $filter search-term injection
    /// attempts are covered the same way.
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
        /// Control: a legitimate table must still return 200. This proves the endpoint/route works,
        /// so the rejections asserted by the injection tests below are specific to the payloads and
        /// not an unrelated failure.
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_WithValidTable_ReturnsOk_Control()
        {
            AddAuthHeaders();

            var response = await HttpClient.GetAsync(
                $"{BaseUrl}/datasets('{TestDataset}')/tables('{ControlTable}')/items");
            var content = await response.Content.ReadAsStringAsync();
            LogResponse("Control (valid table)", response, content);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode,
                "Control request with a valid table name should succeed.");
        }

        /// <summary>
        /// Exact reproduction of the live-confirmed exploit. Asserts the request is rejected AND the
        /// response body does not contain the Snowflake session user (i.e. the subquery never ran).
        /// The current user is resolved up front via the /sql endpoint so we know the precise value a
        /// successful injection would leak.
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_WithCurrentUserSubqueryTableName_IsRejectedAndDoesNotLeak()
        {
            var currentUser = await GetCurrentSnowflakeUserAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(currentUser),
                "Precondition: should be able to resolve CURRENT_USER() via the /sql endpoint.");

            AddAuthHeaders();

            const string injectionTable = "(SELECT CURRENT_USER()) t";
            var url = $"{BaseUrl}/datasets('{TestDataset}')/tables('{Uri.EscapeDataString(injectionTable)}')/items";

            var response = await HttpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            LogResponse($"Table-name injection '{injectionTable}'", response, content);

            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode,
                $"Table-name injection must be rejected before any SQL runs. Status: {response.StatusCode}, Body: {content}");

            Assert.IsFalse(
                content.IndexOf(currentUser, StringComparison.OrdinalIgnoreCase) >= 0,
                "Response must not contain CURRENT_USER(); its presence means the injected subquery executed.");
        }

        /// <summary>
        /// Same exploit as above but with the payload <em>double</em> URL-encoded, mirroring the exact
        /// wire format used against the live environment. The controller decodes the table twice
        /// (OData model binding, then <c>HttpUtility.UrlDecode</c>), so a double-encoded value collapses
        /// back to <c>(SELECT CURRENT_USER()) t</c> before validation. This is defense-in-depth against
        /// decode surprises: it must still be rejected and must not leak the session user.
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_WithDoubleEncodedCurrentUserSubquery_IsRejectedAndDoesNotLeak()
        {
            var currentUser = await GetCurrentSnowflakeUserAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(currentUser),
                "Precondition: should be able to resolve CURRENT_USER() via the /sql endpoint.");

            AddAuthHeaders();

            const string injectionTable = "(SELECT CURRENT_USER()) t";
            var doubleEncoded = Uri.EscapeDataString(Uri.EscapeDataString(injectionTable));
            var url = $"{BaseUrl}/datasets('{TestDataset}')/tables('{doubleEncoded}')/items";

            var response = await HttpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            LogResponse($"Double-encoded table-name injection '{doubleEncoded}'", response, content);

            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode,
                $"Double-encoded table-name injection must be rejected. Status: {response.StatusCode}, Body: {content}");

            Assert.IsFalse(
                content.IndexOf(currentUser, StringComparison.OrdinalIgnoreCase) >= 0,
                "Response must not contain CURRENT_USER(); its presence means the injected subquery executed.");
        }

        /// <summary>
        /// Additional read-only, non-destructive injection payloads supplied as the table name. Each
        /// must be rejected (identifier validation fails before the statement is built/executed).
        /// </summary>
        [DataTestMethod]
        [DataRow("(SELECT CURRENT_USER()) t")]
        [DataRow("(SELECT 1) x")]
        [DataRow("CUSTOMERS UNION SELECT CURRENT_USER()")]
        [DataRow("CUSTOMERS WHERE 1=1")]
        [DataRow("CUSTOMERS--")]
        public async Task GetItemsEndpoint_WithMaliciousTableName_IsRejected(string injectionTable)
        {
            AddAuthHeaders();

            var url = $"{BaseUrl}/datasets('{TestDataset}')/tables('{Uri.EscapeDataString(injectionTable)}')/items";

            var response = await HttpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            LogResponse($"Malicious table name '{injectionTable}'", response, content);

            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode,
                $"Malicious table name '{injectionTable}' must be rejected. Status: {response.StatusCode}, Body: {content}");
        }

        /// <summary>
        /// Injection via the OData <c>$orderby</c> clause against a valid table must be rejected
        /// (via <c>EnsureValidOrderByClause</c>).
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_WithMaliciousOrderBy_IsRejected()
        {
            AddAuthHeaders();

            const string maliciousOrderBy = "NAME; SELECT CURRENT_USER()";
            var url = $"{BaseUrl}/datasets('{TestDataset}')/tables('{ControlTable}')/items?$orderby={Uri.EscapeDataString(maliciousOrderBy)}";

            var response = await HttpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            LogResponse("Malicious $orderby", response, content);

            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode,
                $"Malicious $orderby must be rejected. Status: {response.StatusCode}, Body: {content}");
        }

        /// <summary>
        /// Injection via the OData <c>$select</c> clause against a valid table must be rejected
        /// (via <c>EnsureValidSelectClause</c>) and must not leak the session user.
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_WithMaliciousSelect_IsRejectedAndDoesNotLeak()
        {
            var currentUser = await GetCurrentSnowflakeUserAsync();
            Assert.IsFalse(string.IsNullOrWhiteSpace(currentUser),
                "Precondition: should be able to resolve CURRENT_USER() via the /sql endpoint.");

            AddAuthHeaders();

            const string maliciousSelect = "NAME, (SELECT CURRENT_USER())";
            var url = $"{BaseUrl}/datasets('{TestDataset}')/tables('{ControlTable}')/items?$select={Uri.EscapeDataString(maliciousSelect)}";

            var response = await HttpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            LogResponse("Malicious $select", response, content);

            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode,
                $"Malicious $select must be rejected. Status: {response.StatusCode}, Body: {content}");

            Assert.IsFalse(
                content.IndexOf(currentUser, StringComparison.OrdinalIgnoreCase) >= 0,
                "Response must not contain CURRENT_USER(); its presence means the injected projection executed.");
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
