using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Enhanced error handling classes
public class ExecutionError
{
    public int LineNumber { get; set; }
    public string ErrorType { get; set; }
    public string Message { get; set; }
    public string Context { get; set; }
    public DateTime Timestamp { get; set; }
}

public class ExecutionStatistics
{
    public long ExecutionTimeMs { get; set; }
    public int LinesExecuted { get; set; }
    public long MemoryUsedBytes { get; set; }
    public int VariablesCreated { get; set; }
    public int FunctionsCalled { get; set; }
    public int ErrorsEncountered { get; set; }
    public int ArraysCreated { get; set; }
    public int MaxNestingLevel { get; set; }
}

public class ResourceLimits
{
    public int MaxVariables { get; set; } = 1000;
    public int MaxArraySize { get; set; } = 10000;
    public int MaxStringLength { get; set; } = 32000;
    public int MaxNestingLevel { get; set; } = 50;
    public int MaxExecutionTimeMs { get; set; } = 120000; // 2 minutes
    public long MaxMemoryBytes { get; set; } = 10485760; // 10MB
}

public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        try 
        {
            this.Context.Logger.LogInformation($"BASIC Connector operation: {this.Context.OperationId}");
            
            if (this.Context.OperationId == "ExecuteBasicCode")
            {
                return await HandleBasicExecution().ConfigureAwait(false);
            }
            else if (this.Context.OperationId == "ValidateBasicCode")
            {
                return await HandleBasicValidation().ConfigureAwait(false);
            }
            else if (this.Context.OperationId == "GetBasicFunctions")
            {
                return await HandleGetFunctions().ConfigureAwait(false);
            }

            return CreateErrorResponse(HttpStatusCode.BadRequest, 
                $"Unknown operation ID '{this.Context.OperationId}'");
        }
        catch (Exception ex)
        {
            this.Context.Logger.LogError(ex, 
                "Unexpected error in BASIC connector. CorrelationId: {CorrelationId}", 
                this.Context.CorrelationId);
            
            return CreateErrorResponse(HttpStatusCode.InternalServerError, 
                "An unexpected error occurred", ex.Message);
        }
    }

    private async Task<HttpResponseMessage> HandleBasicExecution()
    {
        try
        {
            var requestContent = await this.Context.Request.Content.ReadAsStringAsync();
            var request = JObject.Parse(requestContent);
            
            var basicCode = request["code"]?.ToString();
            if (string.IsNullOrEmpty(basicCode))
            {
                return CreateErrorResponse(HttpStatusCode.BadRequest, 
                    "BASIC code is required");
            }

            var maxExecutionTime = request["maxExecutionTime"]?.Value<int>() ?? 30;
            var enableDebug = request["enableDebug"]?.Value<bool>() ?? false;
            var caseSensitive = request["caseSensitive"]?.Value<bool>() ?? false;
            var enableGraphics = request["enableGraphics"]?.Value<bool>() ?? false;
            var enableFileIO = request["enableFileIO"]?.Value<bool>() ?? false;
            var enableMemory = request["enableMemory"]?.Value<bool>() ?? false;
            var files = request["files"]?.ToObject<Dictionary<string, string>>() ?? new Dictionary<string, string>();
            var initialVariables = request["variables"]?.ToObject<Dictionary<string, object>>() 
                ?? new Dictionary<string, object>();

            // Execute BASIC code with timeout
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(Math.Min(maxExecutionTime, 120))))
            {
                var interpreter = new BasicInterpreter(this.Context.Logger, caseSensitive, enableGraphics, enableFileIO, enableMemory);
                interpreter.LoadFiles(files);
                var result = await interpreter.ExecuteAsync(basicCode, initialVariables, enableDebug, cts.Token);
                
                var response = new JObject
                {
                    ["success"] = result.Success,
                    ["output"] = JArray.FromObject(result.Output),
                    ["variables"] = JObject.FromObject(result.Variables),
                    ["executionTime"] = result.ExecutionTime.TotalSeconds,
                    ["linesExecuted"] = result.LinesExecuted
                };

                if (enableDebug && result.DebugTrace != null)
                {
                    response["debugTrace"] = JArray.FromObject(result.DebugTrace);
                }

                if (enableGraphics && result.GraphicsOutput != null && result.GraphicsOutput.Count > 0)
                {
                    response["graphics"] = JArray.FromObject(result.GraphicsOutput);
                }

                if (enableFileIO && result.Files != null && result.Files.Count > 0)
                {
                    response["files"] = JObject.FromObject(result.Files);
                }

                if (enableMemory && result.Memory != null)
                {
                    response["memory"] = JObject.FromObject(result.Memory);
                }

                if (!result.Success && result.Error != null)
                {
                    response["error"] = JObject.FromObject(result.Error);
                }

                return CreateJsonResponse(HttpStatusCode.OK, response.ToString());
            }
        }
        catch (OperationCanceledException)
        {
            return CreateErrorResponse(HttpStatusCode.RequestTimeout, 
                "BASIC program execution timed out");
        }
        catch (BasicSyntaxException ex)
        {
            return CreateErrorResponse(HttpStatusCode.BadRequest, 
                "BASIC syntax error", ex.Message);
        }
        catch (BasicRuntimeException ex)
        {
            return CreateErrorResponse(HttpStatusCode.BadRequest, 
                "BASIC runtime error", ex.Message);
        }
    }

    private async Task<HttpResponseMessage> HandleBasicValidation()
    {
        try
        {
            var requestContent = await this.Context.Request.Content.ReadAsStringAsync();
            var request = JObject.Parse(requestContent);
            
            var basicCode = request["code"]?.ToString();
            if (string.IsNullOrEmpty(basicCode))
            {
                return CreateErrorResponse(HttpStatusCode.BadRequest, "BASIC code is required");
            }

            var interpreter = new BasicInterpreter(this.Context.Logger);
            var validationResult = interpreter.ValidateSyntax(basicCode);
            
            var response = new JObject
            {
                ["isValid"] = validationResult.IsValid,
                ["errors"] = JArray.FromObject(validationResult.Errors),
                ["warnings"] = JArray.FromObject(validationResult.Warnings)
            };

            return CreateJsonResponse(HttpStatusCode.OK, response.ToString());
        }
        catch (Exception ex)
        {
            this.Context.Logger.LogError(ex, "Error validating BASIC code");
            return CreateErrorResponse(HttpStatusCode.InternalServerError, "Validation error", ex.Message);
        }
    }

    private async Task<HttpResponseMessage> HandleGetFunctions()
    {
        try
        {
            var functions = BasicInterpreter.GetSupportedFunctions();
            var response = new JObject
            {
                ["functions"] = JArray.FromObject(functions)
            };

            return CreateJsonResponse(HttpStatusCode.OK, response.ToString());
        }
        catch (Exception ex)
        {
            this.Context.Logger.LogError(ex, "Error getting BASIC functions");
            return CreateErrorResponse(HttpStatusCode.InternalServerError, "Error retrieving functions", ex.Message);
        }
    }

    private HttpResponseMessage CreateErrorResponse(HttpStatusCode statusCode, string error, string details = null)
    {
        var errorObj = new JObject
        {
            ["error"] = error,
            ["timestamp"] = DateTime.UtcNow.ToString("O")
        };

        if (!string.IsNullOrEmpty(details))
        {
            errorObj["details"] = details;
        }

        var response = new HttpResponseMessage(statusCode);
        response.Content = CreateJsonContent(errorObj.ToString());
        return response;
    }

    private HttpResponseMessage CreateJsonResponse(HttpStatusCode statusCode, string content)
    {
        var response = new HttpResponseMessage(statusCode);
        response.Content = CreateJsonContent(content);
        return response;
    }
}

// Exception classes
public class BasicSyntaxException : Exception
{
    public int LineNumber { get; }
    public int Position { get; }
    
    public BasicSyntaxException(string message, int lineNumber = 0, int position = 0) : base(message)
    {
        LineNumber = lineNumber;
        Position = position;
    }
}

public class BasicRuntimeException : Exception
{
    public int LineNumber { get; }
    
    public BasicRuntimeException(string message, int lineNumber = 0) : base(message)
    {
        LineNumber = lineNumber;
    }
}

// Data structures
public class BasicExecutionResult
{
    public bool Success { get; set; }
    public List<string> Output { get; set; } = new List<string>();
    public Dictionary<string, object> Variables { get; set; } = new Dictionary<string, object>();
    public TimeSpan ExecutionTime { get; set; }
    public int LinesExecuted { get; set; }
    public List<DebugTraceEntry> DebugTrace { get; set; }
    public List<GraphicsCommand> GraphicsOutput { get; set; } = new List<GraphicsCommand>();
    public Dictionary<string, string> Files { get; set; } = new Dictionary<string, string>();
    public VirtualMemory Memory { get; set; }
    public BasicError Error { get; set; }
}

public class BasicValidationResult
{
    public bool IsValid { get; set; }
    public List<SyntaxError> Errors { get; set; } = new List<SyntaxError>();
    public List<SyntaxWarning> Warnings { get; set; } = new List<SyntaxWarning>();
}

public class DebugTraceEntry
{
    public int LineNumber { get; set; }
    public string Instruction { get; set; }
    public DateTime Timestamp { get; set; }
    public Dictionary<string, object> Variables { get; set; }
}

public class BasicError
{
    public string Type { get; set; }
    public string Message { get; set; }
    public int LineNumber { get; set; }
    public int Position { get; set; }
}

public class SyntaxError
{
    public int LineNumber { get; set; }
    public int Position { get; set; }
    public string Message { get; set; }
    public string Severity { get; set; }
}

public class SyntaxWarning
{
    public int LineNumber { get; set; }
    public string Message { get; set; }
}

