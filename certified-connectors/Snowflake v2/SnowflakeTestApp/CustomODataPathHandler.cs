// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeTestApp
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http;
    using System.Web.OData.Routing;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Constants;
    using Microsoft.OData.Edm;
    using SnowflakeV2CoreLogic.Exceptions;

    public class CustomODataPathHandler : DefaultODataPathHandler
    {
        /// <summary>
        /// datasets segment index
        /// </summary>
        private const int DataSetsIndex = 0;

        /// <summary>
        /// dataset segment index
        /// </summary>
        private const int DataSetIndex = 1;

        /// <summary>
        /// tables segment index
        /// </summary>
        private const int TablesIndex = 2;

        /// <summary>
        /// table segment index
        /// </summary>
        private const int TableIndex = 3;

        /// <summary>
        /// items segment index
        /// </summary>
        private const int ItemsIndex = 4;

        /// <summary>
        /// item segment index
        /// </summary>
        private const int ItemIndex = 5;

        /// <summary>
        /// Trigger segment index
        /// </summary>
        private const int TriggerIndex = 4;

        /// <summary>
        /// procedures segment index
        /// </summary>
        private const int ProceduresIndex = 2;

        /// <summary>
        /// procedure segment index
        /// </summary>
        private const int ProcedureIndex = 3;

        /// <summary>
        /// pass-through native query segment index
        /// </summary>
        private const int PassThroughNativeQueryIndex = 2;

        /// <summary>
        /// pass-through native query language index
        /// </summary>
        private const int PassThroughNativeQueryLanguageIndex = 3;

        /// <summary>
        /// Gets OData path for a given "()" path syntax or "/" path syntax
        /// </summary>
        /// <param name="path">input path</param>
        /// <returns>OData path</returns>
        public static string GetODataPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            StringBuilder odataPathStringBuilder = new StringBuilder(string.Empty);

            string uriPath = path;
            string queryString = string.Empty;

            int queryPathStartIndex = path.IndexOf('?');

            if (queryPathStartIndex >= 0)
            {
                uriPath = path.Substring(0, queryPathStartIndex);
                queryString = path.Substring(queryPathStartIndex);
            }

            var segments = uriPath.Split(new char[] { '/' });

            if (segments.Length >= DataSetsIndex + 1)
            {
                // Handle datasets
                if (segments[DataSetsIndex].Equals(StringConstants.DataSets, System.StringComparison.OrdinalIgnoreCase))
                {
                    ParseDataSets(segments, odataPathStringBuilder);
                }
                else
                {
                    // else use odata path as is
                    odataPathStringBuilder.Append(uriPath);
                }
            }

            // Handle query parameters
            if (!string.IsNullOrEmpty(queryString))
            {
                // Validate OData query parameters before proceeding further.
                ValidateOdataQueryParameters(path);

                // Explicitly encode ':' to prevent exceptions in path parsing later on in case of long Url (> 1024 characters + length of service root).
                // Even if Location header contains encoded ':', APIM can provide unencoded ':' in referrer header for query parameters.
                odataPathStringBuilder.Append(queryString.Replace(":", "%3A"));
            }

            return odataPathStringBuilder.ToString();
        }

        /// <summary>
        /// Parses the specified OData path as an System.Web.OData.Routing.ODataPath
        /// that contains additional information about the EDM type and entity set for
        /// the path.
        /// </summary>
        /// <param name="model">The model to use for path parsing.</param>
        /// <param name="serviceRoot">The service root of the OData path.</param>
        /// <param name="odataPath">The OData path to parse.</param>
        /// <returns> A parsed representation of the path, or null if the path does not match the model.</returns>
        public override ODataPath Parse(IEdmModel model, string serviceRoot, string odataPath)
        {
            if (string.IsNullOrEmpty(odataPath))
            {
                return base.Parse(model, serviceRoot, odataPath);
            }

            if (odataPath.Equals("favicon.ico", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            string newOdataPath = GetODataPath(odataPath);

            return base.Parse(model, serviceRoot, newOdataPath);
        }

        /// <summary>
        /// Parses datasets present in the web api uri path and converts them into OData path format
        /// </summary>
        /// <param name="segments">uri segments</param>
        /// <param name="odataPathStringBuilder">OData path string builder</param>
        private static void ParseDataSets(string[] segments, StringBuilder odataPathStringBuilder)
        {
            // datasets
            odataPathStringBuilder.Append(StringConstants.DataSets);

            // dataset
            if (segments.Length >= DataSetIndex + 1)
            {
                odataPathStringBuilder.Append(string.Concat("('", segments[DataSetIndex], "')"));
            }

            if (segments.Length >= TablesIndex + 1)
            {
                // Handle tables
                if (segments[TablesIndex].Equals(StringConstants.Tables, StringComparison.OrdinalIgnoreCase))
                {
                    odataPathStringBuilder.Append("/");
                    ParseTables(segments, odataPathStringBuilder);
                }
            }

            if (segments.Length >= ProceduresIndex + 1)
            {
                if (segments[ProceduresIndex].Equals(StringConstants.Procedures, StringComparison.OrdinalIgnoreCase))
                {
                    odataPathStringBuilder.Append("/");
                    ParseProcedures(segments, odataPathStringBuilder);
                }
            }

            if (segments.Length >= PassThroughNativeQueryIndex + 1)
            {
                if (segments[PassThroughNativeQueryIndex].Equals(StringConstants.Query, StringComparison.OrdinalIgnoreCase))
                {
                    odataPathStringBuilder.Append("/");
                    ParsePassThroughDirectQueries(segments, odataPathStringBuilder);
                }
            }
        }

        /// <summary>
        /// Parses pass-through direct queries present in the web api uri path and converts them into OData path format
        /// </summary>
        /// <param name="segments">uri segments</param>
        /// <param name="odataPathStringBuilder">OData path string builder</param>
        private static void ParsePassThroughDirectQueries(string[] segments, StringBuilder odataPathStringBuilder)
        {
            odataPathStringBuilder.Append(StringConstants.Query);

            if (segments.Length >= PassThroughNativeQueryLanguageIndex + 1)
            {
                odataPathStringBuilder.Append(string.Concat("('", segments[PassThroughNativeQueryLanguageIndex], "')"));
            }
        }

        /// <summary>
        /// Parses procedures present in the web api uri path and converts them into OData path format
        /// </summary>
        /// <param name="segments">uri segments</param>
        /// <param name="odataPathStringBuilder">OData path string builder</param>
        private static void ParseProcedures(string[] segments, StringBuilder odataPathStringBuilder)
        {
            odataPathStringBuilder.Append(StringConstants.Procedures);

            if (segments.Length >= ProcedureIndex + 1)
            {
                odataPathStringBuilder.Append(string.Concat("('", segments[ProcedureIndex], "')"));
            }
        }

        /// <summary>
        /// Parses tables present in the web api uri path and converts them into OData path format
        /// </summary>
        /// <param name="segments">uri segments</param>
        /// <param name="odataPathStringBuilder">OData path string builder</param>
        private static void ParseTables(string[] segments, StringBuilder odataPathStringBuilder)
        {
            // tables
            odataPathStringBuilder.Append(StringConstants.Tables);

            // table
            if (segments.Length >= TableIndex + 1)
            {
                odataPathStringBuilder.Append(string.Concat("('", segments[TableIndex], "')"));
            }

            if (segments.Length >= ItemsIndex + 1)
            {
                // Handle items
                if (segments[ItemsIndex].Equals(StringConstants.Items, System.StringComparison.OrdinalIgnoreCase))
                {
                    odataPathStringBuilder.Append("/");
                    ParseItems(segments, odataPathStringBuilder);
                }
            }

            if (segments.Length >= TriggerIndex + 1)
            {
                // Handle triggers
                if (segments[TriggerIndex].Equals(StringConstants.NewItemTrigger, System.StringComparison.OrdinalIgnoreCase) ||
                    segments[TriggerIndex].Equals(StringConstants.UpdatedItemTrigger, System.StringComparison.OrdinalIgnoreCase) ||
                    segments[TriggerIndex].Equals(StringConstants.DeletedItemTrigger, System.StringComparison.OrdinalIgnoreCase) ||
                    segments[TriggerIndex].Equals(StringConstants.ChangedItemTrigger, System.StringComparison.OrdinalIgnoreCase))
                {
                    odataPathStringBuilder.Append("/");
                    odataPathStringBuilder.Append(segments[TriggerIndex]);
                }
            }
        }

        /// <summary>
        /// Parses items present in the web api uri path and converts them into OData path format
        /// </summary>
        /// <param name="segments">uri segments</param>
        /// <param name="odataPathStringBuilder">OData path string builder</param>
        private static void ParseItems(string[] segments, StringBuilder odataPathStringBuilder)
        {
            // items
            odataPathStringBuilder.Append(StringConstants.Items);

            // item
            if (segments.Length >= ItemIndex + 1)
            {
                odataPathStringBuilder.Append(string.Concat("('", segments[ItemIndex], "')"));
            }
        }

        /// <summary>
        /// Validate OData Query Parameters
        /// </summary>
        /// <param name="path">Path</param>
        private static void ValidateOdataQueryParameters(string path)
        {
            // validate the OData Query String. - OData API throws ODataException if the OData Query Parameters like $skip, $top, $filter, $orderby is empty
            // Create a dummy request and read query parameters
            using (HttpRequestMessage dummmyRequest = new HttpRequestMessage(HttpMethod.Get, new Uri("http://localhost.com/" + path)))
            {
                IEnumerable<KeyValuePair<string, string>> queryParameterKeyValuePairs = dummmyRequest.GetQueryNameValuePairs();

                foreach (var queryParameter in queryParameterKeyValuePairs)
                {
                    if (queryParameter.Key.Equals("$skip", StringComparison.OrdinalIgnoreCase) ||
                        queryParameter.Key.Equals("$top", StringComparison.OrdinalIgnoreCase) ||
                        queryParameter.Key.Equals("$select", StringComparison.OrdinalIgnoreCase) ||
                        queryParameter.Key.Equals("$filter", StringComparison.OrdinalIgnoreCase) ||
                        queryParameter.Key.Equals("$orderby", StringComparison.OrdinalIgnoreCase) ||
                        queryParameter.Key.Equals("$apply", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrWhiteSpace(queryParameter.Value))
                        {
                            string message = string.Format("The value for OData query '{0}' cannot be empty", queryParameter.Key);
                            throw new HttpResponseException(SnowflakeHttpException.CreateHttpResponseMessage(HttpStatusCode.BadRequest, message));
                        }
                    }
                }
            }
        }
    }

}