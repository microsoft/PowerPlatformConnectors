// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Utilities
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Microsoft.OData.Core.UriParser.Semantic;
    using Microsoft.OData.Core.UriParser.TreeNodeKinds;
    using Microsoft.OData.Edm.Library;

    /// <summary>
    /// OData to SQL Parser
    /// 
    /// Caller-supplied string values are escaped before being embedded as SQL string literals, and
    /// identifiers (column/property names) are validated as Snowflake identifiers.
    /// 
    /// Supported Operations:
    /// 
    /// Comparison Operators:
    /// - eq (equal to)
    /// - ne (not equal to)
    /// - gt (greater than)
    /// - lt (less than)
    /// - ge (greater than or equal to)
    /// - le (less than or equal to)
    /// 
    /// Logical Operators:
    /// - and (logical AND)
    /// - or (logical OR)
    /// - not (logical NOT)
    /// 
    /// String Functions:
    /// - contains(property, value): Checks if property contains the specified value
    /// - startswith(property, value): Checks if property starts with the specified value
    /// - endswith(property, value): Checks if property ends with the specified value
    /// - tolower(expression): Converts the expression to lowercase
    /// - toupper(expression): Converts the expression to uppercase
    /// 
    /// </summary>
    public class ODataToSqlParser
    {
        private readonly bool useCaseInsensitiveFilters;

        public ODataToSqlParser(bool useCaseInsensitiveFilters = false)
        {
            this.useCaseInsensitiveFilters = useCaseInsensitiveFilters;
        }

        public string ParseFilterToSql(FilterClause filterClause)
        {
            if (filterClause == null)
            {
                return string.Empty;
            }

            return ParseExpression(filterClause.Expression);
        }

        private string ParseExpression(SingleValueNode expression)
        {
            // Handle ConvertNode
            if (expression is ConvertNode convertNode)
            {
                // Just parse the source of the conversion
                return ParseExpression(convertNode.Source);
            }
            else if (expression is BinaryOperatorNode binaryOperatorNode)
            {
                return ParseBinaryOperator(binaryOperatorNode);
            }
            else if (expression is UnaryOperatorNode unaryOperatorNode)
            {
                return ParseUnaryOperator(unaryOperatorNode);
            }
            else if (expression is SingleValueFunctionCallNode functionCallNode)
            {
                return ParseFunctionCall(functionCallNode);
            }
            else if (expression is SingleValuePropertyAccessNode propertyAccessNode)
            {
                return propertyAccessNode.Property.Name.ToSafeSnowflakeIdentifier("Filter property");
            }
            // Handle open property access (dynamic properties)
            else if (expression is SingleValueOpenPropertyAccessNode openPropertyNode)
            {
                return openPropertyNode.Name.ToSafeSnowflakeIdentifier("Filter property");
            }
            else if (expression is ConstantNode constantNode)
            {
                return FormatConstant(constantNode);
            }

            throw new NotSupportedException($"Unsupported expression type: {expression.GetType().Name}");
        }

        /// <summary>
        /// Formats an OData constant as a Snowflake SQL literal.
        /// Strings, GUIDs and temporal values are emitted as single-quoted literals; Snowflake casts them
        /// implicitly when compared to the column. Unquoted, a GUID or date would be parsed as arithmetic.
        /// On open types a date-only literal is parsed as a DateTimeOffset at midnight in the server's time
        /// zone, so it is emitted as a date to leave its interpretation to the Snowflake session.
        /// </summary>
        private static string FormatConstant(ConstantNode constantNode)
        {
            var value = constantNode.Value;
            switch (value)
            {
                case null:
                    return "NULL";
                case string stringValue:
                    return $"'{EscapeStringLiteral(stringValue)}'";
                case Guid guidValue:
                    return $"'{FormatGuid(guidValue, constantNode.LiteralText)}'";
                case DateTimeOffset dateValue when IsDateOnlyLiteral(constantNode.LiteralText):
                    return $"'{dateValue.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}'";
                case DateTimeOffset dateTimeOffsetValue:
                    return $"'{dateTimeOffsetValue.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz", CultureInfo.InvariantCulture)}'";
                case Date dateValue:
                    return string.Format(CultureInfo.InvariantCulture, "'{0:D4}-{1:D2}-{2:D2}'", dateValue.Year, dateValue.Month, dateValue.Day);
                case TimeOfDay timeOfDayValue:
                    return $"'{((TimeSpan)timeOfDayValue).ToString(@"hh\:mm\:ss\.FFFFFFF", CultureInfo.InvariantCulture).TrimEnd('.')}'";
                default:
                    return value.ToString();
            }
        }

        /// <summary>
        /// GUIDs are stored in Snowflake as text and compared case-sensitively, so the caller's casing is kept.
        /// The literal is only emitted once it has been validated as a canonical GUID.
        /// </summary>
        private static string FormatGuid(Guid value, string literalText)
        {
            return Guid.TryParseExact(literalText, "D", out var parsed) && parsed == value
                ? literalText
                : value.ToString("D");
        }

        private static bool IsDateOnlyLiteral(string literalText)
        {
            return !string.IsNullOrEmpty(literalText) && literalText.IndexOf("T", StringComparison.OrdinalIgnoreCase) < 0;
        }

        /// <summary>
        /// Escapes a string so it can be safely embedded inside a single-quoted Snowflake string literal.
        /// Backslashes are escaped first (Snowflake honours backslash escape sequences) and then single quotes.
        /// </summary>
        private static string EscapeStringLiteral(string value)
        {
            return value.Replace("\\", "\\\\").Replace("'", "''");
        }

        private string ParseBinaryOperator(BinaryOperatorNode binaryOperatorNode)
        {
            var left = ParseExpression(binaryOperatorNode.Left);
            var right = ParseExpression(binaryOperatorNode.Right);

            switch (binaryOperatorNode.OperatorKind)
            {
                case BinaryOperatorKind.Equal:
                    return $"{left} = {right}";
                case BinaryOperatorKind.NotEqual:
                    return $"{left} <> {right}";
                case BinaryOperatorKind.And:
                    return $"({left}) AND ({right})";
                case BinaryOperatorKind.Or:
                    return $"({left}) OR ({right})";
                case BinaryOperatorKind.GreaterThan:
                    return $"{left} > {right}";
                case BinaryOperatorKind.GreaterThanOrEqual:
                    return $"{left} >= {right}";
                case BinaryOperatorKind.LessThan:
                    return $"{left} < {right}";
                case BinaryOperatorKind.LessThanOrEqual:
                    return $"{left} <= {right}";
                default:
                    throw new NotSupportedException($"Unsupported binary operator: {binaryOperatorNode.OperatorKind}");
            }
        }

        private string ParseFunctionCall(SingleValueFunctionCallNode functionCallNode)
        {
            var likeOperator = useCaseInsensitiveFilters ? "ILIKE" : "LIKE";

            if (functionCallNode.Name.Equals("contains", StringComparison.OrdinalIgnoreCase))
            {
                return ParseLikeFunction(functionCallNode, likeOperator, "Contains", prefix: "%", suffix: "%");
            }
            else if (functionCallNode.Name.Equals("startswith", StringComparison.OrdinalIgnoreCase))
            {
                return ParseLikeFunction(functionCallNode, likeOperator, "StartsWith", prefix: string.Empty, suffix: "%");
            }
            else if (functionCallNode.Name.Equals("endswith", StringComparison.OrdinalIgnoreCase))
            {
                return ParseLikeFunction(functionCallNode, likeOperator, "EndsWith", prefix: "%", suffix: string.Empty);
            }
            else if (functionCallNode.Name.Equals("tolower", StringComparison.OrdinalIgnoreCase))
            {
                var arguments = functionCallNode.Parameters.ToList();
                if (arguments.Count != 1)
                    throw new InvalidOperationException("tolower function requires exactly one argument");

                var operand = ParseExpression(arguments[0] as SingleValueNode);
                return $"LOWER({operand})";
            }
            else if (functionCallNode.Name.Equals("toupper", StringComparison.OrdinalIgnoreCase))
            {
                var arguments = functionCallNode.Parameters.ToList();
                if (arguments.Count != 1)
                    throw new InvalidOperationException("toupper function requires exactly one argument");

                var operand = ParseExpression(arguments[0] as SingleValueNode);
                return $"UPPER({operand})";
            }

            throw new NotSupportedException($"Unsupported function: {functionCallNode.Name}");
        }

        private string ParseLikeFunction(
            SingleValueFunctionCallNode functionCallNode,
            string likeOperator,
            string displayName,
            string prefix,
            string suffix)
        {
            var arguments = functionCallNode.Parameters.ToList();
            if (arguments.Count < 2)
            {
                throw new InvalidOperationException($"{displayName} function requires two arguments");
            }

            var property = ParseExpression(arguments[0] as SingleValueNode);
            var searchTerm = GetSearchTermLiteral(arguments[1] as SingleValueNode, displayName);

            return $"{property} {likeOperator} '{prefix}{EscapeStringLiteral(searchTerm)}{suffix}'";
        }

        /// <summary>
        /// The search term is embedded inside a single SQL string literal together with the LIKE wildcards,
        /// so it must be a string constant. Rendering any other expression (e.g. a nested tolower/toupper call)
        /// would splice raw SQL into that literal and let caller-supplied text escape it.
        /// </summary>
        private static string GetSearchTermLiteral(SingleValueNode node, string displayName)
        {
            while (node is ConvertNode convertNode)
            {
                node = convertNode.Source;
            }

            if (node is ConstantNode constantNode && constantNode.Value is string stringValue)
            {
                return stringValue;
            }

            throw new ArgumentException($"{displayName} function only supports a string literal as its search argument.");
        }

        private string ParseUnaryOperator(UnaryOperatorNode unaryOperatorNode)
        {
            var operand = ParseExpression(unaryOperatorNode.Operand);

            switch (unaryOperatorNode.OperatorKind)
            {
                case UnaryOperatorKind.Not:
                    return $"NOT ({operand})";
                // Handle other unary operators if needed
                default:
                    throw new NotSupportedException($"Unsupported unary operator: {unaryOperatorNode.OperatorKind}");
            }
        }
    }
}
