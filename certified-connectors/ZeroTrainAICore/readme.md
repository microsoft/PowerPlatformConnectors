# ZeroTrain AI Core Connector

## Title

ZeroTrain AI Core is a deterministic AI decision engine that evaluates structured input data and returns a recommended action with a confidence score and execution details. Unlike traditional machine learning systems, ZeroTrain does not require model training or historical datasets. This connector allows Power Platform workflows to send data to ZeroTrain and receive reliable, repeatable decision results instantly.

---

## Publisher

ZeroTrain AI

---

## Prerequisites

To use this connector you will need:

- A ZeroTrain AI Core account  
- A valid API key generated from the ZeroTrain AI portal  
- Access to the ZeroTrain AI Core API endpoint  

Users must have permission to call the ZeroTrain API service and submit evaluation requests.

---

## Supported Operations

This connector allows Power Platform workflows to send structured input data to ZeroTrain AI Core and receive a deterministic decision result.

Supported capabilities include:

- Evaluating decision logic against input data
- Returning a recommended action
- Returning a confidence score
- Returning execution details suitable for logging or auditing

---

### Operation 1: Evaluate Decision

Sends structured input data to the ZeroTrain AI Core API and evaluates the data against the configured decision logic. The response includes the selected decision, confidence score, and additional execution metadata.

---

### Operation 2: Batch Evaluation

Allows multiple rows of structured input data to be evaluated in a single request. Each row is evaluated independently and returns a decision result with associated confidence scores.

---

## Obtaining Credentials

ZeroTrain AI Core uses API key authentication.

To obtain credentials:

1. Sign in to the ZeroTrain AI portal.
2. Generate an API key for your organization.
3. Provide the API key when configuring the connector inside Power Platform.

The API key is included in the request header when calling the ZeroTrain AI Core service.

---

## Getting Started

1. Add the ZeroTrain AI Core connector to your Power Automate, Logic Apps, or Power Apps workflow.
2. Configure the connector using your ZeroTrain API key.
3. Provide the input data required for evaluation.
4. Execute the workflow to receive the decision result and confidence score.

This allows workflows to automate decision routing, approvals, risk evaluation, and operational recommendations using ZeroTrain.

---

## Known Issues and Limitations

- The connector requires an active internet connection to reach the ZeroTrain AI Core API service.
- API rate limits may apply depending on the selected service plan.
- Input data must match the schema expected by the ZeroTrain decision configuration.

---

## Frequently Asked Questions

### What is ZeroTrain AI Core?

ZeroTrain AI Core is a deterministic AI decision engine designed to evaluate structured data using explicit decision logic rather than probabilistic machine learning models.

### Do I need to train a model before using ZeroTrain?

No. ZeroTrain operates without model training and evaluates decisions immediately using defined logic and rules.

---

## Deployment Instructions

To deploy this connector as a custom connector:

1. Download the connector definition files.
2. In Power Platform, navigate to **Custom Connectors**.
3. Select **Import OpenAPI file**.
4. Upload the `apiDefinition.swagger.json` file.
5. Configure authentication using your ZeroTrain API key.
6. Save and test the connector.

Once deployed, the connector can be used in Power Automate, Logic Apps, and Power Apps workflows.
