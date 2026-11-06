namespace SnowflakeV2CoreLogic.Tests.Providers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Extensions.Logging.Abstractions;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;
    using SnowflakeV2CoreLogic.Providers;
    using SnowflakeV2CoreLogic.Tests.TestDoubles;

    [TestClass]
    public sealed class SnowflakeTableDataProviderTest
    {
        private const string Dataset = "default";
        private const string Table = "T";

        [TestMethod]
        public async Task GetItem_CompositePrimaryKey_FiltersOnEveryKeyColumn()
        {
            var (provider, client) = Create(Respond(OrderLinesKey(), select: SnowflakeResponses.Rows(1)));

            await provider.GetItemAsync(Request(), Dataset, Table, "1,2");

            var select = client.Statements.Single(s => s.Statement.StartsWith("SELECT"));
            Assert.AreEqual("SELECT * FROM T WHERE ORDER_ID = ? AND LINE_NO = ?", select.Statement);
            CollectionAssert.AreEqual(new[] { "1", "2" }, select.BindingValues.ToArray());
        }

        [TestMethod]
        public async Task PatchItem_CompositePrimaryKey_BindsKeyValuesAfterUpdatedValues()
        {
            var (provider, client) = Create(Respond(OrderLinesKey(), dml: SnowflakeResponses.RowsUpdated(1)));

            await provider.PatchItemAsync(Request(), Dataset, Table, "1,2", NewItem(("NOTE", "n"), ("QTY", 5)));

            var update = client.Statements.Single(s => s.Statement.StartsWith("UPDATE"));
            Assert.AreEqual("UPDATE T SET NOTE = ?, QTY = ? WHERE ORDER_ID = ? AND LINE_NO = ?", update.Statement);
            CollectionAssert.AreEqual(new[] { "n", "5", "1", "2" }, update.BindingValues.ToArray());
        }

        [TestMethod]
        public async Task DeleteItem_CompositePrimaryKey_FiltersOnEveryKeyColumn()
        {
            var (provider, client) = Create(Respond(OrderLinesKey(), dml: SnowflakeResponses.RowsDeleted(1)));

            await provider.DeleteItemAsync(Request(), Dataset, Table, "1,2");

            var delete = client.Statements.Single(s => s.Statement.StartsWith("DELETE"));
            Assert.AreEqual("DELETE FROM T WHERE ORDER_ID = ? AND LINE_NO = ?", delete.Statement);
            CollectionAssert.AreEqual(new[] { "1", "2" }, delete.BindingValues.ToArray());
        }

        [TestMethod]
        public async Task DeleteItem_KeyRowsNotInKeyOrder_UsesKeySequence()
        {
            var keys = SnowflakeResponses.PrimaryKeys(("LINE_NO", 3), ("TENANT", 1), ("ORDER_ID", 2));
            var (provider, client) = Create(Respond(keys, dml: SnowflakeResponses.RowsDeleted(1)));

            await provider.DeleteItemAsync(Request(), Dataset, Table, "acme,1,2");

            var delete = client.Statements.Single(s => s.Statement.StartsWith("DELETE"));
            Assert.AreEqual("DELETE FROM T WHERE TENANT = ? AND ORDER_ID = ? AND LINE_NO = ?", delete.Statement);
            CollectionAssert.AreEqual(new[] { "acme", "1", "2" }, delete.BindingValues.ToArray());
        }

        [DataRow("1")]
        [DataRow("1,2,3")]
        [DataTestMethod]
        public async Task GetItem_CompositePrimaryKeyWithWrongNumberOfValues_ReturnsBadRequestWithoutQueryingTable(string id)
        {
            var (provider, client) = Create(Respond(OrderLinesKey()));

            var ex = await Assert.ThrowsExceptionAsync<HttpResponseException>(() => provider.GetItemAsync(Request(), Dataset, Table, id));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.Response.StatusCode);
            AssertOnlyPrimaryKeyLookup(client);
        }

        [DataRow("1")]
        [DataRow("1,2,3")]
        [DataTestMethod]
        public async Task PatchItem_CompositePrimaryKeyWithWrongNumberOfValues_ReturnsBadRequestWithoutUpdating(string id)
        {
            var (provider, client) = Create(Respond(OrderLinesKey()));

            var ex = await Assert.ThrowsExceptionAsync<HttpResponseException>(
                () => provider.PatchItemAsync(Request(), Dataset, Table, id, NewItem(("NOTE", "n"))));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.Response.StatusCode);
            AssertOnlyPrimaryKeyLookup(client);
        }

        [DataRow("1")]
        [DataRow("1,2,3")]
        [DataTestMethod]
        public async Task DeleteItem_CompositePrimaryKeyWithWrongNumberOfValues_ReturnsBadRequestWithoutDeleting(string id)
        {
            var (provider, client) = Create(Respond(OrderLinesKey()));

            var ex = await Assert.ThrowsExceptionAsync<HttpResponseException>(() => provider.DeleteItemAsync(Request(), Dataset, Table, id));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.Response.StatusCode);
            AssertOnlyPrimaryKeyLookup(client);
        }

        [TestMethod]
        public async Task GetItem_SingleColumnPrimaryKey_DoesNotSplitIdOnCommas()
        {
            var (provider, client) = Create(Respond(SnowflakeResponses.PrimaryKeys(("ID", 1)), select: SnowflakeResponses.Rows(1)));

            var item = await provider.GetItemAsync(Request(), Dataset, Table, "a,b");

            var select = client.Statements.Single(s => s.Statement.StartsWith("SELECT"));
            Assert.AreEqual("SELECT * FROM T WHERE ID = ?", select.Statement);
            CollectionAssert.AreEqual(new[] { "a,b" }, select.BindingValues.ToArray());
            Assert.AreEqual(1, item.DynamicProperties["ID"]);
        }

        [TestMethod]
        public async Task PatchItem_SingleColumnPrimaryKey_FiltersOnKey()
        {
            var (provider, client) = Create(Respond(SnowflakeResponses.PrimaryKeys(("ID", 1)), dml: SnowflakeResponses.RowsUpdated(1)));

            await provider.PatchItemAsync(Request(), Dataset, Table, "5", NewItem(("NAME", "n"), ("EMAIL", "e")));

            var update = client.Statements.Single(s => s.Statement.StartsWith("UPDATE"));
            Assert.AreEqual("UPDATE T SET NAME = ?, EMAIL = ? WHERE ID = ?", update.Statement);
            CollectionAssert.AreEqual(new[] { "n", "e", "5" }, update.BindingValues.ToArray());
        }

        [TestMethod]
        public async Task DeleteItem_SingleColumnPrimaryKey_FiltersOnKey()
        {
            var (provider, client) = Create(Respond(SnowflakeResponses.PrimaryKeys(("ID", 1)), dml: SnowflakeResponses.RowsDeleted(1)));

            await provider.DeleteItemAsync(Request(), Dataset, Table, "7");

            var delete = client.Statements.Single(s => s.Statement.StartsWith("DELETE"));
            Assert.AreEqual("DELETE FROM T WHERE ID = ?", delete.Statement);
            CollectionAssert.AreEqual(new[] { "7" }, delete.BindingValues.ToArray());
        }

        [TestMethod]
        public async Task DeleteItem_TableWithoutPrimaryKey_ThrowsWithoutDeleting()
        {
            var (provider, client) = Create(Respond(SnowflakeResponses.NoPrimaryKey()));

            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => provider.DeleteItemAsync(Request(), Dataset, Table, "1"));

            StringAssert.Contains(ex.Message, "Unable to determine primary key");
            AssertOnlyPrimaryKeyLookup(client);
        }

        [TestMethod]
        public async Task GetItem_CompositePrimaryKeyMatchingSeveralRows_Throws()
        {
            var (provider, _) = Create(Respond(OrderLinesKey(), select: SnowflakeResponses.Rows(2)));

            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => provider.GetItemAsync(Request(), Dataset, Table, "1,2"));

            StringAssert.Contains(ex.Message, "Multiple items returned when querying by primary key ORDER_ID, LINE_NO");
        }

        private static SnowflakeTableData OrderLinesKey() => SnowflakeResponses.PrimaryKeys(("ORDER_ID", 1), ("LINE_NO", 2));

        private static (SnowflakeTableDataProvider Provider, SnowflakeClientSpy Client) Create(Func<RecordedStatement, SnowflakeTableData> responder)
        {
            var client = new SnowflakeClientSpy(responder);
            var operations = new SnowflakeDBOperations(client, new HttpClient(), NullLogger.Instance);
            var connectionParametersProvider = new SnowflakeConnectionParametersProvider(new EmptyConnectionParametersProvider(), NullLogger.Instance);
            return (new SnowflakeTableDataProvider(operations, connectionParametersProvider, NullLogger.Instance), client);
        }

        private static Func<RecordedStatement, SnowflakeTableData> Respond(
            SnowflakeTableData primaryKeys,
            SnowflakeTableData? select = null,
            SnowflakeTableData? dml = null)
        {
            return statement =>
            {
                if (statement.Statement.StartsWith("SHOW PRIMARY KEYS"))
                {
                    return primaryKeys;
                }

                if (statement.Statement.StartsWith("SELECT"))
                {
                    return select ?? throw new AssertFailedException($"Unexpected statement: {statement.Statement}");
                }

                if (statement.Statement.StartsWith("UPDATE") || statement.Statement.StartsWith("DELETE"))
                {
                    return dml ?? throw new AssertFailedException($"Unexpected statement: {statement.Statement}");
                }

                throw new AssertFailedException($"Unexpected statement: {statement.Statement}");
            };
        }

        private static HttpRequestMessage Request() => new HttpRequestMessage(HttpMethod.Get, "https://localhost/datasets/default/tables/T/items");

        private static Item NewItem(params (string Column, object? Value)[] values)
        {
            var item = new Item();
            foreach (var (column, value) in values)
            {
                item.DynamicProperties.Add(column, value!);
            }

            return item;
        }

        private static void AssertOnlyPrimaryKeyLookup(SnowflakeClientSpy client)
        {
            Assert.AreEqual(1, client.Statements.Count, string.Join(Environment.NewLine, client.Statements.Select(s => s.Statement)));
            StringAssert.StartsWith(client.Statements[0].Statement, "SHOW PRIMARY KEYS");
        }
    }
}
