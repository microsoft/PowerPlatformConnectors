using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace SnowflakeTestApp.Tests.Data
{
    /// <summary>
    /// Base class for integration tests that create their own tables, which are dropped after every test.
    /// </summary>
    [TestClass]
    public abstract class ScratchTableIntegrationTest : BaseIntegrationTest
    {
        protected const string TestDataset = "default";

        private readonly List<string> createdTables = new List<string>();

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            EnsureApplicationIsRunning();

            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetTestToken()}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [TestCleanup]
        public override void TestCleanup()
        {
            foreach (var table in createdTables)
            {
                try
                {
                    DataSeeder.ExecuteSqlStatement($"DROP TABLE IF EXISTS {table}").GetAwaiter().GetResult();
                }
                catch (Exception)
                {
                    // Ignore cleanup errors to prevent masking test failures
                }
            }

            createdTables.Clear();
            base.TestCleanup();
        }

        protected async Task CreateTable(string name, string columns)
        {
            createdTables.Add(name);
            await Sql($"CREATE OR REPLACE TABLE {name} ({columns})");
        }

        protected Task<string> Sql(string statement)
        {
            return DataSeeder.ExecuteSqlStatement(statement);
        }

        protected async Task<long> Count(string table, string where = null)
        {
            var sql = $"SELECT COUNT(*) AS RESULT FROM {table}" + (where == null ? string.Empty : $" WHERE {where}");
            return Convert.ToInt64(await Scalar(sql));
        }

        /// <summary>
        /// Runs a query returning at most one row and returns the value of its first column.
        /// </summary>
        protected async Task<string> Scalar(string sql)
        {
            var response = JObject.Parse(await Sql(sql));
            var rows = response.GetValue("Data", StringComparison.OrdinalIgnoreCase) as JArray;
            Assert.IsNotNull(rows, $"Unexpected SQL response for: {sql}");
            Assert.IsTrue(rows.Count <= 1, $"Expected at most one row for: {sql}");

            if (rows.Count == 0)
            {
                return null;
            }

            var value = ((JObject)rows[0]).Properties().First().Value;
            return value.Type == JTokenType.Null ? null : value.ToString();
        }

        /// <summary>
        /// Returns the schema.items object of the table metadata, which has the column properties and the required columns.
        /// </summary>
        protected async Task<JObject> GetRowSchema(string table)
        {
            var response = await HttpClient.GetAsync($"{BaseUrl}/$metadata.json/datasets/{TestDataset}/tables/{table}");
            var content = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, content);
            return (JObject)JObject.Parse(content)["schema"]["items"];
        }

        protected async Task<JObject> GetTableProperties(string table)
        {
            return (JObject)(await GetRowSchema(table))["properties"];
        }

        /// <summary>
        /// Asserts the key type and key order of a column, where a null key order means the column must not have one.
        /// </summary>
        protected static void AssertKeyColumn(JObject properties, string column, string keyType, int? keyOrder)
        {
            var property = properties[column] as JObject;
            Assert.IsNotNull(property, $"Missing column {column}");
            Assert.AreEqual(keyType, (string)property["x-ms-keyType"], $"{column} key type");
            if (keyOrder == null)
            {
                Assert.IsNull(property["x-ms-keyOrder"], $"{column} must not have a key order");
            }
            else
            {
                Assert.AreEqual(keyOrder, (int?)property["x-ms-keyOrder"], $"{column} key order");
            }
        }

        protected string ItemsUrl(string table)
        {
            return $"{BaseUrl}/datasets('{TestDataset}')/tables('{table}')/items";
        }

        protected string ItemUrl(string table, string id)
        {
            return $"{ItemsUrl(table)}('{id}')";
        }

        protected Task<HttpResponseMessage> Post(string table, object body)
        {
            return HttpClient.PostAsync(ItemsUrl(table), CreateJsonContent(body));
        }

        protected Task<HttpResponseMessage> Patch(string url, object body)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = CreateJsonContent(body),
            };

            return HttpClient.SendAsync(request);
        }
    }
}
