# U.K. Government Check VAT
GOV.UK's Check VAT API provides validation of registered VAT numbers and information about used name and address of the given business. With the growing need for verified and quality customer data, businesses and consumers can count on GOV.UK Check VAT API.

## Publisher: Fördős András

## Deprecation

**This connector is deprecated and its operations are non-functional.** This connector uses HMRC's unauthenticated version 1 of the "Check a UK VAT number" API, which HMRC retired on 17 February 2025. The current version 2 of the API moved behind authentication and now requires OAuth 2.0 (scope `read:vat`). Because changing a published connector's authentication type is not a supported in-place update, this connector cannot be fixed by adding authentication; a separate, new OAuth 2.0-based connector is required instead.

If you are using it in your flows or apps, please migrate away from it. See the [Recommended Alternative](#recommended-alternative) below, or reach out if you need help migrating.

## Prerequisites
There are no prerequisites needed for this connector.

## Obtaining Credentials
This connector does not use authentication, so no credentials are needed.

## Supported Operations
### Get VAT registration (deprecated)
Allows the retrieval of name and address of a VAT regstered company.
### Get VAT registration with reference (deprecated)
Allows the retrieval of name and address of a VAT registered company while providing proof that you have performed the check.

## Known Issues and Limitations
This connector is deprecated and its operations are non-functional, because the underlying HMRC "Check a UK VAT number" version 1 API it relies on was retired on 17 February 2025 (see [Deprecation](#deprecation) above).

## Recommended Alternative
To continue checking UK VAT numbers directly against HMRC, use HMRC's [Check a UK VAT number API version 2](https://developer.service.hmrc.gov.uk/api-documentation/docs/api/service/vat-registered-companies-api/2.0), which requires OAuth 2.0 authentication. A separate OAuth 2.0-based connector for this API is planned; until it is available, you can also use the **Abstract VAT Validator** independent publisher connector, which validates VAT numbers and returns company details (registered country, name, and address). Note that Abstract uses a different provider and requires an AbstractAPI account/API key, so evaluate it against your needs before migrating.

## Example

![Example with Microsoft UK](example.png "Example")