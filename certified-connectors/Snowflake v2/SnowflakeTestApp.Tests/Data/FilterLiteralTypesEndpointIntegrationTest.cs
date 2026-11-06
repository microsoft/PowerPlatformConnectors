using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace SnowflakeTestApp.Tests.Data
{
    /// <summary>
    /// Integration tests for $filter values that OData parses as non-string literals (GUID, date, time of day and
    /// date-time with offset). The connector must send them to Snowflake as quoted literals, otherwise Snowflake reads
    /// e.g. the hyphens of a GUID or date as subtraction.
    /// This test class seeds and drops its own tables so the shared CUSTOMERS table is left untouched.
    /// </summary>
    [TestClass]
    public class FilterLiteralTypesEndpointIntegrationTest : BaseIntegrationTest
    {
        private const string TestTable = "FILTER_LITERAL_TYPES";
        private const string DateBoundaryTable = "FILTER_LITERAL_DATE_BOUNDARY";
        private const string TestDataset = "default";

        private const string Guid1 = "db4735e6-77f2-ef11-be20-000d3ae154d9";
        private const string Guid2 = "3a0806c4-b0b0-f111-aaac-6045bd40e597";
        private const string Guid3 = "7FFC8564-1D2E-4C3B-9A8F-0123456789AB";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DataSeeder.ExecuteSqlStatement($@"
                CREATE OR REPLACE TABLE {TestTable} (
                    ID NUMBER PRIMARY KEY,
                    ACCOUNTID VARCHAR(36),
                    BIRTH_DATE DATE,
                    START_TIME TIME,
                    CREATED_NTZ TIMESTAMP_NTZ,
                    CREATED_LTZ TIMESTAMP_LTZ,
                    CREATED_TZ TIMESTAMP_TZ
                )").GetAwaiter().GetResult();

            DataSeeder.ExecuteSqlStatement($@"
                INSERT INTO {TestTable} VALUES
                    (1, '{Guid1}', '2026-09-15', '12:30:00', '2026-09-15 10:20:30', '2026-09-15 10:20:30 +00:00', '2026-09-15 10:20:30 +02:00'),
                    (2, '{Guid2}', '2026-10-01', '08:00:00.25', '2026-10-01 00:00:00', '2026-10-01 00:00:00 +00:00', '2026-10-01 00:00:00 +00:00'),
                    (3, '{Guid3}', '2026-11-05', '17:10:00', '2026-11-05 17:10:00.5', '2026-11-05 17:10:00.5 +00:00', '2026-11-05 17:10:00.5 -05:00')
                ").GetAwaiter().GetResult();

            // Instants around midnight of 2026-10-01, spread far enough (-36h to +38h from UTC midnight) that every
            // session time zone puts some rows before and some after its own midnight.
            DataSeeder.ExecuteSqlStatement($@"
                CREATE OR REPLACE TABLE {DateBoundaryTable} (
                    ID NUMBER PRIMARY KEY,
                    CREATED_LTZ TIMESTAMP_LTZ,
                    CREATED_TZ TIMESTAMP_TZ
                )").GetAwaiter().GetResult();

            DataSeeder.ExecuteSqlStatement($@"
                INSERT INTO {DateBoundaryTable} VALUES
                    (1, '2026-09-29 12:00:00 +00:00', '2026-09-29 12:00:00 +00:00'),
                    (2, '2026-09-30 21:30:00 +00:00', '2026-09-30 23:30:00 +02:00'),
                    (3, '2026-09-30 22:30:00 +00:00', '2026-10-01 00:30:00 +02:00'),
                    (4, '2026-09-30 23:30:00 +00:00', '2026-09-30 23:30:00 +00:00'),
                    (5, '2026-10-01 00:30:00 +00:00', '2026-10-01 00:30:00 +00:00'),
                    (6, '2026-10-01 06:30:00 +00:00', '2026-09-30 23:30:00 -07:00'),
                    (7, '2026-10-01 07:30:00 +00:00', '2026-10-01 00:30:00 -07:00'),
                    (8, '2026-10-02 14:00:00 +00:00', '2026-10-02 14:00:00 +00:00')
                ").GetAwaiter().GetResult();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            foreach (var table in new[] { TestTable, DateBoundaryTable })
            {
                try
                {
                    DataSeeder?.ExecuteSqlStatement($"DROP TABLE IF EXISTS {table}").GetAwaiter().GetResult();
                }
                catch (Exception)
                {
                    // Ignore cleanup errors to prevent masking test failures
                }
            }
        }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            EnsureApplicationIsRunning();

            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetTestToken()}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [DataTestMethod]
        [DataRow("ACCOUNTID eq " + Guid1, new[] { 1 })]
        [DataRow("ACCOUNTID ne " + Guid1, new[] { 2, 3 })]
        [DataRow("ACCOUNTID eq " + Guid3, new[] { 3 })]
        [DataRow("ACCOUNTID eq DB4735E6-77F2-EF11-BE20-000D3AE154D9", new int[0])]
        [DataRow("ACCOUNTID eq " + Guid1 + " or ACCOUNTID eq " + Guid2, new[] { 1, 2 })]
        [DataRow("ACCOUNTID eq " + Guid2 + " and BIRTH_DATE eq 2026-10-01", new[] { 2 })]
        [DataRow("ACCOUNTID eq '" + Guid1 + "'", new[] { 1 })]
        public async Task ListItems_FilterByGuid_ReturnsMatchingRows(string filter, int[] expectedIds)
        {
            await AssertFilterReturns(filter, expectedIds);
        }

        [DataTestMethod]
        [DataRow("BIRTH_DATE eq 2026-09-15", new[] { 1 })]
        [DataRow("BIRTH_DATE ge 2026-10-01", new[] { 2, 3 })]
        [DataRow("BIRTH_DATE gt 2026-09-15 and BIRTH_DATE lt 2026-11-05", new[] { 2 })]
        public async Task ListItems_FilterByDate_ReturnsMatchingRows(string filter, int[] expectedIds)
        {
            await AssertFilterReturns(filter, expectedIds);
        }

        [DataTestMethod]
        [DataRow("START_TIME eq 12:30:00", new[] { 1 })]
        [DataRow("START_TIME eq 08:00:00.25", new[] { 2 })]
        [DataRow("START_TIME lt 12:00:00", new[] { 2 })]
        public async Task ListItems_FilterByTimeOfDay_ReturnsMatchingRows(string filter, int[] expectedIds)
        {
            await AssertFilterReturns(filter, expectedIds);
        }

        [DataTestMethod]
        [DataRow("CREATED_NTZ eq 2026-09-15T10:20:30Z", new[] { 1 })]
        [DataRow("CREATED_NTZ ge 2026-10-01T00:00:00Z", new[] { 2, 3 })]
        [DataRow("CREATED_NTZ eq 2026-11-05T17:10:00.5Z", new[] { 3 })]
        [DataRow("CREATED_LTZ eq 2026-09-15T12:20:30+02:00", new[] { 1 })]
        [DataRow("CREATED_TZ eq 2026-09-15T08:20:30Z", new[] { 1 })]
        [DataRow("CREATED_TZ gt 2026-10-01T00:00:00Z", new[] { 3 })]
        public async Task ListItems_FilterByDateTimeOffset_ReturnsMatchingRows(string filter, int[] expectedIds)
        {
            await AssertFilterReturns(filter, expectedIds);
        }

        /// <summary>
        /// A date-only literal compared to a timestamp with time zone must mean midnight in the Snowflake session's
        /// time zone, exactly as the same comparison written in SQL does, and not midnight in the time zone of the
        /// server running the connector. The expected rows are therefore taken from Snowflake itself.
        /// </summary>
        [DataTestMethod]
        [DataRow("CREATED_LTZ ge 2026-10-01", "CREATED_LTZ >= '2026-10-01'")]
        [DataRow("CREATED_LTZ lt 2026-10-01", "CREATED_LTZ < '2026-10-01'")]
        [DataRow("CREATED_TZ ge 2026-10-01", "CREATED_TZ >= '2026-10-01'")]
        [DataRow("CREATED_TZ lt 2026-10-01", "CREATED_TZ < '2026-10-01'")]
        public async Task ListItems_FilterTimestampWithTimeZoneByDate_MatchesSnowflakeDateSemantics(string filter, string sqlCondition)
        {
            var expectedIds = await QueryIds($"SELECT ID FROM {DateBoundaryTable} WHERE {sqlCondition} ORDER BY ID");
            Assert.IsTrue(expectedIds.Length > 0 && expectedIds.Length < 8, $"Seed data must have rows on both sides of the session's midnight, got [{string.Join(", ", expectedIds)}]");

            await AssertFilterReturns(filter, expectedIds, DateBoundaryTable);
        }

        private static async Task<int[]> QueryIds(string sql)
        {
            var rows = (JArray)JObject.Parse(await DataSeeder.ExecuteSqlStatement(sql))["Data"];
            return rows.Select(row => row.Value<int>("ID")).ToArray();
        }

        private async Task AssertFilterReturns(string filter, int[] expectedIds, string table = TestTable)
        {
            var response = await HttpClient.GetAsync(
                $"{BaseUrl}/datasets('{TestDataset}')/tables('{table}')/items?$filter={Uri.EscapeDataString(filter)}");
            var content = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"$filter={filter} failed: {content}");

            var ids = ((JArray)JObject.Parse(content)["value"])
                .Select(item => item.Value<int>("ID"))
                .OrderBy(id => id)
                .ToArray();
            CollectionAssert.AreEqual(expectedIds, ids, $"$filter={filter} returned IDs [{string.Join(", ", ids)}]");
        }
    }
}
