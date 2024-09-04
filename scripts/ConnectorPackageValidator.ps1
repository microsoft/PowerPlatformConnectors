param (
    [Parameter(HelpMessage = "Specify valid zip file path.", Mandatory = $true)][string]$zipFilePath,
    [Parameter(HelpMessage = "Specify 'y'/'yes' (case-insensitive) if plugin is enabled else specify 'n'/'no' (case-insensitive).", Mandatory = $true)][string]$pluginEnabled
)

function RemoveTempFolder($tempFolderPath)
{
    # Check if the folder exists before attempting to remove items
    if ($tempFolderPath -and (Test-Path $tempFolderPath)) 
    {
        # Remove all items (files and subfolders) within the temp folder
        Get-ChildItem -Path $tempFolderPath | Remove-Item -Force -Recurse
    } 
}

function DisplayReferDocumentation() {
        Write-Host ""
        $url = "https://learn.microsoft.com/en-us/connectors/custom-connectors/certification-submission"
        $escape = [char]27
        $blue = "${escape}[33m" # ANSI escape code for yellow color
        $underline = "${escape}[4m" # ANSI escape code for underline
        $reset = "${escape}[0m" # ANSI escape code to reset formatting
        # Display the URL as a clickable hyperlink
        [System.Console]::WriteLine("For a better understanding of how to create a connector package zip file, please refer to the public document placed at ${blue}${underline}$Url${reset}")
}

function ValidateFolderAndFilesInPackage {
    param (
        [bool] $pluginEnabled,
        [int] $levelOfHierarchy,
        [string] $parentFolderPath,
        [string] $folderPath,
        [int] $expectedFolderCount,
        [hashtable] $expectedFileCounts = $null
    )
    # Initialize counters
    $actualFolderCount = 0
    $actualFileCounts = @{}
    $actualFiles = @{}
    # Check if the folder exists
    if (Test-Path $folderPath -PathType Container) {
        # Get all folders in the specified path
        $folders = Get-ChildItem -Path $folderPath -Directory
        $actualFolderCount = $folders.Count

        # Initialize actual file counts for specified extensions
        if ($expectedFileCounts) {
            $countForMultipleMismatch = 0
            foreach ($ext in $expectedFileCounts.Keys) {
                $actualFiles[$ext] = @()
                $actualFileCounts[$ext] = (Get-ChildItem -Path $folderPath -File -Filter $ext).Count
                # Populate actual file names
                if ($actualFileCounts[$ext] -ne $expectedFileCounts[$ext]) {
                    $countForMultipleMismatch ++
                }
                $actualFiles[$ext] = Get-ChildItem -Path $folderPath -File -Filter $ext | Select-Object -ExpandProperty Name
            }
        }

        # Validation against expected counts
        $validationPassed = $true
        $validationMessages = @()
        $currentFolderName = if ($levelOfHierarchy -eq 3) {Split-Path -Leaf $folderPath} else {""}
        
        # Validate folder count
        if ($actualFolderCount -ne $expectedFolderCount) {
            $validationPassed = $false
            switch ($levelOfHierarchy) {
                1 {
                    if (-not $expectedFolderCount -and $actualFolderCount)
                    {
                        $validationMessages += "The provided zip file should not contain any folder inside '$parentFolderPath'."
                        $strFolderNum = if ($actualFolderCount -eq 1) {"folder"} else {"folders"}
                        $validationMessages += "Please remove the $strfolderNum $($folders -join ', ')."
                    }
                }
                2 {
                    if ($expectedFolderCount -and -not $actualFolderCount)
                    {
                        $validationMessages += "The package zip file should contain a folder ideally named 'PkgAssets' inside '$parentFolderPath'."
                        $validationMessages += "Please add the folder containing the solution zip files"
                    }
                }
                3 {     
                    if (-not $expectedFolderCount -and $actualFolderCount) {
                        $validationMessages += "The package folder '$currentFolderName' should not contain any folder inside '$parentFolderPath'."
                        $strFolderNum = if ($actualFolderCount -eq 1) {"folder"} else {"folders"}
                        $validationMessages += "Please remove the $strfolderNum  $($folders -join ', ')."
                    }
                }
            }
        }

        # Validate file counts
        $misMatchFound = $false
        if ($expectedFileCounts) {
            foreach ($ext in $expectedFileCounts.Keys) {
                if (-not $misMatchFound) {
                    $expectedCount = $expectedFileCounts[$ext]
                    $actualCount = $actualFileCounts[$ext]
                    
                    if ($actualCount -ne $expectedCount) {
                        $misMatchFound = $true
                        $validationPassed = $false
                        switch ($levelOfHierarchy) {
                            1 {
                                if ($expectedCount -and -not $actualCount)
                                {
                                    $missingFileDeatils = if ($countForMultipleMismatch -gt 1) {"The intro.md file and package zip file are"} else {if ($ext -eq "*.md") {"The intro.md file is"} else {"The package zip file is"}}
                                    $validationMessages += "The provided zip file should contain both intro.md and package zip file in '$parentFolderPath'."
                                    $strFileNum = if ($countForMultipleMismatch -gt 1) {"files"} else {"file"}
                                    $validationMessages += "$missingFileDeatils missing. Please add the required $strFileNum."
                                }
                            }
                            3 {
                                if ($expectedCount -and (-not $actualCount))
                                {
                                    $validationMessages += "The package folder '$currentFolderName' does not contain solution zip files in '$parentFolderPath'."
                                    $detailsSolutionZip = if ($pluginEnabled) {"connector, flow and AIplugin "} else {"connector and flow"}
                                    $validationMessages += "Please add the required $detailsSolutionZip solution zip files."
                                }
                                elseif ((-not $pluginEnabled) -and (($expectedCount -eq 2) -and ($actualCount -gt 2)))
                                {
                                    $detailsSolutionZip = if ($pluginEnabled) {"connector, flow and AIplugin "} else {"only connector and flow"}
                                    $validationMessages += "The package folder '$currentFolderName' should contain $detailsSolutionZip solution zip files in '$parentFolderPath'."
                                }
                                elseif ($pluginEnabled -and (($expectedCount -eq 3) -and ($actualCount -lt 3)))
                                {
                                    $detailsSolutionZip = if ($pluginEnabled) {"connector, flow and AIplugin "} else {"connector and flow"}
                                    $validationMessages += "The package folder '$currentFolderName' should contain $detailsSolutionZip solution zip files in '$parentFolderPath'."
                                    $validationMessages += "Please have all the three solution zip files as this is a plugin connector."
                                }
                            }
                        }  
                    }
                }
            }
        }

        # Output validation result
        if ($validationPassed) {
            return $true
        } else {
            foreach ($message in $validationMessages) {
                Write-Host $message
            }
            return $false
        }
    } else {
        Write-Host  ""
        Write-Host "Folder not found: $folderPath while working with actual working folder path:'$parentFolderPath'"
        return $false
    }
}

