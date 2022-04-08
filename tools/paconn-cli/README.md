# Microsoft Power Platform Connectors CLI

**NOTE**

**These release notes describe functionality that may not have been released yet.** To see when this functionality is planned to release, please review [What's new and planned for Common Data Model and Data Integration](https://docs.microsoft.com/en-us/business-applications-release-notes/April19/cdm-data-integration/planned-features). Delivery timelines and projected functionality may change or may not ship (see [Microsoft policy](https://go.microsoft.com/fwlink/p/?linkid=2007332)).


The `paconn` command line tool is designed to aid Microsoft Power Platform custom connectors development.

## Installing

1. Install Python 3.5+ from https://www.python.org/downloads. Click the 'Download' link on any version of Python greater than Python 3.5. For Linux and Mac OS X please follow the appropriate link on the page. You can also install using an OS specific package manager of your choice.

2. Run the installer to begin installation and be sure to check the box 'Add Python X.X to PATH'.

3. Make sure the installation path is in the PATH variable by running:

    `python --version`

4. After python is installed, install `paconn` by running:

    `pip install paconn`

  If you get errors saying 'Access is denied', consider using the `--user` option or running the command as an Administrator (Windows).

## Custom Connector Directory and Files

A custom connector consists of three files: An Open API swagger definition, an API properties file, and an optional icon for the connector. The files are generally located in a directory with the connector ID as the name of the directory.

Sometimes, the custom connector directory may include a `settings.json` file. Although this file is not part of the connector definition, it can be used as an argument-store for the CLI.

### API Definition (Swagger) File

The API definition, also known as the swagger, describes the API for the custom connector using the OpenAPI specification. More information about API definition to write custom connector can be found in [the connector documentation on the subject](https://docs.microsoft.com/en-us/connectors/custom-connectors/define-openapi-definition). Please also review the tutorial on [extending an OpenApi definition](https://docs.microsoft.com/en-us/connectors/custom-connectors/openapi-extensions).

### API Properties File

The API properties file contains some properties for the custom connector. These properties are not part of the API definition. It contains information such as the brand color, authentication information, etc. A typical API properties file looks like the following:

```json
{
  "properties": {
    "capabilities": [],
    "connectionParameters": {
      "api_key": {
        "type": "securestring",
        "uiDefinition": {
          "constraints": {
            "clearText": false,
            "required": "true",
            "tabIndex": 2
          },
          "description": "The KEY for this API",
          "displayName": "KEY",
          "tooltip": "Provide your KEY"
        }
      }
    },
    "iconBrandColor": "#007EE6",
    "scriptOperations": [
        "getCall",
        "postCall",
        "putCall"
    ],
    "policyTemplateInstances": [
      {
        "title": "MyPolicy",
        "templateId": "setqueryparameter",
        "parameters": {
            "x-ms-apimTemplateParameter.name": "queryParameterName",
            "x-ms-apimTemplateParameter.value": "queryParameterValue",
            "x-ms-apimTemplateParameter.existsAction": "override"
        }
      }
    ]    
  }
}
```

More information on the each of the properties are given below:

* `properties`: The container for the information.

* `connectionParameters`: Defines the connection parameter for the service.

* `iconBrandColor`: The icon brand color in HTML hex code for the custom connector.

* `scriptOperations`: A list of the operations that are executed with the script file. An empty scriptOperations list indicates that all operations are executed with the script file.

* `capabilities`: Describes the capabilities for the connector, e.g. cloud only, on-prem gateway etc.

* `policyTemplateInstances`: An optional list of policy template instances and values used in the custom connector.

### Icon File

The icon is icon image file for the custom connector.

### Script File

The script is a CSX script file that is deployed for the custom connector and executed for every call to a subset of the connector's operations.

### Settings File

Instead of providing the arguments in the command line, a `settings.json` file can be used to specify them. A typical `settings.json` file looks like the following: 

```json
{
  "connectorId": "CONNECTOR-ID",
  "environment": "ENVIRONMENT-GUID",
  "apiProperties": "apiProperties.json",
  "apiDefinition": "apiDefinition.swagger.json",
  "icon": "icon.png",
  "script": "script.csx",
  "powerAppsApiVersion": "2016-11-01",
  "powerAppsUrl": "https://api.powerapps.com"
}
```

In the settings file the following items are expected. If an option is missing but required, the console will prompt for the missing information.

* `connectorId`: The connector ID string for the custom connector. This parameter is required for download and update operations, but not for the create operation since a new custom connector with the new ID will be created. It's also not needed for the validate command. If you need to update a custom connector just created using the same settings file, please make sure the settings file is correctly updated with the new connector ID from the create operation.

* `environment`: The environment ID string for the custom connector. This parameter is required for all operations, except the validate operation.

* `apiProperties`: The path to the API properties file. It is required for the create and update operation. When this option is present during the download operation, the file will be downloaded to the given location, otherwise it will be saved as `apiProperties.json`.

* `apiDefinition`: The path to the swagger file. It is required for the create, update, and validate operations. When this option is present during the download operation, the file in the given location will be written to, otherwise it will be saved as `apiDefinition.swagger.json`.

* `icon`: The path to the optional icon file. The create and update operations will use the default icon when this parameter is no specified. When this option is present during the download operation, the file in the given location will be written to, otherwise it will be saved as `icon.png`.

* `script`: The path to the optional script file. The create and update operations will use only the value within the specified parameter. When this option is present during the download operation, the file in the given location will be written to, otherwise it will be saved as `script.csx`.

* `powerAppsUrl`: The API URL for PowerApps. This is optional and set to `https://api.powerapps.com` by default.

* `powerAppsApiVersion`: The API version to use for PowerApps. This is optional and set to `2016-11-01` by default.


## Command Line Operations

### Login

Login to Power Platform by running:

`paconn login`

This will ask you to login using device code login process. Please follow the prompt for the login. Service Principle authentication is not supported at this point. Please review [a customer workaround posted in the issues page](https://github.com/microsoft/PowerPlatformConnectors/issues/287).

### Logout

Logout by running:
   
`paconn logout`

### Download Custom Connector Files

The connector files are always downloaded into a sub-directory with the connector ID as the directory name. When a destination directory is specified, the sub-directory will be created in the specified one. Otherwise, it will be created in the current directory. In addition to the three connector files, the download operation will also write a fourth file called settings.json containing the parameters used to download the files.

Download the custom connector files by running:

`paconn download`

or
   
`paconn download -e [Power Platform Environment ID] -c [Connector ID]`
   
or
   
`paconn download -s [Path to settings.json]`

When environment or connector ID is not specified the command will prompt for the missing argument(s). The command will print the download location for the connector on successful completion.

```
Arguments
   --cid -c       : The custom connector ID.
   --dest -d      : Destination directory.   
   --env -e       : Power Platform environment ID.
   --overwrite -w : Overwrite all the existing connector and settings files.
   --pau -u       : Power Platform URL.
   --pav -v       : Power Platform API version.
   --settings -s  : A settings file containing required parameters.
                    When a settings file is specified some command 
                    line parameters are ignored.
```
### Create a New Custom Connector

A new custom connector can be created from the connectors files using the `create` operation. Create a connector by running:
   
`paconn create --api-prop [Path to apiProperties.json] --api-def [Path to apiDefinition.swagger.json]`
   
or
   
`paconn create -e [Power Platform Environment ID] --api-prop [Path to apiProperties.json] --api-def [Path to apiDefinition.swagger.json] --icon [Path to icon.png] --secret [The OAuth2 client secret for the connector]`
   
or
   
`paconn create -s [Path to settings.json] --secret [The OAuth2 client secret for the connector]`

When environment is not specified the command will prompt for it. However, the API definition and API properties file must be provided as part of the command line argument or a settings file. The OAuth2 secret must be provided for a connector using OAuth2. The command will print the connector ID for the newly created custom connector on successful completion. If you are using a settings file for the create command, please make sure to update it with the new connector ID before you update the newly created connector.

```
Arguments
   --api-def     : Location for the Open API definition JSON document.
   --api-prop    : Location for the API properties JSON document.
   --env -e      : Power Platform environment ID.
   --icon        : Location for the icon file.
   --script -x   : Location for the script file.
   --pau -u      : Power Platform URL.
   --pav -v      : Power Platform API version.
   --secret -r   : The OAuth2 client secret for the connector.
   --settings -s : A settings file containing required parameters.
                   When a settings file is specified some command 
                   line parameters are ignored.
```
### Update an Existing Custom Connector

Like the `create` operation, an existing custom connector can be updated using the `update` operation. Update a connector by running:
   
`paconn update --api-prop [Path to apiProperties.json] --api-def [Path to apiDefinition.swagger.json]`
   
or
   
`paconn update -e [Power Platform Environment ID] -c [Connector ID] --api-prop [Path to apiProperties.json] --api-def [Path to apiDefinition.swagger.json] --icon [Path to icon.png] --secret [The OAuth2 client secret for the connector]`
   
or
   
`paconn update -s [Path to settings.json] --secret [The OAuth2 client secret for the connector]`

When environment or connector ID is not specified the command will prompt for the missing argument(s). However, the API definition and API properties file must be provided as part of the command line argument or a settings file. The OAuth2 secret must be provided for a connector using OAuth2. The command will print the updated connector ID on successful completion. If you are using a settings file for the update command, please make sure correct environment and connector ID are specified.
  
```
Arguments
   --api-def     : Location for the Open API definition JSON document.
   --api-prop    : Location for the API properties JSON document.
   --cid -c      : The custom connector ID.
   --env -e      : Power Platform environment ID.
   --icon        : Location for the icon file.
   --script -x   : Location for the script file.
   --pau -u      : Power Platform URL.
   --pav -v      : Power Platform API version.
   --secret -r   : The OAuth2 client secret for the connector.
   --settings -s : A settings file containing required parameters.
                   When a settings file is specified some command 
                   line parameters are ignored.
   ```

### Validate a Swagger JSON

The validate operation takes a swagger file and verfies if it follows all the recommended rules. Validate a swagger file by running:
   
`paconn validate --api-def [Path to apiDefinition.swagger.json]`

or

`paconn validate -s [Path to settings.json]`

The command will print the error, warning, or success message depending result of the validation.
  
```
Arguments
   --api-def     : Location for the Open API definition JSON document.
   --pau -u      : Power Platform URL.
   --pav -v      : Power Platform API version.
   --settings -s : A settings file containing required parameters.
                   When a settings file is specified some command 
                   line parameters are ignored.
   ```


### Best Practice

Download all of your connectors and use git or any other source code management system to save the files. In case of an incorrect update, redeploy the connector by rerunning the update command with the correct set of files from the source code management system.

Please test the custom connector and the settings file in a test environment before deploying in the production environment. Always double check that the environment and connector id are correct.

## Limitations

The project is limited to creation, update, and download of custom connector in flow and powerapps environment. When an environment is not specified only the flow envrionments are displayed to choose from. For non-custom connector the swagger file is not returned.

**Stack Owner Property & apiProperties file:**

Currently, there is a limitation that prevents you from updating your connector's artificats in your environment using Paconn when the `stackOwner` property is present in your `apiProperties.json` file. As a workaround to this, create two versions of your connector artifacts: the first being the version that is submitted to certification and contains the `stackOwner` property, the second having the `stackOwner` property omitted to enable updating within your environment. We are working to remove the limitation and will update this section once complete. 

## Reporting issues and feedback

If you encounter any bugs with the tool please file an issue in the [Issues](https://github.com/Microsoft/PowerPlatformConnectors/issues) section of our GitHub repo.

If you believe you have found a security vulnerability that meets [Microsoft's definition of a security vulnerability](https://docs.microsoft.com/en-us/previous-versions/tn-archive/cc751383%28v=technet.10%29), please submit the [report to MSRC](https://msrc.microsoft.com/create-report). More information can be found at [MSRC frequently asked questions on reporting](https://www.microsoft.com/en-us/msrc/faqs-report-an-issue).


## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

To contibute a connector to the open source repo, please start by creating a fork on the github repo.
Once you have the fork created, create a new branch on the forked repo. Clone this forked repo on you 
local machine, and checkout the branch. Create a folder for your connector under the `connectors` folder
and place the connector files in the sub-folder. Commit and push the changes to your forked branch.
Create a pull request from the forked branch to the main repo to merge your changes into the main repo.
[Please see this document for more information](https://github.com/CoolProp/CoolProp/wiki/Contributing:-git-development-workflow).

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Legal Notices

Microsoft and any contributors grant you a license to the Microsoft documentation and other content
in this repository under the [Creative Commons Attribution 4.0 International Public License](https://creativecommons.org/licenses/by/4.0/legalcode),
see the [LICENSE](LICENSE) file, and grant you a license to any code in the repository under the [MIT License](https://opensource.org/licenses/MIT), see the
[LICENSE-CODE](LICENSE-CODE) file.

Microsoft, Windows, Microsoft Azure and/or other Microsoft products and services referenced in the documentation
may be either trademarks or registered trademarks of Microsoft in the United States and/or other countries.
The licenses for this project do not grant you rights to use any Microsoft names, logos, or trademarks.
Microsoft's general trademark guidelines can be found at http://go.microsoft.com/fwlink/?LinkID=254653.

Privacy information can be found at https://privacy.microsoft.com/en-us/

Microsoft and any contributors reserve all others rights, whether under their respective copyrights, patents,
or trademarks, whether by implication, estoppel or otherwise.

## License

```
Microsoft Power Platform Connectors CLI (paconn)

Copyright (c) Microsoft Corporation
All rights reserved.

MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
```

