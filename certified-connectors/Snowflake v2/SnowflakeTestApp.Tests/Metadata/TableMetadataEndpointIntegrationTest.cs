using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SnowflakeTestApp.Tests.Metadata
{
    /// <summary>
    /// Integration tests for the table metadata endpoints.
    /// These tests document the expected behavior and can be used to verify the endpoints manually.
    /// </summary>
    [TestClass]
    public class TableMetadataEndpointIntegrationTest : BaseIntegrationTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            EnsureApplicationIsRunning();
        }

        /// <summary>
        /// Test the /$metadata.json/datasets/{dataset}/tables/{table} endpoint with authentication
        /// Note: This test uses the globally seeded CUSTOMERS table
        /// </summary>
        [TestMethod]
        public async Task GetTableMetadataEndpoint_WithAuth_ReturnsOk()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");

            var response = await HttpClient.GetAsync($"{BaseUrl}/$metadata.json/datasets/default/tables/CUSTOMERS");
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var metadata = JsonConvert.DeserializeObject<JObject>(content);
            var properties = metadata["schema"]["items"]["properties"];

            Assert.IsNotNull(properties["ID"]);
            Assert.IsNotNull(properties["NAME"]);
            Assert.IsNotNull(properties["EMAIL"]);
            Assert.IsNotNull(properties["PHONE"]);
            Assert.IsNotNull(properties["IS_ACTIVE"]);
            Assert.IsNotNull(properties["BALANCE"]);
            Assert.IsNotNull(properties["CREATED_DATE"]);
        }

        /// <summary>
        /// Test the /$metadata.json/datasets/{dataset}/tables/{table} endpoint without authentication
        /// </summary>
        [TestMethod]
        public async Task GetTableMetadataEndpoint_WithoutAuth_ReturnsInternalServerError()
        {
            var response = await HttpClient.GetAsync($"{BaseUrl}/$metadata.json/datasets/default/tables/CUSTOMERS");
            
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            StringAssert.Contains(content, "Bearer token is missing in the HTTP request authorization header");
        }

        /// <summary>
        /// Test the /$metadata.json/datasets/{dataset}/tables/{table} endpoint with invalid table
        /// </summary>
        [TestMethod]
        public async Task GetTableMetadataEndpoint_WithInvalidTable_ReturnsInternalServerError()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");

            var response = await HttpClient.GetAsync($"{BaseUrl}/$metadata.json/datasets/default/tables/INVALID_TABLE");
            
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            StringAssert.Contains(content, "Table 'INVALID_TABLE' does not exist or not authorized");
        }
    }
} 
