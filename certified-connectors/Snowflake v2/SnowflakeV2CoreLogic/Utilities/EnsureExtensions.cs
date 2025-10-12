// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Utilities
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides extension methods to ensure that the variables contain correct values.
    /// </summary>
    internal static class EnsureExtensions
    {
        // Setup the regex for the snowflake identifier
        private static readonly string UnquotedIdentifierRegexRules = @"^[a-zA-Z_][a-zA-Z0-9_$]{0,255}";

        // Quoted identifiers can be between 0 - 253 (not including quotes), and can contain ascii or extended ascii values: ^"[\x00-\x7F\x80-\xFF\u0100-\uFFFF]{0,253}"$
        private static readonly string QuotedIdentifierRegexRules = @"^""[\x00-\x7F\x80-\xFF\u0100-\uFFFF]{0,253}""$";

        private static readonly string IdentifierFullRegex = $"{UnquotedIdentifierRegexRules}|{QuotedIdentifierRegexRules}";
        private static readonly Regex IdentifierRegexPattern = new Regex(IdentifierFullRegex, RegexOptions.Compiled);

        // Setup the regex for the snowflake URL
        private static readonly string UrlFullRegex = @"[a-zA-Z0-9-_.]+\.snowflakecomputing\.com|[a-zA-Z0-9-_.]+\.privatelink\.snowflakecomputing\.com";
        private static readonly Regex UrlRegexPattern = new Regex(UrlFullRegex, RegexOptions.Compiled);

        /// <summary>
        /// Returns string if it is not null or empty. Throws ArgumentNullException otherwise.
        /// </summary>
        /// <param name="stringReference">String reference</param>
        /// <param name="name">SNOWFLAKE_HTTP_HEADER_TOKEN_TOKEN of string reference</param>
        /// <returns>String reference that is not null or empty</returns>
        public static string EnsureNotEmpty(
            this string stringReference,
            string name)
        {
            if (string.IsNullOrEmpty(stringReference))
            {
                throw new ArgumentNullException(name);
            }

            return stringReference;
        }

        /// <summary>
        /// Returns object reference if it is not set to null. Throws ArgumentNullException otherwise.
        /// </summary>
        /// <typeparam name="TReference">Type of object reference</typeparam>
        /// <param name="reference">Object reference</param>
        /// <param name="name">SNOWFLAKE_HTTP_HEADER_TOKEN_TOKEN of object reference</param>
        /// <returns>Non null object reference</returns>
        public static TReference EnsureNotNull<TReference>(
            this TReference reference,
            string name)
            where TReference : class
        {
            if (reference == null)
            {
                throw new ArgumentNullException(name);
            }

            return reference;
        }

        /// <summary>
        /// Returns string if it is not null or whitespace-only. Throws ArgumentNullException otherwise.
        /// </summary>
        /// <param name="stringReference">String reference</param>
        /// <param name="name">SNOWFLAKE_HTTP_HEADER_TOKEN_TOKEN of string reference</param>
        /// <returns>String reference that is not null or whitespace-only</returns>
        public static string EnsureNotWhiteSpace(
            this string stringReference,
            string name)
        {
            if (string.IsNullOrWhiteSpace(stringReference))
            {
                throw new ArgumentNullException(name);
            }

            return stringReference;
        }

        /// <summary>
        /// Checks if a snowflake identifier adheres to the Snowflake object identifier requirements
        /// Columns, tables names and other identifiers must follow these rules: [a-zA-Z_][a-zA-Z0-9_]{0,255}
        /// https://docs.snowflake.com/en/sql-reference/identifiers
        /// </summary>
        /// <param name="identifier">string reference of the identifier</param>
        /// <param name="nameOfIdentifier">SNOWFLAKE_HTTP_HEADER_TOKEN_TOKEN of the identifier</param>
        /// <returns>Validate snowflake identifiers</returns>
        public static string EnsureValidSnowflakeIdentifier(
            this string identifier,
            string nameOfIdentifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentNullException(identifier);
            }

            if (!IdentifierRegexPattern.IsMatch(identifier))
            {
                throw new ArgumentException($"Invalid snowflake identifier: {nameOfIdentifier}. Must adhere to the following regex: ${IdentifierFullRegex}");
            }

            return identifier;
        }

        /// <summary>
        /// Validates if a Snowflake URL is valid
        /// </summary>
        /// <param name="url">Snowflake URL</param>
        /// <param name="nameOfUrl">Identifier for this url</param>
        /// <returns>URL if it passes validation</returns>
        public static string EnsureValidSnowflakeUrl(
            this string url,
            string nameOfUrl)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(url);
            }

            if (!UrlRegexPattern.IsMatch(url))
            {
                throw new ArgumentException($"Invalid snowflake URL: {nameOfUrl}. Must adhere to the following regex: ${UrlFullRegex}");
            }

            return url;
        }
    }
}