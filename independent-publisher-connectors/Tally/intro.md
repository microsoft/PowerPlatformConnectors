# Tally Connector - Independent Publisher Proposal

## Overview

Tally is an online form-building platform that enables users to create forms, collect responses, and manage submissions.

This proposed connector will integrate Tally with Microsoft Power Automate, Power Apps, and supported Power Platform services, enabling users to automate form management and submission-related workflows.

## Proposed Publisher

- **Organization:** Unique Software Development
- **Website:** https://uniquesoftwaredev.com/
- **Contact Email:** shahbaz.raza@uniquesoftwaredev.com
- **GitHub Maintainer:** @usdshahbazraza
- **Connector Name:** Tally (Independent Publisher)
- **API Provider:** Tally (tally.so)

## Connector Capabilities

The initial connector provides five actions:

1. **List Forms:** Retrieve available forms from a Tally account.
2. **Get Form Details:** Retrieve a specific form's details and configuration.
3. **List Form Submissions:** Retrieve submissions and responses for a selected form.
4. **Create Form:** Create a new Tally form.
5. **Update Form:** Update an existing form's configuration.

## Authentication

The connector uses Tally API-key authentication. Users must supply their own valid Tally API key when creating a connection.

## Use Cases

- Retrieve Tally form submissions through automated workflows.
- Process form responses using Microsoft Power Automate.
- Integrate Tally data with Microsoft services.
- Create and update Tally forms through supported Power Platform applications.

## Development Status

The five proposed API operations have undergone successful initial testing in Microsoft Power Automate.

Additional validation, documentation, and certification testing will be completed before final submission.

## References

- Tally: https://tally.so
- Tally API Documentation: https://developers.tally.so/api-reference/introduction
- Unique Software Development: https://uniquesoftwaredev.com/

This is an independent community-developed integration proposal. Unique Software Development does not own the Tally API.
