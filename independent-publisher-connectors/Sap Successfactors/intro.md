# SuccessFactors Career Connector

## Introduction

The SuccessFactors Career Connector provides a structured integration layer for creating and maintaining career-related data in SAP SuccessFactors. The connector is designed to simplify the interaction between external systems, automation platforms, and SuccessFactors by exposing clear and reusable actions for common recruiting and career-management scenarios.

In its current version, the connector focuses on the **Candidate** area of SAP SuccessFactors. It supports the creation and modification of candidate records and is intended to help automate candidate intake processes from external sources such as Copilot Studio, Power Automate, custom business applications, HR portals, or other integration services.

The connector is being developed step by step. The first major implementation phase concentrates on the **Career / Recruiting module**, starting with candidate management and gradually extending into related recruiting objects and processes.

## Current Capabilities

The initial version of the connector supports core candidate operations in SuccessFactors.

This includes the ability to create new candidates and update existing candidate information. Candidate data can be enriched with additional metadata to provide a more complete candidate profile inside SuccessFactors.

Examples of supported or planned candidate-related metadata include:

- Personal candidate information
- Contact details
- Education history
- Outside work experience
- Job position references
- Career-related background information
- Additional recruiting metadata required for downstream processes

The goal is to provide a reliable and reusable connector interface that hides the complexity of the underlying SuccessFactors API and allows consuming systems to work with a simplified and business-oriented model.

## Planned Extension of the Career Module

After the candidate-management functionality has been completed and stabilized, the next development step is to extend the connector further into the recruiting process.

The planned roadmap includes support for creating and managing job-related objects, especially job offerings and job requisition-related data. This will allow external systems not only to submit and enrich candidate data, but also to support the creation and maintenance of job opportunities within SuccessFactors.

The intended flow is:

1. Create or update candidate records.
2. Enrich candidates with relevant metadata such as education, work experience, and job-position information.
3. Extend the connector to support job offering creation.
4. Build a broader integration layer for recruiting and career-related SuccessFactors processes.

## Purpose of the Connector

The main purpose of this connector is to provide a clean, reusable, and automation-friendly interface for SAP SuccessFactors Career and Recruiting scenarios.

Instead of requiring every consuming application or automation flow to deal directly with the complexity of the SuccessFactors OData API, the connector offers dedicated actions with clear input and output structures.

This makes it easier to:

- Integrate SuccessFactors with Copilot Studio
- Build HR automation workflows
- Standardize candidate creation processes
- Reduce manual data entry
- Improve data quality through structured metadata enrichment
- Reuse the same integration logic across multiple business scenarios
- Prepare future extensions for job offerings and additional recruiting processes

## Target Scenario

A typical scenario for this connector is an HR or recruiting automation where candidate information is collected through an external interface and then transferred into SuccessFactors.

For example, a Copilot Studio agent could collect candidate information during a conversation, extract structured data such as name, contact details, education, work experience, and job interests, and then use this connector to create or update the candidate record in SuccessFactors.

In a later stage, the same connector family can be extended to support the creation of job offers or job postings, allowing a more complete end-to-end recruiting automation scenario.

## Development Status

This connector is currently in an early implementation phase.

The current focus is:

- Candidate creation
- Candidate modification
- Candidate metadata enrichment
- Preparation for additional career-module functionality

Future development will focus on expanding the recruiting scope, especially around job offerings and related career objects.

## Summary

The SuccessFactors Career Connector is intended to become a dedicated integration layer for SAP SuccessFactors recruiting and career processes. The first implementation step focuses on candidates and candidate metadata enrichment. Future versions will expand the connector to support job offerings and broader recruiting workflows.

This phased approach allows the connector to deliver value early while keeping the architecture flexible for future SuccessFactors Career module extensions.