# sms77io

A Germany based communication provider established in 2003.

## Send SMS

Send a message to multiple numbers and/or contacts at once.

- Specify a custom sender name with up to 11 alphanumeric or 16 numeric characters
- Enable performance tracking for SMS marketing purposes
- Send flash SMS displayed directly on the receiver’s display
- Set a custom time to live
- Return a detailed response as text or JSON
- Set a custom label for creating relationships
- Set a custom foreign ID which gets returned in callbacks
- Force unicode encoding overriding the API detection
- Force UTF-8 encoding overriding the API detectio
- Specify a custom user data header

## Text-To-Speech

Call any number of choice and read a given text out loud.

- Accepts XML and plain text with a maximum of 10000 characters
- Specify a custom caller ID: choose among our shared numbers or register own
  inbound number
- Contact us for individual expansion with interactive menus, MP3s and much more
- Set up a webhook to receive realtime status updates of your calls

## Phone Number Lookups

### Caller ID (CNAM)

Looks up the caller ID name:

```json
{
    "code": "100",
    "name": "GERMANY",
    "number": "+4915126716517",
    "success": "true"
}
```

### Home Location Register (HLR)

Returns detailed information from the cellular carrier:

```json
{
    "country_code": "DE",
    "country_code_iso3": "DEU",
    "country_name": "Germany",
    "country_prefix": "49",
    "current_carrier": {
        "network_code": "26203",
        "name": "E-Plus Mobilfunk",
        "country": "DE",
        "network_type": "mobile"
    },
    "international_format_number": "491632429751",
    "international_formatted": "+49 163 2429751",
    "lookup_outcome": 0,
    "lookup_outcome_message": "Success",
    "national_format_number": "0163 2429751",
    "original_carrier": {
        "country": "DE",
        "name": "E-Plus Mobilfunk",
        "network_code": "26203",
        "network_type": "mobile"
    },
    "ported": "assumed_not_ported",
    "reachable": "reachable",
    "roaming": "not_roaming",
    "status_message": "Success",
    "valid_number": "valid"
}
```

### Mobile Number Portability (MNP)

Checks the mobile number portability status:

```json
{
    "code": 100,
    "mnp": {
        "country": "DE",
        "international_formatted": "+49 163 2429751",
        "isPorted": false,
        "mccmnc": "26201",
        "national_format": "01512 6716517",
        "network": "Telekom Deutschland GmbH (Telekom)",
        "number": "+4915126716517"
    },
    "success": true
}
```

### Formats

Retrieves number formats in different versions:

```json
{
    "carrier": "Eplus",
    "country_code": "49",
    "country_iso": "DE",
    "country_name": "Germany",
    "international": "+491632429751",
    "international_formatted": "+49 163 2429751",
    "national": "0163 2429751",
    "network_type": "mobile",
    "success": true
}
```

## Analytics

Retrieve detailed statistics of your account.

- Set a custom start and end date
- Limit to a certain label
- Limit to main account or certain subaccount
- Group by date, label, subaccount or country

## Delivery Reports

Retrieves the transport status for SMS with the given ID. Possible values:

**DELIVERED**  SMS was successfully delivered.

**NOTDELIVERED** SMS could not be delivered. Please check the recipient number if
necessary.

**BUFFERED** SMS was sent successfully, but was cached by the SMSC because the recipient
isn’t reachable.

**TRANSMITTED** SMS was sent by the SMSC and should arrive soon.

**ACCEPTED** SMS was accepted by the SMSC.

**EXPIRED**    SMS was not received before the end of validity.

**REJECTED** SMS was rejected by the carrier.

**FAILED** Error while sending SMS.

**UNKNOWN** Unknown status report.

## Pricing Information

Retrieve pricing information as a comma separated values (CSV) or in machine-readable JSON
format. You can also specify an ISO country code to narrow down the search to a country of
choice.

## Read/Delete Contacts

Create, delete, edit and read your contacts for easier bulk messaging. Specifying a
contact instead of a phone number. Phone numbers can can change anytime, names rarely do
so.

For a detailed documentation check out
our [SMS HTTP API Documentation](https://www.sms77.io/en/docs/gateway/http-api/sms-disptach/)
.

## Pre-requisites

- A Microsoft Power Apps or Power Automate plan with custom connector feature
- [Sms77.io API Key](https://app.sms77.io/settings#httpapi)
