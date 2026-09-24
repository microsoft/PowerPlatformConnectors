# Microsoft BASIC Interpreter

The Microsoft BASIC M6502 Interpreter connector provides a comprehensive implementation of the classic Microsoft BASIC programming language for the Power Platform. Execute BASIC programs with advanced features including error handling, performance monitoring, graphics output, file I/O operations, and memory simulation.

## Publisher: Troy Taylor

## Prerequisites

There are no prerequisites needed for this service.

## Obtaining Credentials

No credentials are required. This is a code-only connector that runs the BASIC interpreter entirely within the connector's sandboxed environment without connecting to any external services.

## Supported Operations

### Execute BASIC Code
Executes a BASIC program and returns comprehensive results including output, variables, execution statistics, graphics commands, file operations, and memory operations.

### Validate BASIC Syntax
Validates BASIC program syntax without executing the code, returning detailed error information if syntax issues are found.

### Get Supported Functions
Returns a complete list of all supported BASIC functions and their syntax, organized by category.

## Complete BASIC Language Support

### Core Language Features
- Variables and assignment (numeric and string)
- Arrays (single and multi-dimensional with DIM)
- Control structures (FOR/NEXT, IF/THEN, WHILE/WEND)
- Subroutines (GOSUB/RETURN, ON...GOTO/GOSUB)
- User-defined functions (DEF FN)
- Data statements (DATA/READ/RESTORE)
- Input/Output (PRINT, INPUT with pre-supplied values)

### Mathematical Functions (15)
- Basic: ABS, ATN, COS, EXP, INT, LOG, RND, SGN, SIN, SQR, TAN
- Enhanced: ROUND, FIX, CINT, CDBL

### String Functions (15)
- Standard: ASC, CHR$, LEFT$, LEN, MID$, RIGHT$, STR$, VAL
- Advanced: INSTR, SPACE$, STRING$, UCASE$, LCASE$, LTRIM$, RTRIM$

### System Functions
- TAB, SPC, FRE, POS for formatting and system information

### Graphics Commands (when enabled)
- PLOT, HLIN, VLIN, COLOR for creating visual output

### File I/O Commands (when enabled)
- OPEN, CLOSE, PRINT#, INPUT# for virtual file operations

### Memory Operations (when enabled)
- PEEK, POKE, CALL for virtual memory simulation

### Program Management
- LIST, NEW, CLEAR, RUN, STOP, CONT for program control

### Array Functions
- UBOUND, LBOUND for array boundary management

## Example Usage

### Simple BASIC Program
```json
{
  "code": "10 PRINT \"Hello World\"\n20 FOR I = 1 TO 5\n30 PRINT \"Count: \"; I\n40 NEXT I\n50 END"
}
```

### Financial Calculations
```json
{
  "code": "10 DEF FN COMPOUND(P,R,T) = P * (1 + R/100) ^ T\n20 PRINCIPAL = 10000\n30 RATE = 5.5\n40 YEARS = 10\n50 RESULT = FN COMPOUND(PRINCIPAL, RATE, YEARS)\n60 PRINT \"Investment grows to: $\"; ROUND(RESULT, 2)",
  "inputValues": []
}
```

### String Processing
```json
{
  "code": "10 INPUT NAME$\n20 FORMATTED$ = UCASE$(LEFT$(NAME$, 1)) + LCASE$(MID$(NAME$, 2))\n30 CLEANED$ = LTRIM$(RTRIM$(FORMATTED$))\n40 PRINT \"Formatted name: \"; CLEANED$",
  "inputValues": ["  john doe  "],
  "caseSensitive": false
}
```

### Graphics Output
```json
{
  "code": "10 COLOR = 1\n20 FOR I = 0 TO 10\n30 PLOT I*5, I*3\n40 NEXT I\n50 COLOR = 2\n60 HLIN 0, 50 AT 15\n70 VLIN 0, 30 AT 25",
  "enableGraphics": true
}
```

### Data Analysis with Arrays
```json
{
  "code": "10 DIM SALES(12)\n20 TOTAL = 0\n30 FOR I = 1 TO 12\n40 READ SALES(I)\n50 TOTAL = TOTAL + SALES(I)\n60 NEXT I\n70 AVERAGE = TOTAL / 12\n80 PRINT \"Average monthly sales: $\"; ROUND(AVERAGE, 2)\n90 DATA 10500,12000,9500,15000,11000,13000,14000,16000,12000,10000,11000,12500"
}
```

## Request Parameters

