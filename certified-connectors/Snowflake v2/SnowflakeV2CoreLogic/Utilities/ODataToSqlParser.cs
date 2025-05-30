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
    /// 
    /// </summary>
    public class ODataToSqlParser
    {
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
            if (functionCallNode.Name.Equals("contains", StringComparison.OrdinalIgnoreCase))
            {
                var arguments = functionCallNode.Parameters.ToList();

                if (arguments.Count < 2)
                    throw new InvalidOperationException("Contains function requires two arguments");

                var property = ParseExpression(arguments[0] as SingleValueNode);
                var value = ParseExpression(arguments[1] as SingleValueNode);

                // Remove quotes from value if present
                if (value.StartsWith("'") && value.EndsWith("'"))
                {
                    value = value.Substring(1, value.Length - 2);
                }

                return $"{property} LIKE '%{value}%'";
            }
            else if (functionCallNode.Name.Equals("startswith", StringComparison.OrdinalIgnoreCase))
            {
                var arguments = functionCallNode.Parameters.ToList();
                if (arguments.Count < 2)
                    throw new InvalidOperationException("StartsWith function requires two arguments");

                var property = ParseExpression(arguments[0] as SingleValueNode);
                var value = ParseExpression(arguments[1] as SingleValueNode);

                if (value.StartsWith("'") && value.EndsWith("'"))
                    value = value.Substring(1, value.Length - 2);

                return $"{property} LIKE '{value}%'";
            }
            else if (functionCallNode.Name.Equals("endswith", StringComparison.OrdinalIgnoreCase))
            {
                var arguments = functionCallNode.Parameters.ToList();
                if (arguments.Count < 2)
                    throw new InvalidOperationException("EndsWith function requires two arguments");

                var property = ParseExpression(arguments[0] as SingleValueNode);
                var value = ParseExpression(arguments[1] as SingleValueNode);

                if (value.StartsWith("'") && value.EndsWith("'"))
                    value = value.Substring(1, value.Length - 2);

                return $"{property} LIKE '%{value}'";
            }

            throw new NotSupportedException($"Unsupported function: {functionCallNode.Name}");
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
