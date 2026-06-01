// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.QueryOptions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Web.OData.Query;
    using Microsoft.OData.Core;
    using Microsoft.OData.Core.UriParser;
    using Microsoft.OData.Core.UriParser.Semantic;

    /// <summary>
    /// Filter, Apply and pagination options for SQL queries.
    /// </summary>
    public class QueryOptions
    {
        /// <summary>
        /// Creates a new instance of <see cref="SnowflakeV2Contracts.QueryOptions"/>.
        /// </summary>
        public QueryOptions()
        {
            Top = 0;
            Skip = 0;
            OrderBy = new OrderedDictionary();
        }

        /// <summary>
        /// Denotes ordering of the entries in a sql result set
        /// </summary>
        public enum Order
        {
            /// <summary>
            /// denotes ascending order.
            /// </summary>
            Asc,

            /// <summary>
            /// denotes descending order
            /// </summary>
            Desc,
        }

        /// <summary>
        /// OrderBy Clause for the query
        /// </summary>
        public IOrderedDictionary OrderBy { get; private set; }

        /// <summary>
        /// Size of the result set
        /// </summary>
        public long Top { get; set; }

        /// <summary>
        /// Initial number of entries to be skipped in the result set
        /// </summary>
        public long Skip { get; set; }

        /// <summary>
        /// Raw select string
        /// </summary>
        public string Select { get; set; }

        /// <summary>
        /// Select list
        /// </summary>
        public IEnumerable<SelectItem> SelectItems { get; set; }

        /// <summary>
        /// Expand list
        /// </summary>
        public string Expand { get; set; }

        /// <summary>
        /// Abstract Syntax Tree representation of $filter.
        /// </summary>
        public FilterQueryOption Filter { get; private set; }

        /// <summary>
        /// Abstract Syntax Tree representation of $apply
        /// </summary>
        public ApplyQueryOption Apply { get; private set; }

        /// <summary>
        /// Tag to differentiate between Top=null and Top=0
        /// </summary>
        public bool IsTopSet { get; private set; }

        /// <summary>
        /// Creates <see cref="SnowflakeV2Contracts.QueryOptions"/> from <see cref="ODataQueryOptions"/>.
        /// </summary>
        /// <param name="options">User options in open data format.</param>
        /// <returns>User options in the format specific to connector implementation.</returns>
        public static QueryOptions Parse(ODataQueryOptions options)
        {
            QueryOptions parsedQueryOptions = new QueryOptions();
            if (options != null)
            {
                var settings = new ODataValidationSettings();

                try
                {
                    options.Validate(settings);
                }
                catch (ODataException exception)
                {
                    if (exception.Message.Contains($"The node count limit of '{settings.MaxNodeCount}' has been exceeded"))
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ContractResource.ODataSyntaxTreeLimit, settings.MaxNodeCount), exception);
                    }

                    throw new ArgumentException(exception.Message, exception);
                }

                if (options.Top != null)
                {
                    parsedQueryOptions.IsTopSet = true;
                }

                SetOrderBy(options, parsedQueryOptions);
                SetTopSkipFilterAndApply(options, parsedQueryOptions);
                SetSelectAndExpand(options, parsedQueryOptions);
            }

            return parsedQueryOptions;
        }

        /// <summary>
        /// Sets the OrderBy query option.
        /// </summary>
        /// <param name="odataOptions">The OData options from which the query options need to be extracted.</param>
        /// <param name="queryOptions">The resulting query options.</param>
        private static void SetOrderBy(ODataQueryOptions odataOptions, QueryOptions queryOptions)
        {
            if (odataOptions.OrderBy == null)
            {
                return;
            }

            try
            {
                // Parse and set OrderBy.
                foreach (OrderByOpenPropertyNode node in odataOptions.OrderBy.OrderByNodes.OfType<OrderByOpenPropertyNode>())
                {
                    queryOptions.OrderBy.Add(node.PropertyName, node.Direction == OrderByDirection.Descending ? Order.Desc : Order.Asc);
                }
            }
            catch (ODataException exception)
            {
                throw new ArgumentException(exception.Message, exception);
            }
        }

        /// <summary>
        /// Sets the Top, Skip, Filter and Apply query options.
        /// </summary>
        /// <param name="odataOptions">The OData options from which the query options need to be extracted.</param>
        /// <param name="queryOptions">The resulting query options.</param>
        private static void SetTopSkipFilterAndApply(ODataQueryOptions odataOptions, QueryOptions queryOptions)
        {
            if (odataOptions == null)
            {
                return;
            }

            try
            {
                // Parse and set Top.
                if (odataOptions.Top != null)
                {
                    queryOptions.Top = odataOptions.Top.Value;
                }

                // Parse and set Skip.
                if (odataOptions.Skip != null)
                {
                    queryOptions.Skip = odataOptions.Skip.Value;
                }

                // Set already parsed filters.
                if (odataOptions.Filter != null)
                {
                    queryOptions.Filter = odataOptions.Filter;
                }

                if (odataOptions.Apply != null)
                {
                    queryOptions.Apply = odataOptions.Apply;
                }
            }
            catch (ODataException exception)
            {
                throw new ArgumentException(exception.Message, exception);
            }
        }

        /// <summary>
        /// Sets select and expand
        /// </summary>
        /// <param name="odataOptions">OData options</param>
        /// <param name="queryOptions">parsed query options</param>
        private static void SetSelectAndExpand(ODataQueryOptions odataOptions, QueryOptions queryOptions)
        {
            if (odataOptions == null)
            {
                return;
            }

            try
            {
                if (odataOptions.SelectExpand != null)
                {
                    if (!odataOptions.SelectExpand.SelectExpandClause.AllSelected)
                    {
                        queryOptions.Select = odataOptions.SelectExpand.RawSelect;
                        queryOptions.SelectItems = odataOptions.SelectExpand.SelectExpandClause.SelectedItems;
                    }

                    queryOptions.Expand = odataOptions.SelectExpand.RawExpand;
                }
            }
            catch (ODataException exception)
            {
                throw new ArgumentException(exception.Message, exception);
            }
        }
    }
}
