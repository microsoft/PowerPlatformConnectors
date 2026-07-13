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
        private static readonly string UnquotedIdentifierRegexRules = @"[a-zA-Z_][a-zA-Z0-9_$]{0,255}";

        // Quoted identifiers are delimited by double quotes and may contain any characters. An
        // embedded double quote must be escaped by doubling it ("") per Snowflake's rules, so a lone
        // (unescaped) double quote ends the identifier. Combined with the start/end anchoring below,
        // this accepts every valid quoted identifier while preventing a value from breaking out of
        // the quoting (any trailing SQL after an unescaped quote fails the \z anchor).
        private static readonly string QuotedIdentifierRegexRules = @"""(?:[^""]|""""){0,253}""";

        // The whole value must be a single identifier (anchored start-to-end with \A and \z) so that
        // a valid prefix followed by arbitrary SQL cannot pass validation.
        private static readonly string IdentifierFullRegex = $@"\A(?:{UnquotedIdentifierRegexRules}|{QuotedIdentifierRegexRules})\z";
        private static readonly Regex IdentifierRegexPattern = new Regex(IdentifierFullRegex, RegexOptions.Compiled);

        // Matches a value that is a valid *unquoted* Snowflake identifier (anchored start-to-end).
        private static readonly Regex UnquotedIdentifierRegexPattern = new Regex($@"\A{UnquotedIdentifierRegexRules}\z", RegexOptions.Compiled);

        // Setup the regex for the snowflake URL. The top-level domain is ".com" for global
        // deployments and ".cn" for the China deployment (e.g. account.region.snowflakecomputing.cn).
        private static readonly string UrlFullRegex = @"^([a-zA-Z0-9-_.]+\.snowflakecomputing\.(?:com|cn)|[a-zA-Z0-9-_.]+\.privatelink\.snowflakecomputing\.(?:com|cn))$";
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
                throw new ArgumentNullException(nameOfIdentifier);
            }

            if (!IdentifierRegexPattern.IsMatch(identifier))
            {
                throw new ArgumentException($"Invalid snowflake identifier: {nameOfIdentifier}.");
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
        /// Returns a safe SQL representation of a caller-supplied identifier (for example a filter
        /// property name) for direct interpolation into a statement. A value that is already a valid
        /// <em>unquoted</em> Snowflake identifier is returned unchanged, preserving Snowflake's
        /// case-insensitive resolution for the common case. Any other value is wrapped in double
        /// quotes with embedded quotes escaped (<c>"</c> -&gt; <c>""</c>), producing a safe,
        /// case-sensitive quoted identifier. Because every embedded quote is doubled, the value can
        /// never break out of the quoting, so this is injection-safe for arbitrary input.
        /// </summary>
        /// <param name="identifier">The identifier to render.</param>
        /// <param name="nameOfIdentifier">Identifier for this value (used in error messages).</param>
        /// <returns>The identifier as-is when unquoted-valid; otherwise a quoted, escaped identifier.</returns>
        public static string ToSafeSnowflakeIdentifier(
            this string identifier,
            string nameOfIdentifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentNullException(nameOfIdentifier);
            }

            if (UnquotedIdentifierRegexPattern.IsMatch(identifier))
            {
                return identifier;
            }

            return $"\"{identifier.EscapeSnowflakeQuotedIdentifier()}\"";
        }

        /// <summary>
        /// Validates an OData <c>$select</c> projection that is interpolated directly into the SQL
        /// statement. The projection must be either <c>*</c> or a comma-separated list of valid
        /// Snowflake identifiers; anything else (sub-queries, expressions, stacked statements, ...)
        /// is rejected. Quoted identifiers may contain commas (the field list is split in a
        /// quote-aware manner).
        /// </summary>
        /// <param name="select">The select projection.</param>
        /// <param name="nameOfSelect">Identifier for this value (used in error messages).</param>
        /// <returns>The original value if every field passes validation.</returns>
        public static string EnsureValidSelectClause(
            this string select,
            string nameOfSelect)
        {
            if (string.IsNullOrWhiteSpace(select))
            {
                throw new ArgumentNullException(nameOfSelect);
            }

            if (string.Equals(select.Trim(), "*", StringComparison.Ordinal))
            {
                return select;
            }

            foreach (var field in SplitTopLevel(select, ','))
            {
                var trimmedField = field.Trim();

                if (string.Equals(trimmedField, "*", StringComparison.Ordinal))
                {
                    continue;
                }

                if (trimmedField.Length == 0)
                {
                    throw new ArgumentException($"Invalid select clause: {nameOfSelect}. Fields must not be empty.");
                }

                trimmedField.EnsureValidSnowflakeIdentifier(nameOfSelect);
            }

            return select;
        }

        /// <summary>
        /// Validates an OData <c>$orderby</c> clause that is interpolated directly into the SQL
        /// statement. Each comma-separated entry must be a valid Snowflake identifier optionally
        /// followed by an <c>asc</c>/<c>desc</c> direction; anything else is rejected. Quoted
        /// identifiers may contain whitespace and commas (the clause is parsed in a quote-aware
        /// manner).
        /// </summary>
        /// <param name="orderBy">The order-by clause.</param>
        /// <param name="nameOfOrderBy">Identifier for this value (used in error messages).</param>
        /// <returns>The original value if every entry passes validation.</returns>
        public static string EnsureValidOrderByClause(
            this string orderBy,
            string nameOfOrderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                throw new ArgumentNullException(nameOfOrderBy);
            }

            foreach (var entry in SplitTopLevel(orderBy, ','))
            {
                SplitOrderByEntry(entry, out var identifier, out var direction);

                if (identifier.Length == 0)
                {
                    throw new ArgumentException($"Invalid order by clause: {nameOfOrderBy}. Each entry must be '<identifier> [asc|desc]'.");
                }

                identifier.EnsureValidSnowflakeIdentifier(nameOfOrderBy);

                if (direction.Length > 0
                    && !string.Equals(direction, "asc", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException($"Invalid order by direction: {nameOfOrderBy}. Only 'asc' and 'desc' are allowed.");
                }
            }

            return orderBy;
        }

        /// <summary>
        /// Splits a clause on the given delimiter, ignoring delimiters that appear inside a double
        /// quoted identifier (where an embedded double quote is escaped by doubling it).
        /// </summary>
        private static IEnumerable<string> SplitTopLevel(string input, char delimiter)
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
        /// Splits a single order-by entry into its identifier and optional direction. The identifier
        /// may be a double quoted identifier containing whitespace; the direction (if present) is the
        /// remaining text after the identifier.
        /// </summary>
        private static void SplitOrderByEntry(string entry, out string identifier, out string direction)
        {
            var trimmed = entry.Trim();

            if (trimmed.Length > 0 && trimmed[0] == '"')
            {
                var i = 1;
                while (i < trimmed.Length)
                {
                    if (trimmed[i] == '"')
                    {
                        if (i + 1 < trimmed.Length && trimmed[i + 1] == '"')
                        {
                            i += 2;
                            continue;
                        }

                        break;
                    }

                    i++;
                }

                if (i >= trimmed.Length)
                {
                    identifier = trimmed;
                    direction = string.Empty;
                    return;
                }

                identifier = trimmed.Substring(0, i + 1);
                direction = trimmed.Substring(i + 1).Trim();
            }
            else
            {
                var whitespaceIndex = trimmed.IndexOfAny(new[] { ' ', '\t', '\r', '\n' });

                if (whitespaceIndex < 0)
                {
                    identifier = trimmed;
                    direction = string.Empty;
                }
                else
                {
                    identifier = trimmed.Substring(0, whitespaceIndex);
                    direction = trimmed.Substring(whitespaceIndex).Trim();
                }
            }
        }

        /// <summary>
        /// Escapes the contents of a Snowflake quoted identifier by doubling any embedded double
        /// quotes, so a value wrapped in double quotes by the caller cannot terminate the quoting.
        /// </summary>
        /// <param name="identifier">The identifier body to escape.</param>
        /// <returns>The escaped identifier body.</returns>
        public static string EscapeSnowflakeQuotedIdentifier(
            this string identifier)
        {
            return identifier == null ? string.Empty : identifier.Replace("\"", "\"\"");
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
                throw new ArgumentNullException(nameOfUrl);
            }

            if (!UrlRegexPattern.IsMatch(url))
            {
                throw new ArgumentException($"Invalid snowflake URL: {nameOfUrl}.");
            }

            return url;
        }
    }
}
