# ZeroTrain AI Core Connector

## Overview

ZeroTrain AI Core is a deterministic decision engine designed for business automation.
Unlike probabilistic AI systems that generate predictions, ZeroTrain computes decisions
using explicit, rule-based logic and returns traceable outcomes suitable for compliance,
audit, and operational workflows.

This connector allows Power Automate users to submit business data and decision rules
and receive a computed decision, confidence score, and execution trace in a single step.

---

## What Problem Does This Connector Solve?

Many automation workflows require decisions that must be:
- Deterministic (same input produces the same result)
- Explainable and traceable
- Suitable for regulated or audited environments
- Independent of probabilistic or black-box AI models

ZeroTrain AI Core addresses these needs by computing decisions using explicit logic
instead of predictions, making outcomes reliable, reproducible, and transparent.

---

## Key Capabilities

- Computes deterministic decisions from business data
- Returns a concrete action (for example: Approve, Review, Buy)
- Provides a confidence score based on rule satisfaction
- Emits an execution trace showing which logic conditions passed or failed
- Supports decision rules sourced from databases, CSV files, Excel, or Dataverse
- Designed for automation, compliance, and operational decision workflows

---

## Typical Use Cases

- Policy and eligibility decisions
- Order approval and routing
- Risk and compliance checks
- Operational decision automation
- Rule-based business logic execution
- Explainable decision workflows

---

## Authentication

This connector uses an API key for authentication, passed via the
`x-engine-access-key` HTTP header.

---

## Publisher Contact

If you have questions about this connector or are interested in collaboration,
please contact:

**Leonard Gambrell**  
Email: support@zerotrain.ai
