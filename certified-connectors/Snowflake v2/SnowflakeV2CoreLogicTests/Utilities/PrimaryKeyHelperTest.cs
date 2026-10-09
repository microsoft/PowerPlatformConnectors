namespace SnowflakeV2CoreLogic.Tests.Utilities
{
    using System.Net;
    using System.Web.Http;
    using SnowflakeV2CoreLogic.Tests.TestDoubles;
    using SnowflakeV2CoreLogic.Utilities;

    [TestClass]
    public sealed class PrimaryKeyHelperTest
    {
        [TestMethod]
        public void GetPrimaryKeyColumns_SingleColumn_ReturnsColumn()
        {
            var columns = PrimaryKeyHelper.GetPrimaryKeyColumns(SnowflakeResponses.PrimaryKeys(("ID", 1)));

            CollectionAssert.AreEqual(new[] { "ID" }, columns.ToArray());
        }

        [TestMethod]
        public void GetPrimaryKeyColumns_RowsNotInKeyOrder_OrdersByKeySequence()
        {
            var columns = PrimaryKeyHelper.GetPrimaryKeyColumns(SnowflakeResponses.PrimaryKeys(("C", 3), ("A", 1), ("B", 2)));

            CollectionAssert.AreEqual(new[] { "A", "B", "C" }, columns.ToArray());
        }

        [TestMethod]
        public void GetPrimaryKeyColumns_NoPrimaryKey_ReturnsEmpty()
        {
            Assert.AreEqual(0, PrimaryKeyHelper.GetPrimaryKeyColumns(SnowflakeResponses.NoPrimaryKey()).Count);
            Assert.AreEqual(0, PrimaryKeyHelper.GetPrimaryKeyColumns(null).Count);
        }

        [TestMethod]
        public void GetPrimaryKeyColumns_MissingKeySequence_ReturnsEmpty()
        {
            var response = SnowflakeResponses.Table(new[] { ("column_name", "text", (int?)null) }, new string?[] { "ID" });

            Assert.AreEqual(0, PrimaryKeyHelper.GetPrimaryKeyColumns(response).Count);
        }

        [DataRow("1")]
        [DataRow("a,b")]
        [DataRow(",")]
        [DataTestMethod]
        public void ParseItemId_SingleColumnKey_UsesWholeId(string id)
        {
            var key = PrimaryKeyHelper.ParseItemId(id, new[] { "ID" }, "T");

            Assert.AreEqual(1, key.Count);
            Assert.AreEqual(("ID", id), key[0]);
        }

        [TestMethod]
        public void ParseItemId_CompositeKey_PairsValuesWithColumnsInOrder()
        {
            var key = PrimaryKeyHelper.ParseItemId("acme,1,2", new[] { "TENANT", "ORDER_ID", "LINE_NO" }, "T");

            CollectionAssert.AreEqual(new[] { ("TENANT", "acme"), ("ORDER_ID", "1"), ("LINE_NO", "2") }, key.ToArray());
        }

        [TestMethod]
        public void ParseItemId_CompositeKey_KeepsEmptyAndWhitespaceValues()
        {
            var key = PrimaryKeyHelper.ParseItemId(", b ", new[] { "A", "B" }, "T");

            CollectionAssert.AreEqual(new[] { ("A", string.Empty), ("B", " b ") }, key.ToArray());
        }

        [DataRow("1", 1)]
        [DataRow("1,2,3", 3)]
        [DataRow("1,2,", 3)]
        [DataTestMethod]
        public async Task ParseItemId_CompositeKeyWithWrongNumberOfValues_ReturnsBadRequest(string id, int valueCount)
        {
            var ex = Assert.ThrowsException<HttpResponseException>(() => PrimaryKeyHelper.ParseItemId(id, new[] { "ORDER_ID", "LINE_NO" }, "T"));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.Response.StatusCode);
            var message = await ex.Response.Content.ReadAsStringAsync();
            StringAssert.Contains(message, "primary key of table 'T'");
            StringAssert.Contains(message, "2 comma-separated values in this order: ORDER_ID, LINE_NO");
            StringAssert.Contains(message, $"The id contains {valueCount} values");
        }
    }
}
