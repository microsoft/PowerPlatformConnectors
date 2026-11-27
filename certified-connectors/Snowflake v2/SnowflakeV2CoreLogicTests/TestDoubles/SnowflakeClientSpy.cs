namespace SnowflakeV2CoreLogic.Tests.TestDoubles
{
    using System.Net.Http;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Interfaces;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using SnowflakeV2CoreLogic.Models;
    using SnowflakeV2CoreLogic.Models.ConnectorModels;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;
    using SnowflakeV2CoreLogic.Utilities;

    /// <summary>
    /// A statement sent to Snowflake, with its positional binding values in index order.
    /// </summary>
    internal sealed class RecordedStatement
    {
        public RecordedStatement(string statement, SnowflakeRequestBindings? bindings)
        {
            Statement = statement;
            BindingValues = bindings?.bindings?.Properties()
                .OrderBy(p => int.Parse(p.Name))
                .Select(p => (string?)p.Value["value"])
                .ToList() ?? new List<string?>();
        }

        public string Statement { get; }

        public IReadOnlyList<string?> BindingValues { get; }
    }

    /// <summary>
    /// Records every statement and answers it with a canned response.
    /// </summary>
    internal sealed class SnowflakeClientSpy : ISnowflakeClient
    {
        private readonly Func<RecordedStatement, SnowflakeTableData> responder;
        private readonly Func<int, SnowflakeAPIResponseModel>? partitionResponder;

        public SnowflakeClientSpy(
            Func<RecordedStatement, SnowflakeTableData> responder,
            Func<int, SnowflakeAPIResponseModel>? partitionResponder = null)
        {
            this.responder = responder;
            this.partitionResponder = partitionResponder;
        }

        public List<RecordedStatement> Statements { get; } = new List<RecordedStatement>();

        public List<(string StatementHandle, int Partition)> PartitionRequests { get; } = new List<(string StatementHandle, int Partition)>();

        public Task<SnowflakeTableData> CallAPIAsync(
            HttpClient? client,
            string sqlStatement,
            string endpoint,
            SnowflakeRequestBindings? requestBindings = null,
            SnowflakeConnectionParameters? perRequestConnectionParameters = null,
            RequestParameters? requestParameters = null,
            bool isSerializerSettings = false)
        {
            var recorded = new RecordedStatement(sqlStatement, requestBindings);
            Statements.Add(recorded);
            return Task.FromResult(responder(recorded));
        }

        public Task<SnowflakeAPIResponseModel> ExecuteSqlStatementAsync(HttpClient? client, ExecuteSqlStatementModel fullAPIRequestPayload, HeaderParameters headerParameters, QueryParameters queryParams, string endpoint)
            => throw new NotImplementedException();

        public Task<SnowflakeAPIResponseModel> GetResultsAsync(HttpClient? client, string statementHandle, HeaderParameters headerParameters, QueryParameters queryParams)
        {
            if (partitionResponder == null)
            {
                throw new NotImplementedException();
            }

            int partition = queryParams.Partition ?? 0;
            lock (PartitionRequests)
            {
                PartitionRequests.Add((statementHandle, partition));
            }

            return Task.FromResult(partitionResponder(partition));
        }

        public Task<SnowflakeAPIResponseModel> CancelRequestAsync(HttpClient? client, string statementHandle, HeaderParameters headerParameters, QueryParameters queryParams)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Builds responses shaped like the ones returned by the Snowflake SQL API (all values are strings).
    /// </summary>
    internal static class SnowflakeResponses
    {
        private static readonly string[] ShowPrimaryKeysColumns =
        {
            "created_on", "database_name", "schema_name", "table_name", "column_name", "key_sequence", "constraint_name", "rely", "comment",
        };

        public static SnowflakeTableData Table(IReadOnlyList<(string Name, string Type, int? Scale)> columns, params string?[][] rows)
        {
            return new SnowflakeTableData
            {
                ResultSetMetaData = new ResultSetMetaData
                {
                    NumRows = rows.Length,
                    RowType = columns.Select(c => new RowType { Name = c.Name, Type = c.Type, Scale = c.Scale }).ToList(),
                },
                Data = rows.Select(r => r.Select(v => (object)v!).ToList()).ToList(),
            };
        }

        /// <summary>
        /// A SHOW PRIMARY KEYS response with one row per key column.
        /// </summary>
        public static SnowflakeTableData PrimaryKeys(params (string Column, int Sequence)[] keys)
        {
            var columns = ShowPrimaryKeysColumns
                .Select(name => (Name: name, Type: name == "key_sequence" ? "fixed" : "text", Scale: name == "key_sequence" ? (int?)0 : null))
                .ToList();

            var rows = keys
                .Select(k => new string?[] { "1700000000.000", "DB", "PUBLIC", "T", k.Column, k.Sequence.ToString(), "PK_T", "false", null })
                .ToArray();

            return Table(columns, rows);
        }

        public static SnowflakeTableData NoPrimaryKey() => PrimaryKeys();

        public static SnowflakeTableData RowsDeleted(long count)
            => Table(new[] { ("number of rows deleted", "fixed", (int?)0) }, new[] { count.ToString() });

        public static SnowflakeTableData RowsUpdated(long count)
            => Table(
                new[] { ("number of rows updated", "fixed", (int?)0), ("number of multi-joined rows updated", "fixed", (int?)0) },
                new[] { count.ToString(), "0" });

        public static SnowflakeTableData Rows(int count) => Rows(1, count);

        /// <summary>
        /// ID and NAME rows with consecutive IDs starting at <paramref name="firstId"/>.
        /// </summary>
        public static SnowflakeTableData Rows(int firstId, int count)
        {
            var rows = Enumerable.Range(firstId, count).Select(i => new string?[] { i.ToString(), $"name {i}" }).ToArray();
            return Table(new[] { ("ID", "fixed", (int?)0), ("NAME", "text", (int?)null) }, rows);
        }

        /// <summary>
        /// The first partition of a statement whose result Snowflake split into partitions of the given row counts.
        /// </summary>
        public static SnowflakeTableData FirstPartition(string statementHandle, params int[] partitionRowCounts)
        {
            var response = Rows(1, partitionRowCounts[0]);
            response.StatementHandle = statementHandle;
            response.ResultSetMetaData!.NumRows = partitionRowCounts.Sum();
            response.ResultSetMetaData.PartitionInfo = partitionRowCounts.Select(count => new PartitionInfo { RowCount = count }).ToList();
            return response;
        }

        public static SnowflakeTableData Count(long count)
            => Table(new[] { ("COUNT(*)", "fixed", (int?)0) }, new[] { count.ToString() });
    }

    /// <summary>
    /// Connection parameters provider for a connection to one Snowflake server, called through the given URL.
    /// </summary>
    internal sealed class ServerConnectionParametersProvider : IConnectionParametersProvider
    {
        private const string Server = "account.snowflakecomputing.com";
        private readonly Uri referrerUri;

        public ServerConnectionParametersProvider(Uri referrerUri)
        {
            this.referrerUri = referrerUri;
        }

        public T GetProperty<T>(string key) => PropertyExists(key) ? (T)(object)Server : throw new KeyNotFoundException(key);

        public bool PropertyExists(string key) => key == Constants.Server;

        public bool TryGetProperty<T>(string key, out T value)
        {
            value = default!;
            return false;
        }

        public IToken GetToken() => null!;

        public Uri GetReferrerUri() => referrerUri;
    }

    /// <summary>
    /// Connection parameters provider with no properties, which yields default connection parameters.
    /// </summary>
    internal sealed class EmptyConnectionParametersProvider : IConnectionParametersProvider
    {
        public T GetProperty<T>(string key) => throw new KeyNotFoundException(key);

        public bool PropertyExists(string key) => false;

        public bool TryGetProperty<T>(string key, out T value)
        {
            value = default!;
            return false;
        }

        public IToken GetToken() => null!;

        public Uri GetReferrerUri() => new Uri("https://localhost/");
    }
}
