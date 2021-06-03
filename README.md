# Microsoft Power Platform Connectors

Welcome to the Microsoft Power Platform Connectors open source repository. This repository contains custom connectors, certified connectors, and related tools to facilitate connector development for Azure Logic Apps, Microsoft Power Apps, and Microsoft Power Automate.

## Custom Connectors

The ```custom-connectors``` folder contains fully functional connector samples which can be deployed to the Power Platform for extension and use. These samples may not be certified connectors, but should be maintained by the open source community to offer useful scenarios or examples of connector concepts.

## Certified Connectors

The ```certified-connectors``` folder contains certified connectors which are already deployed and available out-of-box within the Power Platform for use. 
A requirement of our [connector certification program](https://docs.microsoft.com/connectors/custom-connectors/submit-certification) is that new certified connectors be open sourced for community contributions. 
The ```certified-connectors``` folder is managed by the Microsoft Connector Certification Team to ensure that within the ```master``` branch, the connector version is identical to that deployed in the Power Platform. 
The ```dev``` branch is maintained by the connector owner and the Microsoft Connector Certification Team to allow community development of the connector prior to certification and deployment of a version. 

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

### Files to Include
Please submit the following files: An Open API swagger definition, an API properties file, a README.md, and an Intro.md.

### API Definition (Swagger) File

The API definition, also known as the swagger, describes the API for the custom connector using the OpenAPI specification.

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

* `capabilities`: Describes the capabilities for the connector, e.g. cloud only, on-prem gateway etc.

* `policyTemplateInstances`: An optional list of policy template instances and values used in the custom connector.

### README.md

README.md file for your connector includes a description for your connector, any prerequisite customer may need to deploy your connector, how to use your connector and api, how to get credentials, known issues and limiations, etc. This file is meant to be a standalone guide for deploying and using your connector by other users and developers. A sample can be found [here](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/custom-connectors/AzureKeyVault/Readme.md).

### Creating a Fork

To contibute to this open source repository, start by creating a fork on this repository. 

Clone Microsoft's Connectors repository

```git clone https://github.com/microsoft/PowerPlatformConnectors.git```

Pull to make sure the repo is up-to-date

```git pull```

Create a new branch for your connector 

```git checkout -b YOUR_GITHUB_ACCOUNT_NAME/YOUR_CONNECTOR_NAME```

You are now ready to develop your connector in your own branch. Create a folder with your connector name (without spaces) inside 'certified-connectors' or 'custom-connectors' based on your connector type

Stage your connector files

```git add -A```

Make sure that you are adding or updating your connector files only

```git status```

Commit your changes

For new connectors: ```git commit -m "[YUOR_CONNECTOR_NAME] New Partner Connector```

For updates: ```git commit -m "[YUOR_CONNECTOR_NAME] Partner Connector Updates```

Push your branch to origin ore remote

```git push origin YOUR_GITHUB_ACCOUNT_NAME/YOUR_CONNECTOR_NAME```

### Submitting to the Open Source Repository

Contributions to the open source repository are made through pull requests.
Prior to submitting a pull request, ensure that your pull request does not contain any sensitive or specific information, for example Client IDs or Client Secrets. 
Any sensitive values can be replaced with fake or dummy values for the purposes of submission as long as it is clearly indicated. 
Also, ensure that the readme.md of the connector is updated with the latest information, or created for new connector submissions. 
An example of a clear, structured, readme.md can be found in the [Azure Key Vault](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/custom-connectors/AzureKeyVault/Readme.md) connector repository. 
Include this completed `readme.md` in same connector directory which contains the artifacts. 

#### Certified Connectors

For new connectors which will be submitted for certification, create a directory under the ```certified-connectors``` directory, place the connector files in the sub-folder, and submit a pull request to the ```dev``` branch. Ensure that a clear, structured, readme.md is included. 

Updates to certified connectors must first be made through a pull request to the ```dev``` branch for review by the connector owner. 

Once a pull request has been merged to the ```dev``` branch, the connector owner can submit the connector for certification through the Connector certification tab in [ISV Studio](https://isvstudio.powerapps.com). Once certified, the Microsoft Certification team will handle merging the updates from ```dev``` to ```master```. 

Updates to an existing custom connector can be made through a simple pull request to the ```dev``` branch to update the custom connector files.

#### Custom Connectors

Updates to an existing custom connector can be made through a simple pull request to update the custom connector files.

For new custom connectors, create a directory under the ```custom-connectors``` directory and place the connector files in the sub-folder. Ensure that a clear, structured, readme.md is included. 

### Tooling and Validation

#### CLA

When a pull request is submitted, a CLA-bot will automatically determine whether you need to provide
a CLA and annotate the PR appropriately. Simply follow the instructions
provided by the bot to ensure your pull request can be properly reviewed.
You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

#### Swagger Validation

A submitted pull request will also be validated against our Swagger Validator tool, which checks the connector files to ensure it is a proper Swagger file and adheres to our connector requirements and guidelines. Any errors or warnings will be added to the PR for both the submitter and the reviewer to understand. We do not accept pull requests with outstanding unresolved Swagger Validator issues. 

#### Breaking Change Detector

Another validation which runs on a submitted pull request is the breaking changes validator. This is to catch any inadvertent, non-backwards-compatible (i.e. breaking) changes which may break a current user experience, for example, deleting a published operation. The Breaking Change Detector compares the previous version of the Swagger with the new submission and raises awareness of any breaking change. The submitter and reviewer must both acknowledge any breaking changes submitted and ensure that no end users are inadvertently negatively affected. 

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
