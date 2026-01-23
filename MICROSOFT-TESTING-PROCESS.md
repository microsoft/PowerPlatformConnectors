# Microsoft's Connector Testing Process

## What Microsoft Actually Tests

Microsoft's certification team performs **automated technical validation**, not comprehensive functional testing of every operation.

### Automated Validation (What They Check)

1. **Package Structure**
   - ✅ All required files present
   - ✅ Valid zip structure
   - ✅ Solution files can be imported

2. **Swagger/OpenAPI Validation**
   - ✅ Valid OpenAPI 2.0 format
   - ✅ No syntax errors
   - ✅ All operations have summaries/descriptions
   - ✅ Response schemas defined
   - ✅ No empty operations

3. **Policy Compliance**
   - ✅ Title follows naming convention
   - ✅ Description length (30-500 chars)
   - ✅ Icon brand color correct
   - ✅ No forbidden words
   - ✅ HTTPS with TLS 1.2+

4. **Import Test**
   - ✅ Connector imports successfully
   - ✅ Connection can be created
   - ✅ No runtime errors during import

5. **Basic Connectivity**
   - ✅ Authentication works
   - ✅ Can make at least one successful API call
   - ✅ No immediate failures

## What They DON'T Test

❌ **They do NOT:**
- Test every single operation
- Verify all operations return correct data
- Test against real production data
- Validate business logic
- Test edge cases
- Verify all parameter combinations
- Test against Dataverse or any specific system

## Your Responsibility: Test Flow

**You must provide a test flow** that demonstrates:
- Connector can be used successfully
- At least one operation works end-to-end
- Authentication is configured correctly

### Test Flow Requirements

Your test flow should:
1. **Use your connector** (not just exist)
2. **Execute successfully** (green checkmark)
3. **Show at least 10 successful runs** per operation you want to highlight
4. **Be simple** - doesn't need to be complex

### Example Test Flow

```
Trigger: Manual trigger
↓
Action: Get Account (Apollo Enrichment)
  - Domain: "microsoft.com"
↓
Action: Compose
  - Inputs: @{outputs('Get_Account')?['body/organization/name']}
```

That's it! Just prove the connector works.

## What YOU Need to Test (Before Submission)

### Your Testing Checklist

For connectors with many operations, you should test:

✅ **Critical Operations** (10+ successful calls each):
- Most commonly used operations
- Operations with complex parameters
- Operations that write data (POST/PUT/DELETE)

✅ **Authentication**:
- Connection creation works
- API key/credentials are valid
- Auth headers are sent correctly

✅ **Error Handling**:
- Invalid inputs return proper errors
- Rate limits are handled
- 401/403 errors are clear

✅ **Response Schemas**:
- Responses match your swagger definitions
- No unexpected fields cause errors
- Arrays and objects parse correctly

### Testing Against What?

**You test against the actual service** (Apollo, Kit, QuickBooks, etc.):
- Use your own test account
- Use real API credentials
- Make real API calls
- Verify real responses

**NOT against Dataverse** - that's just one potential use case.

## Connectors with Many Operations

### Strategy for Large Connectors

If you have 50+ operations:

1. **Test Core Operations** (10-15 most important)
   - List/Get operations
   - Create/Update operations
   - Most commonly used endpoints

2. **Spot Check Others** (5-10 calls each)
   - Verify they don't error
   - Check response structure
   - Confirm parameters work

3. **Document Known Issues**
   - List untested operations in readme
   - Note any limitations
   - Explain which operations are most reliable

### Example: QuickBooks Connector

If QuickBooks has 100 operations:

**Must Test Thoroughly:**
- Get Customer
- Create Invoice
- List Invoices
- Get Account
- Create Payment

**Spot Check:**
- Other entity operations
- Less common endpoints

**Document:**
```markdown
## Known Issues and Limitations

- **Tested Operations**: Core customer, invoice, and payment operations have been thoroughly tested
- **Other Operations**: Additional operations follow the same patterns but may have edge cases
- **Rate Limits**: QuickBooks API limits apply (500 requests per minute)
```

## Microsoft's Focus

Microsoft cares about:
1. **Technical compliance** - Does it meet all requirements?
2. **Security** - Is authentication secure?
3. **User experience** - Are operations well-documented?
4. **Reliability** - Does it import and run without errors?

They DON'T care about:
- Whether you tested every operation
- What systems you integrate with
- Your specific use cases
- Dataverse compatibility

## After Certification

Once certified, **users** will test your connector:
- They'll find bugs you missed
- They'll request new features
- They'll report issues

You can then:
- Submit updates
- Fix bugs
- Add operations
- Improve documentation

## Summary

**Microsoft Tests:**
- Package structure ✅
- Swagger validity ✅
- Policy compliance ✅
- Basic connectivity ✅
- Can import and create connection ✅

**You Test:**
- All critical operations work
- Authentication is correct
- Responses match schemas
- Error handling is reasonable

**You DON'T Need:**
- To test every operation exhaustively
- To test against Dataverse
- To test every parameter combination
- To have 100% coverage

**Focus on:**
- Core operations work well
- Documentation is clear
- Known issues are documented
- Test flow demonstrates success

---

## Practical Advice

For a connector with 50 operations:
- **Thoroughly test**: 10-15 core operations (10+ calls each)
- **Spot check**: 20-30 other operations (3-5 calls each)
- **Document**: Remaining operations as "following same patterns"
- **Be honest**: List any known limitations in readme

Microsoft will certify based on technical compliance, not functional completeness. Users will help you find issues after launch.