public class GraphicsCommand
{
    public string Command { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    public int LineNumber { get; set; }
    public string Color { get; set; } = "white";
    public int Thickness { get; set; } = 1;
    public string Style { get; set; } = "solid";
}

public class VirtualMemory
{
    public Dictionary<int, byte> Memory { get; set; } = new Dictionary<int, byte>();
    public List<MemoryOperation> Operations { get; set; } = new List<MemoryOperation>();
}

public class MemoryOperation
{
    public string Operation { get; set; }
    public int Address { get; set; }
    public byte? Value { get; set; }
    public int LineNumber { get; set; }
}

public class FileOperation
{
    public string Operation { get; set; }
    public string FileName { get; set; }
    public string Mode { get; set; }
    public string Data { get; set; }
    public int LineNumber { get; set; }
}

public class BasicFunction
{
    public string Name { get; set; }
    public string Syntax { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
}

// BASIC Interpreter Implementation
public class BasicInterpreter
{
    private readonly ILogger _logger;
    private readonly bool _caseSensitive;
    private readonly bool _enableGraphics;
    private readonly bool _enableFileIO;
    private readonly bool _enableMemory;
    private Dictionary<string, object> _variables;
    private Dictionary<string, Array> _arrays;
    private Dictionary<string, UserDefinedFunction> _userFunctions;
    private Dictionary<int, string> _program;
    private List<string> _output;
    private List<DebugTraceEntry> _debugTrace;
    private List<GraphicsCommand> _graphicsOutput;
    private Dictionary<string, string> _virtualFiles;
    private Dictionary<int, StreamWriter> _openFiles;
    private VirtualMemory _virtualMemory;
    private string _currentGraphicsColor;
    private bool _debugMode;
    private int _currentLine;
    private Stack<ForLoop> _forLoopStack;
    private Stack<int> _subroutineStack;
    private Random _random;
    private List<DataStatement> _dataStatements;
    private int _dataPointer;
    
    // Enhanced fields for improvements
    private ResourceLimits _resourceLimits;
    private List<ExecutionError> _errors;
    private ExecutionStatistics _stats;
    private Stopwatch _executionTimer;
    private int _nestingLevel;
    private string _basicVersion;
    private Queue<string> _inputQueue;
    private int _functionCallCount;
    private Dictionary<string, int> _arrayBounds;
    
    public BasicInterpreter(ILogger logger = null, bool caseSensitive = false, bool enableGraphics = false, bool enableFileIO = false, bool enableMemory = false)
    {
        _logger = logger;
        _caseSensitive = caseSensitive;
        _enableGraphics = enableGraphics;
        _enableFileIO = enableFileIO;
        _enableMemory = enableMemory;
        var comparer = caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
        _variables = new Dictionary<string, object>(comparer);
        _arrays = new Dictionary<string, Array>(comparer);
        _userFunctions = new Dictionary<string, UserDefinedFunction>(comparer);
        _program = new Dictionary<int, string>();
        _output = new List<string>();
        _debugTrace = new List<DebugTraceEntry>();
        _graphicsOutput = new List<GraphicsCommand>();
        _virtualFiles = new Dictionary<string, string>();
        _openFiles = new Dictionary<int, StreamWriter>();
        _virtualMemory = new VirtualMemory();
        _currentGraphicsColor = "white";
        _forLoopStack = new Stack<ForLoop>();
        _subroutineStack = new Stack<int>();
        _random = new Random();
        _dataStatements = new List<DataStatement>();
        _dataPointer = 0;
        
        // Initialize enhanced fields
        _resourceLimits = new ResourceLimits();
        _errors = new List<ExecutionError>();
        _stats = new ExecutionStatistics();
        _executionTimer = new Stopwatch();
        _nestingLevel = 0;
        _basicVersion = "microsoft6502";
        _inputQueue = new Queue<string>();
        _functionCallCount = 0;
        _arrayBounds = new Dictionary<string, int>();
    }
    
    // Enhanced properties
    public bool CaseSensitive { get; set; }
    public bool EnableGraphics { get; set; }
    public bool EnableFileIO { get; set; }
    public bool EnableMemory { get; set; }
    public string BasicVersion { get; set; }
    public ResourceLimits ResourceLimits => _resourceLimits;
    public List<ExecutionError> Errors => _errors;
    public ExecutionStatistics Statistics => _stats;

    public void LoadFiles(Dictionary<string, string> files)
    {
        if (files != null)
        {
            foreach (var file in files)
            {
                _virtualFiles[file.Key] = file.Value;
            }
        }
    }
    
    // Enhanced error handling methods
    private void ThrowError(string message, int lineNumber = -1, string errorType = "Runtime Error")
    {
        var error = new ExecutionError
        {
            LineNumber = lineNumber == -1 ? _currentLine : lineNumber,
            ErrorType = errorType,
            Message = message,
            Context = lineNumber != -1 && _program.ContainsKey(lineNumber) ? _program[lineNumber] : "",
            Timestamp = DateTime.UtcNow
        };
        
        _errors.Add(error);
        _stats.ErrorsEncountered++;
        
        var errorMessage = lineNumber == -1 ? message : $"Line {lineNumber}: {message}";
        throw new Exception(errorMessage);
    }
    
    private void LogError(string message, int lineNumber = -1, string errorType = "Warning")
    {
        var error = new ExecutionError
        {
            LineNumber = lineNumber == -1 ? _currentLine : lineNumber,
            ErrorType = errorType,
            Message = message,
            Context = lineNumber != -1 && _program.ContainsKey(lineNumber) ? _program[lineNumber] : "",
            Timestamp = DateTime.UtcNow
        };
        
        _errors.Add(error);
        _logger?.LogWarning($"{errorType} at line {error.LineNumber}: {message}");
    }
    
    // Input validation methods
    private void ValidateInput(string code)
    {
        if (string.IsNullOrEmpty(code))
            ThrowError("Code cannot be empty", -1, "Validation Error");
            
        if (code.Length > _resourceLimits.MaxStringLength)
            ThrowError($"Code too long. Maximum {_resourceLimits.MaxStringLength} characters allowed", -1, "Validation Error");
            
        // Check for potentially dangerous patterns
        var dangerousPatterns = new[] { "while true", "goto 10", "for i=1 to 999999" };
        var lowerCode = code.ToLower();
        
        foreach (var pattern in dangerousPatterns)
        {
            if (lowerCode.Contains(pattern))
            {
                LogError($"Potentially infinite loop detected: {pattern}", -1, "Warning");
            }
        }
    }
    
    // Resource management methods
    private void CheckResourceLimits()
    {
        if (_variables.Count > _resourceLimits.MaxVariables)
            ThrowError($"Too many variables. Maximum {_resourceLimits.MaxVariables} allowed", _currentLine, "Resource Error");
            
        if (_nestingLevel > _resourceLimits.MaxNestingLevel)
            ThrowError($"Nesting too deep. Maximum {_resourceLimits.MaxNestingLevel} levels allowed", _currentLine, "Resource Error");
            
        if (_executionTimer.ElapsedMilliseconds > _resourceLimits.MaxExecutionTimeMs)
            ThrowError("Execution timeout exceeded", _currentLine, "Timeout Error");
    }
    
    // Enhanced input handling
    public void SetInputValues(List<string> inputValues)
    {
        _inputQueue.Clear();
        if (inputValues != null)
        {
            foreach (var value in inputValues)
            {
                _inputQueue.Enqueue(value);
            }
        }
    }
    
    public void SetVirtualFiles(Dictionary<string, string> files)
    {
        LoadFiles(files);
    }
    
    // Program analysis and introspection methods
    public List<string> GetVariableList()
    {
        return _variables.Keys.ToList();
    }
    
    public List<string> GetFunctionList()
    {
        return _userFunctions.Keys.ToList();
    }
    
    public Dictionary<int, string> GetLineMap()
    {
        return new Dictionary<int, string>(_program);
    }
    
    public List<string> ValidateSyntaxInternal(string code = null)
    {
        var errors = new List<string>();
        var codeToValidate = code ?? string.Join("\n", _program.OrderBy(p => p.Key).Select(p => $"{p.Key} {p.Value}"));
        
        try
        {
            ValidateInput(codeToValidate);
        }
        catch (Exception ex)
        {
            errors.Add(ex.Message);
        }
        
        return errors;
    }
    
    public Dictionary<string, object> GetExecutionSummary()
    {
        return new Dictionary<string, object>
        {
            ["TotalVariables"] = _variables.Count,
            ["TotalArrays"] = _arrays.Count,
            ["TotalUserFunctions"] = _userFunctions.Count,
            ["TotalProgramLines"] = _program.Count,
            ["ExecutionErrors"] = _errors.Count,
            ["MemoryUsage"] = GC.GetTotalMemory(false),
            ["LastExecutionTime"] = _stats.ExecutionTimeMs,
            ["MaxNestingLevel"] = _stats.MaxNestingLevel
        };
    }
    
    // Enhanced array functions
    public int GetArrayUpperBound(string arrayName, int dimension = 0)
    {
        arrayName = arrayName.ToUpper();
        if (_arrays.TryGetValue(arrayName, out var array))
        {
            return array.GetUpperBound(dimension);
        }
        ThrowError($"Array {arrayName} not found", _currentLine, "Array Error");
        return -1;
    }
    
    public int GetArrayLowerBound(string arrayName, int dimension = 0)
    {
        arrayName = arrayName.ToUpper();
        if (_arrays.TryGetValue(arrayName, out var array))
        {
            return array.GetLowerBound(dimension);
        }
        ThrowError($"Array {arrayName} not found", _currentLine, "Array Error");
        return -1;
    }

