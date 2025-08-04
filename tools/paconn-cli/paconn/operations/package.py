# -----------------------------------------------------------------------------
# Copyright (c) 2025 Troy Taylor (troy@troystaylor.com). All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# 
# Permission is hereby granted to Microsoft Corporation and any other party
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of this software under the terms of the MIT License.
# -----------------------------------------------------------------------------
"""
Package operation - Creates a Power Platform solution package from component files.
"""

import os
import json
import zipfile
import shutil
import glob
from datetime import datetime

from knack.util import CLIError
from knack.prompting import prompt_y_n

from paconn.common.util import format_json
from paconn.settings.util import SETTINGS_FILE


def _validate_source_directory(source_path):
    """
    Validate that the source directory exists and contains zip files.
    """
    if not os.path.isdir(source_path):
        raise CLIError('Source directory {} does not exist.'.format(source_path))
    
    # Check for zip files
    zip_files = glob.glob(os.path.join(source_path, "*.zip"))
    
    if not zip_files:
        raise CLIError(
            'Source directory {} does not contain any ZIP files to package.'.format(source_path))
    
    return True


def _get_default_mappings():
    """
    Get default file mappings for Power Platform solution types.
    """
    return {
        "*Connector*": "Connector.zip",
        "*Flow*": "Flow.zip", 
        "*AIPlugin*": "AIPlugin.zip"
    }


def _match_pattern(filename, pattern):
    """
    Check if filename matches the wildcard pattern.
    """
    import fnmatch
    return fnmatch.fnmatch(filename, pattern)


def _rename_solution_files(source_path, custom_mappings=None):
    """
    Rename zip files according to Power Platform solution naming conventions.
    """
    all_mappings = _get_default_mappings()
    
    # Merge custom mappings if provided
    if custom_mappings:
        all_mappings.update(custom_mappings)
    
    zip_files = glob.glob(os.path.join(source_path, "*.zip"))
    renamed_count = 0
    
    for file_path in zip_files:
        filename = os.path.basename(file_path)
        new_name = None
        matched_pattern = None
        
        # Check each mapping pattern
        for pattern, target_name in all_mappings.items():
            if _match_pattern(filename, pattern):
                new_name = target_name
                matched_pattern = pattern
                break
        
        if new_name and filename != new_name:
            new_path = os.path.join(source_path, new_name)
            
            # Check if target file already exists
            if os.path.exists(new_path):
                print("Warning: Target file '{}' already exists. Skipping '{}'.".format(new_name, filename))
                continue
            
            os.rename(file_path, new_path)
            print("✓ Renamed '{}' to '{}' (matched: {})".format(filename, new_name, matched_pattern))
            renamed_count += 1
        else:
            print("○ No matching pattern found for '{}'".format(filename))
    
    return renamed_count


def _create_pkg_assets_folder(source_path):
    """
    Create PkgAssets folder and move all zip files into it.
    """
    pkg_assets_path = os.path.join(source_path, "PkgAssets")
    
    if not os.path.exists(pkg_assets_path):
        os.makedirs(pkg_assets_path)
        print("Created 'PkgAssets' folder.")
    
    # Move all zip files to PkgAssets
    zip_files = glob.glob(os.path.join(source_path, "*.zip"))
    moved_count = 0
    
    for zip_file in zip_files:
        filename = os.path.basename(zip_file)
        destination_path = os.path.join(pkg_assets_path, filename)
        
        # Check if file already exists in destination
        if os.path.exists(destination_path):
            print("Warning: File '{}' already exists in PkgAssets folder. Skipping move.".format(filename))
            continue
        
        shutil.move(zip_file, destination_path)
        print("→ Moved '{}' to PkgAssets folder".format(filename))
        moved_count += 1
    
    return pkg_assets_path, moved_count


def _create_intro_file(source_path):
    """
    Copy readme.md to intro.md, or use first available .md file.
    """
    readme_path = os.path.join(source_path, "readme.md")
    intro_path = os.path.join(source_path, "intro.md")
    
    if os.path.exists(readme_path):
        shutil.copy2(readme_path, intro_path)
        print("✓ Copied readme.md to intro.md")
        return intro_path
    else:
        # Look for any .md file as fallback
        md_files = [f for f in glob.glob(os.path.join(source_path, "*.md")) 
                   if os.path.basename(f).lower() != "intro.md"]
        
        if md_files:
            source_md_file = md_files[0]
            shutil.copy2(source_md_file, intro_path)
            print("✓ Copied '{}' to intro.md (readme.md not found)".format(os.path.basename(source_md_file)))
            return intro_path
        else:
            print("Warning: No .md files found to copy to intro.md")
            return None


