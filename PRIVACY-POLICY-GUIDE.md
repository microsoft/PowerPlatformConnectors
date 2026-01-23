# Privacy Policy Requirements for Independent Publisher Connectors

## Which Privacy Policy to Use?

For Independent Publisher connectors, you should use the **service's privacy policy**, not your own.

### ✅ Correct Approach

Use the privacy policy of the service the connector integrates with:

| Connector | Privacy Policy URL |
|-----------|-------------------|
| Apollo | https://www.apollo.io/privacy |
| Kit | https://kit.com/privacy |
| QuickBooks | https://www.intuit.com/privacy/ |
| MailChimp | https://www.intuit.com/privacy/statement/ |
| WhatsApp Business | https://www.whatsapp.com/legal/privacy-policy-eea |

### Why Use the Service's Privacy Policy?

1. **Data Handling**: The service (Apollo, Kit, etc.) handles and stores the user data, not you
2. **User Agreement**: Users need to understand how the service processes their data
3. **Compliance**: The service's privacy policy covers GDPR, CCPA, and other regulations
4. **Liability**: The service is responsible for data privacy, not the connector publisher

## In Your Connector Files

### apiDefinition.swagger.json

```json
{
  "x-ms-connector-metadata": [
    {
      "propertyName": "Website",
      "propertyValue": "https://forceworks.com"  // Your website
    },
    {
      "propertyName": "Privacy policy",
      "propertyValue": "https://www.apollo.io/privacy"  // Service's privacy policy
    },
    {
      "propertyName": "Categories",
      "propertyValue": "Sales and CRM;Data"
    }
  ]
}
```

## When Would You Use Your Own Privacy Policy?

You would only use your own privacy policy if:

1. **You're the service owner** (Certified Connector, not Independent Publisher)
2. **You collect additional data** beyond what the service collects
3. **You process data** before sending it to the service

For Independent Publishers, you're just providing a connector interface - you don't collect, store, or process user data yourself.

## Finding Service Privacy Policies

### Common Patterns

Most services have their privacy policy at:
- `https://[service].com/privacy`
- `https://[service].com/legal/privacy`
- `https://[service].com/privacy-policy`
- `https://www.intuit.com/privacy/` (for Intuit products like QuickBooks, MailChimp)

### How to Find It

1. Go to the service's website
2. Scroll to the footer
3. Look for "Privacy", "Privacy Policy", or "Legal"
4. Copy the full URL

### Verify the URL

Make sure the privacy policy URL:
- ✅ Is publicly accessible (no login required)
- ✅ Is HTTPS
- ✅ Is the current/active policy
- ✅ Covers the service's data handling practices

## PR Submission

When submitting your PR, you'll provide:

| Field | Value | Example |
|-------|-------|---------|
| **Publisher Name** | Your name | Steve Mordue |
| **Publisher Email** | Your email | steve@forceworks.com |
| **Support Email** | Your email | steve@forceworks.com |
| **Website URL** | Your website | https://forceworks.com |
| **Privacy Policy URL** | Service's privacy policy | https://www.apollo.io/privacy |

## Summary

**For Independent Publishers:**
- Website URL = Your website (https://forceworks.com)
- Privacy Policy URL = Service's privacy policy (https://www.apollo.io/privacy)
- Contact Info = Your info (Steve Mordue, steve@forceworks.com)

**You are NOT:**
- Collecting user data
- Storing user data
- Processing user data
- Responsible for the service's privacy practices

**You ARE:**
- Providing an interface to the service
- Directing users to the service's privacy policy
- Supporting users with connector-related questions

---

## Current Connector Privacy Policies

| Connector | Privacy Policy URL | Status |
|-----------|-------------------|--------|
| Apollo | https://www.apollo.io/privacy | ✅ Correct |
| Kit | https://kit.com/privacy | ✅ Correct |
| QuickBooks | https://www.intuit.com/privacy/ | ⚠️ Verify |
| MailChimp | https://www.intuit.com/privacy/statement/ | ⚠️ Verify |
| WhatsApp Business | https://www.whatsapp.com/legal/privacy-policy-eea | ⚠️ Verify |
| ExchangeRate | TBD | ❌ Need to add |
| PartnerCenter | https://privacy.microsoft.com/privacystatement | ⚠️ Verify |

