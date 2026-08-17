using System.Linq;
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

        /// <summary>
        /// Verifies the metadata endpoint only returns columns from the connection's active schema
        /// (PUBLIC) and does not leak columns from other schemas that happen to have a table
        /// with the same name.
        ///
        /// Setup:
        ///   - PUBLIC.DUAL_SCHEMA_TABLE          (ID, P_NAME, P_EMAIL)           — connection schema
        ///   - SCHEMA_META_OTHER.DUAL_SCHEMA_TABLE (ID, O_LABEL, O_AMOUNT, O_ACTIVE) — other schema
        ///
        /// GetTableMetadataAsync filters by TABLE_SCHEMA = [configured schema], so only the
        /// PUBLIC table's columns should appear in the response.
        /// </summary>
        [TestMethod]
        public async Task GetTableMetadataEndpoint_SameTableInTwoSchemas_ReturnsOnlyConnectionSchemaColumns()
        {
            const string tableName = "DUAL_SCHEMA_TABLE";
            const string connectionSchema = "PUBLIC";
            const string otherSchema = "DEST7";

            DataSeeder.ExecuteSqlStatement(
                $"CREATE OR REPLACE TABLE {connectionSchema}.{tableName} (" +
                "  ID NUMBER(38,0) NOT NULL," +
                "  P_NAME VARCHAR(200) NOT NULL," +
                "  P_EMAIL VARCHAR(200)," +
                "  PRIMARY KEY (ID)" +
                ")").GetAwaiter().GetResult();

            DataSeeder.ExecuteSqlStatement(
                $"CREATE OR REPLACE TABLE {otherSchema}.{tableName} (" +
                "  ID NUMBER(38,0) NOT NULL," +
                "  O_LABEL VARCHAR(200) NOT NULL," +
                "  O_AMOUNT NUMBER(10,2)," +
                "  O_ACTIVE BOOLEAN," +
                "  PRIMARY KEY (ID)" +
                ")").GetAwaiter().GetResult();

            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");

            var response = await HttpClient.GetAsync(
                $"{BaseUrl}/$metadata.json/datasets/default/tables/{tableName}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var metadata = JsonConvert.DeserializeObject<JObject>(content);
            var properties = metadata["schema"]["items"]["properties"] as JObject;
            Assert.IsNotNull(properties, "Metadata response should contain schema.items.properties");

            var columnNames = properties.Properties().Select(p => p.Name).ToList();

            // Only columns from PUBLIC (the connection schema) should be present
            Assert.IsTrue(columnNames.Contains("ID"),
                "Should contain ID from PUBLIC schema");
            Assert.IsTrue(columnNames.Contains("P_NAME"),
                "Should contain P_NAME from PUBLIC schema");
            Assert.IsTrue(columnNames.Contains("P_EMAIL"),
                "Should contain P_EMAIL from PUBLIC schema");

            Assert.AreEqual(3, columnNames.Count,
                $"Expected exactly 3 columns from PUBLIC schema but got: [{string.Join(", ", columnNames)}]");

            // Columns from table in the other schema must NOT leak through
            Assert.IsFalse(columnNames.Contains("O_LABEL"),
                "O_LABEL from SCHEMA_META_OTHER should NOT appear");
            Assert.IsFalse(columnNames.Contains("O_AMOUNT"),
                "O_AMOUNT from SCHEMA_META_OTHER should NOT appear");
            Assert.IsFalse(columnNames.Contains("O_ACTIVE"),
                "O_ACTIVE from SCHEMA_META_OTHER should NOT appear");
        }
    }
} 
