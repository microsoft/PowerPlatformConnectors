# -----------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# -----------------------------------------------------------------------------

"""
C# Script validation for Power Platform connectors.
Always runs in strict mode.
"""

import os
import re
from dataclasses import dataclass
from typing import List, Optional


@dataclass
class ValidationResult:
    """Results from script validation"""
    errors: List[str]
    warnings: List[str]
    file_path: str
    
    @property
    def has_errors(self) -> bool:
        return len(self.errors) > 0
    
    @property
    def has_warnings(self) -> bool:
        return len(self.warnings) > 0
    
    def format_errors(self) -> str:
        """Format errors for display"""
        if not self.errors:
            return ""
        formatted = []
        for i, error in enumerate(self.errors, 1):
            formatted.append(f"  Error {i}: {error}")
        return "\n".join(formatted)
    
    def format_warnings(self) -> str:
        """Format warnings for display"""
        if not self.warnings:
            return ""
        formatted = []
        for i, warning in enumerate(self.warnings, 1):
            formatted.append(f"  Warning {i}: {warning}")
        return "\n".join(formatted)


class CSharpScriptValidator:
    """
    Validates C# script files for Power Platform connectors.
    Always runs in strict mode with comprehensive checks.
    """
    
    # Allowed namespaces from Microsoft documentation
    ALLOWED_NAMESPACES = {
        'System',
        'System.Collections',
        'System.Collections.Generic',
        'System.Diagnostics',
        'System.IO',
        'System.IO.Compression',
        'System.Linq',
        'System.Net',
        'System.Net.Http',
        'System.Net.Http.Headers',
        'System.Net.Security',
        'System.Security.Authentication',
        'System.Security.Cryptography',
        'System.Text',
        'System.Text.RegularExpressions',
        'System.Threading',
        'System.Threading.Tasks',
        'System.Web',
        'System.Xml',
        'System.Xml.Linq',
        'System.Drawing',
        'System.Drawing.Drawing2D',
        'System.Drawing.Imaging',
        'Microsoft.Extensions.Logging',
        'Newtonsoft.Json',
        'Newtonsoft.Json.Linq'
    }
    
    MAX_FILE_SIZE = 1048576  # 1MB in bytes
    
    def validate_script(self, script_path: str) -> ValidationResult:
        """
        Validate a C# script file (always strict mode).
        """
        errors = []
        warnings = []
        
        # File existence and basic checks
        if not os.path.exists(script_path):
            errors.append(f"Script file not found: {script_path}")
            return ValidationResult(errors, warnings, script_path)
        
        if not script_path.lower().endswith('.csx'):
            errors.append(f"Script file must have .csx extension: {script_path}")
        
        # File size check
        file_size = os.path.getsize(script_path)
        if file_size > self.MAX_FILE_SIZE:
            errors.append(f"Script file size ({file_size} bytes) exceeds 1MB limit")
        
        # Read and validate content
        try:
            with open(script_path, 'r', encoding='utf-8') as file:
                content = file.read()
        except Exception as e:
            errors.append(f"Failed to read script file: {e}")
            return ValidationResult(errors, warnings, script_path)
        
        # Content validations
        self._validate_namespaces(content, errors)
        self._validate_class_structure(content, errors)
        self._validate_execute_async_method(content, errors)
        self._validate_best_practices(content, warnings)
        self._validate_security_patterns(content, errors, warnings)
        
        return ValidationResult(errors, warnings, script_path)
    
    def _validate_namespaces(self, content: str, errors: List[str]):
        """Validate using statements against allowed namespaces"""
        using_pattern = r'^\s*using\s+([^;]+);'
        using_statements = re.findall(using_pattern, content, re.MULTILINE)
        
        for using_stmt in using_statements:
            namespace = using_stmt.strip()
            if namespace not in self.ALLOWED_NAMESPACES:
                errors.append(f"Namespace '{namespace}' is not allowed. Use only the 26 approved namespaces for Power Platform connectors.")
    
    def _validate_class_structure(self, content: str, errors: List[str]):
        """Validate Script class structure"""
        # Check for Script class
        class_pattern = r'public\s+class\s+Script\s*:\s*ScriptBase'
        if not re.search(class_pattern, content):
            errors.append("Missing required 'public class Script : ScriptBase' declaration")
    
    def _validate_execute_async_method(self, content: str, errors: List[str]):
        """Validate ExecuteAsync method signature"""
        method_pattern = r'public\s+override\s+async\s+Task<HttpResponseMessage>\s+ExecuteAsync\s*\(\s*\)'
        if not re.search(method_pattern, content):
            errors.append("Missing required 'public override async Task<HttpResponseMessage> ExecuteAsync()' method")
    
    def _validate_best_practices(self, content: str, warnings: List[str]):
        """Check for best practices (strict mode warnings)"""
        # Check for ConfigureAwait(false)
        await_lines = [line for line in content.split('\n') if 'await ' in line]
        for line in await_lines:
            if 'await ' in line and 'ConfigureAwait(false)' not in line:
                warnings.append("Consider using '.ConfigureAwait(false)' with await statements for better performance")
                break  # Only warn once
        
        # Check for proper OperationId handling
        if 'Context.OperationId' in content and 'base64' not in content.lower():
            warnings.append("Consider implementing base64 decoding for OperationId to handle regional differences")
        
        # Check for CreateJsonContent usage
        if 'new JObject' in content and 'CreateJsonContent' not in content:
            warnings.append("Consider using 'CreateJsonContent()' helper method for JSON responses")
    
    def _validate_security_patterns(self, content: str, errors: List[str], warnings: List[str]):
        """Validate security patterns and practices"""
        # Check for direct HttpClient usage
        if re.search(r'new\s+HttpClient\s*\(', content):
            warnings.append("Consider using 'this.Context.SendAsync' instead of direct HttpClient instantiation")
        
        # Check for proper Context.SendAsync usage
        if 'HttpClient' in content and 'Context.SendAsync' not in content:
            warnings.append("Use 'this.Context.SendAsync' for HTTP requests instead of direct HttpClient")
        
        # Check for potentially unsafe operations
        if re.search(r'File\.|Directory\.|Path\.', content):
            errors.append("File system operations are not allowed in connector scripts")
        
        # Check for network operations outside of Context.SendAsync
        if re.search(r'Socket|TcpClient|UdpClient', content):
            errors.append("Direct network operations are not allowed. Use Context.SendAsync for HTTP requests")


def validate_script(script_path: str) -> ValidationResult:
    """
    Public function to validate a C# script file.
    Always runs in strict mode.
    """
    validator = CSharpScriptValidator()
    return validator.validate_script(script_path)