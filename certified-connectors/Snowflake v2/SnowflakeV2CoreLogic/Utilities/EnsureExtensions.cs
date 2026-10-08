// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Text;
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
        private static readonly string UrlFullRegex = @"^([a-zA-Z0-9-_.]+\.snowflakecomputing\.com|[a-zA-Z0-9-_.]+\.privatelink\.snowflakecomputing\.com)$";
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
        /// Validates a possibly-qualified Snowflake object identifier (for example <c>TABLE</c>,
        /// <c>SCHEMA.TABLE</c> or <c>DATABASE.SCHEMA.TABLE</c>) that is interpolated directly into
        /// the SQL statement. Each dot-separated part must be a valid (quoted or unquoted) Snowflake
        /// identifier. The value is split in a quote-aware manner, so a quoted part may itself
        /// contain a dot (for example <c>DB."my.schema".TABLE</c>).
        /// </summary>
        /// <param name="identifier">The (optionally qualified) identifier.</param>
        /// <param name="nameOfIdentifier">Identifier for this value (used in error messages).</param>
        /// <returns>The original value if every part passes validation.</returns>
        public static string EnsureValidQualifiedSnowflakeIdentifier(
            this string identifier,
            string nameOfIdentifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentNullException(nameOfIdentifier);
            }

            foreach (var part in SplitTopLevel(identifier, '.'))
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    throw new ArgumentException($"Invalid snowflake identifier: {nameOfIdentifier}. Qualified name parts must not be empty.");
                }

                part.EnsureValidSnowflakeIdentifier(nameOfIdentifier);
            }

            return identifier;
        }

        /// <summary>
        /// Validates a possibly-qualified Snowflake object identifier and ensures that its qualifiers,
        /// if present, resolve to the given database and schema. A null or empty database or schema
        /// leaves the corresponding qualifier unchecked. Qualifiers are resolved the way Snowflake
        /// resolves identifiers: unquoted parts are upper-cased and quoted parts are taken verbatim.
        /// The database and schema are exact names, so they are compared case-sensitively.
        /// </summary>
        /// <param name="identifier">The (optionally qualified) identifier.</param>
        /// <param name="database">The database the identifier must resolve to, if any.</param>
        /// <param name="schema">The schema the identifier must resolve to, if any.</param>
        /// <param name="nameOfIdentifier">Identifier for this value (used in error messages).</param>
        /// <returns>The original value if it is valid and resolves to the given database and schema.</returns>
        public static string EnsureQualifiedIdentifierWithinScope(
            this string identifier,
            string database,
            string schema,
            string nameOfIdentifier)
        {
            identifier.EnsureValidQualifiedSnowflakeIdentifier(nameOfIdentifier);

            var parts = SplitTopLevel(identifier, '.');

            if (parts.Count > 3)
            {
                throw new ArgumentException($"Invalid snowflake identifier: {nameOfIdentifier}. At most a database, schema and object name may be specified.");
            }

            if (parts.Count >= 2 && !string.IsNullOrEmpty(schema) && !string.Equals(ResolveSnowflakeIdentifier(parts[parts.Count - 2]), schema, StringComparison.Ordinal))
            {
                throw new ArgumentException($"Invalid snowflake identifier: {nameOfIdentifier}. The schema must match the schema in use.");
            }

            if (parts.Count == 3 && !string.IsNullOrEmpty(database) && !string.Equals(ResolveSnowflakeIdentifier(parts[0]), database, StringComparison.Ordinal))
            {
                throw new ArgumentException($"Invalid snowflake identifier: {nameOfIdentifier}. The database must match the database in use.");
            }

            return identifier;
        }

        /// <summary>
        /// Splits a clause on the given delimiter, ignoring delimiters that appear inside a double
        /// quoted identifier (where an embedded double quote is escaped by doubling it).
        /// </summary>
        private static List<string> SplitTopLevel(string input, char delimiter)
        {
            var segments = new List<string>();
            var current = new StringBuilder();
            var inQuotes = false;

            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];

                if (c == '"')
                {
                    current.Append(c);

                    if (inQuotes && i + 1 < input.Length && input[i + 1] == '"')
                    {
                        current.Append(input[++i]);
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == delimiter && !inQuotes)
                {
                    segments.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            segments.Add(current.ToString());
            return segments;
        }

        /// <summary>
        /// Returns the object name a single, already validated identifier refers to: the unescaped
        /// body of a quoted identifier, or the upper-cased value of an unquoted one.
        /// </summary>
        private static string ResolveSnowflakeIdentifier(string identifier)
        {
            if (identifier.Length >= 2 && identifier[0] == '"' && identifier[identifier.Length - 1] == '"')
            {
                return identifier.Substring(1, identifier.Length - 2).Replace("\"\"", "\"");
            }

            return identifier.ToUpperInvariant();
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