### Execute BASIC Code Parameters
- **code** (required): The BASIC program code to execute
- **caseSensitive** (optional): Enable case-sensitive variable names (default: false)
- **enableGraphics** (optional): Enable graphics commands (default: false)
- **enableFileIO** (optional): Enable virtual file I/O (default: false)
- **enableMemory** (optional): Enable virtual memory operations (default: false)
- **basicVersion** (optional): BASIC dialect version (default: "microsoft6502")
- **inputValues** (optional): Array of pre-supplied input values for INPUT statements
- **files** (optional): JSON object with virtual files (filename: content)
- **variables** (optional): JSON object with initial variable values
- **maxExecutionTimeMs** (optional): Maximum execution time in milliseconds
- **resourceLimits** (optional): Object with resource constraints

### Resource Limits
- **maxVariables**: Maximum number of variables (default: 1000)
- **maxArraySize**: Maximum array elements (default: 10000)
- **maxStringLength**: Maximum string length (default: 32000)
- **maxNestingLevel**: Maximum loop/function nesting (default: 50)
- **maxExecutionTimeMs**: Maximum execution time (default: 120000)
- **maxMemoryBytes**: Maximum memory usage (default: 10485760)

## Response Format

### Execute Response
- **success**: Boolean indicating execution success
- **output**: Program output text
- **variables**: Final variable values as JSON object
- **graphics**: Array of graphics commands (if enabled)
- **files**: Modified virtual files (if file I/O enabled)
- **memory**: Virtual memory operations (if memory enabled)
- **executionStats**: Performance metrics object
- **errors**: Array of error objects with line numbers and context
- **linesExecuted**: Number of program lines executed
- **memoryUsedBytes**: Memory usage in bytes
- **functionsCalled**: Number of function calls made

### Execution Statistics
- **executionTimeMs**: Total execution time
- **linesExecuted**: Program lines processed
- **memoryUsedBytes**: Memory consumption
- **variablesCreated**: Number of variables created
- **functionsCalled**: Function invocations
- **errorsEncountered**: Error count
- **arraysCreated**: Arrays created
- **maxNestingLevel**: Maximum nesting depth reached

## Error Handling

The connector provides comprehensive error handling with:
- **Line-specific error reporting** with context and timestamps
- **Error categorization**: Syntax, Runtime, Resource, Array, Validation errors
- **Warning system** for non-fatal issues
- **Comprehensive error logging** with detailed information

## Performance Features

- **Execution statistics**: Time, memory usage, lines executed
- **Resource monitoring**: Variable count, array usage, nesting levels
- **Performance metrics**: Function calls, execution time, memory allocation
- **Resource limits**: Configurable constraints to prevent resource exhaustion

## Use Cases

✅ **Excellent for:**
- Mathematical calculations and algorithms
- Data processing and transformation
- Business logic implementation
- Educational BASIC programming
- Report generation and formatting
- Financial modeling and analysis
- Legacy system integration
- Algorithmic problem solving

❌ **Not suitable for:**
- Interactive programs requiring user input during execution
- Programs requiring persistent file storage
- Real-time graphics or game programming
- System administration tasks
- Long-running computations (>2 minutes)

## Technical Architecture

- **Language**: C# (.NET Standard 2.0)
- **Platform**: Power Platform Custom Connectors
- **Authentication**: None (code-only connector)
- **Execution Environment**: Sandboxed with resource monitoring
- **Memory Management**: Virtual memory simulation with limits
- **File System**: Virtual file system with temporary storage
- **Graphics**: Command-based output for external interpretation
- **Error Handling**: Comprehensive with line-level tracking
- String variables (ending with $)
- Numeric variables

### Input/Output
- `INPUT` - Interactive input (simulated in connector)
- `READ/DATA` - Read from data statements
- `RESTORE` - Reset data pointer

### User-Defined Functions
- `DEF FN` - Define custom functions

### Mathematical Functions
- `ABS(x)` - Absolute value
- `ATN(x)` - Arctangent
- `COS(x)` - Cosine
- `EXP(x)` - Exponential (e^x)
- `INT(x)` - Integer part
- `LOG(x)` - Natural logarithm
- `RND[(x)]` - Random number generation
- `SGN(x)` - Sign function (-1, 0, or 1)
- `SIN(x)` - Sine
- `SQR(x)` - Square root
- `TAN(x)` - Tangent

### String Functions
- `ASC(string)` - ASCII code of first character
- `CHR$(code)` - Character from ASCII code
- `LEFT$(string, length)` - Left substring
- `LEN(string)` - Length of string
- `MID$(string, start[, length])` - Middle substring
- `RIGHT$(string, length)` - Right substring
- `STR$(number)` - Convert number to string
- `VAL(string)` - Convert string to number
- String concatenation with `+` operator

### Operators
- Arithmetic: `+`, `-`, `*`, `/`
- Comparison: `=`, `<>`, `<`, `>`, `<=`, `>=`
- Parentheses for expression grouping

