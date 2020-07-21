# Microsoft Power Platform Connectors

Welcome to the Microsoft Power Platform Connectors open source repository. This repository contains custom connectors, certified connectors, and related tools to facilitate connector development for Azure Logic Apps, Microsoft Power Apps, and Microsoft Power Automate.

## Custom Connectors

The ```custom-connectors``` folder contains fully functional connector samples which can be deployed to the Power Platform for extension and use. These samples may not be certified connectors, but should be maintained by the open source community to offer useful scenarios or examples of connector concepts.

## Certified Connectors

The ```certified-connectors``` folder contains certified connectors which are already deployed and available out-of-box within the Power Platform for use. A requirement of our [connector certification program](https://docs.microsoft.com/connectors/custom-connectors/submit-certification) is that new certified connectors be open sourced for community contributions. The ```certified-connectors``` folder is managed by the Microsoft Connector Certification Team to ensure that within the ```master``` branch, the connector version is identical to that deployed in the Power Platform. The ```dev``` branch is maintained by the connector owner and the Microsoft Connector Certification Team to allow community development of the connector prior to certification and deployment of a version. 

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

### Creating a Fork

To contibute to this open source repository, start by creating a fork on this repository. To do so, select the "fork" button on the upper right corner, and create your own copy of the repository. Next, sync your fork with the remote repository and clone your forked repository to your local machine.

```git clone https://github.com/YOUR-USERNAME/PowerPlatformConnectors.git```

Check your remote URL.

```git remote -v```

```origin  https://github.com/YOUR_USERNAME/PowerPlatformConnectors.git``` (fetch)

```origin  https://github.com/YOUR_USERNAME/PowerPlatformConnectors.git``` (push)

Add an upstream repository for your clone.

```git remote add upstream https://github.com/microsoft/PowerPlatformConnectors.git```

Verify the upstream links.

```git remote -v```

```origin    https://github.com/YOUR_USERNAME/PowerPlatformConnectors.git``` (fetch) 

```origin    https://github.com/YOUR_USERNAME/PowerPlatformConnectors.git``` (push) 

```upstream  https://github.com/microsoft/PowerPlatformConnectors.git``` (fetch) 

```upstream  https://github.com/microsoft/PowerPlatformConnectors.git``` (push) 

To keep your fork up to date with this repository's updates, run these commands:

```git fetch upstream```

```git checkout master```

```git merge upstream/master```

You are now ready to develop your connector in your own branch.

### Submitting to the Open Source Repository

For certified connectors, create a folder for your connector under the `certified-connectors` folder and place the connector files in the sub-folder. Otherwise, create a folder for your connector under the `custom-connectors` folder and place your connector files in that sub-folder.

Ensure that you have removed any sensitive information, such as Client IDs, from your artifacts before continuing. Any sensitive information can be replaced with fake values for the purpose of your submission.

Next, create a `readme.md` for your connector. An example can be found in the [Azure Key Vault](https://github.com/microsoft/PowerPlatformConnectors/tree/master/custom-connectors/AzureKeyVault) connector repository. Include this completed `readme.md` in your connector's folder which contains your artifacts. 

Once complete, commit and push the changes to your forked branch. Create a pull request from the forked branch to the main repo to merge your changes into the main repo.
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
