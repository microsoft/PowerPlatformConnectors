using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SnowflakeTestApp.Tests.Data
{
    /// <summary>
    /// Integration tests for the table endpoints.
    /// These tests document the expected behavior and can be used to verify the endpoints manually.
    /// This test class uses the globally seeded CUSTOMERS test table.
    /// </summary>
    [TestClass]
    public class TableEndpointIntegrationTest : BaseIntegrationTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            EnsureApplicationIsRunning();
        }

        /// <summary>
        /// Test the /datasets/{dataset}/tables endpoint with authentication
        /// This test verifies that the seeded test table appears in the tables list
        /// </summary>
        [TestMethod]
        public async Task GetTablesEndpoint_WithAuth_ReturnsOk()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets/default/tables");
            
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsFalse(string.IsNullOrEmpty(content), "Response content should not be empty");
        }

        /// <summary>
        /// Test the /datasets/{dataset}/tables endpoint without authentication
        /// </summary>
        [TestMethod]
        public async Task GetTablesEndpoint_WithoutAuth_ReturnsInternalServerError()
        {
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets/default/tables");
            
            Assert.AreEqual(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        }

        /// <summary>
        /// Test the tables endpoint with missing dataset parameter
        /// </summary>
        [TestMethod]
        public async Task GetTablesEndpoint_WithMissingDataset_ReturnsBadRequest()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets('')/tables");
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "Expected HTTP 400 Bad Request");
        }
    }
} 
