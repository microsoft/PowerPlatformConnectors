// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Providers
{
    /// <summary>
    /// Contains the query constants for Snowflake.
    /// </summary>
    public static class QueryConstants
    {
        /// <summary>
        /// Query to select a list of objects in triggers.
        /// {0} = Comma separated field names. {1} = Table name.
        /// {2} = Ignored so that a common format string can be used for queries with and without filter.
        /// {3} = List of order by clauses. {4} = Number of results to return {5} = Number of results to skip.
        /// </summary>
        public const string SelectItemsQueryWithoutFilter = "SELECT {0} FROM {1} ORDER BY {2} LIMIT {3} OFFSET {4}";
        public const string SelectItemsQueryWithoutFilterAndOrderBy = "SELECT {0} FROM {1}  LIMIT {2} OFFSET {3}";

        public const string SelectItemsQueryWithFilter = "SELECT {0} FROM {1} WHERE {2} LIMIT {3} OFFSET {4}";
        public const string SelectItemsQueryWithFilterAndOrderBy = "SELECT {0} FROM {1} WHERE {2} ORDER BY {3} LIMIT {4} OFFSET {5}";
    }
}