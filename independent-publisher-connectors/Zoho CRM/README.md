# Zoho CRM for Power BI Basic

The Zoho CRM is a Power BI custom data connector that enables seamless integration between Zoho CRM and Microsoft Power BI. It provides authenticated, paginated access to all Zoho CRM modules with proper data typing, lazy-loaded navigation, and support for scheduled refresh via the On-premises Data Gateway.

## Publisher: iFour Technolab Pvt Ltd.

## Prerequisites

You need to have a Zoho CRM Login Credentials.

## Obtaining Secrets

Once you have created an organization, create a Self Client from the [Zoho API Console](https://api-console.zoho.com/). This will create a Client ID and Client Password to be used with the connector.

---

## Setup & Installation

### Step 1 — Get Zoho API Credentials

1. Navigate to [https://api-console.zoho.in](https://api-console.zoho.in)
2. Create a new **Server-based Application**
3. Set the redirect URI to:
   ```
   https://oauth.powerbi.com/views/oauthredirect.html
   ```
4. Copy your **Client ID** and **Client Secret**
5. Required scopes:
   ```
   ZohoCRM.modules.ALL, ZohoCRM.settings.ALL
   ```

### Step 2 — Configure Credentials

Create the credential files in your project root and add the secretes here.

`client_id`

```
1000.XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
```

`client_secret`

```
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
```

---

## Supported Operations

### Get Modules

- Get all the modules list.

### Get Fields

- Get all the fields of the module.

### Get Module Data

- Get all the module data.

## API Reference

| Endpoint                                         | Used For                            |
| ------------------------------------------------ | ----------------------------------- |
| `GET crm/v8/settings/modules`                    | Fetch list of all CRM modules       |
| `GET crm/v8/settings/fields?module={name}`       | Fetch field metadata for a module   |
| `GET crm/v8/{ModuleName}?page=N&per_page=200`    | Fetch paginated records             |
| `GET crm/v8/Leads?converted=true`                | Fetch converted leads               |
| `POST accounts.zoho.in/oauth/v8/token`           | Exchange auth code for access token |
| `POST accounts.zoho.in/oauth/v8/token` (refresh) | Refresh expired access token        |

---

## License & Contact

| Property          | Details                                         |
| ----------------- | ----------------------------------------------- |
| Developer         | iFour Technolab                                 |
| Website           | https://www.ifourtechnolab.com                  |
| Zoho CRM API Docs | https://www.zoho.com/crm/developer/docs/api/v8/ |
| Power Query SDK   | https://docs.microsoft.com/power-query/         |
| License           | Proprietary — All rights reserved               |

---

## Known Issues and Limitations

There are no known issues at this time.