### Graphics Commands (when enableGraphics = true)
- `PLOT x, y` - Plot a point at coordinates
- `HLIN x1, x2 AT y` - Draw horizontal line
- `VLIN y1, y2 AT x` - Draw vertical line
- `COLOR = value` - Set graphics color (0-7)

### File I/O Commands (when enableFileIO = true)
- `OPEN "filename", #channel[, mode]` - Open virtual file
- `CLOSE #channel` - Close file channel
- `PRINT# channel, data` - Write to file
- `INPUT# channel, variable` - Read from file

### Memory Operations (when enableMemory = true)
- `PEEK(address)` - Read from virtual memory
- `POKE address, value` - Write to virtual memory
- `CALL address` - Simulate machine language call

### Program Management Commands
- `LIST [start[-end]]` - List program lines
- `NEW` - Clear current program
- `CLEAR` - Clear variables and reset
- `RUN [line]` - Run program from specified line
- `STOP` - Stop program execution
- `CONT` - Continue execution after STOP

### Formatting Functions
- `TAB(n)` - Tab to column position
- `SPC(n)` - Print n spaces
- `FRE(x)` - Free memory available
- `POS(x)` - Current print position

## Operations

### Execute BASIC Code
Executes a BASIC program and returns results.

**Input Parameters:**
- `code` (required): The BASIC program code
- `variables` (optional): Initial variable values as JSON object
- `maxExecutionTime` (optional): Maximum execution time in seconds (default: 30, max: 120)
- `enableDebug` (optional): Enable debug tracing (default: false)
- `caseSensitive` (optional): Enable case-sensitive variable names (default: false)
- `enableGraphics` (optional): Enable graphics commands and return graphics output (default: false)
- `enableFileIO` (optional): Enable virtual file I/O operations (default: false)
- `enableMemory` (optional): Enable virtual memory operations (default: false)
- `files` (optional): Virtual files to load as JSON object (filename: content)

**Response:**
- `success`: Boolean indicating if execution was successful
- `output`: Array of program output lines
- `variables`: Final variable values after execution
- `executionTime`: Execution time in seconds
- `linesExecuted`: Number of program lines executed
- `debugTrace`: Debug information (if debug mode enabled)
- `graphics`: Graphics commands output (if graphics mode enabled)
- `files`: Virtual files modified during execution (if file I/O enabled)
- `memory`: Virtual memory state and operations (if memory mode enabled)
- `error`: Error details (if execution failed)

### Validate BASIC Syntax
Validates BASIC program syntax without executing the code.

**Input Parameters:**
- `code` (required): The BASIC program code to validate

**Response:**
- `isValid`: Boolean indicating if syntax is valid
- `errors`: Array of syntax errors found
- `warnings`: Array of syntax warnings

### Get Supported Functions
Returns a list of all supported BASIC functions and their syntax.

**Response:**
- `functions`: Array of function objects with name, syntax, description, and category

## Example Usage

### Simple BASIC Program
```basic
10 PRINT "HELLO WORLD"
20 FOR I = 1 TO 5
30 PRINT "COUNT: "; I
40 NEXT I
50 END
```

### Advanced Examples

### Arrays and Data Processing
```basic
10 DIM A(10), B(5,5)
20 FOR I = 0 TO 10
30 A(I) = I * 2
40 NEXT I
50 FOR I = 0 TO 5
60 FOR J = 0 TO 5
70 B(I,J) = I + J
80 NEXT J
90 NEXT I
100 PRINT "A(5) ="; A(5)
110 PRINT "B(2,3) ="; B(2,3)
120 END
```

### String Processing
```basic
10 A$ = "HELLO"
20 B$ = "WORLD"
30 C$ = A$ + " " + B$
40 PRINT C$
50 PRINT "Length:"; LEN(C$)
60 PRINT "Left 5:"; LEFT$(C$, 5)
70 PRINT "Right 5:"; RIGHT$(C$, 5)
80 PRINT "Middle:"; MID$(C$, 7, 5)
90 END
```

### Mathematical Functions
```basic
10 FOR X = 0 TO 3.14159 STEP 0.5
20 PRINT "SIN("; X; ") ="; SIN(X)
30 PRINT "COS("; X; ") ="; COS(X)
40 PRINT "TAN("; X; ") ="; TAN(X)
50 NEXT X
60 PRINT "SQRT(16) ="; SQR(16)
70 PRINT "LOG(10) ="; LOG(10)
80 PRINT "EXP(1) ="; EXP(1)
90 END
```

### User-Defined Functions
```basic
10 DEF FNS(X) = X * X
20 DEF FNC(X) = X * X * X
30 FOR I = 1 TO 5
40 PRINT "Square of"; I; "is"; FNS(I)
50 PRINT "Cube of"; I; "is"; FNC(I)
60 NEXT I
70 END
```