    public async Task<BasicExecutionResult> ExecuteAsync(string code, Dictionary<string, object> initialVariables, bool debugMode, CancellationToken cancellationToken)
    {
        var startTime = DateTime.UtcNow;
        var result = new BasicExecutionResult { Success = true };
        
        try
        {
            // Initialize enhanced tracking
            _executionTimer.Restart();
            ValidateInput(code);
            
            _debugMode = debugMode;
            _variables.Clear();
            _output.Clear();
            _debugTrace.Clear();
            _program.Clear();
            _forLoopStack.Clear();
            _subroutineStack.Clear();
            _errors.Clear();
            _nestingLevel = 0;
            _functionCallCount = 0;
            
            // Initialize statistics
            _stats.LinesExecuted = 0;
            _stats.VariablesCreated = 0;
            _stats.FunctionsCalled = 0;
            _stats.ErrorsEncountered = 0;
            _stats.ArraysCreated = 0;
            _stats.MaxNestingLevel = 0;
            
            // Initialize variables
            foreach (var kvp in initialVariables)
            {
                _variables[kvp.Key] = kvp.Value;
                _stats.VariablesCreated++;
            }
            
            // Parse program
            ParseProgram(code);
            
            // Execute program
            await ExecuteProgram(cancellationToken);
            
            _executionTimer.Stop();
            _stats.ExecutionTimeMs = _executionTimer.ElapsedMilliseconds;
            _stats.MemoryUsedBytes = GC.GetTotalMemory(false);
            
            result.Output = _output;
            result.Variables = _variables.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            result.LinesExecuted = _stats.LinesExecuted;
            result.DebugTrace = debugMode ? _debugTrace : null;
            result.GraphicsOutput = _enableGraphics ? _graphicsOutput : null;
            result.Files = _enableFileIO ? _virtualFiles : null;
            result.Memory = _enableMemory ? _virtualMemory : null;
        }
        catch (BasicSyntaxException ex)
        {
            result.Success = false;
            result.Error = new BasicError
            {
                Type = "SyntaxError",
                Message = ex.Message,
                LineNumber = ex.LineNumber,
                Position = ex.Position
            };
        }
        catch (BasicRuntimeException ex)
        {
            result.Success = false;
            result.Error = new BasicError
            {
                Type = "RuntimeError",
                Message = ex.Message,
                LineNumber = ex.LineNumber
            };
        }
        catch (OperationCanceledException)
        {
            result.Success = false;
            result.Error = new BasicError
            {
                Type = "TimeoutError",
                Message = "Program execution timed out"
            };
        }
        finally
        {
            result.ExecutionTime = DateTime.UtcNow - startTime;
            result.Output = _output;
            result.Variables = _variables.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            result.GraphicsOutput = _enableGraphics ? _graphicsOutput : null;
            result.Files = _enableFileIO ? _virtualFiles : null;
            result.Memory = _enableMemory ? _virtualMemory : null;
        }
        
        return result;
    }

    public BasicValidationResult ValidateSyntax(string code)
    {
        var result = new BasicValidationResult { IsValid = true };
        
        try
        {
            ParseProgram(code);
        }
        catch (BasicSyntaxException ex)
        {
            result.IsValid = false;
            result.Errors.Add(new SyntaxError
            {
                LineNumber = ex.LineNumber,
                Position = ex.Position,
                Message = ex.Message,
                Severity = "Error"
            });
        }
        
        return result;
    }

    private void ParseProgram(string code)
    {
        var lines = code.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        _dataStatements.Clear(); // Clear existing data statements
        
        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("REM"))
                continue;
                
            // Parse line number and statement
            var match = Regex.Match(trimmedLine, @"^(\d+)\s+(.+)$");
            if (match.Success)
            {
                var lineNumber = int.Parse(match.Groups[1].Value);
                var statement = match.Groups[2].Value.Trim();
                _program[lineNumber] = statement;
                
                // Pre-process DATA statements
                if (statement.ToUpper().StartsWith("DATA"))
                {
                    var dataMatch = Regex.Match(statement, @"DATA\s+(.+)", RegexOptions.IgnoreCase);
                    if (dataMatch.Success)
                    {
                        var valuesStr = dataMatch.Groups[1].Value;
                        var values = new List<object>();
                        
                        var items = valuesStr.Split(',');
                        foreach (var item in items)
                        {
                            var trimmed = item.Trim();
                            if (trimmed.StartsWith("\"") && trimmed.EndsWith("\""))
                            {
                                values.Add(trimmed.Substring(1, trimmed.Length - 2));
                            }
                            else if (double.TryParse(trimmed, out var number))
                            {
                                values.Add(number);
                            }
                            else
                            {
                                values.Add(trimmed);
                            }
                        }
                        
                        _dataStatements.Add(new DataStatement
                        {
                            LineNumber = lineNumber,
                            Values = values
                        });
                    }
                }
            }
            else if (!Regex.IsMatch(trimmedLine, @"^\d+$"))
            {
                throw new BasicSyntaxException($"Invalid line format: {trimmedLine}");
            }
        }
    }

    private async Task ExecuteProgram(CancellationToken cancellationToken)
    {
        var lineNumbers = _program.Keys.OrderBy(x => x).ToList();
        
        for (int i = 0; i < lineNumbers.Count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            CheckResourceLimits(); // Check resource limits on each iteration
            
            var lineNumber = lineNumbers[i];
            _currentLine = lineNumber;
            var statement = _program[lineNumber];
            
            _stats.LinesExecuted++;
            
            if (_debugMode)
            {
                _debugTrace.Add(new DebugTraceEntry
                {
                    LineNumber = lineNumber,
                    Instruction = statement,
                    Timestamp = DateTime.UtcNow,
                    Variables = _variables.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                });
            }
            
            var result = await ExecuteStatement(statement, cancellationToken);
            
            // Handle control flow
            if (result.Type == StatementResultType.Goto)
            {
                var targetIndex = lineNumbers.IndexOf(result.TargetLine);
                if (targetIndex >= 0)
                {
                    i = targetIndex - 1; // -1 because loop will increment
                }
                else
                {
                    throw new BasicRuntimeException($"Line {result.TargetLine} not found", lineNumber);
                }
            }
            else if (result.Type == StatementResultType.End)
            {
                break;
            }
            else if (result.Type == StatementResultType.Stop)
            {
                // STOP breaks execution but can be continued with CONT
                break;
            }
            
            // Simulate small delay for async operation
            if (i % 10 == 0)
            {
                await Task.Delay(1, cancellationToken);
            }
        }
    }

    private async Task<StatementResult> ExecuteStatement(string statement, CancellationToken cancellationToken)
    {
        var upperStatement = statement.ToUpper().Trim();
        
        if (upperStatement.StartsWith("PRINT"))
        {
            return ExecutePrint(statement);
        }
        else if (upperStatement.StartsWith("LET") || Regex.IsMatch(upperStatement, @"^[A-Z][A-Z0-9]*\s*="))
        {
            return ExecuteLet(statement);
        }
        else if (upperStatement.StartsWith("DIM"))
        {
            return ExecuteDim(statement);
        }
        else if (upperStatement.StartsWith("INPUT"))
        {
            return ExecuteInput(statement);
        }
        else if (upperStatement.StartsWith("READ"))
        {
            return ExecuteRead(statement);
        }
        else if (upperStatement.StartsWith("DATA"))
        {
            return ExecuteData(statement);
        }
        else if (upperStatement.StartsWith("RESTORE"))
        {
            return ExecuteRestore(statement);
        }
        else if (upperStatement.StartsWith("DEF"))
        {
            return ExecuteDef(statement);
        }
        else if (upperStatement.StartsWith("ON"))
        {
            return ExecuteOn(statement);
        }
        else if (upperStatement.StartsWith("FOR"))
        {
            return ExecuteFor(statement);
        }
        else if (upperStatement.StartsWith("NEXT"))
        {
            return ExecuteNext(statement);
        }
        else if (upperStatement.StartsWith("IF"))
        {
            return await ExecuteIf(statement);
        }
        else if (upperStatement.StartsWith("GOTO"))
        {
            return ExecuteGoto(statement);
        }
        else if (upperStatement.StartsWith("GOSUB"))
        {
            return ExecuteGosub(statement);
        }
        else if (upperStatement.StartsWith("RETURN"))
        {
            return ExecuteReturn();
        }
        else if (upperStatement.StartsWith("END"))
        {
            return new StatementResult { Type = StatementResultType.End };
        }
        else if (upperStatement.StartsWith("REM"))
        {
            return new StatementResult { Type = StatementResultType.Continue };
        }
        else if (_enableGraphics && upperStatement.StartsWith("PLOT"))
        {
            return ExecutePlot(statement);
        }
        else if (_enableGraphics && upperStatement.StartsWith("HLIN"))
        {
            return ExecuteHlin(statement);
        }
        else if (_enableGraphics && upperStatement.StartsWith("VLIN"))
        {
            return ExecuteVlin(statement);
        }
        else if (_enableGraphics && upperStatement.StartsWith("COLOR"))
        {
            return ExecuteColor(statement);
        }
        else if (_enableFileIO && upperStatement.StartsWith("OPEN"))
        {
            return ExecuteOpen(statement);
        }
        else if (_enableFileIO && upperStatement.StartsWith("CLOSE"))
        {
            return ExecuteClose(statement);
        }
        else if (_enableFileIO && upperStatement.StartsWith("PRINT#"))
        {
            return ExecutePrintFile(statement);
        }
        else if (_enableFileIO && upperStatement.StartsWith("INPUT#"))
        {
            return ExecuteInputFile(statement);
        }
        else if (_enableMemory && upperStatement.StartsWith("POKE"))
        {
            return ExecutePoke(statement);
        }
        else if (upperStatement.StartsWith("CALL"))
        {
            return ExecuteCall(statement);
        }
        else if (upperStatement.StartsWith("STOP"))
        {
            return ExecuteStop(statement);
        }
        else if (upperStatement.StartsWith("LIST"))
        {
            return ExecuteList(statement);
        }
        else if (upperStatement.StartsWith("NEW"))
        {
            return ExecuteNew(statement);
        }
        else if (upperStatement.StartsWith("CLEAR"))
        {
            return ExecuteClear(statement);
        }
        else if (upperStatement.StartsWith("RUN"))
        {
            return ExecuteRun(statement);
        }
        else if (upperStatement.StartsWith("CONT"))
        {
            return ExecuteCont(statement);
        }
        else
        {
            throw new BasicRuntimeException($"Unknown statement: {statement}", _currentLine);
        }
    }

