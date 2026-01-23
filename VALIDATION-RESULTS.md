# Connector Validation Results

**Validation Date:** January 23, 2026  
**Tool:** paconn validate  
**Validator:** Steve Mordue

---

## ✅ PASSED Validation

All connectors validated successfully with `paconn validate`:

| Connector | Status | Issues Fixed |
|-----------|--------|--------------|
| **Kit** | ✅ PASS | None |
| **Apollo** | ✅ PASS | None |
| **ExchangeRate** | ✅ PASS | Title (removed "API"), Default response schemas removed |
| **MailChimp** | ✅ PASS | None |

---

## ExchangeRate Fixes Applied

### Issue 1: Title Contains Forbidden Word "API"
**Error:** `info/title : The value 'title' contains at least one of the restricted words: 'api, connector'`

**Fixed:**
- Before: `ExchangeRate-API (Independent Publisher)`
- After: `ExchangeRate (Independent Publisher)`

### Issue 2: Default Responses with Schemas
**Error:** `The 'default' response should not have schema definition. Schemas should be defined on expected responses only.`

**Fixed:** Removed schema from all default responses (8 operations):
- GetLatestRates
- GetPairConversion
- ConvertAmount
- GetEnrichedData
- GetHistoricalRates
- ConvertHistoricalAmount
- GetSupportedCodes
- GetQuotaStatus

**Before:**
```json
"default": {
  "description": "Error response",
  "schema": {
    "$ref": "#/definitions/ErrorResponse"
  }
}
```

**After:**
```json
"default": {
  "description": "Error response"
}
```

---

## Validation Command Used

```powershell
cd "f:/projects/Connectors/independent-publisher-connectors/[ConnectorName]"
python "f:/projects/Connectors/PowerPlatformConnectors/tools/paconn-cli/paconn" validate --api-def "./apiDefinition.swagger.json"
```

---

## Next Steps

### For All Validated Connectors

1. ✅ paconn validation passed
2. ⚠️ Test operations in Power Automate
3. ⚠️ Create test flows
4. ⚠️ Export solutions
5. ⚠️ Create packages
6. ⚠️ Validate packages with ConnectorPackageValidator.ps1
7. ⚠️ Upload to Azure, generate SAS URLs
8. ⚠️ Submit PRs

---

## Remaining Connectors

Not yet validated:
- QuickBooks
- PartnerCenter
- WhatsAppBiz

---

## Common Issues Found

### Forbidden Words in Title
- ❌ "API" - Use "Service" or remove
- ❌ "Connector" - Already implied
- ❌ "Power Apps", "Power Automate", etc.

### Default Response Schemas
- ❌ Don't define schemas on default responses
- ✅ Only define schemas on specific status codes (200, 201, 204, etc.)
- ✅ Default can have description only

### Best Practices
- Use specific error codes (400, 401, 404, 500) with schemas if needed
- Keep default response simple (description only)
- Avoid generic "API" in connector names

---

**Validation Status:** 4/7 connectors validated and passing ✅
