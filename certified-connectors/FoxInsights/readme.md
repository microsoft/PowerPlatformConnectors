# FoxInsights

Bring FoxInsights tank data into Power Automate, Power Apps and Copilot Studio, and through them into Dynamics 365. Get devices, fill levels and the latest readings, trigger orders or alerts when a tank runs low, and keep partners, customers, products and tank settings in sync with your ERP or CRM.

## Publisher: FoxInsights

## Prerequisites

- A FoxInsights partner account with the Partner API enabled. Contact partner-support@foxinsights.ai to enable it.
- Partner API credentials (Client ID and Client secret), created in the FoxInsights portal under **Settings > Partner API**.

## Supported Operations

### Partners
- **List partners**: the partner that owns the credentials and all sub-partners as a flat list.
- **Get partner tree**: the same partners as a nested tree.
- **Get partner** / **Search partners by internal ID**.
- **Create partner**, **Rename partner**, **Update partner internal ID**, **Set partner notification emails**, **Set partner products**.

### Devices
- **Get partner devices**: devices of one partner with current fill level and latest measurement. Devices of sub-partners are not included.
- **Get device**, **Get user devices**, **Search devices by internal ID**, **Search devices by company**, **Get device cancellation state**.
- **Update device name and notes**, **Update device address**, **Set device labels**, **Update device notifications**, **Lock or unlock device**, **Move device to partner**, **Change device product**, **Cancel device contract**.
- **Set custom tank**, **Set catalogue tank**, **Set silo**, **Delete tank configuration**.

### End customers
- **Get user**, **Get user by email**, **Search users by internal ID**, **Search users by custom field**.
- **Create user**, **Update user**, **Assign device to user**, **Unassign device from user**.

### Products
- **Search products by code**, **Get product types**, **Get predefined tank types**.
- **Create product**, **Update product**.

## Obtaining Credentials

1. Sign in to the FoxInsights portal as a partner administrator.
2. Open **Settings > Partner API** and select **New credentials**.
3. Copy the **Client ID** and **Client secret**. The secret is shown only once.

The credentials give access to the partner that created them and all of its sub-partners. Create separate credentials for each integration so they can be revoked independently.

## Getting Started

1. Create a connection with the Client ID and Client secret. The connector requests access tokens automatically; no token has to be entered or renewed.
2. Add **List partners** to a flow and loop over the result with **Apply to each**.
3. Inside the loop, add **Get partner devices** with the partner ID from the loop.
4. Map the device fields to the target system, for example a Dataverse table or a Business Central record, keyed by **Hardware ID**.

## Known Issues and Limitations

- Device lists are returned per partner without paging. To cover a partner tree, call **Get partner devices** for each partner from **List partners**.
- Only the latest measurement is available; there is no measurement history.
- **Measured at** is the time of the latest reliable measurement, not the time of the last sync. A device without a measurement has no **Measured at**; its fill level can be reported as 0, so check **Measured at** before using the fill level.
- Fill levels can exceed 100 percent.
- **Set device labels**, **Set partner notification emails** and **Set partner products** replace the whole list.
- **Unassign device from user** deletes the device address and tank settings unless **Keep device settings** is set.
- **Create partner**, **Create user** and **Create product** are not safe to retry: a repeated call creates a duplicate or fails.
- **Cancel device contract** cannot be undone through the connector.
- Rate limits of the Partner API apply per credentials; a 429 response means the limit was reached.

## Frequently asked questions

### Does the connector store FoxInsights data?
No. Each action reads or writes FoxInsights data at the time it runs.

### Can one connection see several partners?
Yes. A connection sees the partner that created the credentials and all of its sub-partners.

### Where do I get help?
Contact partner-support@foxinsights.ai.

## Deployment Instructions

Deploy the connector as a custom connector with the Power Platform CLI:

```bash
pac connector create --api-definition-file apiDefinition.swagger.json --api-properties-file apiProperties.json --script-file script.csx
```
