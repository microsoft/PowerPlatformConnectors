# MYOB Business - OPC

## Overview

This proposal is for an independent Power Platform connector named **MYOB Business - OPC**.

The connector integrates with the **MYOB Business API** and enables Power Automate, Power Apps, Logic Apps, and Copilot Studio to interact with MYOB Business cloud data through a secure Azure hosted API layer.

The connector is published by **OfficePro Consulting (OPC)** and is not an official MYOB connector.

---

## What the connector does

The connector allows users to automate workflows involving MYOB Business by providing actions that:

- Authenticate with MYOB using OAuth 2.0
- Access MYOB Business cloud organisations authorised by the user
- Read and write MYOB Business data such as contacts, accounts, and invoices
- Use MYOB data inside Power Platform solutions without building custom integrations

All authentication, request handling, and enforcement logic is managed centrally by the connector backend.

---

## Authentication

Authentication is handled using **OAuth 2.0** with the MYOB Business API.

The connector uses a secure Azure hosted API layer to manage:

- OAuth authorisation and token refresh
- Secure storage of access tokens
- Controlled access to MYOB Business organisations
- Server side enforcement of connector usage rules

Documentation will be provided describing how users authenticate and grant consent to access their MYOB Business account.

---

## Intended audience

The connector is intended for:

- Businesses using MYOB Business cloud products
- Consultants and integrators building accounting automations
- Teams using Power Automate to connect MYOB with Microsoft 365 systems

The connector is designed to support both simple workflows and more advanced automation scenarios.

---

## Commercial model

MYOB Business - OPC is offered as a **commercial SaaS connector**.

Access is licensed per customer with plan based limits such as:

- Number of MYOB Business organisations
- Daily request limits per organisation

Licensing and enforcement are handled server side. End users do not need to configure licensing within their flows.

---

## Why this connector is needed

While MYOB provides APIs and SDKs, there is currently no certified Power Platform connector that:

- Targets the MYOB Business API
- Uses OAuth authentication suitable for Power Platform
- Centralises authentication and usage enforcement
- Is designed specifically for Power Automate usage patterns

This connector aims to address that gap.

---

## Support and contact

Documentation and support will be provided via the publisher website.

Support contact email:  
**contact@officeproconsulting.com.au**

---

## Publisher verification

The publisher is willing to complete the Microsoft verified credentials process as part of the certification requirements.