def _create_package_zip(source_path, pkg_assets_path):
    """
    Create package.zip from PkgAssets folder contents.
    """
    package_zip_path = os.path.join(source_path, "package.zip")
    
    # Remove existing package.zip if it exists
    if os.path.exists(package_zip_path):
        os.remove(package_zip_path)
        print("Removed existing package.zip file.")
    
    # Create package.zip containing the entire PkgAssets folder
    if os.path.exists(pkg_assets_path):
        with zipfile.ZipFile(package_zip_path, 'w', zipfile.ZIP_DEFLATED) as zip_file:
            # Add the PkgAssets folder and all its contents
            for root, dirs, files in os.walk(pkg_assets_path):
                for file in files:
                    file_path = os.path.join(root, file)
                    # Create archive path that includes PkgAssets folder
                    arcname = os.path.relpath(file_path, source_path)
                    zip_file.write(file_path, arcname)
        
        print("✓ Created package.zip containing PkgAssets folder contents.")
        
        # Get package size info
        package_size = os.path.getsize(package_zip_path)
        package_size_kb = round(package_size / 1024, 2)
        print("Package size: {} KB".format(package_size_kb))
        
        return package_zip_path
    else:
        print("Warning: PkgAssets folder not found. Cannot create package.zip.")
        return None


def _create_connector_package(source_path, intro_path, package_zip_path):
    """
    Create ConnectorPackage.zip containing intro.md and package.zip.
    """
    connector_package_path = os.path.join(source_path, "ConnectorPackage.zip")
    
    # Remove existing ConnectorPackage.zip if it exists
    if os.path.exists(connector_package_path):
        os.remove(connector_package_path)
        print("Removed existing ConnectorPackage.zip file.")
    
    # Collect files to include
    files_to_include = []
    
    if intro_path and os.path.exists(intro_path):
        files_to_include.append(intro_path)
    else:
        print("Warning: intro.md not found. ConnectorPackage.zip will not include intro.md.")
    
    if package_zip_path and os.path.exists(package_zip_path):
        files_to_include.append(package_zip_path)
    else:
        print("Warning: package.zip not found. ConnectorPackage.zip will not include package.zip.")
    
    if files_to_include:
        with zipfile.ZipFile(connector_package_path, 'w', zipfile.ZIP_DEFLATED) as zip_file:
            for file_path in files_to_include:
                filename = os.path.basename(file_path)
                zip_file.write(file_path, filename)
        
        print("✓ Created ConnectorPackage.zip containing:")
        for file_path in files_to_include:
            filename = os.path.basename(file_path)
            print("  - {}".format(filename))
        
        # Get connector package size info
        connector_package_size = os.path.getsize(connector_package_path)
        connector_package_size_kb = round(connector_package_size / 1024, 2)
        print("ConnectorPackage size: {} KB".format(connector_package_size_kb))
        
        return connector_package_path
    else:
        print("Warning: No files available to create ConnectorPackage.zip.")
        return None


def _cleanup_intermediate_files(source_path, pkg_assets_path, intro_path, package_zip_path):
    """
    Clean up intermediate files and folders.
    """
    print("\nCleaning up intermediate files...")
    
    # Remove PkgAssets folder
    if pkg_assets_path and os.path.exists(pkg_assets_path):
        shutil.rmtree(pkg_assets_path)
        print("✓ Removed PkgAssets folder")
    
    # Remove intro.md file
    if intro_path and os.path.exists(intro_path):
        os.remove(intro_path)
        print("✓ Removed intro.md file")
    
    # Remove package.zip file
    if package_zip_path and os.path.exists(package_zip_path):
        os.remove(package_zip_path)
        print("✓ Removed package.zip file")
    
    print("✓ Cleanup completed!")


def package(source, destination, package_format, settings, overwrite, custom_mappings=None):
    """
    Package operation - Creates a Power Platform solution package.
    
    This function:
    1. Renames zip files according to Power Platform conventions (Connector.zip, Flow.zip, AIPlugin.zip)
    2. Moves all zip files to a PkgAssets folder
    3. Creates intro.md from readme.md (or first .md file)
    4. Compresses PkgAssets into package.zip
    5. Creates final ConnectorPackage.zip with intro.md and package.zip
    6. Cleans up intermediate files
    """
    # Use current directory as source if not specified
    source_path = source if source else os.getcwd()
    source_path = os.path.abspath(source_path)
    
    # Validate source directory
    _validate_source_directory(source_path)
    
    print("Found {} zip file(s) in '{}'".format(
        len(glob.glob(os.path.join(source_path, "*.zip"))), source_path))
    
    try:
        # Step 1: Rename solution files according to conventions
        renamed_count = _rename_solution_files(source_path, custom_mappings)
        print("\nCompleted: {} file(s) renamed successfully.".format(renamed_count))
        
        # Step 2: Create PkgAssets folder and move zip files
        pkg_assets_path, moved_count = _create_pkg_assets_folder(source_path)
        print("\nMoved {} zip file(s) to PkgAssets folder.".format(moved_count))
        
        # Step 3: Create intro.md from readme.md or first .md file
        intro_path = _create_intro_file(source_path)
        
        # Step 4: Create package.zip from PkgAssets folder
        package_zip_path = _create_package_zip(source_path, pkg_assets_path)
        
        # Step 5: Create ConnectorPackage.zip with intro.md and package.zip
        connector_package_path = _create_connector_package(source_path, intro_path, package_zip_path)
        
        # Step 6: Clean up intermediate files
        if connector_package_path:
            _cleanup_intermediate_files(source_path, pkg_assets_path, intro_path, package_zip_path)
            print("✓ ConnectorPackage.zip is ready!")
            return connector_package_path
        else:
            raise CLIError("Failed to create ConnectorPackage.zip")
            
    except Exception as e:
        raise CLIError("An error occurred while packaging: {}".format(str(e)))
