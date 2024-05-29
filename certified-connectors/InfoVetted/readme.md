# Title

InfoVetted provides a connector to request pre-employment vetting services such as Identity Verification (IDV), Right to Work (RTW), and Disclosure & Barring Service (DBS).

The connector also supports querying the status of a requested check and downloading the vetting results as a PDF.

## Publisher: Publisher's Name

**InfoVetted**

## Prerequisites

You must have registered your business and subscribed to the particular vetting product at [InfoVetted](https://www.infovetted.com) to use the connector.

## Supported Operations

### Process Check

Request a vetting check. The following check types are available:

- RightToWork = 0
- StandardDBS = 1
- EnhancedDBS = 2
- BasicDBS = 3

Vetting status updates will be sent to the (optional) webhook address provided in the following format

**Webhook POST payload JSON:**

```
{
    "CheckId": "",
    "Status": 0,
    "StatusDescription": ""
}
```

**Statuses:**

- NotStarted = 0,
- InProgress = 1,
- Complete = 2,
- Deleted = 3,
- PendingApproval = 4,
- Rejected = 5,
- Expired = 6,
- Archived = 7,
- Unknown = 8

### Vetting Status

Returns the vetting status of the requested check

**Statuses:**

- NotStarted = 0,
- InProgress = 1,
- Complete = 2,
- Deleted = 3,
- PendingApproval = 4,
- Rejected = 5,
- Expired = 6,
- Archived = 7,
- Unknown = 8

### PDF Export

Returns a PDF result for the completed vetting

## Obtaining Credentials

Your API Key can be obtained within the InfoVetted portal under **Settings** => **API Keys** => **Primary Key**.

[InfoVetted API Keys](https://portal.infovetted.com/apikeys)

You must have an active subscription for the relevant product or have invoicing enabled

## Known Issues and Limitations

No known issues
