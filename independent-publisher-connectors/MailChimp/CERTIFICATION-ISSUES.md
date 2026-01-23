# Mailchimp Connector - Certification Issues (Current Files)

## Validation Result: FAILED ❌

**Paconn Validation Output:**
```
Swagger certification failed with errors.
```

## Critical Errors (Must Fix)

### 1. Missing x-ms-summary on ALL Parameters
**Error Count**: 50+ parameters missing x-ms-summary
**Impact**: CERTIFICATION BLOCKER
**Location**: All operations in [`apiDefinition.swagger.json`](f:/projects/Connectors/MailChimp/apiDefinition.swagger.json)

**Examples of Missing x-ms-summary:**
- `/lists` GET: count, offset parameters
- `/lists/{list_id}` GET: list_id parameter
- `/lists/{list_id}/members` GET: list_id, count, offset, status, since_last_changed
- `/lists/{list_id}/members/{subscriber_hash}` ALL methods: list_id, subscriber_hash
- `/campaigns` GET: count, offset, status, since_send_time, list_id
- `/campaigns/{campaign_id}` ALL methods: campaign_id
- `/reports/{campaign_id}` ALL methods: campaign_id
- `/batches/{batch_id}` GET: batch_id
- `/templates` GET: count, type

**Fix Required**: Add `x-ms-summary` property to EVERY parameter in EVERY operation

**Example Fix:**
```json
// BEFORE (WRONG):
{
  "name": "list_id",
  "in": "path",
  "required": true,
  "type": "string",
  "description": "The unique ID for the list"
}

// AFTER (CORRECT):
{
  "name": "list_id",
  "in": "path",
  "required": true,
  "type": "string",
  "description": "The unique ID for the list",
  "x-ms-summary": "List ID",
  "x-ms-url-encoding": "single"
}
```

### 2. Missing Contact Information in info Section
**Error**: `info : The 'contact' property is required`
**Impact**: CERTIFICATION BLOCKER
**Location**: [`apiDefinition.swagger.json`](f:/projects/Connectors/MailChimp/apiDefinition.swagger.json) info section

**Fix Required**: Add contact object to info section

**Example Fix:**
```json
"info": {
  "title": "Mailchimp (RapidStart)",
  "description": "Custom Mailchimp connector for RapidStart CRM integration",
  "version": "1.0.0",
  "contact": {
    "name": "Forceworks",
    "url": "https://www.forceworks.com",
    "email": "connect@forceworks.com"
  }
}
```

## Warnings (Should Fix)

### 3. Missing x-ms-url-encoding on Path Parameters
**Warning Count**: 30+ path parameters missing x-ms-url-encoding
**Impact**: Potential invalid requests
**Location**: All path parameters

**Fix Required**: Add `"x-ms-url-encoding": "single"` to all path parameters

## Estimated Fix Effort

- **Add x-ms-summary to ~50 parameters**: 1-2 hours
- **Add contact info**: 2 minutes
- **Add x-ms-url-encoding to ~30 path parameters**: 30 minutes
- **Total**: ~2-3 hours of manual editing

## Recommendation

The current uploaded files need extensive manual fixes to pass certification. 

**Option 1**: Fix all 50+ parameters manually (2-3 hours)
**Option 2**: Use the previously working MailchimpMarketing-IndependentPublisher files which already have all x-ms-summary and x-ms-url-encoding properties

The MailchimpMarketing-IndependentPublisher version already passed validation cleanly. It would be more efficient to use that as the base.
