# PACONN CLI v0.1.0 - C# Script Validation Changes

## Overview
This package contains all the modified and new files for adding C# script validation to the paconn CLI tool.

## Version Update
- **Previous Version**: 0.0.21
- **New Version**: 0.1.0 (minor version bump for new feature)

## Files Modified/Added

### Root Level Files
1. **setup.py** - MODIFIED
   - Updated version from 0.0.21 to 0.1.0
   - Added regex dependency for enhanced C# script parsing

2. **README.md** - MODIFIED
   - Updated validate command documentation
   - Added comprehensive C# script validation examples
   - Added new command line arguments documentation

### paconn/ Directory
3. **paconn/__init__.py** - MODIFIED
   - Updated version from 0.0.21 to 0.1.0

### paconn/commands/ Directory
4. **paconn/commands/validate.py** - MODIFIED
   - Added script parameter to validate function signature
   - Implemented mutual exclusion logic (api-def OR script, not both)
   - Added comprehensive script validation output formatting
   - Enhanced error, warning, and success message formatting

5. **paconn/commands/params.py** - MODIFIED
   - Added SCRIPT parameter to _VALIDATE command arguments
   - Added help text for --script parameter
   - Included mutual exclusion documentation

### paconn/operations/ Directory
6. **paconn/operations/script_validate.py** - NEW FILE
   - Complete C# script validation engine
   - ValidationResult dataclass for structured results
   - CSharpScriptValidator class with comprehensive checks:
     - Namespace validation (26 approved namespaces)
     - Class structure validation (Script : ScriptBase)
     - Method signature validation (ExecuteAsync)
     - Security constraint validation
     - File size validation (1MB limit)
     - Best practices validation
   - Professional error and warning formatting

## New Features Added

### C# Script Validation
- **Always Strict Mode**: No basic/lenient validation option
- **Mutual Exclusion**: Cannot validate both swagger and script simultaneously
- **Comprehensive Checks**: 
  - Required Script class inheritance from ScriptBase
  - Mandatory ExecuteAsync method implementation
  - Only 26 allowed namespaces from Microsoft documentation
  - File size under 1MB limit
  - Security constraints (no file system, network operations)
  - Best practice recommendations

### Command Line Usage
```bash
# Validate C# script (always strict)
paconn validate --script script.csx

# Validate OpenAPI definition (existing functionality)
paconn validate --api-def swagger.json

# Using settings file (supports either, not both)
paconn validate --settings settings.json

# Error when both specified
paconn validate --api-def swagger.json --script script.csx  # ERROR

# Help documentation
paconn validate --help
```

### Settings File Support
Scripts can be validated via settings.json:
```json
{
  "script": "path/to/script.csx"
}
```

### Enhanced Output Formatting
All validation outputs now have consistent, professional formatting:
- Structured headers and sections
- Numbered errors and warnings
- Clear validation summaries
- Explicit result statements

## Installation Notes
- Requires Python 3.5+
- New dependency: regex>=2022.1.18 (automatically installed)
- Backward compatible with existing swagger validation workflows

## Testing
All functionality has been tested with:
- Valid C# scripts following Power Platform requirements
- Invalid scripts with various error types
- Scripts with warnings for best practices
- Mutual exclusion scenarios
- Settings file integration
- Help text and version display

## Deployment
1. Replace the modified files in the repository
2. Update version number references if needed
3. Test the package build: `python setup.py sdist bdist_wheel`
4. Publish to PyPI: `twine upload dist/*`
5. Users can upgrade: `pip install --upgrade paconn`