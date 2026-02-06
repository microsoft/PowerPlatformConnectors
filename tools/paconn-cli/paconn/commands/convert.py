# -----------------------------------------------------------------------------
# Copyright (c) 2025 Troy Taylor <troy@troystaylor.com>
# Licensed under the MIT License for use by Microsoft Corporation.
# See License.txt in the project root for license information.
# -----------------------------------------------------------------------------

"""
Convert command.
"""

import os
import json
import re
from paconn import _CONVERT
from paconn.common.util import display
from paconn.settings.settingsbuilder import SettingsBuilder


def convert(
        openapi_file,
        destination,
        settings_file):
    """
    Convert command.
    """
    # Get settings - most will be None for convert, but we follow the pattern
    settings = SettingsBuilder.get_settings(
        environment=None,
        settings_file=settings_file,
        api_properties=None,
        api_definition=openapi_file,
        icon=None,
        script=None,
        connector_id=None,
        powerapps_url=None,
        powerapps_version=None)

    # Use provided openapi_file or fall back to settings
    openapi_path = openapi_file or settings.api_definition
    
    if not openapi_path:
        raise Exception("OpenAPI file is required. Use --openapi to specify the file path.")
    
    if not os.path.exists(openapi_path):
        raise Exception(f"OpenAPI file not found: {openapi_path}")

    # Load and parse the OpenAPI file
    try:
        with open(openapi_path, 'r', encoding='utf-8') as f:
            openapi_data = json.load(f)
    except json.JSONDecodeError as e:
        raise Exception(f"Invalid JSON in OpenAPI file: {e}")
    except Exception as e:
        raise Exception(f"Error reading OpenAPI file: {e}")

    # Extract title for folder name
    api_title = openapi_data.get('info', {}).get('title', 'UnknownAPI')
    
    # Clean the title to make it a valid folder name
    folder_name = re.sub(r'[<>:"/\\|?*]', '', api_title).strip()
    if not folder_name:
        folder_name = 'UnknownAPI'

    # Determine destination directory
    if destination:
        base_path = destination
    else:
        base_path = os.getcwd()

    connector_dir = os.path.join(base_path, folder_name)

    # Create the connector directory
    os.makedirs(connector_dir, exist_ok=True)

    # Phase 1: Detect incompatible features and operations requiring custom code
    incompatible_features = _detect_incompatible_features(openapi_data)
    
    # Phase 3A: Analyze parameter schemas for transformation opportunities
    transformation_analysis = _analyze_parameter_transformations(openapi_data)
    
    # Phase 4: Enhanced complexity detection and scoring
    complexity_analysis = _analyze_api_complexity(openapi_data)
    
    # Create basic connector files
    _create_api_definition_file(openapi_data, connector_dir, incompatible_features, transformation_analysis, complexity_analysis)
    _create_api_properties_file(openapi_data, connector_dir, incompatible_features, complexity_analysis)
    _create_default_icon_file(connector_dir)
    _create_settings_file(folder_name, connector_dir)
    _create_custom_code_template(openapi_data, connector_dir, incompatible_features, transformation_analysis)
    _create_enhancement_recommendations(openapi_data, connector_dir, complexity_analysis)
    
    # Provide conversion summary
    total_operations = 0
    for path, methods in openapi_data.get('paths', {}).items():
        for method in methods:
            if method.upper() in ['GET', 'POST', 'PUT', 'PATCH', 'DELETE', 'HEAD', 'OPTIONS']:
                total_operations += 1
    
    if transformation_analysis and transformation_analysis.get('transformable_operations'):
        transformable_count = len(transformation_analysis['transformable_operations'])
        simple_count = total_operations - transformable_count
        display(f"Conversion complete: {total_operations} operations ({simple_count} natively in Swagger, {transformable_count} with transformations)")
    else:
        display(f"Conversion complete: {total_operations} operations (all implemented natively in Swagger)")

    display(f'Connector files created successfully in: {connector_dir}')


def _analyze_parameter_transformations(openapi_data):
    """
    Phase 3A: Analyze OpenAPI 3.0 parameter schemas to identify transformation opportunities
    for creating user-friendly Power Platform connector parameters.
    
    Returns:
        dict: Transformation analysis with operation-level parameter transformations
    """
    analysis = {
        'transformable_operations': {},
        'transformation_patterns': set(),
        'complexity_score': 0,
        'summary': {
            'operations_with_transformations': 0,
            'total_transformations': 0,
            'transformation_types': {}
        }
    }
    
    if 'paths' not in openapi_data:
        return analysis
    
    transformation_count = 0
    
    for path, methods in openapi_data['paths'].items():
        for method, operation in methods.items():
            if method.upper() in ['GET', 'POST', 'PUT', 'PATCH', 'DELETE']:
                operation_id = operation.get('operationId', f"{method.upper()}_{path.replace('/', '_')}")
                
                # Analyze parameters for this operation
                transformations = _analyze_operation_parameters(operation, openapi_data)
                
                if transformations:
                    analysis['transformable_operations'][operation_id] = {
                        'path': path,
                        'method': method.upper(),
                        'transformations': transformations,
                        'user_friendly_count': len(transformations),
                        'complexity_reduction': sum(t.get('complexity_reduction', 1) for t in transformations)
                    }
                    transformation_count += len(transformations)
                    
                    # Track transformation patterns
                    for transform in transformations:
                        analysis['transformation_patterns'].add(transform['type'])
                        transform_type = transform['type']
                        if transform_type not in analysis['summary']['transformation_types']:
                            analysis['summary']['transformation_types'][transform_type] = 0
                        analysis['summary']['transformation_types'][transform_type] += 1
    
    analysis['transformation_patterns'] = list(analysis['transformation_patterns'])
    analysis['summary']['operations_with_transformations'] = len(analysis['transformable_operations'])
    analysis['summary']['total_transformations'] = transformation_count
    analysis['complexity_score'] = min(transformation_count * 2, 10)  # Scale 0-10
    
    return analysis


def _analyze_operation_parameters(operation, openapi_data):
    """
    Analyze parameters for a single operation to identify transformation opportunities.
    
    Returns:
        list: List of transformation opportunities for this operation
    """
    transformations = []
    
    # Analyze path parameters
    parameters = operation.get('parameters', [])
    for param in parameters:
        transformation = _analyze_single_parameter(param, 'path', openapi_data)
        if transformation:
            transformations.append(transformation)
    
    # Analyze query parameters  
    for param in parameters:
        if param.get('in') == 'query':
            transformation = _analyze_single_parameter(param, 'query', openapi_data)
            if transformation:
                transformations.append(transformation)
    
    # Analyze header parameters
    for param in parameters:
        if param.get('in') == 'header':
            transformation = _analyze_single_parameter(param, 'header', openapi_data)
            if transformation:
                transformations.append(transformation)
    
    # Analyze request body for complex schemas
    if 'requestBody' in operation:
        body_transformations = _analyze_request_body(operation['requestBody'], openapi_data)
        transformations.extend(body_transformations)
    
    return transformations


def _analyze_single_parameter(param, param_location, openapi_data):
    """
    Analyze a single parameter for transformation opportunities.
    
    Returns:
        dict or None: Transformation specification if one is needed
    """
    param_name = param.get('name', 'unknown')
    schema = param.get('schema', {})
    
    # Check for anyOf/oneOf/allOf schemas
    if 'anyOf' in schema or 'oneOf' in schema or 'allOf' in schema:
        return {
            'type': 'schema_composition',
            'parameter': param_name,
            'location': param_location,
            'original_schema': schema,
            'user_friendly_approach': _suggest_composition_transformation(schema),
            'complexity_reduction': 3,
            'transformation_code_required': True,
            'description': f"Transform {param_name} from schema composition to user-friendly parameter"
        }
    
    # Check for complex object schemas that could be flattened
    if schema.get('type') == 'object' and 'properties' in schema:
        if len(schema['properties']) <= 5:  # Small objects can be flattened
            return {
                'type': 'object_flattening',
                'parameter': param_name,
                'location': param_location,
                'original_schema': schema,
                'user_friendly_approach': _suggest_object_flattening(schema, param_name),
                'complexity_reduction': 2,
                'transformation_code_required': True,
                'description': f"Flatten {param_name} object into individual parameters"
            }
    
    # Check for array parameters with complex items
    if schema.get('type') == 'array':
        items_schema = schema.get('items', {})
        if 'anyOf' in items_schema or 'oneOf' in items_schema or items_schema.get('type') == 'object':
            return {
                'type': 'array_simplification',
                'parameter': param_name,
                'location': param_location,
                'original_schema': schema,
                'user_friendly_approach': _suggest_array_transformation(schema, param_name),
                'complexity_reduction': 2,
                'transformation_code_required': True,
                'description': f"Simplify {param_name} array parameter for easier user input"
            }
    
    # Check for enum parameters that could benefit from dynamic values
    if 'enum' in schema and len(schema['enum']) > 3:
        return {
            'type': 'enum_to_dynamic',
            'parameter': param_name,
            'location': param_location,
            'original_schema': schema,
            'user_friendly_approach': _suggest_enum_dynamic_transformation(schema, param_name),
            'complexity_reduction': 1,
            'transformation_code_required': False,  # Can use x-ms-dynamic-values
            'description': f"Convert {param_name} enum to dynamic value list"
        }
    
    return None


def _analyze_request_body(request_body, openapi_data):
    """
    Analyze request body for transformation opportunities.
    
    Returns:
        list: List of transformation opportunities for request body
    """
    transformations = []
    
    content = request_body.get('content', {})
    for media_type, media_content in content.items():
        if media_type == 'application/json':
            schema = media_content.get('schema', {})
            
            # Check for complex composition schemas
            if 'anyOf' in schema or 'oneOf' in schema or 'allOf' in schema:
                transformations.append({
                    'type': 'body_composition',
                    'parameter': 'requestBody',
                    'location': 'body',
                    'original_schema': schema,
                    'user_friendly_approach': _suggest_body_composition_transformation(schema),
                    'complexity_reduction': 4,
                    'transformation_code_required': True,
                    'description': "Transform request body composition schema to user-friendly parameters"
                })
            
            # Check for deeply nested objects
            elif schema.get('type') == 'object':
                nested_depth = _calculate_schema_depth(schema)
                if nested_depth > 2:
                    transformations.append({
                        'type': 'nested_flattening',
                        'parameter': 'requestBody',
                        'location': 'body',
                        'original_schema': schema,
                        'user_friendly_approach': _suggest_nested_flattening(schema),
                        'complexity_reduction': nested_depth,
                        'transformation_code_required': True,
                        'description': f"Flatten nested request body (depth: {nested_depth}) for easier input"
                    })
    
    return transformations


def _suggest_composition_transformation(schema):
    """Suggest user-friendly approach for anyOf/oneOf/allOf schemas."""
    if 'anyOf' in schema:
        return {
            'approach': 'selector_parameter',
            'description': 'Add a selector parameter to choose the schema variant',
            'parameters': ['schema_type', 'value'],
            'example': 'schema_type: "string", value: "actual_value"'
        }
    elif 'oneOf' in schema:
        return {
            'approach': 'radio_selector',
            'description': 'Present as radio button selection with conditional fields',
            'parameters': ['option_type', 'option_value'],
            'example': 'option_type: "email", option_value: "user@example.com"'
        }
    elif 'allOf' in schema:
        return {
            'approach': 'merged_parameters',
            'description': 'Merge all schema properties into flat parameter list',
            'parameters': ['merged_object'],
            'example': 'All properties from merged schemas as individual parameters'
        }


def _suggest_object_flattening(schema, param_name):
    """Suggest flattening approach for object parameters."""
    properties = schema.get('properties', {})
    return {
        'approach': 'individual_parameters',
        'description': f'Replace {param_name} object with individual parameters',
        'parameters': [f"{param_name}_{prop}" for prop in properties.keys()],
        'example': f'Instead of {param_name}: {{prop1: "val1"}}, use {param_name}_prop1: "val1"'
    }


def _suggest_array_transformation(schema, param_name):
    """Suggest transformation for complex array parameters."""
    return {
        'approach': 'delimited_string',
        'description': f'Convert {param_name} array to delimited string input',
        'parameters': [f"{param_name}_list"],
        'example': f'{param_name}_list: "item1,item2,item3" (converted to array in code)'
    }


def _suggest_enum_dynamic_transformation(schema, param_name):
    """Suggest dynamic values approach for enum parameters."""
    return {
        'approach': 'dynamic_values',
        'description': f'Use x-ms-dynamic-values for {param_name} enum',
        'parameters': [param_name],
        'example': f'{param_name} with dynamic dropdown populated from enum values'
    }


def _suggest_body_composition_transformation(schema):
    """Suggest transformation for request body composition schemas."""
    return {
        'approach': 'conditional_parameters',
        'description': 'Break composition into conditional parameter groups',
        'parameters': ['body_type', 'body_content'],
        'example': 'body_type: "user", body_content: user object fields'
    }


def _suggest_nested_flattening(schema):
    """Suggest flattening approach for deeply nested objects."""
    return {
        'approach': 'dot_notation_flattening',
        'description': 'Flatten nested properties using dot notation',
        'parameters': ['flattened parameters with dot notation'],
        'example': 'user.profile.name instead of nested user: {profile: {name: "value"}}'
    }


def _calculate_schema_depth(schema, current_depth=0):
    """Calculate the maximum nesting depth of a schema."""
    if current_depth > 5:  # Prevent infinite recursion
        return current_depth
    
    max_depth = current_depth
    
    if schema.get('type') == 'object' and 'properties' in schema:
        for prop_schema in schema['properties'].values():
            depth = _calculate_schema_depth(prop_schema, current_depth + 1)
            max_depth = max(max_depth, depth)
    elif schema.get('type') == 'array' and 'items' in schema:
        depth = _calculate_schema_depth(schema['items'], current_depth + 1)
        max_depth = max(max_depth, depth)
    
    return max_depth


def _apply_parameter_transformations(swagger_data, transformation_analysis, openapi_data):
    """
    Phase 3B: Apply user-friendly parameter transformations to Swagger specification.
    
    This function transforms complex OpenAPI 3.0 parameters into user-friendly Power Platform
    parameters while maintaining backend compatibility through custom code.
    """
    if not swagger_data.get('paths') or not transformation_analysis.get('transformable_operations'):
        return swagger_data
    
    # Add transformation metadata to swagger for reference
    if 'x-ms-transformation-metadata' not in swagger_data:
        swagger_data['x-ms-transformation-metadata'] = {
            'version': '1.0',
            'generatedDate': '2025-08-06',
            'transformationSummary': transformation_analysis['summary'],
            'transformationPatterns': transformation_analysis['transformation_patterns']
        }
    
    # Apply transformations to each operation
    for operation_id, transform_details in transformation_analysis['transformable_operations'].items():
        path = transform_details['path']
        method = transform_details['method'].lower()
        
        if path in swagger_data['paths'] and method in swagger_data['paths'][path]:
            operation = swagger_data['paths'][path][method]
            
            # Apply parameter transformations to this operation
            operation = _transform_operation_parameters(
                operation, transform_details['transformations'], operation_id, openapi_data
            )
            
            # Update the operation in swagger
            swagger_data['paths'][path][method] = operation
    
    return swagger_data


def _transform_operation_parameters(operation, transformations, operation_id, openapi_data):
    """Transform parameters for a single operation based on transformation analysis."""
    if not transformations:
        return operation
    
    # Initialize parameters array if it doesn't exist
    if 'parameters' not in operation:
        operation['parameters'] = []
    
    # Group transformations by type for processing
    transformations_by_type = {}
    for transform in transformations:
        transform_type = transform['type']
        if transform_type not in transformations_by_type:
            transformations_by_type[transform_type] = []
        transformations_by_type[transform_type].append(transform)
    
    # Apply each transformation type
    for transform_type, type_transforms in transformations_by_type.items():
        if transform_type == 'schema_composition':
            operation = _apply_schema_composition_transformations(operation, type_transforms, operation_id)
        elif transform_type == 'object_flattening':
            operation = _apply_object_flattening_transformations(operation, type_transforms, operation_id)
        elif transform_type == 'array_simplification':
            operation = _apply_array_simplification_transformations(operation, type_transforms, operation_id)
        elif transform_type == 'enum_to_dynamic':
            operation = _apply_enum_dynamic_transformations(operation, type_transforms, operation_id)
        elif transform_type == 'nested_flattening':
            operation = _apply_nested_flattening_transformations(operation, type_transforms, operation_id)
        elif transform_type == 'body_composition':
            operation = _apply_body_composition_transformations(operation, type_transforms, operation_id)
    
    return operation


