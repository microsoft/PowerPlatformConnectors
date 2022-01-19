# Welcome to the Independent Publisher Connector Directory!

The ```independent-publisher-connectors``` folder contains connectors that are submitted by publishers that do not own the underlying service behind their connector. 
Anyone can submit a new connector to this folder, add functionality to connectors in this folder, and resolve issues related to the connectors in this folder. The folder 
is managed by the Independent Publisher Connector Community, which includes Independent Publishers and Project Coordinators. The master branch is maintained by the Microsoft 
Connector Certification Team, who ensures that the connector version is identical to that deployed in the Power Platform. The dev branch is maintained by the connector
maintainer(s) and the Microsoft Connector Certification Team to allow community development of the connector prior to certification and deployment of a version. 
Click here to read through the Independent Publisher Connector Manifesto.

Please note that if you submit an Independent Publisher connector, it is automatically a premium connector. 

If you're considering building and publishing a connector to a Microsoft first party service, email connectorpartnermgmtteam@microsoft.com with an explanation of the connector and its endpoints. We'll work with you to find publishing options.

## Best Practices for Submission
- You can only submit one connector per PR. This ensures that our validation process runs smoothly.
- The PR for your connector should follow the pattern "Connector Name (Independent Publisher)"
- Please add an email to the support email section. This is in case we need to reach out to you! 
- All Independent Publisher connectors must set iconBrandColor to "#da3b01".
- Please make sure to fill in the privacy policy parameter with the privacy policy for the end service.
- Please make sure that your operation descriptions are detailed. This ensures that the user can understand your operation.
- If your connector uses OAuth, please make sure you provide detailed steps on how to create an app in the readme.md. Otherwise, our team will have to pause certification.
- Please make sure that you add response schemas to your actions, unless the response schema is dynamic. This will ensure that your connector gets more usage :)

## Contributing to this Directory

1. Develop your connector! View the documentation on how to build a connector [here](https://docs.microsoft.com/en-us/connectors/custom-connectors/define-blank)

2. Read through [these](https://docs.microsoft.com/en-us/connectors/custom-connectors/certification-submission-ip) docs to learn how to prepare your connector artifacts so that you can add them to your PR.

3. Create a fork to the ```independent-publisher-connectors directory``` and create your connector in the fork.

4. Submit a pull request to the ```dev``` branch. Ensure that the following files are included: apiProperities.json, apiDefinition.swagger.json, and readme.md. You can find a sample of a readme.md file [here](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/custom-connectors/AzureKeyVault/Readme.md). Fill out the checkbox to acknowledge that you’ve tested this connector. In addition to the check box, you need to provide screenshots within the PR template that prove the connector works. Specifically, you need to add show that at least 3 unique operations (actions/triggers) working within a Flow. This can be in one flow or part of multiple flows. For each one of those flows, paste in screenshots of the Flow succeeding. Furthermore, paste in a screenshot from the Test operations section within the Custom Connector UI. We will not accept PRs without these screenshots.

5. Validation processes: The swagger verification and breaking change validation process will automatically run and leave comments. Please address any requested changes by updating the connector.

6. Once you’ve pushed all necessary changes, the Certification Team will trigger the certification process to run automatically. The script will confirm that either the process has started or that there is an error. If there’s an error, the certification team will leave a comment. Please address this comment.

7. Certification team will release your connector to the testing region and ask you to test your connector. Once you've tested your connector in a preview environment, resolve the comment. Please view our documentation on testing [here](https://docs.microsoft.com/en-us/connectors/custom-connectors/certification-testing). 

8. Microsoft Certification team will release your connector to production and a script will sync dev and master branches.

9. Your GitHub handle will be added to the CODEOWNER file to give you access to submitting PRs directly without having to clone in the future and you will be able to accept changes from other publishers.

## Important Information from the Independent Publisher Connector Manifesto

1. You become an Independent Publisher by submitting a new Independent Publisher connector or by contributing sufficient functionality to an existing Independent Publisher connector. As an Independent Publisher, you agree to maintaining that connector by resolving issues and being listed as one of the “code owners.” As a result, there can be more than one Independent Publisher for each Independent Publisher connector.

2. By adding functionality to an existing Independent Publisher connector without becoming an official Independent Publisher, solving an issue for an Independent Publisher connector, and presenting on an Independent Publisher community call, you are considered an Independent Publisher contributor.

3. It is important to reduce connector duplicates across the Power Platform to reduce the flow and app maker confusion. If a Certified connector or Independent Publisher connector to an end service already exists, it is highly recommended that those changes are submitted to the original Certified or Independent Publisher connector. In cases where the end service has a wide variety of endpoint types and functionality, you can submit an Independent Publisher connector to that service with a more specific name. For example, HubSpot is split up into Marketing, CRM, and CMS.

4. Microsoft doesn’t expect Independent Publishers to understand terms and conditions or other issues that may arise. As a result, Microsoft doesn’t have an obligation to publish your connector to the product. If Microsoft or the Independent Publisher Connector Group receive a complaint from the end service owner, Microsoft reserves the right to remove it from the Power Platform. Microsoft encourages publishers to submit Independent Publisher connectors to end services that we currently do not support to alleviate the risks of the connector having to be removed.

5. If an end service owner reaches out to Microsoft or the Independent Publisher Connector Group and requests that they want ownership of the Independent Publisher connector that connects to their end service, Microsoft reserves the right to provide them with maintainer rights.

Click [here](https://github-wiki-see.page/m/microsoft/PowerPlatformConnectors/wiki/Independent-Publisher-Connector-Group-%22Manifesto%22) to read through the rest of the Independent Publisher Connector Manifesto.

## Top Connector Asks

Looking for a connector to build? Check out our top requests here: https://github.com/microsoft/PowerPlatformConnectors/wiki/Top-Connector-Asks