    private StatementResult ExecutePrint(string statement)
    {
        var printText = statement.Substring(5).Trim();
        
        if (string.IsNullOrEmpty(printText))
        {
            _output.Add("");
            return new StatementResult { Type = StatementResultType.Continue };
        }
        
        // Handle string literals and variables
        var result = EvaluateExpression(printText);
        _output.Add(result?.ToString() ?? "");
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteLet(string statement)
    {
        string varName, expression;
        
        if (statement.ToUpper().StartsWith("LET"))
        {
            var letPart = statement.Substring(3).Trim();
            var equalIndex = letPart.IndexOf('=');
            if (equalIndex < 0)
                throw new BasicRuntimeException("Invalid LET statement", _currentLine);
                
            varName = letPart.Substring(0, equalIndex).Trim();
            expression = letPart.Substring(equalIndex + 1).Trim();
        }
        else
        {
            var equalIndex = statement.IndexOf('=');
            if (equalIndex < 0)
                throw new BasicRuntimeException("Invalid assignment statement", _currentLine);
                
            varName = statement.Substring(0, equalIndex).Trim();
            expression = statement.Substring(equalIndex + 1).Trim();
        }
        
        var value = EvaluateExpression(expression);
        
        // Handle array assignment
        var arrayMatch = Regex.Match(varName, @"([A-Z][A-Z0-9]*)\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (arrayMatch.Success)
        {
            var arrayName = arrayMatch.Groups[1].Value.ToUpper();
            var indicesStr = arrayMatch.Groups[2].Value;
            
            if (!_arrays.TryGetValue(arrayName, out var array))
            {
                // Auto-dimension array if not already defined
                var indices = indicesStr.Split(',')
                    .Select(i => (int)EvaluateNumericExpression(i.Trim()) + 1)
                    .ToArray();
                
                array = Array.CreateInstance(typeof(double), indices);
                _arrays[arrayName] = array;
            }
            
            var accessIndices = indicesStr.Split(',')
                .Select(i => (int)EvaluateNumericExpression(i.Trim()))
                .ToArray();
            
            if (accessIndices.Length != array.Rank)
                throw new BasicRuntimeException("Wrong number of array indices", _currentLine);
            
            for (int i = 0; i < accessIndices.Length; i++)
            {
                if (accessIndices[i] < 0 || accessIndices[i] >= array.GetLength(i))
                    throw new BasicRuntimeException("Array index out of bounds", _currentLine);
            }
            
            array.SetValue(Convert.ToDouble(value), accessIndices);
        }
        else
        {
            // Regular variable assignment
            _variables[varName.ToUpper()] = value;
        }
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteFor(string statement)
    {
        var forMatch = Regex.Match(statement, @"FOR\s+([A-Z][A-Z0-9]*)\s*=\s*(.+?)\s+TO\s+(.+?)(?:\s+STEP\s+(.+))?$", RegexOptions.IgnoreCase);
        
        if (!forMatch.Success)
            throw new BasicRuntimeException("Invalid FOR statement", _currentLine);
        
        var varName = forMatch.Groups[1].Value;
        var startValue = EvaluateExpression(forMatch.Groups[2].Value);
        var endValue = EvaluateExpression(forMatch.Groups[3].Value);
        var stepValue = forMatch.Groups[4].Success ? EvaluateExpression(forMatch.Groups[4].Value) : 1.0;
        
        _variables[varName] = Convert.ToDouble(startValue);
        
        _forLoopStack.Push(new ForLoop
        {
            Variable = varName,
            EndValue = Convert.ToDouble(endValue),
            StepValue = Convert.ToDouble(stepValue),
            StartLine = _currentLine
        });
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteNext(string statement)
    {
        if (_forLoopStack.Count == 0)
            throw new BasicRuntimeException("NEXT without FOR", _currentLine);
        
        var forLoop = _forLoopStack.Peek();
        var currentValue = Convert.ToDouble(_variables[forLoop.Variable]);
        currentValue += forLoop.StepValue;
        _variables[forLoop.Variable] = currentValue;
        
        bool continueLoop;
        if (forLoop.StepValue > 0)
            continueLoop = currentValue <= forLoop.EndValue;
        else
            continueLoop = currentValue >= forLoop.EndValue;
        
        if (continueLoop)
        {
            return new StatementResult { Type = StatementResultType.Goto, TargetLine = forLoop.StartLine };
        }
        else
        {
            _forLoopStack.Pop();
            return new StatementResult { Type = StatementResultType.Continue };
        }
    }

    private async Task<StatementResult> ExecuteIf(string statement)
    {
        var ifMatch = Regex.Match(statement, @"IF\s+(.+?)\s+THEN\s+(.+)$", RegexOptions.IgnoreCase);
        
        if (!ifMatch.Success)
            throw new BasicRuntimeException("Invalid IF statement", _currentLine);
        
        var condition = ifMatch.Groups[1].Value;
        var thenPart = ifMatch.Groups[2].Value;
        
        var conditionResult = EvaluateCondition(condition);
        
        if (conditionResult)
        {
            // Check if THEN part is a line number (GOTO) or a statement
            if (int.TryParse(thenPart.Trim(), out int lineNumber))
            {
                return new StatementResult { Type = StatementResultType.Goto, TargetLine = lineNumber };
            }
            else
            {
                // Execute the statement
                return await ExecuteStatement(thenPart, CancellationToken.None);
            }
        }
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteGoto(string statement)
    {
        var gotoMatch = Regex.Match(statement, @"GOTO\s+(\d+)", RegexOptions.IgnoreCase);
        
        if (!gotoMatch.Success)
            throw new BasicRuntimeException("Invalid GOTO statement", _currentLine);
        
        var lineNumber = int.Parse(gotoMatch.Groups[1].Value);
        return new StatementResult { Type = StatementResultType.Goto, TargetLine = lineNumber };
    }

    private StatementResult ExecuteGosub(string statement)
    {
        var gosubMatch = Regex.Match(statement, @"GOSUB\s+(\d+)", RegexOptions.IgnoreCase);
        
        if (!gosubMatch.Success)
            throw new BasicRuntimeException("Invalid GOSUB statement", _currentLine);
        
        var lineNumber = int.Parse(gosubMatch.Groups[1].Value);
        _subroutineStack.Push(_currentLine);
        return new StatementResult { Type = StatementResultType.Goto, TargetLine = lineNumber };
    }

    private StatementResult ExecuteReturn()
    {
        if (_subroutineStack.Count == 0)
            throw new BasicRuntimeException("RETURN without GOSUB", _currentLine);
        
        var returnLine = _subroutineStack.Pop();
        return new StatementResult { Type = StatementResultType.Goto, TargetLine = returnLine };
    }

    private StatementResult ExecuteDim(string statement)
    {
        var dimMatch = Regex.Match(statement, @"DIM\s+([A-Z][A-Z0-9]*)\s*\(\s*(.+)\s*\)", RegexOptions.IgnoreCase);
        
        if (!dimMatch.Success)
            ThrowError("Invalid DIM statement", _currentLine, "Syntax Error");
        
        var arrayName = dimMatch.Groups[1].Value.ToUpper();
        var dimensionsStr = dimMatch.Groups[2].Value;
        
        var dimensions = dimensionsStr.Split(',')
            .Select(d => (int)EvaluateNumericExpression(d.Trim()) + 1)
            .ToArray();
        
        if (dimensions.Any(d => d <= 0))
            ThrowError("Invalid array dimensions", _currentLine, "Array Error");
            
        // Check array size limits
        var totalElements = dimensions.Aggregate(1, (a, b) => a * b);
        if (totalElements > _resourceLimits.MaxArraySize)
            ThrowError($"Array too large. Maximum {_resourceLimits.MaxArraySize} elements allowed", _currentLine, "Resource Error");
        
        var array = Array.CreateInstance(typeof(double), dimensions);
        _arrays[arrayName] = array;
        _arrayBounds[arrayName] = totalElements;
        _stats.ArraysCreated++;
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteInput(string statement)
    {
        var inputMatch = Regex.Match(statement, @"INPUT\s+(.+)", RegexOptions.IgnoreCase);
        
        if (!inputMatch.Success)
            ThrowError("Invalid INPUT statement", _currentLine, "Syntax Error");
        
        var variables = inputMatch.Groups[1].Value.Split(',')
            .Select(v => v.Trim())
            .ToArray();
        
        // Use pre-supplied input values from queue
        foreach (var variable in variables)
        {
            object value;
            
            if (_inputQueue.Count > 0)
            {
                var inputValue = _inputQueue.Dequeue();
                value = variable.EndsWith("$") ? inputValue : 
                        double.TryParse(inputValue, out var numValue) ? numValue : 0.0;
            }
            else
            {
                // Default values if no input provided
                value = variable.EndsWith("$") ? "" : 0.0;
                LogError($"No input value provided for variable {variable}, using default", _currentLine, "Warning");
            }
            
            _variables[variable.ToUpper()] = value;
            _stats.VariablesCreated++;
        }
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteRead(string statement)
    {
        var readMatch = Regex.Match(statement, @"READ\s+(.+)", RegexOptions.IgnoreCase);
        
        if (!readMatch.Success)
            throw new BasicRuntimeException("Invalid READ statement", _currentLine);
        
        var variables = readMatch.Groups[1].Value.Split(',')
            .Select(v => v.Trim())
            .ToArray();
        
        foreach (var variable in variables)
        {
            if (_dataPointer >= _dataStatements.Sum(d => d.Values.Count))
                throw new BasicRuntimeException("Out of DATA", _currentLine);
            
            var value = GetNextDataValue();
            _variables[variable] = value;
        }
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteData(string statement)
    {
        var dataMatch = Regex.Match(statement, @"DATA\s+(.+)", RegexOptions.IgnoreCase);
        
        if (!dataMatch.Success)
            return new StatementResult { Type = StatementResultType.Continue };
        
        var valuesStr = dataMatch.Groups[1].Value;
        var values = new List<object>();
        
        var items = valuesStr.Split(',');
        foreach (var item in items)
        {
            var trimmed = item.Trim();
            if (trimmed.StartsWith("\"") && trimmed.EndsWith("\""))
            {
                values.Add(trimmed.Substring(1, trimmed.Length - 2));
            }
            else if (double.TryParse(trimmed, out var number))
            {
                values.Add(number);
            }
            else
            {
                values.Add(trimmed);
            }
        }
        
        _dataStatements.Add(new DataStatement
        {
            LineNumber = _currentLine,
            Values = values
        });
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteRestore(string statement)
    {
        _dataPointer = 0;
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteDef(string statement)
    {
        var defMatch = Regex.Match(statement, @"DEF\s+FN([A-Z][A-Z0-9]*)\s*\(\s*([A-Z][A-Z0-9]*)\s*\)\s*=\s*(.+)", RegexOptions.IgnoreCase);
        
        if (!defMatch.Success)
            throw new BasicRuntimeException("Invalid DEF statement", _currentLine);
        
        var functionName = "FN" + defMatch.Groups[1].Value.ToUpper();
        var parameter = defMatch.Groups[2].Value.ToUpper();
        var expression = defMatch.Groups[3].Value;
        
        _userFunctions[functionName] = new UserDefinedFunction
        {
            Name = functionName,
            Parameter = parameter,
            Expression = expression,
            LineNumber = _currentLine
        };
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteOn(string statement)
    {
        var onMatch = Regex.Match(statement, @"ON\s+(.+?)\s+(GOTO|GOSUB)\s+(.+)", RegexOptions.IgnoreCase);
        
        if (!onMatch.Success)
            throw new BasicRuntimeException("Invalid ON statement", _currentLine);
        
        var expression = onMatch.Groups[1].Value;
        var command = onMatch.Groups[2].Value.ToUpper();
        var targets = onMatch.Groups[3].Value.Split(',').Select(t => int.Parse(t.Trim())).ToArray();
        
        var index = (int)EvaluateNumericExpression(expression) - 1;
        
        if (index >= 0 && index < targets.Length)
        {
            var targetLine = targets[index];
            
            if (command == "GOSUB")
            {
                _subroutineStack.Push(_currentLine);
            }
            
            return new StatementResult { Type = StatementResultType.Goto, TargetLine = targetLine };
        }
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecutePlot(string statement)
    {
        // PLOT X, Y
        var plotMatch = Regex.Match(statement, @"PLOT\s+(.+),\s*(.+)", RegexOptions.IgnoreCase);
        
        if (!plotMatch.Success)
            throw new BasicRuntimeException("Invalid PLOT statement", _currentLine);
        
        var x = EvaluateNumericExpression(plotMatch.Groups[1].Value);
        var y = EvaluateNumericExpression(plotMatch.Groups[2].Value);
        
        _graphicsOutput.Add(new GraphicsCommand
        {
            Command = "PLOT",
            Parameters = new Dictionary<string, object>
            {
                ["x"] = x,
                ["y"] = y
            },
            LineNumber = _currentLine
        });
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteHlin(string statement)
    {
        // HLIN X1, X2 AT Y
        var hlinMatch = Regex.Match(statement, @"HLIN\s+(.+),\s*(.+)\s+AT\s+(.+)", RegexOptions.IgnoreCase);
        
        if (!hlinMatch.Success)
            throw new BasicRuntimeException("Invalid HLIN statement", _currentLine);
        
        var x1 = EvaluateNumericExpression(hlinMatch.Groups[1].Value);
        var x2 = EvaluateNumericExpression(hlinMatch.Groups[2].Value);
        var y = EvaluateNumericExpression(hlinMatch.Groups[3].Value);
        
        _graphicsOutput.Add(new GraphicsCommand
        {
            Command = "HLIN",
            Parameters = new Dictionary<string, object>
            {
                ["x1"] = x1,
                ["x2"] = x2,
                ["y"] = y
            },
            LineNumber = _currentLine
        });
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteVlin(string statement)
    {
        // VLIN Y1, Y2 AT X
        var vlinMatch = Regex.Match(statement, @"VLIN\s+(.+),\s*(.+)\s+AT\s+(.+)", RegexOptions.IgnoreCase);
        
        if (!vlinMatch.Success)
            throw new BasicRuntimeException("Invalid VLIN statement", _currentLine);
        
        var y1 = EvaluateNumericExpression(vlinMatch.Groups[1].Value);
        var y2 = EvaluateNumericExpression(vlinMatch.Groups[2].Value);
        var x = EvaluateNumericExpression(vlinMatch.Groups[3].Value);
        
        _graphicsOutput.Add(new GraphicsCommand
        {
            Command = "VLIN",
            Parameters = new Dictionary<string, object>
            {
                ["y1"] = y1,
                ["y2"] = y2,
                ["x"] = x
            },
            LineNumber = _currentLine,
            Color = _currentGraphicsColor,
            Thickness = 1,
            Style = "solid"
        });
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteColor(string statement)
    {
        // COLOR = value (sets graphics color)
        var colorMatch = Regex.Match(statement, @"COLOR\s*=\s*(.+)", RegexOptions.IgnoreCase);
        
        if (!colorMatch.Success)
            throw new BasicRuntimeException("Invalid COLOR statement", _currentLine);
        
        var colorValue = (int)EvaluateNumericExpression(colorMatch.Groups[1].Value);
        
        // Convert numeric color to name (simplified)
        _currentGraphicsColor = colorValue switch
        {
            0 => "black",
            1 => "white",
            2 => "red",
            3 => "green",
            4 => "blue",
            5 => "yellow",
            6 => "cyan",
            7 => "magenta",
            _ => "white"
        };
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteOpen(string statement)
    {
        // OPEN "filename", #channel[, mode]
        var openMatch = Regex.Match(statement, @"OPEN\s+""([^""]+)""\s*,\s*#(\d+)(?:\s*,\s*(\w+))?", RegexOptions.IgnoreCase);
        
        if (!openMatch.Success)
            throw new BasicRuntimeException("Invalid OPEN statement", _currentLine);
        
        var filename = openMatch.Groups[1].Value;
        var channel = int.Parse(openMatch.Groups[2].Value);
        var mode = openMatch.Groups[3].Success ? openMatch.Groups[3].Value.ToUpper() : "R";
        
        if (!_virtualFiles.ContainsKey(filename) && mode == "R")
        {
            _virtualFiles[filename] = ""; // Create empty file for reading
        }
        
        // Simulate file opening (store channel mapping)
        if (!_openFiles.ContainsKey(channel))
        {
            var memoryStream = new MemoryStream();
            _openFiles[channel] = new StreamWriter(memoryStream); // Placeholder for file operations
        }
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteClose(string statement)
    {
        // CLOSE #channel
        var closeMatch = Regex.Match(statement, @"CLOSE\s+#(\d+)", RegexOptions.IgnoreCase);
        
        if (!closeMatch.Success)
            throw new BasicRuntimeException("Invalid CLOSE statement", _currentLine);
        
        var channel = int.Parse(closeMatch.Groups[1].Value);
        
        if (_openFiles.ContainsKey(channel))
        {
            _openFiles[channel]?.Dispose();
            _openFiles.Remove(channel);
        }
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecutePrintFile(string statement)
    {
        // PRINT# channel, data
        var printMatch = Regex.Match(statement, @"PRINT#\s*(\d+)\s*,\s*(.+)", RegexOptions.IgnoreCase);
        
        if (!printMatch.Success)
            throw new BasicRuntimeException("Invalid PRINT# statement", _currentLine);
        
        var channel = int.Parse(printMatch.Groups[1].Value);
        var data = EvaluateExpression(printMatch.Groups[2].Value)?.ToString() ?? "";
        
        // Find the filename associated with this channel and append data
        var filename = $"CHANNEL_{channel}.txt"; // Simplified channel-to-file mapping
        if (_virtualFiles.ContainsKey(filename))
        {
            _virtualFiles[filename] += data + "\n";
        }
        else
        {
            _virtualFiles[filename] = data + "\n";
        }
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteInputFile(string statement)
    {
        // INPUT# channel, variable
        var inputMatch = Regex.Match(statement, @"INPUT#\s*(\d+)\s*,\s*([A-Z][A-Z0-9]*\$?)", RegexOptions.IgnoreCase);
        
        if (!inputMatch.Success)
            throw new BasicRuntimeException("Invalid INPUT# statement", _currentLine);
        
        var channel = int.Parse(inputMatch.Groups[1].Value);
        var variable = inputMatch.Groups[2].Value;
        
        // Read from virtual file
        var filename = $"CHANNEL_{channel}.txt";
        if (_virtualFiles.ContainsKey(filename))
        {
            var lines = _virtualFiles[filename].Split('\n');
            if (lines.Length > 0)
            {
                var value = lines[0];
                _variables[variable] = value;
                
                // Remove the read line from the file
                _virtualFiles[filename] = string.Join("\n", lines.Skip(1));
            }
        }
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecutePoke(string statement)
    {
        // POKE address, value
        var pokeMatch = Regex.Match(statement, @"POKE\s+(.+),\s*(.+)", RegexOptions.IgnoreCase);
        
        if (!pokeMatch.Success)
            throw new BasicRuntimeException("Invalid POKE statement", _currentLine);
        
        var address = (int)EvaluateNumericExpression(pokeMatch.Groups[1].Value);
        var value = (byte)EvaluateNumericExpression(pokeMatch.Groups[2].Value);
        
        _virtualMemory.Memory[address] = value;
        _virtualMemory.Operations.Add(new MemoryOperation
        {
            Operation = "POKE",
            Address = address,
            Value = value,
            LineNumber = _currentLine
        });
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteCall(string statement)
    {
        // CALL address (simulate machine language call)
        var callMatch = Regex.Match(statement, @"CALL\s+(.+)", RegexOptions.IgnoreCase);
        
        if (!callMatch.Success)
            throw new BasicRuntimeException("Invalid CALL statement", _currentLine);
        
        var address = (int)EvaluateNumericExpression(callMatch.Groups[1].Value);
        
        if (_enableMemory)
        {
            _virtualMemory.Operations.Add(new MemoryOperation
            {
                Operation = "CALL",
                Address = address,
                LineNumber = _currentLine
            });
            
            // Simulate some effect based on address (simplified)
            _output.Add($"CALL executed at address {address}");
        }
        else
        {
            _output.Add($"CALL {address} (memory operations disabled)");
        }
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteStop(string statement)
    {
        _output.Add("BREAK IN " + _currentLine);
        return new StatementResult { Type = StatementResultType.Stop };
    }

    private StatementResult ExecuteList(string statement)
    {
        // LIST [start[-end]]
        var listMatch = Regex.Match(statement, @"LIST\s*(\d+)?\s*(-\s*(\d+))?", RegexOptions.IgnoreCase);
        
        int startLine = 0;
        int endLine = int.MaxValue;
        
        if (listMatch.Groups[1].Success)
        {
            startLine = int.Parse(listMatch.Groups[1].Value);
        }
        
        if (listMatch.Groups[3].Success)
        {
            endLine = int.Parse(listMatch.Groups[3].Value);
        }
        else if (listMatch.Groups[1].Success)
        {
            endLine = startLine; // List single line
        }
        
        foreach (var line in _program.Where(kvp => kvp.Key >= startLine && kvp.Key <= endLine).OrderBy(kvp => kvp.Key))
        {
            _output.Add($"{line.Key} {line.Value}");
        }
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteNew(string statement)
    {
        _program.Clear();
        _variables.Clear();
        _arrays.Clear();
        _userFunctions.Clear();
        _dataStatements.Clear();
        _dataPointer = 0;
        _output.Add("NEW");
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteClear(string statement)
    {
        _variables.Clear();
        _arrays.Clear();
        _userFunctions.Clear();
        _dataStatements.Clear();
        _dataPointer = 0;
        _forLoopStack.Clear();
        _subroutineStack.Clear();
        
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private StatementResult ExecuteRun(string statement)
    {
        // RUN [line]
        var runMatch = Regex.Match(statement, @"RUN\s*(\d+)?", RegexOptions.IgnoreCase);
        
        int startLine = 0;
        if (runMatch.Groups[1].Success)
        {
            startLine = int.Parse(runMatch.Groups[1].Value);
        }
        
        // Clear variables but keep program
        _variables.Clear();
        _arrays.Clear();
        _userFunctions.Clear();
        _dataStatements.Clear();
        _dataPointer = 0;
        _forLoopStack.Clear();
        _subroutineStack.Clear();
        
        // Find the starting line
        var nextLine = _program.Keys.Where(k => k >= startLine).OrderBy(k => k).FirstOrDefault();
        if (nextLine == 0 && !_program.ContainsKey(startLine))
        {
            throw new BasicRuntimeException($"Undefined line number: {startLine}", _currentLine);
        }
        
        return new StatementResult { Type = StatementResultType.Goto, TargetLine = nextLine };
    }

    private StatementResult ExecuteCont(string statement)
    {
        // Continue execution from where STOP was encountered
        // This is simplified - in a real implementation we'd need to track the stop position
        _output.Add("CONT");
        return new StatementResult { Type = StatementResultType.Continue };
    }

    private object GetNextDataValue()
    {
        int currentIndex = 0;
        foreach (var dataStatement in _dataStatements)
        {
            if (_dataPointer < currentIndex + dataStatement.Values.Count)
            {
                var value = dataStatement.Values[_dataPointer - currentIndex];
                _dataPointer++;
                return value;
            }
            currentIndex += dataStatement.Values.Count;
        }
        
        throw new BasicRuntimeException("Out of DATA", _currentLine);
    }

    private object EvaluateExpression(string expression)
    {
        expression = expression.Trim();
        
        // Handle string literals
        if (expression.StartsWith("\"") && expression.EndsWith("\""))
        {
            return expression.Substring(1, expression.Length - 2);
        }
        
        // Handle numeric literals
        if (double.TryParse(expression, out double numValue))
        {
            return numValue;
        }
        
        // Handle string concatenation
        var concatMatch = Regex.Match(expression, @"(.+?)\s*\+\s*(.+)");
        if (concatMatch.Success)
        {
            var left = EvaluateExpression(concatMatch.Groups[1].Value);
            var right = EvaluateExpression(concatMatch.Groups[2].Value);
            
            // If either operand is a string, concatenate
            if (left is string || right is string)
            {
                return left.ToString() + right.ToString();
            }
            // Otherwise, add numerically
            return Convert.ToDouble(left) + Convert.ToDouble(right);
        }
        
        // Handle mathematical functions
        var mathFunctions = new Dictionary<string, Func<double, double>>
        {
            ["SQR"] = Math.Sqrt,
            ["ABS"] = Math.Abs,
            ["INT"] = Math.Floor,
            ["SIN"] = Math.Sin,
            ["COS"] = Math.Cos,
            ["TAN"] = Math.Tan,
            ["ATN"] = Math.Atan,
            ["LOG"] = Math.Log,
            ["EXP"] = Math.Exp
        };
        
        foreach (var func in mathFunctions)
        {
            var funcMatch = Regex.Match(expression, $@"{func.Key}\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
            if (funcMatch.Success)
            {
                var arg = EvaluateNumericExpression(funcMatch.Groups[1].Value);
                return func.Value(arg);
            }
        }
        
        // Handle SGN function
        var sgnMatch = Regex.Match(expression, @"SGN\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (sgnMatch.Success)
        {
            var value = EvaluateNumericExpression(sgnMatch.Groups[1].Value);
            return value > 0 ? 1.0 : value < 0 ? -1.0 : 0.0;
        }
        
        // Handle RND function
        var rndMatch = Regex.Match(expression, @"RND(\s*\(\s*(.+?)\s*\))?", RegexOptions.IgnoreCase);
        if (rndMatch.Success)
        {
            if (rndMatch.Groups[2].Success)
            {
                var seed = (int)EvaluateNumericExpression(rndMatch.Groups[2].Value);
                if (seed < 0)
                {
                    _random = new Random(Math.Abs(seed));
                    return _random.NextDouble();
                }
            }
            return _random.NextDouble();
        }
        
        // Handle PEEK function (memory operations)
        if (_enableMemory)
        {
            var peekMatch = Regex.Match(expression, @"PEEK\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
            if (peekMatch.Success)
            {
                var address = (int)EvaluateNumericExpression(peekMatch.Groups[1].Value);
                
                _virtualMemory.Operations.Add(new MemoryOperation
                {
                    Operation = "PEEK",
                    Address = address,
                    LineNumber = _currentLine
                });
                
                return _virtualMemory.Memory.ContainsKey(address) ? (double)_virtualMemory.Memory[address] : 0.0;
            }
        }
        
        // Handle TAB function
        var tabMatch = Regex.Match(expression, @"TAB\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (tabMatch.Success)
        {
            var position = (int)EvaluateNumericExpression(tabMatch.Groups[1].Value);
            return new string(' ', Math.Max(0, position));
        }
        
        // Handle SPC function  
        var spcMatch = Regex.Match(expression, @"SPC\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (spcMatch.Success)
        {
            var spaces = (int)EvaluateNumericExpression(spcMatch.Groups[1].Value);
            return new string(' ', Math.Max(0, spaces));
        }
        
        // Handle FRE function (free memory)
        var freMatch = Regex.Match(expression, @"FRE\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (freMatch.Success)
        {
            // Simulate free memory (simplified)
            return 32768.0; // Return a simulated memory value
        }
        
        // Handle POS function (print position)
        var posMatch = Regex.Match(expression, @"POS\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (posMatch.Success)
        {
            // Simulate print position (simplified)
            return 0.0; // Return column 0
        }
        
        // Handle string functions
        var leftMatch = Regex.Match(expression, @"LEFT\$\s*\(\s*(.+?)\s*,\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (leftMatch.Success)
        {
            var str = EvaluateExpression(leftMatch.Groups[1].Value).ToString();
            var length = (int)EvaluateNumericExpression(leftMatch.Groups[2].Value);
            return length >= str.Length ? str : str.Substring(0, Math.Max(0, length));
        }
        
        var rightMatch = Regex.Match(expression, @"RIGHT\$\s*\(\s*(.+?)\s*,\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (rightMatch.Success)
        {
            var str = EvaluateExpression(rightMatch.Groups[1].Value).ToString();
            var length = (int)EvaluateNumericExpression(rightMatch.Groups[2].Value);
            return length >= str.Length ? str : str.Substring(Math.Max(0, str.Length - length));
        }
        
        var midMatch = Regex.Match(expression, @"MID\$\s*\(\s*(.+?)\s*,\s*(.+?)\s*(,\s*(.+?))?\s*\)", RegexOptions.IgnoreCase);
        if (midMatch.Success)
        {
            var str = EvaluateExpression(midMatch.Groups[1].Value).ToString();
            var start = (int)EvaluateNumericExpression(midMatch.Groups[2].Value) - 1; // BASIC uses 1-based indexing
            var length = midMatch.Groups[4].Success ? (int)EvaluateNumericExpression(midMatch.Groups[4].Value) : str.Length;
            
            start = Math.Max(0, start);
            if (start >= str.Length) return "";
            
            length = Math.Min(length, str.Length - start);
            return str.Substring(start, Math.Max(0, length));
        }
        
        var lenMatch = Regex.Match(expression, @"LEN\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (lenMatch.Success)
        {
            var str = EvaluateExpression(lenMatch.Groups[1].Value).ToString();
            return (double)str.Length;
        }
        
        var ascMatch = Regex.Match(expression, @"ASC\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (ascMatch.Success)
        {
            var str = EvaluateExpression(ascMatch.Groups[1].Value).ToString();
            return str.Length > 0 ? (double)str[0] : 0.0;
        }
        
        var chrMatch = Regex.Match(expression, @"CHR\$\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (chrMatch.Success)
        {
            var code = (int)EvaluateNumericExpression(chrMatch.Groups[1].Value);
            return ((char)code).ToString();
        }
        
        var strMatch = Regex.Match(expression, @"STR\$\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (strMatch.Success)
        {
            var value = EvaluateNumericExpression(strMatch.Groups[1].Value);
            return " " + value.ToString(); // STR$ includes leading space for positive numbers
        }
        
        var valMatch = Regex.Match(expression, @"VAL\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (valMatch.Success)
        {
            var str = EvaluateExpression(valMatch.Groups[1].Value).ToString().Trim();
            return double.TryParse(str, out var result) ? result : 0.0;
        }
        
        // Enhanced string functions
        var instrMatch = Regex.Match(expression, @"INSTR\s*\(\s*(.+?)\s*,\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (instrMatch.Success)
        {
            var str = EvaluateExpression(instrMatch.Groups[1].Value).ToString();
            var searchStr = EvaluateExpression(instrMatch.Groups[2].Value).ToString();
            var index = str.IndexOf(searchStr, StringComparison.OrdinalIgnoreCase);
            return index == -1 ? 0.0 : (double)(index + 1); // BASIC uses 1-based indexing
        }
        
        var spaceMatch = Regex.Match(expression, @"SPACE\$\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (spaceMatch.Success)
        {
            var count = (int)EvaluateNumericExpression(spaceMatch.Groups[1].Value);
            return new string(' ', Math.Max(0, count));
        }
        
        var stringMatch = Regex.Match(expression, @"STRING\$\s*\(\s*(.+?)\s*,\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (stringMatch.Success)
        {
            var count = (int)EvaluateNumericExpression(stringMatch.Groups[1].Value);
            var charExpr = EvaluateExpression(stringMatch.Groups[2].Value);
            var ch = charExpr is string s && s.Length > 0 ? s[0] : 
                     charExpr is double d ? (char)(int)d : ' ';
            return new string(ch, Math.Max(0, count));
        }
        
        var ucaseMatch = Regex.Match(expression, @"UCASE\$\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (ucaseMatch.Success)
        {
            var str = EvaluateExpression(ucaseMatch.Groups[1].Value).ToString();
            return str.ToUpper();
        }
        
        var lcaseMatch = Regex.Match(expression, @"LCASE\$\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (lcaseMatch.Success)
        {
            var str = EvaluateExpression(lcaseMatch.Groups[1].Value).ToString();
            return str.ToLower();
        }
        
        var ltrimMatch = Regex.Match(expression, @"LTRIM\$\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (ltrimMatch.Success)
        {
            var str = EvaluateExpression(ltrimMatch.Groups[1].Value).ToString();
            return str.TrimStart();
        }
        
        var rtrimMatch = Regex.Match(expression, @"RTRIM\$\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (rtrimMatch.Success)
        {
            var str = EvaluateExpression(rtrimMatch.Groups[1].Value).ToString();
            return str.TrimEnd();
        }
        
        // Enhanced math functions
        var roundMatch = Regex.Match(expression, @"ROUND\s*\(\s*(.+?)\s*(,\s*(.+?))?\s*\)", RegexOptions.IgnoreCase);
        if (roundMatch.Success)
        {
            var value = EvaluateNumericExpression(roundMatch.Groups[1].Value);
            var decimals = roundMatch.Groups[3].Success ? (int)EvaluateNumericExpression(roundMatch.Groups[3].Value) : 0;
            return Math.Round(value, Math.Max(0, decimals));
        }
        
        var fixMatch = Regex.Match(expression, @"FIX\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (fixMatch.Success)
        {
            var value = EvaluateNumericExpression(fixMatch.Groups[1].Value);
            return value >= 0 ? Math.Floor(value) : Math.Ceiling(value);
        }
        
        var cintMatch = Regex.Match(expression, @"CINT\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (cintMatch.Success)
        {
            var value = EvaluateNumericExpression(cintMatch.Groups[1].Value);
            return (double)(int)Math.Round(value);
        }
        
        var cdblMatch = Regex.Match(expression, @"CDBL\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (cdblMatch.Success)
        {
            return EvaluateNumericExpression(cdblMatch.Groups[1].Value);
        }
        
        // Array bound functions
        var uboundMatch = Regex.Match(expression, @"UBOUND\s*\(\s*([A-Z][A-Z0-9]*)\s*(,\s*(.+?))?\s*\)", RegexOptions.IgnoreCase);
        if (uboundMatch.Success)
        {
            var arrayName = uboundMatch.Groups[1].Value.ToUpper();
            var dimension = uboundMatch.Groups[3].Success ? (int)EvaluateNumericExpression(uboundMatch.Groups[3].Value) : 0;
            return (double)GetArrayUpperBound(arrayName, dimension);
        }
        
        var lboundMatch = Regex.Match(expression, @"LBOUND\s*\(\s*([A-Z][A-Z0-9]*)\s*(,\s*(.+?))?\s*\)", RegexOptions.IgnoreCase);
        if (lboundMatch.Success)
        {
            var arrayName = lboundMatch.Groups[1].Value.ToUpper();
            var dimension = lboundMatch.Groups[3].Success ? (int)EvaluateNumericExpression(lboundMatch.Groups[3].Value) : 0;
            return (double)GetArrayLowerBound(arrayName, dimension);
        }
        
        // Handle user-defined functions
        var fnMatch = Regex.Match(expression, @"(FN[A-Z][A-Z0-9]*)\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (fnMatch.Success)
        {
            var funcName = fnMatch.Groups[1].Value.ToUpper();
            var argument = EvaluateExpression(fnMatch.Groups[2].Value);
            
            if (_userFunctions.TryGetValue(funcName, out var userFunc))
            {
                var savedValue = _variables.TryGetValue(userFunc.Parameter, out var temp) ? temp : null;
                _variables[userFunc.Parameter] = argument;
                
                var result = EvaluateExpression(userFunc.Expression);
                
                if (savedValue != null)
                    _variables[userFunc.Parameter] = savedValue;
                else
                    _variables.Remove(userFunc.Parameter);
                
                return result;
            }
            
            throw new BasicRuntimeException($"Undefined function: {funcName}", _currentLine);
        }
        
        // Handle array access
        var arrayMatch = Regex.Match(expression, @"([A-Z][A-Z0-9]*)\s*\(\s*(.+?)\s*\)", RegexOptions.IgnoreCase);
        if (arrayMatch.Success)
        {
            var arrayName = arrayMatch.Groups[1].Value.ToUpper();
            var indicesStr = arrayMatch.Groups[2].Value;
            
            if (_arrays.TryGetValue(arrayName, out var array))
            {
                var indices = indicesStr.Split(',')
                    .Select(i => (int)EvaluateNumericExpression(i.Trim()))
                    .ToArray();
                
                if (indices.Length != array.Rank)
                    throw new BasicRuntimeException("Wrong number of array indices", _currentLine);
                
                for (int i = 0; i < indices.Length; i++)
                {
                    if (indices[i] < 0 || indices[i] >= array.GetLength(i))
                        throw new BasicRuntimeException("Array index out of bounds", _currentLine);
                }
                
                return (double)array.GetValue(indices);
            }
        }
        
        // Handle variables (including string variables)
        if (Regex.IsMatch(expression, @"^[A-Z][A-Z0-9]*\$?$", RegexOptions.IgnoreCase))
        {
            return _variables.TryGetValue(expression.ToUpper(), out object value) ? value : 
                   (expression.EndsWith("$") ? "" : 0.0);
        }
        
        // Handle arithmetic operations (improved precedence handling)
        return EvaluateArithmeticExpression(expression);
    }
    
    private double EvaluateNumericExpression(string expression)
    {
        var result = EvaluateExpression(expression);
        if (result is string strResult && double.TryParse(strResult, out var numResult))
        {
            return numResult;
        }
        return Convert.ToDouble(result);
    }
    
    private object EvaluateArithmeticExpression(string expression)
    {
        // Simple arithmetic parser with basic precedence
        // This is a simplified version - a full parser would be more complex
        
        // Handle parentheses first
        var parenMatch = Regex.Match(expression, @"\(([^()]+)\)");
        if (parenMatch.Success)
        {
            var innerResult = EvaluateExpression(parenMatch.Groups[1].Value);
            var newExpression = expression.Replace(parenMatch.Value, innerResult.ToString());
            return EvaluateExpression(newExpression);
        }
        
        // Handle multiplication and division
        var multDivMatch = Regex.Match(expression, @"(.+?)\s*([*/])\s*(.+)");
        if (multDivMatch.Success)
        {
            var left = EvaluateNumericExpression(multDivMatch.Groups[1].Value);
            var op = multDivMatch.Groups[2].Value;
            var right = EvaluateNumericExpression(multDivMatch.Groups[3].Value);
            
            switch (op)
            {
                case "*": return left * right;
                case "/": return right != 0 ? left / right : throw new BasicRuntimeException("Division by zero", _currentLine);
            }
        }
        
        // Handle addition and subtraction
        var addSubMatch = Regex.Match(expression, @"(.+?)\s*([+\-])\s*(.+)");
        if (addSubMatch.Success)
        {
            var left = EvaluateNumericExpression(addSubMatch.Groups[1].Value);
            var op = addSubMatch.Groups[2].Value;
            var right = EvaluateNumericExpression(addSubMatch.Groups[3].Value);
            
            switch (op)
            {
                case "+": return left + right;
                case "-": return left - right;
            }
        }
        
        return expression;
    }

    private bool EvaluateCondition(string condition)
    {
        // Handle comparison operations
        var comparisonOps = new[] { "<=", ">=", "<>", "=", "<", ">" };
        
        foreach (var op in comparisonOps)
        {
            var index = condition.IndexOf(op);
            if (index > 0)
            {
                var left = EvaluateExpression(condition.Substring(0, index).Trim());
                var right = EvaluateExpression(condition.Substring(index + op.Length).Trim());
                
                var leftNum = Convert.ToDouble(left);
                var rightNum = Convert.ToDouble(right);
                
                switch (op)
                {
                    case "=": return Math.Abs(leftNum - rightNum) < 0.0001;
                    case "<>": return Math.Abs(leftNum - rightNum) >= 0.0001;
                    case "<": return leftNum < rightNum;
                    case ">": return leftNum > rightNum;
                    case "<=": return leftNum <= rightNum;
                    case ">=": return leftNum >= rightNum;
                }
            }
        }
        
        // If no comparison operator, treat as boolean expression
        var result = EvaluateExpression(condition);
        return Convert.ToDouble(result) != 0;
    }

    public static List<BasicFunction> GetSupportedFunctions()
    {
        return new List<BasicFunction>
        {
            // Control statements
            new BasicFunction { Name = "PRINT", Syntax = "PRINT expression", Description = "Output text or values", Category = "I/O" },
            new BasicFunction { Name = "LET", Syntax = "LET variable = expression", Description = "Assign value to variable", Category = "Control" },
            new BasicFunction { Name = "DIM", Syntax = "DIM array(size1[,size2,...])", Description = "Dimension arrays", Category = "Control" },
            new BasicFunction { Name = "INPUT", Syntax = "INPUT variable[,variable...]", Description = "Input values from user", Category = "I/O" },
            new BasicFunction { Name = "READ", Syntax = "READ variable[,variable...]", Description = "Read values from DATA statements", Category = "I/O" },
            new BasicFunction { Name = "DATA", Syntax = "DATA value[,value...]", Description = "Store data values", Category = "I/O" },
            new BasicFunction { Name = "RESTORE", Syntax = "RESTORE", Description = "Reset DATA pointer", Category = "I/O" },
            new BasicFunction { Name = "DEF", Syntax = "DEF FNname(parameter) = expression", Description = "Define user function", Category = "Control" },
            new BasicFunction { Name = "ON", Syntax = "ON expression GOTO/GOSUB line1[,line2...]", Description = "Computed branching", Category = "Control" },
            new BasicFunction { Name = "FOR", Syntax = "FOR variable = start TO end [STEP increment]", Description = "Start a loop", Category = "Control" },
            new BasicFunction { Name = "NEXT", Syntax = "NEXT [variable]", Description = "End a loop", Category = "Control" },
            new BasicFunction { Name = "IF", Syntax = "IF condition THEN statement", Description = "Conditional execution", Category = "Control" },
            new BasicFunction { Name = "GOTO", Syntax = "GOTO line_number", Description = "Jump to line", Category = "Control" },
            new BasicFunction { Name = "GOSUB", Syntax = "GOSUB line_number", Description = "Call subroutine", Category = "Control" },
            new BasicFunction { Name = "RETURN", Syntax = "RETURN", Description = "Return from subroutine", Category = "Control" },
            new BasicFunction { Name = "END", Syntax = "END", Description = "End program", Category = "Control" },
            new BasicFunction { Name = "REM", Syntax = "REM comment", Description = "Comment/remark", Category = "Control" },
            
            // Mathematical functions
            new BasicFunction { Name = "ABS", Syntax = "ABS(number)", Description = "Absolute value", Category = "Math" },
            new BasicFunction { Name = "ATN", Syntax = "ATN(number)", Description = "Arctangent", Category = "Math" },
            new BasicFunction { Name = "COS", Syntax = "COS(number)", Description = "Cosine", Category = "Math" },
            new BasicFunction { Name = "EXP", Syntax = "EXP(number)", Description = "Exponential (e^x)", Category = "Math" },
            new BasicFunction { Name = "INT", Syntax = "INT(number)", Description = "Integer part", Category = "Math" },
            new BasicFunction { Name = "LOG", Syntax = "LOG(number)", Description = "Natural logarithm", Category = "Math" },
            new BasicFunction { Name = "RND", Syntax = "RND[(number)]", Description = "Random number 0-1", Category = "Math" },
            new BasicFunction { Name = "SGN", Syntax = "SGN(number)", Description = "Sign (-1, 0, or 1)", Category = "Math" },
            new BasicFunction { Name = "SIN", Syntax = "SIN(number)", Description = "Sine", Category = "Math" },
            new BasicFunction { Name = "SQR", Syntax = "SQR(number)", Description = "Square root", Category = "Math" },
            new BasicFunction { Name = "TAN", Syntax = "TAN(number)", Description = "Tangent", Category = "Math" },
            new BasicFunction { Name = "ROUND", Syntax = "ROUND(number[, decimals])", Description = "Round to decimal places", Category = "Math" },
            new BasicFunction { Name = "FIX", Syntax = "FIX(number)", Description = "Truncate decimal part", Category = "Math" },
            new BasicFunction { Name = "CINT", Syntax = "CINT(number)", Description = "Convert to integer", Category = "Math" },
            new BasicFunction { Name = "CDBL", Syntax = "CDBL(expression)", Description = "Convert to double", Category = "Math" },
            
            // String functions
            new BasicFunction { Name = "ASC", Syntax = "ASC(string)", Description = "ASCII code of first character", Category = "String" },
            new BasicFunction { Name = "CHR$", Syntax = "CHR$(number)", Description = "Character from ASCII code", Category = "String" },
            new BasicFunction { Name = "LEFT$", Syntax = "LEFT$(string, length)", Description = "Left substring", Category = "String" },
            new BasicFunction { Name = "LEN", Syntax = "LEN(string)", Description = "Length of string", Category = "String" },
            new BasicFunction { Name = "MID$", Syntax = "MID$(string, start[, length])", Description = "Middle substring", Category = "String" },
            new BasicFunction { Name = "RIGHT$", Syntax = "RIGHT$(string, length)", Description = "Right substring", Category = "String" },
            new BasicFunction { Name = "STR$", Syntax = "STR$(number)", Description = "Convert number to string", Category = "String" },
            new BasicFunction { Name = "VAL", Syntax = "VAL(string)", Description = "Convert string to number", Category = "String" },
            new BasicFunction { Name = "INSTR", Syntax = "INSTR(string, substring)", Description = "Find position of substring", Category = "String" },
            new BasicFunction { Name = "SPACE$", Syntax = "SPACE$(count)", Description = "Generate spaces", Category = "String" },
            new BasicFunction { Name = "STRING$", Syntax = "STRING$(count, character)", Description = "Repeat character", Category = "String" },
            new BasicFunction { Name = "UCASE$", Syntax = "UCASE$(string)", Description = "Convert to uppercase", Category = "String" },
            new BasicFunction { Name = "LCASE$", Syntax = "LCASE$(string)", Description = "Convert to lowercase", Category = "String" },
            new BasicFunction { Name = "LTRIM$", Syntax = "LTRIM$(string)", Description = "Remove leading spaces", Category = "String" },
            new BasicFunction { Name = "RTRIM$", Syntax = "RTRIM$(string)", Description = "Remove trailing spaces", Category = "String" },
            
            // Program management commands  
            new BasicFunction { Name = "LIST", Syntax = "LIST [start[-end]]", Description = "List program lines", Category = "Program" },
            new BasicFunction { Name = "NEW", Syntax = "NEW", Description = "Clear current program", Category = "Program" },
            new BasicFunction { Name = "CLEAR", Syntax = "CLEAR", Description = "Clear variables and reset", Category = "Program" },
            new BasicFunction { Name = "RUN", Syntax = "RUN [line]", Description = "Run program from specified line", Category = "Program" },
            new BasicFunction { Name = "STOP", Syntax = "STOP", Description = "Stop program execution", Category = "Program" },
            new BasicFunction { Name = "CONT", Syntax = "CONT", Description = "Continue execution after STOP", Category = "Program" },
            
            // I/O formatting functions
            new BasicFunction { Name = "TAB", Syntax = "TAB(n)", Description = "Tab to column position", Category = "I/O" },
            new BasicFunction { Name = "SPC", Syntax = "SPC(n)", Description = "Print n spaces", Category = "I/O" },
            new BasicFunction { Name = "FRE", Syntax = "FRE(x)", Description = "Free memory available", Category = "System" },
            new BasicFunction { Name = "POS", Syntax = "POS(x)", Description = "Current print position", Category = "System" },
            
            // Graphics commands (when enabled)
            new BasicFunction { Name = "PLOT", Syntax = "PLOT x, y", Description = "Plot a point at coordinates", Category = "Graphics" },
            new BasicFunction { Name = "HLIN", Syntax = "HLIN x1, x2 AT y", Description = "Draw horizontal line", Category = "Graphics" },
            new BasicFunction { Name = "VLIN", Syntax = "VLIN y1, y2 AT x", Description = "Draw vertical line", Category = "Graphics" },
            new BasicFunction { Name = "COLOR", Syntax = "COLOR = value", Description = "Set graphics color", Category = "Graphics" },
            
            // File I/O commands (when enabled)
            new BasicFunction { Name = "OPEN", Syntax = "OPEN \"filename\", #channel[, mode]", Description = "Open virtual file", Category = "File I/O" },
            new BasicFunction { Name = "CLOSE", Syntax = "CLOSE #channel", Description = "Close file channel", Category = "File I/O" },
            new BasicFunction { Name = "PRINT#", Syntax = "PRINT# channel, data", Description = "Write to file", Category = "File I/O" },
            new BasicFunction { Name = "INPUT#", Syntax = "INPUT# channel, variable", Description = "Read from file", Category = "File I/O" },
            
            // Memory operations (when enabled)  
            new BasicFunction { Name = "PEEK", Syntax = "PEEK(address)", Description = "Read from virtual memory", Category = "Memory" },
            new BasicFunction { Name = "POKE", Syntax = "POKE address, value", Description = "Write to virtual memory", Category = "Memory" },
            new BasicFunction { Name = "CALL", Syntax = "CALL address", Description = "Simulate machine language call", Category = "Memory" },
            
            // Array functions
            new BasicFunction { Name = "UBOUND", Syntax = "UBOUND(array[, dimension])", Description = "Upper bound of array dimension", Category = "Array" },
            new BasicFunction { Name = "LBOUND", Syntax = "LBOUND(array[, dimension])", Description = "Lower bound of array dimension", Category = "Array" }
        };
    }
}

// Additional helper classes for enhanced functionality
public class UserDefinedFunction
{
    public string Name { get; set; }
    public string Parameter { get; set; }
    public string Expression { get; set; }
    public int LineNumber { get; set; }
}

public class DataStatement
{
    public int LineNumber { get; set; }
    public List<object> Values { get; set; }
}

// Enhanced request/response classes
public class BasicExecutionRequest
{
    public string Code { get; set; }
    public bool CaseSensitive { get; set; } = false;
    public bool EnableGraphics { get; set; } = false;
    public bool EnableFileIO { get; set; } = false;
    public bool EnableMemory { get; set; } = false;
    public string BasicVersion { get; set; } = "microsoft6502";
    public List<string> InputValues { get; set; } = new List<string>();
    public Dictionary<string, string> Files { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, object> Variables { get; set; } = new Dictionary<string, object>();
    public int MaxExecutionTimeMs { get; set; } = 120000;
    public ResourceLimits ResourceLimits { get; set; } = new ResourceLimits();
}

public class BasicExecutionResponse
{
    public string Output { get; set; }
    public Dictionary<string, object> Variables { get; set; }
    public List<GraphicsCommand> Graphics { get; set; }
    public Dictionary<string, string> Files { get; set; }
    public VirtualMemory Memory { get; set; }
    public ExecutionStatistics ExecutionStats { get; set; }
    public List<ExecutionError> Errors { get; set; }
    public bool Success { get; set; }
    public int LinesExecuted { get; set; }
    public long MemoryUsedBytes { get; set; }
    public int FunctionsCalled { get; set; }
}

// Helper classes
public class ForLoop
{
    public string Variable { get; set; }
    public double EndValue { get; set; }
    public double StepValue { get; set; }
    public int StartLine { get; set; }
}

public enum StatementResultType
{
    Continue,
    Goto,
    End,
    Stop
}

public class StatementResult
{
    public StatementResultType Type { get; set; }
    public int TargetLine { get; set; }
}