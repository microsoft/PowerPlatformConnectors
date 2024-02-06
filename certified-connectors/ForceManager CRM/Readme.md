# Power Platform Connector Certification

## Task checklist

-   Connector local validation (FM task)
-   Github pull request (FM task)
    pull request merge (MS team task)
-   ISV studio form submission (FM task)
-   functional verification (MS team task)
-   deploy on MS environment (MS team task)
-   preview enviroment testing (FM task)
-   release (MS team task)

## Formal MS instructions

This is what Microsoft reported directly, to be followed:

1.  Ensure that your connector complies with Microsoft requirements, as specified in [Meet submission requirements](https://docs.microsoft.com/en-us/connectors/custom-connectors/certification-submission#step-2-meet-submission-requirements).

2.  Preparea set of files called connector artifacts to Microsoft by following the instructions in [Prepare the connector artifacts](https://docs.microsoft.com/en-us/connectors/custom-connectors/certification-submission#step-4-prepare-the-connector-artifacts).

3.  Ensure that your connector [contains valid metadata](https://docs.microsoft.com/en-us/connectors/custom-connectors/certification-submission#step-3-add-metadata).

4.  Submit your connector by following the instructions in [Submit your connector to Microsoft for certification](https://docs.microsoft.com/en-us/connectors/custom-connectors/submit-for-certification).
    a. [Open source the connector files](https://docs.microsoft.com/en-us/connectors/custom-connectors/submit-for-certification#open-source-the-connector). Our Certification Engineers will review within **1-2 weeks**, and you will be notified in GitHub. Please include the below files in your pull request.

        i. apiDefinition.swagger.json
        ii. apiProperties.json
        iii. Readme.md (you can find an example [here](https://github.com/microsoft/PowerPlatformConnectors/blob/dev/templates/certified-connectors/readme.md) and for raw view for code [here](https://raw.githubusercontent.com/microsoft/PowerPlatformConnectors/dev/templates/certified-connectors/readme.md))

    b. [Upload from local files](https://docs.microsoft.com/en-us/connectors/custom-connectors/submit-for-certification#upload-from-local-files) to ISV Studio (If you've been granted an exception from open sourcing). We will review your submission within **1-2 weeks**. Please include the below files in your submission.

        i. apiDefinition.swagger.json
        ii. apiProperties.json
        iii. Intro.md(You can [find an example of intro.md file on this page](https://docs.microsoft.com/en-us/connectors/custom-connectors/submit-for-certification#submit-to-isv-studio) at step 6)
        iv. Icon(Please [make sure that the icon meets all the requirements,](https://docs.microsoft.com/en-us/connectors/custom-connectors/certification-submission#design-an-icon-for-your-connector) that it’s size is 230x230, icon background is not white or transparent and it matches the iconBrandColor property in apiProperties.json file)

    c. After the pull request is **merged**, you may [submit the files to ISV Studio](https://docs.microsoft.com/en-us/connectors/custom-connectors/submit-for-certification#submit-to-isv-studio), please allow us **1-2 weeks** to review your submission, and our Engineers will notify you in the “Activity Control” area in ISV Studio. Please include the below files in your submission.

        i. apiDefinition.swagger.json
        ii. apiProperties.json
        iii. Intro.md(You can [find an example of intro.md file on this page](https://docs.microsoft.com/en-us/connectors/custom-connectors/submit-for-certification#submit-to-isv-studio) at step 6)
        iv. Icon(Please [make sure that the icon meets all the requirements,](https://docs.microsoft.com/en-us/connectors/custom-connectors/certification-submission#design-an-icon-for-your-connector) that it’s size is 230x230, icon background is not white or transparent and it matches the iconBrandColor property in apiProperties.json file)

5.  [Create an environment in the Preview region](https://docs.microsoft.com/en-us/connectors/custom-connectors/certification-testing#create-an-environment-in-the-preview-region). You will use this environment later to [test your connector](https://docs.microsoft.com/en-us/connectors/custom-connectors/certification-testing#test-your-connector) after Microsoft is done with the functional verification for your connector.

We expect all tests to be completed **within 1-2 weeks**.

6. If your connector passes all tests, the deployment process begins, and it typically takes up to **3-4 weeks** to deploy your connector to all of our regions.

For detailed steps and instructions, go to [Certification process](https://docs.microsoft.com/en-us/connectors/custom-connectors/certification-submission).

## Internal notes

To certify the Power Platform Connector (PCC) it was necessary initially to fill a registration form. The submitted form is available [here](https://forms.office.com/Pages/ResponseDetailPage.aspx?id=v4j5cvGGr0GRqy180BHbR6Pz8V1FjqlLrUJHAW7-rtFUQk5DVEhBWTBDV05JRExVUUdGQTJTTVRaMy4u&rid=89&GetResponseToken=KpykcMIFCsWiIILALTjCtJaGaYWZA9Z_SpNxqH5QADQ&origin=rc) accessible via the Microsoft account filippo.zanella@forcemanager.net.

All instructions to complete the verification process as a verified publisher is well documented [here](https://learn.microsoft.com/en-us/connectors/custom-connectors/certification-submission).

To sum-up briefly, those are the typical steps required to certify a connector that is updated:

-   The `Readme.md` artificat file contained in the connector folder has to be kept update in case of any substantial changes.

-   Run the solution checker to validate the connector as per explained [here](https://learn.microsoft.com/en-us/connectors/custom-connectors/validate-custom-connector). A solution should already exists to be run. Wait for the [report](https://learn.microsoft.com/en-us/power-apps/maker/data-platform/use-powerapps-checker#review-the-solution-checker-report).

-   To submit the connector for deployment, two steps needs to be done as per explained [here](https://learn.microsoft.com/en-us/connectors/custom-connectors/submit-for-certification):

    -   a pull request as to open in the [open-source repository](https://github.com/microsoft/PowerPlatformConnectors) of PA connectors. After 1-2 weeks will be validated by Microsoft.

    -   When the pull request is approved and merged, the connector artifacts have to be submitted in the [ISV Studio](https://isvstudio.powerapps.com/ep/connector), in the "Connector certification" tab.

-   When the connector is validated, tests must be run as per explained in the [testing connector in certification](https://learn.microsoft.com/en-us/connectors/custom-connectors/certification-testing), creating an environment in the Preview region if does not already exist. Test must be completed in a week and must be notified to the Microsoft contact, so that it can start the deployment.

-   After the connector is validated for testing, Microsoft will deploy it across all products and regions. On average it takes 3 to 4 weeks to deploy the connector, regardless of the size or complexity of your connector, whether it's new or an update.

## ISV Studio informations

#### Testing Information

To test the connector you can use these APIs keys of a ForceManager implementation 103617:

-   public key: ccd38ozuij4ypyy3qdk3o8l5zsk6hh
-   private key: b96cbc03146239d04ab42bfabd7b0654

these are needed to create the connection in PA the first time.

Then to test each of the triggers, in the way they can be activated, you need to create/update an account/contact/opportunity inside ForceManager CRM app. You can access our CRM via web at this link: https://app.forcemanager.net/ using the following credentials:

-   email: sigitig182@gyxmz.com
-   password: Y1vpZG@F7x2i

To test instead all the connector actions we can provide you a flow that contains a pre-defined flow with all the ForceManager actions, setup for implementation 103617, the same one of the provided api keys above. We can invite you via email to the flow or sending you the exported zip file.

#### Support Email

filippo.zanella@forcemanager.net

#### Why do you want your connector to be certified?

Seeking certification for our connector aligns with our commitment to providing the highest quality and most secure solutions to our customers. Certification by Microsoft not only adds credibility but also assures users of the connector's reliability and compliance with Microsoft's standards. It enhances visibility in the Microsoft ecosystem, potentially increasing adoption and user trust. Furthermore, certification provides access to Microsoft's support and resources, enabling us to continually improve and evolve our connector in line with the latest technological advancements and customer needs.

Additionally, a step-by-step walkthrough of our connector's functionalities includes:

-   Seamless integration with CRM for data management.
-   Automation of tasks like contact updates and account management.
-   Real-time synchronization for opportunities.
-   User-friendly interface with Microsoft Power Automate, enhancing workflow efficiency.

This holistic approach underscores our dedication to delivering top-tier, user-centric solutions.
