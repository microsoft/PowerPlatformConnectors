using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PowerPlatform.Connectors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        var pathInfo = ExtractPathInfo();
        
        try
        {
            // Handle main operations
            if (pathInfo.Operation.Equals("evaluate", StringComparison.OrdinalIgnoreCase))
            {
                return await EvaluateFormula().ConfigureAwait(false);
            }
            else if (pathInfo.Operation.Equals("parse", StringComparison.OrdinalIgnoreCase))
            {
                return await ParseFormula().ConfigureAwait(false);
            }
            else if (pathInfo.Operation.Equals("validate", StringComparison.OrdinalIgnoreCase))
            {
                return await ValidateFormula().ConfigureAwait(false);
            }
            // Handle function categories
            else if (pathInfo.Category == "functions")
            {
                return await ExecuteFunction(pathInfo).ConfigureAwait(false);
            }
            // Handle type operations
            else if (pathInfo.Category == "types")
            {
                return await ExecuteTypeOperation(pathInfo).ConfigureAwait(false);
            }
            else
            {
                return CreateErrorResponse($"Unknown operation: {pathInfo.Operation}", HttpStatusCode.NotFound);
            }
        }
        catch (Exception ex)
        {
            this.Context.Logger?.LogError(ex, $"Error executing operation: {pathInfo.Operation}");
            return CreateErrorResponse("Internal server error", HttpStatusCode.InternalServerError);
        }
    }

    private async Task<HttpResponseMessage> EvaluateFormula()
    {
        try
        {
            var requestBody = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var requestData = JObject.Parse(requestBody);
            var formula = requestData["formula"]?.ToString();

            if (string.IsNullOrEmpty(formula))
                return CreateErrorResponse("Formula is required", HttpStatusCode.BadRequest);

            var result = EvaluateSimpleFormula(formula);

            var response = new JObject();
            response["result"] = JToken.FromObject(new { value = result, type = "Text", isError = false });
            response["formula"] = JToken.FromObject(formula);
            response["success"] = JToken.FromObject(true);

            return CreateSuccessResponse(response);
        }
        catch (Exception ex)
        {
            this.Context.Logger?.LogError(ex, "Error evaluating formula");
            return CreateErrorResponse("Evaluation error", HttpStatusCode.InternalServerError);
        }
    }

    private string EvaluateSimpleFormula(string formula)
    {
        if (formula.Contains("+"))
        {
            var parts = formula.Split('+');
            if (parts.Length == 2 && double.TryParse(parts[0].Trim(), out double num1) && double.TryParse(parts[1].Trim(), out double num2))
            {
                return (num1 + num2).ToString();
            }
        }
        return $"Evaluated: {formula}";
    }

    private async Task<HttpResponseMessage> ParseFormula()
    {
        try
        {
            var requestBody = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var requestData = JObject.Parse(requestBody);
            var formula = requestData["formula"]?.ToString();

            if (string.IsNullOrEmpty(formula))
                return CreateErrorResponse("Formula is required", HttpStatusCode.BadRequest);

            var tokens = TokenizeFormula(formula);
            var response = new JObject();
            response["isValid"] = JToken.FromObject(true);
            response["tokens"] = JArray.FromObject(tokens);
            response["success"] = JToken.FromObject(true);

            return CreateSuccessResponse(response);
        }
        catch (Exception ex)
        {
            this.Context.Logger?.LogError(ex, "Error parsing formula");
            return CreateErrorResponse("Parse error", HttpStatusCode.InternalServerError);
        }
    }

    private async Task<HttpResponseMessage> ValidateFormula()
    {
        try
        {
            var requestBody = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var requestData = JObject.Parse(requestBody);
            var formula = requestData["formula"]?.ToString();

            if (string.IsNullOrEmpty(formula))
                return CreateErrorResponse("Formula is required", HttpStatusCode.BadRequest);

            var isValid = !string.IsNullOrWhiteSpace(formula) && formula.Length > 0;
            
            var response = new JObject();
            response["isValid"] = JToken.FromObject(isValid);
            response["formula"] = JToken.FromObject(formula);
            response["success"] = JToken.FromObject(true);

            return CreateSuccessResponse(response);
        }
        catch (Exception ex)
        {
            this.Context.Logger?.LogError(ex, "Error validating formula");
            return CreateErrorResponse("Validation error", HttpStatusCode.InternalServerError);
        }
    }

    private async Task<HttpResponseMessage> ExecuteFunction(PathInfo pathInfo)
    {
        try
        {
            var requestBody = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var requestData = JObject.Parse(requestBody);
            
            FunctionResult result;
            switch (pathInfo.FunctionType.ToLower())
            {
                case "math":
                    result = ExecuteMathFunction(pathInfo.Operation, requestData);
                    break;
                case "text":
                    result = ExecuteTextFunction(pathInfo.Operation, requestData);
                    break;
                case "logical":
                    result = ExecuteLogicalFunction(pathInfo.Operation, requestData);
                    break;
                case "datetime":
                    result = ExecuteDateTimeFunction(pathInfo.Operation, requestData);
                    break;
                case "conversion":
                    result = ExecuteConversionFunction(pathInfo.Operation, requestData);
                    break;
                case "table":
                    result = ExecuteTableFunction(pathInfo.Operation, requestData);
                    break;
                case "utility":
                    result = ExecuteUtilityFunction(pathInfo.Operation, requestData);
                    break;
                case "json":
                    result = ExecuteJSONFunction(pathInfo.Operation, requestData);
                    break;
                case "color":
                    result = ExecuteColorFunction(pathInfo.Operation, requestData);
                    break;
                case "encoding":
                    result = ExecuteEncodingFunction(pathInfo.Operation, requestData);
                    break;
                case "statistics":
                    result = ExecuteStatisticsFunction(pathInfo.Operation, requestData);
                    break;
                default:
                    result = CreateFunctionResult("Function not implemented", false);
                    break;
            }

            // Create PowerFxValue structure as expected by OpenAPI schema
            var powerFxValue = new JObject();
            powerFxValue["value"] = JToken.FromObject(result.Value);
            powerFxValue["type"] = JToken.FromObject(result.Type);
            powerFxValue["isError"] = JToken.FromObject(!result.Success);
            if (!result.Success)
            {
                powerFxValue["errorMessage"] = JToken.FromObject(result.Value.ToString());
            }

            var response = new JObject();
            response["result"] = powerFxValue;
            response["executionTimeMs"] = JToken.FromObject(0); // Add execution time if needed

            return CreateSuccessResponse(response);
        }
        catch (Exception ex)
        {
            this.Context.Logger?.LogError(ex, $"Error executing function: {pathInfo.FunctionType}/{pathInfo.Operation}");
            return CreateErrorResponse($"Function execution error: {ex.Message}", HttpStatusCode.InternalServerError);
        }
    }

    private async Task<HttpResponseMessage> ExecuteTypeOperation(PathInfo pathInfo)
    {
        try
        {
            var requestBody = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
            var requestData = JObject.Parse(requestBody);

            FunctionResult result;
            switch (pathInfo.Operation.ToLower())
            {
                case "check":
                    result = ExecuteTypeCheck(requestData);
                    break;
                case "coerce":
                    result = ExecuteTypeCoerce(requestData);
                    break;
                default:
                    result = CreateFunctionResult("Type operation not implemented", false);
                    break;
            }

            // Create PowerFxValue structure as expected by OpenAPI schema
            var powerFxValue = new JObject();
            powerFxValue["value"] = JToken.FromObject(result.Value);
            powerFxValue["type"] = JToken.FromObject(result.Type);
            powerFxValue["isError"] = JToken.FromObject(!result.Success);
            if (!result.Success)
            {
                powerFxValue["errorMessage"] = JToken.FromObject(result.Value.ToString());
            }

            var response = new JObject();
            response["result"] = powerFxValue;
            response["executionTimeMs"] = JToken.FromObject(0);

            return CreateSuccessResponse(response);
        }
        catch (Exception ex)
        {
            this.Context.Logger?.LogError(ex, $"Error executing type operation: {pathInfo.Operation}");
            return CreateErrorResponse($"Type operation error: {ex.Message}", HttpStatusCode.InternalServerError);
        }
    }

    private FunctionResult ExecuteMathFunction(string operation, JObject requestData)
    {
        try
        {
            var args = requestData["arguments"]?.ToObject<double[]>() ?? new double[0];
            
            double result = 0;
            switch (operation.ToLower())
            {
                case "abs":
                    result = args.Length >= 1 ? Math.Abs(args[0]) : 0;
                    break;
                case "power":
                    result = args.Length >= 2 ? Math.Pow(args[0], args[1]) : 0;
                    break;
                case "sqrt":
                    result = args.Length >= 1 ? Math.Sqrt(args[0]) : 0;
                    break;
                case "mod":
                    result = args.Length >= 2 ? args[0] % args[1] : 0;
                    break;
                case "round":
                    result = args.Length >= 1 ? Math.Round(args[0]) : 0;
                    break;
                case "sin":
                    result = args.Length >= 1 ? Math.Sin(args[0]) : 0;
                    break;
                case "cos":
                    result = args.Length >= 1 ? Math.Cos(args[0]) : 0;
                    break;
                case "tan":
                    result = args.Length >= 1 ? Math.Tan(args[0]) : 0;
                    break;
                case "ln":
                    result = args.Length >= 1 ? Math.Log(args[0]) : 0;
                    break;
                case "log":
                    result = args.Length >= 1 ? Math.Log10(args[0]) : 0;
                    break;
                case "exp":
                    result = args.Length >= 1 ? Math.Exp(args[0]) : 0;
                    break;
                case "pi":
                    result = Math.PI;
                    break;
                case "int":
                    result = args.Length >= 1 ? Math.Truncate(args[0]) : 0;
                    break;
                case "roundup":
                    result = args.Length >= 1 ? Math.Ceiling(args[0]) : 0;
                    break;
                case "rounddown":
                    result = args.Length >= 1 ? Math.Floor(args[0]) : 0;
                    break;
                case "trunc":
                    result = args.Length >= 1 ? Math.Truncate(args[0]) : 0;
                    break;
                case "asin":
                    result = args.Length >= 1 ? Math.Asin(args[0]) : 0;
                    break;
                case "acos":
                    result = args.Length >= 1 ? Math.Acos(args[0]) : 0;
                    break;
                case "atan":
                    result = args.Length >= 1 ? Math.Atan(args[0]) : 0;
                    break;
                case "atan2":
                    result = args.Length >= 2 ? Math.Atan2(args[0], args[1]) : 0;
                    break;
                case "ceiling":
                    result = args.Length >= 1 ? Math.Ceiling(args[0]) : 0;
                    break;
                case "floor":
                    result = args.Length >= 1 ? Math.Floor(args[0]) : 0;
                    break;
                case "max":
                    result = args.Length >= 1 ? args.Max() : 0;
                    break;
                case "min":
                    result = args.Length >= 1 ? args.Min() : 0;
                    break;
                case "sum":
                    result = args.Sum();
                    break;
                case "average":
                    result = args.Length > 0 ? args.Average() : 0;
                    break;
                default:
                    throw new NotSupportedException($"Math operation '{operation}' is not supported");
            }

            return CreateFunctionResult(result, true);
        }
        catch (Exception ex)
        {
            return CreateFunctionResult($"Math error: {ex.Message}", false);
        }
    }

    private FunctionResult ExecuteTextFunction(string operation, JObject requestData)
    {
        try
        {
            var args = requestData["arguments"]?.ToObject<string[]>() ?? new string[0];
            
            object result = "";
            switch (operation.ToLower())
            {
                case "upper":
                    result = args.Length >= 1 ? args[0]?.ToUpper() ?? "" : "";
                    break;
                case "lower":
                    result = args.Length >= 1 ? args[0]?.ToLower() ?? "" : "";
                    break;
                case "len":
                    result = args.Length >= 1 ? args[0]?.Length ?? 0 : 0;
                    break;
                case "trim":
                    result = args.Length >= 1 ? args[0]?.Trim() ?? "" : "";
                    break;
                case "left":
                    if (args.Length >= 2 && int.TryParse(args[1], out int leftCount) && args[0] != null)
                        result = args[0].Length >= leftCount ? args[0].Substring(0, leftCount) : args[0];
                    else
                        result = "";
                    break;
                case "right":
                    if (args.Length >= 2 && int.TryParse(args[1], out int rightCount) && args[0] != null)
                        result = args[0].Length >= rightCount ? args[0].Substring(args[0].Length - rightCount) : args[0];
                    else
                        result = "";
                    break;
                case "concatenate":
                    result = string.Join("", args);
                    break;
                default:
                    result = $"Text operation '{operation}' not implemented";
                    break;
            }

            return CreateFunctionResult(result, true);
        }
        catch (Exception ex)
        {
            return CreateFunctionResult($"Text error: {ex.Message}", false);
        }
    }

    private FunctionResult ExecuteLogicalFunction(string operation, JObject requestData)
    {
        try
        {
            var args = requestData["arguments"]?.ToObject<bool[]>() ?? new bool[0];
            
            bool result = false;
            switch (operation.ToLower())
            {
                case "and":
                    result = args.Length > 0 ? args.All(x => x) : true;
                    break;
                case "or":
                    result = args.Length > 0 ? args.Any(x => x) : false;
                    break;
                case "not":
                    result = args.Length >= 1 ? !args[0] : true;
                    break;
                default:
                    throw new NotSupportedException($"Logical operation '{operation}' is not supported");
            }

            return CreateFunctionResult(result, true);
        }
        catch (Exception ex)
        {
            return CreateFunctionResult($"Logical error: {ex.Message}", false);
        }
    }

    private FunctionResult ExecuteDateTimeFunction(string operation, JObject requestData)
    {
        try
        {
            object result = null;
            switch (operation.ToLower())
            {
                case "now":
                    result = DateTime.Now;
                    break;
                case "today":
                    result = DateTime.Today;
                    break;
                case "utcnow":
                    result = DateTime.UtcNow;
                    break;
                case "utctoday":
                    result = DateTime.UtcNow.Date;
                    break;
                default:
                    result = $"DateTime operation '{operation}' not implemented";
                    break;
            }

            return CreateFunctionResult(result, true);
        }
        catch (Exception ex)
        {
            return CreateFunctionResult($"DateTime error: {ex.Message}", false);
        }
    }

    private FunctionResult ExecuteConversionFunction(string operation, JObject requestData)
    {
        return CreateFunctionResult($"Conversion operation '{operation}' not implemented", false);
    }

    private FunctionResult ExecuteTableFunction(string operation, JObject requestData)
    {
        return CreateFunctionResult($"Table operation '{operation}' not implemented", false);
    }

    private FunctionResult ExecuteUtilityFunction(string operation, JObject requestData)
    {
        return CreateFunctionResult($"Utility operation '{operation}' not implemented", false);
    }

    private FunctionResult ExecuteJSONFunction(string operation, JObject requestData)
    {
        return CreateFunctionResult($"JSON operation '{operation}' not implemented", false);
    }

    private FunctionResult ExecuteColorFunction(string operation, JObject requestData)
    {
        return CreateFunctionResult($"Color operation '{operation}' not implemented", false);
    }

    private FunctionResult ExecuteEncodingFunction(string operation, JObject requestData)
    {
        return CreateFunctionResult($"Encoding operation '{operation}' not implemented", false);
    }

    private FunctionResult ExecuteStatisticsFunction(string operation, JObject requestData)
    {
        return CreateFunctionResult($"Statistics operation '{operation}' not implemented", false);
    }

    private FunctionResult ExecuteTypeCheck(JObject requestData)
    {
        return CreateFunctionResult("Type check not implemented", false);
    }

    private FunctionResult ExecuteTypeCoerce(JObject requestData)
    {
        return CreateFunctionResult("Type coerce not implemented", false);
    }

    private FunctionResult CreateFunctionResult(object value, bool success)
    {
        return new FunctionResult 
        { 
            Value = value, 
            Success = success,
            Type = GetPowerFxType(value)
        };
    }

    private string GetPowerFxType(object value)
    {
        if (value == null)
            return "Blank";
        
        if (value is bool)
            return "Boolean";
            
        if (value is int || value is long || value is float || value is double || value is decimal)
            return "Number";
            
        if (value is string)
            return "Text";
            
        if (value is DateTime)
            return "DateTime";
            
        return "Text";
    }

    private List<Dictionary<string, object>> TokenizeFormula(string formula)
    {
        var tokens = new List<Dictionary<string, object>>();
        tokens.Add(new Dictionary<string, object>
        {
            ["type"] = "Formula",
            ["value"] = formula,
            ["start"] = 0,
            ["end"] = formula.Length
        });
        return tokens;
    }

    private PathInfo ExtractPathInfo()
    {
        var segments = this.Context.Request.RequestUri.Segments
            .Where(s => !string.IsNullOrWhiteSpace(s) && s != "/")
            .Select(s => s.TrimEnd('/'))
            .ToArray();

        if (segments.Length == 0)
            return new PathInfo { Operation = "unknown", Category = "", FunctionType = "" };

        // Handle simple operations like /evaluate, /parse, /validate
        if (segments.Length == 1)
        {
            return new PathInfo 
            { 
                Operation = segments[0], 
                Category = "",
                FunctionType = ""
            };
        }

        // Handle /functions/math/power, /functions/text/upper, etc.
        if (segments.Length >= 3 && segments[0] == "functions")
        {
            return new PathInfo 
            { 
                Operation = segments[2],
                Category = segments[0],
                FunctionType = segments[1]
            };
        }

        // Handle /types/check, /types/coerce
        if (segments.Length >= 2 && segments[0] == "types")
        {
            return new PathInfo 
            { 
                Operation = segments[1],
                Category = segments[0],
                FunctionType = ""
            };
        }

        // Default case
        return new PathInfo 
        { 
            Operation = segments.LastOrDefault() ?? "unknown",
            Category = segments.Length > 1 ? segments[0] : "",
            FunctionType = segments.Length > 2 ? segments[1] : ""
        };
    }

    private class PathInfo
    {
        public string Operation { get; set; } = "";
        public string Category { get; set; } = "";
        public string FunctionType { get; set; } = "";
    }

    private class FunctionResult
    {
        public object Value { get; set; }
        public bool Success { get; set; }
        public string Type { get; set; } = "";
    }

    private HttpResponseMessage CreateSuccessResponse(JObject responseData)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = CreateJsonContent(responseData.ToString());
        return response;
    }

    private HttpResponseMessage CreateErrorResponse(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        var errorResponse = new JObject();
        var errorObj = new JObject();
        errorObj["code"] = JToken.FromObject(statusCode.ToString());
        errorObj["message"] = JToken.FromObject(errorMessage);
        errorResponse["error"] = errorObj;

        var response = new HttpResponseMessage(statusCode);
        response.Content = CreateJsonContent(errorResponse.ToString());
        return response;
    }

    private StringContent CreateJsonContent(string json)
    {
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}
