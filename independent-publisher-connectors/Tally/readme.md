# Tally (Independent Publisher)

Tally is an online form builder for collecting information and responses. This independent-publisher connector lets users list and inspect forms, read submissions, and create or update forms in Tally.

## Publisher

Unique Software Development  
Website: https://uniquesoftwaredev.com/  
Support: shahbaz.raza@uniquesoftwaredev.com

This connector is community-developed and is not an official Tally product.

## Prerequisites

- A Tally account with access to the forms you wish to manage.
- A Tally API key generated from the Tally account settings.
- A supported environment and applicable licensing to use custom/premium connectors.

## Supported operations

- **List Forms** (`GET /forms`) — lists available forms.
- **Get Form Details** (`GET /forms/{formId}`) — gets form metadata and blocks.
- **List Form Submissions** (`GET /forms/{formId}/submissions`) — reads questions, submissions and answers.
- **Create Form** (`POST /forms`) — creates a form from a supplied block structure.
- **Update Form** (`PATCH /forms/{formId}`) — updates an existing form.

## Obtaining credentials

In Tally, open Settings > API Keys and create an API key. Because this connector uses an Authorization-header API-key configuration, enter `Bearer ` followed by your Tally API key in the connection field (for example, `Bearer <your-key>`). The prefix and separating space are required for the current definition. Never include your actual key in screenshots or GitHub files. Never commit API keys, signed URLs, or respondent data to the repository. Store and rotate credentials securely.

## Getting started

1. Create a connection using the format `Bearer <your-key>` in the Tally Bearer Token field.
2. Run **List Forms** and copy a returned form ID.
3. Run **Get Form Details** with the ID.
4. Run **List Form Submissions** with the same ID for a form with responses.
5. Test **Create Form** and **Update Form** only on disposable draft forms.

## Known issues and limitations

- Initial tests covered one published form and one newly created draft; a broader set of field types and data sizes needs verification.
- The imported response shapes were inferred from example responses. Fields may be absent/null, and some response answer types vary by question type.
- Pagination inputs and automated paging are not yet added to these operations.
- **Create Form** uses Tally's block format, which is more complex than a simple title-only request. Users must supply valid blocks and unique identifiers.
- **Update Form** was initially tested as a DRAFT-to-DRAFT request only. Changing blocks has not been verified end-to-end. Preserve any existing blocks that you want to keep.
- No trigger for new submissions is provided in this first version.
- Three different operations were successfully tested using solution cloud flows (List Forms, Get Form Details and List Form Submissions). Cross-user testing, repeated operation tests, Create/Update flow demonstrations, package validation and final certification are still pending.

## References

- Tally: https://tally.so
- Tally API documentation: https://developers.tally.so/api-reference/introduction
- Tally privacy policy: https://tally.so/help/privacy-policy
