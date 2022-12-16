# Unearth Ingest

Unearth AI ingests, analyses, and stores virtually any information. It provides analysis and discovery and robotic process automation. All of this is powered by layers of AI, including the training and hosting of custom AI that can operate on any information. Unearth AI empowers staff with unparalleled access to information, and the ability to use it to that to make decisions that has never been possible before.

Use Unearth AI Ingest to push information into selected Corpora in the Knowledgebase through Ingest.

## Publisher: Wild Mouse Australia

Wild Mouse builds enterprise SaaS products, including Unearth AI, Xmouse Live white labelled streaming app platform and SDX secure workflow powered file transfer. Visit us at [www.wildmouse.com](https://www.wildmouse.com)

## Prerequisites

You will need the following to proceed:

- A Microsoft Power Apps or Power Automate plan with custom connector feature
- An Unearth AI Subscription
- Permission to Ingest information into the selected Corpus in Unearth AI

## Supported Operations

The connector supports the following operations:

### Ingest

Ingest information into the provided Corpus in Unearth AI.

### Heartbeat

Tests connection to Unearth AI.

### Authenticated

Tests authentication of the current user.

### ProcessingStatus

Retrieves the processing status of the provided operation.

## Obtaining Credentials

### When creating connection

- Ingest API URL: This will be provided when Unearth is configured (e.g. `unearthabc-processwebapi.azurewebsites.net`) make sure it doesn't have any leading slashes or https:// at the start
- Authentication scope: This will be provided when Unearth is configured (e.g. `api://6073429f-e073-4838-89da-93e7a00c3703/processing`)

### When deploying

This will be provided by Wild Mouse support

- clientId
- tenantId
- client secret

## Deployment Instructions

Please use [these instructions](https://docs.microsoft.com/en-us/connectors/custom-connectors/paconn-cli) to deploy this connector as a custom connector within Microsoft Power Automate and Power Apps.
