# U.K. Government Check VAT V2

Check a UK VAT registration number and retrieve the registered business's name and address directly from HM Revenue & Customs (HMRC). This connector uses version 2 of HMRC's *Check a UK VAT number* API and authenticates application-to-application using the OAuth 2.0 client credentials grant — **no user sign-in and no redirect URL are required**.

## Publisher

Fördős András (Independent Publisher)

## Prerequisites

An application registered on the [HMRC Developer Hub](https://developer.service.hmrc.gov.uk/) that is subscribed to the **Check a UK VAT number** API. You connect using that application's **Client ID** and **Client secret**.

## Obtaining Credentials

1. **Register** for a developer account on the [HMRC Developer Hub registration page](https://developer.service.hmrc.gov.uk/developer/registration) (skip if you already have one), then sign in.
2. **Create an application:** go to [Your applications](https://developer.service.hmrc.gov.uk/developer/applications) and select **Add an application to the sandbox**.
3. **Subscribe to the API:** open the application, select **API subscriptions**, find **Check a UK VAT number**, and turn the subscription on. This grants the `read:vat` scope. See [Using the Developer Hub](https://developer.service.hmrc.gov.uk/api-documentation/docs/using-the-hub) for guidance.
4. **Get your credentials:** on the application's **Credentials** page, copy the **Client ID** and **Client secret**.
5. **For live data:** apply for production credentials (**Get production credentials**) and complete HMRC's checklist; approval can take a few days. Sandbox credentials work immediately for testing.
6. **Create the connection:** in Power Platform, enter the **Client ID** and **Client secret**. The connector requests and refreshes the access token for you using the client credentials grant (scope `read:vat`).

## Supported Operations

### Get VAT registration
Checks a UK VAT number and returns the registered business's name and address.

### Get VAT registration with reference
Checks a UK VAT number on behalf of your own VAT-registered business and returns a consultation reference number as proof that the check was performed.

## Known Issues and Limitations

- **Authentication is implemented in the connector's custom code rather than the built-in connection wizard.** HMRC's Check VAT API is application-restricted (OAuth 2.0 *client credentials* grant), but Power Platform custom connectors' built-in OAuth 2.0 supports only the *authorization code* flow (interactive user sign-in). To provide the correct application-to-application behaviour, the connector takes your Client ID and Client secret as connection fields and performs the client credentials token exchange itself. This is why the connection asks for a Client ID and Client secret instead of showing a sign-in prompt, and why no redirect URL is required.
- Access to live data requires production credentials, which must be approved by HMRC on the Developer Hub. Until then, use sandbox credentials with HMRC's published test VAT numbers.
- HMRC access tokens are short-lived; the connector requests a fresh token per call, so no token management is required.
