using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SnowflakeTestApp.Tests.Connection
{
    /// <summary>
    /// Integration tests for the /testconnection endpoint.
    /// </summary>
    [TestClass]
    public class TestConnectionEndpointIntegrationTest : BaseIntegrationTest
    {
        private const string TEST_CONNECTION_ENDPOINT = "/testconnection";
        private const string INVALID_TOKEN = "invalid-token";
        private const string MALFORMED_AUTH_HEADER = "InvalidFormat";
        private const string BEARER_TOKEN_MISSING_ERROR = "Bearer token is missing in the HTTP request authorization header.";
        private const string INVALID_OAUTH_TOKEN_ERROR = "Invalid OAuth access token.";
        private const string POST_METHOD_NOT_ALLOWED_ERROR = "The requested resource does not support http method 'POST'.";

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            EnsureApplicationIsRunning();
        }

        /// <summary>
        /// Test the /testconnection endpoint with authentication
        /// </summary>
        [TestMethod]
        public async Task TestConnectionEndpoint_WithValidAuth_ReturnsOK()
        {
            var testToken = GetTestToken();
            AddAuthorizationHeader(testToken);

            var response = await HttpClient.GetAsync($"{BaseUrl}{TEST_CONNECTION_ENDPOINT}");
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Test the endpoint without authentication
        /// </summary>
        [TestMethod]
        public async Task TestConnectionEndpoint_WithoutAuth_ReturnsInternalServerError()
        {
            var response = await HttpClient.GetAsync($"{BaseUrl}{TEST_CONNECTION_ENDPOINT}");
            var content = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            StringAssert.Contains(content, BEARER_TOKEN_MISSING_ERROR);
        }

        /// <summary>
        /// Test the endpoint with invalid authentication
        /// </summary>
        [TestMethod]
        public async Task TestConnectionEndpoint_WithInvalidAuth_ReturnsInternalServerError()
        {
            AddAuthorizationHeader(INVALID_TOKEN);

            var response = await HttpClient.GetAsync($"{BaseUrl}{TEST_CONNECTION_ENDPOINT}");
            var content = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            StringAssert.Contains(content, INVALID_OAUTH_TOKEN_ERROR);
        }

        /// <summary>
        /// Test the endpoint with malformed authorization header
        /// </summary>
        [TestMethod]
        public async Task TestConnectionEndpoint_WithMalformedAuth_ReturnsInternalServerError()
        {
            HttpClient.DefaultRequestHeaders.Add("Authorization", MALFORMED_AUTH_HEADER);

            var response = await HttpClient.GetAsync($"{BaseUrl}{TEST_CONNECTION_ENDPOINT}");
            var content = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            StringAssert.Contains(content, INVALID_OAUTH_TOKEN_ERROR);
        }

        /// <summary>
        /// Test the endpoint with POST method (should not be allowed)
        /// </summary>
        [TestMethod]
        public async Task TestConnectionEndpoint_WithPOST_ReturnsMethodNotAllowed()
        {
            var testToken = GetTestToken();
            AddAuthorizationHeader(testToken);

            var response = await HttpClient.PostAsync($"{BaseUrl}{TEST_CONNECTION_ENDPOINT}", new StringContent(""));
            var content = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.MethodNotAllowed, response.StatusCode);
            StringAssert.Contains(content, POST_METHOD_NOT_ALLOWED_ERROR);
        }

        private void AddAuthorizationHeader(string token)
        {
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }
} 