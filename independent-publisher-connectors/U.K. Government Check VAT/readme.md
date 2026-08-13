# U.K. Government Check VAT
GOV.UK's Check VAT API provides validation of registered VAT numbers and information about used name and address of the given business. With the growing need for verified and quality customer data, businesses and consumers can count on GOV.UK Check VAT API.

## Publisher: Fördős András

## Deprecation

**This connector is deprecated.** Its endpoint (`api.service.hmrc.gov.uk`) no longer works for this connector, so its operations are non-functional and can no longer be relied on.

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
This connector is deprecated and its operations are non-functional, because the underlying endpoint no longer works for this connector (see [Deprecation](#deprecation) above).

## Recommended Alternative
For maintained VAT number validation, see the **Abstract VAT Validator** independent publisher connector, which validates VAT numbers and returns company details (registered country, name, and address). Note that it uses a different provider and requires an AbstractAPI account/API key, so evaluate it against your needs before migrating.

## Example

![Example with Microsoft UK](example.png "Example")