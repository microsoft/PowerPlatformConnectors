// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

#nullable enable
namespace SnowflakeV2CoreLogic.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using SnowflakeV2CoreLogic.Exceptions;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;

    /// <summary>
    /// Maps item ids to primary key values.
    /// An item of a table with a single-column primary key is addressed by the key value as is.
    /// An item of a table with a composite primary key is addressed by its key values joined with commas,
    /// in the order of the key columns, which is the convention used by Power Platform tabular connectors.
    /// </summary>
    internal static class PrimaryKeyHelper
    {
        public const char CompositeKeySeparator = ',';

        private const string ColumnNameColumn = "column_name";
        private const string KeySequenceColumn = "key_sequence";

        /// <summary>
        /// Returns the primary key columns from a SHOW PRIMARY KEYS response, ordered by their position in the key.
        /// </summary>
        /// <param name="primaryKeyResponse">The SHOW PRIMARY KEYS response, which has one row per key column.</param>
        /// <returns>The key columns, or an empty list if the table has no primary key or the response cannot be read.</returns>
        public static IReadOnlyList<string> GetPrimaryKeyColumns(SnowflakeAPIResponseModel? primaryKeyResponse)
        {
            var rowType = primaryKeyResponse?.ResultSetMetaData?.RowType;
            var rows = primaryKeyResponse?.Data;
            if (rowType == null || rows == null || rows.Count == 0)
            {
                return Array.Empty<string>();
            }

            int columnNameIndex = rowType.FindIndex(x => string.Equals(x.Name, ColumnNameColumn, StringComparison.OrdinalIgnoreCase));
            int keySequenceIndex = rowType.FindIndex(x => string.Equals(x.Name, KeySequenceColumn, StringComparison.OrdinalIgnoreCase));
            if (columnNameIndex < 0 || keySequenceIndex < 0)
            {
                return Array.Empty<string>();
            }

            var keyColumns = new List<(string Column, int Sequence)>();
            foreach (var row in rows)
            {
                string? column = row[columnNameIndex]?.ToString();
                string? sequence = row[keySequenceIndex]?.ToString();
                if (string.IsNullOrEmpty(column) || !int.TryParse(sequence, NumberStyles.Integer, CultureInfo.InvariantCulture, out int keySequence))
                {
                    return Array.Empty<string>();
                }

                keyColumns.Add((column!, keySequence));
            }

            return keyColumns.OrderBy(k => k.Sequence).Select(k => k.Column).ToList();
        }

        /// <summary>
        /// Splits an item id into the values of the primary key columns.
        /// The id of a table with a single-column primary key is never split, so it may contain commas.
        /// </summary>
        /// <param name="id">The item id.</param>
        /// <param name="keyColumns">The primary key columns, ordered by their position in the key.</param>
        /// <param name="table">The table name.</param>
        /// <returns>The key columns paired with their values.</returns>
        public static IReadOnlyList<(string Column, string Value)> ParseItemId(string id, IReadOnlyList<string> keyColumns, string table)
        {
            if (keyColumns.Count == 1)
            {
                return new[] { (keyColumns[0], id) };
            }

            string[] values = id.Split(CompositeKeySeparator);
            if (values.Length != keyColumns.Count)
            {
                throw new HttpResponseException(
                    SnowflakeHttpException.CreateHttpResponseMessage(
                        HttpStatusCode.BadRequest,
                        string.Format(
                            CultureInfo.InvariantCulture,
                            Resource.SnowflakeCompositeItemIdMismatch,
                            table,
                            keyColumns.Count,
                            string.Join(", ", keyColumns),
                            values.Length)));
            }

            return keyColumns.Select((column, i) => (column, values[i])).ToList();
        }
    }
}
