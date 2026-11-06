using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace SnowflakeTestApp.Tests.Data
{
    /// <summary>
    /// Integration tests for tables with identity (AUTOINCREMENT) columns, whose values Snowflake generates when they are omitted.
    /// A primary key made of such a column is reported as read-only and not required, so clients create rows without it.
    /// Every test creates its own tables, which are dropped afterwards.
    /// </summary>
    [TestClass]
    public class IdentityColumnEndpointIntegrationTest : ScratchTableIntegrationTest
    {
        private const string IdentityKeyTable = "IDC_IDENTITY_KEY";

        [DataRow("NUMBER AUTOINCREMENT")]
        [DataRow("NUMBER IDENTITY")]
        [DataRow("NUMBER IDENTITY(100, 10)")]
        [DataRow("NUMBER AUTOINCREMENT START 1 INCREMENT 1 ORDER")]
        [DataRow("NUMBER AUTOINCREMENT START 1 INCREMENT 1 NOORDER")]
        [DataTestMethod]
        public async Task Metadata_IdentityPrimaryKey_IsReadOnlyAndNotRequired(string identityType)
        {
            await CreateTable(IdentityKeyTable, $"ID {identityType} PRIMARY KEY, NAME VARCHAR NOT NULL, NOTE VARCHAR");

            var rowSchema = await GetRowSchema(IdentityKeyTable);

            var properties = (JObject)rowSchema["properties"];
            AssertKeyColumn(properties, "ID", "primary", 1);
            AssertKeyColumn(properties, "NAME", "none", null);
            AssertKeyColumn(properties, "NOTE", "none", null);
            AssertWritability(rowSchema, "ID", "read-only", required: false);
            AssertWritability(rowSchema, "NAME", "read-write", required: true);
            AssertWritability(rowSchema, "NOTE", "read-write", required: false);
        }

        [TestMethod]
        public async Task Metadata_UserProvidedPrimaryKey_IsWritableAndRequired()
        {
            await CreateTable("IDC_USER_KEY", "ID NUMBER(38,0) NOT NULL PRIMARY KEY, NOTE VARCHAR");

            var rowSchema = await GetRowSchema("IDC_USER_KEY");

            AssertKeyColumn((JObject)rowSchema["properties"], "ID", "primary", 1);
            AssertWritability(rowSchema, "ID", "read-write", required: true);
            AssertWritability(rowSchema, "NOTE", "read-write", required: false);
        }

        [TestMethod]
        public async Task Metadata_IdentityColumnNotInPrimaryKey_IsWritableAndNotRequired()
        {
            await CreateTable("IDC_IDENTITY_NON_KEY", "CODE VARCHAR NOT NULL PRIMARY KEY, SEQ NUMBER AUTOINCREMENT, NOTE VARCHAR");

            var rowSchema = await GetRowSchema("IDC_IDENTITY_NON_KEY");

            AssertKeyColumn((JObject)rowSchema["properties"], "SEQ", "none", null);
            AssertWritability(rowSchema, "CODE", "read-write", required: true);
            AssertWritability(rowSchema, "SEQ", "read-write", required: false);
        }

        [TestMethod]
        public async Task Metadata_CompositeKeyWithIdentityColumn_OnlyIdentityColumnIsServerGenerated()
        {
            await CreateTable("IDC_COMPOSITE", "TENANT VARCHAR NOT NULL, ID NUMBER AUTOINCREMENT, NOTE VARCHAR, PRIMARY KEY (TENANT, ID)");

            var rowSchema = await GetRowSchema("IDC_COMPOSITE");

            var properties = (JObject)rowSchema["properties"];
            AssertKeyColumn(properties, "TENANT", "primary", 1);
            AssertKeyColumn(properties, "ID", "primary", 2);
            AssertWritability(rowSchema, "TENANT", "read-write", required: true);
            AssertWritability(rowSchema, "ID", "read-only", required: false);
        }

        [TestMethod]
        public async Task CreateItem_IdentityPrimaryKeyOmitted_GeneratesDistinctKeys()
        {
            await CreateIdentityKeyTable();

            var first = await Post(IdentityKeyTable, new { NAME = "first" });
            var second = await Post(IdentityKeyTable, new { NAME = "second", NOTE = "note" });

            Assert.AreEqual(HttpStatusCode.Created, first.StatusCode, await first.Content.ReadAsStringAsync());
            Assert.AreEqual(HttpStatusCode.Created, second.StatusCode, await second.Content.ReadAsStringAsync());
            Assert.AreEqual(2, await Count(IdentityKeyTable));
            Assert.AreEqual(0, await Count(IdentityKeyTable, "ID IS NULL"));
            Assert.AreEqual("2", await Scalar($"SELECT COUNT(DISTINCT ID) AS RESULT FROM {IdentityKeyTable}"));
            Assert.AreEqual("note", await Scalar($"SELECT NOTE AS RESULT FROM {IdentityKeyTable} WHERE NAME = 'second'"));
        }

        [TestMethod]
        public async Task CreatedItemWithGeneratedKey_CanBeReadUpdatedAndDeleted()
        {
            await CreateIdentityKeyTable();
            await Sql($"INSERT INTO {IdentityKeyTable} (NAME) VALUES ('other')");

            var create = await Post(IdentityKeyTable, new { NAME = "created" });
            Assert.AreEqual(HttpStatusCode.Created, create.StatusCode, await create.Content.ReadAsStringAsync());
            var id = await Scalar($"SELECT ID AS RESULT FROM {IdentityKeyTable} WHERE NAME = 'created'");
            Assert.IsNotNull(id, "The created row should have a generated key");

            var get = await HttpClient.GetAsync(ItemUrl(IdentityKeyTable, id));
            Assert.AreEqual(HttpStatusCode.OK, get.StatusCode, await get.Content.ReadAsStringAsync());
            Assert.AreEqual("created", (string)JObject.Parse(await get.Content.ReadAsStringAsync())["NAME"]);

            var patch = await Patch(ItemUrl(IdentityKeyTable, id), new { NOTE = "patched" });
            Assert.AreEqual(HttpStatusCode.OK, patch.StatusCode, await patch.Content.ReadAsStringAsync());
            Assert.AreEqual("patched", await Scalar($"SELECT NOTE AS RESULT FROM {IdentityKeyTable} WHERE ID = {id}"));

            var delete = await HttpClient.DeleteAsync(ItemUrl(IdentityKeyTable, id));
            Assert.AreEqual(HttpStatusCode.OK, delete.StatusCode, await delete.Content.ReadAsStringAsync());
            Assert.AreEqual(1, await Count(IdentityKeyTable));
            Assert.AreEqual(1, await Count(IdentityKeyTable, "NAME = 'other'"));
        }

        [TestMethod]
        public async Task CreateItem_IdentityColumnNotInPrimaryKeyOmitted_GeneratesValue()
        {
            await CreateTable("IDC_IDENTITY_NON_KEY", "CODE VARCHAR NOT NULL PRIMARY KEY, SEQ NUMBER AUTOINCREMENT, NOTE VARCHAR");

            var response = await Post("IDC_IDENTITY_NON_KEY", new { CODE = "a" });

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, await response.Content.ReadAsStringAsync());
            Assert.AreEqual(1, await Count("IDC_IDENTITY_NON_KEY", "CODE = 'a' AND SEQ IS NOT NULL"));
        }

        private Task CreateIdentityKeyTable()
        {
            return CreateTable(IdentityKeyTable, "ID NUMBER AUTOINCREMENT PRIMARY KEY, NAME VARCHAR NOT NULL, NOTE VARCHAR");
        }

        private static void AssertWritability(JObject rowSchema, string column, string permission, bool required)
        {
            var property = rowSchema["properties"][column];
            Assert.IsNotNull(property, $"Missing column {column}");
            Assert.AreEqual(permission, (string)property["x-ms-permission"], $"{column} permission");

            var requiredColumns = (JArray)rowSchema["required"];
            Assert.AreEqual(required, requiredColumns.Any(c => (string)c == column), $"{column} in schema.items.required");
        }
    }
}
