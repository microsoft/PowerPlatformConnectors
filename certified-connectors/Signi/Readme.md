# Signi.com

Signi.com connector allows you to sign your documents digitally with the [Signi.com](https://signi.com/) service. Streamline your paperwork!

## Publisher: NETWORG
This connector is managed and published by NETWORG.

## Prerequisites
You will need the following to proceed:

* A Microsoft Power Apps or Power Automate plan
* Signi API key

### How to get started

1. Generate an Signi API key for your workspace by following [this guide](https://signi.refined.site/space/PKNB/383516725), the generation is done at [this page](https://app.signi.com/api)
2. Fill out the following [form](https://go.tntg.cz/ismlouva-signup) so that we can set everything up for you (the form is in Czech, here is a quick sum-up):
   1. Login name (e-mail)
      * Enter your login name (email) that you use to log in to your company account. We will contact you at this email in case of problems.
   2. Workspace name
      * Enter the name of the workspace as you want it to appear to users in Microsoft Power Automate
   3. Signi API key
      * Enter the API key from the Signi website (https://app.signi.com/api)
   4. Do you want to limit the use of an API key to a specific group of users?
      * If you want only specific departments in your company to be able to use the workspace, select yes

**You can also find the full guide in Czech on [this page](https://docs.talxis.com/cz/customizer-guide/integration/signi).**


## Supported Operations

### Get Workspaces (V2)
List all available workspaces for the user.
### Get Contract Template Schema (V2)
Returns schema definition for specified template for dynamic properties to work.
### Wait for Contract Signature (V2)
Register webhook and wait for response.
### Get Contract PDF (DEPRECATED)
Download the signed contract as PDF file.
### Get Contract PDF (V2)
Download the signed contract as PDF file.
### Sign Contract From Template (V1)
Create a new contract from template and request a signature. This action waits until the operation has been completed.
### Sign Contract From Template (DEPRECATED)
Create a new contract from template and request a signature. This action waits until the operation has been completed.
### Sign Contract From Provided File (V2)
Create a new contract from custom file.
### Sign Contract From Provided File (DEPRECATED)
Create a new contract from custom file and request a signature. This action waits until the operation has been completed.
### Sign Contract From Provided File (V1)
Create a new contract from custom file and request a signature. This action waits until the operation has been completed.
### Get Contract Detail (V2)
Returns details about the specific contract identified by the ID.
### Get Contract Detail (DEPRECATED)
Returns details about the specific contract identified by the ID.
### Get Templates (V2)
Lists all available templates.
### Get Revision List PDF (V2)
Download the revision list for the specified contract.
### Get Contract Template Schema
Returns schema definition for specified template for dynamic properties to work.
### Get Templates
Lists all available templates.
### Sign Contract From Provided File and Wait (V2)
Create a new contract from custom file. This action waits until the operation has been completed.

## Known Issues and Limitations

No limitations.

## Deployment Instructions

This connector is fully managed and part of the certified connector's catalogue. If you wish to use it, find it in the list of the available connectors.
