using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SnowflakeTestApp.Tests.Data
{
    /// <summary>
    /// Integration tests for the /dataset endpoint.
    /// These tests document the expected behavior and can be used to verify the endpoint manually.
    /// </summary>
    [TestClass]
    public class DatasetEndpointIntegrationTest : BaseIntegrationTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            EnsureApplicationIsRunning();
        }

        /// <summary>
        /// Test the /datasets endpoint with authentication
        /// Verifies that the default dataset is returned with proper XML structure
        /// </summary>
        [TestMethod]
        public async Task GetDatasetsEndpoint_WithAuth_ReturnsOk()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets");
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsFalse(string.IsNullOrEmpty(content), "Response content should not be empty");
            
            var xmlDoc = XDocument.Parse(content);
            var dataSetElement = xmlDoc.Descendants().FirstOrDefault(e => e.Name.LocalName == "DataSet");
            Assert.IsNotNull(dataSetElement, "Could not find DataSet element in response");

            var displayNameElement = dataSetElement.Elements().First(e => e.Name.LocalName == "DisplayName");
            Assert.AreEqual("dataset", displayNameElement.Value);

            var nameElement = dataSetElement.Elements().First(e => e.Name.LocalName == "Name");
            Assert.AreEqual("default", nameElement.Value);

            var tablesElement = dataSetElement.Elements().FirstOrDefault(e => e.Name.LocalName == "tables");
            Assert.IsNotNull(tablesElement, "tables element should exist");
            Assert.AreEqual(0, tablesElement.Elements().Count());
        }

        /// <summary>
        /// Test the /datasets endpoint with JSON accept header
        /// Verifies that the endpoint can handle different content types
        /// </summary>
        [TestMethod]
        public async Task GetDatasetsEndpoint_WithJsonAccept_ReturnsOk()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets");
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsFalse(string.IsNullOrEmpty(content), "Response content should not be empty");
        }
    }
} 