### Data Processing
```basic
10 DATA 10, 20, 30, "HELLO", "WORLD"
20 DATA 40, 50, 60
30 FOR I = 1 TO 8
40 READ X$
50 PRINT "Item"; I; "="; X$
60 NEXT I
70 RESTORE
80 READ A, B, C
90 PRINT "Sum ="; A + B + C
100 END
```

### Computed Branching
```basic
10 INPUT "Enter choice (1-3)"; N
20 ON N GOTO 100, 200, 300
30 PRINT "Invalid choice"
40 GOTO 10
100 PRINT "You chose option 1"
110 GOTO 400
200 PRINT "You chose option 2"
210 GOTO 400
300 PRINT "You chose option 3"
400 END
```

### Graphics Commands (when enableGraphics = true)
```basic
10 REM Draw a colorful house
20 COLOR = 1: REM White
30 PLOT 50, 50
40 COLOR = 2: REM Red
50 HLIN 10, 90 AT 80
60 COLOR = 3: REM Green
70 VLIN 50, 80 AT 10
80 VLIN 50, 80 AT 90
90 COLOR = 4: REM Blue
100 HLIN 10, 90 AT 50
110 PRINT "House drawn with graphics commands"
120 END
```

### File I/O Operations (when enableFileIO = true)
```basic
10 REM File operations example
20 OPEN "DATA.TXT", #1, "W"
30 FOR I = 1 TO 5
40 PRINT# 1, "Line " + STR$(I)
50 NEXT I
60 CLOSE #1
70 OPEN "DATA.TXT", #1, "R"
80 FOR I = 1 TO 5
90 INPUT# 1, A$
100 PRINT A$
110 NEXT I
120 CLOSE #1
130 END
```

### Memory Operations (when enableMemory = true)
```basic
10 REM Memory operations example
20 FOR I = 0 TO 10
30 POKE 1000 + I, I * 2
40 NEXT I
50 FOR I = 0 TO 10
60 A = PEEK(1000 + I)
70 PRINT "Memory["; 1000 + I; "] = "; A
80 NEXT I
90 CALL 2000
100 END
```

### Program Management Examples
```basic
10 PRINT "This is a sample program"
20 PRINT "Line 20"
30 STOP
40 PRINT "This line after STOP"
50 END

REM To list the program:
LIST

REM To list specific lines:
LIST 10-30

REM To run from line 40:
RUN 40

REM To continue after STOP:
CONT
```

### Formatting Examples
```basic
10 PRINT "Name"; TAB(15); "Score"; TAB(25); "Grade"
20 PRINT "John"; TAB(15); "95"; TAB(25); "A"
30 PRINT "Start"; SPC(10); "End"
40 PRINT "Free Memory:"; FRE(0)
50 PRINT "Print Position:"; POS(0)
60 END
```

## Error Handling

The connector provides comprehensive error handling:
- **Syntax Errors**: Invalid BASIC syntax with line numbers
- **Runtime Errors**: Execution errors like division by zero
- **Timeout Errors**: Program execution exceeded time limit
- **Memory Errors**: Memory allocation issues

## Limitations

- Execution time limited to 120 seconds maximum
- Script file size limited to 1MB
- File I/O operations work with virtual files in memory (not persistent storage)
- Graphics commands return structured data rather than visual display
- Memory operations simulate virtual memory space (not actual system memory)

## New Features

### Configurable Case Sensitivity
- Set `caseSensitive` parameter to `true` for case-sensitive variable names
- Default behavior maintains traditional BASIC case-insensitive variables

### Enhanced Graphics Output Support
- Enable `enableGraphics` parameter to use PLOT, HLIN, VLIN, and COLOR commands
- Graphics commands include color, thickness, and style information
- Calling applications can render graphics based on the enhanced command data

### Virtual File I/O Operations
- Enable `enableFileIO` parameter to use OPEN, CLOSE, PRINT#, and INPUT# commands
- Files are stored in virtual memory during execution
- Input files can be provided in the request, output files returned in the response
- Supports simulated file channels and basic read/write operations

### Virtual Memory Operations
- Enable `enableMemory` parameter to use PEEK, POKE, and CALL commands
- Simulates a virtual memory space for low-level operations
- All memory operations are tracked and returned in the response
- Safe sandbox environment prevents actual system memory access

## License

This connector is inspired by the Microsoft BASIC-M6502 project which is licensed under the MIT License. The connector implementation respects all original license terms.

## Known Issues and Limitations

- Execution time limited to 120 seconds maximum (Power Platform constraint)
- Script file size limited to 1MB
- File I/O operations work with virtual files in memory (not persistent storage)
- Graphics commands return structured data rather than visual display
- Memory operations simulate virtual memory space (not actual system memory)
- Input must be pre-supplied (no interactive prompts during flow execution)