// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Utilities
{
    using System;
    using System.Linq;
    using Microsoft.OData.Core.UriParser.Semantic;
    using Microsoft.OData.Core.UriParser.TreeNodeKinds;

    /// <summary>
    /// OData to SQL Parser
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
                return propertyAccessNode.Property.Name;
            }
            // Handle open property access (dynamic properties)
            else if (expression is SingleValueOpenPropertyAccessNode openPropertyNode)
            {
                // For open properties, we use the property name directly
                return openPropertyNode.Name;
            }
            else if (expression is ConstantNode constantNode)
            {
                if (constantNode.Value == null)
                    return "NULL";
                else if (constantNode.Value is string)
                    return $"'{constantNode.Value}'";
                else
                    return constantNode.Value.ToString();
            }

            throw new NotSupportedException($"Unsupported expression type: {expression.GetType().Name}");
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