def _apply_schema_composition_transformations(operation, transformations, operation_id):
    """Apply anyOf/oneOf/allOf schema transformations to create user-friendly selectors."""
    for transform in transformations:
        param_name = transform['parameter']
        location = transform['location']
        original_schema = transform['original_schema']
        
        if location in ['query', 'path', 'header']:
            # Replace complex schema with selector + value parameters
            _remove_parameter_by_name(operation, param_name, location)
            
            # Add schema type selector parameter
            schema_selector = {
                'name': f"{param_name}_type",
                'in': location,
                'required': False,
                'type': 'string',
                'enum': _extract_schema_type_options(original_schema),
                'description': f"Select the type for {param_name}",
                'x-ms-summary': f"{param_name} Type",
                'x-ms-visibility': 'important'
            }
            operation['parameters'].append(schema_selector)
            
            # Add value parameter
            value_param = {
                'name': f"{param_name}_value",
                'in': location,
                'required': False,
                'type': 'string',
                'description': f"Value for {param_name} (format depends on selected type)",
                'x-ms-summary': f"{param_name} Value",
                'x-ms-visibility': 'important'
            }
            operation['parameters'].append(value_param)
    
    return operation


def _apply_object_flattening_transformations(operation, transformations, operation_id):
    """Apply object flattening transformations to create individual parameters."""
    for transform in transformations:
        param_name = transform['parameter']
        location = transform['location']
        original_schema = transform['original_schema']
        
        if location in ['query', 'path', 'header'] and 'properties' in original_schema:
            # Remove original complex parameter
            _remove_parameter_by_name(operation, param_name, location)
            
            # Add individual parameters for each property
            for prop_name, prop_schema in original_schema['properties'].items():
                flattened_param = {
                    'name': f"{param_name}_{prop_name}",
                    'in': location,
                    'required': prop_name in original_schema.get('required', []),
                    'type': prop_schema.get('type', 'string'),
                    'description': prop_schema.get('description', f"{param_name} {prop_name}"),
                    'x-ms-summary': f"{param_name.title()} {prop_name.title()}",
                    'x-ms-visibility': 'important'
                }
                
                # Add enum if present
                if 'enum' in prop_schema:
                    flattened_param['enum'] = prop_schema['enum']
                
                operation['parameters'].append(flattened_param)
    
    return operation


def _apply_array_simplification_transformations(operation, transformations, operation_id):
    """Apply array simplification transformations to create delimited string inputs."""
    for transform in transformations:
        param_name = transform['parameter']
        location = transform['location']
        
        if location in ['query', 'path', 'header']:
            # Replace array parameter with delimited string parameter
            _remove_parameter_by_name(operation, param_name, location)
            
            simplified_param = {
                'name': f"{param_name}_list",
                'in': location,
                'required': False,
                'type': 'string',
                'description': f"{param_name} as comma-separated values (e.g., 'item1,item2,item3')",
                'x-ms-summary': f"{param_name.title()} List",
                'x-ms-visibility': 'important',
                'x-ms-transformation-note': 'Will be converted to array in custom code'
            }
            operation['parameters'].append(simplified_param)
    
    return operation


def _apply_enum_dynamic_transformations(operation, transformations, operation_id):
    """Apply enum to dynamic values transformations."""
    for transform in transformations:
        param_name = transform['parameter']
        location = transform['location']
        original_schema = transform['original_schema']
        
        if location in ['query', 'path', 'header'] and 'enum' in original_schema:
            # Find and update existing parameter
            for param in operation.get('parameters', []):
                if param.get('name') == param_name and param.get('in') == location:
                    # Add dynamic values extension
                    param['x-ms-dynamic-values'] = {
                        'operationId': f'get_{param_name}_options',
                        'value-path': 'value',
                        'value-title': 'display'
                    }
                    param['x-ms-summary'] = f"Select {param_name.title()}"
                    param['x-ms-visibility'] = 'important'
                    break
    
    return operation


def _apply_nested_flattening_transformations(operation, transformations, operation_id):
    """Apply nested object flattening for request bodies."""
    for transform in transformations:
        if transform['location'] == 'body':
            # Add note to operation about nested flattening
            if 'x-ms-transformation-notes' not in operation:
                operation['x-ms-transformation-notes'] = []
            
            operation['x-ms-transformation-notes'].append({
                'type': 'nested_flattening',
                'description': 'Request body will be flattened from nested structure in custom code',
                'complexity_reduction': transform.get('complexity_reduction', 1)
            })
    
    return operation


def _apply_body_composition_transformations(operation, transformations, operation_id):
    """Apply request body composition transformations."""
    for transform in transformations:
        if transform['location'] == 'body':
            # Add body type selector parameter
            body_type_param = {
                'name': 'body_type',
                'in': 'query',
                'required': True,
                'type': 'string',
                'enum': _extract_composition_types(transform['original_schema']),
                'description': 'Select the type of request body to send',
                'x-ms-summary': 'Request Body Type',
                'x-ms-visibility': 'important'
            }
            operation.setdefault('parameters', []).append(body_type_param)
            
            # Add generic body content parameter
            body_content_param = {
                'name': 'body_content',
                'in': 'query',
                'required': True,
                'type': 'string',
                'description': 'Request body content (JSON string matching selected type)',
                'x-ms-summary': 'Request Body Content',
                'x-ms-visibility': 'important'
            }
            operation['parameters'].append(body_content_param)
    
    return operation


def _remove_parameter_by_name(operation, param_name, location):
    """Remove a parameter from the operation by name and location."""
    if 'parameters' not in operation:
        return
    
    operation['parameters'] = [
        p for p in operation['parameters'] 
        if not (p.get('name') == param_name and p.get('in') == location)
    ]


def _extract_schema_type_options(schema):
    """Extract type options from anyOf/oneOf/allOf schema."""
    options = []
    
    if 'anyOf' in schema:
        for i, sub_schema in enumerate(schema['anyOf']):
            schema_type = sub_schema.get('type', f'option_{i}')
            options.append(schema_type)
    elif 'oneOf' in schema:
        for i, sub_schema in enumerate(schema['oneOf']):
            schema_type = sub_schema.get('type', f'option_{i}')
            options.append(schema_type)
    elif 'allOf' in schema:
        options.append('merged')
    
    return options if options else ['string', 'object', 'array']


def _extract_composition_types(schema):
    """Extract composition types from request body schema."""
    if 'oneOf' in schema:
        types = []
        for sub_schema in schema['oneOf']:
            if 'properties' in sub_schema and 'type' in sub_schema['properties']:
                type_prop = sub_schema['properties']['type']
                if 'enum' in type_prop and len(type_prop['enum']) > 0:
                    types.append(type_prop['enum'][0])
        return types if types else ['individual', 'business']
    
    return ['type_a', 'type_b']


def _detect_incompatible_features(openapi_data):
    """
    Phase 1: Detect OpenAPI 3.0 features that are incompatible with Swagger 2.0
    and flag operations that require custom code.
    
    Returns a dictionary with:
    - operation_features: Dict of operation_id -> list of incompatible features
    - global_features: List of global incompatible features
    - custom_code_operations: Set of operation IDs that need custom code
    """
    result = {
        'operation_features': {},
        'global_features': [],
        'custom_code_operations': set(),
        'feature_summary': {
            'schema_complexity': [],
            'parameter_complexity': [],
            'response_complexity': [],
            'authentication_complexity': [],
            'content_type_complexity': [],
            'workflow_complexity': []
        }
    }
    
    # Check global features
    _detect_global_incompatible_features(openapi_data, result)
    
    # Check each operation
    if 'paths' in openapi_data:
        for path_key, path_item in openapi_data['paths'].items():
            http_methods = ['get', 'put', 'post', 'delete', 'options', 'head', 'patch']
            for method in http_methods:
                if method in path_item:
                    operation = path_item[method]
                    operation_id = operation.get('operationId', f"{method}_{path_key.replace('/', '_').replace('{', '').replace('}', '')}")
                    _detect_operation_incompatible_features(operation, operation_id, path_key, method, result)
    
    return result


def _detect_global_incompatible_features(openapi_data, result):
    """Detect global OpenAPI 3.0 features that require custom code."""
    
    # Multiple servers with variables
    servers = openapi_data.get('servers', [])
    if len(servers) > 1:
        result['global_features'].append('multiple_servers')
        result['feature_summary']['workflow_complexity'].append({
            'feature': 'multiple_servers',
            'description': 'Multiple server configurations detected',
            'count': len(servers)
        })
    
    # Server variables
    for server in servers:
        if 'variables' in server:
            result['global_features'].append('server_variables')
            result['feature_summary']['workflow_complexity'].append({
                'feature': 'server_variables', 
                'description': 'Server URL variables detected',
                'variables': list(server['variables'].keys())
            })
            break
    
    # Complex security schemes
    if 'components' in openapi_data and 'securitySchemes' in openapi_data['components']:
        for scheme_name, scheme in openapi_data['components']['securitySchemes'].items():
            if scheme.get('type') == 'openIdConnect':
                result['global_features'].append('openid_connect')
                result['feature_summary']['authentication_complexity'].append({
                    'feature': 'openid_connect',
                    'scheme': scheme_name,
                    'description': 'OpenID Connect authentication scheme'
                })
            elif scheme.get('type') == 'oauth2' and 'flows' in scheme:
                flows = scheme['flows']
                if len(flows) > 1:
                    result['global_features'].append('complex_oauth2')
                    result['feature_summary']['authentication_complexity'].append({
                        'feature': 'complex_oauth2',
                        'scheme': scheme_name,
                        'description': 'Multiple OAuth2 flows detected',
                        'flows': list(flows.keys())
                    })


def _detect_operation_incompatible_features(operation, operation_id, path_key, method, result):
    """Detect operation-specific incompatible features."""
    operation_features = []
    
    # Check parameters for complex features
    if 'parameters' in operation:
        for param in operation['parameters']:
            param_features = _detect_parameter_incompatible_features(param, operation_id)
            operation_features.extend(param_features)
    
    # Check request body for complex features
    if 'requestBody' in operation:
        body_features = _detect_request_body_incompatible_features(operation['requestBody'], operation_id)
        operation_features.extend(body_features)
    
    # Check responses for complex features  
    if 'responses' in operation:
        response_features = _detect_response_incompatible_features(operation['responses'], operation_id)
        operation_features.extend(response_features)
    
    # Check for callbacks (workflow complexity)
    if 'callbacks' in operation:
        operation_features.append('callbacks')
        result['feature_summary']['workflow_complexity'].append({
            'feature': 'callbacks',
            'operation': operation_id,
            'description': 'Callback operations detected',
            'count': len(operation['callbacks'])
        })
    
    # Store results
    if operation_features:
        result['operation_features'][operation_id] = operation_features
        result['custom_code_operations'].add(operation_id)
    
    return operation_features


def _detect_parameter_incompatible_features(param, operation_id):
    """Detect parameter-specific incompatible features."""
    features = []
    
    if 'schema' in param:
        schema = param['schema']
        
        # Complex schema features
        if any(key in schema for key in ['oneOf', 'anyOf', 'allOf']):
            features.append('complex_parameter_schema')
        
        if schema.get('nullable') is True:
            features.append('nullable_parameter')
        
        # Advanced parameter styling
        style = param.get('style')
        if style and style not in ['simple', 'form']:
            features.append('advanced_parameter_styling')
        
        if param.get('explode') is not None:
            features.append('parameter_explode')
        
        if param.get('allowReserved') is True:
            features.append('allow_reserved_parameter')
    
    return features


def _detect_request_body_incompatible_features(request_body, operation_id):
    """Detect request body incompatible features."""
    features = []
    
    if 'content' in request_body:
        content = request_body['content']
        
        # Multiple content types
        if len(content) > 1:
            features.append('multiple_request_content_types')
        
        # Check each content type for complex schemas
        for media_type, media_obj in content.items():
            if 'schema' in media_obj:
                schema_features = _detect_schema_complexity(media_obj['schema'], operation_id)
                features.extend(schema_features)
    
    return features


def _detect_response_incompatible_features(responses, operation_id):
    """Detect response incompatible features."""
    features = []
    
    for status_code, response in responses.items():
        if 'content' in response:
            content = response['content']
            
            # Multiple content types in response
            if len(content) > 1:
                features.append('multiple_response_content_types')
            
            # Check schemas in responses
            for media_type, media_obj in content.items():
                if 'schema' in media_obj:
                    schema_features = _detect_schema_complexity(media_obj['schema'], operation_id)
                    features.extend(schema_features)
        
        # Links (workflow complexity)
        if 'links' in response:
            features.append('response_links')
    
    return features


def _detect_schema_complexity(schema, operation_id):
    """Detect complex schema features that need custom code."""
    features = []
    
    # oneOf/anyOf/allOf
    if 'oneOf' in schema:
        features.append('oneof_schema')
    if 'anyOf' in schema:
        features.append('anyof_schema')  
    if 'allOf' in schema:
        features.append('allof_schema')
    
    # Discriminator
    if 'discriminator' in schema:
        features.append('discriminator_schema')
    
    # Nullable
    if schema.get('nullable') is True:
        features.append('nullable_schema')
    
    # writeOnly/readOnly
    if schema.get('writeOnly') is True or schema.get('readOnly') is True:
        features.append('readonly_writeonly_properties')
    
    # Multiple examples
    if 'examples' in schema and len(schema['examples']) > 1:
        features.append('multiple_examples')
    
    # Recursively check nested schemas
    if 'properties' in schema:
        for prop_name, prop_schema in schema['properties'].items():
            nested_features = _detect_schema_complexity(prop_schema, operation_id)
            features.extend(nested_features)
    
    if 'items' in schema:
        nested_features = _detect_schema_complexity(schema['items'], operation_id)
        features.extend(nested_features)
    
    return features


def _analyze_api_complexity(openapi_data):
    """
    Phase 4: Enhanced Detection Logic
    
    Comprehensive analysis of API complexity including:
    - Schema complexity scoring (oneOf/anyOf/allOf, nesting depth, conditional properties)
    - Parameter complexity (custom styles, array serialization, dependencies)
    - Response variability (multiple content types, conditional responses, links)
    - Authentication complexity (multiple schemes, conditional security)
    """
    display("Analyzing API complexity and sophistication...")
    
    complexity_analysis = {
        'schema_complexity': _analyze_schema_complexity(openapi_data),
        'parameter_complexity': _analyze_parameter_complexity(openapi_data),
        'response_complexity': _analyze_response_complexity(openapi_data),
        'auth_complexity': _analyze_authentication_complexity(openapi_data),
        'overall_score': 0,
        'complexity_factors': {},
        'recommendations': []
    }
    
    # Calculate overall complexity score
    schema_score = complexity_analysis['schema_complexity']['score']
    param_score = complexity_analysis['parameter_complexity']['score']
    response_score = complexity_analysis['response_complexity']['score']
    auth_score = complexity_analysis['auth_complexity']['score']
    
    # Weighted average (schema and parameters are most important for Power Platform)
    complexity_analysis['overall_score'] = (
        (schema_score * 0.35) + 
        (param_score * 0.35) + 
        (response_score * 0.20) + 
        (auth_score * 0.10)
    )
    
    complexity_analysis['complexity_factors'] = {
        'schema_contribution': schema_score * 0.35,
        'parameter_contribution': param_score * 0.35,
        'response_contribution': response_score * 0.20,
        'auth_contribution': auth_score * 0.10
    }
    
    # Generate recommendations based on complexity analysis
    complexity_analysis['recommendations'] = _generate_complexity_recommendations(complexity_analysis)
    
    # Display complexity summary
    _display_complexity_summary(complexity_analysis)
    
    return complexity_analysis