# Start of the validation
try {
    Write-Host ""
    # Trim the two input string arguments for "" or '' (double quote or single quote) and white spaces
    $zipFilePath = $zipFilePath -replace '"', ''
    $zipFilePath = $zipFilePath -replace "'", ''
    $pluginEnabled = $pluginEnabled -replace '"', ''
    $pluginEnabled = $pluginEnabled -replace "'", ''
    $zipFilePath = $zipFilePath.Trim()
    $pluginEnabled = $pluginEnabled.Trim()
    # Check if null or empty values are given as inputs
    if ($zipFilePath -eq "") {
        Write-Host "Validation failed: The zip file path is empty or blank." -ForegroundColor Red
        Write-Host ""
        exit
    }
    if ($pluginEnabled -eq "") {
        Write-Host "Validation failed: The plugin enabled argument is empty or blank." -ForegroundColor Red
        Write-Host ""
        exit
    }
    # Verify using regular expression pattern to match the input file path (example pattern)
    $pattern = "^[a-zA-Z]:(\\[^<>:""/\\|?*]+)*\\?[^<>:""/\\|?*]*$"
    if (-not ($zipFilePath -match $pattern)) {
        Write-Host "Validation failed: The zip file path '$zipFilePath' does not look like a valid file path." -ForegroundColor Red
        Write-Host ""
        exit
    }
    # Verify if the zip file exists
    if (-not [System.IO.Path]::IsPathRooted($zipFilePath)) {
        Write-Host "Validation failed: The zip file path '$zipFilePath' is not a valid absolute path or is relative." -ForegroundColor Red
        Write-Host ""
        exit
    }
    $zipfileName = Split-Path -Path $zipFilePath -Leaf
    # Use Test-Path to check if the path exists and is a directory
    if (Test-Path -Path $zipFilePath -PathType Container) {
        Write-Host "Validation failed: The path '$zipFilePath' contains a folder '$zipfileName' instead of a zip file." -ForegroundColor Red
        Write-Host ""
        exit
    }
    # Check if the leaf (file name with extension) ends with ".zip"
    if (-not ($zipfileName -like "*.zip")) {
        Write-Host "Validation failed: The file '$zipfileName' provided is not a zip file." -ForegroundColor Red
        Write-Host ""
        exit
    }
    # bool isPluginEnabled from string pluginEnabled
    if (($pluginEnabled -eq 'y') -or ($pluginEnabled -eq 'yes')) {
        $isPluginEnabled = $true
    }
    elseif (($pluginEnabled -eq 'n') -or ($pluginEnabled -eq 'no')) {
        $isPluginEnabled = $false
    }
    else {
        Write-Host "Validation failed: The second argument '$pluginEnabled' provided for pluginEnabled is not correct." -ForegroundColor Red 
        Write-Host "It should be either 'y'/'n' or 'yes'/'no'" -ForegroundColor Red
        Write-Host ""
        exit
    }
    # Step 0: Extract the zip location/folder
    # Get the directory containing the file
    $parentFolderPath = Split-Path -Path $zipFilePath -Parent

    $parentFolderPath  = "$parentFolderPath\$zipFileName"

    # Step 1: Extract the contents of the zip file to a temporary folder
    $tempFolder = New-Item -ItemType Directory -Path (Join-Path $env:TEMP (New-Guid))

    Expand-Archive -Path $zipFilePath -DestinationPath $tempFolder -Force

    # Step 2: First Folder should contain intro.md and *.pdpkg.zip - but we prefer not to check with any name and only check extensions
    $folderPath = "$tempFolder"
    $expectedFolderCount = 0
    $expectedFileCounts = @{
        "*.md" = 1
        "*.zip" = 1
    }

    # Call the validation function
    $resultOfValidation = ValidateFolderAndFilesInPackage -pluginEnabled $isPluginEnabled -levelOfHierarchy 1 -parentFolderPath $parentFolderPath -folderPath $folderPath -expectedFolderCount $expectedFolderCount -expectedFileCounts $expectedFileCounts
    if (-not $resultOfValidation)
    {
        DisplayReferDocumentation
        Write-Host ""
        Write-Host "Validation failed: Invalid package structure. Check previous messages for details." -ForegroundColor Red
        Write-Host ""
        RemoveTempFolder $tempFolder
        exit
    }
    # Step 3: Extract the zip file in first level

    $firstLevelZipFile = Get-ChildItem -Path  $tempFolder -Filter "*.zip" | Select-Object -ExpandProperty Name
    # Update the corret currentFolder to the parentFolderPath
    $parentFolderPath = "$parentFolderPath\$firstLevelZipFile"

    $firstLevelZipFile = "$tempFolder/$firstLevelZipFile"
    $tempFolder2 = New-Item -ItemType Directory -Path (Join-Path $env:TEMP (New-Guid))

    Expand-Archive -Path $firstLevelZipFile -DestinationPath $tempFolder2 -Force

    # Step 4: The extracted Folder in temp path should contain 1 pkgassets folder and 3 files with .xml, ,dll, .pdb estensions each - we check with only check extensions
    $folderPath = "$tempFolder2"
    $expectedFolderCount = 1
    $expectedFileCounts = $null # no need to consider the files present normally as these are dependent on environments xml pdb

    # Call the validation function
    $resultOfValidation = ValidateFolderAndFilesInPackage -pluginEnabled $isPluginEnabled -levelOfHierarchy 2 -parentFolderPath $parentFolderPath -folderPath $folderPath -expectedFolderCount $expectedFolderCount -expectedFileCounts $expectedFileCounts
    if (-not $resultOfValidation)
    {
        DisplayReferDocumentation
        Write-Host ""
        Write-Host "Validation failed: Invalid package structure. Check previous messages for details." -ForegroundColor Red
        Write-Host ""
        RemoveTempFolder $tempFolder
        RemoveTempFolder $tempFolder2
        exit
    }

    # Step 5: The contents in extracted temp path folder should have no folder, 3 zip files if plugin enabled, 2 zip files if no plugin
    $pkgAssetFoldereName = Get-ChildItem -Path $tempFolder2 -Directory

    $folderPath = "$tempFolder2/$pkgAssetFoldereName"

    # Update the corret currentFolder to the parentFolderPath
    $parentFolderPath = "$parentFolderPath/$pkgAssetFoldereName"

    $expectedFolderCount = 0
    $expectedFileCounts  = if ($isPluginEnabled) { 
        @{
            "*.zip" = 3
            # no need to consider the files present normally .xml .json as they are env specific
        } 
    } else {
        @{
            "*.zip" = 2
            # no need to consider the files present normally .xml .json as they are env specific
        } 
    }

    # Call the function
    $resultOfValidation = ValidateFolderAndFilesInPackage -pluginEnabled $isPluginEnabled -levelOfHierarchy 3 -parentFolderPath $parentFolderPath -folderPath $folderPath -expectedFolderCount $expectedFolderCount -expectedFileCounts $expectedFileCounts
    if ($resultOfValidation) {
        Write-Host "Validation successful: The package structure is correct." -ForegroundColor Green
        Write-Host ""
    } else {
        DisplayReferDocumentation
        Write-Host ""
        Write-Host "Validation failed: Invalid package structure. Check previous messages for details." -ForegroundColor Red
        Write-Host ""
    }
}
catch {
    Write-Host "Validation failed: Internal issue while working with current folder as '$parentFolderPath' - error occurred: $($_.Exception.Message)" -ForegroundColor Yellow
    Write-Host ""
}
finally {
    RemoveTempFolder $tempFolder
    RemoveTempFolder $tempFolder2
}
