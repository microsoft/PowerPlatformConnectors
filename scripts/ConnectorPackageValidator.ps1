param (
    [Parameter(HelpMessage = "Specify valid zip file path.", Mandatory = $true)][string]$zipFilePath,
    [Parameter(HelpMessage = "Specify 'y'/'yes' (case-insensitive) if plugin is enabled else specify 'n'/'no' (case-insensitive).", Mandatory = $true)][string]$isPluginEnabled
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

function CheckValidFile($expectedFiles, $actualFiles) {
    $missingFiles = $null
    foreach ($filePattern in $expectedFiles) {
        $matchingFiles = $actualFiles | Where-Object { $_.Name -eq $filePattern }
        if ($matchingFiles.Count -eq 0) {
            if (-not $missingFiles) {
                $missingFiles = @()
            }
            $missingFiles  +=  $filePattern            
        } 
    }
    return $missingFiles   
}

function ValidateFolderAndFilesInPackage {
    param (
        [bool] $isPluginEnabled,
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
                                    $detailsSolutionZip = if ($isPluginEnabled) {"connector, flow and AIplugin "} else {"connector and flow"}
                                    $validationMessages += "Please add the required $detailsSolutionZip solution zip files."
                                }
                                elseif ((-not $isPluginEnabled) -and (($expectedCount -eq 2) -and ($actualCount -gt 2)))
                                {
                                    $detailsSolutionZip = if ($isPluginEnabled) {"connector, flow and AIplugin "} else {"only connector and flow"}
                                    $validationMessages += "The package folder '$currentFolderName' should contain $detailsSolutionZip solution zip files in '$parentFolderPath'."
                                }
                                elseif ($isPluginEnabled -and (($expectedCount -eq 3) -and ($actualCount -lt 3)))
                                {
                                    $detailsSolutionZip = if ($isPluginEnabled) {"connector, flow and AIplugin "} else {"connector and flow"}
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
    $isPluginEnabled = $isPluginEnabled -replace '"', ''
    $isPluginEnabled = $isPluginEnabled -replace "'", ''
    $zipFilePath = $zipFilePath.Trim()
    $isPluginEnabled = $isPluginEnabled.Trim()
    # Check if null or empty values are given as inputs
    if ($zipFilePath -eq "") {
        Write-Host "Validation failed: The zip file path is empty or blank." -ForegroundColor Red
        Write-Host ""
        exit
    }
    if ($isPluginEnabled -eq "") {
        Write-Host "Validation failed: The isPluginEnabled argument is empty or blank." -ForegroundColor Red
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
    # bool isBoolPluginEnabled from string isPluginEnabled
    if (($isPluginEnabled -eq 'y') -or ($isPluginEnabled -eq 'yes')) {
        $isBoolPluginEnabled = $true
    }
    elseif (($isPluginEnabled -eq 'n') -or ($isPluginEnabled -eq 'no')) {
        $isBoolPluginEnabled = $false
    }
    else {
        Write-Host "Validation failed: The second argument '$isPluginEnabled' provided for isPluginEnabled is not correct." -ForegroundColor Red 
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
    $resultOfValidation = ValidateFolderAndFilesInPackage -isPluginEnabled $isBoolPluginEnabled -levelOfHierarchy 1 -parentFolderPath $parentFolderPath -folderPath $folderPath -expectedFolderCount $expectedFolderCount -expectedFileCounts $expectedFileCounts
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
    $resultOfValidation = ValidateFolderAndFilesInPackage -isPluginEnabled $isBoolPluginEnabled -levelOfHierarchy 2 -parentFolderPath $parentFolderPath -folderPath $folderPath -expectedFolderCount $expectedFolderCount -expectedFileCounts $expectedFileCounts
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
    $expectedFileCounts  = if ($isBoolPluginEnabled) { 
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

    # Call the validation function
    $resultOfValidation = ValidateFolderAndFilesInPackage -isPluginEnabled $isBoolPluginEnabled -levelOfHierarchy 3 -parentFolderPath $parentFolderPath -folderPath $folderPath -expectedFolderCount $expectedFolderCount -expectedFileCounts $expectedFileCounts
    if (-not $resultOfValidation) {
        DisplayReferDocumentation
        Write-Host ""
        Write-Host "Validation failed: Invalid package structure. Check previous messages for details." -ForegroundColor Red
        Write-Host ""
        RemoveTempFolder $tempFolder
        RemoveTempFolder $tempFolder2
        exit
    }

    # Check inside the solution zip
    $secondLevelZipFiles = Get-ChildItem -Path  "$tempFolder2/$pkgAssetFoldereName" -Filter "*.zip" | Select-Object -ExpandProperty Name
    $originalParentFolderPath = $parentFolderPath
    $nodesToCheck = @()
    $isConnectorSolutionPresent= $false
    $isFlowSolutionPresent = $false
    $isPluginSolutionPresent = $false
    foreach ($secondLevelZipFile in $secondLevelZipFiles) {
        $isConnectorFound = $false
        $isWorkflowsFound = $false
        $isPluginFound = $false

        $tempFolder3 = New-Item -ItemType Directory -Path (Join-Path $env:TEMP (New-Guid))
        # Update the corret currentFolder to the parentFolderPath
        $parentFolderPath = "$originalParentFolderPath/$secondLevelZipFile"

        $secondLevelZipFilePath = "$tempFolder2/$pkgAssetFoldereName/$secondLevelZipFile"
        Expand-Archive -Path $secondLevelZipFilePath -DestinationPath $tempFolder3 -Force

        $actualFiles = Get-ChildItem -Path $tempFolder3 -File
        $expectedFiles = @("[Content_Types].xml", "customizations.xml", "solution.xml")
        $missingFiles = $null
        $missingFiles = CheckValidFile $expectedFiles $actualFiles
        # Call the validation function
        if ($missingFiles) {
            Write-Host "The solution zip file '$secondLevelZipFile' in '$originalParentFolderPath' should contain three files namely '[Content_Types].xml', 'customizations.xml', 'solution.xml'."
            Write-Host "Please add the missing '$($missingFiles -join ', ')' to the solution zip file."
            Write-Host "NOTE: These files are by default present in an exported solution and should not be manually modified/removed. If done, please revert the changes."
            DisplayReferDocumentation
            Write-Host ""
            Write-Host "Validation failed: Invalid package structure. Check previous messages for details." -ForegroundColor Red
            Write-Host ""
            RemoveTempFolder $tempFolder
            RemoveTempFolder $tempFolder2
            RemoveTempFolder $tempFolder3
            exit
        }
        # Read the customizations.xml and look for any node "Connector" or "Workflows". If they exist, validate if the folders also exist
        # Define the path to your XML file and the node name you want to check
        $xmlFilePath = "$tempFolder3/customizations.xml"
        $connectorNodeName = "Connector"
        # Load the XML file into an XmlDocument object
        [xml]$xmlContent = Get-Content -Path $xmlFilePath
        # Use XPath to find the node
        $connectorNode = $xmlContent.SelectSingleNode("//$connectorNodeName")

        # Check if the connector node exists and set the appropriate flags
        if ($connectorNode -ne $null) {
            if ($connectorNode.HasChildNodes -or $connectorNode.Attributes.Count -gt 0) {
                $isConnectorFound = $true
            }
        }

        $workflowsNodeName = "Workflows"
        # Use XPath to find the node
        $workflowsNode = $xmlContent.SelectSingleNode("//$workflowsNodeName")

        # Check if the workflows node exists and set the appropriate flags
        if ($workflowsNode -ne $null) {
            if ($workflowsNode.HasChildNodes -or $workflowsNode.Attributes.Count -gt 0) {
                $isWorkflowsFound = $true
            }
        }

        if ($isBoolPluginEnabled) {
            # aiplugin operation
            $pluginNodeName = "aicopilot_aiplugin"
            # Use XPath to find the node
            $pluginNode = $xmlContent.SelectSingleNode("//$pluginNodeName")

            # Check if the aicopilot_aiplugin node exists and set the appropriate flags
            if ($pluginNode -ne $null) {
                if ($pluginNode.HasChildNodes -or $pluginNode.Attributes.Count -gt 0) {
                    $isPluginFound = $true
                }
            }
        }
        
        # categorize each of the solution zip
        if (($isConnectorFound) -and (-not $isWorkflowsFound) -and (-not $isPluginFound)) {
            $isConnectorSolutionPresent = $true
        }
        if (($isConnectorFound) -and ($isWorkflowsFound) -and (-not $isPluginFound)) {
            $isFlowSolutionPresent = $true
        }
        if (($isConnectorFound) -and (-not $isWorkflowsFound) -and ($isPluginFound)) {
            $isPluginSolutionPresent = $true
        }
        # check if the connector folder name is present with the same name
        if ($isConnectorFound) {
            $connectorFolder = Get-ChildItem -Path "$tempFolder3" -Directory | Where-Object {$_.Name -eq $connectorNodeName}
            if (-not $connectorFolder) {
                Write-Host "The solution zip file '$secondLevelZipFile' in '$originalParentFolderPath' should contain one folder namely '$connectorNodeName."
                Write-Host "The folders should match the customization.xml file. They are present by default in an exported solution and should not be modified/removed manually."
                Write-Host "Please add the required folder to the solution zip or export the correctsolution again."
                DisplayReferDocumentation
                Write-Host ""
                Write-Host "Validation failed: Invalid package structure. Check previous messages for details." -ForegroundColor Red
                Write-Host ""
                RemoveTempFolder $tempFolder
                RemoveTempFolder $tempFolder2
                RemoveTempFolder $tempFolder3
                exit
            }
        }

        if ($isWorkflowsFound) {
            # check if the folder name is present with the same name
            $connectorFolder = Get-ChildItem -Path "$tempFolder3" -Directory | Where-Object {$_.Name -eq $connectorNodeName}
            $workFlowsFolder = Get-ChildItem -Path "$tempFolder3" -Directory | Where-Object {$_.Name -eq $workflowsNodeName}
            if ((-not $workFlowsFolder) -or (-not $connectorFolder)) {
                $folderNameMissing = if ((-not $workFlowsFolder) -and $connectorFolder) {"folder namely '$workflowsNodeName'"} elseif ((-not $connectorFolder) -and $workFlowsFolder) {"folder namely '$connectorNodeName"} else {"folders namely '$connectorNodeName', '$workflowsNodeName'"}
                Write-Host "The solution zip file '$secondLevelZipFile' in '$originalParentFolderPath' should contain '$folderNameMissing'."
                Write-Host "The folders should match the customization.xml file. They are present by default in an exported solution and should not be modified/removed manually."
                Write-Host "Please add the required folder to the solution zip or export the correct solution again."
                DisplayReferDocumentation
                Write-Host ""
                Write-Host "Validation failed: Invalid package structure. Check previous messages for details." -ForegroundColor Red
                Write-Host ""
                RemoveTempFolder $tempFolder
                RemoveTempFolder $tempFolder2
                RemoveTempFolder $tempFolder3
                exit
            }
        }
        
        if ($isPluginFound) {
            # check if the AI plugin related folder names are present with the same name
            $connectorFolder = Get-ChildItem -Path "$tempFolder3" -Directory | Where-Object {$_.Name -eq $connectorNodeName}
            $folderNameToCheck1 = "aiplugins"
            $nodeNameFolder1 = Get-ChildItem -Path "$tempFolder3" -Directory | Where-Object {$_.Name -eq $folderNameToCheck1}
            $folderNameToCheck2 = "aipluginoperations"
            $nodeNameFolder2 = Get-ChildItem -Path "$tempFolder3" -Directory | Where-Object {$_.Name -eq $folderNameToCheck2}
            
            if ((-not $connectorFolder) -or (-not $nodeNameFolder1) -or (-not $nodeNameFolder2)) {
                $folderNameMissing = if ((-not $nodeNameFolder1) -and $connectorFolder -and $nodeNameFolder2) {"folder namely '$folderNameToCheck1'"} elseif ((-not $connectorFolder) -and $nodeNameFolder1 -and $nodeNameFolder2) {"folder namely '$connectorNodeName"} elseif ((-not $nodeNameFolder2) -and $connectorFolder -and $nodeNameFolder2) {"folder namely '$folderNameToCheck2"} elseif ((-not $nodeNameFolder1) -and (-not $connectorFolder) -and $nodeNameFolder2) {"folders namely '$connectorNodeName', '$folderNameToCheck1'"} elseif ((-not $nodeNameFolder2) -and (-not $connectorFolder) -and $nodeNameFolder1) {"folders namely '$connectorNodeName', '$folderNameToCheck2'"} elseif ((-not $nodeNameFolder1) -and (-not $nodeNameFolder2) -and $connectorFolder) {"folders namely '$folderNameToCheck1', '$folderNameToCheck2'"} else {"folders namely '$connectorNodeName', '$folderNameToCheck1'. '$folderNameToCheck2'"}
                Write-Host "The solution zip file '$secondLevelZipFile' in '$originalParentFolderPath' should contain '$folderNameMissing"
                Write-Host "The folders should match the customization.xml file. They are present by default in an exported solution and should not be modified/removed manually."
                Write-Host "Please add the required folders to the solution zip or export the correct solution again."
                DisplayReferDocumentation
                Write-Host ""
                Write-Host "Validation failed: Invalid package structure. Check previous messages for details." -ForegroundColor Red
                Write-Host ""
                RemoveTempFolder $tempFolder
                RemoveTempFolder $tempFolder2
                RemoveTempFolder $tempFolder3
                exit
            } 
        }
    } 
    write-Host "****** isPluginEnabled: $isPluginEnabled, isBoolPluginEnabled: $isBoolPluginEnabled ******"  
    if (-not $isConnectorSolutionPresent) {
        Write-Host "Connector solution in '$originalParentFolderPath' is invalid. Connector solution should only contain 'Connector' folder."
        Write-Host "Validate the connector solution has no extra component except 'Connector'. If not so, please remove them from the solution and export again."
        $resultOfValidation = $false
    }
    elseif (-not $isFlowSolutionPresent) {
        Write-Host "Flow solution in '$originalParentFolderPath' is invalid. Flow solution should contain both 'Connector' and 'Workflows' folders."
        Write-Host "Validate the flow solution has both 'Connector' and 'Workflows' components. If not so, please recreate the solution and export again."
        $resultOfValidation = $false
    }
    elseif ($isBoolPluginEnabled -and (-not $isPluginSolutionPresent)) {
        Write-Host "Plugin solution in '$originalParentFolderPath' is invalid. Plugin solution should contain 'Connector', 'aiplugins' and 'aipluginoperations' folders."
        Write-Host "Validate the plugin solution has 'Connector', 'aiplugins' and 'aipluginoperations' components. If not so, please recreate the solution and export again."
        $resultOfValidation = $false
    }
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
