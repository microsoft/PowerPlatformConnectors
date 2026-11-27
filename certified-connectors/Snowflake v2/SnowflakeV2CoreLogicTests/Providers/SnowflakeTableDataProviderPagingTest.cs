namespace SnowflakeV2CoreLogic.Tests.Providers
{
    using System.Collections.Specialized;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.OData;
    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;
    using System.Web.OData.Query;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.OData.Edm;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;
    using SnowflakeV2CoreLogic.Providers;
    using SnowflakeV2CoreLogic.Tests.TestDoubles;

    [TestClass]
    public sealed class SnowflakeTableDataProviderPagingTest
    {
        private const string ItemsUrl = "https://localhost/datasets/default/tables/T/items";
        private const string Handle = "01b2c3d4-0000-0000-0000-000000000001";

        private static readonly IEdmModel Model = BuildModel();

        [TestMethod]
        public async Task ListItems_WithoutTop_ReadsOneRowPastTheDefaultPage()
        {
            var (provider, client) = Create(SnowflakeResponses.Rows(10));

            await ListAsync(provider, string.Empty);

            Assert.AreEqual("SELECT * FROM T  LIMIT 52 OFFSET 0", client.Statements.Single().Statement);
        }

        [TestMethod]
        public async Task ListItems_WithTopAndSkip_ReadsOneRowPastTheRequestedPage()
        {
            var (provider, client) = Create(SnowflakeResponses.Rows(10));

            await ListAsync(provider, "?$top=10&$skip=20");

            Assert.AreEqual("SELECT * FROM T  LIMIT 11 OFFSET 20", client.Statements.Single().Statement);
        }

        [TestMethod]
        public async Task ListItems_WithoutTopAndMoreRowsThanFit_ReturnsDefaultPageAndLinksToNextPage()
        {
            var (provider, _) = Create(SnowflakeResponses.Rows(52));

            var (items, nextLink) = await ListAsync(provider, string.Empty);

            CollectionAssert.AreEqual(Enumerable.Range(1, 51).ToList(), Ids(items));
            Assert.IsNotNull(nextLink);
            Assert.AreEqual(ItemsUrl, nextLink.GetLeftPart(UriPartial.Path));
            Assert.AreEqual("51", Query(nextLink)["$skip"]);
            Assert.IsNull(Query(nextLink)["$skiptoken"]);
            Assert.IsNull(Query(nextLink)["$top"]);
        }

        [TestMethod]
        public async Task ListItems_WithoutTopAndExactlyOnePage_HasNoNextLink()
        {
            var (provider, _) = Create(SnowflakeResponses.Rows(51));

            var (items, nextLink) = await ListAsync(provider, string.Empty);

            Assert.AreEqual(51, items.Count);
            Assert.IsNull(nextLink);
        }

        [TestMethod]
        public async Task ListItems_WithTopAndMoreRowsThanFit_LinksToNextPageOfSameSize()
        {
            var (provider, _) = Create(SnowflakeResponses.Rows(21, 11));

            var (items, nextLink) = await ListAsync(provider, "?$filter=ID gt 0&$orderby=ID desc&$select=ID,NAME&$top=10&$skip=20");

            CollectionAssert.AreEqual(Enumerable.Range(21, 10).ToList(), Ids(items));
            Assert.IsNotNull(nextLink);
            var query = Query(nextLink);
            Assert.AreEqual("ID gt 0", query["$filter"]);
            Assert.AreEqual("ID desc", query["$orderby"]);
            Assert.AreEqual("ID,NAME", query["$select"]);
            Assert.AreEqual("10", query["$top"]);
            Assert.AreEqual("30", query["$skip"]);
            Assert.IsNull(query["$skiptoken"]);
        }

        [TestMethod]
        public async Task ListItems_TopZero_ReturnsNoRowsAndNoNextLink()
        {
            var (provider, client) = Create(SnowflakeResponses.Rows(0));

            var (items, nextLink) = await ListAsync(provider, "?$top=0");

            Assert.AreEqual("SELECT * FROM T  LIMIT 0 OFFSET 0", client.Statements.Single().Statement);
            Assert.AreEqual(0, items.Count);
            Assert.IsNull(nextLink);
        }

        [TestMethod]
        public async Task ListItems_WithCountAndMoreRowsThanFit_ReturnsTotalAndLinksToNextPage()
        {
            var (provider, _) = Create(SnowflakeResponses.Rows(11), count: 120);

            var request = Request("?$top=10&$count=true");
            var items = await provider.ListItemsAsync(request, "default", "T", Options(request));

            Assert.AreEqual(10, items.Count());
            Assert.AreEqual(120, request.ODataProperties().TotalCount);
            Assert.AreEqual("10", Query(request.ODataProperties().NextLink!)["$skip"]);
        }

        [TestMethod]
        public async Task ListItems_FilterWithReservedCharacters_IsEncodedInNextLink()
        {
            var (provider, _) = Create(SnowflakeResponses.Rows(2));

            var (_, nextLink) = await ListAsync(provider, "?$filter=NAME eq 'a%2Bb%26c'&$top=1");

            Assert.IsNotNull(nextLink);
            Assert.AreEqual("NAME eq 'a+b&c'", Query(nextLink)["$filter"]);
            Assert.AreEqual("1", Query(nextLink)["$skip"]);
        }

        [TestMethod]
        public async Task ListItems_PartitionedPage_LinksToNextPartitionKeepingTopAndSkip()
        {
            var (provider, _) = Create(SnowflakeResponses.FirstPartition(Handle, 2, 9));

            var (items, nextLink) = await ListAsync(provider, "?$filter=ID gt 0&$top=10&$skip=20");

            Assert.AreEqual(2, items.Count);
            Assert.IsNotNull(nextLink);
            var query = Query(nextLink);
            Assert.AreEqual($"{Handle}~1~2", query["$skiptoken"]);
            Assert.AreEqual("10", query["$top"]);
            Assert.AreEqual("20", query["$skip"]);
            Assert.AreEqual("ID gt 0", query["$filter"]);
        }

        [TestMethod]
        public async Task ListItems_PageEndingAtPartitionBoundary_LinksToNextPageInsteadOfLookAheadPartition()
        {
            var (provider, _) = Create(SnowflakeResponses.FirstPartition(Handle, 10, 1));

            var (items, nextLink) = await ListAsync(provider, "?$top=10");

            Assert.AreEqual(10, items.Count);
            Assert.IsNotNull(nextLink);
            Assert.AreEqual("10", Query(nextLink)["$skip"]);
            Assert.AreEqual("10", Query(nextLink)["$top"]);
            Assert.IsNull(Query(nextLink)["$skiptoken"]);
        }

        [TestMethod]
        public async Task ListItems_MiddlePartitionEndingThePage_LinksToNextPage()
        {
            var (provider, _) = Create(
                select: null,
                partitions: FirstAndSecondPartition(SnowflakeResponses.FirstPartition(Handle, 4, 6, 1), SnowflakeResponses.Rows(5, 6)));

            var (items, nextLink) = await ListAsync(provider, $"?$top=10&$skiptoken={Handle}~1~3");

            CollectionAssert.AreEqual(Enumerable.Range(5, 6).ToList(), Ids(items));
            Assert.IsNotNull(nextLink);
            Assert.AreEqual("10", Query(nextLink)["$skip"]);
            Assert.IsNull(Query(nextLink)["$skiptoken"]);
        }

        [TestMethod]
        public async Task ListItems_LastPartitionOfFullPage_DropsLookAheadRowAndLinksToNextPage()
        {
            var (provider, client) = Create(
                select: null,
                partitions: FirstAndSecondPartition(SnowflakeResponses.FirstPartition(Handle, 2, 50), SnowflakeResponses.Rows(54, 50)));

            var (items, nextLink) = await ListAsync(provider, $"?$skip=51&$skiptoken={Handle}~1~2");

            Assert.AreEqual(0, client.Statements.Count, "A partition follow-up must not run the query again");
            CollectionAssert.AreEquivalent(new[] { (Handle, 0), (Handle, 1) }, client.PartitionRequests);
            CollectionAssert.AreEqual(Enumerable.Range(54, 49).ToList(), Ids(items));
            Assert.IsNotNull(nextLink);
            Assert.AreEqual("102", Query(nextLink)["$skip"]);
            Assert.IsNull(Query(nextLink)["$skiptoken"]);
        }

        [TestMethod]
        public async Task ListItems_LastPartitionWithTopAndSkip_KeepsRestOfPageAndLinksToNextPage()
        {
            var (provider, _) = Create(
                select: null,
                partitions: FirstAndSecondPartition(SnowflakeResponses.FirstPartition(Handle, 4, 7), SnowflakeResponses.Rows(25, 7)));

            var (items, nextLink) = await ListAsync(provider, $"?$top=10&$skip=20&$skiptoken={Handle}~1~2");

            CollectionAssert.AreEqual(Enumerable.Range(25, 6).ToList(), Ids(items));
            Assert.IsNotNull(nextLink);
            Assert.AreEqual("10", Query(nextLink)["$top"]);
            Assert.AreEqual("30", Query(nextLink)["$skip"]);
            Assert.IsNull(Query(nextLink)["$skiptoken"]);
        }

        [TestMethod]
        public async Task ListItems_LastPartitionOfLastPage_HasNoNextLink()
        {
            var (provider, _) = Create(
                select: null,
                partitions: FirstAndSecondPartition(SnowflakeResponses.FirstPartition(Handle, 2, 28), SnowflakeResponses.Rows(3, 28)));

            var (items, nextLink) = await ListAsync(provider, $"?$skiptoken={Handle}~1~2");

            Assert.AreEqual(28, items.Count);
            Assert.IsNull(nextLink);
        }

        [TestMethod]
        public async Task ListItems_SkipTokenWithWrongPartitionCount_FollowsPartitionsOfStatement()
        {
            var (provider, _) = Create(
                select: null,
                partitions: FirstAndSecondPartition(SnowflakeResponses.FirstPartition(Handle, 2, 30, 20), SnowflakeResponses.Rows(3, 30)));

            var (items, nextLink) = await ListAsync(provider, $"?$skiptoken={Handle}~1~2");

            Assert.AreEqual(30, items.Count);
            Assert.IsNotNull(nextLink);
            Assert.AreEqual($"{Handle}~2~3", Query(nextLink)["$skiptoken"]);
        }

        [DataTestMethod]
        [DataRow("~0~2")]
        [DataRow("~-1~2")]
        public async Task ListItems_SkipTokenForFirstPartition_ReturnsBadRequest(string partitionSuffix)
        {
            var (provider, _) = Create(select: null, partitions: _ => throw new AssertFailedException("No partition should be fetched"));

            var ex = await Assert.ThrowsExceptionAsync<HttpResponseException>(() => ListAsync(provider, $"?$skiptoken={Handle}{partitionSuffix}"));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.Response.StatusCode);
        }

        [DataTestMethod]
        [DataRow("abc")]
        [DataRow(Handle + "~x~2")]
        [DataRow("../../other-endpoint%3Ffoo=bar%23~1~2")]
        [DataRow("{" + Handle + "}~1~2")]
        [DataRow("01b2c3d4000000000000000000000001~1~2")]
        public async Task ListItems_SkipTokenNotForAPartition_ReturnsBadRequestWithoutQuerying(string skipToken)
        {
            var (provider, client) = Create(SnowflakeResponses.Rows(1));

            var ex = await Assert.ThrowsExceptionAsync<HttpResponseException>(() => ListAsync(provider, $"?$skiptoken={skipToken}"));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.Response.StatusCode);
            var message = await ex.Response.Content.ReadAsStringAsync();
            StringAssert.Contains(message.ToLowerInvariant(), "skiptoken");
            Assert.AreEqual(0, client.Statements.Count);
            Assert.AreEqual(0, client.PartitionRequests.Count);
        }

        [TestMethod]
        public async Task ListItems_PartitionedPageWithoutStatementHandle_LinksToRowsAfterPartition()
        {
            var firstPartition = SnowflakeResponses.FirstPartition(Handle, 2, 9);
            firstPartition.StatementHandle = null;
            var (provider, _) = Create(firstPartition);

            var (items, nextLink) = await ListAsync(provider, "?$top=10&$skip=20");

            Assert.AreEqual(2, items.Count);
            Assert.IsNotNull(nextLink);
            Assert.AreEqual("22", Query(nextLink)["$skip"]);
            Assert.AreEqual("10", Query(nextLink)["$top"]);
            Assert.IsNull(Query(nextLink)["$skiptoken"]);
        }

        [TestMethod]
        public async Task ListItems_EmptyPartitionWithoutStatementHandle_HasNoNextLinkBackToSameRequest()
        {
            var firstPartition = SnowflakeResponses.FirstPartition(Handle, 0, 9);
            firstPartition.StatementHandle = null;
            var (provider, _) = Create(firstPartition);

            var (items, nextLink) = await ListAsync(provider, "?$top=10&$skip=20");

            Assert.AreEqual(0, items.Count);
            Assert.IsNull(nextLink);
        }

        [TestMethod]
        public async Task ListItems_PartitionFollowUpWithInvalidTop_ReturnsBadRequest()
        {
            var (provider, _) = Create(
                select: null,
                partitions: FirstAndSecondPartition(SnowflakeResponses.FirstPartition(Handle, 2, 3), SnowflakeResponses.Rows(1, 3)));

            var ex = await Assert.ThrowsExceptionAsync<HttpResponseException>(() => ListAsync(provider, $"?$top=abc&$skiptoken={Handle}~1~2"));

            Assert.AreEqual(HttpStatusCode.BadRequest, ex.Response.StatusCode);
        }

        private static Func<int, SnowflakeAPIResponseModel> FirstAndSecondPartition(SnowflakeAPIResponseModel first, SnowflakeAPIResponseModel second)
            => partition => partition == 0 ? first
                : partition == 1 ? second
                : throw new AssertFailedException($"Unexpected partition: {partition}");

        private static IEdmModel BuildModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Item>("items");
            return builder.GetEdmModel();
        }

        private static (SnowflakeTableDataProvider Provider, SnowflakeClientSpy Client) Create(
            SnowflakeTableData? select,
            Func<int, SnowflakeAPIResponseModel>? partitions = null,
            long? count = null)
        {
            var client = new SnowflakeClientSpy(
                statement => statement.Statement.StartsWith("SELECT COUNT") && count != null
                    ? SnowflakeResponses.Count(count.Value)
                    : statement.Statement.StartsWith("SELECT") && !statement.Statement.StartsWith("SELECT COUNT") && select != null
                    ? select
                    : throw new AssertFailedException($"Unexpected statement: {statement.Statement}"),
                partitions);
            var operations = new SnowflakeDBOperations(client, new HttpClient(), NullLogger.Instance);
            var connectionParametersProvider = new SnowflakeConnectionParametersProvider(
                new ServerConnectionParametersProvider(new Uri(ItemsUrl)), NullLogger.Instance);
            return (new SnowflakeTableDataProvider(operations, connectionParametersProvider, NullLogger.Instance), client);
        }

        private static async Task<(List<Item> Items, Uri? NextLink)> ListAsync(SnowflakeTableDataProvider provider, string query)
        {
            var request = Request(query);

            var items = await provider.ListItemsAsync(request, "default", "T", Options(request));

            return (items.ToList(), request.ODataProperties().NextLink);
        }

        private static HttpRequestMessage Request(string query) => new HttpRequestMessage(HttpMethod.Get, ItemsUrl + query);

        private static ODataQueryOptions<Item> Options(HttpRequestMessage request)
            => new ODataQueryOptions<Item>(new ODataQueryContext(Model, typeof(Item), null), request);

        private static List<int> Ids(IEnumerable<Item> items) => items.Select(i => (int)i.DynamicProperties["ID"]).ToList();

        private static NameValueCollection Query(Uri link) => link.ParseQueryString();
    }
}
