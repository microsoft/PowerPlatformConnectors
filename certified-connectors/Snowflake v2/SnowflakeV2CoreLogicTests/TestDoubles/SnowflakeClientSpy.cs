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

        public SnowflakeClientSpy(Func<RecordedStatement, SnowflakeTableData> responder)
        {
            this.responder = responder;
        }

        public List<RecordedStatement> Statements { get; } = new List<RecordedStatement>();

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
            => throw new NotImplementedException();

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

        public static SnowflakeTableData Rows(int count)
        {
            var rows = Enumerable.Range(1, count).Select(i => new string?[] { i.ToString(), $"name {i}" }).ToArray();
            return Table(new[] { ("ID", "fixed", (int?)0), ("NAME", "text", (int?)null) }, rows);
        }
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