def _analyze_schema_complexity(openapi_data):
    """Analyze schema complexity: composition, nesting depth, and conditional properties."""
    schema_analysis = {
        'score': 0,
        'composition_usage': 0,
        'max_nesting_depth': 0,
        'conditional_properties': 0,
        'complex_schemas': [],
        'factors': {}
    }
    
    # Analyze schemas in components
    if 'components' in openapi_data and 'schemas' in openapi_data['components']:
        for schema_name, schema in openapi_data['components']['schemas'].items():
            analysis = _analyze_single_schema_complexity(schema, schema_name)
            
            schema_analysis['composition_usage'] += analysis['composition_count']
            schema_analysis['max_nesting_depth'] = max(
                schema_analysis['max_nesting_depth'], 
                analysis['nesting_depth']
            )
            schema_analysis['conditional_properties'] += analysis['conditional_props']
            
            if analysis['complexity_score'] > 3:
                schema_analysis['complex_schemas'].append({
                    'name': schema_name,
                    'score': analysis['complexity_score'],
                    'factors': analysis['complexity_factors']
                })
    
    # Analyze inline schemas in paths
    if 'paths' in openapi_data:
        for path, methods in openapi_data['paths'].items():
            for method, operation in methods.items():
                if not isinstance(operation, dict):
                    continue
                
                # Analyze request body schemas
                if 'requestBody' in operation:
                    request_body = operation['requestBody']
                    if 'content' in request_body:
                        for content_type, content in request_body['content'].items():
                            if 'schema' in content:
                                analysis = _analyze_single_schema_complexity(
                                    content['schema'], 
                                    f"{method.upper()} {path} request"
                                )
                                schema_analysis['composition_usage'] += analysis['composition_count']
                                schema_analysis['max_nesting_depth'] = max(
                                    schema_analysis['max_nesting_depth'], 
                                    analysis['nesting_depth']
                                )
                                schema_analysis['conditional_properties'] += analysis['conditional_props']
                
                # Analyze response schemas
                if 'responses' in operation:
                    for status_code, response in operation['responses'].items():
                        if 'content' in response:
                            for content_type, content in response['content'].items():
                                if 'schema' in content:
                                    analysis = _analyze_single_schema_complexity(
                                        content['schema'], 
                                        f"{method.upper()} {path} response {status_code}"
                                    )
                                    schema_analysis['composition_usage'] += analysis['composition_count']
                                    schema_analysis['max_nesting_depth'] = max(
                                        schema_analysis['max_nesting_depth'], 
                                        analysis['nesting_depth']
                                    )
                                    schema_analysis['conditional_properties'] += analysis['conditional_props']
    
    # Calculate complexity score (0-10)
    factors = {
        'composition_factor': min(schema_analysis['composition_usage'] * 2, 4),  # Max 4 points
        'depth_factor': min(schema_analysis['max_nesting_depth'] * 1.5, 3),     # Max 3 points
        'conditional_factor': min(schema_analysis['conditional_properties'] * 1, 3)  # Max 3 points
    }
    
    schema_analysis['factors'] = factors
    schema_analysis['score'] = sum(factors.values())
    
    return schema_analysis


def _analyze_single_schema_complexity(schema, schema_name):
    """Analyze complexity of a single schema definition."""
    analysis = {
        'complexity_score': 0,
        'composition_count': 0,
        'nesting_depth': 0,
        'conditional_props': 0,
        'complexity_factors': []
    }
    
    if not isinstance(schema, dict):
        return analysis
    
    # Check for composition keywords (oneOf, anyOf, allOf)
    composition_keywords = ['oneOf', 'anyOf', 'allOf']
    for keyword in composition_keywords:
        if keyword in schema:
            analysis['composition_count'] += 1
            analysis['complexity_factors'].append(f"Uses {keyword}")
            if isinstance(schema[keyword], list) and len(schema[keyword]) > 2:
                analysis['complexity_score'] += 2
            else:
                analysis['complexity_score'] += 1
    
    # Calculate nesting depth
    analysis['nesting_depth'] = _calculate_schema_depth(schema)
    if analysis['nesting_depth'] > 3:
        analysis['complexity_factors'].append(f"Deep nesting (depth: {analysis['nesting_depth']})")
        analysis['complexity_score'] += min(analysis['nesting_depth'] - 2, 3)
    
    # Check for conditional properties
    if 'if' in schema or 'then' in schema or 'else' in schema:
        analysis['conditional_props'] += 1
        analysis['complexity_factors'].append("Has conditional properties")
        analysis['complexity_score'] += 2
    
    # Check for discriminator
    if 'discriminator' in schema:
        analysis['complexity_factors'].append("Uses discriminator")
        analysis['complexity_score'] += 1
    
    # Check for complex patterns
    if 'pattern' in schema and len(schema.get('pattern', '')) > 20:
        analysis['complexity_factors'].append("Complex regex pattern")
        analysis['complexity_score'] += 1
    
    return analysis


def _analyze_parameter_complexity(openapi_data):
    """Analyze parameter complexity: custom styles, array serialization, dependencies."""
    param_analysis = {
        'score': 0,
        'custom_styles': 0,
        'array_serialization_complexity': 0,
        'parameter_dependencies': 0,
        'complex_parameters': [],
        'factors': {}
    }
    
    if 'paths' not in openapi_data:
        return param_analysis
    
    for path, methods in openapi_data['paths'].items():
        for method, operation in methods.items():
            if not isinstance(operation, dict):
                continue
            
            operation_id = operation.get('operationId', f"{method.upper()} {path}")
            
            # Analyze parameters
            parameters = operation.get('parameters', [])
            for param in parameters:
                if not isinstance(param, dict):
                    continue
                
                param_complexity = _analyze_single_parameter_complexity(param, operation_id)
                
                param_analysis['custom_styles'] += param_complexity['custom_style_count']
                param_analysis['array_serialization_complexity'] += param_complexity['array_complexity']
                
                if param_complexity['complexity_score'] > 2:
                    param_analysis['complex_parameters'].append({
                        'operation': operation_id,
                        'parameter': param.get('name', 'unknown'),
                        'score': param_complexity['complexity_score'],
                        'factors': param_complexity['factors']
                    })
    
    # Analyze parameter dependencies (parameters that reference each other)
    param_analysis['parameter_dependencies'] = _analyze_parameter_dependencies(openapi_data)
    
    # Calculate complexity score (0-10)
    factors = {
        'style_factor': min(param_analysis['custom_styles'] * 1, 3),           # Max 3 points
        'array_factor': min(param_analysis['array_serialization_complexity'] * 0.5, 2), # Max 2 points
        'dependency_factor': min(param_analysis['parameter_dependencies'] * 2, 5)   # Max 5 points
    }
    
    param_analysis['factors'] = factors
    param_analysis['score'] = sum(factors.values())
    
    return param_analysis


def _analyze_single_parameter_complexity(param, operation_id):
    """Analyze complexity of a single parameter."""
    analysis = {
        'complexity_score': 0,
        'custom_style_count': 0,
        'array_complexity': 0,
        'factors': []
    }
    
    # Check for custom serialization styles
    style = param.get('style')
    if style and style not in ['simple', 'form']:
        analysis['custom_style_count'] += 1
        analysis['factors'].append(f"Custom style: {style}")
        analysis['complexity_score'] += 1
    
    # Check explode parameter
    if 'explode' in param and not param.get('explode', True):
        analysis['factors'].append("Non-standard explode behavior")
        analysis['complexity_score'] += 1
    
    # Check array serialization complexity
    if param.get('schema', {}).get('type') == 'array':
        analysis['array_complexity'] += 1
        
        # Check for complex array items
        items_schema = param.get('schema', {}).get('items', {})
        if isinstance(items_schema, dict):
            if items_schema.get('type') == 'object':
                analysis['factors'].append("Array of objects")
                analysis['complexity_score'] += 2
            elif 'oneOf' in items_schema or 'anyOf' in items_schema:
                analysis['factors'].append("Array with schema composition")
                analysis['complexity_score'] += 2
    
    # Check for deep object parameters
    if param.get('schema', {}).get('type') == 'object':
        depth = _calculate_schema_depth(param.get('schema', {}))
        if depth > 2:
            analysis['factors'].append(f"Deep object parameter (depth: {depth})")
            analysis['complexity_score'] += min(depth - 1, 3)
    
    return analysis


def _analyze_parameter_dependencies(openapi_data):
    """Analyze dependencies between parameters across operations."""
    dependencies = 0
    
    # This is a simplified analysis - in practice, you'd need more sophisticated logic
    # to detect actual dependencies between parameters
    
    if 'paths' not in openapi_data:
        return dependencies
    
    for path, methods in openapi_data['paths'].items():
        for method, operation in methods.items():
            if not isinstance(operation, dict):
                continue
            
            parameters = operation.get('parameters', [])
            
            # Check for parameters with similar names (potential dependencies)
            param_names = [p.get('name', '') for p in parameters if isinstance(p, dict)]
            
            for i, name1 in enumerate(param_names):
                for name2 in param_names[i+1:]:
                    # Simple heuristic: parameters with similar prefixes might be related
                    if name1 and name2 and len(name1) > 3 and len(name2) > 3:
                        if name1[:3] == name2[:3] or name1.split('_')[0] == name2.split('_')[0]:
                            dependencies += 0.5  # Partial dependency detected
    
    return int(dependencies)


def _analyze_response_complexity(openapi_data):
    """Analyze response complexity: multiple content types, conditional responses, links."""
    response_analysis = {
        'score': 0,
        'multiple_content_types': 0,
        'conditional_responses': 0,
        'link_relationships': 0,
        'complex_responses': [],
        'factors': {}
    }
    
    if 'paths' not in openapi_data:
        return response_analysis
    
    for path, methods in openapi_data['paths'].items():
        for method, operation in methods.items():
            if not isinstance(operation, dict):
                continue
            
            operation_id = operation.get('operationId', f"{method.upper()} {path}")
            
            # Analyze responses
            responses = operation.get('responses', {})
            for status_code, response in responses.items():
                if not isinstance(response, dict):
                    continue
                
                response_complexity = _analyze_single_response_complexity(response, operation_id, status_code)
                
                response_analysis['multiple_content_types'] += response_complexity['content_type_count']
                response_analysis['link_relationships'] += response_complexity['link_count']
                
                if len(responses) > 4:  # Many different response codes
                    response_analysis['conditional_responses'] += 0.2
                
                if response_complexity['complexity_score'] > 2:
                    response_analysis['complex_responses'].append({
                        'operation': operation_id,
                        'status_code': status_code,
                        'score': response_complexity['complexity_score'],
                        'factors': response_complexity['factors']
                    })
    
    # Calculate complexity score (0-10)
    factors = {
        'content_type_factor': min(response_analysis['multiple_content_types'] * 0.5, 3), # Max 3 points
        'conditional_factor': min(response_analysis['conditional_responses'], 4),         # Max 4 points
        'link_factor': min(response_analysis['link_relationships'] * 1, 3)               # Max 3 points
    }
    
    response_analysis['factors'] = factors
    response_analysis['score'] = sum(factors.values())
    
    return response_analysis


def _analyze_single_response_complexity(response, operation_id, status_code):
    """Analyze complexity of a single response definition."""
    analysis = {
        'complexity_score': 0,
        'content_type_count': 0,
        'link_count': 0,
        'factors': []
    }
    
    # Check for multiple content types
    if 'content' in response:
        content_types = list(response['content'].keys())
        analysis['content_type_count'] = len(content_types)
        
        if len(content_types) > 1:
            analysis['factors'].append(f"Multiple content types: {', '.join(content_types)}")
            analysis['complexity_score'] += len(content_types) - 1
        
        # Check for complex schemas in responses
        for content_type, content in response['content'].items():
            if 'schema' in content:
                schema_depth = _calculate_schema_depth(content['schema'])
                if schema_depth > 3:
                    analysis['factors'].append(f"Deep response schema (depth: {schema_depth})")
                    analysis['complexity_score'] += min(schema_depth - 2, 2)
    
    # Check for response links (OpenAPI 3.0 feature)
    if 'links' in response:
        analysis['link_count'] = len(response['links'])
        analysis['factors'].append(f"Response links: {analysis['link_count']}")
        analysis['complexity_score'] += min(analysis['link_count'], 3)
    
    # Check for complex headers
    if 'headers' in response:
        complex_headers = 0
        for header_name, header in response['headers'].items():
            if isinstance(header, dict) and 'schema' in header:
                if header['schema'].get('type') == 'object':
                    complex_headers += 1
        
        if complex_headers > 0:
            analysis['factors'].append(f"Complex headers: {complex_headers}")
            analysis['complexity_score'] += min(complex_headers, 2)
    
    return analysis


def _analyze_authentication_complexity(openapi_data):
    """Analyze authentication complexity: multiple schemes, conditional security."""
    auth_analysis = {
        'score': 0,
        'multiple_schemes': 0,
        'conditional_security': 0,
        'oauth_complexity': 0,
        'auth_schemes': [],
        'factors': {}
    }
    
    # Analyze global security schemes
    if 'components' in openapi_data and 'securitySchemes' in openapi_data['components']:
        schemes = openapi_data['components']['securitySchemes']
        auth_analysis['multiple_schemes'] = len(schemes)
        
        for scheme_name, scheme in schemes.items():
            if not isinstance(scheme, dict):
                continue
            
            scheme_complexity = _analyze_single_auth_scheme(scheme, scheme_name)
            auth_analysis['oauth_complexity'] += scheme_complexity['oauth_complexity']
            auth_analysis['auth_schemes'].append({
                'name': scheme_name,
                'type': scheme.get('type', 'unknown'),
                'complexity': scheme_complexity['complexity_score'],
                'factors': scheme_complexity['factors']
            })
    
    # Analyze conditional security (operation-level security requirements)
    if 'paths' in openapi_data:
        operations_with_custom_security = 0
        total_operations = 0
        
        for path, methods in openapi_data['paths'].items():
            for method, operation in methods.items():
                if not isinstance(operation, dict):
                    continue
                
                total_operations += 1
                
                if 'security' in operation:
                    operations_with_custom_security += 1
        
        if total_operations > 0:
            auth_analysis['conditional_security'] = operations_with_custom_security / total_operations
    
    # Calculate complexity score (0-10)
    factors = {
        'scheme_count_factor': min(auth_analysis['multiple_schemes'] * 1.5, 4),    # Max 4 points
        'oauth_factor': min(auth_analysis['oauth_complexity'] * 2, 3),             # Max 3 points
        'conditional_factor': min(auth_analysis['conditional_security'] * 6, 3)    # Max 3 points
    }
    
    auth_analysis['factors'] = factors
    auth_analysis['score'] = sum(factors.values())
    
    return auth_analysis


