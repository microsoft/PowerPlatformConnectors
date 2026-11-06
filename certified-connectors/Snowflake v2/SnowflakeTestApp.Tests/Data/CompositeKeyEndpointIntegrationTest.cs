using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace SnowflakeTestApp.Tests.Data
{
    /// <summary>
    /// Integration tests for single item operations on tables with a composite primary key.
    /// Such items are addressed by their key values joined with commas, in the order of the key columns.
    /// Every test creates its own tables, which are dropped afterwards.
    /// </summary>
    [TestClass]
    public class CompositeKeyEndpointIntegrationTest : ScratchTableIntegrationTest
    {
        private const string OrderLinesTable = "CK_ORDER_LINES";
        private const string KeyMismatchError = "does not match the primary key of table";

        [TestMethod]
        public async Task GetItem_CompositePrimaryKey_ReturnsMatchingRow()
        {
            await CreateOrderLinesTable();

            var response = await HttpClient.GetAsync(ItemUrl(OrderLinesTable, "1,2"));

            var content = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, content);
            var item = JObject.Parse(content);
            Assert.AreEqual(1, (int)item["ORDER_ID"]);
            Assert.AreEqual(2, (int)item["LINE_NO"]);
            Assert.AreEqual("b", (string)item["NOTE"]);
        }

        [TestMethod]
        public async Task PatchItem_CompositePrimaryKey_UpdatesOnlyMatchingRow()
        {
            await CreateOrderLinesTable();

            var response = await Patch(ItemUrl(OrderLinesTable, "1,2"), new { NOTE = "updated" });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, await response.Content.ReadAsStringAsync());
            await AssertOrderLines(("a", "updated", "c", "d"));
        }

        [TestMethod]
        public async Task PutItem_CompositePrimaryKey_UpdatesOnlyMatchingRow()
        {
            await CreateOrderLinesTable();

            var response = await HttpClient.PutAsync(
                ItemUrl(OrderLinesTable, "2,1"),
                CreateJsonContent(new { ORDER_ID = 2, LINE_NO = 1, NOTE = "updated" }));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, await response.Content.ReadAsStringAsync());
            await AssertOrderLines(("a", "b", "updated", "d"));
        }

        [TestMethod]
        public async Task DeleteItem_CompositePrimaryKey_DeletesOnlyMatchingRow()
        {
            await CreateOrderLinesTable();

            var response = await HttpClient.DeleteAsync(ItemUrl(OrderLinesTable, "1,2"));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, await response.Content.ReadAsStringAsync());
            await AssertOrderLines(("a", null, "c", "d"));
        }

        [TestMethod]
        public async Task DeleteItem_ThreeColumnPrimaryKey_DeletesOnlyMatchingRow()
        {
            await CreateTable("CK_THREE_KEYS",
                "TENANT VARCHAR NOT NULL, ORDER_ID NUMBER(38,0) NOT NULL, LINE_NO NUMBER(38,0) NOT NULL, NOTE VARCHAR, PRIMARY KEY (TENANT, ORDER_ID, LINE_NO)");
            await Sql("INSERT INTO CK_THREE_KEYS VALUES ('acme', 1, 1, 'a'), ('acme', 1, 2, 'b'), ('other', 1, 1, 'c')");

            var response = await HttpClient.DeleteAsync(ItemUrl("CK_THREE_KEYS", "acme,1,1"));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, await response.Content.ReadAsStringAsync());
            Assert.AreEqual(2, await Count("CK_THREE_KEYS"));
            Assert.AreEqual(0, await Count("CK_THREE_KEYS", "TENANT = 'acme' AND ORDER_ID = 1 AND LINE_NO = 1"));
        }

        [TestMethod]
        public async Task DeleteItem_KeyDeclaredInDifferentOrderThanColumns_UsesKeyOrder()
        {
            await CreateTable("CK_REORDERED",
                "LINE_NO NUMBER(38,0) NOT NULL, NOTE VARCHAR, ORDER_ID NUMBER(38,0) NOT NULL, PRIMARY KEY (ORDER_ID, LINE_NO)");
            await Sql("INSERT INTO CK_REORDERED (LINE_NO, NOTE, ORDER_ID) VALUES (2, 'target', 1), (1, 'swapped', 2)");

            // The id lists ORDER_ID first, because it comes first in the primary key
            var response = await HttpClient.DeleteAsync(ItemUrl("CK_REORDERED", "1,2"));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, await response.Content.ReadAsStringAsync());
            Assert.AreEqual(1, await Count("CK_REORDERED"));
            Assert.AreEqual("swapped", await Note("CK_REORDERED", "ORDER_ID = 2 AND LINE_NO = 1"));
        }

        [DataRow("1")]
        [DataRow("1,2,3")]
        [DataTestMethod]
        public async Task GetItem_CompositePrimaryKeyWithWrongNumberOfValues_ReturnsBadRequest(string id)
        {
            await CreateOrderLinesTable();

            var response = await HttpClient.GetAsync(ItemUrl(OrderLinesTable, id));

            await AssertKeyMismatch(response);
        }

        [DataRow("1")]
        [DataRow("1,2,3")]
        [DataTestMethod]
        public async Task PatchItem_CompositePrimaryKeyWithWrongNumberOfValues_ReturnsBadRequestAndChangesNothing(string id)
        {
            await CreateOrderLinesTable();

            var response = await Patch(ItemUrl(OrderLinesTable, id), new { NOTE = "updated" });

            await AssertKeyMismatch(response);
            await AssertOrderLines(("a", "b", "c", "d"));
        }

        [DataRow("1")]
        [DataRow("1,2,3")]
        [DataTestMethod]
        public async Task DeleteItem_CompositePrimaryKeyWithWrongNumberOfValues_ReturnsBadRequestAndDeletesNothing(string id)
        {
            await CreateOrderLinesTable();

            var response = await HttpClient.DeleteAsync(ItemUrl(OrderLinesTable, id));

            await AssertKeyMismatch(response);
            await AssertOrderLines(("a", "b", "c", "d"));
        }

        [TestMethod]
        public async Task CreateItem_CompositePrimaryKey_InsertsRow()
        {
            await CreateOrderLinesTable();

            var response = await Post(OrderLinesTable, new { ORDER_ID = 3, LINE_NO = 1, NOTE = "new" });

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, await response.Content.ReadAsStringAsync());
            Assert.AreEqual(5, await Count(OrderLinesTable));
            Assert.AreEqual("new", await Note(OrderLinesTable, "ORDER_ID = 3 AND LINE_NO = 1"));
        }

        [TestMethod]
        public async Task CompositePrimaryKey_ListItems_ReturnsAllRows()
        {
            await CreateOrderLinesTable();

            var response = await HttpClient.GetAsync(ItemsUrl(OrderLinesTable));

            var content = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, content);
            Assert.AreEqual(4, ((JArray)JObject.Parse(content)["value"]).Count);
        }

        [TestMethod]
        public async Task CompositePrimaryKey_Metadata_MarksEveryKeyColumnInKeyOrder()
        {
            await CreateTable("CK_REORDERED_META",
                "LINE_NO NUMBER(38,0) NOT NULL, NOTE VARCHAR, ORDER_ID NUMBER(38,0) NOT NULL, PRIMARY KEY (ORDER_ID, LINE_NO)");

            var properties = await GetTableProperties("CK_REORDERED_META");

            AssertKeyColumn(properties, "ORDER_ID", "primary", 1);
            AssertKeyColumn(properties, "LINE_NO", "primary", 2);
            AssertKeyColumn(properties, "NOTE", "none", null);
        }

        [TestMethod]
        public async Task SingleColumnPrimaryKey_Metadata_MarksKeyColumn()
        {
            await CreateTable("CK_SINGLE_KEY_META", "ID NUMBER(38,0) NOT NULL, NOTE VARCHAR, PRIMARY KEY (ID)");

            var properties = await GetTableProperties("CK_SINGLE_KEY_META");

            AssertKeyColumn(properties, "ID", "primary", 1);
            AssertKeyColumn(properties, "NOTE", "none", null);
        }

        [TestMethod]
        public async Task SingleColumnPrimaryKey_GetPatchDelete_StillWork()
        {
            await CreateTable("CK_SINGLE_KEY", "ID NUMBER(38,0) NOT NULL, NOTE VARCHAR, PRIMARY KEY (ID)");
            await Sql("INSERT INTO CK_SINGLE_KEY VALUES (1, 'a'), (2, 'b')");

            var get = await HttpClient.GetAsync(ItemUrl("CK_SINGLE_KEY", "1"));
            Assert.AreEqual(HttpStatusCode.OK, get.StatusCode, await get.Content.ReadAsStringAsync());
            Assert.AreEqual("a", (string)JObject.Parse(await get.Content.ReadAsStringAsync())["NOTE"]);

            var patch = await Patch(ItemUrl("CK_SINGLE_KEY", "1"), new { NOTE = "patched" });
            Assert.AreEqual(HttpStatusCode.OK, patch.StatusCode, await patch.Content.ReadAsStringAsync());
            Assert.AreEqual("patched", await Note("CK_SINGLE_KEY", "ID = 1"));
            Assert.AreEqual("b", await Note("CK_SINGLE_KEY", "ID = 2"));

            var delete = await HttpClient.DeleteAsync(ItemUrl("CK_SINGLE_KEY", "1"));
            Assert.AreEqual(HttpStatusCode.OK, delete.StatusCode, await delete.Content.ReadAsStringAsync());
            Assert.AreEqual(1, await Count("CK_SINGLE_KEY"));
            Assert.AreEqual(0, await Count("CK_SINGLE_KEY", "ID = 1"));
        }

        [TestMethod]
        public async Task SingleColumnPrimaryKey_ValueContainingComma_StillWorks()
        {
            await CreateTable("CK_COMMA_SINGLE", "CODE VARCHAR NOT NULL, NOTE VARCHAR, PRIMARY KEY (CODE)");
            await Sql("INSERT INTO CK_COMMA_SINGLE VALUES ('a,b', 'comma'), ('a', 'plain'), ('b', 'other')");

            var get = await HttpClient.GetAsync(ItemUrl("CK_COMMA_SINGLE", "a,b"));
            Assert.AreEqual(HttpStatusCode.OK, get.StatusCode, await get.Content.ReadAsStringAsync());
            Assert.AreEqual("comma", (string)JObject.Parse(await get.Content.ReadAsStringAsync())["NOTE"]);

            var delete = await HttpClient.DeleteAsync(ItemUrl("CK_COMMA_SINGLE", "a,b"));
            Assert.AreEqual(HttpStatusCode.OK, delete.StatusCode, await delete.Content.ReadAsStringAsync());
            Assert.AreEqual(2, await Count("CK_COMMA_SINGLE"));
            Assert.AreEqual(0, await Count("CK_COMMA_SINGLE", "CODE = 'a,b'"));
        }

        [TestMethod]
        public async Task TableWithoutPrimaryKey_Delete_StillFailsAndDeletesNothing()
        {
            await CreateTable("CK_NO_PK", "ID NUMBER(38,0), NOTE VARCHAR");
            await Sql("INSERT INTO CK_NO_PK VALUES (1, 'a'), (1, 'b'), (2, 'c')");

            var response = await HttpClient.DeleteAsync(ItemUrl("CK_NO_PK", "1"));

            Assert.IsFalse(response.IsSuccessStatusCode, "DELETE without a primary key must fail");
            Assert.AreEqual(3, await Count("CK_NO_PK"));
        }

        private async Task CreateOrderLinesTable()
        {
            await CreateTable(OrderLinesTable,
                "ORDER_ID NUMBER(38,0) NOT NULL, LINE_NO NUMBER(38,0) NOT NULL, NOTE VARCHAR, PRIMARY KEY (ORDER_ID, LINE_NO)");
            await Sql($"INSERT INTO {OrderLinesTable} VALUES (1, 1, 'a'), (1, 2, 'b'), (2, 1, 'c'), (2, 2, 'd')");
        }

        /// <summary>
        /// Asserts the notes of rows (1,1), (1,2), (2,1) and (2,2), where null means the row must not exist.
        /// </summary>
        private async Task AssertOrderLines((string Row11, string Row12, string Row21, string Row22) expected)
        {
            var rows = new[]
            {
                ("ORDER_ID = 1 AND LINE_NO = 1", expected.Row11),
                ("ORDER_ID = 1 AND LINE_NO = 2", expected.Row12),
                ("ORDER_ID = 2 AND LINE_NO = 1", expected.Row21),
                ("ORDER_ID = 2 AND LINE_NO = 2", expected.Row22),
            };

            Assert.AreEqual(rows.Count(r => r.Item2 != null), await Count(OrderLinesTable), "Row count");
            foreach (var (where, note) in rows)
            {
                Assert.AreEqual(note == null ? 0 : 1, await Count(OrderLinesTable, where), $"Rows where {where}");
                if (note != null)
                {
                    Assert.AreEqual(note, await Note(OrderLinesTable, where), $"Note where {where}");
                }
            }
        }

        private static async Task AssertKeyMismatch(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, content);
            StringAssert.Contains(content, KeyMismatchError);
        }

        private Task<string> Note(string table, string where)
        {
            return Scalar($"SELECT NOTE AS RESULT FROM {table} WHERE {where}");
        }
    }
}