def _analyze_single_auth_scheme(scheme, scheme_name):
    """Analyze complexity of a single authentication scheme."""
    analysis = {
        'complexity_score': 0,
        'oauth_complexity': 0,
        'factors': []
    }
    
    scheme_type = scheme.get('type', '')
    
    if scheme_type == 'oauth2':
        analysis['oauth_complexity'] = 1
        analysis['complexity_score'] += 2
        analysis['factors'].append("OAuth2 authentication")
        
        # Check for multiple OAuth2 flows
        flows = scheme.get('flows', {})
        if len(flows) > 1:
            analysis['factors'].append(f"Multiple OAuth2 flows: {', '.join(flows.keys())}")
            analysis['complexity_score'] += len(flows) - 1
        
        # Check for complex scopes
        total_scopes = 0
        for flow_name, flow in flows.items():
            if 'scopes' in flow:
                total_scopes += len(flow['scopes'])
        
        if total_scopes > 5:
            analysis['factors'].append(f"Many OAuth2 scopes: {total_scopes}")
            analysis['complexity_score'] += min(total_scopes // 5, 2)
    
    elif scheme_type == 'openIdConnect':
        analysis['factors'].append("OpenID Connect")
        analysis['complexity_score'] += 2
    
    elif scheme_type in ['http', 'apiKey']:
        analysis['factors'].append(f"Standard {scheme_type} auth")
        analysis['complexity_score'] += 0.5
    
    return analysis


def _generate_complexity_recommendations(complexity_analysis):
    """Generate recommendations based on complexity analysis."""
    recommendations = []
    overall_score = complexity_analysis['overall_score']
    
    if overall_score > 7:
        recommendations.append("HIGH COMPLEXITY: Consider implementing in phases, starting with core operations")
        recommendations.append("Extensive custom code and testing will be required")
    elif overall_score > 4:
        recommendations.append("MEDIUM COMPLEXITY: Plan for moderate custom code development")
        recommendations.append("Focus on parameter transformation and response handling")
    else:
        recommendations.append("LOW COMPLEXITY: Should convert well with minimal custom code")
    
    # Schema-specific recommendations
    schema_score = complexity_analysis['schema_complexity']['score']
    if schema_score > 5:
        recommendations.append("Complex schemas detected: Consider schema simplification or versioning")
    
    # Parameter-specific recommendations
    param_score = complexity_analysis['parameter_complexity']['score']
    if param_score > 5:
        recommendations.append("Complex parameters: Implement parameter transformation helpers")
    
    # Response-specific recommendations
    response_score = complexity_analysis['response_complexity']['score']
    if response_score > 5:
        recommendations.append("Complex responses: Consider response transformation and error handling")
    
    # Auth-specific recommendations
    auth_score = complexity_analysis['auth_complexity']['score']
    if auth_score > 5:
        recommendations.append("Complex authentication: Plan OAuth2/OpenID Connect implementation carefully")
    
    return recommendations


def _display_complexity_summary(complexity_analysis):
    """Display complexity analysis summary to user."""
    display("API Complexity Analysis Results:")
    display(f"├─ Overall Complexity Score: {complexity_analysis['overall_score']:.1f}/10")
    
    # Display individual scores
    display(f"├─ Schema Complexity: {complexity_analysis['schema_complexity']['score']:.1f}/10")
    if complexity_analysis['schema_complexity']['composition_usage'] > 0:
        display(f"│  ├─ Schema Composition (oneOf/anyOf/allOf): {complexity_analysis['schema_complexity']['composition_usage']} uses")
    if complexity_analysis['schema_complexity']['max_nesting_depth'] > 0:
        display(f"│  ├─ Maximum Nesting Depth: {complexity_analysis['schema_complexity']['max_nesting_depth']} levels")
    if complexity_analysis['schema_complexity']['conditional_properties'] > 0:
        display(f"│  └─ Conditional Properties: {complexity_analysis['schema_complexity']['conditional_properties']} detected")
    
    display(f"├─ Parameter Complexity: {complexity_analysis['parameter_complexity']['score']:.1f}/10")
    if complexity_analysis['parameter_complexity']['custom_styles'] > 0:
        display(f"│  ├─ Custom Parameter Styles: {complexity_analysis['parameter_complexity']['custom_styles']} found")
    if complexity_analysis['parameter_complexity']['array_serialization_complexity'] > 0:
        display(f"│  ├─ Complex Array Parameters: {complexity_analysis['parameter_complexity']['array_serialization_complexity']} found")
    if complexity_analysis['parameter_complexity']['parameter_dependencies'] > 0:
        display(f"│  └─ Parameter Dependencies: {complexity_analysis['parameter_complexity']['parameter_dependencies']} detected")
    
    display(f"├─ Response Complexity: {complexity_analysis['response_complexity']['score']:.1f}/10")
    if complexity_analysis['response_complexity']['multiple_content_types'] > 0:
        display(f"│  ├─ Multiple Content Types: {complexity_analysis['response_complexity']['multiple_content_types']} responses")
    if complexity_analysis['response_complexity']['link_relationships'] > 0:
        display(f"│  └─ Response Links: {complexity_analysis['response_complexity']['link_relationships']} found")
    
    display(f"└─ Authentication Complexity: {complexity_analysis['auth_complexity']['score']:.1f}/10")
    if complexity_analysis['auth_complexity']['multiple_schemes'] > 0:
        display(f"   ├─ Security Schemes: {complexity_analysis['auth_complexity']['multiple_schemes']} defined")
    if complexity_analysis['auth_complexity']['oauth_complexity'] > 0:
        display(f"   └─ OAuth2/OpenID Connect detected")
    
    # Display recommendations
    if complexity_analysis['recommendations']:
        display("Complexity-Based Recommendations:")
        for i, rec in enumerate(complexity_analysis['recommendations'], 1):
            display(f"└─ {rec}")


def _create_api_definition_file(openapi_data, connector_dir, incompatible_features=None, transformation_analysis=None, complexity_analysis=None):
    """
    Create the API definition (swagger) file.
    Convert OpenAPI 3.0 to Power Platform Swagger 2.0 format.
    """
    api_def_path = os.path.join(connector_dir, 'apiDefinition.swagger.json')
    
    try:
        # Convert OpenAPI 3.0 to Swagger 2.0 for Power Platform
        if openapi_data.get('openapi', '').startswith('3.'):
            swagger_data = _convert_openapi_to_swagger(openapi_data, complexity_analysis)
            
            # Phase 3B: Apply user-friendly parameter transformations to Swagger
            if transformation_analysis and transformation_analysis.get('transformable_operations'):
                swagger_data = _apply_parameter_transformations(swagger_data, transformation_analysis, openapi_data)
        else:
            # Already Swagger 2.0, just ensure it has Power Platform extensions
            swagger_data = _ensure_power_platform_format(openapi_data)
        
        with open(api_def_path, 'w', encoding='utf-8') as f:
            json.dump(swagger_data, f, indent=2, ensure_ascii=False)
    except Exception as e:
        display(f"Error converting OpenAPI specification: {e}")
        # Fallback: create basic template
        _create_fallback_swagger(openapi_data, api_def_path)


def _convert_openapi_to_swagger(openapi_spec, complexity_analysis=None):
    """Convert OpenAPI 3.0 specification to Swagger 2.0 format for Power Platform."""
    swagger_spec = {
        "swagger": "2.0",
        "info": _convert_info(openapi_spec.get('info', {})),
        "paths": {},
        "definitions": {},
        "parameters": {},
        "responses": {},
        "securityDefinitions": {},
        "security": [],
        "x-ms-connector-metadata": _create_connector_metadata(openapi_spec.get('info', {}), complexity_analysis)
    }
    
    # Convert servers to host/basePath
    host, base_path, schemes = _convert_servers(openapi_spec.get('servers', []))
    swagger_spec['host'] = host
    swagger_spec['basePath'] = base_path
    swagger_spec['schemes'] = schemes
    
    # Convert paths
    if 'paths' in openapi_spec:
        swagger_spec['paths'] = _convert_paths(openapi_spec['paths'])
    
    # Convert components to definitions and other root-level objects
    if 'components' in openapi_spec:
        _convert_components(openapi_spec['components'], swagger_spec)
    
    # Convert security
    if 'security' in openapi_spec:
        swagger_spec['security'] = _convert_security_requirements(openapi_spec['security'])
    
    # Set default consumes/produces
    swagger_spec['consumes'] = ['application/json']
    swagger_spec['produces'] = ['application/json']
    
    return swagger_spec


def _ensure_power_platform_format(swagger_data):
    """Ensure existing Swagger 2.0 has Power Platform required fields."""
    if 'x-ms-connector-metadata' not in swagger_data:
        swagger_data['x-ms-connector-metadata'] = _create_connector_metadata(swagger_data.get('info', {}))
    
    return swagger_data


def _create_fallback_swagger(openapi_data, api_def_path):
    """Create a basic Swagger template when conversion fails."""
    info = openapi_data.get('info', {})
    fallback_swagger = {
        "swagger": "2.0",
        "info": {
            "title": info.get('title', 'Converted API'),
            "version": info.get('version', '1.0'),
            "description": info.get('description', 'Converted from OpenAPI 3.0.')
        },
        "host": "example.com",
        "basePath": "/",
        "schemes": ["https"],
        "consumes": ["application/json"],
        "produces": ["application/json"],
        "paths": {},
        "definitions": {},
        "securityDefinitions": {},
        "security": [],
        "x-ms-connector-metadata": _create_connector_metadata(info)
    }
    
    with open(api_def_path, 'w', encoding='utf-8') as f:
        json.dump(fallback_swagger, f, indent=2, ensure_ascii=False)


def _convert_info(info):
    """Convert OpenAPI 3.0 info object to Swagger 2.0."""
    converted_info = {
        "title": info.get('title', 'Converted API'),
        "version": info.get('version', '1.0')
    }
    
    if 'description' in info:
        converted_info['description'] = info['description']
    
    if 'contact' in info:
        converted_info['contact'] = info['contact']
    
    if 'license' in info:
        converted_info['license'] = info['license']
        
    return converted_info


def _create_connector_metadata(info, complexity_analysis=None):
    """Create x-ms-connector-metadata from info object."""
    metadata = []
    
    # Try to extract website from contact or create default
    website = "https://example.com"
    if 'contact' in info and 'url' in info['contact']:
        website = info['contact']['url']
    
    metadata.extend([
        {"propertyName": "Website", "propertyValue": website},
        {"propertyName": "Privacy policy", "propertyValue": f"{website}/privacy"},
        {"propertyName": "Categories", "propertyValue": "Productivity"}
    ])
    
    # Phase 4: Add complexity analysis metadata
    if complexity_analysis:
        metadata.extend([
            {"propertyName": "API Complexity Score", "propertyValue": f"{complexity_analysis['overall_score']:.1f}/10"},
            {"propertyName": "Schema Complexity", "propertyValue": f"{complexity_analysis['schema_complexity']['score']:.1f}/10"},
            {"propertyName": "Parameter Complexity", "propertyValue": f"{complexity_analysis['parameter_complexity']['score']:.1f}/10"},
            {"propertyName": "Response Complexity", "propertyValue": f"{complexity_analysis['response_complexity']['score']:.1f}/10"},
            {"propertyName": "Auth Complexity", "propertyValue": f"{complexity_analysis['auth_complexity']['score']:.1f}/10"}
        ])
    
    return metadata


def _convert_servers(servers):
    """Convert OpenAPI 3.0 servers array to Swagger 2.0 host/basePath/schemes."""
    if not servers:
        return "example.com", "/", ["https"]
    
    # Use the first server
    server = servers[0]
    server_url = server.get('url', 'https://example.com/')
    
    # Parse the URL - need to import urllib.parse at top or use simple parsing
    if '://' in server_url:
        scheme_part, rest = server_url.split('://', 1)
        scheme = scheme_part
        
        if '/' in rest:
            host, path = rest.split('/', 1)
            base_path = '/' + path
        else:
            host = rest
            base_path = '/'
    else:
        scheme = 'https'
        if '/' in server_url:
            host, path = server_url.split('/', 1)
            base_path = '/' + path
        else:
            host = server_url or 'example.com'
            base_path = '/'
    
    return host, base_path, [scheme]


def _convert_paths(paths):
    """Convert OpenAPI 3.0 paths to Swagger 2.0."""
    converted_paths = {}
    
    for path_key, path_item in paths.items():
        converted_paths[path_key] = _convert_path_item(path_item)
    
    return converted_paths


def _convert_path_item(path_item):
    """Convert OpenAPI 3.0 path item to Swagger 2.0."""
    converted_path_item = {}
    
    # Convert operations (get, post, put, delete, etc.)
    http_methods = ['get', 'put', 'post', 'delete', 'options', 'head', 'patch']
    
    for method in http_methods:
        if method in path_item:
            converted_path_item[method] = _convert_operation(path_item[method])
    
    # Convert parameters (path-level parameters)
    if 'parameters' in path_item:
        converted_path_item['parameters'] = _convert_parameters(path_item['parameters'])
    
    return converted_path_item


def _convert_operation(operation):
    """Convert OpenAPI 3.0 operation to Swagger 2.0 with Power Platform enhancements."""
    converted_op = {}
    
    # Direct copies
    for field in ['operationId', 'summary', 'description', 'tags', 'deprecated']:
        if field in operation:
            converted_op[field] = operation[field]
    
    # Add Power Platform specific extensions
    _add_power_platform_operation_extensions(converted_op, operation)
    
    # Convert parameters
    if 'parameters' in operation:
        converted_op['parameters'] = _convert_parameters(operation['parameters'])
    
    # Convert requestBody to parameters
    if 'requestBody' in operation:
        body_params = _convert_request_body(operation['requestBody'])
        if 'parameters' not in converted_op:
            converted_op['parameters'] = []
        converted_op['parameters'].extend(body_params)
    
    # Convert responses (required in Swagger 2.0)
    if 'responses' in operation:
        converted_op['responses'] = _convert_responses(operation['responses'])
    else:
        # Default response if none provided
        converted_op['responses'] = {
            "200": {
                "description": "Success"
            }
        }
    
    # Convert security
    if 'security' in operation:
        converted_op['security'] = _convert_security_requirements(operation['security'])
    
    return converted_op


def _convert_parameters(parameters):
    """Convert OpenAPI 3.0 parameters to Swagger 2.0 with Power Platform enhancements."""
    converted_params = []
    
    for param in parameters:
        if '$ref' in param:
            # Handle reference
            converted_params.append(param)
            continue
            
        converted_param = {
            "name": param.get('name'),
            "in": param.get('in'),
            "required": param.get('required', False)
        }
        
        if 'description' in param:
            converted_param['description'] = param['description']
        
        # Add Power Platform specific extensions
        _add_power_platform_parameter_extensions(converted_param, param)
        
        # Convert schema to type/format for non-body parameters
        if 'schema' in param and param.get('in') != 'body':
            schema = param['schema']
            if 'type' in schema:
                converted_param['type'] = schema['type']
            if 'format' in schema:
                converted_param['format'] = schema['format']
            if 'enum' in schema:
                converted_param['enum'] = schema['enum']
                # Add dynamic values suggestion for enum parameters
                _add_enum_dynamic_values_suggestion(converted_param, param)
            if 'default' in schema:
                converted_param['default'] = schema['default']
        
        converted_params.append(converted_param)
    
    return converted_params


def _add_power_platform_parameter_extensions(converted_param, original_param):
    """Add Power Platform specific extensions to parameters."""
    param_name = converted_param.get('name', '')
    
    # Add x-ms-summary (friendly display name)
    if 'x-ms-summary' not in converted_param:
        converted_param['x-ms-summary'] = _generate_friendly_name(param_name)
    
    # Add x-ms-visibility based on parameter characteristics
    if param_name.lower() in ['limit', 'offset', 'page', 'pagesize', 'format', 'verbose']:
        converted_param['x-ms-visibility'] = 'advanced'
    elif param_name.lower() in ['apikey', 'token', 'authorization']:
        converted_param['x-ms-visibility'] = 'internal'
    elif converted_param.get('required', False):
        converted_param['x-ms-visibility'] = 'important'


def _add_enum_dynamic_values_suggestion(converted_param, original_param):
    """Add a comment suggesting dynamic values for enum parameters."""
    if 'enum' in converted_param:
        # Add a suggestion comment for the developer
        converted_param['x-ms-dynamic-values-suggestion'] = {
            "note": "Consider replacing this enum with x-ms-dynamic-list for live values",
            "example": {
                "x-ms-dynamic-list": {
                    "operationId": "GetEnumValues",
                    "itemsPath": "values",
                    "itemValuePath": "value",
                    "itemTitlePath": "displayName"
                }
            }
        }


def _generate_friendly_name(param_name):
    """Generate a friendly display name from parameter name."""
    # Convert camelCase or snake_case to Title Case
    name = re.sub(r'([a-z])([A-Z])', r'\1 \2', param_name)  # camelCase
    name = re.sub(r'[_-]', ' ', name)  # snake_case or kebab-case
    return ' '.join(word.capitalize() for word in name.split())


def _add_power_platform_operation_extensions(converted_op, original_op):
    """Add Power Platform specific extensions to operations."""
    operation_id = original_op.get('operationId', '')
    summary = original_op.get('summary', '')
    
    # Detect trigger operations
    if _is_trigger_operation(operation_id, summary):
        converted_op['x-ms-trigger'] = 'single'  # or 'batch' based on response type
        converted_op['x-ms-trigger-hint'] = f"This operation is triggered when {summary.lower()}"
    
    # Add visibility based on operation characteristics
    if any(keyword in operation_id.lower() for keyword in ['test', 'health', 'ping']):
        converted_op['x-ms-visibility'] = 'internal'
    elif any(keyword in operation_id.lower() for keyword in ['list', 'get', 'search']):
        converted_op['x-ms-visibility'] = 'important'
    
    # Add operation context for test connections
    if 'test' in operation_id.lower() or 'health' in operation_id.lower():
        converted_op['x-ms-operation-context'] = {
            "testConnection": True
        }


def _is_trigger_operation(operation_id, summary):
    """Detect if an operation is likely a trigger."""
    trigger_keywords = [
        'webhook', 'subscribe', 'trigger', 'event', 'notify', 'callback',
        'when', 'on_', 'upon', 'fired', 'created', 'updated', 'deleted'
    ]
    
    text_to_check = f"{operation_id} {summary}".lower()
    return any(keyword in text_to_check for keyword in trigger_keywords)


def _convert_request_body(request_body):
    """Convert OpenAPI 3.0 requestBody to Swagger 2.0 body parameter."""
    if '$ref' in request_body:
        return [request_body]
    
    body_params = []
    
    if 'content' in request_body:
        content = request_body['content']
        
        # Handle JSON content
        if 'application/json' in content:
            json_content = content['application/json']
            body_param = {
                "name": "body",
                "in": "body",
                "required": request_body.get('required', False)
            }
            
            if 'description' in request_body:
                body_param['description'] = request_body['description']
            
            if 'schema' in json_content:
                body_param['schema'] = json_content['schema']
            
            body_params.append(body_param)
        
        # Handle form data
        elif 'application/x-www-form-urlencoded' in content:
            form_content = content['application/x-www-form-urlencoded']
            if 'schema' in form_content and 'properties' in form_content['schema']:
                for prop_name, prop_schema in form_content['schema']['properties'].items():
                    form_param = {
                        "name": prop_name,
                        "in": "formData",
                        "type": prop_schema.get('type', 'string'),
                        "required": prop_name in form_content['schema'].get('required', [])
                    }
                    
                    if 'description' in prop_schema:
                        form_param['description'] = prop_schema['description']
                    
                    body_params.append(form_param)
    
    return body_params


def _convert_responses(responses):
    """Convert OpenAPI 3.0 responses to Swagger 2.0."""
    converted_responses = {}
    
    for status_code, response in responses.items():
        if '$ref' in response:
            converted_responses[status_code] = response
            continue
        
        converted_response = {}
        
        if 'description' in response:
            converted_response['description'] = response['description']
        
        # Convert content to schema
        if 'content' in response:
            content = response['content']
            
            # Use the first content type (usually application/json)
            for media_type, media_obj in content.items():
                if 'schema' in media_obj:
                    converted_response['schema'] = media_obj['schema']
                    break
        
        converted_responses[status_code] = converted_response
    
    return converted_responses


def _convert_components(components, swagger_spec):
    """Convert OpenAPI 3.0 components to Swagger 2.0 definitions and other objects."""
    
    # Convert schemas to definitions
    if 'schemas' in components:
        swagger_spec['definitions'].update(_convert_schemas(components['schemas']))
    
    # Convert parameters
    if 'parameters' in components:
        swagger_spec['parameters'].update(_convert_parameters_definitions(components['parameters']))
    
    # Convert responses
    if 'responses' in components:
        swagger_spec['responses'].update(_convert_responses_definitions(components['responses']))
    
    # Convert security schemes
    if 'securitySchemes' in components:
        swagger_spec['securityDefinitions'].update(_convert_security_schemes(components['securitySchemes']))


def _convert_schemas(schemas):
    """Convert OpenAPI 3.0 schemas to Swagger 2.0 definitions."""
    converted_schemas = {}
    
    for schema_name, schema in schemas.items():
        converted_schemas[schema_name] = _convert_schema(schema)
    
    return converted_schemas


def _convert_schema(schema):
    """Convert OpenAPI 3.0 schema to Swagger 2.0."""
    if '$ref' in schema:
        return schema
    
    converted_schema = {}
    
    # Direct copies for compatible fields
    compatible_fields = [
        'type', 'format', 'title', 'description', 'default', 'enum',
        'minimum', 'maximum', 'exclusiveMinimum', 'exclusiveMaximum',
        'minLength', 'maxLength', 'pattern', 'minItems', 'maxItems',
        'uniqueItems', 'minProperties', 'maxProperties', 'required',
        'multipleOf', 'example'
    ]
    
    for field in compatible_fields:
        if field in schema:
            converted_schema[field] = schema[field]
    
    # Convert nested schemas
    if 'properties' in schema:
        converted_schema['properties'] = {}
        for prop_name, prop_schema in schema['properties'].items():
            converted_schema['properties'][prop_name] = _convert_schema(prop_schema)
    
    if 'items' in schema:
        converted_schema['items'] = _convert_schema(schema['items'])
    
    if 'additionalProperties' in schema:
        if isinstance(schema['additionalProperties'], dict):
            converted_schema['additionalProperties'] = _convert_schema(schema['additionalProperties'])
        else:
            converted_schema['additionalProperties'] = schema['additionalProperties']
    
    # Convert composition keywords
    for comp_keyword in ['allOf', 'anyOf', 'oneOf']:
        if comp_keyword in schema:
            converted_schema[comp_keyword] = [_convert_schema(s) for s in schema[comp_keyword]]
    
    if 'not' in schema:
        converted_schema['not'] = _convert_schema(schema['not'])
    
    return converted_schema


def _convert_parameters_definitions(parameters):
    """Convert OpenAPI 3.0 parameters components to Swagger 2.0."""
    converted_params = {}
    
    for param_name, param in parameters.items():
        converted_params[param_name] = _convert_parameters([param])[0]
    
    return converted_params


def _convert_responses_definitions(responses):
    """Convert OpenAPI 3.0 responses components to Swagger 2.0."""
    converted_responses = {}
    
    for response_name, response in responses.items():
        converted_responses[response_name] = _convert_responses({response_name: response})[response_name]
    
    return converted_responses


def _convert_security_schemes(security_schemes):
    """Convert OpenAPI 3.0 securitySchemes to Swagger 2.0 securityDefinitions."""
    converted_schemes = {}
    
    for scheme_name, scheme in security_schemes.items():
        if '$ref' in scheme:
            converted_schemes[scheme_name] = scheme
            continue
        
        converted_scheme = {}
        scheme_type = scheme.get('type')
        
        if scheme_type == 'apiKey':
            converted_scheme = {
                "type": "apiKey",
                "name": scheme.get('name'),
                "in": scheme.get('in')
            }
        elif scheme_type == 'http':
            if scheme.get('scheme') == 'basic':
                converted_scheme = {"type": "basic"}
            elif scheme.get('scheme') == 'bearer':
                # Swagger 2.0 doesn't have native bearer token support
                # Convert to API key in header
                converted_scheme = {
                    "type": "apiKey",
                    "name": "Authorization",
                    "in": "header"
                }
        elif scheme_type == 'oauth2':
            converted_scheme = {
                "type": "oauth2"
            }
            
            # Convert flows
            if 'flows' in scheme:
                flows = scheme['flows']
                
                if 'implicit' in flows:
                    converted_scheme['flow'] = 'implicit'
                    converted_scheme['authorizationUrl'] = flows['implicit'].get('authorizationUrl')
                    converted_scheme['scopes'] = flows['implicit'].get('scopes', {})
                elif 'authorizationCode' in flows:
                    converted_scheme['flow'] = 'accessCode'
                    converted_scheme['authorizationUrl'] = flows['authorizationCode'].get('authorizationUrl')
                    converted_scheme['tokenUrl'] = flows['authorizationCode'].get('tokenUrl')
                    converted_scheme['scopes'] = flows['authorizationCode'].get('scopes', {})
                elif 'password' in flows:
                    converted_scheme['flow'] = 'password'
                    converted_scheme['tokenUrl'] = flows['password'].get('tokenUrl')
                    converted_scheme['scopes'] = flows['password'].get('scopes', {})
                elif 'clientCredentials' in flows:
                    converted_scheme['flow'] = 'application'
                    converted_scheme['tokenUrl'] = flows['clientCredentials'].get('tokenUrl')
                    converted_scheme['scopes'] = flows['clientCredentials'].get('scopes', {})
        
        if 'description' in scheme:
            converted_scheme['description'] = scheme['description']
        
        converted_schemes[scheme_name] = converted_scheme
    
    return converted_schemes


def _convert_security_requirements(security_requirements):
    """Convert OpenAPI 3.0 security requirements to Swagger 2.0."""
    # Security requirements format is the same in both versions
    return security_requirements


def _create_api_properties_file(openapi_data, connector_dir, incompatible_features=None, complexity_analysis=None):
    """
    Create the API properties file with basic properties and custom code operation markers.
    """
    api_props_path = os.path.join(connector_dir, 'apiProperties.json')
    
    # Extract some basic info from OpenAPI
    title = openapi_data.get('info', {}).get('title', 'Custom Connector')
    description = openapi_data.get('info', {}).get('description', '')
    
    # Create basic API properties structure
    api_properties = {
        "properties": {
            "capabilities": [],
            "connectionParameters": {
                "api_key": {
                    "type": "securestring",
                    "uiDefinition": {
                        "constraints": {
                            "clearText": False,
                            "required": "true",
                            "tabIndex": 2
                        },
                        "description": "The API key for this service",
                        "displayName": "API Key",
                        "tooltip": "Provide your API key"
                    }
                }
            },
            "iconBrandColor": "#007EE6",
            "scriptOperations": [],
            "policyTemplateInstances": []
        }
    }
    
    # Phase 1: Mark operations requiring custom code
    if incompatible_features and incompatible_features.get('custom_code_operations'):
        script_operations = []
        for operation_id in incompatible_features['custom_code_operations']:
            features = incompatible_features['operation_features'].get(operation_id, [])
            script_operations.append(operation_id)  # Just the operation ID string
        
        # Only include scriptOperations if there are operations that need custom code
        api_properties['properties']['scriptOperations'] = script_operations
        
        # Add metadata about incompatible features
        api_properties['x-ms-openapi-conversion'] = {
            "version": "1.0",
            "conversionDate": "2025-08-06",  # Current date
            "incompatibleFeatures": {
                "globalFeatures": incompatible_features.get('global_features', []),
                "operationFeatures": incompatible_features.get('operation_features', {}),
                "customCodeRequired": len(incompatible_features.get('custom_code_operations', [])) > 0,
                "featureSummary": incompatible_features.get('feature_summary', {})
            }
        }
    else:
        # For APIs with no incompatible features, set empty array to indicate no operations use custom code
        api_properties['properties']['scriptOperations'] = []
    
    with open(api_props_path, 'w', encoding='utf-8') as f:
        json.dump(api_properties, f, indent=2, ensure_ascii=False)


def _create_default_icon_file(connector_dir):
    """
    Create a placeholder for the icon file.
    For now, we'll create a text file indicating where to place the icon.
    """
    icon_placeholder_path = os.path.join(connector_dir, 'icon_placeholder.txt')
    
    placeholder_content = """Icon Placeholder

Place your connector icon file here as 'icon.png'.

The icon should be:
- PNG format
- 32x32 pixels recommended
- Represents your API/service
- Will be displayed in the Power Platform connector gallery
"""
    
    with open(icon_placeholder_path, 'w', encoding='utf-8') as f:
        f.write(placeholder_content)


def _create_settings_file(connector_name, connector_dir):
    """
    Create a basic settings.json file for the connector.
    """
    settings_path = os.path.join(connector_dir, 'settings.json')
    
    settings_data = {
        "connectorId": "CONNECTOR-ID-WILL-BE-GENERATED",
        "environment": "ENVIRONMENT-GUID-REQUIRED",
        "apiProperties": "apiProperties.json",
        "apiDefinition": "apiDefinition.swagger.json",
        "icon": "icon.png",
        "powerAppsApiVersion": "2016-11-01",
        "powerAppsUrl": "https://api.powerapps.com"
    }
    
    with open(settings_path, 'w', encoding='utf-8') as f:
        json.dump(settings_data, f, indent=2, ensure_ascii=False)


def _create_custom_code_template(openapi_data, connector_dir, incompatible_features=None, transformation_analysis=None):
    """
    Create a custom code template file with examples and specific handlers for incompatible features.
    """
    code_template_path = os.path.join(connector_dir, 'script.csx')
    
    # Extract operation IDs for the template
    operation_ids = []
    paths = openapi_data.get('paths', {})
    for path, methods in paths.items():
        for method, operation in methods.items():
            if isinstance(operation, dict) and 'operationId' in operation:
                operation_ids.append(operation['operationId'])
    
    # Phase 1: Generate custom code with specific handling for incompatible features
    incompatible_ops = incompatible_features.get('custom_code_operations', set()) if incompatible_features else set()
    
    # Only generate custom code if there are operations that need it
    if not incompatible_ops:
        # Create a minimal script template for APIs with no incompatible features
        code_template = f'''// Custom Code Template for {openapi_data.get('info', {}).get('title', 'API')}
// This file provides a starting point for implementing custom transformations
// Reference: https://learn.microsoft.com/en-us/connectors/custom-connectors/write-code

// No OpenAPI 3.0 incompatible features detected - all operations can be handled natively by Power Platform
// This template is provided for future customizations if needed

public class Script : ScriptBase
{{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {{
        // All operations for this API can be handled natively by Power Platform
        // Custom code is not required but this template is available for future enhancements
        
        // Log the operation being executed
        this.Context.Logger?.LogInformation($"Executing operation: {{this.Context.OperationId}} (no custom code required)");
        
        // Forward all operations without modification
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken);
    }}
    
    // Add custom operation handlers here if needed in the future
}}'''
    else:
        # Generate full template with specific handlers for incompatible operations
        code_template = f'''// Custom Code Template for {openapi_data.get('info', {}).get('title', 'API')}
// This file provides a starting point for implementing custom transformations
// Reference: https://learn.microsoft.com/en-us/connectors/custom-connectors/write-code

{_generate_incompatible_features_comment(incompatible_features, transformation_analysis)}

public class Script : ScriptBase
{{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {{
        // Log the operation being executed
        this.Context.Logger?.LogInformation($"Executing operation: {{this.Context.OperationId}}");
        
        // Apply parameter transformations before sending request
        if (!await ApplyParameterTransformations())
        {{
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {{
                Content = new StringContent(
                    "{{\\"error\\": \\"Parameter transformation failed\\"}}", 
                    Encoding.UTF8, 
                    "application/json"
                )
            }};
        }}
        
        // Handle different operations
        switch (this.Context.OperationId)
        {{
{_generate_operation_handlers(list(incompatible_ops), incompatible_features)}
            default:
                // Forward all other operations without modification
                return await this.Context.SendAsync(this.Context.Request, this.CancellationToken);
        }}
    }}

{_generate_transformation_methods(transformation_analysis, openapi_data) if transformation_analysis else ""}
{_generate_operation_methods(list(incompatible_ops), incompatible_features)}
{_generate_incompatible_feature_handlers(incompatible_features)}
    
    /// <summary>
    /// Example: Transform request before sending to backend
    /// </summary>
    private async Task<HttpResponseMessage> TransformRequest()
    {{
        // Read and modify request content
        var contentString = await this.Context.Request.Content.ReadAsStringAsync();
        var requestData = JObject.Parse(contentString);
        
        // Example: Add timestamp to request
        requestData["timestamp"] = DateTime.UtcNow.ToString("O");
        
        // Update request content
        this.Context.Request.Content = CreateJsonContent(requestData.ToString());
        
        // Forward to backend
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken);
    }}
    
    /// <summary>
    /// Example: Transform response from backend
    /// </summary>
    private async Task<HttpResponseMessage> TransformResponse()
    {{
        // Get response from backend
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken);
        
        if (response.IsSuccessStatusCode)
        {{
            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JObject.Parse(responseString);
            
            // Example: Wrap response with metadata
            var wrappedResponse = new JObject
            {{
                ["data"] = responseData,
                ["metadata"] = new JObject
                {{
                    ["processed_at"] = DateTime.UtcNow,
                    ["operation"] = this.Context.OperationId,
                    ["correlation_id"] = this.Context.CorrelationId
                }}
            }};
            
            response.Content = CreateJsonContent(wrappedResponse.ToString());
        }}
        
        return response;
    }}
    
    /// <summary>
    /// Example: Validate input and return custom error
    /// </summary>
    private async Task<HttpResponseMessage> ValidateAndForward()
    {{
        try
        {{
            var contentString = await this.Context.Request.Content.ReadAsStringAsync();
            var requestData = JObject.Parse(contentString);
            
            // Example validation: Check required field
            if (string.IsNullOrEmpty((string)requestData["email"]))
            {{
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                errorResponse.Content = CreateJsonContent(new JObject
                {{
                    ["error"] = "Email is required",
                    ["code"] = "VALIDATION_ERROR",
                    ["operation"] = this.Context.OperationId
                }}.ToString());
                return errorResponse;
            }}
            
            // Forward valid requests
            return await this.Context.SendAsync(this.Context.Request, this.CancellationToken);
        }}
        catch (Exception ex)
        {{
            var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.Content = CreateJsonContent(new JObject
            {{
                ["error"] = "Request processing failed",
                ["details"] = ex.Message,
                ["operation"] = this.Context.OperationId
            }}.ToString());
            return errorResponse;
        }}
    }}
}}'''
    
    with open(code_template_path, 'w', encoding='utf-8') as f:
        f.write(code_template)



def _create_enhancement_recommendations(openapi_data, connector_dir, complexity_analysis=None):
    """
    Analyze the OpenAPI spec and create recommendations for Power Platform enhancements.
    """
    recommendations_path = os.path.join(connector_dir, 'ENHANCEMENT_RECOMMENDATIONS.md')
    
    recommendations = []
    recommendations.append("# Power Platform Connector Enhancement Recommendations")
    recommendations.append("")
    recommendations.append("This file contains recommendations for enhancing your connector with Power Platform specific features.")
    recommendations.append("")
    
    # Analyze for dynamic values opportunities
    dynamic_opportunities = _analyze_dynamic_opportunities(openapi_data)
    if dynamic_opportunities:
        recommendations.append("## Dynamic Values Opportunities")
        recommendations.append("")
        recommendations.append("The following parameters could benefit from dynamic values (dropdown lists populated from API calls):")
        recommendations.append("")
        for opportunity in dynamic_opportunities:
            recommendations.append(f"- **{opportunity['operation']}** → **{opportunity['parameter']}**")
            recommendations.append(f"  - {opportunity['description']}")
            recommendations.append(f"  - Consider adding `x-ms-dynamic-list` extension")
            recommendations.append("")
    
    # Analyze for dynamic schema opportunities
    schema_opportunities = _analyze_schema_opportunities(openapi_data)
    if schema_opportunities:
        recommendations.append("## Dynamic Schema Opportunities")
        recommendations.append("")
        recommendations.append("The following operations could benefit from dynamic schemas:")
        recommendations.append("")
        for opportunity in schema_opportunities:
            recommendations.append(f"- **{opportunity['operation']}**")
            recommendations.append(f"  - {opportunity['description']}")
            recommendations.append(f"  - Consider adding `x-ms-dynamic-properties` extension")
            recommendations.append("")
    
    # Analyze for custom code opportunities
    code_opportunities = _analyze_custom_code_opportunities(openapi_data)
    if code_opportunities:
        recommendations.append("## Custom Code Opportunities")
        recommendations.append("")
        recommendations.append("The following operations could benefit from custom code:")
        recommendations.append("")
        for opportunity in code_opportunities:
            recommendations.append(f"- **{opportunity['operation']}**")
            recommendations.append(f"  - {opportunity['description']}")
            recommendations.append(f"  - Use case: {opportunity['use_case']}")
            recommendations.append("")
    
    # Phase 4: Add complexity analysis results
    if complexity_analysis:
        recommendations.append("## API Complexity Analysis")
        recommendations.append("")
        recommendations.append(f"**Overall Complexity Score:** {complexity_analysis['overall_score']:.1f}/10")
        recommendations.append("")
        
        # Add complexity breakdown
        recommendations.append("### Complexity Breakdown:")
        recommendations.append(f"- **Schema Complexity:** {complexity_analysis['schema_complexity']['score']:.1f}/10")
        if complexity_analysis['schema_complexity']['composition_usage'] > 0:
            recommendations.append(f"  - Schema composition (oneOf/anyOf/allOf): {complexity_analysis['schema_complexity']['composition_usage']} uses")
        if complexity_analysis['schema_complexity']['max_nesting_depth'] > 0:
            recommendations.append(f"  - Maximum nesting depth: {complexity_analysis['schema_complexity']['max_nesting_depth']} levels")
        if complexity_analysis['schema_complexity']['conditional_properties'] > 0:
            recommendations.append(f"  - Conditional properties: {complexity_analysis['schema_complexity']['conditional_properties']} detected")
        
        recommendations.append(f"- **Parameter Complexity:** {complexity_analysis['parameter_complexity']['score']:.1f}/10")
        if complexity_analysis['parameter_complexity']['custom_styles'] > 0:
            recommendations.append(f"  - Custom parameter styles: {complexity_analysis['parameter_complexity']['custom_styles']} found")
        if complexity_analysis['parameter_complexity']['array_serialization_complexity'] > 0:
            recommendations.append(f"  - Complex array parameters: {complexity_analysis['parameter_complexity']['array_serialization_complexity']} found")
        if complexity_analysis['parameter_complexity']['parameter_dependencies'] > 0:
            recommendations.append(f"  - Parameter dependencies: {complexity_analysis['parameter_complexity']['parameter_dependencies']} detected")
        
        recommendations.append(f"- **Response Complexity:** {complexity_analysis['response_complexity']['score']:.1f}/10")
        if complexity_analysis['response_complexity']['multiple_content_types'] > 0:
            recommendations.append(f"  - Multiple content types: {complexity_analysis['response_complexity']['multiple_content_types']} responses")
        if complexity_analysis['response_complexity']['link_relationships'] > 0:
            recommendations.append(f"  - Response links: {complexity_analysis['response_complexity']['link_relationships']} found")
        
        recommendations.append(f"- **Authentication Complexity:** {complexity_analysis['auth_complexity']['score']:.1f}/10")
        if complexity_analysis['auth_complexity']['multiple_schemes'] > 0:
            recommendations.append(f"  - Security schemes: {complexity_analysis['auth_complexity']['multiple_schemes']} defined")
        if complexity_analysis['auth_complexity']['oauth_complexity'] > 0:
            recommendations.append("  - OAuth2/OpenID Connect detected")
        
        recommendations.append("")
        
        # Add complexity-based recommendations
        if complexity_analysis['recommendations']:
            recommendations.append("### Implementation Recommendations:")
            for rec in complexity_analysis['recommendations']:
                recommendations.append(f"- {rec}")
            recommendations.append("")
    
    # General Power Platform recommendations
    recommendations.append("## General Power Platform Enhancements")
    recommendations.append("")
    recommendations.append("### OpenAPI Extensions to Consider:")
    recommendations.append("- `x-ms-summary`: Friendly display names for operations and parameters")
    recommendations.append("- `x-ms-visibility`: Control which parameters are shown ('important', 'advanced', 'internal')")
    recommendations.append("- `x-ms-trigger`: Mark operations as triggers for Power Automate")
    recommendations.append("- `x-ms-capabilities`: Add test connection and other capabilities")
    recommendations.append("")
    recommendations.append("### Authentication Enhancements:")
    recommendations.append("- Review connection parameters in apiProperties.json")
    recommendations.append("- Consider OAuth2 if supported by your API")
    recommendations.append("- Add proper error handling for authentication failures")
    recommendations.append("")
    recommendations.append("### Documentation:")
    recommendations.append("- Add detailed descriptions to all operations and parameters")
    recommendations.append("- Provide examples in OpenAPI specification")
    recommendations.append("- Create help documentation for end users")
    recommendations.append("")
    recommendations.append("---")
    recommendations.append("For more information, see:")
    recommendations.append("- [OpenAPI Extensions](https://learn.microsoft.com/en-us/connectors/custom-connectors/openapi-extensions)")
    recommendations.append("- [Dynamic Values](https://learn.microsoft.com/en-us/connectors/custom-connectors/openapi-extensions#use-dynamic-values)")
    recommendations.append("- [Custom Code](https://learn.microsoft.com/en-us/connectors/custom-connectors/write-code)")
    
    with open(recommendations_path, 'w', encoding='utf-8') as f:
        f.write('\n'.join(recommendations))


def _analyze_dynamic_opportunities(openapi_data):
    """Analyze OpenAPI spec for dynamic values opportunities."""
    opportunities = []
    paths = openapi_data.get('paths', {})
    
    # Look for operations that return lists (could be used for dynamic values)
    list_operations = []
    for path, methods in paths.items():
        for method, operation in methods.items():
            if not isinstance(operation, dict):
                continue
                
            # Check if operation returns an array
            responses = operation.get('responses', {})
            for status, response in responses.items():
                if status.startswith('2'):  # Success response
                    schema = None
                    if 'content' in response:
                        json_content = response.get('content', {}).get('application/json', {})
                        schema = json_content.get('schema', {})
                    elif 'schema' in response:  # Swagger 2.0 format
                        schema = response['schema']
                    
                    if schema and schema.get('type') == 'array':
                        list_operations.append({
                            'operation_id': operation.get('operationId'),
                            'path': path,
                            'method': method,
                            'description': operation.get('summary', operation.get('description', ''))
                        })
    
    # Look for parameters that could use these list operations
    for path, methods in paths.items():
        for method, operation in methods.items():
            if not isinstance(operation, dict):
                continue
                
            parameters = operation.get('parameters', [])
            for param in parameters:
                param_name = param.get('name', '')
                param_type = param.get('type', '')
                
                # Look for parameters that might benefit from dynamic values
                if any(keyword in param_name.lower() for keyword in ['id', 'type', 'category', 'status', 'region']):
                    opportunities.append({
                        'operation': operation.get('operationId', f"{method.upper()} {path}"),
                        'parameter': param_name,
                        'description': f"Parameter '{param_name}' could use dynamic values from list operations"
                    })
    
    return opportunities


def _analyze_schema_opportunities(openapi_data):
    """Analyze OpenAPI spec for dynamic schema opportunities."""
    opportunities = []
    paths = openapi_data.get('paths', {})
    
    for path, methods in paths.items():
        for method, operation in methods.items():
            if not isinstance(operation, dict):
                continue
                
            # Look for operations with complex request/response schemas
            has_complex_schema = False
            
            # Check request body
            request_body = operation.get('requestBody', {})
            if request_body:
                content = request_body.get('content', {})
                json_content = content.get('application/json', {})
                schema = json_content.get('schema', {})
                
                # Check if schema has multiple properties that could vary
                if schema.get('type') == 'object' and len(schema.get('properties', {})) > 3:
                    has_complex_schema = True
            
            # Check parameters for body parameters (Swagger 2.0)
            parameters = operation.get('parameters', [])
            for param in parameters:
                if param.get('in') == 'body' and 'schema' in param:
                    schema = param['schema']
                    if schema.get('type') == 'object' and len(schema.get('properties', {})) > 3:
                        has_complex_schema = True
            
            if has_complex_schema:
                opportunities.append({
                    'operation': operation.get('operationId', f"{method.upper()} {path}"),
                    'description': "Complex schema that could benefit from dynamic properties based on user selections"
                })
    
    return opportunities


def _analyze_custom_code_opportunities(openapi_data):
    """Analyze OpenAPI spec for custom code opportunities."""
    opportunities = []
    paths = openapi_data.get('paths', {})
    
    for path, methods in paths.items():
        for method, operation in methods.items():
            if not isinstance(operation, dict):
                continue
                
            operation_id = operation.get('operationId', f"{method.upper()} {path}")
            
            # Look for operations that might need transformation
            reasons = []
            
            # Check for complex parameter validation
            parameters = operation.get('parameters', [])
            if len(parameters) > 5:
                reasons.append("Complex parameter validation")
            
            # Check for multiple content types
            request_body = operation.get('requestBody', {})
            content_types = list(request_body.get('content', {}).keys())
            if len(content_types) > 1:
                reasons.append("Multiple content type handling")
            
            # Check for complex responses
            responses = operation.get('responses', {})
            if len(responses) > 3:
                reasons.append("Complex response transformation")
            
            # Look for operations that might need authentication handling
            if 'security' in operation or any(keyword in path.lower() for keyword in ['/auth', '/token', '/login']):
                reasons.append("Authentication handling")
            
            if reasons:
                opportunities.append({
                    'operation': operation_id,
                    'description': f"Operation with {len(parameters)} parameters and {len(responses)} response types",
                    'use_case': ', '.join(reasons)
                })
    
    return opportunities


def _generate_incompatible_features_comment(incompatible_features, transformation_analysis=None):
    """Generate comment block describing incompatible features that require custom code."""
    if not incompatible_features or not incompatible_features.get('custom_code_operations'):
        return ""
    
    comment = "/*\n * INCOMPATIBLE FEATURES DETECTED - CUSTOM CODE REQUIRED\n"
    comment += " * This API uses OpenAPI 3.0 features that cannot be directly converted to Swagger 2.0:\n *\n"
    
    # Phase 2: Add structured metadata as parseable comments
    comment += " * === METADATA (JSON) ===\n"
    comment += " * {\n"
    comment += f' *   "conversionVersion": "1.0",\n'
    comment += f' *   "conversionDate": "2025-08-06",\n'
    comment += f' *   "totalIncompatibleOperations": {len(incompatible_features.get("custom_code_operations", []))},\n'
    comment += f' *   "globalIncompatibleFeatures": {json.dumps(incompatible_features.get("global_features", []))},\n'
    comment += f' *   "operationIncompatibleFeatures": {json.dumps(incompatible_features.get("operation_features", {}))},\n'
    comment += f' *   "featureSummary": {json.dumps(incompatible_features.get("feature_summary", {}), indent=4).replace(chr(10), chr(10) + " *     ")},\n'
    
    # Phase 3A: Add transformation analysis metadata
    if transformation_analysis:
        comment += f' *   "parameterTransformations": {{\n'
        comment += f' *     "complexityScore": {transformation_analysis.get("complexity_score", 0)},\n'
        comment += f' *     "operationsWithTransformations": {transformation_analysis["summary"]["operations_with_transformations"]},\n'
        comment += f' *     "totalTransformations": {transformation_analysis["summary"]["total_transformations"]},\n'
        comment += f' *     "transformationTypes": {json.dumps(transformation_analysis["summary"]["transformation_types"])},\n'
        comment += f' *     "transformationPatterns": {json.dumps(transformation_analysis["transformation_patterns"])},\n'
        comment += f' *     "operationDetails": {json.dumps(transformation_analysis["transformable_operations"], indent=6).replace(chr(10), chr(10) + " *       ")}\n'
        comment += f' *   }}\n'
    else:
        comment += f' *   "parameterTransformations": null\n'
    
    comment += " * }\n"
    comment += " * === END METADATA ===\n *\n"
    
    # Global features
    global_features = incompatible_features.get('global_features', [])
    if global_features:
        comment += " * Global Features Requiring Custom Code:\n"
        for feature in global_features:
            comment += f" *   - {feature}: {_get_feature_description(feature)}\n"
        comment += " *\n"
    
    # Phase 3A: Add transformation analysis summary
    if transformation_analysis and transformation_analysis.get('transformable_operations'):
        comment += " * PARAMETER TRANSFORMATION OPPORTUNITIES:\n"
        comment += f" * Found {transformation_analysis['summary']['operations_with_transformations']} operations with {transformation_analysis['summary']['total_transformations']} parameter transformations\n"
        comment += f" * Complexity Score: {transformation_analysis['complexity_score']}/10\n"
        comment += " *\n"
        
        # List transformation types
        if transformation_analysis['transformation_patterns']:
            comment += " * Transformation Types Detected:\n"
            for transform_type in transformation_analysis['transformation_patterns']:
                count = transformation_analysis['summary']['transformation_types'].get(transform_type, 0)
                comment += f" *   - {transform_type}: {count} instances - {_get_transformation_description(transform_type)}\n"
            comment += " *\n"
        
        # List operations with transformations
        comment += " * Operations with Parameter Transformations:\n"
        for op_id, details in transformation_analysis['transformable_operations'].items():
            comment += f" *   - {op_id} ({details['method']} {details['path']}):\n"
            comment += f" *     * {details['user_friendly_count']} parameters can be made more user-friendly\n"
            comment += f" *     * Complexity reduction score: {details['complexity_reduction']}\n"
            for transform in details['transformations']:
                comment += f" *     * {transform['parameter']} ({transform['location']}): {transform['description']}\n"
        comment += " *\n"
    
    # Operation-specific features
    operation_features = incompatible_features.get('operation_features', {})
    if operation_features:
        comment += " * Operations Requiring Custom Code:\n"
        for op_id, features in operation_features.items():
            comment += f" *   - {op_id}:\n"
            for feature in features:
                comment += f" *     * {feature}: {_get_feature_description(feature)}\n"
        comment += " *\n"
    
    comment += " * The methods below provide specific handling for these incompatible features.\n"
    comment += " * Each feature type has dedicated helper methods for implementation.\n"
    comment += " */\n"
    
    return comment


def _generate_transformation_methods(transformation_analysis, openapi_data):
    """
    Generate specific transformation methods for parameter conversion.
    """
    if not transformation_analysis or not transformation_analysis.get('transformable_operations'):
        return ""
    
    display("Generating transformation code for parameter conversions...")
    
    methods = """
    /// <summary>
    /// Apply parameter transformations before sending request to backend API
    /// </summary>
    private async Task<bool> ApplyParameterTransformations()
    {
        try
        {
            // Get operation-specific transformation requirements
            var transformations = GetOperationTransformations(this.Context.OperationId);
            if (transformations == null || transformations.Count == 0)
            {
                return true; // No transformations needed
            }
            
            this.Context.Logger?.LogInformation($"Applying {transformations.Count} parameter transformations for {this.Context.OperationId}");
            
            // Apply each transformation
            foreach (var transformation in transformations)
            {
                if (!await ApplyTransformation(transformation))
                {
                    this.Context.Logger?.LogError($"Failed to apply transformation: {transformation.Type} for parameter {transformation.Parameter}");
                    return false;
                }
            }
            
            return true;
        }
        catch (Exception ex)
        {
            this.Context.Logger?.LogError($"Parameter transformation failed: {ex.Message}");
            return false;
        }
    }
    
    /// <summary>
    /// Get transformation requirements for a specific operation
    /// </summary>
    private List<ParameterTransformation> GetOperationTransformations(string operationId)
    {
        var transformations = new List<ParameterTransformation>();
        
        // Operation-specific transformation mapping
        switch (operationId)
        {"""
    
    # Generate transformation cases for each operation
    for op_id, details in transformation_analysis['transformable_operations'].items():
        display(f"└─ Generating transformations for operation: {op_id}")
        methods += f"""
            case "{op_id}":"""
        
        # Group transformations by type for better organization
        transformations_by_type = {}
        for transform in details['transformations']:
            transform_type = transform['type']
            if transform_type not in transformations_by_type:
                transformations_by_type[transform_type] = []
            transformations_by_type[transform_type].append(transform)
        
        # Generate transformation entries
        for transform_type, transforms in transformations_by_type.items():
            display(f"   ├─ {transform_type}: {len(transforms)} parameter(s)")
            for transform in transforms:
                param_name = transform['parameter']
                location = transform['location']
                methods += f"""
                transformations.Add(new ParameterTransformation
                {{
                    Type = "{transform_type}",
                    Parameter = "{param_name}",
                    Location = "{location}",
                    OriginalSchema = @"{_escape_json_for_csharp(transform.get('original_schema', {}))}"
                }});"""
        
        methods += """
                break;"""
    
    methods += """
        }
        
        return transformations;
    }
    
    /// <summary>
    /// Apply a single parameter transformation
    /// </summary>
    private async Task<bool> ApplyTransformation(ParameterTransformation transformation)
    {
        try
        {
            switch (transformation.Type)
            {
                case "schema_composition":
                    return await ApplySchemaCompositionTransformation(transformation);
                case "object_flattening":
                    return await ApplyObjectFlatteningTransformation(transformation);
                case "array_simplification":
                    return await ApplyArraySimplificationTransformation(transformation);
                case "enum_to_dynamic":
                    return await ApplyEnumDynamicTransformation(transformation);
                case "nested_flattening":
                    return await ApplyNestedFlatteningTransformation(transformation);
                case "body_composition":
                    return await ApplyBodyCompositionTransformation(transformation);
                default:
                    this.Context.Logger?.LogWarning($"Unknown transformation type: {transformation.Type}");
                    return true; // Continue processing
            }
        }
        catch (Exception ex)
        {
            this.Context.Logger?.LogError($"Transformation failed for {transformation.Parameter}: {ex.Message}");
            return false;
        }
    }
"""
    
    # Generate specific transformation method implementations
    methods += _generate_schema_composition_methods(transformation_analysis)
    methods += _generate_object_flattening_methods(transformation_analysis)
    methods += _generate_array_simplification_methods(transformation_analysis)
    methods += _generate_enum_dynamic_methods(transformation_analysis)
    methods += _generate_nested_flattening_methods(transformation_analysis)
    methods += _generate_body_composition_methods(transformation_analysis)
    
    # Add parameter manipulation helper methods
    methods += _generate_parameter_helper_methods()
    
    # Add helper classes and data structures
    methods += """
    
    /// <summary>
    /// Parameter transformation configuration
    /// </summary>
    private class ParameterTransformation
    {
        public string Type { get; set; }
        public string Parameter { get; set; }
        public string Location { get; set; }
        public string OriginalSchema { get; set; }
    }
"""
    
    display(f"Generated transformation code for {len(transformation_analysis['transformable_operations'])} operations")
    
    # Calculate operations that were handled in Swagger-only (no custom code needed)
    total_operations = len(openapi_data.get('paths', {}))
    if total_operations > 0:
        # Count operations that have at least one HTTP method
        actual_operations = 0
        for path, methods in openapi_data.get('paths', {}).items():
            for method in methods:
                if method.upper() in ['GET', 'POST', 'PUT', 'PATCH', 'DELETE', 'HEAD', 'OPTIONS']:
                    actual_operations += 1
        
        swagger_only_operations = actual_operations - len(transformation_analysis['transformable_operations'])
        if swagger_only_operations > 0:
            display(f"└─ {swagger_only_operations} operations implemented natively in Swagger (no transformations needed)")
    
    return methods


def _escape_json_for_csharp(json_obj):
    """Escape JSON for embedding in C# verbatim strings."""
    json_str = json.dumps(json_obj, separators=(',', ':'))
    return json_str.replace('"', '""')


def _generate_schema_composition_methods(transformation_analysis):
    """Generate methods for schema composition transformations (anyOf/oneOf/allOf)."""
    display("   ├─ Generating schema composition transformation methods")
    
    return '''
    /// <summary>
    /// Transform anyOf/oneOf/allOf parameters into type selector + value
    /// </summary>
    private async Task<bool> ApplySchemaCompositionTransformation(ParameterTransformation transformation)
    {
        var paramName = transformation.Parameter;
        var location = transformation.Location;
        
        // Get the type selector and value parameters
        var typeSelectorName = $"{paramName}_type";
        var valueParameterName = $"{paramName}_value";
        
        var selectedType = GetParameterValue(typeSelectorName, location);
        var parameterValue = GetParameterValue(valueParameterName, location);
        
        if (string.IsNullOrEmpty(selectedType) || string.IsNullOrEmpty(parameterValue))
        {
            this.Context.Logger?.LogInformation($"Schema composition parameters not provided for {paramName}");
            return true; // Optional parameters
        }
        
        // Convert based on selected type
        object convertedValue;
        switch (selectedType.ToLower())
        {
            case "string":
                convertedValue = parameterValue;
                break;
            case "object":
                try
                {
                    convertedValue = JObject.Parse(parameterValue);
                }
                catch (JsonException ex)
                {
                    this.Context.Logger?.LogError($"Invalid JSON object for {paramName}: {ex.Message}");
                    return false;
                }
                break;
            case "array":
                try
                {
                    convertedValue = JArray.Parse(parameterValue);
                }
                catch (JsonException ex)
                {
                    this.Context.Logger?.LogError($"Invalid JSON array for {paramName}: {ex.Message}");
                    return false;
                }
                break;
            default:
                this.Context.Logger?.LogWarning($"Unknown schema type '{selectedType}' for {paramName}, using as string");
                convertedValue = parameterValue;
                break;
        }
        
        // Set the original parameter with converted value
        SetParameterValue(paramName, location, convertedValue);
        
        // Remove the temporary selector parameters
        RemoveParameter(typeSelectorName, location);
        RemoveParameter(valueParameterName, location);
        
        this.Context.Logger?.LogInformation($"Applied schema composition transformation for {paramName}: {selectedType}");
        return true;
    }'''


def _generate_object_flattening_methods(transformation_analysis):
    """Generate methods for object flattening transformations."""
    display("   ├─ Generating object flattening transformation methods")
    
    return '''
    /// <summary>
    /// Transform flattened object parameters back into nested object
    /// </summary>
    private async Task<bool> ApplyObjectFlatteningTransformation(ParameterTransformation transformation)
    {
        var paramName = transformation.Parameter;
        var location = transformation.Location;
        
        // Parse original schema to understand structure
        JObject originalSchema;
        try
        {
            originalSchema = JObject.Parse(transformation.OriginalSchema);
        }
        catch (JsonException ex)
        {
            this.Context.Logger?.LogError($"Invalid original schema for {paramName}: {ex.Message}");
            return false;
        }
        
        // Build object from flattened parameters
        var reconstructedObject = new JObject();
        var properties = originalSchema["properties"] as JObject;
        
        if (properties != null)
        {
            foreach (var property in properties)
            {
                var flattenedParamName = $"{paramName}_{property.Key}";
                var value = GetParameterValue(flattenedParamName, location);
                
                if (!string.IsNullOrEmpty(value))
                {
                    // Convert value based on property type
                    var propertySchema = property.Value as JObject;
                    var propertyType = propertySchema?["type"]?.ToString();
                    
                    switch (propertyType)
                    {
                        case "number":
                            if (double.TryParse(value, out double numValue))
                                reconstructedObject[property.Key] = numValue;
                            break;
                        case "integer":
                            if (int.TryParse(value, out int intValue))
                                reconstructedObject[property.Key] = intValue;
                            break;
                        case "boolean":
                            if (bool.TryParse(value, out bool boolValue))
                                reconstructedObject[property.Key] = boolValue;
                            break;
                        default:
                            reconstructedObject[property.Key] = value;
                            break;
                    }
                    
                    // Remove the flattened parameter
                    RemoveParameter(flattenedParamName, location);
                }
            }
        }
        
        // Set the reconstructed object as the original parameter
        if (reconstructedObject.HasValues)
        {
            SetParameterValue(paramName, location, reconstructedObject);
        }
        
        this.Context.Logger?.LogInformation($"Applied object flattening transformation for {paramName}");
        return true;
    }'''


def _generate_array_simplification_methods(transformation_analysis):
    """Generate methods for array simplification transformations."""
    display("   ├─ Generating array simplification transformation methods")
    
    return '''
    /// <summary>
    /// Transform delimited string back into array parameter
    /// </summary>
    private async Task<bool> ApplyArraySimplificationTransformation(ParameterTransformation transformation)
    {
        var paramName = transformation.Parameter;
        var location = transformation.Location;
        var listParamName = $"{paramName}_list";
        
        var delimitedValue = GetParameterValue(listParamName, location);
        
        if (string.IsNullOrEmpty(delimitedValue))
        {
            this.Context.Logger?.LogInformation($"Array parameter {paramName} not provided");
            return true; // Optional parameter
        }
        
        // Split the delimited string into array
        var arrayItems = delimitedValue.Split(',')
            .Select(item => item.Trim())
            .Where(item => !string.IsNullOrEmpty(item))
            .ToArray();
        
        // Create JSON array
        var jsonArray = new JArray();
        foreach (var item in arrayItems)
        {
            // Try to parse as JSON first, fallback to string
            try
            {
                var parsedItem = JToken.Parse(item);
                jsonArray.Add(parsedItem);
            }
            catch
            {
                jsonArray.Add(item); // Add as string
            }
        }
        
        // Set the original parameter with array value
        SetParameterValue(paramName, location, jsonArray);
        
        // Remove the temporary list parameter
        RemoveParameter(listParamName, location);
        
        this.Context.Logger?.LogInformation($"Applied array simplification transformation for {paramName}: {arrayItems.Length} items");
        return true;
    }'''


def _generate_enum_dynamic_methods(transformation_analysis):
    """Generate methods for enum to dynamic value transformations."""
    display("   ├─ Generating enum dynamic transformation methods")
    
    return '''
    /// <summary>
    /// Handle enum to dynamic value transformations (no conversion needed - handled by Power Platform)
    /// </summary>
    private async Task<bool> ApplyEnumDynamicTransformation(ParameterTransformation transformation)
    {
        // Dynamic values are handled by Power Platform x-ms-dynamic-values extension
        // No transformation needed in custom code
        this.Context.Logger?.LogInformation($"Enum dynamic transformation for {transformation.Parameter}: handled by Power Platform");
        return true;
    }'''


def _generate_nested_flattening_methods(transformation_analysis):
    """Generate methods for nested object flattening transformations."""
    display("   ├─ Generating nested flattening transformation methods")
    
    return '''
    /// <summary>
    /// Transform request body from flattened parameters to nested structure
    /// </summary>
    private async Task<bool> ApplyNestedFlatteningTransformation(ParameterTransformation transformation)
    {
        if (transformation.Location != "body")
        {
            return true; // Only applies to request body
        }
        
        // For nested flattening, we typically need to reconstruct the nested structure
        // This is a complex transformation that depends on the specific schema
        
        try
        {
            var contentString = await this.Context.Request.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(contentString))
            {
                return true; // No body content
            }
            
            var requestData = JObject.Parse(contentString);
            
            // Apply nested structure reconstruction based on original schema
            // This would need customization based on specific API requirements
            var reconstructedBody = ReconstructNestedStructure(requestData, transformation.OriginalSchema);
            
            // Update request content
            this.Context.Request.Content = CreateJsonContent(reconstructedBody.ToString());
            
            this.Context.Logger?.LogInformation($"Applied nested flattening transformation for request body");
            return true;
        }
        catch (Exception ex)
        {
            this.Context.Logger?.LogError($"Nested flattening transformation failed: {ex.Message}");
            return false;
        }
    }
    
    /// <summary>
    /// Reconstruct nested structure from flattened object
    /// </summary>
    private JObject ReconstructNestedStructure(JObject flattenedData, string originalSchema)
    {
        // This is a placeholder implementation
        // In practice, this would need to be customized based on the specific schema
        
        // For now, return the data as-is
        // TODO: Implement schema-specific nested reconstruction logic
        return flattenedData;
    }'''


def _generate_body_composition_methods(transformation_analysis):
    """Generate methods for request body composition transformations."""
    display("   ├─ Generating body composition transformation methods")
    
    return '''
    /// <summary>
    /// Transform body type selector + content into composed request body
    /// </summary>
    private async Task<bool> ApplyBodyCompositionTransformation(ParameterTransformation transformation)
    {
        if (transformation.Location != "body")
        {
            return true; // Only applies to request body
        }
        
        // Get the body type selector and content parameters
        var bodyType = GetParameterValue("body_type", "query");
        var bodyContent = GetParameterValue("body_content", "query");
        
        if (string.IsNullOrEmpty(bodyType) || string.IsNullOrEmpty(bodyContent))
        {
            this.Context.Logger?.LogInformation("Body composition parameters not provided");
            return true; // Optional parameters
        }
        
        // Parse the body content
        JObject composedBody;
        try
        {
            composedBody = JObject.Parse(bodyContent);
        }
        catch (JsonException ex)
        {
            this.Context.Logger?.LogError($"Invalid JSON in body_content: {ex.Message}");
            return false;
        }
        
        // Ensure the body has the correct type discriminator
        composedBody["type"] = bodyType;
        
        // Set the composed body as request content
        this.Context.Request.Content = CreateJsonContent(composedBody.ToString());
        
        // Remove the temporary parameters from query string
        RemoveParameter("body_type", "query");
        RemoveParameter("body_content", "query");
        
        this.Context.Logger?.LogInformation($"Applied body composition transformation: type={bodyType}");
        return true;
    }'''


def _generate_parameter_helper_methods():
    """Generate helper methods for parameter manipulation."""
    display("   ├─ Generating parameter manipulation helper methods")
    
    return '''
    
    /// <summary>
    /// Get parameter value from request based on location
    /// </summary>
    private string GetParameterValue(string parameterName, string location)
    {
        switch (location.ToLower())
        {
            case "query":
                var queryParameters = this.Context.Request.RequestUri.ParseQueryString();
                return queryParameters[parameterName];
                
            case "path":
                // Path parameters are typically already resolved in the URL
                // This would need custom implementation based on the specific API
                return null;
                
            case "header":
                if (this.Context.Request.Headers.TryGetValues(parameterName, out var headerValues))
                {
                    return headerValues.FirstOrDefault();
                }
                return null;
                
            default:
                this.Context.Logger?.LogWarning($"Unknown parameter location: {location}");
                return null;
        }
    }
    
    /// <summary>
    /// Set parameter value in request based on location
    /// </summary>
    private void SetParameterValue(string parameterName, string location, object value)
    {
        switch (location.ToLower())
        {
            case "query":
                var queryParameters = this.Context.Request.RequestUri.ParseQueryString();
                queryParameters[parameterName] = value?.ToString();
                
                // Rebuild the request URI with updated query parameters
                var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
                uriBuilder.Query = queryParameters.ToString();
                
                // Create new request with updated URI
                var newRequest = new HttpRequestMessage(this.Context.Request.Method, uriBuilder.Uri);
                foreach (var header in this.Context.Request.Headers)
                {
                    newRequest.Headers.Add(header.Key, header.Value);
                }
                if (this.Context.Request.Content != null)
                {
                    newRequest.Content = this.Context.Request.Content;
                }
                
                this.Context.Request = newRequest;
                break;
                
            case "header":
                if (this.Context.Request.Headers.Contains(parameterName))
                {
                    this.Context.Request.Headers.Remove(parameterName);
                }
                this.Context.Request.Headers.Add(parameterName, value?.ToString());
                break;
                
            case "path":
                // Path parameters require URL template replacement
                // This would need custom implementation based on the specific API
                this.Context.Logger?.LogWarning($"Path parameter modification not implemented for {parameterName}");
                break;
                
            default:
                this.Context.Logger?.LogWarning($"Unknown parameter location: {location}");
                break;
        }
    }
    
    /// <summary>
    /// Remove parameter from request based on location
    /// </summary>
    private void RemoveParameter(string parameterName, string location)
    {
        switch (location.ToLower())
        {
            case "query":
                var queryParameters = this.Context.Request.RequestUri.ParseQueryString();
                queryParameters.Remove(parameterName);
                
                // Rebuild the request URI without the parameter
                var uriBuilder = new UriBuilder(this.Context.Request.RequestUri);
                uriBuilder.Query = queryParameters.ToString();
                
                // Create new request with updated URI
                var newRequest = new HttpRequestMessage(this.Context.Request.Method, uriBuilder.Uri);
                foreach (var header in this.Context.Request.Headers)
                {
                    newRequest.Headers.Add(header.Key, header.Value);
                }
                if (this.Context.Request.Content != null)
                {
                    newRequest.Content = this.Context.Request.Content;
                }
                
                this.Context.Request = newRequest;
                break;
                
            case "header":
                if (this.Context.Request.Headers.Contains(parameterName))
                {
                    this.Context.Request.Headers.Remove(parameterName);
                }
                break;
                
            case "path":
                // Path parameters cannot be easily removed without URL reconstruction
                this.Context.Logger?.LogInformation($"Path parameter removal not applicable for {parameterName}");
                break;
                
            default:
                this.Context.Logger?.LogWarning($"Unknown parameter location: {location}");
                break;
        }
    }'''


def _generate_operation_handlers(operation_ids, incompatible_features=None):
    """Generate switch case handlers for operations, highlighting those needing custom code."""
    if not operation_ids:
        return ""
    
    incompatible_ops = incompatible_features.get('custom_code_operations', set()) if incompatible_features else set()
    
    handlers = []
    for op_id in operation_ids:
        if op_id in incompatible_ops:
            handlers.append(f'            case "{op_id}":')
            handlers.append(f'                // ⚠️  REQUIRES CUSTOM CODE: {", ".join(incompatible_features["operation_features"][op_id])}')
            handlers.append(f'                return await Handle{_camel_case(op_id)}Operation();')
        else:
            handlers.append(f'            case "{op_id}":')
            handlers.append(f'                return await Handle{_camel_case(op_id)}Operation();')
    
    return '\n'.join(handlers)


def _generate_operation_methods(operation_ids, incompatible_features=None):
    """Generate individual operation handler methods with specific logic for incompatible features."""
    if not operation_ids:
        return ""
    
    incompatible_ops = incompatible_features.get('custom_code_operations', set()) if incompatible_features else set()
    operation_features = incompatible_features.get('operation_features', {}) if incompatible_features else {}
    
    methods = []
    for op_id in operation_ids:
        camel_case_id = _camel_case(op_id)
        method = f'''
    /// <summary>
    /// Handle {op_id} operation'''
        
        if op_id in incompatible_ops:
            features = operation_features.get(op_id, [])
            method += f'''
    /// ⚠️  CUSTOM CODE REQUIRED: {", ".join(features)}'''
        
        method += f'''
    /// </summary>
    private async Task<HttpResponseMessage> Handle{camel_case_id}Operation()
    {{'''
        
        if op_id in incompatible_ops:
            features = operation_features.get(op_id, [])
            
            method += f'''
        // TODO: Implement custom logic for {op_id}
        // This operation requires custom code due to: {", ".join(features)}
        
        // Phase 2: Feature-specific implementation guidance:'''
            
            # Add feature-specific guidance
            for feature in features:
                method += f'''
        // 
        // 🔧 {feature.upper()}:
        //    {_get_feature_implementation_guidance(feature)}'''
            
            method += f'''
        
        // Example: Log operation details
        this.Context.Logger?.LogInformation("Processing {op_id} operation with incompatible features: {', '.join(features)}");'''
            
            # Add feature-specific helper method calls
            schema_features = [f for f in features if 'schema' in f.lower()]
            content_features = [f for f in features if 'content' in f.lower()]
            param_features = [f for f in features if 'parameter' in f.lower()]
            
            if schema_features:
                method += '''
        
        // Handle complex schema features
        var requestContent = await this.Context.Request.Content.ReadAsStringAsync();
        if (!string.IsNullOrEmpty(requestContent))
        {
            var requestData = JObject.Parse(requestContent);
            var validationResult = await ValidateComplexSchema(requestData);
            if (!validationResult.IsValid)
            {
                return CreateErrorResponse(HttpStatusCode.BadRequest, validationResult.ErrorMessage);
            }
        }'''
            
            if content_features:
                method += '''
        
        // Handle multiple content types
        await HandleContentTypeNegotiation();'''
                
            if param_features:
                method += '''
        
        // Handle advanced parameter processing
        await ProcessAdvancedParameters();'''
            
            method += '''
        
        // Forward to backend with custom processing
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken);'''
        else:
            method += f'''
        // TODO: Implement custom logic for {op_id}
        
        // Example: Log operation details
        this.Context.Logger?.LogInformation("Processing {op_id} operation");
        
        // For now, just forward the request
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken);'''
        
        method += '''
    }'''
        methods.append(method)
    
    return '\n'.join(methods)


def _generate_incompatible_feature_handlers(incompatible_features):
    """Generate specific helper methods for handling incompatible OpenAPI 3.0 features."""
    if not incompatible_features or not incompatible_features.get('custom_code_operations'):
        return ""
    
    handlers = []
    all_features = set()
    
    # Collect all unique incompatible features
    for features_list in incompatible_features.get('operation_features', {}).values():
        all_features.update(features_list)
    all_features.update(incompatible_features.get('global_features', []))
    
    # Phase 2: Generate sophisticated handlers for specific feature categories
    
    # Schema validation handlers
    if any('schema' in f.lower() for f in all_features):
        handlers.append('''
    /// <summary>
    /// Validate complex schema features (oneOf/anyOf/allOf/discriminator/nullable)
    /// Phase 2: Enhanced validation with detailed error reporting
    /// </summary>
    private async Task<(bool IsValid, string ErrorMessage)> ValidateComplexSchema(JObject data)
    {
        try
        {
            // TODO: Load original OpenAPI 3.0 schema from embedded resource
            // var openApiSchema = LoadEmbeddedOpenApiSchema();
            
            this.Context.Logger?.LogInformation("Validating complex schema features");
            
            // Example validation patterns:
            
            // oneOf validation: Check discriminator and validate against correct schema
            if (data.ContainsKey("type"))
            {
                var discriminatorValue = data["type"]?.ToString();
                // TODO: Route to appropriate schema validation based on discriminator
                this.Context.Logger?.LogInformation($"Discriminator value: {discriminatorValue}");
            }
            
            // nullable validation: Handle null values appropriately
            foreach (var property in data.Properties())
            {
                if (property.Value.Type == JTokenType.Null)
                {
                    // TODO: Check if this property allows null in original schema
                    this.Context.Logger?.LogInformation($"Null value found for property: {property.Name}");
                }
            }
            
            // For now, assume validation passes - implement actual logic here
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, $"Schema validation failed: {ex.Message}");
        }
    }''')
    
    # Content type negotiation
    if any('content' in f.lower() for f in all_features):
        handlers.append('''
    /// <summary>
    /// Handle content type negotiation for multiple media types
    /// Phase 2: Enhanced content negotiation with transformation support
    /// </summary>
    private async Task<HttpResponseMessage> HandleContentTypeNegotiation()
    {
        var acceptHeader = this.Context.Request.Headers.Accept?.FirstOrDefault()?.MediaType ?? "application/json";
        var contentTypeHeader = this.Context.Request.Content?.Headers?.ContentType?.MediaType ?? "application/json";
        
        this.Context.Logger?.LogInformation($"Content negotiation - Accept: {acceptHeader}, Content-Type: {contentTypeHeader}");
        
        // TODO: Implement content transformation logic
        switch (contentTypeHeader.ToLower())
        {
            case "application/xml":
                // TODO: Convert XML to JSON for backend processing
                this.Context.Logger?.LogInformation("XML content type detected - implement XML to JSON conversion");
                break;
            case "text/plain":
                // TODO: Handle plain text content
                this.Context.Logger?.LogInformation("Plain text content type detected");
                break;
            case "application/x-www-form-urlencoded":
                // TODO: Handle form data
                this.Context.Logger?.LogInformation("Form data content type detected");
                break;
            default:
                // JSON - no conversion needed
                break;
        }
        
        return await this.Context.SendAsync(this.Context.Request, this.CancellationToken);
    }''')
    
    # Advanced parameter processing
    if any('parameter' in f.lower() for f in all_features):
        handlers.append('''
    /// <summary>
    /// Process advanced parameter features (styling, explode, reserved chars)
    /// Phase 2: Enhanced parameter processing with OpenAPI 3.0 compliance
    /// </summary>
    private async Task ProcessAdvancedParameters()
    {
        this.Context.Logger?.LogInformation("Processing advanced parameters");
        
        var queryParams = this.Context.Request.RequestUri.Query;
        
        // TODO: Implement advanced parameter processing
        // Example patterns:
        
        // Style: deepObject - ?filter[name]=john&filter[age]=30
        if (queryParams.Contains("[") && queryParams.Contains("]"))
        {
            this.Context.Logger?.LogInformation("DeepObject style parameters detected");
            // TODO: Parse deepObject parameters into nested structure
        }
        
        // Style: matrix - ;name=john;age=30
        if (queryParams.Contains(";"))
        {
            this.Context.Logger?.LogInformation("Matrix style parameters detected");
            // TODO: Parse matrix style parameters
        }
        
        // AllowReserved: preserve characters like :/?#[]@!$&'()*+,;=
        var hasReservedChars = queryParams.Any(c => ":/?#[]@!$&'()*+,;=".Contains(c));
        if (hasReservedChars)
        {
            this.Context.Logger?.LogInformation("Reserved characters in parameters detected");
            // TODO: Handle reserved characters without encoding
        }
        
        // Explode: handle array/object parameter serialization
        // TODO: Implement explode handling based on parameter definitions
    }''')
    
    # Dynamic values and schema
    if any('dynamic' in f.lower() for f in all_features):
        handlers.append('''
    /// <summary>
    /// Handle dynamic values and schema resolution
    /// Phase 2: Enhanced dynamic handling with caching and error recovery
    /// </summary>
    private async Task<JArray> GetDynamicValues(string parameterName, JObject context)
    {
        this.Context.Logger?.LogInformation($"Fetching dynamic values for parameter: {parameterName}");
        
        try
        {
            // TODO: Implement dynamic value lookup with caching
            // Example patterns:
            
            switch (parameterName.ToLower())
            {
                case "categories":
                    // TODO: Make API call to get current categories
                    return await FetchCategories(context);
                
                case "users":
                    // TODO: Make API call to get current users
                    return await FetchUsers(context);
                
                default:
                    this.Context.Logger?.LogWarning($"No dynamic handler for parameter: {parameterName}");
                    return new JArray();
            }
        }
        catch (Exception ex)
        {
            this.Context.Logger?.LogError($"Dynamic value lookup failed for {parameterName}: {ex.Message}");
            return new JArray(); // Return empty array on error
        }
    }
    
    private async Task<JArray> FetchCategories(JObject context)
    {
        // TODO: Implement category fetching logic
        // Example: Make API call to /categories endpoint
        return new JArray();
    }
    
    private async Task<JArray> FetchUsers(JObject context)  
    {
        // TODO: Implement user fetching logic
        // Example: Make API call to /users endpoint
        return new JArray();
    }''')
    
    # Error response helper
    handlers.append('''
    /// <summary>
    /// Create standardized error response
    /// Phase 2: Enhanced error handling with structured error information
    /// </summary>
    private HttpResponseMessage CreateErrorResponse(HttpStatusCode statusCode, string message, string details = null)
    {
        var errorResponse = new HttpResponseMessage(statusCode);
        var errorObject = new JObject
        {
            ["error"] = message,
            ["timestamp"] = DateTime.UtcNow.ToString("O"),
            ["operation"] = this.Context.OperationId,
            ["correlationId"] = this.Context.CorrelationId
        };
        
        if (!string.IsNullOrEmpty(details))
        {
            errorObject["details"] = details;
        }
        
        errorResponse.Content = CreateJsonContent(errorObject.ToString());
        return errorResponse;
    }''')
    
    return '\n'.join(handlers)


def _get_feature_description(feature):
    """Get human-readable description for incompatible feature."""
    descriptions = {
        # Schema complexity
        'oneof_schema': 'Uses oneOf schema validation requiring conditional logic',
        'anyof_schema': 'Uses anyOf schema validation requiring flexible validation',
        'allof_schema': 'Uses allOf schema composition requiring merged validation',
        'discriminator_schema': 'Uses discriminator for polymorphic type handling',
        'nullable_schema': 'Contains nullable properties requiring null handling',
        'readonly_writeonly_properties': 'Has read-only/write-only properties requiring filtering',
        'multiple_examples': 'Contains multiple examples requiring selection logic',
        
        # Parameter complexity
        'complex_parameter_schema': 'Complex parameter schemas requiring custom validation',
        'advanced_parameter_styling': 'Advanced parameter styling (deepObject, matrix, label)',
        'parameter_explode': 'Parameter explosion settings requiring custom serialization',
        'allow_reserved_parameter': 'Reserved characters in parameters requiring encoding',
        'nullable_parameter': 'Nullable parameters requiring null value handling',
        
        # Content type complexity
        'multiple_request_content_types': 'Multiple request content types requiring negotiation',
        'multiple_response_content_types': 'Multiple response content types requiring formatting',
        
        # Workflow complexity
        'callbacks': 'Callback operations requiring webhook handling',
        'response_links': 'Response links requiring operation chaining',
        'server_variables': 'Server URL variables requiring dynamic endpoints',
        
        # Authentication complexity
        'openid_connect': 'OpenID Connect authentication requiring token validation',
        'complex_oauth2': 'Multiple OAuth2 flows requiring flow selection'
    }
    return descriptions.get(feature, 'Advanced OpenAPI 3.0 feature requiring custom handling')


def _get_feature_implementation_guidance(feature):
    """Get specific implementation guidance for incompatible features."""
    guidance = {
        # Schema complexity
        'oneof_schema': 'Use discriminator property to determine which schema variant to validate against',
        'anyof_schema': 'Validate against all possible schemas and accept if any succeed',
        'allof_schema': 'Merge all schema requirements and validate against combined rules',
        'discriminator_schema': 'Check discriminator property value and route to appropriate schema validation',
        'nullable_schema': 'Check for null values and handle appropriately in validation logic',
        'readonly_writeonly_properties': 'Filter properties based on operation type (read vs write)',
        'multiple_examples': 'Select appropriate example based on context or user preference',
        
        # Parameter complexity  
        'complex_parameter_schema': 'Implement custom validation logic referencing original OpenAPI spec',
        'advanced_parameter_styling': 'Parse parameters using matrix/label/deepObject rules manually',
        'parameter_explode': 'Handle array/object parameter serialization based on explode setting',
        'allow_reserved_parameter': 'Preserve reserved characters without URL encoding',
        'nullable_parameter': 'Accept and handle null parameter values appropriately',
        
        # Content type complexity
        'multiple_request_content_types': 'Check Accept header and transform request to expected format',
        'multiple_response_content_types': 'Check Content-Type header and format response accordingly',
        
        # Workflow complexity
        'callbacks': 'Implement webhook endpoint handling and callback URL management',
        'response_links': 'Extract link values from response and make follow-up API calls',
        'server_variables': 'Build dynamic URLs by substituting server variable values',
        
        # Authentication complexity
        'openid_connect': 'Implement OpenID Connect token validation and user info retrieval',
        'complex_oauth2': 'Handle multiple OAuth2 flows based on context and availability'
    }
    return guidance.get(feature, 'Implement custom handling logic for this OpenAPI 3.0 feature')


def _get_transformation_description(transform_type):
    """Get human-readable description for parameter transformation types."""
    descriptions = {
        'schema_composition': 'Convert anyOf/oneOf/allOf schemas to user-friendly parameter selection',
        'object_flattening': 'Flatten nested object parameters into individual fields',
        'array_simplification': 'Convert complex array parameters to simple string or selection inputs',
        'enum_to_dynamic': 'Convert static enum values to dynamic dropdown lists',
        'body_composition': 'Transform complex request body schemas to conditional parameter groups',
        'nested_flattening': 'Flatten deeply nested objects using dot notation or prefixed parameters'
    }
    return descriptions.get(transform_type, 'Parameter transformation for improved usability')


def _camel_case(snake_str):
    """Convert snake_case or kebab-case to CamelCase."""
    components = re.split(r'[-_]', snake_str)
    return ''.join(word.capitalize() for word in components)
